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
using Client.Models.InventoryPartsIssue;
using Client.BusinessWrapper.InventoryPartsIssue;
using Client.Models.Common;
using Newtonsoft.Json;

namespace Client.Controllers.InventoryPartsIssue
{
    public class InventoryPartsIssueController : SomaxBaseController
    {
        [CheckUserSecurity(securityType = SecurityConstants.Parts_Issue)]
        public ActionResult Index()
        {
            //#region IssueTo
            PartsIssue objPartsIssueModel = new PartsIssue();
            InventoryPartsIssueVM objPartsissueVM = new InventoryPartsIssueVM();
            InventoryPartsIssueWrapper invWrapper = new InventoryPartsIssueWrapper(userData);
          
            if (userData.DatabaseKey.Client.UseMultiStoreroom)
            {
                CommonWrapper commonWrapper = new CommonWrapper(userData);
                objPartsIssueModel.StoreroomList = commonWrapper.GetStoreroomList(StoreroomAuthType.Issue);
            }
            objPartsIssueModel.MultiStoreroom = userData.DatabaseKey.Client.UseMultiStoreroom;
           
            objPartsissueVM.userData = this.userData;
            var personnelList = invWrapper.getPersonnelIssuTo();
            if (personnelList != null)
            {
                objPartsIssueModel.IssueToList = personnelList.Select(x => new SelectListItem
                {
                    Text = x.LookupIdWithName,
                    Value = Convert.ToString(x.PersonnelId),
                });
            }
            //for setting default index of the dropdown
            objPartsIssueModel.selectedPersonnelId = userData.DatabaseKey.Personnel.PersonnelId;

            #region ChargeType
            var ChargeTypeList = UtilityFunction.populateChargeTypeForInventoryCheckout();
            if (ChargeTypeList != null)
            {
                
                    ChargeTypeList = ChargeTypeList.Where(x => x.text != "Location").ToList();
                    objPartsIssueModel.ChargeTypeList = ChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
               
            }
            #endregion
            objPartsissueVM.partsIssue = objPartsIssueModel;
            LocalizeControls(objPartsissueVM, LocalizeResourceSetConstants.PartDetails);
            return View(objPartsissueVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SavePartsIssue(InventoryPartsIssueVM objPartsissueVM, string cmdBtn)
        {
            InventoryPartsIssueWrapper invWrapper = new InventoryPartsIssueWrapper(userData);
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                var SelectedpersonnelName = invWrapper.getPersonnelIssuTo().Where(m=>m.PersonnelId == objPartsissueVM.partsIssue.selectedPersonnelId).Select(m=>m.ClientLookupId).FirstOrDefault();
                objPartsissueVM.partsIssue.IssueToClentLookupId = string.IsNullOrEmpty(SelectedpersonnelName)? string.Empty : SelectedpersonnelName;
                var partsIssuesdata = invWrapper.SavePartIssueData(objPartsissueVM.partsIssue);
                var partHistory = partsIssuesdata.Item1;
                string errorMsg = partsIssuesdata.Item2;
                if (string.IsNullOrEmpty(errorMsg))
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), btnType = cmdBtn, PersonnelId = userData.DatabaseKey.Personnel.PersonnelId }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.failed.ToString(), btnType = cmdBtn, PersonnelId = userData.DatabaseKey.Personnel.PersonnelId , errorMsg= errorMsg }, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {

                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #region  Wokrorder Chunk LookupList 
        public string GetWorkOrderLookupListChunkSearchForPartsIssue(int? draw, int? start, int? length, string ClientLookupId = "", string Description = "", string ChargeTo = "", string WorkAssigned = "", string Requestor = "", string Status = "")
        {
            List<WorkOrderLookUpModel> modelList = new List<WorkOrderLookUpModel>();
            InventoryPartsIssueWrapper invWrapper = new InventoryPartsIssueWrapper(userData);
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            start = start.HasValue
              ? start / length
              : 0;
            int skip = start * length ?? 0;

            ClientLookupId = !string.IsNullOrEmpty(ClientLookupId) ? ClientLookupId.Trim() : string.Empty;
            Description = !string.IsNullOrEmpty(Description) ? Description.Trim() : string.Empty;
            ChargeTo = !string.IsNullOrEmpty(ChargeTo) ? ChargeTo.Trim() : string.Empty;
            WorkAssigned = !string.IsNullOrEmpty(WorkAssigned) ? WorkAssigned.Trim() : string.Empty;
            Requestor = !string.IsNullOrEmpty(Requestor) ? Requestor.Trim() : string.Empty;
            Status = !string.IsNullOrEmpty(Status) ? Status.Trim() : string.Empty;

            modelList = invWrapper.GetWorkOrderLookupListGridData(order, orderDir, skip, length.Value, ClientLookupId, Description, ChargeTo, WorkAssigned, Requestor, Status);
            long totalRecords = 0;
            long recordsFiltered = 0;
            if (modelList != null && modelList.Count > 0)
            {
                totalRecords = modelList[0].TotalCount;
                recordsFiltered = modelList[0].TotalCount;
            }

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = modelList });
        }

