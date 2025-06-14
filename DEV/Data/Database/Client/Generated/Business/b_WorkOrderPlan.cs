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
    /// Business object that stores a record from the WorkOrderPlan table.InsertIntoDatabase
    /// </summary>
    [Serializable()]
    public partial class b_WorkOrderPlan : DataBusinessBase
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public b_WorkOrderPlan()
        {
            ClientId = 0;
            WorkOrderPlanId = 0;
            SiteId = 0;
            AreaId = 0;
            DepartmentId = 0;
            StoreroomId = 0;
            Description = String.Empty;
            StartDate = new System.Nullable<System.DateTime>();
            EndDate = new System.Nullable<System.DateTime>();
            Assign_PersonnelId = 0;
            CompleteDate = new System.Nullable<System.DateTime>();
            CompleteBy_PersonnelId = 0;
            LockPlan = false;
            Status = String.Empty;
        }

        /// <summary>
        /// WorkOrderPlanId property
        /// </summary>
        public long WorkOrderPlanId { get; set; }

        /// <summary>
        /// SiteId property
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// AreaId property
        /// </summary>
        public long AreaId { get; set; }

        /// <summary>
        /// DepartmentId property
        /// </summary>
        public long DepartmentId { get; set; }

        /// <summary>
        /// StoreroomId property
        /// </summary>
        public long StoreroomId { get; set; }

        /// <summary>
        /// Description property
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// StartDate property
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// EndDate property
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Assign_PersonnelId property
        /// </summary>
        public long Assign_PersonnelId { get; set; }

        /// <summary>
        /// CompleteDate property
        /// </summary>
        public DateTime? CompleteDate { get; set; }

        /// <summary>
        /// CompleteBy_PersonnelId property
        /// </summary>
        public long CompleteBy_PersonnelId { get; set; }

        /// <summary>
        /// LockPlan property
        /// </summary>
        public bool LockPlan { get; set; }

        /// <summary>
        /// Status property
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Process the current row in the input SqlDataReader into a b_WorkOrderPlan object.
        /// This routine should be applied to the usp_WorkOrderPlan_RetrieveByPK stored procedure.
        /// This routine should be applied to the usp_WorkOrderPlan_RetrieveAll. stored procedure.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        /// <returns>object cast of the b_WorkOrderPlan object</returns>
        public static object ProcessRow(SqlDataReader reader)
        {
            // Create instance of object
            b_WorkOrderPlan obj = new b_WorkOrderPlan();

            // Load the object from the database
            obj.LoadFromDatabase(reader);

            // Return result
            return (object)obj;
        }

        /// <summary>
        /// Load the current row in the input SqlDataReader into a b_WorkOrderPlan object.
        /// This routine should be applied to the usp_WorkOrderPlan_RetrieveByPK stored procedure.
        /// This routine should be applied to the usp_WorkOrderPlan_RetrieveAll. stored procedure.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        public int LoadFromDatabase(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // WorkOrderPlanId column, bigint, not null
                WorkOrderPlanId = reader.GetInt64(i++);

                // SiteId column, bigint, not null
                SiteId = reader.GetInt64(i++);

                // AreaId column, bigint, not null
                AreaId = reader.GetInt64(i++);

                // DepartmentId column, bigint, not null
                DepartmentId = reader.GetInt64(i++);

                // StoreroomId column, bigint, not null
                StoreroomId = reader.GetInt64(i++);

                // Description column, nvarchar(200), not null
                Description = reader.GetString(i++);

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
                // EndDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    EndDate = reader.GetDateTime(i);
                }
                else
                {
                    EndDate = DateTime.MinValue;
                }
                i++;
                // Assign_PersonnelId column, bigint, not null
                Assign_PersonnelId = reader.GetInt64(i++);

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
                // CompleteBy_PersonnelId column, bigint, not null
                CompleteBy_PersonnelId = reader.GetInt64(i++);

                // LockPlan column, bit, not null
                LockPlan = reader.GetBoolean(i++);

                // Status column, nvarchar(15), not null
                Status = reader.GetString(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["WorkOrderPlanId"].ToString(); }
                catch { missing.Append("WorkOrderPlanId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["AreaId"].ToString(); }
                catch { missing.Append("AreaId "); }

                try { reader["DepartmentId"].ToString(); }
                catch { missing.Append("DepartmentId "); }

                try { reader["StoreroomId"].ToString(); }
                catch { missing.Append("StoreroomId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["StartDate"].ToString(); }
                catch { missing.Append("StartDate "); }

                try { reader["EndDate"].ToString(); }
                catch { missing.Append("EndDate "); }

                try { reader["Assign_PersonnelId"].ToString(); }
                catch { missing.Append("Assign_PersonnelId "); }

                try { reader["CompleteDate"].ToString(); }
                catch { missing.Append("CompleteDate "); }

                try { reader["CompleteBy_PersonnelId"].ToString(); }
                catch { missing.Append("CompleteBy_PersonnelId "); }

                try { reader["LockPlan"].ToString(); }
                catch { missing.Append("LockPlan "); }

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

        /// <summary>
        /// Insert this object into the database as a WorkOrderPlan table record.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public override void InsertIntoDatabase(
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
                Database.StoredProcedure.usp_WorkOrderPlan_Create.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        /// Update the WorkOrderPlan table record represented by this object in the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public override void UpdateInDatabase(
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
                Database.StoredProcedure.usp_WorkOrderPlan_UpdateByPK.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        /// Delete the WorkOrderPlan table record represented by this object from the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public override void DeleteFromDatabase(
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
                Database.StoredProcedure.usp_WorkOrderPlan_DeleteByPK.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        /// Retrieve all WorkOrderPlan table records represented by this object in the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="data">b_WorkOrderPlan[] that contains the results</param>
        public void RetrieveAllFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName,
            ref b_WorkOrderPlan[] data
        )
        {
            Database.SqlClient.ProcessRow<b_WorkOrderPlan> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_WorkOrderPlan[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_WorkOrderPlan>(reader => { b_WorkOrderPlan obj = new b_WorkOrderPlan(); obj.LoadFromDatabase(reader); return obj; });
                results = Database.StoredProcedure.usp_WorkOrderPlan_RetrieveAll.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, ClientId);

                // Extract the results
                if (null != results)
                {
                    data = (b_WorkOrderPlan[])results.ToArray(typeof(b_WorkOrderPlan));
                }
                else
                {
                    data = new b_WorkOrderPlan[0];
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
        /// Retrieve WorkOrderPlan table records with specified primary key from the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="key">System.Guid that contains the key to use in the lookup</param>
        /// <param name="data">b_WorkOrderPlan[] that contains the results</param>
        public override void RetrieveByPKFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName
        )
        {
            Database.SqlClient.ProcessRow<b_WorkOrderPlan> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_WorkOrderPlan>(reader => { this.LoadFromDatabase(reader); return this; });
                Database.StoredProcedure.usp_WorkOrderPlan_RetrieveByPK.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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
        /// Test equality of two b_WorkOrderPlan objects.
        /// </summary>
        /// <param name="obj">b_WorkOrderPlan object to compare against the current object.</param>
        public bool Equals(b_WorkOrderPlan obj)
        {
            if (ClientId != obj.ClientId) return false;
            if (WorkOrderPlanId != obj.WorkOrderPlanId) return false;
            if (SiteId != obj.SiteId) return false;
            if (AreaId != obj.AreaId) return false;
            if (DepartmentId != obj.DepartmentId) return false;
            if (StoreroomId != obj.StoreroomId) return false;
            if (!Description.Equals(obj.Description)) return false;
            if (!StartDate.Equals(obj.StartDate)) return false;
            if (!EndDate.Equals(obj.EndDate)) return false;
            if (Assign_PersonnelId != obj.Assign_PersonnelId) return false;
            if (!CompleteDate.Equals(obj.CompleteDate)) return false;
            if (CompleteBy_PersonnelId != obj.CompleteBy_PersonnelId) return false;
            if (LockPlan != obj.LockPlan) return false;
            if (!Status.Equals(obj.Status)) return false;
            return true;
        }

        /// <summary>
        /// Test equality of two b_WorkOrderPlan objects.
        /// </summary>
        /// <param name="obj1">b_WorkOrderPlan object to use in the comparison.</param>
        /// <param name="obj2">b_WorkOrderPlan object to use in the comparison.</param>
        public static bool Equals(b_WorkOrderPlan obj1, b_WorkOrderPlan obj2)
        {
            if ((null == obj1) && (null == obj2)) return true;
            if ((null == obj1) && (null != obj2)) return false;
            if ((null != obj1) && (null == obj2)) return false;
            return obj1.Equals(obj2);
        }
    }
}
