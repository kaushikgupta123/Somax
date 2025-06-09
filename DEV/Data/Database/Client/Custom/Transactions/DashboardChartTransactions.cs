
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Enumerations;
using System.Data.SqlClient;
using Database.StoredProcedure;
using Database.Business;

namespace Database.Transactions
{
    public class PieChartBaseTransaction : AbstractTransactionManager
    {
        public PieChartBaseTransaction()
        {
            base.UseDatabase = DatabaseTypeEnum.Client;
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        // Result Sets
        public List<KeyValuePair<string, long>> Entries { get; set; }

        // Result Sets
        public List<KeyValuePair<string, List<b_WorkOrder>>> WoEntries { get; set; }

        // Result Sets
        public List<KeyValuePair<string, decimal>> WOLaborHoursEntries { get; set; }

        public List<Tuple< string, long, string>> ChartDataList = new List<Tuple<string, long, string>>();

        public List<Tuple<string, decimal, string>> ChartDataListDecimal = new List<Tuple<string, decimal, string>>();


        #region Fleet Only
        public List<KeyValuePair<string, decimal>> SOLaborHoursEntries { get; set; }
        // Result Sets
        public List<KeyValuePair<int, string>> SOBackLogEntries { get; set; }
        #endregion

        #region Enterprise User
        public List<KeyValuePair<string, decimal>> EnterpriseUserBarChartEntries { get; set; }
        #endregion

        // Result Sets
        public List<KeyValuePair<int, string>> WOBackLogEntries { get; set; }

        // Result Sets
        public List<b_WorkOrder> WorkOrderSourceEntries { get; set; }

        public override void PerformWorkItem()
        {
            throw new NotImplementedException();
        }

        public override void Preprocess()
        {
            // throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            // throw new NotImplementedException();
        }
    }

    public class Equipment_RetrieveDownTimeByPK : PieChartBaseTransaction
    {

