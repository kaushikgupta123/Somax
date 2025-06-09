using Client.BusinessWrapper.Configuration.UIConfiguration;
using Client.Common;
using Client.Controllers.Common;
using Client.Models.Configuration.UIConfiguration;
using Common.Constants;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Client.Controllers.Configuration.UIConfiguration
{
    public class UiConfigurationController : ConfigBaseController
    {
        // GET: UiConfiguration
        public ActionResult Index()
        {
            UIConfigurationWrapper uiConfigurationWrapper = new UIConfigurationWrapper(userData);
            UiConfigurationVM uiConfigurationVM = new UiConfigurationVM();
            UIViewModel uIViewModel = new UIViewModel();
            var uiViewNameList = uiConfigurationWrapper.GetAllCustomViewName();
            if (uiViewNameList != null)
            {
                uIViewModel.ViewNameLookUpList = uiViewNameList.Select(x => new SelectListItem { Text = x.Name, Value = x.Value.ToString() });
            }
            uiConfigurationVM.uiViewModel = uIViewModel;
            LocalizeControls(uiConfigurationVM, LocalizeResourceSetConstants.UIConfiguration);
            return View("~/Views/Configuration/UIConfiguration/Index.cshtml", uiConfigurationVM);
        }

        #region For Available and Selected list
        public PartialViewResult GetAvailableandSelectedList(long ViewId= 2001)
        {
            UIConfigurationWrapper uiConfigurationWrapper = new UIConfigurationWrapper(userData);
            UiConfigurationVM uiConfigurationVM = new UiConfigurationVM();
            UiViewDetails ViewDetails = new UiViewDetails();
            UIViewModel uIViewModel = new UIViewModel();
            List<AvailableUIConfigurationModel> availableList = new List<AvailableUIConfigurationModel>();
            List<SelectedUIConfigurationMedel> selectedList = new List<SelectedUIConfigurationMedel>();
            Task[] tasks = new Task[3];
            tasks[0] = Task.Factory.StartNew(() => availableList = uiConfigurationWrapper.GetAvailableUilist(ViewId));
            tasks[1] = Task.Factory.StartNew(() => selectedList = uiConfigurationWrapper.GetSelectedUilist(ViewId));
            tasks[2] = Task.Factory.StartNew(() => uIViewModel.uiViewDetails = uiConfigurationWrapper.GetUiViewDetails(ViewId));
            Task.WaitAll(tasks);
            if(!(tasks[0].IsFaulted && tasks[1].IsFaulted && tasks[2].IsFaulted) && tasks[0].IsCompleted && tasks[1].IsCompleted && tasks[2].IsCompleted)
            {
                uiConfigurationVM.AvailableListModel = availableList;
                uiConfigurationVM.SelectedListModel = selectedList;
                uiConfigurationVM.uiViewModel = uIViewModel;
            }
            LocalizeControls(uiConfigurationVM, LocalizeResourceSetConstants.UIConfiguration);
            return PartialView("~/Views/Configuration/UIConfiguration/_AvailableSelectedList.cshtml", uiConfigurationVM);
        }

        #endregion

        #region Update Selected and Available list

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateSelecetdandAvailableList(UiConfigurationVM objUIC)
        {
            UIConfigurationWrapper UICWrapper = new UIConfigurationWrapper(userData);
            var updateResult = new List<string>();
            List<SelectedListParam> dashboardContentModelList = JsonConvert.DeserializeObject<List<SelectedListParam>>(objUIC.hiddenSelectedList);
            objUIC.selectedListParam = dashboardContentModelList;
            updateResult=UICWrapper.UpdateSelectedandAvailableList(objUIC);
            if(updateResult != null && updateResult.Count > 0)
            {
                return Json(updateResult, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.success.ToString(), UiViewId = objUIC.UIViewId }, JsonRequestBehavior.AllowGet);
            }
            
        }
        #endregion
        #region For Selected Coulumn Setting 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateColumnSettingDetails( UiConfigurationVM objUIC)
        {
            
            string ModelValidationFailedMessage = string.Empty;
            if (objUIC.columnSettingConfigModel.DataDictionaryId > 0)
            {
                UIConfigurationWrapper uiConfigurationWrapper = new UIConfigurationWrapper(userData);
                DataContracts.DataDictionary dd = new DataContracts.DataDictionary();
                dd = uiConfigurationWrapper.updateColumnSetting(objUIC.columnSettingConfigModel.DataDictionaryId, objUIC.columnSettingConfigModel.UIConfigurationId, objUIC.columnSettingConfigModel.Required, objUIC.columnSettingConfigModel.ColumnLabel, objUIC.columnSettingConfigModel.DisplayonForm);
                
                if (dd.ErrorMessages != null && dd.ErrorMessages.Count > 0)
                {
                    return Json(dd.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), DataDictionaryId = dd.DataDictionaryId, Required = dd.Required,Columnlabel=dd.ColumnLabel, DisplayonForm = dd.DisplayonForm }, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
       
        //public JsonResult UpdateColumnSettingDetails1(long DataDictionaryId, bool isRequired,string description)
        //{
        //    var errorList = new List<string>();
        //    if (DataDictionaryId > 0)
        //    {
        //        UIConfigurationWrapper uiConfigurationWrapper = new UIConfigurationWrapper(userData);
        //        errorList = uiConfigurationWrapper.updateColumnSetting(DataDictionaryId, isRequired, description);
        //        if (errorList != null && errorList.Count > 0)
        //        {
        //            var returnOjb = new { success = false, errorList = errorList };
        //            return Json(returnOjb, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {

        //            var returnOjb = new { success = true };
        //            return Json(returnOjb, JsonRequestBehavior.AllowGet);
        //        }

        //    }
        //    else
        //    {
        //        var returnOjb = new { success = false, errorList = errorList };
        //        return Json(returnOjb, JsonRequestBehavior.AllowGet);
        //    }
        //}
        public JsonResult removeColumnCardfromSelectedCardUI(long UIConfigurationId)
        {
            var errorList = new List<string>();
            if (UIConfigurationId > 0)
            {
                UIConfigurationWrapper uiConfigurationWrapper = new UIConfigurationWrapper(userData);
                errorList = uiConfigurationWrapper.removeColumnfromSelectedCardUI(UIConfigurationId);
                if (errorList != null && errorList.Count > 0)
                {
                    var returnOjb = new { success = false, errorList = errorList };
                    return Json(returnOjb, JsonRequestBehavior.AllowGet);
                }
                else
                {

                    var returnOjb = new { success = true };
                    return Json(returnOjb, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                var returnOjb = new { success = false, errorList = errorList };
                return Json(returnOjb, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region Add User Define Field
        public JsonResult GetAllDDList(long UiViewId)
        {
            UIConfigurationWrapper uiConfigurationWrapper = new UIConfigurationWrapper(userData);
            List<DataDictionaryModel> DDList = new List<DataDictionaryModel>();
            DDList = uiConfigurationWrapper.GeDataDictionaryRetrieveDetailsByClientId(UiViewId);           
            return Json(new { data = DDList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddSelectUDF(string ListofIds)
        {
            UIConfigurationWrapper uiConfigurationWrapper = new UIConfigurationWrapper(userData);
            uiConfigurationWrapper.AddUDF(ListofIds);
            return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Add Section
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSection(UiConfigurationVM objUICVM)
        {
            List<string> ErrorList = new List<string>();
            UIConfigurationWrapper UICWrapper = new UIConfigurationWrapper(userData);
            string ModelValidationFailedMessage = string.Empty;

            if (ModelState.IsValid)
            {
                DataContracts.UIConfiguration UIC = new DataContracts.UIConfiguration();

                UIC = UICWrapper.AddSection(objUICVM);
                if (UIC.ErrorMessages != null && UIC.ErrorMessages.Count > 0)
                {
                    return Json(UIC.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(),UiViewId=UIC.UIViewId, UiConfigurationId = UIC.UIConfigurationId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Configure Icon
        public PartialViewResult SetConfigurationDetails(long DataDictionaryId, long UIConfigurationId, string ColumnName,string columnlabel,bool IsRequired,bool IsUDF,string ColumnType,string Listname,bool DisplayonForm)
        {
            UiConfigurationVM objUIC = new UiConfigurationVM();
            ColumnSettingConfigModel config = new ColumnSettingConfigModel();
            config.DataDictionaryId = DataDictionaryId;
            config.UIConfigurationId = UIConfigurationId;
            config.ColumnName = ColumnName;
            config.ColumnLabel = columnlabel;
            config.Required = IsRequired;
            config.UDF = IsUDF;
            config.ColumnType = ColumnType;
            config.ListName = Listname;
            config.DisplayonForm = DisplayonForm;
            objUIC.columnSettingConfigModel = config;
            LocalizeControls(objUIC, LocalizeResourceSetConstants.UIConfiguration);
            return PartialView("~/Views/Configuration/UIConfiguration/_ColumnCardConfiguration.cshtml", objUIC);
        }
        #endregion

        #region Get all lookuplist
        //[HttpPost]
        //public string GetUDFLookUpListsGrid(int? draw, int? start, int? length, string DescriptionLookUp)
        //{
        //    string order = Request.Form.GetValues("order[0][column]")[0];
        //    string orderDir = Request.Form.GetValues("order[0][dir]")[0];
        //    var colname = Request.Form.GetValues("columns[" + order + "][name]");
        //    var InitialtotalRecords = 0;
        //    UIConfigurationWrapper UICWrap = new UIConfigurationWrapper(userData);
        //    List<UDFLookupListModel> LookUpListsModelList = UICWrap.populateUDFLookUpList(DescriptionLookUp);
        //    LookUpListsModelList = this.GetLookUpListsModelListGridSortByColumnWithOrder(colname[0], orderDir, LookUpListsModelList);
        //    if (LookUpListsModelList != null)
        //    {
        //        InitialtotalRecords = LookUpListsModelList.Count();
                
        //    }
        //    var totalRecords = 0;
        //    var recordsFiltered = 0;
        //    start = start.HasValue
        //        ? start / length
        //        : 0;
        //    recordsFiltered = LookUpListsModelList.Count();
        //    totalRecords = LookUpListsModelList.Count();
        //    int initialPage = start.Value;
        //    var filteredResult = LookUpListsModelList
        //        .Skip(initialPage * length ?? 0)
        //        .Take(length ?? 0)
        //        .ToList();
        //    return JsonConvert.SerializeObject(new { draw = draw, InitialtotalRecords = InitialtotalRecords, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        //}
        //private List<UDFLookupListModel> GetLookUpListsModelListGridSortByColumnWithOrder(string order, string orderDir, List<UDFLookupListModel> data)
        //{
        //    List<UDFLookupListModel> lst = new List<UDFLookupListModel>();
        //    switch (order)
        //    {
        //        case "0":
        //            lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ListValue).ToList() : data.OrderBy(p => p.ListValue).ToList();
        //            break;
        //        case "1":
        //            lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
        //            break;
        //        default:
        //            lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ListValue).ToList() : data.OrderBy(p => p.ListValue).ToList();
        //            break;
        //    }
        //    return lst;
        //}

        public ActionResult GetLookupListGrid(string DescriptionLookUp)
        {
            UIConfigurationWrapper UICWrap = new UIConfigurationWrapper(userData);
            UiConfigurationVM objUIC = new UiConfigurationVM();
            objUIC.udfLookuplistModelList = UICWrap.populateUDFLookUpList(DescriptionLookUp);
            LocalizeControls(objUIC, LocalizeResourceSetConstants.UIConfiguration);
            return View("~/Views/Configuration/UIConfiguration/_UDFLookuplistDetails.cshtml", objUIC);
        }
        #endregion

        #region Add Edit Delete Lookup
        [HttpGet]
        public PartialViewResult AddEditLookUpLists(string ListName, string DescriptionLookUpText,string ListValue,int updateIndex=0, long LookupListId = 0)
        {
            UiConfigurationVM objUICVM = new UiConfigurationVM();
            UIConfigurationWrapper LWrapper = new UIConfigurationWrapper(userData);
            UDFLookupListModel lookUpListsModel = new UDFLookupListModel();
            lookUpListsModel.LookupListId = LookupListId;
            lookUpListsModel.Description = DescriptionLookUpText;
            lookUpListsModel.ListName = ListName;
            lookUpListsModel.ListValue = ListValue;
            lookUpListsModel.UpdateIndex = updateIndex;
            objUICVM.udfLookuplistModel = lookUpListsModel;
            LocalizeControls(objUICVM, LocalizeResourceSetConstants.UIConfiguration);
            return PartialView("~/Views/Configuration/UIConfiguration/_AddEditUdfLookuplist.cshtml", objUICVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditUDFLookupList(UiConfigurationVM objUICVM)
        {
            List<string> ErrorList = new List<string>();
            UIConfigurationWrapper UICWrapper = new UIConfigurationWrapper(userData);
            string ModelValidationFailedMessage = string.Empty;
            string Mode = string.Empty;
            if (ModelState.IsValid)
            {
                UDFLookupListModel lookUpListsModel = new UDFLookupListModel();
                if (objUICVM.udfLookuplistModel != null && objUICVM.udfLookuplistModel.LookupListId == 0)
                {
                    Mode = "add";
                    lookUpListsModel = UICWrapper.AddLookUpLists(objUICVM.udfLookuplistModel);
                    if (lookUpListsModel.ErrorMessage == "duplicate")
                    {
                        ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("DuplicateStatusMsg", LocalizeResourceSetConstants.StoredProcValidation);
                        return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    lookUpListsModel = UICWrapper.EditLookUpLists(objUICVM.udfLookuplistModel);

                }
                return Json(new { Result = JsonReturnEnum.success.ToString(), LookupListId = lookUpListsModel.LookupListId, mode = Mode, lookuplistName= objUICVM.udfLookuplistModel.ListName }, JsonRequestBehavior.AllowGet);
                
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteLookUpLists(long LookupListId)
        {
            UIConfigurationWrapper UICWrapper = new UIConfigurationWrapper(userData);
            if (UICWrapper.DeleteLookUpLists(LookupListId))
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