        #endregion
        #region Mobile
     
        public JsonResult GetPartLookupListchunksearch_Mobile(int Start, int Length, string Search = "", string Storeroomid = "")
        {
            var modelList = new List<PartXRefGridDataModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);

            string order = "0"; //Request.Form.GetValues("order[0][column]")[0];
            string orderDir = "asc"; //Request.Form.GetValues("order[0][dir]")[0];

            modelList = commonWrapper.GetPartLookupListGridData_Mobile(order, orderDir, Start, Length, Search, Search, Storeroomid);

            return Json(new { recordsTotal = modelList.Count() > 0 ? modelList[0].TotalCount : 0, data = modelList });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SavePartsIssue_Mobile(InventoryPartsIssueVM objPartsissueVM, string cmdBtn)
        {
            InventoryPartsIssueWrapper invWrapper = new InventoryPartsIssueWrapper(userData);
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                var SelectedpersonnelName = invWrapper.getPersonnelIssuTo().Where(m => m.PersonnelId == objPartsissueVM.partsIssue.selectedPersonnelId).Select(m => m.ClientLookupId).FirstOrDefault();
                objPartsissueVM.partsIssue.IssueToClentLookupId = string.IsNullOrEmpty(SelectedpersonnelName) ? string.Empty : SelectedpersonnelName;
                var partsIssuesdata = invWrapper.SavePartIssueData(objPartsissueVM.partsIssue);
                var partHistory = partsIssuesdata.Item1;
                string errorMsg = partsIssuesdata.Item2;
                if (string.IsNullOrEmpty(errorMsg))
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), btnType = cmdBtn, PersonnelId = userData.DatabaseKey.Personnel.PersonnelId }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.failed.ToString(), btnType = cmdBtn, PersonnelId = userData.DatabaseKey.Personnel.PersonnelId, errorMsg = errorMsg }, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {

                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetWorkOrderLookupListchunksearch_Mobile(int Start, int Length, string Search = "")
        {
            List<WorkOrderLookUpModel> modelList = new List<WorkOrderLookUpModel>();
            InventoryPartsIssueWrapper invWrapper = new InventoryPartsIssueWrapper(userData);
            string order = "0";
            string orderDir = "asc";

            var ClientLookupId = !string.IsNullOrEmpty(Search) ? Search.Trim() : string.Empty;
            var Description = !string.IsNullOrEmpty(Search) ? Search.Trim() : string.Empty;
            var ChargeTo = !string.IsNullOrEmpty(Search) ? Search.Trim() : string.Empty;

            modelList = invWrapper.GetWorkOrderLookupListGridData(order, orderDir, Start, Length, ClientLookupId, Description, ChargeTo, "", "", "");
            long totalRecords = 0;
            long recordsFiltered = 0;
            if (modelList != null && modelList.Count > 0)
            {
                totalRecords = modelList[0].TotalCount;
                recordsFiltered = modelList[0].TotalCount;
            }

            return Json(new { recordsTotal = totalRecords, data = modelList });
        }
        #endregion
    }
}