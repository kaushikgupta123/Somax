using Common.Constants;
using DataContracts;
using InterfaceAPI.BusinessWrapper.Interface;
using InterfaceAPI.Models;
using InterfaceAPI.Models.Common;
using InterfaceAPI.Models.PurchaseOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Web;

namespace InterfaceAPI.BusinessWrapper.Implementation
{
    public class POImportWrapper : CommonWrapper, IPOImportWrapper
    {
        POImpHdr _objPOImpHdr;
        public POImportWrapper(UserData userData, POImpHdr objPOImpHdr) : base(userData)
        {
            _userData = userData;
            _dbKey = userData.DatabaseKey;
            _objPOImpHdr = objPOImpHdr;
        }
        

        #region Create POImpHdr
        public ProcessLogModel CreatePOImportHeader(List<PurchaseOrderImportModel> PurchaseOrderImportModelList)
        {
            int TotalProcess = 0;
            int SuccessfulProcess = 0;
            int FailedProcess = 0;
            string logMessage = string.Empty;
            POImpHdr _poImpHdr;
            POImpLine _poImpLine;
            TotalProcess = PurchaseOrderImportModelList.Count;
            ProcessLogModel objProcessLogModel = new ProcessLogModel();
            foreach (var item in PurchaseOrderImportModelList)
            {
                _poImpHdr = new POImpHdr();
                _poImpLine = new POImpLine();
                //--Retrieve header
                _poImpHdr.ClientId = item.ClientId;
                _poImpHdr.SiteId = item.SiteId;
                // RKL - 2020-Aug-13
                // The external purchase order number is a required column
                // If it is not there - the item will not be processed
                _poImpHdr.EXPOID = item.ExPOId;
                _poImpHdr.GetPOImpHdrRetrievedDataByEXPOId(_userData.DatabaseKey);               
                if (_poImpHdr.POImpHdrId <= 0)  //Header item does not exist in the header table 
                {
                    //---Create Header---
                    _poImpHdr.ClientId = item.ClientId;
                    _poImpHdr.SiteId = item.SiteId;
                    InsertPoHdr(_poImpHdr, item);
                    // -- Create Line Items
                    _poImpLine = new POImpLine();
                    foreach (var litem in item.LineItems)
                    {
                      _poImpLine.Clear();
                      _poImpLine.ClientId = item.ClientId;
                      _poImpLine.POImpHdrId = _poImpHdr.POImpHdrId; // RKL - 2020-Aug-13 -Added POImpHdrId column to the line item (Active and History)
                      InsertPoLine(_poImpLine, item, litem);
                    }
                }
                else    //Header item exists in the header table 
                {
                    //---Update Header in case it changed ---
                    UpdatePoHdr(_poImpHdr, item, "", "", null);
                    //---line---
                    foreach (var litem in item.LineItems)
                    {
                        // Retrieve the Line Item (if exists) based on the ClientId, POImpHdrId, External PO ID and the external PO Line Item ID
                        _poImpLine = new POImpLine()
                        {
                          ClientId = item.ClientId,
                          POImpHdrId = _poImpHdr.POImpHdrId,
                          EXPOID = _poImpHdr.EXPOID,
                          EXPOLineID = litem.EXPOLineId
                        };
                        _poImpLine.GetPOImpLineRetrieveByExPOEXPOLine(_userData.DatabaseKey);                        
                        if (_poImpLine.POImpLineId <= 0) //--line item does not exist
                        {
                            //---Create Line item---
                            _poImpLine.ClientId = item.ClientId;
                            _poImpLine.POImpHdrId = _poImpHdr.POImpHdrId;
                            InsertPoLine(_poImpLine, item, litem);
                        }
                        else  //--line item already exists
                        {
                            //---Update Line item---
                            _poImpLine.ClientId = item.ClientId;
                            _poImpLine.POImpHdrId = _poImpHdr.POImpHdrId;
                            UpdatePoLine(_poImpLine, item, litem, "", "", null);
                        }
                    }
                }
                if (_poImpHdr.ErrorMessages == null || _poImpHdr.ErrorMessages.Count == 0)
                {
                    SuccessfulProcess++;
                }
                else
                {
                    FailedProcess++;
                }
            } //end of foreach
            if (FailedProcess == 0)
            {
                logMessage = "Json data inserted into temporary table.";
            }
            else
            {
                logMessage = "Conversion of json data to temporary data tables failed.";
            }
            objProcessLogModel.TotalProcess = TotalProcess;
            objProcessLogModel.SuccessfulProcess = SuccessfulProcess;
            objProcessLogModel.FailedProcess = FailedProcess;
            objProcessLogModel.logMessage = logMessage;
            return objProcessLogModel;
        }
        #endregion

