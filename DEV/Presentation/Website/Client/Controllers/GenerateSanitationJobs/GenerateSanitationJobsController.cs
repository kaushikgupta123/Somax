using System;
using System.Collections.Generic;
using System.Linq;
using Common.Constants;
using System.Web.Mvc;
using Client.Common;
using Client.BusinessWrapper.Common;
using Client.Controllers.Common;
using Client.ActionFilters;
using System.Threading.Tasks;
using Client.Models.Equipment;
using Client.BusinessWrapper;
using Client.Models.GenerateSanitationJobs;
using Newtonsoft.Json;
using System.Globalization;
using Client.BusinessWrapper.GenerateSanitationJobs;

namespace Client.Controllers.GenerateSanitationJobs
{
    [CheckUserSecurity(securityType = SecurityConstants.Sanitation_Job_Gen)]
    public class GenerateSanitationJobsController : SomaxBaseController
    {
        // GET: GenerateSanitationJobs
        public ActionResult Index()
        {
            GenerateSanitationJobsVM objGenerateSanitationJobsVM = new GenerateSanitationJobsVM();
            GenerateSanitationJobsModel generateSanitationJobsModel = new GenerateSanitationJobsModel();
            Task taskAssetGroup1LookUp;
            Task taskAssetGroup2LookUp;
            Task taskAssetGroup3LookUp;
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);
            CommonWrapper pWrapper = new CommonWrapper(userData);

            #region task           

            List<AssetGroup1Model> astGroup1 = new List<AssetGroup1Model>();
            taskAssetGroup1LookUp = Task.Factory.StartNew(() => astGroup1 = eWrapper.GetAssetGroup1Dropdowndata());

            List<AssetGroup2Model> astGroup2 = new List<AssetGroup2Model>();
            taskAssetGroup2LookUp = Task.Factory.StartNew(() => astGroup2 = eWrapper.GetAssetGroup2Dropdowndata());

            List<AssetGroup3Model> astGroup3 = new List<AssetGroup3Model>();
            taskAssetGroup3LookUp = Task.Factory.StartNew(() => astGroup3 = eWrapper.GetAssetGroup3Dropdowndata());

