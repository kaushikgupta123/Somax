using Client.Models.WorkOrderPlanning;
using DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using INTDataLayer.BAL;
using INTDataLayer.EL;
using System.Data;
using Client.BusinessWrapper.Common;
using Client.Models.Common;
using Common.Constants;
using Common.Enumerations;
namespace Client.BusinessWrapper.WorkOrderPlanning
{
    public class WorkOrderPlanningResourceListWrapper
    {
        UserEL objUserEL = new UserEL();
        private DatabaseKey _dbKey;
        private UserData userData;
        public WorkOrderPlanningResourceListWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
            objUserEL.UserInfoId = userData.DatabaseKey.User.UserInfoId;
            objUserEL.UserFullName = userData.DatabaseKey.UserName;
            objUserEL.ClientId = userData.DatabaseKey.Client.ClientId;
            objUserEL.SiteId = userData.Site.SiteId;
        }
        #region Common
        public List<List<WorkOrderSchedule>> SchedulePersonnelList(string WorkOrderId = "")
        {
            WorkOrderSchedule WS = new WorkOrderSchedule();
            WS.ClientId = userData.DatabaseKey.Client.ClientId;
            WS.SiteId = userData.Site.SiteId;
            WS.WorkOrderId = string.IsNullOrEmpty(WorkOrderId) ? 0 : Convert.ToInt64(WorkOrderId);
            WS.RetrievePersonnel(userData.DatabaseKey);
            return WS.TotalRecords;
        }

        public List<List<WorkOrderSchedule>> SchedulePersonnelListByAssetGroupMasterQuery(string WorkOrderId = "")
        {
            WorkOrderSchedule WS = new WorkOrderSchedule();
            WS.ClientId = userData.DatabaseKey.Client.ClientId;
            WS.SiteId = userData.Site.SiteId;
            WS.WorkOrderId = string.IsNullOrEmpty(WorkOrderId) ? 0 : Convert.ToInt64(WorkOrderId);
            WS.RetrievePersonnelByAssetGroupMasterQuery(userData.DatabaseKey);
            return WS.TotalRecords;
        }
        #endregion
        #region Search
        public List<ResourceListSearchModel> GetResourceListGridData(long WorkOrderPlanId, string orderbycol = "", string orderDir = "", int skip = 0, int length = 0, string clientLookupId = "", string name = "", string Description = "", string RequiredDate = "",  string type = "", List<string> PersonnelList = null, string searchText = "")
        {

            ResourceListSearchModel resourceListSearchModel;
            List<ResourceListSearchModel> resourceListSearchModelList = new List<ResourceListSearchModel>();
            List<WorkOrderPlan> ResourceListing = new List<WorkOrderPlan>();
            WorkOrderPlan ResourceList = new WorkOrderPlan();
            //  ResourceList.CustomQueryDisplayId = CustomQueryDisplayId;
            ResourceList.ClientId = this.userData.DatabaseKey.Client.ClientId;
            ResourceList.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            ResourceList.WorkOrderPlanId = WorkOrderPlanId;
            ResourceList.OrderbyColumn = orderbycol;
            ResourceList.OrderBy = orderDir;
            ResourceList.OffSetVal = skip;
            ResourceList.NextRow = length;
            ResourceList.ClientLookupId = clientLookupId;
            ResourceList.ChargeTo_Name = name;
            ResourceList.Description = Description;           
            ResourceList.RequireDate = RequiredDate;
            ResourceList.Type = type;
            ResourceList.PersonnelList = PersonnelList != null && PersonnelList.Count > 0 ? string.Join(",", PersonnelList) : string.Empty;
            ResourceList.SearchText = searchText;
            ResourceListing = ResourceList.RetrieveResourceListChunkSearch(userData.DatabaseKey, userData.Site.TimeZone);

            foreach (var item in ResourceListing)
            {
                resourceListSearchModel = new ResourceListSearchModel();
                resourceListSearchModel.WorkOrderId = item.WorkOrderId;
                resourceListSearchModel.PersonnelName = item.PersonnelName;
                resourceListSearchModel.WorkOrderClientLookupId = item.WorkOrderClientLookupId;
                resourceListSearchModel.Description = item.Description;
                resourceListSearchModel.Type = item.Type;
                if (item.ScheduledStartDate != null && item.ScheduledStartDate == default(DateTime))
                {
                    resourceListSearchModel.ScheduledStartDate = null;
                }
                else
                {
                    resourceListSearchModel.ScheduledStartDate = item.ScheduledStartDate;
                }
                resourceListSearchModel.ScheduledHours = item.ScheduledHours;
                if (item.RequiredDate != null && item.RequiredDate == default(DateTime))
                {
                    resourceListSearchModel.RequiredDate = null;
                }
                else
                {
                    resourceListSearchModel.RequiredDate = item.RequiredDate;
                }
                resourceListSearchModel.EquipmentClientLookupId = item.EquipmentClientLookupId;
                resourceListSearchModel.ChargeTo_Name = item.ChargeTo_Name;

                resourceListSearchModel.TotalCount = item.TotalCount;
                resourceListSearchModel.WoStatus = item.Status;
                resourceListSearchModel.SumPersonnelHour = item.SumPersonnelHour;
                resourceListSearchModel.SumScheduledateHour = item.SumScheduledateHour;
                resourceListSearchModel.GrandTotalHour = item.GrandTotalHour;
                resourceListSearchModel.PerNextValue = item.PerNextValue;
                resourceListSearchModel.SDNextValue = item.SDNextValue;
                resourceListSearchModel.PerIDNextValue = item.PerIDNextValue;
                resourceListSearchModel.PersonnelId = item.PersonnelId;
                resourceListSearchModel.WorkOrderScheduleId = item.WorkOrderScheduleId;
                resourceListSearchModelList.Add(resourceListSearchModel);
            }

            return resourceListSearchModelList;
        }
        #endregion
    

        

