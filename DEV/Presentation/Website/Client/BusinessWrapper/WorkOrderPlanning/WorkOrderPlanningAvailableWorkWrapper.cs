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
    public class WorkOrderPlanningAvailableWorkWrapper
    {
        UserEL objUserEL = new UserEL();
        private DatabaseKey _dbKey;
        private UserData userData;
        public WorkOrderPlanningAvailableWorkWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
            objUserEL.UserInfoId = userData.DatabaseKey.User.UserInfoId;
            objUserEL.UserFullName = userData.DatabaseKey.UserName;
            objUserEL.ClientId = userData.DatabaseKey.Client.ClientId;
            objUserEL.SiteId = userData.Site.SiteId;
        }
        #region Available Work

        public List<AvailableWoScheduleModel> GetAvailableWorkResourceListSchedulingGridData(string orderbycol = "", string orderDir = "", int skip = 0, int length = 0,long WorkOrderPlanId=0, string clientLookupId = "", string ChargeTo = "", string ChargeToName = "",
         string Description = "", string Status = "", string Priority = "", string Type = "", string flag = "0", string searchText = "")
        {

            AvailableWoScheduleModel newAvailableWoScheduleModel;
            List<AvailableWoScheduleModel> newAvailableWoScheduleModelList = new List<AvailableWoScheduleModel>();
            List<WorkOrderPlan> ResourceListAWO = new List<WorkOrderPlan>();
            WorkOrderPlan ResourceAWO = new WorkOrderPlan();
            ResourceAWO.ClientId = this.userData.DatabaseKey.Client.ClientId;
            ResourceAWO.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            ResourceAWO.WorkOrderPlanId = WorkOrderPlanId;
            ResourceAWO.OrderbyColumn = orderbycol;
            ResourceAWO.OrderBy = orderDir;
            ResourceAWO.OffSetVal = skip;
            ResourceAWO.NextRow = length;
            ResourceAWO.ClientLookupId = clientLookupId;
            ResourceAWO.ChargeTo = ChargeTo;
            ResourceAWO.ChargeTo_Name = ChargeToName;
            ResourceAWO.Description = Description;
            ResourceAWO.Status = Status;
            ResourceAWO.Priority = Priority;
            ResourceAWO.Type = Type;
            // LaborScheduling.RequiredDate = RequiredDate;
           // ResourceAWO.ScheduleFlag = flag;
            ResourceAWO.SearchText = searchText;
            ResourceListAWO = ResourceAWO.RetrieveAvailableWorkSearch(userData.DatabaseKey, userData.Site.TimeZone);

            foreach (var item in ResourceListAWO)
            {
                newAvailableWoScheduleModel = new AvailableWoScheduleModel();
                newAvailableWoScheduleModel.WorkOrderId = item.WorkOrderId;
                newAvailableWoScheduleModel.ClientLookupId = item.WorkOrderClientLookupId;
                newAvailableWoScheduleModel.ChargeTo = item.ChargeTo;
                newAvailableWoScheduleModel.ChargeToName = item.ChargeTo_Name;
                newAvailableWoScheduleModel.Description = item.Description;
                newAvailableWoScheduleModel.Status = item.Status;
                newAvailableWoScheduleModel.Priority = item.Priority;
                newAvailableWoScheduleModel.Type = item.Type;
                newAvailableWoScheduleModel.Duration= item.ScheduledDuration;
                if (item.RequiredDate != null && item.RequiredDate == default(DateTime))
                {
                    newAvailableWoScheduleModel.RequiredDate = null;
                }
                else
                {
                    newAvailableWoScheduleModel.RequiredDate = item.RequiredDate;
                }
                newAvailableWoScheduleModel.TotalCount = item.TotalCount;

                newAvailableWoScheduleModelList.Add(newAvailableWoScheduleModel);
            }

            return newAvailableWoScheduleModelList;
        }
        public List<AvailableWoScheduleModel> PopulateResourceListAvailable(string flag)
        {
            LaborSchedulingBAL objLaborAvailable = new LaborSchedulingBAL();
            List<AvailableWoScheduleModel> LabourAvailableList = new List<AvailableWoScheduleModel>();
            DataTable AvailWO = new DataTable();
            AvailWO = objLaborAvailable.GetWorkOrderSearchCriteria(objUserEL, Convert.ToString(flag), this.userData.DatabaseKey.AdminConnectionString);
            if (AvailWO != null && AvailWO.Rows.Count > 0)
            {
                for (int i = 0; i < AvailWO.Rows.Count; i++)
                {
                    var RequiredDate = AvailWO.Rows[i]["RequiredDate"];
                    var Duration = AvailWO.Rows[i]["ScheduledDuration"];
                    var StartDate = AvailWO.Rows[i]["ScheduledStartDate"];
                    AvailableWoScheduleModel objLAM = new AvailableWoScheduleModel();
                    objLAM.ClientLookupId = AvailWO.Rows[i]["ClientLookupId"].ToString();
                    objLAM.ChargeTo = AvailWO.Rows[i]["ChargeTo"].ToString();
                    objLAM.ChargeToName = AvailWO.Rows[i]["ChargeToName"].ToString();
                    objLAM.Description = AvailWO.Rows[i]["Description"].ToString();
                    objLAM.Status = AvailWO.Rows[i]["Status"].ToString();
                    objLAM.Priority = AvailWO.Rows[i]["Priority"].ToString();
                    objLAM.Type = AvailWO.Rows[i]["Type"].ToString();
                    objLAM.WorkOrderId = Convert.ToInt64(AvailWO.Rows[i]["WorkOrderId"].ToString());
                    if ((RequiredDate != null) && (Convert.ToString(RequiredDate) != "") && Convert.ToDateTime(Convert.ToString(RequiredDate)) != default(DateTime))
                    {
                        objLAM.RequiredDate = Convert.ToDateTime(RequiredDate);
                    }
                    else
                    {
                        objLAM.RequiredDate = null;
                    }
                    LabourAvailableList.Add(objLAM);
                }
            }
            return LabourAvailableList;
        }

        public WorkOrder AddAvailableWorkAssign(AvailableWorkAssignModel awam)
        {
            // bool isSuccess = true;
            string PersonnelList = String.Empty;
            ProcessAlert objAlert = new ProcessAlert(this.userData);
            WorkOrder w = new WorkOrder();
            w.ClientId = userData.DatabaseKey.Client.ClientId;
            w.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            w.WorkOrderId = awam.WorkOrderId;
            w.ScheduledDuration = awam.ScheduledDuration;
            w.ScheduledStartDate = awam.Schedulestartdate;
            CommonWrapper coWrapper = new CommonWrapper(userData);
            var namelist = coWrapper.MentionList("");
            List<UserMentionDataModel.UserMentionData> userMentionDataList = new List<UserMentionDataModel.UserMentionData>();
            UserMentionDataModel.UserMentionData objUserMentionData;
            List<long> nos = new List<long>() { awam.WorkOrderId };
            if (awam.PersonnelIds != null && awam.PersonnelIds.Count > 0)
            {
                foreach (var item in awam.PersonnelIds)
                {
                    PersonnelList += item + ",";
                    objUserMentionData = new UserMentionDataModel.UserMentionData();
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
            if ((awam.PersonnelIds != null && awam.PersonnelIds.Count > 0) && (awam.Schedulestartdate != null && awam.Schedulestartdate != default(DateTime)))
            {
                w.IsDeleteFlag = false;
            }
            else
            {
                w.IsDeleteFlag = true;
            }

            w.AddScheduleRecord(userData.DatabaseKey);
            //CreateEventLog(Convert.ToInt64(w.WorkOrderId), WorkOrderEvents.Scheduled);
            objAlert.CreateAlert<DataContracts.WorkOrder>(AlertTypeEnum.WorkOrderAssigned, nos, UserList);
            return w;
        }
       
        #endregion
    }
}