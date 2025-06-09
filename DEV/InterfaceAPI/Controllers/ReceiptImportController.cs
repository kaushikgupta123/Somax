using Common.Constants;
using InterfaceAPI.ActionFilters;
using InterfaceAPI.BusinessWrapper.Interface;
using InterfaceAPI.Models;
using InterfaceAPI.Models.PurchaseOrderReceipt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace InterfaceAPI.Controllers
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
  [EnableCors("*", "*", "*")]
    [CustomAuthorize]
    public class ReceiptImportController : BaseApiController
    {
        readonly IReceiptImportWrapper _ReceiptImportWrapper;
        public ReceiptImportController(IReceiptImportWrapper ReceiptImportWrapper)
        {
            _ReceiptImportWrapper = ReceiptImportWrapper;
        }

        /// <summary>
        /// All ReceiptImpHdr and ReceiptImpLine Records for the Current Client are read into a list of ReceiptImpHdr and ReceiptImpLine data contracts. These are to be processed one at a time by a validation stored procedure. If the record passes validation, the record is to be processed by a stored procedure
        /// </summary>
        public HttpResponseMessage Post([FromBody]List<ReceiptImportModel> ReceiptImportModel)
        {
            if (ReceiptImportModel == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Data is null.");
            }

            #region Authentication
            ClaimsPrincipal currentClaimsPrincipal = ClaimsPrincipal.Current;
            if (!_ReceiptImportWrapper.IsUserAuthentiCate(currentClaimsPrincipal))
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized");
            }

            if (_ReceiptImportWrapper.IsMaintenanceGoingOn())
            {
                return Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, "ServiceUnavailable");
            }
            #endregion

            #region Interface Setup Properties Check 
            if (!_ReceiptImportWrapper.CheckIsActiveInterface(ApiConstants.ReceiptImport))
            {
                return Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, "The process is not enabled for the client.");
            }
            #endregion
            #region Status from Header to Line Item 
            // V2-412
            // The header contains the status information 
            // However, we need to use it at the line item level
            // Loop through the line items and post the status to each line item
            foreach(ReceiptImportModel imp in ReceiptImportModel)
            {
              foreach(ReceiptLineImportModel line in imp.ReceiptItems)
              {
                line.Status = imp.Status;
              }
            }
            #endregion

            #region Insert/Update ReceiptImpHdr & ReceiptImpLine tables
            var result = _ReceiptImportWrapper.InsertReceiptImpHdr(ReceiptImportModel);
            #endregion

            #region Create & Update log
            var logId = _ReceiptImportWrapper.CreateLog("poReceipt");
            if (logId != 0)
            {
                _ReceiptImportWrapper.UpdateLog(logId, result.TotalProcess, result.SuccessfulProcess, result.FailedProcess, result.logMessage, Common.Constants.ApiConstants.DeanFoodsPOReceiptImport);
            }
            #endregion
            #region Validation & Process(Import)
            var vResult = _ReceiptImportWrapper.ReceiptsImportValidate(logId, ReceiptImportModel);
            #endregion

            if (vResult != null && vResult.Count > 0)
            {
                var resp = new HttpResponseMessage
                {
                    Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(vResult), System.Text.Encoding.UTF8, "application/json"),
                    StatusCode = HttpStatusCode.BadRequest,
                };
                return resp;
            }
            else
            {
                ReceiptResponseModel receipt_response = new ReceiptResponseModel()
                {
                  response_message = "Receipt Import Successful",
                  receipts = ReceiptImportModel
                };
                HttpResponseMessage resp = new HttpResponseMessage
                {
                  StatusCode = HttpStatusCode.OK,
                  Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(receipt_response),System.Text.Encoding.UTF8,"application/json")
                };
                return resp;
                //return Request.CreateResponse(HttpStatusCode.OK, "Receipt Import Successful");
            }
        }
    }
}