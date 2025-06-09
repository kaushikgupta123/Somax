using Client.Controllers.Common;

using Newtonsoft.Json;
using Client.Models.VendorRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Constants;
using Client.BusinessWrapper;
using Client.BusinessWrapper.Common;
using Client.Common;
using Client.ActionFilters;

namespace Client.Controllers.VendorRequest
{
    public class VendorRequestController : SomaxBaseController
    {
        // GET: VendorRequest
        #region Search
        [CheckUserSecurity(securityType = SecurityConstants.Vendor_Create_Vendor_Request)]
        public ActionResult Index()
        {
            VendorRequestVM VendorReqVM = new VendorRequestVM();
            VendorReqVM.security = this.userData.Security;
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            VendorReqVM.VendorRequestViewList= commonWrapper.PopulateCustomQueryDisplay(AttachmentTableConstant.VendorRequest);
            CommonWrapper cwrapper = new CommonWrapper(userData);
            var AllLookUpLists = cwrapper.GetAllLookUpList();
            if (AllLookUpLists != null)
            {
                List<DataContracts.LookupList> objLookupType = AllLookUpLists.Where(x => x.ListName == LookupListConstants.VENDOR_TYPE).ToList();
                if (objLookupType != null)
                {
                    VendorReqVM.LookupTypeList = objLookupType.Select(x => new SelectListItem { Text = x.ListValue + "  -  " + x.Description, Value = x.ListValue });
                }
            }

            LocalizeControls(VendorReqVM, LocalizeResourceSetConstants.Global);
            return View("~/Views/VendorRequest/Index.cshtml", VendorReqVM);
        }
        [HttpPost]
        public string GetVendorRequestchunkGrid(int? draw, int? start, int length = 0, string _name = null, string _addresscity = null, string _addressstate = null, string _type = null, string _status = null,  int customDisplay = 1, string srcData = ""
                                 , string Order = "1")
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var colname = Request.Form.GetValues("columns[" + order + "][name]");
            VendorRequestWrapper sWrapper = new VendorRequestWrapper(userData);
             var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            int skip = start * length ?? 0;
            var VendorRequestchunkDataList = sWrapper.GetVendorRequestchunkData(order, orderDir, skip, length, customDisplay, _name, _addresscity, _addressstate, _type, _status, srcData);

            if (VendorRequestchunkDataList != null && VendorRequestchunkDataList.Count > 0)
            {
                recordsFiltered = VendorRequestchunkDataList[0].TotalCount;
                totalRecords = VendorRequestchunkDataList[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }
            int initialPage = start.Value;
            var filteredResult = VendorRequestchunkDataList
                .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult}, JsonSerializerDateSettings);
        }
        #endregion
        #region Print
        [HttpGet]
        public string GetVendorRequestPrintData(string _name = null, string _addresscity = null, string _addressstate = null, string _type = null, string _status = null, int customDisplay = 1, string srcData = "",string colname = "0", string coldir = "asc")
        {
            
            VendorRequestWrapper sWrapper = new VendorRequestWrapper(userData);
            var VendorRequestchunkDataList = sWrapper.GetVendorRequestPrintData(colname, coldir, 0, 100000, customDisplay, _name, _addresscity, _addressstate, _type, _status, srcData);

            return JsonConvert.SerializeObject(new { data = VendorRequestchunkDataList }, JsonSerializerDateSettings);
        }

        #endregion
        #region Add/Edit
        [HttpGet]
        public PartialViewResult AddVendorRequest()
        {
            VendorRequestWrapper sWrapper = new VendorRequestWrapper(userData);
            VendorRequestVM VRVM = new VendorRequestVM();
            VendorRequestModel vendorRequest = new VendorRequestModel();
            sWrapper.PopulateDropdownControls(vendorRequest);
            VRVM.vendorRequestModel = vendorRequest;
            LocalizeControls(VRVM, LocalizeResourceSetConstants.Global);
            return PartialView("~/Views/VendorRequest/_AddOrEditVendorRequest.cshtml", VRVM);
        }
        [HttpGet]
        public PartialViewResult EditVendorRequest(long VendorRequestID)
        {
            VendorRequestWrapper sWrapper = new VendorRequestWrapper(userData);
            VendorRequestVM VRVM = new VendorRequestVM();
            VRVM.vendorRequestModel = sWrapper.GetVendorRequestDetailsByVendorID(VendorRequestID);
            sWrapper.PopulateDropdownControls(VRVM.vendorRequestModel);
            LocalizeControls(VRVM, LocalizeResourceSetConstants.Global);
            return PartialView("~/Views/VendorRequest/_AddOrEditVendorRequest.cshtml", VRVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VendorRequestAddOrEdit(VendorRequestVM objVM, string Command)
        {
            if (ModelState.IsValid)
            {
                VendorRequestWrapper sWrapper = new VendorRequestWrapper(userData);
                string Mode = string.Empty;

                List<String> errorList = sWrapper.AddOrEditVendorRequestRecord(objVM.vendorRequestModel);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (objVM.vendorRequestModel.VendorRequestId == 0)
                    {
                        Mode = "add";
                    }
                    return Json(new { Result = JsonReturnEnum.success.ToString(), mode = Mode, Command = Command }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteVendorRequest(long VendorRequestID)
        {
            VendorRequestWrapper sWrapper = new VendorRequestWrapper(userData);
            List<String> errorList = sWrapper.DeleteVendorRequest(VendorRequestID);
            if (errorList != null && errorList.Count > 0)
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);

            }
        }
        #endregion
        #region Approval/Deny
        [HttpPost]
        public ActionResult ApprovalVendorRequest(long VendorRequestID)
        {
            VendorRequestWrapper sWrapper = new VendorRequestWrapper(userData);
            List<String> errorList = sWrapper.ApprovalVendorRequest(VendorRequestID);
            if (errorList != null && errorList.Count > 0)
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);

            }
        }
        [HttpPost]
        public ActionResult DenyVendorRequest(long VendorRequestID)
        {
            VendorRequestWrapper sWrapper = new VendorRequestWrapper(userData);
            List<String> errorList = sWrapper.DenyVendorRequest(VendorRequestID);
            if (errorList != null && errorList.Count > 0)
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);

            }
        }
        #endregion
    }
}