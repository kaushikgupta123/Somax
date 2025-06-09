using Client.ActionFilters;
using Client.BusinessWrapper.Configuration.MasterSanitationLibrary;
using Client.Common;
using Client.Controllers.Common;
using Client.Models.Configuration.MasterSanitationLibrary;
using Common.Constants;
using DataContracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace Client.Controllers.Configuration.MasterSanitation
{
    public class MasterSanitationLibraryController : ConfigBaseController
    {
        #region Search
        [CheckUserSecurity(securityType = SecurityConstants.MasterSanitation_Library)]
        public ActionResult Index()
        {
            MasterSanitationVM objVM = new MasterSanitationVM();
            MasterSanitationModel objModel = new MasterSanitationModel();
            MasterSanitationWrapper mWrapper = new MasterSanitationWrapper(userData);
            objVM.security = this.userData.Security;
            LocalizeControls(objVM, LocalizeResourceSetConstants.LibraryDetails);
            return View("~/Views/Configuration/MasterSanitationLibrary/Index.cshtml", objVM);
        }
        [HttpPost]
        public string GetSanitationLibraryGridData(int? draw, int? start, int? length, decimal? JobDuration, int? Frequency, string MasterSanLibraryId = "", string FrequencyType = "", string Description = "", DateTime? CreateDate = null, string srcData = "")
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var colname = Request.Form.GetValues("columns[" + order + "][name]");
            MasterSanitationWrapper mWrapper = new MasterSanitationWrapper(userData);
            var mSaniLibraryList = mWrapper.RetrieveAllBySiteId();
            List<string> frequencyTypeList = new List<string>();
            if (mSaniLibraryList != null)
            {
                frequencyTypeList = mSaniLibraryList.Where(x => !string.IsNullOrEmpty(x.FrequencyType)).Select(x => x.FrequencyType).Distinct().ToList();
            }
            mSaniLibraryList = this.GetAllSaniLibraryListSortByColumnWithOrder(colname[0], orderDir, mSaniLibraryList);
            #region Text-Search
            string filter = srcData;
            if (!string.IsNullOrEmpty(filter))
            {
                filter = filter.ToUpper();

                DateTime createdate;
                DateTime.TryParseExact(filter, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out createdate);

                int valfrequency;
                bool outfrequency = int.TryParse(filter, out valfrequency);

                decimal valduration;
                bool outvalduration = decimal.TryParse(filter, out valduration);

                mSaniLibraryList = mSaniLibraryList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookUpId) && x.ClientLookUpId.ToUpper().Contains(filter))
                                                        || (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(filter))
                                                        || (!string.IsNullOrWhiteSpace(x.FrequencyType) && x.FrequencyType.ToUpper().Contains(filter))
                                                        || (outvalduration == true && x.JobDuration.Equals(valduration))
                                                        || (outfrequency == true && x.Frequency.Equals(valfrequency))
                                                        || (x.CreateDate != null && x.CreateDate.Value != default(DateTime) && x.CreateDate.Value.Date.Equals(createdate))
                                                        ).ToList();
            }
            #endregion
            mSaniLibraryList = GetPRSearchResult(mSaniLibraryList, JobDuration, Frequency, MasterSanLibraryId, FrequencyType, Description, CreateDate);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = mSaniLibraryList.Count();
            totalRecords = mSaniLibraryList.Count();
            int initialPage = start.Value;
            var filteredResult = mSaniLibraryList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, frequencyTypeList = frequencyTypeList }, JsonSerializerDateSettings);
        }

        public string GetMasterSanitationPrintData(string colname, string coldir, decimal? JobDuration, int? Frequency, string OrderCol = "0", string OrderDir = "asc", string MasterSanLibraryId = "", string FrequencyType = "", string Description = "", DateTime? CreateDate = null, string srcData = "")
        {
            List<MasterSanitationPrintModel> MasterSanitationPrintModelList = new List<MasterSanitationPrintModel>();
            MasterSanitationPrintModel objMasterSanitationPrintModel;
            MasterSanitationWrapper mWrapper = new MasterSanitationWrapper(userData);
            var mSaniLibraryList = mWrapper.RetrieveAllBySiteId();
            List<string> frequencyTypeList = new List<string>();
            if (mSaniLibraryList != null)
            {
                frequencyTypeList = mSaniLibraryList.Where(x => !string.IsNullOrEmpty(x.FrequencyType)).Select(x => x.FrequencyType).Distinct().ToList();
            }

            mSaniLibraryList = this.GetAllSaniLibraryListSortByColumnWithOrder(colname, coldir, mSaniLibraryList);
            #region Text-Search
            string filter = srcData;
            if (!string.IsNullOrEmpty(filter))
            {
                filter = filter.ToUpper();
                DateTime createdate;
                DateTime.TryParseExact(filter, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out createdate);
                int valfrequency;
                bool outfrequency = int.TryParse(filter, out valfrequency);
                decimal valduration;
                bool outvalduration = decimal.TryParse(filter, out valduration);
                mSaniLibraryList = mSaniLibraryList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookUpId) && x.ClientLookUpId.ToUpper().Contains(filter))
                                                        || (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(filter))
                                                        || (!string.IsNullOrWhiteSpace(x.FrequencyType) && x.FrequencyType.ToUpper().Contains(filter))
                                                        || (outvalduration == true && x.JobDuration.Equals(valduration))
                                                        || (outfrequency == true && x.Frequency.Equals(outfrequency))
                                                        || (x.CreateDate != null && x.CreateDate.Value != default(DateTime) && x.CreateDate.Value.Date.Equals(createdate))
                                                        ).ToList();
            }
            #endregion
            mSaniLibraryList = GetPRSearchResult(mSaniLibraryList, JobDuration, Frequency, MasterSanLibraryId, FrequencyType, Description, CreateDate);
            foreach (var item in mSaniLibraryList)
            {
                objMasterSanitationPrintModel = new MasterSanitationPrintModel();
                objMasterSanitationPrintModel.ClientLookUpId = item.ClientLookUpId;
                objMasterSanitationPrintModel.Description = item.Description;
                objMasterSanitationPrintModel.JobDuration = item.JobDuration;
                objMasterSanitationPrintModel.FrequencyType = item.FrequencyType;
                objMasterSanitationPrintModel.Frequency = item.Frequency;
                objMasterSanitationPrintModel.CreateDate = item.CreateDate;
                MasterSanitationPrintModelList.Add(objMasterSanitationPrintModel);
            }

            return JsonConvert.SerializeObject(new { data = MasterSanitationPrintModelList }, JsonSerializerDateSettings);
        }
        private List<MasterSanitationModel> GetAllSaniLibraryListSortByColumnWithOrder(string order, string orderDir, List<MasterSanitationModel> data)
        {
            List<MasterSanitationModel> lst = new List<MasterSanitationModel>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookUpId).ToList() : data.OrderBy(p => p.ClientLookUpId).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.JobDuration).ToList() : data.OrderBy(p => p.JobDuration).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.FrequencyType).ToList() : data.OrderBy(p => p.FrequencyType).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Frequency).ToList() : data.OrderBy(p => p.Frequency).ToList();
                    break;
                case "5":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CreateDate).ToList() : data.OrderBy(p => p.CreateDate).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookUpId).ToList() : data.OrderBy(p => p.ClientLookUpId).ToList();
                    break;
            }
            return lst;
        }
        private List<MasterSanitationModel> GetPRSearchResult(List<MasterSanitationModel> mSaniLibraryList, decimal? JobDuration, int? Frequency, string MasterSanLibraryId = "", string FrequencyType = "", string Description = "", DateTime? CreateDate = null)
        {
            if (mSaniLibraryList != null)
            {
                if (JobDuration.HasValue)
                {
                    mSaniLibraryList = mSaniLibraryList.Where(x => x.JobDuration.Equals(JobDuration)).ToList();
                }
                if (Frequency.HasValue)
                {
                    mSaniLibraryList = mSaniLibraryList.Where(x => x.Frequency.Equals(Frequency)).ToList();
                }
                if (!string.IsNullOrEmpty(MasterSanLibraryId))
                {
                    MasterSanLibraryId = MasterSanLibraryId.ToUpper();
                    mSaniLibraryList = mSaniLibraryList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookUpId) && x.ClientLookUpId.ToUpper().Contains(MasterSanLibraryId))).ToList();
                }
                if (!string.IsNullOrEmpty(FrequencyType))
                {
                    FrequencyType = FrequencyType.ToUpper();
                    mSaniLibraryList = mSaniLibraryList.Where(x => (!string.IsNullOrWhiteSpace(x.FrequencyType) && x.FrequencyType.ToUpper().Contains(FrequencyType))).ToList();
                }
                if (!string.IsNullOrEmpty(Description))
                {
                    Description = Description.ToUpper();
                    mSaniLibraryList = mSaniLibraryList.Where(x => (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(Description))).ToList();
                }
                if (CreateDate != null)
                {
                    mSaniLibraryList = mSaniLibraryList.Where(x => (x.CreateDate != null && x.CreateDate.Value.Date.Equals(CreateDate.Value.Date))).ToList();
                }

            }
            return mSaniLibraryList;
        }
        #endregion
        #region Details
        public PartialViewResult SaniLibraryDetail(long MasterSanLibraryId = 0)
        {
            MasterSanitationVM objVM = new MasterSanitationVM();
            MasterSanitationModel objModel = new MasterSanitationModel();
            MasterSanitationWrapper mWrapper = new MasterSanitationWrapper(userData);
            objModel = mWrapper.populateSaniLibraryDetails(MasterSanLibraryId);
            objVM.masterSanitationModel = objModel;
            objVM.security = this.userData.Security;           
            LocalizeControls(objVM, LocalizeResourceSetConstants.LibraryDetails);
            return PartialView("~/Views/Configuration/MasterSanitationLibrary/_MasterSanitationDetails.cshtml", objVM);
        }
        #endregion
        #region Sanitation Master Add/Edit
        public ActionResult AddSanitationMaster(string ClientLookUpId, long? MasterId)
        {
            MasterSanitationVM objVM = new MasterSanitationVM();
            MasterSanitationModel objModel = new MasterSanitationModel();
            MasterSanitationWrapper mWrapper = new MasterSanitationWrapper(userData);
            List<DataContracts.LookupList> Type = new List<DataContracts.LookupList>();
            #region Dropdown
            var FrequencyTypeList = UtilityFunction.populateFrequencyTypeList();
            if (FrequencyTypeList != null)
            {
                objModel.FrequencyTypeList = FrequencyTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            var ScheduleTypeList = UtilityFunction.GetPMLibraryScheduleType();
            if (ScheduleTypeList != null)
            {
                objModel.ScheduleTypeList = ScheduleTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            var ScheduleMethodList = UtilityFunction.populateScheduleMethodList();
            if (ScheduleMethodList != null)
            {
                objModel.ScheduleMethodList = ScheduleMethodList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            #endregion
            objModel.MasterSanLibraryId = 0;
            objModel.MasterIdForCancel = MasterId ?? 0;
            objVM.masterSanitationModel = objModel;
            LocalizeControls(objVM, LocalizeResourceSetConstants.LibraryDetails);
            return PartialView("~/Views/Configuration/MasterSanitationLibrary/_AddOrEditMasterSanitation.cshtml", objVM);
        }

        public ActionResult EditSanitationMaster(long MasterId, string ClientLookUpId, string Description, bool InactiveFlag, int? Frequency, string FrequencyType = "", string ScheduleType = "", string ScheduleMethod = "")
        {
            MasterSanitationVM objVM = new MasterSanitationVM();
            MasterSanitationModel objModel = new MasterSanitationModel()
            {
                MasterSanLibraryId = MasterId,
                ClientLookUpId = ClientLookUpId,
                Description = Description,
                InactiveFlag = InactiveFlag,
                FrequencyType = FrequencyType,
                Frequency = Frequency,
                ScheduleType = ScheduleType,
                ScheduleMethod = ScheduleMethod
            };
            #region Dropdown
            var FrequencyTypeList = UtilityFunction.populateFrequencyTypeList();
            if (FrequencyTypeList != null)
            {
                objModel.FrequencyTypeList = FrequencyTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            var ScheduleTypeList = UtilityFunction.GetPMLibraryScheduleType();
            if (ScheduleTypeList != null)
            {
                objModel.ScheduleTypeList = ScheduleTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            var ScheduleMethodList = UtilityFunction.populateScheduleMethodList();
            if (ScheduleMethodList != null)
            {
                objModel.ScheduleMethodList = ScheduleMethodList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            #endregion
            objVM.masterSanitationModel = objModel;
            LocalizeControls(objVM, LocalizeResourceSetConstants.LibraryDetails);
            return PartialView("~/Views/Configuration/MasterSanitationLibrary/_AddOrEditMasterSanitation.cshtml", objVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSanitationMaster(MasterSanitationVM objVM, string Command)
        {
            if (ModelState.IsValid)
            {
                MasterSanitationWrapper mWrapper = new MasterSanitationWrapper(userData);
                string Mode = string.Empty;
                long masterSanLibraryId = 0;
                List<String> errorList = mWrapper.AddOrUpdateMasterSanitation(objVM, ref Mode, ref masterSanLibraryId);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), MasterSanLibraryId = masterSanLibraryId, mode = Mode, Command = Command }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region Task
        public string PopulateTasks(int? draw, int? start, int? length, long MasterSanLibraryId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            MasterSanitationWrapper mWrapper = new MasterSanitationWrapper(userData);
            var Tasks = mWrapper.PopulateTasks(MasterSanLibraryId);
            Tasks = this.GetAllTasksSortByColumnWithOrder(order, orderDir, Tasks);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = Tasks.Count();
            totalRecords = Tasks.Count();
            int initialPage = start.Value;
            var filteredResult = Tasks
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            bool showAddBtn = userData.Security.OnDemandLibrary.Create;
            bool showEditBtn = userData.Security.OnDemandLibrary.Edit;
            bool showDeleteBtn = userData.Security.OnDemandLibrary.Delete;
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, showAddBtn = showAddBtn, showEditBtn = showEditBtn, showDeleteBtn = showDeleteBtn });
        }
        private List<MasterSanLibraryTask> GetAllTasksSortByColumnWithOrder(string order, string orderDir, List<MasterSanLibraryTask> data)
        {
            List<MasterSanLibraryTask> lst = new List<MasterSanLibraryTask>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TaskId).ToList() : data.OrderBy(p => p.TaskId).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                    break;
            }
            return lst;
        }

        [HttpGet]
        public ActionResult AddTask(long MasterId, string ClientLookUpId)
        {
            MasterSanitationVM objVM = new MasterSanitationVM();
            TaskModel objModel = new TaskModel();
            MasterSanitationWrapper mWrapper = new MasterSanitationWrapper(userData);
            objModel.MasterSanLibraryId = MasterId;
            objModel.MasterSanLibraryTaskId = 0;
            objModel.ClientLookUpId = ClientLookUpId;
            objVM.taskModel = objModel;
            objVM.security = this.userData.Security;
            LocalizeControls(objVM, LocalizeResourceSetConstants.LibraryDetails);
            return PartialView("~/Views/Configuration/MasterSanitationLibrary/_AddOrEditTask.cshtml", objVM);
        }
        public ActionResult EditTask(long MasterId, string ClientLookUpId, long MasterSanLibraryTaskId, string TaskId, string Description)
        {
            MasterSanitationVM objVM = new MasterSanitationVM();
            TaskModel objModel = new TaskModel();
            objModel.MasterSanLibraryId = MasterId;
            objModel.ClientLookUpId = ClientLookUpId;
            objModel.MasterSanLibraryTaskId = MasterSanLibraryTaskId;
            objModel.TaskId = TaskId;
            objModel.Description = Description;
            objVM.taskModel = objModel;
            objVM.security = this.userData.Security;
            LocalizeControls(objVM, LocalizeResourceSetConstants.LibraryDetails);
            return PartialView("~/Views/Configuration/MasterSanitationLibrary/_AddOrEditTask.cshtml", objVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTask(MasterSanitationVM objVM)
        {
            if (ModelState.IsValid)
            {
                MasterSanitationWrapper mWrapper = new MasterSanitationWrapper(userData);
                string Mode = string.Empty;
                List<String> errorList = mWrapper.AddOrUpdateTask(objVM, ref Mode);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), MasterSanLibraryTaskId = objVM.taskModel.MasterSanLibraryId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult DeleteTasks(long MasterId, long MasterTaskId)
        {
            MasterSanitationWrapper mWrapper = new MasterSanitationWrapper(userData);
            if (mWrapper.DeleteTask(MasterTaskId))
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}