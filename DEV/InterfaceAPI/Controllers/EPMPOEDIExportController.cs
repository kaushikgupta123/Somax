using Common.Constants;
using InterfaceAPI.BusinessWrapper.Interface;
using InterfaceAPI.Models.EPMPOEDIExport;
using InterfaceAPI.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace InterfaceAPI.Controllers.EPMPOEDIExport
{
    public class EPMPOEDIExportController : BaseApiController
    {
        readonly IEPMPOEDIExportWrapper _EPMPOEDIExportWrapper;
        readonly List<IEPMPOEDIExportWrapper> _EPMPOEDIExportWrapperList;
        Int64 logId;
        public EPMPOEDIExportController(IEPMPOEDIExportWrapper EPMPOEDIExportWrapper)
        {
            _EPMPOEDIExportWrapper = EPMPOEDIExportWrapper;
        }

        [HttpPost]
        [Route("api/EPMPOEDIExportDelim/")]
        public HttpResponseMessage EPMPOEDIExport([FromBody] EPMPOEDIExportAPIParams aPIParams)
        {
            List<EPMPOEDIExportResponseModel> _EPMPOEDIExportResponseModelList = new List<EPMPOEDIExportResponseModel>();
            EPMPOEDIExportResponseModel model = new EPMPOEDIExportResponseModel();
            #region Authentication
            ClaimsPrincipal currentClaimsPrincipal = ClaimsPrincipal.Current;
            if (!_EPMPOEDIExportWrapper.CheckLoginSession(aPIParams.LoginSessionId))
            {
                model = new EPMPOEDIExportResponseModel();
                model.errMessage = ErrorMessageConstants.Authentication_Failed;
                _EPMPOEDIExportResponseModelList.Add(model);
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, ErrorMessageConstants.Authentication_Failed);
            }
            if (_EPMPOEDIExportWrapper.IsMaintenanceGoingOn())
            {
                model = new EPMPOEDIExportResponseModel();
                model.errMessage = ErrorMessageConstants.ServiceUnavailable; 
                _EPMPOEDIExportResponseModelList.Add(model);
                return Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, ErrorMessageConstants.ServiceUnavailable);
            }
            #endregion
            #region Interface Setup Properties Check 
            if (!_EPMPOEDIExportWrapper.CheckIsActiveInterface(ApiConstants.EPMEDIPOExport))
            {
                model = new EPMPOEDIExportResponseModel();
                model.errMessage = ErrorMessageConstants.The_Process_Is_Not_Enabled_For_The_Client;
                _EPMPOEDIExportResponseModelList.Add(model);
                return Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, ErrorMessageConstants.The_Process_Is_Not_Enabled_For_The_Client);
            }
            #endregion
            #region Initiate SFTP Values
            _EPMPOEDIExportWrapper.RetrieveInterfacePropValues(ApiConstants.EPMEDIPOExport);
            #endregion

            #region Extract Data from database
            var EXPModel = _EPMPOEDIExportWrapper.ConvertDataRowToModel(aPIParams.PurchaseOrderId);
            #endregion
            if (aPIParams.PurchaseOrderId > 0)
            { 
              #region Convert to data text file
              string localFilePath = string.Empty;
              localFilePath = _EPMPOEDIExportWrapper.ConvertToDataFile(EXPModel);
              model = new EPMPOEDIExportResponseModel();
                model.errMessage = ErrorMessageConstants.Process_Completed_Successfully; 
              _EPMPOEDIExportResponseModelList.Add(model);

              #endregion

              #region Export to SFTP

              string sftpPath = _EPMPOEDIExportWrapper.ExportToSFTP(localFilePath, FileTypeEnum.EPMEDIPOExport);
              model = new EPMPOEDIExportResponseModel();
                model.errMessage = ErrorMessageConstants.File_Exported_To_SFTP;
              _EPMPOEDIExportResponseModelList.Add(model);
              #endregion
            }

            var resp = new HttpResponseMessage
            {
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(_EPMPOEDIExportResponseModelList.Where(a => a.errMessage != "").Select(a => a.errMessage)), System.Text.Encoding.UTF8, "application/json"),
                StatusCode = HttpStatusCode.OK,
            };

            return resp;
        }


    }
}