        #region Validation
        /// <summary>
        /// POImportValidate
        /// RKL - 2019-09-24 - We are processing the records in the PurchaseOrderImportModel list
        ///                    We should be processing all the records in the table 
        ///                    there is no reason to store if the table if we are not going to using it
        /// Validates Each PurchaseOrderImportModel in the list
        ///   If the header is not valid - the saved import record is updated with an error 
        ///                              - all line items in the import records are noted with an error
        ///   If the header is valid - a new PO record is created 
        ///                          - each line item in the list is validated
        ///                          - if the line is not valid - the saved record is marked with an error 
        ///                          - if the line is valid - a new PO Line Item is created
        /// </summary>
        /// <param name="logId"></param>
        /// <param name="PurchaseOrderImportModelList"></param>
        /// <returns></returns>
        public List<ImportErrorResponseModel> POImportValidate(long logId, List<PurchaseOrderImportModel> PurchaseOrderImportModelList)
        {
            int TotalProcess = 0;
            int SuccessfulProcess = 0;
            int FailedProcess = 0;
            string logMessage = string.Empty;
            string errCodes = string.Empty;
            string errMsgs = string.Empty;

            #if DEBUG
              // Save the Json String to a directory on the FTP Server 
              if (PurchaseOrderImportModelList != null && PurchaseOrderImportModelList.Count > 0)
              {
                string POImport = JsonConvert.SerializeObject(PurchaseOrderImportModelList);
                this.ArchivePOImportFiles(POImport);
              }
            #endif
            ImportErrorResponseModel objImportErrorResponseModel;
            ImportLineErrorModel objImportLineErrorModel;
            List<ImportErrorResponseModel> ImportErrorResponseList = new List<ImportErrorResponseModel>();

            TotalProcess = PurchaseOrderImportModelList.Count;

            foreach (var item in PurchaseOrderImportModelList)
            {
                objImportErrorResponseModel = new ImportErrorResponseModel();
                //---validate header---
                POImpHdr _poImpHdr = new POImpHdr()
                {
                    ClientId = item.ClientId,
                    EXPOID = item.ExPOId,
                    SiteId = item.SiteId,
                    PONumber = item.PONumber,
                    SOMAXPRID = item.SOMAXReqId,
                    POCreateDate = item.CreateDate,
                    EXVendorId = item.ExVendorId,
                    RequiredDate = item.RequiredDate,
                    Status = item.Status
                };
                _poImpHdr.CheckPOImpValidate(_userData.DatabaseKey);
                if (_poImpHdr.ErrorMessages != null && _poImpHdr.ErrorMessages.Count > 0)  //---header validation fails
                {
                    objImportErrorResponseModel.EXPOID = item.ExPOId;
                    errCodes = "";
                    errMsgs = "";
                    if (_poImpHdr.ErrorCodd != null)
                    {
                        foreach (var v in _poImpHdr.ErrorCodd)
                        {
                            errCodes += v;
                            errCodes += ",";
                        }
                        errCodes = errCodes.TrimEnd(',');
                    }
                    foreach (var v in _poImpHdr.ErrorMessages)
                    {
                        errMsgs += v;
                        errMsgs += ",";
                        objImportErrorResponseModel.HdrErrMsgList.Add(v);
                    }
                    errMsgs = errMsgs.TrimEnd(',');
                    _poImpHdr = new POImpHdr();
                    _poImpHdr.ClientId = item.ClientId;

                    //---update header table---
                    _poImpHdr.SiteId = item.SiteId;
                    if (item.ExPOId > 0)
                    {
                        _poImpHdr.EXPOID = item.ExPOId;
                        _poImpHdr.GetPOImpHdrRetrievedDataByEXPOId(_userData.DatabaseKey);
                    }
                    else if (item.SOMAXReqId > 0)
                    {
                        _poImpHdr.SOMAXPRID = item.SOMAXReqId;
                        _poImpHdr.GetPOImpHdrRetrievedDataByPRId(_userData.DatabaseKey);
                    }
                    UpdatePoHdr(_poImpHdr, item, errCodes, errMsgs, null);  //--update header with validation failure message
                    logMessage = logMessage + errMsgs;
                    // The line items all have the POImpHdrId as a property.
                    // We could "mass" update them with the validation message
                    // Maybe do this - later
                    foreach (var litem in item.LineItems)
                    {
                        POImpLine _poImpLine = new POImpLine();
                        _poImpLine.ClientId = item.ClientId;
                        _poImpLine.SiteId = item.SiteId;
                        _poImpLine.POImpHdrId = _poImpHdr.POImpHdrId;
                        _poImpLine.EXPOID = item.ExPOId;  
                        _poImpLine.EXPOLineID = litem.EXPOLineId;
                        _poImpLine.GetPOImpLineRetrieveByExPOEXPOLine(_userData.DatabaseKey);
                        UpdatePoLine(_poImpLine, item, litem, errCodes, "Header validation failed for this line item", null);
                    }
                }
                else  //---header validation successful
                {
                    //--process header
                    _poImpHdr.ClientId = item.ClientId;
                    _poImpHdr.SiteId = item.SiteId;
                    _poImpHdr.EXPOID = item.ExPOId;
                    _poImpHdr.GetPOImpHdrRetrievedDataByEXPOId(_userData.DatabaseKey);
                    _poImpHdr.PersonnelId = _userData.DatabaseKey.Personnel.PersonnelId;
                    _poImpHdr.PurchasingEventCreate = PurchasingEvents.Import;
                    _poImpHdr.PurchasingEventUpdate = PurchasingEvents.ImportUpdate;
                    _poImpHdr.GetPOImpoHdrProcessImport(_userData.DatabaseKey);

                    //---line---
                    foreach (var litem in item.LineItems)
                    {
                        //---validate line---
                        objImportLineErrorModel = new ImportLineErrorModel();
                        POImpLine _poImpLine = new POImpLine();
                        _poImpLine.ClientId = item.ClientId;
                        _poImpLine.SiteId = item.SiteId;
                        _poImpLine.EXPOID = item.ExPOId;  //?? verify reference
                        _poImpLine.EXPOLineID = litem.EXPOLineId;
                        _poImpLine.SOMAXPRLineId = litem.SOMAXPRLineId;
                        _poImpLine.PLCreateDate = litem.CreateDate;
                        _poImpLine.Status = litem.Status;

                        _poImpLine.CheckPOImpLineValidate(_userData.DatabaseKey);
                        if (_poImpLine.ErrorMessages != null && _poImpLine.ErrorMessages.Count > 0)  //--line validation fails
                        {
                            objImportErrorResponseModel.EXPOID = item.ExPOId;
                            objImportLineErrorModel.ExPOLineId = litem.EXPOLineId;
                            errCodes = "";
                            errMsgs = "";
                            if (_poImpLine.ErrorCodd != null)
                            {
                                foreach (var v in _poImpLine.ErrorCodd)
                                {
                                    errCodes += v;
                                    errCodes += ",";
                                }
                                errCodes = errCodes.TrimEnd(',');
                            }
                            foreach (var v in _poImpLine.ErrorMessages)
                            {
                                errMsgs += v;
                                errMsgs += ",";
                                objImportLineErrorModel.LineErrMsgList.Add(v);
                            }
                            errMsgs = errMsgs.TrimEnd(',');
                            objImportErrorResponseModel.ImportLineErrorList.Add(objImportLineErrorModel);
                            //---Update Line item---
                            _poImpLine.ClientId = item.ClientId;//  _userData.DatabaseKey.Client.ClientId;
                            _poImpLine.EXPOLineID = litem.EXPOLineId;
                            _poImpLine.SOMAXPRLineId = litem.SOMAXPRLineId > 0 ? litem.SOMAXPRLineId : 0;
                            _poImpLine.GetPOImpLineRetrieveByExPOEXPOLine(_userData.DatabaseKey);
                            UpdatePoLine(_poImpLine, item, litem, errCodes, errMsgs, null);
                            logMessage = logMessage + errMsgs;
                        }
                        else    //--line validation succeeded
                        {
                            //---process line---
                            _poImpLine.ClientId = item.ClientId;
                            _poImpLine.SiteId = item.SiteId;
                            _poImpLine.EXPOLineID = litem.EXPOLineId;
                            _poImpLine.SOMAXPRLineId = litem.SOMAXPRLineId > 0 ? litem.SOMAXPRLineId : 0;
                            _poImpLine.POImpHdrId = _poImpHdr.POImpHdrId;
                            _poImpLine.GetPOImpLineRetrieveByExPOEXPOLine(_userData.DatabaseKey);
                            _poImpLine.GetPOImpLineProcessImport(_userData.DatabaseKey);
                            //--delete line--
                            // the line is being deleted in the process stored procedure
                            //_poImpLine.Delete(_userData.DatabaseKey);
                        }
                    }
                    //--delete header--
                    // RKL 2020-Aug-17 - The header is being deleted in the process sp
                    //_poImpHdr.Delete(_userData.DatabaseKey);
                    // RKL 2020-Aug-27 
                    //  We have updated the header and line items 
                    //  We now are going to update the header status based on the line item status values 
                    PurchaseOrder po = new PurchaseOrder
                    {
                      ClientId = item.ClientId,
                      SiteId = item.SiteId,
                      PurchaseOrderId = _poImpHdr.PurchaseOrderId
                    };
                    po.RetrieveByPKForeignKeys(_userData.DatabaseKey, _userData.Site.TimeZone);
                    po.UpdateStatus(_userData.DatabaseKey);
                }
                var xErrdata = ImportErrorResponseList.FirstOrDefault(p => p.EXPOID.Equals(item.ExPOId));
                if (xErrdata == null && (objImportErrorResponseModel.HdrErrMsgList.Count > 0 || objImportErrorResponseModel.ImportLineErrorList.Count > 0))
                {
                    ImportErrorResponseList.Add(objImportErrorResponseModel);
                }
                
                if (errMsgs == null || errMsgs.Length == 0)
                {
                    SuccessfulProcess++;
                }
                else
                {
                    FailedProcess++;
                }
            }    //end of foreach
            if (FailedProcess == 0)
            {
                logMessage = "Process Complete – All items processed successfully.";
            }
            //---update log---
            CommonWrapper commonWrapper = new CommonWrapper(_userData);
            commonWrapper.UpdateLog(logId, TotalProcess, SuccessfulProcess, FailedProcess, logMessage, Common.Constants.ApiConstants.DeanFoodsPOImport);
            return ImportErrorResponseList;
        }
#endregion

#region Common

