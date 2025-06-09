using Client.BusinessWrapper.InventoryCheckout;
using Client.Common;
using Client.Models.InventoryCheckout;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Common.Constants;
using Client.Controllers.Common;
using Client.ActionFilters;
using Client.BusinessWrapper.Common;

namespace Client.Controllers.InventoryPartsCheckout
{
    public class InventoryCheckoutController : SomaxBaseController
    {
        [CheckUserSecurity(securityType = SecurityConstants.Parts_Checkout)]
        public ActionResult Index()
        {
            //#region IssueTo
            InventoryCheckoutModel objInventoryCheckoutModel = new InventoryCheckoutModel();
            InventoryCheckVM objInventoryCheckVM = new InventoryCheckVM();
            InventoryCheckoutWrapper invWrapper = new InventoryCheckoutWrapper(userData);
            objInventoryCheckVM.userData = this.userData;         
            objInventoryCheckVM.inventoryCheckoutModel = objInventoryCheckoutModel;
            LocalizeControls(objInventoryCheckVM, LocalizeResourceSetConstants.PartDetails);
            return View(objInventoryCheckVM);
        }

        public ActionResult SetChargeToLookup(string chargeType = "")
        {
            InventoryCheckoutModel objInventoryCheckoutModel = new InventoryCheckoutModel();
            InventoryCheckVM objInventoryCheckVM = new InventoryCheckVM();
            var chargetToLookUplist = PopulatelookUpListByType(chargeType);
            var jsonResult = Json(new { data = chargetToLookUplist }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [ValidateAntiForgeryToken]
        public JsonResult ValiDateControlls(InventoryCheckVM objInventoryCheckVM)
        {
            if (ModelState.IsValid)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult PopulateInventorySelectTable(long _personnelId, decimal _TransactionQuantity, long _partId, long _chargeToId, string _chargeToClientLookupId, string _chargeType = "", string _comments = "", long _StoreroomId = 0 , string _StoreroomName="")
        {
            InventoryCheckoutWrapper invWrapper = new InventoryCheckoutWrapper(userData);
            var personnelList = invWrapper.FillIssuTo();
            string issueToClientLookupId = string.Empty;
            string chargeToClientLookupId = string.Empty;
            if (personnelList != null)
            {
                issueToClientLookupId = personnelList.Where(x => x.PersonnelId == _personnelId).Select(a => a.ClientLookupId).FirstOrDefault().ToString();
            }

            chargeToClientLookupId = _chargeToClientLookupId;//PopulatelookUpListByType(_chargeType);
            var partList = invWrapper.populatePartDetails(_partId, _StoreroomId);
            //dynamic PartStoreroomId = invWrapper.GetStoreroomId(_partId);
            long PartStoreroomId = partList.PartStoreroomId;
            InventoryCheckoutValidationModel objInventoryCheckoutValidationModel = new InventoryCheckoutValidationModel();

            objInventoryCheckoutValidationModel.partHistoryModel = invWrapper.ValidateSelectedItems(issueToClientLookupId, PartStoreroomId, _chargeType, chargeToClientLookupId, _chargeToId, _TransactionQuantity, partList.ClientLookupId, _partId, partList.Description, partList.UPCCode, _comments);

            if (objInventoryCheckoutValidationModel != null && objInventoryCheckoutValidationModel.partHistoryModel.ErrorMessages.Count > 0)
            {
                objInventoryCheckoutValidationModel.IssueToClientLookupId = issueToClientLookupId;
                objInventoryCheckoutValidationModel.chargeToClientLookupId = _chargeToClientLookupId;
                objInventoryCheckoutValidationModel.objPart = partList;
                objInventoryCheckoutValidationModel.PartStoreroomId = PartStoreroomId;
                objInventoryCheckoutValidationModel.Comments = _comments;
                objInventoryCheckoutValidationModel.StoreroomId = _StoreroomId;
                objInventoryCheckoutValidationModel.StoreroomName = _StoreroomName;
                foreach (var v in objInventoryCheckoutValidationModel.partHistoryModel.ErrorMessages)
                {
                    objInventoryCheckoutValidationModel.ErrorMsg += v + ",";
                }
            }
            else
            {
                objInventoryCheckoutValidationModel.IssueToClientLookupId = issueToClientLookupId;
                objInventoryCheckoutValidationModel.chargeToClientLookupId = chargeToClientLookupId;
                objInventoryCheckoutValidationModel.objPart = partList;
                objInventoryCheckoutValidationModel.PartStoreroomId = PartStoreroomId;
                objInventoryCheckoutValidationModel.Comments = _comments;
                objInventoryCheckoutValidationModel.StoreroomId = _StoreroomId;
                objInventoryCheckoutValidationModel.StoreroomName = _StoreroomName;
                objInventoryCheckoutValidationModel.ErrorMsg = null;
            }
            return Json(objInventoryCheckoutValidationModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ConfirmInventorydata(List<InventoryCheckoutModel> dataList)
        {
            InventoryCheckoutWrapper invWrapper = new InventoryCheckoutWrapper(userData);
            var partHistoryListTemp = invWrapper.ConfirmData(dataList);
            return Json(partHistoryListTemp, JsonRequestBehavior.AllowGet);
        }

        #region return part
        [HttpPost]
        public PartialViewResult LoadReturnPart()
        {
            InventoryCheckoutModel objInventoryCheckoutModel = new InventoryCheckoutModel();
            InvenroryCheckoutReturnModel objInventoryCheckoutReturnModel = new InvenroryCheckoutReturnModel();
            InventoryCheckVM objInventoryCheckVM = new InventoryCheckVM();
            InventoryCheckoutWrapper invWrapper = new InventoryCheckoutWrapper(userData);
            objInventoryCheckVM.userData = this.userData;
            var personnelList = invWrapper.FillIssuTo();
            if (personnelList != null)
            {
                objInventoryCheckoutModel.IssueToList = personnelList.Select(x => new SelectListItem
                {
                    Text = x.LookupIdWithName,
                    Value = Convert.ToString(x.PersonnelId),
                });
            }
            //for setting default index of the dropdown
            objInventoryCheckoutReturnModel.selectedPersonnelId = userData.DatabaseKey.Personnel.PersonnelId;

            #region ChargeType
            var ChargeTypeList = UtilityFunction.populateChargeTypeForInventoryCheckout();
            if (ChargeTypeList != null)
            {
                if (userData.DatabaseKey.Client.BusinessType.ToUpper() != BusinessTypeConstants.Facilities)
                {
                    ChargeTypeList = ChargeTypeList.Where(x => x.text != "Location").ToList();
                    objInventoryCheckoutModel.ChargeTypeList = ChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                }
                else
                {
                    objInventoryCheckoutModel.ChargeTypeList = ChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                }
            }
            #endregion
            objInventoryCheckVM.inventoryCheckoutModel = objInventoryCheckoutModel;
            objInventoryCheckVM.inventoryCheckoutReturnModel = objInventoryCheckoutReturnModel;
            //V2-687
            if (userData.DatabaseKey.Client.UseMultiStoreroom)
            {
                CommonWrapper commonWrapper = new CommonWrapper(userData);
                objInventoryCheckoutReturnModel.StoreroomList = commonWrapper.GetStoreroomList(StoreroomAuthType.Issue);
            }
            objInventoryCheckoutReturnModel.MultiStoreroom = userData.DatabaseKey.Client.UseMultiStoreroom;
            // 
            LocalizeControls(objInventoryCheckVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("~/Views/InventoryCheckout/_ReturnPart.cshtml", objInventoryCheckVM);
        }
        [ValidateAntiForgeryToken]
        public JsonResult ValiDateReturnControlls(InventoryCheckVM objInventoryCheckVM)
        {
            if (ModelState.IsValid)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult ReturnConfirmInventorydata(List<InventoryCheckoutModel> dataList)
        {
            InventoryCheckoutWrapper invWrapper = new InventoryCheckoutWrapper(userData);
            var partHistoryListTemp = invWrapper.PartReturnConfirmData(dataList);
            return Json(partHistoryListTemp, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Load Issue Parts
        [HttpPost]
        public PartialViewResult LoadIssueParts()
        {
            InventoryCheckoutModel objInventoryCheckoutModel = new InventoryCheckoutModel();
            InventoryCheckVM objInventoryCheckVM = new InventoryCheckVM();
            InventoryCheckoutWrapper invWrapper = new InventoryCheckoutWrapper(userData);
            //V2-687
            if (userData.DatabaseKey.Client.UseMultiStoreroom)
            {
                CommonWrapper commonWrapper = new CommonWrapper(userData);
                objInventoryCheckoutModel.StoreroomList = commonWrapper.GetStoreroomList(StoreroomAuthType.Issue); 
            }
            objInventoryCheckoutModel.MultiStoreroom = userData.DatabaseKey.Client.UseMultiStoreroom;
            // 
            objInventoryCheckVM.userData = this.userData;
            var personnelList = invWrapper.FillIssuTo();
            if (personnelList != null)
            {
                objInventoryCheckoutModel.IssueToList = personnelList.Select(x => new SelectListItem
                {
                    Text = x.LookupIdWithName,
                    Value = Convert.ToString(x.PersonnelId),
                });
            }
            //for setting default index of the dropdown
            objInventoryCheckoutModel.selectedPersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
          
            #region ChargeType
            var ChargeTypeList = UtilityFunction.populateChargeTypeForInventoryCheckout();
            if (ChargeTypeList != null)
            {
                if (userData.DatabaseKey.Client.BusinessType.ToUpper() != BusinessTypeConstants.Facilities)
                {
                    ChargeTypeList = ChargeTypeList.Where(x => x.text != "Location").ToList();
                    objInventoryCheckoutModel.ChargeTypeList = ChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                }
                else
                {
                    objInventoryCheckoutModel.ChargeTypeList = ChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                }
            }
            #endregion
            objInventoryCheckVM.inventoryCheckoutModel = objInventoryCheckoutModel;
            LocalizeControls(objInventoryCheckVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("~/Views/InventoryCheckout/_IssueParts.cshtml", objInventoryCheckVM);
        }    
        #endregion

    }
}