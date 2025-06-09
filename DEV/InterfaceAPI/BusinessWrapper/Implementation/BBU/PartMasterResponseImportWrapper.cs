using InterfaceAPI.BusinessWrapper.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InterfaceAPI.Models;
using InterfaceAPI.Models.Common;
using DataContracts;
using InterfaceAPI.Models.BBU.PartMasterResponseImport;
using InterfaceAPI.BusinessWrapper.Interface.BBU;
using Common.Constants;
using Common.Enumerations;

namespace InterfaceAPI.BusinessWrapper.Implementation.BBU
{
    public class PartMasterResponseImportWrapper : CommonWrapper, IPartMasterResponseImportWrapper
    {
        public PartMasterResponseImportWrapper(UserData userData) : base(userData)
        {
            _userData = userData;
            _dbKey = userData.DatabaseKey;
        }


        #region Test Upload File
        public void EncryptandUploadFile(string fileName, string apiName)
        {
            EncryptandUploadFileToSFTP(fileName, apiName);
        }
        #endregion

        #region Download files in Local
        public string DownloadToLocal(string fileName, string apiName)
        {
            string filePath = string.Empty;
            filePath = DownloadFileToLocalDirectory(fileName, apiName);
            return filePath;
        }
        #endregion

        #region Decrypt File
        public string DecryptFile(string fileName, string encryptFilePath, string apiName)
        {
            return DecryptLocalFile(fileName, encryptFilePath, apiName);
        }
        #endregion

        #region Archive File
        public string ArchiveFileWithDBValue(string SourceFilePath, string sourceFileName, string DestinationPath)
        {
            return ArchiveFileWithDBvalue(SourceFilePath, sourceFileName, DestinationPath);
        }
        #endregion

        #region Create and Save in table
        public ProcessLogModel CreatePartMasterResponseImport(List<PartMasterResponseImportJSONModel> partMasterResponseImportModelList, long logID)
        {
            int TotalProcess = 0;
            int SuccessfulProcess = 0;
            int FailedProcess = 0;
            int NewTransactions = 0;
            string logMessage = string.Empty;
            PartMasterResponse _PartMasterResponseImport;
            PartMasterResponseImportModel pmi = new PartMasterResponseImportModel();
            TotalProcess = partMasterResponseImportModelList.Count;
            ProcessLogModel objProcessLogModel = new ProcessLogModel();
            try
            {               
                foreach (var jItem in partMasterResponseImportModelList)
                {
                    pmi.ClientId = _userData.DatabaseKey.Client.ClientId;
                    pmi.PartMasterResponseId = 0;
                    pmi.PartMasterRequestId = jItem.ECO_REQUEST_NUM == "" ? 0 : Convert.ToInt64(jItem.ECO_REQUEST_NUM);
                    pmi.RequestType = jItem.REQUEST_TYPE;
                    pmi.EXPartNumber = jItem.ORACLE_PART_NUM;
                    pmi.EXPartId = jItem.ORACLE_PART_ID == "" ? 0 : Convert.ToInt64(jItem.ORACLE_PART_ID);
                    pmi.Status = jItem.STATUS;
                    pmi.ErrorMessage = "";
                    pmi.LastProcessed = null;
                    pmi.ImportLogId = logID;
                    pmi.CreateBy = _userData.DatabaseKey.UserName;
                    pmi.CreateDate = DateTime.UtcNow;
                    _PartMasterResponseImport = SavePartMasterResponseImport(pmi);
                    ++NewTransactions;
                    if (_PartMasterResponseImport.ErrorMessage.Count() > 0)
                    {
                        return objProcessLogModel;
                    }                  

                    if (_PartMasterResponseImport.ErrorMessages == null || _PartMasterResponseImport.ErrorMessages.Count == 0)
                    {
                        SuccessfulProcess++;
                    }
                    else
                    {
                        FailedProcess++;
                    }
                }//end of foreach
            }
            catch (Exception ex)
            {

            }
            if (FailedProcess == 0)
            {
                logMessage = "Json data inserted into temporary table.";
            }
            else
            {
                logMessage = "Conversion of json data to temporary data tables failed.";
            }
            objProcessLogModel.NewProcess = NewTransactions;
            objProcessLogModel.TotalProcess = TotalProcess;
            objProcessLogModel.SuccessfulProcess = SuccessfulProcess;
            objProcessLogModel.FailedProcess = FailedProcess;
            objProcessLogModel.logMessage = logMessage;
            return objProcessLogModel;
        }
        private PartMasterResponse SavePartMasterResponseImport(PartMasterResponseImportModel pmi)
        {
            PartMasterResponse partMasterResponse = new PartMasterResponse();
            string errMsg = "";
            Boolean valid = true;
            if (valid)
            {
                //Row Validation Check
                //string check = checkRowLevelValidation(pmi);
                string error_message = "";
                DataContracts.PartMasterResponse res = new DataContracts.PartMasterResponse();
                res.ClientId = pmi.ClientId;
                res.PartMasterRequestId = pmi.PartMasterRequestId;
                res.RequestType = pmi.RequestType;
                res.EXPartNumber = pmi.EXPartNumber;
                res.EXPartId = pmi.EXPartId;
                res.Status = pmi.Status;
                res.ErrorMessage = pmi.ErrorMessage;
                res.ImportLogId = pmi.ImportLogId;
                try
                {                                
                    if (res.ImportLogId >0)
                    {
                        res.Create(_userData.DatabaseKey);
                    }
                    else
                    {
                        partMasterResponse.ErrorMessage = errMsg;
                    }
                }
                catch (Exception ex)
                {
                    errMsg = ex.ToString();
                    partMasterResponse.ErrorMessage = errMsg;
                }
            }
            return partMasterResponse;
        }
        #endregion

