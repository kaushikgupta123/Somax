using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Business
{
    public partial class b_PMSchedAssign
    {
        #region property
        public long SiteId { get; set; }
        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public int offset1 { get; set; }
        public int? nextrow { get; set; }
        public string ClientLookupId { get; set; }
        public string PersonnelFullName { get; set; }
        public int TotalCount { get; set; }
        #endregion

        public void RetrievePMSchedAssignSearch(
   SqlConnection connection,
   SqlTransaction transaction,
   long callerUserInfoId,
   string callerUserName,
   ref List<b_PMSchedAssign> results
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

                results = Database.StoredProcedure.usp_PMSchedAssign_RetrieveByPrevMaintSchedIdForPMSchedulingChildGrid_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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


        public static b_PMSchedAssign ProcessRowForPMSchedAssignGrid(SqlDataReader reader)
        {
            // Create instance of object
            b_PMSchedAssign obj = new b_PMSchedAssign();

            // Load the object from the database
            obj.LoadFromDatabaseForGrid_V2(reader);

            // Return result
            return obj;
        }

        public void LoadFromDatabaseForGrid_V2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                PMSchedAssignId = reader.GetInt64(i++);

                PrevMaintSchedId=reader.GetInt64(i++);

                ClientLookupId = reader.GetString(i++);

                PersonnelId = reader.GetInt64(i++);

                PersonnelFullName = reader.GetString(i++);

                ScheduledHours = reader.GetDecimal(i++);

                TotalCount = reader.GetInt32(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["PMSchedAssignId"].ToString(); }
                catch { missing.Append("PMSchedAssignId "); }
                
                try { reader["PrevMaintSchedId"].ToString(); }
                catch { missing.Append("PrevMaintSchedId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["PersonnelId"].ToString(); }
                catch { missing.Append("PersonnelId "); }

                try { reader["PersonnelFullName"].ToString(); }
                catch { missing.Append("PersonnelFullName "); }

                try { reader["ScheduledHours"].ToString(); }
                catch { missing.Append("ScheduledHours "); }

                try { reader["TotalCount"].ToString(); }
                catch { missing.Append("TotalCount "); }


                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }

        #region V2-1204
        public void RetrieveByPMSchedId_V2(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName,
      ref List<b_PMSchedAssign> data
        )
        {
            SqlCommand command = null;
            string message = String.Empty;
            List<b_PMSchedAssign> results = null;
            data = new List<b_PMSchedAssign>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_PMSchedAssign_RetrieveByPrevMaintSchedId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_PMSchedAssign>();
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
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
        #endregion
    }
}
