using DataContracts;

using InterfaceAPI.BusinessWrapper.Interface.BBU;
using InterfaceAPI.Models.BBU.PartCategoryMasterImport;
using InterfaceAPI.Models.BBU.PartMasterImport;
using InterfaceAPI.Models.Common;

using System;
using System.Collections.Generic;
using System.Linq;

namespace InterfaceAPI.BusinessWrapper.Implementation.BBU
{
    public class PartCategoryMasterImportWrapper : CommonWrapper, IPartCategoryMasterImportWrapper
    {
        public PartCategoryMasterImportWrapper(UserData userData) : base(userData)
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
        public ProcessLogModel CreatePartCategoryMasterImport(List<PartCategoryMasterImportJSONModel> PartCategoryMasterImportModelList)
        {
            int TotalProcess = 0;
            int SuccessfulProcess = 0;
            int FailedProcess = 0;
            int NewTransactions = 0;
            string logMessage = string.Empty;
            PartCategoryMasterImport _partCategoryMasterImport;
            PartCategoryMasterImportModel pmi = new PartCategoryMasterImportModel();
            TotalProcess = PartCategoryMasterImportModelList.Count;
            ProcessLogModel objProcessLogModel = new ProcessLogModel();
            try
            {
                //Site site = new Site();
                //site.ClientId = _userData.DatabaseKey.Client.ClientId;
                //List<DataContracts.Site> listsite = site.RetrieveAll(_userData.DatabaseKey);
                //string mastersite = System.Configuration.ConfigurationManager.AppSettings["PartMasterPlantId"].ToString().Trim();
                foreach (var jItem in PartCategoryMasterImportModelList)
                {
                    _partCategoryMasterImport = new PartCategoryMasterImport();
                    //--Retrieve header
                    pmi.ClientId = _userData.DatabaseKey.Client.ClientId;
                    pmi.ClientLookupId = jItem.CATEGORY_CODE ?? "";
                    pmi.Description = jItem.DESCRIPTION ?? "";
                    pmi.InactiveFlag = !(jItem.ENABLED_FLAG == "Y" ? true : false);                    

                    // If an invalid site - skip for now
                    if (!string.IsNullOrEmpty(pmi.ClientLookupId))
                    {
                        _partCategoryMasterImport = SavePartCategoryMasterImport(pmi);
                        ++NewTransactions;
                        if (_partCategoryMasterImport.ErrorMessage.Count() > 0)
                        {
                            return objProcessLogModel;
                        }
                    }

                    if (_partCategoryMasterImport.ErrorMessages == null || _partCategoryMasterImport.ErrorMessages.Count == 0)
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
        private PartCategoryMasterImport SavePartCategoryMasterImport(PartCategoryMasterImportModel pmi)
        {
            PartCategoryMasterImport partCategoryMasterImport = new PartCategoryMasterImport();
            string errMsg = "";
            Boolean valid = true;
            if (valid)
            {
                //Row Validation Check
                //string check = checkRowLevelValidation(pmi);
                partCategoryMasterImport.ClientId = _userData.DatabaseKey.Client.ClientId;
                partCategoryMasterImport.ClientLookupId = pmi.ClientLookupId;
                partCategoryMasterImport.Description = pmi.Description;
                partCategoryMasterImport.InactiveFlag = pmi.InactiveFlag;
                partCategoryMasterImport.ErrorMessage = string.Empty; //check
                partCategoryMasterImport.LastProcess = System.DateTime.UtcNow;

                // Check if PartMasterImport record exists 
                // Match on ClientId, SiteId and ClientLookupId (External Part Number)
                PartCategoryMasterImport PartCategoryMasterImport = new PartCategoryMasterImport();
                PartCategoryMasterImport.ClientId = pmi.ClientId;
                PartCategoryMasterImport.ClientLookupId = pmi.ClientLookupId;
                PartCategoryMasterImport.RetrieveClientLookupID(_userData.DatabaseKey);

                try
                {
                    // Update if there is record for clientid, siteid and clientlookupid - ignore error message
                    // Update only that rows of partMasterImport table Where Error Message is found by ClientLookupId             
                    if (PartCategoryMasterImport.PartCategoryMasterImportId > 0)
                    {
                        partCategoryMasterImport.PartCategoryMasterImportId = PartCategoryMasterImport.PartCategoryMasterImportId;
                        partCategoryMasterImport.Update(_userData.DatabaseKey);
                    }
                    else
                    {
                        //First Time entry in partCategoryMasterImport table, may be any error found  
                        partCategoryMasterImport.Create(_userData.DatabaseKey);
                    }
                }
                catch (Exception ex)
                {
                    errMsg = ex.ToString();
                    partCategoryMasterImport.ErrorMessage = errMsg;
                }
            }
            return partCategoryMasterImport;
        }
        #endregion

        #region Process Part Category Master Import
        public ProcessLogModel ProcessPartCategoryMasterImport()
        {
            ProcessLogModel objProcessLogModel = new ProcessLogModel();
            PartCategoryMasterImport PartCategoryMasterImport = new PartCategoryMasterImport();
            List<PartCategoryMasterImport> PartCategorymasterImlist = PartCategoryMasterImport.PartCategoryMasterImportRetrieveAll(_userData.DatabaseKey);
            int TotalProcess = PartCategorymasterImlist.Count;
            int SucessfulTransaction = 0;
            int FailTransaction = 0;
            foreach (var item in PartCategorymasterImlist)
            {
                PartCategoryMasterImport PartCategoryMasterImportinterface = new PartCategoryMasterImport();
                PartCategoryMasterImportinterface.CallerUserInfoId = _userData.DatabaseKey.Client.CallerUserInfoId;
                PartCategoryMasterImportinterface.CallerUserName = _userData.DatabaseKey.Client.CallerUserName;
                PartCategoryMasterImportinterface.ClientId = _userData.DatabaseKey.Client.ClientId;
                PartCategoryMasterImportinterface.PartCategoryMasterImportId = item.PartCategoryMasterImportId;
                try
                {
                    //There is no Store Procedure Validation. So, we put it in try catch block
                    PartCategoryMasterImportinterface.Create_PartCategoryMasterProcessInterface(_userData.DatabaseKey);
                    // Delete is handled by the SP
                    if (PartCategoryMasterImportinterface.error_message_count == 0) 
                    {
                        SucessfulTransaction++;
                        // Delete is actually handled in the stored procedure
                        //PartMasterImportinterface.Delete(UserData.DatabaseKey);
                    }
                    else
                    {
                        FailTransaction++;
                    }
                    objProcessLogModel.SuccessfulProcess = SucessfulTransaction;
                    objProcessLogModel.FailedProcess = FailTransaction;
                }
                catch (Exception ex)
                {
                    objProcessLogModel.logMessage = ex.Message.ToString();
                }
            }
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