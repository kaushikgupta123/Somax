
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Transactions;
using Database.Business;

namespace DataContracts
{
   public static class DashboardReports
    {
        public static System.Data.DataTable Equipment_RetrieveDownTimeforChart(DatabaseKey dbKey, int timeframe)
        {
            Equipment_Retrieve_Downtime_Chart2 trans = new Equipment_Retrieve_Downtime_Chart2()
            {
                dbKey = dbKey.ToTransDbKey(),
                TimeFrame = timeframe
            };
            trans.Execute();
            return trans.table_rows;
        }
        public static System.Data.DataTable Equipment_RetrieveDownTimeByPK(DatabaseKey dbKey, int timeframe,int EquipmentId)
        {
            Equipment_RetrieveDownTimeByPK trans = new Equipment_RetrieveDownTimeByPK()
            {
                dbKey = dbKey.ToTransDbKey(),
                TimeFrame = timeframe,
                EquipmentId= EquipmentId
            };
            trans.Execute();
            return trans.table_rows;
        }

        public static List<KeyValuePair<string, long>> WorkOrder_ByWorkOrderType(DatabaseKey dbKey, int timeframe,int flag)
        {
            WorkOrder_Retrieve_ByType_Chart trans = new WorkOrder_Retrieve_ByType_Chart()
            {
                dbKey = dbKey.ToTransDbKey(),
                TimeFrame = timeframe,
                Flag= flag
            };
            trans.Execute();
            return trans.Entries;
        }

        public static List<KeyValuePair<string, long>> WorkOrder_ByWorkOrderPriority(DatabaseKey dbKey, int timeframe)
        {
            WorkOrder_Retrieve_ByPriority_Chart trans = new WorkOrder_Retrieve_ByPriority_Chart()
            {
                dbKey = dbKey.ToTransDbKey(),
                TimeFrame = timeframe
            };
            trans.Execute();
            return trans.Entries;
        }


        public static List<KeyValuePair<string, long>> WorkOrder_Retrieve_Complete_vs_Scheduled(DatabaseKey dbKey, int timeframe)
        {
            WorkOrder_Retrieve_Complete_vs_Scheduled trans = new WorkOrder_Retrieve_Complete_vs_Scheduled()
            {
                dbKey = dbKey.ToTransDbKey(),
                TimeFrame = timeframe
            };
            trans.Execute();
            return trans.Entries;
        }
        public static List<KeyValuePair<string,List<b_WorkOrder>>> WorkOrder_RetrieveByWOCountBForChart(DatabaseKey dbKey, int timeframe)
        {
            WorkOrder_RetrieveByWOCountBForChart trans = new WorkOrder_RetrieveByWOCountBForChart()
            {
                dbKey = dbKey.ToTransDbKey(),
                TimeFrame = timeframe
            };
            trans.Execute();
            return trans.WoEntries;
        }
        public static List<KeyValuePair<string, long>> WorkOrder_RetrieveByWOStatusForChart_1(DatabaseKey dbKey, int flag, long EquipmentId = 0)
        {
            WorkOrder_RetrieveByWOStatusForChart_1 trans = new WorkOrder_RetrieveByWOStatusForChart_1()
            {
                dbKey = dbKey.ToTransDbKey(),
                Flag = flag,
                ChargeToId = EquipmentId
            };
            trans.Execute();
            return trans.Entries;
        }
        public static List<KeyValuePair<long, long>> WorkOrder_RetrieveByWOStatusForChart_3(DatabaseKey dbKey,int flag, long EquipmentId = 0)
        {
            WorkOrder_RetrieveByWOStatusForChart_3 trans = new WorkOrder_RetrieveByWOStatusForChart_3()
            {
                dbKey = dbKey.ToTransDbKey(),
                Flag=flag,
                ChargeToId = EquipmentId
            };
            trans.Execute();
            return trans.OverDuePMEntries;
        }
        public static List<KeyValuePair<string, long>> PartStoreRoom_LowParts(DatabaseKey dbKey)
        {
            PartStoreRoom_LowParts trans = new PartStoreRoom_LowParts()
            {
                dbKey = dbKey.ToTransDbKey(),
            };
            trans.Execute();
            return trans.Entries;
        }
        public static List<KeyValuePair<string, decimal>> WorkOrderLaborHrs(DatabaseKey dbKey, int timeframe)
        {
            WorkOrderLaborHrs trans = new WorkOrderLaborHrs()
            {
                dbKey = dbKey.ToTransDbKey(),
                TimeFrame = timeframe
            };
            trans.Execute();
            return trans.WOLaborHoursEntries;
        }
        public static List<KeyValuePair<int, string>> WorkOrderBacklog(DatabaseKey dbKey, int timeframe=0)
        {
            WorkOrderBacklog trans = new WorkOrderBacklog()
            {
                dbKey = dbKey.ToTransDbKey(),
            };
            trans.Execute();
            return trans.WOBackLogEntries;
        }
        public static List<b_WorkOrder> WorkOrderSource(DatabaseKey dbKey, int timeframe)
        {
            WorkOrderSource trans = new WorkOrderSource()
            {
                dbKey = dbKey.ToTransDbKey(),
                TimeFrame = timeframe
            };
            trans.Execute();
            return trans.WorkOrderSourceEntries;
        }
        #region Fleet Only
        public static List<KeyValuePair<string, decimal>> ServiceOrderLaborHrs(DatabaseKey dbKey, int timeframe)
        {
            ServiceOrderLaborHrs trans = new ServiceOrderLaborHrs()
            {
                dbKey = dbKey.ToTransDbKey(),
                TimeFrame = timeframe
            };
            trans.Execute();
            return trans.SOLaborHoursEntries;
        }

        public static List<KeyValuePair<int, string>> ServiceOrderBacklog(DatabaseKey dbKey, int timeframe = 0)
        {
            ServiceOrderBacklog trans = new ServiceOrderBacklog()
            {
                dbKey = dbKey.ToTransDbKey(),
            };
            trans.Execute();
            return trans.SOBackLogEntries;
        }

        #endregion

        #region
        public static List<KeyValuePair<string, decimal>> EnterpriseUserBarChart(DatabaseKey dbKey, string flag)
        {
            EnterpriseUserBarChartBySite trans = new EnterpriseUserBarChartBySite()
            {
                dbKey = dbKey.ToTransDbKey(),
                Flag=flag
            };
            trans.Execute();
            return trans.EnterpriseUserBarChartEntries;
        }

        #endregion
    }
}
