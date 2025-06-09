using Client.ActionFilters;
using Client.BusinessWrapper.Common;
using Client.BusinessWrapper.PartsManagement;
using Client.Common;
using Client.Controllers.Common;
using Client.Models;
using Client.Models.PartsManagement.PartsManagementRequest;
using Common.Constants;
using DataContracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Controllers.PartsManagement
{
    public class PartsManagementRequestController : SomaxBaseController
    {
        public static CommonWrapper comWrapper { get; set; }

        #region Search
        [CheckUserSecurity(securityType = SecurityConstants.PartMasterRequest)]
        public ActionResult Index()
        {
            comWrapper = new CommonWrapper(userData);
            PartsManagementRequestVM obj = new PartsManagementRequestVM();
            PartsManagementRequestModel partsManagementRequestModel = new PartsManagementRequestModel();
            List<KeyValuePair<string, string>> customList = new List<KeyValuePair<string, string>>();
            customList = DataContracts.CustomQueryDisplay.RetrieveQueryItemsByTableAndLanguage(userData.DatabaseKey, "PartMasterRequest", userData.Site.LocalizationLanguage, userData.Site.LocalizationCulture);
            if (customList != null)
            {
                customList.Insert(0, new KeyValuePair<string, string>("0", "--Select All--"));
                partsManagementRequestModel.PMRStatusList = customList.Select(x => new SelectListItem { Text = x.Key, Value = x.Value });
            }
            #region V2-798
            var StatusList = comWrapper.GetListFromConstVals(LookupListConstants.PMRStatus);
            if (StatusList != null)
            {
                partsManagementRequestModel.StatusList = StatusList.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
            }
            var RequestTpeList = comWrapper.GetListFromConstVals(LookupListConstants.PMRRequestType);
            if (RequestTpeList != null)
            {
                partsManagementRequestModel.RequestTypeList = RequestTpeList.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() }).OrderBy(m => m.Text);
            }
            // V2-989
            var PersonnelLookUplist = GetAllPartManagement_Personnel();
            if (PersonnelLookUplist != null)
            {
                partsManagementRequestModel.CreatedByPersonnelList = PersonnelLookUplist.Select(x => new SelectListItem { Text = x.NameFirst + " " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() });
            }
            #endregion
            obj.partsManagementRequestModel = partsManagementRequestModel;

            //--------Security for drop down-------
            if (userData.Security.PartMasterRequest.Create)
            {
                obj.PmMenuVisibility = true;
            }
            else
            {
                obj.PmMenuVisibility = false;
            }
                LocalizeControls(obj, LocalizeResourceSetConstants.PartsManagementDetail);
            return View(obj);
        }
        [HttpPost]
        public string GetPartmanagementRequestGridData(int? draw, int? start, int? length, int? SearchTextDropID, long? PartId, string Requestor = "", string Justification = "", string RequestType = "", string Status = "", string Manufacturer = "",
   string ManufacturerPartNumber = "") //This method is no longer needed after implementation of V2-798
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var colname = Request.Form.GetValues("columns[" + order + "][name]");
            List<string> StatusList = new List<string>();
            List<string> RequesterList = new List<string>();
            //List<SelectListItem> RequestTypeList = new List<SelectListItem>(); //Commented for V2-798
            List<string> RequestTypeList = new List<string>();
            //IEnumerable<SelectListItem> RequestTypeList=new  ; //Commented for V2-798
            PartsManagementRequestWrapper pWrapper = new PartsManagementRequestWrapper(userData);
            var pTransList = pWrapper.GetPartsManagementRequests(SearchTextDropID ?? 0);
            if (pTransList != null)
            {
                pTransList = this.GetAllPMRequestSortByColumnWithOrder(colname[0], orderDir, pTransList);
                pTransList = GetPartTransferSearchResult(pTransList, PartId, Requestor, Justification, RequestType, Status, Manufacturer, ManufacturerPartNumber);
            }
            if (pTransList != null && pTransList.Count > 0)
            {
                //StatusList = pTransList.Select(r => r.Status_Display).GroupBy(x => x).Select(x => x.First()).ToList(); //Commented for V2-798
                RequesterList = pTransList.Select(r => r.Requester).GroupBy(x => x.ToString()).Select(x => x.First()).ToList();
                //RequestTypeList = pTransList.OrderBy(a => a.LocalizedRequestType).Select(r => r.LocalizedRequestType).GroupBy(x => x.ToString()).Select(x => x.First()).ToList(); //Commented for V2-798
            }
            //var RequestTypes = UtilityFunction.GetPartMasterRequestTypesList();
            //if (RequestTypes != null)
            //{
            //    RequestTypeList = RequestTypes.Select(x => new SelectListItem { Text = x.text, Value = x.value }).ToList();
            //}
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = pTransList.Count();
            totalRecords = pTransList.Count();

            int initialPage = start.Value;

            var filteredResult = pTransList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, StatusList = StatusList, RequesterList = RequesterList, RequestTypeList= RequestTypeList }, JsonSerializerDateSettings);
        }
        private List<PartsManagementRequestModel> GetAllPMRequestSortByColumnWithOrder(string order, string orderDir, List<PartsManagementRequestModel> data)
        {
            List<PartsManagementRequestModel> lst = new List<PartsManagementRequestModel>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PartMasterRequestId).ToList() : data.OrderBy(p => p.PartMasterRequestId).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Requester).ToList() : data.OrderBy(p => p.Requester).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Justification).ToList() : data.OrderBy(p => p.Justification).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.RequestType).ToList() : data.OrderBy(p => p.RequestType).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Status_Display).ToList() : data.OrderBy(p => p.Status_Display).ToList();
                    break;

                case "5":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Manufacturer).ToList() : data.OrderBy(p => p.Manufacturer).ToList();
                    break;
                case "6":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ManufacturerId).ToList() : data.OrderBy(p => p.ManufacturerId).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PartMasterRequestId).ToList() : data.OrderBy(p => p.PartMasterRequestId).ToList();
                    break;
            }
            return lst;
        }
        private List<PartsManagementRequestModel> GetPartTransferSearchResult(List<PartsManagementRequestModel> retList, long? PartId, string Requestor = "", string Justification = "", string RequestType = "", string Status = "", string Manufacturer = "",
   string ManufacturerPartNumber = "")
        {
            if (retList != null)
            {
                if (PartId != null)
                {
                    retList = retList.Where(x => (Convert.ToString(x.PartMasterRequestId).Contains(Convert.ToString(PartId)))).ToList();
                }
                if (!string.IsNullOrEmpty(Requestor))
                {
                    Requestor = Requestor.ToUpper();
                    retList = retList.Where(x => (!string.IsNullOrWhiteSpace(x.Requester) && x.Requester.ToUpper().Equals(Requestor))).ToList();
                }
                if (!string.IsNullOrEmpty(Justification))
                {
                    Justification = Justification.ToUpper();
                    retList = retList.Where(x => (!string.IsNullOrWhiteSpace(x.Justification) && x.Justification.ToUpper().Contains(Justification))).ToList();
                }
                if (!string.IsNullOrEmpty(RequestType))
                {
                    RequestType = RequestType.ToUpper();
                    retList = retList.Where(x => (!string.IsNullOrWhiteSpace(x.LocalizedRequestType) && x.LocalizedRequestType.ToUpper().Equals(RequestType))).ToList();
                }
                if (!string.IsNullOrEmpty(Status))
                {
                    Status = Status.ToUpper();
                    retList = retList.Where(x => (!string.IsNullOrWhiteSpace(x.Status_Display) && x.Status_Display.ToUpper().Equals(Status))).ToList();
                }
                if (!string.IsNullOrEmpty(Manufacturer))
                {
                    Manufacturer = Manufacturer.ToUpper();
                    retList = retList.Where(x => (!string.IsNullOrWhiteSpace(x.Manufacturer) && x.Manufacturer.ToUpper().Contains(Manufacturer))).ToList();
                }
                if (!string.IsNullOrEmpty(ManufacturerPartNumber))
                {
                    ManufacturerPartNumber = ManufacturerPartNumber.ToUpper();
                    retList = retList.Where(x => (!string.IsNullOrWhiteSpace(x.ManufacturerId) && x.ManufacturerId.ToUpper().Contains(ManufacturerPartNumber))).ToList();
                }
            }
            return retList;
        }

        public string GetPartmanagementRequestPrintData(string colname, string coldir, int? SearchTextDropID, long? PartId, long? Requestor = 0, string Justification = "", string RequestType = "", string Status = "", string Manufacturer = "",
string ManufacturerPartNumber = "")
        {
            PartsManagementRequestPrintModel partManagementRequestPrintModel;
            List<PartsManagementRequestPrintModel> partManagementRequestPrintModelList = new List<PartsManagementRequestPrintModel>();
            PartsManagementRequestWrapper pWrapper = new PartsManagementRequestWrapper(userData);
            //var pTransList = pWrapper.GetPartsManagementRequests(SearchTextDropID ?? 0);
            var pTransList = pWrapper.GetPartsManagementRequestsChunkList(SearchTextDropID ?? 0, 0, 1000000, colname, coldir, PartId, Requestor, Justification, RequestType, Status, Manufacturer, ManufacturerPartNumber);
            //if (pTransList != null)
            //{
            //    pTransList = this.GetAllPMRequestSortByColumnWithOrder(colname, coldir, pTransList);
            //    pTransList = GetPartTransferSearchResult(pTransList, PartId, Requestor, Justification, RequestType, Status, Manufacturer, ManufacturerPartNumber);
            //}
            foreach (var item in pTransList)
            {
                partManagementRequestPrintModel = new PartsManagementRequestPrintModel();
                partManagementRequestPrintModel.PartMasterRequestId = item.PartMasterRequestId;
                partManagementRequestPrintModel.Requester = item.Requester;
                partManagementRequestPrintModel.Justification = item.Justification;
                partManagementRequestPrintModel.RequestType = item.LocalizedRequestType;
                partManagementRequestPrintModel.Status_Display = item.Status_Display;
                partManagementRequestPrintModel.Manufacturer = item.Manufacturer;
                partManagementRequestPrintModel.ManufacturerId = item.ManufacturerId;
                partManagementRequestPrintModelList.Add(partManagementRequestPrintModel);
            }
            return JsonConvert.SerializeObject(new { data = partManagementRequestPrintModelList });
        }

        #region AssignPart
        public ActionResult AssignPartMastertoIndusnetBakery()
        {
            PartsManagementRequestVM partsManagementRequestVM = new PartsManagementRequestVM();
            AssignPartMastertoIndusnetBakeryModel assignPartMastertoIndusnetBakeryModel = new AssignPartMastertoIndusnetBakeryModel();
            assignPartMastertoIndusnetBakeryModel.RequestType = PartMasterRequestTypeConstants.Addition;
            partsManagementRequestVM.assignPartMastertoIndusnetBakeryModel = assignPartMastertoIndusnetBakeryModel;

            string spnAssignPartMasterto = string.Empty;
            spnAssignPartMasterto = UtilityFunction.GetMessageFromResource("spnAssignPartMasterto", LocalizeResourceSetConstants.PartsManagementDetail);

            string spnBakery = string.Empty;
            spnBakery = UtilityFunction.GetMessageFromResource("spnBakery", LocalizeResourceSetConstants.PartsManagementDetail);


            partsManagementRequestVM.PageHeader = spnAssignPartMasterto +" "+ userData.Site.Name + " " + spnBakery;
            LocalizeControls(partsManagementRequestVM, LocalizeResourceSetConstants.PartsManagementDetail);
            //return PartialView("_AssignPartMasterModal", partsManagementRequestVM);
            return PartialView("~/Views/PartsManagementRequest/_AssignPartMasterModal.cshtml", partsManagementRequestVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignPartMastertoIndusnetBakery(PartsManagementRequestVM obj)
        {
            string ModelValidationFailedMessage = string.Empty;
            var errors = ModelState.Select(x => x.Value.Errors)
                         .Where(y => y.Count > 0)
                         .ToList();
            if (ModelState.IsValid)
            {
                PartsManagementRequestWrapper pWrapper = new PartsManagementRequestWrapper(userData);
                var result = pWrapper.AssignPartMastertoIndusnetBakery(obj.assignPartMastertoIndusnetBakeryModel);
                if (result != null && result.Count > 0)
                {
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region ReplacePart
        public ActionResult ReplacePart()
        {
            PartsManagementRequestVM partsManagementRequestVM = new PartsManagementRequestVM();
            ReplacePartModal replacePartModal = new ReplacePartModal();
            replacePartModal.RequestType = PartMasterRequestTypeConstants.Replacement;
            partsManagementRequestVM.replacePartModal = replacePartModal;

            string spnReplacePartin = string.Empty;
            spnReplacePartin = UtilityFunction.GetMessageFromResource("spnReplacePartin", LocalizeResourceSetConstants.PartsManagementDetail);


            string spnBakery = string.Empty;
            spnBakery = UtilityFunction.GetMessageFromResource("spnBakery", LocalizeResourceSetConstants.PartsManagementDetail);

            partsManagementRequestVM.PageHeader = spnReplacePartin + " " + userData.Site.Name + " " + spnBakery;
            LocalizeControls(partsManagementRequestVM, LocalizeResourceSetConstants.PartsManagementDetail);
            //return PartialView("_ReplacePartMasterModal", partsManagementRequestVM);
            return PartialView("~/Views/PartsManagementRequest/_ReplacePartMasterModal.cshtml", partsManagementRequestVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReplacePart(ReplacePartModal replacePartModal)
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                PartsManagementRequestWrapper pWrapper = new PartsManagementRequestWrapper(userData);
                var result = pWrapper.ReplacePart(replacePartModal);
                if (result != null && result.Count > 0)
                {
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region InactivePart
        public ActionResult InactivePart()
        {
            PartsManagementRequestVM partsManagementRequestVM = new PartsManagementRequestVM();
            InactivePartModel inactivePartModel = new InactivePartModel();
            inactivePartModel.RequestType = PartMasterRequestTypeConstants.Inactivation;
            partsManagementRequestVM.inactivePartModel = inactivePartModel;
            string spnInactivatePartin = string.Empty;
            spnInactivatePartin = UtilityFunction.GetMessageFromResource("spnInactivatePartin", LocalizeResourceSetConstants.PartsManagementDetail);

            string spnBakery = string.Empty;
            spnBakery = UtilityFunction.GetMessageFromResource("spnBakery", LocalizeResourceSetConstants.PartsManagementDetail);

            partsManagementRequestVM.PageHeader = spnInactivatePartin + " " + userData.Site.Name + " " + spnBakery;
            LocalizeControls(partsManagementRequestVM, LocalizeResourceSetConstants.PartsManagementDetail);
            //return PartialView("_InactivatePartMasterModal", partsManagementRequestVM);
            return PartialView("~/Views/PartsManagementRequest/_InactivatePartMasterModal.cshtml", partsManagementRequestVM);
        }
        [HttpPost]
        public ActionResult InactivePart(InactivePartModel inactivePartModel)
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                PartsManagementRequestWrapper pWrapper = new PartsManagementRequestWrapper(userData);
                var result = pWrapper.InactivePart(inactivePartModel);
                if (result != null && result.Count > 0)
                {
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region ReplaceSXPart
        public ActionResult ReplaceSXPart()
        {
            PartsManagementRequestVM partsManagementRequestVM = new PartsManagementRequestVM();
            ReplaceSXPartModel replaceSXPartModel = new ReplaceSXPartModel();
            replaceSXPartModel.RequestType = PartMasterRequestTypeConstants.SX_Replacement;
            partsManagementRequestVM.replaceSXPartModel = replaceSXPartModel;
            string spnReplaceSXPartin = string.Empty;
            spnReplaceSXPartin = UtilityFunction.GetMessageFromResource("spnReplaceSXPartin ", LocalizeResourceSetConstants.PartsManagementDetail);
            string spnBakery = string.Empty;
            spnBakery = UtilityFunction.GetMessageFromResource("spnBakery", LocalizeResourceSetConstants.PartsManagementDetail);
            partsManagementRequestVM.PageHeader = spnReplaceSXPartin + " " + userData.Site.Name + " " + spnBakery;
            LocalizeControls(partsManagementRequestVM, LocalizeResourceSetConstants.PartsManagementDetail);
            return PartialView("~/Views/PartsManagementRequest/_ReplaceSXPartMasterModal.cshtml", partsManagementRequestVM);
        }
        [HttpPost]
        public ActionResult ReplaceSXPart(ReplaceSXPartModel replaceSXPartModel)
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                PartsManagementRequestWrapper pWrapper = new PartsManagementRequestWrapper(userData);
                var result = pWrapper.ReplaceSXPart(replaceSXPartModel);
                if (result != null && result.Count > 0)
                {
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region AddNewPartMaster
        public PartialViewResult AddNewPartMaster()
        {
            PartsManagementRequestVM partsManagementRequestVM = new PartsManagementRequestVM();
            PartsManagementRequestModel partsManagementRequestModel = new PartsManagementRequestModel();

            partsManagementRequestVM.partsManagementRequestModel = partsManagementRequestModel;
            LocalizeControls(partsManagementRequestVM, LocalizeResourceSetConstants.PartsManagementDetail);
            return PartialView("_AddNewPartMasterModal", partsManagementRequestVM);
        }
        #endregion

        #region AddNew
        public PartialViewResult AddNewPartManagement(string ModeOfAction="")
        {
            PartsManagementRequestVM pmReqVM = new PartsManagementRequestVM();
            PartsManagementRequestModel pmReqModel = new PartsManagementRequestModel();
            PartsManagementRequestWrapper pmrWrapper = new PartsManagementRequestWrapper(userData);

            // UnitofMeasure
            List<DataContracts.LookupList> UoMeasure = new List<DataContracts.LookupList>();
            UoMeasure = pmrWrapper.GetUnitofMeasureList();
            if (UoMeasure != null)
            {
                UoMeasure = UoMeasure.Where(x => x.ListName == LookupListConstants.Unit_Of_Measure).ToList();
                pmReqModel.UnitOfMeasureList = UoMeasure.Select(x => new SelectListItem { Text = x.ListValue.ToString() + "-" + x.Description, Value = x.ListValue.ToString() });
            }
            //Purchase frequency
            var PurchaseFrequencyList = UtilityFunction.GetPurchaseFrequencyList();
            if (PurchaseFrequencyList != null)
            {
                pmReqModel.PurchaseFreqList = PurchaseFrequencyList.Select(x => new SelectListItem { Text = x.text, Value = x.value });
            }
            //Purchase lead time
            var PurchaseLeadTimeList = UtilityFunction.GetPurchaseLeadTimeList();
            if (PurchaseLeadTimeList != null)
            {
                pmReqModel.PurchaseLeadTimeList = PurchaseLeadTimeList.Select(x => new SelectListItem { Text = x.text, Value = x.value });
            }
            //Purchase cost
            var PurchaseCostList = UtilityFunction.GetPurchaseCostList();
            if (PurchaseCostList != null)
            {
                pmReqModel.PurchaseCostList = PurchaseCostList.Select(x => new SelectListItem { Text = x.text, Value = x.value });
            }

            //Show/hide  SOMAX Part No Popup
            string RequestType = string.Empty;
            string PageHeader = string.Empty;
            if(ModeOfAction.Equals("repalcewithnew"))
            {
                RequestType= PartMasterRequestTypeConstants.ECO_Replace;
                string spnReplacePartinSite = string.Empty;
                spnReplacePartinSite = UtilityFunction.GetMessageFromResource("spnReplacePartinSite", LocalizeResourceSetConstants.PartsManagementDetail);

                string spnNewPartMaster = string.Empty;
                spnNewPartMaster = UtilityFunction.GetMessageFromResource("spnNewPartMaster", LocalizeResourceSetConstants.PartsManagementDetail);

                PageHeader = spnReplacePartinSite + " " + userData.Site.Name + " " + spnNewPartMaster;
            }
            else if(ModeOfAction.Equals("repalcewithexisting"))
            {
                string spnReplaceSXPartinSite = string.Empty;
                spnReplaceSXPartinSite = UtilityFunction.GetMessageFromResource("spnReplaceSXPartinSite", LocalizeResourceSetConstants.PartsManagementDetail);

                string spnNewPartMaster = string.Empty;
                spnNewPartMaster = UtilityFunction.GetMessageFromResource("spnNewPartMaster", LocalizeResourceSetConstants.PartsManagementDetail);

                RequestType = PartMasterRequestTypeConstants.ECO_SX_Replace;
                PageHeader = spnReplaceSXPartinSite + " " + userData.Site.Name + " " + spnNewPartMaster;
            }
            else
            {
                string spnAddNewParMasterRequest = string.Empty;
                spnAddNewParMasterRequest = UtilityFunction.GetMessageFromResource("spnAddNewParMasterRequest", LocalizeResourceSetConstants.PartsManagementDetail);



                PageHeader = spnAddNewParMasterRequest;
            }
            pmReqModel.RequestType = RequestType;
            pmReqVM.PageHeader = PageHeader;
            //FOR PART ID POPUP GRID POPULATION
            if (RequestType == PartMasterRequestTypeConstants.ECO_Replace || RequestType == PartMasterRequestTypeConstants.ECO_SX_Replace)
            {
                pmReqVM.SomaxPartNoVisiblity = true;
                if (RequestType == PartMasterRequestTypeConstants.ECO_Replace)
                {
                    pmReqModel.RequestTypeForPopUp = PartMasterRequestTypeConstants.Replacement;
                }
                else if(RequestType == PartMasterRequestTypeConstants.ECO_SX_Replace)
                {
                    pmReqModel.RequestTypeForPopUp = PartMasterRequestTypeConstants.SX_Replacement;
                }
            }
            else
            {
                pmReqVM.SomaxPartNoVisiblity = false;
            }
            
            pmReqVM.partsManagementRequestModel = pmReqModel;
            LocalizeControls(pmReqVM, LocalizeResourceSetConstants.PartsManagementDetail);
            return PartialView("~/Views/PartsManagementRequest/_AddNewPartMasterModal.cshtml", pmReqVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddNewPartManagement(PartsManagementRequestVM pmReqVM)
        {
            PartsManagementRequestModel pmReqModel = new PartsManagementRequestModel();
            var errors = ModelState.Select(x => x.Value.Errors)
                         .Where(y => y.Count > 0)
                         .ToList();
            if (ModelState.IsValid)
            {
                PartsManagementRequestWrapper pmrWrapper = new PartsManagementRequestWrapper(userData);

                List<string> ErrMsg = pmrWrapper.AddPartMr(pmReqVM.partsManagementRequestModel);
                if (ErrMsg != null && ErrMsg.Count > 0)
                {
                    return Json(ErrMsg, JsonRequestBehavior.AllowGet);
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

        #endregion AddNew

        #region ReplacePartWithNewPartMaster
        #endregion

        #region ReplacePartWithExistingPartMaster
        #endregion

        #endregion Search 

        #region Details
        public PartialViewResult GetPartMgmtDetail(long PartMasterRequestId, string RequestType = "", bool delf = false)
        {
            //CommonWrapper commonWrapper = new CommonWrapper(userData);
            PartsManagementRequestVM pmReqVM = new PartsManagementRequestVM();
            PartsManagementRequestModel pmReqModel = new PartsManagementRequestModel();
            PartMRequestSendApprovalModel objPartMRequestSendApprovalModel = new PartMRequestSendApprovalModel();

            PartMRequestReturnRequesterModel objPartMRequestReturnRequesterModel = new PartMRequestReturnRequesterModel();
            PartMRequestDenyModel objPartMRequestDenyModel = new PartMRequestDenyModel();
            string returnUrl = string.Empty;

            List<DataContracts.LookupList> DenyReasonList = new List<DataContracts.LookupList>();

            PartsManagementRequestWrapper pmrWrapper = new PartsManagementRequestWrapper(userData);
            // V2-478 
            // Security Items to include: SecurityConstants.PartMasterRequest_Approve
            // Security Properties      : ItemAccess
            //List<Personnel> personList = new List<Personnel>();
            // V2-478 
            // Use the "GetPersonnelList"
            // Security Items to include: SecurityConstants.PartMasterRequest_Approve
            // Security Properties      : ItemAccess
            //Personnel p = new Personnel();
            //p.SiteId = userData.DatabaseKey.Personnel.SiteId;
            
            //p.ItemAccess = true;
            //p.ItemName = SecurityConstants.PartMasterRequest_Approve;

            //personList = p.RetrieveForLookupListBySecurityItem(userData.DatabaseKey);
            var personList = Get_PersonnelList(SecurityConstants.PartMasterRequest_Approve,"ItemAccess");           

            if (personList != null)
            {
                //objPartMRequestSendApprovalModel.SendToIdList = personList.Select(x => new SelectListItem { Text = x.ClientLookupId + "-" + x.NameFirst + " " + x.NameLast, Value = x.PersonnelId.ToString() }).ToList();
                objPartMRequestSendApprovalModel.SendToIdList = personList.Select(x => new SelectListItem { Text = x.AssignedTo_PersonnelClientLookupId + "-" + x.NameFirst + " " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() }).ToList();
            }
            RequestType = pmrWrapper.CheckRequestType(RequestType);

            objPartMRequestSendApprovalModel.PartMasterRequestId = PartMasterRequestId;
            objPartMRequestSendApprovalModel.RequestType = RequestType;
            objPartMRequestReturnRequesterModel.PartMasterRequestId = PartMasterRequestId;
            objPartMRequestReturnRequesterModel.RequestType = RequestType;
            objPartMRequestDenyModel.PartMasterRequestId = PartMasterRequestId;
            objPartMRequestDenyModel.RequestType = RequestType;
            pmReqVM.partMRequestSendApprovalModel = objPartMRequestSendApprovalModel;
            pmReqVM.partMRequestReturnRequesterModel = objPartMRequestReturnRequesterModel;
            pmReqVM.partMRequestDenyModel = objPartMRequestDenyModel;
            if ((RequestType == PartMasterRequestTypeConstants.Addition) || (RequestType == PartMasterRequestTypeConstants.Replacement) ||
                   (RequestType == PartMasterRequestTypeConstants.Inactivation) || (RequestType == PartMasterRequestTypeConstants.SX_Replacement))
            {
                pmReqModel = pmrWrapper.GetPMRequestDetailSite(PartMasterRequestId);
                pmReqVM.partsManagementRequestModel = pmReqModel;
                returnUrl = "_PartsMRequestsDetails";
            }
            if ((RequestType == PartMasterRequestTypeConstants.ECO_New) || (RequestType == PartMasterRequestTypeConstants.ECO_Replace) ||
               (RequestType == PartMasterRequestTypeConstants.ECO_SX_Replace) || (RequestType == ""))
            {
                pmReqModel = pmrWrapper.GetPMRequestDetails(PartMasterRequestId);               
                pmReqModel.ImageURL = PartsManagementRequestImageUrl(PartMasterRequestId);//comWrapper.GetAzureImageUrl(PartMasterRequestId, AttachmentTableConstant.PartMasterRequest);
                pmReqModel.ShowDeleteBtnAfterUpload = true;
                pmReqVM.partsManagementRequestModel = pmReqModel;
                returnUrl = "_PartsMRequestDetailsSite";
            }
            //--------Security for drop down-------
            if (userData.Security.PartMasterRequest.Create)
            {
                pmReqVM.PmMenuVisibility = true;
            }
            else
            {
                pmReqVM.PmMenuVisibility = false;
            }
            LocalizeControls(pmReqVM, LocalizeResourceSetConstants.PartsManagementDetail);
            return PartialView(returnUrl, pmReqVM);
        }
        
        #endregion

        #region EditPartManagementRequest
        public PartialViewResult EditPartManagementRequest(long PartMasterRequestId, string RequestType = "")
        {
            PartsManagementRequestVM pmReqVM = new PartsManagementRequestVM();
            PartsManagementRequestModel pmReqModel = new PartsManagementRequestModel();

            PartsManagementRequestDetailModel partsManagementRequestDetailModel = new PartsManagementRequestDetailModel();
            AssignPartMastertoIndusnetBakeryModel assignPartMastertoIndusnetBakeryModel = new AssignPartMastertoIndusnetBakeryModel();
            PartsManagementRequestWrapper pmrWrapper = new PartsManagementRequestWrapper(userData);
            RequestType = pmrWrapper.CheckRequestType(RequestType);
            string returnUrl = string.Empty;
            #region V2-798
            if (RequestType == PartMasterRequestTypeConstants.Addition)
            {
                pmReqModel = pmrWrapper.GetPMRequestDetailSite(PartMasterRequestId);
                assignPartMastertoIndusnetBakeryModel.PartMasterRequestId = pmReqModel.PartMasterRequestId;
                assignPartMastertoIndusnetBakeryModel.Justification = pmReqModel.Justification;
                assignPartMastertoIndusnetBakeryModel.RequestType = pmReqModel.RequestType;
                assignPartMastertoIndusnetBakeryModel.Part_ClientLookupId = pmReqModel.Part_ClientLookupId;
                assignPartMastertoIndusnetBakeryModel.PartMaster_ClientLookupId = pmReqModel.PartMaster_ClientLookupId;
                assignPartMastertoIndusnetBakeryModel.PartMaster_LongDescription = pmReqModel.PartMaster_LongDescription;
                #region V2-798
                //if (RequestType == PartMasterRequestTypeConstants.Addition)
                //{
                assignPartMastertoIndusnetBakeryModel.UnitCost = pmReqModel.UnitCost;
                assignPartMastertoIndusnetBakeryModel.Location = pmReqModel.Location;
                assignPartMastertoIndusnetBakeryModel.QtyMinimum = pmReqModel.QtyMinimum;
                assignPartMastertoIndusnetBakeryModel.QtyMaximum = pmReqModel.QtyMaximum;
                //}
                #endregion
                pmReqVM.assignPartMastertoIndusnetBakeryModel = assignPartMastertoIndusnetBakeryModel;
                returnUrl = "_PartMasterRequestEditForAssign";
            }
            #endregion
            if ((RequestType == PartMasterRequestTypeConstants.Replacement) ||
                 (RequestType == PartMasterRequestTypeConstants.Inactivation) || (RequestType == PartMasterRequestTypeConstants.SX_Replacement))
            {
                pmReqModel = pmrWrapper.GetPMRequestDetailSite(PartMasterRequestId);
                partsManagementRequestDetailModel.PartMasterRequestId = pmReqModel.PartMasterRequestId;
                partsManagementRequestDetailModel.Justification = pmReqModel.Justification;
                partsManagementRequestDetailModel.RequestType = pmReqModel.RequestType;
                partsManagementRequestDetailModel.Part_ClientLookupId = pmReqModel.Part_ClientLookupId;
                partsManagementRequestDetailModel.PartMaster_ClientLookupId = pmReqModel.PartMaster_ClientLookupId;
                partsManagementRequestDetailModel.PartMaster_LongDescription = pmReqModel.PartMaster_LongDescription;
                #region V2-874
                partsManagementRequestDetailModel.Part_ClientLookupId = pmReqModel.Part_ClientLookupId;
                partsManagementRequestDetailModel.Part_Description = pmReqModel.Part_Description;
                #endregion
                pmReqVM.partsManagementRequestDetailModel = partsManagementRequestDetailModel;
                returnUrl = "_PartMasterRequestEdit";
            }
            if ((RequestType == PartMasterRequestTypeConstants.ECO_New) || (RequestType == PartMasterRequestTypeConstants.ECO_Replace) ||
               (RequestType == PartMasterRequestTypeConstants.ECO_SX_Replace) || (RequestType == ""))
            {
                pmReqModel = pmrWrapper.GetPMRequestDetails(PartMasterRequestId);

                var daysOfWeek = UtilityFunction.DaysOfWeekList();

                var PurchaseCostList = UtilityFunction.GetPurchaseCostList();
                if (PurchaseCostList != null)
                {
                    pmReqModel.PurchaseCostList = PurchaseCostList.Select(x => new SelectListItem { Text = x.text, Value = x.value });
                }

                var PurchaseLeadTimeList = UtilityFunction.GetPurchaseLeadTimeList();
                if (PurchaseLeadTimeList != null)
                {
                    pmReqModel.PurchaseLeadTimeList = PurchaseLeadTimeList.Select(x => new SelectListItem { Text = x.text, Value = x.value });
                }
                var PurchaseFrequencyList = UtilityFunction.GetPurchaseFrequencyList();
                if (PurchaseFrequencyList != null)
                {
                    pmReqModel.PurchaseFreqList = PurchaseFrequencyList.Select(x => new SelectListItem { Text = x.text, Value = x.value });
                }
                //pmReqModel.ImageURL = pmrWrapper.GetPartManagementRequestImageUrl(pmReqModel);
                pmReqModel.ImageURL = PartsManagementRequestImageUrl(PartMasterRequestId);// comWrapper.GetAzureImageUrl(PartMasterRequestId, AttachmentTableConstant.PartMasterRequest);
                //Configure UnitofMeasure
                List<DataContracts.LookupList> UoMeasure = new List<DataContracts.LookupList>();
                UoMeasure = pmrWrapper.GetUnitofMeasureList();
                if (UoMeasure != null)
                {
                    UoMeasure = UoMeasure.Where(x => x.ListName == LookupListConstants.Unit_Of_Measure).ToList();
                    pmReqModel.UnitOfMeasureList = UoMeasure.Select(x => new SelectListItem { Text = x.ListValue.ToString() + "-" + x.Description, Value = x.ListValue.ToString() });
                }

                pmReqVM.partsManagementRequestModel = pmReqModel;
                returnUrl = "_PartMasterRequestSiteEdit";
            }
            LocalizeControls(pmReqVM, LocalizeResourceSetConstants.PartsManagementDetail);
            return PartialView(returnUrl, pmReqVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EditPartManagementRequestSite(PartsManagementRequestVM pmReqVM)
        {
            PartMasterRequest partMasterRequest = new PartMasterRequest();
            if (ModelState.IsValid)
            {
                PartsManagementRequestWrapper pmrWrapper = new PartsManagementRequestWrapper(userData);
                partMasterRequest = pmrWrapper.EditPartManagementRequestSite(pmReqVM.partsManagementRequestModel);
                if (partMasterRequest.ErrorMessages != null && partMasterRequest.ErrorMessages.Count > 0)
                {
                    return Json(partMasterRequest.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), RequestType = partMasterRequest.RequestType, PartMasterRequestId = partMasterRequest.PartMasterRequestId }, JsonRequestBehavior.AllowGet);
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
        public JsonResult EditPartManagementRequest(PartsManagementRequestVM pmReqVM)
        {
            PartMasterRequest partMasterRequest = new PartMasterRequest();
            if (ModelState.IsValid)
            {
                PartsManagementRequestWrapper pmrWrapper = new PartsManagementRequestWrapper(userData);
                partMasterRequest = pmrWrapper.EditPartManagementRequest(pmReqVM.partsManagementRequestDetailModel);
                if (partMasterRequest.ErrorMessages != null && partMasterRequest.ErrorMessages.Count > 0)
                {
                    return Json(partMasterRequest.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), RequestType = partMasterRequest.RequestType, PartMasterRequestId = partMasterRequest.PartMasterRequestId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }

        }


        #endregion EditPartManagementRequest

        #region Attachment 
        [HttpPost]
        public string PopulateMSAttachments(int? draw, int? start, int? length, long PartMasterRequestId,string RequestType)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            //PartsManagementRequestWrapper pmrWrapper = new PartsManagementRequestWrapper(userData);
            //var Attachments = pmrWrapper.PopulatePMRAttachment(PartMasterRequestId, RequestType);
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            var Attachments = objCommonWrapper.PopulateAttachments(PartMasterRequestId, "PartMasterRequest",userData.Security.PartMasterRequest.Edit);
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
        private List<PartsManagementAttachmentModel> GetAllAttachmentsSortByColumnWithOrder(string order, string orderDir, List<PartsManagementAttachmentModel> data)
        {
            List<PartsManagementAttachmentModel> lst = new List<PartsManagementAttachmentModel>();
            if (data != null)
            {
                switch (order)
                {
                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.FileName).ToList() : data.OrderBy(p => p.FileName).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.UploadedBy).ToList() : data.OrderBy(p => p.UploadedBy).ToList();
                        break;
                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CreateDate).ToList() : data.OrderBy(p => p.CreateDate).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.FileName).ToList() : data.OrderBy(p => p.FileName).ToList();
                        break;
                }
            }
            return lst;
        }
        [HttpGet]
        public PartialViewResult AddAttachment(long PartMasterRequestId, string ClientLookupId, string RequestType)
        {
            PartsManagementRequestVM pmReqVM = new PartsManagementRequestVM();
            PartsManagementRequestModel pmReqModel = new PartsManagementRequestModel();
            Models.PartsManagement.PartsManagementRequest.PartsManagementAttachmentModel attachment = new Models.PartsManagement.PartsManagementRequest.PartsManagementAttachmentModel();
            attachment.PartMasterRequestId = PartMasterRequestId;
            attachment.ClientLookupId = ClientLookupId;
            pmReqVM.partsManagementAttachmentModel = attachment;
            pmReqVM.partsManagementAttachmentModel.ClientLookupId = ClientLookupId;
            pmReqVM.partsManagementAttachmentModel.RequestType = RequestType;
            LocalizeControls(pmReqVM, LocalizeResourceSetConstants.PartsManagementDetail);
            return PartialView("_PartMRequestAttachmentAdd", pmReqVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAttachment()
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                Stream stream = Request.Files[0].InputStream;
                var fileName = Request.Files[0].FileName;
                AttachmentModel attachmentModel = new AttachmentModel();
                CommonWrapper objCommonWrapper = new CommonWrapper(userData);
                Attachment fileAtt = new Attachment();
                attachmentModel.FileName = System.IO.Path.GetFileNameWithoutExtension(Request.Files[0].FileName);
                string fileExt = System.IO.Path.GetExtension(Request.Files[0].FileName);
                attachmentModel.FileType = fileExt.Substring(1);

                attachmentModel.FileSize = Request.Files[0].ContentLength;
                attachmentModel.ContentType = Request.Files[0].ContentType;
                attachmentModel.ObjectId = Convert.ToInt64(Request.Form["partsmanagementAttachmentModel.PartMasterRequestId"]);
                attachmentModel.Subject = String.IsNullOrEmpty(Request.Form["partsmanagementAttachmentModel.Description"]) ? "No Subject" : Request.Form["partsmanagementAttachmentModel.Description"];
                attachmentModel.RequestType= String.IsNullOrEmpty(Request.Form["partsmanagementAttachmentModel.RequestType"]) ? "" : Request.Form["partsmanagementAttachmentModel.RequestType"];
                attachmentModel.TableName = "PartMasterRequest";
                bool attachStatus = false;
                bool OnPremise = userData.DatabaseKey.Client.OnPremise;
                if (OnPremise)
                {
                    fileAtt = objCommonWrapper.AddAttachmentOnPremise(attachmentModel, stream, ref attachStatus, userData.Security.PartMasterRequest.Edit);
                }
                else
                {
                    fileAtt = objCommonWrapper.AddAttachment(attachmentModel, stream, ref attachStatus, userData.Security.PartMasterRequest.Edit);
                }
                if (attachStatus)
                {
                    if (fileAtt.ErrorMessages != null && fileAtt.ErrorMessages.Count > 0)
                    {
                        return Json(fileAtt.ErrorMessages, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { Result = JsonReturnEnum.success.ToString(), partMasterRequestId = Convert.ToInt64(Request.Form["partsmanagementAttachmentModel.PartMasterRequestId"]),requestType= Request.Form["partsmanagementAttachmentModel.RequestType"] }, JsonRequestBehavior.AllowGet);
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
        [HttpPost]
        public ActionResult DeleteAttachments(long _fileAttachmentId)
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



        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EditAttachment(PartsManagementRequestVM pmReqVM)
        {
            List<string> errMsg = new List<string>();
            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {

                PartsManagementRequestWrapper pWrapper = new PartsManagementRequestWrapper(userData);
                errMsg = pWrapper.EditAttachment(pmReqVM.partsManagementAttachmentModel);
                if (errMsg != null && errMsg.Count > 0)
                {
                    return Json(errMsg, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), RequestType = pmReqVM.partsManagementAttachmentModel.RequestType, PartMasterRequestId = pmReqVM.partsManagementAttachmentModel.PartMasterRequestId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
       
        #endregion Attachment

        #region Review Log
        [HttpPost]
        public string PopulateReviewLog(int? draw, int? start, int? length, long PartMasterRequestId, DateTime? ReviewDate, string Reviewed_By = "", string Comments = "")
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            PartsManagementRequestWrapper pWrapper = new PartsManagementRequestWrapper(userData);
            var pmrLog = pWrapper.PopulatePMRReviewLog(PartMasterRequestId);
            pmrLog = GetAllReviewLogSortByColumnWithOrder(order, orderDir, pmrLog);
            if (pmrLog != null)
            {
                #region AdvSearch
                if (ReviewDate != null)
                {
                    pmrLog = pmrLog.Where(x => (x.ReviewDate != null && x.ReviewDate.Value.Date.Equals(ReviewDate.Value.Date))).ToList();
                }
                if (!string.IsNullOrEmpty(Reviewed_By))
                {
                    Reviewed_By = Reviewed_By.ToUpper();
                    pmrLog = pmrLog.Where(x => (!string.IsNullOrWhiteSpace(x.Reviewed_By) && x.Reviewed_By.ToUpper().Contains(Reviewed_By))).ToList();
                }
                if (!string.IsNullOrEmpty(Comments))
                {
                    Comments = Comments.ToUpper();
                    pmrLog = pmrLog.Where(x => (!string.IsNullOrWhiteSpace(x.Comments) && x.Comments.ToUpper().Contains(Comments))).ToList();
                }

                #endregion
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = pmrLog.Count();
            totalRecords = pmrLog.Count();
            int initialPage = start.Value;
            var filteredResult = pmrLog
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeSettings);
        }

        private List<PartManagementReviewLog> GetAllReviewLogSortByColumnWithOrder(string order, string orderDir, List<PartManagementReviewLog> data)
        {
            List<PartManagementReviewLog> lst = new List<PartManagementReviewLog>();
            if (data != null)
            {
                switch (order)
                {
                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Reviewed_By).ToList() : data.OrderBy(p => p.Reviewed_By).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Comments).ToList() : data.OrderBy(p => p.Comments).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ReviewDate).ToList() : data.OrderBy(p => p.ReviewDate).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Reviewed_By).ToList() : data.OrderBy(p => p.Reviewed_By).ToList();
                        break;
                }
            }
            return lst;
        }
        #endregion Event log

        #region Photos
        public JsonResult DeleteImageFromAzure(string _PartMasterRequestId, string TableName, bool Profile, bool Image)
        {
            //string rtrMsg = string.Empty;
            //PartsManagementRequestWrapper pWrapper = new PartsManagementRequestWrapper(userData);
            //pWrapper.DeleteImage(Convert.ToInt64(_PartMasterRequestId), TableName, Profile, Image, ref rtrMsg);
            //return Json(rtrMsg, JsonRequestBehavior.AllowGet);
            
            string isSuccess = string.Empty;
            comWrapper.DeleteAzureImage(Convert.ToInt64(_PartMasterRequestId), AttachmentTableConstant.PartMasterRequest, ref isSuccess);
            return Json(isSuccess.ToLower(), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ButtonEvents
        #region SendApproval
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult PartsManagementRequestSendApproval(PartsManagementRequestVM pmReqVM)
        {
            List<string> errMsg = new List<string>();

            if (ModelState.IsValid)
            {

                PartsManagementRequestWrapper pWrapper = new PartsManagementRequestWrapper(userData);
                errMsg = pWrapper.SavePMRSendApproval(pmReqVM.partMRequestSendApprovalModel);
                if (errMsg != null && errMsg.Count > 0)
                {
                    return Json(errMsg, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), RequestType = pmReqVM.partMRequestSendApprovalModel.RequestType, PartMasterRequestId = pmReqVM.partMRequestSendApprovalModel.PartMasterRequestId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion SendApproval

        #region ReturnRequester
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult PartsManagementRequestReturnRequester(PartsManagementRequestVM pmReqVM)
        {
            List<string> errMsg = new List<string>();

            if (ModelState.IsValid)
            {

                PartsManagementRequestWrapper pWrapper = new PartsManagementRequestWrapper(userData);
                errMsg = pWrapper.SavePMRReturnRequester(pmReqVM.partMRequestReturnRequesterModel);
                if (errMsg != null && errMsg.Count > 0)
                {
                    return Json(errMsg, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), RequestType = pmReqVM.partMRequestReturnRequesterModel.RequestType, PartMasterRequestId = pmReqVM.partMRequestReturnRequesterModel.PartMasterRequestId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion ReturnRequester

        #region Deny
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult PartsManagementRequestDeny(PartsManagementRequestVM pmReqVM)
        {
            List<string> errMsg = new List<string>();

            if (ModelState.IsValid)
            {

                PartsManagementRequestWrapper pWrapper = new PartsManagementRequestWrapper(userData);
                errMsg = pWrapper.SavePMRDeny(pmReqVM.partMRequestDenyModel);
                if (errMsg != null && errMsg.Count > 0)
                {
                    return Json(errMsg, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), RequestType = pmReqVM.partMRequestDenyModel.RequestType, PartMasterRequestId = pmReqVM.partMRequestDenyModel.PartMasterRequestId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion Deny

        #region SiteApprove
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult PartsManagementRequestSiteApprove(long PartMasterRequestId)
        {
            List<string> errMsg = new List<string>();

            //if (ModelState.IsValid)
            //{

            PartsManagementRequestWrapper pWrapper = new PartsManagementRequestWrapper(userData);
            errMsg = pWrapper.SaveSiteApprove(PartMasterRequestId);
            if (errMsg != null && errMsg.Count > 0)
            {
                return Json(errMsg, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.success.ToString(), PartMasterRequestId = PartMasterRequestId }, JsonRequestBehavior.AllowGet);
            }
            //}
            //else
            //{
            //    string ModelValidationFailedMessage = string.Empty;
            //    ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.PartTransferDetail);
            //    return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            //}
        }
        #endregion SiteApprove

        #region Cancel
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult PartsManagementRequestCancel(long PartMasterRequestId)
        {
            List<string> errMsg = new List<string>();

            //if (ModelState.IsValid)
            //{

            PartsManagementRequestWrapper pWrapper = new PartsManagementRequestWrapper(userData);
            errMsg = pWrapper.Cancel(PartMasterRequestId);
            if (errMsg != null && errMsg.Count > 0)
            {
                return Json(errMsg, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.success.ToString(), PartMasterRequestId = PartMasterRequestId }, JsonRequestBehavior.AllowGet);
            }
            //}
            //else
            //{
            //    string ModelValidationFailedMessage = string.Empty;
            //    ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.PartTransferDetail);
            //    return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            //}
        }
        #endregion Cancel

        #region EnterpriseApprove
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult PartsManagementRequestEnterpriseApprove(long PartMasterRequestId)
        {
            List<string> errMsg = new List<string>();

            //if (ModelState.IsValid)
            //{

            PartsManagementRequestWrapper pWrapper = new PartsManagementRequestWrapper(userData);
            errMsg = pWrapper.SaveEnterpriseApprove(PartMasterRequestId);
            if (errMsg != null && errMsg.Count > 0)
            {
                return Json(errMsg, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.success.ToString(), PartMasterRequestId = PartMasterRequestId }, JsonRequestBehavior.AllowGet);
            }
            //}
            //else
            //{
            //    string ModelValidationFailedMessage = string.Empty;
            //    ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.PartTransferDetail);
            //    return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            //}
        }
        #endregion EnterpriseApprove
        #endregion ButtonEvents

        #region Commom
        public string PartsManagementRequestImageUrl(long PartMasterRequestId)
        {
            string ImageUrl = string.Empty;
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            bool ClientOnPremise = userData.DatabaseKey.Client.OnPremise;
            if (ClientOnPremise)
            {
                ImageUrl = objCommonWrapper.GetOnPremiseImageUrl(PartMasterRequestId, AttachmentTableConstant.PartMasterRequest);
            }
            else
            {
                ImageUrl = objCommonWrapper.GetAzureImageUrl(PartMasterRequestId, AttachmentTableConstant.PartMasterRequest);
            }
            return ImageUrl;

        }
        #endregion

        #region V2-798
        [HttpPost]
        public string GetPartmanagementRequestGridDataChunkList(int? draw, int? start, int? length, int? SearchTextDropID, long? PartId, long? Requestor, string Justification = "", string RequestType = "", string Status = "", string Manufacturer = "",
   string ManufacturerPartNumber = "")
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var colname = Request.Form.GetValues("columns[" + order + "][name]");
            PartsManagementRequestWrapper pWrapper = new PartsManagementRequestWrapper(userData);
            var pTransList = pWrapper.GetPartsManagementRequestsChunkList(SearchTextDropID ?? 0, start ?? 0, length ?? 0, order, orderDir, PartId, Requestor, Justification, RequestType, Status, Manufacturer, ManufacturerPartNumber);

            var totalRecords = 0;
            var recordsFiltered = 0;
            if (pTransList != null && pTransList.Count > 0)
            {
                recordsFiltered = pTransList[0].TotalCount;
                totalRecords = pTransList[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }
            int initialPage = start.Value;
            var filteredResult = pTransList
              .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EditPartManagementRequestForAssign(PartsManagementRequestVM pmReqVM)
        {
            PartMasterRequest partMasterRequest = new PartMasterRequest();
            if (ModelState.IsValid)
            {
                PartsManagementRequestWrapper pmrWrapper = new PartsManagementRequestWrapper(userData);
                partMasterRequest = pmrWrapper.EditPartManagementRequestForAssign(pmReqVM.assignPartMastertoIndusnetBakeryModel);
                if (partMasterRequest.ErrorMessages != null && partMasterRequest.ErrorMessages.Count > 0)
                {
                    return Json(partMasterRequest.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), RequestType = partMasterRequest.RequestType, PartMasterRequestId = partMasterRequest.PartMasterRequestId }, JsonRequestBehavior.AllowGet);
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
    }
}