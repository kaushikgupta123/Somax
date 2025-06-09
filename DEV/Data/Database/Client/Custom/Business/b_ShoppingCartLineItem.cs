using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.Business
{
    public partial class b_ShoppingCartLineItem
    {
        #region Property
        public string Name { get; set; }
        public string Mode { get; set; }
        public string Status { get; set; }
        public long CreatedBy_PersonnelId { get; set; }
        public string CreatedBy_Name { get; set; }
        public string PurchaseOrderClientLookupId { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
        public string ChargeToClientLookupId { get; set; }
        public string VendorID_ClientLookupID { get; set; }
        public long SiteId { get; set; }
        public decimal QuantityReceived { get; set; }
        public decimal QuantityToDate { get; set; }
        public decimal CurrentAverageCost { get; set; }
        public decimal CurrentAppliedCost { get; set; }
        public decimal CurrentOnHandQuantity { get; set; }
        public string ChargeTo_Name { get; set; }
        public string StockType { get; set; }
        public decimal QuantityBackOrdered { get; set; }
        public decimal TotalCost { get; set; }
        public string PartClientLookupId { get; set; }
        public string PMClientLookupId { get; set; }    // Part Master ClientLookupId
        public string PurchaseRequestNo { get; set; }
        public string Vendor_Name { get; set; }
        public bool Vendor_InactiveFlag { get; set; }
        #endregion

        #region Retrival Methods
        public void UpdateLineItem(
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
                Database.StoredProcedure.usp_ShoppingCartLineItem_UpdateInItem.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public void InsertByForeignKeysIntoDatabase(
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
                Database.StoredProcedure.usp_ShoppingCartLineItem_CreateForNonCatalogPart.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public void ShoppingCartLineItem_RetrieveByShoppingCartLineItemId(
    SqlConnection connection,
    SqlTransaction transaction,
    long callerUserInfoId,
    string callerUserName


    )
        {
            Database.SqlClient.ProcessRow<b_ShoppingCartLineItem> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;
                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_ShoppingCartLineItem>(reader => { this.LoadFromDatabaseExtendedPO(reader); return this; });
                Database.StoredProcedure.usp_ShoppingCartLineItem_RetrieveByShoppingCartLineItemId.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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
                ClientId = 0;
            }
        }
        public void ReOrderLineNumber(
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
                Database.StoredProcedure.usp_ShoppingCartLineItem_ReOrderLineNumber.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public void RetrieveShoppingCartLineItem(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            ref b_ShoppingCartLineItem[] data
        )
        {
            Database.SqlClient.ProcessRow<b_ShoppingCartLineItem> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_ShoppingCartLineItem[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_ShoppingCartLineItem>(reader => { b_ShoppingCartLineItem obj = new b_ShoppingCartLineItem(); obj.LoadExtendFromDatabase(reader); return obj; });
                results = Database.StoredProcedure.usp_ShoppingCartLineItem_Retrieve.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

                // Extract the results
                if (null != results)
                {
                    data = (b_ShoppingCartLineItem[])results.ToArray(typeof(b_ShoppingCartLineItem));
                }
                else
                {
                    data = new b_ShoppingCartLineItem[0];
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
        //SOM-1513
        public void RetrieveShoppingCartListForPart(
                SqlConnection connection,
                SqlTransaction transaction,
                long callerUserInfoId,
                string callerUserName,
                ref b_ShoppingCartLineItem[] data
            )
        {
            Database.SqlClient.ProcessRow<b_ShoppingCartLineItem> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_ShoppingCartLineItem[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_ShoppingCartLineItem>(reader => { b_ShoppingCartLineItem obj = new b_ShoppingCartLineItem(); obj.LoadDataFromDatabaseForListOfPart(reader); return obj; });
                results = Database.StoredProcedure.usp_ShoppingCartLineItem_RetrieveByPartId.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

                // Extract the results
                if (null != results)
                {
                    data = (b_ShoppingCartLineItem[])results.ToArray(typeof(b_ShoppingCartLineItem));
                }
                else
                {
                    data = new b_ShoppingCartLineItem[0];
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
        public void RetrieveByShoppingCartLineItemFromDatabase(
    SqlConnection connection,
    SqlTransaction transaction,
    long callerUserInfoId,
    string callerUserName,
    ref List<b_ShoppingCartLineItem> data
    )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_ShoppingCartLineItem> results = null;
            data = new List<b_ShoppingCartLineItem>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_ShoppingCartLineItem_RetrieveReceiptItem.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_ShoppingCartLineItem>();
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
        public int LoadDataFromDatabaseForListOfPart(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // ShoppingCartLineItemId column, bigint, not null
                ShoppingCartLineItemId = reader.GetInt64(i++);

                // ShoppingCartId column, bigint, not null
                ShoppingCartId = reader.GetInt64(i++);

                // LineNumber column, int, not null
                LineNumber = reader.GetInt32(i++);

                // PartId column, bigint, not null
                PartId = reader.GetInt64(i++);

                // Description column, nvarchar(MAX), not null
                Description = reader.GetString(i++);

                // OrderQuantity column, decimal(15,6), not null
                OrderQuantity = reader.GetDecimal(i++);

                // UnitofMeasure column, nvarchar(15), not null
                UnitofMeasure = reader.GetString(i++);
                //PartClientLookupId , nvarchar(31), 

                if (false == reader.IsDBNull(i))
                {
                    PartClientLookupId = reader.GetString(i);
                }
                else
                {
                    PartClientLookupId = string.Empty; ;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Status = reader.GetString(i);
                }
                else
                {
                    Status = string.Empty;
                }
                i++;

                CreatedBy_PersonnelId = reader.GetInt64(i++);

                if (false == reader.IsDBNull(i))
                {
                    CreatedBy_Name = reader.GetString(i);
                }
                else
                {
                    CreatedBy_Name = string.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    PurchaseOrderClientLookupId = reader.GetString(i);
                }
                else
                {
                    PurchaseOrderClientLookupId = string.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    PurchaseRequestNo = reader.GetString(i);
                }
                else
                {
                    PurchaseRequestNo = string.Empty;
                }
                i++;


            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["PurchaseRequestNo"].ToString(); }
                catch { missing.Append("PurchaseRequestNo "); }

                try { reader["PMClientLookupId"].ToString(); }
                catch { missing.Append("PMClientLookupId "); }

                try { reader["PartClientLookupId"].ToString(); }
                catch { missing.Append("PartClientLookupId "); }

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


        public int LoadExtendFromDatabase(SqlDataReader reader)
        {
            int i = LoadFromDatabase(reader);
            try
            {
                Status = reader.GetString(i++);
                PurchaseRequestNo = reader.GetString(i++);
                PMClientLookupId = reader.GetString(i++);
                PartClientLookupId = reader.GetString(i++);
                VendorID_ClientLookupID = reader.GetString(i++);
                Vendor_Name = reader.GetString(i++);
                Vendor_InactiveFlag = reader.GetBoolean(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["PurchaseRequestNo"].ToString(); }
                catch { missing.Append("PurchaseRequestNo "); }

                try { reader["PMClientLookupId"].ToString(); }
                catch { missing.Append("PMClientLookupId "); }

                try { reader["PartClientLookupId"].ToString(); }
                catch { missing.Append("PartClientLookupId "); }

                try { reader["VendorID_ClientLookupID"].ToString(); }
                catch { missing.Append("VendorID_ClientLookupID "); }

                try { reader["Vendor_Name"].ToString(); }
                catch { missing.Append("Vendor_Name "); }

                try { reader["Vendor_InactiveFlag"].ToString(); }
                catch { missing.Append("Vendor_InactiveFlag "); }

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


        public static object ProcessRowByShoppingCartLineItemId(SqlDataReader reader)
        {
            // Create instance of object
            b_ShoppingCartLineItem obj = new b_ShoppingCartLineItem();

            // Load the object from the database
            obj.LoadFromDatabaseShoppingCartLineItemId(reader);

            // Return result
            return (object)obj;
        }

        public void LoadFromDatabaseShoppingCartLineItemId(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                LineNumber = reader.GetInt32(i++);

                PartId = reader.GetInt64(i++);

                Description = reader.GetString(i++);

                OrderQuantity = reader.GetDecimal(i++);

                UnitofMeasure = reader.GetString(i++);

                UnitCost = reader.GetDecimal(i++);

                Name = reader.GetString(i++);
                // SOM-1617
                PartClientLookupId = reader.GetString(i++);
                if (false == reader.IsDBNull(i))      //SOM-1526
                {
                    RequiredDate = reader.GetDateTime(i);
                }
                else
                {
                    RequiredDate = DateTime.MinValue;
                }
                i++;
                if (false == reader.IsDBNull(i))      //SOM-1595
                {
                    Vendor_InactiveFlag = reader.GetBoolean(i);
                }
                else
                {
                    Vendor_InactiveFlag = true;
                }
                i++;

      }
      catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["LineNumber"].ToString(); }
                catch { missing.Append("LineNumber "); }
                try { reader["PartId"].ToString(); }
                catch { missing.Append("PartId "); }
                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }
                try { reader["OrderQuantity"].ToString(); }
                catch { missing.Append("OrderQuantity "); }
                try { reader["UnitofMeasure"].ToString(); }
                catch { missing.Append("UnitofMeasure "); }
                try { reader["UnitCost"].ToString(); }
                catch { missing.Append("UnitCost "); }
                try { reader["Name"].ToString(); }
                catch { missing.Append("Name "); }
                try { reader["PartClientLookupId"].ToString(); }
                catch { missing.Append("PartClientLookupId "); }
                try { reader["RequiredDate"].ToString(); }
                catch { missing.Append("RequiredDate "); }
                try { reader["Vendor_InactiveFlag"].ToString(); }
                catch { missing.Append("Vendor_InactiveFlag "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }
        public void LoadFromDatabaseExtendedPO(SqlDataReader reader)
        {
            int i = this.LoadFromDatabase(reader);
            try
            {
                // TotalCost
                if (false == reader.IsDBNull(i))
                {
                    TotalCost = reader.GetDecimal(i);
                }
                else
                {
                    TotalCost = 0;
                }
                i++;
                // VendorID_ClientLookupID
                if (false == reader.IsDBNull(i))
                {
                    VendorID_ClientLookupID = reader.GetString(i);
                }
                else
                {
                    VendorID_ClientLookupID = "";
                }
                i++;
                // PartClientLookupId
                if (false == reader.IsDBNull(i))
                {
                    PartClientLookupId = reader.GetString(i);
                }
                else
                {
                    PartClientLookupId = "";
                }
                i++;
                // ChargeToClientLookupId 
                if (false == reader.IsDBNull(i))
                {
                    ChargeToClientLookupId = reader.GetString(i);
                }
                else
                {
                    ChargeToClientLookupId = "";
                }
                i++;
                // From here down - these are not used currently - think about removing for speed.
                if (false == reader.IsDBNull(i))
                {
                    QuantityReceived = reader.GetDecimal(i);
                }
                else
                {
                    QuantityReceived = 0;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    QuantityToDate = reader.GetDecimal(i);
                }
                else
                {
                    QuantityToDate = 0;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    CurrentAverageCost = reader.GetDecimal(i);
                }
                else
                {
                    CurrentAverageCost = 0;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    CurrentAppliedCost = reader.GetDecimal(i);
                }
                else
                {
                    CurrentAppliedCost = 0;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    StockType = reader.GetString(i);
                }
                else
                {
                    StockType = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    QuantityBackOrdered = reader.GetDecimal(i);
                }
                else
                {
                    QuantityBackOrdered = 0;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    ChargeTo_Name = reader.GetString(i);
                }
                else
                {
                    ChargeTo_Name = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Vendor_Name = reader.GetString(i);
                }
                else
                {
                    Vendor_Name = "";
                }
            }

            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["TotalCost"].ToString(); }
                catch { missing.Append("TotalCost "); }

                try { reader["VendorID_ClientLookupID"].ToString(); }
                catch { missing.Append("VendorID_ClientLookupID "); }

                try { reader["PartClientLookupId"].ToString(); }
                catch { missing.Append("PartClientLookupId "); }

                try { reader["ChargeToClientLookupId"].ToString(); }
                catch { missing.Append("ChargeToClientLookupId "); }

                try { reader["ChargeToClientLookupId"].ToString(); }
                catch { missing.Append("ChargeToClientLookupId "); }

                try { reader["QuantityReceived"].ToString(); }
                catch { missing.Append("QuantityReceived "); }

                try { reader["QuantityToDate"].ToString(); }
                catch { missing.Append("QuantityToDate "); }
                try { reader["CurrentAverageCost"].ToString(); }
                catch { missing.Append("CurrentAverageCost "); }

                try { reader["CurrentOnHandQuantity"].ToString(); }
                catch { missing.Append("CurrentOnHandQuantity "); }

                try { reader["StockType"].ToString(); }
                catch { missing.Append("StockType "); }

                try { reader["QuantityBackOrdered"].ToString(); }
                catch { missing.Append("QuantityBackOrdered "); }

                try { reader["ChargeTo_Name"].ToString(); }
                catch { missing.Append("ChargeTo_Name "); }

                try { reader["Vendor_Name"].ToString(); }
                catch { missing.Append("Vendor_Name "); }
                // SOM-398
                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }
        public void ValidateNonStock(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, ref List<b_StoredProcValidationError> data)
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
                results = Database.StoredProcedure.usp_ShoppingCartLineItem_ValidateNonStock.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
    public void ValidateNonCatalog(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, ref List<b_StoredProcValidationError> data)
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
        results = Database.StoredProcedure.usp_ShoppingCartLineItem_ValidateNonCatalog.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
    public void ValidateCatalog(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, ref List<b_StoredProcValidationError> data)
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
        results = Database.StoredProcedure.usp_ShoppingCartLineItem_ValidateCatalog.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
    public void LoadCartItemsForAutoGenNotification(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                ShoppingCartId = reader.GetInt64(i++);
                Vendor_Name = reader.GetString(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                try { reader["ShoppingCartId"].ToString(); }
                catch { missing.Append("ShoppingCartId "); }

                try { reader["Vendor_Name"].ToString(); }
                catch { missing.Append("Vendor_Name "); }


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
