
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Reflection;
using System.Text;
using System.Data;

using Database;
using Database.Business;
using Common.Extensions;
using Newtonsoft.Json;
using Database.Transactions;

namespace DataContracts
{
    public partial  class WorkOrderPlan : DataContractBase
    {
        #region Property
        public string PersonnelList { get; set; }
        public string ScheduledDateStart { get; set; }
        public string ScheduledDateEnd { get; set; }
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int OffSetVal { get; set; }
        public int NextRow { get; set; }
        public string ClientLookupId { get; set; }
        public string ChargeTo_Name { get; set; }
        public string ChargeTo { get; set; }
        public string RequireDate { get; set; }
        public string Type { get; set; }
        public int CustomQueryDisplayId { get; set; }
        public string SearchText { get; set; }
        public long WorkOrderId { get; set; }
        public Int64 PersonnelId { get; set; }
        public long WorkOrderScheduleId { get; set; }
        public string WorkOrderClientLookupId { get; set; }
        public decimal ScheduledHours { get; set; }
        public DateTime? ScheduledStartDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public string EquipmentClientLookupId { get; set; }
       
        public string PersonnelName { get; set; }
        public string PerNextValue { get; set; }
        public DateTime SDNextValue { get; set; }
        public decimal SumPersonnelHour { get; set; }
        public decimal SumScheduledateHour { get; set; }
        public decimal GrandTotalHour { get; set; }
        public long PerIDNextValue { get; set; }
        public int TotalCount { get; set; }
        public DateTime CreateDate { get; set; }
        public int ChildCount { get; set; }
        public bool IsDeleteFlag { get; set; }
        public bool EquipDown { get; set; }
        public bool DownRequired { get; set; }
        public string DepartmentName { get; set; }
        public string WorkAssigned_Name { get; set; }
        public decimal ScheduledDuration { get; set; }
        public string ChargeType { get; set; }
        public string SeriesName { get; set; }
        public Decimal Total { get; set; }
        #endregion Property
        #region  Work Order Plan Details Property

        public string PersonneNameFirst { get; set; }
        public string PersonneNameLast { get; set; }
        public string Priority { get; set; }
        public long ChargeToId { get; set; }
        public long WOPlanLineItemId { get; set; }
        public string WOPlanLineItemType { get; set; }
        #endregion

        public List<WorkOrderPlan> RetrieveResourceListChunkSearch(DatabaseKey dbKey, string TimeZone)
        {

            WorkOrderPlan_RetrieveResourceListForChunkSearch trans = new WorkOrderPlan_RetrieveResourceListForChunkSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.WorkOrderPlan = this.ToDatabaseObjectResourceListChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<WorkOrderPlan> WorkOrderPlanList = new List<WorkOrderPlan>();
            foreach (b_WorkOrderPlan WorkOrderPlan in trans.WorkOrderPlanList)
            {
                WorkOrderPlan tmpWorkOrder = new WorkOrderPlan();

                tmpWorkOrder.UpdateFromDatabaseObjectforResourceListChunkSearch(WorkOrderPlan, TimeZone);
                WorkOrderPlanList.Add(tmpWorkOrder);
            }
            return WorkOrderPlanList;
        }

        public b_WorkOrderPlan ToDatabaseObjectResourceListChunkSearch()
        {
            b_WorkOrderPlan dbObj = this.ToDatabaseObject();

            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.WorkOrderPlanId = this.WorkOrderPlanId;
            dbObj.PersonnelList = this.PersonnelList;
            //dbObj.ScheduledDateStart = this.ScheduledDateStart;
            //dbObj.ScheduledDateEnd = this.ScheduledDateEnd;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;
            dbObj.ClientLookupId = this.ClientLookupId;
            dbObj.ChargeTo_Name = this.ChargeTo_Name;
            dbObj.Description = this.Description;
            dbObj.RequireDate = this.RequireDate;
            dbObj.Type = this.Type;
            //dbObj.CustomQueryDisplayId = this.CustomQueryDisplayId;
            dbObj.SearchText = this.SearchText;

            return dbObj;
        }

