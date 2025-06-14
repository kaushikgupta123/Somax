/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2021 by SOMAX Inc.
 * All rights reserved. 
 ******************************************************************************
 * THIS CODE HAS BEEN GENERATED BY AN AUTOMATED PROCESS.
 * DO NOT MODIFY BY HAND.    MODIFY THE TEMPLATE AND REGENERATE THE CODE 
 * USING THE CURRENT DATABASE IF MODIFICATIONS ARE NEEDED.
 ******************************************************************************
 */

using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using Database.SqlClient;

namespace Database.Business
{
    /// <summary>
    /// Business object that stores a record from the Timecard table.InsertIntoDatabase
    /// </summary>
    [Serializable()]
    public partial class b_Timecard : DataBusinessBase
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public b_Timecard ()
        {
            ClientId = 0;
            TimecardId = 0;
            ChargeType_Primary = String.Empty;
            ChargeToId_Primary = 0;
            ChargeType_Secondary = String.Empty;
            ChargeToId_Secondary = 0;
            AccountId = 0;
            ActionTakenCode = String.Empty;
            BasePay = 0;
            Comments = String.Empty;
            CompleteWorkOrder = false;
            CraftId = 0;
            Craft = String.Empty;
            Crew = String.Empty;
            DifferentialCode = String.Empty;
            DifferentialMult = false;
            DifferentialValue = 0;
            FailureCode = String.Empty;
            FinishDate = new System.Nullable<System.DateTime>();
            Hours = 0;
            OvertimeCode = String.Empty;
            OvertimeMult = false;
            OvertimeValue = 0;
            PersonnelId = 0;
            ReasonNotDone = String.Empty;
            Shift = String.Empty;
            StartDate = new System.Nullable<System.DateTime>();
            Value = 0;
            VMRSWorkAccomplished = String.Empty;
            UpdateIndex = 0;
        }

        /// <summary>
        /// TimecardId property
        /// </summary>
        public long TimecardId { get; set; }

        /// <summary>
        /// ChargeType_Primary property
        /// </summary>
        public string ChargeType_Primary { get; set; }

        /// <summary>
        /// ChargeToId_Primary property
        /// </summary>
        public long ChargeToId_Primary { get; set; }

        /// <summary>
        /// ChargeType_Secondary property
        /// </summary>
        public string ChargeType_Secondary { get; set; }

        /// <summary>
        /// ChargeToId_Secondary property
        /// </summary>
        public long ChargeToId_Secondary { get; set; }

        /// <summary>
        /// AccountId property
        /// </summary>
        public long AccountId { get; set; }

        /// <summary>
        /// ActionTakenCode property
        /// </summary>
        public string ActionTakenCode { get; set; }

        /// <summary>
        /// BasePay property
        /// </summary>
        public decimal BasePay { get; set; }

        /// <summary>
        /// Comments property
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        /// CompleteWorkOrder property
        /// </summary>
        public bool CompleteWorkOrder { get; set; }

        /// <summary>
        /// CraftId property
        /// </summary>
        public long CraftId { get; set; }

        /// <summary>
        /// Craft property
        /// </summary>
        public string Craft { get; set; }

        /// <summary>
        /// Crew property
        /// </summary>
        public string Crew { get; set; }

        /// <summary>
        /// DifferentialCode property
        /// </summary>
        public string DifferentialCode { get; set; }

        /// <summary>
        /// DifferentialMult property
        /// </summary>
        public bool DifferentialMult { get; set; }

        /// <summary>
        /// DifferentialValue property
        /// </summary>
        public decimal DifferentialValue { get; set; }

        /// <summary>
        /// FailureCode property
        /// </summary>
        public string FailureCode { get; set; }

        /// <summary>
        /// FinishDate property
        /// </summary>
        public DateTime? FinishDate { get; set; }

        /// <summary>
        /// Hours property
        /// </summary>
        public decimal Hours { get; set; }

        /// <summary>
        /// OvertimeCode property
        /// </summary>
        public string OvertimeCode { get; set; }

        /// <summary>
        /// OvertimeMult property
        /// </summary>
        public bool OvertimeMult { get; set; }

        /// <summary>
        /// OvertimeValue property
        /// </summary>
        public decimal OvertimeValue { get; set; }