        private void InsertPoHdr(POImpHdr _poImpHdr, PurchaseOrderImportModel item)
        {
            _poImpHdr.PONumber = item.PONumber;
            _poImpHdr.Revision = item.Revision;
            _poImpHdr.EXPOID = item.ExPOId;
            _poImpHdr.EXPRID = item.ExPRId;
            _poImpHdr.SOMAXPRNumber = item.SOMAXReqNo ?? "";
            _poImpHdr.SOMAXPRID = item.SOMAXReqId;
            _poImpHdr.POCreateDate = (item.CreateDate == null || item.CreateDate == DateTime.MinValue ? Convert.ToDateTime(DateTime.UtcNow) : item.CreateDate);
            _poImpHdr.Currency = item.Currency;
            _poImpHdr.EXVendor = item.Vendor;
            _poImpHdr.EXVendorId = item.ExVendorId;
            _poImpHdr.RequiredDate = item.RequiredDate == null || item.RequiredDate == DateTime.MinValue ? Convert.ToDateTime(DateTime.UtcNow) : item.RequiredDate;
            _poImpHdr.PaymentTerms = item.PaymentTerms;
            _poImpHdr.Status = item.Status ?? "";
            _poImpHdr.ErrorCodes = "";
            _poImpHdr.ErrorMessage = "";
            _poImpHdr.LastProcess = null;
            _poImpHdr.CustomCreate(_userData.DatabaseKey);
        }


