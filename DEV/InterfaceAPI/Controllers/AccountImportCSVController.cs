using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InterfaceAPI.BusinessWrapper.Interface;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Web.Http;
using System.Security.Claims;
using Common.Constants;
using InterfaceAPI.Models.Account;
using InterfaceAPI.Models.Common;
using InterfaceAPI.CommonUtility;

namespace InterfaceAPI.Controllers
{
    public class AccountImportCSVController : BaseApiController
    {
        readonly IImportCSVWrapper _ImportCSVWrapper;
        readonly IAccountImportWrapper _AccountImportWrapper;

        public AccountImportCSVController(IImportCSVWrapper ImportCSVWrapper, IAccountImportWrapper AccountImportWrapper)
        {
            _ImportCSVWrapper = ImportCSVWrapper;
            _AccountImportWrapper = AccountImportWrapper;
        }
        public long logID { get; set; }

        public HttpResponseMessage Post(SchedulerAPICredentials credentials)
        {
            #region Authentication
            ClaimsPrincipal currentClaimsPrincipal = ClaimsPrincipal.Current;
            if (!_AccountImportWrapper.CheckAuthentication(credentials.UserName, credentials.Password))
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Authentication Failed");
            }
            if (_AccountImportWrapper.IsMaintenanceGoingOn())
            {
                return Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, "ServiceUnavailable");
            }
            #endregion

