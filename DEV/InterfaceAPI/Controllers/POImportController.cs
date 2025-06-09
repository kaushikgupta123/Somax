using Business.Authentication;
using Common.Constants;
using Common.Enumerations;
using DataContracts;
using InterfaceAPI.ActionFilters;
using InterfaceAPI.BusinessWrapper.Interface;
using InterfaceAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Cors;

namespace InterfaceAPI.Controllers
{
    [EnableCors("*", "*", "*")]
    [CustomAuthorize]

    public class POImportController : BaseApiController
    {
        readonly IPOImportWrapper _POImportWrapper;
        public POImportController(IPOImportWrapper POImportWrapper)
        {
            _POImportWrapper = POImportWrapper;
        }

        /// <summary>
        /// The PurchaseOrderImportModel objects are converted into POImpHdr and POImpLine data rows. 
        /// The PurchaseOrderImportModel objects have a header/detail structure where the header contains 
        /// the PO Header information and a list of detail objects contains the line item information.
        /// All header and line records for the Current Client are read into a list of header and line 
        /// data contracts. These are to be processed one at a time by a validation stored procedure 
        /// and if the record passes validation, the record is to be processed by a stored procedure.
        /// </summary>
        public HttpResponseMessage Post([FromBody]List<PurchaseOrderImportModel> PurchaseOrderImportModel)
        {

            if (PurchaseOrderImportModel == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Data is null.");
            }

            #region Authentication
            ClaimsPrincipal currentClaimsPrincipal = ClaimsPrincipal.Current;
            if (!_POImportWrapper.IsUserAuthentiCate(currentClaimsPrincipal))
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized");
            }

            if (_POImportWrapper.IsMaintenanceGoingOn())
            {
                return Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, "ServiceUnavailable");
            }
            #endregion
            #region Interface Setup Properties Check 
            if (!_POImportWrapper.CheckIsActiveInterface(ApiConstants.PurchaseOrderImport))
            {
                return Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, "The process is not enabled for the client.");
            }
            #endregion
            #region PurchaseOrderImportModel Objects Converted to POImpHdr and POImpLine data rows 
            var result = _POImportWrapper.CreatePOImportHeader(PurchaseOrderImportModel);
            #region Create & Update log
            var logId = _POImportWrapper.CreateLog("poImport");
            if (logId != 0)
            {
                _POImportWrapper.UpdateLog(logId, result.TotalProcess, result.SuccessfulProcess, result.FailedProcess, result.logMessage, Common.Constants.ApiConstants.DeanFoodsPOImport);
            }
            #endregion
            #endregion

            #region Validation & Process(Import)
            var vResult = _POImportWrapper.POImportValidate(logId, PurchaseOrderImportModel);
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
                return Request.CreateResponse(HttpStatusCode.OK, "PO Import is Successful.");
            }
        }
    }
}