        public void UpdateFromDatabaseObjectforResourceListChunkSearch(b_WorkOrderPlan dbObj, string TimeZone)
        {
            this.WorkOrderId = dbObj.WorkOrderId;
            this.PersonnelId = dbObj.PersonnelId;
            this.WorkOrderScheduleId = dbObj.WorkOrderScheduleId;
            this.WorkOrderClientLookupId = dbObj.WorkOrderClientLookupId;
            this.Type = dbObj.Type;
            this.Description = dbObj.Description;
            this.ScheduledHours = dbObj.ScheduledHours;
            this.ScheduledStartDate = dbObj.ScheduledStartDate;
            this.RequiredDate = dbObj.RequiredDate;
            this.EquipmentClientLookupId = dbObj.EquipmentClientLookupId;
            this.ChargeTo_Name = dbObj.ChargeTo_Name;
            this.Status = dbObj.Status;
            this.PersonnelName = dbObj.PersonnelName;          
            this.PerNextValue = dbObj.PerNextValue;
            this.SDNextValue = dbObj.SDNextValue;
            this.SumPersonnelHour = dbObj.SumPersonnelHour;
            this.SumScheduledateHour = dbObj.SumScheduledateHour;
            this.GrandTotalHour = dbObj.GrandTotalHour;
            this.PerIDNextValue = dbObj.PerIDNextValue;

            this.TotalCount = dbObj.TotalCount;

        }
        #region  Workorder plan Details
        public WorkOrderPlan RetrieveListForRetrieveByWorkOrderPlanId(DatabaseKey dbKey, string TimeZone)
        {

            WorkOrderPlan_RetrieveByWorkOrderPlanId trans = new WorkOrderPlan_RetrieveByWorkOrderPlanId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.WorkOrderPlan = this.ToDatabaseObjectRetrieveByWorkOrderPlanId();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            WorkOrderPlan WorkOrderPlanDetails = new WorkOrderPlan();
            var records = trans.WorkOrderPlans;
            WorkOrderPlanDetails.WorkOrderPlanId = records.WorkOrderPlanId;
            WorkOrderPlanDetails.Description = records.Description;
            WorkOrderPlanDetails.EndDate = records.EndDate;
            WorkOrderPlanDetails.CompleteDate = records.CompleteDate;
            WorkOrderPlanDetails.StartDate = records.StartDate;
            WorkOrderPlanDetails.LockPlan = records.LockPlan;
            WorkOrderPlanDetails.Status = records.Status;
            WorkOrderPlanDetails.PersonneNameFirst = records.PersonneNameFirst;
            WorkOrderPlanDetails.PersonneNameLast = records.PersonneNameLast;
            return WorkOrderPlanDetails;
        }
        public b_WorkOrderPlan ToDatabaseObjectRetrieveByWorkOrderPlanId()
        {
            b_WorkOrderPlan dbObj = this.ToDatabaseObject();

            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.WorkOrderPlanId = this.WorkOrderPlanId;
            dbObj.Description = this.Description;
            dbObj.EndDate = this.EndDate;
            dbObj.CompleteDate = this.CompleteDate;
            dbObj.StartDate = this.StartDate;
            dbObj.LockPlan = this.LockPlan;
            dbObj.Status = this.Status;
            dbObj.PersonneNameFirst = this.PersonneNameFirst;
            dbObj.PersonneNameLast = this.PersonneNameLast;
            return dbObj;
        }
        #endregion

        #region Work Order for Work order plan chunk search
        public List<WorkOrderPlan> RetrieveWorkOrderForWorkOrderPlanChunkSearch(DatabaseKey dbKey)
        {
            WorkOrderPlan_RetrieveWorkOrderForWorkOrderPlanChunkSearch trans = new WorkOrderPlan_RetrieveWorkOrderForWorkOrderPlanChunkSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.WorkOrderPlan = this.ToDateBaseObjectForRetrieveWorkOrderForWorkOrderPlanChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<WorkOrderPlan> WorkOrderlist = new List<WorkOrderPlan>();

            foreach (b_WorkOrderPlan WO in trans.WorkOrderList)
            {
                WorkOrderPlan tmpWO = new WorkOrderPlan();
                tmpWO.UpdateFromDatabaseObjectForRetriveAllWorkOrderForSearch(WO);
                WorkOrderlist.Add(tmpWO);
            }
            return WorkOrderlist;
        }
        public b_WorkOrderPlan ToDateBaseObjectForRetrieveWorkOrderForWorkOrderPlanChunkSearch()
        {
            b_WorkOrderPlan dbObj = this.ToDatabaseObject();
            dbObj.WOPlanLineItemId = this.WOPlanLineItemId;
            dbObj.WOPlanLineItemType = this.WOPlanLineItemType;
            dbObj.WorkOrderId = this.WorkOrderId;
            dbObj.WorkOrderPlanId = this.WorkOrderPlanId;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;
            dbObj.WorkOrderClientLookupId = this.WorkOrderClientLookupId;
            dbObj.Description = this.Description;
            dbObj.Type = this.Type;
            dbObj.RequireDate = this.RequireDate;
            dbObj.EquipmentClientLookupId = this.EquipmentClientLookupId;
            dbObj.ChargeTo_Name = this.ChargeTo_Name;
            dbObj.TotalCount = this.TotalCount;
            dbObj.SearchText = this.SearchText;
            return dbObj;
        }
        public void UpdateFromDatabaseObjectForRetriveAllWorkOrderForSearch(b_WorkOrderPlan dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.WOPlanLineItemId = dbObj.WOPlanLineItemId;
            this.WOPlanLineItemType = dbObj.WOPlanLineItemType;
            this.WorkOrderId = dbObj.WorkOrderId;
            this.WorkOrderPlanId = dbObj.WorkOrderPlanId;
            this.OrderbyColumn = dbObj.OrderbyColumn;
            this.OrderBy = dbObj.OrderBy;
            this.OffSetVal = dbObj.OffSetVal;
            this.NextRow = dbObj.NextRow;
            this.WorkOrderClientLookupId = dbObj.WorkOrderClientLookupId;
            this.Description = dbObj.Description;
            this.Type = dbObj.Type;
            this.RequiredDate = dbObj.RequiredDate;
            this.EquipmentClientLookupId = dbObj.EquipmentClientLookupId;
            this.ChargeTo_Name = dbObj.ChargeTo_Name;
            this.TotalCount = dbObj.TotalCount;
        }
        #endregion

