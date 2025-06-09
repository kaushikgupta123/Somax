using Common.Constants;
using InterfaceAPI.ActionFilters;
using InterfaceAPI.BusinessWrapper.Implementation;
using InterfaceAPI.BusinessWrapper.Implementation.BBU;
using InterfaceAPI.BusinessWrapper.Interface;
using InterfaceAPI.BusinessWrapper.Interface.BBU;
using InterfaceAPI.CommonUtility;
using InterfaceAPI.Models.BBU.PartMasterImport;
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

namespace InterfaceAPI.Controllers.BBU.PartMasterImport
{
    //[EnableCors("*", "*", "*")]
    //[CustomAuthorize]

    public class PartMasterImportController : BaseApiController
    {
        readonly IPartMasterImportWrapper _PartMasterImportWrapper;
        public long logID { get; set; }
        public static string jsonPath { get; set; }
        public static string encryptedPath { get; set; }
        public static string decryptedPath { get; set; }
        public PartMasterImportController(IImportCSVWrapper ImportCSVWrapper, IPartMasterImportWrapper PartMasterImportWrapper)
        {
            _PartMasterImportWrapper = PartMasterImportWrapper;
        }
        [HttpPost]
        public HttpResponseMessage PartMasterImport([FromBody] SchedulerAPICredentials credentials)
        {
            List<PartMasterImportResponseModel> PartMasterImportResponseModelList = new List<PartMasterImportResponseModel>();
            #region Authentication
            ClaimsPrincipal currentClaimsPrincipal = ClaimsPrincipal.Current;
            if (!_PartMasterImportWrapper.CheckAuthentication(credentials.UserName, credentials.Password))
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Authentication Failed");
            }
            if (_PartMasterImportWrapper.IsMaintenanceGoingOn())
            {
                return Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, "ServiceUnavailable");
            }
            #endregion
            #region Interface Setup Properties Check 
            if (!_PartMasterImportWrapper.CheckIsActiveInterface(ApiConstants.PartMasterImport))
            {
                return Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, "The process is not enabled for the client.");
            }
            #endregion
            #region Initiate SFTP Values
            _PartMasterImportWrapper.RetrieveInterfacePropValues(ApiConstants.PartMasterImport);
            #endregion

            #region Test Encrypt and UploadFile to SFTP
            //_PartMasterImportWrapper.EncryptandUploadFile("01_Sample Data_Parts Master Interface.txt", ApiConstants.PartMasterImport);//---comment out before publish
            //_PartMasterImportWrapper.EncryptandUploadFile("SOMAXITEM-DAT-0616.txt", ApiConstants.PartMasterImport);//---comment out before publish
            #endregion

            #region Retrieve Interface Prop values for SFTP File Download
            var sftpDirPath = SFTPCred.ftpFileDirectory;
            var sftpArchiveDirPath = SFTPCred.ftpArcDirectory;
            string result = string.Empty;
            string errCsv = string.Empty;
            var fileList = _PartMasterImportWrapper.GetFileList(sftpDirPath);

            #endregion


            #region Read files fron SFTP
            PartMasterImportResponseModel partMasterImportResponseModel;
            List<PartMasterImportResponseModel> partMasterImportResponseModelList = new List<PartMasterImportResponseModel>();
            if (fileList != null && fileList.Count == 0)
            {
                partMasterImportResponseModel = new PartMasterImportResponseModel();
                partMasterImportResponseModel.status = StatusEnum.failed.ToString();
                partMasterImportResponseModel.errMessage = "No file found at source directory.";
                partMasterImportResponseModelList.Add(partMasterImportResponseModel);
                var respFile = new HttpResponseMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(partMasterImportResponseModelList),
                                            System.Text.Encoding.UTF8, "application/json")
                };
                return respFile;
            }

            //---Get json from csv---
            foreach (var fileName in fileList)
            {
                // If the file is encrypted - we will need to change the extension
                string dcFileName;  // decrypted file name
                // Download the file to the local directory
                encryptedPath = _PartMasterImportWrapper.DownloadToLocal(fileName, ApiConstants.PartMasterImport);
                // If the file is encrypted - decrypt - the extension will change from .pgp to .txt
                if (SFTPCred.filesEncrypted)
                {
                    decryptedPath = _PartMasterImportWrapper.DecryptFile(fileName, encryptedPath, ApiConstants.PartMasterImport);
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
                    partMasterImportResponseModel = new PartMasterImportResponseModel();
                    partMasterImportResponseModel.fileName = dcFileName;
                    //partMasterImportResponseModel.fileName = fileName;
                    partMasterImportResponseModel.status = StatusEnum.failed.ToString();
                    partMasterImportResponseModel.errMessage = "File type " + thisExtension + " is not supported.";
                    partMasterImportResponseModelList.Add(partMasterImportResponseModel);
                }
                else
                {
                    //var ReflectionProperty = typeof(PartMasterImportJSONModel).GetProperties().ToList();
                    //string[] PropertyNames = ReflectionProperty.Select(x => x.Name).ToArray();
                    //result = _PartMasterImportWrapper.GetJsonFromDownloadedSFTPFiles(decryptedPath, dcFileName, FileTypeEnum.PartMasterImport, PropertyNames);
                    result = _PartMasterImportWrapper.GetJsonFromDownloadedSFTPFiles(decryptedPath, fileName, FileTypeEnum.PartMasterImport);
                    if (result.Equals(StatusEnum.ErrorConvertingJson.ToString()))
                    {
                        partMasterImportResponseModel = new PartMasterImportResponseModel();
                        partMasterImportResponseModel.fileName = dcFileName;
                        partMasterImportResponseModel.status = StatusEnum.failed.ToString();
                        partMasterImportResponseModel.errMessage = "File read failure.";
                        PartMasterImportResponseModelList.Add(partMasterImportResponseModel);
                        continue;
                    }
                    //else if (result.Equals(StatusEnum.ColumnMismatch.ToString()))
                    //{
                    //    partMasterImportResponseModel = new PartMasterImportResponseModel();
                    //    partMasterImportResponseModel.fileName = dcFileName;
                    //    partMasterImportResponseModel.status = StatusEnum.failed.ToString();
                    //    partMasterImportResponseModel.errMessage = "File is expected to be containing these columns - " +
                    //      String.Join(", ", PropertyNames) + " .";
                    //    PartMasterImportResponseModelList.Add(partMasterImportResponseModel);
                    //    continue;
                    //}
                    //--- serialize JSON directly to a file
                    List<PartMasterImportJSONModel> aList = new List<PartMasterImportJSONModel>();
                    aList = JsonConvert.DeserializeObject<List<PartMasterImportJSONModel>>(result.ToString());
                    string firstName = Utility.GetFileName(dcFileName);
                    jsonPath = HttpContext.Current.Server.MapPath("~/JsonSchema/BBU/PartMasterImport/" + firstName + ".json");//Create Json Schema
                    using (StreamWriter file = File.CreateText(jsonPath))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(file, aList);
                    }
                    //---compare the JSON with schema
                    JsonSchema jschema = new JsonSchema();

                    String schemaPath = HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["PartMasterImportSchemaPath"]);// Create entry in webconfig
                    ////---check if schema file exists. If not return relevent response---
                    if (!File.Exists(schemaPath))
                    {
                        partMasterImportResponseModel = new PartMasterImportResponseModel();
                        partMasterImportResponseModel.fileName = dcFileName;
                        partMasterImportResponseModel.status = StatusEnum.failed.ToString();
                        partMasterImportResponseModel.errMessage = "The refence schema requred for json validation is not found.";
                        partMasterImportResponseModelList.Add(partMasterImportResponseModel);
                        var respFile = new HttpResponseMessage
                        {
                            Content = new StringContent(JsonConvert.SerializeObject(partMasterImportResponseModelList),
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
                            partMasterImportResponseModel = new PartMasterImportResponseModel();
                            partMasterImportResponseModel.fileName = dcFileName;
                            partMasterImportResponseModel.status = StatusEnum.failed.ToString();
                            partMasterImportResponseModel.errMessage = "The json file does not match required schema.";
                            partMasterImportResponseModelList.Add(partMasterImportResponseModel);
                            var respJson = new HttpResponseMessage
                            {
                                Content = new StringContent(JsonConvert.SerializeObject(partMasterImportResponseModelList),
                                              System.Text.Encoding.UTF8, "application/json")
                            };
                            return respJson;
                        }
                    }

                    archiveErr = _PartMasterImportWrapper.ArchiveFileWithDBValue(sftpDirPath, fileName, sftpArchiveDirPath);
                    if (String.IsNullOrEmpty(archiveErr))
                    {
                        partMasterImportResponseModel = new PartMasterImportResponseModel();
                        partMasterImportResponseModel.fileName = fileName;
                        partMasterImportResponseModel.status = StatusEnum.success.ToString();
                        partMasterImportResponseModel.errMessage = string.Empty;
                        //---process json
                        var respMessage = ProcessLocalPartMasterImportJsonFile();
                        partMasterImportResponseModel.PartMasterImportResponseModelList = respMessage;
                        PartMasterImportResponseModelList.Add(partMasterImportResponseModel);


                    }
                    else
                    {
                        partMasterImportResponseModel = new PartMasterImportResponseModel();
                        partMasterImportResponseModel.fileName = fileName;
                        partMasterImportResponseModel.status = StatusEnum.failed.ToString();
                        partMasterImportResponseModel.errMessage = archiveErr;
                        PartMasterImportResponseModelList.Add(partMasterImportResponseModel);
                    }
                }
            }
            #region Send Mail
            if (SFTPCred.isMailSend == true)
            {
                _PartMasterImportWrapper.SendEMail(credentials.UserName, (JsonConvert.SerializeObject(PartMasterImportResponseModelList)));
            }

            #endregion

            var resp = new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(PartMasterImportResponseModelList),
                                               System.Text.Encoding.UTF8, "application/json")
            };
            return resp;
        }
        #endregion
        private List<PartMasterImportResponseModel> ProcessLocalPartMasterImportJsonFile()
        {
            String FilePath = HttpContext.Current.Server.MapPath("~/JsonSchema/BBU/PartMasterImport/");

            string[] file = Directory.GetFiles(FilePath, "*.json");
            List<PartMasterImportResponseModel> vmrList = new List<PartMasterImportResponseModel>();
            PartMasterImportResponseModel partMasterImportResponseModel = new PartMasterImportResponseModel();
            using (StreamReader r = new StreamReader(file[0]))
            {
                string json = r.ReadToEnd();
                List<PartMasterImportJSONModel> partMasterImportModelList = JsonConvert.DeserializeObject<List<PartMasterImportJSONModel>>(json);

                if (partMasterImportModelList == null)
                {
                    partMasterImportResponseModel.status = StatusEnum.failed.ToString();
                    partMasterImportResponseModel.errMessage = "Data is null.";
                    vmrList.Add(partMasterImportResponseModel);
                    return (vmrList);
                }
                #region partmasterimportmodel objects converted to data tables
                var result = _PartMasterImportWrapper.CreatePartMasterImport(partMasterImportModelList);
                #endregion

                #region create & update log
                var logid = _PartMasterImportWrapper.CreateLog(ApiConstants.PartMasterImport);

                #endregion

                #region process(import)
                var vResult = _PartMasterImportWrapper.ProcessPartMasterImport();
                #endregion

                #region UpdateLog
                if (logid != 0)
                {
                    _PartMasterImportWrapper.UpdateLog(logid, result.TotalProcess, result.SuccessfulProcess, result.FailedProcess, result.logMessage, Common.Constants.ApiConstants.PartMasterImport);
                }
                #endregion

                if (vResult.FailedProcess != null && vResult.FailedProcess > 0)
                {
                    partMasterImportResponseModel.status = StatusEnum.failed.ToString();
                    partMasterImportResponseModel.errMessage = vResult.logMessage;
                }
                else
                {
                    partMasterImportResponseModel.status = StatusEnum.success.ToString();
                    partMasterImportResponseModel.errMessage = vResult.logMessage;
                }
                vmrList.Add(partMasterImportResponseModel);
            }
            //delete all files from local directory
            _PartMasterImportWrapper.DeleteLocalFiles(Path.GetDirectoryName(jsonPath));
            _PartMasterImportWrapper.DeleteLocalFiles(Path.GetDirectoryName(encryptedPath));
            _PartMasterImportWrapper.DeleteLocalFiles(Path.GetDirectoryName(decryptedPath));


            return vmrList;
        }
    }
}