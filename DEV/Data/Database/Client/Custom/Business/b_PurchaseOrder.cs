/*
**************************************************************************************************
* PROPRIETARY DATA 
**************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc. and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
**************************************************************************************************
* Copyright (c) 2014-2017 by SOMAX Inc. All rights reserved. 
**************************************************************************************************
* Date         JIRA Item Person            Description
* ===========  ========= ================= =======================================================
* 2017-Apr-04  SOM-1276  Nick Fuchs        Added Vendor Customer Account
* 2017-Sep-18  SOM-1413  Nick Fuchs        Added Buyer Name                                          
**************************************************************************************************
*/
using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.Business
{
    public partial class b_PurchaseOrder
    {
        #region Property
        public string VendorName { get; set; }
        public string VendorPhoneNumber { get; set; }
        public string VendorCustomerAccount { get; set; }
        public string VendorEmailAddress { get; set; }
        public string VendorClientLookupId { get; set; }
        public string Creator_PersonnelName { get; set; }
        public string Completed_PersonnelName { get; set; }
        public string Buyer_PersonnelName { get; set; }
        public string CreateBy_PersonnelName { get; set; }
        public string PersonnelEmail { get; set; }

        public int CountLineItem { get; set; }
        public decimal TotalCost { get; set; }
        public int CustomQueryDisplayId { get; set; }
        public string DateRange { get; set; }
        public string DateColumn { get; set; }
        public DateTime CreateDate { get; set; }

        public string VendorAddress1 { get; set; }
        public string VendorAddress2 { get; set; }
        public string VendorAddress3 { get; set; }
        public string VendorAddressCity { get; set; }
        public string VendorAddressCountry { get; set; }
        public string VendorAddressPostCode { get; set; }
        public string VendorAddressState { get; set; }
        public string SiteName { get; set; }
        public string SiteAddress1 { get; set; }
        public string SiteAddress2 { get; set; }
        public string SiteAddress3 { get; set; }
        public string SiteAddressCity { get; set; }
        public string SiteAddressCountry { get; set; }
        public string SiteAddressPostCode { get; set; }
        public string SiteAddressState { get; set; }
        public string SiteBillToName { get; set; }
        public string SiteBillToAddress1 { get; set; }
        public string SiteBillToAddress2 { get; set; }
        public string SiteBillToAddress3 { get; set; }
        public string SiteBillToAddressCity { get; set; }
        public string SiteBillToAddressCountry { get; set; }
        public string SiteBillToAddressPostCode { get; set; }
        public string SiteBillToAddressState { get; set; }
        public string SiteBillToComment { get; set; }
        public string WorkFlowMessageForceComplete { get; set; }
        public Int64 FilterValue { get; set; } //--SOM-892--//
        // som-939
        public long POImportId { get; set; }
        public string UnitOfMeasure { get; set; }
        //- SOM: 936
        public string ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
        public string CreateBy { get; set; }

        //V2-305
        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public string offset1 { get; set; }
        public string nextrow { get; set; }
        public string CreatedDate { get; set; }
        public string PhoneNo { get; set; }
        public string CompleteDT { get; set; }
        public string Buyer { get; set; }
        public string TtlCost { get; set; }
        public string SearchText { get; set; }
        public string PartClientLookupId { get; set; }
        public string Description { get; set; }
        public UtilityAdd utilityAdd { get; set; }
        public Int32 ChildCount { get; set; }
        public int TotalCount { get; set; }
        //V2-347
        public string CompleteStartDateVw { get; set; }
        public string CompleteEndDateVw { get; set; }
        public string StartCompleteDate { get; set; }
        public string EndCompleteDate { get; set; }
        public string StartCreateDate { get; set; }
        public string EndCreateDate { get; set; }

        //V2-364
        public string strCreatedDate { get; set; }//V2-331
        public string CreateStartDateVw { get; set; }
        public string CreateEndDateVw { get; set; }
        public string StoreroomName { get; set; }  //V2-738
        public string PurchaseOrderIDList { get; set; } // V2-946
        public List<b_PurchaseOrder> listOfPO { get; set; } // V2-946
        public List<b_PurchaseOrderLineItem> listOfPOLineItem { get; set; } // V2-946
        public List<b_POHeaderUDF> listOfPOHeaderUDF { get; set; } // V2-946
        public List<b_POLineUDF> listOfPOLineUDF { get; set; } // V2-946
        public List<b_Attachment> listOfAttachment { get; set; } //V2-949
        //V2-947
        public decimal FreightAmount { get; set; }
        public string FreightBill { get; set; }
        public string PackingSlip { get; set; }
        public string Comments { get; set; }
        public string ReceivedPersonnelName { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public long ReceiptNumber { get; set; }
        //V2-947
        #region V2-1073
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string AccountClientLookupId { get; set; }
        public string AccountName { get; set; }
        public int LineNumber { get; set; }
        public string POLDescription { get; set; }
        public decimal OrderQuantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal LineTotal { get; set; }
        public decimal QuantityReceived { get; set; }
        public decimal ReceivedTotalCost { get; set; }
        public string LineItemStatus { get; set; }
        #endregion
        public string Shipto_ClientLookUpId { get; set; }  //V2-1086
        #region V2-1079
        public string TermDescription { get; set; }
        public string ShipToName { get; set; }
        public string ShipToAddress1 { get; set; }
        public string ShipToAddress2 { get; set; }
        public string ShipToAddress3 { get; set; }
        public string ShipToAddressCity { get; set; }
        public string ShipToAddressState { get; set; }
        public string ShipToAddressPostCode { get; set; }
        public string ShipToAddressCountry { get; set; }
        public string Buyer_Phone { get; set; }
        #endregion
        //V2-1112
        public DateTime? StatusDate { get; set; }
        #endregion

        public void InsertByForeignKeysIntoDatabase(
          SqlConnection connection,
          SqlTransaction transaction,
          long callerUserInfoId,
          string callerUserName, b_WorkFlowLog objworkflowlog)
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                // We have our command and we the SQL transaction is active
                // First - Create the purchase order
                //Database.StoredProcedure.usp_PurchaseOrder_Create.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
                Database.StoredProcedure.usp_PurchaseOrder_CreateByPKForeignKey.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
                //Now add workorderflow
                if (this.PurchaseOrderId > 0)
                {
                    objworkflowlog.ObjectId = this.PurchaseOrderId;

                    Database.StoredProcedure.usp_WorkFlowLog_Create.CallStoredProcedure(command, callerUserInfoId, callerUserName, objworkflowlog);
                }


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

        public void InsertByForeignKeysIntoDatabase_V2(
  SqlConnection connection,
  SqlTransaction transaction,
  long callerUserInfoId,
  string callerUserName, b_WorkFlowLog objworkflowlog)
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                // We have our command and we the SQL transaction is active
                // First - Create the purchase order
                //Database.StoredProcedure.usp_PurchaseOrder_Create.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
                Database.StoredProcedure.usp_PurchaseOrder_CreateByPKForeignKey_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
                //Now add workorderflow
                if (this.PurchaseOrderId > 0)
                {
                    objworkflowlog.ObjectId = this.PurchaseOrderId;

                    Database.StoredProcedure.usp_WorkFlowLog_Create.CallStoredProcedure(command, callerUserInfoId, callerUserName, objworkflowlog);
                }


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


        public void ValidateByClientLookupIdFromDatabase(
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
            List<b_StoredProcValidationError> results2 = null;
            data = new List<b_StoredProcValidationError>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;
                b_Vendor vendor = new b_Vendor();
                vendor.ClientId = this.ClientId;
                vendor.ClientLookupId = this.VendorClientLookupId;
                vendor.SiteId = this.SiteId;
                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_PurchaseOrder_ValidateByClientLookupId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
                results2 = Database.StoredProcedure.usp_Vendor_ValidateByClientLookupId_SiteId.CallStoredProcedure(command, callerUserInfoId, callerUserName, vendor);
                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_StoredProcValidationError>();
                }
                if (results2 != null)
                {
                    data.AddRange(results2);
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

        public void RetrieveByClientIDAndSiteIdAndClientLookUpId(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName,
            ref List<b_PurchaseOrder> results
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

                results = Database.StoredProcedure.usp_PurchaseOrder_RetrieveByClientIDAndSiteIdAndClientLookUpId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        //som-939//
        public void UpdatePurchaseOrderLineItemByPurchaseOrderId(
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
                Database.StoredProcedure.usp_PurchaseOrder_UpdateLineItemByPurchaseOrderId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        /// <summary>
        /// This method runs the usp_PurchaseOrder_UpdateStatus
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <param name="callerUserInfoId"></param>
        /// <param name="callerUserName"></param>
        /// <param name="ReceiptHeader"></param>
        public void UpdateStatus(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            b_POReceiptHeader ReceiptHeader)
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                Database.StoredProcedure.usp_PurchaseOrder_UpdateStatus.CallStoredProcedure(command, callerUserInfoId, callerUserName, this, ReceiptHeader);
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

        public void RetrieveByForeignKeysFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName
        )
        {
            Database.SqlClient.ProcessRow<b_PurchaseOrder> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_PurchaseOrder>(reader => { this.LoadFromDatabaseByPKForeignKey(reader); return this; });
                Database.StoredProcedure.usp_PurchaseOrder_RetrieveByPKForeignKeys.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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

        public void RetrieveAllForSearch(
 SqlConnection connection,
 SqlTransaction transaction,
 long callerUserInfoId,
 string callerUserName,
 ref List<b_PurchaseOrder> results
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

                results = Database.StoredProcedure.usp_PurchaseOrder_RetrieveAllForSearch.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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


        #region V2-331

        public void RetrievePOReceiptChunkSearch(
     SqlConnection connection,
     SqlTransaction transaction,
     long callerUserInfoId,
     string callerUserName,
     ref List<b_PurchaseOrder> results
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

                results = Database.StoredProcedure.usp_PurchaseOrderReceipt_RetrieveChunkSearch_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        public void ValidateByVendorPartNumberPurchaseRequestUnitOfMeasure(SqlConnection connection,
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
                results = Database.StoredProcedure.usp_PurchaseOrder_ValidateByVendorPartNumberPurchaseRequestUnitOfMeasure.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public void LoadFromDatabaseByPKForeignKey(SqlDataReader reader)
        {
            int i = this.LoadFromDatabase(reader);

            try
            {
                if (false == reader.IsDBNull(i))
                {
                    CreateDate = reader.GetDateTime(i);
                }
                else
                {
                    CreateDate = DateTime.MinValue;
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
                    VendorPhoneNumber = reader.GetString(i);
                }
                else
                {
                    VendorPhoneNumber = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    VendorEmailAddress = reader.GetString(i);
                }
                else
                {
                    VendorEmailAddress = "";
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
                    Creator_PersonnelName = reader.GetString(i);
                }
                else
                {
                    Creator_PersonnelName = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Completed_PersonnelName = reader.GetString(i);
                }
                else
                {
                    Completed_PersonnelName = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    CountLineItem = reader.GetInt32(i);
                }
                else
                {
                    CountLineItem = 0;
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
                    VendorAddress1 = reader.GetString(i);
                }
                else
                {
                    VendorAddress1 = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    VendorAddress2 = reader.GetString(i);
                }
                else
                {
                    VendorAddress2 = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    VendorAddress3 = reader.GetString(i);
                }
                else
                {
                    VendorAddress3 = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    VendorAddressCity = reader.GetString(i);
                }
                else
                {
                    VendorAddressCity = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    VendorAddressCountry = reader.GetString(i);
                }
                else
                {
                    VendorAddressCountry = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    VendorAddressPostCode = reader.GetString(i);
                }
                else
                {
                    VendorAddressPostCode = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    VendorAddressState = reader.GetString(i);
                }
                else
                {
                    VendorAddressState = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    SiteName = reader.GetString(i);
                }
                else
                {
                    SiteName = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    SiteAddress1 = reader.GetString(i);
                }
                else
                {
                    SiteAddress1 = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    SiteAddress2 = reader.GetString(i);
                }
                else
                {
                    SiteAddress2 = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    SiteAddress3 = reader.GetString(i);
                }
                else
                {
                    SiteAddress3 = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    SiteAddressCity = reader.GetString(i);
                }
                else
                {
                    SiteAddressCity = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    SiteAddressCountry = reader.GetString(i);
                }
                else
                {
                    SiteAddressCountry = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    SiteAddressPostCode = reader.GetString(i);
                }
                else
                {
                    SiteAddressPostCode = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    SiteAddressState = reader.GetString(i);
                }
                else
                {
                    SiteAddressState = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    SiteBillToName = reader.GetString(i);
                }
                else
                {
                    SiteBillToName = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    SiteBillToAddress1 = reader.GetString(i);
                }
                else
                {
                    SiteBillToAddress1 = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    SiteBillToAddress2 = reader.GetString(i);
                }
                else
                {
                    SiteBillToAddress2 = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    SiteBillToAddress3 = reader.GetString(i);
                }
                else
                {
                    SiteBillToAddress3 = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    SiteBillToAddressCity = reader.GetString(i);
                }
                else
                {
                    SiteBillToAddressCity = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    SiteBillToAddressCountry = reader.GetString(i);
                }
                else
                {
                    SiteBillToAddressCountry = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    SiteBillToAddressPostCode = reader.GetString(i);
                }
                else
                {
                    SiteBillToAddressPostCode = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    SiteBillToAddressState = reader.GetString(i);
                }
                else
                {
                    SiteBillToAddressState = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    SiteBillToComment = reader.GetString(i);
                }
                else
                {
                    SiteBillToComment = "";
                }
                i++;

                //- SOM: 936
                if (false == reader.IsDBNull(i))
                {
                    ModifyBy = reader.GetString(i);
                }
                else
                {
                    ModifyBy = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    ModifyDate = reader.GetDateTime(i);
                }
                else
                {
                    ModifyDate = DateTime.Now;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    CreateBy = reader.GetString(i);
                }
                else
                {
                    CreateBy = "";
                }
                i++;
                //SOM-1276
                if (false == reader.IsDBNull(i))
                {
                    VendorCustomerAccount = reader.GetString(i);
                }
                else
                {
                    VendorCustomerAccount = "";
                }
                i++;
                //SOM-1413
                if (false == reader.IsDBNull(i))
                {
                    Buyer_PersonnelName = reader.GetString(i);
                }
                else
                {
                    Buyer_PersonnelName = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    PersonnelEmail = reader.GetString(i);
                }
                else
                {
                    PersonnelEmail = "";
                }
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate"); }

                try { reader["VendorName"].ToString(); }
                catch { missing.Append("VendorName "); }

                try { reader["VendorPhoneNumber"].ToString(); }
                catch { missing.Append("VendorPhoneNumber "); }

                try { reader["VendorEmailAddress"].ToString(); }
                catch { missing.Append("VendorEmailAddress "); }

                try { reader["VendorClientLookupId"].ToString(); }
                catch { missing.Append("VendorClientLookupId "); }

                try { reader["Creator_PersonnelName"].ToString(); }
                catch { missing.Append("Creator_PersonnelName "); }

                try { reader["Completed_PersonnelName"].ToString(); }
                catch { missing.Append("Completed_PersonnelName "); }

                try { reader["CountLineItem"].ToString(); }
                catch { missing.Append("CountLineItem "); }

                try { reader["TotalCost"].ToString(); }
                catch { missing.Append("TotalCost "); }

                try { reader["VendorAddress1"].ToString(); }
                catch { missing.Append("VendorAddress1 "); }

                try { reader["VendorAddress2"].ToString(); }
                catch { missing.Append("VendorAddress2 "); }

                try { reader["VendorAddress3"].ToString(); }
                catch { missing.Append("VendorAddress3 "); }

                try { reader["VendorAddressCity"].ToString(); }
                catch { missing.Append("VendorAddressCity "); }

                try { reader["VendorAddressCountry"].ToString(); }
                catch { missing.Append("VendorAddressCountry "); }

                try { reader["VendorAddressPostCode"].ToString(); }
                catch { missing.Append("VendorAddressPostCode "); }

                try { reader["VendorAddressState"].ToString(); }
                catch { missing.Append("VendorAddressState "); }

                try { reader["SiteName"].ToString(); }
                catch { missing.Append("SiteName "); }

                try { reader["SiteAddress1"].ToString(); }
                catch { missing.Append("SiteAddress1 "); }

                try { reader["SiteAddress2"].ToString(); }
                catch { missing.Append("SiteAddress2 "); }

                try { reader["SiteAddress3"].ToString(); }
                catch { missing.Append("SiteAddress3 "); }

                try { reader["SiteAddressCity"].ToString(); }
                catch { missing.Append("SiteAddressCity "); }

                try { reader["SiteAddressCountry"].ToString(); }
                catch { missing.Append("SiteAddressCountry "); }

                try { reader["SiteAddressPostCode"].ToString(); }
                catch { missing.Append("SiteAddressPostCode "); }

                try { reader["SiteAddressState"].ToString(); }
                catch { missing.Append("SiteAddressState "); }

                try { reader["SiteBillToName"].ToString(); }
                catch { missing.Append("SiteBillToName "); }

                try { reader["SiteBillToAddress1"].ToString(); }
                catch { missing.Append("SiteBillToAddress1 "); }

                try { reader["SiteBillToAddress2"].ToString(); }
                catch { missing.Append("SiteBillToAddress2 "); }

                try { reader["SiteBillToAddress3"].ToString(); }
                catch { missing.Append("SiteBillToAddress3 "); }

                try { reader["SiteBillToAddressCity"].ToString(); }
                catch { missing.Append("SiteBillToAddressCity "); }

                try { reader["SiteBillToAddressCountry"].ToString(); }
                catch { missing.Append("SiteBillToAddressCountry "); }

                try { reader["SiteBillToAddressPostCode"].ToString(); }
                catch { missing.Append("SiteBillToAddressPostCode "); }

                try { reader["SiteBillToAddressState"].ToString(); }
                catch { missing.Append("SiteBillToAddressState "); }

                try { reader["SiteBillToComment"].ToString(); }
                catch { missing.Append("SiteBillToComment "); }

                try { reader["ModifyBy"].ToString(); }
                catch { missing.Append("ModifyBy "); }

                try { reader["[ModifyDate"].ToString(); }
                catch { missing.Append("[ModifyDate "); }

                try { reader["[CreateBy"].ToString(); }
                catch { missing.Append("[CreateBy "); }

                try { reader["VendorCustomerAccount"].ToString(); }
                catch { missing.Append("VendorCustomerAccount "); }

                try { reader["Buyer_PersonnelName"].ToString(); }
                catch { missing.Append("Buyer_PersonnelName "); }

                try { reader["PersonnelEmail"].ToString(); }
                catch { missing.Append("PersonnelEmail "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }

        public static b_PurchaseOrder ProcessRowForChunkSearch(SqlDataReader reader)
        {
            // Create instance of object
            b_PurchaseOrder PurchaseOrder = new b_PurchaseOrder();
            PurchaseOrder.LoadFromDatabaseForChunkSearch(reader);
            return PurchaseOrder;
        }
        public static b_PurchaseOrder ProcessRowForPurchaseOrderRetriveAllForSearch(SqlDataReader reader)
        {
            // Create instance of object
            b_PurchaseOrder PurchaseOrder = new b_PurchaseOrder();
            PurchaseOrder.LoadFromDatabaseForPOReceiptChunkSearch(reader);//V2-331
            return PurchaseOrder;
        }
        public int LoadFromDatabaseForPurchaseOrderRetriveAllForSearch(SqlDataReader reader)
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
                // PurchaseOrderId column, bigint, not null             
                if (false == reader.IsDBNull(i))
                {
                    PurchaseOrderId = reader.GetInt64(i);
                }
                else
                {
                    PurchaseOrderId = 0;
                }
                i++;
                // SiteId column, bigint, not null               
                if (false == reader.IsDBNull(i))
                {
                    SiteId = reader.GetInt64(i);
                }
                else
                {
                    SiteId = 0;
                }
                i++;
                // DepartmentId column, bigint, not null              
                if (false == reader.IsDBNull(i))
                {
                    DepartmentId = reader.GetInt64(i);
                }
                else
                {
                    DepartmentId = 0;
                }
                i++;
                // AreaId column, bigint, not null               
                if (false == reader.IsDBNull(i))
                {
                    AreaId = reader.GetInt64(i);
                }
                else
                {
                    AreaId = 0;
                }
                i++;
                // StoreroomId column, bigint, not null               
                if (false == reader.IsDBNull(i))
                {
                    StoreroomId = reader.GetInt64(i);
                }
                else
                {
                    StoreroomId = 0;
                }
                i++;
                // ClientLookupId column, nvarchar(15), not null
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = string.Empty;
                }
                i++;
                // Attention column, nvarchar(63), not null             
                if (false == reader.IsDBNull(i))
                {
                    Attention = reader.GetString(i);
                }
                else
                {
                    Attention = string.Empty;
                }
                i++;
                // Buyer_PersonnelId column, bigint, not null              
                if (false == reader.IsDBNull(i))
                {
                    Buyer_PersonnelId = reader.GetInt64(i);
                }
                else
                {
                    Buyer_PersonnelId = 0;
                }
                i++;
                // Carrier column, nvarchar(15), not null              
                if (false == reader.IsDBNull(i))
                {
                    Carrier = reader.GetString(i);
                }
                else
                {
                    Carrier = string.Empty;
                }
                i++;
                // CompleteBy_PersonnelId column, bigint, not null              
                if (false == reader.IsDBNull(i))
                {
                    CompleteBy_PersonnelId = reader.GetInt64(i);
                }
                else
                {
                    CompleteBy_PersonnelId = 0;
                }
                i++;
                // CompleteDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    CompleteDate = reader.GetDateTime(i);
                }
                else
                {
                    CompleteDate = DateTime.MinValue;
                }
                i++;
                // Creator_PersonnelId column, bigint, not null              
                if (false == reader.IsDBNull(i))
                {
                    Creator_PersonnelId = reader.GetInt64(i);
                }
                else
                {
                    Creator_PersonnelId = 0;
                }
                i++;
                // FOB column, nvarchar(15), not null             
                if (false == reader.IsDBNull(i))
                {
                    FOB = reader.GetString(i);
                }
                else
                {
                    FOB = string.Empty;
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
                // Terms column, nvarchar(15), not null             
                if (false == reader.IsDBNull(i))
                {
                    Terms = reader.GetString(i);
                }
                else
                {
                    Terms = string.Empty;
                }
                i++;
                // VendorId column, bigint, not null              
                if (false == reader.IsDBNull(i))
                {
                    VendorId = reader.GetInt64(i);
                }
                else
                {
                    VendorId = 0;
                }
                i++;
                // VoidBy_PersonnelId column, bigint, not null               
                if (false == reader.IsDBNull(i))
                {
                    VoidBy_PersonnelId = reader.GetInt64(i);
                }
                else
                {
                    VoidBy_PersonnelId = 0;
                }
                i++;
                // VoidDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    VoidDate = reader.GetDateTime(i);
                }
                else
                {
                    VoidDate = DateTime.MinValue;
                }
                i++;

                // VoidReason column, nvarchar(15), not null

                if (false == reader.IsDBNull(i))
                {
                    VoidReason = reader.GetString(i);
                }
                else
                {
                    VoidReason = string.Empty;
                }
                i++;
                //--SOM-914--//
                if (false == reader.IsDBNull(i))
                {
                    Reason = reader.GetString(i);
                }
                else
                {
                    Reason = string.Empty;
                }
                i++;
                // UpdateIndex column, int, not null
                UpdateIndex = reader.GetInt32(i++);


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

                //Vendor Name                
                if (false == reader.IsDBNull(i))
                {
                    VendorName = reader.GetString(i);
                }
                else
                {
                    VendorName = string.Empty;
                }
                i++;

                //VendorPhoneNumber              
                if (false == reader.IsDBNull(i))
                {
                    VendorPhoneNumber = reader.GetString(i);
                }
                else
                {
                    VendorPhoneNumber = string.Empty;
                }
                i++;
                //VendorClientLookupId               
                if (false == reader.IsDBNull(i))
                {
                    VendorClientLookupId = reader.GetString(i);
                }
                else
                {
                    VendorClientLookupId = string.Empty;
                }
                i++;
                //Creator_PersonnelName              
                if (false == reader.IsDBNull(i))
                {
                    Creator_PersonnelName = reader.GetString(i);
                }
                else
                {
                    Creator_PersonnelName = string.Empty;
                }
                i++;

                //Completed_PersonnelName               
                if (false == reader.IsDBNull(i))
                {
                    Completed_PersonnelName = reader.GetString(i);
                }
                else
                {
                    Completed_PersonnelName = string.Empty;
                }
                i++;

                //Buyer_PersonnelName               
                if (false == reader.IsDBNull(i))
                {
                    Buyer_PersonnelName = reader.GetString(i);
                }
                else
                {
                    Buyer_PersonnelName = string.Empty;
                }
                i++;

                //Total Cost
                TotalCost = reader.GetDecimal(i++);


            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["PurchaseOrderId"].ToString(); }
                catch { missing.Append("PurchaseOrderId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["DepartmentId"].ToString(); }
                catch { missing.Append("DepartmentId "); }

                try { reader["AreaId"].ToString(); }
                catch { missing.Append("AreaId "); }

                try { reader["StoreroomId"].ToString(); }
                catch { missing.Append("StoreroomId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Attention"].ToString(); }
                catch { missing.Append("Attention "); }

                try { reader["Buyer_PersonnelId"].ToString(); }
                catch { missing.Append("Buyer_PersonnelId "); }

                try { reader["Carrier"].ToString(); }
                catch { missing.Append("Carrier "); }

                try { reader["CompleteBy_PersonnelId"].ToString(); }
                catch { missing.Append("CompleteBy_PersonnelId "); }

                try { reader["CompleteDate"].ToString(); }
                catch { missing.Append("CompleteDate "); }

                try { reader["Creator_PersonnelId"].ToString(); }
                catch { missing.Append("Creator_PersonnelId "); }

                try { reader["FOB"].ToString(); }
                catch { missing.Append("FOB "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["Terms"].ToString(); }
                catch { missing.Append("Terms "); }

                try { reader["VendorId"].ToString(); }
                catch { missing.Append("VendorId "); }

                try { reader["VoidBy_PersonnelId"].ToString(); }
                catch { missing.Append("VoidBy_PersonnelId "); }

                try { reader["VoidDate"].ToString(); }
                catch { missing.Append("VoidDate "); }

                try { reader["VoidReason"].ToString(); }
                catch { missing.Append("VoidReason "); }

                try { reader["UpdateIndex"].ToString(); }
                catch { missing.Append("UpdateIndex "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["VendorName"].ToString(); }
                catch { missing.Append("VendorName "); }

                try { reader["VendorPhoneNumber"].ToString(); }
                catch { missing.Append("VendorPhoneNumber "); }

                try { reader["VendorClientLookupId"].ToString(); }
                catch { missing.Append("VendorClientLookupId "); }

                try { reader["Creator_PersonnelName"].ToString(); }
                catch { missing.Append("Creator_PersonnelName "); }

                try { reader["Completed_PersonnelName"].ToString(); }
                catch { missing.Append("Completed_PersonnelName "); }

                try { reader["Buyer_PersonnelName"].ToString(); }
                catch { missing.Append("Buyer_PersonnelName "); }

                try { reader["TotalCost"].ToString(); }
                catch { missing.Append("TotalCost "); }




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
        public int LoadFromDatabaseForChunkSearch(SqlDataReader reader)
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
                // PurchaseOrderId column, bigint, not null             
                if (false == reader.IsDBNull(i))
                {
                    PurchaseOrderId = reader.GetInt64(i);
                }
                else
                {
                    PurchaseOrderId = 0;
                }
                i++;
                // SiteId column, bigint, not null               
                if (false == reader.IsDBNull(i))
                {
                    SiteId = reader.GetInt64(i);
                }
                else
                {
                    SiteId = 0;
                }
                i++;
                // DepartmentId column, bigint, not null              
                if (false == reader.IsDBNull(i))
                {
                    DepartmentId = reader.GetInt64(i);
                }
                else
                {
                    DepartmentId = 0;
                }
                i++;
                // AreaId column, bigint, not null               
                if (false == reader.IsDBNull(i))
                {
                    AreaId = reader.GetInt64(i);
                }
                else
                {
                    AreaId = 0;
                }
                i++;
                // StoreroomId column, bigint, not null               
                if (false == reader.IsDBNull(i))
                {
                    StoreroomId = reader.GetInt64(i);
                }
                else
                {
                    StoreroomId = 0;
                }
                i++;
                // ClientLookupId column, nvarchar(15), not null
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = string.Empty;
                }
                i++;
                // Attention column, nvarchar(63), not null             
                if (false == reader.IsDBNull(i))
                {
                    Attention = reader.GetString(i);
                }
                else
                {
                    Attention = string.Empty;
                }
                i++;
                // Buyer_PersonnelId column, bigint, not null              
                if (false == reader.IsDBNull(i))
                {
                    Buyer_PersonnelId = reader.GetInt64(i);
                }
                else
                {
                    Buyer_PersonnelId = 0;
                }
                i++;
                // Carrier column, nvarchar(15), not null              
                if (false == reader.IsDBNull(i))
                {
                    Carrier = reader.GetString(i);
                }
                else
                {
                    Carrier = string.Empty;
                }
                i++;
                // CompleteBy_PersonnelId column, bigint, not null              
                if (false == reader.IsDBNull(i))
                {
                    CompleteBy_PersonnelId = reader.GetInt64(i);
                }
                else
                {
                    CompleteBy_PersonnelId = 0;
                }
                i++;
                // CompleteDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    CompleteDate = reader.GetDateTime(i);
                }
                else
                {
                    CompleteDate = DateTime.MinValue;
                }
                i++;
                // Creator_PersonnelId column, bigint, not null              
                if (false == reader.IsDBNull(i))
                {
                    Creator_PersonnelId = reader.GetInt64(i);
                }
                else
                {
                    Creator_PersonnelId = 0;
                }
                i++;
                // FOB column, nvarchar(15), not null             
                if (false == reader.IsDBNull(i))
                {
                    FOB = reader.GetString(i);
                }
                else
                {
                    FOB = string.Empty;
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
                // Terms column, nvarchar(15), not null             
                if (false == reader.IsDBNull(i))
                {
                    Terms = reader.GetString(i);
                }
                else
                {
                    Terms = string.Empty;
                }
                i++;
                // VendorId column, bigint, not null              
                if (false == reader.IsDBNull(i))
                {
                    VendorId = reader.GetInt64(i);
                }
                else
                {
                    VendorId = 0;
                }
                i++;
                // VoidBy_PersonnelId column, bigint, not null               
                if (false == reader.IsDBNull(i))
                {
                    VoidBy_PersonnelId = reader.GetInt64(i);
                }
                else
                {
                    VoidBy_PersonnelId = 0;
                }
                i++;
                // VoidDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    VoidDate = reader.GetDateTime(i);
                }
                else
                {
                    VoidDate = DateTime.MinValue;
                }
                i++;

                // VoidReason column, nvarchar(15), not null

                if (false == reader.IsDBNull(i))
                {
                    VoidReason = reader.GetString(i);
                }
                else
                {
                    VoidReason = string.Empty;
                }
                i++;
                //--SOM-914--//
                if (false == reader.IsDBNull(i))
                {
                    Reason = reader.GetString(i);
                }
                else
                {
                    Reason = string.Empty;
                }
                i++;
                // UpdateIndex column, int, not null
                UpdateIndex = reader.GetInt32(i++);


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

                //Vendor Name                
                if (false == reader.IsDBNull(i))
                {
                    VendorName = reader.GetString(i);
                }
                else
                {
                    VendorName = string.Empty;
                }
                i++;

                //VendorPhoneNumber              
                if (false == reader.IsDBNull(i))
                {
                    VendorPhoneNumber = reader.GetString(i);
                }
                else
                {
                    VendorPhoneNumber = string.Empty;
                }
                i++;
                //VendorClientLookupId               
                if (false == reader.IsDBNull(i))
                {
                    VendorClientLookupId = reader.GetString(i);
                }
                else
                {
                    VendorClientLookupId = string.Empty;
                }
                i++;
                //Creator_PersonnelName              
                if (false == reader.IsDBNull(i))
                {
                    Creator_PersonnelName = reader.GetString(i);
                }
                else
                {
                    Creator_PersonnelName = string.Empty;
                }
                i++;

                //Completed_PersonnelName               
                if (false == reader.IsDBNull(i))
                {
                    Completed_PersonnelName = reader.GetString(i);
                }
                else
                {
                    Completed_PersonnelName = string.Empty;
                }
                i++;

                //Buyer_PersonnelName               
                if (false == reader.IsDBNull(i))
                {
                    Buyer_PersonnelName = reader.GetString(i);
                }
                else
                {
                    Buyer_PersonnelName = string.Empty;
                }
                i++;

                //PartClientLookupId               
                if (false == reader.IsDBNull(i))
                {
                    PartClientLookupId = reader.GetString(i);
                }
                else
                {
                    PartClientLookupId = string.Empty;
                }
                i++;


                //Description
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i);
                }
                else
                {
                    Description = string.Empty;
                }
                i++;
                //Child Count
                ChildCount = reader.GetInt32(i);
                i++;
                //Total Cost
                TotalCost = reader.GetDecimal(i);
                i++;
                //Required
                if (false == reader.IsDBNull(i))
                {
                    Required = reader.GetDateTime(i);
                }
                else
                {
                    Required = DateTime.MinValue;
                }
                i++;
                //Total Count
                TotalCount = reader.GetInt32(i);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["PurchaseOrderId"].ToString(); }
                catch { missing.Append("PurchaseOrderId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["DepartmentId"].ToString(); }
                catch { missing.Append("DepartmentId "); }

                try { reader["AreaId"].ToString(); }
                catch { missing.Append("AreaId "); }

                try { reader["StoreroomId"].ToString(); }
                catch { missing.Append("StoreroomId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Attention"].ToString(); }
                catch { missing.Append("Attention "); }

                try { reader["Buyer_PersonnelId"].ToString(); }
                catch { missing.Append("Buyer_PersonnelId "); }

                try { reader["Carrier"].ToString(); }
                catch { missing.Append("Carrier "); }

                try { reader["CompleteBy_PersonnelId"].ToString(); }
                catch { missing.Append("CompleteBy_PersonnelId "); }

                try { reader["CompleteDate"].ToString(); }
                catch { missing.Append("CompleteDate "); }

                try { reader["Creator_PersonnelId"].ToString(); }
                catch { missing.Append("Creator_PersonnelId "); }

                try { reader["FOB"].ToString(); }
                catch { missing.Append("FOB "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["Terms"].ToString(); }
                catch { missing.Append("Terms "); }

                try { reader["VendorId"].ToString(); }
                catch { missing.Append("VendorId "); }

                try { reader["VoidBy_PersonnelId"].ToString(); }
                catch { missing.Append("VoidBy_PersonnelId "); }

                try { reader["VoidDate"].ToString(); }
                catch { missing.Append("VoidDate "); }

                try { reader["VoidReason"].ToString(); }
                catch { missing.Append("VoidReason "); }

                try { reader["UpdateIndex"].ToString(); }
                catch { missing.Append("UpdateIndex "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["VendorName"].ToString(); }
                catch { missing.Append("VendorName "); }

                try { reader["VendorPhoneNumber"].ToString(); }
                catch { missing.Append("VendorPhoneNumber "); }

                try { reader["VendorClientLookupId"].ToString(); }
                catch { missing.Append("VendorClientLookupId "); }

                try { reader["Creator_PersonnelName"].ToString(); }
                catch { missing.Append("Creator_PersonnelName "); }

                try { reader["Completed_PersonnelName"].ToString(); }
                catch { missing.Append("Completed_PersonnelName "); }

                try { reader["Buyer_PersonnelName"].ToString(); }
                catch { missing.Append("Buyer_PersonnelName "); }

                try { reader["ChildCount"].ToString(); }
                catch { missing.Append("ChildCount "); }

                try { reader["TotalCost"].ToString(); }
                catch { missing.Append("TotalCost "); }
                
                try { reader["Required"].ToString(); }
                catch { missing.Append("Required "); }

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
        #region V2-331
        public int LoadFromDatabaseForPOReceiptChunkSearch(SqlDataReader reader)
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
                // PurchaseOrderId column, bigint, not null             
                if (false == reader.IsDBNull(i))
                {
                    PurchaseOrderId = reader.GetInt64(i);
                }
                else
                {
                    PurchaseOrderId = 0;
                }
                i++;
                // SiteId column, bigint, not null               
                if (false == reader.IsDBNull(i))
                {
                    SiteId = reader.GetInt64(i);
                }
                else
                {
                    SiteId = 0;
                }
                i++;
                // DepartmentId column, bigint, not null              
                if (false == reader.IsDBNull(i))
                {
                    DepartmentId = reader.GetInt64(i);
                }
                else
                {
                    DepartmentId = 0;
                }
                i++;
                // AreaId column, bigint, not null               
                if (false == reader.IsDBNull(i))
                {
                    AreaId = reader.GetInt64(i);
                }
                else
                {
                    AreaId = 0;
                }
                i++;
                // StoreroomId column, bigint, not null               
                if (false == reader.IsDBNull(i))
                {
                    StoreroomId = reader.GetInt64(i);
                }
                else
                {
                    StoreroomId = 0;
                }
                i++;
                // ClientLookupId column, nvarchar(15), not null
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = string.Empty;
                }
                i++;
                // Attention column, nvarchar(63), not null             
                if (false == reader.IsDBNull(i))
                {
                    Attention = reader.GetString(i);
                }
                else
                {
                    Attention = string.Empty;
                }
                i++;
                // Buyer_PersonnelId column, bigint, not null              
                if (false == reader.IsDBNull(i))
                {
                    Buyer_PersonnelId = reader.GetInt64(i);
                }
                else
                {
                    Buyer_PersonnelId = 0;
                }
                i++;
                // Carrier column, nvarchar(15), not null              
                if (false == reader.IsDBNull(i))
                {
                    Carrier = reader.GetString(i);
                }
                else
                {
                    Carrier = string.Empty;
                }
                i++;
                // CompleteBy_PersonnelId column, bigint, not null              
                if (false == reader.IsDBNull(i))
                {
                    CompleteBy_PersonnelId = reader.GetInt64(i);
                }
                else
                {
                    CompleteBy_PersonnelId = 0;
                }
                i++;
                // CompleteDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    CompleteDate = reader.GetDateTime(i);
                }
                else
                {
                    CompleteDate = DateTime.MinValue;
                }
                i++;
                // Creator_PersonnelId column, bigint, not null              
                if (false == reader.IsDBNull(i))
                {
                    Creator_PersonnelId = reader.GetInt64(i);
                }
                else
                {
                    Creator_PersonnelId = 0;
                }
                i++;
                // FOB column, nvarchar(15), not null             
                if (false == reader.IsDBNull(i))
                {
                    FOB = reader.GetString(i);
                }
                else
                {
                    FOB = string.Empty;
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
                // Terms column, nvarchar(15), not null             
                if (false == reader.IsDBNull(i))
                {
                    Terms = reader.GetString(i);
                }
                else
                {
                    Terms = string.Empty;
                }
                i++;
                // VendorId column, bigint, not null              
                if (false == reader.IsDBNull(i))
                {
                    VendorId = reader.GetInt64(i);
                }
                else
                {
                    VendorId = 0;
                }
                i++;
                // VoidBy_PersonnelId column, bigint, not null               
                if (false == reader.IsDBNull(i))
                {
                    VoidBy_PersonnelId = reader.GetInt64(i);
                }
                else
                {
                    VoidBy_PersonnelId = 0;
                }
                i++;
                // VoidDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    VoidDate = reader.GetDateTime(i);
                }
                else
                {
                    VoidDate = DateTime.MinValue;
                }
                i++;

                // VoidReason column, nvarchar(15), not null

                if (false == reader.IsDBNull(i))
                {
                    VoidReason = reader.GetString(i);
                }
                else
                {
                    VoidReason = string.Empty;
                }
                i++;
                //--SOM-914--//
                if (false == reader.IsDBNull(i))
                {
                    Reason = reader.GetString(i);
                }
                else
                {
                    Reason = string.Empty;
                }
                i++;
                // UpdateIndex column, int, not null
                UpdateIndex = reader.GetInt32(i++);


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

                //Vendor Name                
                if (false == reader.IsDBNull(i))
                {
                    VendorName = reader.GetString(i);
                }
                else
                {
                    VendorName = string.Empty;
                }
                i++;

                //VendorPhoneNumber              
                if (false == reader.IsDBNull(i))
                {
                    VendorPhoneNumber = reader.GetString(i);
                }
                else
                {
                    VendorPhoneNumber = string.Empty;
                }
                i++;
                //VendorClientLookupId               
                if (false == reader.IsDBNull(i))
                {
                    VendorClientLookupId = reader.GetString(i);
                }
                else
                {
                    VendorClientLookupId = string.Empty;
                }
                i++;
                //Creator_PersonnelName              
                if (false == reader.IsDBNull(i))
                {
                    Creator_PersonnelName = reader.GetString(i);
                }
                else
                {
                    Creator_PersonnelName = string.Empty;
                }
                i++;

                //Completed_PersonnelName               
                if (false == reader.IsDBNull(i))
                {
                    Completed_PersonnelName = reader.GetString(i);
                }
                else
                {
                    Completed_PersonnelName = string.Empty;
                }
                i++;

                //Buyer_PersonnelName               
                if (false == reader.IsDBNull(i))
                {
                    Buyer_PersonnelName = reader.GetString(i);
                }
                else
                {
                    Buyer_PersonnelName = string.Empty;
                }
                i++;

                //Total Cost
                TotalCost = reader.GetDecimal(i++);

                //Total Count
                TotalCount = reader.GetInt32(i);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["PurchaseOrderId"].ToString(); }
                catch { missing.Append("PurchaseOrderId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["DepartmentId"].ToString(); }
                catch { missing.Append("DepartmentId "); }

                try { reader["AreaId"].ToString(); }
                catch { missing.Append("AreaId "); }

                try { reader["StoreroomId"].ToString(); }
                catch { missing.Append("StoreroomId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Attention"].ToString(); }
                catch { missing.Append("Attention "); }

                try { reader["Buyer_PersonnelId"].ToString(); }
                catch { missing.Append("Buyer_PersonnelId "); }

                try { reader["Carrier"].ToString(); }
                catch { missing.Append("Carrier "); }

                try { reader["CompleteBy_PersonnelId"].ToString(); }
                catch { missing.Append("CompleteBy_PersonnelId "); }

                try { reader["CompleteDate"].ToString(); }
                catch { missing.Append("CompleteDate "); }

                try { reader["Creator_PersonnelId"].ToString(); }
                catch { missing.Append("Creator_PersonnelId "); }

                try { reader["FOB"].ToString(); }
                catch { missing.Append("FOB "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["Terms"].ToString(); }
                catch { missing.Append("Terms "); }

                try { reader["VendorId"].ToString(); }
                catch { missing.Append("VendorId "); }

                try { reader["VoidBy_PersonnelId"].ToString(); }
                catch { missing.Append("VoidBy_PersonnelId "); }

                try { reader["VoidDate"].ToString(); }
                catch { missing.Append("VoidDate "); }

                try { reader["VoidReason"].ToString(); }
                catch { missing.Append("VoidReason "); }

                try { reader["UpdateIndex"].ToString(); }
                catch { missing.Append("UpdateIndex "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["VendorName"].ToString(); }
                catch { missing.Append("VendorName "); }

                try { reader["VendorPhoneNumber"].ToString(); }
                catch { missing.Append("VendorPhoneNumber "); }

                try { reader["VendorClientLookupId"].ToString(); }
                catch { missing.Append("VendorClientLookupId "); }

                try { reader["Creator_PersonnelName"].ToString(); }
                catch { missing.Append("Creator_PersonnelName "); }

                try { reader["Completed_PersonnelName"].ToString(); }
                catch { missing.Append("Completed_PersonnelName "); }

                try { reader["Buyer_PersonnelName"].ToString(); }
                catch { missing.Append("Buyer_PersonnelName "); }

                try { reader["TotalCost"].ToString(); }
                catch { missing.Append("TotalCost "); }

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

        //--------------SOM-684---------------------------------------------------------------
        public void UpdateByForeignKeysForceComplete(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName)
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                Database.StoredProcedure.usp_PurchaseOrder_ForceComplete_UpdateByFK.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        //--SOM-822--//
        public static object ProcessRowForLookUp(SqlDataReader reader)
        {
            // Create instance of object
            b_PurchaseOrder obj = new b_PurchaseOrder();

            // Load the object from the database
            obj.LoadFromDatabaseForLookup(reader);

            // Return result
            return (object)obj;
        }

        /// <summary>
        /// Load the current row in the input SqlDataReader into a b_PurchaseOrder object.
        /// This routine should be applied to the usp_PurchaseOrder_RetrieveByPK stored procedure.
        /// This routine should be applied to the usp_PurchaseOrder_RetrieveAll. stored procedure.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        public int LoadFromDatabaseForLookup(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // PurchaseOrderId column, bigint, not null
                PurchaseOrderId = reader.GetInt64(i++);

                // SiteId column, bigint, not null
                SiteId = reader.GetInt64(i++);


                // AreaId column, bigint, not null
                AreaId = reader.GetInt64(i++);

                // DepartmentId column, bigint, not null
                DepartmentId = reader.GetInt64(i++);


                // StoreroomId column, bigint, not null
                StoreroomId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(15), not null
                ClientLookupId = reader.GetString(i++);

                // UpdateIndex column, int, not null
                UpdateIndex = reader.GetInt32(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["PurchaseOrderId"].ToString(); }
                catch { missing.Append("PurchaseOrderId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["AreaId"].ToString(); }
                catch { missing.Append("AreaId "); }

                try { reader["DepartmentId"].ToString(); }
                catch { missing.Append("DepartmentId "); }

                try { reader["StoreroomId"].ToString(); }
                catch { missing.Append("StoreroomId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["UpdateIndex"].ToString(); }
                catch { missing.Append("UpdateIndex "); }


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

        /// <summary>
        /// Retrieve all PurchaseOrder table records represented by this object in the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="data">b_PurchaseOrder[] that contains the results</param>
        public void RetrieveAll_V2(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName,
            ref b_PurchaseOrder[] data
        )
        {
            Database.SqlClient.ProcessRow<b_PurchaseOrder> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_PurchaseOrder[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_PurchaseOrder>(reader => { b_PurchaseOrder obj = new b_PurchaseOrder(); obj.LoadFromDatabase(reader); return obj; });
                results = Database.StoredProcedure.usp_PurchaseOrder_RetrieveAll_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, ClientId, SiteId);

                // Extract the results
                if (null != results)
                {
                    data = (b_PurchaseOrder[])results.ToArray(typeof(b_PurchaseOrder));
                }
                else
                {
                    data = new b_PurchaseOrder[0];
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

        public void RetrieveChunkSearch(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref List<b_PurchaseOrder> results
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

                results = Database.StoredProcedure.usp_PurchaseOrder_RetrieveChunkSearch_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        #region  V2-738
        public void RetrieveByForeignKeys_V2FromDatabase(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
           string callerUserName
       )
        {
            Database.SqlClient.ProcessRow<b_PurchaseOrder> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_PurchaseOrder>(reader => { this.LoadFromDatabaseByPKForeignKey_V2(reader); return this; });
                Database.StoredProcedure.usp_PurchaseOrder_RetrieveByPKForeignKeys_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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
        public void LoadFromDatabaseByPKForeignKey_V2(SqlDataReader reader)
        {
            int i = this.LoadFromDatabase(reader);

            try
            {
                if (false == reader.IsDBNull(i))
                {
                    CreateDate = reader.GetDateTime(i);
                }
                else
                {
                    CreateDate = DateTime.MinValue;
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
                    VendorPhoneNumber = reader.GetString(i);
                }
                else
                {
                    VendorPhoneNumber = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    VendorEmailAddress = reader.GetString(i);
                }
                else
                {
                    VendorEmailAddress = "";
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
                    Creator_PersonnelName = reader.GetString(i);
                }
                else
                {
                    Creator_PersonnelName = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Completed_PersonnelName = reader.GetString(i);
                }
                else
                {
                    Completed_PersonnelName = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    CountLineItem = reader.GetInt32(i);
                }
                else
                {
                    CountLineItem = 0;
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
                    VendorAddress1 = reader.GetString(i);
                }
                else
                {
                    VendorAddress1 = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    VendorAddress2 = reader.GetString(i);
                }
                else
                {
                    VendorAddress2 = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    VendorAddress3 = reader.GetString(i);
                }
                else
                {
                    VendorAddress3 = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    VendorAddressCity = reader.GetString(i);
                }
                else
                {
                    VendorAddressCity = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    VendorAddressCountry = reader.GetString(i);
                }
                else
                {
                    VendorAddressCountry = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    VendorAddressPostCode = reader.GetString(i);
                }
                else
                {
                    VendorAddressPostCode = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    VendorAddressState = reader.GetString(i);
                }
                else
                {
                    VendorAddressState = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    SiteName = reader.GetString(i);
                }
                else
                {
                    SiteName = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    SiteAddress1 = reader.GetString(i);
                }
                else
                {
                    SiteAddress1 = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    SiteAddress2 = reader.GetString(i);
                }
                else
                {
                    SiteAddress2 = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    SiteAddress3 = reader.GetString(i);
                }
                else
                {
                    SiteAddress3 = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    SiteAddressCity = reader.GetString(i);
                }
                else
                {
                    SiteAddressCity = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    SiteAddressCountry = reader.GetString(i);
                }
                else
                {
                    SiteAddressCountry = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    SiteAddressPostCode = reader.GetString(i);
                }
                else
                {
                    SiteAddressPostCode = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    SiteAddressState = reader.GetString(i);
                }
                else
                {
                    SiteAddressState = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    SiteBillToName = reader.GetString(i);
                }
                else
                {
                    SiteBillToName = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    SiteBillToAddress1 = reader.GetString(i);
                }
                else
                {
                    SiteBillToAddress1 = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    SiteBillToAddress2 = reader.GetString(i);
                }
                else
                {
                    SiteBillToAddress2 = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    SiteBillToAddress3 = reader.GetString(i);
                }
                else
                {
                    SiteBillToAddress3 = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    SiteBillToAddressCity = reader.GetString(i);
                }
                else
                {
                    SiteBillToAddressCity = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    SiteBillToAddressCountry = reader.GetString(i);
                }
                else
                {
                    SiteBillToAddressCountry = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    SiteBillToAddressPostCode = reader.GetString(i);
                }
                else
                {
                    SiteBillToAddressPostCode = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    SiteBillToAddressState = reader.GetString(i);
                }
                else
                {
                    SiteBillToAddressState = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    SiteBillToComment = reader.GetString(i);
                }
                else
                {
                    SiteBillToComment = "";
                }
                i++;

                //- SOM: 936
                if (false == reader.IsDBNull(i))
                {
                    ModifyBy = reader.GetString(i);
                }
                else
                {
                    ModifyBy = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    ModifyDate = reader.GetDateTime(i);
                }
                else
                {
                    ModifyDate = DateTime.Now;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    CreateBy = reader.GetString(i);
                }
                else
                {
                    CreateBy = "";
                }
                i++;
                //SOM-1276
                if (false == reader.IsDBNull(i))
                {
                    VendorCustomerAccount = reader.GetString(i);
                }
                else
                {
                    VendorCustomerAccount = "";
                }
                i++;
                //SOM-1413
                if (false == reader.IsDBNull(i))
                {
                    Buyer_PersonnelName = reader.GetString(i);
                }
                else
                {
                    Buyer_PersonnelName = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    PersonnelEmail = reader.GetString(i);
                }
                else
                {
                    PersonnelEmail = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    StoreroomName = reader.GetString(i);
                }
                else
                {
                    StoreroomName = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Shipto_ClientLookUpId = reader.GetString(i);
                }
                else
                {
                    Shipto_ClientLookUpId = "";
                }
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate"); }

                try { reader["VendorName"].ToString(); }
                catch { missing.Append("VendorName "); }

                try { reader["VendorPhoneNumber"].ToString(); }
                catch { missing.Append("VendorPhoneNumber "); }

                try { reader["VendorEmailAddress"].ToString(); }
                catch { missing.Append("VendorEmailAddress "); }

                try { reader["VendorClientLookupId"].ToString(); }
                catch { missing.Append("VendorClientLookupId "); }

                try { reader["Creator_PersonnelName"].ToString(); }
                catch { missing.Append("Creator_PersonnelName "); }

                try { reader["Completed_PersonnelName"].ToString(); }
                catch { missing.Append("Completed_PersonnelName "); }

                try { reader["CountLineItem"].ToString(); }
                catch { missing.Append("CountLineItem "); }

                try { reader["TotalCost"].ToString(); }
                catch { missing.Append("TotalCost "); }

                try { reader["VendorAddress1"].ToString(); }
                catch { missing.Append("VendorAddress1 "); }

                try { reader["VendorAddress2"].ToString(); }
                catch { missing.Append("VendorAddress2 "); }

                try { reader["VendorAddress3"].ToString(); }
                catch { missing.Append("VendorAddress3 "); }

                try { reader["VendorAddressCity"].ToString(); }
                catch { missing.Append("VendorAddressCity "); }

                try { reader["VendorAddressCountry"].ToString(); }
                catch { missing.Append("VendorAddressCountry "); }

                try { reader["VendorAddressPostCode"].ToString(); }
                catch { missing.Append("VendorAddressPostCode "); }

                try { reader["VendorAddressState"].ToString(); }
                catch { missing.Append("VendorAddressState "); }

                try { reader["SiteName"].ToString(); }
                catch { missing.Append("SiteName "); }

                try { reader["SiteAddress1"].ToString(); }
                catch { missing.Append("SiteAddress1 "); }

                try { reader["SiteAddress2"].ToString(); }
                catch { missing.Append("SiteAddress2 "); }

                try { reader["SiteAddress3"].ToString(); }
                catch { missing.Append("SiteAddress3 "); }

                try { reader["SiteAddressCity"].ToString(); }
                catch { missing.Append("SiteAddressCity "); }

                try { reader["SiteAddressCountry"].ToString(); }
                catch { missing.Append("SiteAddressCountry "); }

                try { reader["SiteAddressPostCode"].ToString(); }
                catch { missing.Append("SiteAddressPostCode "); }

                try { reader["SiteAddressState"].ToString(); }
                catch { missing.Append("SiteAddressState "); }

                try { reader["SiteBillToName"].ToString(); }
                catch { missing.Append("SiteBillToName "); }

                try { reader["SiteBillToAddress1"].ToString(); }
                catch { missing.Append("SiteBillToAddress1 "); }

                try { reader["SiteBillToAddress2"].ToString(); }
                catch { missing.Append("SiteBillToAddress2 "); }

                try { reader["SiteBillToAddress3"].ToString(); }
                catch { missing.Append("SiteBillToAddress3 "); }

                try { reader["SiteBillToAddressCity"].ToString(); }
                catch { missing.Append("SiteBillToAddressCity "); }

                try { reader["SiteBillToAddressCountry"].ToString(); }
                catch { missing.Append("SiteBillToAddressCountry "); }

                try { reader["SiteBillToAddressPostCode"].ToString(); }
                catch { missing.Append("SiteBillToAddressPostCode "); }

                try { reader["SiteBillToAddressState"].ToString(); }
                catch { missing.Append("SiteBillToAddressState "); }

                try { reader["SiteBillToComment"].ToString(); }
                catch { missing.Append("SiteBillToComment "); }

                try { reader["ModifyBy"].ToString(); }
                catch { missing.Append("ModifyBy "); }

                try { reader["[ModifyDate"].ToString(); }
                catch { missing.Append("[ModifyDate "); }

                try { reader["[CreateBy"].ToString(); }
                catch { missing.Append("[CreateBy "); }

                try { reader["VendorCustomerAccount"].ToString(); }
                catch { missing.Append("VendorCustomerAccount "); }

                try { reader["Buyer_PersonnelName"].ToString(); }
                catch { missing.Append("Buyer_PersonnelName "); }

                try { reader["PersonnelEmail"].ToString(); }
                catch { missing.Append("PersonnelEmail "); }

                try { reader["StoreroomName"].ToString(); }
                catch { missing.Append("StoreroomName "); }

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
        #region V2-946

        public void RetrieveAllByPurchaseOrdeV2Print(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref b_PurchaseOrder results
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

                results = Database.StoredProcedure.usp_POPrint_RetrieveAllByPurchaseOrder_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public static b_PurchaseOrder ProcessRowForPurchaseOrderPrint(SqlDataReader reader)
        {
            // Create instance of object
            b_PurchaseOrder obj = new b_PurchaseOrder();

            // Load the object from the database
            obj.LoadFromDatabaseForPurchaseOrderPrint(reader);

            // Return result
            return obj;
        }
        public void LoadFromDatabaseForPurchaseOrderPrint(SqlDataReader reader)
        {
            //   int i = this.LoadFromDatabase(reader);
            int i = 0;
            try
            {

                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // PurchaseOrderId  column, bigint, not null
                PurchaseOrderId = reader.GetInt64(i++);

                // SiteId  column, bigint, not null
                SiteId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);

                // Reason column, nvarchar(256), not null
                if (false == reader.IsDBNull(i))
                {
                    Reason = reader.GetString(i++);
                }
                else
                {
                    Reason = "";
                }
                // Reason CreateDate, Datetime2(7),null
                if (false == reader.IsDBNull(i))
                {
                    CreateDate = reader.GetDateTime(i);
                }
                else
                {
                    CreateDate = DateTime.MinValue;
                }
                i++;
                // Required column, Datetime2(7),null
                if (false == reader.IsDBNull(i))
                {
                    Required = reader.GetDateTime(i);
                }
                else
                {
                    Required = DateTime.MinValue;
                }
                i++;
                // Carrier column, nvarchar(15), not null
                if (false == reader.IsDBNull(i))
                {
                    Carrier = reader.GetString(i);
                }
                else
                {
                    Carrier = "";
                }
                i++;
                // Attention column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    Attention = reader.GetString(i);
                }
                else
                {
                    Attention = "";
                }
                i++;
                // MessagetoVendor column, nvarchar(255), not null
                if (false == reader.IsDBNull(i))
                {
                    MessageToVendor = reader.GetString(i);
                }
                else
                {
                    MessageToVendor = "";
                }
                i++;
                // Terms column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    Terms = reader.GetString(i);
                }
                else
                {
                    Terms = "";
                }
                i++;
                // FOB column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    FOB = reader.GetString(i);
                }
                else
                {
                    FOB = "";
                }
                i++;
                // VendorCustomerAccount column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    VendorCustomerAccount = reader.GetString(i);
                }
                else
                {
                    VendorCustomerAccount = "";
                }
                i++;
                // Buyer_PersonnelName column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    Buyer_PersonnelName = reader.GetString(i);
                }
                else
                {
                    Buyer_PersonnelName = "";
                }
                i++;
                // VendorName column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    VendorName = reader.GetString(i);
                }
                else
                {
                    VendorName = "";
                }
                i++;
                // VendorEmailAddress column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    VendorEmailAddress = reader.GetString(i);
                }
                else
                {
                    VendorEmailAddress = "";
                }
                i++;
                // VendorAddress1 column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    VendorAddress1 = reader.GetString(i);
                }
                else
                {
                    VendorAddress1 = "";
                }
                i++;
                // VendorAddress2 column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    VendorAddress2 = reader.GetString(i);
                }
                else
                {
                    VendorAddress2 = "";
                }
                i++;
                // VendorAddress3 column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    VendorAddress3 = reader.GetString(i);
                }
                else
                {
                    VendorAddress3 = "";
                }
                i++;
                // VendorAddressCity column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    VendorAddressCity = reader.GetString(i);
                }
                else
                {
                    VendorAddressCity = "";
                }
                i++;
                // VendorAddressCountry column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    VendorAddressCountry = reader.GetString(i);
                }
                else
                {
                    VendorAddressCountry = "";
                }
                i++;
                // VendorAddressPostCode column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    VendorAddressPostCode = reader.GetString(i);
                }
                else
                {
                    VendorAddressPostCode = "";
                }
                i++;
                // VendorAddressState column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    VendorAddressState = reader.GetString(i);
                }
                else
                {
                    VendorAddressState = "";
                }
                i++;
                // VendorPhoneNumber column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    VendorPhoneNumber = reader.GetString(i);
                }
                else
                {
                    VendorPhoneNumber = "";
                }
                i++;

                // SiteName column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    SiteName = reader.GetString(i);
                }
                else
                {
                    SiteName = "";
                }
                i++;
                // SiteAddress1 column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    SiteAddress1 = reader.GetString(i);
                }
                else
                {
                    SiteAddress1 = "";
                }
                i++;
                // SiteAddress2 column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    SiteAddress2 = reader.GetString(i);
                }
                else
                {
                    SiteAddress2 = "";
                }
                i++;
                // SiteAddress3 column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    SiteAddress3 = reader.GetString(i);
                }
                else
                {
                    SiteAddress3 = "";
                }
                i++;
                // SiteAddressCity column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    SiteAddressCity = reader.GetString(i);
                }
                else
                {
                    SiteAddressCity = "";
                }
                i++;
                // SiteAddressCountry column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    SiteAddressCountry = reader.GetString(i);
                }
                else
                {
                    SiteAddressCountry = "";
                }
                i++;
                // SiteAddressPostCode column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    SiteAddressPostCode = reader.GetString(i);
                }
                else
                {
                    SiteAddressPostCode = "";
                }
                i++;
                // SiteAddressState column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    SiteAddressState = reader.GetString(i);
                }
                else
                {
                    SiteAddressState = "";
                }
                i++;
                // SiteBillToName column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    SiteBillToName = reader.GetString(i);
                }
                else
                {
                    SiteBillToName = "";
                }
                i++;
                // SiteBillToAddress1 column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    SiteBillToAddress1 = reader.GetString(i);
                }
                else
                {
                    SiteBillToAddress1 = "";
                }
                i++;
                // SiteBillToAddress2 column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    SiteBillToAddress2 = reader.GetString(i);
                }
                else
                {
                    SiteBillToAddress2 = "";
                }
                i++;
                // SiteBillToAddress3 column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    SiteBillToAddress3 = reader.GetString(i);
                }
                else
                {
                    SiteBillToAddress3 = "";
                }
                i++;
                // SiteBillToAddressCity column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    SiteBillToAddressCity = reader.GetString(i);
                }
                else
                {
                    SiteBillToAddressCity = "";
                }
                i++;
                // SiteBillToAddressCountry column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    SiteBillToAddressCountry = reader.GetString(i);
                }
                else
                {
                    SiteBillToAddressCountry = "";
                }
                i++;
                // SiteBillToAddressPostCode column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    SiteBillToAddressPostCode = reader.GetString(i);
                }
                else
                {
                    SiteBillToAddressPostCode = "";
                }
                i++;
                // SiteBillToAddressState column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    SiteBillToAddressState = reader.GetString(i);
                }
                else
                {
                    SiteBillToAddressState = "";
                }
                i++;
                // SiteBillToComment column, nvarchar(255), not null
                if (false == reader.IsDBNull(i))
                {
                    SiteBillToComment = reader.GetString(i);
                }
                else
                {
                    SiteBillToComment = "";
                }
                i++;
                // Creator_PersonnelName column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    Creator_PersonnelName = reader.GetString(i);
                }
                else
                {
                    Creator_PersonnelName = "";
                }

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["PurchaseOrderId"].ToString(); }
                catch { missing.Append("PurchaseOrderId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Reason"].ToString(); }
                catch { missing.Append("Reason "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["Required"].ToString(); }
                catch { missing.Append("Required "); }

                try { reader["Carrier"].ToString(); }
                catch { missing.Append("Carrier "); }

                try { reader["Attention"].ToString(); }
                catch { missing.Append("Attention "); }

                try { reader["MessageToVendor"].ToString(); }
                catch { missing.Append("MessageToVendor "); }

                try { reader["Terms"].ToString(); }
                catch { missing.Append("Terms "); }

                try { reader["FOB"].ToString(); }
                catch { missing.Append("FOB "); }

                try { reader["VendorCustomerAccount"].ToString(); }
                catch { missing.Append("VendorCustomerAccount "); }

                try { reader["Buyer_PersonnelName"].ToString(); }
                catch { missing.Append("Buyer_PersonnelName "); }

                try { reader["VendorName"].ToString(); }
                catch { missing.Append("VendorName "); }

                try { reader["VendorEmailAddress"].ToString(); }
                catch { missing.Append("VendorEmailAddress "); }

                try { reader["VendorAddress1"].ToString(); }
                catch { missing.Append("VendorAddress1 "); }

                try { reader["VendorAddress2"].ToString(); }
                catch { missing.Append("VendorAddress2 "); }

                try { reader["VendorAddress3"].ToString(); }
                catch { missing.Append("VendorAddress3 "); }

                try { reader["VendorAddressCity"].ToString(); }
                catch { missing.Append("VendorAddressCity "); }

                try { reader["VendorAddressCountry"].ToString(); }
                catch { missing.Append("VendorAddressCountry "); }

                try { reader["VendorAddressPostCode"].ToString(); }
                catch { missing.Append("VendorAddressPostCode "); }

                try { reader["VendorAddressState"].ToString(); }
                catch { missing.Append("VendorAddressState "); }

                try { reader["VendorPhoneNumber"].ToString(); }
                catch { missing.Append("VendorPhoneNumber "); }

                try { reader["SiteName"].ToString(); }
                catch { missing.Append("SiteName "); }

                try { reader["SiteAddress1"].ToString(); }
                catch { missing.Append("SiteAddress1 "); }

                try { reader["SiteAddress2"].ToString(); }
                catch { missing.Append("SiteAddress2 "); }

                try { reader["SiteAddress3"].ToString(); }
                catch { missing.Append("SiteAddress3 "); }

                try { reader["SiteAddressCity"].ToString(); }
                catch { missing.Append("SiteAddressCity "); }

                try { reader["SiteAddressCountry"].ToString(); }
                catch { missing.Append("SiteAddressCountry "); }

                try { reader["SiteAddressPostCode"].ToString(); }
                catch { missing.Append("SiteAddressPostCode "); }

                try { reader["SiteAddressState"].ToString(); }
                catch { missing.Append("SiteAddressState "); }

                try { reader["SiteBillToName"].ToString(); }
                catch { missing.Append("SiteBillToName "); }

                try { reader["SiteBillToAddress1"].ToString(); }
                catch { missing.Append("SiteBillToAddress1 "); }

                try { reader["SiteBillToAddress2"].ToString(); }
                catch { missing.Append("SiteBillToAddress2 "); }

                try { reader["SiteBillToAddress3"].ToString(); }
                catch { missing.Append("SiteBillToAddress3 "); }

                try { reader["SiteBillToAddressCity"].ToString(); }
                catch { missing.Append("SiteBillToAddressCity "); }

                try { reader["SiteBillToAddressCountry"].ToString(); }
                catch { missing.Append("SiteBillToAddressCountry "); }

                try { reader["SiteBillToAddressPostCode"].ToString(); }
                catch { missing.Append("SiteBillToAddressPostCode "); }

                try { reader["SiteBillToAddressState"].ToString(); }
                catch { missing.Append("SiteBillToAddressState "); }

                try { reader["SiteBillComment"].ToString(); }
                catch { missing.Append("SiteBillComment "); }

                try { reader["Creator_PersonnelName"].ToString(); }
                catch { missing.Append("Creator_PersonnelName "); }

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
        #region V2-947
        public static b_PurchaseOrder ProcessRowForPurchaseOrderPrintforPOR(SqlDataReader reader)
        {
            // Create instance of object
            b_PurchaseOrder obj = new b_PurchaseOrder();

            // Load the object from the database
            obj.ProcessRowForPurchaseOrderAllPrintforPOR(reader);

            // Return result
            return obj;
        }
        public void ProcessRowForPurchaseOrderAllPrintforPOR(SqlDataReader reader)
        {
            //   int i = this.LoadFromDatabase(reader);
            int i = 0;
            try
            {

                // VendorName column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    VendorName = reader.GetString(i);
                }
                else
                {
                    VendorName = "";
                }
                i++;

                // VendorAddress1 column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    VendorAddress1 = reader.GetString(i);
                }
                else
                {
                    VendorAddress1 = "";
                }
                i++;
                // VendorAddress2 column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    VendorAddress2 = reader.GetString(i);
                }
                else
                {
                    VendorAddress2 = "";
                }
                i++;
                // VendorAddress3 column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    VendorAddress3 = reader.GetString(i);
                }
                else
                {
                    VendorAddress3 = "";
                }
                i++;
                // VendorAddressCity column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    VendorAddressCity = reader.GetString(i);
                }
                else
                {
                    VendorAddressCity = "";
                }
                i++;
                // VendorAddressCountry column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    VendorAddressState = reader.GetString(i);
                }
                else
                {
                    VendorAddressState = "";
                }
                i++;
                // VendorAddressPostCode column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    VendorAddressPostCode = reader.GetString(i);
                }
                else
                {
                    VendorAddressPostCode = "";
                }
                i++;
                // VendorAddressState column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    VendorAddressCountry = reader.GetString(i);
                }
                else
                {
                    VendorAddressCountry = "";
                }

                i++;
                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);
                ReceiptNumber = reader.GetInt64(i++);
                // ReceiveDate column, Datetime2(7),null
                if (false == reader.IsDBNull(i))
                {
                    ReceiveDate = reader.GetDateTime(i);
                }
                else
                {
                    ReceiveDate = DateTime.MinValue;
                }
                i++;

                // ReceivedPersonnelName column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    ReceivedPersonnelName = reader.GetString(i);
                }
                else
                {
                    ReceivedPersonnelName = "";
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
                // SiteName column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    SiteName = reader.GetString(i);
                }
                else
                {
                    SiteName = "";
                }
                i++;
                // SiteAddress1 column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    SiteAddress1 = reader.GetString(i);
                }
                else
                {
                    SiteAddress1 = "";
                }
                i++;
                // SiteAddress2 column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    SiteAddress2 = reader.GetString(i);
                }
                else
                {
                    SiteAddress2 = "";
                }
                i++;
                // SiteAddress3 column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    SiteAddress3 = reader.GetString(i);
                }
                else
                {
                    SiteAddress3 = "";
                }
                i++;
                // SiteAddressCity column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    SiteAddressCity = reader.GetString(i);
                }
                else
                {
                    SiteAddressCity = "";
                }
                i++;
                // SiteAddressCountry column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    SiteAddressCountry = reader.GetString(i);
                }
                else
                {
                    SiteAddressCountry = "";
                }
                i++;
                // SiteAddressPostCode column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    SiteAddressPostCode = reader.GetString(i);
                }
                else
                {
                    SiteAddressPostCode = "";
                }
                i++;
                // SiteAddressState column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    SiteAddressState = reader.GetString(i);
                }
                else
                {
                    SiteAddressState = "";
                }
                i++;
                // SiteBillToName column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    SiteBillToName = reader.GetString(i);
                }
                else
                {
                    SiteBillToName = "";
                }
                i++;
                // SiteBillToAddress1 column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    SiteBillToAddress1 = reader.GetString(i);
                }
                else
                {
                    SiteBillToAddress1 = "";
                }
                i++;
                // SiteBillToAddress2 column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    SiteBillToAddress2 = reader.GetString(i);
                }
                else
                {
                    SiteBillToAddress2 = "";
                }
                i++;
                // SiteBillToAddress3 column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    SiteBillToAddress3 = reader.GetString(i);
                }
                else
                {
                    SiteBillToAddress3 = "";
                }
                i++;
                // SiteBillToAddressCity column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    SiteBillToAddressCity = reader.GetString(i);
                }
                else
                {
                    SiteBillToAddressCity = "";
                }
                i++;
                // SiteBillToAddressCountry column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    SiteBillToAddressCountry = reader.GetString(i);
                }
                else
                {
                    SiteBillToAddressCountry = "";
                }
                i++;
                // SiteBillToAddressPostCode column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    SiteBillToAddressPostCode = reader.GetString(i);
                }
                else
                {
                    SiteBillToAddressPostCode = "";
                }
                i++;
                // SiteBillToAddressState column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    SiteBillToAddressState = reader.GetString(i);
                }
                else
                {
                    SiteBillToAddressState = "";
                }

                i++;
                // Carrier column, nvarchar(15), not null
                if (false == reader.IsDBNull(i))
                {
                    Carrier = reader.GetString(i);
                }
                else
                {
                    Carrier = "";
                }
                i++;
                // Attention column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    Attention = reader.GetString(i);
                }
                else
                {
                    Attention = "";
                }
                i++;
                // MessagetoVendor column, nvarchar(255), not null
                if (false == reader.IsDBNull(i))
                {
                    MessageToVendor = reader.GetString(i);
                }
                else
                {
                    MessageToVendor = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    PackingSlip = reader.GetString(i);
                }
                else
                {
                    PackingSlip = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    FreightBill = reader.GetString(i);
                }
                else
                {
                    FreightBill = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    FreightAmount = reader.GetDecimal(i++);
                }
                else
                {
                    FreightAmount = 0; i++;
                }

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["VendorName"].ToString(); }
                catch { missing.Append("VendorName "); }


                try { reader["VendorAddress1"].ToString(); }
                catch { missing.Append("VendorAddress1 "); }

                try { reader["VendorAddress2"].ToString(); }
                catch { missing.Append("VendorAddress2 "); }

                try { reader["VendorAddress3"].ToString(); }
                catch { missing.Append("VendorAddress3 "); }

                try { reader["VendorAddressCity"].ToString(); }
                catch { missing.Append("VendorAddressCity "); }

                try { reader["VendorAddressState"].ToString(); }
                catch { missing.Append("VendorAddressState "); }

                try { reader["VendorAddressCountry"].ToString(); }
                catch { missing.Append("VendorAddressCountry "); }

                try { reader["VendorAddressPostCode"].ToString(); }
                catch { missing.Append("VendorAddressPostCode "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["ReceiptNumber"].ToString(); }
                catch { missing.Append("ReceiptNumber "); }

                try { reader["ReceiveDate"].ToString(); }
                catch { missing.Append("ReceiveDate "); }

                try { reader["ReceivedPersonnelName"].ToString(); }
                catch { missing.Append("ReceivedPersonnelName "); }


                try { reader["SiteName"].ToString(); }
                catch { missing.Append("SiteName "); }

                try { reader["SiteAddress1"].ToString(); }
                catch { missing.Append("SiteAddress1 "); }

                try { reader["SiteAddress2"].ToString(); }
                catch { missing.Append("SiteAddress2 "); }

                try { reader["SiteAddress3"].ToString(); }
                catch { missing.Append("SiteAddress3 "); }

                try { reader["SiteAddressCity"].ToString(); }
                catch { missing.Append("SiteAddressCity "); }

                try { reader["SiteAddressCountry"].ToString(); }
                catch { missing.Append("SiteAddressCountry "); }

                try { reader["SiteAddressPostCode"].ToString(); }
                catch { missing.Append("SiteAddressPostCode "); }

                try { reader["SiteAddressState"].ToString(); }
                catch { missing.Append("SiteAddressState "); }

                try { reader["SiteBillToName"].ToString(); }
                catch { missing.Append("SiteBillToName "); }

                try { reader["SiteBillToAddress1"].ToString(); }
                catch { missing.Append("SiteBillToAddress1 "); }

                try { reader["SiteBillToAddress2"].ToString(); }
                catch { missing.Append("SiteBillToAddress2 "); }

                try { reader["SiteBillToAddress3"].ToString(); }
                catch { missing.Append("SiteBillToAddress3 "); }

                try { reader["SiteBillToAddressCity"].ToString(); }
                catch { missing.Append("SiteBillToAddressCity "); }

                try { reader["SiteBillToAddressCountry"].ToString(); }
                catch { missing.Append("SiteBillToAddressCountry "); }

                try { reader["SiteBillToAddressPostCode"].ToString(); }
                catch { missing.Append("SiteBillToAddressPostCode "); }

                try { reader["SiteBillToAddressState"].ToString(); }
                catch { missing.Append("SiteBillToAddressState "); }


                try { reader["Carrier"].ToString(); }
                catch { missing.Append("Carrier "); }

                try { reader["Attention"].ToString(); }
                catch { missing.Append("Attention "); }

                try { reader["MessageToVendor"].ToString(); }
                catch { missing.Append("MessageToVendor "); }

                try { reader["PackingSlip"].ToString(); }
                catch { missing.Append("PackingSlip "); }

                try { reader["FreightBill"].ToString(); }
                catch { missing.Append("FreightBill "); }

                try { reader["FreightAmount"].ToString(); }
                catch { missing.Append("FreightAmount "); }

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


        #region V2-981
        public void RetrievePurchaseOrderLookuplistChunkSearchV2(SqlConnection connection, SqlTransaction transaction,
                        long callerUserInfoId, string callerUserName, ref List<b_PurchaseOrder> results)
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

                results = Database.StoredProcedure.usp_PurchaseOrder_RetrieveChunkSearchLookupList_V2
                    .CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public static b_PurchaseOrder ProcessRowForChunkSearchLookupList(SqlDataReader reader)
        {
            b_PurchaseOrder purchaseOrder = new b_PurchaseOrder();

            purchaseOrder.LoadFromDatabaseForLookupListChunkSearchV2(reader);
            return purchaseOrder;
        }
        public int LoadFromDatabaseForLookupListChunkSearchV2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // Client Id
                ClientId = reader.GetInt64(i++);

                // VendorId column, bigint, not null
                PurchaseOrderId = reader.GetInt64(i++);

                //ClientLookupId
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = "";
                }
                i++;

                //Status
                if (false == reader.IsDBNull(i))
                {
                    Status = reader.GetString(i);
                }
                else
                {
                    Status = "";
                }
                i++;

                //VendorClientLookupId
                if (false == reader.IsDBNull(i))
                {
                    VendorClientLookupId = reader.GetString(i);
                }
                else
                {
                    VendorClientLookupId = "";
                }
                i++;
                //VendorName
                if (false == reader.IsDBNull(i))
                {
                    VendorName = reader.GetString(i);
                }
                else
                {
                    VendorName = "";
                }
                i++;

                //TotalCount
                if (false == reader.IsDBNull(i))
                {
                    TotalCount = reader.GetInt32(i);
                }
                else
                {
                    TotalCount = 0;
                }
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["PurchaseOrderId"].ToString(); }
                catch { missing.Append("PurchaseOrderId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

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

            return i;
        }

        #endregion

        #region V2-1073
        public void RetrievePOByAccountReport(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref List<b_PurchaseOrder> results
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

                results = Database.StoredProcedure.usp_PurchaseOrders_by_Account.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public static b_PurchaseOrder ProcessRowForPOByAccountReport(SqlDataReader reader)
        {
            // Create instance of object
            b_PurchaseOrder PurchaseOrder = new b_PurchaseOrder();
            PurchaseOrder.LoadFromDatabaseForPOByAccountReport(reader);
            return PurchaseOrder;
        }
        public int LoadFromDatabaseForPOByAccountReport(SqlDataReader reader)
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
                if (false == reader.IsDBNull(i))
                {
                    SiteId = reader.GetInt64(i);
                }
                else
                {
                    SiteId = 0;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    AreaId = reader.GetInt64(i);
                }
                else
                {
                    AreaId = 0;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    DepartmentId = reader.GetInt64(i);
                }
                else
                {
                    DepartmentId = 0;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    StoreroomId = reader.GetInt64(i);
                }
                else
                {
                    StoreroomId = 0;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    AccountClientLookupId = reader.GetString(i);
                }
                else
                {
                    AccountClientLookupId = string.Empty;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    AccountName = reader.GetString(i);
                }
                else
                {
                    AccountName = string.Empty;
                }
                i++;
                // ClientLookupId column, nvarchar(15), not null
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = string.Empty;
                }
                i++;
                // Reason column, nvarchar(63), not null             
                if (false == reader.IsDBNull(i))
                {
                    Reason = reader.GetString(i);
                }
                else
                {
                    Reason = string.Empty;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    VendorClientLookupId = reader.GetString(i);
                }
                else
                {
                    VendorClientLookupId = string.Empty;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    VendorName = reader.GetString(i);
                }
                else
                {
                    VendorName = string.Empty;
                }
                i++;
                // Status column, bigint, not null              
                if (false == reader.IsDBNull(i))
                {
                    Status = reader.GetString(i);
                }
                else
                {
                    Status = string.Empty;
                }
                i++;
                // CreateDate column, nvarchar(15), not null              
                if (false == reader.IsDBNull(i))
                {
                    CreateDate = reader.GetDateTime(i);
                }
                else
                {
                    CreateDate = DateTime.MinValue;
                }
                i++;
                // LineNumber column, bigint, not null              
                if (false == reader.IsDBNull(i))
                {
                    LineNumber = reader.GetInt32(i);
                }
                else
                {
                    LineNumber = 0;
                }
                i++;
                // PartClientLookupId column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    PartClientLookupId = reader.GetString(i);
                }
                else
                {
                    PartClientLookupId = string.Empty;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    POLDescription = reader.GetString(i);
                }
                else
                {
                    POLDescription = string.Empty;
                }
                i++;
                // OrderQuantity column, bigint, not null              
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
                    UnitOfMeasure = reader.GetString(i);
                }
                else
                {
                    UnitOfMeasure = string.Empty;
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
                    LineTotal = reader.GetDecimal(i);
                }
                else
                {
                    LineTotal = 0;
                }
                i++;
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
                    ReceivedTotalCost = reader.GetDecimal(i);
                }
                else
                {
                    ReceivedTotalCost = 0;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    LineItemStatus = reader.GetString(i);
                }
                else
                {
                    LineItemStatus = string.Empty;
                }
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["AreaId"].ToString(); }
                catch { missing.Append("AreaId "); }

                try { reader["DepartmentId"].ToString(); }
                catch { missing.Append("DepartmentId "); }

                try { reader["StoreroomId"].ToString(); }
                catch { missing.Append("StoreroomId "); }

                try { reader["AccountClientLookupId"].ToString(); }
                catch { missing.Append("AccountClientLookupId "); }

                try { reader["AccountName"].ToString(); }
                catch { missing.Append("AccountName "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Reason"].ToString(); }
                catch { missing.Append("Reason "); }

                try { reader["VendorClientLookupId"].ToString(); }
                catch { missing.Append("VendorClientLookupId "); }

                try { reader["VendorName"].ToString(); }
                catch { missing.Append("VendorName "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["LineNumber"].ToString(); }
                catch { missing.Append("LineNumber "); }

                try { reader["PartClientLookupId"].ToString(); }
                catch { missing.Append("PartClientLookupId "); }

                try { reader["POLDescription"].ToString(); }
                catch { missing.Append("POLDescription "); }

                try { reader["Terms"].ToString(); }
                catch { missing.Append("Terms "); }

                try { reader["VendorId"].ToString(); }
                catch { missing.Append("VendorId "); }

                try { reader["VoidBy_PersonnelId"].ToString(); }
                catch { missing.Append("VoidBy_PersonnelId "); }

                try { reader["OrderQuantity"].ToString(); }
                catch { missing.Append("OrderQuantity "); }

                try { reader["UnitOfMeasure"].ToString(); }
                catch { missing.Append("UnitOfMeasure "); }

                try { reader["UnitCost"].ToString(); }
                catch { missing.Append("UnitCost "); }

                try { reader["LineTotal"].ToString(); }
                catch { missing.Append("LineTotal "); }

                try { reader["QuantityReceived"].ToString(); }
                catch { missing.Append("QuantityReceived "); }

                try { reader["ReceivedTotalCost"].ToString(); }
                catch { missing.Append("ReceivedTotalCost "); }

                try { reader["LineItemStatus"].ToString(); }
                catch { missing.Append("LineItemStatus "); }


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

        #region V2-1079
        public void RetrieveForEDIExport_V2(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
           string callerUserName
       )
        {
            Database.SqlClient.ProcessRow<b_PurchaseOrder> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_PurchaseOrder>(reader => { this.LoadFromDatabaseEDIExport_V2(reader); return this; });
                Database.StoredProcedure.usp_PurchaseOrder_RetrieveByPKForEDIExport_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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
        public void LoadFromDatabaseEDIExport_V2(SqlDataReader reader)
        {
            int i = 0;

            try
            {

                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // PurchaseOrderId column, bigint, not null
                PurchaseOrderId = reader.GetInt64(i++);

                // SiteId column, bigint, not null
                SiteId = reader.GetInt64(i++);

                // DepartmentId column, bigint, not null
                DepartmentId = reader.GetInt64(i++);

                // AreaId column, bigint, not null
                AreaId = reader.GetInt64(i++);

                // StoreroomId column, bigint, not null
                StoreroomId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);

                if (false == reader.IsDBNull(i))
                {
                    CreateDate = reader.GetDateTime(i);
                }
                else
                {
                    CreateDate = DateTime.MinValue;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Attention = reader.GetString(i);
                }
                else
                {
                    Attention = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    TermDescription = reader.GetString(i);
                }
                else
                {
                    TermDescription = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Carrier = reader.GetString(i);
                }
                else
                {
                    Carrier = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Shipto_ClientLookUpId = reader.GetString(i);
                }
                else
                {
                    Shipto_ClientLookUpId = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    ShipToAddress1 = reader.GetString(i);
                }
                else
                {
                    ShipToAddress1 = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    ShipToAddress2 = reader.GetString(i);
                }
                else
                {
                    ShipToAddress2 = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    ShipToAddress3 = reader.GetString(i);
                }
                else
                {
                    ShipToAddress3 = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    ShipToAddressCity = reader.GetString(i);
                }
                else
                {
                    ShipToAddressCity = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    ShipToAddressState = reader.GetString(i);
                }
                else
                {
                    ShipToAddressState = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    ShipToAddressPostCode = reader.GetString(i);
                }
                else
                {
                    ShipToAddressPostCode = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    ShipToAddressCountry = reader.GetString(i);
                }
                else
                {
                    ShipToAddressCountry = "";
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
                    VendorAddress1 = reader.GetString(i);
                }
                else
                {
                    VendorAddress1 = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    VendorAddress2 = reader.GetString(i);
                }
                else
                {
                    VendorAddress2 = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    VendorAddress3 = reader.GetString(i);
                }
                else
                {
                    VendorAddress3 = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    VendorAddressCity = reader.GetString(i);
                }
                else
                {
                    VendorAddressCity = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    VendorAddressState = reader.GetString(i);
                }
                else
                {
                    VendorAddressState = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    VendorAddressPostCode = reader.GetString(i);
                }
                else
                {
                    VendorAddressPostCode = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    VendorAddressCountry = reader.GetString(i);
                }
                else
                {
                    VendorAddressCountry = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Buyer_PersonnelName = reader.GetString(i);
                }
                else
                {
                    Buyer_PersonnelName = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Buyer_Phone = reader.GetString(i);
                }
                else
                {
                    Buyer_Phone = "";
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
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["PurchaseOrderId"].ToString(); }
                catch { missing.Append("PurchaseOrderId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["DepartmentId"].ToString(); }
                catch { missing.Append("DepartmentId "); }

                try { reader["AreaId"].ToString(); }
                catch { missing.Append("AreaId "); }

                try { reader["StoreroomId"].ToString(); }
                catch { missing.Append("StoreroomId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate"); }

                try { reader["Attention"].ToString(); }
                catch { missing.Append("Attention "); }

                try { reader["TermDescription"].ToString(); }
                catch { missing.Append("TermDescription"); }

                try { reader["CarrierName"].ToString(); }
                catch { missing.Append("CarrierName "); }

                try { reader["ShipToClientLookupId"].ToString(); }
                catch { missing.Append("ShipToClientLookupId "); }

                try { reader["ShipToName"].ToString(); }
                catch { missing.Append("ShipToName "); }

                try { reader["ShipToAddress1"].ToString(); }
                catch { missing.Append("ShipToAddress1 "); }

                try { reader["ShipToAddress2"].ToString(); }
                catch { missing.Append("ShipToAddress2 "); }

                try { reader["ShipToAddress3"].ToString(); }
                catch { missing.Append("ShipToAddress3 "); }

                try { reader["ShipToAddressCity"].ToString(); }
                catch { missing.Append("ShipToAddressCity "); }

                try { reader["ShipToAddressState"].ToString(); }
                catch { missing.Append("ShipToAddressState "); }

                try { reader["ShipToAddressPostCode"].ToString(); }
                catch { missing.Append("ShipToAddressPostCode "); }

                try { reader["ShipToAddressCountry"].ToString(); }
                catch { missing.Append("ShipToAddressCountry "); }

                try { reader["VendorClientLookupId"].ToString(); }
                catch { missing.Append("VendorClientLookupId "); }

                try { reader["VendorName"].ToString(); }
                catch { missing.Append("VendorName "); }

                try { reader["VendorAddress1"].ToString(); }
                catch { missing.Append("VendorAddress1 "); }

                try { reader["VendorAddress2"].ToString(); }
                catch { missing.Append("VendorAddress2 "); }

                try { reader["VendorAddress3"].ToString(); }
                catch { missing.Append("VendorAddress3 "); }

                try { reader["VendorAddressCity"].ToString(); }
                catch { missing.Append("VendorAddressCity "); }

                try { reader["VendorAddressState"].ToString(); }
                catch { missing.Append("VendorAddressState "); }

                try { reader["VendorAddressPostCode"].ToString(); }
                catch { missing.Append("VendorAddressPostCode "); }

                try { reader["VendorAddressCountry"].ToString(); }
                catch { missing.Append("VendorAddressCountry "); }

                try { reader["Buyer_PersonnelName"].ToString(); }
                catch { missing.Append("Buyer_PersonnelName "); }

                try { reader["Buyer_Phone"].ToString(); }
                catch { missing.Append("Buyer_Phone "); }

                try { reader["TotalCost"].ToString(); }
                catch { missing.Append("TotalCost "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }
        public void RetrieveByClientIDAndSiteIdAndClientLookUpId_V2(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName,
          ref List<b_PurchaseOrder> results
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

                results = Database.StoredProcedure.usp_PurchaseOrder_RetrieveByClientIDAndSiteIdAndClientLookUpIdForEPMInvoiceImport_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public static b_PurchaseOrder ProcessRowForEPMInvoiceImport(SqlDataReader reader)
        {
            // Create instance of object
            b_PurchaseOrder PurchaseOrder = new b_PurchaseOrder();
            PurchaseOrder.LoadFromDatabaseForEPMInvoiceImport(reader);
            return PurchaseOrder;
        }
        public int LoadFromDatabaseForEPMInvoiceImport(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // PurchaseOrderId column, bigint, not null
                PurchaseOrderId = reader.GetInt64(i++);

                // SiteId column, bigint, not null
                SiteId = reader.GetInt64(i++);

                // DepartmentId column, bigint, not null
                DepartmentId = reader.GetInt64(i++);

                // AreaId column, bigint, not null
                AreaId = reader.GetInt64(i++);

                // StoreroomId column, bigint, not null
                StoreroomId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);

                // Attention column, nvarchar(63), not null
                Attention = reader.GetString(i++);

                // Buyer_PersonnelId column, bigint, not null
                Buyer_PersonnelId = reader.GetInt64(i++);

                // Carrier column, nvarchar(15), not null
                Carrier = reader.GetString(i++);

                // CompleteBy_PersonnelId column, bigint, not null
                CompleteBy_PersonnelId = reader.GetInt64(i++);

                // Required column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    Required = reader.GetDateTime(i);
                }
                else
                {
                    Required = DateTime.MinValue;
                }
                i++;
                // CompleteDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    CompleteDate = reader.GetDateTime(i);
                }
                else
                {
                    CompleteDate = DateTime.MinValue;
                }
                i++;
                // Creator_PersonnelId column, bigint, not null
                Creator_PersonnelId = reader.GetInt64(i++);

                // FOB column, nvarchar(15), not null
                FOB = reader.GetString(i++);

                // Status column, nvarchar(15), not null
                Status = reader.GetString(i++);

                // Terms column, nvarchar(15), not null
                Terms = reader.GetString(i++);

                // VendorId column, bigint, not null
                VendorId = reader.GetInt64(i++);

                // VoidBy_PersonnelId column, bigint, not null
                VoidBy_PersonnelId = reader.GetInt64(i++);

                // VoidDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    VoidDate = reader.GetDateTime(i);
                }
                else
                {
                    VoidDate = DateTime.MinValue;
                }
                i++;
                // VoidReason column, nvarchar(15), not null
                VoidReason = reader.GetString(i++);

                // Reason column, nvarchar(255), not null
                Reason = reader.GetString(i++);

                // MessageToVendor column, nvarchar(255), not null
                MessageToVendor = reader.GetString(i++);

                // ExPurchaseOrderId column, bigint, not null
                ExPurchaseOrderId = reader.GetInt64(i++);

                // ExPurchaseRequest column, nvarchar(31), not null
                ExPurchaseRequest = reader.GetString(i++);

                // Currency column, nvarchar(15), not null
                Currency = reader.GetString(i++);

                // Revision column, int, not null
                Revision = reader.GetInt32(i++);

                // PaymentTerms column, nvarchar(50), not null
                PaymentTerms = reader.GetString(i++);

                // IsExternal column, bit, not null
                IsExternal = reader.GetBoolean(i++);

                // IsPunchout column, bit, not null
                IsPunchout = reader.GetBoolean(i++);

                // SentOrderRequest column, bit, not null
                SentOrderRequest = reader.GetBoolean(i++);

                // Shipto column, bigint, not null
                Shipto = reader.GetInt64(i++);

                // UpdateIndex column, int, not null
                UpdateIndex = reader.GetInt32(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["PurchaseOrderId"].ToString(); }
                catch { missing.Append("PurchaseOrderId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["DepartmentId"].ToString(); }
                catch { missing.Append("DepartmentId "); }

                try { reader["AreaId"].ToString(); }
                catch { missing.Append("AreaId "); }

                try { reader["StoreroomId"].ToString(); }
                catch { missing.Append("StoreroomId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Attention"].ToString(); }
                catch { missing.Append("Attention "); }

                try { reader["Buyer_PersonnelId"].ToString(); }
                catch { missing.Append("Buyer_PersonnelId "); }

                try { reader["Carrier"].ToString(); }
                catch { missing.Append("Carrier "); }

                try { reader["CompleteBy_PersonnelId"].ToString(); }
                catch { missing.Append("CompleteBy_PersonnelId "); }

                try { reader["Required"].ToString(); }
                catch { missing.Append("Required "); }

                try { reader["CompleteDate"].ToString(); }
                catch { missing.Append("CompleteDate "); }

                try { reader["Creator_PersonnelId"].ToString(); }
                catch { missing.Append("Creator_PersonnelId "); }

                try { reader["FOB"].ToString(); }
                catch { missing.Append("FOB "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["Terms"].ToString(); }
                catch { missing.Append("Terms "); }

                try { reader["VendorId"].ToString(); }
                catch { missing.Append("VendorId "); }

                try { reader["VoidBy_PersonnelId"].ToString(); }
                catch { missing.Append("VoidBy_PersonnelId "); }

                try { reader["VoidDate"].ToString(); }
                catch { missing.Append("VoidDate "); }

                try { reader["VoidReason"].ToString(); }
                catch { missing.Append("VoidReason "); }

                try { reader["Reason"].ToString(); }
                catch { missing.Append("Reason "); }

                try { reader["MessageToVendor"].ToString(); }
                catch { missing.Append("MessageToVendor "); }

                try { reader["ExPurchaseOrderId"].ToString(); }
                catch { missing.Append("ExPurchaseOrderId "); }

                try { reader["ExPurchaseRequest"].ToString(); }
                catch { missing.Append("ExPurchaseRequest "); }

                try { reader["Currency"].ToString(); }
                catch { missing.Append("Currency "); }

                try { reader["Revision"].ToString(); }
                catch { missing.Append("Revision "); }

                try { reader["PaymentTerms"].ToString(); }
                catch { missing.Append("PaymentTerms "); }

                try { reader["IsExternal"].ToString(); }
                catch { missing.Append("IsExternal "); }

                try { reader["IsPunchout"].ToString(); }
                catch { missing.Append("IsPunchout "); }

                try { reader["SentOrderRequest"].ToString(); }
                catch { missing.Append("SentOrderRequest "); }

                try { reader["Shipto"].ToString(); }
                catch { missing.Append("Shipto "); }

                try { reader["UpdateIndex"].ToString(); }
                catch { missing.Append("UpdateIndex "); }


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

        #region V2-1112
        public void RetrieveEPMByPurchaseOrdeV2Print(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref b_PurchaseOrder results
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

                results = Database.StoredProcedure.usp_POPrint_RetrieveEPMByPurchaseOrder_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public static b_PurchaseOrder ProcessRowForPurchaseOrderEPMPrint(SqlDataReader reader)
        {
            // Create instance of object
            b_PurchaseOrder obj = new b_PurchaseOrder();

            // Load the object from the database
            obj.LoadFromDatabaseForPurchaseOrderEPMPrint(reader);

            // Return result
            return obj;
        }
        public void LoadFromDatabaseForPurchaseOrderEPMPrint(SqlDataReader reader)
        {
            //   int i = this.LoadFromDatabase(reader);
            int i = 0;
            try
            {
                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // PurchaseOrderId  column, bigint, not null
                PurchaseOrderId = reader.GetInt64(i++);

                // SiteId  column, bigint, not null
                SiteId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);

                // CreateDate column, Datetime2(7),null
                if (false == reader.IsDBNull(i))
                {
                    CreateDate = reader.GetDateTime(i);
                }
                else
                {
                    CreateDate = DateTime.MinValue;
                }
                i++;

                // Status column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    Status = reader.GetString(i);
                }
                else
                {
                    Status = "";
                }
                i++;

                // StatusDate column, Datetime2(7),null
                if (false == reader.IsDBNull(i))
                {
                    StatusDate = reader.GetDateTime(i);
                }
                else
                {
                    StatusDate = DateTime.MinValue;
                }
                i++;

                // StoreroomId column, bigint, not null
                StoreroomId = reader.GetInt64(i++);

                // Shipto column, bigint, not null
                Shipto = reader.GetInt64(i++);

                // Shipto_ClientLookUpId column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    Shipto_ClientLookUpId = reader.GetString(i);
                }
                else
                {
                    Shipto_ClientLookUpId = "";
                }
                i++;

                // ShipToAddress1 column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    ShipToAddress1 = reader.GetString(i);
                }
                else
                {
                    ShipToAddress1 = "";
                }
                i++;

                // ShipToAddress2 column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    ShipToAddress2 = reader.GetString(i);
                }
                else
                {
                    ShipToAddress2 = "";
                }
                i++;

                // ShipToAddress3 column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    ShipToAddress3 = reader.GetString(i);
                }
                else
                {
                    ShipToAddress3 = "";
                }
                i++;

                // ShipToAddressCity column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    ShipToAddressCity = reader.GetString(i);
                }
                else
                {
                    ShipToAddressCity = "";
                }
                i++;

                // ShipToAddressState column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    ShipToAddressState = reader.GetString(i);
                }
                else
                {
                    ShipToAddressState = "";
                }
                i++;

                // ShipToAddressPostCode column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    ShipToAddressPostCode = reader.GetString(i);
                }
                else
                {
                    ShipToAddressPostCode = "";
                }
                i++;

                // ShipToAddressCountry column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    ShipToAddressCountry = reader.GetString(i);
                }
                else
                {
                    ShipToAddressCountry = "";
                }
                i++;

                // VendorName column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    VendorName = reader.GetString(i);
                }
                else
                {
                    VendorName = "";
                }
                i++;

                // VendorAddress1 column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    VendorAddress1 = reader.GetString(i);
                }
                else
                {
                    VendorAddress1 = "";
                }
                i++;

                // VendorAddress2 column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    VendorAddress2 = reader.GetString(i);
                }
                else
                {
                    VendorAddress2 = "";
                }
                i++;

                // VendorAddress3 column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    VendorAddress3 = reader.GetString(i);
                }
                else
                {
                    VendorAddress3 = "";
                }
                i++;

                // VendorAddressCity column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    VendorAddressCity = reader.GetString(i);
                }
                else
                {
                    VendorAddressCity = "";
                }
                i++;

                // VendorAddressState column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    VendorAddressState = reader.GetString(i);
                }
                else
                {
                    VendorAddressState = "";
                }
                i++;

                // VendorAddressPostCode column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    VendorAddressPostCode = reader.GetString(i);
                }
                else
                {
                    VendorAddressPostCode = "";
                }
                i++;

                // VendorAddressCountry column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    VendorAddressCountry = reader.GetString(i);
                }
                else
                {
                    VendorAddressCountry = "";
                }
                i++;

                // SiteName column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    SiteName = reader.GetString(i);
                }
                else
                {
                    SiteName = "";
                }
                i++;

                // SiteAddress1 column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    SiteAddress1 = reader.GetString(i);
                }
                else
                {
                    SiteAddress1 = "";
                }
                i++;

                // SiteAddress2 column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    SiteAddress2 = reader.GetString(i);
                }
                else
                {
                    SiteAddress2 = "";
                }
                i++;

                // SiteAddress3 column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    SiteAddress3 = reader.GetString(i);
                }
                else
                {
                    SiteAddress3 = "";
                }
                i++;

                // SiteAddressCity column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    SiteAddressCity = reader.GetString(i);
                }
                else
                {
                    SiteAddressCity = "";
                }
                i++;

                // SiteAddressState column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    SiteAddressState = reader.GetString(i);
                }
                else
                {
                    SiteAddressState = "";
                }
                i++;

                // SiteAddressPostCode column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    SiteAddressPostCode = reader.GetString(i);
                }
                else
                {
                    SiteAddressPostCode = "";
                }
                i++;

                // SiteAddressCountry column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    SiteAddressCountry = reader.GetString(i);
                }
                else
                {
                    SiteAddressCountry = "";
                }
                i++;

                // Required column, Datetime2(7),null
                if (false == reader.IsDBNull(i))
                {
                    Required = reader.GetDateTime(i);
                }
                else
                {
                    Required = DateTime.MinValue;
                }
                i++;

                // Terms column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    Terms = reader.GetString(i);
                }
                else
                {
                    Terms = "";
                }
                i++;

                // Carrier column, nvarchar(15), not null
                if (false == reader.IsDBNull(i))
                {
                    Carrier = reader.GetString(i);
                }
                else
                {
                    Carrier = "";
                }
                i++;

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["PurchaseOrderId"].ToString(); }
                catch { missing.Append("PurchaseOrderId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["StatusDate"].ToString(); }
                catch { missing.Append("StatusDate "); }

                try { reader["StoreroomId"].ToString(); }
                catch { missing.Append("StoreroomId "); }

                try { reader["Shipto"].ToString(); }
                catch { missing.Append("Shipto "); }

                try { reader["Shipto_ClientLookUpId"].ToString(); }
                catch { missing.Append("Shipto_ClientLookUpId "); }

                try { reader["ShipToAddress1"].ToString(); }
                catch { missing.Append("ShipToAddress1 "); }

                try { reader["ShipToAddress2"].ToString(); }
                catch { missing.Append("ShipToAddress2 "); }

                try { reader["ShipToAddress3"].ToString(); }
                catch { missing.Append("ShipToAddress3 "); }

                try { reader["ShipToAddressCity"].ToString(); }
                catch { missing.Append("ShipToAddressCity "); }

                try { reader["ShipToAddressState"].ToString(); }
                catch { missing.Append("ShipToAddressState "); }

                try { reader["ShipToAddressPostCode"].ToString(); }
                catch { missing.Append("ShipToAddressPostCode "); }

                try { reader["ShipToAddressCountry"].ToString(); }
                catch { missing.Append("ShipToAddressCountry "); }

                try { reader["VendorName"].ToString(); }
                catch { missing.Append("VendorName "); }

                try { reader["VendorAddress1"].ToString(); }
                catch { missing.Append("VendorAddress1 "); }

                try { reader["VendorAddress2"].ToString(); }
                catch { missing.Append("VendorAddress2 "); }

                try { reader["VendorAddress3"].ToString(); }
                catch { missing.Append("VendorAddress3 "); }

                try { reader["VendorAddressCity"].ToString(); }
                catch { missing.Append("VendorAddressCity "); }

                try { reader["VendorAddressState"].ToString(); }
                catch { missing.Append("VendorAddressState "); }

                try { reader["VendorAddressPostCode"].ToString(); }
                catch { missing.Append("VendorAddressPostCode "); }

                try { reader["VendorAddressCountry"].ToString(); }
                catch { missing.Append("VendorAddressCountry "); }

                try { reader["SiteName"].ToString(); }
                catch { missing.Append("SiteName "); }

                try { reader["SiteAddress1"].ToString(); }
                catch { missing.Append("SiteAddress1 "); }

                try { reader["SiteAddress2"].ToString(); }
                catch { missing.Append("SiteAddress2 "); }

                try { reader["SiteAddress3"].ToString(); }
                catch { missing.Append("SiteAddress3 "); }

                try { reader["SiteAddressCity"].ToString(); }
                catch { missing.Append("SiteAddressCity "); }

                try { reader["SiteAddressState"].ToString(); }
                catch { missing.Append("SiteAddressState "); }

                try { reader["SiteAddressPostCode"].ToString(); }
                catch { missing.Append("SiteAddressPostCode "); }

                try { reader["SiteAddressCountry"].ToString(); }
                catch { missing.Append("SiteAddressCountry "); }

                try { reader["Required"].ToString(); }
                catch { missing.Append("Required "); }

                try { reader["Terms"].ToString(); }
                catch { missing.Append("Terms "); }

                try { reader["Carrier"].ToString(); }
                catch { missing.Append("Carrier "); }

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
