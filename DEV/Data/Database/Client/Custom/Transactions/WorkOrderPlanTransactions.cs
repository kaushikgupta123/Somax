using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

using Database;
using Common.Enumerations;
using Database.Business;
using Database.StoredProcedure;

namespace Database
{
    public class WorkOrderPlan_RetrieveResourceListForChunkSearch : WorkOrderPlan_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public List<b_WorkOrderPlan> WorkOrderPlanList { get; set; }
      
        public override void PerformWorkItem()
        {

            List<b_WorkOrderPlan> tmpList = null;
            WorkOrderPlan.RetrieveResourceListForChunkSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            WorkOrderPlanList = tmpList;
            //WorkOrder.RetrieveListForLaborSchedulingSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();

        }
    }

    #region Search Work Order Plan
    public class WorkOrderPlan_RetrieveChunkSearch : WorkOrderPlan_TransactionBaseClass
    {

        public List<b_WorkOrderPlan> WorkOrderPlanList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (WorkOrderPlan.WorkOrderPlanId > 0)
            {
                string message = "Work Order Plan has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_WorkOrderPlan> tmpList = null;
            WorkOrderPlan.RetrieveChunkSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            WorkOrderPlanList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion

    #region Resource Calendar
    public class WorkOrderPlan_RetrieveResourceCalendarForChunkSearch : WorkOrderPlan_TransactionBaseClass
    {
        //public override void PerformLocalValidation()
        //{
        //    base.PerformLocalValidation();
        //}
        public List<b_WorkOrderPlan> WorkOrderPlanList { get; set; }
        public override void PerformWorkItem()
        {
            List<b_WorkOrderPlan> tmpList = null;
            WorkOrderPlan.RetrieveResourceCalendarForChunkSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            WorkOrderPlanList = tmpList;            
        }

        //public override void Postprocess()
        //{
        //    base.Postprocess();
        //}
    }

    public class WorkOrderPlan_RetrieveWOForScheduleCalendar : WorkOrderPlan_TransactionBaseClass
    {
        //public override void PerformLocalValidation()
        //{
        //    base.PerformLocalValidation();
        //}
        public List<b_WorkOrderPlan> WorkOrderPlanList { get; set; }
        public override void PerformWorkItem()
        {
            List<b_WorkOrderPlan> tmpList = null;
            WorkOrderPlan.RetrieveWorkOrderForAddSchedule(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            WorkOrderPlanList = tmpList;
        }

        //public override void Postprocess()
        //{
        //    base.Postprocess();
        //}
    }
    public class WorkOrderPlan_CalendarAddScheduleRecord : WorkOrderPlan_TransactionBaseClass
    {

        //public override void PerformLocalValidation()
        //{
        //    base.PerformLocalValidation();

        //}

        public override void PerformWorkItem()
        {
            WorkOrderPlan.AddScheduleRecordFromResourceCalendar(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
    public class WorkOrderPlan_CalendarRemoveScheduleRecord : WorkOrderPlan_TransactionBaseClass
    {

        //public override void PerformLocalValidation()
        //{
        //    base.PerformLocalValidation();

        //}

        public override void PerformWorkItem()
        {
            WorkOrderPlan.RemoveScheduleRecordFromResourceCalendar(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
    public class WorkOrderPlan_DragWorkOrderScheduleFromCalendar : WorkOrderPlan_TransactionBaseClass
    {

        //public override void PerformLocalValidation()
        //{
        //    base.PerformLocalValidation();

        //}

        public override void PerformWorkItem()
        {
            WorkOrderPlan.DragWorkOrderScheduleFromCalendar(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
    public class WorkOrderPlan_CalendarUpdateScheduleRecord : WorkOrderPlan_TransactionBaseClass
    {

        //public override void PerformLocalValidation()
        //{
        //    base.PerformLocalValidation();

        //}

        public override void PerformWorkItem()
        {
            WorkOrderPlan.UpdateScheduleRecordFromResourceCalendar(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
    #endregion

    #region  Workorder plan Details
    public class WorkOrderPlan_RetrieveByWorkOrderPlanId : WorkOrderPlan_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public b_WorkOrderPlan WorkOrderPlans { get; set; }
        public override void PerformWorkItem()
        {
            b_WorkOrderPlan tmpList = null;
            WorkOrderPlan.RetrieveWorkOrderPlanListForRetrieveByWorkOrderPlanId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            WorkOrderPlans = new b_WorkOrderPlan();
            WorkOrderPlans = tmpList;

        }

        public override void Postprocess()
        {
            base.Postprocess();

        }
    }
    #endregion

    #region Search Work Order for Work Order Plan
    public class WorkOrderPlan_RetrieveWorkOrderForWorkOrderPlanChunkSearch : WorkOrderPlan_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public List<b_WorkOrderPlan> WorkOrderList { get; set; }
        public override void PerformWorkItem()
        {
            List<b_WorkOrderPlan> tmpList = null;
            WorkOrderPlan.RetrieveWorkOrderListForWorkOrderPlanForChunkSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            WorkOrderList = new List<b_WorkOrderPlan>();
            WorkOrderList = tmpList;

        }

        public override void Postprocess()
        {
            base.Postprocess();

        }
    }
    #endregion
    #region WorkOrder WorkOrderPlan LookupList By Search Criteria
    public class WorkOrder_WorkOrderPlanLookupListBySearchCriteria : WorkOrderPlan_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public List<b_WorkOrderPlan> WorkOrderPlanList { get; set; }
        public override void PerformWorkItem()
        {

            List<b_WorkOrderPlan> tmpList = null;
            WorkOrderPlan.RetrieveWorkOrder_WorkOrderPlanLookupListBySearchCriteria(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            WorkOrderPlanList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();

        }
    }
    #endregion
    public class WorkOrderPlan_AvailableWorkSearch : WorkOrderPlan_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public List<b_WorkOrderPlan> WorkOrderPlanList { get; set; }
        public override void PerformWorkItem()
        {

            List<b_WorkOrderPlan> tmpList = null;
            WorkOrderPlan.RetrieveAvailableWorkSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            WorkOrderPlanList = tmpList;
            //WorkOrder.RetrieveListForLaborSchedulingSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();

        }
    }

    public class PlannerWorkorderEstimatedHoursByAssigned : WorkOrderPlan_TransactionBaseClass
    {

        public List<b_WorkOrderPlan> WorkOrderPlanList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }
        public override void PerformWorkItem()
        {
            List<b_WorkOrderPlan> tmpList = null;
            WorkOrderPlan.RetrievePlannedWorkorderEstimatedHoursByAssigned(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            WorkOrderPlanList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class WorkOrderPlanningEstimateHours : WorkOrderPlan_TransactionBaseClass
    {      
        public List<KeyValuePair<string, decimal>> Entries { get; set; }
        public override void PerformWorkItem()
        {
            List<KeyValuePair<string, decimal>> tmpArray = null;

            WorkOrderPlan.WorkOrderPlanningEstimateHours(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            Entries = new List<KeyValuePair<string, decimal>>();
            for (int i = 0; i < tmpArray.Count; i++)
            {
                Entries.Add(tmpArray[i]);
            }

        }
    }
    public class PlannerWorkOrderLineItemsStatuses : WorkOrderPlan_TransactionBaseClass
    {     
        public List<KeyValuePair<string, long>> Entries { get; set; }
        public override void PerformWorkItem()
        {
            List<KeyValuePair<string, long>> tmpArray = null;

            WorkOrderPlan.PlannerWorkOrderLineItemsStatuses(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            Entries = new List<KeyValuePair<string, long>>();
            for (int i = 0; i < tmpArray.Count; i++)
            {
                Entries.Add(tmpArray[i]);
            }

        }
    }

    public class PlannerWorkorderActualHoursByAssigned : WorkOrderPlan_TransactionBaseClass
    {

        public List<b_WorkOrderPlan> WorkOrderPlanList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }
        public override void PerformWorkItem()
        {
            List<b_WorkOrderPlan> tmpList = null;
            WorkOrderPlan.RetrievePlannedWorkorderActualHoursByAssigned(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            WorkOrderPlanList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class CompleteWorkorderByAssigned : WorkOrderPlan_TransactionBaseClass
    {

        public List<b_WorkOrderPlan> WorkOrderPlanList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }
        public override void PerformWorkItem()
        {
            List<b_WorkOrderPlan> tmpList = null;
            WorkOrderPlan.RetrieveCompleteWorkorderByAssigned(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            WorkOrderPlanList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    public class IncompleteWorkorderByAssigned : WorkOrderPlan_TransactionBaseClass
    {

        public List<b_WorkOrderPlan> WorkOrderPlanList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }
        public override void PerformWorkItem()
        {
            List<b_WorkOrderPlan> tmpList = null;
            WorkOrderPlan.RetrieveInCompleteWorkorderByAssigned(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            WorkOrderPlanList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class CompleteWorkorderByType : WorkOrderPlan_TransactionBaseClass
    {

        public List<b_WorkOrderPlan> WorkOrderPlanList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }
        public override void PerformWorkItem()
        {
            List<b_WorkOrderPlan> tmpList = null;
            WorkOrderPlan.RetrieveCompleteWorkorderByType(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            WorkOrderPlanList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class IncompleteWorkorderByType : WorkOrderPlan_TransactionBaseClass
    {

        public List<b_WorkOrderPlan> WorkOrderPlanList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }
        public override void PerformWorkItem()
        {
            List<b_WorkOrderPlan> tmpList = null;
            WorkOrderPlan.RetrieveIncompleteWorkorderByType(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            WorkOrderPlanList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    #region  Dashboard
    public class WorkOrderPlan_RetrieveCountPlannedWorkorderByComplete : WorkOrderPlan_TransactionBaseClass
    {
        public WorkOrderPlan_RetrieveCountPlannedWorkorderByComplete()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }

        public long CountPlannedWorkorderByComplete { get; set; }
        public long WorkOrderPlanId { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            CountPlannedWorkorderByComplete = WorkOrderPlan.RetrieveCountPlannedWorkorderByComplete(this.Connection, this.Transaction, this.dbKey.User.UserInfoId, this.dbKey.UserName, this.dbKey.Client.ClientId,this.dbKey.User.DefaultSiteId, this.WorkOrderPlanId);
        }

        public override void Preprocess()
        {
            // nothing to do here
        }

        public override void Postprocess()
        {
            // nothing to do here
        }
    }
    public class WorkOrderPlan_RetrieveCountPlannedWorkorderByInComplete : WorkOrderPlan_TransactionBaseClass
    {
        public WorkOrderPlan_RetrieveCountPlannedWorkorderByInComplete()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }

        public long CountPlannedWorkorderByInComplete { get; set; }
        public long WorkOrderPlanId { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }
        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            CountPlannedWorkorderByInComplete = WorkOrderPlan.RetrieveCountPlannedWorkorderByInComplete(this.Connection, this.Transaction,this.dbKey.User.UserInfoId, this.dbKey.UserName, this.dbKey.Client.ClientId, this.dbKey.User.DefaultSiteId, this.WorkOrderPlanId);
        }

        public override void Preprocess()
        {
            // nothing to do here
        }

        public override void Postprocess()
        {
            // nothing to do here
        }
    }
    #endregion
}
