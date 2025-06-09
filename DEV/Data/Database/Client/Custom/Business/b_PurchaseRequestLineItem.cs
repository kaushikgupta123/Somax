/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2015-2017 by SOMAX Inc.
* b_PurchaseRequestLineItem.cs (Data Object)
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID  Person             Description
* =========== ======== ================== =========================================================
* 2014-Oct-17 SOM-369  Roger Lawton       
* 2014-Nov-03 SOM-398  Roger Lawton       Clean up method LoadFromDatabaseExtended
* 2014-Nov-12 SOM-419  Roger Lawton       Modified              
* 2015-Mar-08 SOM-594  Roger Lawton       Removed PartStoreroomId (handled in LoadFromDatabase)
* 2015-Mar-18 SOM-608  Roger Lawton       Added Account_ClientLookupId
*                                         Added CreateWithReplication method
* 2017-Mar-21 SOM-1286 Nick Fuchs         Add Mfg and Mfg ID                                         
****************************************************************************************************
*/

using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.Business
{

    public partial class b_PurchaseRequestLineItem
    {
        #region properties
        public decimal TotalCost { get; set; }
        public string PartClientLookupId { get; set; }
        public string ChargeToClientLookupId { get; set; }
        public string ChargeTo_Name { get; set; }
        public string Account_ClientLookupId { get; set; }
        public decimal QuantityReceived { get; set; }
        public decimal QuantityToDate { get; set; }
        public decimal CurrentAverageCost { get; set; }
        public decimal CurrentAppliedCost { get; set; }
        public decimal CurrentOnHandQuantity { get; set; }
        public string StockType { get; set; }
        public decimal QuantityBackOrdered { get; set; }
        public string Part_ManufacturerId { get; set; }    // Add to PR Print
        public string Part_Manufacturer { get; set; }      // Add to PR Print
        public bool UOMConvRequired { get; set; }
        public bool Ispunchout { get; set; } //V2-548
        public string PartCategoryMasterClientLookupId { get; set; } //V2-717
        public long StoreroomId { get; set; } //V2-738
        public string ClientLookupId { get; set; }//V2-894
        public string Name { get; set; }//V2-894
        public DateTime CreateDate { get; set; }//V2-894
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int OffSetVal { get; set; }
        public int NextRow { get; set; }
        public long TotalCount { get; set; }
        #region V2-1046
        public string VendorClientLookupId { get; set; }
        public string VendorName { get; set; }
        public long SiteId { get; set; }
        public long PersonnelId { get; set; }
        public string PRLineItemIds { get; set; }
        #endregion

        #region V2-1063
        public string WorkOrderClientLookupId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitCostQuantity { get; set; }
        #endregion

        #endregion properties

        public void RetrieveByPurchaseOrderLineItemFromDatabase(
       SqlConnection connection,
       SqlTransaction transaction,
       long callerUserInfoId,
       string callerUserName,
       ref List<b_PurchaseRequestLineItem> data
   )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_PurchaseRequestLineItem> results = null;
            data = new List<b_PurchaseRequestLineItem>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_PurchaseRequestLineItem_RetrieveReceiptItem.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_PurchaseRequestLineItem>();
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
        public void PurchaseRequestLineItem_RetrieveByPurchaseRequestId(
             SqlConnection connection,
             SqlTransaction transaction,
             long callerUserInfoId,
             string callerUserName,

             ref List<b_PurchaseRequestLineItem> purchaseRequestLineItemList
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
                purchaseRequestLineItemList = Database.StoredProcedure.usp_PurchaseRequestLineItem_RetrieveByPurchaseRequestId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        public void LoadFromDatabaseExtended(SqlDataReader reader)
        {
            int i = this.LoadFromDatabase(reader);
            try
            {

                //this.TotalCost = reader.GetString(i++);


                if (false == reader.IsDBNull(i))
                {
                    TotalCost = reader.GetDecimal(i);
                }
                else
                {
                    TotalCost = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    PartClientLookupId = reader.GetString(i);
                }
                else
                {
                    PartClientLookupId = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    ChargeToClientLookupId = reader.GetString(i);
                }
                else
                {
                    ChargeToClientLookupId = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                  Account_ClientLookupId = reader.GetString(i);
                }
                else
                {
                  Account_ClientLookupId = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Part_Manufacturer = reader.GetString(i);
                }
                else
                {
                    Part_Manufacturer = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Part_ManufacturerId = reader.GetString(i);
                }
                else
                {
                    Part_ManufacturerId = "";
                }
                
            }

            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["PurchaseRequestLineItemId"].ToString(); }
                catch { missing.Append("PurchaseRequestLineItemId "); }

                try { reader["PartClientLookupId"].ToString(); }
                catch { missing.Append("PartClientLookupId "); }

                try { reader["ChargeToClientLookupId"].ToString(); }
                catch { missing.Append("ChargeToClientLookupId "); }

                try { reader["Account_ClientLookupId"].ToString(); }
                catch { missing.Append("Account_ClientLookupId "); }

                try { reader["Part_Manufacturer"].ToString(); }
                catch { missing.Append("Manufacturer "); }

                try { reader["Part_ManufacturerId"].ToString(); }
                catch { missing.Append("ManufacturerId "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                  msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }
        public void LoadFromDatabaseExtendedPR(SqlDataReader reader)
        {
            int i = this.LoadFromDatabase(reader);
            try
            {

                //this.TotalCost = reader.GetString(i++);


                if (false == reader.IsDBNull(i))
                {
                    TotalCost = reader.GetDecimal(i);
                }
                else
                {
                    TotalCost = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    PartClientLookupId = reader.GetString(i);
                }
                else
                {
                    PartClientLookupId = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    ChargeToClientLookupId = reader.GetString(i);
                }
                else
                {
                    ChargeToClientLookupId = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Account_ClientLookupId = reader.GetString(i);
                }
                else
                {
                    Account_ClientLookupId = "";
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

                //if (false == reader.IsDBNull(i))
                //{
                //    Ispunchout = reader.GetBoolean(i);
                //}
                //else
                //{
                //    Ispunchout =false;
                //}
                //i++;
                

            }

            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["TotalCost"].ToString(); }
                catch { missing.Append("TotalCost "); }

                try { reader["PartClientLookupId"].ToString(); }
                catch { missing.Append("PartClientLookupId "); }

                try { reader["ChargeToClientLookupId"].ToString(); }
                catch { missing.Append("ChargeToClientLookupId "); }

                try { reader["Account_ClientLookupId"].ToString(); }
                catch { missing.Append("Account_ClientLookupId "); }


                try { reader["ChargeTo_Name"].ToString(); }
                catch { missing.Append("ChargeTo_Name "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }


        public void LoadFromDatabaseExtendedPR_V2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // PurchaseRequestLineItemId column, bigint, not null
                PurchaseRequestLineItemId = reader.GetInt64(i++);

                // PurchaseRequestId column, bigint, not null
                PurchaseRequestId = reader.GetInt64(i++);

                // PartId column, bigint, not null
                PartId = reader.GetInt64(i++);

                // LineNumber column, int, not null
                LineNumber = reader.GetInt32(i++);

                // Description column, nvarchar(MAX), not null
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i);
                }
                else
                {
                    Description = "";
                }
                i++;

                // OrderQuantity column, decimal(15,6), not null
                if (false == reader.IsDBNull(i))
                {
                    OrderQuantity = reader.GetDecimal(i);
                }
                else
                {
                    OrderQuantity = 0;
                }
                i++;
                
                // UnitofMeasure column, nvarchar(15), not null
                if (false == reader.IsDBNull(i))
                {
                    UnitofMeasure = reader.GetString(i);
                }
                else
                {
                    UnitofMeasure = "";
                }
                i++;
                // UnitCost column, decimal(15,5), not null
                if (false == reader.IsDBNull(i))
                {
                    UnitCost = reader.GetDecimal(i);
                }
                else
                {
                    UnitCost = 0;
                }
                i++;    
                // AccountId column, bigint, not null
                AccountId = reader.GetInt64(i++);             


                // ChargeType column, nvarchar(15), not null
                if (false == reader.IsDBNull(i))
                {
                    ChargeType = reader.GetString(i);
                }
                else
                {
                    ChargeType = "";
                }
                i++;               

                // ChargeToID column, bigint, not null
                ChargeToID = reader.GetInt64(i++);

                if (false == reader.IsDBNull(i))
                {
                    Ispunchout = reader.GetBoolean(i);
                }
                else
                {
                    Ispunchout = false;
                }
                i++;

                //this.TotalCost = reader.GetString(i++);
                if (false == reader.IsDBNull(i))
                {
                    TotalCost = reader.GetDecimal(i);
                }
                else
                {
                    TotalCost = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    PartClientLookupId = reader.GetString(i);
                }
                else
                {
                    PartClientLookupId = "";
                }
                i++;


                if (false == reader.IsDBNull(i))
                {
                    ChargeToClientLookupId = reader.GetString(i);
                }
                else
                {
                    ChargeToClientLookupId = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Account_ClientLookupId = reader.GetString(i);
                }
                else
                {
                    Account_ClientLookupId = "";
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
                // PurchaseUOM column, nvarchar(15), not null
                if (false == reader.IsDBNull(i))
                {
                    PurchaseUOM = reader.GetString(i);
                }
                else
                {
                    PurchaseUOM = "";
                }
                i++;
                // UOMConvRequired column, bool, not null
                if (false == reader.IsDBNull(i))
                {
                    UOMConvRequired = reader.GetBoolean(i);
                }
                else
                {
                    UOMConvRequired = false;
                }
                i++;

                // UOMConversion column, decimal(15,5), not null
                if (false == reader.IsDBNull(i))
                {
                    UOMConversion = reader.GetDecimal(i);
                }
                else
                {
                    UOMConversion = 0;
                }
                i++;
                // RequiredDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    RequiredDate = reader.GetDateTime(i);
                }
                else
                {
                    RequiredDate = DateTime.MinValue;
                }
                i++;
                // SupplierPartId column, nvarchar(63), not null
                SupplierPartId = reader.GetString(i++);

                // SupplierPartAuxiliaryId column, nvarchar(63), not null
                SupplierPartAuxiliaryId = reader.GetString(i++);

                // ManufacturerPartId column, nvarchar(63), not null
                ManufacturerPartId = reader.GetString(i++);

                // Manufacturer column, nvarchar(31), not null
                Manufacturer = reader.GetString(i++);

                // Classification column, nvarchar(31), not null
                //Classification = reader.GetString(i++); V2-717

                // VendorCatalogItemId column, bigint, not null
                VendorCatalogItemId = reader.GetInt64(i++);

                // UNSPSC column, bigint, not null
                UNSPSC = reader.GetInt64(i++);

                //PartCategoryMasterClientLookupId
                if (false == reader.IsDBNull(i))
                {
                    PartCategoryMasterClientLookupId = reader.GetString(i);
                }
                else
                {
                    PartCategoryMasterClientLookupId = "";
                }
                i++;
              

            }

            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["PurchaseRequestLineItemId"].ToString(); }
                catch { missing.Append("PurchaseRequestLineItemId "); }

                try { reader["PurchaseRequestId"].ToString(); }
                catch { missing.Append("PurchaseRequestId "); }

                try { reader["PartId"].ToString(); }
                catch { missing.Append("PartId "); }

                try { reader["LineNumber"].ToString(); }
                catch { missing.Append("LineNumber "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["OrderQuantity"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["UnitofMeasure"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["UnitCost"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["AccountId"].ToString(); }
                catch { missing.Append("AccountId "); }

                try { reader["ChargeType"].ToString(); }
                catch { missing.Append("ChargeType "); } 

                try { reader["ChargeToID"].ToString(); }
                catch { missing.Append("ChargeToID "); }

                try { reader["Ispunchout"].ToString(); }
                catch { missing.Append("Ispunchout "); }

                try { reader["TotalCost"].ToString(); }
                catch { missing.Append("TotalCost "); }

                try { reader["PartClientLookupId"].ToString(); }
                catch { missing.Append("PartClientLookupId "); }

                try { reader["ChargeToClientLookupId"].ToString(); }
                catch { missing.Append("ChargeToClientLookupId "); }

                try { reader["Account_ClientLookupId"].ToString(); }
                catch { missing.Append("Account_ClientLookupId "); }

                try { reader["ChargeTo_Name"].ToString(); }
                catch { missing.Append("ChargeTo_Name "); }

                try { reader["PurchaseUOM"].ToString(); }
                catch { missing.Append("PurchaseUOM "); }

                try { reader["UOMConvRequired"].ToString(); }
                catch { missing.Append("UOMConvRequired "); }

                try { reader["UOMConversion"].ToString(); }
                catch { missing.Append("UOMConversion "); }

                try { reader["RequiredDate"].ToString(); }
                catch { missing.Append("RequiredDate "); }

                try { reader["SupplierPartId"].ToString(); }
                catch { missing.Append("SupplierPartId "); }

                try { reader["SupplierPartAuxiliaryId"].ToString(); }
                catch { missing.Append("SupplierPartAuxiliaryId "); }

                try { reader["ManufacturerPartId"].ToString(); }
                catch { missing.Append("ManufacturerPartId "); }

                try { reader["Manufacturer"].ToString(); }
                catch { missing.Append("Manufacturer "); }
                //V2-717
                //try { reader["Classification"].ToString(); }
                //catch { missing.Append("Classification "); }

                try { reader["VendorCatalogItemId"].ToString(); }
                catch { missing.Append("VendorCatalogItemId "); }

                try { reader["UNSPSC"].ToString(); }
                catch { missing.Append("UNSPSC "); }

                try { reader["PartCategoryMasterClientLookupId"].ToString(); }
                catch { missing.Append("PartCategoryMasterClientLookupId "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }
        public void PurchaseRequestLineItem_RetrieveByPurchaseRequestLineItemId(
             SqlConnection connection,
             SqlTransaction transaction,
             long callerUserInfoId,
             string callerUserName


         )
        {
            Database.SqlClient.ProcessRow<b_PurchaseRequestLineItem> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;
                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_PurchaseRequestLineItem>(reader => { this.LoadFromDatabaseExtendedPR(reader); return this; });
                Database.StoredProcedure.usp_PurchaserequestLineItem_RetrieveByPurchaseRequestLineItemId.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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



        public void PurchaseRequestLineItem_RetrieveByPurchaseRequestLineItemIdV2(
         SqlConnection connection,
         SqlTransaction transaction,
         long callerUserInfoId,
         string callerUserName


     )
        {
            Database.SqlClient.ProcessRow<b_PurchaseRequestLineItem> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;
                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_PurchaseRequestLineItem>(reader => { this.LoadFromDatabaseExtendedPR_V2(reader); return this; });
                Database.StoredProcedure.usp_PurchaseRequestLineItem_RetrieveByPurchaseRequestLineItemId_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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


        /// <summary>
        /// Insert this object into the database as a PurchaseRequestLineItem table record.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public void CreateWithReplication(SqlConnection connection,SqlTransaction transaction, long callerUserInfoId, string callerUserName )
        {
          SqlCommand command = null;

          try
          {
            command = connection.CreateCommand();
            if (null != transaction)
            {
              command.Transaction = transaction;
            }
            Database.StoredProcedure.usp_PurchaseRequestLineItem_CreateWithReplication_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public void CreateFromShoppingCart(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName)
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                Database.StoredProcedure.usp_PurchaseRequestLineItem_CreateFromShoppingCart_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public void CreateFromPunchOutShoppingCart(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName)
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                Database.StoredProcedure.usp_PurchaseRequestLineItem_CreateFromPunchOutShoppingCart_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public void PurchaseRequestLineItem_Validation(
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
            //data = new List<b_StoredProcValidationError>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_PurchaseRequestLineItem_Validate.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        public void ReOrderPRLineNumber(
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
                Database.StoredProcedure.usp_PurchaseRequestLineItem_ReOrderLineNumber.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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


        public static object ProcessRowByPurchaseOrderLineItemId(SqlDataReader reader)
        {
            // Create instance of object
            b_PurchaseRequestLineItem obj = new b_PurchaseRequestLineItem();

            // Load the object from the database
            obj.LoadFromDatabasePurchaseOrderLineItemId(reader);

            // Return result
            return (object)obj;
        }
        public void LoadFromDatabasePurchaseOrderLineItemId(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                LineNumber = reader.GetInt32(i++);

                PartId = reader.GetInt64(i++);
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i);
                }
                else
                {
                    Description = "";
                }
                i++;


                OrderQuantity = reader.GetDecimal(i++);

                if (false == reader.IsDBNull(i))
                {
                    UnitofMeasure = reader.GetString(i);
                }
                else
                {
                    UnitofMeasure = "";
                }
                i++;

                UnitCost = reader.GetDecimal(i++);

                if (false == reader.IsDBNull(i))
                {
                    PartClientLookupId = reader.GetString(i);
                }
                else
                {
                    PartClientLookupId = "<<Non-Stock>>";
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
        }

        #region V2-563
        public void CreateFromAdditionalCatalogItem(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName)
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                Database.StoredProcedure.usp_PurchaseRequestLineItem_CreateFromAdditionalCatalog_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        #region V2-693 SOMAX to SAP Purchase request export
        public void RetrievePRLineItemByIdForExportSAP(
             SqlConnection connection,
             SqlTransaction transaction,
             long callerUserInfoId,
             string callerUserName,

             ref List<b_PurchaseRequestLineItem> purchaseRequestLineItemList
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
                purchaseRequestLineItemList = StoredProcedure.usp_PurchaseRequestLineItem_RetrieveForExportSAP_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public void LoadFromDatabaseForExportSAP(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // LineNumber column, int, not null
                LineNumber = reader.GetInt32(i++);

                // Description column, nvarchar(MAX), not null
                Description = reader.GetString(i++);

                if (false == reader.IsDBNull(i))
                {
                    Account_ClientLookupId = reader.GetString(i);
                }
                else
                {
                    Account_ClientLookupId = "";
                }
                i++;

                // UnitofMeasure column, nvarchar(15), not null
                UnitofMeasure = reader.GetString(i++);

                // UnitCost column, decimal(15,5), not null
                UnitCost = reader.GetDecimal(i++);

                // OrderQuantity column, decimal(15,6), not null
                OrderQuantity = reader.GetDecimal(i++);
            }

            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["LineNumber"].ToString(); }
                catch { missing.Append("LineNumber "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["Account_ClientLookupId"].ToString(); }
                catch { missing.Append("Account_ClientLookupId "); }

                try { reader["UnitofMeasure"].ToString(); }
                catch { missing.Append("UnitofMeasure "); }

                try { reader["UnitCost"].ToString(); }
                catch { missing.Append("UnitCost "); }

                try { reader["OrderQuantity"].ToString(); }
                catch { missing.Append("OrderQuantity "); }

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

        #region V2-738
        public void CreateFromShoppingCartForMultiStoreroom(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName)
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                Database.StoredProcedure.usp_PurchaseRequestLineItem_CreateFromShoppingCartForMultiStoreroom_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        #region V2-894
        public void PurchaseRequestLineItem_RetrieveLookupListByPartId_V2(
     SqlConnection connection,
     SqlTransaction transaction,
     long callerUserInfoId,
     string callerUserName,
     ref List<b_PurchaseRequestLineItem> purchaseRequestLineItemList

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
                purchaseRequestLineItemList = Database.StoredProcedure.usp_PurchaseRequestLineItem_RetrieveForLookupListByPartId_V2.CallStoredProcedure(command,callerUserInfoId, callerUserName, this);

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
        public void LoadFromDatabaseLookupListByPartid(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // clientlookup column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = "";
                }
                i++;
                // LineNumber column, int, not null
                LineNumber = reader.GetInt32(i++);

                // OrderQuantity column, decimal(15,6), not null
                if (false == reader.IsDBNull(i))
                {
                    OrderQuantity = reader.GetDecimal(i);
                }
                else
                {
                    OrderQuantity = 0;
                }
                i++;

                // UnitofMeasure column, nvarchar(15), not null
                if (false == reader.IsDBNull(i))
                {
                    UnitofMeasure = reader.GetString(i);
                }
                else
                {
                    UnitofMeasure = "";
                }
                i++;
                // Name column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    Name = reader.GetString(i);
                }
                else
                {
                    Name = "";
                }
                i++;
                // CreateDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    CreateDate = reader.GetDateTime(i);
                }
                else
                {
                    CreateDate = DateTime.MinValue;
                }
                i++;
                // TotalCount column, bigint, not null
                TotalCount = reader.GetInt32(i++);
            }

            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["LineNumber"].ToString(); }
                catch { missing.Append("LineNumber "); }

                try { reader["OrderQuantity"].ToString(); }
                catch { missing.Append("OrderQuantity "); }

                try { reader["LineNumber"].ToString(); }
                catch { missing.Append("LineNumber "); }

                try { reader["UnitofMeasure"].ToString(); }
                catch { missing.Append("UnitofMeasure "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append("Name "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

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

        #region V2-945
        public static b_PurchaseRequestLineItem ProcessRowFortmpPurchaseRequestLineItemPrint(SqlDataReader reader)
        {
            b_PurchaseRequestLineItem prLineItem = new b_PurchaseRequestLineItem();

            prLineItem.LoadFromDatabaseForurchaseRequestLineItemPrintt(reader);
            return prLineItem;
        }

        public void LoadFromDatabaseForurchaseRequestLineItemPrintt(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // PurchaseRequestLineItemId column, bigint, not null
                PurchaseRequestLineItemId = reader.GetInt64(i++);

                // PurchaseRequestId column, bigint, not null
                PurchaseRequestId = reader.GetInt64(i++);

                // Description column, nvarchar(MAX), not null
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i);
                }
                else
                {
                    Description = "";
                }
                i++;

                // RequiredDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    RequiredDate = reader.GetDateTime(i);
                }
                else
                {
                    RequiredDate = DateTime.MinValue;
                }
                i++;

                // LineNumber column, int, not null
                LineNumber = reader.GetInt32(i++);

                // OrderQuantity column, decimal(15,6), not null
                if (false == reader.IsDBNull(i))
                {
                    OrderQuantity = reader.GetDecimal(i);
                }
                else
                {
                    OrderQuantity = 0;
                }
                i++;

                // UnitofMeasure column, nvarchar(15), not null
                if (false == reader.IsDBNull(i))
                {
                    UnitofMeasure = reader.GetString(i);
                }
                else
                {
                    UnitofMeasure = "";
                }
                i++;
                // UnitCost column, decimal(15,5), not null
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
                    TotalCost = reader.GetDecimal(i);
                }
                else
                {
                    TotalCost = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    PartClientLookupId = reader.GetString(i);
                }
                else
                {
                    PartClientLookupId = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    ChargeToClientLookupId = reader.GetString(i);
                }
                else
                {
                    ChargeToClientLookupId = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Account_ClientLookupId = reader.GetString(i);
                }
                else
                {
                    Account_ClientLookupId = "";
                }
                i++;
            }

            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["PurchaseRequestLineItemId"].ToString(); }
                catch { missing.Append("PurchaseRequestLineItemId "); }

                try { reader["PurchaseRequestId"].ToString(); }
                catch { missing.Append("PurchaseRequestId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["RequiredDate"].ToString(); }
                catch { missing.Append("RequiredDate "); }

                try { reader["LineNumber"].ToString(); }
                catch { missing.Append("LineNumber "); }

                try { reader["OrderQuantity"].ToString(); }
                catch { missing.Append("OrderQuantity "); }

                try { reader["UnitofMeasure"].ToString(); }
                catch { missing.Append("UnitofMeasure "); }

                try { reader["UnitCost"].ToString(); }
                catch { missing.Append("UnitCost "); }

                try { reader["TotalCost"].ToString(); }
                catch { missing.Append("TotalCost "); }

                try { reader["PartClientLookupId"].ToString(); }
                catch { missing.Append("PartClientLookupId "); }

                try { reader["ChargeToClientLookupId"].ToString(); }
                catch { missing.Append("ChargeToClientLookupId "); }

                try { reader["Account_ClientLookupId"].ToString(); }
                catch { missing.Append("Account_ClientLookupId "); }

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

        #region V2-1046
        public void PurchaseRequestLineItem_RetrieveForConsolidate(
             SqlConnection connection,
             SqlTransaction transaction,
             long callerUserInfoId,
             string callerUserName,

             ref List<b_PurchaseRequestLineItem> purchaseRequestLineItemList
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
                purchaseRequestLineItemList = Database.StoredProcedure.usp_PurchaseRequestLineItem_RetrieveChunkSearchForConsolidate_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public void LoadFromDatabaseForConsolidate(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                if (false == reader.IsDBNull(i))
                {
                    ClientId = reader.GetInt64(i);
                }
                else
                {
                    ClientId = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    PurchaseRequestLineItemId = reader.GetInt64(i);
                }
                else
                {
                    PurchaseRequestLineItemId = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    PartClientLookupId = reader.GetString(i);
                }
                else
                {
                    PartClientLookupId = "";
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
                    OrderQuantity = reader.GetDecimal(i);
                }
                else
                {
                    OrderQuantity = 0;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    UnitofMeasure = reader.GetString(i);
                }
                else
                {
                    UnitofMeasure = "";
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
                    VendorClientLookupId = reader.GetString(i);
                }
                else
                {
                    VendorClientLookupId = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    VendorName = reader.GetString(i);
                }
                else
                {
                    VendorName = "";
                }
                i++;
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

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }
                
                try { reader["PurchaseRequestLineItemId"].ToString(); }
                catch { missing.Append("PurchaseRequestLineItemId "); }

                try { reader["PartClientLookupId"].ToString(); }
                catch { missing.Append("PartClientLookupId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["OrderQuantity"].ToString(); }
                catch { missing.Append("OrderQuantity "); }

                try { reader["UnitofMeasure"].ToString(); }
                catch { missing.Append("UnitofMeasure "); }

                try { reader["UnitCost"].ToString(); }
                catch { missing.Append("UnitCost "); }
                
                try { reader["VendorClientLookupId"].ToString(); }
                catch { missing.Append("VendorClientLookupId "); }

                try { reader["VendorName"].ToString(); }
                catch { missing.Append("VendorName "); }
                
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
        public void PRConsolidate(
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
                Database.StoredProcedure.usp_PurchaseRequestLineItem_ConsolidateProcess_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        #region V2-1063
        public void LineItem_RetrieveForMaterialRequest(
             SqlConnection connection,
             SqlTransaction transaction,
             long callerUserInfoId,
             string callerUserName,

             ref List<b_PurchaseRequestLineItem> purchaseRequestLineItemList
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
                purchaseRequestLineItemList = StoredProcedure.usp_EstimatedCosts_RetrieveChunkSearchForMaterialRequest_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public void LoadFromDatabaseForMaterialRequest(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                if (false == reader.IsDBNull(i))
                {
                    ClientId = reader.GetInt64(i);
                }
                else
                {
                    ClientId = 0;
                }
                i++;

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
                    PartClientLookupId = reader.GetString(i);
                }
                else
                {
                    PartClientLookupId = "";
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
                    UnitCostQuantity = reader.GetDecimal(i);
                }
                else
                {
                    UnitCostQuantity = 0;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    WorkOrderClientLookupId = reader.GetString(i);
                }
                else
                {
                    WorkOrderClientLookupId = "";
                }
                i++;
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

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["EstimatedCostsId"].ToString(); }
                catch { missing.Append("EstimatedCostsId "); }

                try { reader["PartClientLookupId"].ToString(); }
                catch { missing.Append("PartClientLookupId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["UnitCost"].ToString(); }
                catch { missing.Append("UnitCost "); }

                try { reader["Quantity"].ToString(); }
                catch { missing.Append("Quantity "); }

                try { reader["UnitCostQuantity"].ToString(); }
                catch { missing.Append("UnitCostQuantity "); }

                try { reader["WorkOrderClientLookupId"].ToString(); }
                catch { missing.Append("WorkOrderClientLookupId "); }

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
    }
}
