using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Globalization;
using Common.Constants;
using Client.BusinessWrapper.PrevMaintWrapper;
using System.Web.Mvc;
using Client.Models.PreventiveMaintenance;
using Client.Common;
using Client.BusinessWrapper.Common;
using Client.Controllers.Common;
using Client.ActionFilters;
using System.Threading.Tasks;
using Client.Models.Equipment;
using Client.BusinessWrapper;

namespace Client.Controllers.Preventive
{
    public class PMGenerateWorkOrdersController : SomaxBaseController
    {
        #region Search
        [CheckUserSecurity(securityType = SecurityConstants.PrevMaint_WO_Gen)]
        public ActionResult Index()
        {
            PrevMaintVM objPrevMaintVM = new PrevMaintVM();
            PMGenerateWorkOrdersModel pmGenWoModel = new PMGenerateWorkOrdersModel();
            Task taskAssetGroup1LookUp;
            Task taskAssetGroup2LookUp;
            Task taskAssetGroup3LookUp;
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            PrevMaintWrapper pWrapper = new PrevMaintWrapper(userData);

            #region task           

            List<AssetGroup1Model> astGroup1 = new List<AssetGroup1Model>();
            taskAssetGroup1LookUp = Task.Factory.StartNew(() => astGroup1 = eWrapper.GetAssetGroup1Dropdowndata());

            List<AssetGroup2Model> astGroup2 = new List<AssetGroup2Model>();
            taskAssetGroup2LookUp = Task.Factory.StartNew(() => astGroup2 = eWrapper.GetAssetGroup2Dropdowndata());

            List<AssetGroup3Model> astGroup3 = new List<AssetGroup3Model>();
            taskAssetGroup3LookUp = Task.Factory.StartNew(() => astGroup3 = eWrapper.GetAssetGroup3Dropdowndata());

            Task.WaitAll( taskAssetGroup1LookUp, taskAssetGroup2LookUp, taskAssetGroup3LookUp);
            #endregion
            var ScheduleTypeList = UtilityFunction.GetScheduleType();
            if (ScheduleTypeList != null)
            {
                pmGenWoModel.ScheduleTypeList = ScheduleTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });               
            }
            var AllLookUps = pWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {               
                var OnDemandList = AllLookUps.Where(x => x.ListName == LookupListConstants.Preventive_Maint_Ondemand_Grp).ToList();
                if (OnDemandList != null)
                {
                    pmGenWoModel.OnDemandList = OnDemandList.Select(x => new SelectListItem { Text = x.ListValue + "  -  " + x.Description, Value = x.ListValue });
                }
                var PrevMaintSchedTypeList = AllLookUps.Where(x => x.ListName == LookupListConstants.Preventive_Maint_WO_Type).ToList();
                if (PrevMaintSchedTypeList != null)
                {
                    pmGenWoModel.PrevMaintSchedTypeList = PrevMaintSchedTypeList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue });
                }
                var PrevMaintMasterTypeList = AllLookUps.Where(x => x.ListName == LookupListConstants.Preventive_Maint_Type).ToList();
                if (PrevMaintMasterTypeList != null)
                {
                    pmGenWoModel.PrevMaintMasterTypeList = PrevMaintMasterTypeList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue });
                }
                if (astGroup1 != null)
                {
                    pmGenWoModel.AssetGroup1List = astGroup1.Select(x => new SelectListItem { Text = x.AssetGroup1DescWithClientLookupId, Value = x.AssetGroup1Id.ToString() });
                }
                if (astGroup2 != null)
                {
                    pmGenWoModel.AssetGroup2List = astGroup2.Select(x => new SelectListItem { Text = x.AssetGroup2DescWithClientLookupId, Value = x.AssetGroup2Id.ToString() });
                }
                if (astGroup3 != null)
                {
                    pmGenWoModel.AssetGroup3List = astGroup3.Select(x => new SelectListItem { Text = x.AssetGroup3DescWithClientLookupId, Value = x.AssetGroup3Id.ToString() });
                }
                //V2-1082
                var Shift = AllLookUps.Where(x => x.ListName == LookupListConstants.Shift).ToList();
                if (Shift != null)
                {
                    var tmpShift = Shift.GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).ToList();
                    pmGenWoModel.ShiftList = tmpShift.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue });
                }
                var DownRequiredStatusList = UtilityFunction.DownRequiredStatusTypesWithBoolValue();
                if (DownRequiredStatusList != null)
                {
                    pmGenWoModel.DownRequiredInactiveFlagList = DownRequiredStatusList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                }
            }
            this.GetAssetGroupHeaderName(pmGenWoModel);
            objPrevMaintVM.userData = userData;
            objPrevMaintVM.pMGenerateWorkOrdersModel = pmGenWoModel;          
            LocalizeControls(objPrevMaintVM, LocalizeResourceSetConstants.PrevMaintDetails);
            return View(objPrevMaintVM);
        }
     
        public JsonResult GetOnDemandGroupList()
        {
            List<SelectListItem> OnDemandGroupList = new List<SelectListItem>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                var onDemandList = AllLookUps.Where(x => x.ListName == LookupListConstants.Preventive_Maint_Ondemand_Grp).ToList();
                if (onDemandList != null)
                {
                    OnDemandGroupList = onDemandList.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue }).ToList();
                }
            }
            return Json(OnDemandGroupList, JsonRequestBehavior.AllowGet);
        }
     
        private void GetAssetGroupHeaderName(PMGenerateWorkOrdersModel pmGenWoModel)
        {
            pmGenWoModel.AssetGroup1Name = String.IsNullOrEmpty(this.userData.Site.AssetGroup1Name) ? "Asset Group 1" : this.userData.Site.AssetGroup1Name;
            pmGenWoModel.AssetGroup2Name = String.IsNullOrEmpty(this.userData.Site.AssetGroup1Name) ? "Asset Group 2" : this.userData.Site.AssetGroup2Name;
            pmGenWoModel.AssetGroup3Name = String.IsNullOrEmpty(this.userData.Site.AssetGroup1Name) ? "Asset Group 3" : this.userData.Site.AssetGroup3Name;

        }
        #region GenerateWO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GenerateWorkOrder(PrevMaintVM prevMaintVM)
        {
            string wrapperMsg = string.Empty;

            bool Preview = true;
            string PMWOGenerateMethod = userData.DatabaseKey.Client.PMWOGenerateMethod;
            if (PMWOGenerateMethod == PMWOGenerationMethodConstants.NoPreview)
            {
                Preview = false;
            }
            if (ModelState.IsValid)
            {

                if(Preview)
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString() , Previewval= Preview }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    PrevMaintWrapper pWrapper = new PrevMaintWrapper(userData);
                    List<Int64> createdWorkOrderList = new List<Int64>();
                    wrapperMsg = pWrapper.CreatePMWorkOrder(prevMaintVM.pMGenerateWorkOrdersModel, ref createdWorkOrderList);
                   
                    if (!string.IsNullOrEmpty(wrapperMsg))
                    {
                        if (prevMaintVM.pMGenerateWorkOrdersModel.chkPrintWorkOrder == true)
                        {
                            return Json(new { Msg = wrapperMsg, WoList = createdWorkOrderList, Previewval = Preview }, JsonRequestBehavior.AllowGet);
                        }
                        {
                            return Json(new { Msg = wrapperMsg, WoList = new List<Int64>(), Previewval = Preview }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        string ModelValidationFailedMessage = string.Empty;
                        ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                        return Json(new { Msg = ModelValidationFailedMessage }, JsonRequestBehavior.AllowGet);                       
                    }
                }
               
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
        public string GetPreventiveMainGenWOGrid(int? draw, int? start, int? length,  string ScheduleType = "",  DateTime ? GeneratedThroughDate = null, string OnDemandGroup = "", List<string> AssetGroup1Ids = null, List<string> AssetGroup2Ids = null, List<string> AssetGroup3Ids = null, List<string> WOType = null, List<string> PMType = null, DateTime ? PrevBEDueDate = null, string EquipmentClientLookupId = "",string EquipmentName = "",string PrevMaintMasterClientLookupId = "",string PrevMaintMasterDescription = ""
 , long ReturnPrevMaintBatchHeaderId = 0, bool? downRequired = null, List<string> shifts = null)
        {           
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var colname = Request.Form.GetValues("columns[" + order + "][name]");
            string _GeneratedThroughDate = string.Empty;
           
           
            start = start.HasValue
               ? start / length
               : 0;
            int skip = start * length ?? 0;
            List<string> typeList = new List<string>();
            PrevMaintWrapper pWrapper = new PrevMaintWrapper(userData);
            List<PMGenerateWorkOrdersSearchModel> PMGenerateWorkOrdersSearchModelList = pWrapper.GetPMGenerateWorkOrdersGridData( order, orderDir, skip, length ?? 0,  ScheduleType, GeneratedThroughDate, OnDemandGroup, AssetGroup1Ids, AssetGroup2Ids, AssetGroup3Ids, WOType, PMType, PrevBEDueDate, EquipmentClientLookupId, EquipmentName, PrevMaintMasterClientLookupId, PrevMaintMasterDescription, ReturnPrevMaintBatchHeaderId, downRequired, shifts);
            var totalRecords = 0;
            var recordsFiltered = 0;
            long PrevMaintBatchHeaderId = 0;
            if (PMGenerateWorkOrdersSearchModelList != null && PMGenerateWorkOrdersSearchModelList.Count > 0)
            {
                recordsFiltered = PMGenerateWorkOrdersSearchModelList[0].TotalCount;
                totalRecords = PMGenerateWorkOrdersSearchModelList[0].TotalCount;
                PrevMaintBatchHeaderId = PMGenerateWorkOrdersSearchModelList[0].PrevMaintBatchHeaderId;
            }
            int initialPage = start.Value;
            var filteredResult = PMGenerateWorkOrdersSearchModelList
              .ToList();
         
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, ReturnPrevMaintBatchHeaderId = PrevMaintBatchHeaderId }, JsonSerializerDateSettings);
        }


        public JsonResult GetSearchAllPreventiveMainGenWOGrid(string colname, string coldir, string ScheduleType = "", DateTime? GeneratedThroughDate = null, string OnDemandGroup = "", List<string> AssetGroup1Ids = null, List<string> AssetGroup2Ids = null, List<string> AssetGroup3Ids = null, List<string> WOType = null, List<string> PMType = null, DateTime? PrevBEDueDate = null, string EquipmentClientLookupId = "", string EquipmentName = "", string PrevMaintMasterClientLookupId = "", string PrevMaintMasterDescription = "", long ReturnPrevMaintBatchHeaderId = 0, bool? downRequired = null, List<string> shifts = null)
        {
            PrevMaintWrapper pWrapper = new PrevMaintWrapper(userData);
            var modelList = pWrapper.GetPMGenerateWorkOrdersGridData(colname, coldir, 0, 100000, ScheduleType, GeneratedThroughDate, OnDemandGroup, AssetGroup1Ids, AssetGroup2Ids, AssetGroup3Ids, WOType, PMType, PrevBEDueDate, EquipmentClientLookupId, EquipmentName, PrevMaintMasterClientLookupId, PrevMaintMasterDescription,ReturnPrevMaintBatchHeaderId, downRequired, shifts);
          
            return Json(modelList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreatePMWorkOrderPreview(PrevBatchEntryModel dataList)
        {
            string wrapperMsg = string.Empty;
            PMGenerateWorkOrdersModel obj = new PMGenerateWorkOrdersModel();
            PrevMaintWrapper pWrapper = new PrevMaintWrapper(userData);           
            List<Int64> createdWorkOrderList = new List<Int64>();

            long PrevMaintBatchHeaderId = dataList.list.Count > 0 ? dataList.list.Select(m => m.PrevMaintBatchHeaderId).FirstOrDefault() : 0;
            wrapperMsg = pWrapper.CreatePMWorkOrderPreview(dataList, ref createdWorkOrderList);          
            if (!string.IsNullOrEmpty(wrapperMsg))
            {             
                if (dataList.chkPrintWorkOrder== true)
                {
                    return Json(new { Msg = wrapperMsg, WoList = createdWorkOrderList , ReturnPrevMaintBatchHeaderId = PrevMaintBatchHeaderId }, JsonRequestBehavior.AllowGet);
                }
                {
                    return Json(new { Msg = wrapperMsg, WoList = new List<Int64>(), ReturnPrevMaintBatchHeaderId = PrevMaintBatchHeaderId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(new { Msg = ModelValidationFailedMessage }, JsonRequestBehavior.AllowGet);              
            }


        }
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public string GetPreventiveMainGenWOGridPrintData(string colname, string coldir, string ScheduleType = "", DateTime? GeneratedThroughDate = null, string OnDemandGroup = "", List<string> AssetGroup1Ids = null, List<string> AssetGroup2Ids = null, List<string> AssetGroup3Ids = null, List<string> WOType = null, List<string> PMType = null, DateTime? PrevBEDueDate = null, string EquipmentClientLookupId = "", string EquipmentName = "", string PrevMaintMasterClientLookupId = "", string PrevMaintMasterDescription = "", long ReturnPrevMaintBatchHeaderId = 0, bool? downRequired = null, List<string> shifts = null)
        {
            PMGeneratedWorkOrderPrintModel objPMGeneratedWorkOrderPrintModel;
            List<PMGeneratedWorkOrderPrintModel> PMGeneratedWorkOrderPrintModelList = new List<PMGeneratedWorkOrderPrintModel>();
            EquipmentClientLookupId = EquipmentClientLookupId.Replace("%", "[%]");
            EquipmentName = EquipmentName.Replace("%", "[%]");
            PrevMaintMasterClientLookupId = PrevMaintMasterClientLookupId.Replace("%", "[%]");
            PrevMaintMasterDescription = PrevMaintMasterDescription.Replace("%", "[%]");
         
            int lengthForPrint = 100000;
            PrevMaintWrapper pWrapper = new PrevMaintWrapper(userData);
            var PMGenerateWorkOrderList = pWrapper.GetPMGenerateWorkOrdersGridData(colname, coldir, 0, lengthForPrint, ScheduleType, GeneratedThroughDate, OnDemandGroup, AssetGroup1Ids, AssetGroup2Ids, AssetGroup3Ids, WOType, PMType, PrevBEDueDate, EquipmentClientLookupId, EquipmentName, PrevMaintMasterClientLookupId, PrevMaintMasterDescription, ReturnPrevMaintBatchHeaderId, downRequired, shifts);
            foreach (var item in PMGenerateWorkOrderList)
            {
                objPMGeneratedWorkOrderPrintModel = new PMGeneratedWorkOrderPrintModel();               
                if (item.DueDate != null && item.DueDate != default(DateTime))
                {
                    objPMGeneratedWorkOrderPrintModel.DueDate = item.DueDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    objPMGeneratedWorkOrderPrintModel.DueDate = "";
                }

                objPMGeneratedWorkOrderPrintModel.EquipmentClientLookupId = item.EquipmentClientLookupId;
                objPMGeneratedWorkOrderPrintModel.EquipmentName = item.EquipmentName;
                objPMGeneratedWorkOrderPrintModel.PrevMaintMasterClientLookupId = item.PrevMaintMasterClientLookupId;
                objPMGeneratedWorkOrderPrintModel.PrevMaintMasterDescription = item.PrevMaintMasterDescription;
                if (item.PMRequiredDate != null && item.PMRequiredDate != default(DateTime))
                {
                    objPMGeneratedWorkOrderPrintModel.RequiredDate = item.PMRequiredDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    objPMGeneratedWorkOrderPrintModel.RequiredDate = "";
                }

                objPMGeneratedWorkOrderPrintModel.AssignedToName = item.AssignedTo_Name;
                objPMGeneratedWorkOrderPrintModel.DownRequired= item.DownRequired;
                objPMGeneratedWorkOrderPrintModel.Shift = item.Shift;
                PMGeneratedWorkOrderPrintModelList.Add(objPMGeneratedWorkOrderPrintModel);
            }
            return JsonConvert.SerializeObject(new { data = PMGeneratedWorkOrderPrintModelList }, JsonSerializerDateSettings);
        }

        #endregion
    }
}