using Client.BusinessWrapper.Common;
using Client.Models.Configuration.FleetServiceTask;
using DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.BusinessWrapper.Configuration.FleetServiceTask
{
    public class FleetServiceTaskWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        List<string> errorMessage = new List<string>();
        public FleetServiceTaskWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        #region Search
        public List<FleetServiceTaskSearchModel> GetFleetServiceTaskGridData(string orderbycol = "", string orderDir = "", int skip = 0, int length = 0, string clientLookupId = "", string description = "", string searchText = "")
        {
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            FleetServiceTaskSearchModel fleetServiceTaskSearchModel;
            List<FleetServiceTaskSearchModel> fleetServiceTaskSearchModelList = new List<FleetServiceTaskSearchModel>();
            List<ServiceTasks> servicetaskList = new List<ServiceTasks>();
            ServiceTasks servicetask = new ServiceTasks();
            servicetask.ClientId = this.userData.DatabaseKey.Client.ClientId;
            servicetask.OrderbyColumn = orderbycol;
            servicetask.OrderBy = orderDir;
            servicetask.OffSetVal = skip;
            servicetask.NextRow = length;
            servicetask.ClientLookupId = clientLookupId;
            servicetask.Description = description;
            servicetask.SearchText = searchText;
            servicetaskList = servicetask.ServiceTaskRetrieveChunkSearchV2(userData.DatabaseKey, userData.Site.TimeZone);
            foreach (var item in servicetaskList)
            {
                fleetServiceTaskSearchModel = new FleetServiceTaskSearchModel();
                fleetServiceTaskSearchModel.ServiceTaskId = item.ServiceTasksId;
                fleetServiceTaskSearchModel.ClientLookupId = item.ClientLookupId;
                fleetServiceTaskSearchModel.Description = item.Description;
                fleetServiceTaskSearchModel.InactiveFlag = item.InactiveFlag;
                fleetServiceTaskSearchModel.TotalCount = item.TotalCount;
                fleetServiceTaskSearchModelList.Add(fleetServiceTaskSearchModel);
            }
          
            return fleetServiceTaskSearchModelList;
        }
        #endregion

        #region Add Service Task
        public ServiceTasks AddServiceTask(ServiceTaskVM objSTM)
        {
            ServiceTaskModel objServiceTask = new ServiceTaskModel();

            #region Insert into Service Task Table
            ServiceTasks serviceTask = new ServiceTasks();
            serviceTask.ClientId = this.userData.DatabaseKey.User.ClientId;
            serviceTask.ClientLookupId = objSTM.ServiceTaskModel.ClientLookupId;
            serviceTask.Description = objSTM.ServiceTaskModel.Description;
            serviceTask.Create(this.userData.DatabaseKey);          
            #endregion

            return serviceTask;
        }

        #endregion
        #region Edit Service Tasks
        public ServiceTasks UpdateSurviceTaskRecords(long SurviceTaskId, string ClientLookupId, string Description, bool InactiveFlag)
        {
            ServiceTasks serviceTask = new ServiceTasks()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ServiceTasksId = SurviceTaskId
            };
            serviceTask.Retrieve(_dbKey);
            if (serviceTask.ClientLookupId != ClientLookupId && ClientLookupId != "")
            {
                serviceTask.ClientLookupId = ClientLookupId != "" ? ClientLookupId : serviceTask.ClientLookupId;
                serviceTask.Description = Description != "" ? Description : serviceTask.Description;
                serviceTask.InactiveFlag = InactiveFlag;
                serviceTask.CheckDuplicateServiceTask(this.userData.DatabaseKey);
                if (serviceTask.ErrorMessages == null || serviceTask.ErrorMessages.Count == 0)
                {
                    serviceTask.Update(this.userData.DatabaseKey);
                }
            }
            else
            {
                serviceTask.ClientLookupId = ClientLookupId != "" ? ClientLookupId : serviceTask.ClientLookupId;
                serviceTask.Description = Description != "" ? Description : serviceTask.Description;
                serviceTask.InactiveFlag = InactiveFlag;
                serviceTask.Update(this.userData.DatabaseKey);
            }

            return serviceTask;
        }

        #endregion
        #region Active/Inactive Service Tasks
        public ServiceTasks ActiveInactiveServiceTaskStatus(long SurviceTaskId)
        {
            ServiceTasks serviceTask = new ServiceTasks()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ServiceTasksId = SurviceTaskId
            };
            serviceTask.Retrieve(_dbKey);
            if (serviceTask.InactiveFlag == true)
            {
                serviceTask.InactiveFlag = false;
                serviceTask.Update(this.userData.DatabaseKey);
            }
            else
            {
                serviceTask.Flag = "Inactivate";
                serviceTask.CheckServiceTaskIsInactivateorActivate(this.userData.DatabaseKey);
                if (serviceTask.ErrorMessages == null || serviceTask.ErrorMessages.Count == 0)
                {
                    serviceTask.InactiveFlag = true;
                    serviceTask.Update(this.userData.DatabaseKey);
                }
                
            }

            return serviceTask;
        }

        #endregion
        #region Check Service Tasks Exists
        public int CountIfServiceTaskExist(string clientlookupid)
        {
            int count = 0;
            ServiceTasks _service = new ServiceTasks();
            _service.ClientId = userData.DatabaseKey.Client.ClientId;
            _service.ClientLookupId = clientlookupid;
            _service.CheckDuplicateServiceTask(this.userData.DatabaseKey);
            if (_service.ErrorMessages != null)
            {
                count = _service.ErrorMessages.Count;
            }
            return count;
        }

        #endregion

        #region Get All Fleet Service Task
        public List<SelectListItem> GetServiceTask()
        {
            ServiceTasks ServiceTasks = new ServiceTasks();
            ServiceTasks.ClientId = userData.DatabaseKey.Client.ClientId;
            List<ServiceTasks> lstServiceTasks = ServiceTasks.RetrieveAllCustom(userData.DatabaseKey);
            var ServTasks = lstServiceTasks.Select(x => new SelectListItem { Text = x.Description, Value = x.ServiceTasksId.ToString() }).ToList();
            return ServTasks;
        }

        #endregion

    }
}