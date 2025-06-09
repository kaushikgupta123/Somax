using System;
using System.Collections.Generic;
using System.Web;
using InterfaceAPI.BusinessWrapper.Interface;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Web.Http;
using Common.Constants;
using InterfaceAPI.Models.Common;
using InterfaceAPI.CommonUtility;
using InterfaceAPI.BusinessWrapper.Implementation;
using InterfaceAPI.Models.MonnitIoTReading;

namespace InterfaceAPI.Controllers
{
    public class MonnitIoTReadingImportController : BaseApiController
    {
        readonly IImportCSVWrapper _ImportCSVWrapper;
        readonly IMonnitIoTReadingImportWrapper _MonnitIoTReadingImportWrapper;
        public MonnitIoTReadingImportController(IImportCSVWrapper ImportCSVWrapper, IMonnitIoTReadingImportWrapper MonnitIoTReadingImportWrapper)
        {
            _ImportCSVWrapper = ImportCSVWrapper;
            _MonnitIoTReadingImportWrapper = MonnitIoTReadingImportWrapper;
        }
        public long logID { get; set; }
        [HttpPost]
        public HttpResponseMessage IoTReadingImport([FromBody] SchedulerAPICredentials credentials)
        {
            #region Authentication
            if (!_MonnitIoTReadingImportWrapper.CheckAuthentication(credentials.UserName, credentials.Password))
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Authentication Failed");
            }
            if (_MonnitIoTReadingImportWrapper.IsMaintenanceGoingOn())
            {
                return Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, "ServiceUnavailable");
            }
            #endregion

            #region Initiate SFTP Values
            _MonnitIoTReadingImportWrapper.RetrieveInterfacePropValues(ApiConstants.MonnitIoTReadingImport);
            #endregion

