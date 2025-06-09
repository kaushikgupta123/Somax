using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Business
{
    public partial class b_ServiceOrderEventLog
    {
        #region Property
        public string PersonnelClientLookupId { get; set; }
        public string NameFirst { get; set; }
        public string NameLast { get; set; }
        public string NameMiddle { get; set; }

        public string PersonnelInitial
        {
            get
            {
                string InitFirstName = NameFirst.Trim().Substring(0, 1);
                string InitLastName = NameLast.Trim().Substring(0, 1);
                return (InitFirstName + InitLastName);
            }
        }
        #endregion

        public static object ProcessRowByServiceOrderId(SqlDataReader reader)
        {
            // Create instance of object
            b_ServiceOrderEventLog obj = new b_ServiceOrderEventLog();

            // Load the object from the database
            obj.LoadFromDatabaseCustom(reader);

            // Return result
            return (object)obj;
        }
        private int LoadFromDatabaseCustom(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // Event column, nvarchar(15), not null
                Event = reader.GetString(i++);
                // PersonnelId column, bigint, not null
                PersonnelId = reader.GetInt64(i++);
                // TransactionDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    TransactionDate = reader.GetDateTime(i);
                }
                else
                {
                    TransactionDate = DateTime.MinValue;
                }
                i++;
                // Comments column, nvarchar(MAX), not null
                Comments = reader.GetString(i++);
                // PersonnelClientLookupId column, nvarchar(MAX), not null
                PersonnelClientLookupId = reader.GetString(i++);
                // NameLast column, nvarchar(MAX), not null
                NameLast = reader.GetString(i++);
                // NameFirst column, nvarchar(MAX), not null
                NameFirst = reader.GetString(i++);
                // Comments column, nvarchar(MAX), not null
                NameMiddle = reader.GetString(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["Event"].ToString(); }
                catch { missing.Append("Event "); }

                try { reader["PersonnelId"].ToString(); }
                catch { missing.Append("PersonnelId "); }

                try { reader["TransactionDate"].ToString(); }
                catch { missing.Append("TransactionDate "); }

                try { reader["Comments"].ToString(); }
                catch { missing.Append("Comments "); }

                try { reader["PersonnelClientLookupId"].ToString(); }
                catch { missing.Append("PersonnelClientLookupId "); }

                try { reader["NameLast"].ToString(); }
                catch { missing.Append("NameLast "); }

                try { reader["NameFirst"].ToString(); }
                catch { missing.Append("NameFirst "); }

                try { reader["NameMiddle"].ToString(); }
                catch { missing.Append("NameMiddle "); }

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
        public void RetrieveByServiceOrderId(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            ref List<b_ServiceOrderEventLog> data
        )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_ServiceOrderEventLog> results = null;
            data = new List<b_ServiceOrderEventLog>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_ServiceOrderEventLog_RetrieveByServiceOrderId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_ServiceOrderEventLog>();
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
    }
}
