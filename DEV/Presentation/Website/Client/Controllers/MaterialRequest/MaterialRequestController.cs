using Client.ActionFilters;
using Client.BusinessWrapper;
using Client.BusinessWrapper.Common;
using Client.BusinessWrapper.MultiSitePartSearch;
using Client.Common;
using Client.Controllers.Common;
using Client.Models.MaterialRequest;
using Client.Models.PartLookup;

using Common.Constants;

using Newtonsoft.Json;

using Rotativa;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Controllers.MaterialRequest
{
    public class MaterialRequestController : SomaxBaseController
    {

        #region Search

        [CheckUserSecurity(securityType = SecurityConstants.Parts_MaterialRequest)]
        public ActionResult Index()
        {
            MaterialRequestVM materialRequestVM = new MaterialRequestVM();
            MaterialRequestModel materialRequestModel = new MaterialRequestModel();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            materialRequestVM.security = userData.Security;
            var AcclookUpList = GetAccountByActiveState(true);
            if (AcclookUpList != null)
            {
                materialRequestModel.AccountList = AcclookUpList.Select(x => new SelectListItem { Text = x.Account + " - " + x.Name, Value = x.AccountId.ToString() });
            }
            materialRequestModel.ScheduleWorkList = commonWrapper.PopulateCustomQueryDisplay(AttachmentTableConstant.MaterialRequest);
            materialRequestVM.MaterialRequestModel= materialRequestModel;
            LocalizeControls(materialRequestVM, LocalizeResourceSetConstants.MaterialRequest);
            return View("~/Views/MaterialRequest/index.cshtml", materialRequestVM);
        }
        public string GetMRGridData(int? draw, int? start, int? length, int CustomQueryDisplayId = 0, long? MaterialRequestId=null,string Description=null,DateTime? RequiredDate=null,string AccountClientLookupId = "",string Status="",DateTime? CreateDate=null, DateTime? CompleteDate=null, string Order = "1",string SearchText="")
        {
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var filter = CustomQueryDisplayId;
            start = start.HasValue
          ? start / length
          : 0;
            int skip = start * length ?? 0;
            MaterialRequestWrapper mrWrapper = new MaterialRequestWrapper(userData);
            List<MaterialRequestModel> mRList = mrWrapper.GetMaterialRequestChunkList(CustomQueryDisplayId,skip,length??0,Order,orderDir,MaterialRequestId,Description,Status, AccountClientLookupId, RequiredDate,CreateDate,CompleteDate,SearchText);
            var totalRecords = 0;
            var recordsFiltered = 0;
            if (mRList != null && mRList.Count > 0)
            {
                recordsFiltered = mRList[0].TotalCount;
                totalRecords = mRList[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }
            int initialPage = start.Value;
            var filteredResult = mRList
              .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult}, JsonSerializerDateSettings);
        }

        public ActionResult GetMRInnerGrid(long MaterialRequestID)
        {
            MaterialRequestVM objMaterialRequestVM = new MaterialRequestVM();
            MaterialRequestWrapper mrWrapper = new MaterialRequestWrapper(userData);
            objMaterialRequestVM.listChildGridModel = mrWrapper.PopulateChilditems(MaterialRequestID);
            LocalizeControls(objMaterialRequestVM, LocalizeResourceSetConstants.MaterialRequest);
            return View("_InnerGridMRChildItem", objMaterialRequestVM);
        }

        #region Print Grid
        [HttpGet]
        public string GetMRPrintData(string colname, string coldir, int CustomQueryDisplayId = 0, long? MaterialRequestId = null, string Description = null, DateTime? RequiredDate = null, string AccountClientLookupId = "", string Status = "", DateTime? CreateDate = null, DateTime? CompleteDate = null, string SearchText = "")
        {
            List<MaterialRequestPrintModel> mrSearchModelList = new List<MaterialRequestPrintModel>();
            MaterialRequestPrintModel objMaterialRequestPrintModel;
            MaterialRequestWrapper mrWrapper = new MaterialRequestWrapper(userData);
            List<MaterialRequestModel> mRList = mrWrapper.GetMaterialRequestChunkList(CustomQueryDisplayId, 0, 100000, colname, coldir, MaterialRequestId, Description, Status, AccountClientLookupId, RequiredDate, CreateDate, CompleteDate, SearchText);
            foreach (var p in mRList)
            {
                objMaterialRequestPrintModel = new MaterialRequestPrintModel();
                objMaterialRequestPrintModel.MaterialRequestId = p.MaterialRequestId;
                objMaterialRequestPrintModel.CreateDate = p.CreateDate;
                objMaterialRequestPrintModel.Account_ClientLookupId = p.Account_ClientLookupId;
                objMaterialRequestPrintModel.RequiredDate = p.RequiredDate;
                objMaterialRequestPrintModel.CompleteDate = p.CompleteDate;
                objMaterialRequestPrintModel.Status = p.Status;
                objMaterialRequestPrintModel.Description = p.Description;
                mrSearchModelList.Add(objMaterialRequestPrintModel);
            }
            return JsonConvert.SerializeObject(new { data = mrSearchModelList }, JsonSerializerDateSettings);
        }

        public JsonResult SetPrintData(MRPrintParams mrPrintParams)
        {
            Session["PRINTPARAMS"] = mrPrintParams;
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [EncryptedActionParameter]
        public ActionResult ExportASPDF(string d = "")
        {
            MaterialRequestWrapper mrWrapper = new MaterialRequestWrapper(userData);
            MaterialRequestPDFPrintModel objMRPrintModel;
            MaterialRequestVM objMRVM = new MaterialRequestVM();
            List<MaterialRequestPDFPrintModel> MRPrintModelList = new List<MaterialRequestPDFPrintModel>();
            var locker = new object();

            MRPrintParams MRPrintParams = (MRPrintParams)Session["PRINTPARAMS"];

            List<MaterialRequestModel> pRList = mrWrapper.GetMaterialRequestChunkList(MRPrintParams.CustomQueryDisplayId, 0, 100000, MRPrintParams.colname, MRPrintParams.coldir,
               MRPrintParams.MaterialRequestId, MRPrintParams.Description, MRPrintParams.Status, MRPrintParams.Account_ClientLookupId, MRPrintParams.RequiredDate, MRPrintParams.CreateDate, MRPrintParams.CompleteDate, MRPrintParams.txtSearchval);

            foreach (var p in pRList)
            {
                objMRPrintModel = new MaterialRequestPDFPrintModel();
                objMRPrintModel.MaterialRequestId = p.MaterialRequestId;

                objMRPrintModel.Description = p.Description;
                objMRPrintModel.Status = p.Status;
                objMRPrintModel.Account_ClientLookupId = p.Account_ClientLookupId;
                if (p.CreateDate != null && p.CreateDate != default(DateTime))
                {
                    objMRPrintModel.CreateDateString = p.CreateDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    objMRPrintModel.CreateDateString = "";
                }
                if (p.RequiredDate != null && p.RequiredDate != default(DateTime))
                {
                    objMRPrintModel.RequiredDateString = p.RequiredDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    objMRPrintModel.RequiredDateString = "";
                }
                if (p.CompleteDate != null && p.CompleteDate != default(DateTime))
                {
                    objMRPrintModel.CompleteDateString = p.CompleteDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    objMRPrintModel.CompleteDateString = "";
                }
                if (p.ChildCount > 0)
                {
                    objMRPrintModel.ChildModelList = mrWrapper.PopulateChilditems(p.MaterialRequestId);
                    objMRPrintModel.Total = objMRPrintModel.ChildModelList.Sum(x => x.TotalCost);
                }
                lock (locker)
                {
                    MRPrintModelList.Add(objMRPrintModel);
                }
            }
            objMRVM.materialRequestPDFPrintModel = MRPrintModelList;
            objMRVM.tableHaederProps = MRPrintParams.tableHaederProps;
            LocalizeControls(objMRVM, LocalizeResourceSetConstants.MaterialRequest);
            if (d == "PDF")
            {
                return new PartialViewAsPdf("MRGridPdfPrintTemplate", objMRVM)
                {
                    PageSize = Rotativa.Options.Size.A4,
                    FileName = "Material Request.pdf",
                    PageMargins = { Left = 3, Right = 3, Top = 3, Bottom = 3 },
                };
            }
            else
            {
                return new ViewAsPdf("MRGridPdfPrintTemplate", objMRVM)
                {
                    PageSize = Rotativa.Options.Size.A4,
                    PageMargins = { Left = 3, Right = 3, Top = 3, Bottom = 3 },
                };
            }

        }
        #endregion
        #endregion

        #region MaterialRequest-Add/Edit
        public PartialViewResult AddMaterialRequest()
        {
            MaterialRequestVM objMRVM = new MaterialRequestVM();
            MaterialRequestWrapper mrWrapper = new MaterialRequestWrapper(userData);
            MaterialRequestModel mrModel = new MaterialRequestModel();
            objMRVM.security = this.userData.Security;

            var AcclookUpList = GetAccountByActiveState(true);
            if (AcclookUpList != null)
            {
                mrModel.AccountList = AcclookUpList.Select(x => new SelectListItem { Text = x.Account + " - " + x.Name, Value = x.AccountId.ToString() });
            }
            mrModel.Mode="Add";
            objMRVM.MaterialRequestModel = mrModel;
            LocalizeControls(objMRVM, LocalizeResourceSetConstants.MaterialRequest);
            return PartialView("~/Views/MaterialRequest/_AddMaterialRequest.cshtml", objMRVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddMaterialRequest(MaterialRequestVM mrVM, string Command)
        {
            string ModelValidationFailedMessage = string.Empty;
            string Mode = string.Empty;
            if (ModelState.IsValid)
            {
                MaterialRequestWrapper mrWrapper = new MaterialRequestWrapper(userData);
                if (mrVM.MaterialRequestModel.MaterialRequestId == 0)
                {
                    Mode = "add";
                }
                var objMaterialRequest = mrWrapper.SaveMaterialRequest(mrVM.MaterialRequestModel);

                if (objMaterialRequest.ErrorMessages != null && objMaterialRequest.ErrorMessages.Count > 0)
                {
                    return Json(objMaterialRequest.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Command = Command, MaterialRequestId = objMaterialRequest.MaterialRequestId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
            return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult EditMaterialRequest(long MaterialRequestId)
        {
            MaterialRequestVM objMaterialRequestVM = new MaterialRequestVM();
            MaterialRequestWrapper mrWrapper = new MaterialRequestWrapper(userData);
            MaterialRequestSummaryModel mrSummaryModel = new MaterialRequestSummaryModel();
            CommonWrapper comWrapper = new CommonWrapper(userData);
            objMaterialRequestVM.security = this.userData.Security;
            var materialRequest = mrWrapper.PopulateMaterialRequestDetails(MaterialRequestId);
            mrSummaryModel=GetMaterialRequestSummary(MaterialRequestId);
            var AcclookUpList = GetAccountByActiveState(true);
            if (AcclookUpList != null)
            {
                materialRequest.AccountList = AcclookUpList.Select(x => new SelectListItem { Text = x.Account + " - " + x.Name, Value = x.AccountId.ToString() });
            }
            if(materialRequest.RequiredDate!=null && materialRequest.RequiredDate.Value==default(DateTime))
            {
                materialRequest.RequiredDate=null;
            }
            materialRequest.Mode="Edit";
            objMaterialRequestVM.MaterialRequestModel = materialRequest;
            objMaterialRequestVM.materialRequestSummaryModel = mrSummaryModel;
            ViewBag.IsMaterialRequestDetails=false;
            LocalizeControls(objMaterialRequestVM, LocalizeResourceSetConstants.MaterialRequest);
            return PartialView("~/Views/MaterialRequest/_AddMaterialRequest.cshtml", objMaterialRequestVM);
        }
        #endregion

        #region Material Request Details
        public PartialViewResult MaterialRequestDetails(long MaterialRequestId)
        {
            MaterialRequestVM objMaterialRequestVM = new MaterialRequestVM();
            MaterialRequestWrapper mrWrapper = new MaterialRequestWrapper(userData);
            MaterialRequestModel materialRequestModel = new MaterialRequestModel();
            MaterialRequestSummaryModel mrSummaryModel = new MaterialRequestSummaryModel();
            ApprovalRouteModel approvalRouteModel = new ApprovalRouteModel();
            materialRequestModel=mrWrapper.PopulateMaterialRequestDetails(MaterialRequestId);
            materialRequestModel.IsUseMultiStoreroom=userData.DatabaseKey.Client.UseMultiStoreroom;//V2-732
            mrSummaryModel = GetMaterialRequestSummary(MaterialRequestId);
            var ismaterialrequest = mrWrapper.RetrieveApprovalGroupMaterialRequestStaus();
            approvalRouteModel.IsMaterialRequest=ismaterialrequest;
            objMaterialRequestVM.security = this.userData.Security;
            objMaterialRequestVM.udata = this.userData;

            objMaterialRequestVM.MaterialRequestModel = materialRequestModel;
            objMaterialRequestVM.materialRequestSummaryModel = mrSummaryModel;
            objMaterialRequestVM.ApprovalRouteModel = approvalRouteModel;
            ViewBag.IsMaterialRequestDetails = true;
            LocalizeControls(objMaterialRequestVM, LocalizeResourceSetConstants.MaterialRequest);
            return PartialView("~/Views/MaterialRequest/_MaterialRequestDetails.cshtml", objMaterialRequestVM);
        }

        public MaterialRequestSummaryModel GetMaterialRequestSummary(long MaterialRequestId)
        {
            MaterialRequestWrapper mrWrapper = new MaterialRequestWrapper(userData);
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            MaterialRequestSummaryModel mrSummary = new MaterialRequestSummaryModel();
            var materialRequestDetails = mrWrapper.PopulateMaterialRequestDetails(MaterialRequestId);
            if (materialRequestDetails != null)
            {
                mrSummary.MaterialRequestId = materialRequestDetails.MaterialRequestId;
                mrSummary.Description = materialRequestDetails.Description;
                mrSummary.Status = materialRequestDetails.Status;
                mrSummary.Personnel_NameFirst = materialRequestDetails.Personnel_NameFirst;
                mrSummary.Personnel_NameLast= materialRequestDetails.Personnel_NameLast;
                mrSummary.CreateDate = materialRequestDetails.CreateDate;
                mrSummary.RequiredDate = materialRequestDetails.RequiredDate;
                mrSummary.CompleteDate = materialRequestDetails.CompleteDate;
                mrSummary.Account_ClientLookupId= materialRequestDetails.Account_ClientLookupId;
            }
            return mrSummary;
        }
        #endregion

        #region Grid Item

        public string PopulateMaterialRequestDetailsGrid(int? draw, int? start, int? length, long MaterialRequestId = 0, string PartId = "",
                                    string Description = "", decimal Quantity = 0, string UnitCost = "", string TotalCost = "")
        {
            MaterialRequestVM objMRVM = new MaterialRequestVM();
            MaterialRequestWrapper mrWrapper = new MaterialRequestWrapper(userData);
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var MaterialRequestItems = mrWrapper.PopulateChilditems(MaterialRequestId);
            var IsInitiated = MaterialRequestItems.Where(x => x.Status=="Initiated").Count()>0 ? true : false;
            MaterialRequestItems = this.GetMaterialRequestItemsByColumnWithOrder(order, orderDir, MaterialRequestItems);

            if(MaterialRequestItems!=null)
            {
                if (!string.IsNullOrEmpty(PartId))
                {
                    PartId = PartId.ToUpper();
                    MaterialRequestItems = MaterialRequestItems.Where(x => (!string.IsNullOrWhiteSpace(x.PartClientLookupId) && x.PartClientLookupId.ToUpper().Contains(PartId))).ToList();
                }
                if (!string.IsNullOrEmpty(Description))
                {
                    Description = Description.ToUpper();
                    MaterialRequestItems = MaterialRequestItems.Where(x => (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(Description))).ToList();
                }
                if (Quantity != 0)
                {
                    MaterialRequestItems = MaterialRequestItems.Where(x => x.Quantity.Equals(Quantity)).ToList();
                }
                if (!string.IsNullOrEmpty(UnitCost))
                {
                    decimal number;
                    if (Decimal.TryParse(UnitCost, out number))
                        MaterialRequestItems = MaterialRequestItems.Where(x => x.UnitCost.Equals(number)).ToList();
                }
                if (!string.IsNullOrEmpty(TotalCost))
                {
                    decimal number;
                    if (Decimal.TryParse(TotalCost, out number))
                        MaterialRequestItems = MaterialRequestItems.Where(x => x.TotalCost.Equals(number)).ToList();
                }
             }

            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = MaterialRequestItems.Count();
            totalRecords = MaterialRequestItems.Count();

            int initialPage = start.Value;
            var filteredResult = MaterialRequestItems
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, IsInitiated= IsInitiated }, JsonSerializerDateSettings);
        }
        private List<ChildGridModel> GetMaterialRequestItemsByColumnWithOrder(string order, string orderDir, List<ChildGridModel> data)
        {
            List<ChildGridModel> lst = new List<ChildGridModel>();

            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PartClientLookupId).ToList() : data.OrderBy(p => p.PartClientLookupId).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.UnitCost).ToList() : data.OrderBy(p => p.UnitCost).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Quantity).ToList() : data.OrderBy(p => p.Quantity).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TotalCost).ToList() : data.OrderBy(p => p.TotalCost).ToList();
                    break;

            }
            return lst;
        }
        #endregion

        #region Part Not In Inventory

        public PartialViewResult AddNonPartInInventory(long MaterialRequestId)
        {
            MaterialRequestVM objMaterialRequestVM = new MaterialRequestVM();
            MaterialRequestWrapper pWrapper = new MaterialRequestWrapper(userData);
            var MRDetails = pWrapper.PopulateMaterialRequestDetails(MaterialRequestId);
            PartNotInInventoryModel ChildItems = new PartNotInInventoryModel();
            objMaterialRequestVM.security = this.userData.Security;
            ChildItems.ObjectId = MaterialRequestId;
            ChildItems.ClientId = MRDetails.ClientId;

            objMaterialRequestVM.PartNotInInventoryModel = ChildItems;
            ViewBag.IsMaterialRequestDetails=false;
            LocalizeControls(objMaterialRequestVM, LocalizeResourceSetConstants.MaterialRequest);
            return PartialView("_AddNonPartInInventory", objMaterialRequestVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddPartNonInInventory(MaterialRequestVM objMaterialRequestVM)
        {
            MaterialRequestWrapper mrWrapper = new MaterialRequestWrapper(userData);
            string ModelValidationFailedMessage = string.Empty;
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                var lineItem = mrWrapper.AddPartNotInInventory(objMaterialRequestVM.PartNotInInventoryModel);
                if (lineItem.ErrorMessages != null && lineItem.ErrorMessages.Count > 0)
                {
                    return Json(lineItem.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), MaterialRequestId = objMaterialRequestVM.PartNotInInventoryModel.ObjectId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult EditPartNotInInventory(long EstimatedCostsId, long MaterialRequestId)
        {
            MaterialRequestVM objMaterialRequestVM = new MaterialRequestVM();
            MaterialRequestWrapper mrWrapper = new MaterialRequestWrapper(userData);
            objMaterialRequestVM.security = this.userData.Security;
            #region V2-1148 Add Colomn for Edit Part Not In Inventory
            // Retrieve all lookup values and filter for unit of measure
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            var AllLookUps = commonWrapper.GetAllLookUpList();
            var UNIT_OF_MEASURE = AllLookUps.Where(x => x.ListName == LookupListConstants.Unit_Of_Measure).ToList();
            // Populate unit of measure dropdown list
            if (UNIT_OF_MEASURE != null)
            {
                objMaterialRequestVM.UnitOfmesureListMR = UNIT_OF_MEASURE.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
            }

            // Retrieve line item details for the given estimated cost ID and material request ID
            var childItem = mrWrapper.GetLineItem(EstimatedCostsId, MaterialRequestId);
            objMaterialRequestVM.PartNotInInventoryModel = childItem;
            objMaterialRequestVM.PartNotInInventoryModel.ShoppingCart = userData.Site.ShoppingCart;

            // Check if the user has a shopping cart and the estimate part category is not specified
            if (userData.Site.ShoppingCart && objMaterialRequestVM.PartNotInInventoryModel.CategoryId == 0)
            {
                objMaterialRequestVM.PartNotInInventoryModel.IsAccountClientLookupIdReq = true;
                objMaterialRequestVM.PartNotInInventoryModel.IsPartCategoryClientLookupIdReq = true;
            }
            #endregion
            LocalizeControls(objMaterialRequestVM, LocalizeResourceSetConstants.MaterialRequest);
            return PartialView("~/Views/MaterialRequest/_EditNonPartInInventory.cshtml", objMaterialRequestVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPartNotInInventory(MaterialRequestVM PurchaseRequestVM)
        {
            string ModelValidationFailedMessage = string.Empty;
            MaterialRequestWrapper pWrapper = new MaterialRequestWrapper(userData);
            if (ModelState.IsValid)
            {
                var lineItem = pWrapper.EditPartNotInInventory(PurchaseRequestVM.PartNotInInventoryModel);
                if (lineItem.ErrorMessages != null && lineItem.ErrorMessages.Count > 0)
                {
                    return Json(lineItem.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), MaterialRequestId = PurchaseRequestVM.PartNotInInventoryModel.ObjectId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult DeleteLineItem(long EstimatedCostsId)
        {
            MaterialRequestWrapper mrWrapper = new MaterialRequestWrapper(userData);
            var deleteResult = mrWrapper.DeleteLineItem(EstimatedCostsId);
            if (deleteResult)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Part In Inventory
        public PartialViewResult AddPartInInventory(long MaterialRequestId, long vendorId = 0,long StoreroomId=0)
        {
            PartLookupVM partLookupVM = new PartLookupVM();
            var commonWrapper = new CommonWrapper(userData);
            partLookupVM.MaterialRequestId = MaterialRequestId;
            partLookupVM.VendorId = vendorId;
            partLookupVM.ShoppingCart = userData.Site.ShoppingCart;
            partLookupVM.StoreroomId=StoreroomId;
            LocalizeControls(partLookupVM, LocalizeResourceSetConstants.PartLookUpDetails);
            return PartialView("~/views/partlookup/indexWO.cshtml", partLookupVM);
        }

        public PartialViewResult EditPartInInventory(long EstimatedCostsId, long MaterialRequestId)
        {
            MaterialRequestVM objMaterialRequestVM = new MaterialRequestVM();
            MaterialRequestWrapper mrWrapper = new MaterialRequestWrapper(userData);
            objMaterialRequestVM.security = this.userData.Security;
            var childItem = mrWrapper.GetPartInInventoryItem(EstimatedCostsId, MaterialRequestId);
            objMaterialRequestVM.PartInInventoryModel = childItem;
            #region V2-1148 Add Colomn for Edit Part In Inventory
            // Retrieve all lookup values and filter for unit of measure
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            var AllLookUps = commonWrapper.GetAllLookUpList();
            var UNIT_OF_MEASURE = AllLookUps.Where(x => x.ListName == LookupListConstants.Unit_Of_Measure).ToList();
            // Populate unit of measure dropdown list
            if (UNIT_OF_MEASURE != null)
            {
                objMaterialRequestVM.UnitOfmesureListMR = UNIT_OF_MEASURE.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
            }
            // Check if the user has a shopping cart and the estimate part category is not specified
            if (userData.Site.ShoppingCart && objMaterialRequestVM.PartInInventoryModel.CategoryId == 0)
            {
                objMaterialRequestVM.PartInInventoryModel.IsAccountClientLookupIdReq = true;
                objMaterialRequestVM.PartInInventoryModel.IsPartCategoryClientLookupIdReq = true;
                
            }
            // Set the shopping cart flag
            if (userData.Site.ShoppingCart)
            {
                objMaterialRequestVM.PartInInventoryModel.ShoppingCart = true;
            }
            #endregion
            LocalizeControls(objMaterialRequestVM, LocalizeResourceSetConstants.MaterialRequest);
            return PartialView("~/Views/MaterialRequest/_EditPartInInventory.cshtml", objMaterialRequestVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPartInInventory(MaterialRequestVM PurchaseRequestVM)
        {
            string ModelValidationFailedMessage = string.Empty;
            MaterialRequestWrapper pWrapper = new MaterialRequestWrapper(userData);
            if (ModelState.IsValid)
            {
                var lineItem = pWrapper.EditPartInInventory(PurchaseRequestVM.PartInInventoryModel);
                if (lineItem.ErrorMessages != null && lineItem.ErrorMessages.Count > 0)
                {
                    return Json(lineItem.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), MaterialRequestId = PurchaseRequestVM.PartInInventoryModel.ObjectId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region V2-726
        public PartialViewResult SendForApproval(long MaterialRequestId)
        {
            MaterialRequestVM objMaterialRequestVM = new MaterialRequestVM();

            ApprovalRouteModel approvalRouteModel = new ApprovalRouteModel();
            var dataModels = Get_ApproverList(this.userData.DatabaseKey.Personnel.PersonnelId, ApprovalGroupRequestTypes.MaterialRequest, 1);
            var approvers = new List<SelectListItem>();
            if (dataModels.Count>0)
            {
                approvers = dataModels.Select(x => new SelectListItem
                {
                    Text = x.ApproverName,
                    Value = x.ApproverId.ToString()
                }).ToList();
            }
            else
            {
                var securityName = SecurityConstants.MaterialRequest_Approve;
                var ItemAccess = "ItemAccess";
                dataModels=Get_PersonnelList(securityName, ItemAccess);
                approvers = dataModels.Select(x => new SelectListItem
                {
                    Text = x.NameFirst + " " + x.NameLast,
                    Value = x.AssignedTo_PersonnelId.ToString()
                }).ToList();
            }
            approvalRouteModel.ApproverList = approvers;
            approvalRouteModel.ApproverCount=approvers.Count;
            approvalRouteModel.ObjectId=MaterialRequestId;

            objMaterialRequestVM.ApprovalRouteModel = approvalRouteModel;
            ViewBag.IsMaterialRequestDetails=false;
            LocalizeControls(objMaterialRequestVM, LocalizeResourceSetConstants.MaterialRequest);
            return PartialView("_SendItemsForApproval", objMaterialRequestVM);
        }
        [HttpPost]
        public JsonResult SendForApproval(MaterialRequestVM mrVM)
        {
            MaterialRequestWrapper mrWrapper = new MaterialRequestWrapper(userData);
            if (ModelState.IsValid)
            {
                string MaterialRequest = ApprovalGroupRequestTypes.MaterialRequest;
                long ApprovalGroupId = RetrieveApprovalGroupIdByRequestorIdAndRequestType(MaterialRequest);
                mrVM.ApprovalRouteModel.ApprovalGroupId = ApprovalGroupId;
                mrVM.ApprovalRouteModel.RequestType = MaterialRequest;
                if (ApprovalGroupId >= 0)
                {
                    var objWorkOrder = mrWrapper.SendForApproval(mrVM.ApprovalRouteModel);
                    if (objWorkOrder.ErrorMessages != null && objWorkOrder.ErrorMessages.Count > 0)
                    {
                        return Json(objWorkOrder.ErrorMessages, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { data = JsonReturnEnum.success.ToString(), MaterialRequestId = mrVM.ApprovalRouteModel.ObjectId, ApprovalGroupId = ApprovalGroupId }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString(), MaterialRequestId = mrVM.ApprovalRouteModel.ObjectId, ApprovalGroupId = ApprovalGroupId }, JsonRequestBehavior.AllowGet);
                }
                
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region V2-732
        public PartialViewResult PopulateStorerooms()
        {
            MaterialRequestVM materialRequestVM = new MaterialRequestVM();
            var commonWrapper = new CommonWrapper(userData);
            if (userData.DatabaseKey.Client.UseMultiStoreroom)
            {
                materialRequestVM.StoreroomList = commonWrapper.GetStoreroomList(StoreroomAuthType.Issue);
            }
            LocalizeControls(materialRequestVM, LocalizeResourceSetConstants.MaterialRequest);
            return PartialView("_StoreroomList", materialRequestVM);
        }
        [HttpPost]
        public JsonResult SelectStoreroom(MaterialRequestVM mrVM)
        {
            if(ModelState.IsValid)
            {
                return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }

}

