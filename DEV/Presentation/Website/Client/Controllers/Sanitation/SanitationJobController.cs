using Client.ActionFilters;
using Client.BusinessWrapper;
using Client.BusinessWrapper.Common;
using Client.Common;
using Client.Controllers.Common;
using Client.Localization;
using Client.Models;
using Client.Models.Common;
using Client.Models.Sanitation;
using Client.Models.Work_Order;

using Common.Constants;

using DataContracts;

using Newtonsoft.Json;

using Rotativa;
using Rotativa.Options;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Client.Controllers.Sanitation
{
    public class SanitationJobController : SomaxBaseController
    {
        public static CommonWrapper comWrapper { get; set; }

        [CheckUserSecurity(securityType = SecurityConstants.SanitationJob)]
        public ActionResult Index()
        {
            SanitationVM objSanitationVM = new SanitationVM();
            SanitationJobSearchModel objSanitationJobSearchModel = new SanitationJobSearchModel();
            SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            objSanitationVM.sanitationJobModel = new SanitationJobModel();

            comWrapper = new CommonWrapper(userData);
            var sanitExtractedList = UtilityFunction.SanitExtractedList();
            if (sanitExtractedList != null)
            {
                objSanitationJobSearchModel.SanitExtractedList = sanitExtractedList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }

            if (TempData["RedirectFromWO"] != null)
            {
                long SanitationJobId = 0;
                long.TryParse(Convert.ToString(TempData["RedirectFromWO"]), out SanitationJobId);
                if (SanitationJobId != 0)
                {
                    SanitationJobModel SJM = new SanitationJobModel();
                    SanitationJobDetailsModel details = sWrapper.RetrieveBy_SanitationJobId(SanitationJobId);
                    details.ExternalSanitation = userData.Site.ExternalSanitation;
                    details.ClientOnPremise = ClientOnPremiseVal();
                    objSanitationVM.JobDetailsModel = details;
                    SJM.ImageURI = SanitationImageUrl(SanitationJobId);//comWrapper.GetAzureImageUrl(SanitationJobId, AttachmentTableConstant.Sanitation);
                    objSanitationVM.sanitationJobModel = SJM;
                    objSanitationVM.IsRedirectFromWorkorder = true;
                }
            }
            else
            {
                var schduleJobList = sWrapper.DisplayIdList();
                if (schduleJobList != null)
                {
                    objSanitationJobSearchModel.TextSearchList = schduleJobList.Select(x => new SelectListItem { Text = x.Key, Value = x.Value }).OrderBy(x => x.Text);
                }
                List<DataContracts.LookupList> shiftList = new List<DataContracts.LookupList>();
                var AllLookUpList = sWrapper.GetAllLookUpList();
                var plist = comWrapper.PersonnelList();
                objSanitationJobSearchModel.CreateByList = plist.Select(x => new SelectListItem { Text = x.NameFirst + " " + x.NameLast, Value = x.NameFirst + " " + x.NameLast }).ToList();
                objSanitationJobSearchModel.AssignedList = plist.Select(x => new SelectListItem { Text = x.NameFirst + " " + x.NameLast, Value = x.NameFirst + " " + x.NameLast }).ToList();
                objSanitationJobSearchModel.VerifiedByList = plist.Select(x => new SelectListItem { Text = x.NameFirst + " " + x.NameLast, Value = x.NameFirst + " " + x.NameLast }).ToList();

                var statuslist = comWrapper.GetListFromConstVals("SanitationJobStatus");
                objSanitationJobSearchModel.StatusList = statuslist.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }).ToList();
                if (AllLookUpList != null)
                {
                    shiftList = AllLookUpList.Where(x => x.ListName == LookupListConstants.Shift).ToList(); //GetShiftList();
                }
                if (shiftList != null)
                {
                    objSanitationJobSearchModel.ShiftList = shiftList.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue });
                }

                objSanitationVM.JobDetailsModel = new SanitationJobDetailsModel();
                objSanitationVM.sanitationJobSearchModel = objSanitationJobSearchModel;
            }
            objSanitationVM.security = this.userData.Security;
            objSanitationJobSearchModel.SanitationJobViewSearchList = commonWrapper.PopulateCustomQueryDisplay(AttachmentTableConstant.SanitationJob).OrderBy(x => Convert.ToInt32(x.Value)).ToList(); 
            objSanitationVM.sanitationJobSearchModel = objSanitationJobSearchModel;
            objSanitationVM.JobDetailsModel.ExternalSanitation = userData.Site.ExternalSanitation;
            objSanitationVM.sanitationJobModel.DateRangeDropListForSJCreatedate = UtilityFunction.GetTimeRangeDropForAllStatusFS().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            objSanitationVM.sanitationJobModel.DateRangeDropListForSJ = UtilityFunction.GetTimeRangeDropForCompletedSJ().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            objSanitationVM.sanitationJobModel.DateRangeDropListForFailedSJ = UtilityFunction.GetTimeRangeDropForFailedSJ().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            objSanitationVM.sanitationJobModel.DateRangeDropListForPassedSJ = UtilityFunction.GetTimeRangeDropForPassedSJ().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            objSanitationVM._userdata = this.userData;
            this.GetAssetGroupHeaderName(objSanitationVM);
            LocalizeControls(objSanitationVM, LocalizeResourceSetConstants.SanitationDetails);
            return View(objSanitationVM);
        }
        private void GetAssetGroupHeaderName(SanitationVM objComb)
        {

            objComb.sanitationJobModel.AssetGroup1Name = String.IsNullOrEmpty(this.userData.Site.AssetGroup1Name) ? "Asset Group 1" : this.userData.Site.AssetGroup1Name;
            objComb.sanitationJobModel.AssetGroup2Name = String.IsNullOrEmpty(this.userData.Site.AssetGroup1Name) ? "Asset Group 2" : this.userData.Site.AssetGroup2Name;
            objComb.sanitationJobModel.AssetGroup3Name = String.IsNullOrEmpty(this.userData.Site.AssetGroup1Name) ? "Asset Group 3" : this.userData.Site.AssetGroup3Name;
        }
        #region Populate
        private List<SanitationJobSearchModel> GetSanitSearchResult(List<SanitationJobSearchModel> sJlist, string ClientLookupId = "", string Description = "", string ChargeTo_ClientLookupId = "", string ChargeTo_Name = "",
                                string Status = "", string Shift = "", string AssetGroup1_ClientLookUpId = "", string AssetGroup2_ClientLookUpId = "", string AssetGroup3_ClientLookUpId = "", DateTime? CreateDate = null, string CreateBy = "", string Assigned = "", DateTime? CompleteDate = null, string VerifiedBy = "", DateTime? VerifiedDate = null, string Extracted = "", DateTime? ScheduledDate = null)
        {
            if (!string.IsNullOrEmpty(ClientLookupId))
            {
                ClientLookupId = ClientLookupId.ToUpper();
                sJlist = sJlist.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookupId) && x.ClientLookupId.ToUpper().Contains(ClientLookupId))).ToList();
            }
            if (!string.IsNullOrEmpty(Description))
            {
                Description = Description.ToUpper();
                sJlist = sJlist.Where(x => (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(Description))).ToList();
            }
            if (!string.IsNullOrEmpty(ChargeTo_ClientLookupId))
            {
                ChargeTo_ClientLookupId = ChargeTo_ClientLookupId.ToUpper();
                sJlist = sJlist.Where(x => (!string.IsNullOrWhiteSpace(x.ChargeTo_ClientLookupId) && x.ChargeTo_ClientLookupId.ToUpper().Contains(ChargeTo_ClientLookupId))).ToList();
            }
            if (!string.IsNullOrEmpty(ChargeTo_Name))
            {
                ChargeTo_Name = ChargeTo_Name.ToUpper();
                sJlist = sJlist.Where(x => (!string.IsNullOrWhiteSpace(x.ChargeTo_Name) && x.ChargeTo_Name.ToUpper().Contains(ChargeTo_Name))).ToList();
            }
            if (!string.IsNullOrEmpty(Status))
            {
                sJlist = sJlist.Where(x => (!string.IsNullOrWhiteSpace(x.Status) && x.Status.Equals(Status))).ToList();
            }
            if (!string.IsNullOrEmpty(Shift))
            {
                Shift = Shift.ToUpper();
                sJlist = sJlist.Where(x => (!string.IsNullOrWhiteSpace(x.Shift) && x.Shift.ToUpper().Equals(Shift))).ToList();
            }

            if (!string.IsNullOrEmpty(AssetGroup1_ClientLookUpId))
            {
                AssetGroup1_ClientLookUpId = AssetGroup1_ClientLookUpId.ToUpper();
                sJlist = sJlist.Where(x => (!string.IsNullOrWhiteSpace(x.AssetGroup1_ClientLookUpId) && x.AssetGroup1_ClientLookUpId.ToUpper().Equals(AssetGroup1_ClientLookUpId))).ToList();
            }
            if (!string.IsNullOrEmpty(AssetGroup2_ClientLookUpId))
            {
                AssetGroup2_ClientLookUpId = AssetGroup2_ClientLookUpId.ToUpper();
                sJlist = sJlist.Where(x => (!string.IsNullOrWhiteSpace(x.AssetGroup2_ClientLookUpId) && x.AssetGroup2_ClientLookUpId.ToUpper().Equals(AssetGroup2_ClientLookUpId))).ToList();
            }
            if (!string.IsNullOrEmpty(AssetGroup3_ClientLookUpId))
            {
                AssetGroup3_ClientLookUpId = AssetGroup3_ClientLookUpId.ToUpper();
                sJlist = sJlist.Where(x => (!string.IsNullOrWhiteSpace(x.AssetGroup3_ClientLookUpId) && x.AssetGroup3_ClientLookUpId.ToUpper().Equals(AssetGroup3_ClientLookUpId))).ToList();
            }

            if (CreateDate != null)
            {
                sJlist = sJlist.Where(x => (x.CreateDate != null && x.CreateDate.Value.Date.Equals(CreateDate.Value.Date))).ToList();
            }
            if (!string.IsNullOrEmpty(CreateBy))
            {
                CreateBy = CreateBy.ToUpper();
                sJlist = sJlist.Where(x => (!string.IsNullOrWhiteSpace(x.CreateByName) && x.CreateByName.ToUpper().Equals(CreateBy))).ToList();
            }
            if (!string.IsNullOrEmpty(Assigned))
            {
                Assigned = Assigned.ToUpper();
                sJlist = sJlist.Where(x => (!string.IsNullOrWhiteSpace(x.Assigned) && x.Assigned.ToUpper().Equals(Assigned))).ToList();
            }
            if (CompleteDate != null)
            {
                sJlist = sJlist.Where(x => (x.CompleteDate != null && x.CompleteDate.Value.Date.Equals(CompleteDate.Value.Date))).ToList();
            }
            if (!string.IsNullOrEmpty(VerifiedBy))
            {
                sJlist = sJlist.Where(x => (!string.IsNullOrWhiteSpace(x.VerifiedBy) && x.VerifiedBy.ToUpper().Equals(VerifiedBy.ToUpper()))).ToList();
            }
            if (VerifiedDate != null)
            {
                sJlist = sJlist.Where(x => (x.VerifiedDate != null && x.VerifiedDate.Value.Date.Equals(VerifiedDate.Value.Date))).ToList();
            }
            if (!string.IsNullOrEmpty(Extracted))
            {
                var extracted = Extracted.Equals("0") ? false : true;
                sJlist = sJlist.Where(x => (x.Extracted.Equals(extracted))).ToList();
            }
            if (ScheduledDate != null)
            {
                sJlist = sJlist.Where(x => (x.ScheduledDate != null && x.ScheduledDate.Value.Date.Equals(ScheduledDate.Value.Date))).ToList();
            }
            return sJlist;
        }

        public string GetSantGridData(int? draw, int? start, int? length, int CustomQueryDisplayId = 0, string SearchText = "", string ClientLookupId = "", string Description = "", string ChargeTo_ClientLookupId = "", string ChargeTo_Name = "", string AssetLocation = "",
                                string Status = "", string Shift = "", string AssetGroup1_ClientLookUpId = "", string AssetGroup2_ClientLookUpId = "", string AssetGroup3_ClientLookUpId = "", DateTime? CreateDate = null, string CreateBy = "",
                                string Assigned = "", DateTime? CompleteDate = null, string VerifiedBy = "", DateTime? VerifiedDate = null, string Extracted = "", DateTime? ScheduledDate = null,
                                DateTime? CreateStartDateVw = null, DateTime? CreateEndDateVw = null, DateTime? CompleteStartDateVw = null, DateTime? CompleteEndDateVw = null,
                                DateTime? FailedStartDateVw = null, DateTime? FailedEndDateVw = null, DateTime? PassedStartDateVw = null, DateTime? PassedEndDateVw = null,
                                string Order = "1"//, string orderDir = "asc")
            )
        {
            string _createDate = string.Empty;
            string _completeDate = string.Empty;
            string _verifiedDate = string.Empty;
            string _scheduledDate = string.Empty;
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var extracted = string.IsNullOrEmpty(Extracted) ? false : (Extracted.Equals("0") ? false : true);
            _createDate = CreateDate.HasValue ? CreateDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _completeDate = CompleteDate.HasValue ? CompleteDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _verifiedDate = VerifiedDate.HasValue ? VerifiedDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _scheduledDate = ScheduledDate.HasValue ? ScheduledDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;

            start = start.HasValue
       ? start / length
           : 0;
            int skip = start * length ?? 0;
            SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);

            List<SanitationJobSearchModel> SanitationJobList = sWrapper.GetSanitationChunkList(CustomQueryDisplayId, skip, length ?? 0, Order, orderDir,
                ClientLookupId, Description, ChargeTo_ClientLookupId, ChargeTo_Name, AssetLocation, Status, Shift, AssetGroup1_ClientLookUpId, AssetGroup2_ClientLookUpId, AssetGroup3_ClientLookUpId, _createDate, CreateBy, Assigned,
                _completeDate, VerifiedBy, _verifiedDate, extracted, _scheduledDate, SearchText, CreateStartDateVw, CreateEndDateVw, CompleteStartDateVw, CompleteEndDateVw, FailedStartDateVw, FailedEndDateVw, PassedStartDateVw, PassedEndDateVw);

            var totalRecords = 0;
            var recordsFiltered = 0;

            if (SanitationJobList != null && SanitationJobList.Count > 0)
            {
                recordsFiltered = SanitationJobList[0].TotalCount;
                totalRecords = SanitationJobList[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }
            int initialPage = start.Value;
            var filteredResult = SanitationJobList
              .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);

        }

        private List<SanitationJobSearchModel> GetAllEquipmentsSortByColumnWithOrder(string order, string orderDir, List<SanitationJobSearchModel> data)
        {
            List<SanitationJobSearchModel> lst = new List<SanitationJobSearchModel>();
            switch (order)
            {
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ChargeTo_ClientLookupId).ToList() : data.OrderBy(p => p.ChargeTo_ClientLookupId).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ChargeTo_Name).ToList() : data.OrderBy(p => p.ChargeTo_Name).ToList();
                    break;
                case "5":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Status).ToList() : data.OrderBy(p => p.Status).ToList();
                    break;
                case "6":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Shift).ToList() : data.OrderBy(p => p.Shift).ToList();
                    break;
                case "7":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CreateDate).ToList() : data.OrderBy(p => p.CreateDate).ToList();
                    break;
                case "8":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CreateByName).ToList() : data.OrderBy(p => p.CreateByName).ToList();
                    break;
                case "9":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Assigned).ToList() : data.OrderBy(p => p.Assigned).ToList();
                    break;
                case "10":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CompleteDate).ToList() : data.OrderBy(p => p.CompleteDate).ToList();
                    break;
                case "11":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.VerifiedBy).ToList() : data.OrderBy(p => p.VerifiedBy).ToList();
                    break;
                case "12":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.VerifiedDate).ToList() : data.OrderBy(p => p.VerifiedDate).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
            }
            return lst;
        }

        #endregion

        #region Details
        public PartialViewResult SJobDetails(long SanitationJobId)
        {
            SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
            if (comWrapper == null)
            {
                comWrapper = new CommonWrapper(userData);
            }
            SanitationVM objVM = new SanitationVM()
            {
                JobDetailsModel = sWrapper.RetrieveBy_SanitationJobId(SanitationJobId),
                sanitationJobModel = new SanitationJobModel
                {
                    ImageURI = SanitationImageUrl(SanitationJobId)//comWrapper.GetAzureImageUrl(SanitationJobId, AttachmentTableConstant.Sanitation)                    

                }
            };
            objVM.security = this.userData.Security;
            objVM.userData = this.userData;
            objVM.JobDetailsModel.ExternalSanitation = userData.Site.ExternalSanitation;
            objVM.JobDetailsModel.ClientOnPremise = ClientOnPremiseVal();
            Task attTask;
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            attTask = Task.Factory.StartNew(() => objVM.attachmentCount = objCommonWrapper.AttachmentCount(SanitationJobId, AttachmentTableConstant.Sanitation, userData.Security.SanitationJob.Edit));
            attTask.Wait();
            objVM.sanitationJobModel.SanitationJobId = SanitationJobId;
            LocalizeControls(objVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("~/Views/SanitationJob/_SanitationJobDetails.cshtml", objVM);
        }
        #endregion

        #region Edit
        [HttpGet]
        public PartialViewResult EditSanitationJobDetails(long SanitationJobId)
        {

            SanitationVM objVM = new SanitationVM();
            SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
            SanitationJobDetailsModel details = sWrapper.RetrieveBy_SanitationJobId(SanitationJobId);
            objVM.DemandModel = new AddODemandModel();
            #region Drop
            List<DataContracts.LookupList> Shift = new List<DataContracts.LookupList>();
            var AllLookUpList = sWrapper.GetAllLookUpList();
            if (AllLookUpList != null)
            {
                Shift = AllLookUpList.Where(x => x.ListName == LookupListConstants.Shift).ToList();
            }
            if (Shift != null)
            {
                objVM.ShiftList = Shift.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue.ToString() });
            }
            if (details.ScheduledDate != null && details.ScheduledDate.Value == default(DateTime))
            {
                details.ScheduledDate = null;
            }
            if (details.CompleteDate != null && details.CompleteDate.Value == default(DateTime))
            {
                details.CompleteDate = null;
            }
            if (userData.Security.SanitationJob.Edit && !userData.Site.ExternalSanitation)
            {
                //Changes done as per V2-266
                if (details.Status == SanitationJobConstant.Approved || details.Status == SanitationJobConstant.Scheduled || details.Status == SanitationJobConstant.JobRequest)
                {
                    details.IsCompleteButtonShow = true;
                }
                else
                {
                    details.IsCompleteButtonShow = false;
                }
            }
            #endregion
            details.PlantLocationDescription = details.ChargeToId_string;
            objVM.JobDetailsModel = details;
            SanitationJobModel SJM = new SanitationJobModel();
            SJM.ImageURI = SanitationImageUrl(SanitationJobId);// comWrapper.GetAzureImageUrl(SanitationJobId, AttachmentTableConstant.Sanitation);
            objVM.sanitationJobModel = SJM;
            objVM.JobDetailsModel.ExternalSanitation = userData.Site.ExternalSanitation;
            objVM.JobDetailsModel.PlantLocation = userData.Site.PlantLocation;
            objVM.JobDetailsModel.ClientOnPremise = ClientOnPremiseVal();
            objVM.AssetTree = this.userData.DatabaseKey.Client.AssetTree;
            LocalizeControls(objVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("~/Views/SanitationJob/_SanitationJobAdd.cshtml", objVM);
        }

        [HttpPost]
        public JsonResult CompleteSanitationJobList(SanitationJobCompleteModel sanitationJobCompleteModel)
        {
            List<string> returnMessage = new List<string>();
            SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
            List<BatchCompleteResultModel> WoBatchList = new List<BatchCompleteResultModel>();
            BatchCompleteResultModel objWoBatchList;
            string strF = UtilityFunction.GetMessageFromResource("GlobalFailed", LocalizeResourceSetConstants.Global);
            string strSc = UtilityFunction.GetMessageFromResource("GlobalSuccess", LocalizeResourceSetConstants.Global);
            foreach (var item in sanitationJobCompleteModel.list)
            {
                var result = sWrapper.CompleteSanitationJob(item.SanitationJobId, sanitationJobCompleteModel.comments);
                returnMessage.AddRange(result.ErrorMessages);
                objWoBatchList = new BatchCompleteResultModel();
                objWoBatchList.ClientLookupId = item.ClientLookupId;
                objWoBatchList.ErrMsg = result.ErrorMessages;
                objWoBatchList.Status = (result.ErrorMessages != null && result.ErrorMessages.Count > 0) ? strF : strSc;
                WoBatchList.Add(objWoBatchList);
            }
            return Json(WoBatchList, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region Save
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public JsonResult AddSanitationJobDetails(SanitationVM jobVM, string Command)
        {
            string Mode = string.Empty;
            SanitationJob resultObj = new SanitationJob();
            if (ModelState.IsValid)
            {
                SanitationVM objVM = new SanitationVM();
                SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
                //Int64 plantLocationId = 0;
                //if (jobVM.TplantLocationId != null)
                //{
                //    plantLocationId = Convert.ToInt64(jobVM.TplantLocationId);
                //}
                if (jobVM.TchargeType != null)
                {
                    jobVM.JobDetailsModel.ChargeType = jobVM.TchargeType;
                    // jobVM.JobDetailsModel.PlantLocationId = jobVM.JobDetailsModel.ChargeType == "PlantLocation" ? plantLocationId : 0;
                    jobVM.JobDetailsModel.PlantLocationId = jobVM.TplantLocationId;
                }
                if (jobVM.TplantLocationDescription != null)
                {
                    var index = jobVM.TplantLocationDescription.IndexOf('(');
                    if (index != -1) jobVM.JobDetailsModel.ChargeTo_ClientLookupId = jobVM.TplantLocationDescription.Substring(0, index);
                    else jobVM.JobDetailsModel.ChargeTo_ClientLookupId = jobVM.TplantLocationDescription;
                }
                if (Command == "save")
                {
                    resultObj = sWrapper.SaveEditSanitationJob(jobVM.JobDetailsModel);
                }
                //else if (Command == "complete")
                //{
                //    #region Complete Rules
                //    bool schedDuration = false;
                //    decimal ScheduledDuration = 0;
                //    schedDuration = decimal.TryParse(jobVM.JobDetailsModel.ScheduledDuration.ToString().Trim(), out ScheduledDuration);
                //    bool actDuration = false;
                //    decimal ActualDuration = 0;
                //    actDuration = decimal.TryParse(jobVM.JobDetailsModel.ActualDuration.ToString().Trim(), out ActualDuration);
                //    #endregion                   
                //    resultObj = sWrapper.CompleteSanitationJob(jobVM.JobDetailsModel);
                //    Mode = "complete";
                //}

                if (resultObj.ErrorMessages != null && resultObj.ErrorMessages.Count > 0)
                {
                    return Json(resultObj.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = "success", Command = Command, SanitationJobId = resultObj.SanitationJobId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(new { Result = ModelValidationFailedMessage }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public JsonResult EditSanitationJobDetailsMobile(SanitationVM jobVM, string Command)
        {
            string Mode = string.Empty;
            SanitationJob resultObj = new SanitationJob();
            if (ModelState.IsValid)
            {
                SanitationVM objVM = new SanitationVM();
                SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
                //Int64 plantLocationId = 0;
                //if (jobVM.TplantLocationId != null)
                //{
                //    plantLocationId = Convert.ToInt64(jobVM.TplantLocationId);
                //}
                if (jobVM.TchargeType != null)
                {
                    jobVM.JobDetailsModel.ChargeType = jobVM.TchargeType;
                    // jobVM.JobDetailsModel.PlantLocationId = jobVM.JobDetailsModel.ChargeType == "PlantLocation" ? plantLocationId : 0;
                    jobVM.JobDetailsModel.PlantLocationId = jobVM.TplantLocationId;
                }
                if (jobVM.TplantLocationDescription != null)
                {
                    var index = jobVM.TplantLocationDescription.IndexOf('(');
                    if (index != -1) jobVM.JobDetailsModel.ChargeTo_ClientLookupId = jobVM.TplantLocationDescription.Substring(0, index);
                    else jobVM.JobDetailsModel.ChargeTo_ClientLookupId = jobVM.TplantLocationDescription;
                }
                if (Command == "save")
                {
                    resultObj = sWrapper.SaveEditSanitationJobMobile(jobVM.JobDetailsModel);
                }


                if (resultObj.ErrorMessages != null && resultObj.ErrorMessages.Count > 0)
                {
                    return Json(resultObj.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = "success", Command = Command, SanitationJobId = resultObj.SanitationJobId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(new { Result = ModelValidationFailedMessage }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Cancel

        [HttpPost]
        [ValidateAntiForgeryToken]

        public JsonResult CancelSanitationJob(SanitationVM sanitationVM)
        {
            long SanitationJobId = sanitationVM.sanitationCancelAndPrintListModel.SanitationJobId;
            string CancelReason = sanitationVM.sanitationCancelAndPrintListModel.CancelReason;
            string Comments = sanitationVM.sanitationCancelAndPrintListModel.Comments;
            string result = string.Empty;
            SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
            SanitationJob sanitationjob = sWrapper.CancelJob(SanitationJobId, CancelReason, Comments);
            if (sanitationjob.ErrorMessages != null && sanitationjob.ErrorMessages.Count > 0)
            {
                return Json(sanitationjob.ErrorMessages, JsonRequestBehavior.AllowGet);
            }
            else
            {
                result = "success";
            }
            return Json(new { data = result, SanitationJobId = SanitationJobId }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CancelSanitationList(SanitationCancelAndPrintModel model)
        {
            SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
            System.Text.StringBuilder failedList = new System.Text.StringBuilder();

            foreach (var item in model.list)
            {
                if (item.Status == SanitationJobConstant.Complete || item.Status == SanitationJobConstant.Canceled)
                {
                    failedList.Append(item.ClientLookupId + ",");
                }
                else
                {
                    var result = sWrapper.CancelJob(item.SanitationJobId, model.cancelreason, model.comments);
                }
            }
            if (!string.IsNullOrEmpty(failedList.ToString()))
            {
                return Json(new { data = "WorkOrder(s) " + failedList + " can't be cancelled. Please check the status." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Photos
        public JsonResult DeleteImageFromAzure(string _SanitationJobId, string TableName, bool Profile, bool Image)
        {
            string isSuccess = string.Empty;
            comWrapper.DeleteAzureImage(Convert.ToInt64(_SanitationJobId), AttachmentTableConstant.Sanitation, ref isSuccess);
            return Json(isSuccess.ToLower(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteImageFromOnPremise(string _SanitationJobId, string TableName, bool Profile, bool Image)
        {
            CommonWrapper comWrapper = new CommonWrapper(userData);
            string isSuccess = string.Empty;
            comWrapper.DeleteOnPremiseImage(Convert.ToInt64(_SanitationJobId), AttachmentTableConstant.Sanitation, ref isSuccess);
            return Json(isSuccess.ToLower(), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Print

        [HttpGet]
        public string GetSantPrintData(string colname, string coldir, int CustomQueryDisplayId = 0, string ClientLookupId = "", string Description = "", string ChargeTo_ClientLookupId = "", string ChargeTo_Name = "", string AssetLocation = "",
                               string Status = "", string Shift = "", string AssetGroup1_ClientLookUpId = "", string AssetGroup2_ClientLookUpId = "", string AssetGroup3_ClientLookUpId = "", DateTime? CreateDate = null,
                               string CreateBy = "", string Assigned = "", DateTime? CompleteDate = null, string VerifiedBy = "", DateTime? VerifiedDate = null, string Extracted = "", DateTime? ScheduledDate = null, string txtSearchval = "",
                               DateTime? CreateStartDateVw = null, DateTime? CreateEndDateVw = null, DateTime? CompleteStartDateVw = null, DateTime? CompleteEndDateVw = null,
                                DateTime? FailedStartDateVw = null, DateTime? FailedEndDateVw = null, DateTime? PassedStartDateVw = null, DateTime? PassedEndDateVw = null)
        {
            List<SanitationJobPrintModel> sanitationJobPrintModelList = new List<SanitationJobPrintModel>();
            SanitationJobPrintModel objSanitationJobPrintModel;
            SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
            string _createDate = string.Empty;
            string _completeDate = string.Empty;
            string _verifiedDate = string.Empty;
            string _scheduledDate = string.Empty;
            var extracted = string.IsNullOrEmpty(Extracted) ? false : (Extracted.Equals("0") ? false : true);
            _createDate = CreateDate.HasValue ? CreateDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _completeDate = CompleteDate.HasValue ? CompleteDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _verifiedDate = VerifiedDate.HasValue ? VerifiedDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _scheduledDate = ScheduledDate.HasValue ? ScheduledDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;


            List<SanitationJobSearchModel> SanJobList = sWrapper.GetSanitationChunkList(CustomQueryDisplayId, 0, 100000, colname, coldir, ClientLookupId, Description, ChargeTo_ClientLookupId, ChargeTo_Name, AssetLocation, Status, Shift,
                AssetGroup1_ClientLookUpId, AssetGroup2_ClientLookUpId, AssetGroup3_ClientLookUpId, _createDate, CreateBy, Assigned, _completeDate, VerifiedBy, _verifiedDate, extracted, _scheduledDate, txtSearchval,
                CreateStartDateVw, CreateEndDateVw, CompleteStartDateVw, CompleteEndDateVw, FailedStartDateVw, FailedEndDateVw, PassedStartDateVw, PassedEndDateVw);
            foreach (var sanJob in SanJobList)
            {
                objSanitationJobPrintModel = new SanitationJobPrintModel();
                objSanitationJobPrintModel.ClientLookupId = sanJob.ClientLookupId;
                objSanitationJobPrintModel.Description = sanJob.Description;
                objSanitationJobPrintModel.ChargeTo_ClientLookupId = sanJob.ChargeTo_ClientLookupId;
                objSanitationJobPrintModel.ChargeTo_Name = sanJob.ChargeTo_Name;
                objSanitationJobPrintModel.AssetLocation = sanJob.AssetLocation;
                objSanitationJobPrintModel.Status = sanJob.Status;
                objSanitationJobPrintModel.Shift = sanJob.ShiftDescription;
                objSanitationJobPrintModel.CreateDate = sanJob.CreateDate;
                objSanitationJobPrintModel.CreateBy = sanJob.CreateByName;
                objSanitationJobPrintModel.Assigned = sanJob.Assigned;
                objSanitationJobPrintModel.CompleteDate = sanJob.CompleteDate;
                objSanitationJobPrintModel.VerifiedBy = sanJob.VerifiedBy;
                objSanitationJobPrintModel.VerifiedDate = sanJob.VerifiedDate;
                objSanitationJobPrintModel.Extracted = sanJob.Extracted;
                objSanitationJobPrintModel.ScheduledDate = sanJob.ScheduledDate;
                objSanitationJobPrintModel.AssetGroup1_ClientLookUpId = sanJob.AssetGroup1_ClientLookUpId;
                objSanitationJobPrintModel.AssetGroup2_ClientLookUpId = sanJob.AssetGroup2_ClientLookUpId;
                objSanitationJobPrintModel.AssetGroup3_ClientLookUpId = sanJob.AssetGroup3_ClientLookUpId;
                sanitationJobPrintModelList.Add(objSanitationJobPrintModel);

            }
            return JsonConvert.SerializeObject(new { data = sanitationJobPrintModelList }, JsonSerializerDateSettings);
        }
        [EncryptedActionParameter]
        public ActionResult Print(long SanitationJobId, string ChargeToClientLookupId)
        {
            SanitationVM objSanitationVM = new SanitationVM();
            SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
            string AzureImageURL = string.Empty;
            #region SanitationDetails
            SanitationJobDetailsModel details = new SanitationJobDetailsModel();
            Task task1 = Task.Factory.StartNew(() => objSanitationVM.JobDetailsModel = sWrapper.RetrieveBy_SanitationJobId(SanitationJobId));
            #endregion
            #region Tools
            List<ToolsModel> ToolsList = new List<ToolsModel>();
            Task task2 = Task.Factory.StartNew(() => ToolsList = sWrapper.PopulateTools(SanitationJobId)).ContinueWith(_ => { objSanitationVM.ToolsModelList = ToolsList; });
            #endregion
            #region Chemicals
            List<ChemicalSuppliesModel> ChemicalList = new List<ChemicalSuppliesModel>();
            Task task3 = Task.Factory.StartNew(() => ChemicalList = sWrapper.PopulateChemicalSupplies(SanitationJobId)).ContinueWith(_ => { objSanitationVM.ChemicalSuppliesModelList = ChemicalList; });

            #endregion

            if (!userData.Site.ExternalSanitation)
            {
                #region Tasks
                Task task4;
                List<SanitationJobTaskModel> TaskList = new List<SanitationJobTaskModel>();
                task4 = Task.Factory.StartNew(() => TaskList = sWrapper.SanitationJobTaskPopulate(SanitationJobId, ChargeToClientLookupId)).ContinueWith(_ => { objSanitationVM.SanitationJobTaskModelList = TaskList; });
                #endregion
                #region Labour
                Task task5;
                List<LaborModel> LabourList = new List<LaborModel>();
                task5 = Task.Factory.StartNew(() => LabourList = sWrapper.PopulateLabors(SanitationJobId)).ContinueWith(_ => { objSanitationVM.LaborModelList = LabourList; });
                #endregion
                Task.WaitAll(task1, task2, task3, task4, task5);
            }
            else
            {
                Task.WaitAll(task1, task2, task3);
            }
            AzureImageURL = comWrapper.GetClientLogoUrl();
            objSanitationVM.JobDetailsModel.AzureImageURL = AzureImageURL;
            objSanitationVM.JobDetailsModel.ExternalSanitation = userData.Site.ExternalSanitation;
            string customSwitches = string.Format("--header-html  \"{0}\" " +
                                   "--header-spacing \"1\" " +
                                   "--header-font-size \"10\" ",
                                   Url.Action("Header", "SanitationJob", new { id = userData.LoginAuditing.SessionId, SanitationJobId = SanitationJobId }, Request.Url.Scheme));

            LocalizeControls(objSanitationVM, LocalizeResourceSetConstants.SanitationDetails);
            return new ViewAsPdf("SJDetailPrintTemplate", objSanitationVM)
            {
                PageMargins = new Margins(43, 12, 21, 12),// it’s in millimeters
                CustomSwitches = customSwitches
            };
        }

        [AllowAnonymous]
        [SkipSessionExpiaryActionFilter]
        public ActionResult Header(string id, long SanitationJobId)
        {
            SanitationVM objSanitationVM = new SanitationVM();
            if (CheckLoginSession(id))
            {
                SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
                #region SanitationDetails
                SanitationJobDetailsModel details = new SanitationJobDetailsModel();
                details = sWrapper.RetrieveBy_SanitationJobId(SanitationJobId);
                details.AzureImageURL = comWrapper.GetClientLogoUrl();
                objSanitationVM.JobDetailsModel = details;
                #endregion
            }
            LocalizeControls(objSanitationVM, LocalizeResourceSetConstants.SanitationDetails);
            return View("PrintHeader", objSanitationVM);
        }

        [AllowAnonymous]
        [SkipSessionExpiaryActionFilter]
        public ActionResult Footer(string id)
        {
            SanitationVM objSanitationVM = new SanitationVM();
            if (CheckLoginSession(id))
            {
                LocalizeControls(objSanitationVM, LocalizeResourceSetConstants.SanitationDetails);
            }
            return View("PrintFooter", objSanitationVM);
        }

        [EncryptedActionParameter]
        public Byte[] PrintGetByteStream(long SanitationJobId, string ChargeToClientLookupId)
        {
            SanitationVM objSanitationVM = new SanitationVM();
            SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
            string AzureImageURL = string.Empty;
            #region SanitationDetails
            //SanitationJobDetailsModel details = new SanitationJobDetailsModel();
            // Task task1 = Task.Factory.StartNew(() => details = sWrapper.RetrieveBy_SanitationJobId(SanitationJobId)).ContinueWith(_ => { objSanitationVM.JobDetailsModel = details; });
            Task task1 = Task.Factory.StartNew(() => objSanitationVM.JobDetailsModel = sWrapper.RetrieveBy_SanitationJobId(SanitationJobId));

            #endregion
            #region Tools
            List<ToolsModel> ToolsList = new List<ToolsModel>();
            Task task2 = Task.Factory.StartNew(() => ToolsList = sWrapper.PopulateTools(SanitationJobId)).ContinueWith(_ => { objSanitationVM.ToolsModelList = ToolsList; });
            #endregion
            #region Chemicals
            List<ChemicalSuppliesModel> ChemicalList = new List<ChemicalSuppliesModel>();
            Task task3 = Task.Factory.StartNew(() => ChemicalList = sWrapper.PopulateChemicalSupplies(SanitationJobId)).ContinueWith(_ => { objSanitationVM.ChemicalSuppliesModelList = ChemicalList; });
            #endregion

            if (!userData.Site.ExternalSanitation)
            {
                #region Tasks
                Task task4;
                List<SanitationJobTaskModel> TaskList = new List<SanitationJobTaskModel>();
                task4 = Task.Factory.StartNew(() => TaskList = sWrapper.SanitationJobTaskPopulate(SanitationJobId, ChargeToClientLookupId)).ContinueWith(_ => { objSanitationVM.SanitationJobTaskModelList = TaskList; });
                #endregion
                #region Labour
                Task task5;
                List<LaborModel> LabourList = new List<LaborModel>();
                task5 = Task.Factory.StartNew(() => LabourList = sWrapper.PopulateLabors(SanitationJobId)).ContinueWith(_ => { objSanitationVM.LaborModelList = LabourList; });
                #endregion
                Task.WaitAll(task1, task2, task3, task4, task5);
            }
            else
            {
                Task.WaitAll(task1, task2, task3);
            }
            AzureImageURL = comWrapper.GetClientLogoUrl();
            objSanitationVM.JobDetailsModel.AzureImageURL = AzureImageURL;
            objSanitationVM.JobDetailsModel.ExternalSanitation = userData.Site.ExternalSanitation;
            string customSwitches = string.Format("--header-html  \"{0}\" " +
                                   "--header-spacing \"1\" " +
                                   "--header-font-size \"10\" ",
                                   Url.Action("Header", "SanitationJob", new { id = userData.LoginAuditing.SessionId, SanitationJobId = SanitationJobId }, Request.Url.Scheme));

            LocalizeControls(objSanitationVM, LocalizeResourceSetConstants.SanitationDetails);
            var mailpdft = new ViewAsPdf("SJDetailPrintTemplate", objSanitationVM)
            {
                PageMargins = new Margins(43, 12, 21, 12),// it’s in millimeters
                CustomSwitches = customSwitches
            };
            Byte[] PdfData = mailpdft.BuildPdf(ControllerContext);
            return PdfData;
        }

        public JsonResult PrintSanitationListFromIndex(SanitationCancelAndPrintModel model)
        {
            if (model.list.Count > 0)
            {
                using (var ms = new MemoryStream())
                {
                    using (var doc = new iTextSharp.text.Document())
                    {
                        using (var copy = new iTextSharp.text.pdf.PdfSmartCopy(doc, ms))
                        {
                            doc.Open();
                            foreach (var item in model.list)
                            {
                                var msSinglePDf = new MemoryStream(PrintGetByteStream(item.SanitationJobId, ""));
                                using (var reader = new iTextSharp.text.pdf.PdfReader(msSinglePDf))
                                {
                                    copy.AddDocument(reader);
                                }
                            }
                            doc.Close();
                        }
                    }

                    byte[] pdf = ms.ToArray();
                    string strPdf = System.Convert.ToBase64String(pdf);
                    var returnOjb = new { success = true, pdf = strPdf };
                    var jsonResult = Json(returnOjb, JsonRequestBehavior.AllowGet);
                    jsonResult.MaxJsonLength = int.MaxValue;
                    return jsonResult;
                }

            }
            else
            {
                var returnOjb = new { success = false };
                return Json(returnOjb, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Task

        #region Populate
        [HttpPost]
        public string PopulateTask(int? draw, int? start, int? length, long sanitationJobId, string ChargeToClientLookupId)
        {
            bool ActionSecurity = false;
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
            List<SanitationJobTaskModel> TaskList = sWrapper.SanitationJobTaskPopulate(sanitationJobId, ChargeToClientLookupId);
            if (TaskList != null)
            {
                TaskList = this.GetAllTaskOrderSortByColumnWithOrder(order, orderDir, TaskList);
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = TaskList.Count();
            totalRecords = TaskList.Count();
            int initialPage = start.Value;
            var filteredResult = TaskList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            bool IsForAllSecurity = userData.Security.SanitationJob.Edit;
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, ActionSecurity = ActionSecurity, IsForAllSecurity = IsForAllSecurity }, JsonSerializer12HoursDateAndTimeSettings);
        }
        private List<SanitationJobTaskModel> GetAllTaskOrderSortByColumnWithOrder(string order, string orderDir, List<SanitationJobTaskModel> data)
        {
            List<SanitationJobTaskModel> lst = new List<SanitationJobTaskModel>();
            switch (order)
            {
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TaskId).ToList() : data.OrderBy(p => p.TaskId).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Status).ToList() : data.OrderBy(p => p.Status).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TaskId).ToList() : data.OrderBy(p => p.TaskId).ToList();
                    break;
            }
            return lst;
        }

        [HttpGet]
        public JsonResult PopulateTaskIds(long sanitationJobId, string ChargeToClientLookupId)
        {
            SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
            List<SanitationJobTaskModel> sjTaskList = sWrapper.SanitationJobTaskPopulate(sanitationJobId, ChargeToClientLookupId);
            return Json(sjTaskList, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Add
        public PartialViewResult AddTasks(long sanitationJobId, string ClientLookupId, string ChargeToClientLookupId, string ChargeType)
        {
            SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
            SanitationVM objVM = new SanitationVM()
            {
                sanitationJobTaskModel = new SanitationJobTaskModel
                {
                    SanitationJobId = sanitationJobId,
                    ClientLookupId = ClientLookupId,
                    ChargeToClientLookupId = ChargeToClientLookupId,
                    ChargeType = ChargeType
                },
                //JobDetailsModel = new SanitationJobDetailsModel
                //{ ClientLookupId = ClientLookupId ,
                //  ClientOnPremise = ClientOnPremiseVal()
                //},
                JobDetailsModel = sWrapper.RetrieveBy_SanitationJobId(sanitationJobId), //V2-988
                sanitationJobModel = new SanitationJobModel
                {
                    ImageURI = SanitationImageUrl(sanitationJobId)// comWrapper.GetAzureImageUrl(sanitationJobId, AttachmentTableConstant.Sanitation)
                }
            };
            objVM.JobDetailsModel.ClientOnPremise = ClientOnPremiseVal();
            LocalizeControls(objVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("~/Views/SanitationJob/_AddSjTask.cshtml", objVM);

        }
        #endregion

        #region Edit
        public PartialViewResult EditTasks(long SanitationJobId, long _taskId, string ClientLookupId)
        {
            SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
            var AllLookUpList = sWrapper.GetAllLookUpList();
            SanitationVM objVM = new SanitationVM()
            {
                sanitationJobTaskModel = sWrapper.SanitationJobTaskRetrieveSingleBySanitationJobId(SanitationJobId, _taskId, ClientLookupId),
                //JobDetailsModel = new SanitationJobDetailsModel() { ClientLookupId = ClientLookupId, ClientOnPremise = ClientOnPremiseVal() },
                sanitationJobModel = new SanitationJobModel
                {
                    ImageURI = SanitationImageUrl(SanitationJobId)// comWrapper.GetAzureImageUrl(SanitationJobId, AttachmentTableConstant.Sanitation)
                },
                JobDetailsModel = sWrapper.RetrieveBy_SanitationJobId(SanitationJobId), //V2-988
            };

            var ChargeTypeLookUpList = PopulatelookUpListByType(objVM.sanitationJobTaskModel.ChargeType);
            if (!String.IsNullOrEmpty(objVM.sanitationJobTaskModel.ChargeToClientLookupId) && ChargeTypeLookUpList != null && ChargeTypeLookUpList.Count > 0)
            {
                objVM.sanitationJobTaskModel.ChargeToClientLookupId = ChargeTypeLookUpList.Where(x => x.ChargeToClientLookupId == objVM.sanitationJobTaskModel.ChargeToClientLookupId).Select(x => x.ChargeToClientLookupId + " - " + x.Name).FirstOrDefault();
            }
            if (objVM.sanitationJobTaskModel.Status == SanitationJobConstant.TaskCancel && !String.IsNullOrEmpty(objVM.sanitationJobTaskModel.CancelReason) && AllLookUpList != null && AllLookUpList.Count > 0)
            {
                objVM.sanitationJobTaskModel.CancelReason = AllLookUpList.Where(x => x.ListName == LookupListConstants.SANIT_CAN_REASN && x.ListValue.ToString() == objVM.sanitationJobTaskModel.CancelReason).Select(x => x.ListValue.ToString() + " - " + x.Description).FirstOrDefault();
            }
            else
            {
                objVM.sanitationJobTaskModel.CancelReason = string.Empty;
            }
            objVM.JobDetailsModel.ClientOnPremise = ClientOnPremiseVal(); //V2-988
            LocalizeControls(objVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("~/Views/SanitationJob/_AddSjTask.cshtml", objVM);
        }
        #endregion

        #region Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTasks(SanitationVM sjVM)
        {
            SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
            if (ModelState.IsValid)
            {
                string Mode = string.Empty;
                List<String> errorList = new List<string>();
                if (sjVM.sanitationJobTaskModel.SanitationJobTaskId != 0)
                {
                    Mode = "update";
                    errorList = sWrapper.UpdateSanitJobTask(sjVM);
                }
                else
                {
                    Mode = "add";
                    errorList = sWrapper.CreateSanitJobTask(sjVM);
                }
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = "success", sjid = sjVM.sanitationJobTaskModel.SanitationJobId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(new { Result = ModelValidationFailedMessage }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Delete
        public ActionResult DeleteTasks(long taskNumber)
        {
            SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
            var deleteResult = sWrapper.DeleteSanitJobTask(taskNumber);
            if (deleteResult)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Cancel Reason
        public JsonResult PopulateCancelReasonDropdown()
        {
            SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
            List<SelectListItem> cancelReasonList = new List<SelectListItem>();
            List<DataContracts.LookupList> cancelReasonLookUpList = new List<DataContracts.LookupList>();
            var AllLookUpLists = sWrapper.GetAllLookUpList();
            if (AllLookUpLists != null)
            {
                cancelReasonLookUpList = AllLookUpLists.Where(x => x.ListName == LookupListConstants.SANIT_CAN_REASN).ToList();
            }
            if (cancelReasonLookUpList != null)
            {
                cancelReasonList = cancelReasonLookUpList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue.ToString() }).ToList();
            }
            return Json(new { cancelReasonList = cancelReasonList }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Complete-Task:
        public ActionResult CompleteTask(string taskList, long sanitationJobId)
        {
            SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
            string[] completeArray = null;
            if (taskList != null)
            {
                completeArray = taskList.Split(',');
            }
            string result = string.Empty;
            int successCount = 0;

            foreach (var task in completeArray)
            {
                successCount += sWrapper.CompleteSanitJobTask(sanitationJobId, Convert.ToInt64(task));
            }

            if (successCount > 0)
            {
                result = "success";
            }
            else
            {
                result = "failed";
            }
            return Json(new { data = result, successcount = successCount }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Reopen-Task:
        public ActionResult ReOpenTask(string taskList, long sanitationJobId)
        {
            SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
            string[] reOpenArray = null;
            if (taskList != null)
            {
                reOpenArray = taskList.Split(',');
            }
            //As the tasklist is coming in the following format i.e. TaskId,Status,TaskId,Status .., in the following code we structed the data in Key, Value format i.e. List of TaskId and Status
            var taskListWithStatus = reOpenArray
                .SelectMany((s, i) =>
                {
                    var dictionary = new Dictionary<string, string>();
                    if (i == 0 || i % 2 == 0)
                    {
                        dictionary.Add(s, reOpenArray[i + 1]);
                    }

                    return dictionary.Select(s1 => new
                    {
                        TaskId = s1.Key,
                        Status = s1.Value
                    });
                })
            .ToList();

            string result = string.Empty;
            int successCount = 0;

            foreach (var task in taskListWithStatus)
            {
                successCount += sWrapper.ReOpenSanitJobTask(sanitationJobId, Convert.ToInt64(task.TaskId), task.Status);
            }

            if (successCount > 0)
            {
                result = "success";
            }
            else
            {
                result = "failed";
            }
            return Json(new { data = result, successcount = successCount }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Cancel-Task:
        public ActionResult CancelTask(string taskList, string cancelReason, long sanitationJobId)
        {
            string[] sjcancelArray = null;
            SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
            if (taskList != null)
            {
                sjcancelArray = taskList.Split(',');
            }
            string result = string.Empty;
            int successCount = 0;

            var taskListWithStatus = sjcancelArray
               .SelectMany((s, i) =>
               {
                   var dictionary = new Dictionary<string, string>();
                   if (i == 0 || i % 2 == 0)
                   {
                       dictionary.Add(s, sjcancelArray[i + 1]);
                   }

                   return dictionary.Select(s1 => new
                   {
                       TaskId = s1.Key,
                       Status = s1.Value
                   });
               })
           .ToList();
            foreach (var item in taskListWithStatus)
            {
                successCount = successCount + sWrapper.CancelSanitJobTask(sanitationJobId, Convert.ToInt64(item.TaskId), cancelReason, item.Status);
            }
            if (successCount > 0)
            {
                result = "success";
            }
            else
            {
                result = "failed";
            }
            return Json(new { data = result, successcount = successCount }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion Task

        #region Notes

        #region Populate
        [HttpPost]
        public string PopulateNotes(int? draw, int? start, int? length, long SanitationJobId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            SanitationJobWrapper pWrapper = new SanitationJobWrapper(userData);
            var Notes = pWrapper.PopulateNotes(SanitationJobId);
            Notes = this.GetAllNotesSortByColumnWithOrder(order, orderDir, Notes);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = Notes.Count();
            totalRecords = Notes.Count();
            int initialPage = start.Value;
            var filteredResult = Notes
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeSettings);
        }
        private List<Client.Models.Sanitation.NotesModel> GetAllNotesSortByColumnWithOrder(string order, string orderDir, List<Client.Models.Sanitation.NotesModel> data)
        {
            List<Client.Models.Sanitation.NotesModel> lst = new List<Client.Models.Sanitation.NotesModel>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Subject).ToList() : data.OrderBy(p => p.Subject).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.OwnerName).ToList() : data.OrderBy(p => p.OwnerName).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ModifiedDate).ToList() : data.OrderBy(p => p.ModifiedDate).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Subject).ToList() : data.OrderBy(p => p.Subject).ToList();
                    break;
            }
            return lst;
        }
        #endregion

        #region Add
        [HttpGet]
        public ActionResult AddNotes(long SanitationJobId, string ClientLookupId)
        {
            SanitationJobWrapper pWrapper = new SanitationJobWrapper(userData);
            SanitationVM objVM = new SanitationVM()
            {
                notesModel = new Models.Sanitation.NotesModel
                {
                    SanitationJobId = SanitationJobId,
                    ClientLookupId = ClientLookupId
                },
                sanitationJobModel = new SanitationJobModel
                {
                    ImageURI = SanitationImageUrl(SanitationJobId)//comWrapper.GetAzureImageUrl(SanitationJobId, AttachmentTableConstant.Sanitation)
                },
                JobDetailsModel = pWrapper.RetrieveBy_SanitationJobId(SanitationJobId), //V2-988
            };
            //SanitationJobDetailsModel sanitJobDetailsModel = new SanitationJobDetailsModel();
            //sanitJobDetailsModel.ClientLookupId = ClientLookupId;
            //sanitJobDetailsModel.ClientOnPremise = ClientOnPremiseVal();
            //objVM.JobDetailsModel = sanitJobDetailsModel;
            objVM.JobDetailsModel.ClientOnPremise = ClientOnPremiseVal();
            LocalizeControls(objVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("_AddSanitationNotes", objVM);
        }
        #endregion

        #region Edit
        public ActionResult EditNote(string ClientLookupId, long SanitationJobId, long notesId, string subject, string content, long updatedindex)
        {
            SanitationJobWrapper pWrapper = new SanitationJobWrapper(userData);
            SanitationVM objVM = new SanitationVM()
            {
                notesModel = new Models.Sanitation.NotesModel
                {
                    SanitationJobId = SanitationJobId,
                    ClientLookupId = ClientLookupId,
                    NotesId = notesId,
                    updatedindex = updatedindex,
                    Subject = subject,
                    Content = content
                },
                sanitationJobModel = new SanitationJobModel
                {
                    ImageURI = SanitationImageUrl(SanitationJobId)// comWrapper.GetAzureImageUrl(SanitationJobId, AttachmentTableConstant.Sanitation)
                },
                JobDetailsModel = pWrapper.RetrieveBy_SanitationJobId(SanitationJobId), //V2-988
            };
            //SanitationJobDetailsModel sanitJobDetailsModel = new SanitationJobDetailsModel();
            //sanitJobDetailsModel.ClientLookupId = ClientLookupId;
            //sanitJobDetailsModel.ClientOnPremise = ClientOnPremiseVal();
            //objVM.JobDetailsModel = sanitJobDetailsModel;
            objVM.JobDetailsModel.ClientOnPremise = ClientOnPremiseVal();
            LocalizeControls(objVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("_AddSanitationNotes", objVM);
        }
        #endregion

        #region Save
        [HttpPost]
        public ActionResult AddNotes(SanitationVM sanVM)
        {
            if (ModelState.IsValid)
            {
                SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
                string Mode = string.Empty;
                List<String> errorList = sWrapper.AddOrUpdateNote(sanVM, ref Mode);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), SanitationJobId = sanVM.notesModel.SanitationJobId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Delete
        [HttpPost]
        public ActionResult DeleteNotes(long _notesId)
        {
            SanitationJobWrapper pWrapper = new SanitationJobWrapper(userData);
            var deleteResult = pWrapper.DeleteNote(_notesId);
            if (deleteResult)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #endregion

        #region Attachments

        #region Populate
        [HttpPost]
        public string PopulateAttachments(int? draw, int? start, int? length, long SanitationJobId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            var Attachments = objCommonWrapper.PopulateAttachments(SanitationJobId, "Sanitation", userData.Security.SanitationJob.Edit);
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
        private List<Client.Models.Sanitation.AttachmentModel> GetAllAttachmentsSortByColumnWithOrder(string order, string orderDir, List<Client.Models.Sanitation.AttachmentModel> data)
        {
            List<Client.Models.Sanitation.AttachmentModel> lst = new List<Client.Models.Sanitation.AttachmentModel>();

            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Subject).ToList() : data.OrderBy(p => p.Subject).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.FileName).ToList() : data.OrderBy(p => p.FileName).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.FileSize).ToList() : data.OrderBy(p => p.FileSize).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.OwnerName).ToList() : data.OrderBy(p => p.OwnerName).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CreateDate).ToList() : data.OrderBy(p => p.CreateDate).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Subject).ToList() : data.OrderBy(p => p.Subject).ToList();
                    break;
            }

            return lst;
        }
        #endregion

        #region Add
        [HttpGet]
        public PartialViewResult AddAttachments(long SanitationJobId, string ClientLookupId)
        {
            SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
            SanitationVM objVM = new SanitationVM()
            {
                attachmentModel = new Models.Sanitation.AttachmentModel
                {
                    SanitationJobId = SanitationJobId,
                    ClientLookupId = ClientLookupId
                },
                sanitationJobModel = new SanitationJobModel
                {
                    ImageURI = SanitationImageUrl(SanitationJobId)  //comWrapper.GetAzureImageUrl(SanitationJobId, AttachmentTableConstant.Sanitation)
                },
                JobDetailsModel = sWrapper.RetrieveBy_SanitationJobId(SanitationJobId), //V2-988
            };
            //SanitationJobDetailsModel sanitJobDetailsModel = new SanitationJobDetailsModel();
            //sanitJobDetailsModel.ClientLookupId = ClientLookupId;
            //sanitJobDetailsModel.ClientOnPremise = ClientOnPremiseVal();
            //objVM.JobDetailsModel = sanitJobDetailsModel;
            objVM.JobDetailsModel.ClientOnPremise = ClientOnPremiseVal();
            LocalizeControls(objVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("_AddSanitationAttachment", objVM);

        }
        #endregion

        #region Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAttachments()
        {
            var SanitationJobId = Convert.ToInt64(Request.Form["attachmentModel.SanitationJobId"]);
            if (ModelState.IsValid)
            {
                Stream stream = Request.Files[0].InputStream;
                var fileName = Request.Files[0].FileName;
                Client.Models.AttachmentModel attachmentModel = new Client.Models.AttachmentModel();
                CommonWrapper cWrapper = new CommonWrapper(userData);
                Attachment fileAtt = new Attachment();
                attachmentModel.FileName = System.IO.Path.GetFileNameWithoutExtension(Request.Files[0].FileName);
                string fileExt = System.IO.Path.GetExtension(Request.Files[0].FileName);
                attachmentModel.FileType = fileExt.Substring(1);
                attachmentModel.FileSize = Request.Files[0].ContentLength;
                attachmentModel.ContentType = Request.Files[0].ContentType;
                attachmentModel.ObjectId = Convert.ToInt64(Request.Form["attachmentModel.SanitationJobId"]);
                attachmentModel.Subject = String.IsNullOrEmpty(Request.Form["attachmentModel.Subject"]) ? "No Subject" : Request.Form["attachmentModel.Subject"];
                attachmentModel.TableName = "Sanitation";
                bool attachStatus = false;
                bool OnPremise = userData.DatabaseKey.Client.OnPremise;
                if (OnPremise)
                {
                    fileAtt = cWrapper.AddAttachmentOnPremise(attachmentModel, stream, ref attachStatus, userData.Security.SanitationJob.Edit);
                }
                else
                {
                    fileAtt = cWrapper.AddAttachment(attachmentModel, stream, ref attachStatus, userData.Security.SanitationJob.Edit);
                }
                if (attachStatus)
                {
                    if (fileAtt.ErrorMessages != null && fileAtt.ErrorMessages.Count > 0)
                    {
                        return Json(fileAtt.ErrorMessages, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { data = JsonReturnEnum.success.ToString(), SanitationJobId = Convert.ToInt64(Request.Form["attachmentModel.SanitationJobId"]) }, JsonRequestBehavior.AllowGet);
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
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(new { Result = ModelValidationFailedMessage }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Download
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
        #endregion

        #region Delete
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
        #endregion

        #endregion

        #region Event Log

        #region populate
        [HttpPost]
        public string PopulateEventLog(int? draw, int? start, int? length, long SanitationJobId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            SanitationJobWrapper pWrapper = new SanitationJobWrapper(userData);
            var EventLog = pWrapper.PopulateEventLog(SanitationJobId);
            EventLog = this.GetAllEventLogSortByColumnWithOrder(order, orderDir, EventLog);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = EventLog.Count();
            totalRecords = EventLog.Count();
            int initialPage = start.Value;
            var filteredResult = EventLog
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();


            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeSettings);
        }
        private List<EventLogModel> GetAllEventLogSortByColumnWithOrder(string order, string orderDir, List<EventLogModel> data)
        {
            List<EventLogModel> lst = new List<EventLogModel>();

            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Events).ToList() : data.OrderBy(p => p.Events).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Personnel).ToList() : data.OrderBy(p => p.Personnel).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TransactionDate).ToList() : data.OrderBy(p => p.TransactionDate).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Comments).ToList() : data.OrderBy(p => p.Comments).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Events).ToList() : data.OrderBy(p => p.Events).ToList();
                    break;
            }

            return lst;
        }
        #endregion

        #endregion

        #region Labor

        #region Populate
        [HttpPost]
        public string PopulateLabor(int? draw, int? start, int? length, long SanitationJobId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            SanitationJobWrapper pWrapper = new SanitationJobWrapper(userData);
            var Labors = pWrapper.PopulateLabors(SanitationJobId);
            Labors = this.GetAllLaborsSortByColumnWithOrder(order, orderDir, Labors);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = Labors.Count();
            totalRecords = Labors.Count();
            int initialPage = start.Value;
            var filteredResult = Labors
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
        private List<LaborModel> GetAllLaborsSortByColumnWithOrder(string order, string orderDir, List<LaborModel> data)
        {
            List<LaborModel> lst = new List<LaborModel>();

            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PersonnelClientLookupId).ToList() : data.OrderBy(p => p.PersonnelClientLookupId).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.StartDate).ToList() : data.OrderBy(p => p.StartDate).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Hours).ToList() : data.OrderBy(p => p.Hours).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Value).ToList() : data.OrderBy(p => p.Value).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PersonnelClientLookupId).ToList() : data.OrderBy(p => p.PersonnelClientLookupId).ToList();
                    break;
            }
            return lst;
        }
        #endregion

        #region Add
        [HttpGet]
        public ActionResult AddLabor(long SanitationJobId, string ClientLookupId)
        {
            SanitationJobWrapper pWrapper = new SanitationJobWrapper(userData);
            var PersonnelLookUplist = GetList_Personnel();
            SanitationVM objVM = new SanitationVM()
            {
                laborModel = new LaborModel
                {
                    ChargeToId_Primary = SanitationJobId,
                    ClientLookupId = ClientLookupId,
                    PersonnelIdList = PersonnelLookUplist != null ? PersonnelLookUplist.Select(x => new SelectListItem { Text = x.AssignedTo_PersonnelClientLookupId + " - " + x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() }) : new SelectList(new[] { "" })
                },
                sanitationJobModel = new SanitationJobModel
                {
                    ImageURI = SanitationImageUrl(SanitationJobId) //comWrapper.GetAzureImageUrl(SanitationJobId, AttachmentTableConstant.Sanitation)
                },
                JobDetailsModel = pWrapper.RetrieveBy_SanitationJobId(SanitationJobId), //V2-988

            };
            //SanitationJobDetailsModel sanitJobDetailsModel = new SanitationJobDetailsModel();
            //sanitJobDetailsModel.ClientLookupId = ClientLookupId;
            //sanitJobDetailsModel.ClientOnPremise = ClientOnPremiseVal();
            //objVM.JobDetailsModel = sanitJobDetailsModel;
            objVM.JobDetailsModel.ClientOnPremise = ClientOnPremiseVal();
            LocalizeControls(objVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("_AddSanitationLabor", objVM);
        }
        #endregion

        #region Edit
        [HttpGet]
        public ActionResult EditLabor(string ClientLookupId, long SanitationJobId, long chargeToId_Primary, decimal? hours, DateTime? startDate, long personnelId, int updateIndex, long timecardId)
        {
            SanitationJobWrapper pWrapper = new SanitationJobWrapper(userData);
            var PersonnelLookUplist = GetList_Personnel();
            SanitationVM objVM = new SanitationVM()
            {
                laborModel = new LaborModel
                {
                    ClientLookupId = ClientLookupId,
                    ChargeToId_Primary = chargeToId_Primary,
                    Hours = hours,
                    StartDate = startDate,
                    PersonnelId = personnelId,
                    UpdateIndex = updateIndex,
                    TimecardId = timecardId,
                    PersonnelIdList = PersonnelLookUplist != null ? PersonnelLookUplist.Select(x => new SelectListItem { Text = x.AssignedTo_PersonnelClientLookupId + " - " + x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() }) : new SelectList(new[] { "" })
                },
                sanitationJobModel = new SanitationJobModel
                {
                    ImageURI = SanitationImageUrl(SanitationJobId) // comWrapper.GetAzureImageUrl(SanitationJobId, AttachmentTableConstant.Sanitation)
                },
                JobDetailsModel = pWrapper.RetrieveBy_SanitationJobId(SanitationJobId), //V2-988
            };
            //SanitationJobDetailsModel sanitJobDetailsModel = new SanitationJobDetailsModel();
            //sanitJobDetailsModel.ClientLookupId = ClientLookupId;
            //sanitJobDetailsModel.ClientOnPremise = ClientOnPremiseVal();
            //objVM.JobDetailsModel = sanitJobDetailsModel;
            objVM.JobDetailsModel.ClientOnPremise = ClientOnPremiseVal();
            LocalizeControls(objVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("_AddSanitationLabor", objVM);
        }
        #endregion

        #region Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Addlabor(SanitationVM sanVM)
        {
            if (ModelState.IsValid)
            {
                SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
                string Mode = string.Empty;
                List<String> errorList = sWrapper.AddOrUpdateLabor(sanVM, ref Mode);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString(), SanitationJobId = sanVM.laborModel.ChargeToId_Primary, mode = Mode }, JsonRequestBehavior.AllowGet);
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

        #region Delete
        [HttpPost]
        public ActionResult DeleteLabor(long timecardId)
        {
            SanitationJobWrapper pWrapper = new SanitationJobWrapper(userData);
            var deleteResult = pWrapper.DeleteLabor(timecardId);
            if (deleteResult)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #endregion

        #region Assignment

        #region populate
        [HttpPost]
        public string PopulateAssignment(int? draw, int? start, int? length, long SanitationJobId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            SanitationJobWrapper pWrapper = new SanitationJobWrapper(userData);
            var Assignments = pWrapper.PopulateAssignments(SanitationJobId);
            Assignments = this.GetAllAssignmentsSortByColumnWithOrder(order, orderDir, Assignments);

            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = Assignments.Count();
            totalRecords = Assignments.Count();
            int initialPage = start.Value;
            var filteredResult = Assignments
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            bool IsAddOrDel = userData.Security.SanitationJob.Edit;
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, IsAddOrDel = IsAddOrDel }, JsonSerializerDateSettings);
        }
        private List<AssignmentModel> GetAllAssignmentsSortByColumnWithOrder(string order, string orderDir, List<AssignmentModel> data)
        {
            List<AssignmentModel> lst = new List<AssignmentModel>();

            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Name).ToList() : data.OrderBy(p => p.Name).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ScheduledStartDate).ToList() : data.OrderBy(p => p.ScheduledStartDate).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ScheduledHours).ToList() : data.OrderBy(p => p.ScheduledHours).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
            }
            return lst;
        }
        #endregion

        #region Add
        [HttpGet]
        public ActionResult AddAssignment(long SanitationJobId, string ClientLookupId)
        {
            SanitationJobWrapper pWrapper = new SanitationJobWrapper(userData);
            var PersonnelLookUplist = GetList_Personnel();
            SanitationVM objVM = new SanitationVM()
            {
                assignmentModel = new AssignmentModel
                {
                    SanitationJobId = SanitationJobId,
                    ClientLookupId = ClientLookupId,
                    PersonnelIdList = PersonnelLookUplist != null ? PersonnelLookUplist.Select(x => new SelectListItem { Text = x.AssignedTo_PersonnelClientLookupId + " - " + x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() }) : new SelectList(new[] { "" })
                },
                sanitationJobModel = new SanitationJobModel
                {
                    ImageURI = SanitationImageUrl(SanitationJobId) // comWrapper.GetAzureImageUrl(SanitationJobId, AttachmentTableConstant.Sanitation)
                },
                JobDetailsModel = pWrapper.RetrieveBy_SanitationJobId(SanitationJobId), //V2-988
            };
            //SanitationJobDetailsModel sanitJobDetailsModel = new SanitationJobDetailsModel();
            //sanitJobDetailsModel.ClientLookupId = ClientLookupId;
            //sanitJobDetailsModel.ClientOnPremise = ClientOnPremiseVal();
            //objVM.JobDetailsModel = sanitJobDetailsModel;
            objVM.JobDetailsModel.ClientOnPremise = ClientOnPremiseVal();
            LocalizeControls(objVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("_AddSanitationAssignment", objVM);
        }
        #endregion

        #region Edit
        [HttpGet]
        public ActionResult EditAssignment(string ClientLookupId, long SanitationJobId, decimal? scheduledHours, DateTime? scheduledStartDate, long personnelId, int updateIndex, long sanitationJobScheduleId)
        {
            SanitationJobWrapper pWrapper = new SanitationJobWrapper(userData);
            var PersonnelLookUplist = GetList_Personnel();
            SanitationVM objVM = new SanitationVM()
            {
                assignmentModel = new AssignmentModel
                {
                    ClientLookupId = ClientLookupId,
                    SanitationJobId = SanitationJobId,
                    ScheduledHours = scheduledHours,
                    ScheduledStartDate = scheduledStartDate,
                    PersonnelId = personnelId,
                    UpdateIndex = updateIndex,
                    SanitationJobScheduleId = sanitationJobScheduleId,
                    PersonnelIdList = PersonnelLookUplist != null ? PersonnelLookUplist.Select(x => new SelectListItem { Text = x.AssignedTo_PersonnelClientLookupId + " - " + x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() }) : new SelectList(new[] { "" })
                },
                sanitationJobModel = new SanitationJobModel
                {
                    ImageURI = SanitationImageUrl(SanitationJobId) // comWrapper.GetAzureImageUrl(SanitationJobId, AttachmentTableConstant.Sanitation)
                },
                JobDetailsModel = pWrapper.RetrieveBy_SanitationJobId(SanitationJobId), //V2-988
            };
            //SanitationJobDetailsModel sanitJobDetailsModel = new SanitationJobDetailsModel();
            //sanitJobDetailsModel.ClientLookupId = ClientLookupId;
            //sanitJobDetailsModel.ClientOnPremise = ClientOnPremiseVal();
            //objVM.JobDetailsModel = sanitJobDetailsModel;
            objVM.JobDetailsModel.ClientOnPremise = ClientOnPremiseVal();
            LocalizeControls(objVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("_AddSanitationAssignment", objVM);
        }
        #endregion

        #region Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAssignment(SanitationVM sanVM)
        {

            if (ModelState.IsValid)
            {
                SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
                string Mode = string.Empty;
                List<String> errorList = sWrapper.AddOrUpdateAssignment(sanVM, ref Mode);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString(), SanitationJobId = sanVM.assignmentModel.SanitationJobId, mode = Mode }, JsonRequestBehavior.AllowGet);
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

        #region delete
        [HttpPost]
        public ActionResult DeleteAssignment(long SanitationJobId, long sanitationJobScheduledId)
        {
            SanitationJobWrapper pWrapper = new SanitationJobWrapper(userData);
            var deleteResult = pWrapper.DeleteAssignment(sanitationJobScheduledId, SanitationJobId);
            if (deleteResult)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #endregion

        #region Tools

        #region Populate
        [HttpPost]
        public string PopulateTools(int? draw, int? start, int? length, long SanitationJobId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            SanitationJobWrapper pWrapper = new SanitationJobWrapper(userData);
            var tools = pWrapper.PopulateTools(SanitationJobId);
            tools = this.GetAllToolsSortByColumnWithOrder(order, orderDir, tools);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = tools.Count();
            totalRecords = tools.Count();
            int initialPage = start.Value;
            var filteredResult = tools
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            bool IsAddOrEdit = userData.Security.SanitationJob.Edit;
            bool IsDelelte = userData.Security.SanitationJob.Delete;

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, IsAddOrEdit = IsAddOrEdit, IsDelelte = IsDelelte }, JsonSerializerDateSettings);

        }
        private List<ToolsModel> GetAllToolsSortByColumnWithOrder(string order, string orderDir, List<ToolsModel> data)
        {
            List<ToolsModel> lst = new List<ToolsModel>();

            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CategoryValue).ToList() : data.OrderBy(p => p.CategoryValue).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Instructions).ToList() : data.OrderBy(p => p.Instructions).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Quantity).ToList() : data.OrderBy(p => p.Quantity).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CategoryValue).ToList() : data.OrderBy(p => p.CategoryValue).ToList();
                    break;
            }

            return lst;
        }
        #endregion

        #region Add
        [HttpGet]
        public ActionResult AddTools(long SanitationJobId, string ClientLookupId)
        {
            SanitationJobWrapper pWrapper = new SanitationJobWrapper(userData);
            List<DataContracts.LookupList> ToolLookUplist = comWrapper.GetAllLookUpList().Where(x => x.ListName == SanitationJobConstant.SanitationPlanning_Tool).ToList();
            SanitationVM objVM = new SanitationVM()
            {
                toolModel = new ToolsModel
                {
                    SanitationJobId = SanitationJobId,
                    ClientLookupId = ClientLookupId,
                    CategoryIdList = ToolLookUplist != null ? ToolLookUplist.Select(x => new SelectListItem { Text = x.ListValue.ToString() + " | " + x.Description, Value = x.ListValue.ToString() }) : new SelectList(new[] { "" })
                },
                sanitationJobModel = new SanitationJobModel
                {
                    ImageURI = SanitationImageUrl(SanitationJobId) // comWrapper.GetAzureImageUrl(SanitationJobId, AttachmentTableConstant.Sanitation)
                },
                JobDetailsModel = pWrapper.RetrieveBy_SanitationJobId(SanitationJobId), //V2-988
            };
            //SanitationJobDetailsModel sanitJobDetailsModel = new SanitationJobDetailsModel();
            //sanitJobDetailsModel.ClientLookupId = ClientLookupId;
            //sanitJobDetailsModel.ClientOnPremise = ClientOnPremiseVal();
            //objVM.JobDetailsModel = sanitJobDetailsModel;
            objVM.JobDetailsModel.ClientOnPremise = ClientOnPremiseVal();
            LocalizeControls(objVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("_AddSanitationTool", objVM);
        }
        #endregion

        #region Edit
        [HttpGet]
        public ActionResult EditTools(long SanitationJobId, string ClientLookupId, string categoryValue, string description, string instructions, long sanitationPlanningId, decimal? quantity)
        {
            SanitationJobWrapper pWrapper = new SanitationJobWrapper(userData);
            SanitationVM objVM = new SanitationVM()
            {
                toolModel = new ToolsModel
                {
                    ClientLookupId = ClientLookupId,
                    SanitationJobId = SanitationJobId,
                    CategoryValue = categoryValue,
                    Description = description,
                    Instructions = instructions,
                    Quantity = quantity,
                    SanitationPlanningId = sanitationPlanningId
                },
                sanitationJobModel = new SanitationJobModel
                {
                    ImageURI = SanitationImageUrl(SanitationJobId) // comWrapper.GetAzureImageUrl(SanitationJobId, AttachmentTableConstant.Sanitation)
                },
                JobDetailsModel = pWrapper.RetrieveBy_SanitationJobId(SanitationJobId), //V2-988
            };
            //SanitationJobDetailsModel sanitJobDetailsModel = new SanitationJobDetailsModel();
            //sanitJobDetailsModel.ClientLookupId = ClientLookupId;
            //sanitJobDetailsModel.ClientOnPremise = ClientOnPremiseVal();
            //objVM.JobDetailsModel = sanitJobDetailsModel;
            objVM.JobDetailsModel.ClientOnPremise = ClientOnPremiseVal();
            LocalizeControls(objVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("_AddSanitationTool", objVM);
        }
        #endregion

        #region Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTools(SanitationVM sanVM)
        {
            if (ModelState.IsValid)
            {
                SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
                string Mode = string.Empty;
                List<String> errorList = sWrapper.AddOrUpdateTools(sanVM, ref Mode);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString(), SanitationJobId = sanVM.toolModel.SanitationJobId, mode = Mode }, JsonRequestBehavior.AllowGet);
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

        #region Delete

        [HttpPost]
        public ActionResult DeleteTools(string _SanitationPlanningId)
        {
            SanitationJobWrapper pWrapper = new SanitationJobWrapper(userData);
            var deleteResult = pWrapper.DeleteTools(_SanitationPlanningId);
            if (deleteResult)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #endregion

        #region ChemicalSupplies

        #region Populate
        [HttpPost]
        public string PopulateChemicalSupplies(int? draw, int? start, int? length, long SanitationJobId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            SanitationJobWrapper pWrapper = new SanitationJobWrapper(userData);
            var ChemicalSupplies = pWrapper.PopulateChemicalSupplies(SanitationJobId);
            ChemicalSupplies = this.GetAllChemicalSuppliesSortByColumnWithOrder(order, orderDir, ChemicalSupplies);

            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = ChemicalSupplies.Count();
            totalRecords = ChemicalSupplies.Count();
            int initialPage = start.Value;
            var filteredResult = ChemicalSupplies
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            bool IsAddOrEditOrDel = userData.Security.SanitationJob.Edit;

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, IsAddOrEditOrDel = IsAddOrEditOrDel }, JsonSerializerDateSettings);
        }
        private List<ChemicalSuppliesModel> GetAllChemicalSuppliesSortByColumnWithOrder(string order, string orderDir, List<ChemicalSuppliesModel> data)
        {
            List<ChemicalSuppliesModel> lst = new List<ChemicalSuppliesModel>();

            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CategoryValue).ToList() : data.OrderBy(p => p.CategoryValue).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Instructions).ToList() : data.OrderBy(p => p.Instructions).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Quantity).ToList() : data.OrderBy(p => p.Quantity).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CategoryValue).ToList() : data.OrderBy(p => p.CategoryValue).ToList();
                    break;
            }
            return lst;
        }
        #endregion

        #region Add
        [HttpGet]
        public ActionResult AddChemicalSupplies(long SanitationJobId, string ClientLookupId)
        {
            SanitationJobWrapper pWrapper = new SanitationJobWrapper(userData);
            List<DataContracts.LookupList> ChemicalSuppliesLookUplist = comWrapper.GetAllLookUpList().Where(x => x.ListName == SanitationJobConstant.SanitationChemicalSupplies_Tool).ToList();
            SanitationVM objVM = new SanitationVM()
            {
                chemicalSuppliesModel = new ChemicalSuppliesModel
                {
                    SanitationJobId = SanitationJobId,
                    ClientLookupId = ClientLookupId,
                    CategoryIdList = ChemicalSuppliesLookUplist != null ? ChemicalSuppliesLookUplist.Select(x => new SelectListItem { Text = x.ListValue.ToString() + " | " + x.Description, Value = x.ListValue.ToString() }) : new SelectList(new[] { "" })
                },
                sanitationJobModel = new SanitationJobModel
                {
                    ImageURI = SanitationImageUrl(SanitationJobId) //comWrapper.GetAzureImageUrl(SanitationJobId, AttachmentTableConstant.Sanitation)
                },
                JobDetailsModel = pWrapper.RetrieveBy_SanitationJobId(SanitationJobId), //V2-988
            };
            //SanitationJobDetailsModel sanitJobDetailsModel = new SanitationJobDetailsModel();
            //sanitJobDetailsModel.ClientLookupId = ClientLookupId;
            //sanitJobDetailsModel.ClientOnPremise = ClientOnPremiseVal();
            //objVM.JobDetailsModel = sanitJobDetailsModel;
            objVM.JobDetailsModel.ClientOnPremise = ClientOnPremiseVal();
            LocalizeControls(objVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("_AddSanitationChemicalSupplies", objVM);
        }
        #endregion

        #region Edit
        [HttpGet]
        public ActionResult EditChemicalSupplies(string ClientLookupId, long SanitationJobId, long sanitationPlanningId, string categoryValue, string description, string instructions, string dilution, decimal? quantity)
        {
            SanitationJobWrapper pWrapper = new SanitationJobWrapper(userData);
            SanitationVM objVM = new SanitationVM()
            {
                chemicalSuppliesModel = new ChemicalSuppliesModel
                {
                    ClientLookupId = ClientLookupId,
                    SanitationJobId = SanitationJobId,
                    SanitationPlanningId = sanitationPlanningId,
                    CategoryValue = categoryValue,
                    Description = description,
                    Instructions = instructions,
                    Dilution = dilution,
                    Quantity = quantity
                },
                sanitationJobModel = new SanitationJobModel
                {
                    ImageURI = SanitationImageUrl(SanitationJobId) // comWrapper.GetAzureImageUrl(SanitationJobId, AttachmentTableConstant.Sanitation)
                },
                JobDetailsModel = pWrapper.RetrieveBy_SanitationJobId(SanitationJobId), //V2-988
            };
            //SanitationJobDetailsModel sanitJobDetailsModel = new SanitationJobDetailsModel();
            //sanitJobDetailsModel.ClientLookupId = ClientLookupId;
            //sanitJobDetailsModel.ClientOnPremise = ClientOnPremiseVal();
            //objVM.JobDetailsModel = sanitJobDetailsModel;
            objVM.JobDetailsModel.ClientOnPremise = ClientOnPremiseVal();
            LocalizeControls(objVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("_AddSanitationChemicalSupplies", objVM);
        }
        #endregion

        #region Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddChemicalSupplies(SanitationVM sanVM)
        {
            if (ModelState.IsValid)
            {
                SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
                string Mode = string.Empty;
                List<String> errorList = sWrapper.AddOrUpadateChemical(sanVM, ref Mode);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString(), SanitationJobId = sanVM.chemicalSuppliesModel.SanitationJobId, mode = Mode }, JsonRequestBehavior.AllowGet);
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

        #region Delete
        [HttpPost]
        public ActionResult DeleteChemicalSupplies(long _SanitationPlanningId)
        {
            SanitationJobWrapper pWrapper = new SanitationJobWrapper(userData);
            var deleteResult = pWrapper.DeleteChemicalSupplies(_SanitationPlanningId);
            if (deleteResult)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #endregion

        #region Add Sanitation Request & Demand PopUp
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddSanitationRequestAndDemand(SanitationVM jobVM, string SaveType)
        {
            string ModelValidationFailedMessage = string.Empty;
            bool IsSavedFromDashboard = jobVM.IsJobAddFromDashboard;
            string Mode = "add";

            if (ModelState.IsValid)
            {
                SanitationVM objVM = new SanitationVM();
                SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
                objVM.DemandModel = new AddODemandModel();
                if (jobVM.DemandModel != null)
                {
                    objVM.DemandModel.OnDemandId = jobVM.DemandModel.OnDemandId;
                    objVM.DemandModel.RequiredDate = jobVM.DemandModel.RequiredDate;
                }
                else { objVM.DemandModel.Description = jobVM.ODescribeModel.Description; }
                //V2-609
                //Int64 plantLocationId = 0;
                //if (jobVM.TplantLocationId != null)
                //{
                //    plantLocationId = Convert.ToInt64(jobVM.TplantLocationId);
                //}
                if (jobVM.TchargeType != null)
                {
                    objVM.DemandModel.ChargeType = jobVM.TchargeType;
                    //objVM.DemandModel.PlantLocationId = objVM.DemandModel.ChargeType == "PlantLocation" ? plantLocationId : 0;
                    objVM.DemandModel.PlantLocationId = jobVM.TplantLocationId;
                }
                if (jobVM.TplantLocationDescription != null)
                {
                    var index = jobVM.TplantLocationDescription.IndexOf('(');
                    if (index != -1) objVM.DemandModel.ChargeToClientLookupId = jobVM.TplantLocationDescription.Substring(0, index);
                    else objVM.DemandModel.ChargeToClientLookupId = jobVM.TplantLocationDescription;
                }

                SanitationRequest response = sWrapper.AddSanitationJobRequestandDemand(objVM.DemandModel, SaveType, IsSavedFromDashboard);

                if (response.ErrorMessages != null && response.ErrorMessages.Count > 0)
                {
                    return Json(response.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = "success", Command = "save", SanitationJobId = response.SanitationJobId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
            return Json(new { Result = ModelValidationFailedMessage }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public PartialViewResult AddRequestDemand(string ClientLookupId, long SanitationJobId = 0)
        {
            SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
            var OnDemand = sWrapper.SanOnDemandMaster();
            SanitationVM objVM = new SanitationVM()
            {
                JobDetailsModel = new SanitationJobDetailsModel
                {
                    SanitationJobId = SanitationJobId,
                    ClientLookupId = ClientLookupId
                },
                DemandModel = new AddODemandModel()
                {
                    OnDemandList = OnDemand != null ? OnDemand.Select(x => new SelectListItem { Text = x.Description == "" ? x.ClientLookUpId : x.Description + "  ||  " + x.ClientLookUpId, Value = x.SanOnDemandMasterId.ToString() }) : new SelectList(new[] { "" })
                },
                IsJobAddFromDashboard = SanitationJobId == 0 ? true : false,
                IsJobAddFromIndex = SanitationJobId == 0 ? true : false
            };
            objVM.JobDetailsModel.PlantLocation = userData.Site.PlantLocation;
            //For V2-609
            objVM.AssetTree = this.userData.DatabaseKey.Client.AssetTree;
            LocalizeControls(objVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("~/Views/SanitationJob/_AddORequestDemand.cshtml", objVM);
        }
        [HttpPost]
        public PartialViewResult AddRequestDescribe(string ClientLookupId, long SanitationJobId = 0)
        {
            SanitationVM objVM = new SanitationVM()
            {
                JobDetailsModel = new SanitationJobDetailsModel
                {
                    SanitationJobId = SanitationJobId,
                    ClientLookupId = ClientLookupId
                },
                IsJobAddFromIndex = SanitationJobId == 0 ? true : false,
                IsJobAddFromDashboard = SanitationJobId == 0 ? true : false
            };
            objVM.JobDetailsModel.PlantLocation = userData.Site.PlantLocation;
            //For V2-609
            objVM.AssetTree = this.userData.DatabaseKey.Client.AssetTree;
            LocalizeControls(objVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("~/Views/SanitationJob/_AddORequestDescribe.cshtml", objVM);
        }
        [HttpPost]
        public PartialViewResult AddJobDemand(string ClientLookupId, long SanitationJobId = 0)
        {
            SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
            var OnDemand = sWrapper.SanOnDemandMaster();
            SanitationVM objVM = new SanitationVM()
            {
                JobDetailsModel = new SanitationJobDetailsModel
                {
                    SanitationJobId = SanitationJobId,
                    ClientLookupId = ClientLookupId
                },
                DemandModel = new AddODemandModel()
                {
                    OnDemandList = OnDemand != null ? OnDemand.Select(x => new SelectListItem { Text = x.Description == "" ? x.ClientLookUpId : x.Description + "  ||  " + x.ClientLookUpId, Value = x.SanOnDemandMasterId.ToString() }) : new SelectList(new[] { "" })
                },
                IsJobAddFromDashboard = SanitationJobId == 0 ? true : false,
                IsJobAddFromIndex = SanitationJobId == 0 ? true : false
            };
            objVM.DemandModel.PlantLocation = userData.Site.PlantLocation;
            //For V2-609
            objVM.AssetTree = this.userData.DatabaseKey.Client.AssetTree;
            LocalizeControls(objVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("~/Views/SanitationJob/_AddOJobDemand.cshtml", objVM);
        }
        [HttpPost]
        public PartialViewResult AddJobDescribe(string ClientLookupId, long SanitationJobId = 0)
        {
            SanitationVM objVM = new SanitationVM()
            {
                JobDetailsModel = new SanitationJobDetailsModel
                {
                    SanitationJobId = SanitationJobId,
                    ClientLookupId = ClientLookupId
                },
                IsJobAddFromIndex = SanitationJobId == 0 ? true : false,
                IsJobAddFromDashboard = SanitationJobId == 0 ? true : false
            };
            objVM.JobDetailsModel.PlantLocation = userData.Site.PlantLocation;
            //For V2-609
            objVM.AssetTree = this.userData.DatabaseKey.Client.AssetTree;
            LocalizeControls(objVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("~/Views/SanitationJob/_AddOJobDescribe.cshtml", objVM);
        }
        public ViewResult Add(string addfor, string addtype)
        {
            SanitationVM objVM = new SanitationVM();
            SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);

            if (addtype == "Demand")
                objVM.IsJobOnDemandAdd = true;
            if (addtype == "Describe")
                objVM.IsJobDescribeAdd = true;
            if (addfor == "Request")
                objVM.IsAddForRequest = true;
            else if (addfor == "Job")
                objVM.IsAddForJob = true;

            objVM.IsJobAddFromDashboard = true;
            objVM.DemandModel = new AddODemandModel();
            var OnDemand = sWrapper.SanOnDemandMaster();
            if (OnDemand != null)
            {
                objVM.DemandModel.OnDemandList = OnDemand.Select(x => new SelectListItem { Text = x.Description == "" ? x.ClientLookUpId : x.Description + "  ||  " + x.ClientLookUpId, Value = x.SanOnDemandMasterId.ToString() });
            }
            objVM.DemandModel.PlantLocation = userData.Site.PlantLocation;
            //For v2-609
            objVM.AssetTree = this.userData.DatabaseKey.Client.AssetTree;
            objVM._userdata = this.userData;  // RKL 2023-May-03
            LocalizeControls(objVM, LocalizeResourceSetConstants.SanitationDetails);
            return View("~/Views/SanitationJob/Index.cshtml", objVM);
        }
        #endregion

        #region Add Sanitation WO Request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddSanitationRequest(SanitationVM jobVM, string SaveType)
        {
            string ModelValidationFailedMessage = string.Empty;
            bool IsSavedFromDashboard = jobVM.IsJobAddFromDashboard;
            string Mode = "add";

            if (ModelState.IsValid)
            {
                SanitationVM objVM = new SanitationVM();
                SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
                objVM.DemandModel = new AddODemandModel();
                if (jobVM.DemandModel != null)
                {
                    objVM.DemandModel.OnDemandId = jobVM.DemandModel.OnDemandId;
                    objVM.DemandModel.RequiredDate = jobVM.DemandModel.RequiredDate;
                }
                else { objVM.DemandModel.Description = jobVM.ODescribeModel.Description; }
                //V2-609
                //Int64 plantLocationId = 0;
                //if (jobVM.TplantLocationId != null)
                //{
                //    plantLocationId = Convert.ToInt64(jobVM.TplantLocationId);
                //}
                if (jobVM.TchargeType != null)
                {
                    objVM.DemandModel.ChargeType = jobVM.TchargeType;
                    // objVM.DemandModel.PlantLocationId = objVM.DemandModel.ChargeType == "PlantLocation" ? plantLocationId : 0;
                    objVM.DemandModel.PlantLocationId = jobVM.TplantLocationId;
                }
                if (jobVM.TplantLocationDescription != null)
                {
                    var index = jobVM.TplantLocationDescription.IndexOf('(');
                    if (index != -1) objVM.DemandModel.ChargeToClientLookupId = jobVM.TplantLocationDescription.Substring(0, index);
                    else objVM.DemandModel.ChargeToClientLookupId = jobVM.TplantLocationDescription;
                }
                SanitationRequest response = sWrapper.AddSanitationJobRequestandDemand(objVM.DemandModel, SaveType, IsSavedFromDashboard);

                if (response.ErrorMessages != null && response.ErrorMessages.Count > 0)
                {
                    return Json(response.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = "success", Command = "save", SanitationJobId = response.SanitationJobId, mode = Mode, type = jobVM.IsAddForRequest }, JsonRequestBehavior.AllowGet);
                }
            }
            ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
            return Json(new { Result = ModelValidationFailedMessage }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Commom
        public string SanitationImageUrl(long SanitationJobId)
        {
            string ImageUrl = string.Empty;
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            bool ClientOnPremise = userData.DatabaseKey.Client.OnPremise;
            if (ClientOnPremise)
            {
                ImageUrl = objCommonWrapper.GetOnPremiseImageUrl(SanitationJobId, AttachmentTableConstant.Sanitation);
            }
            else
            {
                ImageUrl = objCommonWrapper.GetAzureImageUrl(SanitationJobId, AttachmentTableConstant.Sanitation);
            }
            return ImageUrl;

        }
        public bool ClientOnPremiseVal()
        {
            bool ClientOnPremise = userData.DatabaseKey.Client.OnPremise;
            return ClientOnPremise;
        }
        #endregion

        #region V2-841 Multiple Images
        public PartialViewResult GetImages(int currentpage, int? start, int? length, long SanitationJobId)
        {

            SanitationVM objSanitationVM = new SanitationVM();
            List<ImageAttachmentModel> imgDatalist = new List<ImageAttachmentModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<Attachment> Attachments = new List<Attachment>();
            objSanitationVM.security = this.userData.Security;
            objSanitationVM._userdata = this.userData;

            start = start.HasValue
            ? start / length
            : 0;
            int skip = start * length ?? 0;
            ViewBag.Start = skip;

            if (userData.DatabaseKey.Client.OnPremise)
            {
                Attachments = commonWrapper.GetOnPremiseMultipleImageUrl(skip, length ?? 0, SanitationJobId, AttachmentTableConstant.Sanitation);
            }
            else
            {
                Attachments = commonWrapper.GetAzureMultipleImageUrl(skip, length ?? 0, SanitationJobId, AttachmentTableConstant.Sanitation);
            }
            foreach (var attachment in Attachments)
            {
                ImageAttachmentModel imgdata = new ImageAttachmentModel();
                imgdata.AttachmentId = attachment.AttachmentId;
                imgdata.ObjectId = attachment.ObjectId;
                imgdata.Profile = attachment.Profile;
                imgdata.FileName = attachment.FileName;
                string SASImageURL = string.Empty;
                if (!string.IsNullOrEmpty(attachment.AttachmentURL))
                {
                    SASImageURL = attachment.AttachmentURL;
                }
                else
                {
                    commonWrapper.GetNoImageUrl();
                }
                imgdata.AttachmentURL = SASImageURL;
                imgdata.ObjectName = attachment.ObjectName;
                imgdata.TotalCount = attachment.TotalCount;
                imgDatalist.Add(imgdata);
            }
            var recordsFiltered = 0;
            recordsFiltered = imgDatalist.Select(o => o.TotalCount).FirstOrDefault();
            ViewBag.TotalRecords = imgDatalist.Select(o => o.TotalCount).FirstOrDefault();
            ViewBag.Length = length;
            ViewBag.CurrentPage = currentpage;
            objSanitationVM.imageAttachmentModels = imgDatalist;
            LocalizeControls(objSanitationVM, LocalizeResourceSetConstants.Global);

            return PartialView("~/Views/SanitationJob/_AllSanitationJobImages.cshtml", objSanitationVM);
        }
        #endregion

        #region V2-910 For MobileChanges
        [HttpPost]
        public PartialViewResult GetSanitationJobMainGridMobile(int currentpage = 0, int? start = 0, int? length = 0, int CustomQueryDisplayId = 0,
           string SearchText = "", string ClientLookupId = "", string Description = "", string ChargeTo_ClientLookupId = "", string ChargeTo_Name = "", string AssetLocation = "",
                                string Status = "", string Shift = "", string AssetGroup1_ClientLookUpId = "", string AssetGroup2_ClientLookUpId = "", string AssetGroup3_ClientLookUpId = "", DateTime? CreateDate = null, string CreateBy = "",
                                string Assigned = "", DateTime? CompleteDate = null, string VerifiedBy = "", DateTime? VerifiedDate = null, string Extracted = "", DateTime? ScheduledDate = null, DateTime? CreateStartDateVw = null, DateTime? CreateEndDateVw = null, DateTime? CompleteStartDateVw = null, DateTime? CompleteEndDateVw = null,
                                DateTime? FailedStartDateVw = null, DateTime? FailedEndDateVw = null, DateTime? PassedStartDateVw = null, DateTime? PassedEndDateVw = null, string Order = "1", string orderDir = "asc")

        {
            SanitationVM objSanitationVM = new SanitationVM();
            SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
            List<string> statusList = new List<string>();
            objSanitationVM._userdata = this.userData;
            start = start.HasValue
                ? start / length
                : 0;
            int skip = start * length ?? 0;
            string _createDate = string.Empty;
            string _completeDate = string.Empty;
            string _verifiedDate = string.Empty;
            string _scheduledDate = string.Empty;
            var extracted = string.IsNullOrEmpty(Extracted) ? false : (Extracted.Equals("0") ? false : true);
            _createDate = CreateDate.HasValue ? CreateDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _completeDate = CompleteDate.HasValue ? CompleteDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _verifiedDate = VerifiedDate.HasValue ? VerifiedDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _scheduledDate = ScheduledDate.HasValue ? ScheduledDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            List<SanitationJobSearchModel> sanitationJobList = sWrapper.GetSanitationChunkList(CustomQueryDisplayId, skip, length ?? 0, Order, orderDir,
               ClientLookupId, Description, ChargeTo_ClientLookupId, ChargeTo_Name, AssetLocation, Status, Shift, AssetGroup1_ClientLookUpId, AssetGroup2_ClientLookUpId, AssetGroup3_ClientLookUpId, _createDate, CreateBy, Assigned,
               _completeDate, VerifiedBy, _verifiedDate, extracted, _scheduledDate, SearchText, CreateStartDateVw, CreateEndDateVw, CompleteStartDateVw, CompleteEndDateVw, FailedStartDateVw, FailedEndDateVw, PassedStartDateVw, PassedEndDateVw);
            var totalRecords = 0;
            var recordsFiltered = 0;
            recordsFiltered = sanitationJobList.Select(o => o.TotalCount).FirstOrDefault();
            totalRecords = sanitationJobList.Select(o => o.TotalCount).FirstOrDefault();
            var filteredResult = sanitationJobList
                .ToList();
            ViewBag.Start = skip;
            ViewBag.TotalRecords = totalRecords;
            ViewBag.Length = length;
            ViewBag.CurrentPage = currentpage;
            Parallel.ForEach(sanitationJobList, item =>
            {
                item.ClientOnPremise = userData.DatabaseKey.Client.OnPremise;
                item.AzureImageURL = SanitationJobImageUrl(item.SanitationJobId);
            });
            objSanitationVM.SanitationJobModelList = sanitationJobList;
            LocalizeControls(objSanitationVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("~/Views/SanitationJob/Mobile/_SanitationJobGridCardView.cshtml", objSanitationVM);
        }
        public string SanitationJobImageUrl(long SanitationJobId)
        {
            string ImageUrl = string.Empty;
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            bool ClientOnPremise = userData.DatabaseKey.Client.OnPremise;
            if (ClientOnPremise)
            {
                ImageUrl = objCommonWrapper.GetOnPremiseImageUrl(SanitationJobId, AttachmentTableConstant.Sanitation);
            }
            else
            {
                ImageUrl = objCommonWrapper.GetAzureImageUrl(SanitationJobId, AttachmentTableConstant.Sanitation);
            }
            return ImageUrl;

        }

        #region AddSanitation Job/Request
        public PartialViewResult AddDemandSJ_Mobile()
        {
            SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
            var OnDemand = sWrapper.SanOnDemandMaster();
            SanitationVM objVM = new SanitationVM()
            {
                DemandModel = new AddODemandModel()
                {
                    OnDemandList = OnDemand != null ? OnDemand.Select(x => new SelectListItem { Text = x.Description == "" ? x.ClientLookUpId : x.Description + "  ||  " + x.ClientLookUpId, Value = x.SanOnDemandMasterId.ToString() }) : new SelectList(new[] { "" })
                }
            };
            objVM.DemandModel.PlantLocation = userData.Site.PlantLocation;
            objVM.AssetTree = this.userData.DatabaseKey.Client.AssetTree;
            LocalizeControls(objVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("~/Views/SanitationJob/Mobile/_AddSanitationJobDemand.cshtml", objVM);
        }

        public PartialViewResult AddDescribeSJ_Mobile()
        {
            SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
            SanitationVM objVM = new SanitationVM();

            objVM.AssetTree = this.userData.DatabaseKey.Client.AssetTree;
            LocalizeControls(objVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("~/Views/SanitationJob/Mobile/_AddSanitationJobDescribe.cshtml", objVM);
        }
        public PartialViewResult AddDemandSR_Mobile()
        {
            SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
            var OnDemand = sWrapper.SanOnDemandMaster();
            SanitationVM objVM = new SanitationVM()
            {
                DemandModel = new AddODemandModel()
                {
                    OnDemandList = OnDemand != null ? OnDemand.Select(x => new SelectListItem { Text = x.Description == "" ? x.ClientLookUpId : x.Description + "  ||  " + x.ClientLookUpId, Value = x.SanOnDemandMasterId.ToString() }) : new SelectList(new[] { "" })
                }
            };
            objVM.DemandModel.PlantLocation = userData.Site.PlantLocation;
            objVM.AssetTree = this.userData.DatabaseKey.Client.AssetTree;
            LocalizeControls(objVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("~/Views/SanitationJob/Mobile/_AddSanitationRequestDemand.cshtml", objVM);
        }
        public PartialViewResult AddDescribeSR_Mobile()
        {
            SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
            var OnDemand = sWrapper.SanOnDemandMaster();
            SanitationVM objVM = new SanitationVM();
            objVM.AssetTree = this.userData.DatabaseKey.Client.AssetTree;
            LocalizeControls(objVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("~/Views/SanitationJob/Mobile/_AddSanitationRequestDescribe.cshtml", objVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddSanitationRequestAndDemand_Mobile(SanitationVM jobVM, string SaveType)
        {
            string ModelValidationFailedMessage = string.Empty;
            bool IsSavedFromDashboard = jobVM.IsJobAddFromDashboard;
            string Mode = "add";

            if (ModelState.IsValid)
            {
                SanitationVM objVM = new SanitationVM();
                SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
                objVM.DemandModel = new AddODemandModel();
                if (jobVM.DemandModel != null)
                {
                    objVM.DemandModel.OnDemandId = jobVM.DemandModel.OnDemandId;
                    objVM.DemandModel.RequiredDate = jobVM.DemandModel.RequiredDate;
                }
                else { objVM.DemandModel.Description = jobVM.ODescribeModel.Description; }

                if (jobVM.TchargeType != null)
                {
                    objVM.DemandModel.ChargeType = jobVM.TchargeType;
                    objVM.DemandModel.PlantLocationId = jobVM.TplantLocationId;
                }
                if (jobVM.TplantLocationDescription != null)
                {
                    var index = jobVM.TplantLocationDescription.IndexOf('(');
                    if (index != -1) objVM.DemandModel.ChargeToClientLookupId = jobVM.TplantLocationDescription.Substring(0, index);
                    else objVM.DemandModel.ChargeToClientLookupId = jobVM.TplantLocationDescription;
                }

                SanitationRequest response = sWrapper.AddSanitationJobRequestandDemand(objVM.DemandModel, SaveType, IsSavedFromDashboard);

                if (response.ErrorMessages != null && response.ErrorMessages.Count > 0)
                {
                    return Json(response.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = "success", Command = "save", SanitationJobId = response.SanitationJobId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
            return Json(new { Result = ModelValidationFailedMessage }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region V2-910 SanitationJob Mobile Details
        #region Details
        public PartialViewResult SJobDetails_Mobile(long SanitationJobId)
        {
            SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
            if (comWrapper == null)
            {
                comWrapper = new CommonWrapper(userData);
            }
            SanitationVM objVM = new SanitationVM()
            {
                JobDetailsModel = sWrapper.RetrieveBy_SanitationJobId(SanitationJobId),
                sanitationJobModel = new SanitationJobModel
                {
                    ImageURI = SanitationImageUrl(SanitationJobId)//comWrapper.GetAzureImageUrl(SanitationJobId, AttachmentTableConstant.Sanitation)                    

                }
            };
            objVM.security = this.userData.Security;
            objVM.userData = this.userData;
            objVM.JobDetailsModel.ExternalSanitation = userData.Site.ExternalSanitation;
            objVM.JobDetailsModel.ClientOnPremise = ClientOnPremiseVal();
            Task attTask;
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            attTask = Task.Factory.StartNew(() => objVM.attachmentCount = objCommonWrapper.AttachmentCount(SanitationJobId, AttachmentTableConstant.Sanitation, userData.Security.SanitationJob.Edit));
            attTask.Wait();
            objVM.sanitationJobModel.SanitationJobId = SanitationJobId;
            LocalizeControls(objVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("~/Views/SanitationJob/Mobile/_SanitationJobDetails.cshtml", objVM);
        }
        #endregion

        #region Edit
        [HttpGet]
        public PartialViewResult EditSanitationJobDetails_Mobile(long SanitationJobId)
        {

            SanitationVM objVM = new SanitationVM();
            SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
            SanitationJobDetailsModel details = sWrapper.RetrieveBy_SanitationJobId(SanitationJobId);
            objVM.DemandModel = new AddODemandModel();
            #region Drop
            List<DataContracts.LookupList> Shift = new List<DataContracts.LookupList>();
            var AllLookUpList = sWrapper.GetAllLookUpList();
            if (AllLookUpList != null)
            {
                Shift = AllLookUpList.Where(x => x.ListName == LookupListConstants.Shift).ToList();
            }
            if (Shift != null)
            {
                objVM.ShiftList = Shift.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue.ToString() });
            }
            if (details.ScheduledDate != null && details.ScheduledDate.Value == default(DateTime))
            {
                details.ScheduledDate = null;
            }
            if (details.CompleteDate != null && details.CompleteDate.Value == default(DateTime))
            {
                details.CompleteDate = null;
            }
            if (userData.Security.SanitationJob.Edit && !userData.Site.ExternalSanitation)
            {
                if (details.Status == SanitationJobConstant.Approved || details.Status == SanitationJobConstant.Scheduled || details.Status == SanitationJobConstant.JobRequest)
                {
                    details.IsCompleteButtonShow = true;
                }
                else
                {
                    details.IsCompleteButtonShow = false;
                }
            }
            #endregion
            details.PlantLocationDescription = details.ChargeToId_string;
            objVM.JobDetailsModel = details;
            objVM.JobDetailsModel.ExternalSanitation = userData.Site.ExternalSanitation;
            objVM.JobDetailsModel.PlantLocation = userData.Site.PlantLocation;
            objVM.AssetTree = this.userData.DatabaseKey.Client.AssetTree;
            LocalizeControls(objVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("~/Views/SanitationJob/Mobile/_SanitationJobEdit.cshtml", objVM);
        }
        //public PartialViewResult EquipmentLookupListView_Mobile()
        //{
        //    return PartialView("~/Views/SanitationJob/Mobile/_EquipmentGridPopUp.cshtml");
        //}
        [HttpPost]
        public JsonResult GetEquipmentLookupListchunksearch_Mobile(int Start, int Length, string Search = "")
        {
            var modelList = new List<EquipmentLookupModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);

            string order = "0";
            string orderDir = "asc";

            modelList = commonWrapper.GetEquipmentLookupListGridDataMobile(order, orderDir, Start, Length, Search, Search);

            return Json(new { recordsTotal = modelList.Count() > 0 ? modelList[0].TotalCount : 0, data = modelList });
        }
        #endregion

        #region Photos
        [HttpPost]
        public PartialViewResult LoadPhotos_Mobile()
        {
            SanitationVM sanitationVM = new SanitationVM();
            sanitationVM._userdata = this.userData;
            sanitationVM.security = this.userData.Security;
            LocalizeControls(sanitationVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("~/Views/SanitationJob/Mobile/_ImageDetails.cshtml", sanitationVM);
        }
        public PartialViewResult GetImages_Mobile(int currentpage, int? start, int? length, long SanitationJobId)
        {

            SanitationVM objSanitationVM = new SanitationVM();
            List<ImageAttachmentModel> imgDatalist = new List<ImageAttachmentModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<Attachment> Attachments = new List<Attachment>();
            objSanitationVM.security = this.userData.Security;
            objSanitationVM._userdata = this.userData;

            start = start.HasValue
            ? start / length
            : 0;
            int skip = start * length ?? 0;
            ViewBag.Start = skip;

            if (userData.DatabaseKey.Client.OnPremise)
            {
                Attachments = commonWrapper.GetOnPremiseMultipleImageUrl(skip, length ?? 0, SanitationJobId, AttachmentTableConstant.Sanitation);
            }
            else
            {
                Attachments = commonWrapper.GetAzureMultipleImageUrl(skip, length ?? 0, SanitationJobId, AttachmentTableConstant.Sanitation);
            }
            foreach (var attachment in Attachments)
            {
                ImageAttachmentModel imgdata = new ImageAttachmentModel();
                imgdata.AttachmentId = attachment.AttachmentId;
                imgdata.ObjectId = attachment.ObjectId;
                imgdata.Profile = attachment.Profile;
                imgdata.FileName = attachment.FileName;
                string SASImageURL = string.Empty;
                if (!string.IsNullOrEmpty(attachment.AttachmentURL))
                {
                    SASImageURL = attachment.AttachmentURL;
                }
                else
                {
                    commonWrapper.GetNoImageUrl();
                }
                imgdata.AttachmentURL = SASImageURL;
                imgdata.ObjectName = attachment.ObjectName;
                imgdata.TotalCount = attachment.TotalCount;
                imgDatalist.Add(imgdata);
            }
            var recordsFiltered = 0;
            recordsFiltered = imgDatalist.Select(o => o.TotalCount).FirstOrDefault();
            ViewBag.TotalRecords = imgDatalist.Select(o => o.TotalCount).FirstOrDefault();
            ViewBag.Length = length;
            ViewBag.CurrentPage = currentpage;
            objSanitationVM.imageAttachmentModels = imgDatalist;
            LocalizeControls(objSanitationVM, LocalizeResourceSetConstants.Global);

            return PartialView("~/Views/SanitationJob/Mobile/_AllImages.cshtml", objSanitationVM);
        }
        #endregion

        #region Tools
        [HttpPost]
        public PartialViewResult LoadSJTools_Mobile()
        {
            SanitationVM sanitationVM = new SanitationVM();
            sanitationVM._userdata = this.userData;
            sanitationVM.security = this.userData.Security;
            LocalizeControls(sanitationVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("~/Views/SanitationJob/Mobile/_SanitationJobToolsListSearch.cshtml", sanitationVM);
        }
        #region Add
        [HttpGet]
        public ActionResult AddTools_Mobile(long SanitationJobId, string ClientLookupId)
        {
            SanitationJobWrapper pWrapper = new SanitationJobWrapper(userData);
            List<DataContracts.LookupList> ToolLookUplist = comWrapper.GetAllLookUpList().Where(x => x.ListName == SanitationJobConstant.SanitationPlanning_Tool).ToList();
            SanitationVM objVM = new SanitationVM()
            {
                toolModel = new ToolsModel
                {
                    SanitationJobId = SanitationJobId,
                    ClientLookupId = ClientLookupId,
                    CategoryIdList = ToolLookUplist != null ? ToolLookUplist.Select(x => new SelectListItem { Text = x.ListValue.ToString() + " | " + x.Description, Value = x.ListValue.ToString() }) : new SelectList(new[] { "" })
                }
            };
            LocalizeControls(objVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("Mobile/_AddSanitationTool", objVM);
        }
        #endregion

        #region Edit
        [HttpGet]
        public ActionResult EditTools_Mobile(long SanitationJobId, string ClientLookupId, string categoryValue, string description, string instructions, long sanitationPlanningId, decimal? quantity)
        {
            SanitationJobWrapper pWrapper = new SanitationJobWrapper(userData);
            SanitationVM objVM = new SanitationVM()
            {
                toolModel = new ToolsModel
                {
                    ClientLookupId = ClientLookupId,
                    SanitationJobId = SanitationJobId,
                    CategoryValue = categoryValue,
                    Description = description,
                    Instructions = instructions,
                    Quantity = quantity,
                    SanitationPlanningId = sanitationPlanningId
                }
            };
            LocalizeControls(objVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("Mobile/_AddSanitationTool", objVM);
        }
        #endregion
        #endregion

        #region Chemical And Supplies
        [HttpPost]
        public PartialViewResult LoadSJChemicalSupplies_Mobile()
        {
            SanitationVM sanitationVM = new SanitationVM();
            sanitationVM._userdata = this.userData;
            sanitationVM.security = this.userData.Security;
            LocalizeControls(sanitationVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("~/Views/SanitationJob/Mobile/_SanitationJobChemicalSuppliesListSearch.cshtml", sanitationVM);
        }
        #region Add
        [HttpGet]
        public ActionResult AddChemicalSupplies_Mobile(long SanitationJobId, string ClientLookupId)
        {
            SanitationJobWrapper pWrapper = new SanitationJobWrapper(userData);
            List<DataContracts.LookupList> ChemicalSuppliesLookUplist = comWrapper.GetAllLookUpList().Where(x => x.ListName == SanitationJobConstant.SanitationChemicalSupplies_Tool).ToList();
            SanitationVM objVM = new SanitationVM()
            {
                chemicalSuppliesModel = new ChemicalSuppliesModel
                {
                    SanitationJobId = SanitationJobId,
                    ClientLookupId = ClientLookupId,
                    CategoryIdList = ChemicalSuppliesLookUplist != null ? ChemicalSuppliesLookUplist.Select(x => new SelectListItem { Text = x.ListValue.ToString() + " | " + x.Description, Value = x.ListValue.ToString() }) : new SelectList(new[] { "" })
                }
            };
            LocalizeControls(objVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("Mobile/_AddSanitationChemicalSupplies", objVM);
        }
        #endregion

        #region Edit
        [HttpGet]
        public ActionResult EditChemicalSupplies_Mobile(string ClientLookupId, long SanitationJobId, long sanitationPlanningId, string categoryValue, string description, string instructions, string dilution, decimal? quantity)
        {
            SanitationJobWrapper pWrapper = new SanitationJobWrapper(userData);
            SanitationVM objVM = new SanitationVM()
            {
                chemicalSuppliesModel = new ChemicalSuppliesModel
                {
                    ClientLookupId = ClientLookupId,
                    SanitationJobId = SanitationJobId,
                    SanitationPlanningId = sanitationPlanningId,
                    CategoryValue = categoryValue,
                    Description = description,
                    Instructions = instructions,
                    Dilution = dilution,
                    Quantity = quantity
                }
            };
            LocalizeControls(objVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("Mobile/_AddSanitationChemicalSupplies", objVM);
        }
        #endregion
        #endregion

        #region Assignment
        [HttpPost]
        public PartialViewResult LoadSJAssignment_Mobile()
        {
            SanitationVM sanitationVM = new SanitationVM();
            sanitationVM._userdata = this.userData;
            sanitationVM.security = this.userData.Security;
            LocalizeControls(sanitationVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("~/Views/SanitationJob/Mobile/_SanitationJobAssignmentListSearch.cshtml", sanitationVM);
        }

        #region Add
        [HttpGet]
        public ActionResult AddAssignment_Mobile(long SanitationJobId, string ClientLookupId)
        {
            SanitationJobWrapper pWrapper = new SanitationJobWrapper(userData);
            var PersonnelLookUplist = GetList_Personnel();
            SanitationVM objVM = new SanitationVM()
            {
                assignmentModel = new AssignmentModel
                {
                    SanitationJobId = SanitationJobId,
                    ClientLookupId = ClientLookupId,
                    PersonnelIdList = PersonnelLookUplist != null ? PersonnelLookUplist.Select(x => new SelectListItem { Text = x.AssignedTo_PersonnelClientLookupId + " - " + x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() }) : new SelectList(new[] { "" })
                }
            };
            LocalizeControls(objVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("Mobile/_AddSanitationAssignment", objVM);
        }
        #endregion

        #region Edit
        [HttpGet]
        public ActionResult EditAssignment_Mobile(string ClientLookupId, long SanitationJobId, decimal? scheduledHours, DateTime? scheduledStartDate, long personnelId, int updateIndex, long sanitationJobScheduleId)
        {
            SanitationJobWrapper pWrapper = new SanitationJobWrapper(userData);
            var PersonnelLookUplist = GetList_Personnel();
            SanitationVM objVM = new SanitationVM()
            {
                assignmentModel = new AssignmentModel
                {
                    ClientLookupId = ClientLookupId,
                    SanitationJobId = SanitationJobId,
                    ScheduledHours = scheduledHours,
                    ScheduledStartDate = scheduledStartDate,
                    PersonnelId = personnelId,
                    UpdateIndex = updateIndex,
                    SanitationJobScheduleId = sanitationJobScheduleId,
                    PersonnelIdList = PersonnelLookUplist != null ? PersonnelLookUplist.Select(x => new SelectListItem { Text = x.AssignedTo_PersonnelClientLookupId + " - " + x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() }) : new SelectList(new[] { "" })
                }
            };
            LocalizeControls(objVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("Mobile/_AddSanitationAssignment", objVM);
        }
        #endregion
        #endregion

        #region  V2-983 Tasks 
        [HttpPost]
        public PartialViewResult LoadSJTask_Mobile()
        {
            SanitationVM sanitationVM = new SanitationVM();
            sanitationVM._userdata = this.userData;
            sanitationVM.security = this.userData.Security;
            LocalizeControls(sanitationVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("~/Views/SanitationJob/Mobile/_SanitationJobTasksListSearch.cshtml", sanitationVM);
        }
        public JsonResult PopulateCancelReasonDropdownMbl()
        {
            SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
            List<SelectListItem> cancelReasonList = new List<SelectListItem>();
            List<DataContracts.LookupList> cancelReasonLookUpList = new List<DataContracts.LookupList>();
            var AllLookUpLists = sWrapper.GetAllLookUpList();
            if (AllLookUpLists != null)
            {
                cancelReasonLookUpList = AllLookUpLists.Where(x => x.ListName == LookupListConstants.SANIT_CAN_REASN).ToList();
            }
            if (cancelReasonLookUpList != null)
            {
                cancelReasonList = cancelReasonLookUpList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue.ToString() }).ToList();
            }
            return Json(new { cancelReasonList = cancelReasonList }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult AddTasksMbl(long sanitationJobId, string ChargeToClientLookupId, string ChargeType)
        {
            SanitationVM objVM = new SanitationVM()
            {
                sanitationJobTaskModel = new SanitationJobTaskModel
                {
                    SanitationJobId = sanitationJobId,
                    ChargeToClientLookupId = ChargeToClientLookupId,
                    ChargeType = ChargeType
                },
            };
            LocalizeControls(objVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("~/Views/SanitationJob/Mobile/_AddEditSanitationJobTask.cshtml", objVM);

        }
        public PartialViewResult EditTasksMbl(long SanitationJobId, long _taskId, string ClientLookupId)
        {
            SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
            var AllLookUpList = sWrapper.GetAllLookUpList();
            SanitationVM objVM = new SanitationVM()
            {
                sanitationJobTaskModel = sWrapper.SanitationJobTaskRetrieveSingleBySanitationJobId(SanitationJobId, _taskId, ClientLookupId),
            };

            var ChargeTypeLookUpList = PopulatelookUpListByType(objVM.sanitationJobTaskModel.ChargeType);
            if (!String.IsNullOrEmpty(objVM.sanitationJobTaskModel.ChargeToClientLookupId) && ChargeTypeLookUpList != null && ChargeTypeLookUpList.Count > 0)
            {
                objVM.sanitationJobTaskModel.ChargeToClientLookupId = ChargeTypeLookUpList.Where(x => x.ChargeToClientLookupId == objVM.sanitationJobTaskModel.ChargeToClientLookupId).Select(x => x.ChargeToClientLookupId + " - " + x.Name).FirstOrDefault();
            }
            if (objVM.sanitationJobTaskModel.Status == SanitationJobConstant.TaskCancel && !String.IsNullOrEmpty(objVM.sanitationJobTaskModel.CancelReason) && AllLookUpList != null && AllLookUpList.Count > 0)
            {
                objVM.sanitationJobTaskModel.CancelReason = AllLookUpList.Where(x => x.ListName == LookupListConstants.SANIT_CAN_REASN && x.ListValue.ToString() == objVM.sanitationJobTaskModel.CancelReason).Select(x => x.ListValue.ToString() + " - " + x.Description).FirstOrDefault();
            }
            else
            {
                objVM.sanitationJobTaskModel.CancelReason = string.Empty;
            }

            LocalizeControls(objVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("~/Views/SanitationJob/Mobile/_AddEditSanitationJobTask.cshtml", objVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTasksMbl(SanitationVM sjVM)
        {
            SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
            if (ModelState.IsValid)
            {
                string Mode = string.Empty;
                List<String> errorList = new List<string>();
                if (sjVM.sanitationJobTaskModel.SanitationJobTaskId != 0)
                {
                    Mode = "update";
                    errorList = sWrapper.UpdateSanitJobTask(sjVM);
                }
                else
                {
                    Mode = "add";
                    errorList = sWrapper.CreateSanitJobTask(sjVM);
                }
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = "success", sjid = sjVM.sanitationJobTaskModel.SanitationJobId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(new { Result = ModelValidationFailedMessage }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #endregion
        #region V2-912
        public JsonResult SanitationJobApprove(long SanitationJobId)
        {

            if (SanitationJobId > 0)
            {
                SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
                SanitationJob sanitationjob = sWrapper.SanitationJobApprove(SanitationJobId);
                if (sanitationjob.ErrorMessages != null && sanitationjob.ErrorMessages.Count > 0)
                {
                    return Json(new { Result = JsonReturnEnum.failed.ToString(), SanitationJobId = SanitationJobId }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), SanitationJobId = SanitationJobId }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { Result = JsonReturnEnum.failed.ToString(), SanitationJobId = SanitationJobId }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult SanitationJobComplete(long SanitationJobId)
        {

            if (SanitationJobId > 0)
            {
                SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
                SanitationJob sanitationjob = sWrapper.SanitationJobComplete(SanitationJobId);
                if (sanitationjob.ErrorMessages != null && sanitationjob.ErrorMessages.Count > 0)
                {
                    return Json(new { Result = JsonReturnEnum.failed.ToString(), SanitationJobId = SanitationJobId }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), SanitationJobId = SanitationJobId }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { Result = JsonReturnEnum.failed.ToString(), SanitationJobId = SanitationJobId }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult SanitationJobPass(long SanitationJobId)
        {
            if (SanitationJobId > 0)
            {
                SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
                SanitationJob sanitationjob = sWrapper.SanitationJobPass(SanitationJobId);
                if (sanitationjob.ErrorMessages != null && sanitationjob.ErrorMessages.Count > 0)
                {
                    return Json(new { Result = JsonReturnEnum.failed.ToString(), SanitationJobId = SanitationJobId }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), SanitationJobId = SanitationJobId }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { Result = JsonReturnEnum.failed.ToString(), SanitationJobId = SanitationJobId }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult GetFailVarificationSanitationJob(long SanitationJobId)
        {
            SanitationVM objVM = new SanitationVM();
            SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
            List<DataContracts.LookupList> FaillookupLists = new List<DataContracts.LookupList>();
            FailVarificationModel failVarificationModel = new FailVarificationModel();
            var AllLookUpList = sWrapper.GetAllLookUpList();
            if (AllLookUpList != null)
            {
                FaillookupLists = AllLookUpList.Where(x => x.ListName == LookupListConstants.SANIT_FAIL_RESN).ToList();
            }
            if (FaillookupLists != null)
            {
                failVarificationModel.FailReasonList = FaillookupLists.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue.ToString() });
            }
            failVarificationModel.SanitationJobId = SanitationJobId;
            failVarificationModel.Comments = string.Empty;
            objVM.failVarificationModel = failVarificationModel;

            LocalizeControls(objVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("~/Views/SanitationJob/_FailVarificationModal.cshtml", objVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public JsonResult FailVarificationSanitationJob(SanitationVM sanitationVM)
        {

            string result = string.Empty;
            if (sanitationVM.failVarificationModel.SanitationJobId > 0)
            {
                SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
                sanitationVM.failVarificationModel.Comments = sanitationVM.failVarificationModel.Comments == null ? string.Empty : sanitationVM.failVarificationModel.Comments;
                SanitationJob sanitationjob = sWrapper.SanitationJobFailVarification(sanitationVM.failVarificationModel);
                if (sanitationjob.ErrorMessages != null && sanitationjob.ErrorMessages.Count > 0)
                {
                    return Json(sanitationjob.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result = "success";
                }
            }
            return Json(new { data = result, SanitationJobId = sanitationVM.failVarificationModel.SanitationJobId }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region V2-912 Mobile
        public JsonResult SanitationJobApprove_Mobile(long SanitationJobId)
        {

            if (SanitationJobId > 0)
            {
                SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
                SanitationJob sanitationjob = sWrapper.SanitationJobApprove(SanitationJobId);
                if (sanitationjob.ErrorMessages != null && sanitationjob.ErrorMessages.Count > 0)
                {
                    return Json(new { Result = JsonReturnEnum.failed.ToString(), SanitationJobId = SanitationJobId }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), SanitationJobId = SanitationJobId }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { Result = JsonReturnEnum.failed.ToString(), SanitationJobId = SanitationJobId }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult SanitationJobComplete_Mobile(long SanitationJobId)
        {

            if (SanitationJobId > 0)
            {
                SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
                SanitationJob sanitationjob = sWrapper.SanitationJobComplete(SanitationJobId);
                if (sanitationjob.ErrorMessages != null && sanitationjob.ErrorMessages.Count > 0)
                {
                    return Json(new { Result = JsonReturnEnum.failed.ToString(), SanitationJobId = SanitationJobId }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), SanitationJobId = SanitationJobId }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { Result = JsonReturnEnum.failed.ToString(), SanitationJobId = SanitationJobId }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult SanitationJobPass_Mobile(long SanitationJobId)
        {
            if (SanitationJobId > 0)
            {
                SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
                SanitationJob sanitationjob = sWrapper.SanitationJobPass(SanitationJobId);
                if (sanitationjob.ErrorMessages != null && sanitationjob.ErrorMessages.Count > 0)
                {
                    return Json(new { Result = JsonReturnEnum.failed.ToString(), SanitationJobId = SanitationJobId }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), SanitationJobId = SanitationJobId }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { Result = JsonReturnEnum.failed.ToString(), SanitationJobId = SanitationJobId }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult GetFailVarificationSanitationJob_Mobile(long SanitationJobId)
        {
            SanitationVM objVM = new SanitationVM();
            SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
            List<DataContracts.LookupList> FaillookupLists = new List<DataContracts.LookupList>();
            FailVarificationModel failVarificationModel = new FailVarificationModel();
            var AllLookUpList = sWrapper.GetAllLookUpList();
            if (AllLookUpList != null)
            {
                FaillookupLists = AllLookUpList.Where(x => x.ListName == LookupListConstants.SANIT_FAIL_RESN).ToList();
            }
            if (FaillookupLists != null)
            {
                failVarificationModel.FailReasonList = FaillookupLists.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue.ToString() });
            }
            failVarificationModel.SanitationJobId = SanitationJobId;
            objVM.failVarificationModel = failVarificationModel;

            LocalizeControls(objVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("~/Views/SanitationJob/Mobile/_FailVarificationModal.cshtml", objVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public JsonResult FailVarificationSanitationJob_Mobile(SanitationVM sanitationVM)
        {

            string result = string.Empty;
            if (sanitationVM.failVarificationModel.SanitationJobId > 0)
            {
                SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
                sanitationVM.failVarificationModel.Comments = sanitationVM.failVarificationModel.Comments == null ? string.Empty : sanitationVM.failVarificationModel.Comments;
                SanitationJob sanitationjob = sWrapper.SanitationJobFailVarification(sanitationVM.failVarificationModel);
                if (sanitationjob.ErrorMessages != null && sanitationjob.ErrorMessages.Count > 0)
                {
                    return Json(sanitationjob.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result = "success";
                }
            }
            return Json(new { data = result, SanitationJobId = sanitationVM.failVarificationModel.SanitationJobId }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region V2-1055 Add Work Request to Sanitation
        public PartialViewResult AddWoRequestDynamicInSanitationJobDeatails(string SJChargeTo_ClientLookupId, long SJChargeToId = 0, long SJId = 0)
        {
            SanitationVM objSanitationVM = new SanitationVM();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            AllLookUps = commonWrapper.GetAllLookUpList();
            objSanitationVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
                                                .Retrieve(DataDictionaryViewNameConstant.AddWorkRequest, userData);
            IList<string> LookupNames = objSanitationVM.UIConfigurationDetails.ToList()
                                     .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
                                     .Select(s => s.LookupName)
                                     .ToList();
            objSanitationVM.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                  .GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault())
                                                  .Select(s => new WOAddUILookupList
                                                  { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                  .ToList();

            objSanitationVM._userdata = this.userData;     // RKL - Missing - Caused error when hitting the add work request button
            objSanitationVM.IsAddWoRequestDynamic = true;
            objSanitationVM.AddWorkRequest = new Models.Work_Order.UIConfiguration.AddWorkRequestModelDynamic();
            objSanitationVM.AddWorkRequest.ChargeToId = SJChargeToId;
            objSanitationVM.AddWorkRequest.ChargeToClientLookupId = SJChargeTo_ClientLookupId;
            objSanitationVM._userdata = this.userData;
            objSanitationVM.BusinessType = this.userData.DatabaseKey.Client.BusinessType.ToUpper();

            objSanitationVM.DateRangeDropList = UtilityFunction.GetTimeRangeDrop().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }); //V2-347
            objSanitationVM.DateRangeDropListForWO = UtilityFunction.GetTimeRangeDropForWO().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }); //V2-347
            objSanitationVM.DateRangeDropListForWOCreatedate = UtilityFunction.GetTimeRangeDropForWOCreateDate().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }); //V2-364

            objSanitationVM.SanitationJobId = SJId;//V2-1055 
            LocalizeControls(objSanitationVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/SanitationJob/_AddWorkRequestDynamicPopUp.cshtml", objSanitationVM);
        }

        public PartialViewResult AddWoRequestDynamicInSanitationJobDeatails_Mobile(string SJChargeTo_ClientLookupId, long SJChargeToId = 0, long SJId = 0)
        {
            SanitationVM objSanitationVM = new SanitationVM();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            AllLookUps = commonWrapper.GetAllLookUpList();
            objSanitationVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
                                                .Retrieve(DataDictionaryViewNameConstant.AddWorkRequest, userData);
            IList<string> LookupNames = objSanitationVM.UIConfigurationDetails.ToList()
                                     .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
                                     .Select(s => s.LookupName)
                                     .ToList();
            objSanitationVM.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                  .GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault())
                                                  .Select(s => new WOAddUILookupList
                                                  { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                  .ToList();

            objSanitationVM._userdata = this.userData;     // RKL - Missing - Caused error when hitting the add work request button
            objSanitationVM.IsAddWoRequestDynamic = true;
            objSanitationVM.AddWorkRequest = new Models.Work_Order.UIConfiguration.AddWorkRequestModelDynamic();
            objSanitationVM.AddWorkRequest.ChargeToId = SJChargeToId;
            objSanitationVM.AddWorkRequest.ChargeToClientLookupId = SJChargeTo_ClientLookupId;
            objSanitationVM._userdata = this.userData;
            objSanitationVM.BusinessType = this.userData.DatabaseKey.Client.BusinessType.ToUpper();
            objSanitationVM.DateRangeDropList = UtilityFunction.GetTimeRangeDrop().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }); //V2-347
            objSanitationVM.DateRangeDropListForWO = UtilityFunction.GetTimeRangeDropForWO().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }); //V2-347
            objSanitationVM.DateRangeDropListForWOCreatedate = UtilityFunction.GetTimeRangeDropForWOCreateDate().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }); //V2-364
            objSanitationVM.SanitationJobId = SJId;//V2-1055 
            LocalizeControls(objSanitationVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/SanitationJob/Mobile/_AddWorkRequestDynamicPopUp.cshtml", objSanitationVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveWorkRequestDynamic(SanitationVM sanitationVM, string Command)
        {
            if (ModelState.IsValid)
            {
                List<string> ErrorMsg = new List<string>();
                SanitationVM objVM = new SanitationVM();
                SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
                sanitationVM.IsDepartmentShow = true;
                sanitationVM.IsTypeShow = true;
                sanitationVM.IsDescriptionShow = true;
                sanitationVM.ChargeType = ChargeType.Equipment;

                var returnObj = sWrapper.AddWorkRequestDynamic(sanitationVM, ref ErrorMsg);
                if (ErrorMsg != null && ErrorMsg.Count > 0)
                {
                    return Json(ErrorMsg, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString(), Command = Command, SanitationJobId = sanitationVM.SanitationJobId }, JsonRequestBehavior.AllowGet);
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
        public JsonResult SaveWorkRequestDynamic_Mobile(SanitationVM sanitationVM, string Command)
        {
            if (ModelState.IsValid)
            {
                List<string> ErrorMsg = new List<string>();
                SanitationVM objVM = new SanitationVM();
                SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
                sanitationVM.IsDepartmentShow = true;
                sanitationVM.IsTypeShow = true;
                sanitationVM.IsDescriptionShow = true;
                sanitationVM.ChargeType = ChargeType.Equipment;

                var returnObj = sWrapper.AddWorkRequestDynamic(sanitationVM, ref ErrorMsg);
                if (ErrorMsg != null && ErrorMsg.Count > 0)
                {
                    return Json(ErrorMsg, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString(), Command = Command, workOrderID = returnObj.WorkOrderId }, JsonRequestBehavior.AllowGet);
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

        #region V2-1071 DevExpressPrint
        public JsonResult SetPrintSanitationJobListFromIndex(SjPrintingModel model)
        {
            var SanitationJobIds = model.list.ConvertAll(x => x.SanitationJobId);
            Session["PrintSJList"] = SanitationJobIds;
            return Json(new { success = true });
        }
        [EncryptedActionParameter]
        public ActionResult GenerateSanitationJobPrint()
        {
            List<long> SanitationJobIds = new List<long>();
            if (Session["PrintSJList"] != null)
            {
                SanitationJobIds = (List<long>)Session["PrintSJList"];
            }
            SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
            var objPrintModelList = sWrapper.PrintDevExpressFromIndex(SanitationJobIds);
            return View("SJDevExpressPrint", objPrintModelList);
        }

        #endregion

        #region V2-1118 Labor And Notes for Mobile
        #region Labor
        [HttpPost]
        public PartialViewResult LoadSJLabor_Mobile()
        {
            SanitationVM sanitationVM = new SanitationVM();
            sanitationVM._userdata = this.userData;
            sanitationVM.security = this.userData.Security;
            LocalizeControls(sanitationVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("~/Views/SanitationJob/Mobile/_SanitationJobLaborListSearch.cshtml", sanitationVM);
        }
        #region Populate
        [HttpPost]
        public string PopulateLabor_Mobile(int? draw, int? start, int? length, long SanitationJobId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            SanitationJobWrapper pWrapper = new SanitationJobWrapper(userData);
            var Labors = pWrapper.PopulateLabors(SanitationJobId);
            Labors = this.GetAllLaborsSortByColumnWithOrder_Mobile(order, orderDir, Labors);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = Labors.Count();
            totalRecords = Labors.Count();
            int initialPage = start.Value;
            var filteredResult = Labors
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
        private List<LaborModel> GetAllLaborsSortByColumnWithOrder_Mobile(string order, string orderDir, List<LaborModel> data)
        {
            List<LaborModel> lst = new List<LaborModel>();

            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PersonnelClientLookupId).ToList() : data.OrderBy(p => p.PersonnelClientLookupId).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.StartDate).ToList() : data.OrderBy(p => p.StartDate).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Hours).ToList() : data.OrderBy(p => p.Hours).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Value).ToList() : data.OrderBy(p => p.Value).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PersonnelClientLookupId).ToList() : data.OrderBy(p => p.PersonnelClientLookupId).ToList();
                    break;
            }
            return lst;
        }
        #endregion

        #region Add
        [HttpGet]
        public ActionResult AddLabor_Mobile(long SanitationJobId, string ClientLookupId)
        {
            SanitationJobWrapper pWrapper = new SanitationJobWrapper(userData);
            var PersonnelLookUplist = GetList_Personnel();
            SanitationVM objVM = new SanitationVM()
            {
                laborModel = new LaborModel
                {
                    ChargeToId_Primary = SanitationJobId,
                    ClientLookupId = ClientLookupId,
                    PersonnelIdList = PersonnelLookUplist != null ? PersonnelLookUplist.Select(x => new SelectListItem { Text = x.AssignedTo_PersonnelClientLookupId + " - " + x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() }) : new SelectList(new[] { "" })
                }
            };
            LocalizeControls(objVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("Mobile/_AddEditLabor", objVM);
        }
        #endregion

        #region Edit
        [HttpGet]
        public ActionResult EditLabor_Mobile(string ClientLookupId, long SanitationJobId, long chargeToId_Primary, decimal? hours, DateTime? startDate, long personnelId, int updateIndex, long timecardId)
        {
            SanitationJobWrapper pWrapper = new SanitationJobWrapper(userData);
            var PersonnelLookUplist = GetList_Personnel();
            SanitationVM objVM = new SanitationVM()
            {
                laborModel = new LaborModel
                {
                    ClientLookupId = ClientLookupId,
                    ChargeToId_Primary = chargeToId_Primary,
                    Hours = hours,
                    StartDate = startDate,
                    PersonnelId = personnelId,
                    UpdateIndex = updateIndex,
                    TimecardId = timecardId,
                    PersonnelIdList = PersonnelLookUplist != null ? PersonnelLookUplist.Select(x => new SelectListItem { Text = x.AssignedTo_PersonnelClientLookupId + " - " + x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() }) : new SelectList(new[] { "" })
                },
            };
            LocalizeControls(objVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("Mobile/_AddEditLabor", objVM);
        }
        #endregion

        #region Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Savelabor_Mobile(SanitationVM sanVM)
        {
            if (ModelState.IsValid)
            {
                SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
                string Mode = string.Empty;
                List<String> errorList = sWrapper.AddOrUpdateLabor(sanVM, ref Mode);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString(), SanitationJobId = sanVM.laborModel.ChargeToId_Primary, mode = Mode }, JsonRequestBehavior.AllowGet);
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
        #endregion

        #region Notes
        [HttpPost]
        public PartialViewResult LoadSJNotes_Mobile()
        {
            SanitationVM sanitationVM = new SanitationVM();
            sanitationVM._userdata = this.userData;
            sanitationVM.security = this.userData.Security;
            LocalizeControls(sanitationVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("~/Views/SanitationJob/Mobile/_SanitationJobNotesListSearch.cshtml", sanitationVM);
        }
        #region Populate
        [HttpPost]
        public string PopulateNotes_Mobile(int? draw, int? start, int? length, long SanitationJobId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            SanitationJobWrapper pWrapper = new SanitationJobWrapper(userData);
            var Notes = pWrapper.PopulateNotes(SanitationJobId);
            Notes = this.GetAllNotesSortByColumnWithOrder_Mobile(order, orderDir, Notes);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = Notes.Count();
            totalRecords = Notes.Count();
            int initialPage = start.Value;
            var filteredResult = Notes
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeSettings);
        }
        private List<Client.Models.Sanitation.NotesModel> GetAllNotesSortByColumnWithOrder_Mobile(string order, string orderDir, List<Client.Models.Sanitation.NotesModel> data)
        {
            List<Client.Models.Sanitation.NotesModel> lst = new List<Client.Models.Sanitation.NotesModel>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Subject).ToList() : data.OrderBy(p => p.Subject).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.OwnerName).ToList() : data.OrderBy(p => p.OwnerName).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ModifiedDate).ToList() : data.OrderBy(p => p.ModifiedDate).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Subject).ToList() : data.OrderBy(p => p.Subject).ToList();
                    break;
            }
            return lst;
        }
        #endregion

        #region Add
        [HttpGet]
        public ActionResult AddNotes_Mobile(long SanitationJobId, string ClientLookupId)
        {
            SanitationJobWrapper pWrapper = new SanitationJobWrapper(userData);
            SanitationVM objVM = new SanitationVM()
            {
                notesModel = new Models.Sanitation.NotesModel
                {
                    SanitationJobId = SanitationJobId,
                    ClientLookupId = ClientLookupId
                }
            };
            LocalizeControls(objVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("Mobile/_AddEditNotes", objVM);
        }
        #endregion

        #region Edit
        public ActionResult EditNote_Mobile(string ClientLookupId, long SanitationJobId, long notesId, string subject, string content, long updatedindex)
        {
            SanitationJobWrapper pWrapper = new SanitationJobWrapper(userData);
            SanitationVM objVM = new SanitationVM()
            {
                notesModel = new Models.Sanitation.NotesModel
                {
                    SanitationJobId = SanitationJobId,
                    ClientLookupId = ClientLookupId,
                    NotesId = notesId,
                    updatedindex = updatedindex,
                    Subject = subject,
                    Content = content
                }
            };
            LocalizeControls(objVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("Mobile/_AddEditNotes", objVM);
        }
        #endregion

        #region Save
        [HttpPost]
        public ActionResult SaveNotes_Mobile(SanitationVM sanVM)
        {
            if (ModelState.IsValid)
            {
                SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
                string Mode = string.Empty;
                List<String> errorList = sWrapper.AddOrUpdateNote(sanVM, ref Mode);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), SanitationJobId = sanVM.notesModel.SanitationJobId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #endregion
        #endregion
    }
}
