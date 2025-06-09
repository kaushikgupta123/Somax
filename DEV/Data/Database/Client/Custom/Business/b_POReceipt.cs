/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2015 by SOMAX Inc.
* b_POReceipt.cs (Data Object)
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID  Person             Description
* =========== ======== ================== =========================================================
* 2015-Feb-05 SOM-529  Roger Lawton       Modified the reversal and receipt functions
* 2015-Mar-05 SOM-594  Roger Lawton       Added the ReceiveBy_PersonnelId parameter 
*                                         Passed the b_POReceiptsHeader as an input parm
* 2015-Jul-21 SOM-757  Roger Lawton       Do not add part-vendor xref for nonstock
* 2016-Apr-04 SOM-960  Roger Lawton       Add SiteId
***************************************************************************************************
*/
using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.SqlClient;
using Common.Constants;

namespace Database.Business
{
   
    public partial class b_POReceiptItem 
    {
        public string Createby { get; set; }
        public string ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
        public b_PurchaseOrderLineItem POlineItem { get; set; }
        public b_Part POPart { get; set; }
        public b_PartStoreroom POPartStoreRoom { get; set; }
        public b_PartHistory POPartHistory { get; set; }
       
        public b_PurchaseOrder POPurchaseOrder { get; set; }
        public b_POReceiptHeader POHeader { get; set; }
        public Int64 VendorId { get; set; }
        //SOM-960
        public Int64 SiteId { get; set; }

        public long PurchaseOrderId { get; set; }
        public DateTime ReceivedDate { get; set; }
        public Int64 PartId { get; set; }
        public string Description { get; set; }
        public decimal TotalCost { get; set; }
        public string POClientLookupId { get; set; }
        public string PartClientLookupId { get; set; }
        public Int64 PurchaseOrderItemId { get; set; }
        public int PurchaseOrderLineNumber { get; set; }

