using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Business
{
    public partial class b_ProjectTask
    {
        #region Property
        public string WorkOrderClientlookupId { get; set; }
        public string WorkOrderDescription { get; set; }
        public long SiteId { get; set; }
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int OffSetVal { get; set; }
        public int NextRow { get; set; }
        public string SearchText { get; set; }
        public int TotalCount { get; set; }

        #endregion

        #region ProjectTask list

        public void RetrieveProjectTaskRetrieveByProjectId(
       SqlConnection connection,
       SqlTransaction transaction,
       long callerUserInfoId,
       string callerUserName,
       ref List<b_ProjectTask> results
)
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data

                results = Database.StoredProcedure.usp_ProjectTask_RetrieveByProjectId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }

        public static b_ProjectTask ProcessRowForRetrieveByProjectId(SqlDataReader reader)
        {
            // Create instance of object
            b_ProjectTask obj = new b_ProjectTask();

            // Load the object from the database
            obj.LoadFromDatabaseForRetrieveByProjectId(reader);

            // Return result
            return obj;
        }
        public int LoadFromDatabaseForRetrieveByProjectId(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // ProjectTaskId column, bigint, not null
                ProjectTaskId = reader.GetInt64(i++);

                // WorkOrderClientlookupId column, nvarchar(200), not null
                if (false == reader.IsDBNull(i))
                {
                    WorkOrderClientlookupId = reader.GetString(i);
                }
                else
                {
                    WorkOrderClientlookupId = "";
                }
                i++;

                // WorkOrderDescription column, nvarchar(200), not null
                if (false == reader.IsDBNull(i))
                {
                    WorkOrderDescription = reader.GetString(i);
                }
                else
                {
                    WorkOrderDescription = "";
                }
                i++;

                // StartDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    StartDate = reader.GetDateTime(i);
                }
                else
                {
                    StartDate = DateTime.MinValue;
                }
                i++;
                // EndDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    EndDate = reader.GetDateTime(i);
                }
                else
                {
                    EndDate = DateTime.MinValue;
                }
                i++;

                // Progress column, decimal, not null
                if (false == reader.IsDBNull(i))
                {
                    Progress = reader.GetDecimal(i);
                }
                else
                {
                    Progress = 0;
                }
                i++;

                //ProjectId column, bigint, not null
                ProjectId = reader.GetInt64(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ProjectTaskId"].ToString(); }
                catch { missing.Append("ProjectTaskId "); }

                try { reader["WorkOrderClientlookupId"].ToString(); }
                catch { missing.Append("WorkOrderClientlookupId "); }

                try { reader["WorkOrderDescription"].ToString(); }
                catch { missing.Append("WorkOrderDescription "); }

                try { reader["StartDate"].ToString(); }
                catch { missing.Append("StartDate "); }

                try { reader["EndDate"].ToString(); }
                catch { missing.Append("EndDate "); }

                try { reader["Progress"].ToString(); }
                catch { missing.Append("Progress "); }

                try { reader["ProjectId"].ToString(); }
                catch { missing.Append("ProjectId "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
            return i;
        }

        #endregion

        #region ProjectTask list chunk search for Project Details page

        public void RetrieveProjectTaskRetrieveByProjectIdForChunkSearch(
      SqlConnection connection,
      SqlTransaction transaction,
      long callerUserInfoId,
      string callerUserName,
      ref List<b_ProjectTask> results
)
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data

                results = Database.StoredProcedure.usp_ProjectTask_RetrieveByProjectIdChunkSearch_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
        public static b_ProjectTask ProcessRowForRetrieveByProjectIdForChunkSearchFor(SqlDataReader reader)
        {
            // Create instance of object
            b_ProjectTask obj = new b_ProjectTask();

            // Load the object from the database
            obj.LoadFromDatabaseForRetrieveByProjectIdForChunkSearchFor(reader);

            // Return result
            return obj;
        }
        public int LoadFromDatabaseForRetrieveByProjectIdForChunkSearchFor(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // ProjectTaskId column, bigint, not null
                ProjectTaskId = reader.GetInt64(i++);

                // WorkOrderId column, bigint, not null
                if (false == reader.IsDBNull(i))
                {
                    WorkOrderId = reader.GetInt64(i);
                }
                else
                {
                    WorkOrderId = 0;
                }
                i++;

                // WorkOrderClientlookupId column, nvarchar(200), not null
                if (false == reader.IsDBNull(i))
                {
                    WorkOrderClientlookupId = reader.GetString(i);
                }
                else
                {
                    WorkOrderClientlookupId = "";
                }
                i++;

                // WorkOrderDescription column, nvarchar(200), not null
                if (false == reader.IsDBNull(i))
                {
                    WorkOrderDescription = reader.GetString(i);
                }
                else
                {
                    WorkOrderDescription = "";
                }
                i++;

                // StartDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    StartDate = reader.GetDateTime(i);
                }
                else
                {
                    StartDate = DateTime.MinValue;
                }
                i++;
                // EndDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    EndDate = reader.GetDateTime(i);
                }
                else
                {
                    EndDate = DateTime.MinValue;
                }
                i++;

                // Progress column, decimal, not null
                if (false == reader.IsDBNull(i))
                {
                    Progress = reader.GetDecimal(i);
                }
                else
                {
                    Progress = 0;
                }
                i++;
                // TotalCount
                TotalCount = reader.GetInt32(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ProjectTaskId"].ToString(); }
                catch { missing.Append("ProjectTaskId "); }

                try { reader["WorkOrderId"].ToString(); }
                catch { missing.Append("WorkOrderId "); }

                try { reader["WorkOrderClientlookupId"].ToString(); }
                catch { missing.Append("WorkOrderClientlookupId "); }

                try { reader["WorkOrderDescription"].ToString(); }
                catch { missing.Append("WorkOrderDescription "); }

                try { reader["StartDate"].ToString(); }
                catch { missing.Append("StartDate "); }

                try { reader["EndDate"].ToString(); }
                catch { missing.Append("EndDate "); }

                try { reader["Progress"].ToString(); }
                catch { missing.Append("Progress "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
            return i;
        }
        #endregion

        #region Dashboard
        public void ProjectTaskDashboardStatusesChart(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            ref List<KeyValuePair<string, long>> results
        )       
        {      
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data

                results = StoredProcedure.usp_ProjectTask_RetrieveProjectStatuses_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }

        public void ProjectTaskDashboardScheduleComplianceStatuses(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            ref List<KeyValuePair<string, long>> results
        )
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data

                results = StoredProcedure.usp_ProjectTask_RetrieveScheduleComplianceStatuses_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
        #endregion
    }
}
