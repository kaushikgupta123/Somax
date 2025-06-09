using Client.ActionFilters;
using Client.BusinessWrapper;
using Client.BusinessWrapper.Common;
using Client.Controllers.Common;
using Client.Models;
using Client.Models.AutoPRGeneration;
using Client.Models.AutoTRGeneration;
using Client.Models.Common;
using Client.Models.MultiStoreroomPart;

using Common.Constants;
using DataContracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Client.Controllers
{
    public class AutoTRGenerationController : SomaxBaseController
    {
        [CheckUserSecurity(securityType = SecurityConstants.PartTransfer_Auto_Transfer_Generation)]
        public ActionResult Index()
        {
            AutoTRGenerationVM objAutoTRGenerationVM = new AutoTRGenerationVM();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            var StoreroomList = commonWrapper.GetStoreroomList(StoreroomAuthType.Maintain).Count();
            if (StoreroomList > 0)
            {
                objAutoTRGenerationVM.IsMaintain = true;
            }
            LocalizeControls(objAutoTRGenerationVM, LocalizeResourceSetConstants.PartTransfer);
            return View(objAutoTRGenerationVM);
        }
        #region Storeroom chunk lookup list
        public string StoreroomLookupListchunksearch_AutoTRGeneration(int? draw, int? start, int? length, long StoreroomId = 0,
            string Name = "", string Description = "", bool InactiveFlag = true)
        {
            List<StoreroomLookupModel> modelList = new List<StoreroomLookupModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            string order = Convert.ToInt32(Request.Form.GetValues("order[0][column]")[0]) == 1 ? "0" : "1";
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            start = start.HasValue
              ? start / length
              : 0;
            int skip = start * length ?? 0;

            Name = !string.IsNullOrEmpty(Name) ? Name.Trim() : string.Empty;
            Description = !string.IsNullOrEmpty(Description) ? Description.Trim() : string.Empty;

            modelList = commonWrapper.GetStoreroomLookupListGridData(order, orderDir, skip, length.Value,
                StoreroomId, Name, Description);

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
        public JsonResult GetStoreroomLookupListSelectAllData(string colname, string coldir, long StoreroomId = 0,
           string name = "", bool InactiveFlag = true)
        {
            List<StoreroomLookupModel> modelList = new List<StoreroomLookupModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            name = !string.IsNullOrEmpty(name) ? name.Trim() : string.Empty;
            modelList = commonWrapper.GetStoreroomLookupListGridData(colname, coldir, 0, 100000, StoreroomId, name);
            var jsonResult = Json(modelList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion

        [HttpPost]
        public string GetAutoTRGenerationGridData(int? draw, int? start, int? length, string[] StoreroomIdList, string Order = "1")
        {
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            start = start.HasValue ? start / length : 0;
            int skip = start * length ?? 0;
            TransferRequestWrapper TRWrapper = new TransferRequestWrapper(userData);
            var storeroomIds = StoreroomIdList != null && StoreroomIdList.Length > 0 ? string.Join(",", StoreroomIdList) : string.Empty;
            List<AutoTRGenerationSearchModel> TRList = TRWrapper.GetAutoTRGenerateChunkList(skip, length ?? 0, Order, orderDir, storeroomIds);
            var totalRecords = 0;
            var recordsFiltered = 0;
            if (TRList != null && TRList.Count > 0)
            {
                recordsFiltered = TRList[0].TotalCount;
                totalRecords = TRList[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }
            int initialPage = start.Value;
            var filteredResult = TRList
              .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }

        [HttpPost]
        public JsonResult GetAutoTRGenerationSearchSelectAllData(string[] StoreroomIdList, string colname, string coldir)
        {
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            TransferRequestWrapper TRWrapper = new TransferRequestWrapper(userData);
            var vendorids = StoreroomIdList != null && StoreroomIdList.Length > 0 ? string.Join(",", StoreroomIdList) : string.Empty;
            List<AutoTRGenerationSearchModel> TRList = TRWrapper.GetAutoTRGenerateChunkList(0, 100000, colname, coldir, vendorids);
            var jsonResult = Json(TRList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #region Auto TR Generation Grid Print
        [HttpPost]
        public string GetAutoTRGenerationGridPrintData(string[] StoreroomIdLists, string colname, string coldir)
        {
            AutoTRGenerationPrintModel objAutoTRGenerationPrintModel;
            List<AutoTRGenerationPrintModel> AutoTRGenerationPrintModelList = new List<AutoTRGenerationPrintModel>();

            int lengthForPrint = 100000;
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            TransferRequestWrapper TRWrapper = new TransferRequestWrapper(userData);
            var storeroomids = StoreroomIdLists != null && StoreroomIdLists.Length > 0 ? string.Join(",", StoreroomIdLists) : string.Empty;
            List<AutoTRGenerationSearchModel> TRList = TRWrapper.GetAutoTRGenerateChunkList(0, lengthForPrint, colname, coldir, storeroomids);

            foreach (var item in TRList)
            {
                objAutoTRGenerationPrintModel = new AutoTRGenerationPrintModel();
                objAutoTRGenerationPrintModel.PartIdClientLookupId = item.PartIdClientLookupId;
                objAutoTRGenerationPrintModel.RequestStr = item.RequestStr;
                objAutoTRGenerationPrintModel.IssueStr = item.IssueStr;
                objAutoTRGenerationPrintModel.PartDescription = item.PartDescription;
                objAutoTRGenerationPrintModel.TransferQuantity = item.TransferQuantity;
                objAutoTRGenerationPrintModel.Max = item.Max;
                objAutoTRGenerationPrintModel.Min = item.Min;
                objAutoTRGenerationPrintModel.OnHand = item.OnHand;
                AutoTRGenerationPrintModelList.Add(objAutoTRGenerationPrintModel);
            }
            return JsonConvert.SerializeObject(new { data = AutoTRGenerationPrintModelList }, JsonSerializerDateSettings);
        }
        #endregion

        #region TR AutoGeneration Create
        [HttpPost]
        public JsonResult CreateAutoTRGeneration(List<PartTranProcessTableModel> partTranProcessTableLists)
        {
            StoreroomTransfer storeroomTransfer = new StoreroomTransfer();
            if (partTranProcessTableLists != null && partTranProcessTableLists.Count > 0)
            {
                TransferRequestWrapper trWrapper = new TransferRequestWrapper(userData);
                storeroomTransfer = trWrapper.STransAutoGenerate_V2(partTranProcessTableLists);
                if (storeroomTransfer.ErrorMessages != null && storeroomTransfer.ErrorMessages.Count > 0)
                {
                    return Json(new { Result = Client.Common.JsonReturnEnum.failed.ToString(), error=storeroomTransfer.ErrorMessages}, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = Client.Common.JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { Result = Client.Common.JsonReturnEnum.failed.ToString() , error = "Not Selected" }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}