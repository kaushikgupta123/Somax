using Client.ActionFilters;
using Client.BusinessWrapper.Common;
using Client.BusinessWrapper.Configuration.VendorMaster;
using Client.Common;
using Client.Controllers.Common;
using Client.Models.Configuration.VendorMaster;
using Common.Constants;
using DataContracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Controllers.Configuration.VendorMaster
{
    public class VendorMasterController : ConfigBaseController
    {
        #region Search
        [CheckUserSecurity(securityType = SecurityConstants.VendorMaster)]
        public ActionResult Index()
        {
            VendorMasterVM vendorMasterVM = new VendorMasterVM();
            vendorMasterVM.security = this.userData.Security;
            LocalizeControls(vendorMasterVM, LocalizeResourceSetConstants.ConfigMasterDetail);
          
            var ExternalList = UtilityFunction.IsExternalTypesWithBoolValue();
            vendorMasterVM.vendorMasterModel = new VendorMasterModel();
            if (ExternalList != null)
            {
                vendorMasterVM.vendorMasterModel.ExternalTypeList = ExternalList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            return View("~/Views/Configuration/VendorMaster/index.cshtml", vendorMasterVM);
        }
        [HttpPost]
        public string GetVendorMasterGrid(int? draw, int? start, int? length, string clientLookupId = "", string name = "", string exVendorSiteCode = "", string addressCity = "", string addressState = "", string type = "", bool inactiveFlag = false, string IsExternal = "")
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var colname = Request.Form.GetValues("columns[" + order + "][name]");
            VendorMasterWrapper vmWrapper = new VendorMasterWrapper(userData);
            var VendorMasterList = vmWrapper.RetrieveAllMasterList(inactiveFlag);
            if (VendorMasterList != null)
            {
                VendorMasterList = GetVendorMasterSearchResult(VendorMasterList, clientLookupId, name, exVendorSiteCode, addressCity, addressState, type, IsExternal);
                VendorMasterList = this.GetVendorMastGridSortByColumnWithOrder(colname[0], orderDir, VendorMasterList);
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = VendorMasterList.Count();
            totalRecords = VendorMasterList.Count();
            int initialPage = start.Value;
            var filteredResult = VendorMasterList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            #region uiconfig

            CommonWrapper cWrapper = new CommonWrapper(userData);
            //var hiddenList = cWrapper.UiConfigAllColumnsCustom(UiConfigConstants.VendorMasterSearch, UiConfigConstants.IsHideTrue, UiConfigConstants.IsRequiredNone, UiConfigConstants.IsExternalNone, UiConfigConstants.TargetView).Select(x => x.ColumnName).ToList();
            var hiddenList = cWrapper.GetHiddenList(UiConfigConstants.VendorMasterSearch).Select(x => x.ColumnName).ToList(); ;

            #endregion

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, hiddenColumnList = hiddenList }, JsonSerializerDateSettings);
           

        }

        private List<VendorMasterModel> GetVendorMasterSearchResult(List<VendorMasterModel> VendorMasterModelList, string clientLookupId, string name, string exVendorSiteCode, string addressCity, string addressState, string type, string IsExternal = "")
        {
            if (VendorMasterModelList != null)
            {
                if (!string.IsNullOrEmpty(clientLookupId))
                {
                    clientLookupId = clientLookupId.ToUpper();
                    VendorMasterModelList = VendorMasterModelList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookupId) && x.ClientLookupId.ToUpper().Contains(clientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(name))
                {
                    name = name.ToUpper();
                    VendorMasterModelList = VendorMasterModelList.Where(x => (!string.IsNullOrWhiteSpace(x.Name) && x.Name.ToUpper().Contains(name))).ToList();
                }
                if (!string.IsNullOrEmpty(exVendorSiteCode))
                {
                    exVendorSiteCode = exVendorSiteCode.ToUpper();
                    VendorMasterModelList = VendorMasterModelList.Where(x => (!string.IsNullOrWhiteSpace(x.ExVendorSiteCode) && x.ExVendorSiteCode.ToUpper().Contains(exVendorSiteCode))).ToList();
                }
                if (!string.IsNullOrEmpty(addressCity))
                {
                    addressCity = addressCity.ToUpper();
                    VendorMasterModelList = VendorMasterModelList.Where(x => (!string.IsNullOrWhiteSpace(x.AddressCity) && x.AddressCity.ToUpper().Contains(addressCity))).ToList();
                }
                if (!string.IsNullOrEmpty(addressState))
                {
                    addressState = addressState.ToUpper();
                    VendorMasterModelList = VendorMasterModelList.Where(x => (!string.IsNullOrWhiteSpace(x.AddressState) && x.AddressState.ToUpper().Contains(addressState))).ToList();
                }
                if (!string.IsNullOrEmpty(type))
                {
                    type = type.ToUpper();
                    VendorMasterModelList = VendorMasterModelList.Where(x => (!string.IsNullOrWhiteSpace(x.Type) && x.Type.ToUpper().Contains(type))).ToList();
                }
                if (!string.IsNullOrEmpty(IsExternal))
                {
                    bool flagVal = false;
                    if (IsExternal.Equals("true"))
                    {
                        flagVal = true;
                    }
                    else if (IsExternal.Equals("false"))
                    {
                        flagVal = false;
                    }
                    VendorMasterModelList = VendorMasterModelList.Where(x => x.IsExternal == flagVal).ToList();
                }
            }
            return VendorMasterModelList;
        }
        private List<VendorMasterModel> GetVendorMastGridSortByColumnWithOrder(string order, string orderDir, List<VendorMasterModel> data)
        {
            List<VendorMasterModel> lst = new List<VendorMasterModel>();
            if (data != null)
            {
                switch (order)
                {
                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Name).ToList() : data.OrderBy(p => p.Name).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ExVendorSiteCode).ToList() : data.OrderBy(p => p.ExVendorSiteCode).ToList();
                        break;
                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.AddressCity).ToList() : data.OrderBy(p => p.AddressCity).ToList();
                        break;
                    case "4":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.AddressState).ToList() : data.OrderBy(p => p.AddressState).ToList();
                        break;
                    case "5":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Type).ToList() : data.OrderBy(p => p.Type).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                        break;
                }
            }
            return lst;
        }

        public string GetVendorMasterPrintData(string colname, string coldir, string clientLookupId = "", string name = "", string exVendorSiteCode = "", string addressCity = "", string addressState = "", string type = "", bool inactiveFlag = false, string IsExternal = "")
        {
            VendorMasterPrintModel vendorMasterPrintModel;
            List<VendorMasterPrintModel> vendorMasterPrintModelList = new List<VendorMasterPrintModel>();

            VendorMasterWrapper vmWrapper = new VendorMasterWrapper(userData);
            var VendorMasterList = vmWrapper.RetrieveAllMasterList(inactiveFlag);
            if (VendorMasterList != null)
            {
                VendorMasterList = GetVendorMasterSearchResult(VendorMasterList, clientLookupId, name, exVendorSiteCode, addressCity, addressState, type, IsExternal);
                VendorMasterList = this.GetVendorMastGridSortByColumnWithOrder(colname, coldir, VendorMasterList);
            }
            foreach (var item in VendorMasterList)
            {
                vendorMasterPrintModel = new VendorMasterPrintModel();
                vendorMasterPrintModel.ClientLookupId = item.ClientLookupId;
                vendorMasterPrintModel.Name = item.Name;
                vendorMasterPrintModel.ExVendorSiteCode = item.ExVendorSiteCode;
                vendorMasterPrintModel.AddressCity = item.AddressCity;
                vendorMasterPrintModel.AddressState = item.AddressState;
                vendorMasterPrintModel.Type = item.Type;
                vendorMasterPrintModel.IsExternal = item.IsExternal;
                vendorMasterPrintModelList.Add(vendorMasterPrintModel);
            }
            return JsonConvert.SerializeObject(new { data = vendorMasterPrintModelList }, JsonSerializerDateSettings);
        }
        #endregion

        #region Details
        public PartialViewResult VendorMasterDetails(long vendorMasterId)
        {
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            var AllLookUps = commonWrapper.GetAllLookUpList();
            VendorMasterVM vendorMasterVM = new VendorMasterVM();
            vendorMasterVM.changeVendorModel = new ChangeVendorModel();
            VendorMasterWrapper vmWrapper = new VendorMasterWrapper(userData);
            VendorMasterModel vMasterModel = new VendorMasterModel();
            vMasterModel = vmWrapper.GetVendorMasterDetails(vendorMasterId);
            #region Dropdown Desc Set

            if (AllLookUps != null)
            {
                List<DataContracts.LookupList> Terms = new List<DataContracts.LookupList>();
                Terms = AllLookUps.Where(x => x.ListName == LookupListConstants.VENDOR_TERMS).ToList();// woWrapper.GetTypeList();
                if (Terms != null && Terms.Any(cus => cus.ListValue == vMasterModel.Terms))
                {
                    vMasterModel.Terms = Terms.Where(x => x.ListValue == vMasterModel.Terms).Select(x => x.ListValue + ' ' + '|' + ' ' + x.Description).First();
                }

                List<DataContracts.LookupList> Type = new List<DataContracts.LookupList>();
                Type = AllLookUps.Where(x => x.ListName == LookupListConstants.VENDOR_TYPE).ToList();// woWrapper.GetTypeList();
                if (Type != null && Type.Any(cus => cus.ListValue == vMasterModel.Type))
                {
                    vMasterModel.Type = Type.Where(x => x.ListValue == vMasterModel.Type).Select(x => x.ListValue + ' ' + '|' + ' ' + x.Description).First();
                }

                List<DataContracts.LookupList> FobCode = new List<DataContracts.LookupList>();
                FobCode = AllLookUps.Where(x => x.ListName == LookupListConstants.VENDOR_FOBCODE).ToList();// woWrapper.GetTypeList();
                if (FobCode != null && FobCode.Any(cus => cus.ListValue == vMasterModel.FOBCode))
                {
                    vMasterModel.FOBCode = FobCode.Where(x => x.ListValue == vMasterModel.FOBCode).Select(x => x.ListValue + ' ' + '|' + ' ' + x.Description).First();
                }
            }
            #endregion

            vendorMasterVM.vendorMasterModel = vMasterModel;
            vendorMasterVM.changeVendorModel.oldClientLookupId = vMasterModel.ClientLookupId;
            vendorMasterVM.changeVendorModel.UpdateIndex = vMasterModel.UpdateIndex;
            vendorMasterVM.changeVendorModel.VendorMasterId = vendorMasterId;
            vendorMasterVM.security = this.userData.Security;
            LocalizeControls(vendorMasterVM, LocalizeResourceSetConstants.ConfigMasterDetail);

            #region uiconfig
            CommonWrapper cWrapper = new CommonWrapper(userData);
            string isExternal = "";
            if (vMasterModel.IsExternal)
            {
                isExternal = UiConfigConstants.IsExternalTrue;
            }
            else
            {
                isExternal = UiConfigConstants.IsExternalFalse;
            }
            //vendorMasterVM.hiddenColumnList = cWrapper.UiConfigAllColumnsCustom(UiConfigConstants.VendorMasterDetail, UiConfigConstants.IsHideTrue, UiConfigConstants.IsRequiredNone, isExternal, UiConfigConstants.TargetView).Select(x => x.ColumnName).ToList();
            vendorMasterVM.hiddenColumnList = cWrapper.GetHiddenList(UiConfigConstants.VendorMasterDetail, isExternal).Select(x => x.ColumnName).ToList(); ;
            #endregion

            return PartialView("~/Views/Configuration/VendorMaster/_VendorMasterDetails.cshtml", vendorMasterVM);
        }
        #endregion Details

        #region Add/Edit
        public ActionResult AddVendorMaster()
        {
            VendorMasterVM vendorMasterVM = new VendorMasterVM();
            LocalizeControls(vendorMasterVM, LocalizeResourceSetConstants.ConfigMasterDetail);
            return PartialView("~/Views/Configuration/VendorMaster/_AddVendorMaster.cshtml", vendorMasterVM);
        }
        public ActionResult EditVendorMaster(long vendorMasterId)
        {
            VendorMasterVM vendorMasterVM = new VendorMasterVM();
            VendorMasterWrapper vmWrapper = new VendorMasterWrapper(userData);
            VendorMasterModel vMasterModel = new VendorMasterModel();

            CommonWrapper commonWrapper = new CommonWrapper(userData);
            vMasterModel = vmWrapper.GetVendorMasterDetails(vendorMasterId);
            vMasterModel.VendorMasterId = vendorMasterId;
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                var TermList = AllLookUps.Where(x => x.ListName == LookupListConstants.VENDOR_TERMS).ToList(); //woWrapper.GetCancelReasonWo();
                if (TermList != null)
                {
                    vMasterModel.TermsList = TermList.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
                }
                var FobCodeList = AllLookUps.Where(x => x.ListName == LookupListConstants.VENDOR_FOBCODE).ToList(); //woWrapper.GetCancelReasonWo();
                if (FobCodeList != null)
                {
                    vMasterModel.FobCodeList = FobCodeList.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
                }
                var TypeList = AllLookUps.Where(x => x.ListName == LookupListConstants.VENDOR_TYPE).ToList(); //woWrapper.GetCancelReasonWo();
                if (TypeList != null)
                {
                    vMasterModel.TypeList = TypeList.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
                }
            }
            vMasterModel.ViewName = UiConfigConstants.VendorMasterEdit;  //--V2-375--uiconfig//
            vendorMasterVM.vendorMasterModel = vMasterModel;
            LocalizeControls(vendorMasterVM, LocalizeResourceSetConstants.ConfigMasterDetail);

            #region uiconfig
            CommonWrapper cWrapper = new CommonWrapper(userData);
            string isExternal = "";
            if (vMasterModel.IsExternal)
            {
                isExternal = UiConfigConstants.IsExternalTrue;
            }
            else
            {
                isExternal = UiConfigConstants.IsExternalFalse;
            }
            //var totalList = cWrapper.UiConfigAllColumnsCustom(UiConfigConstants.VendorMasterEdit, UiConfigConstants.IsHideNone, UiConfigConstants.IsRequiredNone, isExternal, UiConfigConstants.TargetView);
            var totalList = cWrapper.GetAllUiConfigList(UiConfigConstants.VendorMasterEdit, isExternal);
            var hidList = totalList.Where(x => x.Hide == true);
            vendorMasterVM.hiddenColumnList = new List<string>();
            foreach (var item in hidList)
            {
                vendorMasterVM.hiddenColumnList.Add(item.ColumnName);
            }
            var dsablList = totalList.Where(x => x.Disable == true);
            vendorMasterVM.disabledColumnList = new List<string>();
            foreach (var item in dsablList)
            {
                vendorMasterVM.disabledColumnList.Add(item.ColumnName);
            }
            var impList = totalList.Where(x => x.Required == true && x.Hide == false && x.Disable == false);
            vendorMasterVM.requiredColumnList = new List<string>();
            foreach (var item in impList)
            {
                vendorMasterVM.requiredColumnList.Add(item.ColumnName);
            }
            #endregion

            return PartialView("~/Views/Configuration/VendorMaster/_EditVendorMaster.cshtml", vendorMasterVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveVendorMaster(VendorMasterVM objVM, string Command)
        {
            if (ModelState.IsValid)
            {
                List<String> errorList = new List<string>();
                VendorMasterWrapper vmWrapper = new VendorMasterWrapper(userData);
                string Mode = string.Empty;
                long VendorMasterId = 0;

                if (objVM.vendorMasterModel.VendorMasterId > 0)
                {
                    Mode = "update";
                    errorList = vmWrapper.UpdateVendorMaster(objVM.vendorMasterModel, ref VendorMasterId);
                }
                else
                {
                    Mode = "add";
                    errorList = vmWrapper.AddVendorMaster(objVM.vendorMasterModel, ref VendorMasterId);
                }
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), VendorMasterId = VendorMasterId, mode = Mode, Command = Command }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public string GetVendorMasterAddPopupGrid(int? draw, int? start, int? length, string VendorMasterId, string clientLookupId = "", string name = "",
          string addressCity = "", string addressState = "", string type = "", bool inactiveFlag = false)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var colname = Request.Form.GetValues("columns[" + order + "][name]");
            VendorMasterWrapper vmWrapper = new VendorMasterWrapper(userData);
            var VendorMasterList = vmWrapper.PopulateVMAddGrid();
            if (VendorMasterList != null)
            {
                VendorMasterList = this.GetVendorMasterAddPopupGridGridSortByColumnWithOrder(colname[0], orderDir, VendorMasterList);
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = VendorMasterList.Count();
            totalRecords = VendorMasterList.Count();
            int initialPage = start.Value;
            var filteredResult = VendorMasterList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
        private List<VMAddGridModel> GetVendorMasterAddPopupGridGridSortByColumnWithOrder(string order, string orderDir, List<VMAddGridModel> data)
        {
            List<VMAddGridModel> lst = new List<VMAddGridModel>();
            if (data != null)
            {
                switch (order)
                {
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Name).ToList() : data.OrderBy(p => p.Name).ToList();
                        break;
                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.AddressCity).ToList() : data.OrderBy(p => p.AddressCity).ToList();
                        break;
                    case "4":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.AddressState).ToList() : data.OrderBy(p => p.AddressState).ToList();
                        break;
                    case "5":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Type).ToList() : data.OrderBy(p => p.Type).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                        break;
                }
            }
            return lst;
        }
        public JsonResult GetVendorMasterData()
        {
            VendorMasterWrapper vmWrapper = new VendorMasterWrapper(userData);
            var VendorMasterList = vmWrapper.PopulateVMAddGrid();
            return Json(VendorMasterList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AddVendorFromVMGrid(List<long> VendorsIds)
        {
            List<string> errorMessage = new List<string>();
            VendorMasterWrapper vmWrapper = new VendorMasterWrapper(userData);
            var errorList = vmWrapper.AddVendorFromVMGrid(VendorsIds);
            if (errorList != null && errorList.Count > 0)
            {
                return Json(new { errorList = errorList }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region VendorChange Pop-Up
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ValiDateChangeVendorPage(VendorMasterVM objVM)
        {
            if (ModelState.IsValid)
            {
                string IsAddOrUpdate = string.Empty;
                VendorMasterWrapper vmWrapper = new VendorMasterWrapper(userData);
                long UpdateIndex = 0;
                List<string> errorList = vmWrapper.ChangeVendorID(objVM.changeVendorModel, ref UpdateIndex);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Issuccess = true, ClientLookUpId = objVM.changeVendorModel.ClientLookupId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Create&LastUpdate
        public JsonResult CreateandLastUpdate(long vendorMasterId)
        {
            VendorMasterWrapper vmWrapper = new VendorMasterWrapper(userData);

            DataContracts.VendorMaster obj = vmWrapper.CreatedAndLastupdate(vendorMasterId);

            DateTime? CreateDate = obj?.CreateDate ?? DateTime.MinValue;
            DateTime? ModifyDate = obj?.ModifyDate ?? DateTime.MinValue;

            string CredtFormat = CreateDate.ToString() == "0001-01-01 00:00:00.0000000" ? "" : CreateDate?.ToString("MM/dd/yyyy hh:MM:ss tt");
            string ModdtFormat = ModifyDate.ToString() == "0001-01-01 00:00:00.0000000" ? "" : ModifyDate?.ToString("MM/dd/yyyy hh:MM:ss tt");

            return Json(new { Createby = obj.CreateBy, CreateDate = CredtFormat, ModifyBy = obj.ModifyBy, ModifyDate = ModdtFormat }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
