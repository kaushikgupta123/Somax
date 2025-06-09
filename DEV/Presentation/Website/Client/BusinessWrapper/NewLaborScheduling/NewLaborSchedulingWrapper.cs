using Client.BusinessWrapper.Common;
using Client.Models.Common;
using Client.Models.NewLaborScheduling;

using Common.Constants;
using Common.Enumerations;
using Database.Business;

using DataContracts;
using DocumentFormat.OpenXml;


using INTDataLayer.BAL;
using INTDataLayer.EL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.BusinessWrapper.NewLaborScheduling
{
    public class NewLaborSchedulingWrapper
    {
        UserEL objUserEL = new UserEL();
        private DatabaseKey _dbKey;
        private UserData userData;
        public NewLaborSchedulingWrapper(UserData userData)
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
            WS.RetrievePersonnelByAssetGroupMasterQuery(userData.DatabaseKey);
            return WS.TotalRecords;
        }

       
        #endregion

        #region List view    

        #region Search
        public List<NewLaborSchedulingSearchModel> GetListlaborSchedulingGridData(int CustomQueryDisplayId, string orderbycol = "", string orderDir = "", int skip = 0, int length = 0, string clientLookupId = "", string name = "", string Description = "", string RequiredDate = "", string startScheduledDate = "", string endScheduledDate = "", string type = "", List<string> PersonnelList = null, string searchText = "")
        {

            NewLaborSchedulingSearchModel newLaborSchedulingSearchModel;
            List<NewLaborSchedulingSearchModel> newLaborSchedulingSearchModelList = new List<NewLaborSchedulingSearchModel>();
            List<WorkOrder> LaborSchedulingList = new List<WorkOrder>();
            WorkOrder LaborScheduling = new WorkOrder();
            LaborScheduling.CustomQueryDisplayId = CustomQueryDisplayId;
            LaborScheduling.ClientId = this.userData.DatabaseKey.Client.ClientId;
            LaborScheduling.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            LaborScheduling.OrderbyColumn = orderbycol;
            LaborScheduling.OrderBy = orderDir;
            LaborScheduling.OffSetVal = skip;
            LaborScheduling.NextRow = length;
            LaborScheduling.ClientLookupId = clientLookupId;
            LaborScheduling.ChargeTo_Name = name;
            LaborScheduling.Description = Description;
            LaborScheduling.ScheduledDateStart = startScheduledDate;
            LaborScheduling.ScheduledDateEnd = endScheduledDate;
            LaborScheduling.RequireDate = RequiredDate;
            LaborScheduling.Type = type;
            LaborScheduling.PersonnelList = PersonnelList != null && PersonnelList.Count > 0 ? string.Join(",", PersonnelList) : string.Empty;
            LaborScheduling.SearchText = searchText;
            LaborSchedulingList = LaborScheduling.RetrieveListForLaborSchedulingChunkSearch(userData.DatabaseKey, userData.Site.TimeZone);

            foreach (var item in LaborSchedulingList)
            {
                newLaborSchedulingSearchModel = new NewLaborSchedulingSearchModel();
                newLaborSchedulingSearchModel.WorkOrderId = item.WorkOrderId;
                newLaborSchedulingSearchModel.PersonnelName = item.PersonnelName;
                newLaborSchedulingSearchModel.WorkOrderClientLookupId = item.WorkOrderClientLookupId;
                newLaborSchedulingSearchModel.Description = item.Description;
                newLaborSchedulingSearchModel.Type = item.Type;
                if (item.ScheduledStartDate != null && item.ScheduledStartDate == default(DateTime))
                {
                    newLaborSchedulingSearchModel.ScheduledStartDate = null;
                }
                else
                {
                    newLaborSchedulingSearchModel.ScheduledStartDate = item.ScheduledStartDate;
                }
                newLaborSchedulingSearchModel.ScheduledHours = item.ScheduledHours;
                if (item.RequiredDate != null && item.RequiredDate == default(DateTime))
                {
                    newLaborSchedulingSearchModel.RequiredDate = null;
                }
                else
                {
                    newLaborSchedulingSearchModel.RequiredDate = item.RequiredDate;
                }
                newLaborSchedulingSearchModel.EquipmentClientLookupId = item.EquipmentClientLookupId;
                newLaborSchedulingSearchModel.ChargeTo_Name = item.ChargeTo_Name;

                newLaborSchedulingSearchModel.TotalCount = item.TotalCount;
                newLaborSchedulingSearchModel.WoStatus = item.Status;
                newLaborSchedulingSearchModel.SumPersonnelHour = item.SumPersonnelHour;
                newLaborSchedulingSearchModel.SumScheduledateHour = item.SumScheduledateHour;
                newLaborSchedulingSearchModel.GrandTotalHour = item.GrandTotalHour;
                newLaborSchedulingSearchModel.PerNextValue = item.PerNextValue;
                newLaborSchedulingSearchModel.SDNextValue = item.SDNextValue;
                newLaborSchedulingSearchModel.PerIDNextValue = item.PerIDNextValue;
                newLaborSchedulingSearchModel.PersonnelId = item.PersonnelId;
                newLaborSchedulingSearchModel.WorkOrderScheduleId = item.WorkOrderScheduleId;
                newLaborSchedulingSearchModel.PartsOnOrder = item.PartsOnOrder; //V2-838
                newLaborSchedulingSearchModelList.Add(newLaborSchedulingSearchModel);
            }

            return newLaborSchedulingSearchModelList;
        }
        #endregion
        #endregion

        #region Calendar view
        public List<NewLaborSchedulingCalendarModel> GetLaborSchedulingCalendarData(string CalendarDateStart, string CalendarDateEnd, List<string> PersonnelList = null)
        {
            NewLaborSchedulingCalendarModel newLaborSchedulingCalendarModel;
            List<NewLaborSchedulingCalendarModel> newLaborSchedulingCalendarModelList = new List<NewLaborSchedulingCalendarModel>();
            List<WorkOrder> LaborSchedulingList = new List<WorkOrder>();
            WorkOrder LaborScheduling = new WorkOrder();            
            LaborScheduling.ClientId = this.userData.DatabaseKey.Client.ClientId;
            LaborScheduling.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            LaborScheduling.CalendarDateStart = CalendarDateStart;
            LaborScheduling.CalendarDateEnd = CalendarDateEnd;
            LaborScheduling.PersonnelList = PersonnelList != null && PersonnelList.Count > 0 ? string.Join(",", PersonnelList) : string.Empty;

            LaborSchedulingList = LaborScheduling.RetrieveCalendarForLaborSchedulingChunkSearch(userData.DatabaseKey, userData.Site.TimeZone);

            foreach (var item in LaborSchedulingList)
            {
                newLaborSchedulingCalendarModel = new NewLaborSchedulingCalendarModel();
                newLaborSchedulingCalendarModel.WorkOrderClientLookupId = item.WorkOrderClientLookupId.Trim();
                newLaborSchedulingCalendarModel.Description = item.Description;
                if (item.Description.Length > 6)
                {
                    newLaborSchedulingCalendarModel.Title = item.WorkOrderClientLookupId + " " + item.Description.Substring(0, 6);
                }
                else
                {
                    newLaborSchedulingCalendarModel.Title = item.WorkOrderClientLookupId + " " + item.Description;
                }
                if (item.ScheduledStartDate != null && item.ScheduledStartDate == default(DateTime))
                {
                    newLaborSchedulingCalendarModel.ScheduledStartDate = "";
                }
                else
                {
                    newLaborSchedulingCalendarModel.ScheduledStartDate = item.ScheduledStartDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                }
                newLaborSchedulingCalendarModel.ScheduledHours = item.ScheduledHours;
                newLaborSchedulingCalendarModel.PersonnelFull = item.PersonnelFull;
                newLaborSchedulingCalendarModel.WorkOrderId = item.WorkOrderId;
                newLaborSchedulingCalendarModel.WorkOrderScheduleId = item.WorkOrderScheduleId;
                newLaborSchedulingCalendarModel.PersonnelId = item.PersonnelId;
                //V2-838
                newLaborSchedulingCalendarModel.PartOnOrder = item.PartsOnOrder; 
                if (item.Description.Length > 20)
                {
                    newLaborSchedulingCalendarModel.Tooltip = item.WorkOrderClientLookupId + " " + item.Description.Substring(0, 20);
                }
                else
                {
                    newLaborSchedulingCalendarModel.Tooltip = item.WorkOrderClientLookupId + " " + item.Description;
                }

                newLaborSchedulingCalendarModelList.Add(newLaborSchedulingCalendarModel);
            }

            return newLaborSchedulingCalendarModelList;
        }
        #endregion

        #region Add Reschedule V2-524
        public WorkOrder AddReScheduleRecord(WoRescheduleModel wosm)//--V2-524--//
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

        #region Reassign 
        public WorkOrder ReassignWorkOrderScheduleRecord(ReassignModel _reassignModel, ref List<string> NotAssignedWorkOrderClientLookupIdsList)
        {
            WorkOrder w = new WorkOrder();
            w.ClientId = userData.DatabaseKey.Client.ClientId;
            w.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            w.PersonnelId = _reassignModel.PersonnelId;            
            w.WorkOrderSchedIdsList = _reassignModel.WorkOrderSchedIds; 
            w.ReassignPersonnelScheduleRecord(userData.DatabaseKey);
            if (!string.IsNullOrEmpty(w.WorkOrderClientLookupIdsList))
            {
                var WorkOrderClientLookupIds = w.WorkOrderClientLookupIdsList.Split(',').ToList();
                if (WorkOrderClientLookupIds.Count > 0)
                {
                    foreach (var item in w.WorkOrderClientLookupIdsList.Split(',').ToList())
                    {
                        NotAssignedWorkOrderClientLookupIdsList.Add(item);
                    }
                       
                }
            }
            return w;
        }

        #endregion
        #region Remove Schedule 
        public WorkOrder RemoveScheduleRecord(long workorderId, ref string Statusmsg)//--V2-524--//
        {          
            WorkOrder w = new WorkOrder();
            w.ClientId = userData.DatabaseKey.Client.ClientId;
            w.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            w.WorkOrderId = workorderId;
            w.WorkOrderUpdateOnRemovingSchedule(this.userData.DatabaseKey, userData.Site.TimeZone);
            if (w.ErrorMessages.Count == 0)
            {
                Statusmsg = "success";
            }
            else
            {
                Statusmsg = "error";
            }

            return w;
        }
        #endregion

        #region Available Work

        public List<AvailableWoScheduleModel> GetAvailableWorklaborSchedulingGridData(string orderbycol = "", string orderDir = "", int skip = 0, int length = 0, string clientLookupId = "", string ChargeTo = "", string ChargeToName = "",
         string Description = "", List<string> Status = null, List<string> Priority = null, List<string> Type = null, List<string> Assigned = null, string _RequiredDate = null, string flag = "0")
        {

            AvailableWoScheduleModel newLaborSchedulingSearchModel;
            List<AvailableWoScheduleModel> newLaborSchedulingSearchModelList = new List<AvailableWoScheduleModel>();
            List<WorkOrder> LaborSchedulingList = new List<WorkOrder>();
            WorkOrder LaborScheduling = new WorkOrder();
            LaborScheduling.ClientId = this.userData.DatabaseKey.Client.ClientId;
            LaborScheduling.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            LaborScheduling.OrderbyColumn = orderbycol;
            LaborScheduling.OrderBy = orderDir;
            LaborScheduling.OffSetVal = skip;
            LaborScheduling.NextRow = length;
            LaborScheduling.ClientLookupId = clientLookupId;
            LaborScheduling.ChargeTo = ChargeTo;
            LaborScheduling.ChargeTo_Name = ChargeToName;
            LaborScheduling.Description = Description;
            //V2-984
            LaborScheduling.Status = string.Join(",", Status ?? new List<string>());
            LaborScheduling.Priority = string.Join("," ,Priority?? new List<string>());
            LaborScheduling.Type = string.Join("," ,Type??new List<string>());
            LaborScheduling.Assigned = string.Join(",", Assigned ?? new List<string>());
            LaborScheduling.RequireDate = _RequiredDate;
            LaborScheduling.ScheduleFlag = flag;
            LaborSchedulingList = LaborScheduling.RetrieveAvailableWorkForLaborSchedulingSearch(userData.DatabaseKey, userData.Site.TimeZone);
            List<LookupList> AllLookUps = new List<LookupList>();
            List<LookupList> PriorityData = new List<LookupList>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            AllLookUps = commonWrapper.GetAllLookUpList();

            if (AllLookUps != null)
            {
                PriorityData = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_Priority).ToList();
            }

            foreach (var item in LaborSchedulingList)
            {
                newLaborSchedulingSearchModel = new AvailableWoScheduleModel();
                newLaborSchedulingSearchModel.WorkOrderId = item.WorkOrderId;
                newLaborSchedulingSearchModel.ClientLookupId = item.WorkOrderClientLookupId;
                newLaborSchedulingSearchModel.ChargeTo = item.ChargeTo;
                newLaborSchedulingSearchModel.ChargeToName = item.ChargeTo_Name;
                newLaborSchedulingSearchModel.Description = item.Description;
                newLaborSchedulingSearchModel.Status = item.Status;
                newLaborSchedulingSearchModel.Priority = item.Priority;
                if (PriorityData != null && PriorityData.Any(cus => cus.ListValue == item.Priority))
                {
                    newLaborSchedulingSearchModel.Priority = PriorityData.Where(x => x.ListValue == item.Priority).Select(x => x.Description).First();
                }
                newLaborSchedulingSearchModel.Type = item.Type;
                if (item.RequiredDate != null && item.RequiredDate == default(DateTime))
                {
                    newLaborSchedulingSearchModel.RequiredDate = null;
                }
                else
                {
                    newLaborSchedulingSearchModel.RequiredDate = item.RequiredDate;
                }
                newLaborSchedulingSearchModel.TotalCount = item.TotalCount;
                newLaborSchedulingSearchModel.PartsOnOrder = item.PartsOnOrder; //V2-838
                //V2-984
                newLaborSchedulingSearchModel.WorkAssigned_PersonnelId = item.WorkAssigned_PersonnelId;

                newLaborSchedulingSearchModel.Assigned = item.Personnels;

                newLaborSchedulingSearchModelList.Add(newLaborSchedulingSearchModel);
            }

            return newLaborSchedulingSearchModelList;
        }
        public List<AvailableWoScheduleModel> PopulateLaborAvailable(string flag)
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

        public WorkOrder AddAvailableWorkAssign(AvailableWorkAssignModel awam)//--V2-524--//
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
            CreateEventLog(Convert.ToInt64(w.WorkOrderId), WorkOrderEvents.Scheduled);
            objAlert.CreateAlert<DataContracts.WorkOrder>(AlertTypeEnum.WorkOrderAssigned, nos, UserList);
            return w;
        }
        private void CreateEventLog(Int64 WOId, string Status)
        {
            WorkOrderEventLog log = new WorkOrderEventLog();
            log.ClientId = userData.DatabaseKey.Client.ClientId;
            log.SiteId = userData.DatabaseKey.Personnel.SiteId;
            log.WorkOrderId = WOId;
            log.Event = Status;
            log.TransactionDate = DateTime.UtcNow;
            log.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            log.Comments = "";
            log.SourceId = 0;
            log.Create(userData.DatabaseKey);
        }
        #endregion

        #region Add schedule calendar
        public List<SelectListItem> RetrieveApprovedWorkOrderForLaborScheduling()
        {
            List<SelectListItem> WorkOrderList = new List<SelectListItem>();
            SelectListItem listdata;
            List<WorkOrder> LaborSchedulingList = new List<WorkOrder>();
            WorkOrder LaborScheduling = new WorkOrder();
            LaborScheduling.ClientId = this.userData.DatabaseKey.Client.ClientId;
            LaborScheduling.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            LaborSchedulingList = LaborScheduling.RetrieveApprovedWorkOrderForLaborScheduling(userData.DatabaseKey, userData.Site.TimeZone);

            foreach (var item in LaborSchedulingList)
            {
                listdata = new SelectListItem();
                listdata.Value = item.WorkOrderId.ToString();
                if (item.Description.Length > 20)
                {
                    listdata.Text = item.ClientLookupId + " " + item.Description.Substring(0, 20);
                }
                else
                {
                    listdata.Text = item.ClientLookupId + " " + item.Description;
                }
                WorkOrderList.Add(listdata);
            }

            return WorkOrderList;
        }
        //public WorkOrder AddScheduleCalendar(WoRescheduleModel wosm)
        //{
        //    // bool isSuccess = true;
        //    string PersonnelList = String.Empty;
        //    ProcessAlert objAlert = new ProcessAlert(this.userData);
        //    WorkOrder w = new WorkOrder();
        //    w.ClientId = userData.DatabaseKey.Client.ClientId;
        //    w.SiteId = userData.DatabaseKey.User.DefaultSiteId;
        //    w.WorkOrderId = wosm.WorkOrderId;
        //    w.ScheduledDuration = wosm.ScheduledDuration;
        //    w.ScheduledStartDate = wosm.Schedulestartdate;


        //    CommonWrapper coWrapper = new CommonWrapper(userData);
        //    var namelist = coWrapper.MentionList("");
        //    List<UserMentionDataModel.UserMentionData> userMentionDataList = new List<UserMentionDataModel.UserMentionData>();
        //    UserMentionDataModel.UserMentionData objUserMentionData;
        //    List<long> nos = new List<long>() { wosm.WorkOrderId };
        //    if (wosm.PersonnelIds != null && wosm.PersonnelIds.Count > 0)
        //    {
        //        foreach (var item in wosm.PersonnelIds)
        //        {
        //            PersonnelList += item + ",";
        //            objUserMentionData = new UserMentionDataModel.UserMentionData();//new UserMentionData();
        //            objUserMentionData.userId = Convert.ToInt64(item);
        //            objUserMentionData.userName = namelist.Where(x => x.PersonnelId == Convert.ToInt64(item)).Select(y => y.UserName).FirstOrDefault();
        //            objUserMentionData.emailId = namelist.Where(x => x.PersonnelId == Convert.ToInt64(item)).Select(y => y.Email).FirstOrDefault();
        //            userMentionDataList.Add(objUserMentionData);
        //        }
        //    }
        //    List<long> userIds = new List<long>();
        //    var UserList = new List<Tuple<long, string, string>>();
        //    if (userMentionDataList != null && userMentionDataList.Count > 0)
        //    {
        //        foreach (var item in userMentionDataList)
        //        {
        //            UserList.Add
        //           (
        //                Tuple.Create(Convert.ToInt64(item.userId), item.userName, item.emailId)
        //          );
        //        }

        //    }

        //    w.PersonnelList = (!String.IsNullOrEmpty(PersonnelList)) ? PersonnelList.TrimEnd(',') : string.Empty;
        //    if ((wosm.PersonnelIds != null && wosm.PersonnelIds.Count > 0) && (wosm.Schedulestartdate != null && wosm.Schedulestartdate != default(DateTime)))
        //    {
        //        w.IsDeleteFlag = false;
        //    }
        //    else
        //    {
        //        w.IsDeleteFlag = true;
        //    }

        //    w.AddScheduleRecord(userData.DatabaseKey);
        //    objAlert.CreateAlert<DataContracts.WorkOrder>(AlertTypeEnum.WorkOrderAssign, nos, UserList);
        //    return w;
        //}
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
        public WorkOrderSchedule UpdateWorkOrderSchedule(EditSchedlingCalendarModal editSchedlingCalendarModal)
        {
            WorkOrderSchedule workOrderSchedule = new WorkOrderSchedule();
            workOrderSchedule.WorkOrderSchedId = editSchedlingCalendarModal.WorkOrderID;
            workOrderSchedule.WorkOrderSchedId = editSchedlingCalendarModal.WorkOrderScheduledID;
            workOrderSchedule.ClientId = userData.DatabaseKey.Client.ClientId;
            workOrderSchedule.Retrieve(_dbKey);
            workOrderSchedule.ScheduledHours = editSchedlingCalendarModal.Hours ?? 0;
            workOrderSchedule.Update(userData.DatabaseKey);
            return workOrderSchedule;
        }
        public WorkOrderSchedule RemoveWorkOrderScheduleForLaborScheduling(long WorkOrderID, long WorkOrderScheduledID)
        {
            WorkOrderSchedule workOrderSchedule = new WorkOrderSchedule();
            workOrderSchedule.ClientId = userData.DatabaseKey.Client.ClientId;
            workOrderSchedule.WorkOrderId = WorkOrderID;
            workOrderSchedule.WorkOrderSchedId = WorkOrderScheduledID;
            workOrderSchedule.RemoveWorkOrderScheduleForLaborScheduling(userData.DatabaseKey);
            if (workOrderSchedule.ErrorMessages ==null || workOrderSchedule.ErrorMessages.Count == 0)
            {               
                CreateEventLog(WorkOrderID, WorkOrderEvents.RemoveSchedule);//V2-1183
            }
            return workOrderSchedule;
        }
        #endregion
        #region Update Scheduling Records
        internal string UpdateSchedulingRecords(long WorkOrderSchedId, decimal Hours)
        {
            LaborSchedulingBAL laborSchedulingBAL = new LaborSchedulingBAL();
            string Responcetxt = string.Empty;
            WorkOrderScheduleEL objWorkOrderScheduleEL = new WorkOrderScheduleEL();
            objWorkOrderScheduleEL.WorkOrderSchedId = WorkOrderSchedId;
            objWorkOrderScheduleEL.ClientId = this.userData.DatabaseKey.User.ClientId;
            objWorkOrderScheduleEL.ModifyBy = this.userData.DatabaseKey.UserName;
            objWorkOrderScheduleEL.ModifyDate = DateTime.UtcNow;
            objWorkOrderScheduleEL.ScheduledHours = Hours;
            laborSchedulingBAL.UpdateScheduleRecord(objWorkOrderScheduleEL, out Responcetxt, this.userData.DatabaseKey.ClientConnectionString);
            return Responcetxt;
        }
        #endregion
        #region Update Scheduling Records V2-562
        internal string UpdateSchedulingRecords_V2(long WorkOrderSchedId, decimal Hours,DateTime ScheduleDate)
        {
            LaborSchedulingBAL laborSchedulingBAL = new LaborSchedulingBAL();
            string Responcetxt = string.Empty;
            WorkOrderScheduleEL objWorkOrderScheduleEL = new WorkOrderScheduleEL();
            objWorkOrderScheduleEL.WorkOrderSchedId = WorkOrderSchedId;
            objWorkOrderScheduleEL.ClientId = this.userData.DatabaseKey.User.ClientId;
            objWorkOrderScheduleEL.ModifyBy = this.userData.DatabaseKey.UserName;
            objWorkOrderScheduleEL.ModifyDate = DateTime.UtcNow;
            objWorkOrderScheduleEL.ScheduledHours = Hours;
            objWorkOrderScheduleEL.ScheduledStartDate = ScheduleDate;
            laborSchedulingBAL.UpdateScheduleRecord_V2(objWorkOrderScheduleEL, out Responcetxt, this.userData.DatabaseKey.ClientConnectionString);
            return Responcetxt;
        }
        #endregion

        #region Available work calendar
        public List<AvailableWoScheduleModel> GetAvailableWorklaborSchedulingGridDataCalendar(string orderbycol = "", string orderDir = "", int skip = 0, int length = 0, string clientLookupId = "", string ChargeTo = "", string ChargeToName = "",
        string Description = "", List<string> Status = null, List<string> Priority = null, List<string> Type = null,List<string> Assigned=null, string _RequiredDate = null, string flag = "0")
        {

            AvailableWoScheduleModel newLaborSchedulingSearchModel;
            List<AvailableWoScheduleModel> newLaborSchedulingSearchModelList = new List<AvailableWoScheduleModel>();
            List<WorkOrder> LaborSchedulingList = new List<WorkOrder>();
            WorkOrder LaborScheduling = new WorkOrder();
            LaborScheduling.ClientId = this.userData.DatabaseKey.Client.ClientId;
            LaborScheduling.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            LaborScheduling.OrderbyColumn = orderbycol;
            LaborScheduling.OrderBy = orderDir;
            LaborScheduling.OffSetVal = skip;
            LaborScheduling.NextRow = length;
            LaborScheduling.ClientLookupId = clientLookupId;
            LaborScheduling.ChargeTo = ChargeTo;
            LaborScheduling.ChargeTo_Name = ChargeToName;
            LaborScheduling.Description = Description;
            //V2-984
            LaborScheduling.Status = string.Join(",", Status ?? new List<string>());
            LaborScheduling.Priority = string.Join(",", Priority ?? new List<string>());
            LaborScheduling.Type = string.Join(",", Type ?? new List<string>());
            LaborScheduling.Assigned = string.Join(",", Assigned ?? new List<string>());
            LaborScheduling.RequireDate = _RequiredDate;
            LaborScheduling.ScheduleFlag = flag;
            LaborSchedulingList = LaborScheduling.RetrieveAvailableWorkForLaborSchedulingSearchCalendar(userData.DatabaseKey, userData.Site.TimeZone);
            //V2-984
            List<LookupList> AllLookUps = new List<LookupList>();
            List<LookupList> PriorityData = new List<LookupList>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            AllLookUps = commonWrapper.GetAllLookUpList();

            if (AllLookUps != null)
            {
                PriorityData = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_Priority).ToList();
            }
            foreach (var item in LaborSchedulingList)
            {
                newLaborSchedulingSearchModel = new AvailableWoScheduleModel();
                newLaborSchedulingSearchModel.WorkOrderId = item.WorkOrderId;
                newLaborSchedulingSearchModel.ClientLookupId = item.WorkOrderClientLookupId;
                newLaborSchedulingSearchModel.ChargeTo = item.ChargeTo;
                newLaborSchedulingSearchModel.ChargeToName = item.ChargeTo_Name;
                newLaborSchedulingSearchModel.Description = item.Description;
                newLaborSchedulingSearchModel.Status = item.Status;
                newLaborSchedulingSearchModel.Priority = item.Priority;
                if (PriorityData != null&& PriorityData.Any(x =>x.ListValue == item.Priority))
                {
                    newLaborSchedulingSearchModel.Priority=PriorityData.Where(x=>x.ListValue==item.Priority).Select(x=>x.Description).First();
                }
                newLaborSchedulingSearchModel.Type = item.Type;
                if (item.RequiredDate != null && item.RequiredDate == default(DateTime))
                {
                    newLaborSchedulingSearchModel.RequiredDate = null;
                }
                else
                {
                    newLaborSchedulingSearchModel.RequiredDate = item.RequiredDate;
                }
                newLaborSchedulingSearchModel.TotalCount = item.TotalCount;
                newLaborSchedulingSearchModel.PartsOnOrder = item.PartsOnOrder; //V2-838
                //V2-984
                newLaborSchedulingSearchModel.WorkAssigned_PersonnelId = item.WorkAssigned_PersonnelId;

                newLaborSchedulingSearchModel.Assigned = item.Personnels;
                
                newLaborSchedulingSearchModelList.Add(newLaborSchedulingSearchModel);
            }

            return newLaborSchedulingSearchModelList;
        }
        public WorkOrder AddAvailableWorkAssignCalendar(AvailableWorkAssignModel awam)//--V2-524--//
        {
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
            if ((awam.PersonnelIds != null && awam.PersonnelIds.Count > 0) && (awam.Schedulestartdate != null && awam.Schedulestartdate != default(DateTime)))
            {
                w.IsDeleteFlag = false;
            }
            else
            {
                w.IsDeleteFlag = true;
            }

            w.AddScheduleRecord(userData.DatabaseKey);
            CreateEventLog(Convert.ToInt64(w.WorkOrderId), WorkOrderEvents.Scheduled);
            objAlert.CreateAlert<DataContracts.WorkOrder>(AlertTypeEnum.WorkOrderAssigned, nos, UserList);
            return w;
        }
        #endregion

        #region drag schedule calendar        
        public WorkOrderSchedule DragWorkOrderScheduleFromCalendar(long WorkOrderID, long WorkOrderScheduledID, DateTime ScheduledStartDate)
        {
            WorkOrderSchedule workOrderSchedule = new WorkOrderSchedule();
            workOrderSchedule.ClientId = userData.DatabaseKey.Client.ClientId;
            workOrderSchedule.WorkOrderId = WorkOrderID;
            workOrderSchedule.WorkOrderSchedId = WorkOrderScheduledID;
            workOrderSchedule.ScheduledStartDate = ScheduledStartDate;
            workOrderSchedule.DragWorkOrderScheduleForLaborScheduling(userData.DatabaseKey);
            return workOrderSchedule;
        }
        #endregion


    }
}