        /// <summary>
        /// PersonnelId property
        /// </summary>
        public long PersonnelId { get; set; }

        /// <summary>
        /// ReasonNotDone property
        /// </summary>
        public string ReasonNotDone { get; set; }

        /// <summary>
        /// Shift property
        /// </summary>
        public string Shift { get; set; }

        /// <summary>
        /// StartDate property
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Value property
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// VMRSWorkAccomplished property
        /// </summary>
        public string VMRSWorkAccomplished { get; set; }

        /// <summary>
        /// UpdateIndex property
        /// </summary>
        public int UpdateIndex { get; set; }

        /// <summary>
        /// Process the current row in the input SqlDataReader into a b_Timecard object.
        /// This routine should be applied to the usp_Timecard_RetrieveByPK stored procedure.
        /// This routine should be applied to the usp_Timecard_RetrieveAll. stored procedure.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        /// <returns>object cast of the b_Timecard object</returns>
        public static object ProcessRow (SqlDataReader reader)
        {
            // Create instance of object
            b_Timecard obj = new b_Timecard();

            // Load the object from the database
            obj.LoadFromDatabase(reader);

            // Return result
            return (object) obj;
        }

        /// <summary>
        /// Load the current row in the input SqlDataReader into a b_Timecard object.
        /// This routine should be applied to the usp_Timecard_RetrieveByPK stored procedure.
        /// This routine should be applied to the usp_Timecard_RetrieveAll. stored procedure.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        public int LoadFromDatabase (SqlDataReader reader)
        {
        int i = 0;
        try
        {

                        // ClientId column, bigint, not null
                        ClientId = reader.GetInt64(i++);

                        // TimecardId column, bigint, not null
                        TimecardId = reader.GetInt64(i++);

                        // ChargeType_Primary column, nvarchar(15), not null
                        ChargeType_Primary = reader.GetString(i++);

                        // ChargeToId_Primary column, bigint, not null
                        ChargeToId_Primary = reader.GetInt64(i++);

                        // ChargeType_Secondary column, nvarchar(15), not null
                        ChargeType_Secondary = reader.GetString(i++);

                        // ChargeToId_Secondary column, bigint, not null
                        ChargeToId_Secondary = reader.GetInt64(i++);

                        // AccountId column, bigint, not null
                        AccountId = reader.GetInt64(i++);

                        // ActionTakenCode column, nvarchar(15), not null
                        ActionTakenCode = reader.GetString(i++);

                        // BasePay column, decimal(10,2), not null
                        BasePay = reader.GetDecimal(i++);

                        // Comments column, nvarchar(MAX), not null
                        Comments = reader.GetString(i++);

                        // CompleteWorkOrder column, bit, not null
                        CompleteWorkOrder = reader.GetBoolean(i++);

                        // CraftId column, bigint, not null
                        CraftId = reader.GetInt64(i++);

                        // Craft column, nvarchar(15), not null
                        Craft = reader.GetString(i++);

                        // Crew column, nvarchar(15), not null
                        Crew = reader.GetString(i++);

                        // DifferentialCode column, nvarchar(15), not null
                        DifferentialCode = reader.GetString(i++);

                        // DifferentialMult column, bit, not null
                        DifferentialMult = reader.GetBoolean(i++);

                        // DifferentialValue column, decimal(10,2), not null
                        DifferentialValue = reader.GetDecimal(i++);

                        // FailureCode column, nvarchar(15), not null
                        FailureCode = reader.GetString(i++);

            // FinishDate column, datetime2, not null
            if (false == reader.IsDBNull(i))
            {
                    FinishDate = reader.GetDateTime(i);
            }
            else
            {
                    FinishDate = DateTime.MinValue;
            }
            i++;
                        // Hours column, decimal(10,2), not null
                        Hours = reader.GetDecimal(i++);

                        // OvertimeCode column, nvarchar(15), not null
                        OvertimeCode = reader.GetString(i++);

                        // OvertimeMult column, bit, not null
                        OvertimeMult = reader.GetBoolean(i++);

                        // OvertimeValue column, decimal(10,2), not null
                        OvertimeValue = reader.GetDecimal(i++);

                        // PersonnelId column, bigint, not null
                        PersonnelId = reader.GetInt64(i++);

                        // ReasonNotDone column, nvarchar(15), not null
                        ReasonNotDone = reader.GetString(i++);

                        // Shift column, nvarchar(15), not null
                        Shift = reader.GetString(i++);

            // StartDate column, datetime2, not null
            if (false == reader.IsDBNull(i))
            {
                    StartDate = reader.GetDateTime(i);
            }
            else
            {
                    StartDate = DateTime.MinValue;
            }
            i++;
                        // Value column, decimal(18,2), not null
                        Value = reader.GetDecimal(i++);

                        // VMRSWorkAccomplished column, nvarchar(15), not null
                        VMRSWorkAccomplished = reader.GetString(i++);

                        // UpdateIndex column, int, not null
                        UpdateIndex = reader.GetInt32(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                
                
            try { reader["ClientId"].ToString(); }
            catch { missing.Append("ClientId "); }
            
            try { reader["TimecardId"].ToString(); }
            catch { missing.Append("TimecardId "); }
            
            try { reader["ChargeType_Primary"].ToString(); }
            catch { missing.Append("ChargeType_Primary "); }
            
            try { reader["ChargeToId_Primary"].ToString(); }
            catch { missing.Append("ChargeToId_Primary "); }
            
            try { reader["ChargeType_Secondary"].ToString(); }
            catch { missing.Append("ChargeType_Secondary "); }
            
            try { reader["ChargeToId_Secondary"].ToString(); }
            catch { missing.Append("ChargeToId_Secondary "); }
            
            try { reader["AccountId"].ToString(); }
            catch { missing.Append("AccountId "); }
            
            try { reader["ActionTakenCode"].ToString(); }
            catch { missing.Append("ActionTakenCode "); }
            
            try { reader["BasePay"].ToString(); }
            catch { missing.Append("BasePay "); }
            
            try { reader["Comments"].ToString(); }
            catch { missing.Append("Comments "); }
            
            try { reader["CompleteWorkOrder"].ToString(); }
            catch { missing.Append("CompleteWorkOrder "); }
            
            try { reader["CraftId"].ToString(); }
            catch { missing.Append("CraftId "); }
            
            try { reader["Craft"].ToString(); }
            catch { missing.Append("Craft "); }
            
            try { reader["Crew"].ToString(); }
            catch { missing.Append("Crew "); }
            
            try { reader["DifferentialCode"].ToString(); }
            catch { missing.Append("DifferentialCode "); }
            
            try { reader["DifferentialMult"].ToString(); }
            catch { missing.Append("DifferentialMult "); }
            
            try { reader["DifferentialValue"].ToString(); }
            catch { missing.Append("DifferentialValue "); }
            
            try { reader["FailureCode"].ToString(); }
            catch { missing.Append("FailureCode "); }
            
            try { reader["FinishDate"].ToString(); }
            catch { missing.Append("FinishDate "); }
            
            try { reader["Hours"].ToString(); }
            catch { missing.Append("Hours "); }
            
            try { reader["OvertimeCode"].ToString(); }
            catch { missing.Append("OvertimeCode "); }
            
            try { reader["OvertimeMult"].ToString(); }
            catch { missing.Append("OvertimeMult "); }
            
            try { reader["OvertimeValue"].ToString(); }
            catch { missing.Append("OvertimeValue "); }
            
            try { reader["PersonnelId"].ToString(); }
            catch { missing.Append("PersonnelId "); }
            
            try { reader["ReasonNotDone"].ToString(); }
            catch { missing.Append("ReasonNotDone "); }
            
            try { reader["Shift"].ToString(); }
            catch { missing.Append("Shift "); }
            
            try { reader["StartDate"].ToString(); }
            catch { missing.Append("StartDate "); }
            
            try { reader["Value"].ToString(); }
            catch { missing.Append("Value "); }
            
            try { reader["VMRSWorkAccomplished"].ToString(); }
            catch { missing.Append("VMRSWorkAccomplished "); }
            
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
        /// Insert this object into the database as a Timecard table record.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public override void InsertIntoDatabase (
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
                Database.StoredProcedure.usp_Timecard_Create.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        /// Update the Timecard table record represented by this object in the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public override void UpdateInDatabase (
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
                Database.StoredProcedure.usp_Timecard_UpdateByPK.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        /// Delete the Timecard table record represented by this object from the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public override void DeleteFromDatabase (
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
                Database.StoredProcedure.usp_Timecard_DeleteByPK.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        /// Retrieve all Timecard table records represented by this object in the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="data">b_Timecard[] that contains the results</param>
        public void RetrieveAllFromDatabase (
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName,
            ref b_Timecard[] data
        )
        {
            Database.SqlClient.ProcessRow<b_Timecard> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_Timecard[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_Timecard>(reader => { b_Timecard obj = new b_Timecard(); obj.LoadFromDatabase(reader); return obj; });
                results = Database.StoredProcedure.usp_Timecard_RetrieveAll.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, ClientId);

                // Extract the results
                if (null != results)
                {
                    data = (b_Timecard[])results.ToArray(typeof(b_Timecard));
                }
                else
                {
                    data = new b_Timecard[0];
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

        /// <summary>
        /// Retrieve Timecard table records with specified primary key from the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="key">System.Guid that contains the key to use in the lookup</param>
        /// <param name="data">b_Timecard[] that contains the results</param>
        public override void RetrieveByPKFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName
        )
        {
            Database.SqlClient.ProcessRow<b_Timecard> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_Timecard>(reader => { this.LoadFromDatabase(reader); return this; });
                Database.StoredProcedure.usp_Timecard_RetrieveByPK.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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

        /// <summary>
        /// Test equality of two b_Timecard objects.
        /// </summary>
        /// <param name="obj">b_Timecard object to compare against the current object.</param>
        public bool Equals (b_Timecard obj)
        {
            if (ClientId != obj.ClientId) return false;
            if (TimecardId != obj.TimecardId) return false;
            if (!ChargeType_Primary.Equals(obj.ChargeType_Primary)) return false;
            if (ChargeToId_Primary != obj.ChargeToId_Primary) return false;
            if (!ChargeType_Secondary.Equals(obj.ChargeType_Secondary)) return false;
            if (ChargeToId_Secondary != obj.ChargeToId_Secondary) return false;
            if (AccountId != obj.AccountId) return false;
            if (!ActionTakenCode.Equals(obj.ActionTakenCode)) return false;
            if (BasePay != obj.BasePay) return false;
            if (!Comments.Equals(obj.Comments)) return false;
            if (CompleteWorkOrder != obj.CompleteWorkOrder) return false;
            if (CraftId != obj.CraftId) return false;
            if (!Craft.Equals(obj.Craft)) return false;
            if (!Crew.Equals(obj.Crew)) return false;
            if (!DifferentialCode.Equals(obj.DifferentialCode)) return false;
            if (DifferentialMult != obj.DifferentialMult) return false;
            if (DifferentialValue != obj.DifferentialValue) return false;
            if (!FailureCode.Equals(obj.FailureCode)) return false;
            if (!FinishDate.Equals(obj.FinishDate)) return false;
            if (Hours != obj.Hours) return false;
            if (!OvertimeCode.Equals(obj.OvertimeCode)) return false;
            if (OvertimeMult != obj.OvertimeMult) return false;
            if (OvertimeValue != obj.OvertimeValue) return false;
            if (PersonnelId != obj.PersonnelId) return false;
            if (!ReasonNotDone.Equals(obj.ReasonNotDone)) return false;
            if (!Shift.Equals(obj.Shift)) return false;
            if (!StartDate.Equals(obj.StartDate)) return false;
            if (!Value.Equals(obj.Value)) return false;
            if (!VMRSWorkAccomplished.Equals(obj.VMRSWorkAccomplished)) return false;
            if (UpdateIndex != obj.UpdateIndex) return false;
            return true;
        }

        /// <summary>
        /// Test equality of two b_Timecard objects.
        /// </summary>
        /// <param name="obj1">b_Timecard object to use in the comparison.</param>
        /// <param name="obj2">b_Timecard object to use in the comparison.</param>
        public static bool Equals (b_Timecard obj1, b_Timecard obj2)
        {
            if ((null == obj1) && (null == obj2)) return true;
            if ((null == obj1) && (null != obj2)) return false;
            if ((null != obj1) && (null == obj2)) return false;
            return obj1.Equals(obj2);
        }
    }
}
