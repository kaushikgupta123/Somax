
using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Database.Business
{
    public partial class b_VendorRequest
    {
        #region  Properties
        public int CustomQueryDisplayId { get; set; }

        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public Int32 offset1 { get; set; }
        public Int32 nextrow { get; set; }
        public string SearchText { get; set; }
        public int totalCount { get; set; }

        #endregion

        public void VendorRequestRetrieveChunkSearch(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref List<b_VendorRequest> results
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

                results = Database.StoredProcedure.usp_VendorRequest_RetrieveChunkSearch_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public static object ProcessRowForRetrieveBySearchCriteria_V2(SqlDataReader reader)
        {
            // Create instance of object
            b_VendorRequest obj = new b_VendorRequest();

            // Load the object from the database
            obj.LoadFromDatabaseForRetrieveBySearchCriteria_V2(reader);

            // Return result
            return (object)obj;
        }

        public int LoadFromDatabaseForRetrieveBySearchCriteria_V2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // VendorRequestId column, bigint, not null
                VendorRequestId = reader.GetInt64(i++);
                // Name column, nvarchar(126), not null
                Name = reader.GetString(i++);
                // AddressCity column, nvarchar(126), not null
                AddressCity = reader.GetString(i++);
                // AddressState column, nvarchar(126), not null
                AddressState = reader.GetString(i++);
                // Type column, nvarchar(30), not null
                Type = reader.GetString(i++);
                // Status column, nvarchar(30), not null
                Status = reader.GetString(i++);
                //Total Count
                totalCount = reader.GetInt32(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["VendorRequestId"].ToString(); }
                catch { missing.Append("VendorRequestId "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append("Name "); }

                try { reader["AddressCity"].ToString(); }
                catch { missing.Append("AddressCity "); }

                try { reader["AddressState"].ToString(); }
                catch { missing.Append("AddressState "); }

                try { reader["Type"].ToString(); }
                catch { missing.Append("Type "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["totalCount"].ToString(); }
                catch { missing.Append("totalCount "); }

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