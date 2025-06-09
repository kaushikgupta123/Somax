using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.StoredProcedure;

namespace Database.Business
{
    public partial class b_ShipTo
    {
        #region Properties
        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public Int32 offset1 { get; set; }
        public Int32 nextrow { get; set; }
        public int totalCount { get; set; }
        public string SearchText { get; set; }
        #endregion
        public void RetrieveChunkSearch(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName,
        ref List<b_ShipTo> results
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


                results = usp_ShipTo_RetrieveChunkSearch_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public static b_ShipTo ProcessRowForChunkSearch(SqlDataReader reader)
        {
            // Create instance of object
            b_ShipTo shipTo = new b_ShipTo();
            shipTo.LoadFromDatabaseForChunkSearch(reader);
            return shipTo;
        }
        public int LoadFromDatabaseForChunkSearch(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // ShipToId column, bigint, not null
                ShipToId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(63), not null
                ClientLookupId = reader.GetString(i++);

                // AttnName column, nvarchar(63), not null
                AttnName = reader.GetString(i++);

                // Address1 column, nvarchar(63), not null
                Address1 = reader.GetString(i++);

                // AddressCity column, nvarchar(63), not null
                AddressCity = reader.GetString(i++);

                // AddressState column, nvarchar(63), not null
                AddressState = reader.GetString(i++);

                // totalCount column, int, not null
                totalCount = reader.GetInt32(i++);

            }
            catch (Exception ex)
            {

                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["ShipToId"].ToString(); }
                catch { missing.Append("ShipToId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["AttnName"].ToString(); }
                catch { missing.Append("AttnName "); }

                try { reader["Address1"].ToString(); }
                catch { missing.Append("Address1 "); }

                try { reader["AddressCity"].ToString(); }
                catch { missing.Append("AddressCity "); }

                try { reader["AddressState"].ToString(); }
                catch { missing.Append("AddressState "); }

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
                results = usp_ShipTo_ValidateByClientLookupId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
    }
}
