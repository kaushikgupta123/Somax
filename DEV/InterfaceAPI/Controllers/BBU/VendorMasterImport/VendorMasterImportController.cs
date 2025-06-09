using Common.Constants;
using InterfaceAPI.ActionFilters;
using InterfaceAPI.BusinessWrapper.Implementation;
using InterfaceAPI.BusinessWrapper.Implementation.BBU;
using InterfaceAPI.BusinessWrapper.Interface;
using InterfaceAPI.BusinessWrapper.Interface.BBU;
using InterfaceAPI.CommonUtility;
using InterfaceAPI.Models.BBU.VendorMasterImport;
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

namespace InterfaceAPI.Controllers.BBU.VendorMasterImport
{
    //[EnableCors("*", "*", "*")]
    //[CustomAuthorize]

    public class OracleVendorMasterImportController : BaseApiController
    {
        readonly BusinessWrapper.Interface.BBU.IVendorMasterImportWrapper _VendorMasterImportWrapper;
        public long logID { get; set; }
        public static string jsonPath { get; set; }
        public static string encryptedPath { get; set; }
        public static string decryptedPath { get; set; }
        public OracleVendorMasterImportController(IImportCSVWrapper ImportCSVWrapper, BusinessWrapper.Interface.BBU.IVendorMasterImportWrapper vcImportWrapper)
        {
            _VendorMasterImportWrapper = vcImportWrapper;
        }
        [HttpPost]
        [Route("api/VendorMstImportDelim/")]
        public HttpResponseMessage OracleVendorMasterImport([FromBody] SchedulerAPICredentials credentials)
        {
            List<VendorMasterImportResponseModel> VendorMasterImportResponseModelList = new List<VendorMasterImportResponseModel>();
            #region Authentication
            ClaimsPrincipal currentClaimsPrincipal = ClaimsPrincipal.Current;
            if (!_VendorMasterImportWrapper.CheckAuthentication(credentials.UserName, credentials.Password))
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Authentication Failed");
            }
            if (_VendorMasterImportWrapper.IsMaintenanceGoingOn())
            {
                return Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, "ServiceUnavailable");
            }
            #endregion
            #region Interface Setup Properties Check 
            if (!_VendorMasterImportWrapper.CheckIsActiveInterface(ApiConstants.OracleVendorMasterImport))
            {
                return Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, "The process is not enabled for the client.");
            }
            #endregion
            #region Initiate SFTP Values
            _VendorMasterImportWrapper.RetrieveInterfacePropValues(ApiConstants.OracleVendorMasterImport);
            #endregion

            #region Test Encrypt and UploadFile to SFTP
            //_VendorMasterImportWrapper.EncryptandUploadFile("02_Sample Data_Vendor Master Interface.txt", ApiConstants.OracleVendorMasterImport);//---comment out before publish
            #endregion

            #region Retrieve Interface Prop values for SFTP File Download


            var sftpDirPath = SFTPCred.ftpFileDirectory;
            var sftpArchiveDirPath = SFTPCred.ftpArcDirectory;
            string result = string.Empty;
            string errCsv = string.Empty;
            var fileList = _VendorMasterImportWrapper.GetFileList(sftpDirPath);

            #endregion


            #region Read files fron SFTP
            VendorMasterImportResponseModel VendorMasterImportResponseModel;
            //List<VendorMasterImportResponseModel> VendorMasterImportResponseModelList = new List<VendorMasterImportResponseModel>();
            if (fileList != null && fileList.Count == 0)
            {
                VendorMasterImportResponseModel = new VendorMasterImportResponseModel();
                VendorMasterImportResponseModel.status = StatusEnum.failed.ToString();
                VendorMasterImportResponseModel.errMessage = "No file found at source directory.";
                VendorMasterImportResponseModelList.Add(VendorMasterImportResponseModel);
                var respFile = new HttpResponseMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(VendorMasterImportResponseModelList),
                                            System.Text.Encoding.UTF8, "application/json")
                };
                return respFile;
            }

            //---Get json from csv---
            foreach (var fileName in fileList)
            {
                string dcFileName;
                encryptedPath = _VendorMasterImportWrapper.DownloadToLocal(fileName, ApiConstants.OracleVendorMasterImport);
                // RKL - 2021-Aug-13
                if (SFTPCred.filesEncrypted)
                {
                    decryptedPath = _VendorMasterImportWrapper.DecryptFile(fileName, encryptedPath, ApiConstants.OracleVendorMasterImport);
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
                    VendorMasterImportResponseModel = new VendorMasterImportResponseModel();
                    VendorMasterImportResponseModel.fileName = dcFileName;
                    VendorMasterImportResponseModel.status = StatusEnum.failed.ToString();
                    VendorMasterImportResponseModel.errMessage = "File type " + thisExtension + " is not supported.";
                    VendorMasterImportResponseModelList.Add(VendorMasterImportResponseModel);
                }
                else
                {
                    //var ReflectionProperty = typeof(VendorMasterImportJSONModel).GetProperties().ToList();
                    //string[] PropertyNames = ReflectionProperty.Select(x => x.Name).ToArray();
                    //result = _VendorMasterImportWrapper.GetJsonFromDownloadedSFTPFiles(decryptedPath, dcFileName, FileTypeEnum.OracleVendorMasterImport, PropertyNames);
                    result = _VendorMasterImportWrapper.GetJsonFromDownloadedSFTPFiles(decryptedPath, dcFileName, FileTypeEnum.OracleVendorMasterImport);
                    if (result.Equals(StatusEnum.ErrorConvertingJson.ToString()))
                    {
                        VendorMasterImportResponseModel = new VendorMasterImportResponseModel();
                        VendorMasterImportResponseModel.fileName = dcFileName;
                        VendorMasterImportResponseModel.status = StatusEnum.failed.ToString();
                        VendorMasterImportResponseModel.errMessage = "File read failure.";
                        VendorMasterImportResponseModelList.Add(VendorMasterImportResponseModel);
                        continue;
                    }
                    //else if (result.Equals(StatusEnum.ColumnMismatch.ToString()))
                    //{
                    //    VendorMasterImportResponseModel = new VendorMasterImportResponseModel();
                    //    VendorMasterImportResponseModel.fileName = dcFileName;
                    //    VendorMasterImportResponseModel.status = StatusEnum.failed.ToString();
                    //    VendorMasterImportResponseModel.errMessage = "File is expected to be containing these columns - " +
                    //      String.Join(", ", PropertyNames) + " .";
                    //    VendorMasterImportResponseModelList.Add(VendorMasterImportResponseModel);
                    //    continue;
                    //}
                    //--- serialize JSON directly to a file
                    List<VendorMasterImportJSONModel> aList = new List<VendorMasterImportJSONModel>();
                    aList = JsonConvert.DeserializeObject<List<VendorMasterImportJSONModel>>(result.ToString());
                    string firstName = Utility.GetFileName(fileName);
                    jsonPath = HttpContext.Current.Server.MapPath("~/JsonSchema/BBU/VendorMasterImport/" + firstName + ".json");//Create Json Schema
                    using (StreamWriter file = File.CreateText(jsonPath))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(file, aList);
                    }
                    //---compare the JSON with schema
                    JsonSchema jschema = new JsonSchema();

                    String schemaPath = HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["OracleVendorMasterImportSchemaPath"]);// Create entry in webconfig
                    ////---check if schema file exists. If not return relevent response---
                    if (!File.Exists(schemaPath))
                    {
                        VendorMasterImportResponseModel = new VendorMasterImportResponseModel();
                        VendorMasterImportResponseModel.fileName = fileName;
                        VendorMasterImportResponseModel.status = StatusEnum.failed.ToString();
                        VendorMasterImportResponseModel.errMessage = "The refence schema requred for json validation is not found.";
                        VendorMasterImportResponseModelList.Add(VendorMasterImportResponseModel);
                        var respFile = new HttpResponseMessage
                        {
                            Content = new StringContent(JsonConvert.SerializeObject(VendorMasterImportResponseModelList),
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
                        IList<string> errmsg;
                        schemaMatch = jo.IsValid(jschema, out errmsg);
                        if (schemaMatch == false)
                        {
                            VendorMasterImportResponseModel = new VendorMasterImportResponseModel();
                            VendorMasterImportResponseModel.fileName = fileName;
                            VendorMasterImportResponseModel.status = StatusEnum.failed.ToString();
                            VendorMasterImportResponseModel.errMessage = "The json file does not match required schema.";
                            VendorMasterImportResponseModelList.Add(VendorMasterImportResponseModel);
                            var respJson = new HttpResponseMessage
                            {
                                Content = new StringContent(JsonConvert.SerializeObject(VendorMasterImportResponseModelList),
                                              System.Text.Encoding.UTF8, "application/json")
                            };
                            return respJson;
                        }
                    }
                    archiveErr = _VendorMasterImportWrapper.ArchiveFileWithDBValue(sftpDirPath, fileName, sftpArchiveDirPath);
                    if (String.IsNullOrEmpty(archiveErr))
                    {
                        VendorMasterImportResponseModel = new VendorMasterImportResponseModel();
                        VendorMasterImportResponseModel.fileName = fileName;
                        VendorMasterImportResponseModel.status = StatusEnum.success.ToString();
                        VendorMasterImportResponseModel.errMessage = string.Empty;
                        //---process json
                        var respMessage = ProcessLocalVendorMasterImportJsonFile();
                        VendorMasterImportResponseModel.VendorMasterImportResponseModelList = respMessage;
                        VendorMasterImportResponseModelList.Add(VendorMasterImportResponseModel);


                    }
                    else
                    {
                        VendorMasterImportResponseModel = new VendorMasterImportResponseModel();
                        VendorMasterImportResponseModel.fileName = fileName;
                        VendorMasterImportResponseModel.status = StatusEnum.failed.ToString();
                        VendorMasterImportResponseModel.errMessage = archiveErr;
                        VendorMasterImportResponseModelList.Add(VendorMasterImportResponseModel);
                    }
                }
            }
            #region Send Mail
            if (SFTPCred.isMailSend == true)
            {
                _VendorMasterImportWrapper.SendEMail(credentials.UserName, (JsonConvert.SerializeObject(VendorMasterImportResponseModelList)));
            }

            #endregion

            var resp = new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(VendorMasterImportResponseModelList),
                                               System.Text.Encoding.UTF8, "application/json")
            };
            return resp;
        }
        #endregion
        private List<VendorMasterImportResponseModel> ProcessLocalVendorMasterImportJsonFile()
        {
            String FilePath = HttpContext.Current.Server.MapPath("~/JsonSchema/BBU/VendorMasterImport/");

            string[] file = Directory.GetFiles(FilePath, "*.json");
            List<VendorMasterImportResponseModel> vmrList = new List<VendorMasterImportResponseModel>();
            VendorMasterImportResponseModel VendorMasterImportResponseModel = new VendorMasterImportResponseModel();
            using (StreamReader r = new StreamReader(file[0]))
            {
                string json = r.ReadToEnd();
                List<VendorMasterImportJSONModel> VendorMasterImportModelList = JsonConvert.DeserializeObject<List<VendorMasterImportJSONModel>>(json);

                if (VendorMasterImportModelList == null)
                {
                    VendorMasterImportResponseModel.status = StatusEnum.failed.ToString();
                    VendorMasterImportResponseModel.errMessage = "Data is null.";
                    vmrList.Add(VendorMasterImportResponseModel);
                    return (vmrList);
                }
                #region VendorMasterimportmodel objects converted to data tables
                var result = _VendorMasterImportWrapper.CreateVMImport(VendorMasterImportModelList);
                #endregion

                #region create & update log
                var logid = _VendorMasterImportWrapper.CreateLog(ApiConstants.OracleVendorMasterImport);

                #endregion

                #region process(import)
                var vResult = _VendorMasterImportWrapper.ProcessVMImport();
                #endregion

                #region UpdateLog
                if (logid != 0)
                {
                    _VendorMasterImportWrapper.UpdateLog(logid, result.TotalProcess, result.SuccessfulProcess, result.FailedProcess, result.logMessage, Common.Constants.ApiConstants.OracleVendorMasterImport);
                }
                #endregion

                if (vResult.FailedProcess != null && vResult.FailedProcess > 0)
                {
                    VendorMasterImportResponseModel.status = StatusEnum.failed.ToString();
                    VendorMasterImportResponseModel.errMessage = vResult.logMessage;
                }
                else
                {
                    VendorMasterImportResponseModel.status = StatusEnum.success.ToString();
                    VendorMasterImportResponseModel.errMessage = vResult.logMessage;
                }
                vmrList.Add(VendorMasterImportResponseModel);
            }
            //delete all files from local directory
            _VendorMasterImportWrapper.DeleteLocalFiles(Path.GetDirectoryName(jsonPath));
            _VendorMasterImportWrapper.DeleteLocalFiles(Path.GetDirectoryName(encryptedPath));
            _VendorMasterImportWrapper.DeleteLocalFiles(Path.GetDirectoryName(decryptedPath));


            return vmrList;
        }
    }
}