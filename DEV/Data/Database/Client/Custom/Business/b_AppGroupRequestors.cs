using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Database.Business
{
    public partial class b_AppGroupRequestors
    {
        #region Properties
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int OffSetVal { get; set; }
        public int NextRow { get; set; }
        public Int32 TotalCount { get; set; }
        public string RequestorName { get; set; }
        public List<b_AppGroupRequestors> listOfAppGroupRequestors { get; set; }
        public string RequestType { get; set; }
        public long SiteId { get; set; }
        #endregion

        #region Details
        public void RetrieveChunkSearchFromDetails(
   SqlConnection connection,
   SqlTransaction transaction,
   long callerUserInfoId,
   string callerUserName,
   ref List<b_AppGroupRequestors> results
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

                results = Database.StoredProcedure.usp_AppGroupRequestors_RetrieveChunkSearchForDetails_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName,this);

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

        public static b_AppGroupRequestors ProcessRowForRetrieveChunkSearchFromDetails(SqlDataReader reader)
        {
            b_AppGroupRequestors AppGroupRequestors = new b_AppGroupRequestors();

            AppGroupRequestors.LoadFromDatabaseForRetrieveChunkSearchFromDetails(reader);
            return AppGroupRequestors;
        }

        public int LoadFromDatabaseForRetrieveChunkSearchFromDetails(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                AppGroupRequestorsId = reader.GetInt64(i++);

                if (false == reader.IsDBNull(i))
                {
                    RequestorName = reader.GetString(i);
                }
                else
                {
                    RequestorName = "";
                }
                i++;             
                
                TotalCount = reader.GetInt32(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["AppGroupRequestorsId"].ToString(); }
                catch { missing.Append("AppGroupRequestorsId "); }

                try { reader["PersonnelName"].ToString(); }
                catch { missing.Append("PersonnelName "); }

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

            return i;
        }

        #endregion
        #region   Retrieve By Requestor Id And RequestType
        public void RetrieveByRequestorIdAndRequestType(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref b_AppGroupRequestors results
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

                results = Database.StoredProcedure.usp_AppGroupRequestors_RetrieveByRequestorIdAndRequestType_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public static b_AppGroupRequestors ProcessRowForRetrieveByRequestorIdAndRequestType(SqlDataReader reader)
        {
            b_AppGroupRequestors AppGroupRequestors = new b_AppGroupRequestors();

            AppGroupRequestors.LoadFromDatabaseForRetrieveByRequestorIdAndRequestType(reader);
            return AppGroupRequestors;
        }

        public int LoadFromDatabaseForRetrieveByRequestorIdAndRequestType(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                if (false == reader.IsDBNull(i))
                {
                    ApprovalGroupId = reader.GetInt64(i);
                }
                else
                {
                    ApprovalGroupId = 0;
                }
                i++;

              
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ApprovalGroupId"].ToString(); }
                catch { missing.Append("ApprovalGroupId "); }

               

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
    }
}
