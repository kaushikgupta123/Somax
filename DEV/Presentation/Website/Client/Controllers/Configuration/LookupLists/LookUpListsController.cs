using Client.Controllers.Common;
using Client.Models.Configuration.LookupLists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Client.BusinessWrapper.Configuration.LookUpLists;
using Common.Constants;
using System.Data;
using Newtonsoft.Json;
using Client.Common;
namespace Client.Controllers.Configuration.LookupLists
{
    public class LookUpListsController : ConfigBaseController
    {
        #region Search
        public ActionResult Index()
        {
            LookUpListsVM objLookUpListsVM = new LookUpListsVM();
            LookUpListsWrapper LWrapper = new LookUpListsWrapper(userData);
            LookUpListsModel lookUpListsModel = new LookUpListsModel();
            var DescriptionList = LWrapper.GetAllLookUpList();
            if (DescriptionList != null)
            {
                lookUpListsModel.DescriptionLookUpList = DescriptionList.Select(x => new SelectListItem { Text = x.Name, Value = x.Value.ToString() });
            }
            objLookUpListsVM.lookUpListsModel = lookUpListsModel;
            string currentDescription = Convert.ToString(TempData["CURRENTDESCRIPTION"]);
            if (!string.IsNullOrEmpty(currentDescription))
            {
                objLookUpListsVM.lookUpListsModel.DescriptionLookUp = currentDescription;
            }
            LocalizeControls(objLookUpListsVM, LocalizeResourceSetConstants.LookUpListDetails);
            return View("~/Views/Configuration/LookUpLists/index.cshtml", objLookUpListsVM);
        }
        [HttpPost]
        public string GetLookUpListsGrid(int? draw, int? start, int? length, string DescriptionLookUp, string Description, string Value, string SearchText = "")
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var colname = Request.Form.GetValues("columns[" + order + "][name]");
            var InitialtotalRecords = 0;
            LookUpListsWrapper LWrapper = new LookUpListsWrapper(userData);
            List<LookUpListsModel> LookUpListsModelList = LWrapper.populateLookUpList(DescriptionLookUp);
            LookUpListsModelList = this.GetLookUpListsModelListGridSortByColumnWithOrder(colname[0], orderDir, LookUpListsModelList);
            if (LookUpListsModelList != null)
            {
                InitialtotalRecords = LookUpListsModelList.Count();
                SearchText = SearchText.ToUpper();
                LookUpListsModelList = LookUpListsModelList.Where(x => (!string.IsNullOrWhiteSpace(x.ListValue.Trim()) && x.ListValue.Trim().ToUpper().Contains(SearchText))
                                                 || (!string.IsNullOrWhiteSpace(x.Description.Trim()) && x.Description.Trim().ToUpper().Contains(SearchText))
                                                ).ToList();
                if (!string.IsNullOrEmpty(Value))
                {
                    Value = Value.ToUpper();
                    LookUpListsModelList = LookUpListsModelList.Where(x => (!string.IsNullOrWhiteSpace(x.ListValue) && x.ListValue.ToUpper().Contains(Value))).ToList();
                }
                if (!string.IsNullOrEmpty(Description))
                {
                    Description = Description.ToUpper();
                    LookUpListsModelList = LookUpListsModelList.Where(x => (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(Description))).ToList();
                }
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = LookUpListsModelList.Count();
            totalRecords = LookUpListsModelList.Count();
            int initialPage = start.Value;
            var filteredResult = LookUpListsModelList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, InitialtotalRecords = InitialtotalRecords, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
        private List<LookUpListsModel> GetLookUpListsModelListGridSortByColumnWithOrder(string order, string orderDir, List<LookUpListsModel> data)
        {
            List<LookUpListsModel> lst = new List<LookUpListsModel>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ListValue).ToList() : data.OrderBy(p => p.ListValue).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.InactiveFlag).ToList() : data.OrderBy(p => p.InactiveFlag).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ListValue).ToList() : data.OrderBy(p => p.ListValue).ToList();
                    break;
            }
            return lst;
        }
        public JsonResult SetDescription(string value)
        {
            TempData["CURRENTDESCRIPTION"] = value;
            return Json("success", JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Add-Edit-Delete LookUp
        [HttpGet]
        public PartialViewResult AddEditLookUpLists(string DescriptionLookUp, string DescriptionLookUpText, long LookupListId = 0,bool IsReadOnly=false)
        {
            LookUpListsVM objLookUpListsVM = new LookUpListsVM();
            LookUpListsWrapper LWrapper = new LookUpListsWrapper(userData);
            LookUpListsModel lookUpListsModel = new LookUpListsModel();
            if (LookupListId != 0)
            {
                lookUpListsModel = LWrapper.populateLookUpDetails(LookupListId);
            }
            else { lookUpListsModel.LookupListId = LookupListId; }
            var InActiveLookUpList = UtilityFunction.InActiveLookUpList();
            if (InActiveLookUpList != null)
            {
                lookUpListsModel.InactiveFlagList = InActiveLookUpList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            lookUpListsModel.DescriptionLookUp = DescriptionLookUp;
            lookUpListsModel.DescriptionLookUpText = DescriptionLookUpText;
            lookUpListsModel.IsReadOnly = IsReadOnly;
            objLookUpListsVM.lookUpListsModel = lookUpListsModel;
            LocalizeControls(objLookUpListsVM, LocalizeResourceSetConstants.LookUpListDetails);
            return PartialView("~/Views/Configuration/LookupLists/_AddEditLookUpLists.cshtml", objLookUpListsVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditLookUpLists(LookUpListsVM LookUpListsVM)
        {
            string Mode = string.Empty;
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                LookUpListsWrapper LWrapper = new LookUpListsWrapper(userData);
                LookUpListsModel lookUpListsModel = new LookUpListsModel();
                if (LookUpListsVM.lookUpListsModel != null && LookUpListsVM.lookUpListsModel.LookupListId == 0)
                {
                    Mode = "add";
                    lookUpListsModel = LWrapper.AddLookUpLists(LookUpListsVM.lookUpListsModel);
                    if (lookUpListsModel.ErrorMessage == "duplicate")
                    {
                        ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("DuplicateStatusMsg", LocalizeResourceSetConstants.StoredProcValidation);
                        return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    lookUpListsModel = LWrapper.EditLookUpLists(LookUpListsVM.lookUpListsModel);

                }
                return Json(new { Result = JsonReturnEnum.success.ToString(), LookupListId = lookUpListsModel.LookupListId, mode = Mode }, JsonRequestBehavior.AllowGet);
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
            LookUpListsWrapper LWrapper = new LookUpListsWrapper(userData);
            if (LWrapper.DeleteLookUpLists(LookupListId))
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