        #region Add Reschedule 
        public WorkOrder AddReScheduleRecord(WoRescheduleModel wosm)
        {
            string PersonnelList = String.Empty;
            ProcessAlert objAlert = new ProcessAlert(this.userData);
            WorkOrder w = new WorkOrder();
            w.ClientId = userData.DatabaseKey.Client.ClientId;
            w.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            w.WorkOrderId = wosm.WorkOrderId;
            w.ScheduledDuration = wosm.ScheduledDuration;
            w.ScheduledStartDate = wosm.Schedulestartdate;
            CommonWrapper coWrapper = new CommonWrapper(userData);
            var namelist = coWrapper.MentionList("");
            List<UserMentionDataModel.UserMentionData> userMentionDataList = new List<UserMentionDataModel.UserMentionData>();
            UserMentionDataModel.UserMentionData objUserMentionData;
            List<long> nos = new List<long>() { wosm.WorkOrderId };
            if (wosm.PersonnelIds != null && wosm.PersonnelIds.Count > 0)
            {
                foreach (var item in wosm.PersonnelIds)
                {
                    PersonnelList += item + ",";
                    objUserMentionData = new UserMentionDataModel.UserMentionData();//new UserMentionData();
                    objUserMentionData.userId = Convert.ToInt64(item);
                    objUserMentionData.userName = namelist.Where(x => x.PersonnelId == Convert.ToInt64(item)).Select(y => y.UserName).FirstOrDefault();
                    objUserMentionData.emailId = namelist.Where(x => x.PersonnelId == Convert.ToInt64(item)).Select(y => y.Email).FirstOrDefault();
                    userMentionDataList.Add(objUserMentionData);
                }
            }
            List<long> userIds = new List<long>();
            var UserList = new List<Tuple<long, string, string>>();
            if (userMentionDataList != null && userMentionDataList.Count > 0)
            {
                foreach (var item in userMentionDataList)
                {
                    UserList.Add
                   (
                        Tuple.Create(Convert.ToInt64(item.userId), item.userName, item.emailId)
                  );
                }

            }

            w.PersonnelList = (!String.IsNullOrEmpty(PersonnelList)) ? PersonnelList.TrimEnd(',') : string.Empty;
            if ((wosm.PersonnelIds != null && wosm.PersonnelIds.Count > 0) && (wosm.Schedulestartdate != null && wosm.Schedulestartdate != default(DateTime)))
            {
                w.IsDeleteFlag = false;
            }
            else
            {
                w.IsDeleteFlag = true;
            }

            w.AddScheduleRecord(userData.DatabaseKey);
            objAlert.CreateAlert<DataContracts.WorkOrder>(AlertTypeEnum.WorkOrderAssigned, nos, UserList);
            return w;
        }

        #endregion
        #region Remove Schedule    
        public WorkOrderSchedule RemoveWorkOrderScheduleForResourceList(long WorkOrderID, long WorkOrderScheduledID)
        {
            WorkOrderSchedule workOrderSchedule = new WorkOrderSchedule();
            workOrderSchedule.ClientId = userData.DatabaseKey.Client.ClientId;
            workOrderSchedule.WorkOrderId = WorkOrderID;
            workOrderSchedule.WorkOrderSchedId = WorkOrderScheduledID;
            workOrderSchedule.RemoveWorkOrderScheduleForResourceList(userData.DatabaseKey);
            return workOrderSchedule;
        }
        #endregion
    }
}