using DataContracts;
using InterfaceAPI.BusinessWrapper.Interface;
using InterfaceAPI.Models;
using InterfaceAPI.Models.Common;
using InterfaceAPI.Models.PurchaseOrderReceipt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace InterfaceAPI.BusinessWrapper.Implementation
{
  /*
  **************************************************************************************************
  * PROPRIETARY DATA 
  **************************************************************************************************
  * This work is PROPRIETARY to SOMAX Inc. and is protected 
  * under Federal Law as an unpublished Copyrighted work and under State Law as 
  * a Trade Secret. 
  **************************************************************************************************
  * Copyright (c) 2020 by SOMAX Inc.. All rights reserved. 
  **************************************************************************************************
  * Date        JIRA Item Person       Description
  * =========== ========= ============ =============================================================
  * 2020-Oct-15 V2-412    Roger Lawton Added Receipt Status
  **************************************************************************************************
  */
  public class ReceiptImportWrapper : CommonWrapper, IReceiptImportWrapper
    {
        ReceiptImpHdr _objReceiptImpHdr;
        public ReceiptImportWrapper(UserData userData, ReceiptImpHdr objReceiptImpHdr) : base(userData)
        {
            _userData = userData;
            _dbKey = userData.DatabaseKey;
            _objReceiptImpHdr = objReceiptImpHdr;
        }

        #region Insert into ReceiptImpHdr

        public ProcessLogModel InsertReceiptImpHdr(List<ReceiptImportModel> ReceiptImportModelObjectList)
        {
            int TotalProcess = 0;
            int SuccessfulProcess = 0;
            int FailedProcess = 0;
            string logMessage = string.Empty;
            ReceiptImpHdr _rImpHdr;
            ReceiptImpLine _rImpLine;
            TotalProcess = ReceiptImportModelObjectList.Count;
            ProcessLogModel objProcessLogModel = new ProcessLogModel();
            // For each item in the model list
            foreach (var item in ReceiptImportModelObjectList)
            {
                _rImpHdr = new ReceiptImpHdr();
                _rImpLine = new ReceiptImpLine();
                _rImpHdr.EXRecieptId = item.ExReceiptId;
                _rImpHdr.ClientId = item.ClientId;
                // Retrieve an existing ReceiptImpHeader record if it exists 
                // Retrieve by ClientId and ExReceiptId  
                _rImpHdr.GetReceiptImpHdrRetrievedDataByReceiptId(_userData.DatabaseKey);
                if (_rImpHdr.ReceiptImpHdrId <= 0)  //Header item does not exist in the header table 
                {
                    // Header does not exist
                    // Create It
                    InsertReceiptHdr(_rImpHdr, item);
                    // For each line item in the model list
                    foreach (var litem in item.ReceiptItems)
                    {
                        // Determine if the line item already exists
                        // Retrieve by the Client ID, PO Line Item and the Receipt Transaction ID
                        _rImpLine = new ReceiptImpLine();
                        _rImpLine.EXPOLineID = litem.ExPOLineId; //to be changed to EXReceiptTxnId
                        _rImpLine.EXReceiptTxnId = litem.ExReceiptTxnId;
                        _rImpLine.ClientId = item.ClientId;
                        _rImpLine.GetReceiptImpLineRetrievedDataByEXReceiptTxnId(_userData.DatabaseKey);
                        if (_rImpLine.ReceiptImpLineId <= 0)
                        {
                            //Line Item does not exist - Create it
                            //---Create Line item---

                            InsertReceiptLine(_rImpLine, item, litem);
                        }
                        else
                        {
                            //---Update Line item---

                            UpdateReceiptLine(_rImpLine, item, litem, Convert.ToDateTime(DateTime.UtcNow), "", "");
                        }
                    }
                }
                else //Header item exist in the header table
                {
                    //---Update header---

                    UpdateReceiptHdr(_rImpHdr, item, DateTime.UtcNow, "", "");

                    //---for line item---
                    foreach (var litem in item.ReceiptItems)
                    {
                        _rImpLine = new ReceiptImpLine();
                        _rImpLine.EXReceiptTxnId = litem.ExReceiptTxnId;
                        _rImpLine.EXPOLineID = litem.ExPOLineId;
                        _rImpLine.ClientId = item.ClientId;
                        _rImpLine.GetReceiptImpLineRetrievedDataByEXReceiptTxnId(_userData.DatabaseKey);
                        if (_rImpLine.ReceiptImpLineId <= 0)
                        {
                            //---Create Line item---

                            InsertReceiptLine(_rImpLine, item, litem);
                        }
                        else
                        {
                            //---Update Line item---

                            UpdateReceiptLine(_rImpLine, item, litem, Convert.ToDateTime(DateTime.UtcNow), "", "");
                        }
                    }
                }
                if (_rImpHdr.ErrorMessages == null || _rImpHdr.ErrorMessages.Count == 0)
                {
                    SuccessfulProcess++;
                }
                else
                {
                    FailedProcess++;
                }
            }
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
        public List<ReceiptErrorResponseModel> ReceiptsImportValidate(long logId, List<ReceiptImportModel> ReceiptImportModelObjectList)
        {
            int TotalProcess = 0;
            int SuccessfulProcess = 0;
            int FailedProcess = 0;
            string logMessage = string.Empty;
            string errCodes = string.Empty;
            string errMsgs = string.Empty;
            // RKL - 2020-Aug-19 - Should be processing the records from the db - not the imported objects
            // reason is there may be "old" ones that need to be re-processed
            // Not doing that at this time - revisit later?

            ReceiptErrorResponseModel objReceiptErrorResponseModel;
            ReceiptLineErrorModel objReceiptLineErrorModel;
            List<ReceiptErrorResponseModel> ReceiptErrorResponseList = new List<ReceiptErrorResponseModel>();

            TotalProcess = ReceiptImportModelObjectList.Count;
            foreach (var item in ReceiptImportModelObjectList)
            {
                objReceiptErrorResponseModel = new ReceiptErrorResponseModel();
                //---validate header---
                ReceiptImpHdr _rImpHdr = new ReceiptImpHdr()
                {
                    ClientId = item.ClientId,
                    EXPOID = item.ExPOID,
                    ReceiptDate = item.Receiptdate
                };
                _rImpHdr.CheckReceiptImpValidate(_userData.DatabaseKey);
                if (_rImpHdr.ErrorMessages != null && _rImpHdr.ErrorMessages.Count > 0) //--header validation fails
                {
                    errCodes = "";
                    errMsgs = "";
                    objReceiptErrorResponseModel.EXPOID = item.ExPOID;
                    if (_rImpHdr.ErrorCodd != null)
                    {
                        foreach (var v in _rImpHdr.ErrorCodd)
                        {
                            errCodes += v;
                            errCodes += ",";
                        }
                        errCodes = errCodes.TrimEnd(',');
                    }
                    foreach (var v in _rImpHdr.ErrorMessages)
                    {
                        errMsgs += v;
                        errMsgs += ",";
                        objReceiptErrorResponseModel.HdrErrMsgList.Add(v);
                    }
                    errMsgs = errMsgs.TrimEnd(',');
                    _rImpHdr = new ReceiptImpHdr();
                    _rImpHdr.ClientId = item.ClientId;
                    //---update header table---
                    _rImpHdr.EXRecieptId = item.ExReceiptId;
                    _rImpHdr.GetReceiptImpHdrRetrievedDataByReceiptId(_userData.DatabaseKey);
                    UpdateReceiptHdr(_rImpHdr, item, DateTime.UtcNow, errCodes, errMsgs);
                    ReceiptErrorResponseList.Add(objReceiptErrorResponseModel);
                    logMessage = logMessage + errMsgs;
                    //---update corressponding line items with header validation failure message
                    foreach (var litem in item.ReceiptItems)
                    {
                        ReceiptImpLine _rImpLine = new ReceiptImpLine();
                        _rImpLine.EXPOLineID = litem.ExPOLineId; //to be changed to EXReceiptTxnId
                        _rImpLine.ClientId = item.ClientId;
                        _rImpLine.EXReceiptId = item.ExReceiptId;
                        _rImpLine.EXReceiptTxnId = litem.ExReceiptTxnId;
                        _rImpLine.Quantity = litem.Quantity;
                        _rImpLine.EXReceiptTxnId = litem.ExReceiptTxnId;
                        _rImpLine.GetReceiptImpLineRetrievedDataByPOLineId(_userData.DatabaseKey);
                        UpdateReceiptLine(_rImpLine, item, litem, Convert.ToDateTime(DateTime.UtcNow), errCodes, "Header validation failed for this line item");
                    }
                }
                else  //--header validation succeeds
                {
                    //---process header----
                    _rImpHdr.EXRecieptId = item.ExReceiptId;
                    _rImpHdr.GetReceiptImpHdrRetrievedDataByReceiptId(_userData.DatabaseKey);
                    // Site and PersonnelId to be retrieved based on the PO in the sp
                    //_rImpHdr.SiteId = _userData.Site.SiteId; //--to be determined
                    //_rImpHdr.PersonnelId = _userData.DatabaseKey.Personnel.PersonnelId;
                    _rImpHdr.PurchasingEventCreate = Common.Constants.PurchasingEvents.ReceiptImport;
                    _rImpHdr.PurchasingEventUpdate = Common.Constants.PurchasingEvents.ReceiptImportUpdate;
                    _rImpHdr.GetReceiptImpHdrProcessImport(_userData.DatabaseKey);
                    if (_rImpHdr.ErrorMessages != null && _rImpHdr.ErrorMessages.Count() > 0)
                    {
                        objReceiptErrorResponseModel.EXPOID = item.ExPOID;
                        foreach (var er in _rImpHdr.ErrorMessages)
                        {
                            objReceiptErrorResponseModel.HdrProcessErrMsgList.Add(er);
                        }
                    }
                    //---line---
                    foreach (var litem in item.ReceiptItems)
                    {
                        objReceiptLineErrorModel = new ReceiptLineErrorModel();
                        ReceiptImpLine _rImpLine = new ReceiptImpLine();
                        _rImpLine.EXPOID = item.ExPOID;
                        _rImpLine.EXPOLineID = litem.ExPOLineId; //to be changed to EXReceiptTxnId
                        _rImpLine.ClientId = item.ClientId;
                        _rImpLine.EXReceiptId = item.ExReceiptId;
                        _rImpLine.EXReceiptTxnId = litem.ExReceiptTxnId;
                        _rImpLine.Quantity = litem.Quantity;
                        _rImpLine.UnitCost = litem.UnitCost;
                        _rImpLine.ReceiptStatus = litem.Status;  // V2-412 - Receipt Status originally came from the header
                        _rImpLine.CheckReceiptImpLineValidate(_userData.DatabaseKey);  //--validate line--
                        if (_rImpLine.ErrorMessages != null && _rImpLine.ErrorMessages.Count > 0)  //--line validation fails--
                        {
                            objReceiptErrorResponseModel.EXPOID = item.ExPOID;
                            objReceiptLineErrorModel.ExPOLineId = litem.ExPOLineId;
                            errCodes = "";
                            errMsgs = "";
                            if (_rImpLine.ErrorCodd != null)
                            {
                                foreach (var v in _rImpLine.ErrorCodd)
                                {
                                    errCodes += v;
                                    errCodes += ",";
                                }
                                errCodes = errCodes.TrimEnd(',');
                            }
                            foreach (var v in _rImpLine.ErrorMessages)
                            {
                                errMsgs += v;
                                errMsgs += ",";
                                objReceiptLineErrorModel.LineErrMsgList.Add(v);
                            }
                            errMsgs = errMsgs.TrimEnd(',');
                            objReceiptErrorResponseModel.ReceiptLineErrorList.Add(objReceiptLineErrorModel);
                            logMessage = logMessage + errMsgs;
                            //---update line table---
                            _rImpLine.EXReceiptTxnId = litem.ExReceiptTxnId;
                            _rImpLine.GetReceiptImpLineRetrievedDataByPOLineId(_userData.DatabaseKey);
                            UpdateReceiptLine(_rImpLine, item, litem, Convert.ToDateTime(DateTime.UtcNow), errCodes, errMsgs);
                        }
                        else  //--line validation succeds
                        {
                            //---process line---
                            // 
                            _rImpLine.EXReceiptTxnId = litem.ExReceiptTxnId;
                            _rImpLine.GetReceiptImpLineRetrievedDataByEXReceiptTxnId(_userData.DatabaseKey);
                            _rImpLine.GetReceiptImpLineProcessImport(_userData.DatabaseKey);
                            if (_rImpLine.spErrCode != -100)
                            {
                                _rImpLine.Delete(_userData.DatabaseKey);
                            }
                            else
                            {
                                _rImpLine.ErrorMessages.Add("Line insert error:EXPurchaseOrderLineId not found");
                                UpdateReceiptLine(_rImpLine, item, litem, Convert.ToDateTime(DateTime.UtcNow), "-100", "Line insert error:EXPurchaseOrderLineId not found");
                            }
                            if (_rImpLine.ErrorMessages != null && _rImpLine.ErrorMessages.Count() > 0)
                            {
                                objReceiptErrorResponseModel.EXPOID = item.ExPOID;
                                objReceiptLineErrorModel.ExPOLineId = litem.ExPOLineId;
                                foreach (var er in _rImpLine.ErrorMessages)
                                {
                                    objReceiptLineErrorModel.LineProcessErrMsgList.Add(er);
                                }
                                objReceiptErrorResponseModel.ReceiptLineErrorList.Add(objReceiptLineErrorModel);
                            }
                        }
                        var xErrdata = ReceiptErrorResponseList.FirstOrDefault(p => p.EXPOID.Equals(item.ExPOID));
                        if ((xErrdata == null) && (objReceiptErrorResponseModel.HdrErrMsgList.Count > 0 || objReceiptErrorResponseModel.ReceiptLineErrorList.Count > 0))
                        {
                            ReceiptErrorResponseList.Add(objReceiptErrorResponseModel);
                        }
                    }
                    //---delete header---
                    if (_rImpHdr.spErrCode != -100)
                    {
                        _rImpHdr.Delete(_userData.DatabaseKey);
                    }
                    else
                    {
                        //_rImpHdr.ErrorMessages.Add("PurchaseOrderId not found");
                        _rImpHdr.ErrorMessages.Add("Header insert error:PurchaseOrderId not found");
                        UpdateReceiptHdr(_rImpHdr, item, Convert.ToDateTime(DateTime.UtcNow), "-100", "Header insert error:PurchaseOrderId not found");
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
            }//end of foreach
            if (FailedProcess == 0)
            {
                logMessage = "Process Complete – All items processed successfully";
            }
            //---update log---
            CommonWrapper commonWrapper = new CommonWrapper(_userData);
            commonWrapper.UpdateLog(logId, TotalProcess, SuccessfulProcess, FailedProcess, logMessage, Common.Constants.ApiConstants.DeanFoodsPOReceiptImport);
            return ReceiptErrorResponseList;
        }

        #endregion

        #region Common

        private void InsertReceiptHdr(ReceiptImpHdr rImpHdr, ReceiptImportModel item)
        {
            rImpHdr.PONumber = item.PONumber;
            rImpHdr.EXPOID = item.ExPOID;
            rImpHdr.EXRecieptNo = item.ExReceiptNo;
            rImpHdr.ReceiptDate = (item.Receiptdate == null || item.Receiptdate == DateTime.MinValue ? Convert.ToDateTime(DateTime.UtcNow) : item.Receiptdate);
            rImpHdr.ErrorCodes = "";
            rImpHdr.ErrorMessage = "";
            rImpHdr.LastProcess = null;
            rImpHdr.CreatedBy = _dbKey.User.CallerUserName;
            rImpHdr.ReceiptStatus = item.Status; // V2-412
            rImpHdr.CreateCustom(_userData.DatabaseKey);
        }

        private void UpdateReceiptHdr(ReceiptImpHdr rImpHdr, ReceiptImportModel item, DateTime? LastProcess, string ErrorCodes = "", string ErrorMessage = "")
        {
            rImpHdr.PONumber = item.PONumber;
            rImpHdr.EXPOID = item.ExPOID;
            rImpHdr.EXRecieptNo = item.ExReceiptNo;
            rImpHdr.ReceiptDate = item.Receiptdate;
            rImpHdr.ErrorCodes = ErrorCodes;
            rImpHdr.ErrorMessage = ErrorMessage;
            rImpHdr.LastProcess = LastProcess;
            rImpHdr.CreatedBy = _dbKey.User.CallerUserName;
            rImpHdr.ReceiptStatus = item.Status;
            rImpHdr.UpdateCustom(_userData.DatabaseKey);
        }

        private void InsertReceiptLine(ReceiptImpLine rImpLine, ReceiptImportModel item, ReceiptLineImportModel litem)
        {
            rImpLine.ClientId = item.ClientId;
            rImpLine.EXReceiptId = item.ExReceiptId;
            rImpLine.EXPOLineID = litem.ExPOLineId;
            rImpLine.Quantity = litem.Quantity;
            rImpLine.UnitCost = litem.UnitCost;
            rImpLine.UnitOfMeasure = litem.UnitOfMeasure;
            rImpLine.ErrorCodes = "";
            rImpLine.ErrorMessage = "";
            rImpLine.LastProcess = null;
            rImpLine.ReceiptStatus = litem.Status;    // V2-412 - Receipt Status is actually in the header
            rImpLine.CreateCustom(_userData.DatabaseKey);
        }

        private void UpdateReceiptLine(ReceiptImpLine rImpLine, ReceiptImportModel item, ReceiptLineImportModel litem, DateTime? LastProcess, string ErrorCodes = "", string ErrorMessage = "")
        {
            rImpLine.EXReceiptId = item.ExReceiptId;
            rImpLine.EXReceiptTxnId = litem.ExReceiptTxnId;
            rImpLine.EXPOLineID = litem.ExPOLineId;
            rImpLine.Quantity = litem.Quantity;
            rImpLine.UnitCost = litem.UnitCost;
            rImpLine.UnitOfMeasure = litem.UnitOfMeasure;
            rImpLine.ErrorCodes = ErrorCodes;
            rImpLine.ErrorMessage = ErrorMessage;
            rImpLine.LastProcess = LastProcess;
            rImpLine.ReceiptStatus = litem.Status;    // V2-412 - Receipt Status is actually in the header
            rImpLine.UpdateCustom(_userData.DatabaseKey);
        }

        #endregion
    }


}