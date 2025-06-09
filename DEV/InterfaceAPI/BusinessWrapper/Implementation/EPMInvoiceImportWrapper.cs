using Common.Constants;
using DataContracts;
using InterfaceAPI.BusinessWrapper.Interface;
using InterfaceAPI.Models.Common;
using InterfaceAPI.Models.EPMInvoiceImport;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace InterfaceAPI.BusinessWrapper.Implementation
{
    public class EPMInvoiceImportWrapper : CommonWrapper, IEPMInvoiceImportWrapper
    {
        public EPMInvoiceImportWrapper(UserData userData) : base(userData)
        {
            _userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        #region Create and Save in table
        public ProcessLogModel CreateEPMInvoiceImport(EPMInvoiceImportModel ePMInvoiceImportModel)
        {
            int TotalProcess = 0;
            int SuccessfulProcess = 0;
            int FailedProcess = 0;
            int NewTransactions = 0;
            string logMessage = string.Empty;
            EPMInvoiceImportModel monnitIoTReadingImportModel = new EPMInvoiceImportModel();
            TotalProcess = 1;
            ProcessLogModel objProcessLogModel = new ProcessLogModel();
            try
            {
                EPMInvImpHdr ePMInvImpHdr = SaveHeaderRecordEPMInv(ePMInvoiceImportModel);
                if (ePMInvImpHdr.ErrorMessage.Count() == 0)
                {
                    NewTransactions = 1;
                    foreach (var item in ePMInvoiceImportModel.Items)
                    {
                        EPMInvImpLine ePMInvImpLine = SaveEMPInvLineItems(ePMInvImpHdr, item);
                        if (ePMInvImpLine.ErrorMessages == null || ePMInvImpLine.ErrorMessages.Count == 0)
                        {
                            SuccessfulProcess++;
                        }
                        else
                        {
                            FailedProcess++;
                        }
                    }

                }
            }
            catch (Exception ex)
            {

                throw;
            }
          
            if (FailedProcess == 0)
            {
                logMessage = ErrorMessageConstants.Json_data_inserted_into_temporary_table;
            }
            else
            {
                logMessage =ErrorMessageConstants.Conversion_json_data_temporary_tables_failed;
            }
            objProcessLogModel.NewProcess = NewTransactions;
            objProcessLogModel.TotalProcess = TotalProcess;
            objProcessLogModel.SuccessfulProcess = SuccessfulProcess;
            objProcessLogModel.FailedProcess = FailedProcess;
            objProcessLogModel.logMessage = logMessage;
            return objProcessLogModel;
        }

        private EPMInvImpLine SaveEMPInvLineItems(EPMInvImpHdr ePMInvImpHdr, Models.EPMInvoiceImport.Item item)
        {
            EPMInvImpLine ePMInvImpLine = new EPMInvImpLine();
            ePMInvImpLine.ClientId = _userData.DatabaseKey.Client.ClientId;
            ePMInvImpLine.EPMInvImpHdrId = ePMInvImpHdr.EPMInvImpHdrId;
            ePMInvImpLine.LineNumber = item.LineNumber;
            ePMInvImpLine.BuyerPartNumber = item.BuyerPartNumber;
            ePMInvImpLine.VendorPartNumber = item.VendorPartNumber;
            ePMInvImpLine.Quantity = item.Quantity;
            ePMInvImpLine.UnitCost = item.UnitPrice;
            ePMInvImpLine.UnitOfMeasure = item.UnitOfMeasurement;
            string errMsg = "";
            Boolean valid = true;
            if (valid)
            {
                try
                {
                    ePMInvImpLine.Create(_userData.DatabaseKey);
                }
                catch (Exception ex)
                {
                    errMsg = ex.ToString();
                    ePMInvImpLine.ErrorMessage = errMsg;
                }
            }
          
            return ePMInvImpLine;
        }

        private EPMInvImpHdr SaveHeaderRecordEPMInv(EPMInvoiceImportModel ePMInvoiceImportModel)
        {
           
            EPMInvImpHdr ePMInvImpHdr = new EPMInvImpHdr();
            ePMInvImpHdr.ClientId = _userData.DatabaseKey.Client.ClientId;
            ePMInvImpHdr.SiteId = _userData.Site.SiteId;
            ePMInvImpHdr.PONumber = ePMInvoiceImportModel.PONumber;
            ePMInvImpHdr.InvoiceNumber = ePMInvoiceImportModel.InvoiceNumber;
            if (!string.IsNullOrEmpty(ePMInvoiceImportModel.PODate))
            {
               
                DateTime ReadingDate = DateTime.ParseExact(ePMInvoiceImportModel.PODate, "yyyyMMdd", CultureInfo.InvariantCulture);

                ePMInvImpHdr.PODate = ReadingDate;
            }
            if (!string.IsNullOrEmpty(ePMInvoiceImportModel.InvoiceDate))
            {

                DateTime InvoiceDate = DateTime.ParseExact(ePMInvoiceImportModel.InvoiceDate, "yyyyMMdd", CultureInfo.InvariantCulture);

                ePMInvImpHdr.InvoiceDate = InvoiceDate;
            }
         
            ePMInvImpHdr.TotalInvoiceAmount = ePMInvoiceImportModel.POTotalCost;
            ePMInvImpHdr.Vendor = ePMInvoiceImportModel.VendorNumber;
            ePMInvImpHdr.VendorName = ePMInvoiceImportModel.VendorName;
            Boolean valid = true;
            string errMsg = "";
            if (valid)
            {
                try
                {
                    ePMInvImpHdr.Create(_userData.DatabaseKey);
                }
                catch (Exception ex)
                {
                    errMsg = ex.ToString();
                    ePMInvImpHdr.ErrorMessage = errMsg;
                }
            }
           
            return ePMInvImpHdr;
        }

      
        #endregion

        #region Validation
        public List<EPMInvoiceImportErrorResponseModel> EPMInvoiceImportValidate(long logId)
        {
            int TotalProcess = 0;
            int SuccessfulProcess = 0;
            int FailedProcess = 0;
            string logMessage = string.Empty;
            string errCodes = string.Empty;
            string errMsgs = string.Empty;
            EPMInvoiceImportErrorResponseModel objEPMInvoiceImportErrorResponseModel;

            List<EPMInvoiceImportErrorResponseModel> objEPMInvoiceImportErrorResponseModelList = new List<EPMInvoiceImportErrorResponseModel>();
            EPMInvImpHdr epmInvImpHdr = new EPMInvImpHdr()
            {
                ClientId = _userData.DatabaseKey.Client.ClientId,
                SiteId = _userData.Site.SiteId
            };
            List<EPMInvImpHdr> ePMInvImpHdrslist = epmInvImpHdr.EPMInvImpHdrImportRetrieveAll(_userData.DatabaseKey);
            EPMInvImpLine ePMInvImpLine = new EPMInvImpLine();
            ePMInvImpLine.ClientId = _userData.DatabaseKey.Client.ClientId;
            List<EPMInvImpLine> ePMInvImpLines = ePMInvImpLine.EPMInvImpLineImportRetrieveAll(_userData.DatabaseKey);
            TotalProcess = ePMInvImpHdrslist.Count;
            if (TotalProcess > 0)
            {
                foreach (EPMInvImpHdr _ePMInvImpHdrs in ePMInvImpHdrslist)
                {
                    objEPMInvoiceImportErrorResponseModel = new EPMInvoiceImportErrorResponseModel();
                    errCodes = "";
                    errMsgs = "";
                    _ePMInvImpHdrs.ErrorMessages = null;
                    //header validation
                    _ePMInvImpHdrs.CheckEPMInvImpHdrValidate(_userData.DatabaseKey);
                    if (_ePMInvImpHdrs.ErrorMessages != null && _ePMInvImpHdrs.ErrorMessages.Count > 0)
                    {
                        if (_ePMInvImpHdrs.ErrorCodes != null)
                        {
                            foreach (var v in _ePMInvImpHdrs.ErrorCodd)
                            {
                                errCodes += v;
                                errCodes += ",";
                            }
                            errCodes = errCodes.TrimEnd(',');
                            foreach (var v in _ePMInvImpHdrs.ErrorMessages)
                            {
                                errMsgs += v;
                                errMsgs += ",";
                                objEPMInvoiceImportErrorResponseModel.EPMInvoiceImportErrMsgList.Add(v);
                            }
                            
                            errMsgs = errMsgs.TrimEnd(',');
                            
                            objEPMInvoiceImportErrorResponseModel.PurchaseOrderId = _ePMInvImpHdrs.PONumber;
                            logMessage = logMessage + errMsgs;
                            // Add the error code(2) to the EPMInvImpHdr table
                            _ePMInvImpHdrs.ErrorCodes = errCodes;
                            _ePMInvImpHdrs.ErrorMessage = errMsgs;
                            _ePMInvImpHdrs.LastProcess = DateTime.UtcNow;
                            _ePMInvImpHdrs.Update(_userData.DatabaseKey);
                            if (objEPMInvoiceImportErrorResponseModel.EPMInvoiceImportErrMsgList.Count > 0)
                            {
                                objEPMInvoiceImportErrorResponseModelList.Add(objEPMInvoiceImportErrorResponseModel);
                            }

                        }
                    }
                    else
                    {
                        //get PO Details from the PurchaseOrder
                        PurchaseOrder purchaseOrder = new PurchaseOrder();
                        purchaseOrder.ClientId = _ePMInvImpHdrs.ClientId;
                        purchaseOrder.SiteId = _ePMInvImpHdrs.SiteId;
                        purchaseOrder.ClientLookupId = _ePMInvImpHdrs.PONumber;
                        PurchaseOrder  objpurchaseOrder = purchaseOrder.RetrieveByClientIDAndSiteIdAndClientLookUpId_V2(_userData.DatabaseKey).FirstOrDefault();
                        //GET Vendor Details 
                        Vendor vendor = new Vendor();
                        vendor.ClientId = _ePMInvImpHdrs.ClientId;
                        vendor.SiteId = _ePMInvImpHdrs.SiteId;
                        vendor.ClientLookupId = _ePMInvImpHdrs.Vendor;
                        //create new record for Invoice Match Header
                        Vendor objvendor = vendor.RetrieveBySiteIdAndClientLookUpId(_userData.DatabaseKey).FirstOrDefault();

                        #region
                        //create new record for Invoice Match Header  from EPMInvImpHdr
                        //add copy of EPMInvImpHdr in EPMInvImpHdrHist  
                        //delete from EPMInvImpHdr
                        InvoiceMatchHeader invoiceMatchHeader = CreateInvoiceMatchHeader(_ePMInvImpHdrs, objpurchaseOrder, objvendor);

                        #endregion
                        if (invoiceMatchHeader.ErrorMessages == null || invoiceMatchHeader.ErrorMessages.Count == 0)
                        {

                            CreateInvoiceMatchEventLog(invoiceMatchHeader.InvoiceMatchHeaderId, "Import", "Invoice Created from Import");
                            List<EPMInvImpLine> ePMInvImpLinesByHeaderIdandClientIdList = ePMInvImpLines.Where(m => m.EPMInvImpHdrId == _ePMInvImpHdrs.EPMInvImpHdrId && m.ClientId == _ePMInvImpHdrs.ClientId).ToList();
                            foreach (var _ePMInvImpLine in ePMInvImpLinesByHeaderIdandClientIdList)
                            {
                                objEPMInvoiceImportErrorResponseModel = new EPMInvoiceImportErrorResponseModel();
                                // validate Line item ---
                                _ePMInvImpLine.CheckEPMInvImpLineValidate(_userData.DatabaseKey);
                                if (_ePMInvImpLine.ErrorMessages != null && _ePMInvImpLine.ErrorMessages.Count > 0)
                                {//if validation of line failed getting the errors
                                    logMessage = logMessage + ValiddateEPMInvImpLineError(logMessage, objEPMInvoiceImportErrorResponseModel, _ePMInvImpLine,objpurchaseOrder.ClientLookupId);
                                    if (objEPMInvoiceImportErrorResponseModel.EPMInvoiceImportErrMsgList.Count > 0)
                                    {
                                        objEPMInvoiceImportErrorResponseModelList.Add(objEPMInvoiceImportErrorResponseModel);
                                    }
                                }
                                else
                                {
                                    #region

                                    //1.adding new record in InvoiceMatchItem table from EPMInvImpLine
                                    //2.add new copy of EPMInvImpLine in EPMInvImpLineHist
                                    //3.delete from EPMInvImpLine 
                                  var  _epmInvImpLine= CreateInvoiceMatchItemFromEPMInvImpLine(_ePMInvImpHdrs, invoiceMatchHeader, _ePMInvImpLine);
                                    if (_epmInvImpLine.ErrorMessages == null || _epmInvImpLine.ErrorMessages.Count == 0)
                                    {
                                        SuccessfulProcess++;
                                    }
                                    else
                                    {
                                        FailedProcess++;
                                    }
                                        #endregion
                                    }

                            }
                            TotalProcess= TotalProcess + ePMInvImpLinesByHeaderIdandClientIdList.Count;
                        }

                    }

                    if (errMsgs == null || errMsgs.Length == 0)
                    {
                        SuccessfulProcess++;
                    }
                    else
                    {
                        FailedProcess++;
                    }
                }
            }
            
            if (FailedProcess == 0)
            {
                logMessage = ErrorMessageConstants.Process_Complete_EPM_Invoice_Import_Successfully;
            }
            //---update log---
            CommonWrapper commonWrapper = new CommonWrapper(_userData);
            commonWrapper.UpdateLog(logId, TotalProcess, SuccessfulProcess, FailedProcess, logMessage, Common.Constants.ApiConstants.EPMInvoiceImport);
            return objEPMInvoiceImportErrorResponseModelList;
        }



        #region Create InvoiceMatchHeader
        private InvoiceMatchHeader CreateInvoiceMatchHeader(EPMInvImpHdr _ePMInvImpHdrs, PurchaseOrder purchaseOrder, Vendor vendor)
        {
            //create new record for Invoice Match Header
            InvoiceMatchHeader invoiceMatchHeader = new InvoiceMatchHeader();
            invoiceMatchHeader.ClientId = _ePMInvImpHdrs.ClientId;
            invoiceMatchHeader.SiteId = _ePMInvImpHdrs.SiteId;
            invoiceMatchHeader.ClientLookupId = _ePMInvImpHdrs.InvoiceNumber;
            invoiceMatchHeader.Creator_PersonnelId = _userData.DatabaseKey.Personnel.PersonnelId;
            invoiceMatchHeader.PurchaseOrderId = purchaseOrder.PurchaseOrderId;
            invoiceMatchHeader.ReceiptDate = DateTime.UtcNow;
            invoiceMatchHeader.Responsible_PersonnelId = purchaseOrder.Buyer_PersonnelId;
            invoiceMatchHeader.Status = GlobalConstants.Import;
            invoiceMatchHeader.TotalInput = _ePMInvImpHdrs.TotalInvoiceAmount;
            invoiceMatchHeader.Type = InvoiceMatchingTypeConstants.Purch;
            invoiceMatchHeader.InvoiceDate = _ePMInvImpHdrs.InvoiceDate;
            invoiceMatchHeader.IsExternal = true;
            invoiceMatchHeader.VendorId = vendor.VendorId;
            try
            {
                invoiceMatchHeader.Create(_userData.DatabaseKey);
            }
            catch (Exception ex)
            {
                 
                invoiceMatchHeader.ErrorMessages.Add(ex.ToString());
            }
            
            
            //add copy of EPMInvImpHdr in EPMInvImpHdrHist
            if (invoiceMatchHeader.ErrorMessages == null || invoiceMatchHeader.ErrorMessages.Count == 0)
            {
                EPMInvImpHdrHist ePMInvImpHdrHist = new EPMInvImpHdrHist();
                ePMInvImpHdrHist.ClientId = _ePMInvImpHdrs.ClientId;
                ePMInvImpHdrHist.EPMInvImpHdrId = _ePMInvImpHdrs.EPMInvImpHdrId;
                ePMInvImpHdrHist.SiteId = _ePMInvImpHdrs.SiteId;
                ePMInvImpHdrHist.PONumber = _ePMInvImpHdrs.PONumber;
                ePMInvImpHdrHist.PODate = _ePMInvImpHdrs.PODate;
                ePMInvImpHdrHist.InvoiceNumber = _ePMInvImpHdrs.InvoiceNumber;
                ePMInvImpHdrHist.InvoiceDate = _ePMInvImpHdrs.InvoiceDate;
                ePMInvImpHdrHist.TotalInvoiceAmount = _ePMInvImpHdrs.TotalInvoiceAmount;
                ePMInvImpHdrHist.VendorNumber = _ePMInvImpHdrs.Vendor;
                ePMInvImpHdrHist.VendorName = _ePMInvImpHdrs.VendorName;
                ePMInvImpHdrHist.DateProcessed = DateTime.UtcNow;
                try
                {
                    ePMInvImpHdrHist.Create(_userData.DatabaseKey);
                }
                catch (Exception ex)
                {

                    ePMInvImpHdrHist.ErrorMessages.Add(ex.ToString());
                }
               
                //delete from EPMInvImpHdr
                if (ePMInvImpHdrHist.ErrorMessages == null || ePMInvImpHdrHist.ErrorMessages.Count == 0)
                {
                    EPMInvImpHdr ePMInvImpHdr = new EPMInvImpHdr();
                    ePMInvImpHdr.ClientId = _ePMInvImpHdrs.ClientId;
                    ePMInvImpHdr.EPMInvImpHdrId = _ePMInvImpHdrs.EPMInvImpHdrId;
                    ePMInvImpHdr.SiteId = _ePMInvImpHdrs.SiteId;
                    try
                    {
                        ePMInvImpHdr.Delete(_userData.DatabaseKey);
                    }
                    catch (Exception ex)
                    {

                        ePMInvImpHdr.ErrorMessages.Add(ex.ToString());
                    }
                    
                }
            }

            return invoiceMatchHeader;
        }
        #endregion

        #region Create InvoiceMatchItem From EPMInvImpLine
        private EPMInvImpLine CreateInvoiceMatchItemFromEPMInvImpLine(EPMInvImpHdr _ePMInvImpHdrs, InvoiceMatchHeader invoiceMatchHeader, EPMInvImpLine _ePMInvImpLine)
        {
            //adding new record in InvoiceMatchItem table from EPMInvImpLine
            InvoiceMatchItem invoiceMatchItem = new InvoiceMatchItem();
            invoiceMatchItem.ClientId = _ePMInvImpHdrs.ClientId;
            invoiceMatchItem.InvoiceMatchHeaderId = invoiceMatchHeader.InvoiceMatchHeaderId;
            invoiceMatchItem.POReceiptItemID = 0;
            invoiceMatchItem.Quantity = _ePMInvImpLine.Quantity;
            invoiceMatchItem.UnitCost = _ePMInvImpLine.UnitCost;
            invoiceMatchItem.UnitOfMeasure = _ePMInvImpLine.UnitOfMeasure;
            invoiceMatchItem.Creator_PersonnelId = _userData.DatabaseKey.Personnel.PersonnelId;
            try
            {
                invoiceMatchItem.Create(_userData.DatabaseKey);
            }
            catch (Exception ex)
            {

                invoiceMatchItem.ErrorMessages.Add(ex.ToString());
            }
           
            //add new copy of EPMInvImpLine in EPMInvImpLineHist
            EPMInvImpLineHist ePMInvImpLineHist = new EPMInvImpLineHist();
            ePMInvImpLineHist.ClientId = _ePMInvImpLine.ClientId;
            ePMInvImpLineHist.EPMInvImpHdrId = _ePMInvImpLine.EPMInvImpHdrId;
            ePMInvImpLineHist.LineNumber = _ePMInvImpLine.LineNumber;
            ePMInvImpLineHist.BuyerPartNumber = _ePMInvImpLine.BuyerPartNumber;
            ePMInvImpLineHist.VendorPartNumber = _ePMInvImpLine.VendorPartNumber;
            ePMInvImpLineHist.Quantity = _ePMInvImpLine.Quantity;
            ePMInvImpLineHist.UnitCost = _ePMInvImpLine.UnitCost;
            ePMInvImpLineHist.UnitOfMeasure = _ePMInvImpLine.UnitOfMeasure;
            ePMInvImpLineHist.DateProcessed = DateTime.UtcNow;
            try
            {
                ePMInvImpLineHist.Create(_userData.DatabaseKey);
            }
            catch (Exception ex)
            {

                ePMInvImpLineHist.ErrorMessages.Add(ex.ToString());
            }
            
            //delete from EPMInvImpLine
            if (ePMInvImpLineHist.ErrorMessages == null || ePMInvImpLineHist.ErrorMessages.Count == 0)
            {
                EPMInvImpLine ePMInvImpLine = new EPMInvImpLine();
                ePMInvImpLine.ClientId = _ePMInvImpLine.ClientId;
                ePMInvImpLine.EPMInvImpLineId = _ePMInvImpLine.EPMInvImpLineId;
                try
                {
                    ePMInvImpLine.Delete(_userData.DatabaseKey);
                }
                catch (Exception ex)
                {

                    ePMInvImpLine.ErrorMessages.Add(ex.ToString());
                }
                
            }
            return _ePMInvImpLine;
        }
        #endregion
        private string ValiddateEPMInvImpLineError(string logMessage, EPMInvoiceImportErrorResponseModel objEPMInvoiceImportErrorResponseModel, EPMInvImpLine ePMInvImpLine,string PoClientLookupId)
        {
            string errCodesLine = "";
            string errMsgsLine = "";
            
            if (ePMInvImpLine.ErrorMessages != null && ePMInvImpLine.ErrorMessages.Count > 0)
            {
                if (ePMInvImpLine.ErrorCodes != null)
                {
                    foreach (var v in ePMInvImpLine.ErrorCodd)
                    {
                        errCodesLine += v;
                        errCodesLine += ",";
                        
                    }
                    errCodesLine = errCodesLine.TrimEnd(',');
                    objEPMInvoiceImportErrorResponseModel.PurchaseOrderId = PoClientLookupId;
                 
                    foreach (var v in ePMInvImpLine.ErrorMessages)
                    {
                        errMsgsLine += v;
                        errMsgsLine += ",";
                        objEPMInvoiceImportErrorResponseModel.EPMInvoiceImportErrMsgList.Add(v);
                    }
                    errMsgsLine = errMsgsLine.TrimEnd(',');
                    logMessage = logMessage + errMsgsLine;
                    // Add the error code(2) to the EPMInvImpLine table
                    ePMInvImpLine.ErrorCodes = errCodesLine;
                    ePMInvImpLine.ErrorMessage = errMsgsLine;
                    ePMInvImpLine.LastProcess = DateTime.UtcNow;
                    ePMInvImpLine.Update(_userData.DatabaseKey);
                }
            }

            return logMessage;
        }
        #endregion
        #region Event
        public void CreateInvoiceMatchEventLog(long objId, string eventVal,string comment)
        {
            InvoiceMatchEventLog log = new InvoiceMatchEventLog();
            log.ClientId = _userData.DatabaseKey.Client.ClientId;
            log.SiteId = _userData.DatabaseKey.User.DefaultSiteId;
            log.ObjectId = objId;
            log.TransactionDate = DateTime.UtcNow;
            log.Event = eventVal;
            log.PersonnelId = _userData.DatabaseKey.Personnel.PersonnelId;
            log.Comments = comment;
            log.SourceId = 0;
            log.Create(_userData.DatabaseKey);
        }
        #endregion

    }
}