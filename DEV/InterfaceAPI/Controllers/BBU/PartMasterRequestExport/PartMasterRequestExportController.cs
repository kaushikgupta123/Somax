using Common.Constants;
using InterfaceAPI.ActionFilters;
using InterfaceAPI.BusinessWrapper.Implementation;
using InterfaceAPI.BusinessWrapper.Implementation.BBU;
using InterfaceAPI.BusinessWrapper.Interface.BBU;
using InterfaceAPI.Models.BBU.PartMasterRequestExport;
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

namespace InterfaceAPI.Controllers.BBU.PartMasterRequestExport
{
    //[EnableCors("*", "*", "*")]
    //[CustomAuthorize]
    public class PartMasterRequestExportController : BaseApiController
    {
        readonly IPartMasterRequestExportWrapper _PartMasterRequestExportWrapper;
        readonly List<IPartMasterRequestExportWrapper> _PartMasterRequestWrapperList;
        string ResponseMessage { get; set; }
        Int64 logId;
        public PartMasterRequestExportController(IPartMasterRequestExportWrapper PartMasterRequestExportWrapper)
        {
            _PartMasterRequestExportWrapper = PartMasterRequestExportWrapper;
        }
        
        [HttpPost]
        public HttpResponseMessage PartMasterRequestExport([FromBody]SchedulerAPICredentials credentials)
        {           
            List<PartMasterRequestExportResponseModel> _partMasterRequestExportResponseModelList = new List<PartMasterRequestExportResponseModel>();
            PartMasterRequestExportResponseModel model = new PartMasterRequestExportResponseModel();
            #region Authentication
            ClaimsPrincipal currentClaimsPrincipal = ClaimsPrincipal.Current;
            if (!_PartMasterRequestExportWrapper.CheckAuthentication(credentials.UserName, credentials.Password))
            {
                model = new PartMasterRequestExportResponseModel();
                model.errMessage="Authentication Failed";
                _partMasterRequestExportResponseModelList.Add(model);
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Authentication Failed");
            }
            if (_PartMasterRequestExportWrapper.IsMaintenanceGoingOn())
            {
                model = new PartMasterRequestExportResponseModel();
                model.errMessage = "ServiceUnavailable";
                _partMasterRequestExportResponseModelList.Add(model);
                return Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, "ServiceUnavailable");
            }
            #endregion
            #region Interface Setup Properties Check 
            if (!_PartMasterRequestExportWrapper.CheckIsActiveInterface(ApiConstants.PartMasterRequestExport))
            {
                model = new PartMasterRequestExportResponseModel();
                model.errMessage = "The process is not enabled for the client";
                _partMasterRequestExportResponseModelList.Add(model);
                return Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, "The process is not enabled for the client.");
            }
            #endregion
            #region Initiate SFTP Values
            _PartMasterRequestExportWrapper.RetrieveInterfacePropValues(ApiConstants.PartMasterRequestExport);
            #endregion

            #region Create Export Log
            
            logId = _PartMasterRequestExportWrapper.ExportLog(ApiConstants.PartMasterRequestExport,0,"",0);
            if(logId>0)
            {
                model = new PartMasterRequestExportResponseModel();
                model.errMessage = "Log record added";
                _partMasterRequestExportResponseModelList.Add(model);
            }
            #endregion

            #region Extract Data from database
            List <PartMasterRequestExportModel> _pmrEXPModel = new List<PartMasterRequestExportModel>();
            _pmrEXPModel = _PartMasterRequestExportWrapper.ConvertDataRowToModel(logId);
            #endregion

            #region Convert to data text file
            string localFilePath = string.Empty;
            int rowsExtracted = 0;
            localFilePath = _PartMasterRequestExportWrapper.ConvertToDataFile(_pmrEXPModel, out rowsExtracted);
            model = new PartMasterRequestExportResponseModel();
            model.errMessage = "Process Completed Successfully, Total Rows Extracted : " + rowsExtracted;
            _partMasterRequestExportResponseModelList.Add(model);

            #endregion

            #region Export to SFTP

            string sftpPath =_PartMasterRequestExportWrapper.ExportToSFTP(localFilePath,FileTypeEnum.PartMasterRequestExport);
            model = new PartMasterRequestExportResponseModel();
            model.errMessage = "File exported to SFTP";
            _partMasterRequestExportResponseModelList.Add(model);
            #endregion

            #region Update Export Log
            logId = _PartMasterRequestExportWrapper.ExportLog(ApiConstants.PartMasterRequestExport, logId, "Process Completed Successfully", PartMasterRequestExportWrapper.rowsProcessed);
            model = new PartMasterRequestExportResponseModel();
            model.errMessage = "Log record updated";
            _partMasterRequestExportResponseModelList.Add(model);
            #endregion

            #region Send Mail
            if (SFTPCred.isMailSend==true)
            {
                model = new PartMasterRequestExportResponseModel();
                _PartMasterRequestExportWrapper.SendEMail(credentials.UserName, "Process Completed Successfully, Total Rows Extracted : " + rowsExtracted);
                model.errMessage = "Mail sent to user";
                _partMasterRequestExportResponseModelList.Add(model);
            }

            #endregion

            var resp = new HttpResponseMessage
            {
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(_partMasterRequestExportResponseModelList.Where(a=>a.errMessage!="").Select(a=>a.errMessage)), System.Text.Encoding.UTF8, "application/json"),
                StatusCode = HttpStatusCode.OK,
            };
            
            return resp;
        }

       
    }
}