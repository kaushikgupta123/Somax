using Client.BusinessWrapper;
using Client.BusinessWrapper.PrevMaintWrapper;
using Client.Common;
using Client.Controllers.Common;
using Client.Models.PartCycleCount;
using Client.Models.Parts;
using Client.BusinessWrapper.Common;
using Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Client.BusinessWrapper.PartCycleCount;
using Newtonsoft.Json;
using DataContracts;
using Client.ActionFilters;

namespace Client.Controllers.PartCycleCount
{
    public class PartCycleCountController : SomaxBaseController
    {
        // GET: PartCycleCount
        [CheckUserSecurity(securityType = SecurityConstants.Parts_CycleCount)]
        public ActionResult Index()
        {
            PartCycleCountVM objVM = new PartCycleCountVM();
            objVM.partCycleCountModel = new PartCycleCountModel();
            PartCycleCountWrapper pWrapper = new PartCycleCountWrapper(userData);
            
            var AllLookUps = pWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                var StockTypeList = AllLookUps.Where(x => x.ListName == LookupListConstants.STOCK_TYPE).ToList();
                if (StockTypeList != null)
                {
                    objVM.partCycleCountModel.StockTypeList = StockTypeList.Select(x => new SelectListItem { Text = x.ListValue + "  -  " + x.Description, Value = x.ListValue });
                }
            }
            //V2-687
            if (userData.DatabaseKey.Client.UseMultiStoreroom)
            {
                CommonWrapper commonWrapper = new CommonWrapper(userData);
                objVM.partCycleCountModel.StoreroomList = commonWrapper.GetStoreroomList(StoreroomAuthType.PhysicalInventory);
            }
            objVM.partCycleCountModel.MultiStoreroom = userData.DatabaseKey.Client.UseMultiStoreroom;
            // 
            LocalizeControls(objVM, LocalizeResourceSetConstants.PartDetails);
            return View(objVM);
        }

        #region GeneratePartCycleCount
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ValidateData(PartCycleCountVM partcountVM)
        {
            string wrapperMsg = string.Empty;       
            
            if (ModelState.IsValid)
            {
                    return Json(new { Result = JsonReturnEnum.success.ToString()}, JsonRequestBehavior.AllowGet);
            }

            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(new { Msg = ModelValidationFailedMessage }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion


        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public string GetPartGridData(int? draw, int? start, int? length, string Area = "",  string Row = "", string Shelf = "", string Bin = "", List<string> StockType = null, bool Critical = false, bool Consignment = false, DateTime? GenerateThrough = null, string PartClientLookupId = "", string PartDescription = "", string Section = "", string Order = "0", long StoreroomId = 0
        )
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var colname = Request.Form.GetValues("columns[" + order + "][name]");
          
            start = start.HasValue
               ? start / length
               : 0;
            int skip = start * length ?? 0;
            List<string> typeList = new List<string>();
            PartCycleCountWrapper pWrapper = new PartCycleCountWrapper(userData);
            List<PartCycleCountSearchModel> PartCycleCountSearchModelList = pWrapper.GetPartCycleCountGridData(Order, orderDir, skip, length ?? 0, Area, Row, Shelf, Bin, StockType, Critical, Consignment, GenerateThrough, PartClientLookupId, PartDescription, Section, StoreroomId);
            var totalRecords = 0;
            var recordsFiltered = 0;
            if (PartCycleCountSearchModelList != null && PartCycleCountSearchModelList.Count > 0)
            {
                recordsFiltered = PartCycleCountSearchModelList[0].TotalCount;
                totalRecords = PartCycleCountSearchModelList[0].TotalCount;
            }
            int initialPage = start.Value;
            var filteredResult = PartCycleCountSearchModelList
              .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
       
        public string GetPartCycleCountGridPrintData(string colname, string coldir, string Area = "",  string Row = "", string Shelf = "", string Bin = "", List<string> StockType = null, bool Critical = false, bool Consignment = false, DateTime? GenerateThrough = null,string PartClientLookupId="",string PartDescription="", string Section="", long StoreroomId = 0)
        {
            PartCycleCountPrintModel objPartCycleCountPrintModel;
            List<PartCycleCountPrintModel> PartCycleCountPrintModelList = new List<PartCycleCountPrintModel>();                   
            int lengthForPrint = 100000;
          
            List<string> typeList = new List<string>();
            PartCycleCountWrapper pWrapper = new PartCycleCountWrapper(userData);
           var  PartCycleCountSearchModelList = pWrapper.GetPartCycleCountGridData(colname, coldir, 0, lengthForPrint, Area, Row, Shelf, Bin, StockType, Critical, Consignment, GenerateThrough, PartClientLookupId, PartDescription, Section, StoreroomId);
            foreach (var item in PartCycleCountSearchModelList)
            {
                objPartCycleCountPrintModel = new PartCycleCountPrintModel();


                objPartCycleCountPrintModel.PartId = item.ClientLookupId;
                objPartCycleCountPrintModel.PartDescription = item.PartDescription;
                objPartCycleCountPrintModel.QtyOnHand = item.QtyOnHand;
                objPartCycleCountPrintModel.Count = item.Count;
                objPartCycleCountPrintModel.Area = item.Area;
                objPartCycleCountPrintModel.Section = item.Section;
                objPartCycleCountPrintModel.Row = item.Row;
                objPartCycleCountPrintModel.Shelf = item.Shelf;
                objPartCycleCountPrintModel.Bin = item.Bin;
                objPartCycleCountPrintModel.Variance = item.Variance;
                PartCycleCountPrintModelList.Add(objPartCycleCountPrintModel);
            }
            return JsonConvert.SerializeObject(new { data = PartCycleCountPrintModelList }, JsonSerializerDateSettings);

        }
        #region Adjust On Hand Quantity
        [HttpPost]
      
        public JsonResult SaveListPartCountFromGrid(List<PartCycleCountSearchModel> list , long StoreroomId=0)
        {
            PartCycleCountWrapper pWrapper = new PartCycleCountWrapper(userData);
            PartHistory returnObj = pWrapper.SaveHandsOnQtyList(list, StoreroomId);
            if (returnObj.ErrorMessages != null && returnObj.ErrorMessages.Count > 0)
            {
                return Json(returnObj.ErrorMessages, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}