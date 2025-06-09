using Common.Constants;
using InterfaceAPI.ActionFilters;
using InterfaceAPI.BusinessWrapper.Implementation;
using InterfaceAPI.BusinessWrapper.Implementation.BBU;
using InterfaceAPI.BusinessWrapper.Interface;
using InterfaceAPI.BusinessWrapper.Interface.BBU;
using InterfaceAPI.CommonUtility;
using InterfaceAPI.Models.BBU.PartMasterResponseImport;
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

namespace InterfaceAPI.Controllers.BBU.PartMasterResponseImport
{
    //[EnableCors("*", "*", "*")]
    //[CustomAuthorize]

    public class PartMasterResponseImportController : BaseApiController
    {
        readonly IPartMasterResponseImportWrapper _PartMasterResponseImportWrapper;
        public long logID { get; set; }
        public static string jsonPath { get; set; }
        public static string encryptedPath { get; set; }
        public static string decryptedPath { get; set; }
        public PartMasterResponseImportController(IImportCSVWrapper ImportCSVWrapper, IPartMasterResponseImportWrapper PartMasterResponseImportWrapper)
        {
            _PartMasterResponseImportWrapper = PartMasterResponseImportWrapper;
        }
        [HttpPost]
        public HttpResponseMessage PartMasterResponseImport([FromBody] SchedulerAPICredentials credentials)
        {
            List<PartMasterResponseImportResponseModel> PartMasterResponseImportResponseModelList = new List<PartMasterResponseImportResponseModel>();
            #region Authentication
            ClaimsPrincipal currentClaimsPrincipal = ClaimsPrincipal.Current;
            if (!_PartMasterResponseImportWrapper.CheckAuthentication(credentials.UserName, credentials.Password))
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Authentication Failed");
            }
            if (_PartMasterResponseImportWrapper.IsMaintenanceGoingOn())
            {
                return Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, "ServiceUnavailable");
            }
            #endregion
            #region Interface Setup Properties Check 
            if (!_PartMasterResponseImportWrapper.CheckIsActiveInterface(ApiConstants.OraclePartMasterResponseImport))
            {
                return Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, "The process is not enabled for the client.");
            }
            #endregion
            #region Initiate SFTP Values
            _PartMasterResponseImportWrapper.RetrieveInterfacePropValues(ApiConstants.OraclePartMasterResponseImport);
            #endregion

            #region Test Encrypt and UploadFile to SFTP
            //_PartMasterResponseImportWrapper.EncryptandUploadFile("06_Sample Data_Requisition Interface.txt", ApiConstants.OraclePartMasterResponseImport);//---comment out before publish
            #endregion

            #region Retrieve Interface Prop values for SFTP File Download


            var sftpDirPath = SFTPCred.ftpFileDirectory;
            var sftpArchiveDirPath = SFTPCred.ftpArcDirectory;
            string result = string.Empty;
            string errCsv = string.Empty;
            var fileList = _PartMasterResponseImportWrapper.GetFileList(sftpDirPath);

            #endregion


            #region Read files fron SFTP
            PartMasterResponseImportResponseModel PartMasterResponseImportResponseModel;
            //List<PartMasterResponseImportResponseModel> PartMasterResponseImportResponseModelList = new List<PartMasterResponseImportResponseModel>();
            if (fileList != null && fileList.Count == 0)
            {
                PartMasterResponseImportResponseModel = new PartMasterResponseImportResponseModel();
                PartMasterResponseImportResponseModel.status = StatusEnum.failed.ToString();
                PartMasterResponseImportResponseModel.errMessage = "No file found at source directory.";
                PartMasterResponseImportResponseModelList.Add(PartMasterResponseImportResponseModel);
                var respFile = new HttpResponseMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(PartMasterResponseImportResponseModelList),
                                            System.Text.Encoding.UTF8, "application/json")
                };
                return respFile;
            }

            //---Get json from csv---
            foreach (var fileName in fileList)
            {
                string dcFileName;
                encryptedPath = _PartMasterResponseImportWrapper.DownloadToLocal(fileName, ApiConstants.OraclePartMasterResponseImport);
                // RKL - 2021-Aug-13
                if (SFTPCred.filesEncrypted)
                {
                    decryptedPath = _PartMasterResponseImportWrapper.DecryptFile(fileName, encryptedPath, ApiConstants.OraclePartMasterResponseImport);
                    if (fileName.Contains(".txt.pgp"))
                    {
                        dcFileName = fileName.Replace(".txt.pgp", ".txt");
                    }
                    else
                    {
                        dcFileName = fileName.Replace(".pgp", ".txt");
                    }
                }
                else
                {
                    decryptedPath = encryptedPath;
                    dcFileName = fileName;
                }
                string thisExtension = Utility.GetFileExtension(dcFileName);

                if (!(thisExtension.Contains(FileExtensionEnum.txt.ToString()) || thisExtension.Contains(FileExtensionEnum.csv.ToString())))
                {
                    PartMasterResponseImportResponseModel = new PartMasterResponseImportResponseModel();
                    PartMasterResponseImportResponseModel.fileName = dcFileName;
                    PartMasterResponseImportResponseModel.status = StatusEnum.failed.ToString();
                    PartMasterResponseImportResponseModel.errMessage = "File type " + thisExtension + " is not supported.";
                    PartMasterResponseImportResponseModelList.Add(PartMasterResponseImportResponseModel);
                }
                else
                {
                    //var ReflectionProperty = typeof(PartMasterResponseImportJSONModel).GetProperties().ToList();
                    //string[] PropertyNames = ReflectionProperty.Select(x => x.Name).ToArray();
                    //result = _PartMasterResponseImportWrapper.GetJsonFromDownloadedSFTPFiles(decryptedPath, dcFileName, FileTypeEnum.OraclePartMasterResponseImport, PropertyNames);
                    result = _PartMasterResponseImportWrapper.GetJsonFromDownloadedSFTPFiles(decryptedPath, fileName, FileTypeEnum.OraclePartMasterResponseImport);
                    if (result.Equals(StatusEnum.ErrorConvertingJson.ToString()))
                    {
                        PartMasterResponseImportResponseModel = new PartMasterResponseImportResponseModel();
                        PartMasterResponseImportResponseModel.fileName = dcFileName;
                        PartMasterResponseImportResponseModel.status = StatusEnum.failed.ToString();
                        PartMasterResponseImportResponseModel.errMessage = "File read failure.";
                        PartMasterResponseImportResponseModelList.Add(PartMasterResponseImportResponseModel);
                        continue;
                    }
                    //else if (result.Equals(StatusEnum.ColumnMismatch.ToString()))
                    //{
                    //    PartMasterResponseImportResponseModel = new PartMasterResponseImportResponseModel();
                    //    PartMasterResponseImportResponseModel.fileName = dcFileName;
                    //    PartMasterResponseImportResponseModel.status = StatusEnum.failed.ToString();
                    //    PartMasterResponseImportResponseModel.errMessage = "File is expected to be containing these columns - " +
                    //      String.Join(", ", PropertyNames) + " .";
                    //    PartMasterResponseImportResponseModelList.Add(PartMasterResponseImportResponseModel);
                    //    continue;
                    //}
                    //--- serialize JSON directly to a file
                    List<PartMasterResponseImportJSONModel> aList = new List<PartMasterResponseImportJSONModel>();
                    aList = JsonConvert.DeserializeObject<List<PartMasterResponseImportJSONModel>>(result.ToString());
                    string firstName = Utility.GetFileName(fileName);
                    jsonPath = HttpContext.Current.Server.MapPath("~/JsonSchema/BBU/PartMasterResponseImport/" + firstName + ".json");//Create Json Schema
                    using (StreamWriter file = File.CreateText(jsonPath))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(file, aList);
                    }
                    //---compare the JSON with schema
                    JsonSchema jschema = new JsonSchema();

                    String schemaPath = HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["OraclePartMasterResponseImportSchemaPath"]);// Create entry in webconfig
                    ////---check if schema file exists. If not return relevent response---
                    if (!File.Exists(schemaPath))
                    {
                        PartMasterResponseImportResponseModel = new PartMasterResponseImportResponseModel();
                        PartMasterResponseImportResponseModel.fileName = fileName;
                        PartMasterResponseImportResponseModel.status = StatusEnum.failed.ToString();
                        PartMasterResponseImportResponseModel.errMessage = "The refence schema requred for json validation is not found.";
                        PartMasterResponseImportResponseModelList.Add(PartMasterResponseImportResponseModel);
                        var respFile = new HttpResponseMessage
                        {
                            Content = new StringContent(JsonConvert.SerializeObject(PartMasterResponseImportResponseModelList),
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
                            PartMasterResponseImportResponseModel = new PartMasterResponseImportResponseModel();
                            PartMasterResponseImportResponseModel.fileName = fileName;
                            PartMasterResponseImportResponseModel.status = StatusEnum.failed.ToString();
                            PartMasterResponseImportResponseModel.errMessage = "The json file does not match required schema.";
                            PartMasterResponseImportResponseModelList.Add(PartMasterResponseImportResponseModel);
                            var respJson = new HttpResponseMessage
                            {
                                Content = new StringContent(JsonConvert.SerializeObject(PartMasterResponseImportResponseModelList),
                                              System.Text.Encoding.UTF8, "application/json")
                            };
                            return respJson;
                        }
                    }

                    archiveErr = _PartMasterResponseImportWrapper.ArchiveFileWithDBValue(sftpDirPath, fileName, sftpArchiveDirPath);
                    if (String.IsNullOrEmpty(archiveErr))
                    {
                        PartMasterResponseImportResponseModel = new PartMasterResponseImportResponseModel();
                        PartMasterResponseImportResponseModel.fileName = fileName;
                        PartMasterResponseImportResponseModel.status = StatusEnum.success.ToString();
                        PartMasterResponseImportResponseModel.errMessage = string.Empty;
                        //---process json
                        var respMessage = ProcessLocalPartMasterResponseImportJsonFile();
                        PartMasterResponseImportResponseModel.partMasterResponseImportResponseModelList = respMessage;
                        PartMasterResponseImportResponseModelList.Add(PartMasterResponseImportResponseModel);


                    }
                    else
                    {
                        PartMasterResponseImportResponseModel = new PartMasterResponseImportResponseModel();
                        PartMasterResponseImportResponseModel.fileName = fileName;
                        PartMasterResponseImportResponseModel.status = StatusEnum.failed.ToString();
                        PartMasterResponseImportResponseModel.errMessage = archiveErr;
                        PartMasterResponseImportResponseModelList.Add(PartMasterResponseImportResponseModel);
                    }
                }
            }
            #region Send Mail
            if (SFTPCred.isMailSend == true)
            {
                _PartMasterResponseImportWrapper.SendEMail(credentials.UserName, (JsonConvert.SerializeObject(PartMasterResponseImportResponseModelList)));
            }

            #endregion

            var resp = new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(PartMasterResponseImportResponseModelList),
                                               System.Text.Encoding.UTF8, "application/json")
            };
            return resp;
        }
        #endregion
        private List<PartMasterResponseImportResponseModel> ProcessLocalPartMasterResponseImportJsonFile()
        {
            String FilePath = HttpContext.Current.Server.MapPath("~/JsonSchema/BBU/PartMasterResponseImport/");

            string[] file = Directory.GetFiles(FilePath, "*.json");
            List<PartMasterResponseImportResponseModel> vmrList = new List<PartMasterResponseImportResponseModel>();
            PartMasterResponseImportResponseModel PartMasterResponseImportResponseModel = new PartMasterResponseImportResponseModel();
            using (StreamReader r = new StreamReader(file[0]))
            {
                string json = r.ReadToEnd();
                List<PartMasterResponseImportJSONModel> PartMasterResponseImportModelList = JsonConvert.DeserializeObject<List<PartMasterResponseImportJSONModel>>(json);
                #region create & update log
                var logid = _PartMasterResponseImportWrapper.CreateLog(ApiConstants.OraclePartMasterResponseImport);
                #endregion

                if (PartMasterResponseImportModelList == null)
                {
                    PartMasterResponseImportResponseModel.status = StatusEnum.failed.ToString();
                    PartMasterResponseImportResponseModel.errMessage = "Data is null.";
                    vmrList.Add(PartMasterResponseImportResponseModel);
                    return (vmrList);
                }


                #region PartMasterResponseImportmodel objects converted to data tables
                var result = _PartMasterResponseImportWrapper.CreatePartMasterResponseImport(PartMasterResponseImportModelList, logid);
                #endregion



                #region process(import)
                var vResult = _PartMasterResponseImportWrapper.ProcessPartMasterResponseImport();
                #endregion

                #region UpdateLog
                if (logid != 0)
                {
                    _PartMasterResponseImportWrapper.UpdateLog(logid, result.TotalProcess, result.SuccessfulProcess, result.FailedProcess, result.logMessage, Common.Constants.ApiConstants.OraclePartMasterResponseImport);
                }
                #endregion

                if (vResult.FailedProcess != null && vResult.FailedProcess > 0)
                {
                    PartMasterResponseImportResponseModel.status = StatusEnum.failed.ToString();
                    PartMasterResponseImportResponseModel.errMessage = vResult.logMessage;
                }
                else
                {
                    PartMasterResponseImportResponseModel.status = StatusEnum.success.ToString();
                    PartMasterResponseImportResponseModel.errMessage = vResult.logMessage;
                }
                vmrList.Add(PartMasterResponseImportResponseModel);
            }
            //delete all files from local directory
            _PartMasterResponseImportWrapper.DeleteLocalFiles(Path.GetDirectoryName(jsonPath));
            _PartMasterResponseImportWrapper.DeleteLocalFiles(Path.GetDirectoryName(encryptedPath));
            _PartMasterResponseImportWrapper.DeleteLocalFiles(Path.GetDirectoryName(decryptedPath));


            return vmrList;
        }
    }
}