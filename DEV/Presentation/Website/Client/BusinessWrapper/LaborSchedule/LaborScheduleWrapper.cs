using Client.Models.LaborScheduling;
using Common.Enumerations;
using DataContracts;
using INTDataLayer.BAL;
using INTDataLayer.EL;
using System;
using System.Collections.Generic;
using System.Data;
using Common.Constants;
using Client.BusinessWrapper.Common;
using System.Linq;

namespace Client.BusinessWrapper.LaborSchedule
{
    public class LaborScheduleWrapper
    {
        LaborSchedulingBAL objLaborScheduling = new LaborSchedulingBAL();
        UserEL objUserEL = new UserEL();
        private DatabaseKey _dbKey;
        private UserData userData;
        public LaborScheduleWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;

            objUserEL.UserInfoId = userData.DatabaseKey.User.UserInfoId;
            objUserEL.UserFullName = userData.DatabaseKey.UserName;
            objUserEL.ClientId = userData.DatabaseKey.Client.ClientId;
            objUserEL.SiteId = userData.Site.SiteId;
        }
        #region Scheduling Labor
        public List<LaborSchedulingModel> PopulateLaborScheduling(string PersonnelId, string strDate, string flag)
        {
            LaborSchedulingBAL objLaborScheduling = new LaborSchedulingBAL();
            List<LaborSchedulingModel> LabourSchedulingList = new List<LaborSchedulingModel>();
            DataTable AvailWO = new DataTable();
            AvailWO = objLaborScheduling.GetScheduledWorkOrderSearchCriteriaV2(objUserEL,userData.DatabaseKey.Client.ClientId, this.userData.Site.SiteId, Convert.ToInt64(PersonnelId), strDate, Convert.ToInt32(flag), this.userData.DatabaseKey.AdminConnectionString);
            if (AvailWO != null && AvailWO.Rows.Count > 0)
            {
                for (int i = 0; i < AvailWO.Rows.Count; i++)
                {
                    LaborSchedulingModel objLSM = new LaborSchedulingModel();
                    var dtime = AvailWO.Rows[i]["RequiredDate"];
                    var sHours = AvailWO.Rows[i]["ScheduledHours"];
                    if ((dtime != null) && (Convert.ToString(dtime) != "") && Convert.ToDateTime(Convert.ToString(dtime)) != default(DateTime))
                    {
                        objLSM.Date = Convert.ToDateTime(AvailWO.Rows[i]["RequiredDate"]);
                    }
                    else
                    {
                        objLSM.Date = null;
                    }
                    objLSM.Status = AvailWO.Rows[i]["Status"].ToString();

                    if ((sHours != null) && (Convert.ToString(sHours) != ""))
                    {
                        objLSM.Hours = Convert.ToDecimal(AvailWO.Rows[i]["ScheduledHours"].ToString());
                    }
                    else
                    {
                        objLSM.Hours = Convert.ToDecimal("0.00");
                    }
                    objLSM.Type = AvailWO.Rows[i]["Type"].ToString();
                    objLSM.Description = AvailWO.Rows[i]["Description"].ToString();
                    objLSM.ClientLookupId = AvailWO.Rows[i]["ClientLookupId"].ToString();
                    objLSM.WorkOrderSchedId = Convert.ToUInt32(AvailWO.Rows[i]["WorkOrderSchedId"].ToString());
                    objLSM.PartsOnOrder = Convert.ToUInt16(AvailWO.Rows[i]["PartsOnOrder"].ToString()); //V2-838
                    LabourSchedulingList.Add(objLSM);
                }
            }
            return LabourSchedulingList;
        }
        public List<LaborSchedulingModel> PopulateLaborSchedulingAfterRemove(string[] WorkSchedlIds, string PersonnelId, DateTime strDate, string flag)
        {
            LaborSchedulingBAL objLaborScheduling = new LaborSchedulingBAL();
            String str = "";
            foreach (var item in WorkSchedlIds)
            {
                WorkOrderScheduleEL objWorkOrderScheduleEL = new WorkOrderScheduleEL();
                objWorkOrderScheduleEL.PersonnelId = Convert.ToInt64(PersonnelId);
                objWorkOrderScheduleEL.ClientId = this.userData.DatabaseKey.User.ClientId;
                objWorkOrderScheduleEL.ModifyBy = this.userData.DatabaseKey.UserName;
                objWorkOrderScheduleEL.ModifyDate = DateTime.UtcNow;
                objWorkOrderScheduleEL.ScheduledStartDate = strDate;
                Int32 intResponse = objLaborScheduling.RemoveScheduleRecordWO(item, objWorkOrderScheduleEL, out str, this.userData.DatabaseKey.AdminConnectionString);
                DataTable SchWO = new DataTable();
                SchWO = objLaborScheduling.GetScheduledWorkOrderSearchCriteriaV2(objUserEL,userData.DatabaseKey.Client.ClientId, this.userData.Site.SiteId, Convert.ToInt64(PersonnelId), strDate.ToString("MM/dd/yyyy"), 1, this.userData.DatabaseKey.ClientConnectionString);
                if (SchWO == null || SchWO.Rows.Count == 0)
                {
                    CreateEventLog(Convert.ToInt64(item), WorkOrderEvents.RemoveSchedule);//-----------SOM-1632-------------////V2-1183
                }
            }
            List<LaborSchedulingModel> LabourSchedulingList = new List<LaborSchedulingModel>();
            DataTable AvailWO = new DataTable();
            AvailWO = objLaborScheduling.GetScheduledWorkOrderSearchCriteriaV2(objUserEL,userData.DatabaseKey.Client.ClientId, this.userData.Site.SiteId, Convert.ToInt64(PersonnelId), strDate.ToString("MM/dd/yyyy"), Convert.ToInt32(flag), this.userData.DatabaseKey.AdminConnectionString);
            if (AvailWO != null && AvailWO.Rows.Count > 0)
            {
                for (int i = 0; i < AvailWO.Rows.Count; i++)
                {
                    LaborSchedulingModel objLSM = new LaborSchedulingModel();
                    var RequiredDate = AvailWO.Rows[i]["RequiredDate"];
                    var sHours = AvailWO.Rows[i]["ScheduledHours"];
                    if ((RequiredDate != null) && (Convert.ToString(RequiredDate) != "") && Convert.ToDateTime(Convert.ToString(RequiredDate)) != default(DateTime))
                    {
                        objLSM.Date = Convert.ToDateTime(RequiredDate);
                    }
                    else
                    {
                        objLSM.Date = null;
                    }
                    objLSM.Status = AvailWO.Rows[i]["Status"].ToString();

                    if ((sHours != null) && (Convert.ToString(sHours) != ""))
                    {
                        objLSM.Hours = Convert.ToDecimal(sHours);
                    }
                    else
                    {
                        objLSM.Hours = Convert.ToDecimal("0.00");
                    }
                    objLSM.Type = AvailWO.Rows[i]["Type"].ToString();
                    objLSM.Description = AvailWO.Rows[i]["Description"].ToString();
                    objLSM.ClientLookupId = AvailWO.Rows[i]["ClientLookupId"].ToString();
                    objLSM.WorkOrderSchedId = Convert.ToUInt32(AvailWO.Rows[i]["WorkOrderSchedId"].ToString());
                    LabourSchedulingList.Add(objLSM);
                }
            }
            return LabourSchedulingList;
        }
        #endregion
        #region Event Log
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
        #region Available Labor
        [Obsolete]
        public List<AvailableSchedulingModel> PopulateLaborAvailable(string flag)
        {
            LaborSchedulingBAL objLaborAvailable = new LaborSchedulingBAL();
            List<AvailableSchedulingModel> LabourAvailableList = new List<AvailableSchedulingModel>();
            DataTable AvailWO = new DataTable();
            AvailWO = objLaborAvailable.GetWorkOrderSearchCriteria(objUserEL, Convert.ToString(flag), this.userData.DatabaseKey.AdminConnectionString);
            if (AvailWO != null && AvailWO.Rows.Count > 0)
            {
                for (int i = 0; i < AvailWO.Rows.Count; i++)
                {
                    var RequiredDate = AvailWO.Rows[i]["RequiredDate"];
                    var Duration = AvailWO.Rows[i]["ScheduledDuration"];
                    var StartDate = AvailWO.Rows[i]["ScheduledStartDate"];
                    AvailableSchedulingModel objLAM = new AvailableSchedulingModel();
                    objLAM.ClientLookupId = AvailWO.Rows[i]["ClientLookupId"].ToString();
                    objLAM.ChargeTo = AvailWO.Rows[i]["ChargeTo"].ToString();
                    objLAM.ChargeToName = AvailWO.Rows[i]["ChargeToName"].ToString();
                    objLAM.Description = AvailWO.Rows[i]["Description"].ToString();
                    objLAM.Status = AvailWO.Rows[i]["Status"].ToString();
                    objLAM.Priority = AvailWO.Rows[i]["Priority"].ToString();
                    objLAM.DownRequired = AvailWO.Rows[i]["DownRequired"].ToString();
                    objLAM.Assigned = AvailWO.Rows[i]["WorkAssigned"].ToString();
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
                    if ((StartDate != null) && (Convert.ToString(StartDate) != "") && Convert.ToDateTime(Convert.ToString(StartDate)) != default(DateTime))
                    {
                        objLAM.StartDate = Convert.ToDateTime(StartDate);
                    }
                    else
                    {
                        objLAM.StartDate = null;
                    }
                    if ((Duration != null) && (Convert.ToString(Duration) != ""))
                    {
                        objLAM.Duration = Convert.ToDecimal(AvailWO.Rows[i]["ScheduledHours"].ToString());
                    }
                    else
                    {
                        objLAM.Duration = Convert.ToDecimal("0.00");
                    }
                    LabourAvailableList.Add(objLAM);
                }
            }
            return LabourAvailableList;
        }
        //V2-630 daily Available Work order 
        public List<AvailableSchedulingModel> PopulateLaborAvailableV2(string orderbycol = "", string orderDir = "", int skip = 0, int length = 0, string clientLookupId = "", string ChargeTo = "", string ChargeToName = "",
         string Description = "", List<string> Status = null, List<string> Priority = null,bool? DownRequired=null,List<string> Assigned = null, List<string> Type = null, string _StartDate =null,decimal Duration=0 , string _RequiredDate = null, string flag = "0")
        
