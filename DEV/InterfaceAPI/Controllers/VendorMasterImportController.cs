using Business.Authentication;
using Common.Constants;
using Common.Enumerations;
using DataContracts;
using InterfaceAPI.ActionFilters;
using InterfaceAPI.BusinessWrapper.Interface;
using InterfaceAPI.Models.Vendor;
using System;
using System.Collections.Generic;
using InterfaceAPI.Models;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
//using System.Web.Mvc;
using System.IO;
using Newtonsoft.Json;

namespace InterfaceAPI.Controllers
{
    [EnableCors("*", "*", "*")]
    [CustomAuthorize]

    public class VendorMasterImportController : BaseApiController
    {
        readonly IVendorMasterImportWrapper _VMImportWrapper;
        public VendorMasterImportController(IVendorMasterImportWrapper VMImportWrapper)
        {
            _VMImportWrapper = VMImportWrapper;
        }

        /// <summary>
        /// All existing VendorMasterImport records for the current client are read into a data contract and processed.This includes newly added records as well as records that failed validation previously and have not been deleted
        /// </summary>
        /// 

        //public HttpResponseMessage Post([FromBody]List<VendorMasterImportModel> VendorMasterImportModel=null)
        //{
        //    #region Authentication
        //    ClaimsPrincipal currentClaimsPrincipal = ClaimsPrincipal.Current;
        //    if (!_VMImportWrapper.IsUserAuthentiCate(currentClaimsPrincipal))
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized");
        //    }

        //    if (_VMImportWrapper.IsMaintenanceGoingOn())
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, "ServiceUnavailable");
        //    }
        //    #endregion
        //    #region Interface Setup Properties Check 