        public DateTime OrderDate { get; set; }
        public string VendorClientLookupId { get; set; }
        public string VendorName { get; set; }
        public decimal OrderQuantity { get; set; }       
        public string Status { get; set; }
        public string DateRange { get; set; }
        //V2-947
        public  List<b_POReceiptItem> POReceiptItemlist { get; set; }
        public List<b_POReceiptItem> POLineItemsList { get; set; }
        public string AccountClientLookupId { get; set; }
        public string ChargeToClientLookupId { get; set; }
        public string ManufacturerId { get; set; }
        public int ReceiptNumber { get; set; }
        public int LineNumber { get; set; }
        public string Location { get; set; }
        //V2-947
        #region V2-1011
        public b_POHeaderUDF POHeaderUDF { get; set; }
        public List<b_Notes> listOfNotes { get; set; }
        #endregion
        #region V2-1124
        public b_EstimatedCosts POEstimatedCosts { get; set; }
        #endregion
        public void RetrievePOFromParts(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName,
        ref List<b_POReceiptItem> results
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
                results = Database.StoredProcedure.usp_POReceipt_RetrievePOFromParts.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public static b_POReceiptItem ProcessRowForRetrievePOFromParts(SqlDataReader reader)
        {
            // Create instance of object
            b_POReceiptItem poReceiptItem = new b_POReceiptItem();

            // Load the object from the database
            poReceiptItem.LoadFromDatabaseRetrievePOFromParts(reader);
            // Return result
            return poReceiptItem;
        }
        public int LoadFromDatabaseRetrievePOFromParts(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // POClientLookupId
                if (false == reader.IsDBNull(i))
                {
                    POClientLookupId = reader.GetString(i++);
                }
                else
                {
                    POClientLookupId = ""; i++;
                }

                // PurchaseOrderId
                if (false == reader.IsDBNull(i))
                {
                    PurchaseOrderId = reader.GetInt64(i++);
                }
                else
                {
                    PurchaseOrderId = 0; i++;
                }
                // OrderDate
                if (false == reader.IsDBNull(i))
                {
                    OrderDate = reader.GetDateTime(i++);
                }
                else
                {
                    OrderDate = DateTime.MinValue;
                }
                // VendorClientLookupId
                if (false == reader.IsDBNull(i))
                {
                    VendorClientLookupId = reader.GetString(i++);
                }
                else
                {
                    VendorClientLookupId = ""; i++;
                }
                //VendorName
                if (false == reader.IsDBNull(i))
                {
                    VendorName = reader.GetString(i++);
                }
                else
                {
                    VendorName = ""; i++;
                }
                //OrderQuantity
                if (false == reader.IsDBNull(i))
                {
                    OrderQuantity = reader.GetDecimal(i++);
                }
                else
                {
                    OrderQuantity = 0; i++;
                }
                // UnitCost
                if (false == reader.IsDBNull(i))
                {
                    UnitCost = reader.GetDecimal(i++);
                }
                else
                {
                    UnitCost = 0; i++;
                }
                //Status
                if (false == reader.IsDBNull(i))
                {
                    Status = reader.GetString(i++);
                }
                else
                {
                    Status = ""; i++;
                }

            }
            catch (Exception ex)
            {
                StringBuilder missing = new StringBuilder();


                try { reader["POClientLookupId"].ToString(); }
                catch { missing.Append("POClientLookupId "); }

                try { reader["PurchaseOrderId"].ToString(); }
                catch { missing.Append("PurchaseOrderId "); }

                try { reader["OrderDate"].ToString(); }
                catch { missing.Append("OrderDate "); }

                try { reader["VendorClientLookupId"].ToString(); }
                catch { missing.Append("VendorClientLookupId "); }

                try { reader["VendorName"].ToString(); }
                catch { missing.Append("VendorName "); }

                try { reader["OrderQuantity"].ToString(); }
                catch { missing.Append("OrderQuantity "); }

                try { reader["UnitCost"].ToString(); }
                catch { missing.Append("UnitCost "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

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
        public void RetrieveAllNonInvoicedList(
     SqlConnection connection,
     SqlTransaction transaction,
     long callerUserInfoId,
     string callerUserName,
     ref List<b_POReceiptItem> results
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
                results = Database.StoredProcedure.usp_POReceipt_RetrieveAllNonInvoiced.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public static b_POReceiptItem ProcessRowForPOReceiptRetrieveAllNonInvoiced(SqlDataReader reader)
        {
            // Create instance of object
            b_POReceiptItem poReceiptItem = new b_POReceiptItem();

            // Load the object from the database
            // RKL - Do not need the LoadFromDatabaseWithDepartName method
            poReceiptItem.LoadFromDatabaseForPOReceiptRetrieveAllNonInvoiced(reader);
            //workOrder.LoadFromDatabaseWithDepartName(reader);
            // Return result
            return poReceiptItem;
        }
        public int LoadFromDatabaseForPOReceiptRetrieveAllNonInvoiced(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // PurchaseOrderItemId
                if (false == reader.IsDBNull(i))
                {
                    POReceiptItemId = reader.GetInt64(i++);
                }
                else
                {
                    POReceiptItemId = 0; i++;
                }


                // PurchaseOrderId
                if (false == reader.IsDBNull(i))
                {
                    PurchaseOrderId = reader.GetInt64(i++);
                }
                else
                {
                    PurchaseOrderId = 0; i++;
                }

                //POClientLookupId
                if (false == reader.IsDBNull(i))
                {
                    POClientLookupId = reader.GetString(i++);
                }
                else
                {
                    POClientLookupId = ""; i++;
                }

                // ReceivedDate
                if (false == reader.IsDBNull(i))
                {
                    ReceivedDate = reader.GetDateTime(i++);
                }
                else
                {
                    ReceivedDate = DateTime.MinValue;
                }

                // PartId
                if (false == reader.IsDBNull(i))
                {
                    PartId = reader.GetInt64(i++);
                }
                else
                {
                    PartId = 0; i++;
                }

                // PartClientLookupId
                if (false == reader.IsDBNull(i))
                {
                    PartClientLookupId = reader.GetString(i++);
                }
                else
                {
                    PartClientLookupId = ""; i++;
                }

                // Description
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i++);
                }
                else
                {
                    Description = ""; i++;
                }
                // QuantityReceived
                if (false == reader.IsDBNull(i))
                {
                    QuantityReceived = reader.GetDecimal(i++);
                }
                else
                {
                    QuantityReceived = 0; i++;
                }

                // UnitOfMeasure
                if (false == reader.IsDBNull(i))
                {
                    UnitOfMeasure = reader.GetString(i++);
                }
                else
                {
                    UnitOfMeasure = ""; i++;
                }

                // UnitCost
                if (false == reader.IsDBNull(i))
                {
                    UnitCost = reader.GetDecimal(i++);
                }
                else
                {
                    UnitCost = 0; i++;
                }

                // TotalCost
                if (false == reader.IsDBNull(i))
                {
                    TotalCost = reader.GetDecimal(i++);
                }
                else
                {
                    TotalCost = 0; i++;
                }
                // AccountId
                if (false == reader.IsDBNull(i))
                {
                    AccountId = reader.GetInt64(i++);
                }
                else
                {
                    AccountId = 0; i++;
                }
                i++;
            }
            catch (Exception ex)
            {
                StringBuilder missing = new StringBuilder();


                try { reader["POReceiptItemId"].ToString(); }
                catch { missing.Append("POReceiptItemId "); }

                try { reader["PurchaseOrderId"].ToString(); }
                catch { missing.Append("PurchaseOrderId "); }

                try { reader["PartClientLookupId"].ToString(); }
                catch { missing.Append("PartClientLookupId "); }

                try { reader["ReceivedDate"].ToString(); }
                catch { missing.Append("ReceivedDate "); }

                try { reader["PartId"].ToString(); }
                catch { missing.Append("PartId "); }

                try { reader["PartClientLookupId"].ToString(); }
                catch { missing.Append("PartClientLookupId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["QuantityReceived"].ToString(); }
                catch { missing.Append("QuantityReceived "); }

                try { reader["UnitOfMeasure"].ToString(); }
                catch { missing.Append("UnitOfMeasure "); }

                try { reader["UnitCost"].ToString(); }
                catch { missing.Append("UnitCost "); }

                try { reader["TotalCost"].ToString(); }
                catch { missing.Append("TotalCost "); }

                try { reader["AccountId"].ToString(); }
                catch { missing.Append("AccountId "); }

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
        public void PO_ReverseReceipt(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName)
        {
            SqlCommand command = null;
            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                // Run the Receipt Item Update function
                Database.StoredProcedure.usp_POReceiptItem_UpdateByPK.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
                // Run the POLine and Part/PartStoreroom function  usp_POReceipt_Update_POLine_Part_PartStoreRoom
                Database.StoredProcedure.usp_POReceipt_Update_POLine_Part_PartStoreRoom.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
                // Run the Part History 
                // Check to see if the issue is a return or not (negative quantity)
                if (POPart.PartId != 0)
                {
                    POPartHistory.TransactionType = PartHistoryTranTypes.ReceiptReverse;
                   Database.StoredProcedure.usp_PartHistory_Create.CallStoredProcedure(command, callerUserInfoId, callerUserName, POPartHistory);
                }
                else if (POPart.PartId == 0)
                {
                    POPartHistory.TransactionType = PartHistoryTranTypes.ReceiptReverse;
                    Database.StoredProcedure.usp_PartHistory_Create.CallStoredProcedure(command, callerUserInfoId, callerUserName, POPartHistory);
                    // V2-342 - 2020-Apr-09 - RKL
                    // The same Part History data contract is used to create the reverse and the resulting part isuee 
                    // The quantity must be set to negative for the issue.
                    POPartHistory.TransactionType = PartHistoryTranTypes.PurchaseIssue;
                    POPartHistory.TransactionQuantity = POPartHistory.TransactionQuantity * -1;
                    POPartHistory.TransactionDate = DateTime.UtcNow;
                   Database.StoredProcedure.usp_PartHistory_Create.CallStoredProcedure(command, callerUserInfoId, callerUserName, POPartHistory);
                }
                // Update the purchase order status
                Database.StoredProcedure.usp_PurchaseOrder_UpdateStatus.CallStoredProcedure(command, callerUserInfoId, callerUserName, POPurchaseOrder, POHeader);
                //Database.StoredProcedure.usp_PurchaseOrder_UpdateByPK.CallStoredProcedure(command, callerUserInfoId, callerUserName, POPurchaseOrder);


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

        public void PO_Receipt(
       SqlConnection connection,
       SqlTransaction transaction,
       long callerUserInfoId,
       string callerUserName)
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                Database.StoredProcedure.usp_POReceiptItem_Create.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
                Database.StoredProcedure.usp_POReceipt_Update_POLine_Part_PartStoreRoom_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
                if (POPart.PartId == 0 || (POPart.PartId > 0 && POlineItem.ChargeToId > 0)) // if condition modified for V2-1124 from (POPart.PartId == 0) to (POPart.PartId == 0 || (POPart.PartId > 0 && POlineItem.ChargeToId > 0))
                {
                    POPartHistory.TransactionType = PartHistoryTranTypes.Receipt;
                    Database.StoredProcedure.usp_PartHistory_Create.CallStoredProcedure(command, callerUserInfoId, callerUserName, POPartHistory);
                    POPartHistory.TransactionType = PartHistoryTranTypes.PurchaseIssue;
                    POPartHistory.TransactionDate = DateTime.UtcNow;
                    Database.StoredProcedure.usp_PartHistory_Create.CallStoredProcedure(command, callerUserInfoId, callerUserName, POPartHistory);
                    #region V2-1124
                    if (POEstimatedCosts.EstimatedCostsId > 0)
                    {
                        POEstimatedCosts.Status = PurchaseOrderStatusConstants.Complete;
                        POEstimatedCosts.PartHistoryId = POPartHistory.PartHistoryId;
                        StoredProcedure.usp_EstimatedCosts_UpdateByPK.CallStoredProcedure(command, callerUserInfoId, callerUserName, POEstimatedCosts);
                    }
                    #endregion
                }
                else if (POPart.PartId != 0)
                {
                    POPartHistory.TransactionType = PartHistoryTranTypes.Receipt;
                    Database.StoredProcedure.usp_PartHistory_Create.CallStoredProcedure(command, callerUserInfoId, callerUserName, POPartHistory);
                    // SOM-757 - Moved from after status update to here 
                    Database.StoredProcedure.usp_POReceiptItem_PO_Receipt_Part_Vendor_Xref.CallStoredProcedure(command, callerUserInfoId, callerUserName, POPurchaseOrder, POlineItem);
                }
                // SOM-529 - Update the purchase order status
                Database.StoredProcedure.usp_PurchaseOrder_UpdateStatus.CallStoredProcedure(command, callerUserInfoId, callerUserName, POPurchaseOrder, POHeader);



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


        public void Create_POReceiptHeader(
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
               Database.StoredProcedure.usp_POReceiptHeader_Create_AutoReceiptNo.CallStoredProcedure(command, callerUserInfoId, callerUserName, POHeader);
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

        public void PurchaseOrderReceiptCreate(
         SqlConnection connection,
         SqlTransaction transaction,
         long callerUserInfoId,
         string callerUserName)
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                Database.StoredProcedure.usp_POReceiptItem_Create.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
               Database.StoredProcedure.usp_POReceipt_Update_POLine_Part_PartStoreRoom.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
                if (POPart.PartId != 0)
                {
                    POPartHistory.TransactionType = PartHistoryTranTypes.Receipt;
                    Database.StoredProcedure.usp_PartHistory_Create.CallStoredProcedure(command, callerUserInfoId, callerUserName, POPartHistory);
                }
                else if (POPart.PartId == 0)
                {
                    POPartHistory.TransactionType = PartHistoryTranTypes.Receipt;
                    Database.StoredProcedure.usp_PartHistory_Create.CallStoredProcedure(command, callerUserInfoId, callerUserName, POPartHistory);
                    POPartHistory.TransactionType = PartHistoryTranTypes.PurchaseIssue;
                    POPartHistory.TransactionDate = DateTime.UtcNow;
                   Database.StoredProcedure.usp_PartHistory_Create.CallStoredProcedure(command, callerUserInfoId, callerUserName, POPartHistory);
                }
                // SOM-529 - Update the purchase order status
                Database.StoredProcedure.usp_PurchaseOrder_UpdateStatus.CallStoredProcedure(command, callerUserInfoId, callerUserName, POPurchaseOrder, POHeader);
                Database.StoredProcedure.usp_POReceiptItem_PO_Receipt_Part_Vendor_Xref.CallStoredProcedure(command, callerUserInfoId, callerUserName, POPurchaseOrder, POlineItem);



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


        public void InsertIntoDatabaseByPk(
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
                Database.StoredProcedure.usp_POReceiptItem_CreateByPK.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        #region V2-947
        public static b_POReceiptItem ProcessRowForPOReceiptRetrieveAllPrint(SqlDataReader reader)
        {
            // Create instance of object
            b_POReceiptItem poReceiptItem = new b_POReceiptItem();

            // Load the object from the database
            poReceiptItem.LoadFromDatabaseForProcessRowForPOReceiptRetrieveAllPrint(reader);
            
            // Return result
            return poReceiptItem;
        }
        public int LoadFromDatabaseForProcessRowForPOReceiptRetrieveAllPrint(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // LineNumber
                if (false == reader.IsDBNull(i))
                {
                    LineNumber = reader.GetInt32(i++);
                }
                else
                {
                    LineNumber = 0; i++;
                }
                // PartClientLookupId
                if (false == reader.IsDBNull(i))
                {
                    PartClientLookupId = reader.GetString(i++);
                }
                else
                {
                    PartClientLookupId = ""; i++;
                }
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i++);
                }
                else
                {
                    Description = ""; i++;
                }

                //ManufacturerId
                if (false == reader.IsDBNull(i))
                {
                    ManufacturerId = reader.GetString(i++);
                }
                else
                {
                    ManufacturerId = ""; i++;
                }


                //Location
                if (false == reader.IsDBNull(i))
                {
                    Location = reader.GetString(i++);
                }
                else
                {
                    Location = ""; i++;
                }

                // QuantityReceived
                if (false == reader.IsDBNull(i))
                {
                    QuantityReceived = reader.GetDecimal(i++);
                }
                else
                {
                    QuantityReceived = 0; i++;
                }

                // UnitOfMeasure
                if (false == reader.IsDBNull(i))
                {
                    UnitOfMeasure = reader.GetString(i++);
                }
                else
                {
                    UnitOfMeasure = ""; i++;
                }

                // UnitCost
                if (false == reader.IsDBNull(i))
                {
                    UnitCost = reader.GetDecimal(i++);
                }
                else
                {
                    UnitCost = 0; i++;
                }

                // TotalCost
                if (false == reader.IsDBNull(i))
                {
                    TotalCost = reader.GetDecimal(i++);
                }
                else
                {
                    TotalCost = 0; i++;
                }
                // ReceiptNumber
                if (false == reader.IsDBNull(i))
                {
                    ReceiptNumber = reader.GetInt32(i++);
                }
                else
                {
                    ReceiptNumber = 0; i++;
                }
                // AccountClientLookupId
                if (false == reader.IsDBNull(i))
                {
                    AccountClientLookupId = reader.GetString(i++);
                }
                else
                {
                    AccountClientLookupId = ""; i++;
                }
                // ChargeToClientLookupId
                if (false == reader.IsDBNull(i))
                {
                    ChargeToClientLookupId = reader.GetString(i++);
                }
                else
                {
                    ChargeToClientLookupId = ""; i++;
                }
                i++;
            }
            catch (Exception ex)
            {
                StringBuilder missing = new StringBuilder();

                try { reader["LineNumber"].ToString(); }
                catch { missing.Append("LineNumber "); }

                try { reader["PartClientLookUpId"].ToString(); }
                catch { missing.Append("PartClientLookUpId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["ManufacturerId"].ToString(); }
                catch { missing.Append("ManufacturerId"); }

                try { reader["Location"].ToString(); }
                catch { missing.Append("Location "); }

                try { reader["QuantityReceived"].ToString(); }
                catch { missing.Append("QuantityReceived "); }

                try { reader["UnitOfMeasure"].ToString(); }
                catch { missing.Append("UnitOfMeasure "); }

                try { reader["UnitCost"].ToString(); }
                catch { missing.Append("UnitCost "); }

                try { reader["TotalCost"].ToString(); }
                catch { missing.Append("TotalCost "); }

                try { reader["ReceiptNumber"].ToString(); }
                catch { missing.Append("ReceiptNumber "); }

                try { reader["AccountClientLookupId"].ToString(); }
                catch { missing.Append("AccountClientLookupId"); }

                try { reader["ChargeToClientLookupId"].ToString(); }
                catch { missing.Append("ChargeToClientLookupId"); }

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
        public void RetrieveAllPrint(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref b_POReceiptItem results
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
                results = Database.StoredProcedure.usp_PORPrint_AllRetrieveByPurchaseOrderIdAndPOReceiptHeaderId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
