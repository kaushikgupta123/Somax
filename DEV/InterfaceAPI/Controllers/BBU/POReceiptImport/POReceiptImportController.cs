using Common.Constants;
using InterfaceAPI.ActionFilters;
using InterfaceAPI.BusinessWrapper.Implementation;
using InterfaceAPI.BusinessWrapper.Implementation.BBU;
using InterfaceAPI.BusinessWrapper.Interface;
using InterfaceAPI.BusinessWrapper.Interface.BBU;
using InterfaceAPI.CommonUtility;
using InterfaceAPI.Models.BBU.POReceiptImport;
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

namespace InterfaceAPI.Controllers.BBU.POReceiptImport
{
    //[EnableCors("*", "*", "*")]
    //[CustomAuthorize]

    public class POReceiptImportController : BaseApiController
    {
        readonly BusinessWrapper.Interface.BBU.IPOReceiptImportWrapper _POReceiptImportWrapper;
        public long logID { get; set; }
        public static string jsonPath { get; set; }
        public static string encryptedPath { get; set; }
        public static string decryptedPath { get; set; }
        public POReceiptImportController(IImportCSVWrapper ImportCSVWrapper, BusinessWrapper.Interface.BBU.IPOReceiptImportWrapper poReceiptImportWrapper)
        {
            _POReceiptImportWrapper = poReceiptImportWrapper;
        }
        [HttpPost]
        [Route("api/OracleReceiptImportDelim/")]
        public HttpResponseMessage POReceiptImport([FromBody] SchedulerAPICredentials credentials)
        {
            List<POReceiptImportResponseModel> POReceiptImportResponseModelList = new List<POReceiptImportResponseModel>();
            #region Authentication
            ClaimsPrincipal currentClaimsPrincipal = ClaimsPrincipal.Current;
            if (!_POReceiptImportWrapper.CheckAuthentication(credentials.UserName, credentials.Password))
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Authentication Failed");
            }
            if (_POReceiptImportWrapper.IsMaintenanceGoingOn())
            {
                return Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, "ServiceUnavailable");
            }
            #endregion
            #region Interface Setup Properties Check 
            if (!_POReceiptImportWrapper.CheckIsActiveInterface(ApiConstants.OraclePOReceiptImport))
            {
                return Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, "The process is not enabled for the client.");
            }
            #endregion
            #region Initiate SFTP Values
            _POReceiptImportWrapper.RetrieveInterfacePropValues(ApiConstants.OraclePOReceiptImport);
            #endregion

            #region Test Encrypt and UploadFile to SFTP
            //_POReceiptImportWrapper.EncryptandUploadFile("05_Sample Data_Receipt Interface.txt", ApiConstants.OraclePOReceiptImport);//---comment out before publish
            #endregion

            #region Retrieve Interface Prop values for SFTP File Download


            var sftpDirPath = SFTPCred.ftpFileDirectory;
            var sftpArchiveDirPath = SFTPCred.ftpArcDirectory;
            string result = string.Empty;
            string errCsv = string.Empty;
            var fileList = _POReceiptImportWrapper.GetFileList(sftpDirPath);

            #endregion


            #region Read files fron SFTP
            POReceiptImportResponseModel POReceiptImportResponseModel;
            //List<POReceiptImportResponseModel> POReceiptImportResponseModelList = new List<POReceiptImportResponseModel>();
            if (fileList != null && fileList.Count == 0)
            {
                POReceiptImportResponseModel = new POReceiptImportResponseModel();
                POReceiptImportResponseModel.status = StatusEnum.failed.ToString();
                POReceiptImportResponseModel.errMessage = "No file found at source directory.";
                POReceiptImportResponseModelList.Add(POReceiptImportResponseModel);
                var respFile = new HttpResponseMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(POReceiptImportResponseModelList),
                                            System.Text.Encoding.UTF8, "application/json")
                };
                return respFile;
            }

            //---Get json from csv---
            foreach (var fileName in fileList)
            {
                string dcFileName;
                encryptedPath = _POReceiptImportWrapper.DownloadToLocal(fileName, ApiConstants.OraclePOReceiptImport);
                // RKL - 2021-Aug-13
                if (SFTPCred.filesEncrypted)
                {
                    decryptedPath = _POReceiptImportWrapper.DecryptFile(fileName, encryptedPath, ApiConstants.OraclePOReceiptImport);
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
                    POReceiptImportResponseModel = new POReceiptImportResponseModel();
                    POReceiptImportResponseModel.fileName = fileName;
                    POReceiptImportResponseModel.status = StatusEnum.failed.ToString();
                    POReceiptImportResponseModel.errMessage = "File type " + thisExtension + " is not supported.";
                    POReceiptImportResponseModelList.Add(POReceiptImportResponseModel);
                }
                else
                {
                //var ReflectionProperty = typeof(POReceiptImportJSONModel).GetProperties().ToList();
                //string[] PropertyNames = ReflectionProperty.Select(x => x.Name).ToArray();
                //result = _POReceiptImportWrapper.GetJsonFromDownloadedSFTPFiles(decryptedPath, dcFileName, FileTypeEnum.OraclePOReceiptImport, PropertyNames);
                result = _POReceiptImportWrapper.GetJsonFromDownloadedSFTPFiles(decryptedPath, fileName, FileTypeEnum.OraclePOReceiptImport);
                if (result.Equals(StatusEnum.ErrorConvertingJson.ToString()))
                    {
                        POReceiptImportResponseModel = new POReceiptImportResponseModel();
                        POReceiptImportResponseModel.fileName = dcFileName;
                        POReceiptImportResponseModel.status = StatusEnum.failed.ToString();
                        POReceiptImportResponseModel.errMessage = "File read failure.";
                        POReceiptImportResponseModelList.Add(POReceiptImportResponseModel);
                        continue;
                    }
                    //else if (result.Equals(StatusEnum.ColumnMismatch.ToString()))
                    //{
                    //    POReceiptImportResponseModel = new POReceiptImportResponseModel();
                    //    POReceiptImportResponseModel.fileName = dcFileName;
                    //    POReceiptImportResponseModel.status = StatusEnum.failed.ToString();
                    //    POReceiptImportResponseModel.errMessage = "File is expected to be containing these columns - " +
                    //      String.Join(", ", PropertyNames) + " .";
                    //    POReceiptImportResponseModelList.Add(POReceiptImportResponseModel);
                    //    continue;
                    //}
                    //--- serialize JSON directly to a file
                    List<POReceiptImportJSONModel> aList = new List<POReceiptImportJSONModel>();
                    aList = JsonConvert.DeserializeObject<List<POReceiptImportJSONModel>>(result.ToString());
                    string firstName = Utility.GetFileName(fileName);
                    jsonPath = HttpContext.Current.Server.MapPath("~/JsonSchema/BBU/POReceiptImport/" + firstName + ".json");//Create Json Schema
                    using (StreamWriter file = File.CreateText(jsonPath))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(file, aList);
                    }
                    //---compare the JSON with schema
                    JsonSchema jschema = new JsonSchema();

                    String schemaPath = HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["OraclePOReceiptImportSchemaPath"]);// Create entry in webconfig
                                                                                                                                                                     ////---check if schema file exists. If not return relevent response---
                    if (!File.Exists(schemaPath))
                    {
                        POReceiptImportResponseModel = new POReceiptImportResponseModel();
                        POReceiptImportResponseModel.fileName = fileName;
                        POReceiptImportResponseModel.status = StatusEnum.failed.ToString();
                        POReceiptImportResponseModel.errMessage = "The refence schema requred for json validation is not found.";
                        POReceiptImportResponseModelList.Add(POReceiptImportResponseModel);
                        var respFile = new HttpResponseMessage
                        {
                            Content = new StringContent(JsonConvert.SerializeObject(POReceiptImportResponseModelList),
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
                            POReceiptImportResponseModel = new POReceiptImportResponseModel();
                            POReceiptImportResponseModel.fileName = fileName;
                            POReceiptImportResponseModel.status = StatusEnum.failed.ToString();
                            POReceiptImportResponseModel.errMessage = "The json file does not match required schema.";
                            POReceiptImportResponseModelList.Add(POReceiptImportResponseModel);
                            var respJson = new HttpResponseMessage
                            {
                                Content = new StringContent(JsonConvert.SerializeObject(POReceiptImportResponseModelList),
                                              System.Text.Encoding.UTF8, "application/json")
                            };
                            return respJson;
                        }
                    }

                    archiveErr = _POReceiptImportWrapper.ArchiveFileWithDBValue(sftpDirPath, fileName, sftpArchiveDirPath);
                    if (String.IsNullOrEmpty(archiveErr))
                    {
                        POReceiptImportResponseModel = new POReceiptImportResponseModel();
                        POReceiptImportResponseModel.fileName = fileName;
                        POReceiptImportResponseModel.status = StatusEnum.success.ToString();
                        POReceiptImportResponseModel.errMessage = string.Empty;
                        //---process json
                        var respMessage = ProcessLocalPOReceiptImportJsonFile();
                        POReceiptImportResponseModel.poReceiptImportResponseModelList = respMessage;
                        POReceiptImportResponseModelList.Add(POReceiptImportResponseModel);


                    }
                    else
                    {
                        POReceiptImportResponseModel = new POReceiptImportResponseModel();
                        POReceiptImportResponseModel.fileName = fileName;
                        POReceiptImportResponseModel.status = StatusEnum.failed.ToString();
                        POReceiptImportResponseModel.errMessage = archiveErr;
                        POReceiptImportResponseModelList.Add(POReceiptImportResponseModel);
                    }
                }
            }
            #region Send Mail
            if (SFTPCred.isMailSend == true)
            {
                _POReceiptImportWrapper.SendEMail(credentials.UserName, (JsonConvert.SerializeObject(POReceiptImportResponseModelList)));
            }

            #endregion

            var resp = new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(POReceiptImportResponseModelList),
                                               System.Text.Encoding.UTF8, "application/json")
            };
            return resp;
        }
        #endregion
        private List<POReceiptImportResponseModel> ProcessLocalPOReceiptImportJsonFile()
        {
            String FilePath = HttpContext.Current.Server.MapPath("~/JsonSchema/BBU/POReceiptImport/");

            string[] file = Directory.GetFiles(FilePath, "*.json");
            List<POReceiptImportResponseModel> vmrList = new List<POReceiptImportResponseModel>();
            POReceiptImportResponseModel POReceiptImportResponseModel = new POReceiptImportResponseModel();
            using (StreamReader r = new StreamReader(file[0]))
            {
                string json = r.ReadToEnd();
                List<POReceiptImportJSONModel> POReceiptImportModelList = JsonConvert.DeserializeObject<List<POReceiptImportJSONModel>>(json);

                if (POReceiptImportModelList == null)
                {
                    POReceiptImportResponseModel.status = StatusEnum.failed.ToString();
                    POReceiptImportResponseModel.errMessage = "Data is null.";
                    vmrList.Add(POReceiptImportResponseModel);
                    return (vmrList);
                }
                #region POReceiptimportmodel objects converted to data tables
                var result = _POReceiptImportWrapper.CreatePOReceiptImport(POReceiptImportModelList);
                #endregion

                #region create & update log
                var logid = _POReceiptImportWrapper.CreateLog(ApiConstants.OraclePOReceiptImport);

                #endregion

                #region process(import)
                var vResult = _POReceiptImportWrapper.ProcessPOReceiptImport();
                #endregion

                #region UpdateLog
                if (logid != 0)
                {
                    _POReceiptImportWrapper.UpdateLog(logid, result.TotalProcess, result.SuccessfulProcess, result.FailedProcess, result.logMessage, Common.Constants.ApiConstants.OraclePOReceiptImport);
                }
                #endregion

                if (vResult.FailedProcess != null && vResult.FailedProcess > 0)
                {
                    POReceiptImportResponseModel.status = StatusEnum.failed.ToString();
                    POReceiptImportResponseModel.errMessage = vResult.logMessage;
                }
                else
                {
                    POReceiptImportResponseModel.status = StatusEnum.success.ToString();
                    POReceiptImportResponseModel.errMessage = vResult.logMessage;
                }
                vmrList.Add(POReceiptImportResponseModel);
            }
            //delete all files from local directory
            _POReceiptImportWrapper.DeleteLocalFiles(Path.GetDirectoryName(jsonPath));
            _POReceiptImportWrapper.DeleteLocalFiles(Path.GetDirectoryName(encryptedPath));
            _POReceiptImportWrapper.DeleteLocalFiles(Path.GetDirectoryName(decryptedPath));


            return vmrList;
        }
    }
}