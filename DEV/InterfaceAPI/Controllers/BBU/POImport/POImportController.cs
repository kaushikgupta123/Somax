using Common.Constants;
using InterfaceAPI.ActionFilters;
using InterfaceAPI.BusinessWrapper.Implementation;
using InterfaceAPI.BusinessWrapper.Implementation.BBU;
using InterfaceAPI.BusinessWrapper.Interface;
using InterfaceAPI.BusinessWrapper.Interface.BBU;
using InterfaceAPI.CommonUtility;
using InterfaceAPI.Models.BBU.POImport;
using InterfaceAPI.Models.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using RazorEngine;
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

namespace InterfaceAPI.Controllers.BBU.POImport
{
    //[EnableCors("*", "*", "*")]
    //[CustomAuthorize]

    public class OraclePOImportController : BaseApiController
    {       
        readonly BusinessWrapper.Interface.BBU.IPOImportWrapper _POImportWrapper;
        public long logID { get; set; }
        public static string jsonPath { get; set; }
        public static string encryptedPath { get; set; }
        public static string decryptedPath { get; set; }
        public OraclePOImportController(IImportCSVWrapper ImportCSVWrapper, BusinessWrapper.Interface.BBU.IPOImportWrapper poImportWrapper)
        {           
            _POImportWrapper = poImportWrapper;
        }
        [HttpPost]
        [Route("api/OraclePOImportDelim/")]
        public HttpResponseMessage OraclePOImport([FromBody]SchedulerAPICredentials credentials)
        {
            List<POImportResponseModel> POImportResponseModelList = new List<POImportResponseModel>();
            #region Authentication
            ClaimsPrincipal currentClaimsPrincipal = ClaimsPrincipal.Current;
            if (!_POImportWrapper.CheckAuthentication(credentials.UserName, credentials.Password))
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Authentication Failed");
            }
            if (_POImportWrapper.IsMaintenanceGoingOn())
            {
                return Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, "ServiceUnavailable");
            }
            #endregion
            #region Interface Setup Properties Check 
            if (!_POImportWrapper.CheckIsActiveInterface(ApiConstants.OraclePOImport))
            {
                return Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, "The process is not enabled for the client.");
            }
            #endregion
            #region Initiate SFTP Values
            _POImportWrapper.RetrieveInterfacePropValues(ApiConstants.OraclePOImport);
            #endregion

            #region Test Encrypt and UploadFile to SFTP
            //_POImportWrapper.EncryptandUploadFile("04_Sample Date_Purchase Order Interface.txt", ApiConstants.OraclePOImport);//---comment out before publish
            #endregion
            /*
            #region Test Email of PO
            // 2022-May-12 - Email a PO 
            bool test_of_Send_PO;
            test_of_Send_PO = false;
            if (test_of_Send_PO)
            {
              // PO with 16 Line Items
              long poid = 490642;
              string ponumber = "G076200117687";
              // PO with 7 line items
              //long poid = 490006;
              //string ponumber = "G076200116049";
              // Grainger PO
              //long poid = 490142;
              //string ponumber = "G076200116410";
              // Kaman 
              //long poid = 490006;
              //string ponumber = "G076200116049";
              string emailaddress = "roger@somax.com"; //"ggshmzqukrfh@in.docparser.com"
              Dictionary<long, Tuple<string, string>> email_po = new Dictionary<long, Tuple<string, string>>();
              email_po.Add(poid, Tuple.Create(ponumber, emailaddress));
              _POImportWrapper.SetEmailArray(poid,ponumber,emailaddress);
              _POImportWrapper.SendBBUEmails();
              return Request.CreateResponse(HttpStatusCode.OK, "Email of PO is Successful.");
            }
            #endregion
            */
            #region Retrieve Interface Prop values for SFTP File Download
            var sftpDirPath = SFTPCred.ftpFileDirectory;
            var sftpArchiveDirPath = SFTPCred.ftpArcDirectory;
            string result = string.Empty;
            string errCsv = string.Empty;
            var fileList = _POImportWrapper.GetFileList(sftpDirPath);

            #endregion


            #region Read files fron SFTP
            POImportResponseModel POImportResponseModel;
            //List<POImportResponseModel> POImportResponseModelList = new List<POImportResponseModel>();
            if (fileList != null && fileList.Count == 0)
            {
                POImportResponseModel = new POImportResponseModel();
                POImportResponseModel.status = StatusEnum.failed.ToString();
                POImportResponseModel.errMessage = "No file found at source directory.";
                POImportResponseModelList.Add(POImportResponseModel);
                var respFile = new HttpResponseMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(POImportResponseModelList),
                                            System.Text.Encoding.UTF8, "application/json")
                };
                return respFile;
            }

            //---Get json from csv---
            // Sort the file list - later

            foreach (var fileName in fileList)
            {
                string dcFileName;
                encryptedPath = _POImportWrapper.DownloadToLocal(fileName, ApiConstants.OraclePOImport);
                // RKL - 2021-Aug-13
                if (SFTPCred.filesEncrypted)
                {
                  decryptedPath = _POImportWrapper.DecryptFile(fileName, encryptedPath, ApiConstants.OraclePOImport);
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
                    POImportResponseModel = new POImportResponseModel();
                    POImportResponseModel.fileName = dcFileName;
                    POImportResponseModel.status = StatusEnum.failed.ToString();
                    POImportResponseModel.errMessage = "File type " + thisExtension + " is not supported.";
                    POImportResponseModelList.Add(POImportResponseModel);
                }
                else
                {
                    //var ReflectionProperty = typeof(POImportJSONModel).GetProperties().ToList();
                    //string[] PropertyNames = ReflectionProperty.Select(x => x.Name).ToArray();
                    //result = _POImportWrapper.GetJsonFromDownloadedSFTPFiles(decryptedPath, dcFileName, FileTypeEnum.OraclePOImport,PropertyNames);
                    result = _POImportWrapper.GetJsonFromDownloadedSFTPFiles(decryptedPath, fileName, FileTypeEnum.OraclePOImport);
                    if (result.Equals(StatusEnum.ErrorConvertingJson.ToString()))
                    {
                        POImportResponseModel = new POImportResponseModel();
                        POImportResponseModel.fileName = dcFileName;
                        POImportResponseModel.status = StatusEnum.failed.ToString();
                        POImportResponseModel.errMessage = "File read failure.";
                        POImportResponseModelList.Add(POImportResponseModel);
                        continue;
                    }
                    //else if (result.Equals(StatusEnum.ColumnMismatch.ToString()))
                    //{
                    //    POImportResponseModel = new POImportResponseModel();
                    //    POImportResponseModel.fileName = dcFileName;
                    //    POImportResponseModel.status = StatusEnum.failed.ToString();
                    //    POImportResponseModel.errMessage = "File is expected to be containing these columns - " +
                    //      String.Join(", ", PropertyNames) + " .";
                    //    POImportResponseModelList.Add(POImportResponseModel);
                    //    continue;
                    //}
                    //--- serialize JSON directly to a file
                    List<POImportJSONModel> aList = new List<POImportJSONModel>();
                    aList = JsonConvert.DeserializeObject<List<POImportJSONModel>>(result.ToString());
                    string firstName = Utility.GetFileName(fileName);
                    jsonPath = HttpContext.Current.Server.MapPath("~/JsonSchema/BBU/POImport/" + firstName + ".json");//Create Json Schema
                    using (StreamWriter file = File.CreateText(jsonPath))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(file, aList);
                    }
                    //---compare the JSON with schema
                    JsonSchema jschema = new JsonSchema();

                    String schemaPath = HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["OraclePOImportSchemaPath"]);// Create entry in webconfig
                    ////---check if schema file exists. If not return relevent response---
                    if (!File.Exists(schemaPath))
                    {
                        POImportResponseModel = new POImportResponseModel();
                        POImportResponseModel.fileName = fileName;
                        POImportResponseModel.status = StatusEnum.failed.ToString();
                        POImportResponseModel.errMessage = "The refence schema requred for json validation is not found.";
                        POImportResponseModelList.Add(POImportResponseModel);
                        var respFile = new HttpResponseMessage
                        {
                            Content = new StringContent(JsonConvert.SerializeObject(POImportResponseModelList),
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
                            POImportResponseModel = new POImportResponseModel();
                            POImportResponseModel.fileName = fileName;
                            POImportResponseModel.status = StatusEnum.failed.ToString();
                            POImportResponseModel.errMessage = "The json file does not match required schema.";
                            POImportResponseModelList.Add(POImportResponseModel);
                            var respJson = new HttpResponseMessage
                            {
                                Content = new StringContent(JsonConvert.SerializeObject(POImportResponseModelList),
                                              System.Text.Encoding.UTF8, "application/json")
                            };
                            return respJson;
                        }
                    }

                    archiveErr = _POImportWrapper.ArchiveFileWithDBValue(sftpDirPath, fileName, sftpArchiveDirPath);
                    if (String.IsNullOrEmpty(archiveErr))
                    {
                        POImportResponseModel = new POImportResponseModel();
                        POImportResponseModel.fileName = fileName;
                        POImportResponseModel.status = StatusEnum.success.ToString();
                        POImportResponseModel.errMessage = string.Empty;
                        //---process json
                        var respMessage = ProcessLocalPOImportJsonFile();
                        POImportResponseModel.poImportResponseModelList = respMessage;
                        POImportResponseModelList.Add(POImportResponseModel);


                    }
                    else
                    {
                        POImportResponseModel = new POImportResponseModel();
                        POImportResponseModel.fileName = fileName;
                        POImportResponseModel.status = StatusEnum.failed.ToString();
                        POImportResponseModel.errMessage = archiveErr;
                        POImportResponseModelList.Add(POImportResponseModel);
                    }
                }
            }
            #region Send Mail
            if (SFTPCred.isMailSend == true)
            {
               // _POImportWrapper.SendEMail(credentials.UserName, (JsonConvert.SerializeObject(POImportResponseModelList)));
                _POImportWrapper.SendAlert();       // Send alerts
                _POImportWrapper.SendBBUEmails();   // Send emails to vendors with attachment
                _POImportWrapper.GenerateEvents();  // Generate Events V2-612
            }

            #endregion

            var resp = new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(POImportResponseModelList),
                                               System.Text.Encoding.UTF8, "application/json")
            };
            return resp;
        }
        #endregion
        private List<POImportResponseModel> ProcessLocalPOImportJsonFile()
        {
            String FilePath = HttpContext.Current.Server.MapPath("~/JsonSchema/BBU/POImport/");

            string[] file = Directory.GetFiles(FilePath, "*.json");
            List<POImportResponseModel> vmrList = new List<POImportResponseModel>();
            POImportResponseModel POImportResponseModel = new POImportResponseModel();
            using (StreamReader r = new StreamReader(file[0]))
            {
                string json = r.ReadToEnd();
                List<POImportJSONModel> POImportModelList = JsonConvert.DeserializeObject<List<POImportJSONModel>>(json);

                if (POImportModelList == null)
                {
                    POImportResponseModel.status = StatusEnum.failed.ToString();
                    POImportResponseModel.errMessage = "Data is null.";
                    vmrList.Add(POImportResponseModel);
                    return (vmrList);
                }
                #region POimportmodel objects converted to data tables
                var result = _POImportWrapper.CreatePOImport(POImportModelList);
                #endregion

                #region create & update log
                var logid = _POImportWrapper.CreateLog(ApiConstants.OraclePOImport);

                #endregion

                #region process(import)
                var vResult = _POImportWrapper.ProcessPOImport();
                #endregion

                #region UpdateLog
                if (logid != 0)
                {
                    _POImportWrapper.UpdateLog(logid, result.TotalProcess, result.SuccessfulProcess, result.FailedProcess, result.logMessage, Common.Constants.ApiConstants.OraclePOImport);                   
                }
                #endregion               

                if (vResult.FailedProcess != null && vResult.FailedProcess > 0)
                {
                    POImportResponseModel.status = StatusEnum.failed.ToString();
                    POImportResponseModel.errMessage = vResult.logMessage;
                }
                else
                {
                    POImportResponseModel.status = StatusEnum.success.ToString();
                    POImportResponseModel.errMessage = vResult.logMessage;
                }
                vmrList.Add(POImportResponseModel);
            }
            //delete all files from local directory
            _POImportWrapper.DeleteLocalFiles(Path.GetDirectoryName(jsonPath));
            _POImportWrapper.DeleteLocalFiles(Path.GetDirectoryName(encryptedPath));
            _POImportWrapper.DeleteLocalFiles(Path.GetDirectoryName(decryptedPath));


            return vmrList;
        }

      
       
    }
    
}