        #region Work Order Plan Chunk Search
        public List<WorkOrderPlan> RetrieveChunkSearch(DatabaseKey dbKey)
        {
            WorkOrderPlan_RetrieveChunkSearch trans = new WorkOrderPlan_RetrieveChunkSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.WorkOrderPlan = this.ToDateBaseObjectForRetrieveChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<WorkOrderPlan> WorkOrderPlanlist = new List<WorkOrderPlan>();

            foreach (b_WorkOrderPlan WOP in trans.WorkOrderPlanList)
            {
                WorkOrderPlan tmpWOP = new WorkOrderPlan();
                tmpWOP.UpdateFromDatabaseObjectForRetriveAllForSearch(WOP);
                WorkOrderPlanlist.Add(tmpWOP);
            }
            return WorkOrderPlanlist;
        }
        public b_WorkOrderPlan ToDateBaseObjectForRetrieveChunkSearch()
        {
            b_WorkOrderPlan dbObj = this.ToDatabaseObject();
            dbObj.CustomQueryDisplayId = this.CustomQueryDisplayId;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;
           
            return dbObj;
        }
        public void UpdateFromDatabaseObjectForRetriveAllForSearch(b_WorkOrderPlan dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.CustomQueryDisplayId = dbObj.CustomQueryDisplayId;
            this.CreateDate = dbObj.CreateDate;
            this.OrderbyColumn = dbObj.OrderbyColumn;
            this.OrderBy = dbObj.OrderBy;
            this.OffSetVal = dbObj.OffSetVal;
            this.NextRow = dbObj.NextRow;
            this.ChildCount = dbObj.ChildCount;
            this.TotalCount = dbObj.TotalCount;
        }

        #endregion