        private void UpdatePoHdr(POImpHdr _poImpHdr, PurchaseOrderImportModel item, string ErrorCodes, string ErrorMessage, DateTime? LastProcess)
        {
            _poImpHdr.PONumber = item.PONumber;
            _poImpHdr.Revision = item.Revision;
            _poImpHdr.EXPOID = item.ExPOId;
            _poImpHdr.EXPRID = item.ExPRId;
            _poImpHdr.SOMAXPRNumber = item.SOMAXReqNo ?? "";
            _poImpHdr.SOMAXPRID = item.SOMAXReqId;
            _poImpHdr.POCreateDate = (item.CreateDate == null || item.CreateDate == DateTime.MinValue ? Convert.ToDateTime(DateTime.UtcNow) : item.CreateDate);
            _poImpHdr.Currency = item.Currency;
            _poImpHdr.EXVendor = item.Vendor;
            _poImpHdr.EXVendorId = item.ExVendorId;
            _poImpHdr.RequiredDate = (item.RequiredDate == null || item.RequiredDate == DateTime.MinValue ? Convert.ToDateTime(DateTime.UtcNow) : item.RequiredDate);
            _poImpHdr.PaymentTerms = item.PaymentTerms;
            _poImpHdr.Status = item.Status ?? "";
            _poImpHdr.ErrorCodes = ErrorCodes;
            _poImpHdr.ErrorMessage = ErrorMessage;
            _poImpHdr.LastProcess = LastProcess;
            _poImpHdr.UpdateCustom(_userData.DatabaseKey);
        }


