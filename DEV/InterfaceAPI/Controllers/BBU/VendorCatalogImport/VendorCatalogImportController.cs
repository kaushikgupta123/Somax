using Common.Constants;
using InterfaceAPI.ActionFilters;
using InterfaceAPI.BusinessWrapper.Implementation;
using InterfaceAPI.BusinessWrapper.Implementation.BBU;
using InterfaceAPI.BusinessWrapper.Interface;
using InterfaceAPI.BusinessWrapper.Interface.BBU;
using InterfaceAPI.CommonUtility;
using InterfaceAPI.Models.BBU.VendorCatalogImport;
using InterfaceAPI.Models.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace InterfaceAPI.Controllers.BBU.VendorCatalogImport
{
    //[EnableCors("*", "*", "*")]
    //[CustomAuthorize]

    public class VendorCatalogImportController : BaseApiController
    {
        readonly BusinessWrapper.Interface.BBU.IVendorCatalogImportWrapper _VendorCatalogImportWrapper;
        public long logID { get; set; }
        public static string jsonPath { get; set; }
        public static string encryptedPath { get; set; }
        public static string decryptedPath { get; set; }
        public VendorCatalogImportController(IImportCSVWrapper ImportCSVWrapper, BusinessWrapper.Interface.BBU.IVendorCatalogImportWrapper vcImportWrapper)
        {
            _VendorCatalogImportWrapper = vcImportWrapper;
        }
        [HttpPost]
        [Route("api/VendorCatImportDelim/")]
        public HttpResponseMessage VendorCatalogImport([FromBody] SchedulerAPICredentials credentials)
        {
            List<VendorCatalogImportResponseModel> VendorCatalogImportResponseModelList = new List<VendorCatalogImportResponseModel>();
            #region Authentication
            ClaimsPrincipal currentClaimsPrincipal = ClaimsPrincipal.Current;
            if (!_VendorCatalogImportWrapper.CheckAuthentication(credentials.UserName, credentials.Password))
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Authentication Failed");
            }
            if (_VendorCatalogImportWrapper.IsMaintenanceGoingOn())
            {
                return Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, "ServiceUnavailable");
            }
            #endregion
            #region Interface Setup Properties Check 
            if (!_VendorCatalogImportWrapper.CheckIsActiveInterface(ApiConstants.OracleVendorCatalogImport))
            {
                return Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, "The process is not enabled for the client.");
            }
            #endregion
            #region Initiate SFTP Values
            _VendorCatalogImportWrapper.RetrieveInterfacePropValues(ApiConstants.OracleVendorCatalogImport);
            #endregion

            #region Test Encrypt and UploadFile to SFTP
            //_VendorCatalogImportWrapper.EncryptandUploadFile("03_Sample Date_Catalog Interface.txt", ApiConstants.OracleVendorCatalogImport);//---comment out before publish
            #endregion

            #region Retrieve Interface Prop values for SFTP File Download


            var sftpDirPath = SFTPCred.ftpFileDirectory;
            var sftpArchiveDirPath = SFTPCred.ftpArcDirectory;
            string result = string.Empty;
            string errCsv = string.Empty;
            var fileList = _VendorCatalogImportWrapper.GetFileList(sftpDirPath);

            #endregion


            #region Read files fron SFTP
            VendorCatalogImportResponseModel VendorCatalogImportResponseModel;
            //List<VendorCatalogImportResponseModel> VendorCatalogImportResponseModelList = new List<VendorCatalogImportResponseModel>();
            if (fileList != null && fileList.Count == 0)
            {
                VendorCatalogImportResponseModel = new VendorCatalogImportResponseModel();
                VendorCatalogImportResponseModel.status = StatusEnum.failed.ToString();
                VendorCatalogImportResponseModel.errMessage = "No file found at source directory.";
                VendorCatalogImportResponseModelList.Add(VendorCatalogImportResponseModel);
                var respFile = new HttpResponseMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(VendorCatalogImportResponseModelList),
                                            System.Text.Encoding.UTF8, "application/json")
                };
                return respFile;
            }

            //---Get json from csv---
            foreach (var fileName in fileList)
            {
                string dcFileName;
                encryptedPath = _VendorCatalogImportWrapper.DownloadToLocal(fileName, ApiConstants.OracleVendorCatalogImport);
                // RKL - 2021-Aug-13
                if (SFTPCred.filesEncrypted)
                {
                    decryptedPath = _VendorCatalogImportWrapper.DecryptFile(fileName, encryptedPath, ApiConstants.OracleVendorCatalogImport);
                    dcFileName = fileName.Replace(".txt.pgp", ".txt");
                }
                else
                {
                    decryptedPath = encryptedPath;
                    dcFileName = fileName;
                }
                string thisExtension = Utility.GetFileExtension(dcFileName);

                if (!(thisExtension.Contains(FileExtensionEnum.txt.ToString()) || thisExtension.Contains(FileExtensionEnum.csv.ToString())))
                {
                    VendorCatalogImportResponseModel = new VendorCatalogImportResponseModel();
                    VendorCatalogImportResponseModel.fileName = dcFileName;
                    VendorCatalogImportResponseModel.status = StatusEnum.failed.ToString();
                    VendorCatalogImportResponseModel.errMessage = "File type " + thisExtension + " is not supported.";
                    VendorCatalogImportResponseModelList.Add(VendorCatalogImportResponseModel);
                }
                else
                {
                    //var ReflectionProperty = typeof(VendorCatalogImportJSONModel).GetProperties().ToList();
                    //string[] PropertyNames = ReflectionProperty.Select(x => x.Name).ToArray();
                    //result = _VendorCatalogImportWrapper.GetJsonFromDownloadedSFTPFiles(decryptedPath, dcFileName, FileTypeEnum.OracleVendorCatalogImport, PropertyNames);
                    result = _VendorCatalogImportWrapper.GetJsonFromDownloadedSFTPFiles(decryptedPath, fileName, FileTypeEnum.OracleVendorCatalogImport);
                    if (result.Equals(StatusEnum.ErrorConvertingJson.ToString()))
                    {
                        VendorCatalogImportResponseModel = new VendorCatalogImportResponseModel();
                        VendorCatalogImportResponseModel.fileName = dcFileName;
                        //VendorCatalogImportResponseModel.fileName = fileName;
                        VendorCatalogImportResponseModel.status = StatusEnum.failed.ToString();
                        VendorCatalogImportResponseModel.errMessage = "File read failure.";
                        VendorCatalogImportResponseModelList.Add(VendorCatalogImportResponseModel);
                        continue;
                    }
                    //else if (result.Equals(StatusEnum.ColumnMismatch.ToString()))
                    //{
                    //    VendorCatalogImportResponseModel = new VendorCatalogImportResponseModel();
                    //    VendorCatalogImportResponseModel.fileName = dcFileName;
                    //    VendorCatalogImportResponseModel.status = StatusEnum.failed.ToString();
                    //    VendorCatalogImportResponseModel.errMessage = "File is expected to be containing these columns - " +
                    //      String.Join(", ", PropertyNames) + " .";
                    //    VendorCatalogImportResponseModelList.Add(VendorCatalogImportResponseModel);
                    //    continue;
                    //}
                    //--- serialize JSON directly to a file
                    List<VendorCatalogImportJSONModel> aList = new List<VendorCatalogImportJSONModel>();
                    aList = JsonConvert.DeserializeObject<List<VendorCatalogImportJSONModel>>(result.ToString());
                    string firstName = Utility.GetFileName(fileName);
                    jsonPath = HttpContext.Current.Server.MapPath("~/JsonSchema/BBU/VendorCatalogImport/" + firstName + ".json");//Create Json Schema
                    using (StreamWriter file = File.CreateText(jsonPath))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(file, aList);
                    }
                    //---compare the JSON with schema
                    JsonSchema jschema = new JsonSchema();

                    String schemaPath = HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["OracleVendorCatalogImportSchemaPath"]);// Create entry in webconfig
                    ////---check if schema file exists. If not return relevent response---
                    if (!File.Exists(schemaPath))
                    {
                        VendorCatalogImportResponseModel = new VendorCatalogImportResponseModel();
                        VendorCatalogImportResponseModel.fileName = fileName;
                        VendorCatalogImportResponseModel.status = StatusEnum.failed.ToString();
                        VendorCatalogImportResponseModel.errMessage = "The refence schema requred for json validation is not found.";
                        VendorCatalogImportResponseModelList.Add(VendorCatalogImportResponseModel);
                        var respFile = new HttpResponseMessage
                        {
                            Content = new StringContent(JsonConvert.SerializeObject(VendorCatalogImportResponseModelList),
                                            System.Text.Encoding.UTF8, "application/json")
                        };
                        return respFile;
                    }
                    jschema = JsonSchema.Parse(File.ReadAllText(schemaPath));
                    JObject jo = new JObject();
                    JArray ja = JArray.Parse(File.ReadAllText(jsonPath));
                    bool schemaMatch = false;
                    string archiveErr = string.Empty;
                    foreach (var obj in ja)
                    {
                        jo = JObject.Parse(obj.ToString());
                        schemaMatch = jo.IsValid(jschema);
                        if (schemaMatch == false)
                        {
                            VendorCatalogImportResponseModel = new VendorCatalogImportResponseModel();
                            VendorCatalogImportResponseModel.fileName = fileName;
                            VendorCatalogImportResponseModel.status = StatusEnum.failed.ToString();
                            VendorCatalogImportResponseModel.errMessage = "The json file does not match required schema.";
                            VendorCatalogImportResponseModelList.Add(VendorCatalogImportResponseModel);
                            var respJson = new HttpResponseMessage
                            {
                                Content = new StringContent(JsonConvert.SerializeObject(VendorCatalogImportResponseModelList),
                                              System.Text.Encoding.UTF8, "application/json")
                            };
                            return respJson;
                        }
                    }

                    archiveErr = _VendorCatalogImportWrapper.ArchiveFileWithDBValue(sftpDirPath, fileName, sftpArchiveDirPath);
                    if (String.IsNullOrEmpty(archiveErr))
                    {
                        VendorCatalogImportResponseModel = new VendorCatalogImportResponseModel();
                        VendorCatalogImportResponseModel.fileName = fileName;
                        VendorCatalogImportResponseModel.status = StatusEnum.success.ToString();
                        VendorCatalogImportResponseModel.errMessage = string.Empty;
                        //---process json
                        var respMessage = ProcessLocalVendorCatalogImportJsonFile();
                        VendorCatalogImportResponseModel.VendorCatalogImportResponseModelList = respMessage;
                        VendorCatalogImportResponseModelList.Add(VendorCatalogImportResponseModel);


                    }
                    else
                    {
                        VendorCatalogImportResponseModel = new VendorCatalogImportResponseModel();
                        VendorCatalogImportResponseModel.fileName = fileName;
                        VendorCatalogImportResponseModel.status = StatusEnum.failed.ToString();
                        VendorCatalogImportResponseModel.errMessage = archiveErr;
                        VendorCatalogImportResponseModelList.Add(VendorCatalogImportResponseModel);
                    }
                }
            }
            #region Send Mail
            if (SFTPCred.isMailSend == true)
            {
                _VendorCatalogImportWrapper.SendEMail(credentials.UserName, (JsonConvert.SerializeObject(VendorCatalogImportResponseModelList)));
            }

            #endregion

            var resp = new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(VendorCatalogImportResponseModelList),
                                               System.Text.Encoding.UTF8, "application/json")
            };
            return resp;
        }
        #endregion
        private List<VendorCatalogImportResponseModel> ProcessLocalVendorCatalogImportJsonFile()
        {
            String FilePath = HttpContext.Current.Server.MapPath("~/JsonSchema/BBU/VendorCatalogImport/");

            string[] file = Directory.GetFiles(FilePath, "*.json");
            List<VendorCatalogImportResponseModel> vmrList = new List<VendorCatalogImportResponseModel>();
            VendorCatalogImportResponseModel VendorCatalogImportResponseModel = new VendorCatalogImportResponseModel();
            using (StreamReader r = new StreamReader(file[0]))
            {
                string json = r.ReadToEnd();
                List<VendorCatalogImportJSONModel> VendorCatalogImportModelList = JsonConvert.DeserializeObject<List<VendorCatalogImportJSONModel>>(json);

                if (VendorCatalogImportModelList == null)
                {
                    VendorCatalogImportResponseModel.status = StatusEnum.failed.ToString();
                    VendorCatalogImportResponseModel.errMessage = "Data is null.";
                    vmrList.Add(VendorCatalogImportResponseModel);
                    return (vmrList);
                }
                #region VendorCatalogimportmodel objects converted to data tables
                var result = _VendorCatalogImportWrapper.CreateVCImport(VendorCatalogImportModelList);
                #endregion

                #region create & update log
                var logid = _VendorCatalogImportWrapper.CreateLog(ApiConstants.OracleVendorCatalogImport);

                #endregion

                #region process(import)
                var vResult = _VendorCatalogImportWrapper.ProcessVCImport();
                #endregion

                #region UpdateLog
                if (logid != 0)
                {
                    _VendorCatalogImportWrapper.UpdateLog(logid, result.TotalProcess, result.SuccessfulProcess, result.FailedProcess, result.logMessage, Common.Constants.ApiConstants.OracleVendorCatalogImport);
                }
                #endregion

                if (vResult.FailedProcess != null && vResult.FailedProcess > 0)
                {
                    VendorCatalogImportResponseModel.status = StatusEnum.failed.ToString();
                    VendorCatalogImportResponseModel.errMessage = vResult.logMessage;
                }
                else
                {
                    VendorCatalogImportResponseModel.status = StatusEnum.success.ToString();
                    VendorCatalogImportResponseModel.errMessage = vResult.logMessage;
                }
                vmrList.Add(VendorCatalogImportResponseModel);
            }
            //delete all files from local directory
            _VendorCatalogImportWrapper.DeleteLocalFiles(Path.GetDirectoryName(jsonPath));
            _VendorCatalogImportWrapper.DeleteLocalFiles(Path.GetDirectoryName(encryptedPath));
            _VendorCatalogImportWrapper.DeleteLocalFiles(Path.GetDirectoryName(decryptedPath));


            return vmrList;
        }
    }
}