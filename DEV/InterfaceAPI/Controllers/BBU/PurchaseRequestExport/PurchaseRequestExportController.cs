using Common.Constants;
using InterfaceAPI.ActionFilters;
using InterfaceAPI.BusinessWrapper.Implementation;
using InterfaceAPI.BusinessWrapper.Implementation.BBU;
using InterfaceAPI.BusinessWrapper.Interface.BBU;
using InterfaceAPI.Models.BBU.PurchaseRequestExport;
using InterfaceAPI.Models.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace InterfaceAPI.Controllers.BBU.PurchaseRequestExport
{
    //[EnableCors("*", "*", "*")]
    //[CustomAuthorize]
    public class PurchaseRequestExportController : BaseApiController
    {
        readonly IPurchaseRequestExportWrapper _PurchaseRequestExportWrapper;
        readonly List<IPurchaseRequestExportWrapper> _PurchaseRequestExportWrapperList;
        string ResponseMessage { get; set; }
        Int64 logId;
        public PurchaseRequestExportController(IPurchaseRequestExportWrapper purchaseRequestExportWrapper)
        {
            _PurchaseRequestExportWrapper = purchaseRequestExportWrapper;
        }

        [HttpPost]
        [Route("api/PurchaseRequestExportDelim/")]
        public HttpResponseMessage PurchaseRequestExport([FromBody]SchedulerAPICredentials credentials)
        {
            List<PurchaseRequestExportResponseModel> _PurchaseRequestExportResponseModelList = new List<PurchaseRequestExportResponseModel>();
            PurchaseRequestExportResponseModel model = new PurchaseRequestExportResponseModel();
            #region Authentication
            ClaimsPrincipal currentClaimsPrincipal = ClaimsPrincipal.Current;
            if (!_PurchaseRequestExportWrapper.CheckAuthentication(credentials.UserName, credentials.Password))
            {
                model = new PurchaseRequestExportResponseModel();
                model.errMessage = "Authentication Failed";
                _PurchaseRequestExportResponseModelList.Add(model);
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Authentication Failed");
            }
            if (_PurchaseRequestExportWrapper.IsMaintenanceGoingOn())
            {
                model = new PurchaseRequestExportResponseModel();
                model.errMessage = "ServiceUnavailable";
                _PurchaseRequestExportResponseModelList.Add(model);
                return Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, "ServiceUnavailable");
            }
            #endregion
            #region Interface Setup Properties Check 
            if (!_PurchaseRequestExportWrapper.CheckIsActiveInterface(ApiConstants.OraclePurchaseRequestExport))
            {
                model = new PurchaseRequestExportResponseModel();
                model.errMessage = "The process is not enabled for the client";
                _PurchaseRequestExportResponseModelList.Add(model);
                return Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, "The process is not enabled for the client.");
            }
            #endregion
            #region Initiate SFTP Values
            _PurchaseRequestExportWrapper.RetrieveInterfacePropValues(ApiConstants.OraclePurchaseRequestExport);
            #endregion

            #region Create Export Log

            logId = _PurchaseRequestExportWrapper.ExportLog(ApiConstants.OraclePurchaseRequestExport, 0, "", 0);
            if (logId > 0)
            {
                model = new PurchaseRequestExportResponseModel();
                model.errMessage = "Log record added";
                _PurchaseRequestExportResponseModelList.Add(model);
            }
            #endregion

            #region Extract Data from database
            List<PurchaseRequestExportModel> _pmrEXPModel = new List<PurchaseRequestExportModel>();
            _pmrEXPModel = _PurchaseRequestExportWrapper.ConvertDataRowToModel(logId);
            #endregion
            int rowsExtracted = 0;
            if (_pmrEXPModel.Count > 0)
            { 
              #region Convert to data text file
              string localFilePath = string.Empty;
              localFilePath = _PurchaseRequestExportWrapper.ConvertToDataFile(_pmrEXPModel, out rowsExtracted);
              model = new PurchaseRequestExportResponseModel();
              model.errMessage = "Process Completed Successfully, Total Rows Extracted : " + rowsExtracted;
              _PurchaseRequestExportResponseModelList.Add(model);

              #endregion

              #region Export to SFTP

              string sftpPath = _PurchaseRequestExportWrapper.ExportToSFTP(localFilePath, FileTypeEnum.OraclePurchaseRequestExport);
              model = new PurchaseRequestExportResponseModel();
              model.errMessage = "File exported to SFTP";
              _PurchaseRequestExportResponseModelList.Add(model);
              #endregion
            }
            #region Update Export Log
            logId = _PurchaseRequestExportWrapper.ExportLog(ApiConstants.OraclePurchaseRequestExport, logId, "Process Completed Successfully", PurchaseRequestExportWrapper.rowsProcessed);
            model = new PurchaseRequestExportResponseModel();
            model.errMessage = "Log record updated";
            _PurchaseRequestExportResponseModelList.Add(model);
            #endregion

            #region Send Mail
            if (SFTPCred.isMailSend == true)
            {
                model = new PurchaseRequestExportResponseModel();
                _PurchaseRequestExportWrapper.SendEMail(credentials.UserName, "Process Completed Successfully, Total Rows Extracted : " + rowsExtracted);
                model.errMessage = "Mail sent to user";
                _PurchaseRequestExportResponseModelList.Add(model);
            }

            #endregion

            var resp = new HttpResponseMessage
            {
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(_PurchaseRequestExportResponseModelList.Where(a => a.errMessage != "").Select(a => a.errMessage)), System.Text.Encoding.UTF8, "application/json"),
                StatusCode = HttpStatusCode.OK,
            };

            return resp;
        }


    }
}