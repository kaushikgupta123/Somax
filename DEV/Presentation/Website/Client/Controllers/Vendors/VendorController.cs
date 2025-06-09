using Client.ActionFilters;
using Client.BusinessWrapper;
using Client.BusinessWrapper.Common;
using Client.Common;
using Client.Controllers.Common;
using Client.Localization;
using Client.Models;
using Client.Models.VendorPrint;

using Common.Constants;
using DataContracts;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Client.Controllers
{
    public class VendorController : SomaxBaseController
    {
        #region Search
        [CheckUserSecurity(securityType = SecurityConstants.Vendors)]
        public ActionResult Index()
        {
            VendorsVM objVendorsVM = new VendorsVM();
            VendorsModel vendors = new VendorsModel();
            VendorWrapper _VendorObj = new VendorWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            string Mode = Convert.ToString(TempData["mode"]);
            vendors = _VendorObj.PopulateDropdownControls();
            objVendorsVM.vendors = vendors;
            if (Mode == "Add")
            {
                //vendors = _VendorObj.PopulateDropdownControls();
                vendors.IsVendorAddFromUpperMenu = true;

                #region uiconfig
                CommonWrapper cWrapper = new CommonWrapper(userData);
                vendors.ViewName = UiConfigConstants.VendorAdd;  //--V2-375//
                vendors.IsExternal = false;  //--V2-375//
                //var totalList = cWrapper.UiConfigAllColumnsCustom(UiConfigConstants.VendorAdd, UiConfigConstants.IsHideNone, UiConfigConstants.IsRequiredNone, UiConfigConstants.IsExternalFalse, UiConfigConstants.TargetView);
                var totalList = cWrapper.GetAllUiConfigList(UiConfigConstants.VendorAdd, UiConfigConstants.IsExternalFalse);
                var hidList = totalList.Where(x => x.Hide == true);
                objVendorsVM.hiddenColumnList = new List<string>();
                foreach (var item in hidList)
                {
                    objVendorsVM.hiddenColumnList.Add(item.ColumnName);
                }
                var dsablList = totalList.Where(x => x.Disable == true);
                objVendorsVM.disabledColumnList = new List<string>();
                foreach (var item in dsablList)
                {
                    objVendorsVM.disabledColumnList.Add(item.ColumnName);
                }
                var impList = totalList.Where(x => x.Required == true && x.Hide == false && x.Disable == false);
                objVendorsVM.requiredColumnList = new List<string>();
                foreach (var item in impList)
                {
                    objVendorsVM.requiredColumnList.Add(item.ColumnName);
                }
                #endregion
                //Adding Security for PunchOut tab Config V2-582,V2-587
                //objVendorsVM.vendors.VendorConfigurePunchOutSecurity = userData.Security.Vendor_ConfigurePunchout.Access;
                objVendorsVM.vendors.VendorConfigurePunchOutSecurity = userData.Security.Vendors.ConfigurePunchout;
                objVendorsVM.vendors.IsSitePunchOut = userData.Site.UsePunchOut;
            }
            else if (Mode == "adddynamic")
            {
                vendors.IsVendorAddFromUpperMenu = true;
                var AllLookUps = commonWrapper.GetAllLookUpList();
                objVendorsVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
                                                .Retrieve(DataDictionaryViewNameConstant.AddVendor, userData);
                IList<string> LookupNames = objVendorsVM.UIConfigurationDetails.ToList()
                                            .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
                                            .Select(s => s.LookupName)
                                            .ToList();
                objVendorsVM.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                          .Select(s => new UILookupList
                                                          { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                          .ToList();

                objVendorsVM.AddVendor = new Models.Vendor.UIConfiguration.AddVendorModelDynamic();
                objVendorsVM.IsAddVendorDynamic = true;
            }
            objVendorsVM.security = this.userData.Security;
            objVendorsVM.UseVendorMaster = userData.Site.UseVendorMaster;
            objVendorsVM.VendorMaster_AllowLocal = userData.Site.VendorMaster_AllowLocal;  /*V2-375*/
            var StatusList = UtilityFunction.InactiveActiveVendorStatusTypes();
            if (StatusList != null)
            {
                objVendorsVM.InactiveFlagList = StatusList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            var ExternalList = UtilityFunction.IsExternalTypesWithBoolValue();
            if (ExternalList != null)
            {
                objVendorsVM.vendors.ExternalTypeList = ExternalList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }

            #region V2-796
            var OracleVendorMasterImport = commonWrapper.CheckIsActiveInterface(ApiConstants.OracleVendorMasterImport);
            objVendorsVM.OracleVendorMasterImport = OracleVendorMasterImport;
            #endregion

            LocalizeControls(objVendorsVM, LocalizeResourceSetConstants.VendorDetails);
            return View("VendorSearch", objVendorsVM);
        }

        [HttpPost]
        public string GetVendors(int? draw, int? start, int length = 0, string _vendor = "", string _name = null, string _addresscity = null, string _addressstate = null, string _type = null, string _terms = null, string _fobcode = null, int inactiveFlag = 1, string IsExternal = "", string srcData = ""
                                 , string Order = "0"/*, string orderDir = "asc"*/)//Vendor Sorting
        {
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            start = start.HasValue
                ? start / length
                : 0;
            int skip = start * length ?? 0;
            VendorWrapper objVen = new VendorWrapper(userData);
            var vendorsList = objVen.GetVendorList(Order, length, orderDir, skip, _vendor, _name, _addresscity, _addressstate, _type, _terms, _fobcode, inactiveFlag, IsExternal, srcData);
            var totalRecords = 0;
            var recordsFiltered = 0;
            recordsFiltered = vendorsList.Select(o => o.TotalCount).FirstOrDefault();
            totalRecords = vendorsList.Select(o => o.TotalCount).FirstOrDefault();
            int initialPage = start.Value;
            var filteredResult = vendorsList.ToList();

            #region uiconfig
            CommonWrapper cWrapper = new CommonWrapper(userData);
            //var hiddenList = cWrapper.UiConfigAllColumnsCustom(UiConfigConstants.VendorSearch, UiConfigConstants.IsHideTrue, UiConfigConstants.IsRequiredNone, UiConfigConstants.IsExternalNone, UiConfigConstants.TargetView).Select(x => x.ColumnName).ToList();
            var hiddenList = cWrapper.GetHiddenList(UiConfigConstants.VendorSearch).Select(x => x.ColumnName).ToList();
            #endregion

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult,hiddenColumnList = hiddenList }, JsonSerializerDateSettings);
        }

        [HttpGet]
        public string GetVendorPrintData(string _colname, string _coldir, int? start, int length = 0, string _vendor = "", string _name = null, string _addresscity = null, string _addressstate = null, string _type = null, string _terms = null, string _fobcode = null, int _inactiveFlag = 1, string _IsExternal = "", string _srcData = "")
        {

            List<VendorPrintModel> VendorPrintModelList = new List<VendorPrintModel>();
            VendorPrintModel objVendorPrintModel;
            VendorWrapper objVen = new VendorWrapper(userData);
            var vendorsList = objVen.GetVendorList(_colname, 100000, _coldir, 0, _vendor, _name, _addresscity, _addressstate, _type, _terms, _fobcode, _inactiveFlag, _IsExternal, _srcData);
            if (vendorsList != null)
            {
                foreach (var p in vendorsList)
                {
                    objVendorPrintModel = new VendorPrintModel();
                    objVendorPrintModel.ClientLookupId = p.ClientLookupId;
                    objVendorPrintModel.Name = p.Name;
                    objVendorPrintModel.AddressCity = p.AddressCity;
                    objVendorPrintModel.AddressState = p.AddressState;
                    objVendorPrintModel.Type = p.Type;
                    objVendorPrintModel.Terms = p.Terms;
                    objVendorPrintModel.FOBCode = p.FOBCode;
                    objVendorPrintModel.IsExternal = p.IsExternal;
                    VendorPrintModelList.Add(objVendorPrintModel);
                }
            }
            return JsonConvert.SerializeObject(new { data = VendorPrintModelList }, JsonSerializerDateSettings);
        }


        public ActionResult VendorDetail(long vendorId = 0)
        {
            VendorsVM objVendorsVM = new VendorsVM();
            VendorWrapper _VendorObj = new VendorWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            VendorUDF vendorUDF = new VendorUDF();
            ChangeVendorIDModel objChangeVendorIDModel = new ChangeVendorIDModel();
            VendorEmailConfigurationSetupModel objVendorEmailConfigurationSetupModel = new VendorEmailConfigurationSetupModel();
            VendorsModel objVendorModel = new VendorsModel();/*_VendorObj.populateVendorDetails(vendorId);*/
            var VendorDetailsByVendorId = _VendorObj.RetrieveVendorByVendorId(vendorId);
            Vendor objVen = VendorDetailsByVendorId.Item1;//Item1 has value of Vendor table
            Task[] tasks = new Task[3];

            tasks[0] = Task.Factory.StartNew(() => objVendorsVM.attachmentCount = commonWrapper.AttachmentCount(vendorId, AttachmentTableConstant.Vendor, userData.Security.Vendors.Edit));
            tasks[1]= Task.Factory.StartNew(() => vendorUDF = _VendorObj.RetrieveVendorUDFByVendorId(vendorId));
            tasks[2] = Task.Factory.StartNew(() => objVendorsVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration().Retrieve(DataDictionaryViewNameConstant.ViewVendorWidget, userData));
            Task.WaitAll(tasks);
            
            objChangeVendorIDModel.VendorId = objVen.VendorId;
            objChangeVendorIDModel.NewClientLookupId = objVen.ClientLookupId;
            objChangeVendorIDModel.OldClientLookupId = objVen.ClientLookupId;
            objVendorsVM._ChangeVendorIDModel = objChangeVendorIDModel;

            //objVendorsVM.vendors = objVen;
            objVendorsVM.security = this.userData.Security;
            objVendorsVM.udata = this.userData;
            objVendorsVM.UseVendorMaster = userData.Site.UseVendorMaster;
            objVendorsVM.VendorMaster_AllowLocal = userData.Site.VendorMaster_AllowLocal;  /*V2-375*/
            ViewBag.Sec_Part_Vendor_XRef = userData.Security.Parts.Part_Vendor_XRef;
            objVendorsVM.ViewVendor = new Models.Vendor.UIConfiguration.ViewVendorModelDynamic();
            //V2-642
            objVendorsVM.ViewVendor = _VendorObj.MapVendorDataForView(objVendorsVM.ViewVendor, objVen);
            objVendorsVM.ViewVendor = _VendorObj.MapVendorUDFDataForView(objVendorsVM.ViewVendor,vendorUDF);
            objVendorModel.ExVendorStat = VendorDetailsByVendorId.Item2;//Item2 has value of ExVendorStat
            objVendorModel.VendorId = objVen.VendorId;
            objVendorModel.ClientLookupId = objVen.ClientLookupId;
            objVendorModel.IsExternal = objVen.IsExternal;
            // V2-859 - InactiveFlag - Used to determine which action menu item to display
            objVendorModel.InactiveFlag = objVen.InactiveFlag; 
            objVendorsVM.vendors = objVendorModel;

            //V2-750 Start
            objVendorEmailConfigurationSetupModel.VendorId=objVen.VendorId;
            objVendorEmailConfigurationSetupModel.Email=objVen.EmailAddress;
            objVendorEmailConfigurationSetupModel.AutoEmailPO=objVen.AutoEmailPO;
            objVendorsVM.VendorEmailConfigurationSetupModel = objVendorEmailConfigurationSetupModel;

            InterfaceProp iprop = commonWrapper.RetrieveInterfaceProperties(ApiConstants.OracleVendorMasterImport);
            if (iprop.InUse == true)
            {
                objVendorsVM.vendors.EmailConfigurationCriteriaOnInterfaceProp = true;
            }
            else
            {
                objVendorsVM.vendors.EmailConfigurationCriteriaOnInterfaceProp = false;
            }
            //V2-750 End
            //attTask.Wait();
            if (userData.Security.Vendors.ConfigurePunchout && userData.Site.UsePunchOut)
            {
                #region PunchOutSupport
                objVendorsVM.VendorPunchoutSetupModel.VendorId = objVen.VendorId;
                objVendorsVM.VendorPunchoutSetupModel.PunchoutIndicator = objVen.PunchoutIndicator;
                objVendorsVM.VendorPunchoutSetupModel.VendorDomain = objVen.VendorDomain;
                objVendorsVM.VendorPunchoutSetupModel.VendorIdentity = objVen.VendorIdentity;
                objVendorsVM.VendorPunchoutSetupModel.SharedSecret = objVen.SharedSecret;
                objVendorsVM.VendorPunchoutSetupModel.SenderDomain = objVen.SenderDomain;
                objVendorsVM.VendorPunchoutSetupModel.SenderIdentity = objVen.SenderIdentity;
                objVendorsVM.VendorPunchoutSetupModel.PunchoutURL = objVen.PunchoutURL;
                objVendorsVM.VendorPunchoutSetupModel.AutoSendPunchOutPO = objVen.AutoSendPunchOutPO;
                var domainTypes = commonWrapper.GetListFromConstVals(LookupListConstants.DOMAIN_TYPE);
                objVendorsVM.VendorPunchoutSetupModel.VendorDomainList = domainTypes.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
                objVendorsVM.VendorPunchoutSetupModel.SenderDomainList = objVendorsVM.VendorPunchoutSetupModel.VendorDomainList;
                //V2-582
                objVendorsVM.VendorPunchoutSetupModel.SendPunchoutPOURL = objVen.SendPunchoutPOURL;
                //V2-587
                objVendorsVM.VendorPunchoutSetupModel.SendPunchoutPOEmail = objVen.SendPunchoutPOEmail;
                #endregion
            }



            //objVendorsVM.vendors.VendorConfigurePunchOutSecurity = userData.Security.Vendor_ConfigurePunchout.Access;
            objVendorsVM.vendors.VendorConfigurePunchOutSecurity = userData.Security.Vendors.ConfigurePunchout;

            objVendorsVM.vendors.IsSitePunchOut = userData.Site.UsePunchOut;

            #region V2-796
            var OracleVendorMasterImport = commonWrapper.CheckIsActiveInterface(ApiConstants.OracleVendorMasterImport);
            objVendorsVM.OracleVendorMasterImport = OracleVendorMasterImport;
            #endregion

            LocalizeControls(objVendorsVM, LocalizeResourceSetConstants.VendorDetails);

            //#region uiconfig
            ////CommonWrapper cWrapper = new CommonWrapper(userData);
            //string isExternal = "";
            //if (objVen.IsExternal)
            //{
            //    isExternal = UiConfigConstants.IsExternalTrue;
            //}
            //else
            //{
            //    isExternal = UiConfigConstants.IsExternalFalse;
            //}
            ////objVendorsVM.hiddenColumnList = cWrapper.UiConfigAllColumnsCustom(UiConfigConstants.VendorDetail, UiConfigConstants.IsHideTrue, UiConfigConstants.IsRequiredNone, isExternal, UiConfigConstants.TargetView).Select(x => x.ColumnName).ToList();
            //objVendorsVM.hiddenColumnList = commonWrapper.GetHiddenList(UiConfigConstants.VendorDetail, isExternal).Select(x => x.ColumnName).ToList();
            //#endregion

            return PartialView("VendorDetails", objVendorsVM);
        }


        [HttpPost]
        public JsonResult ValidateForActiveInactive(bool InActiveFlag, long VendorId, string ClientLookupId)
        {
            string validationMessage = string.Empty;
            VendorWrapper _VendorObj = new VendorWrapper(userData);
            string flag = string.Empty;
            if (InActiveFlag)
            {
                flag = ActivationStatusConstant.Activate;
            }
            else
            {
                flag = ActivationStatusConstant.InActivate;
            }
            var vendor = _VendorObj.ValidateVendorStatusChange(VendorId, flag, ClientLookupId);
            if (vendor.ErrorMessages != null && vendor.ErrorMessages.Count > 0)
            {
                return Json(vendor.ErrorMessages, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { validationStatus = true }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult MakeActiveInactive(bool InActiveFlag, long VendorId)
        {
            VendorWrapper _VendorObj = new VendorWrapper(userData);
            var ErrorMessages = _VendorObj.MakeActiveInactive(InActiveFlag, VendorId);
            if (ErrorMessages != null && ErrorMessages.Count > 0)
            {
                return Json(ErrorMessages, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string Event = string.Empty;
                if (InActiveFlag)
                {
                    Event = ActivationStatusConstant.Activate;
                }
                else
                {
                    Event = ActivationStatusConstant.InActivate;
                }
                var createEventStatus = _VendorObj.CreateVendorEvent(VendorId, Event,"");
                if (createEventStatus != null && createEventStatus.Count > 0)
                {
                    return Json(createEventStatus, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        #endregion

        #region Add-Vendor
        public ActionResult Add()
        {
            //TempData["mode"] = "Add";
            TempData["mode"] = "adddynamic";
            return Redirect("/Vendor/Index?page=Inventory_Vendors");
        }
        [HttpGet]
        public ActionResult ShowAddVendor()
        {
            VendorWrapper _VendorObj = new VendorWrapper(userData);
            VendorsModel objVendors = new VendorsModel();
            VendorsVM objVendorsVM = new VendorsVM();
            objVendors = _VendorObj.PopulateDropdownControls();

            #region uiconfig
            objVendors.ViewName = UiConfigConstants.VendorAdd;  //--V2-375//
            objVendors.IsExternal = false;  //--V2-375//
            objVendorsVM.vendors = objVendors;

            CommonWrapper cWrapper = new CommonWrapper(userData);
            //var totalList = cWrapper.UiConfigAllColumnsCustom(UiConfigConstants.VendorAdd, UiConfigConstants.IsHideNone, UiConfigConstants.IsRequiredNone, UiConfigConstants.IsExternalFalse, UiConfigConstants.TargetView);
            var totalList = cWrapper.GetAllUiConfigList(UiConfigConstants.VendorAdd, UiConfigConstants.IsExternalFalse);
            var hidList = totalList.Where(x => x.Hide == true);
            objVendorsVM.hiddenColumnList = new List<string>();
            foreach (var item in hidList)
            {
                objVendorsVM.hiddenColumnList.Add(item.ColumnName);
            }
            var dsablList = totalList.Where(x => x.Disable == true);
            objVendorsVM.disabledColumnList = new List<string>();
            foreach (var item in dsablList)
            {
                objVendorsVM.disabledColumnList.Add(item.ColumnName);
            }
            var impList = totalList.Where(x => x.Required == true && x.Hide == false && x.Disable == false);
            objVendorsVM.requiredColumnList = new List<string>();
            foreach (var item in impList)
            {
                objVendorsVM.requiredColumnList.Add(item.ColumnName);
            }
            #endregion
            //Adding Security for PunchOut tab Config V2-582,V2-587
            objVendorsVM.vendors.VendorConfigurePunchOutSecurity = userData.Security.Vendors.ConfigurePunchout;
            objVendorsVM.vendors.IsSitePunchOut = userData.Site.UsePunchOut;
            LocalizeControls(objVendorsVM, LocalizeResourceSetConstants.VendorDetails);
            return PartialView("_VendorAdd", objVendorsVM);
        }
        [HttpGet]
        public ActionResult ShowAddVendorPopup()
        {
            VendorsVM objVendorsVM = new VendorsVM();
            LocalizeControls(objVendorsVM, LocalizeResourceSetConstants.VendorDetails);
            return Json(objVendorsVM, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddVendor(VendorsVM _vendor, string Command)
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                VendorWrapper _VendorObj = new VendorWrapper(userData);
                var Result = _VendorObj.AddVendorDetails(_vendor.vendors);
                if (Result != null && Result.ErrorMessages.Count > 0)
                {
                    return Json(Result.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    objectId = Result.VendorId;
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Command = Command, vendorid = Result.VendorId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Edit-Vendor
        [HttpPost]
        public ActionResult VendorEdit(long vendorId)
        {
            VendorsVM objVendorsVM = new VendorsVM();
            VendorWrapper _VendorObj = new VendorWrapper(userData);
            VendorsModel objVen = _VendorObj.populateVendorDetails(vendorId);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            objVen = _VendorObj.PopulateDropdownControls(objVen);
            objVen.ViewName = UiConfigConstants.VendorEdit;  //--V2-375--uiconfig//
            objVendorsVM.vendors = objVen;
            objVendorsVM.security = userData.Security;
            if (userData.Security.Vendors.ConfigurePunchout && userData.Site.UsePunchOut)
            {
                #region PunchOutSupport   

                var domainTypes = commonWrapper.GetListFromConstVals(LookupListConstants.DOMAIN_TYPE);
                objVendorsVM.vendors.VendorDomainList = domainTypes.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
                objVendorsVM.vendors.SenderDomainList = objVendorsVM.vendors.VendorDomainList;


                #endregion
            }
            #region uiconfig
            CommonWrapper cWrapper = new CommonWrapper(userData);
            string isExternal = "";
            if (objVen.IsExternal)
            {
                isExternal = UiConfigConstants.IsExternalTrue;
            }
            else
            {
                isExternal = UiConfigConstants.IsExternalFalse;
            }
            //var totalList = cWrapper.UiConfigAllColumnsCustom(UiConfigConstants.VendorEdit, UiConfigConstants.IsHideNone, UiConfigConstants.IsRequiredNone, isExternal, UiConfigConstants.TargetView);
            var totalList = cWrapper.GetAllUiConfigList(UiConfigConstants.VendorEdit, isExternal);
            var hidList = totalList.Where(x => x.Hide == true);
            objVendorsVM.hiddenColumnList = new List<string>();
            foreach (var item in hidList)
            {
                objVendorsVM.hiddenColumnList.Add(item.ColumnName);
            }
            var dsablList = totalList.Where(x => x.Disable == true);
            objVendorsVM.disabledColumnList = new List<string>();
            foreach (var item in dsablList)
            {
                objVendorsVM.disabledColumnList.Add(item.ColumnName);
            }
            var impList = totalList.Where(x => x.Required == true && x.Hide == false && x.Disable == false);
            objVendorsVM.requiredColumnList = new List<string>();
            foreach (var item in impList)
            {
                objVendorsVM.requiredColumnList.Add(item.ColumnName);
            }
            #endregion
            objVendorsVM.vendors.VendorConfigurePunchOutSecurity = userData.Security.Vendors.ConfigurePunchout;
            objVendorsVM.vendors.IsSitePunchOut = userData.Site.UsePunchOut;
            LocalizeControls(objVendorsVM, LocalizeResourceSetConstants.VendorDetails);
            return PartialView("_VendorEdit", objVendorsVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditVendor(VendorsVM _vendor)
        {
            if (ModelState.IsValid)
            {
                VendorWrapper _VendorObj = new VendorWrapper(userData);
                Vendor objVendor = _VendorObj.VendorEdit(_vendor.vendors, _vendor.vendors.VendorId);
                if (objVendor.ErrorMessages != null && objVendor.ErrorMessages.Count > 0)
                {
                    return Json(objVendor.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), vendorid = objVendor.VendorId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ChangeVendorId(VendorsVM vendorsVM)
        {
            if (ModelState.IsValid)
            {
                VendorWrapper vendorWrapper = new VendorWrapper(userData);
                ChangeVendorIDModel _changeVendorIDModel = new ChangeVendorIDModel();
                _changeVendorIDModel = vendorsVM._ChangeVendorIDModel;
                List<string> errorList = new List<string>();
                errorList = vendorWrapper.ChangeVendorId(_changeVendorIDModel);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
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

        #region Contacts
        [HttpPost]
        public string PopulateContacts(int? draw, int? start, int? length, long _vendorId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            VendorWrapper _VendorObj = new VendorWrapper(userData);
            var ContactList = _VendorObj.PopulateVendorContact(_vendorId);
            ContactList = this.GetAllContactsSortByColumnWithOrder(order, orderDir, ContactList);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = ContactList.Count();
            totalRecords = ContactList.Count();
            int initialPage = start.Value;
            var filteredResult = ContactList
               .Skip(initialPage * length ?? 0)
               .Take(length ?? 0)
               .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeSettings);
        }
        private List<ContactModel> GetAllContactsSortByColumnWithOrder(string order, string orderDir, List<ContactModel> data)
        {
            List<ContactModel> lst = new List<ContactModel>();
            if (data != null)
            {
                switch (order)
                {
                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Name).ToList() : data.OrderBy(p => p.Name).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Phone1).ToList() : data.OrderBy(p => p.Phone1).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Email1).ToList() : data.OrderBy(p => p.Email1).ToList();
                        break;
                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.OwnerName).ToList() : data.OrderBy(p => p.OwnerName).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Name).ToList() : data.OrderBy(p => p.Name).ToList();
                        break;
                }
            }
            return lst;
        }
        public PartialViewResult AddContact(long vendorId, string vendorClientLookupId)
        {
            VendorsVM objVendorsVM = new VendorsVM();
            ContactModel Contact = new ContactModel();
            Contact.VendorId = vendorId;
            Contact.ClientLookupId = vendorClientLookupId;
            objVendorsVM.contactModel = Contact;
            LocalizeControls(objVendorsVM, LocalizeResourceSetConstants.VendorDetails);
            return PartialView("_ContactsAdd", objVendorsVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddContacts(VendorsVM objContact)
        {
            string Mode = "add";
            if (ModelState.IsValid)
            {
                var vendorId = objContact.contactModel.VendorId;
                VendorWrapper _VendorObj = new VendorWrapper(userData);
                List<string> errorMessages = new List<string>();
                if (objContact.contactModel.ContactId != 0)
                {
                    Mode = "update";
                    errorMessages = _VendorObj.ContactEdit(objContact.contactModel, vendorId);
                }
                else
                {
                    errorMessages = _VendorObj.ContactAdd(objContact.contactModel, vendorId);
                }
                if (errorMessages != null && errorMessages.Count > 0)
                {
                    return Json(errorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), vendorid = vendorId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public PartialViewResult EditContact(long vendorId, long contactId, int updatedIndex, string vendorClientLookupId)
        {
            VendorWrapper _VendorObj = new VendorWrapper(userData);
            VendorsVM objVendorsVM = new VendorsVM();
            ContactModel _ContactModel = new ContactModel();
            _ContactModel = _VendorObj.ShowEditContact(contactId, vendorId, vendorClientLookupId, updatedIndex);
            objVendorsVM.contactModel = _ContactModel;
            LocalizeControls(objVendorsVM, LocalizeResourceSetConstants.VendorDetails);
            return PartialView("_ContactsAdd", objVendorsVM);
        }
        [HttpPost]
        public ActionResult DeleteContacts(string _contactId)
        {
            VendorWrapper _VendorObj = new VendorWrapper(userData);
            var deleteResult = _VendorObj.ContactDelete(_contactId);
            if (deleteResult)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region parts
        [HttpGet]
        public string GetVendorPartsPrintData(string _colname, string _coldir, long _vendorId)
        {
            string order = _colname;
            string orderDir = _coldir;
            VendorWrapper _VendorObj = new VendorWrapper(userData);
            List<PartVendorXrefModel> pvx = _VendorObj.PopulateParts(_vendorId);
            List<PrintPartVendorXrefModel> PartPrintList=new List<PrintPartVendorXrefModel>();
            PrintPartVendorXrefModel printPartVendorModel;
            pvx = this.GetAllPartsSortByColumnWithOrder(order, orderDir, pvx);
            if(pvx!=null)
            {
                foreach (var p in pvx)
                {
                    printPartVendorModel = new PrintPartVendorXrefModel();
                    printPartVendorModel.Part = p.Part;
                    printPartVendorModel.PartDescription = p.PartDescription;
                    printPartVendorModel.CatalogNumber= p.CatalogNumber;
                    printPartVendorModel.Manufacturer = p.Manufacturer;
                    printPartVendorModel.ManufacturerID= p.ManufacturerID;
                    printPartVendorModel.OrderUnit = p.OrderUnit;
                    printPartVendorModel.Price = p.Price;
                    printPartVendorModel.OrderQuantity = p.OrderQuantity;
                    PartPrintList.Add(printPartVendorModel);
                }
            }
            return JsonConvert.SerializeObject(new { data = PartPrintList }, JsonSerializerDateSettings);

        }
        [HttpPost]
        public string PopulateParts(int? draw, int? start, int? length, long _vendorId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            VendorWrapper _VendorObj = new VendorWrapper(userData);
            List<PartVendorXrefModel> pvx = _VendorObj.PopulateParts(_vendorId);
            pvx = this.GetAllPartsSortByColumnWithOrder(order, orderDir, pvx);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = pvx.Count();
            totalRecords = pvx.Count();
            int initialPage = start.Value;
            var filteredResult = pvx
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeSettings);

        }
        private List<PartVendorXrefModel> GetAllPartsSortByColumnWithOrder(string order, string orderDir, List<PartVendorXrefModel> data)
        {
            List<PartVendorXrefModel> lst = new List<PartVendorXrefModel>();
            if (data != null)
            {
                switch (order)
                {
                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Part).ToList() : data.OrderBy(p => p.Part).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PartDescription).ToList() : data.OrderBy(p => p.PartDescription).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CatalogNumber).ToList() : data.OrderBy(p => p.CatalogNumber).ToList();
                        break;
                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Manufacturer).ToList() : data.OrderBy(p => p.Manufacturer).ToList();
                        break;
                    case "4":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ManufacturerID).ToList() : data.OrderBy(p => p.ManufacturerID).ToList();
                        break;
                    case "5":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.OrderQuantity).ToList() : data.OrderBy(p => p.OrderQuantity).ToList();
                        break;
                    case "6":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.OrderUnit).ToList() : data.OrderBy(p => p.OrderUnit).ToList();
                        break;
                    case "7":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Price).ToList() : data.OrderBy(p => p.Price).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Part).ToList() : data.OrderBy(p => p.Part).ToList();
                        break;
                }
            }
            return lst;
        }
        [HttpGet]
        public ActionResult PartsAdd(long vendorId, string ClientLookupId)
        {
            VendorWrapper _VendorObj = new VendorWrapper(userData);
            VendorsVM objVendorsVM = new VendorsVM();
            PartVendorXrefModel partVendorXref = new PartVendorXrefModel();
            partVendorXref.VendorId = vendorId;
            partVendorXref.VendorClientLookupId = ClientLookupId;
            var objLookup = _VendorObj.PopulateUnitOfMeasure();
            if (objLookup != null)
            {
                partVendorXref.OrderUnitList = objLookup.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue });
            }
            objVendorsVM.partVendorXrefModel = partVendorXref;
            LocalizeControls(objVendorsVM, LocalizeResourceSetConstants.VendorDetails);
            return PartialView("_PartsAdd", objVendorsVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PartsAdd(VendorsVM _PVXModel)
        {
            VendorWrapper _VendorObj = new VendorWrapper(userData);
            Part_Vendor_Xref pvx = new Part_Vendor_Xref();
            string Mode = string.Empty;
            List<string> errorMessages = new List<string>();

            if (ModelState.IsValid)
            {
                if (_PVXModel.partVendorXrefModel.PartVendorXrefId != 0)
                {
                    errorMessages = _VendorObj.UpdatePartVendorXref(_PVXModel.partVendorXrefModel, _PVXModel.partVendorXrefModel.VendorId);
                }
                else
                {
                    Mode = "add";
                    errorMessages = _VendorObj.AddPartVendorXref(_PVXModel.partVendorXrefModel, _PVXModel.partVendorXrefModel.VendorId);
                }

                if (errorMessages == null || errorMessages.Count == 0)
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), vendorid = _PVXModel.partVendorXrefModel.VendorId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(errorMessages, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult PartsEdit(long vendorId, long PartVendorXrefId, int updatedIndex)
        {
            VendorWrapper _VendorObj = new VendorWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            VendorsVM objVendorsVM = new VendorsVM();

            Part_Vendor_Xref pvx = new Part_Vendor_Xref()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                VendorId = vendorId,
                Part_Vendor_XrefId = PartVendorXrefId

            };
            pvx.RetrieveByPKExtended(userData.DatabaseKey);

            PartVendorXrefModel partVendorXref = new PartVendorXrefModel();
            partVendorXref.VendorId = vendorId;
            partVendorXref.VendorClientLookupId = pvx.Vendor_ClientLookupId;
            partVendorXref.CatalogNumber = pvx.CatalogNumber;
            partVendorXref.Manufacturer = pvx.Manufacturer;
            partVendorXref.ManufacturerID = pvx.ManufacturerId;
            partVendorXref.OrderQuantity = pvx.OrderQuantity;
            partVendorXref.OrderUnit = pvx.OrderUnit;
            partVendorXref.Price = pvx.Price;
            partVendorXref.PreferredVendor = pvx.PreferredVendor;
            partVendorXref.PartVendorXrefId = pvx.Part_Vendor_XrefId;
            partVendorXref.Part = pvx.Part_ClientLookupId;
            var AllLookUpList = commonWrapper.GetAllLookUpList();
            if (AllLookUpList != null)
            {
                List<DataContracts.LookupList> objLookup = AllLookUpList.Where(x => x.ListName == LookupListConstants.Unit_Of_Measure).ToList();
                if (objLookup != null)
                {
                    partVendorXref.OrderUnitList = objLookup.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue });
                }
            }
            objVendorsVM.partVendorXrefModel = partVendorXref;
            LocalizeControls(objVendorsVM, LocalizeResourceSetConstants.VendorDetails);
            return PartialView("_PartsAdd", objVendorsVM);
        }

        [HttpPost]
        public ActionResult PartsDelete(long _PartVendorXrefId, long vendorId)
        {
            VendorWrapper _VendorObj = new VendorWrapper(userData);
            var deleteResult = _VendorObj.DeletePart(_PartVendorXrefId, vendorId);
            if (deleteResult)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Notes
        [HttpPost]
        public string PopulateNotes(int? draw, int? start, int? length, long _vendorId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            var Notes = objCommonWrapper.PopulateNotes(_vendorId, "Vendor");
            Notes = this.GetAllNotesSortByColumnWithOrder(order, orderDir, Notes);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = Notes.Count();
            totalRecords = Notes.Count();
            int initialPage = start.Value;
            var filteredResult = Notes
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeSettings);
        }
        private List<NotesModel> GetAllNotesSortByColumnWithOrder(string order, string orderDir, List<NotesModel> data)
        {
            List<NotesModel> lst = new List<NotesModel>();
            if (data != null)
            {
                switch (order)
                {
                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Subject).ToList() : data.OrderBy(p => p.Subject).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.OwnerName).ToList() : data.OrderBy(p => p.OwnerName).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ModifiedDate).ToList() : data.OrderBy(p => p.ModifiedDate).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Subject).ToList() : data.OrderBy(p => p.Subject).ToList();
                        break;
                }
            }
            return lst;
        }
        [HttpGet]
        public PartialViewResult AddNotes(long vendorId, string vendorClientLookupId)
        {
            VendorsVM objVendorsVM = new VendorsVM();
            NotesModel Notes = new NotesModel();
            Notes.VendorId = vendorId;
            Notes.ClientLookupId = vendorClientLookupId;
            Notes.ObjectId = vendorId;
            objVendorsVM.notesModel = Notes;
            LocalizeControls(objVendorsVM, LocalizeResourceSetConstants.VendorDetails);
            return PartialView("_NotesAdd", objVendorsVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNotes(VendorsVM _NotesModel)
        {
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<string> errorMessages = new List<string>();
            if (ModelState.IsValid)
            {
                string Mode = string.Empty;
                if (_NotesModel.notesModel.NotesId == 0)
                {
                    Mode = "add";
                }
                errorMessages = commonWrapper.AddNotes(_NotesModel.notesModel, "Vendor");
                if (errorMessages == null || errorMessages.Count == 0)
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), vendorid = _NotesModel.notesModel.VendorId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(errorMessages, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult DeleteNotes(long _notesId, long vendorId)
        {
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            var deleteResult = commonWrapper.DeleteNotes(_notesId);
            if (deleteResult)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public PartialViewResult EditNotes(long _vendorId, long _notesId, int _updatedIndex, string vendorClientLookupId)
        {
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            VendorsVM objVendorsVM = new VendorsVM();
            NotesModel _NotesModel = new NotesModel();
            _NotesModel = commonWrapper.EditNotes(_vendorId, _notesId);
            _NotesModel.VendorId = _vendorId;
            _NotesModel.ClientLookupId = vendorClientLookupId;
            objVendorsVM.notesModel = _NotesModel;
            LocalizeControls(objVendorsVM, LocalizeResourceSetConstants.VendorDetails);
            return PartialView("_NotesAdd", objVendorsVM);
        }
        #endregion

        #region Attachment

        [HttpPost]
        public string PopulateAttachment(int? draw, int? start, int? length, long _vendorId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            var Attachments = objCommonWrapper.PopulateAttachments(_vendorId, "Vendor", userData.Security.Vendors.Edit);
            if (Attachments != null)
            {
                Attachments = GetAllAttachmentsSortByColumnWithOrder(order, orderDir, Attachments);
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = Attachments.Count();
            totalRecords = Attachments.Count();
            int initialPage = start.Value;
            var filteredResult = Attachments
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeSettings);
        }
        [HttpPost]
        public ActionResult DeleteAttachment(long _fileAttachmentId)
        {
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            bool deleteResult = false;
            string Message = string.Empty;
            bool OnPremise = userData.DatabaseKey.Client.OnPremise;
            if (OnPremise)
            {
                deleteResult = objCommonWrapper.DeleteAttachmentOnPremise(_fileAttachmentId, ref Message);
            }
            else
            {
                deleteResult = objCommonWrapper.DeleteAttachment(_fileAttachmentId);

            }
            if (deleteResult)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString(), Message = Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public PartialViewResult AddAttachment(long vendorId, string vendorClientLookupId)
        {
            VendorWrapper _VendorObj = new VendorWrapper(userData);
            VendorsVM objVendorsVM = new VendorsVM();
            AttachmentModel Attachment = new AttachmentModel();

            Attachment.VendorId = vendorId;
            Attachment.ClientLookupId = vendorClientLookupId;

            objVendorsVM.attachmentModel = Attachment;
            LocalizeControls(objVendorsVM, LocalizeResourceSetConstants.VendorDetails);
            return PartialView("_AttachmentAdd", objVendorsVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAttachment()
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                Stream stream = Request.Files[0].InputStream;
                Client.Models.AttachmentModel attachmentModel = new Client.Models.AttachmentModel();
                CommonWrapper objCommonWrapper = new CommonWrapper(userData);
                Attachment fileAtt = new Attachment();
                attachmentModel.FileName = System.IO.Path.GetFileNameWithoutExtension(Request.Files[0].FileName);
                string fileExt = System.IO.Path.GetExtension(Request.Files[0].FileName);
                attachmentModel.FileType = fileExt.Substring(1);
                attachmentModel.FileSize = Request.Files[0].ContentLength;
                attachmentModel.ContentType = Request.Files[0].ContentType;
                attachmentModel.ObjectId = Convert.ToInt64(Request.Form["attachmentModel.VendorId"]);
                attachmentModel.Subject = String.IsNullOrEmpty(Request.Form["attachmentModel.Subject"]) ? "No Subject" : Request.Form["attachmentModel.Subject"];
                attachmentModel.TableName = "Vendor";
                bool attachStatus = false;
                bool OnPremise = userData.DatabaseKey.Client.OnPremise;
                if (OnPremise)
                {
                    fileAtt = objCommonWrapper.AddAttachmentOnPremise(attachmentModel, stream, ref attachStatus, userData.Security.Vendors.Edit);
                }
                else
                {
                    fileAtt = objCommonWrapper.AddAttachment(attachmentModel, stream, ref attachStatus, userData.Security.Vendors.Edit);
                }
                if (attachStatus)
                {
                    if (fileAtt.ErrorMessages != null && fileAtt.ErrorMessages.Count > 0)
                    {
                        return Json(fileAtt.ErrorMessages, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { Result = JsonReturnEnum.success.ToString(), vendorid = Convert.ToInt64(Request.Form["attachmentModel.VendorId"]) }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var fileTypeMessage = UtilityFunction.GetMessageFromResource("spnInvalidFileType", LocalizeResourceSetConstants.Global);
                    return Json(fileTypeMessage, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DownloadAttachment(long _fileinfoId)
        {
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            DataContracts.Attachment fileInfo = new DataContracts.Attachment();
            bool OnPremise = userData.DatabaseKey.Client.OnPremise;
            if (OnPremise)
            {
                fileInfo = objCommonWrapper.DownloadAttachmentOnPremise(_fileinfoId);
                string contentType = MimeMapping.GetMimeMapping(fileInfo.AttachmentURL);
                return File(fileInfo.AttachmentURL, contentType, fileInfo.FileName + '.' + fileInfo.FileType);
            }
            else
            {
                fileInfo = objCommonWrapper.DownloadAttachment(_fileinfoId);
                return Redirect(fileInfo.AttachmentURL);
            }

        }
        #endregion

        #region PunchOut
        public JsonResult PunchOutSetUp(VendorsVM vendor)
        {
            if (ModelState.IsValid)
            {
                VendorWrapper vendorObj = new VendorWrapper(userData);
                Vendor objVendor = vendorObj.PunchOutSetUp(vendor.VendorPunchoutSetupModel);
                if (objVendor.ErrorMessages != null && objVendor.ErrorMessages.Count > 0)
                {
                    return Json(objVendor.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), vendorid = objVendor.VendorId }, JsonRequestBehavior.AllowGet);
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

        #region V2-642 Add Vendor Dynamic
        [HttpGet]
        public ActionResult ShowAddVendorDynamic()
        {
            VendorWrapper _VendorObj = new VendorWrapper(userData);
            VendorsModel objVendors = new VendorsModel();
            VendorsVM objVendorsVM = new VendorsVM();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            var AllLookUps = commonWrapper.GetAllLookUpList();
            objVendorsVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
                                            .Retrieve(DataDictionaryViewNameConstant.AddVendor, userData);
            IList<string> LookupNames = objVendorsVM.UIConfigurationDetails.ToList()
                                        .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
                                        .Select(s => s.LookupName)
                                        .ToList();
            objVendorsVM.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                      .Select(s => new UILookupList
                                                      { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                      .ToList();

            objVendorsVM.AddVendor = new Models.Vendor.UIConfiguration.AddVendorModelDynamic();
            LocalizeControls(objVendorsVM, LocalizeResourceSetConstants.VendorDetails);
            return PartialView("_VendorAddDynamic", objVendorsVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddVendorDynamic(VendorsVM objVM, string Command)
        {
            VendorWrapper eWrapper = new VendorWrapper(userData);
            string ModelValidationFailedMessage = string.Empty;

            if (ModelState.IsValid)
            {
                Vendor vendor = new Vendor();
                vendor = eWrapper.AddVendorDynamic(objVM);
                if (vendor.ErrorMessages != null && vendor.ErrorMessages.Count > 0)
                {
                    return Json(vendor.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Command = Command, vendorid = vendor.VendorId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region Edit Vendor Dynamic V2-642
        [HttpPost]
        public ActionResult VendorEditDynamic(long vendorId)
        {
            VendorsVM objVendorsVM = new VendorsVM();
            VendorWrapper _VendorObj = new VendorWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            objVendorsVM.security = userData.Security;
            objVendorsVM.EditVendor = _VendorObj.RetrieveVendorDetailsByVendorId(vendorId);
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            AllLookUps = commonWrapper.GetAllLookUpList();
            objVendorsVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
                                         .Retrieve(DataDictionaryViewNameConstant.EditVendor, userData);
            IList<string> LookupNames = objVendorsVM.UIConfigurationDetails.ToList()
                                            .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
                                            .Select(s => s.LookupName)
                                            .ToList();
            if (AllLookUps != null)
            {
                objVendorsVM.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                          .Select(s => new UILookupList
                                                          { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                          .ToList();
            }
            LocalizeControls(objVendorsVM, LocalizeResourceSetConstants.VendorDetails);
            return PartialView("_VendorEditDynamic", objVendorsVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditVendorDynamic(VendorsVM _vendor)
        {
            if (ModelState.IsValid)
            {
                VendorWrapper _VendorObj = new VendorWrapper(userData);
                Vendor objVendor = _VendorObj.EditVendorDynamic(_vendor);
                if (objVendor.ErrorMessages != null && objVendor.ErrorMessages.Count > 0)
                {
                    return Json(objVendor.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), vendorid = objVendor.VendorId }, JsonRequestBehavior.AllowGet);
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

        #region EmailConfiguration V2-750
        public JsonResult EmailConfigurationSetUp(VendorsVM vendor)
        {
            if (ModelState.IsValid)
            {
                VendorWrapper vendorObj = new VendorWrapper(userData);
                Vendor objVendor = vendorObj.EmailConfigurationSetUp(vendor.VendorEmailConfigurationSetupModel);
                if (objVendor.ErrorMessages != null && objVendor.ErrorMessages.Count > 0)
                {
                    return Json(objVendor.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), vendorid = objVendor.VendorId }, JsonRequestBehavior.AllowGet);
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

        #region V2-929
        [HttpPost]
        public string PopulateVendorInsurance(int? draw, int? start, int? length, long _vendorId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            VendorWrapper _VendorObj = new VendorWrapper(userData);
            start = start.HasValue
          ? start / length
          : 0;
            int skip = start * length ?? 0;
            List<VendorInsuranceModel> viList = _VendorObj.PopulateVendorInsuranceGrid(skip, length ?? 0, order, orderDir, _vendorId);
            var totalRecords = 0;
            var recordsFiltered = 0;
            if (viList != null && viList.Count > 0)
            {
                recordsFiltered = viList[0].TotalCount;
                totalRecords = viList[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }
            int initialPage = start.Value;
            var filteredResult = viList
              .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);

        }
        public PartialViewResult AddVendorInsurance(long VendorId)
        {
            VendorsVM objVendorVM = new VendorsVM();
            VendorInsuranceModel vendorInsuranceModel = new VendorInsuranceModel();
            objVendorVM.security = this.userData.Security;
            vendorInsuranceModel.VendorId = VendorId;

            objVendorVM.VendorInsuranceModel = vendorInsuranceModel;
            LocalizeControls(objVendorVM, LocalizeResourceSetConstants.VendorDetails);
            return PartialView("_AddVendorInsurancePopUp", objVendorVM);
        }
        public PartialViewResult EditVendorInsurance(long VendorInsuranceId, long Vendor_InsuranceSource, long VendorId)
        {
            VendorsVM objVendorVM = new VendorsVM();
            VendorWrapper vendorWrapper = new VendorWrapper(userData);
            var vendorInsurance = vendorWrapper.PopulateVendorInsuranceDetails(VendorInsuranceId);
            objVendorVM.security = this.userData.Security;

            if (vendorInsurance.ExpireDate != null && vendorInsurance.ExpireDate.Value == default(DateTime))
            {
                vendorInsurance.ExpireDate = null;
            }
            if (vendorInsurance.EffectiveDate != null && vendorInsurance.EffectiveDate.Value == default(DateTime))
            {
                vendorInsurance.EffectiveDate = null;
            }
            if (vendorInsurance.PKGContractorRecBy != null && vendorInsurance.PKGContractorRecBy.Value == default(DateTime))
            {
                vendorInsurance.PKGContractorRecBy = null;
            }
            if (vendorInsurance.PKGReceiveBy != null && vendorInsurance.PKGReceiveBy.Value == default(DateTime))
            {
                vendorInsurance.PKGReceiveBy = null;
            }
            if (vendorInsurance.PKGSent != null && vendorInsurance.PKGSent.Value == default(DateTime))
            {
                vendorInsurance.PKGSent = null;
            }
            vendorInsurance.VendorId = VendorId;
            vendorInsurance.Vendor_InsuranceSource = Vendor_InsuranceSource;

            objVendorVM.VendorInsuranceModel = vendorInsurance;
            LocalizeControls(objVendorVM, LocalizeResourceSetConstants.VendorDetails);
            return PartialView("_AddVendorInsurancePopUp", objVendorVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddVendorInsurance(VendorsVM objVendorVM)
        {
            VendorWrapper mrWrapper = new VendorWrapper(userData);
            string ModelValidationFailedMessage = string.Empty;
            string Mode = string.Empty;
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                if (objVendorVM.VendorInsuranceModel.VendorInsuranceId == 0)
                {
                    Mode = "add";
                }
                var lineItem = mrWrapper.SaveVendorInsurance(objVendorVM.VendorInsuranceModel);
                if (lineItem.ErrorMessages != null && lineItem.ErrorMessages.Count > 0)
                {
                    return Json(lineItem.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), VendorId = objVendorVM.VendorInsuranceModel.VendorId, Mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult VendorInsuranceDelete(long VendorInsuranceId)
        {
            VendorWrapper _VendorObj = new VendorWrapper(userData);
            var deleteResult = _VendorObj.DeleteVendorInsurance(VendorInsuranceId);
            if (deleteResult)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        public PartialViewResult VendorInsuranceWidgetDetails(long VendorId)
        {
            VendorsVM objVendorVM = new VendorsVM();
            VendorWrapper vendorWrapper = new VendorWrapper(userData);
            var vendorInsurance = vendorWrapper.PopulateVendorInsuranceWidgetDetails(VendorId);
            objVendorVM.security = this.userData.Security;

            if (vendorInsurance.InsuranceExpireDate != null && vendorInsurance.InsuranceExpireDate.Value == default(DateTime))
            {
                vendorInsurance.InsuranceExpireDate = null;
            }
            if (vendorInsurance.InsuranceOverrideDate != null && vendorInsurance.InsuranceOverrideDate.Value == default(DateTime))
            {
                vendorInsurance.InsuranceOverrideDate = null;
            }

            objVendorVM.VendorInsuranceWidgetModel = vendorInsurance;
            LocalizeControls(objVendorVM, LocalizeResourceSetConstants.VendorDetails);
            return PartialView("_VendorInsuranceViewWidget", objVendorVM);
        }
        [HttpPost]
        public ActionResult UpdateVendorInsuranceWdget(long VendorId,bool InsuranceOverride)
        {
            VendorWrapper _VendorObj = new VendorWrapper(userData);
            var Result = _VendorObj.VendorUpdateForVendorInsurance(VendorId, InsuranceOverride);
            if (Result.ErrorMessages == null || Result.ErrorMessages.Count == 0)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult EditVendorInsuranceInfo(long vendorId)
        {
            VendorsVM objVendorsVM = new VendorsVM();
            VendorWrapper _VendorObj = new VendorWrapper(userData);
            VendorInsuranceWidgetModel objVen = _VendorObj.PopulateVendorInsuranceWidgetDetails(vendorId);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            var AllLookUpLists = commonWrapper.GetAllLookUpList();
            if (AllLookUpLists != null)
            {
                List<DataContracts.LookupList> objLookupClassCode = AllLookUpLists.Where(x => x.ListName == LookupListConstants.VEN_ClassCode).ToList();
                if (objLookupClassCode != null)
                {
                    objVen.LookupClassCodeList = objLookupClassCode.Select(x => new SelectListItem { Text = x.ListValue + "  -  " + x.Description, Value = x.ListValue });
                }
            }
            objVendorsVM.VendorInsuranceWidgetModel = objVen;
            objVendorsVM.security = this.userData.Security;
            LocalizeControls(objVendorsVM, LocalizeResourceSetConstants.VendorDetails);
            return PartialView("_EditVendorInsuranceInformation", objVendorsVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditVendorInsuranceInformation(VendorsVM _vendor)
        {
            if (ModelState.IsValid)
            {
                VendorWrapper _VendorObj = new VendorWrapper(userData);
                Vendor objVendor = _VendorObj.UpdateForVendorInsuranceInformation(_vendor.VendorInsuranceWidgetModel);
                if (objVendor.ErrorMessages != null && objVendor.ErrorMessages.Count > 0)
                {
                    return Json(objVendor.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), vendorid = objVendor.VendorId }, JsonRequestBehavior.AllowGet);
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

        #region V2-933

        public PartialViewResult VendorAssetManagementWidgetDetails(long VendorId)
        {
            VendorsVM objVendorVM = new VendorsVM();
            VendorWrapper vendorWrapper = new VendorWrapper(userData);
            var vendorAssetMgt = vendorWrapper.PopulateVendorAssetManagementHeader(VendorId);
            objVendorVM.security = this.userData.Security;

            objVendorVM.VendorAssetManagementWidgetModel = vendorAssetMgt;

            LocalizeControls(objVendorVM, LocalizeResourceSetConstants.VendorDetails);
            return PartialView("_AssetManagementViewWidget", objVendorVM);
        }

        [HttpPost]
        public string PopulateVendorAssetManagement(int? draw, int? start, int? length, long _vendorId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            VendorWrapper _VendorObj = new VendorWrapper(userData);
            start = start.HasValue
          ? start / length
          : 0;
            int skip = start * length ?? 0;
            List<VendorAssetMgtModel> viList = _VendorObj.PopulateVendorAssetManagementGrid(skip, length ?? 0, order, orderDir, _vendorId);
            var totalRecords = 0;
            var recordsFiltered = 0;
            if (viList != null && viList.Count > 0)
            {
                recordsFiltered = viList[0].TotalCount;
                totalRecords = viList[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }
            int initialPage = start.Value;
            var filteredResult = viList
              .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);

        }
        public PartialViewResult AddVendorAssetManagement(long VendorId)
        {
            VendorsVM objVendorVM = new VendorsVM();
            VendorAssetMgtModel vendorAssetMgtModel = new VendorAssetMgtModel();
            objVendorVM.security = this.userData.Security;
            vendorAssetMgtModel.VendorId = VendorId;

            objVendorVM.VendorAssetManagementModel = vendorAssetMgtModel;
            LocalizeControls(objVendorVM, LocalizeResourceSetConstants.VendorDetails);
            return PartialView("_AddVendorAssetManagementPopUp", objVendorVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddVendorAssetManagement(VendorsVM objVendorVM)
        {
            VendorWrapper mrWrapper = new VendorWrapper(userData);
            string ModelValidationFailedMessage = string.Empty;
            string Mode = string.Empty;
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                if (objVendorVM.VendorAssetManagementModel.VendorAssetMgtId == 0)
                {
                    Mode = "add";
                }
                var lineItem = mrWrapper.SaveVendorAssetMgt(objVendorVM.VendorAssetManagementModel);
                if (lineItem.ErrorMessages != null && lineItem.ErrorMessages.Count > 0)
                {
                    return Json(lineItem.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), VendorId = objVendorVM.VendorAssetManagementModel.VendorId, Mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult VendorAssetMgtDelete(long VendorAssetMgtId)
        {
            VendorWrapper _VendorObj = new VendorWrapper(userData);
            var deleteResult = _VendorObj.DeleteVendorAssetMgt(VendorAssetMgtId);
            if (deleteResult)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        public PartialViewResult EditVendorAssetMgt(long VendorAssetMgtId, long AssetMgtSource, long VendorId)
        {
            VendorsVM objVendorVM = new VendorsVM();
            VendorWrapper vendorWrapper = new VendorWrapper(userData);
            var VendorAssetMgt = vendorWrapper.PopulateVendorAssetMgtDetails(VendorAssetMgtId);
            objVendorVM.security = this.userData.Security;

            if (VendorAssetMgt.ExpireDate != null && VendorAssetMgt.ExpireDate.Value == default(DateTime))
            {
                VendorAssetMgt.ExpireDate = null;
            }
            if (VendorAssetMgt.EffectiveDate != null && VendorAssetMgt.EffectiveDate.Value == default(DateTime))
            {
                VendorAssetMgt.EffectiveDate = null;
            }
            if (VendorAssetMgt.PKGContactorRecBy != null && VendorAssetMgt.PKGContactorRecBy.Value == default(DateTime))
            {
                VendorAssetMgt.PKGContactorRecBy = null;
            }
            if (VendorAssetMgt.PKGReceiveBy != null && VendorAssetMgt.PKGReceiveBy.Value == default(DateTime))
            {
                VendorAssetMgt.PKGReceiveBy = null;
            }
            if (VendorAssetMgt.PKGSent != null && VendorAssetMgt.PKGSent.Value == default(DateTime))
            {
                VendorAssetMgt.PKGSent = null;
            }
            VendorAssetMgt.VendorId = VendorId;
            VendorAssetMgt.AssetMgtSource = AssetMgtSource;

            objVendorVM.VendorAssetManagementModel = VendorAssetMgt;
            LocalizeControls(objVendorVM, LocalizeResourceSetConstants.VendorDetails);
            return PartialView("_AddVendorAssetManagementPopUp", objVendorVM);
        }
        [HttpPost]
        public ActionResult UpdateVendorAssetMgtOverrideWdget(long VendorId, bool AssetMgtOverride)
        {
            VendorWrapper _VendorObj = new VendorWrapper(userData);
            var Result = _VendorObj.VendorUpdateForVendorAssetMgt(VendorId, AssetMgtOverride);
            if (Result.ErrorMessages == null || Result.ErrorMessages.Count == 0)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

    }
}