        #region Resource Calendar
        public List<WorkOrderPlan> RetrieveCalendarForLaborSchedulingChunkSearch(DatabaseKey dbKey, string TimeZone)
        {

            WorkOrderPlan_RetrieveResourceCalendarForChunkSearch trans = new WorkOrderPlan_RetrieveResourceCalendarForChunkSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.WorkOrderPlan = this.ToDatabaseObjectResourceCalendarChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<WorkOrderPlan> WorkOrderPlanList = new List<WorkOrderPlan>();
            foreach (b_WorkOrderPlan WorkOrderPlan in trans.WorkOrderPlanList)
            {
                WorkOrderPlan tmpWorkOrder = new WorkOrderPlan();

                tmpWorkOrder.UpdateFromDatabaseObjectForResourceCalendarChunkSearch(WorkOrderPlan, TimeZone);
                WorkOrderPlanList.Add(tmpWorkOrder);
            }
            return WorkOrderPlanList;
        }
        public b_WorkOrderPlan ToDatabaseObjectResourceCalendarChunkSearch()
        {
            b_WorkOrderPlan dbObj = this.ToDatabaseObject();

            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.PersonnelList = this.PersonnelList;
            dbObj.ScheduledDateStart = this.ScheduledDateStart;
            dbObj.ScheduledDateEnd = this.ScheduledDateEnd;
            dbObj.WorkOrderPlanId = this.WorkOrderPlanId;
            //dbObj.OrderbyColumn = this.OrderbyColumn;
            //dbObj.OrderBy = this.OrderBy;
            //dbObj.OffSetVal = this.OffSetVal;
            //dbObj.NextRow = this.NextRow;
            //dbObj.ClientLookupId = this.ClientLookupId;
            //dbObj.ChargeTo_Name = this.ChargeTo_Name;
            //dbObj.Description = this.Description;
            //dbObj.RequireDate = this.RequireDate;
            //dbObj.Type = this.Type;
            //dbObj.CustomQueryDisplayId = this.CustomQueryDisplayId;
            //dbObj.SearchText = this.SearchText;

            return dbObj;
        }
        public void UpdateFromDatabaseObjectForResourceCalendarChunkSearch(b_WorkOrderPlan dbObj, string TimeZone)
        {
            this.WorkOrderId = dbObj.WorkOrderId;
            this.PersonnelId = dbObj.PersonnelId;
            this.WorkOrderScheduleId = dbObj.WorkOrderScheduleId;
            this.WorkOrderClientLookupId = dbObj.WorkOrderClientLookupId;
            //this.Type = dbObj.Type;
            this.Description = dbObj.Description;
            this.ScheduledHours = dbObj.ScheduledHours;
            this.ScheduledStartDate = dbObj.ScheduledStartDate;
            //this.RequiredDate = dbObj.RequiredDate;
            //this.EquipmentClientLookupId = dbObj.EquipmentClientLookupId;
            //this.ChargeTo_Name = dbObj.ChargeTo_Name;
            //this.Status = dbObj.Status;
            this.PersonnelName = dbObj.PersonnelName;
            //this.PerNextValue = dbObj.PerNextValue;
            //this.SDNextValue = dbObj.SDNextValue;
            //this.SumPersonnelHour = dbObj.SumPersonnelHour;
            //this.SumScheduledateHour = dbObj.SumScheduledateHour;
            //this.GrandTotalHour = dbObj.GrandTotalHour;
            //this.PerIDNextValue = dbObj.PerIDNextValue;
            //this.TotalCount = dbObj.TotalCount;
        }
        public List<WorkOrderPlan> RetrieveWOForScheduleCalendar(DatabaseKey dbKey, string TimeZone)
        {
            WorkOrderPlan_RetrieveWOForScheduleCalendar trans = new WorkOrderPlan_RetrieveWOForScheduleCalendar()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.WorkOrderPlan = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<WorkOrderPlan> WorkOrderPlanList = new List<WorkOrderPlan>();
            foreach (b_WorkOrderPlan WorkOrderPlan in trans.WorkOrderPlanList)
            {
                WorkOrderPlan tmpWorkOrder = new WorkOrderPlan();

                tmpWorkOrder.UpdateFromDatabaseObjectForResourceCalendarChunkSearch(WorkOrderPlan, TimeZone);
                WorkOrderPlanList.Add(tmpWorkOrder);
            }
            return WorkOrderPlanList;
        }
        public void UpdateFromDatabaseObjectForRetrieveWOForScheduleCalendar(b_WorkOrderPlan dbObj, string TimeZone)
        {
            this.WorkOrderId = dbObj.WorkOrderId;            
            this.WorkOrderClientLookupId = dbObj.WorkOrderClientLookupId;            
            this.Description = dbObj.Description;            
        }
        public void AddScheduleRecord(DatabaseKey dbKey)
        {
            WorkOrderPlan_CalendarAddScheduleRecord trans = new WorkOrderPlan_CalendarAddScheduleRecord();
            trans.WorkOrderPlan = this.ToDatabaseObject();
            trans.WorkOrderPlan.WorkOrderId = this.WorkOrderId;
            trans.WorkOrderPlan.ScheduledStartDate = this.ScheduledStartDate;
            trans.WorkOrderPlan.ScheduledHours = this.ScheduledHours;
            trans.WorkOrderPlan.PersonnelList = this.PersonnelList;
            trans.WorkOrderPlan.IsDeleteFlag = this.IsDeleteFlag;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.WorkOrderPlan);

        }
        public void RemoveScheduleRecord(DatabaseKey dbKey)
        {
            WorkOrderPlan_CalendarRemoveScheduleRecord trans = new WorkOrderPlan_CalendarRemoveScheduleRecord();
            trans.WorkOrderPlan = this.ToDatabaseObject();
            trans.WorkOrderPlan.WorkOrderId = this.WorkOrderId;
            trans.WorkOrderPlan.WorkOrderScheduleId = this.WorkOrderScheduleId;            
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.WorkOrderPlan);

        }
        public void DragWorkOrderScheduleFromCalendar(DatabaseKey dbKey)
        {
            WorkOrderPlan_DragWorkOrderScheduleFromCalendar trans = new WorkOrderPlan_DragWorkOrderScheduleFromCalendar();
            trans.WorkOrderPlan = this.ToDatabaseObject();
            trans.WorkOrderPlan.WorkOrderId = this.WorkOrderId;
            trans.WorkOrderPlan.WorkOrderScheduleId = this.WorkOrderScheduleId;
            trans.WorkOrderPlan.ScheduledStartDate = this.ScheduledStartDate;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.WorkOrderPlan);

        }
        public void UpdateScheduleRecord(DatabaseKey dbKey)
        {
            WorkOrderPlan_CalendarUpdateScheduleRecord trans = new WorkOrderPlan_CalendarUpdateScheduleRecord();
            trans.WorkOrderPlan = this.ToDatabaseObject();
            trans.WorkOrderPlan.WorkOrderId = this.WorkOrderId;
            trans.WorkOrderPlan.WorkOrderScheduleId = this.WorkOrderScheduleId;
            trans.WorkOrderPlan.ScheduledStartDate = this.ScheduledStartDate;
            trans.WorkOrderPlan.ScheduledHours = this.ScheduledHours;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.WorkOrderPlan);

        }
        #endregion
        #region Dashboard
        //public List<KeyValuePair<string, long>> WorkOrderPlanningLineItemStatuses(DatabaseKey dbKey, long workOrderPlanID)
        //{
        //    PlannerWorkOrderLineItemsStatuses trans = new PlannerWorkOrderLineItemsStatuses()
        //    {
        //        dbKey = dbKey.ToTransDbKey(),
        //        WorkOrderPlanId = workOrderPlanID

        //    };
        //    trans.Execute();
        //    return trans.Entries;
        //}

        public List<KeyValuePair<string, long>> WorkOrderPlanningLineItemStatuses(DatabaseKey dbKey)
        {
            PlannerWorkOrderLineItemsStatuses trans = new PlannerWorkOrderLineItemsStatuses()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.WorkOrderPlan = ToDatabaseObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            return trans.Entries;
        }

        public List<KeyValuePair<string, decimal>> WorkOrderPlanningEstimateHours(DatabaseKey dbKey)
        {
            WorkOrderPlanningEstimateHours trans = new WorkOrderPlanningEstimateHours()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.WorkOrderPlan = ToDatabaseObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            return trans.Entries;
        }
        public b_WorkOrderPlan ToDatabaseObjectExtended()
        {
            b_WorkOrderPlan dbObj = new b_WorkOrderPlan();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.WorkOrderPlanId = this.WorkOrderPlanId;
            return dbObj;
        }     
        public List<WorkOrderPlan> RetrieveWorkOrderPlanningChartEstimatedHoursByAssigned(DatabaseKey dbKey)
        {
            PlannerWorkorderEstimatedHoursByAssigned trans = new PlannerWorkorderEstimatedHoursByAssigned()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.WorkOrderPlan = this.ToDatabaseObjectEstimatedHoursByAssigned();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<WorkOrderPlan> WorkOrderPlanList = new List<WorkOrderPlan>();
            foreach (b_WorkOrderPlan WorkOrderPlan in trans.WorkOrderPlanList)
            {
                WorkOrderPlan tmpWorkOrder = new WorkOrderPlan();

                tmpWorkOrder.UpdateFromDatabaseObjectforEstimatedHoursByAssigned(WorkOrderPlan);
                WorkOrderPlanList.Add(tmpWorkOrder);
            }
            return WorkOrderPlanList;
        }

        public b_WorkOrderPlan ToDatabaseObjectEstimatedHoursByAssigned()
        {
            b_WorkOrderPlan dbObj = this.ToDatabaseObject();

            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.WorkOrderPlanId = this.WorkOrderPlanId;
            return dbObj;
        }

        public void UpdateFromDatabaseObjectforEstimatedHoursByAssigned(b_WorkOrderPlan dbObj)
        {
            this.PersonnelId = dbObj.PersonnelId;
            this.PersonnelName = dbObj.PersonnelName;
            this.SeriesName = dbObj.SeriesName;
            this.Total = dbObj.Total;
        }

        public List<WorkOrderPlan> RetrieveWorkOrderPlanningChartActualHoursByAssigned(DatabaseKey dbKey)
        {
            PlannerWorkorderActualHoursByAssigned trans = new PlannerWorkorderActualHoursByAssigned()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.WorkOrderPlan = this.ToDatabaseObjectActualHoursByAssigned();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<WorkOrderPlan> WorkOrderPlanList = new List<WorkOrderPlan>();
            foreach (b_WorkOrderPlan WorkOrderPlan in trans.WorkOrderPlanList)
            {
                WorkOrderPlan tmpWorkOrder = new WorkOrderPlan();

                tmpWorkOrder.UpdateFromDatabaseObjectforActualHoursByAssigned(WorkOrderPlan);
                WorkOrderPlanList.Add(tmpWorkOrder);
            }
            return WorkOrderPlanList;
        }

        public b_WorkOrderPlan ToDatabaseObjectActualHoursByAssigned()
        {
            b_WorkOrderPlan dbObj = this.ToDatabaseObject();

            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.WorkOrderPlanId = this.WorkOrderPlanId;
            return dbObj;
        }

        public void UpdateFromDatabaseObjectforActualHoursByAssigned(b_WorkOrderPlan dbObj)
        {
            this.PersonnelId = dbObj.PersonnelId;
            this.PersonnelName = dbObj.PersonnelName;
            this.SeriesName = dbObj.SeriesName;
            this.Total = dbObj.Total;
        }

        public List<WorkOrderPlan> RetrieveChartCompleteWorkorderByAssigned(DatabaseKey dbKey)
        {
            CompleteWorkorderByAssigned trans = new CompleteWorkorderByAssigned()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
                
            };
            trans.WorkOrderPlan = this.ToDatabaseObjectCompleteWorkorderByAssigned();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<WorkOrderPlan> WorkOrderPlanList = new List<WorkOrderPlan>();
            foreach (b_WorkOrderPlan WorkOrderPlan in trans.WorkOrderPlanList)
            {
                WorkOrderPlan tmpWorkOrder = new WorkOrderPlan();

                tmpWorkOrder.UpdateFromDatabaseObjectforCompleteWorkorderByAssigned(WorkOrderPlan);
                WorkOrderPlanList.Add(tmpWorkOrder);
            }
            return WorkOrderPlanList;
        }

        public b_WorkOrderPlan ToDatabaseObjectCompleteWorkorderByAssigned()
        {
            b_WorkOrderPlan dbObj = this.ToDatabaseObject();

            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.WorkOrderPlanId = this.WorkOrderPlanId;
            return dbObj;
        }

        public void UpdateFromDatabaseObjectforCompleteWorkorderByAssigned(b_WorkOrderPlan dbObj)
        {
            this.PersonnelId = dbObj.PersonnelId;
            this.PersonnelName = dbObj.PersonnelName;
            this.SeriesName = dbObj.SeriesName;
            this.TotalCount = dbObj.TotalCount;
        }

        public List<WorkOrderPlan> RetrieveChartIncompleteWorkorderByAssigned(DatabaseKey dbKey)
        {
            IncompleteWorkorderByAssigned trans = new IncompleteWorkorderByAssigned()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.WorkOrderPlan = this.ToDatabaseObjectIncompleteWorkorderByAssigned();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<WorkOrderPlan> WorkOrderPlanList = new List<WorkOrderPlan>();
            foreach (b_WorkOrderPlan WorkOrderPlan in trans.WorkOrderPlanList)
            {
                WorkOrderPlan tmpWorkOrder = new WorkOrderPlan();

                tmpWorkOrder.UpdateFromDatabaseObjectforIncompleteWorkorderByAssigned(WorkOrderPlan);
                WorkOrderPlanList.Add(tmpWorkOrder);
            }
            return WorkOrderPlanList;
        }
        public b_WorkOrderPlan ToDatabaseObjectIncompleteWorkorderByAssigned()
        {
            b_WorkOrderPlan dbObj = this.ToDatabaseObject();

            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.WorkOrderPlanId = this.WorkOrderPlanId;
            return dbObj;
        }
        public void UpdateFromDatabaseObjectforIncompleteWorkorderByAssigned(b_WorkOrderPlan dbObj)
        {
            this.PersonnelId = dbObj.PersonnelId;
            this.PersonnelName = dbObj.PersonnelName;
            this.SeriesName = dbObj.SeriesName;
            this.TotalCount = dbObj.TotalCount;
        }

        public List<WorkOrderPlan> RetrieveCompleteWorkorderByType(DatabaseKey dbKey)
        {
            CompleteWorkorderByType trans = new CompleteWorkorderByType()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.WorkOrderPlan = this.ToDatabaseObjectCompleteWorkorderByType();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<WorkOrderPlan> WorkOrderPlanList = new List<WorkOrderPlan>();
            foreach (b_WorkOrderPlan WorkOrderPlan in trans.WorkOrderPlanList)
            {
                WorkOrderPlan tmpWorkOrder = new WorkOrderPlan();

                tmpWorkOrder.UpdateFromDatabaseObjectforCompleteWorkorderByType(WorkOrderPlan);
                WorkOrderPlanList.Add(tmpWorkOrder);
            }
            return WorkOrderPlanList;
        }
        public b_WorkOrderPlan ToDatabaseObjectCompleteWorkorderByType()
        {
            b_WorkOrderPlan dbObj = this.ToDatabaseObject();

            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.WorkOrderPlanId = this.WorkOrderPlanId;
            return dbObj;
        }
        public void UpdateFromDatabaseObjectforCompleteWorkorderByType(b_WorkOrderPlan dbObj)
        {
            this.SeriesName = dbObj.SeriesName;
            this.TotalCount = dbObj.TotalCount;
            this.Type = dbObj.Type;
        }

        public List<WorkOrderPlan> RetrieveIncompleteWorkorderByType(DatabaseKey dbKey)
        {
            IncompleteWorkorderByType trans = new IncompleteWorkorderByType()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.WorkOrderPlan = this.ToDatabaseObjectIncompleteWorkorderByType();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<WorkOrderPlan> WorkOrderPlanList = new List<WorkOrderPlan>();
            foreach (b_WorkOrderPlan WorkOrderPlan in trans.WorkOrderPlanList)
            {
                WorkOrderPlan tmpWorkOrder = new WorkOrderPlan();

                tmpWorkOrder.UpdateFromDatabaseObjectforIncompleteWorkorderByType(WorkOrderPlan);
                WorkOrderPlanList.Add(tmpWorkOrder);
            }
            return WorkOrderPlanList;
        }
        public b_WorkOrderPlan ToDatabaseObjectIncompleteWorkorderByType()
        {
            b_WorkOrderPlan dbObj = this.ToDatabaseObject();

            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.WorkOrderPlanId = this.WorkOrderPlanId;
            return dbObj;
        }
        public void UpdateFromDatabaseObjectforIncompleteWorkorderByType(b_WorkOrderPlan dbObj)
        {
            this.SeriesName = dbObj.SeriesName;
            this.TotalCount = dbObj.TotalCount;
            this.Type = dbObj.Type;
        }



        public long RetrieveCountPlannedWorkorderByComplete(DatabaseKey dbKey, long workOrderPlanID)
        {
            WorkOrderPlan_RetrieveCountPlannedWorkorderByComplete trans = new WorkOrderPlan_RetrieveCountPlannedWorkorderByComplete()
            {
                dbKey = dbKey.ToTransDbKey(),
                WorkOrderPlanId = workOrderPlanID
            };
            trans.WorkOrderPlan = this.ToDatabaseObject();
            trans.Execute();
            return trans.CountPlannedWorkorderByComplete;
        }
        public long RetrieveCountPlannedWorkorderByInComplete(DatabaseKey dbKey, long workOrderPlanID)
        {
            WorkOrderPlan_RetrieveCountPlannedWorkorderByInComplete trans = new WorkOrderPlan_RetrieveCountPlannedWorkorderByInComplete()
            {
                dbKey = dbKey.ToTransDbKey(),
                WorkOrderPlanId = workOrderPlanID
            };
            trans.WorkOrderPlan = this.ToDatabaseObject();
            trans.Execute();
            return trans.CountPlannedWorkorderByInComplete;
        }
        
        #endregion

        #region WorkOrder_WorkOrder Plan LookupList By Search Criteria
        public List<WorkOrderPlan> RetrieveWorkOrder_WorkOrderPlanLookupListBySearchCriteria(DatabaseKey dbKey)
        {
            WorkOrder_WorkOrderPlanLookupListBySearchCriteria trans = new WorkOrder_WorkOrderPlanLookupListBySearchCriteria()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.WorkOrderPlan = this.ToDateBaseObjectForRetrieveWorkOrder_WorkOrderPlanLookupListBySearchCriteria();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<WorkOrderPlan> WorkOrderPlanlist = new List<WorkOrderPlan>();

            foreach (b_WorkOrderPlan WOP in trans.WorkOrderPlanList)
            {
                WorkOrderPlan tmpWOP = new WorkOrderPlan();
                tmpWOP.UpdateFromDatabaseObjectForRetrieveWorkOrder_WorkOrderPlanLookupListBySearchCriteria(WOP);
                WorkOrderPlanlist.Add(tmpWOP);
            }
            return WorkOrderPlanlist;
        }
        public b_WorkOrderPlan ToDateBaseObjectForRetrieveWorkOrder_WorkOrderPlanLookupListBySearchCriteria()
        {
            b_WorkOrderPlan dbObj = this.ToDatabaseObject();
            dbObj.WorkOrderId = this.WorkOrderId;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;
            dbObj.WorkOrderClientLookupId = this.WorkOrderClientLookupId;
            dbObj.ChargeTo = this.ChargeTo;
            dbObj.ChargeTo_Name = this.ChargeTo_Name;
            dbObj.Description = this.Description;
            dbObj.Status = this.Status;
            dbObj.Priority = this.Priority;
            dbObj.Type = this.Type;
            dbObj.RequiredDate = this.RequiredDate;
            dbObj.TotalCount = this.TotalCount;
            return dbObj;
        }
        public void UpdateFromDatabaseObjectForRetrieveWorkOrder_WorkOrderPlanLookupListBySearchCriteria(b_WorkOrderPlan dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.WorkOrderId = dbObj.WorkOrderId;
            this.OrderbyColumn = dbObj.OrderbyColumn;
            this.OrderBy = dbObj.OrderBy;
            this.OffSetVal = dbObj.OffSetVal;
            this.NextRow = dbObj.NextRow;
            this.WorkOrderClientLookupId = dbObj.ClientLookupId;
            this.ChargeTo = dbObj.ChargeTo;
            this.ChargeTo_Name = dbObj.ChargeTo_Name;
            this.Description = dbObj.Description;
            this.Status = dbObj.Status;
            this.Priority = dbObj.Priority;
            this.Type = dbObj.Type;
            this.RequiredDate = dbObj.RequiredDate;
            this.TotalCount = dbObj.TotalCount;
        }
        #endregion

        public List<WorkOrderPlan> RetrieveAvailableWorkSearch(DatabaseKey dbKey, string TimeZone)
        {

            WorkOrderPlan_AvailableWorkSearch trans = new WorkOrderPlan_AvailableWorkSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.WorkOrderPlan = this.ToDatabaseObjectAvailableWorkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<WorkOrderPlan> WorkOrderPlanList = new List<WorkOrderPlan>();
            foreach (b_WorkOrderPlan WorkOrderPlan in trans.WorkOrderPlanList)
            {
                WorkOrderPlan tmpWorkOrderPlan = new WorkOrderPlan();

                tmpWorkOrderPlan.UpdateFromDatabaseObjectforAvailableWorkSearch(WorkOrderPlan, TimeZone);
                WorkOrderPlanList.Add(tmpWorkOrderPlan);
            }
            return WorkOrderPlanList;
        }
        public b_WorkOrderPlan ToDatabaseObjectAvailableWorkSearch()
        {
            b_WorkOrderPlan dbObj = this.ToDatabaseObject();

            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.WorkOrderPlanId = this.WorkOrderPlanId;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;
            dbObj.ClientLookupId = this.ClientLookupId;
            dbObj.ChargeTo = this.ChargeTo;
            dbObj.ChargeTo_Name = this.ChargeTo_Name;
            dbObj.Description = this.Description;
            dbObj.Status = this.Status;
            dbObj.Priority = this.Priority;
            dbObj.Type = this.Type;
            if (dbObj.RequiredDate != null && dbObj.RequiredDate != DateTime.MinValue)
            {
                dbObj.RequireDate = this.RequiredDate.ToShortDate();
            }
            else
            {
                this.RequiredDate = null;
            }

           // dbObj.ScheduleFlag = this.ScheduleFlag;

            dbObj.SearchText = this.SearchText;

            return dbObj;
        }
        public void UpdateFromDatabaseObjectforAvailableWorkSearch(b_WorkOrderPlan dbObj, string TimeZone)
        {
            this.WorkOrderClientLookupId = dbObj.WorkOrderClientLookupId;
            this.WorkOrderId = dbObj.WorkOrderId;
            this.Type = dbObj.Type;
            //this.ChargeType = dbObj.ChargeType;
            this.Description = dbObj.Description;
            this.ScheduledHours = dbObj.ScheduledHours;
            this.ChargeTo_Name = dbObj.ChargeTo_Name;
            this.Status = dbObj.Status;
            this.EquipDown = dbObj.EquipDown;
            this.Priority = dbObj.Priority;
            // V2-773
            // Do NOT convert Schedule Start Date
            this.ScheduledStartDate = dbObj.ScheduledStartDate;
            //if (dbObj.ScheduledStartDate != null && dbObj.ScheduledStartDate != DateTime.MinValue)
            //{
            //    this.ScheduledStartDate = dbObj.ScheduledStartDate.ToUserTimeZone(TimeZone);
            //}
            //else
            //{
            //    this.ScheduledStartDate = dbObj.ScheduledStartDate;
            //}
            this.ChargeToId = dbObj.ChargeToId;
            this.DownRequired = dbObj.DownRequired;
            // V2-773
            // Required Date should NOT be converted
            this.RequiredDate = dbObj.RequiredDate;
            //if (dbObj.RequiredDate != null && dbObj.RequiredDate != DateTime.MinValue)
            //{
            //    this.RequiredDate = dbObj.RequiredDate.ToUserTimeZone(TimeZone);
            //}
            //else
            //{
            //    this.RequiredDate = dbObj.RequiredDate;
            //}
            this.DepartmentName = dbObj.DepartmentName;
            this.WorkAssigned_Name = dbObj.WorkAssigned_Name;
            this.ScheduledDuration = dbObj.ScheduledDuration;
            this.ChargeTo = dbObj.ChargeTo;
            this.ChargeType = dbObj.ChargeType;

            this.TotalCount = dbObj.TotalCount;
        }
    }
}