        public System.Data.DataTable table_rows { get; set; }
        public int TimeFrame { get; set; }
        public int EquipmentId { get; set; }
        public override void PerformWorkItem()
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand()
                {
                    Connection = this.Connection,
                    Transaction = this.Transaction
                };
                table_rows = usp_Equipment_RetrieveDownTimeByPK.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, dbKey.User.DefaultSiteId, TimeFrame, EquipmentId);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
            }
        }
    }

    public class Equipment_Retrieve_Downtime_Chart2 : PieChartBaseTransaction
    {

        public System.Data.DataTable table_rows { get; set; }
        public int TimeFrame { get; set; }

        public override void PerformWorkItem()
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand()
                {
                    Connection = this.Connection,
                    Transaction = this.Transaction
                };
                table_rows = usp_Equipment_RetrieveDownTime_Chart2.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, dbKey.User.DefaultSiteId, TimeFrame);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
            }
        }
    }

    public class WorkOrder_Retrieve_ByType_Chart : PieChartBaseTransaction
    {

        public int TimeFrame { get; set; }
        public int Flag { get; set; }

        public override void PerformWorkItem()
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand()
                {
                    Connection = this.Connection,
                    Transaction = this.Transaction
                };
                Entries = usp_WorkOrder_RetrieveByTypeChart.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, dbKey.User.DefaultSiteId, TimeFrame, Flag);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
            }
        }
    }

    public class WorkOrder_Retrieve_ByPriority_Chart : PieChartBaseTransaction
    {

        public int TimeFrame { get; set; }

        public override void PerformWorkItem()
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand()
                {
                    Connection = this.Connection,
                    Transaction = this.Transaction
                };
                Entries = usp_WorkOrder_RetrieveByPriorityChart.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, dbKey.User.DefaultSiteId, TimeFrame);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
            }
        }
    }


    public class WorkOrder_Retrieve_Complete_vs_Scheduled : PieChartBaseTransaction
    {
        public int TimeFrame { get; set; }

        public override void PerformWorkItem()
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand()
                {
                    Connection = this.Connection,
                    Transaction = this.Transaction
                };
                List<KeyValuePair<long, long>> temp;
                Entries = new List<KeyValuePair<string, long>>();
                temp = usp_WorkOrder_RetrieveCompletevsScheduled.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, dbKey.User.DefaultSiteId, TimeFrame);
                foreach (KeyValuePair<long, long> kvp in temp)
                {
                    if (kvp.Key == 0)
                    {
                        Entries.Add(new KeyValuePair<string, long>("Complete", kvp.Value));   // Must be localized
                    }
                    if (kvp.Key == 1)
                    {
                        Entries.Add(new KeyValuePair<string, long>("Scheduled", kvp.Value));  // Must be localized
                    }
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
            }
        }
    }

    public class WorkOrder_RetrieveByWOCountBForChart : PieChartBaseTransaction
    {
        public int TimeFrame { get; set; }

        public override void PerformWorkItem()
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand()
                {
                    Connection = this.Connection,
                    Transaction = this.Transaction
                };
                List<KeyValuePair<string, List<b_WorkOrder>>> temp;
                WoEntries = new List<KeyValuePair<string, List<b_WorkOrder>>>();
                temp = usp_WorkOrder_RetrieveByWOCountBForChart.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, dbKey.User.DefaultSiteId, TimeFrame);
                foreach (KeyValuePair<string, List<b_WorkOrder>> kvp in temp)
                {
                    WoEntries.Add(new KeyValuePair<string, List<b_WorkOrder>>(kvp.Key, kvp.Value));   // Must be localized
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
            }
        }
    }

    public class WorkOrder_RetrieveByWOStatusForChart_1 : PieChartBaseTransaction
    {
        public int Flag { get; set; }
        public long ChargeToId { get; set; }

        public override void PerformWorkItem()
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand()
                {
                    Connection = this.Connection,
                    Transaction = this.Transaction
                };
                List<KeyValuePair<string, long>> temp;
                Entries = new List<KeyValuePair<string, long>>();
                temp = usp_WorkOrder_RetrieveByWOStatusForChart_1.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, dbKey.User.DefaultSiteId, Flag, ChargeToId);
                foreach (KeyValuePair<string, long> kvp in temp)
                {

                    Entries.Add(new KeyValuePair<string, long>(kvp.Key, kvp.Value));   // Must be localized

                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
            }
        }
    }


    public class WorkOrder_RetrieveByWOStatusForChart_3 : PieChartBaseTransaction
    {
        public List<KeyValuePair<long, long>> OverDuePMEntries { get; set; }
        public int Flag { get; set; }
        public long ChargeToId { get; set; }

        public override void PerformWorkItem()
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand()
                {
                    Connection = this.Connection,
                    Transaction = this.Transaction
                };
                List<KeyValuePair<long, long>> temp;
                OverDuePMEntries = new List<KeyValuePair<long, long>>();

                temp = usp_WorkOrder_RetrieveByWOStatusForChart_3.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, dbKey.User.DefaultSiteId, Flag, ChargeToId);
                foreach (KeyValuePair<long, long> kvp in temp)
                {

                    OverDuePMEntries.Add(new KeyValuePair<long, long>(kvp.Key, kvp.Value));

                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
            }
        }
    }
    public class PartStoreRoom_LowParts : PieChartBaseTransaction
    {

        public override void PerformWorkItem()
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand()
                {
                    Connection = this.Connection,
                    Transaction = this.Transaction
                };
                List<KeyValuePair<string, long>> temp;
                Entries = new List<KeyValuePair<string, long>>();
                temp = usp_PartStoreRoom_LowParts.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, dbKey.User.DefaultSiteId);
                foreach (KeyValuePair<string, long> kvp in temp)
                {
                    Entries.Add(new KeyValuePair<string, long>(kvp.Key, kvp.Value));   // Must be localized
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
            }
        }
    }
    public class WorkOrderLaborHrs : PieChartBaseTransaction
    {
        public int TimeFrame { get; set; }

        public override void PerformWorkItem()
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand()
                {
                    Connection = this.Connection,
                    Transaction = this.Transaction,
                    CommandTimeout = 90
                };
                List<KeyValuePair<string, decimal>> temp;
                WOLaborHoursEntries = new List<KeyValuePair<string, decimal>>();
                temp = usp_WorkOrderLaborHrs.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, dbKey.User.DefaultSiteId, TimeFrame);
                foreach (KeyValuePair<string, decimal> kvp in temp)
                {
                    WOLaborHoursEntries.Add(new KeyValuePair<string, decimal>(kvp.Key, kvp.Value));   // Must be localized
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
            }
        }
    }

    public class WorkOrderBacklog : PieChartBaseTransaction
    {
        public override void PerformWorkItem()
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand()
                {
                    Connection = this.Connection,
                    Transaction = this.Transaction
                };
                List<KeyValuePair<int, string>> temp;
                WOBackLogEntries = new List<KeyValuePair<int, string>>();
                temp = usp_WorkOrderBacklog.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, dbKey.User.DefaultSiteId);
                foreach (KeyValuePair<int, string> kvp in temp)
                {
                    WOBackLogEntries.Add(new KeyValuePair<int, string>(kvp.Key, kvp.Value));   // Must be localized
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
            }
        }
    }
    public class WorkOrderSource : PieChartBaseTransaction
    {
        public int TimeFrame { get; set; }

        public override void PerformWorkItem()
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand()
                {
                    Connection = this.Connection,
                    Transaction = this.Transaction
                };
                List<b_WorkOrder> temp;
                WorkOrderSourceEntries = new List<b_WorkOrder>();
                temp = usp_WorkOrderSource.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, dbKey.User.DefaultSiteId, TimeFrame);
                foreach (var kvp in temp)
                {

                    WorkOrderSourceEntries.Add(kvp);   // Must be localized

                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
            }
        }

    }



    #region Fleet Only
    public class ServiceOrderLaborHrs : PieChartBaseTransaction
    {
        public int TimeFrame { get; set; }

        public override void PerformWorkItem()
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand()
                {
                    Connection = this.Connection,
                    Transaction = this.Transaction
                };
                List<KeyValuePair<string, decimal>> temp;
                SOLaborHoursEntries = new List<KeyValuePair<string, decimal>>();
                temp = usp_ServiceOrderLaborHrs_V2.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, dbKey.User.DefaultSiteId, TimeFrame);
                foreach (KeyValuePair<string, decimal> kvp in temp)
                {
                    SOLaborHoursEntries.Add(new KeyValuePair<string, decimal>(kvp.Key, kvp.Value));   // Must be localized
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
            }
        }
    }

    public class ServiceOrderBacklog : PieChartBaseTransaction
    {
        public override void PerformWorkItem()
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand()
                {
                    Connection = this.Connection,
                    Transaction = this.Transaction
                };
                List<KeyValuePair<int, string>> temp;
                SOBackLogEntries = new List<KeyValuePair<int, string>>();
                temp = usp_ServiceOrderBacklog_V2.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, dbKey.User.DefaultSiteId);
                foreach (KeyValuePair<int, string> kvp in temp)
                {
                    SOBackLogEntries.Add(new KeyValuePair<int, string>(kvp.Key, kvp.Value));   // Must be localized
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
            }
        }
    }
    #endregion

    #region Enterprise User
    public class EnterpriseUserBarChartBySite : PieChartBaseTransaction
    {
        public string Flag { get; set; }

        public override void PerformWorkItem()
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand()
                {
                    Connection = this.Connection,
                    Transaction = this.Transaction
                };
                List<KeyValuePair<string, decimal>> temp;
                EnterpriseUserBarChartEntries = new List<KeyValuePair<string, decimal>>();
                temp = usp_Metrics_RetrieveBarChartDetailsByMetricsName_V2.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, Flag);
                foreach (KeyValuePair<string, decimal> kvp in temp)
                {
                    EnterpriseUserBarChartEntries.Add(new KeyValuePair<string, decimal>(kvp.Key, kvp.Value));   // Must be localized
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
            }
        }
    }
    #endregion


}
