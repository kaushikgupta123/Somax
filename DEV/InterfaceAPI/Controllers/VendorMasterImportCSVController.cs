using Common.Constants;
using InterfaceAPI.BusinessWrapper.Interface;
using InterfaceAPI.CommonUtility;
using InterfaceAPI.Models.Common;
using InterfaceAPI.Models.Vendor;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;

namespace InterfaceAPI.Controllers
{
    public class VendorMasterImportCSVController : BaseApiController
    {
        readonly IImportCSVWrapper _ImportCSVWrapper;
        readonly IVendorMasterImportWrapper _VMImportWrapper;
        public VendorMasterImportCSVController(IImportCSVWrapper ImportCSVWrapper, IVendorMasterImportWrapper VMImportWrapper)
        {
            _ImportCSVWrapper = ImportCSVWrapper;
            _VMImportWrapper = VMImportWrapper;
        }

        public long logID { get; set; }
        public string email { get; set; }

        public HttpResponseMessage Post(SchedulerAPICredentials credentials)
        {
            #region Authentication

            ClaimsPrincipal currentClaimsPrincipal = ClaimsPrincipal.Current;
            if (!_VMImportWrapper.CheckAuthentication(credentials.UserName, credentials.Password))
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Authentication Failed");
            }
            if (_VMImportWrapper.IsMaintenanceGoingOn())
            {
                return Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, "ServiceUnavailable");
            }
            #endregion
            #region Interface Setup Properties Check 

