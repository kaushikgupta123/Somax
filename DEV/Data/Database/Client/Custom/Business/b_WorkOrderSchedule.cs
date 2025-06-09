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
* 2014-Jul-30  SOM-263   Roger Lawton           Corrected creation/updating of schedule records
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
    /// Business object that stores a record from the Equipment_TechSpecs table.
    /// </summary>
    public partial class b_WorkOrderSchedule
    {
        #region Property
        public string AssignedTo_PersonnelClientLookupId { get; set; }
        public string WorkAssigned_Name { get; set; }
        public string ClientLookupId { get; set; }
        public string NameFirst { get; set; }
        public string NameLast { get; set; }
        public string NameMiddle { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool Flag { get; set; }
        public long SiteId { get; set; }    // RKL 2020-May-06
        public List<List<b_WorkOrderSchedule>> TotalRecords { get; set; }
        public string Description { get; set; }
        public long CaseNo { get; set; }
        public Int32 TotalCount { get; set; }
        #endregion
        public static object ProcessRowForScheduleCrossReference(SqlDataReader reader)
        {

            // Create instance of object
            b_WorkOrderSchedule obj = new b_WorkOrderSchedule();

            // Load the object from the database
            obj.LoadFromDatabaseByClientLookupId(reader);

            // Return result
            return (object)obj;
        }

        public void LoadFromDatabaseByClientLookupId(SqlDataReader reader)
        {
            int i = this.LoadFromDatabase(reader);
            //int i = 0;
            try
            {

                if (false == reader.IsDBNull(i))
                {
                    AssignedTo_PersonnelClientLookupId = reader.GetString(i);
                }
                else
                {
                    AssignedTo_PersonnelClientLookupId = string.Empty;
                }
                i++;


                if (false == reader.IsDBNull(i))
                {
                    WorkAssigned_Name = reader.GetString(i);
                }
                else
                {
                    WorkAssigned_Name = string.Empty;
                }
                i++;

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["AssignedTo_PersonnelClientLookupId"].ToString(); }
                catch { missing.Append("AssignedTo_PersonnelClientLookupId "); }



                try { reader["WorkAssigned_Name"].ToString(); }
                catch { missing.Append("WorkAssigned_Name "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }


        public void UpdateFromDatabaseObject(b_WorkOrderSchedule dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.SiteId = dbObj.SiteId;
            this.CaseNo = dbObj.CaseNo;
            this.UserName = dbObj.UserName;
            this.PersonnelId = dbObj.PersonnelId;           
            this.UpdateIndex = dbObj.UpdateIndex;         
           
        }




        public void RetrieveByWorkOrderIdFromDatabase(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
           string callerUserName,
           ref List<b_WorkOrderSchedule> data
       )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_WorkOrderSchedule> results = null;
            data = new List<b_WorkOrderSchedule>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_WorkOrderSchedule_RetrieveByWorkOrderId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_WorkOrderSchedule>();
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
        // SOM-263
        /// <summary>
        /// Insert this object into the database as a WorkOrderSchedule table record.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public void CreateForWorkOrder(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName)
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                Database.StoredProcedure.usp_WorkOrderSchedule_CreateForWorkOrder.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        // SOM-263
        /// <summary>
        /// Update the work order schedule record 
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public void UpdateForWorkOrder(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName)
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                Database.StoredProcedure.usp_WorkOrderSchedule_UpdateForWorkOrder.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        // SOM-1134
        /// <summary>
        /// Remove a work order schedule record 
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public void RemoveRecord(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName)
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                Database.StoredProcedure.usp_WorkOrderSchedule_RemoveRecord.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public void EditandSaveScheduledWorkOrder(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName)
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                Database.StoredProcedure.usp_WorkOrderSchedule_EditedByWorkOrderSchedId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public void DeleteScheduledWorOrder(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName)
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                Database.StoredProcedure.usp_WorkOrderSchedule_DeleteScheduledWorkOrder.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public static object ProcessRowRetrievePersonnel(SqlDataReader reader)
        {

            // Create instance of object
            b_WorkOrderSchedule obj = new b_WorkOrderSchedule();

            // Load the object from the database
            obj.LoadFromDatabaseRetrievePersonnel(reader);

            // Return result
            return (object)obj;
        }

        public void LoadFromDatabaseRetrievePersonnel(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                if (false == reader.IsDBNull(i))
                {
                    PersonnelId = reader.GetInt64(i);
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = string.Empty;
                }
                i++;


                if (false == reader.IsDBNull(i))
                {
                    NameFirst = reader.GetString(i);
                }
                else
                {
                    NameFirst = string.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    NameLast = reader.GetString(i);
                }
                else
                {
                    NameLast = string.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    NameMiddle = reader.GetString(i);
                }
                else
                {
                    NameMiddle = string.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    FullName = reader.GetString(i);
                }
                else
                {
                    FullName = string.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    UserName = reader.GetString(i);
                }
                else
                {
                    UserName = string.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Email = reader.GetString(i);
                }
                else
                {
                    Email = string.Empty;
                }
                i++;

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["PersonnelId"].ToString(); }
                catch { missing.Append("PersonnelId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["NameFirst"].ToString(); }
                catch { missing.Append("NameFirst "); }

                try { reader["NameLast"].ToString(); }
                catch { missing.Append("NameLast "); }

                try { reader["NameMiddle"].ToString(); }
                catch { missing.Append("NameMiddle "); }

                try { reader["FullName"].ToString(); }
                catch { missing.Append("FullName "); }

                try { reader["UserName"].ToString(); }
                catch { missing.Append("UserName "); }

                try { reader["Email"].ToString(); }
                catch { missing.Append("Email "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }

        public void RetrievePersonnel(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
           string callerUserName,
           ref List<List<b_WorkOrderSchedule>> data
       )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<List<b_WorkOrderSchedule>> results = null;
            data = new List<List<b_WorkOrderSchedule>>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_WorkOrderSchedule_RetrievePersonnel_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<List<b_WorkOrderSchedule>>();
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

        public void RetrievePersonnelByAssetGroupMasterQuery(
       SqlConnection connection,
       SqlTransaction transaction,
       long callerUserInfoId,
       string callerUserName,
       ref List<List<b_WorkOrderSchedule>> data
   )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<List<b_WorkOrderSchedule>> results = null;
            data = new List<List<b_WorkOrderSchedule>>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_WorkOrderSchedule_RetrievePersonnelByAssetGroupMasterQuery_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<List<b_WorkOrderSchedule>>();
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
        public void RetrieveByWorkOrderIdAndSchdeuleId(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
           string callerUserName,
           ref b_WorkOrderSchedule results
       )
        {

            SqlCommand command = null;
            string message = String.Empty;
            //List<b_WorkOrderSchedule> results = null;
            //data = new List<b_WorkOrderSchedule>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                //results = Database.StoredProcedure.usp_WorkOrderSchedule_RetrieveByWorkOrderId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
                results = Database.StoredProcedure.usp_WorkOrderSchedule_RetrieveByWorkOrderIdAndSchdeuleId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                //if (results != null)
                //{
                //    data = results;
                //}
                //else
                //{
                //    data = new List<b_WorkOrderSchedule>();
                //}
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
        public static object ProcessRowForRetrieveWorkOrderIdAndSchdeuleId(SqlDataReader reader)
        {

            // Create instance of object
            b_WorkOrderSchedule obj = new b_WorkOrderSchedule();

            // Load the object from the database
            obj.LoadFromDatabaseForRetrieveWorkOrderIdAndSchdeuleId(reader);

            // Return result
            return (object)obj;
        }

        public void LoadFromDatabaseForRetrieveWorkOrderIdAndSchdeuleId(SqlDataReader reader)
        {
            //int i = this.LoadFromDatabase(reader);
            int i = 0;
            try
            {
                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // WorkOrderSchedId column, bigint, not null
                WorkOrderSchedId = reader.GetInt64(i++);

                // WorkOrderId column, bigint, not null
                WorkOrderId = reader.GetInt64(i++);

                // PersonnelId column, bigint, not null
                PersonnelId = reader.GetInt64(i++);

                // ScheduledStartDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    ScheduledStartDate = reader.GetDateTime(i);
                }
                else
                {
                    ScheduledStartDate = DateTime.MinValue;
                }
                i++;
                // ScheduledHours column, decimal(8,2), not null
                ScheduledHours = reader.GetDecimal(i++);

                NameFirst = reader.GetString(i++);

                NameLast = reader.GetString(i++);

                Description = reader.GetString(i++);

                ClientLookupId = reader.GetString(i++);
                
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["WorkOrderSchedId"].ToString(); }
                catch { missing.Append("WorkOrderSchedId "); }

                try { reader["WorkOrderId"].ToString(); }
                catch { missing.Append("WorkOrderId "); }

                try { reader["PersonnelId"].ToString(); }
                catch { missing.Append("PersonnelId "); }

                try { reader["ScheduledStartDate"].ToString(); }
                catch { missing.Append("ScheduledStartDate "); }

                try { reader["ScheduledHours"].ToString(); }
                catch { missing.Append("ScheduledHours "); }

                try { reader["NameFirst"].ToString(); }
                catch { missing.Append("NameFirst "); }

                try { reader["NameLast"].ToString(); }
                catch { missing.Append("NameLast "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }
                
                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }

        public void RemoveWorkOrderScheduleForLaborScheduling(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName)
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                Database.StoredProcedure.usp_WorkOrderSchedule_RemoveScheduleRecordFromLaborScheduling_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public void RemoveWorkOrderScheduleForResourceList(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName)
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                Database.StoredProcedure.usp_WorkOrderSchedule_RemoveScheduleRecordFromResourceList_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public void DragWorkOrderScheduleForLaborScheduling(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName)
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                Database.StoredProcedure.usp_WorkOrderSchedule_DragScheduleRecordFromLaborScheduling_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public static object ProcessRowForRetrieveWorkOrderCompletebySchdeuleStartDate(SqlDataReader reader)
        {

            // Create instance of object
            b_WorkOrderSchedule obj = new b_WorkOrderSchedule();

            // Load the object from the database
            obj.LoadFromDatabaseForRetrieveWorkOrderCompletebySchdeuleStartDate(reader);

            // Return result
            return (object)obj;
        }

        public void LoadFromDatabaseForRetrieveWorkOrderCompletebySchdeuleStartDate(SqlDataReader reader)
        {
            //int i = this.LoadFromDatabase(reader);
            int i = 0;
            try
            {
                // TotalCount column, bigint, not null
                TotalCount = reader.GetInt32(i++);
                
                // ScheduledStartDate Date 
                if (false == reader.IsDBNull(i))
                {
                    ScheduledStartDate = reader.GetDateTime(i);
                }
                else
                {
                    // Create Date should never be null
                    ScheduledStartDate = DateTime.Now;
                }
                i++;                               
                
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["TotalCount"].ToString(); }
                catch { missing.Append("TotalCount "); }               

                try { reader["ScheduledStartDate"].ToString(); }
                catch { missing.Append("ScheduledStartDate "); }                
                
                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }
        public void RetrieveCountWorkOrderScheduleByComplete(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
           string callerUserName,
            ref List<b_WorkOrderSchedule> data
       )
        {
            SqlCommand command = null;
            string message = String.Empty;
            List<b_WorkOrderSchedule> results = null;
            data = new List<b_WorkOrderSchedule>();
            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;
                results = Database.StoredProcedure.usp_WorkOrderSchedule_RetrieveCompleteRecordsCountByScheduledStartDate_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
                //results = usp_LoginAuditing_RetrieveLoginRecordsCountByCreateDate_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_WorkOrderSchedule>();
                }

                // Call the stored procedure to retrieve the data
                // processRow = new Database.SqlClient.ProcessRow<b_WorkOrderPlan>(reader => { this.LoadFromDatabaseCountPlannedWorkorderByStatus(reader); return this; });
                //return Database.StoredProcedure.usp_WorkOrderSchedule_RetrieveCompleteRecordsCountByScheduledStartDate_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, ClientId, SiteId, PersonnelId, CaseNo);


                // Call the stored procedure to retrieve the data
                //return Database.StoredProcedure.usp_WOPlanLineItem_PlannedWorkorderByComplete_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName,clientId, SiteId, obj.WorkOrderPlanId);
            }
            catch(Exception ex)
            {

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
            }
        }
        public void RetrieveCountWorkOrderScheduleByInComplete(
     SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            ref List<b_WorkOrderSchedule> data
     )
        {
            SqlCommand command = null;
            string message = String.Empty;
            List<b_WorkOrderSchedule> results = null;
            data = new List<b_WorkOrderSchedule>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                results = Database.StoredProcedure.usp_WorkOrderSchedule_RetrieveInCompleteRecordsCountByScheduledStartDate_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
                //results = usp_LoginAuditing_RetrieveLoginRecordsCountByCreateDate_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_WorkOrderSchedule>();
                }

                // Call the stored procedure to retrieve the data
                // Call the stored procedure to retrieve the data
                //return Database.StoredProcedure.usp_WorkOrderSchedule_RetrieveInCompleteRecordsCountByScheduledStartDate_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, ClientId, SiteId, PersonnelId, CaseNo);

            }
            catch
            {
                //return 0;
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
            }
        }

        #region V2-944
        public static b_WorkOrderSchedule ProcessRowForWorkOrderSchedulePrint(SqlDataReader reader)
        {
            b_WorkOrderSchedule workOrderSchedule = new b_WorkOrderSchedule();

            workOrderSchedule.LoadFromDatabaseForWorkOrderSchedulePrint(reader);
            return workOrderSchedule;
        }

        public void LoadFromDatabaseForWorkOrderSchedulePrint(SqlDataReader reader)
        {
            //  int i = this.LoadFromDatabase(reader);
            int i = 0;
            try
            {
                WorkOrderId = reader.GetInt64(i++);

                if (false == reader.IsDBNull(i))
                {
                    ScheduledStartDate = reader.GetDateTime(i);
                }
                else
                {
                    ScheduledStartDate = DateTime.MinValue;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    ScheduledHours = reader.GetDecimal(i);
                }
                else
                {
                    ScheduledHours = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    NameFirst = reader.GetString(i);
                }
                else
                {
                    NameFirst = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    NameLast = reader.GetString(i);
                }
                else
                {
                    NameLast = "";
                }
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ScheduledStartDate"].ToString(); }
                catch { missing.Append("ScheduledStartDate "); }

                try { reader["ScheduledHours"].ToString(); }
                catch { missing.Append("ScheduledHours "); }

                try { reader["NameFirst"].ToString(); }
                catch { missing.Append("NameFirst "); }

                try { reader["NameLast"].ToString(); }
                catch { missing.Append("NameLast "); }

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