            #region Interface Setup Properties Check 
            if (!_MonnitIoTReadingImportWrapper.CheckIsActiveInterface(ApiConstants.MonnitIoTReadingImport))
            {
                return Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, "The process is not enabled for the client.");
            }
            #endregion

            #region Retrieve Interface Prop values for SFTP File Download
            var sftpDirPath = SFTPCred.ftpFileDirectory;
            var sftpArchiveDirPath = SFTPCred.ftpArcDirectory;
            string result = string.Empty;
            string errCsv = string.Empty;
            var fileList = _MonnitIoTReadingImportWrapper.GetFileNames(sftpDirPath);
            #endregion

            #region Read files fron SFTP
            MonnitIoTReadingImportResponseModel monnitIoTReadingResponseModel;
            List<MonnitIoTReadingImportResponseModel> monnitIoTReadingResponseModelList = new List<MonnitIoTReadingImportResponseModel>();
            if (fileList != null && fileList.Count == 0)
            {
                monnitIoTReadingResponseModel = new MonnitIoTReadingImportResponseModel();
                monnitIoTReadingResponseModel.status = StatusEnum.failed.ToString();
                monnitIoTReadingResponseModel.errMessage = "No file found at source directory.";
                monnitIoTReadingResponseModelList.Add(monnitIoTReadingResponseModel);
                var respFile = new HttpResponseMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(monnitIoTReadingResponseModelList),
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
                    monnitIoTReadingResponseModel = new MonnitIoTReadingImportResponseModel();
                    monnitIoTReadingResponseModel.fileName = fileName;
                    monnitIoTReadingResponseModel.status = StatusEnum.failed.ToString();
                    monnitIoTReadingResponseModel.errMessage = "File type " + thisExtension + " is not supported.";
                    monnitIoTReadingResponseModelList.Add(monnitIoTReadingResponseModel);
                }
                else
                {
                    result = _MonnitIoTReadingImportWrapper.GetJsonFromSFTPFile(sftpDirPath, fileName, FileTypeEnum.MonnitIoTReadingImport);
                    if (result.Equals(StatusEnum.ErrorConvertingJson.ToString()))
                    {
                        monnitIoTReadingResponseModel = new MonnitIoTReadingImportResponseModel();
                        monnitIoTReadingResponseModel.fileName = fileName;
                        monnitIoTReadingResponseModel.status = StatusEnum.failed.ToString();
                        monnitIoTReadingResponseModel.errMessage = "File read failure.";
                        monnitIoTReadingResponseModelList.Add(monnitIoTReadingResponseModel);
                        continue;
                    }
                    //--- serialize JSON directly to a file
                    List<MonnitIoTReadingImportJsonModel> aList = new List<MonnitIoTReadingImportJsonModel>();
                    aList = JsonConvert.DeserializeObject<List<MonnitIoTReadingImportJsonModel>>(result.ToString());
                    string firstName = Utility.GetFileName(fileName);
                    string jsonPath = HttpContext.Current.Server.MapPath("~/JsonSchema/MonnitIoTReading/" + firstName + ".json");
                    using (StreamWriter file = File.CreateText(jsonPath))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(file, aList);
                    }
                    //---compare the JSON with schema
                    JsonSchema jschema = new JsonSchema();

                    String schemaPath = HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["MonnitIoTReadingSchemaPath"]);
                    ////---check if schema file exists. If not return relevent response---
                    if (!File.Exists(schemaPath))
                    {
                        monnitIoTReadingResponseModel = new MonnitIoTReadingImportResponseModel();
                        monnitIoTReadingResponseModel.fileName = fileName;
                        monnitIoTReadingResponseModel.status = StatusEnum.failed.ToString();
                        monnitIoTReadingResponseModel.errMessage = "The refence schema requred for json validation is not found.";
                        monnitIoTReadingResponseModelList.Add(monnitIoTReadingResponseModel);
                        var respFile = new HttpResponseMessage
                        {
                            Content = new StringContent(JsonConvert.SerializeObject(monnitIoTReadingResponseModelList),
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
                            monnitIoTReadingResponseModel = new MonnitIoTReadingImportResponseModel();
                            monnitIoTReadingResponseModel.fileName = fileName;
                            monnitIoTReadingResponseModel.status = StatusEnum.failed.ToString();
                            monnitIoTReadingResponseModel.errMessage = "The json file does not match required schema.";
                            monnitIoTReadingResponseModelList.Add(monnitIoTReadingResponseModel);
                            var respJson = new HttpResponseMessage
                            {
                                Content = new StringContent(JsonConvert.SerializeObject(monnitIoTReadingResponseModelList),
                                              System.Text.Encoding.UTF8, "application/json")
                            };
                            return respJson;
                        }
                    }

                    archiveErr = _MonnitIoTReadingImportWrapper.ArchiveFileWithDBvalue(sftpDirPath, fileName, sftpArchiveDirPath);
                    if (String.IsNullOrEmpty(archiveErr))
                    {
                        monnitIoTReadingResponseModel = new MonnitIoTReadingImportResponseModel();
                        monnitIoTReadingResponseModel.fileName = fileName;
                        monnitIoTReadingResponseModel.status = StatusEnum.success.ToString();
                        monnitIoTReadingResponseModel.errMessage = string.Empty;
                        //---process json
                        var respMessage = ProcessLocalIoTReadingJsonFile();
                        monnitIoTReadingResponseModel.MonnitIoTReadingImportResponseModelList = respMessage;
                        monnitIoTReadingResponseModelList.Add(monnitIoTReadingResponseModel);
                    }
                    else
                    {
                        monnitIoTReadingResponseModel = new MonnitIoTReadingImportResponseModel();
                        monnitIoTReadingResponseModel.fileName = fileName;
                        monnitIoTReadingResponseModel.status = StatusEnum.failed.ToString();
                        monnitIoTReadingResponseModel.errMessage = archiveErr;
                        monnitIoTReadingResponseModelList.Add(monnitIoTReadingResponseModel);
                    }
                }
            }
            #region Send Mail
            if (SFTPCred.isMailSend == true)
            {
                _MonnitIoTReadingImportWrapper.SendEMail(credentials.UserName, (JsonConvert.SerializeObject(monnitIoTReadingResponseModelList)));
            }

            #endregion
            var resp = new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(monnitIoTReadingResponseModelList),
                                             System.Text.Encoding.UTF8, "application/json")
            };
            return resp;
            #endregion
        }



        private List<MonnitIoTReadingImportResponseModel> ProcessLocalIoTReadingJsonFile()
        {
            String FilePath = HttpContext.Current.Server.MapPath("~/JsonSchema/MonnitIoTReading/");

            string[] file = Directory.GetFiles(FilePath, "*.json");
            List<MonnitIoTReadingImportResponseModel> vmrList = new List<MonnitIoTReadingImportResponseModel>();
            MonnitIoTReadingImportResponseModel ioTReadingImportResponseModel = new MonnitIoTReadingImportResponseModel();
            using (StreamReader r = new StreamReader(file[0]))
            {
                string json = r.ReadToEnd();
                var ioTReadingImportModel = JsonConvert.DeserializeObject<List<MonnitIoTReadingImportJsonModel>>(json);

                if (ioTReadingImportModel == null)
                {
                    ioTReadingImportResponseModel.status = StatusEnum.failed.ToString();
                    ioTReadingImportResponseModel.errMessage = "Data is null.";
                    vmrList.Add(ioTReadingImportResponseModel);
                    return (vmrList);
                }
                #region ioTReadingimportmodel objects converted to data tables
                var result = _MonnitIoTReadingImportWrapper.CreateIoTReadingImport(ioTReadingImportModel);
                #endregion

                #region create log
                var logid = _MonnitIoTReadingImportWrapper.CreateLog(ApiConstants.MonnitIoTReadingImport);
                #endregion

                #region validate validation & process(import)
                var vResult = _MonnitIoTReadingImportWrapper.IoTReadingImportValidate(logid);
                #endregion

                if (vResult != null && vResult.Count > 0)
                {
                    ioTReadingImportResponseModel.status = StatusEnum.failed.ToString();
                    foreach (var vr in vResult)
                    {
                        ioTReadingImportResponseModel.errMessageList.Add(vr);
                    }
                }
                else
                {
                    ioTReadingImportResponseModel.status = StatusEnum.success.ToString();
                    ioTReadingImportResponseModel.errMessage = "";
                }
                vmrList.Add(ioTReadingImportResponseModel);
            }
            //delete all json files from local directory
            _MonnitIoTReadingImportWrapper.DeleteJsonFiles("MonnitIoTReading");
            return vmrList;
        }
    }
}