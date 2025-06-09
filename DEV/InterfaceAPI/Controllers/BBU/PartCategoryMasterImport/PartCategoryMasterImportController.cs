using Common.Constants;

using InterfaceAPI.BusinessWrapper.Implementation;
using InterfaceAPI.BusinessWrapper.Interface;
using InterfaceAPI.BusinessWrapper.Interface.BBU;
using InterfaceAPI.CommonUtility;
using InterfaceAPI.Models.BBU.PartCategoryMasterImport;
using InterfaceAPI.Models.Common;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace InterfaceAPI.Controllers.BBU.PartCategoryMasterImport
{
    //[EnableCors("*", "*", "*")]
    //[CustomAuthorize]

    public class PartCategoryMasterImportController : BaseApiController
    {
        readonly IPartCategoryMasterImportWrapper _PartCategoryMasterImportWrapper;
        public long logID { get; set; }
        public static string jsonPath { get; set; }
        public static string encryptedPath { get; set; }
        public static string decryptedPath { get; set; }
        public PartCategoryMasterImportController(IImportCSVWrapper ImportCSVWrapper, IPartCategoryMasterImportWrapper PartCategoryMasterImportWrapper)
        {
            _PartCategoryMasterImportWrapper = PartCategoryMasterImportWrapper;
        }
        [HttpPost]
        public HttpResponseMessage PartCategoryMasterImport([FromBody] SchedulerAPICredentials credentials)
        {
            List<PartCategoryMasterImportResponseModel> PartCategoryMasterImportResponseModelList = new List<PartCategoryMasterImportResponseModel>();

            #region Authentication
            //ClaimsPrincipal currentClaimsPrincipal = ClaimsPrincipal.Current;
            if (!_PartCategoryMasterImportWrapper.CheckAuthentication(credentials.UserName, credentials.Password))
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Authentication Failed");
            }
            if (_PartCategoryMasterImportWrapper.IsMaintenanceGoingOn())
            {
                return Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, "ServiceUnavailable");
            }
            #endregion

            #region Interface Setup Properties Check 
            if (!_PartCategoryMasterImportWrapper.CheckIsActiveInterface(ApiConstants.PartCategoryMasterImport))
            {
                return Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, "The process is not enabled for the client.");
            }
            #endregion

            #region Initiate SFTP Values
            _PartCategoryMasterImportWrapper.RetrieveInterfacePropValues(ApiConstants.PartCategoryMasterImport);
            #endregion

            #region Test Encrypt and UploadFile to SFTP
            //_PartCategoryMasterImportWrapper.EncryptandUploadFile("sample1.txt", ApiConstants.PartCategoryMasterImport);//---comment out before publish
            //_PartCategoryMasterImportWrapper.EncryptandUploadFile("sample2.txt", ApiConstants.PartCategoryMasterImport);//---comment out before publish
            #endregion

            #region Retrieve Interface Prop values for SFTP File Download
            var sftpDirPath = SFTPCred.ftpFileDirectory;
            var sftpArchiveDirPath = SFTPCred.ftpArcDirectory;
            string result = string.Empty;
            string errCsv = string.Empty;
            var fileList = _PartCategoryMasterImportWrapper.GetFileList(sftpDirPath);
            #endregion

            #region Read files fron SFTP
            PartCategoryMasterImportResponseModel partCategoryMasterImportResponseModel;
            List<PartCategoryMasterImportResponseModel> partCategoryMasterImportResponseModelList = new List<PartCategoryMasterImportResponseModel>();
            if (fileList != null && fileList.Count == 0)
            {
                partCategoryMasterImportResponseModel = new PartCategoryMasterImportResponseModel();
                partCategoryMasterImportResponseModel.status = StatusEnum.failed.ToString();
                partCategoryMasterImportResponseModel.errMessage = "No file found at source directory.";
                partCategoryMasterImportResponseModelList.Add(partCategoryMasterImportResponseModel);
                var respFile = new HttpResponseMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(partCategoryMasterImportResponseModelList),
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
                encryptedPath = _PartCategoryMasterImportWrapper.DownloadToLocal(fileName, ApiConstants.PartCategoryMasterImport);
                // If the file is encrypted - decrypt - the extension will change from .pgp to .txt
                if (SFTPCred.filesEncrypted)
                {
                    decryptedPath = _PartCategoryMasterImportWrapper.DecryptFile(fileName, encryptedPath, ApiConstants.PartCategoryMasterImport);
                    dcFileName = fileName.Replace(".txt.pgp", ".txt");
                    dcFileName = dcFileName.Replace(".pgp", ".txt");
                }
                else
                {
                    decryptedPath = encryptedPath;
                    dcFileName = fileName;
                }
                string thisExtension = Utility.GetFileExtension(dcFileName);
                if (!(thisExtension.Contains(FileExtensionEnum.txt.ToString()) || thisExtension.Contains(FileExtensionEnum.csv.ToString())))
                {
                    partCategoryMasterImportResponseModel = new PartCategoryMasterImportResponseModel();
                    partCategoryMasterImportResponseModel.fileName = dcFileName;
                    //partMasterImportResponseModel.fileName = fileName;
                    partCategoryMasterImportResponseModel.status = StatusEnum.failed.ToString();
                    partCategoryMasterImportResponseModel.errMessage = "File type " + thisExtension + " is not supported.";
                    PartCategoryMasterImportResponseModelList.Add(partCategoryMasterImportResponseModel);
                }
                else
                {
                    //var ReflectionProperty = typeof(PartCategoryMasterImportJSONModel).GetProperties().ToList();
                    //string[] PropertyNames = ReflectionProperty.Select(x => x.Name).ToArray();
                    //result = _PartCategoryMasterImportWrapper.GetJsonFromDownloadedSFTPFiles(decryptedPath, dcFileName, FileTypeEnum.PartCategoryMasterImport, PropertyNames);
                    result = _PartCategoryMasterImportWrapper.GetJsonFromDownloadedSFTPFiles(decryptedPath, fileName, FileTypeEnum.PartMasterImport);
                    if (result.Equals(StatusEnum.ErrorConvertingJson.ToString()))
                    {
                        partCategoryMasterImportResponseModel = new PartCategoryMasterImportResponseModel();
                        partCategoryMasterImportResponseModel.fileName = dcFileName;
                        partCategoryMasterImportResponseModel.status = StatusEnum.failed.ToString();
                        partCategoryMasterImportResponseModel.errMessage = "File read failure.";
                        PartCategoryMasterImportResponseModelList.Add(partCategoryMasterImportResponseModel);
                        continue;
                    }
                    //else if (result.Equals(StatusEnum.ColumnMismatch.ToString()))
                    //{
                    //    partCategoryMasterImportResponseModel = new PartCategoryMasterImportResponseModel();
                    //    partCategoryMasterImportResponseModel.fileName = dcFileName;
                    //    partCategoryMasterImportResponseModel.status = StatusEnum.failed.ToString();
                    //    partCategoryMasterImportResponseModel.errMessage = "File is expected to be containing these columns - " +
                    //      String.Join(", ", PropertyNames) + " .";
                    //    PartCategoryMasterImportResponseModelList.Add(partCategoryMasterImportResponseModel);
                    //    continue;
                    //}
                    //--- serialize JSON directly to a file
                    List<PartCategoryMasterImportJSONModel> aList = new List<PartCategoryMasterImportJSONModel>();
                    aList = JsonConvert.DeserializeObject<List<PartCategoryMasterImportJSONModel>>(result.ToString());

                    string firstName = Utility.GetFileName(dcFileName);
                    jsonPath = HttpContext.Current.Server.MapPath("~/JsonSchema/BBU/PartCategoryMasterImport/" + firstName + ".json");//Create Json Schema
                    using (StreamWriter file = File.CreateText(jsonPath))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(file, aList);
                    }
                    //---compare the JSON with schema
                    JsonSchema jschema = new JsonSchema();

                    String schemaPath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["PartCategoryMasterImportSchemaPath"]);// Create entry in webconfig
                    ////---check if schema file exists. If not return relevent response---
                    if (!File.Exists(schemaPath))
                    {
                        partCategoryMasterImportResponseModel = new PartCategoryMasterImportResponseModel();
                        partCategoryMasterImportResponseModel.fileName = dcFileName;
                        partCategoryMasterImportResponseModel.status = StatusEnum.failed.ToString();
                        partCategoryMasterImportResponseModel.errMessage = "The refence schema requred for json validation is not found.";
                        partCategoryMasterImportResponseModelList.Add(partCategoryMasterImportResponseModel);
                        var respFile = new HttpResponseMessage
                        {
                            Content = new StringContent(JsonConvert.SerializeObject(partCategoryMasterImportResponseModelList),
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
                            partCategoryMasterImportResponseModel = new PartCategoryMasterImportResponseModel();
                            partCategoryMasterImportResponseModel.fileName = dcFileName;
                            partCategoryMasterImportResponseModel.status = StatusEnum.failed.ToString();
                            partCategoryMasterImportResponseModel.errMessage = "The json file does not match required schema.";
                            partCategoryMasterImportResponseModelList.Add(partCategoryMasterImportResponseModel);
                            var respJson = new HttpResponseMessage
                            {
                                Content = new StringContent(JsonConvert.SerializeObject(partCategoryMasterImportResponseModelList),
                                              System.Text.Encoding.UTF8, "application/json")
                            };
                            return respJson;
                        }
                    }

                    archiveErr = _PartCategoryMasterImportWrapper.ArchiveFileWithDBValue(sftpDirPath, fileName, sftpArchiveDirPath);
                    if (String.IsNullOrEmpty(archiveErr))
                    {
                        partCategoryMasterImportResponseModel = new PartCategoryMasterImportResponseModel();
                        partCategoryMasterImportResponseModel.fileName = fileName;
                        partCategoryMasterImportResponseModel.status = StatusEnum.success.ToString();
                        partCategoryMasterImportResponseModel.errMessage = string.Empty;
                        //---process json
                        var respMessage = ProcessLocalPartMasterImportJsonFile();
                        partCategoryMasterImportResponseModel.PartCategoryMasterImportResponseModelList = respMessage;
                        PartCategoryMasterImportResponseModelList.Add(partCategoryMasterImportResponseModel);


                    }
                    else
                    {
                        partCategoryMasterImportResponseModel = new PartCategoryMasterImportResponseModel();
                        partCategoryMasterImportResponseModel.fileName = fileName;
                        partCategoryMasterImportResponseModel.status = StatusEnum.failed.ToString();
                        partCategoryMasterImportResponseModel.errMessage = archiveErr;
                        PartCategoryMasterImportResponseModelList.Add(partCategoryMasterImportResponseModel);
                    }
                }
            }
            #region Send Mail
            if (SFTPCred.isMailSend == true)
            {
                _PartCategoryMasterImportWrapper.SendEMail(credentials.UserName, (JsonConvert.SerializeObject(PartCategoryMasterImportResponseModelList)));
            }

            #endregion

            var resp = new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(PartCategoryMasterImportResponseModelList),
                                               System.Text.Encoding.UTF8, "application/json")
            };
            return resp;
            #endregion
        }
        private List<PartCategoryMasterImportResponseModel> ProcessLocalPartMasterImportJsonFile()
        {
            String FilePath = HttpContext.Current.Server.MapPath("~/JsonSchema/BBU/PartCategoryMasterImport/");

            string[] file = Directory.GetFiles(FilePath, "*.json");
            List<PartCategoryMasterImportResponseModel> vmrList = new List<PartCategoryMasterImportResponseModel>();
            PartCategoryMasterImportResponseModel partCategoryMasterImportResponseModel = new PartCategoryMasterImportResponseModel();
            using (StreamReader r = new StreamReader(file[0]))
            {
                string json = r.ReadToEnd();
                List<PartCategoryMasterImportJSONModel> partCategoryMasterImportModelList = JsonConvert.DeserializeObject<List<PartCategoryMasterImportJSONModel>>(json);

                if (partCategoryMasterImportModelList == null)
                {
                    partCategoryMasterImportResponseModel.status = StatusEnum.failed.ToString();
                    partCategoryMasterImportResponseModel.errMessage = "Data is null.";
                    vmrList.Add(partCategoryMasterImportResponseModel);
                    return (vmrList);
                }
                #region partmasterimportmodel objects converted to data tables
                var result = _PartCategoryMasterImportWrapper.CreatePartCategoryMasterImport(partCategoryMasterImportModelList);
                #endregion

                #region create & update log
                var logid = _PartCategoryMasterImportWrapper.CreateLog(ApiConstants.PartCategoryMasterImport);

                #endregion

                #region process(import)
                var vResult = _PartCategoryMasterImportWrapper.ProcessPartCategoryMasterImport();
                #endregion

                #region UpdateLog
                if (logid != 0)
                {
                    _PartCategoryMasterImportWrapper.UpdateLog(logid, result.TotalProcess, result.SuccessfulProcess, result.FailedProcess, result.logMessage, ApiConstants.PartCategoryMasterImport);
                }
                #endregion

                if (vResult.FailedProcess != null && vResult.FailedProcess > 0)
                {
                    partCategoryMasterImportResponseModel.status = StatusEnum.failed.ToString();
                    partCategoryMasterImportResponseModel.errMessage = vResult.logMessage;
                }
                else
                {
                    partCategoryMasterImportResponseModel.status = StatusEnum.success.ToString();
                    partCategoryMasterImportResponseModel.errMessage = vResult.logMessage;
                }
                vmrList.Add(partCategoryMasterImportResponseModel);
            }
            //delete all files from local directory
            _PartCategoryMasterImportWrapper.DeleteLocalFiles(Path.GetDirectoryName(jsonPath));
            _PartCategoryMasterImportWrapper.DeleteLocalFiles(Path.GetDirectoryName(encryptedPath));
            _PartCategoryMasterImportWrapper.DeleteLocalFiles(Path.GetDirectoryName(decryptedPath));


            return vmrList;
        }
    }
}