        //    if (!_VMImportWrapper.CheckIsActiveInterface(ApiConstants.VendorMasterImport))
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, "The process is not enabled for the client.");
        //    }
        //    #endregion


        //    String FilePath = HttpContext.Current.Server.MapPath("~/JsonSchema/Vendor/");
            
        //    string[] allJson = Directory.GetFiles(FilePath, "*.json");
        //    List<VendorImportResponseModel> vmrList = new List<VendorImportResponseModel>();
        //    foreach (string file in allJson)
        //    {
        //        using (StreamReader r = new StreamReader(file))
        //        {
        //            string json = r.ReadToEnd();
        //            var vendorMasterImportModel = JsonConvert.DeserializeObject<List<VendorMasterImportModel>>(json);

        //            if (vendorMasterImportModel == null)
        //            {
        //                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Data is null.");
        //            }
        //            #region vendorimportmodel objects converted to data tables

        //            var result = _VMImportWrapper.InsertVendarMasterImportData(vendorMasterImportModel);

        //            #region create & update log
        //            var logid = _VMImportWrapper.CreateLog("vendorimport");
        //            if (logid != 0)
        //            {
        //                _VMImportWrapper.UpdateLog(logid, result.TotalProcess, result.SuccessfulProcess, result.FailedProcess, result.logMessage);
        //            }
        //            #endregion

        //            #endregion

        //            #region validate validation & process(import)

        //            var vResult = _VMImportWrapper.VendorMasterImportValidate(logid, vendorMasterImportModel);

        //            #endregion

        //            VendorImportResponseModel vendorImportResponseModel = new VendorImportResponseModel();

        //            if (vResult != null && vResult.Count > 0)
        //            {
        //                vendorImportResponseModel.status = "Failed";
        //                foreach(var vr in vResult)
        //                {
        //                    vendorImportResponseModel.errMessageList.Add(vr) ;
        //                }
        //                vmrList.Add(vendorImportResponseModel);
        //            }
        //            else
        //            {
        //                vendorImportResponseModel.status = "Success";
        //                vmrList.Add(vendorImportResponseModel);
        //            }
        //        }
        //    }

        //    var resp = new HttpResponseMessage
        //    {
        //        Content = new StringContent(JsonConvert.SerializeObject(vmrList),
        //                                     System.Text.Encoding.UTF8, "application/json")
        //    };

        //    //delete all json files from local directory
        //    _VMImportWrapper.DeleteJsonFiles("vendor");
        //    return resp;
        //}


        public HttpResponseMessage Post(List<VendorMasterImportModel> VendorMasterImportModel)
        {

            if (VendorMasterImportModel == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Data is null.");
            }

            #region Authentication
            ClaimsPrincipal currentClaimsPrincipal = ClaimsPrincipal.Current;
            if (!_VMImportWrapper.IsUserAuthentiCate(currentClaimsPrincipal))
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized");
            }

            if (_VMImportWrapper.IsMaintenanceGoingOn())
            {
                return Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, "ServiceUnavailable");
            }
            #endregion
            #region Interface Setup Properties Check 

            if (!_VMImportWrapper.CheckIsActiveInterface(ApiConstants.VendorMasterImport))
            {
                return Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, "The process is not enabled for the client.");
            }
            #endregion

            #region AccountImportModel Objects Converted to data tables

            var result = _VMImportWrapper.InsertVendarMasterImportData(VendorMasterImportModel);

            #region Create & Update Log
            var logId = _VMImportWrapper.CreateLog("vendorImport");
            if (logId != 0)
            {
                _VMImportWrapper.UpdateLog(logId, result.TotalProcess, result.SuccessfulProcess, result.FailedProcess, result.logMessage, Common.Constants.ApiConstants.DeanFoodsVendorMasterImport);
            }
            #endregion

            #endregion

            #region Validate Validation & Process(Import)
            // V2-416
            //var vResult = _VMImportWrapper.VendorMasterImportValidate(logId, VendorMasterImportModel);
            var vResult = _VMImportWrapper.VendorMasterImportValidate(logId);

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
                return Request.CreateResponse(HttpStatusCode.OK, "Vendor master import successful");
            }
        }







        /// <summary>
        /// All existing VendorMasterImport records for the current client are read into a data contract and processed.This includes newly added records as well as records that failed validation previously and have not been deleted
        /// </summary>
        /// 
        //[Route("api/VendorMaster/{VendorMasterImportModel}")]
        //public HttpResponseMessage Post([FromBody]List<VendorMasterImportModel> VendorMasterImportModel)
        //{

        //    if (VendorMasterImportModel == null)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Data is null.");
        //    }

        //    #region Authentication
        //    ClaimsPrincipal currentClaimsPrincipal = ClaimsPrincipal.Current;
        //    if (!_VMImportWrapper.IsUserAuthentiCate(currentClaimsPrincipal))
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized");
        //    }

        //    if (_VMImportWrapper.IsMaintenanceGoingOn())
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, "ServiceUnavailable");
        //    }
        //    #endregion
        //    #region Interface Setup Properties Check 

        //    if (!_VMImportWrapper.CheckIsActiveInterface(ApiConstants.VendorMasterImport))
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, "The process is not enabled for the client.");
        //    }
        //    #endregion

        //    #region AccountImportModel Objects Converted to data tables

        //    var result = _VMImportWrapper.InsertVendarMasterImportData(VendorMasterImportModel);

        //    #region Create & Update Log
        //    var logId = _VMImportWrapper.CreateLog("vendorImport");
        //    if (logId != 0)
        //    {
        //        _VMImportWrapper.UpdateLog(logId, result.TotalProcess, result.SuccessfulProcess, result.FailedProcess, result.logMessage);
        //    }
        //    #endregion

        //    #endregion

        //    #region Validate Validation & Process(Import)

        //    var vResult = _VMImportWrapper.VendorMasterImportValidate(logId, VendorMasterImportModel);

        //    #endregion

        //    if (vResult != null && vResult.Count > 0)
        //    {
        //        var resp = new HttpResponseMessage
        //        {
        //            Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(vResult), System.Text.Encoding.UTF8, "application/json"),
        //            StatusCode = HttpStatusCode.BadRequest,
        //        };
        //        return resp;
        //    }
        //    else
        //    {
        //        return Request.CreateResponse(HttpStatusCode.OK, "Vendor master import successful");
        //    }
        //}
    }
}
