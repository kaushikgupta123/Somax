using Client.ActionFilters;
using Client.BusinessWrapper.Common;
using Client.BusinessWrapper.Receipt;
using Client.Common;
using Client.Controllers.Common;
using Client.Models.InventoryReceipt;
using Common.Constants;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Client.Controllers.InventoryReceipts
{
    public class InventoryReceiptsController : SomaxBaseController
    {
        [CheckUserSecurity(securityType = SecurityConstants.Parts_Receipt)]
        public ActionResult Index()
        {
            ReceiptVM objVM = new ReceiptVM();
            objVM.receiptModel = new ReceiptModel();
            //objVM.receiptModel.PartList = new List<SelectListItem>();
            //V2-687
            if (userData.DatabaseKey.Client.UseMultiStoreroom)
            {
                CommonWrapper commonWrapper = new CommonWrapper(userData);
                objVM.receiptModel.StoreroomList = commonWrapper.GetStoreroomList(StoreroomAuthType.ReceivePurchase);
            }
            objVM.receiptModel.MultiStoreroom = userData.DatabaseKey.Client.UseMultiStoreroom;
            // 
            LocalizeControls(objVM, LocalizeResourceSetConstants.PartDetails);
            return View(objVM);
        }
        public JsonResult GetThisPartLookUpList()
        {
            IEnumerable<SelectListItem> items = null;
            var LookUpList = PopulatelookUpListByType("Part");
            if (LookUpList != null)
            {
                items = LookUpList.Select(x => new SelectListItem { Text = x.ChargeToClientLookupId + " - " + x.Description, Value = x.ChargeToClientLookupId });
            }
            return Json(items, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PopulateGrid(string PartClientLookupId, decimal UnitCost, decimal ReceiptQuantity,int count,long StoreroomId=0,string StoreroomName="")
        {
            ReceiptWrapper reWrapper = new ReceiptWrapper(userData);
            count = count - 1;
            GridPartReceiptList obj = reWrapper.AddPartInGrid(PartClientLookupId, UnitCost, ReceiptQuantity, count, StoreroomId, StoreroomName);
            obj.UnitCost = UnitCost;
            obj.ReceiptQuantity = ReceiptQuantity;
            return Json(new { data = obj }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveListRecieptFromGrid(List<GridPartReceiptList> list)
        {
            ReceiptWrapper reWrapper = new ReceiptWrapper(userData);
            List<ErrorModel> returnObj = reWrapper.SaveReceipt(list);
            var result = string.Empty;
            if (returnObj != null && returnObj.Count > 0)
            {
                return Json(new { Result = returnObj }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ValidateData(ReceiptVM receiptVM)
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
    }
}