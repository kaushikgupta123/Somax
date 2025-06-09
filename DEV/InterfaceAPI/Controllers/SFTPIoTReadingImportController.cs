using System;
using System.Collections.Generic;
using System.Linq;
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
using InterfaceAPI.Models.IoTReading;
using InterfaceAPI.BusinessWrapper.Implementation;

namespace InterfaceAPI.Controllers
{
    public class SFTPIoTReadingImportController : BaseApiController
    {
        readonly IImportCSVWrapper _ImportCSVWrapper;
        readonly IIoTReadingImportWrapper _IoTReadingImportWrapper;
        public SFTPIoTReadingImportController(IImportCSVWrapper ImportCSVWrapper, IIoTReadingImportWrapper IoTReadingImportWrapper)
        {
            _ImportCSVWrapper = ImportCSVWrapper;
            _IoTReadingImportWrapper = IoTReadingImportWrapper;
        }
        public long logID { get; set; }
        [HttpPost]
        public HttpResponseMessage IoTReadingImport([FromBody] SchedulerAPICredentials credentials)
        {
            #region Authentication
            if (!_IoTReadingImportWrapper.CheckAuthentication(credentials.UserName, credentials.Password))
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Authentication Failed");
            }
            if (_IoTReadingImportWrapper.IsMaintenanceGoingOn())
            {
                return Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, "ServiceUnavailable");
            }
            #endregion

            #region Initiate SFTP Values
            _IoTReadingImportWrapper.RetrieveInterfacePropValues(ApiConstants.SFTPIoTReadingImport);
            #endregion

