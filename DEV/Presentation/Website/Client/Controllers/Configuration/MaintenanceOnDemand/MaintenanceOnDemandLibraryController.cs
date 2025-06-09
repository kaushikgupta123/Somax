using Client.ActionFilters;
using Client.BusinessWrapper.Common;
using Client.BusinessWrapper.Configuration.MaintenanceOnDemandLibrary;
using Client.Common;
using Client.Controllers.Common;
using Client.Models.Configuration.MaintenanceOnDemandLibrary;
using Common.Constants;
using DataContracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
namespace Client.Controllers.Configuration.MaintenanceOnDemand
{
    public class MaintenanceOnDemandLibraryController : ConfigBaseController
    {
        [CheckUserSecurity(securityType = SecurityConstants.OnDemand_Library)]
        #region Search
        public ActionResult Index()
        {
            MaintenanceOnDemandVM objVM = new MaintenanceOnDemandVM();
            MaintenanceOnDemandModel objModel = new MaintenanceOnDemandModel();
            MaintenanceOnDemandWrapper mWrapper = new MaintenanceOnDemandWrapper(userData);
            objVM.security = this.userData.Security;
            LocalizeControls(objVM, LocalizeResourceSetConstants.LibraryDetails);
            return View("~/Views/Configuration/MaintenanceOnDemandLibrary/Index.cshtml", objVM);
        }
        [HttpPost]
        public string GetMOnDemandGridData(int? draw, int? start, int? length, string MaintOnDemandMasterId = "", string Description = "", string Type = "", DateTime? CreateDate = null, string srcData = "")
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var colname = Request.Form.GetValues("columns[" + order + "][name]");
            MaintenanceOnDemandWrapper mWrapper = new MaintenanceOnDemandWrapper(userData);
            var mOnDemandList = mWrapper.RetrieveAllBySiteId();
            List<string> typeList = new List<string>();
            if (mOnDemandList != null)
            {
                typeList = mOnDemandList.Where(x => !string.IsNullOrEmpty(x.Type)).Select(x => x.Type).Distinct().ToList();
            }
            mOnDemandList = this.GetAllEquipmentsSortByColumnWithOrder(colname[0], orderDir, mOnDemandList);
            #region Text-Search
            string filter = srcData;
            if (!string.IsNullOrEmpty(filter))
            {
                filter = filter.ToUpper();
                string format = "MM/dd/yyyy";
                DateTime dateTime;
                DateTime.TryParseExact(filter, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
                mOnDemandList = mOnDemandList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookUpId) && x.ClientLookUpId.ToUpper().Contains(filter))
                                                        || (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(filter))
                                                        || (!string.IsNullOrWhiteSpace(x.Type) && x.Type.ToUpper().Contains(filter))
                                                        || (x.CreateDate.Date.ToString("MM/dd/yyyy").Equals(dateTime))
                                                        ).ToList();
            }
            #endregion
            mOnDemandList = GetPRSearchResult(mOnDemandList, MaintOnDemandMasterId, Description, Type, CreateDate);

            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = mOnDemandList.Count();
            totalRecords = mOnDemandList.Count();

            int initialPage = start.Value;

            var filteredResult = mOnDemandList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, typeList = typeList }, JsonSerializerDateSettings);
        }
        public string GetMOnDemandPrintData(string colname, string coldir, string OrderCol = "0", string OrderDir = "asc", string MaintOnDemandMasterId = "", string Description = "", string Type = "", DateTime? CreateDate = null, string srcData = "")
        {
            List<MaintenanceOnDemandPrintModel> MaintenanceOnDemandPrintModelList = new List<MaintenanceOnDemandPrintModel>();
            MaintenanceOnDemandPrintModel objMaintenanceOnDemandPrintModel;
            MaintenanceOnDemandWrapper mWrapper = new MaintenanceOnDemandWrapper(userData);
            var mOnDemandList = mWrapper.RetrieveAllBySiteId();
            List<string> typeList = new List<string>();
            if (mOnDemandList != null)
            {
                #region Text-Search
                string filter = srcData;
                if (!string.IsNullOrEmpty(filter))
                {
                    filter = filter.ToUpper();
                    string format = "MM/dd/yyyy";
                    DateTime dateTime;
                    DateTime.TryParseExact(filter, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
                    mOnDemandList = mOnDemandList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookUpId) && x.ClientLookUpId.ToUpper().Contains(filter))
                                                            || (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(filter))
                                                            || (!string.IsNullOrWhiteSpace(x.Type) && x.Type.ToUpper().Contains(filter))
                                                            || (x.CreateDate.Date.ToString("MM/dd/yyyy").Equals(dateTime))
                                                            ).ToList();
                }
                #endregion
                mOnDemandList = this.GetAllEquipmentsSortByColumnWithOrder(OrderCol, OrderDir, mOnDemandList);
                mOnDemandList = this.GetAllEquipmentsSortByColumnWithOrder(colname, coldir, mOnDemandList);
                mOnDemandList = GetPRSearchResult(mOnDemandList, MaintOnDemandMasterId, Description, Type, CreateDate);
            }
            foreach (var item in mOnDemandList)
            {
                objMaintenanceOnDemandPrintModel = new MaintenanceOnDemandPrintModel();
                objMaintenanceOnDemandPrintModel.ClientLookUpId = item.ClientLookUpId;
                objMaintenanceOnDemandPrintModel.Description = item.Description;
                objMaintenanceOnDemandPrintModel.Type = item.Type;
                objMaintenanceOnDemandPrintModel.CreateDate = item.CreateDate;
                MaintenanceOnDemandPrintModelList.Add(objMaintenanceOnDemandPrintModel);
            }
            return JsonConvert.SerializeObject(new { data = MaintenanceOnDemandPrintModelList }, JsonSerializerDateSettings);
        }
        private List<MaintenanceOnDemandModel> GetAllEquipmentsSortByColumnWithOrder(string order, string orderDir, List<MaintenanceOnDemandModel> data)
        {
            List<MaintenanceOnDemandModel> lst = new List<MaintenanceOnDemandModel>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookUpId).ToList() : data.OrderBy(p => p.ClientLookUpId).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Type).ToList() : data.OrderBy(p => p.Type).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CreateDate).ToList() : data.OrderBy(p => p.CreateDate).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookUpId).ToList() : data.OrderBy(p => p.ClientLookUpId).ToList();
                    break;
            }
            return lst;
        }
        private List<MaintenanceOnDemandModel> GetPRSearchResult(List<MaintenanceOnDemandModel> mOnDemandList, string MaintOnDemandMasterId = "", string Description = "", string Type = "", DateTime? CreateDate = null)
        {
            if (mOnDemandList != null)
            {
                if (!string.IsNullOrEmpty(MaintOnDemandMasterId))
                {
                    MaintOnDemandMasterId = MaintOnDemandMasterId.ToUpper();
                    mOnDemandList = mOnDemandList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookUpId) && x.ClientLookUpId.ToUpper().Contains(MaintOnDemandMasterId))).ToList();
                }
                if (!string.IsNullOrEmpty(Description))
                {
                    Description = Description.ToUpper();
                    mOnDemandList = mOnDemandList.Where(x => (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(Description))).ToList();
                }
                if (!string.IsNullOrEmpty(Type))
                {
                    mOnDemandList = mOnDemandList.Where(x => (!string.IsNullOrWhiteSpace(x.Type) && x.Type.Equals(Type))).ToList();
                }

                if (CreateDate != null)
                {
                    mOnDemandList = mOnDemandList.Where(x => (x.CreateDate != null && x.CreateDate.Date.Equals(CreateDate.Value.Date))).ToList();
                }
            }
            return mOnDemandList;
        }
        #endregion

        #region Details
        public PartialViewResult OndemandDetail(long MaintOnDemandMasterId = 0)
        {
            MaintenanceOnDemandVM objVM = new MaintenanceOnDemandVM();
            MaintenanceOnDemandModel objModel = new MaintenanceOnDemandModel();
            MaintenanceOnDemandWrapper mWrapper = new MaintenanceOnDemandWrapper(userData);
            objModel = mWrapper.populateOndemandDetails(MaintOnDemandMasterId);
            objVM.maintenanceOnDemanModel = objModel;
            objVM.security = this.userData.Security;
            LocalizeControls(objVM, LocalizeResourceSetConstants.LibraryDetails);
            return PartialView("~/Views/Configuration/MaintenanceOnDemandLibrary/_MaintenanceOnDemandDetails.cshtml", objVM);
        }
        #endregion

        #region Task
        public string PopulateTasks(int? draw, int? start, int? length, long MaintOnDemandMasterId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            MaintenanceOnDemandWrapper mWrapper = new MaintenanceOnDemandWrapper(userData);

            var Tasks = mWrapper.PopulateTasks(MaintOnDemandMasterId);
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
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, showAddBtn = showAddBtn, showEditBtn= showEditBtn, showDeleteBtn = showDeleteBtn }, JsonSerializer12HoursDateAndTimeSettings);
        }
        private List<MaintOnDemandMasterTask> GetAllTasksSortByColumnWithOrder(string order, string orderDir, List<MaintOnDemandMasterTask> data)
        {
            List<MaintOnDemandMasterTask> lst = new List<MaintOnDemandMasterTask>();
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
        public ActionResult DeleteTasks(long MasterId, long MasterTaskId)
        {
            MaintenanceOnDemandWrapper mWrapper = new MaintenanceOnDemandWrapper(userData);
            if (mWrapper.DeleteTask(MasterTaskId))
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AddTask(long MasterId, string ClientLookUpId)
        {
            MaintenanceOnDemandVM objVM = new MaintenanceOnDemandVM();
            TaskModel objModel = new TaskModel();
            MaintenanceOnDemandWrapper mWrapper = new MaintenanceOnDemandWrapper(userData);
            objModel.MaintOnDemandMasterId = MasterId;
            objModel.MaintOnDemandMasterTaskId = 0;
            objModel.ClientLookUpId = ClientLookUpId;
            objVM.taskModel = objModel;
            objVM.security = this.userData.Security;
            LocalizeControls(objVM, LocalizeResourceSetConstants.LibraryDetails);
            return PartialView("~/Views/Configuration/MaintenanceOnDemandLibrary/_AddOrEditTask.cshtml", objVM);
        }
        public ActionResult EditTask(long MasterId, string ClientLookUpId, long MaintOnDemandMasterTaskId, string TaskId, string Description)
        {
            MaintenanceOnDemandVM objVM = new MaintenanceOnDemandVM();
            TaskModel objModel = new TaskModel();
            objModel.MaintOnDemandMasterId = MasterId;
            objModel.ClientLookUpId = ClientLookUpId;
            objModel.MaintOnDemandMasterTaskId = MaintOnDemandMasterTaskId;
            objModel.TaskId = TaskId;
            objModel.Description = Description;
            objVM.taskModel = objModel;
            objVM.security = this.userData.Security;
            LocalizeControls(objVM, LocalizeResourceSetConstants.LibraryDetails);
            return PartialView("~/Views/Configuration/MaintenanceOnDemandLibrary/_AddOrEditTask.cshtml", objVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTask(MaintenanceOnDemandVM objVM)
        {
            if (ModelState.IsValid)
            {
                MaintenanceOnDemandWrapper mWrapper = new MaintenanceOnDemandWrapper(userData);
                string Mode = string.Empty;
                List<String> errorList = mWrapper.AddOrUpdateTask(objVM, ref Mode);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), MaintOnDemandMasterId = objVM.taskModel.MaintOnDemandMasterId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region On-Demand Add/Edit
        public ActionResult AddOndemand(string ClientLookUpId, long? MasterId)
        {
            MaintenanceOnDemandVM objVM = new MaintenanceOnDemandVM();
            MaintenanceOnDemandModel objModel = new MaintenanceOnDemandModel();
            MaintenanceOnDemandWrapper mWrapper = new MaintenanceOnDemandWrapper(userData);
            List<DataContracts.LookupList> Type = new List<DataContracts.LookupList>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                Type = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_TYPE).ToList();
            }
            if (Type != null)
            {
                var tmpTypeList = Type.GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).ToList();
                objModel.TypeList = tmpTypeList.Select(x => new SelectListItem { Text = x.ListValue + " | " + x.Description, Value = x.ListValue });
            }
            objModel.MaintOnDemandMasterId = 0;
            objModel.MasterIdForCancel = MasterId ?? 0;
            objVM.maintenanceOnDemanModel = objModel;

            LocalizeControls(objVM, LocalizeResourceSetConstants.LibraryDetails);
            return PartialView("~/Views/Configuration/MaintenanceOnDemandLibrary/_AddOrEditMaintenanceOnDemand.cshtml", objVM);
        }
        public ActionResult EditOndemand(long MasterId, string ClientLookUpId, string Description, string Type)
        {
            MaintenanceOnDemandVM objVM = new MaintenanceOnDemandVM();
            MaintenanceOnDemandModel objModel = new MaintenanceOnDemandModel()
            {
                MaintOnDemandMasterId = MasterId,
                ClientLookUpId = ClientLookUpId,
                Description = Description,
                Type = Type
            };
            #region TypeList
            List<DataContracts.LookupList> TypeList = new List<DataContracts.LookupList>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                TypeList = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_TYPE).ToList();
            }
            if (TypeList != null)
            {
                var tmpTypeList = TypeList.GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).ToList();
                objModel.TypeList = tmpTypeList.Select(x => new SelectListItem { Text = x.ListValue + " | " + x.Description, Value = x.ListValue });
            }
            #endregion
            objVM.maintenanceOnDemanModel = objModel;
            LocalizeControls(objVM, LocalizeResourceSetConstants.LibraryDetails);
            return PartialView("~/Views/Configuration/MaintenanceOnDemandLibrary/_AddOrEditMaintenanceOnDemand.cshtml", objVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOndemand(MaintenanceOnDemandVM objVM, string Command)
        {
            if (ModelState.IsValid)
            {
                MaintenanceOnDemandWrapper mWrapper = new MaintenanceOnDemandWrapper(userData);
                string Mode = string.Empty;
                long onDemandMasterId = 0;
                List<String> errorList = mWrapper.AddOrUpdateOndemand(objVM, ref Mode, ref onDemandMasterId);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), MaintOnDemandMasterId = onDemandMasterId, mode = Mode, Command = Command }, JsonRequestBehavior.AllowGet);
                }
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