using InterfaceAPI.BusinessWrapper.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InterfaceAPI.Models;
using InterfaceAPI.Models.Common;
using DataContracts;
using InterfaceAPI.Models.BBU.VendorCatalogImport;
using InterfaceAPI.BusinessWrapper.Interface.BBU;
using Common.Constants;
using System.Globalization;

namespace InterfaceAPI.BusinessWrapper.Implementation.BBU
{
    public class VendorCatalogImportWrapper : CommonWrapper, IVendorCatalogImportWrapper
    {
        public VendorCatalogImportWrapper(UserData userData) : base(userData)
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
        public ProcessLogModel CreateVCImport(List<VendorCatalogImportJSONModel> vcImportModelList)
        {
            int TotalProcess = 0;
            int SuccessfulProcess = 0;
            int FailedProcess = 0;
            int NewTransactions = 0;
            string logMessage = string.Empty;
            VendorCatalogImport _vcImport;
            VendorCatalogImportModel vci = new VendorCatalogImportModel();
            TotalProcess = vcImportModelList.Count;
            ProcessLogModel objProcessLogModel = new ProcessLogModel();
            try
            {
                foreach (var jItem in vcImportModelList)
                {
                    try
                    {
                      vci.ClientId = _userData.DatabaseKey.Client.ClientId;
                      vci.ExVendorId = jItem.ORACLE_VENDOR_ID == "" ? 0 : Convert.ToInt64(jItem.ORACLE_VENDOR_ID);
                      vci.ExVendorNumber = jItem.ORACLE_VENDOR_NUMBER;
                      vci.ExVendorSiteId = jItem.ORACLE_VENDOR_SITE_ID == "" ? 0 : Convert.ToInt64(jItem.ORACLE_VENDOR_SITE_ID);
                      vci.ExVendorSiteCode = jItem.ORACLE_VENDOR_SITE_CODE;
                      vci.ExSourceId = jItem.ORC_SRC_DOC_ID == "" ? 0 : Convert.ToInt64(jItem.ORC_SRC_DOC_ID);
                      vci.ExSourceDocument = jItem.ORC_SRC_DOC_NUM;
                      string dateformat = "MM/dd/yyyy HH:mm:ss";
                      IFormatProvider culture = new CultureInfo("en-US", true);
                      DateTime stdate;
                      if (DateTime.TryParseExact(jItem.EFFECTIVE_START_DATE, dateformat, culture, DateTimeStyles.None,out stdate))
                      {
                        vci.StartDate = stdate;
                      } 
                      else
                      {
                        vci.StartDate = DateTime.MinValue;
                      }
                      DateTime fndate;
                      if (DateTime.TryParseExact(jItem.EFFECTIVE_END_DATE, dateformat, culture, DateTimeStyles.None,out fndate))
                      {
                        vci.EndDate = fndate;
                      } 
                      else
                      {
                        vci.EndDate = DateTime.MinValue;
                      }
                      //vci.StartDate = jItem.EFFECTIVE_START_DATE == "" ? DateTime.MinValue : DateTime.ParseExact(jItem.EFFECTIVE_START_DATE, dateformat, culture);  
                      //vci.EndDate = jItem.EFFECTIVE_END_DATE == "" ? DateTime.MinValue : DateTime.ParseExact(jItem.EFFECTIVE_END_DATE, dateformat, culture);
                      vci.Canceled = jItem.CANCELLED_FLAG;
                      vci.LineNumber = jItem.ORC_SRC_DOC_LINE_NUM == "" ? 0 : Convert.ToInt32(jItem.ORC_SRC_DOC_LINE_NUM);
                      vci.ExLineId = jItem.ORC_SRC_DOC_LINE_ID == "" ? 0 : Convert.ToInt64(jItem.ORC_SRC_DOC_LINE_ID);
                      vci.ExPartId = jItem.ORACLE_PART_ID == "" ? 0 : Convert.ToInt64(jItem.ORACLE_PART_ID);
                      vci.ExPartNumber = jItem.ORACLE_PART_NUMBER;
                      vci.Category = jItem.UNSPC_CODE_ID_TREE;
                      vci.Description = jItem.LINE_DESCRIPTION;
                      vci.PurchaseUOM = jItem.PURCHASING_UOM;
                      vci.UnitCost = jItem.UNIT_PRICE == "" ? 0 : Convert.ToDecimal(jItem.UNIT_PRICE);
                      vci.UnitOfMeasure = jItem.PRIMARY_UOM;
                      vci.UOMConversion = jItem.UOM_CONVERSION == "" ? 0 : Convert.ToDecimal(jItem.UOM_CONVERSION);
                      vci.VendorPartNumber = jItem.VENDOR_PART_NUMBER;
                      vci.LeadTime = jItem.VENDOR_LEAD_TIME == "" ? 0 : Convert.ToInt32(jItem.VENDOR_LEAD_TIME);
                      vci.ExpirationDate = jItem.EXPIRATION_DATE == "" ? DateTime.MinValue : DateTime.ParseExact(jItem.EXPIRATION_DATE, dateformat, culture);
                      try
                      {
                          Int32 ord_qty = Convert.ToInt32(jItem.MINIMUM_ORDER_QUANTITY);
                          vci.MinimumQuantity = ord_qty;
                      }
                      catch
                      {
                          vci.MinimumQuantity = 0;
                      }

                      _vcImport = SaveVendorCatalogImport(vci);
                      ++NewTransactions;
                      if (_vcImport.ErrorMessage.Count() > 0)
                      {
                          return objProcessLogModel;
                      }

                      if (_vcImport.ErrorMessages == null || _vcImport.ErrorMessages.Count == 0)
                      {
                          SuccessfulProcess++;
                      }
                      else
                      {
                          FailedProcess++;
                      }
                  }
                  catch (Exception ex)
                  {
                    // Put this here so if we have one bad entry - we do not skip all the rest
                    string msg = ex.Message;
                    FailedProcess++;
                  }
                }//end of foreach
            }
            catch (Exception ex)
            {
              string test = ex.Message;
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
        private VendorCatalogImport SaveVendorCatalogImport(VendorCatalogImportModel vci)
        {
            VendorCatalogImport vendorCatalogImport = new VendorCatalogImport();
            string errMsg = "";
            Boolean valid = true;
            if (valid)
            {
               
                vendorCatalogImport.ClientId = _userData.DatabaseKey.Client.ClientId;
                vendorCatalogImport.ExVendorId = vci.ExVendorId;
                vendorCatalogImport.ExVendorNumber = vci.ExVendorNumber;
                vendorCatalogImport.ExVendorSiteId = vci.ExVendorSiteId;
                vendorCatalogImport.ExVendorSiteCode = vci.ExVendorSiteCode;
                vendorCatalogImport.ExSourceId = vci.ExSourceId;
                vendorCatalogImport.ExSourceDocument = vci.ExSourceDocument;
                vendorCatalogImport.StartDate = vci.StartDate;
                vendorCatalogImport.EndDate = vci.EndDate;
                vendorCatalogImport.Canceled = vci.Canceled;
                vendorCatalogImport.LineNumber = vci.LineNumber;
                vendorCatalogImport.ExLineId = vci.ExLineId;
                vendorCatalogImport.ExPartId = vci.ExPartId;
                vendorCatalogImport.ExPartNumber = vci.ExPartNumber;
                vendorCatalogImport.Category = vci.Category;
                vendorCatalogImport.Description = vci.Description;
                vendorCatalogImport.PurchaseUOM = vci.PurchaseUOM;
                vendorCatalogImport.UnitCost = vci.UnitCost;
                vendorCatalogImport.UnitOfMeasure = vci.UnitOfMeasure;
                vendorCatalogImport.UOMConversion = vci.UOMConversion;
                vendorCatalogImport.VendorPartNumber = vci.VendorPartNumber;
                vendorCatalogImport.LeadTime = vci.LeadTime;
                vendorCatalogImport.MinimumQuantity = vci.MinimumQuantity;
                vendorCatalogImport.ExpirationDate = vci.ExpirationDate;
                vendorCatalogImport.ErrorMessage = string.Empty;// check;
                vendorCatalogImport.LastProcessed = System.DateTime.UtcNow;

                VendorCatalogImport vci_check = new VendorCatalogImport();
                vci_check.ClientId = vendorCatalogImport.ClientId;
                vci_check.ExVendorId = vendorCatalogImport.ExVendorId;
                vci_check.ExVendorSiteId = vendorCatalogImport.ExVendorSiteId;
                vci_check.ExSourceId = vendorCatalogImport.ExSourceId;
                vci_check.ExLineId = vendorCatalogImport.ExLineId;
                vci_check.RetrieveForImportCheck(_userData.DatabaseKey);              

                try
                {
                    // If the record exists - update
                    if (vci_check.VendorCatalogImportId > 0)
                    {
                        // RKL - 2021-10-20 - If working this way - have to set the vendorcatalogimport item's VendorCatalogImportId
                        vendorCatalogImport.VendorCatalogImportId = vci_check.VendorCatalogImportId;
                        vendorCatalogImport.UpdateIndex = vci_check.UpdateIndex;
                        vendorCatalogImport.Update(_userData.DatabaseKey);
                    }
                    // If the record does not exist - add 
                    else
                    {
                        vendorCatalogImport.Create(_userData.DatabaseKey);
                    }
                }
                catch (Exception ex)
                {
                    errMsg = ex.ToString();
                    vendorCatalogImport.ErrorMessage = errMsg;
                }
            }
            return vendorCatalogImport;
        }
        #endregion

        #region Process Vendor Catalog Import
        public ProcessLogModel ProcessVCImport()
        {
            ProcessLogModel objProcessLogModel = new ProcessLogModel();
            VendorCatalogImport vcImport = new VendorCatalogImport();
            List<VendorCatalogImport> vcImlist = vcImport.VendorCatalogImportRetrieveAll(_userData.DatabaseKey);
            int TotalProcess = vcImlist.Count;
            int SucessfulTransaction = 0;
            int FailTransaction = 0;
            foreach (var item in vcImlist)
            {
                VendorCatalogImport vendorCatalogImportinterface = new VendorCatalogImport();
                vendorCatalogImportinterface.CallerUserInfoId = _userData.DatabaseKey.User.UserInfoId;
                vendorCatalogImportinterface.CallerUserName = _userData.DatabaseKey.UserName;
                vendorCatalogImportinterface.ClientId = _userData.DatabaseKey.Client.ClientId;
                vendorCatalogImportinterface.VendorCatalogImportId = item.VendorCatalogImportId;
                try
                {
                    //There is no Store Procedure Validation. So, we put it in try catch block
                    vendorCatalogImportinterface.Create_VendorCatalogProcessInterface(_userData.DatabaseKey);
                    // Delete is handled by the SP
                    if (vendorCatalogImportinterface.error_message_count == 0)
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