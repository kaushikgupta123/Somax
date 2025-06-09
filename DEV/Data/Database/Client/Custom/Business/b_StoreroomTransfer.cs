using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Business
{
    public partial class b_StoreroomTransfer
    {
        #region Properties
        public Int64 PartId { get; set; }
        public string PartClientLookupId { get; set; }
        public string PartDescription { get; set; }
        public string StoreroomName { get; set; }
        public int CustomQueryDisplayId { get; set; }
        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public string offset1 { get; set; }
        public string nextrow { get; set; }
        public string SearchText { get; set; }
        public int TotalCount { get; set; }
        public Int64 PersonnelId { get; set; }
        #endregion

        public void RetrieveChunkSearch(
    SqlConnection connection,
    SqlTransaction transaction,
    long callerUserInfoId,
    string callerUserName,
    ref List<b_StoreroomTransfer> results
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

                results = Database.StoredProcedure.usp_StoreroomTransfer_RetrieveForChunkSearch_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public static b_StoreroomTransfer ProcessRowForStoreroomTransferRetriveForChunkSearch(SqlDataReader reader)
        {
            // Create instance of object
            b_StoreroomTransfer StoreroomTransfer = new b_StoreroomTransfer();
            StoreroomTransfer.LoadFromDatabaseForStoreroomTransferRetriveForChunkSearch(reader);
            return StoreroomTransfer;
        }

        public int LoadFromDatabaseForStoreroomTransferRetriveForChunkSearch(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // ClientId column, bigint, not null              
                if (false == reader.IsDBNull(i))
                {
                    ClientId = reader.GetInt64(i);
                }
                else
                {
                    ClientId = 0;
                }
                i++;
                // ClientId column, bigint, not null              
                if (false == reader.IsDBNull(i))
                {
                    SiteId = reader.GetInt64(i);
                }
                else
                {
                    SiteId = 0;
                }
                i++;
                // StoreroomTransferId column, bigint, not null             
                if (false == reader.IsDBNull(i))
                {
                    StoreroomTransferId = reader.GetInt64(i);
                }
                else
                {
                    StoreroomTransferId = 0;
                }
                i++;
                // PartId column, bigint, not null             
                if (false == reader.IsDBNull(i))
                {
                    PartId = reader.GetInt64(i);
                }
                else
                {
                    PartId = 0;
                }
                i++;
                // PartClientLookupId column, nvarchar(200), not null
                if (false == reader.IsDBNull(i))
                {
                    PartClientLookupId = reader.GetString(i);
                }
                else
                {
                    PartClientLookupId = string.Empty;
                }
                i++;
                // PartDescription column, nvarchar(200), not null
                if (false == reader.IsDBNull(i))
                {
                    PartDescription = reader.GetString(i);
                }
                else
                {
                    PartDescription = string.Empty;
                }
                i++;
                // QuantityIssued column, datetime, not null               
                if (false == reader.IsDBNull(i))
                {
                    QuantityIssued = reader.GetDecimal(i);
                }
                else
                {
                    QuantityIssued = 0;
                }
                i++;
                // Status column, nvarchar(15), not null               
                if (false == reader.IsDBNull(i))
                {
                    Status = reader.GetString(i);
                }
                else
                {
                    Status = string.Empty;
                }
                i++;
                // QuantityReceived column, datetime, not null               
                if (false == reader.IsDBNull(i))
                {
                    QuantityReceived = reader.GetDecimal(i);
                }
                else
                {
                    QuantityReceived = 0;
                }
                i++;
                // StoreroomName column, nvarchar(15), not null               
                if (false == reader.IsDBNull(i))
                {
                    StoreroomName = reader.GetString(i);
                }
                else
                {
                    StoreroomName = string.Empty;
                }
                i++;

                //TotalCount
                TotalCount = reader.GetInt32(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["StoreroomTransferId"].ToString(); }
                catch { missing.Append("StoreroomTransferId "); }

                try { reader["PartId"].ToString(); }
                catch { missing.Append("PartId "); }

                try { reader["PartClientLookupId"].ToString(); }
                catch { missing.Append("PartClientLookupId "); }

                try { reader["PartDescription"].ToString(); }
                catch { missing.Append("PartDescription "); }

                try { reader["QuantityIssued"].ToString(); }
                catch { missing.Append("QuantityIssued "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["QuantityReceived"].ToString(); }
                catch { missing.Append("QuantityReceived "); }

                try { reader["StoreroomName"].ToString(); }
                catch { missing.Append("StoreroomName "); }

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
        

        #region Outgoing Transfer
        public void RetrieveOutgoingTransferChunkSearch(
   SqlConnection connection,
   SqlTransaction transaction,
   long callerUserInfoId,
   string callerUserName,
   ref List<b_StoreroomTransfer> results
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

                results = Database.StoredProcedure.usp_StoreroomTransfer_RetrieveForOutgoingChunkSearch_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public static b_StoreroomTransfer ProcessRowForStoreroomTransferRetriveForOutgoingTransferChunkSearch(SqlDataReader reader)
        {
            // Create instance of object
            b_StoreroomTransfer StoreroomTransfer = new b_StoreroomTransfer();
            StoreroomTransfer.LoadFromDatabaseForStoreroomTransferRetriveForOutgoingTransferChunkSearch(reader);
            return StoreroomTransfer;
        }

        public int LoadFromDatabaseForStoreroomTransferRetriveForOutgoingTransferChunkSearch(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // ClientId column, bigint, not null              
                if (false == reader.IsDBNull(i))
                {
                    ClientId = reader.GetInt64(i);
                }
                else
                {
                    ClientId = 0;
                }
                i++;
                // ClientId column, bigint, not null              
                if (false == reader.IsDBNull(i))
                {
                    SiteId = reader.GetInt64(i);
                }
                else
                {
                    SiteId = 0;
                }
                i++;
                // StoreroomTransferId column, bigint, not null             
                if (false == reader.IsDBNull(i))
                {
                    StoreroomTransferId = reader.GetInt64(i);
                }
                else
                {
                    StoreroomTransferId = 0;
                }
                i++;
                // PartId column, bigint, not null             
                if (false == reader.IsDBNull(i))
                {
                    PartId = reader.GetInt64(i);
                }
                else
                {
                    PartId = 0;
                }
                i++;
                // PartClientLookupId column, nvarchar(200), not null
                if (false == reader.IsDBNull(i))
                {
                    PartClientLookupId = reader.GetString(i);
                }
                else
                {
                    PartClientLookupId = string.Empty;
                }
                i++;
                // PartDescription column, nvarchar(200), not null
                if (false == reader.IsDBNull(i))
                {
                    PartDescription = reader.GetString(i);
                }
                else
                {
                    PartDescription = string.Empty;
                }
                i++;
                // RequestQuantity column, datetime, not null               
                if (false == reader.IsDBNull(i))
                {
                    RequestQuantity = reader.GetDecimal(i);
                }
                else
                {
                    RequestQuantity = 0;
                }
                i++;
                // Status column, nvarchar(15), not null               
                if (false == reader.IsDBNull(i))
                {
                    Status = reader.GetString(i);
                }
                else
                {
                    Status = string.Empty;
                }
                i++;
                // QuantityIssued column, datetime, not null               
                if (false == reader.IsDBNull(i))
                {
                    QuantityIssued = reader.GetDecimal(i);
                }
                else
                {
                    QuantityIssued = 0;
                }
                i++;
                // StoreroomName column, nvarchar(15), not null               
                if (false == reader.IsDBNull(i))
                {
                    StoreroomName = reader.GetString(i);
                }
                else
                {
                    StoreroomName = string.Empty;
                }
                i++;

                //TotalCount
                TotalCount = reader.GetInt32(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["StoreroomTransferId"].ToString(); }
                catch { missing.Append("StoreroomTransferId "); }

                try { reader["PartId"].ToString(); }
                catch { missing.Append("PartId "); }

                try { reader["PartClientLookupId"].ToString(); }
                catch { missing.Append("PartClientLookupId "); }

                try { reader["PartDescription"].ToString(); }
                catch { missing.Append("PartDescription "); }

                try { reader["RequestQuantity"].ToString(); }
                catch { missing.Append("RequestQuantity "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["QuantityIssued"].ToString(); }
                catch { missing.Append("QuantityIssued "); }

                try { reader["StoreroomName"].ToString(); }
                catch { missing.Append("StoreroomName "); }

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

        public void ValidateForIssueProcess(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            System.Data.DataTable lulist,
            ref List<b_StoredProcValidationError> data
        )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_StoredProcValidationError> results = null;
            data = new List<b_StoredProcValidationError>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = StoredProcedure.usp_StoreroomTransfer_ValidateForIssueProcess_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this, lulist);

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

        #region Incoming Transfer
        public void ValidateForReceiptProcess(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            System.Data.DataTable lulist,
            ref List<b_StoredProcValidationError> data
        )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_StoredProcValidationError> results = null;
            data = new List<b_StoredProcValidationError>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = StoredProcedure.usp_StoreroomTransfer_ValidateForReceiptProcess_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this, lulist);

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

        #region Create AutoGenerate  V2-1059
        public void StoreroomTransferAutoGeneration_V2(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName,
         System.Data.DataTable lulist)
        {
            SqlCommand command = null;
            try
            {
                command = connection.CreateCommand();
                //command.CommandTimeout = 600;      // SOM-880
                command.CommandTimeout = 1000;      // RKL - 2018-01-04 - In Response to issue from BBU - Frederick
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                Database.StoredProcedure.usp_StoreroomTransfer_AutoGenCreate_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this, lulist);
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
        #endregion
    }
}