            if (!_VMImportWrapper.CheckIsActiveInterface(ApiConstants.VendorMasterImport))
            {
                return Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, "The process is not enabled for the client.");
            }
            #endregion

            //---read csv  from FTP
            var sftpDirPath = System.Configuration.ConfigurationManager.AppSettings["VendorActiveDir"];
            var sftpArchiveDirPath = System.Configuration.ConfigurationManager.AppSettings["VendorArchiveDir"];
            string result = string.Empty;
            string errCsv = string.Empty;
            var fileList = _VMImportWrapper.GetFileNames(sftpDirPath);
            VendorCsvResponseModel vendorCsvResponseModel;
            List<VendorCsvResponseModel> VendorCsvResponseModelList = new List<VendorCsvResponseModel>();
            if (fileList != null && fileList.Count == 0)
            {
                vendorCsvResponseModel = new VendorCsvResponseModel();
                vendorCsvResponseModel.status = StatusEnum.failed.ToString();
                vendorCsvResponseModel.errMessage = "No file found at source directory.";
                VendorCsvResponseModelList.Add(vendorCsvResponseModel);
                var respFile = new HttpResponseMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(VendorCsvResponseModelList),
                                             System.Text.Encoding.UTF8, "application/json")
                };
                return respFile;
            }
            //---Get json from csv---
            foreach (var fileName in fileList)
            {
                string thisExtension = Utility.GetFileExtension(fileName);
                if (!(thisExtension.Contains(FileExtensionEnum.txt.ToString()) || thisExtension.Contains(FileExtensionEnum.csv.ToString())))
                {
                    vendorCsvResponseModel = new VendorCsvResponseModel();
                    vendorCsvResponseModel.fileName = fileName;
                    vendorCsvResponseModel.status = StatusEnum.failed.ToString();
                    vendorCsvResponseModel.errMessage = "File type " + thisExtension + " is not supported.";
                    VendorCsvResponseModelList.Add(vendorCsvResponseModel);
                }
                else
                {
                    result = _VMImportWrapper.GetJsonFromSFTPFile(sftpDirPath, fileName, FileTypeEnum.VendorMaster);
                    if (result.Equals(StatusEnum.ErrorConvertingJson.ToString()))
                    {
                        vendorCsvResponseModel = new VendorCsvResponseModel();
                        vendorCsvResponseModel.fileName = fileName;
                        vendorCsvResponseModel.status = StatusEnum.failed.ToString();
                        vendorCsvResponseModel.errMessage = "File read failure.";
                        VendorCsvResponseModelList.Add(vendorCsvResponseModel);
                    }
                    //--- serialize JSON directly to a file
                    VendorMasterImportModel vendorImport = new VendorMasterImportModel();
                    List<VendorMasterImportModel> vList = new List<VendorMasterImportModel>();
                    vList = JsonConvert.DeserializeObject<List<VendorMasterImportModel>>(result.ToString());
                    string firstName = Utility.GetFileName(fileName);
                    string jsonPath = HttpContext.Current.Server.MapPath("~/JsonSchema/Vendor/" + firstName + ".json");
                    using (StreamWriter file = File.CreateText(jsonPath))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(file, vList);
                    }
                    //---compare the JSON with schema
                    JsonSchema jschema = new JsonSchema();

                    String schemaPath = HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["VendorSchemaPath"]);
                    //---check if schema file exists. If not return relevent response---
                    if (!File.Exists(schemaPath))
                    {
                        vendorCsvResponseModel = new VendorCsvResponseModel();
                        vendorCsvResponseModel.fileName = fileName;
                        vendorCsvResponseModel.status = StatusEnum.failed.ToString();
                        vendorCsvResponseModel.errMessage = "The refence schema requred for json validation is not found.";
                        VendorCsvResponseModelList.Add(vendorCsvResponseModel);
                        var respFile = new HttpResponseMessage
                        {
                            Content = new StringContent(JsonConvert.SerializeObject(VendorCsvResponseModelList),
                                            System.Text.Encoding.UTF8, "application/json")
                        };
                        return respFile;
                    }
                    jschema = JsonSchema.Parse(File.ReadAllText(schemaPath));
                    JObject jo = new JObject();
                    jo = JObject.Parse(File.ReadAllText(schemaPath));
                    bool schemaMatch = jo.IsValid(jschema);
                    string archiveErr = string.Empty;
                    if (schemaMatch)
                    {
                        archiveErr = _VMImportWrapper.ArchiveFile(sftpDirPath, fileName, sftpArchiveDirPath);
                        if (String.IsNullOrEmpty(archiveErr))
                        {
                            vendorCsvResponseModel = new VendorCsvResponseModel();
                            vendorCsvResponseModel.fileName = fileName;
                            vendorCsvResponseModel.status = StatusEnum.success.ToString();
                            vendorCsvResponseModel.errMessage = string.Empty;
                            //---process json
                            var respMessage = ProcessLocalVendorJsonFile();
                            vendorCsvResponseModel.VendorImportResponseModelList = respMessage;
                            VendorCsvResponseModelList.Add(vendorCsvResponseModel);
                        }
                        else
                        {
                            vendorCsvResponseModel = new VendorCsvResponseModel();
                            vendorCsvResponseModel.fileName = fileName;
                            vendorCsvResponseModel.status = StatusEnum.failed.ToString();
                            vendorCsvResponseModel.errMessage = archiveErr;
                            VendorCsvResponseModelList.Add(vendorCsvResponseModel);
                        }
                    }
                    else
                    {
                        vendorCsvResponseModel = new VendorCsvResponseModel();
                        vendorCsvResponseModel.fileName = fileName;
                        vendorCsvResponseModel.status = StatusEnum.failed.ToString();
                        vendorCsvResponseModel.errMessage = "The json file does not match required schema.";
                        VendorCsvResponseModelList.Add(vendorCsvResponseModel);
                    }
                }
            }
            var resp = new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(VendorCsvResponseModelList),
                                             System.Text.Encoding.UTF8, "application/json")
            };
            return resp;
        }

        private List<VendorImportResponseModel> ProcessLocalVendorJsonFile()
        {
            String FilePath = HttpContext.Current.Server.MapPath("~/JsonSchema/Vendor/");
            string[] file = Directory.GetFiles(FilePath, "*.json");
            List<VendorImportResponseModel> vmrList = new List<VendorImportResponseModel>();
            VendorImportResponseModel vendorImportResponseModel = new VendorImportResponseModel();
            using (StreamReader r = new StreamReader(file[0]))
            {
                vendorImportResponseModel.fileName = file[0];
                string json = r.ReadToEnd();
                var vendorMasterImportModel = JsonConvert.DeserializeObject<List<VendorMasterImportModel>>(json);
                if (vendorMasterImportModel == null)
                {
                    vendorImportResponseModel.status = StatusEnum.failed.ToString();
                    vendorImportResponseModel.errMessage = "Data is null.";
                    vmrList.Add(vendorImportResponseModel);
                    return (vmrList);
                }
                #region vendorimportmodel objects converted to data tables
                var result = _VMImportWrapper.InsertVendarMasterImportData(vendorMasterImportModel);
                #region create & update log
                var logid = _VMImportWrapper.CreateLog("vendorimport");
                if (logid != 0)
                {
                    _VMImportWrapper.UpdateLog(logid, result.TotalProcess, result.SuccessfulProcess, result.FailedProcess, result.logMessage, Common.Constants.ApiConstants.DeanFoodsVendorMasterImport);
                }
                #endregion

                #endregion

                #region validate validation & process(import)
                // V2-416
                //var vResult = _VMImportWrapper.VendorMasterImportValidate(logid, vendorMasterImportModel); 
                var vResult = _VMImportWrapper.VendorMasterImportValidate(logid);
                #endregion

                if (vResult != null && vResult.Count > 0)
                {
                    vendorImportResponseModel.status = StatusEnum.failed.ToString();
                    foreach (var vr in vResult)
                    {
                        vendorImportResponseModel.errMessageList.Add(vr);
                    }
                }
                else
                {
                    vendorImportResponseModel.errMessage = "";
                    vendorImportResponseModel.status = StatusEnum.success.ToString();
                }
                vmrList.Add(vendorImportResponseModel);
            }
            //---delete all json files from local directory
            _VMImportWrapper.DeleteJsonFiles("vendor");
            return vmrList;
        }
    }
}