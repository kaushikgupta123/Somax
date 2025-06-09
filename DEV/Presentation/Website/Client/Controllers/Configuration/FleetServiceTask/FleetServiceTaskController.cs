using Client.ActionFilters;
using Client.BusinessWrapper.Configuration.FleetServiceTask;
using Client.Common;
using Client.Controllers.Common;
using Client.Models.Configuration.FleetServiceTask;
using Common.Constants;
using DataContracts;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Client.Controllers.Configuration.FleetServiceTask
{
    public class FleetServiceTaskController  : ConfigBaseController
    {
        [CheckUserSecurity(securityType = SecurityConstants.Fleet_Service_Task)]
        #region Search Service Task
        public ActionResult Index()
        {
            ServiceTaskVM fltservicetaskVM = new ServiceTaskVM();
            fltservicetaskVM.security = this.userData.Security;
            LocalizeControls(fltservicetaskVM, LocalizeResourceSetConstants.FleetServiceTask);
            return View("~/Views/Configuration/FleetServiceTask/Index.cshtml", fltservicetaskVM);
        }
        
        [HttpPost]
        public string GetFleetServiceTaskGridData(int? draw, int? start, int? length, string ClientLookupId = "", string Description = "", string SearchText = "")
        {
            List<FleetServiceTaskSearchModel> fltServiceTaskSearchModelList = new List<FleetServiceTaskSearchModel>();
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var colname = Request.Form.GetValues("columns[" + order + "][name]");
            string _startReadingDate = string.Empty;
            string _endReadingDate = string.Empty;
            SearchText = SearchText.Replace("%", "[%]");
            ClientLookupId = ClientLookupId.Replace("%", "[%]");
            Description = Description.Replace("%", "[%]");
            start = start.HasValue
               ? start / length
               : 0;
            int skip = start * length ?? 0;
            List<string> typeList = new List<string>();
            FleetServiceTaskWrapper fltServiceTaskWrapper = new FleetServiceTaskWrapper(userData);
            List<FleetServiceTaskSearchModel> fleetServiceTaskList = fltServiceTaskWrapper.GetFleetServiceTaskGridData(order, orderDir, skip, length ?? 0, ClientLookupId, Description, SearchText);
            var totalRecords = 0;
            var recordsFiltered = 0;
            if (fleetServiceTaskList != null && fleetServiceTaskList.Count > 0)
            {
                recordsFiltered = fleetServiceTaskList[0].TotalCount;
                totalRecords = fleetServiceTaskList[0].TotalCount;
            }
            int initialPage = start.Value;
            var filteredResult = fleetServiceTaskList
              .ToList();
            bool IsFleetServiceTaskAccessSecurity = userData.Security.Fleet_ServiceTask.Access;

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, IsFleetServiceTaskAccessSecurity = IsFleetServiceTaskAccessSecurity }, JsonSerializerDateSettings);
        }
        #endregion

        #region Add Service Task
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewServiceTask(ServiceTaskVM objST)
        {
            List<string> ErrorList = new List<string>();
            FleetServiceTaskWrapper SAWrapper = new FleetServiceTaskWrapper(userData);
            string ModelValidationFailedMessage = string.Empty;

            if (ModelState.IsValid)
            {
                ServiceTasks servicetask = new ServiceTasks();
                string ST_ClientLookupId = objST.ServiceTaskModel.ClientLookupId.ToUpper().Trim();
                servicetask = SAWrapper.AddServiceTask(objST);
                if (servicetask.ErrorMessages != null && servicetask.ErrorMessages.Count > 0)
                {
                    return Json(servicetask.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), ServiceTaskId = servicetask.ServiceTasksId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult CheckIfServiceTaskExist(ServiceTaskVM ServiceTaskVM)
        {
            bool IfServiceTaskExist = false;
            int count = 0;
            FleetServiceTaskWrapper _servObj = new FleetServiceTaskWrapper(userData);
            count = _servObj.CountIfServiceTaskExist(ServiceTaskVM.ServiceTaskModel.ClientLookupId);
            if (count > 0)
            {
                IfServiceTaskExist = true;
            }
            else
            {
                IfServiceTaskExist = false;
            }
            return Json(!IfServiceTaskExist, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region For Export Service Task
        public string GetServiceTaskPrintData(string _colname, string _coldir, int? draw, int? start, int? length, /*int customQueryDisplayId = 1,*/ string _searchText = "")
        {
            ServiceTaskPrintModel objServiceTaskPrintModel;
            List<ServiceTaskPrintModel> ServiceTaskPrintModelList = new List<ServiceTaskPrintModel>();
            _searchText = _searchText.Replace("%", "[%]");
            start = start.HasValue
               ? start / length
               : 0;
            int skip = start * length ?? 0;
            int lengthForPrint = 100000;

            FleetServiceTaskWrapper ServiceTaskWrapper = new FleetServiceTaskWrapper(userData);
            List<FleetServiceTaskSearchModel> ServiceTaskList = ServiceTaskWrapper.GetFleetServiceTaskGridData( _colname, _coldir, 0, lengthForPrint,"", "", _searchText);
            foreach (var item in ServiceTaskList)
            {
                objServiceTaskPrintModel = new ServiceTaskPrintModel();
                objServiceTaskPrintModel.ClientLookupId = item.ClientLookupId;
                objServiceTaskPrintModel.Description = item.Description;

                ServiceTaskPrintModelList.Add(objServiceTaskPrintModel);
            }
            return JsonConvert.SerializeObject(new { data = ServiceTaskPrintModelList }, JsonSerializerDateSettings);
        }
        #endregion

        #region Get Update Service Task Records

        [HttpPost]
        public JsonResult UpdateServiceTask(long SurviceTaskId, bool InactiveFlag, string ClientLookupId="", string Description="")
        {
            FleetServiceTaskWrapper ServiceTaskWrapper = new FleetServiceTaskWrapper(userData);
            ServiceTasks serviceTask = new ServiceTasks();
            serviceTask = ServiceTaskWrapper.UpdateSurviceTaskRecords(SurviceTaskId, ClientLookupId, Description, InactiveFlag);

            if (serviceTask.ErrorMessages != null && serviceTask.ErrorMessages.Count > 0)
            {
                return Json(serviceTask.ErrorMessages, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { validationStatus = true, ServiceTasksId = serviceTask.ServiceTasksId, ClientLookupId = serviceTask.ClientLookupId }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult ActiveInactiveServiceTask(long SurviceTaskId)
        {
            FleetServiceTaskWrapper ServiceTaskWrapper = new FleetServiceTaskWrapper(userData);
            ServiceTasks serviceTask = new ServiceTasks();
            serviceTask = ServiceTaskWrapper.ActiveInactiveServiceTaskStatus(SurviceTaskId);

            if (serviceTask.ErrorMessages != null && serviceTask.ErrorMessages.Count > 0)
            {
                return Json(serviceTask.ErrorMessages, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { validationStatus = true, ServiceTasksId = serviceTask.ServiceTasksId }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}