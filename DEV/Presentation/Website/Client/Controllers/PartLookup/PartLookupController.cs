using Client.BusinessWrapper.PartLookup;
using Client.Common;
using Client.Controllers.Common;
using Client.Models.PartLookup;
using Common.Constants;
using DataContracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Controllers.PartLookup
{
    public class PartLookupController : SomaxBaseController
    {
        #region Purchase Request
        public ActionResult Index()
        {
            PartLookupVM partLookupVM = new PartLookupVM();
            partLookupVM.IsOnOderCheck = userData.Site.OnOrderCheck;
            LocalizeControls(partLookupVM, LocalizeResourceSetConstants.PartLookUpDetails);
            return View(partLookupVM);
        }
        [HttpPost]
        public string GetPartLookUpGrid(int? draw, int? start, int? length, string orderbycol = "", string orderDir = "", string searchString = "", long VendorId = 0, long StoreroomId = 0)
        {
            PartLookupWrapper partLookupWrapper = new PartLookupWrapper(userData);
            List<PartLookupModel> cardData = new List<PartLookupModel>();
            start = start.HasValue
              ? start / length
              : 0;
            int skip = start * length ?? 0;
            if (userData.Site.ShoppingCart)
            {
                if (userData.DatabaseKey.Client.UseMultiStoreroom) /*V2-738*/
                {
                    cardData = partLookupWrapper.SearchForCart_VendorCatalogForMultiStoreroom(skip, length ?? 0, orderbycol, orderDir, searchString, VendorId,StoreroomId);
                }
                else
                {
                    cardData = partLookupWrapper.SearchForCart_VendorCatalog(skip, length ?? 0, orderbycol, orderDir, searchString, VendorId);
                }
            }
            else
            {
                if (userData.DatabaseKey.Client.UseMultiStoreroom)  /*V2-738*/
                {
                    cardData = partLookupWrapper.SearchForCartMultiStoreroom(skip, length ?? 0, orderbycol, orderDir, searchString, VendorId, StoreroomId);
                }
                else
                {
                    cardData = partLookupWrapper.SearchForCart(skip, length ?? 0, orderbycol, orderDir, searchString, VendorId);
                }
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            recordsFiltered = cardData.Select(o => o.TotalCount).FirstOrDefault();
            totalRecords = cardData.Select(o => o.TotalCount).FirstOrDefault();
            int initialPage = start.Value;
            var filteredResult = cardData.ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }

        public PartialViewResult GetCardViewData(int currentpage, int? start, int? length, string currentorderedcolumn, string currentorder, string searchString = "", long VendorId = 0, long PurchaseOrderId = 0, long StoreroomId = 0)
        {
            PartLookupVM partLookupVM = new PartLookupVM();
            List<PartLookupModel> cardData = new List<PartLookupModel>();
            PartLookupWrapper partLookupWrapper = new PartLookupWrapper(userData);
            start = start.HasValue
            ? start / length
            : 0;
            int skip = start * length ?? 0;
            ViewBag.Start = skip;

            if (userData.Site.ShoppingCart)
            {
                if(userData.DatabaseKey.Client.UseMultiStoreroom)   /*V2-738*/
                { 
                    cardData = partLookupWrapper.SearchForCart_VendorCatalogForMultiStoreroom(skip, length ?? 0, currentorderedcolumn, currentorder, searchString, VendorId,StoreroomId);
                }
                else
                {
                    cardData = partLookupWrapper.SearchForCart_VendorCatalog(skip, length ?? 0, currentorderedcolumn, currentorder, searchString, VendorId);
                }
                
            }
            else
            {
                if (userData.DatabaseKey.Client.UseMultiStoreroom)    /*V2-738*/
                {
                    cardData = partLookupWrapper.SearchForCartMultiStoreroom(skip, length ?? 0, currentorderedcolumn, currentorder, searchString, VendorId, StoreroomId);
                }
                else
                {
                    cardData = partLookupWrapper.SearchForCart(skip, length ?? 0, currentorderedcolumn, currentorder, searchString, VendorId);
                }
            }
            var recordsFiltered = 0;
            recordsFiltered = cardData.Select(o => o.TotalCount).FirstOrDefault();
            ViewBag.TotalRecords = cardData.Select(o => o.TotalCount).FirstOrDefault();
            ViewBag.Length = length;
            ViewBag.CurrentPage = currentpage;
            partLookupVM.PurchaseOrderId = PurchaseOrderId;
            partLookupVM.partLookupModels = cardData;
            partLookupVM.IsOnOderCheck = userData.Site.OnOrderCheck;
            LocalizeControls(partLookupVM, LocalizeResourceSetConstants.PartLookUpDetails);

            return PartialView("~/Views/partlookup/_CardView.cshtml", partLookupVM);
        }
        public PartialViewResult GetAddToCartData(PartAddItemToCartModel model)
        {
            LocalizeControls(model, LocalizeResourceSetConstants.PartLookUpDetails);
            return PartialView("~/Views/partlookup/_AddToCartSideBar.cshtml", model);
        }

        public PartialViewResult GetAllAddToCartData(PartAddToCartModel model)
        {
            LocalizeControls(model, LocalizeResourceSetConstants.PartLookUpDetails);
            return PartialView("~/Views/partlookup/_AddAllToCartSideBar.cshtml", model);
        }


        [HttpPost]
        public ActionResult ProcesssPartCartData(List<PartAddToCartProcessModel> modelData, long PurchaseRequestId = 0, long PurchaseOrderId = 0, long StoreroomId = 0)
        {
            PartLookupWrapper partLookupWrapper = new PartLookupWrapper(userData);
            #region ServerSide Validation Quantity & Price
            bool isValidData = true, isValidReqData = true;
            string message = string.Empty;
            string clientlookUpId = string.Empty;
            if (modelData.Any(x => x.OrderQuantity <= 0 || x.UnitCost <= 0))
            {
                isValidData = false;
                foreach (var model in modelData.Where(x => x.OrderQuantity <= 0 || x.UnitCost <= 0))
                {                    
                    clientlookUpId = clientlookUpId + model.ClientLookUpId + ",";
                }
            }
            else if (userData.Site.ShoppingCart && PurchaseOrderId == 0 && 
                modelData.Any(x => x.RequiredDate == null || x.RequiredDate == DateTime.MinValue))
            {
                isValidReqData = false;
                foreach (var model in modelData.Where(x => x.RequiredDate == null || x.RequiredDate == DateTime.MinValue))
                {                    
                    clientlookUpId = clientlookUpId + model.ClientLookUpId + ",";
                }
            }
            else
            {
                foreach (var model in modelData)
                {
                    model.PurchaseRequestID = PurchaseRequestId;
                    model.PurchaseOrderID = PurchaseOrderId;
                }
            }
            if (isValidData == false)
            {
                clientlookUpId = clientlookUpId.Remove(clientlookUpId.LastIndexOf(','));
                message = UtilityFunction.GetMessageFromResource("QtyPartNotZeroErrorMsg", LocalizeResourceSetConstants.PartLookUpDetails)+ clientlookUpId;
                return Json(new { errormessge = message }, JsonRequestBehavior.AllowGet);
            }
            else if (isValidReqData == false)
            {
                clientlookUpId = clientlookUpId.Remove(clientlookUpId.LastIndexOf(','));
                //message = UtilityFunction.GetMessageFromResource("QtyPartNotZeroErrorMsg", LocalizeResourceSetConstants.PartLookUpDetails);
                message = "Required date can't be blank for PartId " + clientlookUpId;
                return Json(new { errormessge = message }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (PurchaseRequestId > 0)
                {
                    var result = partLookupWrapper.InsertPartLookUpPurchaseRequest(modelData,StoreroomId);
                    if (result != null && result.Count == 0)
                    {
                        message = UtilityFunction.GetMessageFromResource("PRLineItemAdded", LocalizeResourceSetConstants.PartLookUpDetails);
                    }
                    else
                    {
                        message = "fail";
                    }
                }                
                else
                {
                    message = "fail";
                }

                return Json(new { errormessge = "", status = message, data = modelData }, JsonRequestBehavior.AllowGet);
            }
            #endregion

        }
        #endregion

        #region V2-563 Common
        public PartialViewResult AddPRAdditionalCatalogGrid(string clientlookupid)
        {
            PartLookupVM partLookupVM = new PartLookupVM();
            PartLookupModel partlookupmodel = new PartLookupModel();
            partlookupmodel.ClientLookupId = clientlookupid;
            partLookupVM.partLookup = partlookupmodel;
            LocalizeControls(partLookupVM, LocalizeResourceSetConstants.PartLookUpDetails);
            return PartialView("_AdditionalCatalogItems", partLookupVM);
        }
        [HttpPost]
        public string GetPRAdditionalCatalogGrid(int? draw, int? start, int? length, long PartId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            PartLookupWrapper partLookupWrapper = new PartLookupWrapper(userData);
            PartLookupVM partLookupVM = new PartLookupVM();

            start = start.HasValue
          ? start / length
          : 0;
            int skip = start * length ?? 0;

            partLookupVM.additionalCatalogItemlist = partLookupWrapper.PopulateAdditionalCatalogitems(PartId, order, orderDir, skip, length ?? 0);

            var totalRecords = 0;
            var recordsFiltered = 0;

            recordsFiltered = partLookupVM.additionalCatalogItemlist.Count();
            totalRecords = partLookupVM.additionalCatalogItemlist.Count();
            int initialPage = start.Value;
            var filteredResult = partLookupVM.additionalCatalogItemlist
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
        #endregion

        #region Purchase order
        [HttpPost]
        public string GetPartLookUpGridPO(int? draw, int? start, int? length, string orderbycol = "", string orderDir = "", string searchString = "", long VendorId = 0, long StoreroomId = 0)
        {
            PartLookupWrapper partLookupWrapper = new PartLookupWrapper(userData);
            List<PartLookupModel> cardData = new List<PartLookupModel>();
            start = start.HasValue
              ? start / length
              : 0;
            int skip = start * length ?? 0;
            if (userData.Site.ShoppingCart)
            {
                if (userData.DatabaseKey.Client.UseMultiStoreroom) /*V2-738*/
                {
                    cardData = partLookupWrapper.SearchForCart_VendorCatalogForMultiStoreroom(skip, length ?? 0, orderbycol, orderDir, searchString, VendorId,StoreroomId);
                }
                else
                {
                    cardData = partLookupWrapper.SearchForCart_VendorCatalog(skip, length ?? 0, orderbycol, orderDir, searchString, VendorId);
                }
                    
            }
            else
            {
                if (userData.DatabaseKey.Client.UseMultiStoreroom) /*V2-738*/
                {
                    cardData = partLookupWrapper.SearchForCartMultiStoreroom(skip, length ?? 0, orderbycol, orderDir, searchString, VendorId,StoreroomId);
                }
                else
                {
                    cardData = partLookupWrapper.SearchForCart(skip, length ?? 0, orderbycol, orderDir, searchString, VendorId);
                }

            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            recordsFiltered = cardData.Select(o => o.TotalCount).FirstOrDefault();
            totalRecords = cardData.Select(o => o.TotalCount).FirstOrDefault();
            int initialPage = start.Value;
            var filteredResult = cardData.ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }

        public PartialViewResult GetCardViewDataPO(int currentpage, int? start, int? length, string currentorderedcolumn, string currentorder, string searchString = "", long VendorId = 0, long PurchaseOrderId = 0, long StoreroomId = 0)
        {
            PartLookupVM partLookupVM = new PartLookupVM();
            List<PartLookupModel> cardData = new List<PartLookupModel>();
            PartLookupWrapper partLookupWrapper = new PartLookupWrapper(userData);
            start = start.HasValue
            ? start / length
            : 0;
            int skip = start * length ?? 0;
            ViewBag.Start = skip;

            if (userData.Site.ShoppingCart)
            {
                if (userData.DatabaseKey.Client.UseMultiStoreroom) /*V2-738*/
                {
                    cardData = partLookupWrapper.SearchForCart_VendorCatalogForMultiStoreroom(skip, length ?? 0, currentorderedcolumn, currentorder, searchString, VendorId,StoreroomId);
                }
                else
                {
                    cardData = partLookupWrapper.SearchForCart_VendorCatalog(skip, length ?? 0, currentorderedcolumn, currentorder, searchString, VendorId);
                }
                   
            }
            else
            {
                if (userData.DatabaseKey.Client.UseMultiStoreroom) /*V2-738*/
                {
                    cardData = partLookupWrapper.SearchForCartMultiStoreroom(skip, length ?? 0, currentorderedcolumn, currentorder, searchString, VendorId,StoreroomId);
                }
                else
                {
                    cardData = partLookupWrapper.SearchForCart(skip, length ?? 0, currentorderedcolumn, currentorder, searchString, VendorId);
                }
            }
            var recordsFiltered = 0;
            recordsFiltered = cardData.Select(o => o.TotalCount).FirstOrDefault();
            ViewBag.TotalRecords = cardData.Select(o => o.TotalCount).FirstOrDefault();
            ViewBag.Length = length;
            ViewBag.CurrentPage = currentpage;
            partLookupVM.PurchaseOrderId = PurchaseOrderId;
            partLookupVM.partLookupModels = cardData;
            partLookupVM.IsOnOderCheck = userData.Site.OnOrderCheck;
            LocalizeControls(partLookupVM, LocalizeResourceSetConstants.PartLookUpDetails);

            return PartialView("~/Views/partlookup/_CardViewPO.cshtml", partLookupVM);
        }

        public PartialViewResult GetAddToCartDataPO(PartAddItemToCartModel model)
        {
            LocalizeControls(model, LocalizeResourceSetConstants.PartLookUpDetails);
            return PartialView("~/Views/partlookup/_AddToCartSideBarPO.cshtml", model);
        }

        public PartialViewResult GetAllAddToCartDataPO(PartAddToCartModel model)
        {
            LocalizeControls(model, LocalizeResourceSetConstants.PartLookUpDetails);
            return PartialView("~/Views/partlookup/_AddAllToCartSideBarPO.cshtml", model);
        }

        [HttpPost]
        public ActionResult ProcesssPartCartDataPO(List<PartAddToCartProcessModel> modelData, long PurchaseRequestId = 0, long PurchaseOrderId = 0, long StoreroomId = 0)
        {
            PartLookupWrapper partLookupWrapper = new PartLookupWrapper(userData);
            #region ServerSide Validation Quantity & Price
            bool isValidData = true;
            string message = string.Empty;
            string clientlookUpId = string.Empty;
            if (modelData.Any(x => x.OrderQuantity <= 0 || x.UnitCost <= 0))
            {
                isValidData = false;
                foreach (var model in modelData.Where(x => x.OrderQuantity <= 0 || x.UnitCost <= 0))
                {
                    clientlookUpId = clientlookUpId + model.ClientLookUpId + ",";
                }
            }            
            else
            {
                foreach (var model in modelData)
                {
                    model.PurchaseRequestID = PurchaseRequestId;
                    model.PurchaseOrderID = PurchaseOrderId;
                }
            }
            if (isValidData == false)
            {
                clientlookUpId = clientlookUpId.Remove(clientlookUpId.LastIndexOf(','));
                message = UtilityFunction.GetMessageFromResource("QtyPartNotZeroErrorMsg", LocalizeResourceSetConstants.PartLookUpDetails) + clientlookUpId;
                return Json(new { errormessge = message }, JsonRequestBehavior.AllowGet);
            }            
            else
            {
                if (PurchaseOrderId > 0)
                {
                    var result = partLookupWrapper.InsertPartLookUpPurchaseOrder(modelData, StoreroomId);
                    if (result != null && result.Count == 0)
                    {
                        message = UtilityFunction.GetMessageFromResource("POLineItemAdded", LocalizeResourceSetConstants.PartLookUpDetails);
                    }
                    else
                    {
                        message = "fail";
                    }
                }
                else
                {
                    message = "fail";
                }

                return Json(new { errormessge = "", status = message, data = modelData }, JsonRequestBehavior.AllowGet);
            }
            #endregion

        }
        #endregion

        #region Work order
        [HttpPost]
        public string GetPartLookUpGridWO(int? draw, int? start, int? length, string orderbycol = "", string orderDir = "", string searchString = "", long StoreroomId = 0)
        {
            PartLookupWrapper partLookupWrapper = new PartLookupWrapper(userData);
            List<PartLookupModel> cardData = new List<PartLookupModel>();
            start = start.HasValue
              ? start / length
              : 0;
            int skip = start * length ?? 0;
            if (userData.DatabaseKey.Client.UseMultiStoreroom)
            {
                if (userData.Site.ShoppingCart)
                {
                    cardData = partLookupWrapper.SearchForCart_VendorCatalogWOMultiStoreroom(skip, length ?? 0, orderbycol, orderDir, searchString, StoreroomId);
                }
                else
                {
                    cardData = partLookupWrapper.SearchForCartWOMultiStoreroom(skip, length ?? 0, orderbycol, orderDir, searchString, StoreroomId);
                }
            }
            else
            {
                if (userData.Site.ShoppingCart)
                {
                    cardData = partLookupWrapper.SearchForCart_VendorCatalogWO(skip, length ?? 0, orderbycol, orderDir, searchString);
                }
                else
                {
                    cardData = partLookupWrapper.SearchForCartWO(skip, length ?? 0, orderbycol, orderDir, searchString);
                }
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            recordsFiltered = cardData.Select(o => o.TotalCount).FirstOrDefault();
            totalRecords = cardData.Select(o => o.TotalCount).FirstOrDefault();
            int initialPage = start.Value;
            var filteredResult = cardData.ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }

        public PartialViewResult GetCardViewDataWO(int currentpage, int? start, int? length, string currentorderedcolumn, string currentorder, string searchString = "", long WorkOrderID = 0, string ModeForRedirect = "",long StoreroomId = 0)
        {
            PartLookupVM partLookupVM = new PartLookupVM();
            List<PartLookupModel> cardData = new List<PartLookupModel>();
            PartLookupWrapper partLookupWrapper = new PartLookupWrapper(userData);
            start = start.HasValue
            ? start / length
            : 0;
            int skip = start * length ?? 0;
            ViewBag.Start = skip;
            if (userData.DatabaseKey.Client.UseMultiStoreroom)
            {
                if (userData.Site.ShoppingCart)
                {
                    cardData = partLookupWrapper.SearchForCart_VendorCatalogWOMultiStoreroom(skip, length ?? 0, currentorderedcolumn, currentorder, searchString, StoreroomId);
                }
                else
                {
                    cardData = partLookupWrapper.SearchForCartWOMultiStoreroom(skip, length ?? 0, currentorderedcolumn, currentorder, searchString, StoreroomId);
                }
            }
            else
            {
                if (userData.Site.ShoppingCart)
                {
                    cardData = partLookupWrapper.SearchForCart_VendorCatalogWO(skip, length ?? 0, currentorderedcolumn, currentorder, searchString);
                }
                else
                {
                    cardData = partLookupWrapper.SearchForCartWO(skip, length ?? 0, currentorderedcolumn, currentorder, searchString);
                }
            }
            var recordsFiltered = 0;
            recordsFiltered = cardData.Select(o => o.TotalCount).FirstOrDefault();
            ViewBag.TotalRecords = cardData.Select(o => o.TotalCount).FirstOrDefault();
            ViewBag.Length = length;
            ViewBag.CurrentPage = currentpage;
            partLookupVM.WorkOrderID = WorkOrderID;
            partLookupVM.partLookupModels = cardData;
            partLookupVM.ModeForRedirect = ModeForRedirect;
            LocalizeControls(partLookupVM, LocalizeResourceSetConstants.PartLookUpDetails);

            return PartialView("~/Views/partlookup/_CardViewWO.cshtml", partLookupVM);
        }

        public PartialViewResult GetAddToCartDataWO(PartAddItemToCartModel model)
        {
            LocalizeControls(model, LocalizeResourceSetConstants.PartLookUpDetails);
            return PartialView("~/Views/partlookup/_AddToCartSideBarWO.cshtml", model);
        }

        public PartialViewResult GetAllAddToCartDataWO(PartAddToCartModel model)
        {
            LocalizeControls(model, LocalizeResourceSetConstants.PartLookUpDetails);
            return PartialView("~/Views/partlookup/_AddAllToCartSideBarWO.cshtml", model);
        }

        [HttpPost]
        public ActionResult ProcesssPartCartDataWO(List<PartAddToCartProcessModel> modelData, long WorkOrderID, long MaterialRequestId, long PreventiveMaintainId)
        {
            PartLookupWrapper partLookupWrapper = new PartLookupWrapper(userData);
            #region ServerSide Validation Quantity & Price
            bool isValidData = true;
            string message = string.Empty;
            string clientlookUpId = string.Empty;

            // V2-1148 Check if the site is using a shopping cart
            if (userData.Site.ShoppingCart)
            {
                // Validate order quantity and unit cost
                if (modelData.Any(x => x.OrderQuantity <= 0 || x.UnitCost <= 0))
                {
                    isValidData = false;
                    foreach (var model in modelData.Where(x => x.OrderQuantity <= 0 || x.UnitCost <= 0))
                    {
                        clientlookUpId = clientlookUpId + model.ClientLookUpId + ",";
                    }
                }
                else
                {
                    foreach (var model in modelData)
                    {
                        model.WorkOrderID = WorkOrderID;
                        model.MaterialRequestId = MaterialRequestId;
                        model.PreventiveMaintainId = PreventiveMaintainId;
                    }
                }
            }
            else
            {
                foreach (var model in modelData)
                {
                    model.WorkOrderID = WorkOrderID;
                    model.MaterialRequestId = MaterialRequestId;
                    model.PreventiveMaintainId = PreventiveMaintainId;
                }
            }

            // If data is invalid, return error message
            if (isValidData == false)
            {
                clientlookUpId = clientlookUpId.Remove(clientlookUpId.LastIndexOf(','));
                message = UtilityFunction.GetMessageFromResource("QtyPartNotZeroErrorMsg", LocalizeResourceSetConstants.PartLookUpDetails) + clientlookUpId;
                return Json(new { errormessge = message }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                // Process valid data
                if (WorkOrderID > 0)
                {
                    if (userData.DatabaseKey.Client.UseMultiStoreroom)
                    {
                        var result = partLookupWrapper.InsertPartLookUpWorkOrderMultiStoreroom(modelData);
                        if (result != null && result.Count == 0)
                        {
                            message = UtilityFunction.GetMessageFromResource("MaterialRequestAddedAlert", LocalizeResourceSetConstants.JsAlerts);
                        }
                        else
                        {
                            message = "fail";
                        }
                    }
                    else
                    {
                        var result = partLookupWrapper.InsertPartLookUpWorkOrder(modelData);
                        if (result != null && result.Count == 0)
                        {
                            message = UtilityFunction.GetMessageFromResource("MaterialRequestAddedAlert", LocalizeResourceSetConstants.JsAlerts);
                        }
                        else
                        {
                            message = "fail";
                        }
                    }
                }
                else if (MaterialRequestId > 0)
                {
                    if (userData.DatabaseKey.Client.UseMultiStoreroom)
                    {
                        var result = partLookupWrapper.InsertPartLookUpMaterialRequestMultiStoreroom(modelData);
                        if (result != null && result.Count == 0)
                        {
                            message = UtilityFunction.GetMessageFromResource("MaterialRequestItemAddedAlert", LocalizeResourceSetConstants.JsAlerts);
                        }
                        else
                        {
                            message = "fail";
                        }
                    }
                    else
                    {
                        var result = partLookupWrapper.InsertPartLookUpMaterialRequest(modelData);
                        if (result != null && result.Count == 0)
                        {
                            message = UtilityFunction.GetMessageFromResource("MaterialRequestItemAddedAlert", LocalizeResourceSetConstants.JsAlerts);
                        }
                        else
                        {
                            message = "fail";
                        }
                    }
                }
                else if (PreventiveMaintainId > 0)
                {
                    var result = partLookupWrapper.InsertPMMaterialRequest(modelData);
                    if (result != null && result.Count == 0)
                    {
                        message = UtilityFunction.GetMessageFromResource("MaterialRequestItemAddedAlert", LocalizeResourceSetConstants.JsAlerts);
                    }
                    else
                    {
                        message = "fail";
                    }
                }
                else
                {
                    message = "fail";
                }

                return Json(new { errormessge = "", status = message, data = modelData }, JsonRequestBehavior.AllowGet);
            }
            #endregion
        }
        #endregion

        #region V2-894
       
        [HttpPost]
        public string GetLineitemLookuplist(int? draw, int? start, int? length, string orderbycol = "", string orderDir = "", long PartId = 0)
        {
            string orders = Request.Form.GetValues("order[0][column]")[0];
            string orderDisr = Request.Form.GetValues("order[0][dir]")[0];
            orderDir = orderDisr;
            orderbycol = orders;
            PartLookupVM objPLVM = new PartLookupVM();
            PartLookupWrapper plwWrapper = new PartLookupWrapper(userData);
            start = start.HasValue
              ? start / length
              : 0;
            int skip = start * length ?? 0;
            var lineItemOnOrderCheckModellist = plwWrapper.getLineitemLookupList(skip, length??0, orderbycol, orderDir, PartId);
            objPLVM.lineItemOnOrderCheckModel = lineItemOnOrderCheckModellist;
            long totalRecords = 0;
            long recordsFiltered = 0;
            recordsFiltered = lineItemOnOrderCheckModellist.Select(o => o.TotalCount).FirstOrDefault();
            totalRecords = lineItemOnOrderCheckModellist.Select(o => o.TotalCount).FirstOrDefault();
            int initialPage = start.Value;
            var filteredResult = lineItemOnOrderCheckModellist.ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }

        #endregion
    }
}