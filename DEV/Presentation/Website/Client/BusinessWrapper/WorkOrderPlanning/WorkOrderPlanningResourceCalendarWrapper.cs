using Client.BusinessWrapper.Common;
using Client.Models.Common;
using Client.Models.WorkOrderPlanning;
using Common.Enumerations;
using DataContracts;
using INTDataLayer.BAL;
using INTDataLayer.EL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace Client.BusinessWrapper.WorkOrderPlanning
{
    public class WorkOrderPlanningResourceCalendarWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        public WorkOrderPlanningResourceCalendarWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
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

        #region Calendar data
        public List<WorkOrderPlanningResourceCalendar> GetResourceCalendarData(string CalendarDateStart, string CalendarDateEnd, long WorkOrderPlanId,
            List<string> PersonnelList = null)
        {
            WorkOrderPlanningResourceCalendar CalendarModel;
            List<WorkOrderPlanningResourceCalendar> CalendarList = new List<WorkOrderPlanningResourceCalendar>();
            List<WorkOrderPlan> WorkOrderPlanList = new List<WorkOrderPlan>();
            WorkOrderPlan workOrderPlan = new WorkOrderPlan();
            workOrderPlan.ClientId = this.userData.DatabaseKey.Client.ClientId;
            workOrderPlan.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            workOrderPlan.ScheduledDateStart = CalendarDateStart;
            workOrderPlan.ScheduledDateEnd = CalendarDateEnd;
            workOrderPlan.PersonnelList = PersonnelList != null && PersonnelList.Count > 0 ? string.Join(",", PersonnelList) : string.Empty;
            workOrderPlan.WorkOrderPlanId = WorkOrderPlanId;

            WorkOrderPlanList = workOrderPlan.RetrieveCalendarForLaborSchedulingChunkSearch(userData.DatabaseKey, userData.Site.TimeZone);

            foreach (var item in WorkOrderPlanList)
            {
                CalendarModel = new WorkOrderPlanningResourceCalendar();
                CalendarModel.WorkOrderClientLookupId = item.WorkOrderClientLookupId.Trim();
                CalendarModel.Description = item.Description;
                if (item.Description.Length > 20)
                {
                    CalendarModel.Title = item.WorkOrderClientLookupId + " " + item.Description.Substring(0, 20);
                }
                else
                {
                    CalendarModel.Title = item.WorkOrderClientLookupId + " " + item.Description;
                }
                if (item.ScheduledStartDate != null && item.ScheduledStartDate == default(DateTime))
                {
                    CalendarModel.ScheduledStartDate = "";
                }
                else
                {
                    CalendarModel.ScheduledStartDate = item.ScheduledStartDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                }
                CalendarModel.ScheduledHours = item.ScheduledHours;
                CalendarModel.PersonnelFull = item.PersonnelName;
                CalendarModel.WorkOrderId = item.WorkOrderId;
                CalendarModel.WorkOrderScheduleId = item.WorkOrderScheduleId;
                CalendarModel.PersonnelId = item.PersonnelId;
                CalendarList.Add(CalendarModel);
            }
            return CalendarList;
        }
        #endregion        

        #region Add schedule calendar
        public List<SelectListItem> RetrieveWorkOrderListForAddScheduling(long WorkOrderPlanId)
        {
            List<SelectListItem> WorkOrderList = new List<SelectListItem>();
            SelectListItem listdata;
            List<WorkOrderPlan> WorkOrderPlanList = new List<WorkOrderPlan>();
            WorkOrderPlan workOrderPlan = new WorkOrderPlan();
            workOrderPlan.ClientId = this.userData.DatabaseKey.Client.ClientId;
            workOrderPlan.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            workOrderPlan.WorkOrderPlanId = WorkOrderPlanId;
            WorkOrderPlanList = workOrderPlan.RetrieveWOForScheduleCalendar(userData.DatabaseKey, userData.Site.TimeZone);

            foreach (var item in WorkOrderPlanList)
            {
                listdata = new SelectListItem();
                listdata.Value = item.WorkOrderId.ToString();
                if (item.Description.Length > 20)
                {
                    listdata.Text = item.WorkOrderClientLookupId + " " + item.Description.Substring(0, 20);
                }
                else
                {
                    listdata.Text = item.WorkOrderClientLookupId + " " + item.Description;
                }
                WorkOrderList.Add(listdata);
            }

            return WorkOrderList;
        }
        public WorkOrderPlan AddScheduleRecord(ResourceCalendarAddScheduleModel addScheduleModel)//--V2-524--//
        {
            string PersonnelList = String.Empty;
            ProcessAlert objAlert = new ProcessAlert(this.userData);
            WorkOrderPlan w = new WorkOrderPlan()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                WorkOrderId = addScheduleModel.WorkOrderId,
                ScheduledHours = addScheduleModel.ScheduledDuration ?? 0,
                ScheduledStartDate = addScheduleModel.Schedulestartdate,
            };
            CommonWrapper coWrapper = new CommonWrapper(userData);
            var namelist = coWrapper.MentionList("");
            List<UserMentionDataModel.UserMentionData> userMentionDataList = new List<UserMentionDataModel.UserMentionData>();
            UserMentionDataModel.UserMentionData objUserMentionData;
            List<long> nos = new List<long>() { addScheduleModel.WorkOrderId };
            if (addScheduleModel.PersonnelIds != null && addScheduleModel.PersonnelIds.Length > 0)
            {
                foreach (var item in addScheduleModel.PersonnelIds)
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
            if ((addScheduleModel.PersonnelIds != null && addScheduleModel.PersonnelIds.Length > 0) && 
                (addScheduleModel.Schedulestartdate != null && addScheduleModel.Schedulestartdate != default(DateTime)))
            {
                w.IsDeleteFlag = false;
            }
            else
            {
                w.IsDeleteFlag = true;
            }

            w.AddScheduleRecord(userData.DatabaseKey);
            objAlert.CreateAlert<DataContracts.WorkOrder>(AlertTypeEnum.WorkOrderAssign, nos, UserList);
            return w;
        }
        #endregion

        #region Edit schedule calendar
        public WorkOrderSchedule RetrieveWorkOrderSchedule(long WorkOrderId, long WorkOrderSchedId)
        {
            WorkOrderSchedule workOrderSchedule = new WorkOrderSchedule();
            workOrderSchedule.WorkOrderId = WorkOrderId;
            workOrderSchedule.WorkOrderSchedId = WorkOrderSchedId;
            workOrderSchedule.ClientId = userData.DatabaseKey.Client.ClientId;
            workOrderSchedule.RetrieveByWorkOrderIdAndSchdeuleId(userData.DatabaseKey);
            return workOrderSchedule;
        }
        public WorkOrderPlan UpdateWorkOrderSchedule(ResourceCalendarEditScheduleModel editSchedlingCalendarModal)
        {
            WorkOrderPlan workOrderPlan = new WorkOrderPlan();
            workOrderPlan.WorkOrderId = editSchedlingCalendarModal.WorkOrderID;
            workOrderPlan.WorkOrderScheduleId = editSchedlingCalendarModal.WorkOrderScheduledID;
            workOrderPlan.ClientId = userData.DatabaseKey.Client.ClientId;            
            workOrderPlan.ScheduledHours = editSchedlingCalendarModal.Hours ?? 0;
            workOrderPlan.ScheduledStartDate = editSchedlingCalendarModal.ScheduleDate;
            workOrderPlan.UpdateScheduleRecord(userData.DatabaseKey);
            return workOrderPlan;
        }
        public WorkOrderPlan RemoveWorkOrderSchedule(long WorkOrderID, long WorkOrderScheduledID)
        {
            WorkOrderPlan workOrderPlan = new WorkOrderPlan();
            workOrderPlan.ClientId = userData.DatabaseKey.Client.ClientId;
            workOrderPlan.WorkOrderId = WorkOrderID;
            workOrderPlan.WorkOrderScheduleId = WorkOrderScheduledID;
            workOrderPlan.RemoveScheduleRecord(userData.DatabaseKey);
            return workOrderPlan;
        }
        #endregion       

        #region drag schedule calendar        
        public WorkOrderPlan DragWorkOrderScheduleFromCalendar(long WorkOrderID, long WorkOrderScheduledID, DateTime ScheduledStartDate)
        {
            WorkOrderPlan workOrderPlan = new WorkOrderPlan();
            workOrderPlan.ClientId = userData.DatabaseKey.Client.ClientId;
            workOrderPlan.WorkOrderId = WorkOrderID;
            workOrderPlan.WorkOrderScheduleId = WorkOrderScheduledID;
            workOrderPlan.ScheduledStartDate = ScheduledStartDate;
            workOrderPlan.DragWorkOrderScheduleFromCalendar(userData.DatabaseKey);
            return workOrderPlan;
        }
        #endregion
    }
}