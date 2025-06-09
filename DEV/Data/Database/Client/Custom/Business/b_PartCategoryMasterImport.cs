using System;
using System.Data.SqlClient;
using System.Text;

namespace Database.Business
{
    public partial class b_PartCategoryMasterImport
    {
        public int error_message_count { get; set; }
        public void PartCategoryMasterImportProcess(
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
                Database.StoredProcedure.usp_PartCategoryMasterImport_ImportData.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public void RetrieveByClientLookUpIDFromDatabase(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName
        )
        {
            SqlClient.ProcessRow<b_PartCategoryMasterImport> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new SqlClient.ProcessRow<b_PartCategoryMasterImport>(reader => { this.LoadFromDatabaseRetrieveByClientLookUpID(reader); return this; });
                Database.StoredProcedure.usp_PartCategoryMasterImport_RetrieveByClientLookUpID_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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
        public int LoadFromDatabaseRetrieveByClientLookUpID(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // PartCategoryMasterImportId column, bigint, not null
                PartCategoryMasterImportId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(255), not null
                ClientLookupId = reader.GetString(i++);

                // Description column, nvarchar(255), not null
                Description = reader.GetString(i++);

                // InactiveFlag column, bit, not null
                InactiveFlag = reader.GetBoolean(i++);

                // ErrorMessage column, nvarchar(511), not null
                ErrorMessage = reader.GetString(i++);

                // LastProcess column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    LastProcess = reader.GetDateTime(i);
                }
                else
                {
                    LastProcess = DateTime.MinValue;
                }
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["PartCategoryMasterImportId"].ToString(); }
                catch { missing.Append("PartCategoryMasterImportId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["InactiveFlag"].ToString(); }
                catch { missing.Append("InactiveFlag "); }

                try { reader["ErrorMessage"].ToString(); }
                catch { missing.Append("ErrorMessage "); }

                try { reader["LastProcess"].ToString(); }
                catch { missing.Append("LastProcess "); }


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
    }
}
