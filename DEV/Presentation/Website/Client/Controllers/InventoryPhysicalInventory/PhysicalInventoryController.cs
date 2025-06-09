using Client.ActionFilters;
using Client.BusinessWrapper.Common;
using Client.BusinessWrapper.PhysicalInventory;
using Client.Common;
using Client.Controllers.Common;
using Client.Models.Common;
using Client.Models.PhysicalInventory;
using Common.Constants;
using DataContracts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Client.Controllers.InventoryPhysicalInventory
{
    public class InventoryPhysicalInventoryController : SomaxBaseController
    {
        [CheckUserSecurity(securityType = SecurityConstants.Parts_Physical)]
        public ActionResult Index()
        {
            PhysicalInventoryVM objVM = new PhysicalInventoryVM();
            objVM.inventoryModel = new PhysicalInventoryModel();
            //V2-687
            if (userData.DatabaseKey.Client.UseMultiStoreroom)
            {
                CommonWrapper commonWrapper = new CommonWrapper(userData);
                objVM.inventoryModel.StoreroomList = commonWrapper.GetStoreroomList(StoreroomAuthType.PhysicalInventory);
            }
            objVM.inventoryModel.MultiStoreroom = userData.DatabaseKey.Client.UseMultiStoreroom;
            // 
            //objVM.inventoryModel.PartList = new List<SelectListItem>();
            objVM.udata = userData;
            LocalizeControls(objVM, LocalizeResourceSetConstants.PartDetails);
            return View(objVM);
        }
        
        public JsonResult GetThisPartLookUpList()
        {
            IEnumerable<SelectListItem> items = null;
            var LookUpList = PopulatelookUpListByType("Part");
            if (LookUpList != null)
            {
                LookUpList = LookUpList.Where(a => a.InactiveFlag == false).ToList();
                items = LookUpList.Select(x => new SelectListItem { Text = x.ChargeToClientLookupId + " - " + x.Description, Value = x.ChargeToClientLookupId });
            }
            return Json(items, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ValidateData(PhysicalInventoryVM inventoryVM)
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
            return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PopulateGrid(string PartClientLookupId, decimal QuantityCounted, int Count, long StoreroomId = 0, string StoreroomName = "")
        {
            List<string> errorList = new List<string>();
            PhysicalInventoryWrapper phyWrapper = new PhysicalInventoryWrapper(userData);
            GridPhysicalInventoryList obj = phyWrapper.AddInventoryInGrid(PartClientLookupId, QuantityCounted, Count, ref errorList, StoreroomId, StoreroomName);
            if (errorList != null && errorList.Count > 0)
            {
                return Json(errorList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.success.ToString(), data = obj }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult SaveListPhysicalInventoryFromGrid(List<GridPhysicalInventoryList> list)
        {
            PhysicalInventoryWrapper phyWrapper = new PhysicalInventoryWrapper(userData);
            PartHistory returnObj = phyWrapper.SaveReceipt(list);
            if (returnObj.ErrorMessages != null && returnObj.ErrorMessages.Count > 0)
            {
                return Json(returnObj.ErrorMessages, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #region V2-790
        public PartialViewResult PartLookupListView_Mobile()
        {
            return PartialView("~/Views/InventoryPhysicalInventory/Mobile/_AddPartIdPopupSearchGrid.cshtml");
        }
        public JsonResult GetPartLookupListchunksearch_Mobile(int Start, int Length, string Search = "", string Storeroomid = "")
        {
            var modelList = new List<PartXRefGridDataModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);

            string order = "0"; //Request.Form.GetValues("order[0][column]")[0];
            string orderDir = "asc"; //Request.Form.GetValues("order[0][dir]")[0];

            modelList = commonWrapper.GetPartLookupListGridData_Mobile(order, orderDir, Start, Length, Search, Search, Storeroomid);

            return Json(new { recordsTotal = modelList.Count() > 0 ? modelList[0].TotalCount : 0, data = modelList });
        }
        #endregion
    }
}