using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Database.Business
{
    public partial class b_AppGroupApprovers
    {
        #region Properties
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int OffSetVal { get; set; }
        public int NextRow { get; set; }
        public Int32 TotalCount { get; set; }
        public string ApproverName { get; set; }
        public List<b_AppGroupApprovers> listOfAppGroupApprovers { get; set; }
        public string LevelName { get; set; }
        #region V2-726
        public long RequestorId { get; set; }
        public long SiteId { get; set; }
        public string RequestType { get; set; }
        #endregion
        #region V2-730
        public long ObjectId { get; set; }
        #endregion
        #endregion

        #region Child Grid
        public void RetrieveAppGroupApproversChildGridV2(
   SqlConnection connection,
   SqlTransaction transaction,
   long callerUserInfoId,
   string callerUserName,
   ref b_AppGroupApprovers results
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

                results = Database.StoredProcedure.usp_AppGroupApprovers_RetrieveByApprovalGroupId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        public static b_AppGroupApprovers ProcessRetrieveAppGroupApproversChildGridV2(SqlDataReader reader)
        {
            b_AppGroupApprovers AppGroupApprovers = new b_AppGroupApprovers();

            AppGroupApprovers.LoadFromDatabaseForAppGroupApproversChildGridV2(reader);
            return AppGroupApprovers;
        }

        public int LoadFromDatabaseForAppGroupApproversChildGridV2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                AppGroupApproversId = reader.GetInt64(i++);
                Limit = reader.GetDecimal(i++);
                Level = reader.GetInt32(i++);
                if (false == reader.IsDBNull(i))
                {
                    ApproverName = reader.GetString(i);
                }
                else
                {
                    ApproverName = "";
                }
                i++;
                TotalCount = reader.GetInt32(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["AppGroupApproversId"].ToString(); }
                catch { missing.Append("AppGroupApproversId "); }

                try { reader["Limit"].ToString(); }
                catch { missing.Append("Limit "); }

                try { reader["Level"].ToString(); }
                catch { missing.Append("Level "); }

                try { reader["ApproverName"].ToString(); }
                catch { missing.Append("ApproverName "); }

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

        #region Details
        public void RetrieveChunkSearchFromDetails(
   SqlConnection connection,
   SqlTransaction transaction,
   long callerUserInfoId,
   string callerUserName,
   ref List<b_AppGroupApprovers> results
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

                results = Database.StoredProcedure.usp_AppGroupApprovers_RetrieveChunkSearchForDetails_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName,this);

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

        public static b_AppGroupApprovers ProcessRowForRetrieveChunkSearchFromDetails(SqlDataReader reader)
        {
            b_AppGroupApprovers AppGroupApprovers = new b_AppGroupApprovers();

            AppGroupApprovers.LoadFromDatabaseForRetrieveChunkSearchFromDetails(reader);
            return AppGroupApprovers;
        }

        public int LoadFromDatabaseForRetrieveChunkSearchFromDetails(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                AppGroupApproversId = reader.GetInt64(i++);

                ApproverId = reader.GetInt64(i++);

                if (false == reader.IsDBNull(i))
                {
                    ApproverName = reader.GetString(i);
                }
                else
                {
                    ApproverName = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Limit = reader.GetDecimal(i);
                }
                else
                {
                    Limit = 0;
                }
                i++;

                Level = reader.GetInt32(i++);

                LevelName = reader.GetString(i++);

                TotalCount = reader.GetInt32(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["AppGroupApproversId"].ToString(); }
                catch { missing.Append("AppGroupApproversId "); }

                try { reader["PersonnelName"].ToString(); }
                catch { missing.Append("PersonnelName "); }

                try { reader["Limit"].ToString(); }
                catch { missing.Append("Limit "); }

                try { reader["Level"].ToString(); }
                catch { missing.Append("Level "); }

                try { reader["LevelName"].ToString(); }
                catch { missing.Append("LevelName "); }

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

       
        public void RetrieveByIdFromDatabase_V2(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName
        )
        {
            Database.SqlClient.ProcessRow<b_AppGroupApprovers> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_AppGroupApprovers>(reader => { this.LoadFromDatabaseById_V2(reader); return this; });
                Database.StoredProcedure.usp_AppGroupApprovers_RetrieveById_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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

        public int LoadFromDatabaseById_V2(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // AppGroupApproversId column, bigint, not null
                AppGroupApproversId = reader.GetInt64(i++);

                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // ApproverId column, bigint, not null
                ApproverId = reader.GetInt64(i++);

                ApproverName = reader.GetString(i++);

                // ApprovalGroupId column, bigint, not null
                ApprovalGroupId = reader.GetInt64(i++);

                // Level column, int, not null
                Level = reader.GetInt32(i++);
                LevelName=reader.GetString(i++);
                // Limit column, decimal(9,2), not null
                Limit = reader.GetDecimal(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["AppGroupApproversId"].ToString(); }
                catch { missing.Append("AppGroupApproversId "); }

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["ApproverId"].ToString(); }
                catch { missing.Append("ApproverId "); }

                try { reader["ApprovalGroupId"].ToString(); }
                catch { missing.Append("ApprovalGroupId "); }

                try { reader["Level"].ToString(); }
                catch { missing.Append("Level "); }

                try { reader["Limit"].ToString(); }
                catch { missing.Append("Limit "); }


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

        #region V2-726
        public void RetrieveApproversForApproval(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, b_AppGroupApprovers obj, ref List<b_AppGroupApprovers> results)
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

                results = Database.StoredProcedure.usp_AppGroupApprovers_RetrieveByRequestorIdAndRequestType_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, obj);

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
        public static b_AppGroupApprovers ProcessRowForFilterApprover(SqlDataReader reader)
        {
            // Create instance of object
            b_AppGroupApprovers obj = new b_AppGroupApprovers();

            // Load the object from the database
            obj.LoadFromDatabaseForFilterApprover(reader);

            // Return result
            return obj;
        }
        public void LoadFromDatabaseForFilterApprover(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // PersonnelId column, bigint, not null
                ApproverId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(63), not null
                ApproverName = reader.GetString(i++);

            }
            catch (Exception ex)
            {

                StringBuilder missing = new StringBuilder();


                try { reader["ApproverId"].ToString(); }
                catch { missing.Append("ApproverId "); }

                try { reader["ApproverName"].ToString(); }
                catch { missing.Append("ApproverName "); }


                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }

        #endregion

        #region V2-720
        public void ValidateUpperLevelExists(
 SqlConnection connection,
 SqlTransaction transaction,
 long callerUserInfoId,
 string callerUserName,
 ref List<b_StoredProcValidationError> data
)
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_StoredProcValidationError> results = null;


            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_AppGroupApprovers_ValidateUpperLevelExists_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_StoredProcValidationError>();
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

        public void ValidateApproverId(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref List<b_StoredProcValidationError> data
)
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_StoredProcValidationError> results = null;


            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_AppGroupApprovers_ValidateApproverId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_StoredProcValidationError>();
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

        #region V2-730
        public void RetrieveApproversForMultilevelApproval(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, b_AppGroupApprovers obj, ref List<b_AppGroupApprovers> results)
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

                results = Database.StoredProcedure.usp_AppGroupApprovers_RetrieveListForMultiLevelApproval_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, obj);

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
        public static b_AppGroupApprovers ProcessRowForForMultilevelApprover(SqlDataReader reader)
        {
            // Create instance of object
            b_AppGroupApprovers obj = new b_AppGroupApprovers();

            // Load the object from the database
            obj.LoadFromDatabaseForForMultilevelApprover(reader);

            // Return result
            return obj;
        }
        public void LoadFromDatabaseForForMultilevelApprover(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // PersonnelId column, bigint, not null
                ApproverId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(63), not null
                ApproverName = reader.GetString(i++);

            }
            catch (Exception ex)
            {

                StringBuilder missing = new StringBuilder();


                try { reader["ApproverId"].ToString(); }
                catch { missing.Append("ApproverId "); }

                try { reader["ApproverName"].ToString(); }
                catch { missing.Append("ApproverName "); }


                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }

        #endregion
    }
}
