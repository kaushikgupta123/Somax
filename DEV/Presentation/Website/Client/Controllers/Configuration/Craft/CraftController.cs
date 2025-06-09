using Client.BusinessWrapper.Configuration.ConfigCraft;
using Client.Common;
using Client.Controllers.Common;
using Client.Models.Configuration.ConfigCraft;
using Client.Models.Configuration.Craft;
using Common.Constants;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Client.Controllers.Configuration.ConfigCraft
{
    public class CraftController : ConfigBaseController
    {
        #region Search
        public ActionResult Index()
        {
            CraftVM objCraftVM = new CraftVM();
            CraftModel objCraftModel = new CraftModel();

            var StatusList = UtilityFunction.InactiveStatusTypesWithBoolValue();
            if (StatusList != null)
            {
                objCraftModel.InactiveFlagList = StatusList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            objCraftVM.craftModel = objCraftModel;
            LocalizeControls(objCraftVM, LocalizeResourceSetConstants.CraftDetails);
            return View("~/Views/Configuration/Craft/Index.cshtml", objCraftVM);
        }
        public string GetCraftGridData(int? draw, int? start, int? length, string ClientLookUpId = "", string Description = "", string ChargeRate = "", string InactiveFlag = "", string srcData = "")
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var colname = Request.Form.GetValues("columns[" + order + "][name]");
            CraftWrapper crWrapper = new CraftWrapper(userData);

            var CraftList = crWrapper.GetCraftList();
            CraftList = this.GetAllCraftSortByColumnWithOrder(colname[0], orderDir, CraftList);
            #region Text-Search
            string filter = srcData;
            if (!string.IsNullOrEmpty(filter))
            {
                filter = filter.ToUpper();
                CraftList = CraftList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookupId) && x.ClientLookupId.ToUpper().Contains(filter))
                                                        || (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(filter))
                                                        || (!string.IsNullOrWhiteSpace(Convert.ToString(x.ChargeRate)) && Convert.ToString(x.ChargeRate).Contains(filter))
                                                        ).ToList();

            }
            #endregion
            CraftList = GetCraftSearchResult(CraftList, ClientLookUpId, Description, ChargeRate, InactiveFlag);
            //CraftList = new List<CraftModel>();
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = CraftList.Count();
            totalRecords = CraftList.Count();

            int initialPage = start.Value;

            var filteredResult = CraftList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
        private List<CraftModel> GetAllCraftSortByColumnWithOrder(string order, string orderDir, List<CraftModel> data)
        {
            List<CraftModel> lst = new List<CraftModel>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ChargeRate).ToList() : data.OrderBy(p => p.ChargeRate).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.InactiveFlag).ToList() : data.OrderBy(p => p.InactiveFlag).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
            }
            return lst;
        }
        private List<CraftModel> GetCraftSearchResult(List<CraftModel> craftList, string ClientLookUpId = "", string Description = "", string ChargeRate = "", string InactiveFlag = "")
        {
            if (craftList != null)
            {
                decimal outRate = 0;
                bool isDecimal = false;
                
                if (!string.IsNullOrEmpty(ClientLookUpId))
                {
                    ClientLookUpId = ClientLookUpId.ToUpper();
                    craftList = craftList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookupId) && x.ClientLookupId.ToUpper().Contains(ClientLookUpId))).ToList();
                }
                if (!string.IsNullOrEmpty(Description))
                {
                    Description = Description.ToUpper();
                    craftList = craftList.Where(x => (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(Description))).ToList();
                }

                if (!string.IsNullOrEmpty(ChargeRate))
                {
                    isDecimal=decimal.TryParse(ChargeRate, out outRate);

                    if (isDecimal)
                    {                        
                        craftList = craftList.Where(x => (!string.IsNullOrWhiteSpace(Convert.ToString(x.ChargeRate)) && x.ChargeRate.Equals(outRate))).ToList();
                    }                    

                }
                if (!string.IsNullOrEmpty(InactiveFlag))
                {
                    bool conInactiveFlag;
                    var convertToBoll = bool.TryParse(InactiveFlag, out conInactiveFlag);
                    if (convertToBoll)
                    {
                        craftList = craftList.Where(x => x.InactiveFlag.Equals(conInactiveFlag)).ToList();
                    }

                }
            }
            return craftList;
        }
        #endregion

        #region Details
        public PartialViewResult CraftDetails(long CraftId=0)
        {
            CraftVM objCraftVM = new CraftVM();
            CraftModel objCraftModel = new CraftModel();
            CraftWrapper cw = new CraftWrapper(userData);
            objCraftModel = cw.GetCraftByCraftId(CraftId);
            objCraftVM.craftModel = objCraftModel;

            //objCraftVM.c
            LocalizeControls(objCraftVM, LocalizeResourceSetConstants.CraftDetails);
            return PartialView("~/Views/Configuration/Craft/_CraftDetails.cshtml", objCraftVM);
        }

        #endregion

        #region AddEdit
        public PartialViewResult AddCraft()
        {
            CraftVM objCraftVM = new CraftVM();
            CraftModel objCraftModel = new CraftModel();

            var StatusList = UtilityFunction.InactiveStatusTypesWithBoolValue();
            if (StatusList != null)
            {
                objCraftModel.InactiveFlagList = StatusList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            objCraftVM.craftModel = objCraftModel;
            LocalizeControls(objCraftVM, LocalizeResourceSetConstants.CraftDetails);
            return PartialView("~/Views/Configuration/Craft/_CraftAddEdit.cshtml", objCraftVM);
        }
        public PartialViewResult EditCraft(long craftID)
        {
            CraftVM objCraftVM = new CraftVM();
            CraftModel objCraftModel = new CraftModel();
            CraftWrapper crWrapper = new CraftWrapper(userData);

            var StatusList = UtilityFunction.InactiveStatusTypesWithBoolValue();

            objCraftVM.craftModel = crWrapper.GetCraftByCraftId(craftID);
            if (StatusList != null)
            {
                objCraftVM.craftModel.InactiveFlagList = StatusList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            LocalizeControls(objCraftVM, LocalizeResourceSetConstants.CraftDetails);
            return PartialView("~/Views/Configuration/Craft/_CraftAddEdit.cshtml", objCraftVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddCraft(CraftVM objCraftVM)
        {
            if (ModelState.IsValid)
            {
                string mode = string.Empty;
                CraftWrapper crWrapper = new CraftWrapper(userData);
                List<string> errorMessage = new List<string>();
                #region Validation
                if (objCraftVM.craftModel.CraftId == 0)
                {
                    errorMessage = crWrapper.ValidateAddUpdate(objCraftVM.craftModel, ActionModeEnum.add.ToString());
                    mode = ActionModeEnum.add.ToString();
                }
                else
                {
                    errorMessage = crWrapper.ValidateAddUpdate(objCraftVM.craftModel, ActionModeEnum.edit.ToString());
                    mode = ActionModeEnum.edit.ToString();
                }
                #endregion

                if (errorMessage != null && errorMessage.Count > 0)
                {
                    return Json(errorMessage, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var addResult = crWrapper.AddEditCraft(objCraftVM.craftModel);
                    return Json(new { Result = JsonReturnEnum.success.ToString(), mode = mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Delete
        [HttpPost]
        public JsonResult DeleteCraft(long CraftId)
        {
            CraftWrapper crWrapper = new CraftWrapper(userData);
            var delResult = crWrapper.DeleteCraft(CraftId);
            if (delResult != null && delResult.ErrorMessages != null && delResult.ErrorMessages.Count > 0)
            {
                return Json(delResult.ErrorMessages, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Printdata
        [HttpGet]
        public string GetCraftPrintData(string colname, string coldir, string _craft, string _description = null, string _rate=null, string _inactiveFlag = "")
        {
            List<CraftPrintModel> craftPrintModelList = new List<CraftPrintModel>();
            CraftPrintModel objcraftPrintModel;
            CraftWrapper mWrapper = new CraftWrapper(userData);
            var CraftList = mWrapper.GetCraftList();
            if (CraftList != null)
            {
                CraftList = this.GetAllCraftSortByColumnWithOrder(colname, coldir, CraftList);
            }
            CraftList = GetCraftSearchResult(CraftList, _craft, _description, _rate, _inactiveFlag);

            foreach (var p in CraftList)
            {
                objcraftPrintModel = new CraftPrintModel();
                objcraftPrintModel.Craft = p.ClientLookupId;
                objcraftPrintModel.Description = p.Description;
                objcraftPrintModel.Rate = p.ChargeRate.ToString();
                objcraftPrintModel.Inactive = p.InactiveFlag;
                craftPrintModelList.Add(objcraftPrintModel);
            }

            return JsonConvert.SerializeObject(new { data = craftPrintModelList }, JsonSerializerDateSettings);
        }
        #endregion
    }
}