            Task.WaitAll(taskAssetGroup1LookUp, taskAssetGroup2LookUp, taskAssetGroup3LookUp);
            #endregion
            var ScheduleTypeList = UtilityFunction.GetSanitationScheduleType();
            if (ScheduleTypeList != null)
            {
                generateSanitationJobsModel.ScheduleTypeList = ScheduleTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            var AllLookUps = pWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                var OnDemandList = AllLookUps.Where(x => x.ListName == LookupListConstants.SANIT_ON_DEMAND).ToList();
                if (OnDemandList != null)
                {
                    generateSanitationJobsModel.OnDemandList = OnDemandList.Select(x => new SelectListItem { Text = x.ListValue + "  -  " + x.Description, Value = x.ListValue });
                }
                if (astGroup1 != null)
                {
                    generateSanitationJobsModel.AssetGroup1List = astGroup1.Select(x => new SelectListItem { Text = x.AssetGroup1DescWithClientLookupId, Value = x.AssetGroup1Id.ToString() });
                }
                if (astGroup2 != null)
                {
                    generateSanitationJobsModel.AssetGroup2List = astGroup2.Select(x => new SelectListItem { Text = x.AssetGroup2DescWithClientLookupId, Value = x.AssetGroup2Id.ToString() });
                }
                if (astGroup3 != null)
                {
                    generateSanitationJobsModel.AssetGroup3List = astGroup3.Select(x => new SelectListItem { Text = x.AssetGroup3DescWithClientLookupId, Value = x.AssetGroup3Id.ToString() });
                }
            }
            this.GetAssetGroupHeaderName(generateSanitationJobsModel);
            objGenerateSanitationJobsVM.userData = userData;
            objGenerateSanitationJobsVM.generateSanitationJobsModel = generateSanitationJobsModel;
            LocalizeControls(objGenerateSanitationJobsVM, LocalizeResourceSetConstants.SanitationDetails);
            return View(objGenerateSanitationJobsVM);
        }
        private void GetAssetGroupHeaderName(GenerateSanitationJobsModel generateSanitationJobsModel)
        {
            generateSanitationJobsModel.AssetGroup1Name = String.IsNullOrEmpty(this.userData.Site.AssetGroup1Name) ? "Asset Group 1" : this.userData.Site.AssetGroup1Name;
            generateSanitationJobsModel.AssetGroup2Name = String.IsNullOrEmpty(this.userData.Site.AssetGroup1Name) ? "Asset Group 2" : this.userData.Site.AssetGroup2Name;
            generateSanitationJobsModel.AssetGroup3Name = String.IsNullOrEmpty(this.userData.Site.AssetGroup1Name) ? "Asset Group 3" : this.userData.Site.AssetGroup3Name;

        }
        #region Generate Sanitation Job
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GenerateSanitationJobs(GenerateSanitationJobsVM generateSanitationJobsVM)
        {
            string wrapperMsg = string.Empty;

            bool Preview = true;
            string MasterSanGenerateMethod = userData.DatabaseKey.Client.MasterSanGenerateMethod;
            if (MasterSanGenerateMethod == MasterSanitationJobGenerationMethodConstants.NoPreview)
            {
                Preview = false;
            }
            if (ModelState.IsValid)
            {

                if (Preview)
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Previewval = Preview }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    GenerateSanitationJobsWrapper pWrapper = new GenerateSanitationJobsWrapper(userData);
                    
                    List<Int64> GenerateSanitationJobs = new List<Int64>();
                    wrapperMsg = pWrapper.GenerateSanitationJobs(generateSanitationJobsVM.generateSanitationJobsModel, ref GenerateSanitationJobs);

                    if (!string.IsNullOrEmpty(wrapperMsg))
                    {
                        if (generateSanitationJobsVM.generateSanitationJobsModel.chkPrintSanitation == true)
                        {
                            return Json(new { Msg = wrapperMsg, SanitationjobList = GenerateSanitationJobs, Previewval = Preview }, JsonRequestBehavior.AllowGet);
                        }
                        {
                            return Json(new { Msg = wrapperMsg, SanitationjobList = new List<Int64>(), Previewval = Preview }, JsonRequestBehavior.AllowGet);
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
        [HttpPost]
        public ActionResult CreateSanitationJobGenPreview(SanitationJobBatchEntryModel generateSanitationJobsModel)
        {
            string wrapperMsg = string.Empty;
            GenerateSanitationJobsModel obj = new GenerateSanitationJobsModel();
            GenerateSanitationJobsWrapper pWrapper = new GenerateSanitationJobsWrapper(userData);
            List<Int64> createdSanitationJobList = new List<Int64>();

            long SanMasterBatchHeaderId = generateSanitationJobsModel.list.Count > 0 ? generateSanitationJobsModel.list.Select(m => m.SanMasterBatchHeaderId).FirstOrDefault() : 0;


                wrapperMsg = pWrapper.CreateSannitationJobPreview(generateSanitationJobsModel, ref createdSanitationJobList);
            if (!string.IsNullOrEmpty(wrapperMsg))
            {
                if (generateSanitationJobsModel.chkPrintSan == true)
                {
                    return Json(new { Msg = wrapperMsg, SanitationjobList = createdSanitationJobList, ReturnSanMasterBatchHeaderId = SanMasterBatchHeaderId }, JsonRequestBehavior.AllowGet);
                }
                {
                    return Json(new { Msg = wrapperMsg, SanitationjobList = new List<Int64>(), ReturnSanMasterBatchHeaderId = SanMasterBatchHeaderId }, JsonRequestBehavior.AllowGet);
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
        public string GetSanitationJobGenGrid(int? draw, int? start, int? length, string ScheduleType = "", DateTime? GeneratedThroughDate = null, string OnDemandGroup = "", List<string> AssetGroup1Ids = null, List<string> AssetGroup2Ids = null, List<string> AssetGroup3Ids = null, DateTime? SanMasterGenDueDate = null, string EquipmentClientLookupId = "", string EquipmentName = "", string SanMasterGenShift = "", string SanMasterGenDesc = ""
 ,long ReturnSanMasterBatchHeaderId=0)
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
            GenerateSanitationJobsWrapper pWrapper = new GenerateSanitationJobsWrapper(userData);
            List<GenerateSanitationJobsSearchModel> SanBatchEntryGenSanitationJobModelList = new List<GenerateSanitationJobsSearchModel>();
            SanBatchEntryGenSanitationJobModelList = pWrapper.GetSanBatchEntryGenSanitationJobGridData(order, orderDir, skip, length ?? 0, ScheduleType, GeneratedThroughDate, OnDemandGroup, AssetGroup1Ids, AssetGroup2Ids, AssetGroup3Ids, SanMasterGenDueDate, EquipmentClientLookupId, EquipmentName, SanMasterGenShift, SanMasterGenDesc, ReturnSanMasterBatchHeaderId);

             var totalRecords = 0;
            var recordsFiltered = 0;
            long SanMasterBatchHeaderId = 0;
            if (SanBatchEntryGenSanitationJobModelList != null && SanBatchEntryGenSanitationJobModelList.Count > 0)
            {
                recordsFiltered = SanBatchEntryGenSanitationJobModelList[0].TotalCount;
                totalRecords = SanBatchEntryGenSanitationJobModelList[0].TotalCount;
                SanMasterBatchHeaderId = SanBatchEntryGenSanitationJobModelList[0].SanMasterBatchHeaderId;
            }
            int initialPage = start.Value;
            var filteredResult = SanBatchEntryGenSanitationJobModelList
              .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult , ReturnSanMasterBatchHeaderId = SanMasterBatchHeaderId }, JsonSerializerDateSettings);
        }
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public string GetSanitationJobGenGridPrintData(string colname, string coldir, string ScheduleType = "", DateTime? GeneratedThroughDate = null, string OnDemandGroup = "", List<string> AssetGroup1Ids = null, List<string> AssetGroup2Ids = null, List<string> AssetGroup3Ids = null, DateTime? SanMasterGenDueDate = null, string EquipmentClientLookupId = "", string EquipmentName = "", string SanMasterGenShift = "", string SanMasterGenDesc = "", long ReturnSanMasterBatchHeaderId = 0)
        {
            GenerateSanitationJobsPrintModel objPMGeneratedWorkOrderPrintModel;
            List<GenerateSanitationJobsPrintModel> PMGeneratedWorkOrderPrintModelList = new List<GenerateSanitationJobsPrintModel>();
            EquipmentClientLookupId = EquipmentClientLookupId.Replace("%", "[%]");
            EquipmentName = EquipmentName.Replace("%", "[%]");
            SanMasterGenShift = SanMasterGenShift.Replace("%", "[%]");
            SanMasterGenDesc = SanMasterGenDesc.Replace("%", "[%]");

            int lengthForPrint = 100000;
            GenerateSanitationJobsWrapper pWrapper = new GenerateSanitationJobsWrapper(userData);
            var PMGenerateWorkOrderList = pWrapper.GetSanBatchEntryGenSanitationJobGridData(colname, coldir, 0, lengthForPrint, ScheduleType, GeneratedThroughDate, OnDemandGroup, AssetGroup1Ids, AssetGroup2Ids, AssetGroup3Ids, SanMasterGenDueDate, EquipmentClientLookupId, EquipmentName, SanMasterGenShift, SanMasterGenDesc, ReturnSanMasterBatchHeaderId);
          
            foreach (var item in PMGenerateWorkOrderList)
            {
                objPMGeneratedWorkOrderPrintModel = new GenerateSanitationJobsPrintModel();
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
                objPMGeneratedWorkOrderPrintModel.Shift = item.Shift;
                objPMGeneratedWorkOrderPrintModel.Description = item.Description;
                PMGeneratedWorkOrderPrintModelList.Add(objPMGeneratedWorkOrderPrintModel);
            }
            return JsonConvert.SerializeObject(new { data = PMGeneratedWorkOrderPrintModelList }, JsonSerializerDateSettings);
        }

        public JsonResult GetSearchAllSanitationJobGenGrid(string colname, string coldir, string ScheduleType = "", DateTime? GeneratedThroughDate = null, string OnDemandGroup = "", List<string> AssetGroup1Ids = null, List<string> AssetGroup2Ids = null, List<string> AssetGroup3Ids = null, DateTime? SanMasterGenDueDate = null, string EquipmentClientLookupId = "", string EquipmentName = "", string SanMasterGenShift = "", string SanMasterGenDesc = "", long ReturnSanMasterBatchHeaderId = 0)
        {


            GenerateSanitationJobsWrapper pWrapper = new GenerateSanitationJobsWrapper(userData);
            var modelList = pWrapper.GetSanBatchEntryGenSanitationJobGridData(colname, coldir, 0, 100000, ScheduleType, GeneratedThroughDate, OnDemandGroup, AssetGroup1Ids, AssetGroup2Ids, AssetGroup3Ids, SanMasterGenDueDate, EquipmentClientLookupId, EquipmentName, SanMasterGenShift, SanMasterGenDesc, ReturnSanMasterBatchHeaderId);

            return Json(modelList, JsonRequestBehavior.AllowGet);
        }


    }
}