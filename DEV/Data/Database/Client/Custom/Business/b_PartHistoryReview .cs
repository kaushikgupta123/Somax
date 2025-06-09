/*
**************************************************************************************************
* PROPRIETARY DATA 
**************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc. and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
**************************************************************************************************
* Copyright (c) 2014 by SOMAX Inc.. All rights reserved. 
**************************************************************************************************
* Date         JIRA Item Person                 Description
* ===========  ========= ====================== =================================================
* 2014-Aug-02  SOM-264   Roger Lawton           Added InactiveFlad to LoadFromDatabaseForClientIdLookup
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
    /// <summary>
    /// Business object thay>t stores a record from the TechSpecs table.
    /// </summar
    public partial class b_PartHistoryReview
    {
        public long SLNo { get; set; }
        public long ClientId { get; set; }
        public long AccountId { get; set; }
        public string ChargeType_Primary { get; set; }
        public long ChargeToId_Primary { get; set; }
        public long RequestorId { get; set; }
        public string Comments { get; set; }
        public decimal Cost { get; set; }
        public string Description { get; set; }
        public long PerformById { get; set; }
        public DateTime? TransactionDate { get; set; }
        public decimal TransactionQuantity { get; set; }
        public string TransactionType { get; set; }
        public string UnitofMeasure { get; set; }
        public string Account_ClientLookupId { get; set; }
        public string Account_Name { get; set; }
        public string ChargeTo_ClientLookupId { get; set; }
        public string ChargeTo_Name { get; set; }
        public string Requestor_Name { get; set; }
        public string PerformBy_Name { get; set; }
        public string PurchaseOrder_ClientLookupId { get; set; }
        public string Vendor_ClientLookupId { get; set; }
        public string Vendor_Name { get; set; }
        public long PartId { get; set; }
        public bool Receipts { get; set; }
        public bool Reversals { get; set; }
        public string DateRange { get; set; }
        public string Storeroom { get; set; }

     
        public void LoadFromDatabaseforSearch(SqlDataReader reader)
        {
            int i = 0;
          
            try
            {
               
                //ClientId
                this.ClientId = reader.GetInt64(i++);

                //AccountId
                this.AccountId = reader.GetInt64(i++);

                // ChargeType_Primary
                
                if (false == reader.IsDBNull(i))
                {
                    ChargeType_Primary = reader.GetString(i);
                }
                else
                {
                    ChargeType_Primary = "";
                }
                i++;
                //ChargeToId_Primary
                this.ChargeToId_Primary = reader.GetInt64(i++);

                //RequestorId
                this.RequestorId = reader.GetInt64(i++);

                // Comments
                if (false == reader.IsDBNull(i))
                {
                    Comments = reader.GetString(i);
                }
                else
                {
                    Comments = "";
                }
                i++;

                //Cost
                Cost = reader.GetDecimal(i++);

                // Description
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i);
                }
                else
                {
                    Description = "";
                }
                i++;
                //PerformById
                this.PerformById = reader.GetInt64(i++);

                //TransactionDate
                if (false == reader.IsDBNull(i))
                {
                    TransactionDate = reader.GetDateTime(i);
                }
                else
                {
                    TransactionDate = DateTime.MinValue;
                }
                i++;

                //TransactionQuantity
                TransactionQuantity = reader.GetDecimal(i++);

                // TransactionType
                if (false == reader.IsDBNull(i))
                {
                    TransactionType = reader.GetString(i);
                }
                else
                {
                    TransactionType = "";
                }
                i++;

                // UnitofMeasure
                if (false == reader.IsDBNull(i))
                {
                    UnitofMeasure = reader.GetString(i);
                }
                else
                {
                    UnitofMeasure = "";
                }
                i++;

                // Account_ClientLookupId
                if (false == reader.IsDBNull(i))
                {
                    Account_ClientLookupId = reader.GetString(i);
                }
                else
                {
                    Account_ClientLookupId = "";
                }
                i++;
                // Account_Name
                if (false == reader.IsDBNull(i))
                {
                    Account_Name = reader.GetString(i);
                }
                else
                {
                    Account_Name = "";
                }
                i++;
                // ChargeTo_ClientLookupId
                if (false == reader.IsDBNull(i))
                {
                    ChargeTo_ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ChargeTo_ClientLookupId = "";
                }
                i++;
                // ChargeTo_Name
                if (false == reader.IsDBNull(i))
                {
                    ChargeTo_Name = reader.GetString(i);
                }
                else
                {
                    ChargeTo_Name = "";
                }
                i++;
                // Requestor_Name
                if (false == reader.IsDBNull(i))
                {
                    Requestor_Name = reader.GetString(i);
                }
                else
                {
                    Requestor_Name = "";
                }
                i++;
                // PerformBy_Name
                if (false == reader.IsDBNull(i))
                {
                    PerformBy_Name = reader.GetString(i);
                }
                else
                {
                    PerformBy_Name = "";
                }
                i++;            
                // PurchaseOrder_ClientLookupId
                if (false == reader.IsDBNull(i))
                {
                    PurchaseOrder_ClientLookupId = reader.GetString(i);
                }
                else
                {
                    PurchaseOrder_ClientLookupId = "";
                }
                i++;
                // Vendor_ClientLookupId
                if (false == reader.IsDBNull(i))
                {
                    Vendor_ClientLookupId = reader.GetString(i);
                }
                else
                {
                    Vendor_ClientLookupId = "";
                }
                i++;
                // Vendor_Name
                if (false == reader.IsDBNull(i))
                {
                    Vendor_Name = reader.GetString(i);
                }
                else
                {
                    Vendor_Name = "";
                }
                i++;
                // Storeroom
                if (false == reader.IsDBNull(i))
                {
                    Storeroom = reader.GetString(i);
                }
                else
                {
                    Storeroom = "";
                }
                i++;

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["AccountId"].ToString(); }
                catch { missing.Append("AccountId "); }

                try { reader["ChargeType_Primary"].ToString(); }
                catch { missing.Append("ChargeType_Primary "); }


                try { reader["ChargeToId_Primary"].ToString(); }
                catch { missing.Append("ChargeToId_Primary "); }

                try { reader["RequestorId"].ToString(); }
                catch { missing.Append("RequestorId "); }

                try { reader["Comments"].ToString(); }
                catch { missing.Append("Comments "); }

                try { reader["Cost"].ToString(); }
                catch { missing.Append("Cost "); }


                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["PerformById"].ToString(); }
                catch { missing.Append("PerformById "); }

                try { reader["TransactionDate"].ToString(); }
                catch { missing.Append("TransactionDate "); }

                try { reader["TransactionQuantity"].ToString(); }
                catch { missing.Append("TransactionQuantity "); }

                try { reader["TransactionType"].ToString(); }
                catch { missing.Append("TransactionType "); }

                try { reader["UnitofMeasure"].ToString(); }
                catch { missing.Append("UnitofMeasure "); }

                try { reader["Account_ClientLookupId"].ToString(); }
                catch { missing.Append("Account_ClientLookupId "); }

                try { reader["Account_Name"].ToString(); }
                catch { missing.Append("Account_Name "); }

                try { reader["ChargeTo_ClientLookupId"].ToString(); }
                catch { missing.Append("ChargeTo_ClientLookupId "); }

                try { reader["ChargeTo_Name"].ToString(); }
                catch { missing.Append("ChargeTo_Name "); }

                try { reader["Requestor_Name"].ToString(); }
                catch { missing.Append("Requestor_Name "); }


                try { reader["PerformBy_Name"].ToString(); }
                catch { missing.Append("PerformBy_Name "); }

                try { reader["PurchaseOrder_ClientLookupId"].ToString(); }
                catch { missing.Append("PurchaseOrder_ClientLookupId "); }

                try { reader["Vendor_ClientLookupId"].ToString(); }
                catch { missing.Append("Vendor_ClientLookupId "); }

                try { reader["Vendor_Name"].ToString(); }
                catch { missing.Append("Vendor_Name "); }
                
                try { reader["Storeroom"].ToString(); }
                catch { missing.Append("Storeroom "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }

        }


        public void RetrievePartHistoryReviewFromDatabase(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
           string callerUserName,
           ref List<b_PartHistoryReview> data
       )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_PartHistoryReview> results = null;
            data = new List<b_PartHistoryReview>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_PartHistory_RetrieveForReview.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_PartHistoryReview>();
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

        public static object ProcessRow(SqlDataReader reader)
        {
            // Create instance of object
            b_PartHistoryReview obj = new b_PartHistoryReview();

            // Load the object from the database
            obj.LoadFromDatabaseforSearch(reader);

            // Return result
            return (object)obj;
        }

        #region V2-760
        public void RetrievePartHistoryReviewFromDatabase_V2(
          SqlConnection connection,
          SqlTransaction transaction,
          long callerUserInfoId,
          string callerUserName,
          ref List<b_PartHistoryReview> data
      )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_PartHistoryReview> results = null;
            data = new List<b_PartHistoryReview>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_PartHistory_RetrieveForReview_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_PartHistoryReview>();
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
    }
}
