using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using Database.SqlClient;
using System.Collections.Generic;

namespace Database.Business
{
    public partial class b_MemorizeSearch
    {
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public bool IsClear { get; set; }

        public static b_MemorizeSearch ProcessRowForMemorizeSearch_RetrieveByObject(SqlDataReader reader)
        {
            // Create instance of object
            b_MemorizeSearch MemorizeSearchdata = new b_MemorizeSearch();

            // Load the object from the database
            MemorizeSearchdata.LoadFromDatabaseForMemorizeSearch_RetrieveByObject(reader);

            // Return result
            return MemorizeSearchdata;
        }


        public void LoadFromDatabaseForMemorizeSearch_RetrieveByObject(SqlDataReader reader)
        {
            int i = 0;
            try
            {


                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // SiteId column, bigint, not null
                SiteId = reader.GetInt64(i++);

                // SearchText column, nvarchar(500), not null
                SearchText = reader.GetString(i++);

                // ObjectName column, nvarchar(50), not null
                ObjectName = reader.GetString(i++);

                CreateDate = reader.GetDateTime(i++);

                CreateBy= reader.GetString(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }               

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["SearchText"].ToString(); }
                catch { missing.Append("SearchText "); }

                try { reader["ObjectName"].ToString(); }
                catch { missing.Append("ObjectName "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["CreateBy"].ToString(); }
                catch { missing.Append("CreateBy "); }


                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }

        public void RetrieveForSearch(
    SqlConnection connection,
    SqlTransaction transaction,
    long callerUserInfoId,
    string callerUserName,
    ref List<b_MemorizeSearch> results

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

                results = Database.StoredProcedure.usp_MemorizeSearch_RetrieveByObject.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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



        public void RetrieveafterCreateAndDelete(
  SqlConnection connection,
  SqlTransaction transaction,
  long callerUserInfoId,
  string callerUserName,
  ref List<b_MemorizeSearch> results

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

                results = Database.StoredProcedure.usp_MemorizeSearch_CreateAndDelete.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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