        #region Process Part Master Response Import
        public ProcessLogModel ProcessPartMasterResponseImport()
        {
            ProcessLogModel objProcessLogModel = new ProcessLogModel();
            List<PartMasterResponse> pmr_list = new List<PartMasterResponse>();
            PartMasterResponse pmr = new PartMasterResponse();
            int TotalProcess = pmr_list.Count;
            int SucessfulTransaction = 0;
            int FailTransaction = 0;
            // Get A list of the repsonse items           
            pmr.ClientId = _userData.DatabaseKey.Client.ClientId;
            pmr_list = pmr.PartMasterResponseRetrieveAll(_userData.DatabaseKey);
            TotalProcess = pmr_list.Count;
            foreach (var pmr_item in pmr_list)
            {
                PartMasterResponse res = new PartMasterResponse();
                res.ClientId = pmr_item.ClientId;
                res.PartMasterResponseId = pmr_item.PartMasterResponseId;
                res.PerformById = _userData.DatabaseKey.Personnel.PersonnelId;
                switch (pmr_item.RequestType.ToLower())
                {
                    case "addition":
                        res.TransactionType = Common.Constants.PartHistoryTranTypes.Activate;
                        break;
                    case "replacement":
                        res.TransactionType = Common.Constants.PartHistoryTranTypes.IDChange;
                        break;
                    case "inactivation":
                        res.TransactionType = Common.Constants.PartHistoryTranTypes.Inactivate;
                        break;
                    case "sx_replacement":
                        res.TransactionType = Common.Constants.PartHistoryTranTypes.MasterReplace;
                        break;
                    case "eco_new":
                        res.TransactionType = Common.Constants.PartHistoryTranTypes.Activate;
                        break;
                    case "eco_replacement":
                        res.TransactionType = Common.Constants.PartHistoryTranTypes.ECO_REPLACE;
                        break;
                    case "eco_sx_replace":
                    case "eco_sx_replacement": 
                        res.TransactionType = Common.Constants.PartHistoryTranTypes.ECO_SX_REPLACE;
                        break;
                }
                try
                {
                    res.ImportData(_userData.DatabaseKey);
                    if (!res.ImportError)
                    {
                        // SOM-16?? 
                        // Need to check for success or failure
                        // Re-Read from DB and check if still there 
                        ++SucessfulTransaction;
                        // Send Notifications
                        // Send complete notification to creator
                        ProcessAlert objAlert = new ProcessAlert(this._userData);
                        List<long> alertlist = new List<long>();
                        alertlist.Add(pmr_item.PartMasterRequestId);
                        AlertTypeEnum atype;
                        switch (pmr_item.RequestType.ToLower())
                        {
                            case "addition":
                                atype = AlertTypeEnum.PartMasterRequestAdditionProcessed;   // Changed
                                break;
                            case "replacement":
                                atype = AlertTypeEnum.PartMasterRequestReplaceProcessed;   // Changed
                                break;
                            case "inactivation":
                                atype = AlertTypeEnum.PartMasterRequestInactivationProcessed;   // Changed
                                break;
                            case "sx_replacement":
                                atype = AlertTypeEnum.PartMasterRequestSXReplaceProcessed;   // Changed
                                break;
                            case "eco_new":
                                atype = AlertTypeEnum.PartMasterRequestProcessed;
                                break;
                            case "eco_replace":
                                atype = AlertTypeEnum.PartMasterRequestECO_ReplaceProcessed;
                                break;
                            case "eco_sx_replace":
                                atype = AlertTypeEnum.PartMasterRequestECO_SX_ReplaceProcessed;
                                break;
                            default:
                                atype = AlertTypeEnum.PartMasterRequestProcessed;
                                break;
                        }
                        objAlert.CreateAlert<PartMasterRequest>(atype, alertlist);
                    }
                    else
                    {
                        ++FailTransaction;
                    }
                    
                }
                catch (Exception ex)
                {
                    ++FailTransaction;
                }
            }
            objProcessLogModel.SuccessfulProcess = SucessfulTransaction;
            objProcessLogModel.FailedProcess = FailTransaction;
           
            if (FailTransaction == 0)
            {
                objProcessLogModel.logMessage = "Process Complete – All items processed successfully";
            }
            else
            {
                objProcessLogModel.logMessage = "Process Complete – Not all items were processed successfully";
            }

            return objProcessLogModel;
        }
        #endregion

    }

}