            #region Interface Setup Properties Check 
            if (!_AccountImportWrapper.CheckIsActiveInterface(ApiConstants.AccountImport))
            {
                return Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, "The process is not enabled for the client.");
            }
            #endregion

            //---read csv  from FTP
            var sftpDirPath = System.Configuration.ConfigurationManager.AppSettings["AccountActiveDir"];
            var sftpArchiveDirPath = System.Configuration.ConfigurationManager.AppSettings["AccountArchiveDir"];
            string result = string.Empty;
            string errCsv = string.Empty;
            var fileList = _AccountImportWrapper.GetFileNames(sftpDirPath);
            AccountCsvResponseModel accountCsvResponseModel;
            List<AccountCsvResponseModel> AccountCsvResponseModelList = new List<AccountCsvResponseModel>();
            if (fileList != null && fileList.Count == 0)
            {
                accountCsvResponseModel = new AccountCsvResponseModel();
                accountCsvResponseModel.status = StatusEnum.failed.ToString();
                accountCsvResponseModel.errMessage = "No file found at source directory.";
                AccountCsvResponseModelList.Add(accountCsvResponseModel);
                var respFile = new HttpResponseMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(AccountCsvResponseModelList),
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
                    accountCsvResponseModel = new AccountCsvResponseModel();
                    accountCsvResponseModel.fileName = fileName;
                    accountCsvResponseModel.status = StatusEnum.failed.ToString();
                    accountCsvResponseModel.errMessage = "File type " + thisExtension + " is not supported.";
                    AccountCsvResponseModelList.Add(accountCsvResponseModel);
                }
                else
                {
                    result = _AccountImportWrapper.GetJsonFromSFTPFile(sftpDirPath, fileName,FileTypeEnum.Account);
                    if (result.Equals(StatusEnum.ErrorConvertingJson.ToString()))
                    {
                        accountCsvResponseModel = new AccountCsvResponseModel();
                        accountCsvResponseModel.fileName = fileName;
                        accountCsvResponseModel.status = StatusEnum.failed.ToString();
                        accountCsvResponseModel.errMessage = "File read failure.";
                        AccountCsvResponseModelList.Add(accountCsvResponseModel);
                    }
                    //--- serialize JSON directly to a file
                    AccountImportModel accountImport = new AccountImportModel();
                    List<AccountImportModel> aList = new List<AccountImportModel>();
                    aList = JsonConvert.DeserializeObject<List<AccountImportModel>>(result.ToString());
                    string firstName = Utility.GetFileName(fileName);
                    string jsonPath = HttpContext.Current.Server.MapPath("~/JsonSchema/Account/" + firstName + ".json");
                    using (StreamWriter file = File.CreateText(jsonPath))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(file, aList);
                    }
                    //---compare the JSON with schema
                    JsonSchema jschema = new JsonSchema();

                    String schemaPath = HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["AccountSchemaPath"]);
                    ////---check if schema file exists. If not return relevent response---
                    if (!File.Exists(schemaPath))
                    {
                        accountCsvResponseModel = new AccountCsvResponseModel();
                        accountCsvResponseModel.fileName = fileName;
                        accountCsvResponseModel.status = StatusEnum.failed.ToString();
                        accountCsvResponseModel.errMessage = "The refence schema requred for json validation is not found.";
                        AccountCsvResponseModelList.Add(accountCsvResponseModel);
                        var respFile = new HttpResponseMessage
                        {
                            Content = new StringContent(JsonConvert.SerializeObject(AccountCsvResponseModelList),
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
                        archiveErr = _AccountImportWrapper.ArchiveFile(sftpDirPath, fileName, sftpArchiveDirPath);
                        if (String.IsNullOrEmpty(archiveErr))
                        {
                            accountCsvResponseModel = new AccountCsvResponseModel();
                            accountCsvResponseModel.fileName = fileName;
                            accountCsvResponseModel.status = StatusEnum.success.ToString();
                            accountCsvResponseModel.errMessage = string.Empty;
                            //---process json
                            var respMessage = ProcessLocalAccountJsonFile();
                            accountCsvResponseModel.AccountImportResponseModelList = respMessage;
                            AccountCsvResponseModelList.Add(accountCsvResponseModel);
                        }
                        else
                        {
                            accountCsvResponseModel = new AccountCsvResponseModel();
                            accountCsvResponseModel.fileName = fileName;
                            accountCsvResponseModel.status = StatusEnum.failed.ToString();
                            accountCsvResponseModel.errMessage = archiveErr;
                            AccountCsvResponseModelList.Add(accountCsvResponseModel);
                        }
                    }
                    else
                    {
                        accountCsvResponseModel = new AccountCsvResponseModel();
                        accountCsvResponseModel.fileName = fileName;
                        accountCsvResponseModel.status = StatusEnum.failed.ToString();
                        accountCsvResponseModel.errMessage = "The json file does not match required schema.";
                        AccountCsvResponseModelList.Add(accountCsvResponseModel);
                    }
                }
            }
            var resp = new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(AccountCsvResponseModelList),
                                             System.Text.Encoding.UTF8, "application/json")
            };
            return resp;
        }



        private List<AccountImportResponseModel> ProcessLocalAccountJsonFile()
        {
            String FilePath = HttpContext.Current.Server.MapPath("~/JsonSchema/Account/");

            string[] file = Directory.GetFiles(FilePath, "*.json");
            List<AccountImportResponseModel> vmrList = new List<AccountImportResponseModel>();
            AccountImportResponseModel accountImportResponseModel = new AccountImportResponseModel();
            using (StreamReader r = new StreamReader(file[0]))
            {
                string json = r.ReadToEnd();
                var accountImportModel = JsonConvert.DeserializeObject<List<AccountImportModel>>(json);

                if (accountImportModel == null)
                {
                    accountImportResponseModel.status = StatusEnum.failed.ToString();
                    accountImportResponseModel.errMessage = "Data is null.";
                    vmrList.Add(accountImportResponseModel);
                    return (vmrList);
                }
                #region accountimportmodel objects converted to data tables
                var result = _AccountImportWrapper.CreateAccountImport(accountImportModel);
                #endregion

                #region create & update log
                var logid = _AccountImportWrapper.CreateLog("accountimport");
                if (logid != 0)
                {
                    _AccountImportWrapper.UpdateLog(logid, result.TotalProcess, result.SuccessfulProcess, result.FailedProcess, result.logMessage, Common.Constants.ApiConstants.DeanFoodsAccountImport);
                }
                #endregion

                #region validate validation & process(import)
                var vResult = _AccountImportWrapper.AccountImportValidate(logid, accountImportModel);
                #endregion

                if (vResult != null && vResult.Count > 0)
                {
                    accountImportResponseModel.status = StatusEnum.failed.ToString();
                    foreach (var vr in vResult)
                    {
                        accountImportResponseModel.errMessageList.Add(vr);
                    }
                }
                else
                {
                    accountImportResponseModel.status = StatusEnum.success.ToString();
                }
                vmrList.Add(accountImportResponseModel);
            }
            //delete all json files from local directory
            _AccountImportWrapper.DeleteJsonFiles("account");
            return vmrList;
        }
    }
}