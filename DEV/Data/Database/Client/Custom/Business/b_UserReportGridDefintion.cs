using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Database.Business
{
  
    public partial class b_UserReportGridDefintion
    {
        #region properties
        //public Decimal NumofDecPlaces { get; set; }

        //public string NumericFormat { get; set; }
        //public string FilterMethod { get; set; }

        public System.Data.DataTable UserReportList { get; set; }
        #endregion
        public void RetrieveByReportIdFromDatabase_V2(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName
    )
        {
            Database.SqlClient.ProcessRow<b_UserReportGridDefintion> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_UserReportGridDefintion>(reader => { this.LoadFromDatabase(reader); return this; });
                StoredProcedure.usp_UserReportGridDefintion_RetrieveByReportId_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
                processRow = null;
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }


        public void RetrieveByReportId(
       SqlConnection connection,
       SqlTransaction transaction,
       long callerUserInfoId,
 string callerUserName,
          ref List<b_UserReportGridDefintion> data
   )
        {
            Database.SqlClient.ProcessRow<b_UserReportGridDefintion> processRow = null;
            List<b_UserReportGridDefintion> results = null;
            SqlCommand command = null;
            string message = String.Empty;
            // Initialize the results
            data = new List<b_UserReportGridDefintion>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_UserReportGridDefintion>(reader => { b_UserReportGridDefintion obj = new b_UserReportGridDefintion(); obj.LoadFromDatabaseRetrieveByReportId(reader); return obj; });
                results = Database.StoredProcedure.usp_UserReportGridDefintion_RetrieveByReportId.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

                // Extract the results
                if (null != results)
                {
                    data = results;
                }
                else
                {
                    data = null;
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
                processRow = null;
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }




        public int LoadFromDatabaseRetrieveByReportId(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // UserReportGridDefintionId column, bigint, not null
                UserReportGridDefintionId = reader.GetInt64(i++);

                // ReportId column, bigint, not null
                ReportId = reader.GetInt64(i++);

                // Sequence column, int, not null
                Sequence = reader.GetInt32(i++);

                // Columns column, nvarchar(30), not null
                Columns = reader.GetString(i++);

                // Heading column, nvarchar(30), not null
                Heading = reader.GetString(i++);

                // Alignment column, nvarchar(15), not null
                Alignment = reader.GetString(i++);

                // IsGroupTotaled column, bit, not null
                IsGroupTotaled = reader.GetBoolean(i++);

                // IsGrandTotal column, bit, not null
                IsGrandTotal = reader.GetBoolean(i++);

                // LocalizeDate column, bit, not null
                LocalizeDate = reader.GetBoolean(i++);

                // IsChildColumn column, bit, not null
                IsChildColumn = reader.GetBoolean(i++);
                // Display column, bit, not null
                Display = reader.GetBoolean(i++);

                // Required column, bit, not null
                Required = reader.GetBoolean(i++);

                // AvailableonFilter column, bit, not null
                AvailableonFilter = reader.GetBoolean(i++);

                // Filter column, string, not null
                Filter = reader.GetString(i++);

                // NumofDecPlaces column, int, not null
                NumofDecPlaces = reader.GetInt32(i++);

                // NumericFormat column, string, not null
                NumericFormat = reader.GetString(i++);

                // FilterMethod column, string, not null
                FilterMethod = reader.GetString(i++);

                // DateDisplay column, bit, not null
                DateDisplay = reader.GetBoolean(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["UserReportGridDefintionId"].ToString(); }
                catch { missing.Append("UserReportGridDefintionId "); }

                try { reader["ReportId"].ToString(); }
                catch { missing.Append("ReportId "); }

                try { reader["Sequence"].ToString(); }
                catch { missing.Append("Sequence "); }

                try { reader["Columns"].ToString(); }
                catch { missing.Append("Columns "); }

                try { reader["Heading"].ToString(); }
                catch { missing.Append("Heading "); }

                try { reader["Alignment"].ToString(); }
                catch { missing.Append("Alignment "); }

                try { reader["Display"].ToString(); }
                catch { missing.Append("Display "); }

                try { reader["Required"].ToString(); }
                catch { missing.Append("Required "); }

                try { reader["AvailableonFilter"].ToString(); }
                catch { missing.Append("AvailableonFilter "); }

                try { reader["IsGroupTotaled"].ToString(); }
                catch { missing.Append("IsGroupTotaled "); }

                try { reader["IsGrandTotal"].ToString(); }
                catch { missing.Append("IsGrandTotal "); }

                try { reader["LocalizeDate"].ToString(); }
                catch { missing.Append("LocalizeDate "); }

                try { reader["IsChildColumn"].ToString(); }
                catch { missing.Append("IsChildColumn "); }


                try { reader["DateDisplay"].ToString(); }
                catch { missing.Append("DateDisplay "); }

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

        public void UpdateByReportId(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName
)
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                Database.StoredProcedure.usp_UserReportGridDefinition_UpdateByReportId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
            }
        }


        public void RetrieveAllByReportId_V2(
  SqlConnection connection,
  SqlTransaction transaction,
  long callerUserInfoId,
string callerUserName,
     ref List<b_UserReportGridDefintion> data
)
        {
            Database.SqlClient.ProcessRow<b_UserReportGridDefintion> processRow = null;
            List<b_UserReportGridDefintion> results = null;
            SqlCommand command = null;
            string message = String.Empty;
            // Initialize the results
            data = new List<b_UserReportGridDefintion>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_UserReportGridDefintion>(reader => { b_UserReportGridDefintion obj = new b_UserReportGridDefintion(); obj.LoadFromDatabaseRetrieveAllByReportId(reader); return obj; });
                results = Database.StoredProcedure.usp_UserReportGridDefintion_RetrieveAllByReportId_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

                // Extract the results
                if (null != results)
                {
                    data = results;
                }
                else
                {
                    data = null;
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
                processRow = null;
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }

        public int LoadFromDatabaseRetrieveAllByReportId(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // UserReportGridDefintionId column, bigint, not null
                UserReportGridDefintionId = reader.GetInt64(i++);

                // ReportId column, bigint, not null
                ReportId = reader.GetInt64(i++);

                // Sequence column, int, not null
                Sequence = reader.GetInt32(i++);

                // Columns column, nvarchar(30), not null
                Columns = reader.GetString(i++);

                // Heading column, nvarchar(30), not null
                Heading = reader.GetString(i++);

                // Alignment column, nvarchar(15), not null
                Alignment = reader.GetString(i++);
                // IsGroupTotaled column, bit, not null
                IsGroupTotaled = reader.GetBoolean(i++);

                // IsGrandTotal column, bit, not null
                IsGrandTotal = reader.GetBoolean(i++);

                // LocalizeDate column, bit, not null
                LocalizeDate = reader.GetBoolean(i++);
                // NumofDecPlaces column, int, not null
                NumofDecPlaces = reader.GetInt32(i++);

                // NumericFormat column, nvarchar(15), not null
                NumericFormat = reader.GetString(i++);

                // IsChildColumn column, bit, not null
                IsChildColumn = reader.GetBoolean(i++);

                // Display column, bit, not null
                Display = reader.GetBoolean(i++);

                // Required column, bit, not null
                Required = reader.GetBoolean(i++);

                // AvailableonFilter column, bit, not null
                AvailableonFilter = reader.GetBoolean(i++);        
            
                // FilterMethod column, nvarchar(15), not null
                FilterMethod = reader.GetString(i++);

                // Filter column, nvarchar(max), not null
                Filter = reader.GetString(i++);

                // DateDisplay column, bit, not null
                DateDisplay = reader.GetBoolean(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["UserReportGridDefintionId"].ToString(); }
                catch { missing.Append("UserReportGridDefintionId "); }

                try { reader["ReportId"].ToString(); }
                catch { missing.Append("ReportId "); }

                try { reader["Sequence"].ToString(); }
                catch { missing.Append("Sequence "); }

                try { reader["Columns"].ToString(); }
                catch { missing.Append("Columns "); }

                try { reader["Heading"].ToString(); }
                catch { missing.Append("Heading "); }

                try { reader["Alignment"].ToString(); }
                catch { missing.Append("Alignment "); }

                try { reader["IsGroupTotaled"].ToString(); }
                catch { missing.Append("IsGroupTotaled "); }

                try { reader["IsGrandTotal"].ToString(); }
                catch { missing.Append("IsGrandTotal "); }

                try { reader["LocalizeDate"].ToString(); }
                catch { missing.Append("LocalizeDate "); }

                try { reader["NumofDecPlaces"].ToString(); }
                catch { missing.Append("NumofDecPlaces "); }

                try { reader["NumericFormat"].ToString(); }
                catch { missing.Append("NumericFormat "); }

                try { reader["IsChildColumn"].ToString(); }
                catch { missing.Append("IsChildColumn "); }

                try { reader["Display"].ToString(); }
                catch { missing.Append("Display "); }

                try { reader["Required"].ToString(); }
                catch { missing.Append("Required "); }

                try { reader["AvailableonFilter"].ToString(); }
                catch { missing.Append("AvailableonFilter "); }
            
                try { reader["FilterMethod"].ToString(); }
                catch { missing.Append("FilterMethod "); }

                try { reader["Filter"].ToString(); }
                catch { missing.Append("Filter "); }

                try { reader["DateDisplay"].ToString(); }
                catch { missing.Append("DateDisplay "); }

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

        public void DeleteByReportId(
      SqlConnection connection,
      SqlTransaction transaction,
      long callerUserInfoId,
      string callerUserName
)
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                Database.StoredProcedure.usp_UserReportGridDefinition_DeleteByReportId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
            }
        }
        public void UpdateFilterByReportId(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName
)
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                Database.StoredProcedure.usp_UserReportGridDefinition_UpdateFilterByReportId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
            }
        }
    }
}
