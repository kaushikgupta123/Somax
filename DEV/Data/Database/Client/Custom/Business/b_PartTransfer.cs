/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2014 by SOMAX Inc.
* All rights reserved. 
****************************************************************************************************
* Date        Task ID   Person          Description
* =========== ======== ================ ============================================================

 * ****************************************************************************************************
 */

using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.Business
{
    /// <summary>
    /// Business object that stores a record from the TechSpecs table.
    /// </summary>
    public partial class b_PartTransfer 
    {
        // SOM-1123
        public Int64 SiteId { get; set; }
        public bool IsAdmin { get; set; }
        public Int32 CaseNo { get; set; }
        public Int32 CustomQueryDisplayId { get; set; }
        public DateTime CreateDate { get; set; }
        public string RequestPart_ClientLookupId { get; set; }
        public string RequestSite_Name { get; set; }
        public string RequestPart_Description { get; set; }
        public string CreateBy_PersonnelName { get; set; }
        public string IssueSite_Name { get; set; }
        public string IssuePart_ClientLookupId { get; set; }
        public decimal IssuePart_QtyOnHand { get; set; }
        public string IssuePart_Description { get; set; }
        public string LastEventBy_PersonnelName { get; set; }

        public string Description { get; set; }
        public Int64 PartTransferEventLogId { get; set; }

        public decimal QtyOnHand { get; set; }
        public string ShipperName { get; set; }
        public string IssueSite_Address1 { get; set; }
        public string IssueSite_Address2 { get; set; }
        public string IssueSite_CityStateZip { get; set; }
        public DateTime TransactionDate { get; set; }
        public string RequestSite_Address1 { get; set; }
        public string RequestSite_Address2 { get; set; }
        public string RequestSite_CityStateZip { get; set; }
        public string RequesterName { get; set; }      
        public decimal AvgCostAfter { get; set; }
        public decimal CostAfter { get; set; }
        public decimal RequestQuantity { get; set; }
        public Int64 LoggedUser_PersonnelId { get; set; }
        public string TransactionType { get; set; }
        public string Comments { get; set; }
        public string EventCode { get; set; }
        public bool ConfirmForceComplete { get; set; }
        public string CancelOrDenyState { get; set; }
        public string CancelDenyReason { get; set; }
        public decimal TxnQuantity { get; set; }

        public void RetrieveAllForSearch(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref List<b_PartTransfer> results
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

                results = Database.StoredProcedure.usp_PartTransfer_RetrieveAllForSearch.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        public void RetrieveByPKForeignKey(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName
        )
        {
            Database.SqlClient.ProcessRow<b_PartTransfer> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_PartTransfer>(reader => { this.LoadFromDatabaseByPKForeignKey(reader); return this; });
                Database.StoredProcedure.usp_PartTransfer_RetrieveByPKForeignKey.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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
        public static b_PartTransfer ProcessRowForPartTransferRetriveAllForSearch(SqlDataReader reader)
        {
            // Create instance of object
            b_PartTransfer parttransfer = new b_PartTransfer();
            parttransfer.LoadFromDatabaseByPKForeignKey(reader);
            return parttransfer;
        }
        public void LoadFromDatabaseByPKForeignKey(SqlDataReader reader)
        {
            int i = this.LoadFromDatabase(reader);
            //int i = 0;
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
                    RequestPart_ClientLookupId = reader.GetString(i);
                }
                else
                {
                    RequestPart_ClientLookupId = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    RequestSite_Name = reader.GetString(i);
                }
                else
                {
                    RequestSite_Name = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    RequestPart_Description = reader.GetString(i);
                }
                else
                {
                    RequestPart_Description = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    CreateBy_PersonnelName = reader.GetString(i);
                }
                else
                {
                    CreateBy_PersonnelName = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    IssueSite_Name = reader.GetString(i);
                }
                else
                {
                    IssueSite_Name = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    IssuePart_ClientLookupId = reader.GetString(i);
                }
                else
                {
                    IssuePart_ClientLookupId = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    IssuePart_QtyOnHand = reader.GetDecimal(i);
                }
                else
                {
                    IssuePart_QtyOnHand = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    IssuePart_Description = reader.GetString(i);
                }
                else
                {
                    IssuePart_Description = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    LastEventBy_PersonnelName = reader.GetString(i);
                }
                else
                {
                    LastEventBy_PersonnelName = "";
                }
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["RequestPart_ClientLookupId"].ToString(); }
                catch { missing.Append("RequestPart_ClientLookupId "); }

                try { reader["RequestSite_Name"].ToString(); }
                catch { missing.Append("RequestSite_Name "); }

                try { reader["RequestPart_Description"].ToString(); }
                catch { missing.Append("RequestPart_Description "); }

                try { reader["CreateBy_PersonnelName"].ToString(); }
                catch { missing.Append("CreateBy_PersonnelName "); }

                try { reader["IssueSite_Name"].ToString(); }
                catch { missing.Append("IssueSite_Name "); }

                try { reader["IssuePart_ClientLookupId"].ToString(); }
                catch { missing.Append("IssuePart_ClientLookupId "); }

                try { reader["IssuePart_QtyOnHand"].ToString(); }
                catch { missing.Append("IssuePart_QtyOnHand "); }

                try { reader["IssuePart_Description"].ToString(); }
                catch { missing.Append("IssuePart_Description "); }

                try { reader["LastEventBy_PersonnelName"].ToString(); }
                catch { missing.Append("LastEventBy_PersonnelName "); }
               
                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }

        public void PartTransferIssue(
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
                Database.StoredProcedure.usp_PartTransfer_Issue.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public void UpdatePartTransferIssueIntoDatabase(
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
                Database.StoredProcedure.usp_PartTransfer_Issue.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public void UpdatePartTransferReceiveIntoDatabase(
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
                Database.StoredProcedure.usp_PartTransfer_Receive.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public void UpdatePartTransferSendIntoDatabase(
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
                Database.StoredProcedure.usp_PartTransfer_Send.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public void UpdatePartTransferCancelDenyIntoDatabase(
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
                Database.StoredProcedure.usp_PartTransfer_CancelDeny.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public void UpdatePartTransferForceCompleteIntoDatabase(
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
                Database.StoredProcedure.usp_PartTransfer_ForceComplete.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public void RetrieveShipment(
         SqlConnection connection,
         SqlTransaction transaction,
         long callerUserInfoId,
   string callerUserName
           

     )
        {
            Database.SqlClient.ProcessRow<b_PartTransfer> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_PartTransfer>(reader => { this.LoadFromDatabaseShipment(reader); return this; });
                Database.StoredProcedure.usp_PartTransfer_Shipment.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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
        public int LoadFromDatabaseShipment(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                RequestPart_ClientLookupId = reader.GetString(i++); 
                RequestSite_Name = reader.GetString(i++);  
                RequestPart_Description = reader.GetString(i++);          
                RequesterName = reader.GetString(i++);
                IssueSite_Name = reader.GetString(i++);
                IssuePart_ClientLookupId = reader.GetString(i++); 
                QtyOnHand = reader.GetDecimal(i++);               
                IssuePart_Description = reader.GetString(i++);          
                ShipperName = reader.GetString(i++);               
                PartTransferId = reader.GetInt64(i++);                
                IssueSite_Address1 = reader.GetString(i++);
                IssueSite_Address2 = reader.GetString(i++);
                IssueSite_CityStateZip = reader.GetString(i++); 
                if (false == reader.IsDBNull(i))
                {
                    TransactionDate = reader.GetDateTime(i);
                }
                else
                {
                    TransactionDate = DateTime.MinValue;
                }
                i++;
              
                RequestSite_Address1 = reader.GetString(i++);
                RequestSite_Address2 = reader.GetString(i++);
                RequestSite_CityStateZip = reader.GetString(i++);
             
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["RequestPart_ClientLookupId"].ToString(); }
                catch { missing.Append("RequestPart_ClientLookupId "); }

                try { reader["RequestSite_Name"].ToString(); }
                catch { missing.Append("RequestSite_Name "); }

                try { reader["RequestPart_Description"].ToString(); }
                catch { missing.Append("RequestPart_Description "); }

                try { reader["RequesterName"].ToString(); }
                catch { missing.Append("RequesterName "); }

                try { reader["IssueSite_Name"].ToString(); }
                catch { missing.Append("IssueSite_Name "); }

                try { reader["IssuePart_ClientLookupId"].ToString(); }
                catch { missing.Append("IssuePart_ClientLookupId "); }

                try { reader["QtyOnHand"].ToString(); }
                catch { missing.Append("QtyOnHand "); }

                try { reader["IssuePart_Description"].ToString(); }
                catch { missing.Append("IssuePart_Description "); }

                try { reader["ShipperName"].ToString(); }
                catch { missing.Append("ShipperName "); }

                try { reader["PartTransferID"].ToString(); }
                catch { missing.Append("PartTransferID "); }

                try { reader["IssueSite_Address1"].ToString(); }
                catch { missing.Append("IssueSite_Address1 "); }

                try { reader["IssueSite_Address2"].ToString(); }
                catch { missing.Append("IssueSite_Address2 "); }

                try { reader["IssueSite_CityStateZip"].ToString(); }
                catch { missing.Append("IssueSite_CityStateZip "); }

                try { reader["TransactionDate"].ToString(); }
                catch { missing.Append("TransactionDate "); }

                try { reader["RequestSite_Address1"].ToString(); }
                catch { missing.Append("RequestSite_Address1 "); }

                try { reader["RequestSite_Address2"].ToString(); }
                catch { missing.Append("RequestSite_Address2 "); }

                try { reader["RequestSite_CityStateZip"].ToString(); }
                catch { missing.Append("RequestSite_CityStateZip "); }

              

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

    }
}
