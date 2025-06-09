using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.Business;
using Database.SqlClient;
using Database.StoredProcedure;

namespace Data.Database.Business
{
    public partial class b_PartCategoryMaster
    {
        #region Properties
        //V2-666
        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public Int32 offset1 { get; set; }
        public Int32 nextrow { get; set; }
        public int totalCount { get; set; }
        public string SearchText { get; set; } //V2-1068
        #endregion
        public void PartCategoryMaster_RetrieveByInactiveFlag(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName,
        ref b_PartCategoryMaster[] data
    )
        {
            ProcessRow<b_PartCategoryMaster> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_PartCategoryMaster[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new ProcessRow<b_PartCategoryMaster>(reader => { b_PartCategoryMaster obj = new b_PartCategoryMaster(); obj.LoadFromDatabase(reader); return obj; });
                results = StoredProcedure.usp_PartCategoryMaster_RetrieveByInactiveFlag.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

                // Extract the results
                if (null != results)
                {
                    data = (b_PartCategoryMaster[])results.ToArray(typeof(b_PartCategoryMaster));
                }
                else
                {
                    data = new b_PartCategoryMaster[0];
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

        public void ValidateByClientLookupId(SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            ref List<b_StoredProcValidationError> data)
        {
            SqlCommand command = null;
            string message = String.Empty;
            List<b_StoredProcValidationError> results = null;
            data = new List<b_StoredProcValidationError>();
            try
            {
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;
                results = StoredProcedure.usp_PartCategoryMaster_ValidateByClientLookupId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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


        public void RetrieveChunkSearch(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref List<b_PartCategoryMaster> results
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

                
                results = usp_PartCategoryMaster_RetrieveChunkSearch_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public static b_PartCategoryMaster ProcessRowForChunkSearch(SqlDataReader reader)
        {
            // Create instance of object
            b_PartCategoryMaster category = new b_PartCategoryMaster();
            category.LoadFromDatabaseForChunkSearch(reader);
            return category;
        }
        public int LoadFromDatabaseForChunkSearch(SqlDataReader reader)
        {
            int i = 0;
            try
            {
               // i = LoadFromDatabase(reader);
               

                // PartCategoryMasterId column, bigint, not null
                PartCategoryMasterId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);

                // Description column, nvarchar(63), not null
                Description = reader.GetString(i++);

                // InactiveFlag column, bit, not null
                InactiveFlag = reader.GetBoolean(i++);//

                // totalCount column, int, not null
                totalCount = reader.GetInt32(i++);

            }
            catch (Exception ex)
            {

                StringBuilder missing = new StringBuilder();
             

                try { reader["PartCategoryMasterId"].ToString(); }
                catch { missing.Append("PartCategoryMasterId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["InactiveFlag"].ToString(); }
                catch { missing.Append("InactiveFlag "); }

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

        #region V2-717 PartCategoryMaster Lookuplist
        public void RetrieveLookupListChunkSearch(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref List<b_PartCategoryMaster> results
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


                results = usp_PartCategoryMaster_RetrieveChunkSearchLookupList_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public static b_PartCategoryMaster ProcessRowForLookupListChunkSearch(SqlDataReader reader)
        {
            // Create instance of object
            b_PartCategoryMaster category = new b_PartCategoryMaster();
            category.LoadFromDatabaseForLookupListChunkSearch(reader);
            return category;
        }
        public int LoadFromDatabaseForLookupListChunkSearch(SqlDataReader reader)
        {
            int i = 0;
            try
            {
               
                // PartCategoryMasterId column, bigint, not null
                PartCategoryMasterId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);

                // Description column, nvarchar(63), not null
                Description = reader.GetString(i++);

                // totalCount column, int, not null
                totalCount = reader.GetInt32(i++);

            }
            catch (Exception ex)
            {

                StringBuilder missing = new StringBuilder();


                try { reader["PartCategoryMasterId"].ToString(); }
                catch { missing.Append("PartCategoryMasterId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

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
        #endregion
    }
}
