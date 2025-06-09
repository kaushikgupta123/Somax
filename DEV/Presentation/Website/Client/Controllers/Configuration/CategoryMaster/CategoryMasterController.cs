using Client.ActionFilters;
using Client.BusinessWrapper.Configuration;
using Client.Common;
using Client.Controllers.Common;
using Client.Models.Configuration.CategoryMaster;

using Common.Constants;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Controllers.Configuration.CategoryMaster
{
    public class CategoryMasterController : ConfigBaseController
    {
        // GET: CategoryMaster
        #region Search
        [CheckUserSecurity(securityType = SecurityConstants.PartCategoryMaster)]
        public ActionResult Index()
        {
            CategoryMasterVM cVM = new CategoryMasterVM();
            cVM.security = userData.Security;
            LocalizeControls(cVM, LocalizeResourceSetConstants.Global);
            return View("~/Views/Configuration/CategoryMaster/index.cshtml", cVM);
        }

        public string GetGridDataforCategoryMaster(int? draw, int? start, int? length = 0, string ClientLookupId = "", string Description = "", bool Inactive = false, string Order = "0")
        {
            List<CategoryMasterModel> mCategoryMasterModelList = new List<CategoryMasterModel>();
            CategoryMasterVM cVM = new CategoryMasterVM();
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            start = start.HasValue
                ? start / length
                : 0;
            int skip = start * length ?? 0;
            var colname = Request.Form.GetValues("columns[" + order + "][name]");
            List<string> typeList = new List<string>();
            CategoryMasterWrapper cWrapper = new CategoryMasterWrapper(userData);
            List<CategoryMasterModel> cList = cWrapper.GetCategoryList(order, length ?? 0, orderDir, skip, ClientLookupId, Description, Inactive);
            var totalRecords = 0;
            var recordsFiltered = 0;
            
            recordsFiltered = cList.Select(x=>x.TotalCount).FirstOrDefault();
            totalRecords = cList.Select(x => x.TotalCount).FirstOrDefault();
            
            bool showAddBtn = userData.Security.PartCategoryMaster.Create;
            bool showEditBtn = userData.Security.PartCategoryMaster.Edit;
            bool showDeleteBtn = userData.Security.PartCategoryMaster.Delete;
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = cList, typeList = typeList, showAddBtn = showAddBtn, showEditBtn = showEditBtn, showDeleteBtn = showDeleteBtn });
        }
        #endregion

        #region Add/Edit
        [HttpGet]
        public PartialViewResult AddCategoryMaster()
        {
            CategoryMasterWrapper cWrapper = new CategoryMasterWrapper(userData);
            CategoryMasterVM cVM = new CategoryMasterVM();
            CategoryMasterModel categoryMasterModel = new CategoryMasterModel();
            cVM.categoryMasterModel = categoryMasterModel;
            LocalizeControls(cVM, LocalizeResourceSetConstants.Global);
            return PartialView("~/Views/Configuration/CategoryMaster/AddOrEditCategoryMaster.cshtml", cVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOrEditCategoryMaster(CategoryMasterVM objVM, string Command)
        {
            if (ModelState.IsValid)
            {
                CategoryMasterWrapper prtWrapper = new CategoryMasterWrapper(userData);
                string Mode = string.Empty;

                List<String> errorList = prtWrapper.AddOrEditCategoryMasterRecord(objVM.categoryMasterModel);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (objVM.categoryMasterModel.PartCategoryMasterId == 0)
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

        [HttpGet]
        public ActionResult EditCategoryMaster(long PartCategoryMasterId)
        {
            CategoryMasterWrapper cWrapper = new CategoryMasterWrapper(userData);
            CategoryMasterVM categoryMasterVM = new CategoryMasterVM();

            CategoryMasterModel categoryMasterModel = cWrapper.RetrieveCategoryMasterDetailsById(PartCategoryMasterId);
            categoryMasterVM.categoryMasterModel = categoryMasterModel;

            LocalizeControls(categoryMasterVM, LocalizeResourceSetConstants.Global);
            return PartialView("~/Views/Configuration/CategoryMaster/AddOrEditCategoryMaster.cshtml", categoryMasterVM);
        }

        #endregion

        #region Delete
        [HttpPost]
        public ActionResult DeleteCategoryMaster(long PartCategoryMasterId)
        {
            CategoryMasterWrapper _PartsObj = new CategoryMasterWrapper(userData);
            List<String> errorList = _PartsObj.DeleteCategoryMasterRecord(PartCategoryMasterId);
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


        #region Printdata
        [HttpGet]
        public string GetCategoryMasterPrintData(string _colname, string _coldir,  string _partCategoryMasterId = "", string _description = null, bool _inactiveFlag = false)
        {
            List<CategoryMasterPrintModel> categoryMasterPrintModelList = new List<CategoryMasterPrintModel>();
            CategoryMasterPrintModel objCategoryMasterPrintModel;
            CategoryMasterWrapper objCat = new CategoryMasterWrapper(userData);
            var categoryMasterList = objCat.GetCategoryList(_colname, 100000, _coldir, 0, _partCategoryMasterId, _description, _inactiveFlag);
            if (categoryMasterList != null)
            {
                foreach (var p in categoryMasterList)
                {
                    objCategoryMasterPrintModel = new CategoryMasterPrintModel();
                    objCategoryMasterPrintModel.ClientLookupId = p.ClientLookupId;
                    objCategoryMasterPrintModel.Description = p.Description;
                    categoryMasterPrintModelList.Add(objCategoryMasterPrintModel);
                }
            }
            return JsonConvert.SerializeObject(new { data = categoryMasterPrintModelList }, JsonSerializerDateSettings);
        }
        #endregion
    }
}