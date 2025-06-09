using InterfaceAPI.BusinessWrapper.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InterfaceAPI.Models;
using InterfaceAPI.Models.Common;
using DataContracts;
using InterfaceAPI.Models.BBU.POReceiptImport;
using InterfaceAPI.BusinessWrapper.Interface.BBU;
using Common.Constants;
using System.Globalization;
using System.Text;

namespace InterfaceAPI.BusinessWrapper.Implementation.BBU
{
    public class POReceiptImportWrapper : CommonWrapper, IPOReceiptImportWrapper
    {
        public POReceiptImportWrapper(UserData userData) : base(userData)
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
        public ProcessLogModel CreatePOReceiptImport(List<POReceiptImportJSONModel> poReceiptImportModelList)
        {
            int TotalProcess = 0;
            int SuccessfulProcess = 0;
            int FailedProcess = 0;
            int NewTransactions = 0;
            string logMessage = string.Empty;
            POReceiptImport2 _POReceiptImport2;
            POReceiptImportModel poR = new POReceiptImportModel();
            TotalProcess = poReceiptImportModelList.Count;
            ProcessLogModel objProcessLogModel = new ProcessLogModel();
            try
            {
                Site site = new Site();
                site.ClientId = _userData.DatabaseKey.Client.ClientId;
                List<DataContracts.Site> listsite = site.RetrieveAll(_userData.DatabaseKey);
                foreach (var jItem in poReceiptImportModelList)
                {
                    _POReceiptImport2 = new POReceiptImport2();
                    poR.ClientId = _userData.DatabaseKey.Client.ClientId;
                    Site ThisSite = listsite.FirstOrDefault(x => x.ExOrganizationId == Convert.ToString(jItem.SHIP_TO_LOCATION_ID));
                    if (ThisSite == null)
                    {
                        poR.SiteId = -1;
                    }
                    else
                    {
                        poR.SiteId = ThisSite.SiteId;
                    }
                    poR.ExReceipt = jItem.RECEIPT_NUMBER;
                    poR.ExReceiptId = jItem.ORACLE_RECEIPT_ID == 0 ? 0 : Convert.ToInt64(jItem.ORACLE_RECEIPT_ID);
                    poR.ExReceiptTxn = Convert.ToInt64(jItem.TRANSACTION_ID);
                    poR.ExVendorId = jItem.ORACLE_VENDOR_ID == "" ? 0 : Convert.ToInt64(jItem.ORACLE_VENDOR_ID);
                    poR.ExVendor = jItem.ORACLE_VENDOR_NUMBER;
                    // RKL - 2017-Sep-17 - dates are in the format dd-mmm-yy
                    string dateformat = "dd-MMM-yy";
                    //string dateformat = "MM/dd/yyyy HH:mm:ss";
                    IFormatProvider culture = CultureInfo.InvariantCulture;
                    //IFormatProvider culture = new CultureInfo("en-US", true);
                    poR.ReceiptDate = jItem.RECEIPT_DATE == "" ? DateTime.MaxValue : DateTime.ParseExact(jItem.RECEIPT_DATE, dateformat, culture);
                    poR.TransactionDate = jItem.TRANSACTION_DATE == "" ? DateTime.MaxValue : DateTime.ParseExact(jItem.TRANSACTION_DATE, dateformat, culture);
                    poR.ExPurchaseOrderId = jItem.PURCHASE_ORDER_ID == "" ? 0 : Convert.ToInt64(jItem.PURCHASE_ORDER_ID);
                    poR.ExPurchaseOrder = jItem.PURCHASE_ORDER_NUM;
                    poR.ExPurchaseOrderLineId = jItem.PURCHASE_ORDER_LINE_ID == "" ? 0 : Convert.ToInt64(jItem.PURCHASE_ORDER_LINE_ID);
                    poR.POLineNumber = jItem.PURCHASE_ORDER_LINE_NUM == "" ? 0 : Convert.ToInt32(jItem.PURCHASE_ORDER_LINE_NUM);
                    poR.ExPartId = jItem.ORACLE_PARTID == "" ? 0 : Convert.ToInt64(jItem.ORACLE_PARTID);
                    poR.ExPart = jItem.ORACLE_PART_NUM;
                    poR.Description = jItem.ITEM_DESCRIPTION;
                    poR.ReceiptQuantity = jItem.RECEIVED_QUANTITY == "" ? 0 : Convert.ToDecimal(jItem.RECEIVED_QUANTITY);
                    poR.PurchaseUOM = jItem.RECEIVED_UNIT_OF_MEASURE;
                    poR.UnitOfMeasure = jItem.PRIMARY_UNIT_OF_MEASURE;
                    poR.UOMConversion = jItem.UOM_CONVERSION_RATE == "" ? 0 : Convert.ToDecimal(jItem.UOM_CONVERSION_RATE);
                    poR.Reason = jItem.REASON;
                    // If an invalid site - skip for now
                    if (poR.SiteId >= 0)
                    {
                        // Changed concepts here
                        // Do the error checking in the checking in the Create_PartMasterProcessInterface SP
                        //Pass the POImportModel values 
                        _POReceiptImport2 = SavePOReceiptImport(poR);
                        ++NewTransactions;
                        if (_POReceiptImport2.ErrorMessage.Count() > 0)
                        {
                            return objProcessLogModel;
                        }
                    }

                    if (_POReceiptImport2.ErrorMessages == null || _POReceiptImport2.ErrorMessages.Count == 0)
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
        private POReceiptImport2 SavePOReceiptImport(POReceiptImportModel poR)
        {
            POReceiptImport2 poRImport = new POReceiptImport2();
            string errMsg = "";
            Boolean valid = true;
            if (valid)
            {
                poRImport.ClientId = _userData.DatabaseKey.Client.ClientId;
                poRImport.SiteId = poR.SiteId;
                poRImport.ExReceiptId = poR.ExReceiptId;
                poRImport.ExReceipt = poR.ExReceipt;
                poRImport.ExVendorId = poR.ExVendorId;
                poRImport.ExReceiptTxnId = poR.ExReceiptTxn;
                poRImport.ExVendor = poR.ExVendor;
                poRImport.ReceiptDate = poR.ReceiptDate;
                poRImport.TransactionDate = poR.TransactionDate;
                poRImport.ExPurchaseOrderId = poR.ExPurchaseOrderId;
                poRImport.ExPurchaseOrder = poR.ExPurchaseOrder;
                poRImport.ExPurchaseOrderLineId = poR.ExPurchaseOrderLineId;
                poRImport.POLineNumber = poR.POLineNumber;
                poRImport.ExPart = poR.ExPart;
                poRImport.ExPartId = poR.ExPartId;
                poRImport.Description = poR.Description;
                poRImport.ReceiptQuantity = poR.ReceiptQuantity;
                poRImport.PurchaseUOM = poR.PurchaseUOM;
                poRImport.UnitOfMeasure = poR.UnitOfMeasure;
                poRImport.UOMConversion = poR.UOMConversion;
                poRImport.Reason = poR.Reason;

                // Determine if the item already exists 
                // Use Oracle Receipt Id and Oracle Transcation Id 
                POReceiptImport2 pOReceiptImport2 = new POReceiptImport2();
                pOReceiptImport2.ClientId = _userData.DatabaseKey.Client.ClientId;
                pOReceiptImport2.ExReceiptId = poR.ExReceiptId;
                pOReceiptImport2.ExReceiptTxnId = poR.ExReceiptTxn;
                pOReceiptImport2.RetrieveExReceiptId(_userData.DatabaseKey);

                try
                {           
                    if (pOReceiptImport2.POReceiptImport2Id>0)
                    {
                        poRImport.POReceiptImport2Id = pOReceiptImport2.POReceiptImport2Id;
                        poRImport.UpdateIndex = pOReceiptImport2.UpdateIndex;
                        poRImport.Update(_userData.DatabaseKey);
                    }
                    else
                    {
                        poRImport.Create(_userData.DatabaseKey);
                    }
                }
                catch (Exception ex)
                {
                    errMsg = ex.ToString();
                    poRImport.ErrorMessage = errMsg;
                }
            }
            return poRImport;
        }
        #endregion

        #region Process Part Master Import
        public ProcessLogModel ProcessPOReceiptImport()
        {
            ProcessLogModel objProcessLogModel = new ProcessLogModel();
            POReceiptImport2 POReceiptImport2 = new POReceiptImport2();
            List<DataContracts.POReceiptImport2> POReceiptImport2Imlist = POReceiptImport2.POReceiptImportRetrieveAll(_userData.DatabaseKey);
            int TotalProcess = POReceiptImport2Imlist.Count;
            int SucessfulTransaction = 0;
            int FailTransaction = 0;
            foreach (var item in POReceiptImport2Imlist)
            {
                POReceiptImport2 POReceiptImport2interface = new POReceiptImport2();
                POReceiptImport2interface.CallerUserInfoId = _userData.DatabaseKey.Client.CallerUserInfoId;
                POReceiptImport2interface.CallerUserName = _userData.DatabaseKey.Client.CallerUserName;
                POReceiptImport2interface.ClientId = _userData.DatabaseKey.Client.ClientId;
                POReceiptImport2interface.POReceiptImport2Id = item.POReceiptImport2Id;
                POReceiptImport2interface.ValidatePOReceiptImport(_userData.DatabaseKey);
                try
                {
                    
                    // Delete is handled by the SP
                    if (POReceiptImport2interface.ErrorMessages.Count== 0)
                    {
                        POReceiptImport2interface.ProcessInterfacePOReceiptImport(_userData.DatabaseKey);
                        POReceiptImport2interface.Delete(_userData.DatabaseKey);
                        SucessfulTransaction++;
                        // Delete is actually handled in the stored procedure
                        //PartMasterImportinterface.Delete(UserData.DatabaseKey);
                    }
                    else
                    {
                        StringBuilder ErrMsg = new StringBuilder();
                        foreach (var err in POReceiptImport2interface.ErrorMessages)
                        {
                            ErrMsg.AppendLine(err.ToString());
                        }
                        POReceiptImport2interface.Retrieve(_userData.DatabaseKey);
                        POReceiptImport2interface.ErrorMessage = ErrMsg.ToString();
                        POReceiptImport2interface.LastProcess = DateTime.Now;
                        POReceiptImport2interface.Update(_userData.DatabaseKey);
                        FailTransaction++;
                    }
                    objProcessLogModel.SuccessfulProcess = SucessfulTransaction;
                    objProcessLogModel.FailedProcess = FailTransaction;
                }
                catch (Exception ex)
                {
                    //if an error comes update PartMasterImport table with ErrorMessage
                    POReceiptImport2interface.POReceiptImport2Id = item.POReceiptImport2Id;
                    POReceiptImport2interface.Retrieve(_userData.DatabaseKey);
                    if (ex.Message.Contains("invalid return code"))
                    {
                        POReceiptImport2interface.Update(_userData.DatabaseKey);
                    }
                    else
                    {
                        POReceiptImport2interface.ErrorMessage = ex.Message.ToString();
                        POReceiptImport2interface.Update(_userData.DatabaseKey);
                    }
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