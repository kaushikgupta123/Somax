using InterfaceAPI.BusinessWrapper.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InterfaceAPI.Models;
using InterfaceAPI.Models.Common;
using DataContracts;
using InterfaceAPI.Models.BBU.PartMasterImport;
using InterfaceAPI.BusinessWrapper.Interface.BBU;
using Common.Constants;

namespace InterfaceAPI.BusinessWrapper.Implementation.BBU
{
    public class PartMasterImportWrapper : CommonWrapper, IPartMasterImportWrapper
    {
        public PartMasterImportWrapper(UserData userData) : base(userData)
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
        public ProcessLogModel CreatePartMasterImport(List<PartMasterImportJSONModel> PartMasterImportModelList)
        {
            int TotalProcess = 0;
            int SuccessfulProcess = 0;
            int FailedProcess = 0;
            int NewTransactions = 0;
            string logMessage = string.Empty;
            PartMasterImport _partMasterImport;
            PartMasterImportModel pmi = new PartMasterImportModel();
            TotalProcess = PartMasterImportModelList.Count;
            ProcessLogModel objProcessLogModel = new ProcessLogModel();
            try
            {
                Site site = new Site();
                site.ClientId = _userData.DatabaseKey.Client.ClientId;
                List<DataContracts.Site> listsite = site.RetrieveAll(_userData.DatabaseKey);
                string mastersite = System.Configuration.ConfigurationManager.AppSettings["PartMasterPlantId"].ToString().Trim();
                foreach (var jItem in PartMasterImportModelList)
                {
                    _partMasterImport = new PartMasterImport();
                    //--Retrieve header
                    pmi.ClientId = _userData.DatabaseKey.Client.ClientId;
                    pmi.EXPartId = Convert.ToInt64(jItem.ORACLE_PART_ID);
                    pmi.ClientLookupId = jItem.ORACLE_PART_NUMBER;
                    if (jItem.ORACLE_PLANT_ID.ToString() == mastersite)
                        pmi.SiteId = 0;
                    else
                    {
                        Site ThisSite = listsite.FirstOrDefault(x => x.ExOrganizationId == Convert.ToString(jItem.ORACLE_PLANT_ID));
                        if (ThisSite == null)
                        {
                            pmi.SiteId = 0;
                        }
                        else
                        {
                            pmi.SiteId = ThisSite.SiteId;
                        }
                    }
                    pmi.UnitCost = jItem.PRICE == "" ? 0 : Convert.ToDecimal(jItem.PRICE);
                    // Guid May not get a guid
                    Guid uniqueid;
                    if (Guid.TryParse(jItem.UUID, out uniqueid))
                        pmi.ExUniqueId = uniqueid;
                    else
                        pmi.ExUniqueId = Guid.Empty;
                    // Short Description
                    if (string.IsNullOrEmpty(jItem.SHORT_DESCRIPTION))
                        pmi.ShortDescription = string.Empty;
                    else
                        pmi.ShortDescription = jItem.SHORT_DESCRIPTION;
                    // Long Description
                    if (string.IsNullOrEmpty(jItem.LONG_DESCRIPTION))
                        pmi.LongDescription = string.Empty;
                    else
                        pmi.LongDescription = jItem.LONG_DESCRIPTION;
                    // Manufacturer 
                    if (string.IsNullOrEmpty(jItem.MANUFACTURER))
                        pmi.Manufacturer = string.Empty;
                    else
                        pmi.Manufacturer = jItem.MANUFACTURER;
                    // Manufacturer ID
                    if (string.IsNullOrEmpty(jItem.MFG_PART_NUMBER))
                        pmi.ManufacturerId = string.Empty;
                    else
                        pmi.ManufacturerId = jItem.MFG_PART_NUMBER;
                    // Alternate Part Number 1
                    if (string.IsNullOrEmpty(jItem.ATL_PART_NUMBER_1))
                        pmi.ExAltPartId1 = string.Empty;
                    else
                        pmi.ExAltPartId1 = jItem.ATL_PART_NUMBER_1;
                    // Alternate Part Number 2
                    if (string.IsNullOrEmpty(jItem.ATL_PART_NUMBER_2))
                        pmi.ExAltPartId2 = string.Empty;
                    else
                        pmi.ExAltPartId2 = jItem.ATL_PART_NUMBER_2;
                    // Alternate Part Number 3
                    if (string.IsNullOrEmpty(jItem.ATL_PART_NUMBER_3))
                        pmi.ExAltPartId3 = string.Empty;
                    else
                        pmi.ExAltPartId3 = jItem.ATL_PART_NUMBER_3;
                    // Primary Unit of Measure
                    if (string.IsNullOrEmpty(jItem.PRIMARY_UNIT_OF_MEASURE))
                        pmi.UnitOfMeasure = string.Empty;
                    else
                        pmi.UnitOfMeasure = jItem.PRIMARY_UNIT_OF_MEASURE;
                    // Unit of Measure Description
                    if (string.IsNullOrEmpty(jItem.UNIT_OF_MEASURE_DESCRIPTION))
                        pmi.UnitOfMeasureDesc = string.Empty;
                    else
                        pmi.UnitOfMeasureDesc = jItem.UNIT_OF_MEASURE_DESCRIPTION;
                    // UPC Code
                    if (string.IsNullOrEmpty(jItem.UPC))
                        pmi.UPCCode = string.Empty;
                    else
                        pmi.UPCCode = jItem.UPC;
                    // Category 
                    if (string.IsNullOrEmpty(jItem.UNSPSC_CODE_ID))
                        pmi.Category = string.Empty;
                    else
                        pmi.Category = jItem.UNSPSC_CODE_ID;
                    // Category Description
                    if (string.IsNullOrEmpty(jItem.UNSPSC_DESCRIPTION))
                        pmi.CategoryDesc = string.Empty;
                    else
                        pmi.CategoryDesc = jItem.UNSPSC_DESCRIPTION;
                    // Image URL
                    if (string.IsNullOrEmpty(jItem.IMAGE_URL))
                        pmi.ImageURL = string.Empty;
                    else
                        pmi.ImageURL = jItem.IMAGE_URL;
                    // Enabled Flag
                    if (string.IsNullOrEmpty(jItem.ENABLED_FLAG))
                        pmi.Enabled = string.Empty;
                    else
                        pmi.Enabled = jItem.ENABLED_FLAG;

                    // If an invalid site - skip for now
                    if (pmi.SiteId >= 0)
                    {
                        // Changed concepts here
                        // Do the error checking in the checking in the Create_PartMasterProcessInterface SP
                        //Pass the PartMasterImportModel values 
                        _partMasterImport = SavePartMasterImport(pmi);
                        ++NewTransactions;
                        if (_partMasterImport.ErrorMessage.Count() > 0)
                        {
                            return objProcessLogModel;
                        }
                    }

                    if (_partMasterImport.ErrorMessages == null || _partMasterImport.ErrorMessages.Count == 0)
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
        private PartMasterImport SavePartMasterImport(PartMasterImportModel pmi)
        {
            PartMasterImport partMasterImport = new PartMasterImport();
            string errMsg = "";
            Boolean valid = true;
            if (valid)
            {
                //Row Validation Check
                //string check = checkRowLevelValidation(pmi);
                partMasterImport.ClientId = _userData.DatabaseKey.Client.ClientId;
                partMasterImport.SiteId = pmi.SiteId;
                partMasterImport.ClientLookupId = pmi.ClientLookupId;
                partMasterImport.EXPartId = pmi.EXPartId;
                partMasterImport.UnitCost = pmi.UnitCost;
                partMasterImport.ExUniqueId = pmi.ExUniqueId;
                partMasterImport.ShortDescription = pmi.ShortDescription;
                partMasterImport.LongDescription = pmi.LongDescription;
                partMasterImport.Manufacturer = pmi.Manufacturer;
                partMasterImport.ManufacturerId = pmi.ManufacturerId;
                partMasterImport.ExAltPartId1 = pmi.ExAltPartId1;
                partMasterImport.ExAltPartId2 = pmi.ExAltPartId2;
                partMasterImport.ExAltPartId3 = pmi.ExAltPartId3;
                partMasterImport.UnitOfMeasure = pmi.UnitOfMeasure;
                partMasterImport.UnitOfMeasureDesc = pmi.UnitOfMeasureDesc;
                partMasterImport.UPCCode = pmi.UPCCode;
                partMasterImport.Category = pmi.Category;
                partMasterImport.CategoryDesc = pmi.CategoryDesc;
                partMasterImport.ImageURL = pmi.ImageURL;
                partMasterImport.Enabled = pmi.Enabled;
                partMasterImport.ErrorMessage = string.Empty; //check
                partMasterImport.LastProcessed = System.DateTime.UtcNow;

                // Check if PartMasterImport record exists 
                // Match on ClientId, SiteId and ClientLookupId (External Part Number)
                PartMasterImport PartMasterImport = new PartMasterImport();
                PartMasterImport.ClientId = pmi.ClientId;
                PartMasterImport.SiteId = pmi.SiteId;
                PartMasterImport._ClientLookupID = pmi.ClientLookupId;
                PartMasterImport.RetrieveClientLookupID(_userData.DatabaseKey);
                partMasterImport.PartMasterImportId = PartMasterImport.PartMasterImportId;
                partMasterImport.UpdateIndex = PartMasterImport.UpdateIndex;

                try
                {
                    // Update if there is record for clientid, siteid and clientlookupid - ignore error message
                    // Update only that rows of partMasterImport table Where Error Message is found by ClientLookupId             
                    if (PartMasterImport.ClientLookupId.Trim() == pmi.ClientLookupId.Trim())
                    {
                        partMasterImport.Update(_userData.DatabaseKey);
                    }
                    else
                    {
                        //First Time entry in partMasterImport table, may be any error found  
                        partMasterImport.Create(_userData.DatabaseKey);
                    }
                }
                catch (Exception ex)
                {
                    errMsg = ex.ToString();
                    partMasterImport.ErrorMessage = errMsg;
                }
            }
            return partMasterImport;
        }
        #endregion

        #region Process Part Master Import
        public ProcessLogModel ProcessPartMasterImport()
        {
            ProcessLogModel objProcessLogModel = new ProcessLogModel();
            PartMasterImport PartMasterImport = new PartMasterImport();
            List<PartMasterImport> PartmasterImlist = PartMasterImport.PartMasterImportRetrieveAll(_userData.DatabaseKey);
            int TotalProcess = PartmasterImlist.Count;
            int SucessfulTransaction = 0;
            int FailTransaction = 0;
            foreach (var item in PartmasterImlist)
            {
                PartMasterImport PartMasterImportinterface = new PartMasterImport();
                PartMasterImportinterface.CallerUserInfoId = _userData.DatabaseKey.Client.CallerUserInfoId;
                PartMasterImportinterface.CallerUserName = _userData.DatabaseKey.Client.CallerUserName;
                PartMasterImportinterface.ClientId = _userData.DatabaseKey.Client.ClientId;
                PartMasterImportinterface.PartMasterImportId = item.PartMasterImportId;
                try
                {
                    //There is no Store Procedure Validation. So, we put it in try catch block
                    PartMasterImportinterface.Create_PartMasterProcessInterface(_userData.DatabaseKey);
                    // Delete is handled by the SP
                    if (PartMasterImportinterface.error_message_count == 0)
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