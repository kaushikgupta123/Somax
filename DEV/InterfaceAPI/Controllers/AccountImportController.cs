using Common.Constants;
using InterfaceAPI.ActionFilters;
using InterfaceAPI.BusinessWrapper.Interface;
using InterfaceAPI.Models.Account;
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
    [EnableCors("*", "*", "*")]
    [CustomAuthorize]
    public class AccountImportController : BaseApiController
    {
        readonly IAccountImportWrapper _AccountImportWrapper;
        public AccountImportController(IAccountImportWrapper AccountImportWrapper)
        {
            _AccountImportWrapper = AccountImportWrapper;
        }
        /// <summary>
        /// All AccountImport Records for the Current Client are read into a list of AccountImport data contracts. These are to be processed one at a time by a validation stored procedure.
        /// </summary>
        public HttpResponseMessage Post([FromBody]List<AccountImportModel> AccountImportModel)
        {
            if (AccountImportModel == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Data is null.");
            }
            #region Authentication
            ClaimsPrincipal currentClaimsPrincipal = ClaimsPrincipal.Current;
            if (!_AccountImportWrapper.IsUserAuthentiCate(currentClaimsPrincipal))
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized");
            }

            if (_AccountImportWrapper.IsMaintenanceGoingOn())
            {
                return Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, "ServiceUnavailable");
            }
            #endregion

            #region Interface Setup Properties Check 
            if (!_AccountImportWrapper.CheckIsActiveInterface(ApiConstants.AccountImport))
            {
                return Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, "The process is not enabled for the client.");
            }
            #endregion
            #region AccountImportModel Objects Converted to data tables 

            var result = _AccountImportWrapper.CreateAccountImport(AccountImportModel);

            #region Create & Update Log
            var logId = _AccountImportWrapper.CreateLog("accountImport");
            if (logId != 0)
            {
                _AccountImportWrapper.UpdateLog(logId, result.TotalProcess, result.SuccessfulProcess, result.FailedProcess, result.logMessage, Common.Constants.ApiConstants.DeanFoodsAccountImport);
            }
            #endregion

            #endregion

            #region Validation & Process(Import)
            var vResult = _AccountImportWrapper.AccountImportValidate(logId, AccountImportModel);
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
                return Request.CreateResponse(HttpStatusCode.OK, "Account Import Successful");
            }
        }
    }
}