        {
            LaborSchedulingBAL objLaborAvailable = new LaborSchedulingBAL();
            List<AvailableSchedulingModel> LabourAvailableList = new List<AvailableSchedulingModel>();
            List<WorkOrder> LaborSchedulingList = new List<WorkOrder>();
            WorkOrder LaborScheduling = new WorkOrder();
            LaborScheduling.ClientId = this.userData.DatabaseKey.Client.ClientId;
            LaborScheduling.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            LaborScheduling.ScheduleFlag = flag;
                LaborScheduling.OrderbyColumn = orderbycol;
                LaborScheduling.OrderBy = orderDir;
                LaborScheduling.OffSetVal = skip;
                LaborScheduling.NextRow = length;
                LaborScheduling.ClientLookupId = clientLookupId;
                LaborScheduling.ChargeTo = ChargeTo;
                LaborScheduling.ChargeTo_Name = ChargeToName;
                LaborScheduling.Description = Description;
                LaborScheduling.Status = string.Join(",", Status ?? new List<string>());
                LaborScheduling.Priority = string.Join(",", Priority ?? new List<string>());
                LaborScheduling.downRequired = DownRequired;
                LaborScheduling.Type = string.Join(",", Type ?? new List<string>());
                LaborScheduling.Assigned = string.Join(",", Assigned ?? new List<string>());
                LaborScheduling.ScheduledHours = Duration;
                LaborScheduling.ScheduledDateStart = _StartDate;
                LaborScheduling.RequireDate = _RequiredDate;

                LaborSchedulingList = LaborScheduling.RetrieveAvailableWorkForDailyLaborSchedulingBySearchV2(userData.DatabaseKey, userData.Site.TimeZone);
           
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
                AvailableSchedulingModel objLAM = new AvailableSchedulingModel();
                objLAM.ClientLookupId = item.WorkOrderClientLookupId;
                objLAM.ChargeTo = item.ChargeTo;
                objLAM.ChargeToName = item.ChargeTo_Name;
                objLAM.Description = item.Description;
                objLAM.Status = item.Status;
                objLAM.Priority = item.Priority;
                if (PriorityData != null && PriorityData.Any(cus => cus.ListValue == item.Priority))
                {
                    objLAM.Priority = PriorityData.Where(x => x.ListValue == item.Priority).Select(x => x.Description).First();
                }
                objLAM.DownRequired =item.DownRequired.ToString();
                objLAM.Assigned = item.WorkAssigned_Name;
                objLAM.Type = item.Type;
                objLAM.WorkOrderId = item.WorkOrderId;
                objLAM.Duration = item.ScheduledHours;
                if (item.RequiredDate != default(DateTime))
                {
                    objLAM.RequiredDate = item.RequiredDate;
                }
                else
                {
                    objLAM.RequiredDate = null;
                }

                if (item.ScheduledStartDate != default(DateTime))
                {
                    objLAM.StartDate = item.ScheduledStartDate;
                }
                else
                {
                    objLAM.StartDate = null;
                }
                objLAM.PartsOnOrder = item.PartsOnOrder; //V2-838
                //V2-984
                objLAM.WorkAssigned_PersonnelId = item.WorkAssigned_PersonnelId;

                objLAM.Assigned = item.Personnels;
                
                objLAM.TotalCount = item.TotalCount;
                LabourAvailableList.Add(objLAM);
            }
            return LabourAvailableList;
        }
        public List<AWOAddResult> PopulateLaborAvailableAfterAdd(string[] WorkOrderlIds, string PersonnelId, DateTime strDate, string flag)
        {
            List<AWOAddResult> AWOList = new List<AWOAddResult>();
            AWOAddResult objAWO;
            String message = "";
            DataTable dtCnt = new DataTable();
            LaborSchedulingBAL objLaborAvailable = new LaborSchedulingBAL();
            String str = "";
            foreach (var item in WorkOrderlIds)
            {
                //var strScheduledDate = strDate;
                dtCnt = objLaborScheduling.GetScheduledWorkOrderByWorkOrderIDAndDate(Convert.ToInt64(item), this.userData.DatabaseKey.Client.ClientId, this.userData.Site.SiteId, Convert.ToInt64(PersonnelId), strDate.ToString("MM/dd/yyyy"), 0, this.userData.DatabaseKey.AdminConnectionString);
                if (dtCnt.Rows.Count > 0)
                {
                    objAWO = new AWOAddResult();
                    message = "Work Order " + dtCnt.Rows[0]["ClientLookupId"].ToString() + " Is Already Scheduled";
                    objAWO.Status = "Scheduled";
                    objAWO.Message = message;
                    AWOList.Add(objAWO);
                }
                else
                {
                    WorkOrderScheduleEL objWorkOrderScheduleEL = new WorkOrderScheduleEL();
                    objWorkOrderScheduleEL.PersonnelId = Convert.ToInt64(PersonnelId);
                    objWorkOrderScheduleEL.ClientId = this.userData.DatabaseKey.User.ClientId;
                    objWorkOrderScheduleEL.ModifyBy = this.userData.DatabaseKey.UserName;
                    objWorkOrderScheduleEL.ModifyDate = DateTime.UtcNow;
                    objWorkOrderScheduleEL.ScheduledStartDate = strDate;
                    Int32 intResponse = objLaborScheduling.InsertScheduleRecordWO(item, objWorkOrderScheduleEL, out str, this.userData.DatabaseKey.ClientConnectionString);
                    if (intResponse > 0)
                    {
                        CreateEventLog(Convert.ToInt64(item), WorkOrderEvents.Scheduled);
                        WorkOrder workorder = new DataContracts.WorkOrder();
                        ProcessAlert objAlert = new ProcessAlert(this.userData);                        
                        List<long> nos = new List<long>() { Convert.ToInt64(item) };
                        CommonWrapper coWrapper = new CommonWrapper(userData);
                        var namelist = coWrapper.MentionList("");
                        var UserList = new List<Tuple<long, string, string>>();
                        string userName = namelist.Where(x => x.PersonnelId == Convert.ToInt64(PersonnelId)).Select(y => y.UserName).FirstOrDefault();
                        string emailId = namelist.Where(x => x.PersonnelId == Convert.ToInt64(PersonnelId)).Select(y => y.Email).FirstOrDefault();
                        UserList.Add
                            (                              
                             Tuple.Create(Convert.ToInt64(PersonnelId),userName,emailId)
                            );
                          
                        objAlert.CreateAlert<DataContracts.WorkOrder>(AlertTypeEnum.WorkOrderAssign, nos, UserList);
                    }
                }
            }
            return AWOList;
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
    }
}