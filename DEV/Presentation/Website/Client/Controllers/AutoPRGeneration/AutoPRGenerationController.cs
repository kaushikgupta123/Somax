using Client.ActionFilters;
using Client.BusinessWrapper;
using Client.BusinessWrapper.Common;
using Client.Common;
using Client.Controllers.Common;
using Client.Models;
using Client.Models.AutoPRGeneration;
using Client.Models.Common;
using Common.Constants;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Controllers
{
    public class AutoPRGenerationController : SomaxBaseController
    {
        // GET: AutoPRGeneration
        [CheckUserSecurity(securityType = SecurityConstants.PurchaseRequest_AutoGeneration)]
        public ActionResult Index()
        {
            AutoPRGenerationVM objAutoPRGenerationVM = new AutoPRGenerationVM();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            if (userData.DatabaseKey.Client.UseMultiStoreroom)
            {
                objAutoPRGenerationVM.IsMultistoreroomAutoPRGeneration = userData.DatabaseKey.Client.UseMultiStoreroom;
                objAutoPRGenerationVM.StoreroomList = commonWrapper.GetStoreroomList(StoreroomAuthType.Maintain);
            }
            LocalizeControls(objAutoPRGenerationVM, LocalizeResourceSetConstants.PurchaseRequest);
            return View(objAutoPRGenerationVM);
        }
       
        #region Vendor chunk lookup list
        public string VendorLookupListchunksearch_AutoPRGeneration(int? draw, int? start, int? length, string clientLookupId = "",
            string name = "", bool InactiveFlag = true)
        {
            List<VendorLookupModel> modelList = new List<VendorLookupModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            string order = Convert.ToInt32(Request.Form.GetValues("order[0][column]")[0])==1?"0":"1";
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            start = start.HasValue
              ? start / length
              : 0;
            int skip = start * length ?? 0;

            clientLookupId = !string.IsNullOrEmpty(clientLookupId) ? clientLookupId.Trim() : string.Empty;
            name = !string.IsNullOrEmpty(name) ? name.Trim() : string.Empty;

            modelList = commonWrapper.GetVendorLookupListGridData(order, orderDir, skip, length.Value,
                clientLookupId, name);

            long totalRecords = 0;
            long recordsFiltered = 0;
            if (modelList != null && modelList.Count > 0)
            {
                totalRecords = modelList[0].TotalCount;
                recordsFiltered = modelList[0].TotalCount;
            }

            return JsonConvert.SerializeObject(new
            {
                draw = draw,
                recordsTotal = totalRecords,
                recordsFiltered = recordsFiltered,
                data = modelList
            });
        }
        public JsonResult GetVendorLookupListSelectAllData(string colname, string coldir, string clientLookupId = "",
           string name = "", bool InactiveFlag = true)
        {
            List<VendorLookupModel> modelList = new List<VendorLookupModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            name = !string.IsNullOrEmpty(name) ? name.Trim() : string.Empty;
            clientLookupId = !string.IsNullOrEmpty(clientLookupId) ? clientLookupId.Trim() : string.Empty;
            modelList = commonWrapper.GetVendorLookupListGridData(colname, coldir, 0, 100000, clientLookupId, name);
            var jsonResult = Json(modelList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion

        [HttpPost]
        public string GetAutoPRGenerationGridData(int? draw, int? start, int? length,  string[] VendorIdList, string Order = "1",
            string PartClientLookupId ="",
            string Description = "",
            string UnitofMeasure = "",
            string VendorClientLookupId ="",
            string VendorName = "",
            string QtyToOrder ="",
            string LastPurchaseCost ="",
            string StoreroomId = "")
        {
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            start = start.HasValue ? start / length : 0;
            int skip = start * length ?? 0;
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            var vendorids = VendorIdList != null && VendorIdList.Length > 0 ? string.Join(",", VendorIdList) : string.Empty;
            List<AutoPRGenerationSearchModel> pRList = pWrapper.GetAutoPRGenerateChunkList(skip, length ?? 0, Order, orderDir, vendorids, PartClientLookupId, Description, UnitofMeasure, VendorClientLookupId, VendorName, QtyToOrder, LastPurchaseCost,StoreroomId);
            var totalRecords = 0;
            var recordsFiltered = 0;
            if (pRList != null && pRList.Count > 0)
            {
                recordsFiltered = pRList[0].TotalCount;
                totalRecords = pRList[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }
       
            var filteredResult = pRList
              .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult}, JsonSerializerDateSettings);
        }
        [HttpPost]
        public JsonResult GetAutoPRGenerationSearchSelectAllData(string[] VendorIdLists, string colname,string coldir,
            string PartClientLookupId = "",
            string Description = "",
            string UnitofMeasure = "",
            string VendorClientLookupId = "",
            string VendorName = "",
            string QtyToOrder = "",
            string LastPurchaseCost = "",
            string StoreroomId = "")
        {
           
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            var vendorids = VendorIdLists != null && VendorIdLists.Length > 0 ? string.Join(",", VendorIdLists) : string.Empty;
            List<AutoPRGenerationSearchModel> pRList = pWrapper.GetAutoPRGenerateChunkList(0, 100000, colname, coldir, vendorids, PartClientLookupId, Description, UnitofMeasure, VendorClientLookupId, VendorName, QtyToOrder, LastPurchaseCost,StoreroomId);
            var jsonResult = Json(pRList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #region PR AutoGeneration Create
        [HttpPost]
        public JsonResult CreateAutoPRGeneration(List<Models.AutoPRGeneration.PartListTableModel> partTableLists)
        {
            DataContracts.ProcessLog processLog = new DataContracts.ProcessLog();
            Models.PreventiveMaintenance.ProcesLogModel procesLogModel = new Models.PreventiveMaintenance.ProcesLogModel();
           // var partIDs = PartIDList != null && PartIDList.Length > 0 ? string.Join(",", PartIDList) : string.Empty;
            if (partTableLists!=null && partTableLists.Count>0)
            {
                PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
                processLog = pWrapper.PReqAutoGenerate_V2(partTableLists);
                procesLogModel.ItemsReviewed = processLog.ItemsReviewed;
                procesLogModel.HeadersCreated = processLog.HeadersCreated;
                procesLogModel.DetailsCreated = processLog.DetailsCreated;
                return Json(procesLogModel, JsonRequestBehavior.AllowGet);
            }
            return Json(procesLogModel, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region Auto PR Generation Grid Print
        [HttpPost]
        public string GetAutoPRGenerationGridPrintData(string[] VendorIdLists, string colname, string coldir,
            string PartClientLookupId = "",
            string Description = "",
            string UnitofMeasure = "",
            string VendorClientLookupId = "",
            string VendorName = "",
            string QtyToOrder = "",
            string LastPurchaseCost = "",
            string StoreroomId = "")
        {
            AutoPRGenerationPrintModel objAutoPRGenerationPrintModel;
            List<AutoPRGenerationPrintModel> AutoPRGenerationPrintModelList = new List<AutoPRGenerationPrintModel>();
            PartClientLookupId = PartClientLookupId.Replace("%", "[%]");
            Description = Description.Replace("%", "[%]");
            UnitofMeasure = UnitofMeasure.Replace("%", "[%]");
            VendorClientLookupId = VendorClientLookupId.Replace("%", "[%]");
            VendorName = VendorName.Replace("%", "[%]");

            int lengthForPrint = 100000;
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            var vendorids = VendorIdLists != null && VendorIdLists.Length > 0 ? string.Join(",", VendorIdLists) : string.Empty;
            List<AutoPRGenerationSearchModel> pRList = pWrapper.GetAutoPRGenerateChunkList(0, lengthForPrint, colname, coldir, vendorids, PartClientLookupId, Description, UnitofMeasure, VendorClientLookupId, VendorName, QtyToOrder, LastPurchaseCost,StoreroomId);

            foreach (var item in pRList)
            {
                objAutoPRGenerationPrintModel = new AutoPRGenerationPrintModel();
                objAutoPRGenerationPrintModel.PartClientLookupId = item.PartClientLookupId;
                objAutoPRGenerationPrintModel.Description = item.Description;
                objAutoPRGenerationPrintModel.UnitofMeasure = item.UnitofMeasure;
                objAutoPRGenerationPrintModel.VendorClientLookupId = item.VendorClientLookupId;
                objAutoPRGenerationPrintModel.VendorName = item.VendorName;
                objAutoPRGenerationPrintModel.QtyToOrder = item.QtyToOrder!=null? item.QtyToOrder.ToString():"0";
                objAutoPRGenerationPrintModel.LastPurchaseCost = item.LastPurchaseCost!=null? item.LastPurchaseCost.ToString():"0";
                AutoPRGenerationPrintModelList.Add(objAutoPRGenerationPrintModel);
            }
            return JsonConvert.SerializeObject(new { data = AutoPRGenerationPrintModelList }, JsonSerializerDateSettings);
        }
        #endregion
        #region V2-1196
        public JsonResult ValidateStoreroomSelection(AutoPRGenerationVM aprVM)
        {
            if (ModelState.IsValid)
            {
                return Json(new { data = JsonReturnEnum.success.ToString() , StoreroomId = aprVM.autoPRGenerationStoreroomModel.StoreroomId }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
      
        public PartialViewResult LoadAutoPRGeneration()
        {
            AutoPRGenerationVM objAutoPRGenerationVM = new AutoPRGenerationVM();
            
            LocalizeControls(objAutoPRGenerationVM, LocalizeResourceSetConstants.PurchaseRequest);
            return PartialView("~/Views/AutoPRGeneration/_AutoPRGeneration.cshtml", objAutoPRGenerationVM);
        }
        #endregion
    }
}