/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2015 by SOMAX Inc.
* b_PurchaseRequestLineItem.cs (Data Object)
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID  Person             Description
* =========== ======== ================== =========================================================
* 2015-Oct-06 SOM-823  Indus Net          Modifications
* 2015-Dec-10 SOM-880  Roger Lawton       Changed command timeout for auto-request generation
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
    public partial class b_PurchaseRequest
    {
        #region Property
        public string Created { get; set; }
        public string StatusDrop { get; set; }
        public Int64 UserInfoId { get; set; }
        public string VendorName { get; set; }
        public string VendorPhoneNumber { get; set; }
        public string VendorClientLookupId { get; set; }
        public bool VendorIsExternal { get; set; }
        public string PRClientLookupId { get; set; }
        public string VendorEmailAddress { get; set; }
        public string Creator_PersonnelName { get; set; }
        public string PersonnelName { get; set; }
        public string Approved_PersonnelName { get; set; }
        public string Processed_PersonnelName { get; set; }
        public int CountLineItem { get; set; }
        public Int64 PurchaseRequestLineItemId { get; set; }
        public int LineNumber { get; set; }
        public string Description { get; set; }
        public decimal TotalCost { get; set; }
        public int CustomQueryDisplayId { get; set; }
        public string DateRange { get; set; }
        public string DateColumn { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public string PartId { get; set; }
        public decimal OrderQuantity { get; set; }
        public string UnitofMeasure { get; set; }
        public decimal UnitCost { get; set; }
        public DateTime ProcessedDate { get; set; }
        //public long PurchaseOrderId { get; set; }
        public string Prefix { get; set; }
        public long PersonnelId { get; set; }
        public long ProcessLogId { get; set; }
        public string PurchaseOrderClientLookupId { get; set; }
        public string CreatedBy { get; set; }//SOM-800,801
        //---------SOM-1029-Api--------------------
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }

        //SOM -  823
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
        public string SiteBillToName { get; set; } //945
        public string SiteBillToAddress1 { get; set; }
        public string SiteBillToAddress2 { get; set; }
        public string SiteBillToAddress3 { get; set; }
        public string SiteBillToAddressCity { get; set; }
        public string SiteBillToAddressCountry { get; set; }
        public string SiteBillToAddressPostCode { get; set; }
        public string SiteBillToAddressState { get; set; }
        // RKL - 2019-10-17 - Added for Coupa Interface
        public long ExVendorId { get; set; }
        public string EXUserId { get; set; }
        public string Ship_to_Code { get; set; }
        public string Terms_Desc { get; set; }
        public string Currency_Code { get; set; }
        public string Source_Type { get; set; }
        public string Line_Type { get; set; }
        public string Acct_Name { get; set; }
        public string Commodity_Code { get; set; }

        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public string offset1 { get; set; }
        public string nextrow { get; set; }
        public string SearchText { get; set; }
        public int TotalCount { get; set; }
        public string CreatedDate { get; set; }
        public string ProcessingDate { get; set; }
        public UtilityAdd utilityAdd { get; set; }
        public Int32 ChildCount { get; set; }
        //V2-347
        public string CreateStartDate { get; set; }
        public string CreateEndDate { get; set; }
        public string ProcessedStartDate { get; set; }
        public string ProcessedEndDate { get; set; }
        //V2-375
        public string CreateStartDateVw { get; set; }
        public string CreateEndDateVw { get; set; }
        public string ProcessedStartDateVw { get; set; }
        public string ProcessedEndDateVw { get; set; }
        public string CancelandDeniedStartDateVw { get; set; }
        public string CancelandDeniedEndDateVw { get; set; }
        //V2-643
        public string PartClientLookupId { get; set; }
        public string VendorIDList { get; set; }
        public long PartID { get; set; }
        public decimal? LastPurchaseCost { get; set; }
        public decimal? QtyToOrder { get; set; }
        public string PartIDList { get; set; }
        public string ExOracleUserId { get; set; }
        public string UserName { get; set; }
        //V2-738
        public string StoreroomName { get; set; }
        #region V2-945
        public string PurchaseRequestIDList { get; set; }
        public List<b_PurchaseRequest> listOfPR { get; set; }
        public List<b_PRHeaderUDF> listOfPRHeaderUDF { get; set; }
        public List<b_PurchaseRequestLineItem> listOfPRLI { get; set; }
        public List<b_PRLineUDF> listOfPRLineUDF { get; set; }
        #endregion
        #endregion
        public void RetrieveAllWorkbenchSearch(
         SqlConnection connection,
         SqlTransaction transaction,
         long callerUserInfoId,
         string callerUserName,
         ref List<b_PurchaseRequest> results
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

                results = Database.StoredProcedure.usp_PurchaseRequest_WorkBenchRetrieveAll.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        // SOM-1231
        public void LoadFromDatabaseExtended(SqlDataReader reader)
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
                    PurchaseRequestId = reader.GetInt64(i);
                }
                else
                {
                    PurchaseRequestId = 0;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    PRClientLookupId = reader.GetString(i);
                }
                else
                {
                    PRClientLookupId = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Reason = reader.GetString(i);
                }
                else
                {
                    Reason = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    CreatedBy = reader.GetString(i);
                }
                else
                {
                    CreatedBy = "";
                }
                i++;
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
                    TotalCost = reader.GetDecimal(i);
                }
                else
                {
                    TotalCost = 0.00M;
                }
                i++;
            }

            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["PRClientLookupId"].ToString(); }
                catch { missing.Append("PRClientLookupId "); }

                try { reader["Reason"].ToString(); }
                catch { missing.Append("Reason "); }

                try { reader["CreatedBy"].ToString(); }
                catch { missing.Append("CreatedBy "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["VendorClientLookupId"].ToString(); }
                catch { missing.Append("VendorClientLookupId "); }

                try { reader["VendorName"].ToString(); }
                catch { missing.Append("VendorName "); }
                // SOM-1231
                try { reader["TotalCost"].ToString(); }
                catch { missing.Append("TotalCost "); }
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

        public void InsertByForeignKeysIntoDatabase(
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


                // First - Create the purchase Request
                Database.StoredProcedure.usp_PurchaseRequest_Create.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
                results = Database.StoredProcedure.usp_PurchaseRequest_ValidateByClientLookupId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public void RetrieveByForeignKeysFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName
        )
        {
            Database.SqlClient.ProcessRow<b_PurchaseRequest> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_PurchaseRequest>(reader => { this.LoadFromDatabaseByPKForeignKey(reader); return this; });
                Database.StoredProcedure.usp_PurchaseRequest_RetrieveByPKForeignKeys.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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
        // RKL - 2019-10-17 - Added for Coupa Interface
        public void RetrieveForCoupaExport(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName
        )
        {
            Database.SqlClient.ProcessRow<b_PurchaseRequest> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_PurchaseRequest>(reader => { this.LoadForCoupaExport(reader); return this; });
                Database.StoredProcedure.usp_PurchaseRequest_RetrieveForCoupaExport.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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
     ref List<b_PurchaseRequest> results
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

                results = Database.StoredProcedure.usp_PurchaseRequest_RetrieveAllForSearch.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
     ref List<b_PurchaseRequest> results
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

                results = Database.StoredProcedure.usp_PurchaseRequest_RetrieveChunkSearch_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        //       public void POReceiptSearch(
        //SqlConnection connection,
        //SqlTransaction transaction,
        //long callerUserInfoId,
        //string callerUserName,
        //ref List<b_PurchaseOrder> results
        //)
        //       {
        //           SqlCommand command = null;
        //           string message = String.Empty;

        //           try
        //           {
        //                Create the command to use in calling the stored procedures
        //               command = new SqlCommand();
        //               command.Connection = connection;
        //               command.Transaction = transaction;

        //                Call the stored procedure to retrieve the data

        //               results = Database.StoredProcedure.usp_PurchaseOrder_RetrieveReceiptSearch.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

        //           }
        //           finally
        //           {
        //               if (null != command)
        //               {
        //                   command.Dispose();
        //                   command = null;
        //               }

        //               message = String.Empty;
        //               callerUserInfoId = 0;
        //               callerUserName = String.Empty;
        //           }
        //       }
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
                if (false == reader.IsDBNull(i)) //SOM-800,801
                {
                    CreatedBy = reader.GetString(i);
                }
                else
                {
                    CreatedBy = "";
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
                    VendorIsExternal = reader.GetBoolean(i);
                }
                else
                {
                    VendorIsExternal = false;
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
                    Approved_PersonnelName = reader.GetString(i);
                }
                else
                {
                    Approved_PersonnelName = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Processed_PersonnelName = reader.GetString(i);
                }
                else
                {
                    Processed_PersonnelName = "";
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

                //ProcessedDate              
                if (false == reader.IsDBNull(i))
                {
                    ProcessedDate = reader.GetDateTime(i);
                }
                else
                {
                    ProcessedDate = DateTime.MinValue;
                }
                i++;
                //PurchaseOrderClientLookupId              
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
                    StoreroomName = reader.GetString(i);
                }
                else
                {
                    StoreroomName = "";
                }
                i++;

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["CreatedBy"].ToString(); } //SOM-800,801
                catch { missing.Append("CreatedBy "); }

                try { reader["PurchaseRequestId"].ToString(); }
                catch { missing.Append("PurchaseRequestId "); }

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

                try { reader["Reason"].ToString(); }
                catch { missing.Append("Reason "); }

                try { reader["ApprovedBy_PersonnelId"].ToString(); }
                catch { missing.Append("ApprovedBy_PersonnelId "); }

                try { reader["Approved_Date"].ToString(); }
                catch { missing.Append("Approved_Date "); }

                try { reader["CreatedBy_PersonnelId"].ToString(); }
                catch { missing.Append("CreatedBy_PersonnelId "); }

                try { reader["Process_Comments"].ToString(); }
                catch { missing.Append("Process_Comments "); }

                try { reader["Processed_Date"].ToString(); }
                catch { missing.Append("Processed_Date "); }

                try { reader["ProcessBy_PersonnelId"].ToString(); }
                catch { missing.Append("ProcessBy_PersonnelId "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["VendorId"].ToString(); }
                catch { missing.Append("VendorId "); }

                try { reader["UpdateIndex"].ToString(); }
                catch { missing.Append("UpdateIndex "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["VendorName"].ToString(); }
                catch { missing.Append("VendorName "); }


                try { reader["VendorEmailAddress"].ToString(); }
                catch { missing.Append("VendorEmailAddress "); }


                try { reader["VendorClientLookupId"].ToString(); }
                catch { missing.Append("VendorClientLookupId "); }

                try { reader["VendorIsExternal"].ToString(); }
                catch { missing.Append("VendorIsExternal "); }

                try { reader["Creator_PersonnelName"].ToString(); }
                catch { missing.Append("Creator_PersonnelName "); }

                try { reader["Approved_PersonnelName"].ToString(); }
                catch { missing.Append("Approved_PersonnelName "); }

                try { reader["Processed_PersonnelName"].ToString(); }
                catch { missing.Append("Processed_PersonnelName "); }


                try { reader["CountLineItem"].ToString(); }
                catch { missing.Append("CountLineItem "); }

                try { reader["TotalCost"].ToString(); }
                catch { missing.Append("TotalCost "); }

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
        public void LoadForCoupaExport(SqlDataReader reader)
        {
            int i = this.LoadFromDatabase(reader);

            try
            {
                Creator_PersonnelName = reader.GetString(i++);
                CountLineItem = reader.GetInt32(i++);
                //TotalCost = reader.GetDecimal(i++);
                if (false == reader.IsDBNull(i))
                {
                    TotalCost = reader.GetDecimal(i);
                }
                else
                {
                    TotalCost = 0;
                }
                i++;
                //ExVendorId = reader.GetInt64(i++);
                if (false == reader.IsDBNull(i))
                {
                    ExVendorId = reader.GetInt64(i);
                }
                else
                {
                    ExVendorId = 0;
                }
                i++;
                EXUserId = reader.GetString(i++);
                Ship_to_Code = reader.GetString(i++);
                Terms_Desc = reader.GetString(i++);
                Currency_Code = reader.GetString(i++);
                Source_Type = reader.GetString(i++);
                Line_Type = reader.GetString(i++);
                Acct_Name = reader.GetString(i++);
                Commodity_Code = reader.GetString(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["Creator_PersonnelName"].ToString(); }
                catch { missing.Append("Creator_PersonnelName "); }

                try { reader["CountLineItem"].ToString(); } //SOM-800,801
                catch { missing.Append("CountLineItem "); }

                try { reader["TotalCost"].ToString(); }
                catch { missing.Append("TotalCost "); }

                try { reader["ExVendorId"].ToString(); }
                catch { missing.Append("ExVendorId "); }

                try { reader["EXUserId"].ToString(); }
                catch { missing.Append("EXUserId "); }

                try { reader["Ship_To_Code"].ToString(); }
                catch { missing.Append("Ship_To_Code "); }
                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }


        public static b_PurchaseRequest ProcessRowForPurchaseRequestRetriveAllForSearch(SqlDataReader reader)
        {
            // Create instance of object
            b_PurchaseRequest PurchaseRequest = new b_PurchaseRequest();
            PurchaseRequest.LoadFromDatabaseForPurchaseRequestRetriveAllForSearch(reader);
            return PurchaseRequest;
        }

        public int LoadFromDatabaseForPurchaseRequestRetriveAllForSearch(SqlDataReader reader)
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
                // PurchaseRequestId column, bigint, not null             
                if (false == reader.IsDBNull(i))
                {
                    PurchaseRequestId = reader.GetInt64(i);
                }
                else
                {
                    PurchaseRequestId = 0;
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
                // Reason column, nvarchar(255), not null               
                if (false == reader.IsDBNull(i))
                {
                    Reason = reader.GetString(i);
                }
                else
                {
                    Reason = string.Empty;
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
                //Vendor Phone Number                
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
                //VendorIsExternal
                if (false == reader.IsDBNull(i))
                {
                    VendorIsExternal = reader.GetBoolean(i++);
                }
                else
                {
                    VendorIsExternal = false;
                }
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
                //Processed_PersonnelName              
                if (false == reader.IsDBNull(i))
                {
                    Processed_PersonnelName = reader.GetString(i);
                }
                else
                {
                    Processed_PersonnelName = string.Empty;
                }
                i++;
                //ProcessedDate              
                if (false == reader.IsDBNull(i))
                {
                    ProcessedDate = reader.GetDateTime(i);
                }
                else
                {
                    ProcessedDate = DateTime.MinValue;
                }
                i++;
                //PurchaseOrderClientLookupId              
                if (false == reader.IsDBNull(i))
                {
                    PurchaseOrderClientLookupId = reader.GetString(i);
                }
                else
                {
                    PurchaseOrderClientLookupId = string.Empty;
                }
                i++;

                //ChildCount
                ChildCount = reader.GetInt32(i++);

                //TotalCount
                TotalCount = reader.GetInt32(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["PurchaseRequestId"].ToString(); }
                catch { missing.Append("PurchaseRequestId "); }

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

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["VendorId"].ToString(); }
                catch { missing.Append("VendorId "); }

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

                try { reader["VendorIsExternal"].ToString(); }
                catch { missing.Append("VendorIsExternal "); }

                try { reader["Creator_PersonnelName"].ToString(); }
                catch { missing.Append("Creator_PersonnelName "); }

                try { reader["Processed_PersonnelName"].ToString(); }
                catch { missing.Append("Processed_PersonnelName "); }

                try { reader["ProcessedDate"].ToString(); }
                catch { missing.Append("ProcessedDate "); }

                try { reader["PurchaseOrderClientLookupId"].ToString(); }
                catch { missing.Append("PurchaseOrderClientLookupId "); }

                try { reader["ChildCount"].ToString(); }
                catch { missing.Append("ChildCount "); }

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
        public void RetrieveByStatus(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref List<b_PurchaseRequest> results
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

                results = Database.StoredProcedure.usp_PurchaseRequest_RetrieveByStatus.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public static b_PurchaseRequest ProcessRowForPurchaseRequestRetriveByStatus(SqlDataReader reader)
        {
            // Create instance of object
            b_PurchaseRequest PurchaseRequest = new b_PurchaseRequest();
            PurchaseRequest.LoadFromDatabaseForPurchaseRequestRetriveByStatus(reader);
            return PurchaseRequest;
        }

        public int LoadFromDatabaseForPurchaseRequestRetriveByStatus(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // PurchaseRequestId column, bigint, not null
                PurchaseRequestId = reader.GetInt64(i++);

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

                // Reason column, nvarchar(255), not null
                Reason = reader.GetString(i++);

                // ApprovedBy_PersonnelId column, bigint, not null
                ApprovedBy_PersonnelId = reader.GetInt64(i++);

                // Approved_Date column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    Approved_Date = reader.GetDateTime(i);
                }
                else
                {
                    Approved_Date = DateTime.MinValue;
                }
                i++;
                // CreatedBy_PersonnelId column, bigint, not null
                CreatedBy_PersonnelId = reader.GetInt64(i++);

                // Process_Comments column, nvarchar(511), not null
                Process_Comments = reader.GetString(i++);

                // Processed_Date column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    Processed_Date = reader.GetDateTime(i);
                }
                else
                {
                    Processed_Date = DateTime.MinValue;
                }
                i++;
                // ProcessBy_PersonnelId column, bigint, not null
                ProcessBy_PersonnelId = reader.GetInt64(i++);

                // Status column, nvarchar(15), not null
                Status = reader.GetString(i++);

                // VendorId column, bigint, not null
                VendorId = reader.GetInt64(i++);


                // PurchaseOrderId column, bigint, not null
                PurchaseOrderId = reader.GetInt64(i++);

                // UpdateIndex column, int, not null
                UpdateIndex = reader.GetInt32(i++);

                //VendorIsExternal
                if (false == reader.IsDBNull(i))
                {
                    VendorIsExternal = reader.GetBoolean(i);
                }
                //else
                //{
                //    VendorIsExternal = false;
                //}
                i++;

                //No of Line Items --Computed
                CountLineItem = reader.GetInt32(i++);

                // Creator Name column, nvarchar(31), not null
                Creator_PersonnelName = reader.GetString(i++);

                // Approver Name column, nvarchar(31), not null
                Approved_PersonnelName = reader.GetString(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["PurchaseRequestId"].ToString(); }
                catch { missing.Append("PurchaseRequestId "); }

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

                try { reader["Reason"].ToString(); }
                catch { missing.Append("Reason "); }

                try { reader["ApprovedBy_PersonnelId"].ToString(); }
                catch { missing.Append("ApprovedBy_PersonnelId "); }

                try { reader["Approved_Date"].ToString(); }
                catch { missing.Append("Approved_Date "); }

                try { reader["CreatedBy_PersonnelId"].ToString(); }
                catch { missing.Append("CreatedBy_PersonnelId "); }

                try { reader["Process_Comments"].ToString(); }
                catch { missing.Append("Process_Comments "); }

                try { reader["Processed_Date"].ToString(); }
                catch { missing.Append("Processed_Date "); }

                try { reader["ProcessBy_PersonnelId"].ToString(); }
                catch { missing.Append("ProcessBy_PersonnelId "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["VendorId"].ToString(); }
                catch { missing.Append("VendorId "); }

                try { reader["UpdateIndex"].ToString(); }
                catch { missing.Append("UpdateIndex "); }

                try { reader["CountLineItem"].ToString(); }
                catch { missing.Append("CountLineItem "); }

                try { reader["Creator_PersonnelName"].ToString(); }
                catch { missing.Append("Creator_PersonnelName "); }

                try { reader["Approved_PersonnelName"].ToString(); }
                catch { missing.Append("Approved_PersonnelName "); }

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
        public void RetrieveForInformation(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref List<b_PurchaseRequest> results
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

                results = Database.StoredProcedure.usp_PurchaseRequest_RetrieveForInformation.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public static b_PurchaseRequest ProcessRowForPurchaseRequestRetrieveForInformation(SqlDataReader reader)
        {
            // Create instance of object
            b_PurchaseRequest PurchaseRequest = new b_PurchaseRequest();
            PurchaseRequest.LoadFromDatabaseForPurchaseRequestRetrieveForInformation(reader);
            return PurchaseRequest;
        }

        public int LoadFromDatabaseForPurchaseRequestRetrieveForInformation(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // PurchaseRequestId column, bigint, not null
                PurchaseRequestId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(15), not null
                ClientLookupId = reader.GetString(i++);

                // CreatedBy_PersonnelId column, bigint, not null
                CreatedBy_PersonnelId = reader.GetInt64(i++);

                // Create_Date column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    CreateDate = reader.GetDateTime(i);
                }
                else
                {
                    CreateDate = DateTime.MinValue;
                }
                i++;

                // ApprovedBy_PersonnelId column, bigint, not null
                ApprovedBy_PersonnelId = reader.GetInt64(i++);

                // Approved_Date column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    Approved_Date = reader.GetDateTime(i);
                }
                else
                {
                    Approved_Date = DateTime.MinValue;
                }
                i++;

                // ProcessBy_PersonnelId column, bigint, not null
                ProcessBy_PersonnelId = reader.GetInt64(i++);

                // Processed_Date column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    Processed_Date = reader.GetDateTime(i);
                }
                else
                {
                    Processed_Date = DateTime.MinValue;
                }
                i++;

                // AutoGenerated column, boolean
                AutoGenerated = reader.GetBoolean(i++);

                // UpdateIndex column, int, not null
                UpdateIndex = reader.GetInt32(i++);

                // Creator Name column, nvarchar(31), not null
                Creator_PersonnelName = reader.GetString(i++);

                // Approver Name column, nvarchar(31), not null
                Approved_PersonnelName = reader.GetString(i++);

                // Processed Personnel Name column, nvarchar(31), not null
                Processed_PersonnelName = reader.GetString(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["PurchaseRequestId"].ToString(); }
                catch { missing.Append("PurchaseRequestId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["CreatedBy_PersonnelId"].ToString(); }
                catch { missing.Append("CreatedBy_PersonnelId "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["ApprovedBy_PersonnelId"].ToString(); }
                catch { missing.Append("ApprovedBy_PersonnelId "); }

                try { reader["Approved_Date"].ToString(); }
                catch { missing.Append("Approved_Date "); }

                try { reader["ProcessBy_PersonnelId"].ToString(); }
                catch { missing.Append("ProcessBy_PersonnelId "); }

                try { reader["Processed_Date"].ToString(); }
                catch { missing.Append("Processed_Date "); }

                try { reader["AutoGenerated"].ToString(); }
                catch { missing.Append("AutoGenerated "); }

                try { reader["UpdateIndex"].ToString(); }
                catch { missing.Append("UpdateIndex "); }

                try { reader["Creator_PersonnelName"].ToString(); }
                catch { missing.Append("Creator_PersonnelName "); }

                try { reader["Approved_PersonnelName"].ToString(); }
                catch { missing.Append("Approved_PersonnelName "); }

                try { reader["Processed_PersonnelName"].ToString(); }
                catch { missing.Append("Processed_PersonnelName "); }

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
        public void ConvertPurchaseRequest(
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
                Database.StoredProcedure.usp_PurchaseRequest_Convert.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public void ConvertPurchaseRequestV2(
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
                Database.StoredProcedure.usp_PurchaseRequest_Convert_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public void PurchaseRequestAutoGeneration(
      SqlConnection connection,
      SqlTransaction transaction,
      long callerUserInfoId,
      string callerUserName)
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
                Database.StoredProcedure.usp_PurchaseRequest_AutoGenCreate.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        //SOM  - 823

        public void RetrieveByForeignKeysFromDatabaseForReport(
          SqlConnection connection,
          SqlTransaction transaction,
          long callerUserInfoId,
          string callerUserName
      )
        {
            Database.SqlClient.ProcessRow<b_PurchaseRequest> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_PurchaseRequest>(reader => { this.LoadFromDatabaseByPKForeignKeyForReport(reader); return this; });
                Database.StoredProcedure.usp_PurchaseRequest_RetrieveByPKForeignKeysForReport.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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

        //-----------Calls From Api--Som-1029----------------------------------------------
        public void RetrieveForExtraction(SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName,
        ref List<b_PurchaseRequest> data)
        {
            SqlCommand command = null;
            string message = String.Empty;
            List<b_PurchaseRequest> results = null;
            data = new List<b_PurchaseRequest>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_PurchaseRequest_RetrieveForExtraction.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    // data = results;
                    data = new List<b_PurchaseRequest>(results);
                }
                else
                {
                    data = new List<b_PurchaseRequest>();
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
        public void LoadFromDatabaseByPKForeignKeyForReport(SqlDataReader reader)
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
                if (false == reader.IsDBNull(i)) //SOM-823
                {
                    CreatedBy = reader.GetString(i);
                }
                else
                {
                    CreatedBy = "";
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
                    VendorEmailAddress = reader.GetString(i);
                }
                else
                {
                    VendorEmailAddress = "";
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
                    Approved_PersonnelName = reader.GetString(i);
                }
                else
                {
                    Approved_PersonnelName = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Processed_PersonnelName = reader.GetString(i);
                }
                else
                {
                    Processed_PersonnelName = "";
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

                //ProcessedDate              
                if (false == reader.IsDBNull(i))
                {
                    ProcessedDate = reader.GetDateTime(i);
                }
                else
                {
                    ProcessedDate = DateTime.MinValue;
                }
                i++;
                //PurchaseOrderClientLookupId              
                if (false == reader.IsDBNull(i))
                {
                    PurchaseOrderClientLookupId = reader.GetString(i);
                }
                else
                {
                    PurchaseOrderClientLookupId = string.Empty;
                }
                i++;

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["CreateDate"].ToString(); } //SOM-823
                catch { missing.Append("CreateDate "); }

                try { reader["CreateBy"].ToString(); } //SOM-800,801
                catch { missing.Append("CreateBy "); }

                try { reader["PurchaseRequestId"].ToString(); }
                catch { missing.Append("PurchaseRequestId "); }

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

                try { reader["Reason"].ToString(); }
                catch { missing.Append("Reason "); }

                try { reader["ApprovedBy_PersonnelId"].ToString(); }
                catch { missing.Append("ApprovedBy_PersonnelId "); }

                try { reader["Approved_Date"].ToString(); }
                catch { missing.Append("Approved_Date "); }

                try { reader["CreatedBy_PersonnelId"].ToString(); }
                catch { missing.Append("CreatedBy_PersonnelId "); }

                try { reader["Process_Comments"].ToString(); }
                catch { missing.Append("Process_Comments "); }

                try { reader["Processed_Date"].ToString(); }
                catch { missing.Append("Processed_Date "); }

                try { reader["ProcessBy_PersonnelId"].ToString(); }
                catch { missing.Append("ProcessBy_PersonnelId "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["VendorId"].ToString(); }
                catch { missing.Append("VendorId "); }

                try { reader["UpdateIndex"].ToString(); }
                catch { missing.Append("UpdateIndex "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

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

                try { reader["VendorClientLookupId"].ToString(); }
                catch { missing.Append("VendorClientLookupId "); }

                try { reader["Creator_PersonnelName"].ToString(); }
                catch { missing.Append("Creator_PersonnelName "); }

                try { reader["Approved_PersonnelName"].ToString(); }
                catch { missing.Append("Approved_PersonnelName "); }

                try { reader["Processed_PersonnelName"].ToString(); }
                catch { missing.Append("Processed_PersonnelName "); }


                try { reader["CountLineItem"].ToString(); }
                catch { missing.Append("CountLineItem "); }

                try { reader["TotalCost"].ToString(); }
                catch { missing.Append("TotalCost "); }

                try { reader["PurchaseOrderClientLookupId"].ToString(); }
                catch { missing.Append("PurchaseOrderClientLookupId "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }

        public static object ProcessRowPRExtracted(SqlDataReader reader)// added on 25-06-2014 by Indusnet
        {
            // Create instance of object           
            b_PurchaseRequest obj = new b_PurchaseRequest();
            obj.LoadFromDatabasePRExtracted(reader);
            // Return result
            return (object)obj;
        }
        public void LoadFromDatabasePRExtracted(SqlDataReader reader)
        {
            int i = 0;

            try
            {
                //ClientId
                this.ClientId = reader.GetInt64(i++);

                //SiteId
                this.SiteId = reader.GetInt64(i++);
                // PartClientLookupId
                if (false == reader.IsDBNull(i))
                {
                    PurchaseRequestId = reader.GetInt64(i);
                }
                else
                {
                    PurchaseRequestId = 0;
                }
                i++;
                //TransactionDate
                if (false == reader.IsDBNull(i))
                {
                    PRClientLookupId = reader.GetString(i);
                }
                else
                {
                    PRClientLookupId = "";
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
                    Reason = reader.GetString(i);
                }
                else
                {
                    Reason = "";
                }
                i++;
                // ChargeType_Primary

                if (false == reader.IsDBNull(i))
                {
                    PersonnelName = reader.GetString(i);
                }
                else
                {
                    PersonnelName = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    LineNumber = reader.GetInt32(i);
                }
                else
                {
                    LineNumber = 0;
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
                    RequiredDate = reader.GetDateTime(i);
                }
                else
                {
                    RequiredDate = DateTime.MinValue;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    PartId = reader.GetString(i);
                }
                else
                {
                    PartId = "";
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


            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId"); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId"); }

                try { reader["PurchaseRequestId"].ToString(); }
                catch { missing.Append("PurchaseRequestId"); }

                try { reader["PRClientLookupId"].ToString(); }
                catch { missing.Append("PRClientLookupId"); }

                try { reader["VendorClientLookupId"].ToString(); }
                catch { missing.Append("VendorClientLookupId"); }

                try { reader["Reason"].ToString(); }
                catch { missing.Append("Reason"); }

                try { reader["UnitofMeasure"].ToString(); }
                catch { missing.Append("UnitofMeasure"); }

                try { reader["PersonnelName"].ToString(); }
                catch { missing.Append("PersonnelName"); }

                try { reader["LineNumber"].ToString(); }
                catch { missing.Append("LineNumber"); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description"); }
                try { reader["Reason"].ToString(); }
                catch { missing.Append("Reason"); }

                try { reader["RequiredDate"].ToString(); }
                catch { missing.Append("RequiredDate"); }

                try { reader["PartId"].ToString(); }
                catch { missing.Append("PartId"); }

                try { reader["PurchaseRequestLineItemId"].ToString(); }
                catch { missing.Append("PurchaseRequestLineItemId"); }

                try { reader["OrderQuantity"].ToString(); }
                catch { missing.Append("OrderQuantity"); }

                try { reader["UnitCost"].ToString(); }
                catch { missing.Append("UnitCost"); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }
        public void UpdateByPKForeignKeysInDatabase(
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
                Database.StoredProcedure.usp_PurchaseRequest_UpdateByPKForeignKeys.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public string Flag { get; set; }

        #region Chunk Search AutoPRGeneration V2-643
        public static b_PurchaseRequest ProcessRowChunkSearchForAutoPRGeneration(SqlDataReader reader)
        {
            // Create instance of object
            b_PurchaseRequest PurchaseRequest = new b_PurchaseRequest();
            PurchaseRequest.LoadFromDatabaseProcessRowChunkSearchForAutoPRGeneration(reader);
            return PurchaseRequest;
        }

        public int LoadFromDatabaseProcessRowChunkSearchForAutoPRGeneration(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // PartID column, bigint, not null              
                if (false == reader.IsDBNull(i))
                {
                    PartID = reader.GetInt64(i);
                }
                else
                {
                    PartID = 0;
                }

                i++;
                // PartClientLookupId column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    PartClientLookupId = reader.GetString(i);
                }
                else
                {
                    PartClientLookupId = string.Empty;
                }
                i++;
                // Description column, nvarchar(127), not null               
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i);
                }
                else
                {
                    Description = string.Empty;
                }
                i++;
                QtyToOrder = reader.GetDecimal(i++);

                if (false == reader.IsDBNull(i))
                {
                    UnitofMeasure = reader.GetString(i);
                }
                else
                {
                    UnitofMeasure = "";
                }
                i++;
                LastPurchaseCost = reader.GetDecimal(i);


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

                //TotalCount
                TotalCount = reader.GetInt32(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["PartID"].ToString(); }
                catch { missing.Append("PartID "); }

                try { reader["PartClientLookupId"].ToString(); }
                catch { missing.Append("PartClientLookupId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["QtyToOrder"].ToString(); }
                catch { missing.Append("QtyToOrder "); }

                try { reader["UnitofMeasure"].ToString(); }
                catch { missing.Append("UnitofMeasure "); }

                try { reader["LastPurchaseCost"].ToString(); }
                catch { missing.Append("LastPurchaseCost "); }

                try { reader["VendorName"].ToString(); }
                catch { missing.Append("VendorName "); }

                try { reader["VendorClientLookupId"].ToString(); }
                catch { missing.Append("VendorClientLookupId "); }

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

        public void RetrieveChunkSearchForAutoPRGeneration(
 SqlConnection connection,
 SqlTransaction transaction,
 long callerUserInfoId,
 string callerUserName,
 ref List<b_PurchaseRequest> results
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

                results = Database.StoredProcedure.usp_PurchaseRequest_RetrieveChunkSearchForAutoPRGeneration_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        #region Create PR AutoGenerate  V2-643
        public void PurchaseRequestAutoGeneration_V2(
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
                Database.StoredProcedure.usp_PurchaseRequest_AutoGenCreate_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this,lulist);
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
        public void RetrieveByIdForExportSAP(
                    SqlConnection connection,
                    SqlTransaction transaction,
                    long callerUserInfoId,
                    string callerUserName
                )
        {
            ProcessRow<b_PurchaseRequest> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new ProcessRow<b_PurchaseRequest>(reader => { this.LoadFromDatabaseForExportSAP(reader); return this; });
                StoredProcedure.usp_PurchaseRequest_RetrieveForExportSAP_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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
        public void LoadFromDatabaseForExportSAP(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // PurchaseRequestId column, bigint, not null
                PurchaseRequestId = reader.GetInt64(i++);

                // SiteId column, bigint, not null
                SiteId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(15), not null
                ClientLookupId = reader.GetString(i++);

                // Approved_Date column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    Approved_Date = reader.GetDateTime(i);
                }
                else
                {
                    Approved_Date = DateTime.MinValue;
                }
                i++;

                // VendorId column, bigint, not null
                VendorId = reader.GetInt64(i++);

                if (false == reader.IsDBNull(i))
                {
                    VendorClientLookupId = reader.GetString(i);
                }
                else
                {
                    VendorClientLookupId = "";
                }
                i++;

                VendorIsExternal = reader.GetBoolean(i++);

                // Status column, nvarchar(15), not null
                Status = reader.GetString(i++);

                // ExOracleUserId column, nvarchar(63), not null
                ExOracleUserId = reader.GetString(i++);

                // ExOracleUserId column, nvarchar(63), not null
                UserName = reader.GetString(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["PurchaseRequestId"].ToString(); } 
                catch { missing.Append("PurchaseRequestId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Approved_Date"].ToString(); }
                catch { missing.Append("Approved_Date "); }

                try { reader["VendorId"].ToString(); }
                catch { missing.Append("VendorId "); }

                try { reader["VendorClientLookupId"].ToString(); }
                catch { missing.Append("VendorClientLookupId "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["ExOracleUserId"].ToString(); }
                catch { missing.Append("ExOracleUserId "); }

                try { reader["UserName"].ToString(); }
                catch { missing.Append("UserName "); }

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

        #region V2-820

        public void RetrieveAllWorkbenchSearch_V2(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName,
        ref List<b_PurchaseRequest> results
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

                results = Database.StoredProcedure.usp_PurchaseRequest_WorkBenchRetrieveAll_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        public void LoadFromDatabaseExtended_V2(SqlDataReader reader)
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
                    PurchaseRequestId = reader.GetInt64(i);
                }
                else
                {
                    PurchaseRequestId = 0;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    PRClientLookupId = reader.GetString(i);
                }
                else
                {
                    PRClientLookupId = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Reason = reader.GetString(i);
                }
                else
                {
                    Reason = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    CreatedBy = reader.GetString(i);
                }
                else
                {
                    CreatedBy = "";
                }
                i++;
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
                    BuyerReview = reader.GetBoolean(i);
                }
                else
                {
                    BuyerReview = false;
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
                    TotalCost = reader.GetDecimal(i);
                }
                else
                {
                    TotalCost = 0.00M;
                }
                i++;
            }

            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["PRClientLookupId"].ToString(); }
                catch { missing.Append("PRClientLookupId "); }

                try { reader["Reason"].ToString(); }
                catch { missing.Append("Reason "); }

                try { reader["CreatedBy"].ToString(); }
                catch { missing.Append("CreatedBy "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["BuyerReview"].ToString(); }
                catch { missing.Append("BuyerReview "); }

                try { reader["VendorClientLookupId"].ToString(); }
                catch { missing.Append("VendorClientLookupId "); }

                try { reader["VendorName"].ToString(); }
                catch { missing.Append("VendorName "); }
                // SOM-1231
                try { reader["TotalCost"].ToString(); }
                catch { missing.Append("TotalCost "); }
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

        #endregion

        #region V2-945
        public void RetrieveAllByPurchaseRequestV2Print(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref b_PurchaseRequest results
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

                results = Database.StoredProcedure.usp_PRPrint_RetrieveAllByPurchaseRequest_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public static b_PurchaseRequest ProcessRowForPurchaseRequestPrint(SqlDataReader reader)
        {
            b_PurchaseRequest purchaseRequest = new b_PurchaseRequest();

            purchaseRequest.LoadFromDatabaseForPurchaseRequestPrint(reader);
            return purchaseRequest;
        }

        public void LoadFromDatabaseForPurchaseRequestPrint(SqlDataReader reader)
        {
            //  int i = this.LoadFromDatabase(reader);
            int i = 0;
            try
            {
                ClientId = reader.GetInt64(i++);

                PurchaseRequestId = reader.GetInt64(i++);

                SiteId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(15), not null
                ClientLookupId = reader.GetString(i++);

                // Reason column, nvarchar(63), not null
                Reason = reader.GetString(i++);

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
                    VendorEmailAddress = reader.GetString(i);
                }
                else
                {
                    VendorEmailAddress = "";
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
                    Creator_PersonnelName = reader.GetString(i);
                }
                else
                {
                    Creator_PersonnelName = "";
                }
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["PurchaseRequestId"].ToString(); }
                catch { missing.Append("PurchaseRequestId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Reason"].ToString(); }
                catch { missing.Append("Reason "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

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

                try { reader["VendorClientLookupId"].ToString(); }
                catch { missing.Append("VendorClientLookupId "); }

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
        #region V2-1196
        public void RetrieveChunkSearchForMultiStoreroomAutoPRGeneration(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref List<b_PurchaseRequest> results
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

                results = Database.StoredProcedure.usp_PurchaseRequest_RetrieveChunkSearchForMultiStoreroomAutoPRGeneration_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
    }
}