        private void InsertPoLine(POImpLine _poImpLine, PurchaseOrderImportModel item, PurchaseOrderLineImportModel litem)
        {
            //output pk POImpLineId
            _poImpLine.EXPOID = item.ExPOId;
            _poImpLine.EXPOLineID = litem.EXPOLineId;
            _poImpLine.SOMAXPRLineId = litem.SOMAXPRLineId;
            _poImpLine.LineNumber = litem.LineNumber;
            _poImpLine.EXPartId = 0;
            _poImpLine.PartNumber = litem.PartNumber ?? string.Empty;
            _poImpLine.PartID = litem.PartId;
            _poImpLine.Description = litem.Description;
            _poImpLine.Quantity = litem.Quantity;
            _poImpLine.UnitCost = litem.UnitCost;
            _poImpLine.UnitOfMeasure = litem.UnitOfMeasure ?? string.Empty;
            _poImpLine.AccountCode = litem.AccountCode ?? string.Empty;
            _poImpLine.AccountId = 0;
            _poImpLine.PLCreateDate = litem.CreateDate ;
            _poImpLine.Status = litem.Status ?? "";
            _poImpLine.ErrorCodes = "";
            _poImpLine.ErrorMessage = "";
            _poImpLine.LastProcess = null;
            _poImpLine.CreateCustom(_userData.DatabaseKey);
        }

        private void UpdatePoLine(POImpLine _poImpLine, PurchaseOrderImportModel item, PurchaseOrderLineImportModel litem, string ErrorCodes, string ErrorMessage, DateTime? LastProcess)
        {
            // pk POImpLineId
            _poImpLine.EXPOID = item.ExPOId;
            _poImpLine.EXPOLineID = litem.EXPOLineId;
            _poImpLine.SOMAXPRLineId = litem.SOMAXPRLineId;
            _poImpLine.LineNumber = litem.LineNumber;
            _poImpLine.EXPartId = 0;
            _poImpLine.PartNumber = litem.PartNumber ?? string.Empty;
            _poImpLine.PartID = litem.PartId;
            _poImpLine.Description = litem.Description;
            _poImpLine.Quantity = litem.Quantity;
            _poImpLine.UnitCost = litem.UnitCost;
            _poImpLine.UnitOfMeasure = litem.UnitOfMeasure ?? string.Empty;
            _poImpLine.AccountCode = litem.AccountCode ?? string.Empty;
            _poImpLine.AccountId = 0;
            _poImpLine.PLCreateDate = litem.CreateDate;
            _poImpLine.Status = litem.Status ?? "";
            _poImpLine.ErrorCodes = ErrorCodes;
            _poImpLine.ErrorMessage = ErrorMessage;
            _poImpLine.LastProcess = LastProcess;
            _poImpLine.UpdateCustom(_userData.DatabaseKey);
        }
#endregion
    }
}