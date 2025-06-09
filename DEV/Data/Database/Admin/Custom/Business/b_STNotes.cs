using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;

namespace Database.Business
{
    public partial class b_STNotes
    {
        #region Property
        public DateTime ModifyDate { get; set; }
        public DateTime CreateDate { get; set; }
        #endregion
        public void RetrieveBySupportTicketIdFromDatabase(
         SqlConnection connection,
         SqlTransaction transaction,
         long callerUserInfoId,
   string callerUserName,
         ref b_STNotes[] data
     )
        {
            Database.SqlClient.ProcessRow<b_STNotes> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_STNotes[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_STNotes>(reader => { b_STNotes obj = new b_STNotes(); obj.LoadExtendedFromDatabase(reader); return obj; });
                results = Database.StoredProcedure.usp_STNotes_RetrieveBySupportTicketId.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, SupportTicketId);

                // Extract the results
                if (null != results)
                {
                    data = (b_STNotes[])results.ToArray(typeof(b_STNotes));
                }
                else
                {
                    data = new b_STNotes[0];
                }

                // Clear the results collection
                if (null != results)
                {
                    results.Clear();
                    results = null;
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
                results = null;
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
        public void LoadExtendedFromDatabase(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // STNotesId column, bigint, not null
                STNotesId = reader.GetInt64(i++);

                // SupportTicketId column, bigint, not null
                SupportTicketId = reader.GetInt64(i++);

                // Type column, nvarchar(15), not null
                Type = reader.GetString(i++);

                // Content column, nvarchar(max), not null
                Content = reader.GetString(i++);

                // From_PersonnelId column, bigint, not null
                From_PersonnelId = reader.GetInt64(i++);

                // ModifyDate column, datetime2, not null
                ModifyDate = reader.GetDateTime(i++);

                // CreateDate column, datetime2, not null
                CreateDate = reader.GetDateTime(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["STNotesId"].ToString(); }
                catch { missing.Append("STNotesId "); }

                try { reader["SupportTicketId"].ToString(); }
                catch { missing.Append("SupportTicketId "); }

                try { reader["Type"].ToString(); }
                catch { missing.Append("Type "); }

                try { reader["Content"].ToString(); }
                catch { missing.Append("Content "); }

                try { reader["From_PersonnelId"].ToString(); }
                catch { missing.Append("From_PersonnelId "); }

                try { reader["ModifyDate"].ToString(); }
                catch { missing.Append("ModifyDate "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }
    }
}