            #region Interface Setup Properties Check 
            if (!_IoTReadingImportWrapper.CheckIsActiveInterface(ApiConstants.SFTPIoTReadingImport))
            {
                return Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, "The process is not enabled for the client.");
            }
            #endregion

            #region Retrieve Interface Prop values for SFTP File Download
            var sftpDirPath = SFTPCred.ftpFileDirectory;
            var sftpArchiveDirPath = SFTPCred.ftpArcDirectory;
            string result = string.Empty;
            string errCsv = string.Empty;
            var fileList = _IoTReadingImportWrapper.GetFileNames(sftpDirPath);
            #endregion

            #region Read files fron SFTP
            IoTReadingImportResponseModel ioTReadingCsvResponseModel;
            List<IoTReadingImportResponseModel> ioTReadingCsvResponseModelList = new List<IoTReadingImportResponseModel>();
            if (fileList != null && fileList.Count == 0)
            {
                ioTReadingCsvResponseModel = new IoTReadingImportResponseModel();
                ioTReadingCsvResponseModel.status = StatusEnum.failed.ToString();
                ioTReadingCsvResponseModel.errMessage = "No file found at source directory.";
                ioTReadingCsvResponseModelList.Add(ioTReadingCsvResponseModel);
                var respFile = new HttpResponseMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(ioTReadingCsvResponseModelList),
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
                    ioTReadingCsvResponseModel = new IoTReadingImportResponseModel();
                    ioTReadingCsvResponseModel.fileName = fileName;
                    ioTReadingCsvResponseModel.status = StatusEnum.failed.ToString();
                    ioTReadingCsvResponseModel.errMessage = "File type " + thisExtension + " is not supported.";
                    ioTReadingCsvResponseModelList.Add(ioTReadingCsvResponseModel);
                }
                else
                {
                    result = _IoTReadingImportWrapper.GetJsonFromSFTPFile(sftpDirPath, fileName, FileTypeEnum.SFTPIoTReadingImport);
                    if (result.Equals(StatusEnum.ErrorConvertingJson.ToString()))
                    {
                        ioTReadingCsvResponseModel = new IoTReadingImportResponseModel();
                        ioTReadingCsvResponseModel.fileName = fileName;
                        ioTReadingCsvResponseModel.status = StatusEnum.failed.ToString();
                        ioTReadingCsvResponseModel.errMessage = "File read failure.";
                        ioTReadingCsvResponseModelList.Add(ioTReadingCsvResponseModel);
                        continue;
                    }
                    //--- serialize JSON directly to a file
                    List<IoTReadingImportJsonModel> aList = new List<IoTReadingImportJsonModel>();
                    aList = JsonConvert.DeserializeObject<List<IoTReadingImportJsonModel>>(result.ToString());
                    string firstName = Utility.GetFileName(fileName);
                    string jsonPath = HttpContext.Current.Server.MapPath("~/JsonSchema/IoTReading/" + firstName + ".json");
                    using (StreamWriter file = File.CreateText(jsonPath))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(file, aList);
                    }
                    //---compare the JSON with schema
                    JsonSchema jschema = new JsonSchema();

                    String schemaPath = HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["IoTReadingSchemaPath"]);
                    ////---check if schema file exists. If not return relevent response---
                    if (!File.Exists(schemaPath))
                    {
                        ioTReadingCsvResponseModel = new IoTReadingImportResponseModel();
                        ioTReadingCsvResponseModel.fileName = fileName;
                        ioTReadingCsvResponseModel.status = StatusEnum.failed.ToString();
                        ioTReadingCsvResponseModel.errMessage = "The refence schema requred for json validation is not found.";
                        ioTReadingCsvResponseModelList.Add(ioTReadingCsvResponseModel);
                        var respFile = new HttpResponseMessage
                        {
                            Content = new StringContent(JsonConvert.SerializeObject(ioTReadingCsvResponseModelList),
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
                            ioTReadingCsvResponseModel = new IoTReadingImportResponseModel();
                            ioTReadingCsvResponseModel.fileName = fileName;
                            ioTReadingCsvResponseModel.status = StatusEnum.failed.ToString();
                            ioTReadingCsvResponseModel.errMessage = "The json file does not match required schema.";
                            ioTReadingCsvResponseModelList.Add(ioTReadingCsvResponseModel);
                            var respJson = new HttpResponseMessage
                            {
                                Content = new StringContent(JsonConvert.SerializeObject(ioTReadingCsvResponseModelList),
                                              System.Text.Encoding.UTF8, "application/json")
                            };
                            return respJson;
                        }
                    }

                    archiveErr = _IoTReadingImportWrapper.ArchiveFileWithDBvalue(sftpDirPath, fileName, sftpArchiveDirPath);
                    if (String.IsNullOrEmpty(archiveErr))
                    {
                        ioTReadingCsvResponseModel = new IoTReadingImportResponseModel();
                        ioTReadingCsvResponseModel.fileName = fileName;
                        ioTReadingCsvResponseModel.status = StatusEnum.success.ToString();
                        ioTReadingCsvResponseModel.errMessage = string.Empty;
                        //---process json
                        var respMessage = ProcessLocalIoTReadingJsonFile();
                        ioTReadingCsvResponseModel.IoTReadingImportResponseModelList = respMessage;
                        ioTReadingCsvResponseModelList.Add(ioTReadingCsvResponseModel);
                    }
                    else
                    {
                        ioTReadingCsvResponseModel = new IoTReadingImportResponseModel();
                        ioTReadingCsvResponseModel.fileName = fileName;
                        ioTReadingCsvResponseModel.status = StatusEnum.failed.ToString();
                        ioTReadingCsvResponseModel.errMessage = archiveErr;
                        ioTReadingCsvResponseModelList.Add(ioTReadingCsvResponseModel);
                    }
                }
            }
            #region Send Mail
            if (SFTPCred.isMailSend == true)
            {
                _IoTReadingImportWrapper.SendEMail(credentials.UserName, (JsonConvert.SerializeObject(ioTReadingCsvResponseModelList)));
            }

            #endregion
            var resp = new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(ioTReadingCsvResponseModelList),
                                             System.Text.Encoding.UTF8, "application/json")
            };
            return resp;
            #endregion
        }



        private List<IoTReadingImportResponseModel> ProcessLocalIoTReadingJsonFile()
        {
            String FilePath = HttpContext.Current.Server.MapPath("~/JsonSchema/IoTReading/");

            string[] file = Directory.GetFiles(FilePath, "*.json");
            List<IoTReadingImportResponseModel> vmrList = new List<IoTReadingImportResponseModel>();
            IoTReadingImportResponseModel ioTReadingImportResponseModel = new IoTReadingImportResponseModel();
            using (StreamReader r = new StreamReader(file[0]))
            {
                string json = r.ReadToEnd();
                var ioTReadingImportModel = JsonConvert.DeserializeObject<List<IoTReadingImportJsonModel>>(json);

                if (ioTReadingImportModel == null)
                {
                    ioTReadingImportResponseModel.status = StatusEnum.failed.ToString();
                    ioTReadingImportResponseModel.errMessage = "Data is null.";
                    vmrList.Add(ioTReadingImportResponseModel);
                    return (vmrList);
                }
                #region ioTReadingimportmodel objects converted to data tables
                var result = _IoTReadingImportWrapper.CreateIoTReadingImport(ioTReadingImportModel);
                #endregion

                #region create log
                var logid = _IoTReadingImportWrapper.CreateLog(ApiConstants.SFTPIoTReadingImport);
                #endregion

                #region validate validation & process(import)
                var vResult = _IoTReadingImportWrapper.IoTReadingImportValidate(logid);
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
            _IoTReadingImportWrapper.DeleteJsonFiles("IoTReading");
            return vmrList;
        }
    }
}