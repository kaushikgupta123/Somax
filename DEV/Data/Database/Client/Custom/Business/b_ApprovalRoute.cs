

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Database.Business
{
   public partial class b_ApprovalRoute
    {
        public long SiteId { get; set; }
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int Offset { get; set; }
        public int Nextrow { get; set; }
        public string FilterTypeCase { get; set; }
        public long PurchaseRequestId { get; set; }
        public long WorkOrderId { get; set; }
        public string ClientLookupId { get; set; }
        public string VendorName { get; set; }
        public DateTime? Date { get; set; } 
        public string ChargeTo { get; set; }
        public string ChargeToName { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int TotalCount { get; set; }
        #region V2-730
        public int UpdateIndexOut { get; set; }
        #endregion
        #region//**MaterialRequest
        public long EstimatedCostsId { get; set; }
        public long MaterialRequestId { get; set; }
        
        public decimal UnitCost { get; set; }
        public decimal Quantity { get; set; }
        public decimal TotalCost { get; set; }
        #endregion

        public void ApprovalRoute_RetrieveForPurchaseRequest_V2(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            ref List<b_ApprovalRoute> data
        )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_ApprovalRoute> results = null;
            data = new List<b_ApprovalRoute>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_ApprovalRoute_RetrieveChunkSearchForPurchaseRequest_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_ApprovalRoute>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                }

            }
        }
        public static object ProcessRowForApprovalRoute_RetrievePurchaseRequest_V2(SqlDataReader reader)
        {
            // Create instance of object
            b_ApprovalRoute obj = new b_ApprovalRoute();
            // Load the object from the database
            obj.LoadFromDatabaseForPurchaseRequest_V2(reader);
            // Return result
            return (object)obj;
        }

    
        public void LoadFromDatabaseForPurchaseRequest_V2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // ApprovalRouteId column, bigint, not null
                ApprovalRouteId = reader.GetInt64(i++);
                // PurchaseRequestId column, bigint, not null
                PurchaseRequestId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(50), not null
                ClientLookupId = reader.GetString(i++);

                // VendorName column, nvarchar(15)
                if (false == reader.IsDBNull(i))
                {
                    VendorName = reader.GetString(i);
                }
                else
                {
                    VendorName = " ";
                }
                i++;

                // Date column, datetime2
                if (false == reader.IsDBNull(i))
                {
                    Date = reader.GetDateTime(i);
                }
                else
                {
                    Date = DateTime.MinValue;
                }
                i++;

                Comments = reader.GetString(i++);
                // ApprovalGroupId column, bigint, not null
                ApprovalGroupId = reader.GetInt64(i++);
                TotalCount = reader.GetInt32(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ApprovalRouteId"].ToString(); }
                catch { missing.Append("ApprovalRouteId "); }

                try { reader["PurchaseRequestId"].ToString(); }
                catch { missing.Append("PurchaseRequestId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["VendorName"].ToString(); }
                catch { missing.Append("VendorName "); }

                try { reader["Date"].ToString(); }
                catch { missing.Append("Date "); }

                try { reader["Comments"].ToString(); }
                catch { missing.Append("Comments "); }

                try { reader["ApprovalGroupId"].ToString(); }
                catch { missing.Append("ApprovalGroupId "); }

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
        }
       

        #region WorkRequest

        public void ApprovalRoute_RetrieveForWorkRequest_V2(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            ref List<b_ApprovalRoute> data
        )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_ApprovalRoute> results = null;
            data = new List<b_ApprovalRoute>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_ApprovalRoute_RetrieveChunkSearchForWorkRequest_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_ApprovalRoute>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                }

            }
        }
        public static object ProcessRowForApprovalRoute_RetrieveWorkRequest_V2(SqlDataReader reader)
        {
            // Create instance of object
            b_ApprovalRoute obj = new b_ApprovalRoute();
            // Load the object from the database
            obj.LoadFromDatabaseForWorkRequest_V2(reader);
            // Return result
            return (object)obj;
        }

        public void LoadFromDatabaseForWorkRequest_V2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // ApprovalRouteId column, bigint, not null
                ApprovalRouteId = reader.GetInt64(i++);

                WorkOrderId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(50), not null
                ClientLookupId = reader.GetString(i++);

                // ChargeTo column, nvarchar(50)
                if (false == reader.IsDBNull(i))
                {
                    ChargeTo = reader.GetString(i);
                }
                else
                {
                    ChargeTo = " ";
                }
                i++;
                // ChargeToName column, nvarchar(50)
                if (false == reader.IsDBNull(i))
                {
                    ChargeToName = reader.GetString(i);
                }
                else
                {
                    ChargeToName = " ";
                }
                i++;
                // Description column, nvarchar(50)
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i);
                }
                else
                {
                    Description = " ";
                }
                i++;
                // Type column, nvarchar(50)
                if (false == reader.IsDBNull(i))
                {
                    Type = reader.GetString(i);
                }
                else
                {
                    Type = " ";
                }
                i++;
                // Date column, datetime2
                if (false == reader.IsDBNull(i))
                {
                    Date = reader.GetDateTime(i);
                }
                else
                {
                    Date = DateTime.MinValue;
                }
                i++;

                Comments = reader.GetString(i++);
                // ApprovalGroupId column, bigint, not null
                ApprovalGroupId = reader.GetInt64(i++);
                TotalCount = reader.GetInt32(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                try { reader["ApprovalRouteId"].ToString(); }
                catch { missing.Append("ApprovalRouteId "); }

                try { reader["WorkOrderId"].ToString(); }
                catch { missing.Append("WorkOrderId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["ChargeTo"].ToString(); }
                catch { missing.Append("ChargeTo "); }

                try { reader["ChargeToName"].ToString(); }
                catch { missing.Append("ChargeToName "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["Type"].ToString(); }
                catch { missing.Append("Type "); }

                try { reader["Date"].ToString(); }
                catch { missing.Append("Date "); }

                try { reader["Comments"].ToString(); }
                catch { missing.Append("Comments "); }

                try { reader["ApprovalGroupId"].ToString(); }
                catch { missing.Append("ApprovalGroupId "); }

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
        }
        #endregion

       
        #region V2-730
        public void ApprovalRoute_RetrievebyObjectId_V2(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            ref List<b_ApprovalRoute> data
        )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_ApprovalRoute> results = null;
            data = new List<b_ApprovalRoute>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_ApprovalRoute_RetrievebyObjectId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_ApprovalRoute>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                }

            }
        }
        public static object ProcessRowForApprovalRoute_RetrievebyObjectId_V2(SqlDataReader reader)
        {
            // Create instance of object
            b_ApprovalRoute obj = new b_ApprovalRoute();
            // Load the object from the database
            obj.LoadFromDatabasebyObjectId_V2(reader);
            // Return result
            return (object)obj;
        }


        public void LoadFromDatabasebyObjectId_V2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // ApprovalRouteId column, bigint, not null
                if (false == reader.IsDBNull(i))
                    ApprovalRouteId = reader.GetInt64(i);
                else
                    ApprovalRouteId = 0;
                i++;

                // ApprovalGroupId column, bigint, not null
                if (false == reader.IsDBNull(i))
                    ApprovalGroupId = reader.GetInt64(i);
                else
                    ApprovalGroupId = 0;
                i++;

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ApprovalRouteId"].ToString(); }
                catch { missing.Append("ApprovalRouteId "); }

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
        }

        public void UpdateByObjectIdInDatabase_V2(
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
                StoredProcedure.usp_ApprovalRoute_UpdateByObjectId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
       
        #region MaterialRequest
        public void ApprovalRoute_RetrieveForMaterialRequest_V2(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
           string callerUserName,
           ref List<b_ApprovalRoute> data
       )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_ApprovalRoute> results = null;
            data = new List<b_ApprovalRoute>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_ApprovalRoute_RetrieveChunkSearchForMaterialRequest_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_ApprovalRoute>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                }

            }
        }
        public static object ProcessRowForApprovalRoute_RetrieveMaterialRequest_V2(SqlDataReader reader)
        {
            // Create instance of object
            b_ApprovalRoute obj = new b_ApprovalRoute();
            // Load the object from the database
            obj.LoadFromDatabaseForMaterialRequest_V2(reader);
            // Return result
            return (object)obj;
        }


        public void LoadFromDatabaseForMaterialRequest_V2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // ApprovalRouteId column, bigint, not null
                ApprovalRouteId = reader.GetInt64(i++);

                if (false == reader.IsDBNull(i))
                {
                    EstimatedCostsId = reader.GetInt64(i);
                }
                else
                {
                    EstimatedCostsId = 0;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    MaterialRequestId = reader.GetInt64(i);
                }
                else
                {
                    MaterialRequestId = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = "";
                }
                i++;

                
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i);
                }
                else
                {
                    Description = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    UnitCost = reader.GetDecimal(i);
                }
                else
                {
                    UnitCost = 0;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Quantity = reader.GetDecimal(i);
                }
                else
                {
                    Quantity = 0;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    TotalCost = reader.GetDecimal(i);
                }
                else
                {
                    TotalCost = 0;
                }
                i++;
                // Date column, datetime2
                if (false == reader.IsDBNull(i))
                {
                    Date = reader.GetDateTime(i);
                }
                else
                {
                    Date = DateTime.MinValue;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Comments = reader.GetString(i);
                }
                else
                {
                    Comments = "";
                }
                i++;
                // ApprovalGroupId column, bigint, not null
                ApprovalGroupId = reader.GetInt64(i++);
                if (false == reader.IsDBNull(i))
                {
                    TotalCount = reader.GetInt32(i);
                }
                else
                {
                    TotalCount = 0;
                }
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ApprovalRouteId"].ToString(); }
                catch { missing.Append("ApprovalRouteId "); }

                try { reader["EstimatedCostsId"].ToString(); }
                catch { missing.Append("EstimatedCostsId "); }

                try { reader["MaterialRequestId"].ToString(); }
                catch { missing.Append("MaterialRequestId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId"); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description"); }

                try { reader["UnitCost"].ToString(); }
                catch { missing.Append("UnitCost"); }

                try { reader["Quantity"].ToString(); }
                catch { missing.Append("Quantity"); }

                try { reader["TotalCost"].ToString(); }
                catch { missing.Append("TotalCost"); }

                try { reader["Date"].ToString(); }
                catch { missing.Append("Date "); }

                try { reader["Comments"].ToString(); }
                catch { missing.Append("Comments "); }

                try { reader["ApprovalGroupId"].ToString(); }
                catch { missing.Append("ApprovalGroupId "); }

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
        }
        #endregion MaterialRequest
        
    }

}
