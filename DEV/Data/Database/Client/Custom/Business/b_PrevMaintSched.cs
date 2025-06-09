/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2014 by SOMAX Inc.
* PreventiveMaintenanceDetails.aspx.cs
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID  Person             Description
* =========== ======== ================== =========================================================
* 2014-Aug-29 SOM-304  Roger Lawton       Structure Change and Screen Modification
* 2014-Sep-12 SOM-317  Roger Lawton       Need WorkOrder.ClientLookupId
* 2014-Sep-25 SOM-338  Roger Lawton       Modified LoadFromDatabaseForEquipmentCrossReference
****************************************************************************************************
*/
using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using Database.SqlClient;
using System.Collections.Generic;

namespace Database.Business
{
    /// <summary>
    /// Business object that stores a record from the PrevMaintSched table.
    /// </summary>
    public partial class b_PrevMaintSched
    {
        public long SiteId { get; set; }
        public string ClientLookupId { get; set; }
        public string Description { get; set; }
        public decimal EstimatedTotalCosts { get; set; }
        public decimal jobDuration { get; set; }
        public decimal Duration { get; set; }
        public decimal EstLaborHours { get; set; }

        public decimal EstLaborCost { get; set; }

        public decimal EstMaterialCost { get; set; }

        public decimal EstOtherCost { get; set; }

        public string AssignedTo_PersonnelClientLookupId { get; set; }
        public string ChargeToName { get; set; }
        public string ChargeToClientLookupId { get; set; }
        public DateTime SchedueledDate { get; set; }
        public DateTime CurrentDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public int PMForeCastId { get; set; }
        public string AssignedTo_PersonnelName { get; set; }
        public string Meter_ClientLookupId { get; set; }
        public string WorkOrder_ClientLookupId { get; set; }
        public string Planner_ClientLookupId { get; set; } // V2-950
        public string AssignedTo_PersonnelIds { get; set; } //V2-1013
        public string PMForeCastRequiredDate { get; set; } //V2-1013
        public string AssignedMultiple { get; set; } //V2-1013
        public int NumbersOfPMSchedAssignRecords { get; set; } //V2-1161
        public int ChildCount { get; set; }
        public bool? ForecastDownRequired { get; set; }   //1082
        #region V2-1111
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int Offset { get; set; }
        public int Nextrow { get; set; }
        public string AssignedTo { get; set; }
        public string PMID { get; set; }
        public string NextDue { get; set; }
        public int TotalCount { get; set; }

        #endregion
        #region V2-977
        public long PMSAssignId { get; set; }
        public long IndexId { get; set; }
        #endregion
        public string FrequencyWithType { get; set; } //V2-1212
        /// <summary>
        /// Process the current row in the input SqlDataReader into a b_PrevMaintSched object.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        /// <returns>object cast of the b_PrevMaintSched object</returns>
        public static object ProcessRowExtended(SqlDataReader reader)
        {
            // Create instance of object
            b_PrevMaintSched obj = new b_PrevMaintSched();

            // Load the object from the database
            obj.LoadFromDatabaseExtended(reader);

            // Return result
            return (object)obj;
        }
        //SOMAX-1018
        public static object ProcessRowExtendedForCalenderForecast(SqlDataReader reader)
        {
            // Create instance of object
            b_PrevMaintSched obj = new b_PrevMaintSched();

            // Load the object from the database
            obj.LoadFromDatabaseExtendedForForecast(reader);

            // Return result
            return (object)obj;
        }
        public void LoadFromDatabaseExtendedForForecast(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                PMForeCastId = reader.GetInt32(i++);

                ClientId = reader.GetInt64(i++);

                SiteId = reader.GetInt64(i++);

                // Client lookup ID (From PM )
                ClientLookupId = reader.GetString(i++);

                // AssignedTo_PersonnelId = reader.GetInt64(i++);

                if (false == reader.IsDBNull(i))
                {
                    AssignedTo_PersonnelClientLookupId = reader.GetString(i);
                }
                else
                {
                    AssignedTo_PersonnelClientLookupId = string.Empty;
                }
                i++;
                // Assigned To Personnel Name
                if (false == reader.IsDBNull(i))
                {
                    AssignedTo_PersonnelName = reader.GetString(i);
                }
                else
                {
                    AssignedTo_PersonnelName = string.Empty;
                }
                i++;

                SchedueledDate = reader.GetDateTime(i++);

                RequiredDate = reader.GetDateTime(i++);

                // Description (From PM )
                Description = reader.GetString(i++);

                // Charge To Client Lookup Id
                if (false == reader.IsDBNull(i))
                {
                    ChargeToClientLookupId = reader.GetString(i);
                }
                else
                {
                    ChargeToClientLookupId = string.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    ChargeToName = reader.GetString(i);
                }
                else
                {
                    ChargeToName = string.Empty;
                }
                i++;

                Duration = reader.GetDecimal(i++);

                EstLaborHours = reader.GetDecimal(i++);

                EstLaborCost = reader.GetDecimal(i++);

                EstMaterialCost = reader.GetDecimal(i++);

                EstOtherCost = reader.GetDecimal(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["EstimatedTotalCosts"].ToString(); }
                catch { missing.Append("EstimatedTotalCosts "); }

                try { reader["ChargeToClientLookupId"].ToString(); }
                catch { missing.Append("ChargeToClientLookupId "); }

                try { reader["AssignedTo_PersonnelClientLookupId"].ToString(); }
                catch { missing.Append("AssignedTo_PersonnelClientLookupId "); }

                try { reader["AssignedTo_PersonnelName"].ToString(); }
                catch { missing.Append("AssignedTo_PersonnelName "); }

                try { reader["Meter_ClientLookupId"].ToString(); }
                catch { missing.Append("Meter_ClientLookupId "); }

                try { reader["WorkOrder_ClientLookupId"].ToString(); }
                catch { missing.Append("WorkOrder_ClientLookupId "); }


                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }
        /// <summary>
        /// Load the current row in the input SqlDataReader into a b_PrevMaintSched object.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        public void LoadFromDatabaseExtended(SqlDataReader reader)
        {
            int i = LoadFromDatabase(reader);
            try
            {
                // Client lookup ID (From PM Master)
                ClientLookupId = reader.GetString(i++);

                // Description (From PM Master)
                Description = reader.GetString(i++);

                // Total Estimated Cost (Calculated by SP)
                if (false == reader.IsDBNull(i))
                {
                    EstimatedTotalCosts = reader.GetDecimal(i);
                }
                else
                {
                    EstimatedTotalCosts = 0;
                }
                i++;


                // Charge To Client Lookup Id
                if (false == reader.IsDBNull(i))
                {
                    ChargeToClientLookupId = reader.GetString(i);
                }
                else
                {
                    ChargeToClientLookupId = string.Empty;
                }
                i++;

                // Assigne To Personnel Client Lookup Id
                if (false == reader.IsDBNull(i))
                {
                    AssignedTo_PersonnelClientLookupId = reader.GetString(i);
                }
                else
                {
                    AssignedTo_PersonnelClientLookupId = string.Empty;
                }
                i++;

                // Assigned To Personnel Name
                if (false == reader.IsDBNull(i))
                {
                    AssignedTo_PersonnelName = reader.GetString(i);
                }
                else
                {
                    AssignedTo_PersonnelName = string.Empty;
                }
                i++;

                // Meter Client Lookup Id
                if (false == reader.IsDBNull(i))
                {
                    Meter_ClientLookupId = reader.GetString(i);
                }
                else
                {
                    Meter_ClientLookupId = string.Empty;
                }
                i++;

                // Last Work Order Client Lookup Id
                if (false == reader.IsDBNull(i))
                {
                    WorkOrder_ClientLookupId = reader.GetString(i);
                }
                else
                {
                    WorkOrder_ClientLookupId = string.Empty;
                }
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["EstimatedTotalCosts"].ToString(); }
                catch { missing.Append("EstimatedTotalCosts "); }

                try { reader["ChargeToClientLookupId"].ToString(); }
                catch { missing.Append("ChargeToClientLookupId "); }

                try { reader["AssignedTo_PersonnelClientLookupId"].ToString(); }
                catch { missing.Append("AssignedTo_PersonnelClientLookupId "); }

                try { reader["AssignedTo_PersonnelName"].ToString(); }
                catch { missing.Append("AssignedTo_PersonnelName "); }

                try { reader["Meter_ClientLookupId"].ToString(); }
                catch { missing.Append("Meter_ClientLookupId "); }

                try { reader["WorkOrder_ClientLookupId"].ToString(); }
                catch { missing.Append("WorkOrder_ClientLookupId "); }


                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }

        public void RetrieveByEquipmentIdFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            ref List<b_PrevMaintSched> data
        )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_PrevMaintSched> results = null;
            data = new List<b_PrevMaintSched>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_PrevMaintSched_RetrieveByEquipmentId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_PrevMaintSched>();
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

        public void RetrieveByBIMGuidFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            Guid BIMGuid,
            ref List<b_PrevMaintSched> data
        )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_PrevMaintSched> results = null;
            data = new List<b_PrevMaintSched>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_PrevMaintSched_RetrieveByBIMGuid.CallStoredProcedure(command, callerUserInfoId, callerUserName, BIMGuid, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_PrevMaintSched>();
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

        public void RetrieveByLocationIdFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            ref List<b_PrevMaintSched> data
        )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_PrevMaintSched> results = null;
            data = new List<b_PrevMaintSched>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_PrevMaintSched_RetrieveByLocataionId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_PrevMaintSched>();
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

        public void RetrieveByPrevMaintMasterIdFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            ref List<b_PrevMaintSched> data
        )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_PrevMaintSched> results = null;
            data = new List<b_PrevMaintSched>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_PrevMaintSched_RetrieveByPrevMaintMasterId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_PrevMaintSched>();
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

        //V2-630
        public void RetrieveByPrevMaintMasterIdFromDatabase_V2(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            ref List<b_PrevMaintSched> data
        )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_PrevMaintSched> results = null;
            data = new List<b_PrevMaintSched>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_PrevMaintSched_RetrieveByPrevMaintMasterId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_PrevMaintSched>();
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

        //SOM-1018
        public void RetrievePMOnDemandForecastFromDatabase(
    SqlConnection connection,
    SqlTransaction transaction,
    long callerUserInfoId,
    string callerUserName,
    ref List<b_PrevMaintSched> data
    )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_PrevMaintSched> results = null;
            data = new List<b_PrevMaintSched>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                 results = Database.StoredProcedure.usp_Prevmaintsched_OnDemandForecast.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_PrevMaintSched>();
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
        //SOM-1018
        public void RetrievePMOnDemandForecastFromPrevMaintLibrary(
    SqlConnection connection,
    SqlTransaction transaction,
    long callerUserInfoId,
    string callerUserName,
    ref List<b_PrevMaintSched> data
    )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_PrevMaintSched> results = null;
            data = new List<b_PrevMaintSched>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_Prevmaintsched_OnDemandForecastFromPrevMaintLibrary.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_PrevMaintSched>();
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
        public void RetrievePMCalendarForecastFromDatabase(
       SqlConnection connection,
       SqlTransaction transaction,
       long callerUserInfoId,
       string callerUserName,
       ref List<b_PrevMaintSched> data)
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_PrevMaintSched> results = null;
            data = new List<b_PrevMaintSched>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_PrevMaintSched_CalendarForecast.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_PrevMaintSched>();
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
        //-SOM-1669--//
        public void RetrievePMCalendarForecastFromPrevMaintLibrary(
      SqlConnection connection,
      SqlTransaction transaction,
      long callerUserInfoId,
      string callerUserName,
      ref List<b_PrevMaintSched> data)
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_PrevMaintSched> results = null;
            data = new List<b_PrevMaintSched>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_PrevMaintSched_CalendarForecastFromPrevMaintLibrary.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_PrevMaintSched>();
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

        //SOM-972
        public void RetrievePMSchedulingRecordsFromDatabase(
       SqlConnection connection,
       SqlTransaction transaction,
       long callerUserInfoId,
       string callerUserName,
       ref List<b_PrevMaintSched> data
    )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_PrevMaintSched> results = null;
            data = new List<b_PrevMaintSched>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                //results = Database.StoredProcedure.usp_PrevMaintSched_SchedulingRecordsForReassign.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
                results = Database.StoredProcedure.usp_PrevMaintSched_SchedulingRecordsForReassign_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this); //V2-977

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_PrevMaintSched>();
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



        /// <summary>
        /// Process the current row in the input SqlDataReader into a b_PrevMaintSched object.
        /// This routine should be applied to the usp_PrevMaintSched_RetrieveByPK stored procedure.
        /// This routine should be applied to the usp_PrevMaintSched_RetrieveAll. stored procedure.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        /// <returns>object cast of the b_PrevMaintSched object</returns>
        public static object ProcessRowForPrevMaintComposite(SqlDataReader reader)
        {
            // Create instance of object
            b_PrevMaintSched obj = new b_PrevMaintSched();

            // Load the object from the database
            obj.LoadFromDatabaseForPrevMaintComposite(reader);

            // Return result
            return (object)obj;
        }

        /// <summary>
        /// Load the current row in the input SqlDataReader into a b_PrevMaintSched object.
        /// This routine should be applied to the usp_PrevMaintSched_RetrieveByPK stored procedure.
        /// This routine should be applied to the usp_PrevMaintSched_RetrieveAll. stored procedure.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        public void LoadFromDatabaseForPrevMaintComposite(SqlDataReader reader)
        {
            int i = 18;

            try
            {
                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // PrevMaintSchedId column, bigint, not null
                PrevMaintSchedId = reader.GetInt64(i++);

                // PrevMaintMasterId column, bigint, not null
                PrevMaintMasterId = reader.GetInt64(i++);

                // AssignedTo_PersonnelId column, bigint, not null
                AssignedTo_PersonnelId = reader.GetInt64(i++);

                // AssociationGroup column, nvarchar(15), not null
                AssociationGroup = reader.GetString(i++);

                // CalendarSlack column, int, not null
                CalendarSlack = reader.GetInt32(i++);

                // ChargeToId column, bigint, not null
                ChargeToId = reader.GetInt64(i++);

                // ChargeType column, nvarchar(15), not null
                ChargeType = reader.GetString(i++);

                // Crew column, nvarchar(15), not null
                Crew = reader.GetString(i++);

                // CurrentWOComplete column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    CurrentWOComplete = reader.GetDateTime(i);
                }
                else
                {
                    CurrentWOComplete = DateTime.MinValue;
                }
                i++;
                // DownRequired column, bit, not null
                DownRequired = reader.GetBoolean(i++);

                // ExcludeDOW column, nvarchar(7), not null
                ExcludeDOW = reader.GetString(i++);

                // Frequency column, int, not null
                Frequency = reader.GetInt32(i++);

                // InactiveFlag column, bit, not null
                InactiveFlag = reader.GetBoolean(i++);

                // JobPlan column, nvarchar(MAX), not null
                JobPlan = reader.GetString(i++);

                // Last_WorkOrderId column, bigint, not null
                Last_WorkOrderId = reader.GetInt64(i++);

                // LastPerformed column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    LastPerformed = reader.GetDateTime(i);
                }
                else
                {
                    LastPerformed = DateTime.MinValue;
                }
                i++;
                // LastScheduled column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    LastScheduled = reader.GetDateTime(i);
                }
                else
                {
                    LastScheduled = DateTime.MinValue;
                }
                i++;
                // MeterHighLevel column, decimal(17,3), not null
                MeterHighLevel = reader.GetDecimal(i++);

                // MeterId column, bigint, not null
                MeterId = reader.GetInt64(i++);

                // MeterInterval column, decimal(17,3), not null
                MeterInterval = reader.GetDecimal(i++);

                // MeterLastDone column, decimal(17,3), not null
                MeterLastDone = reader.GetDecimal(i++);

                // MeterLastDue column, decimal(17,3), not null
                MeterLastDue = reader.GetDecimal(i++);

                // MeterLastReading column, decimal(17,3), not null
                MeterLastReading = reader.GetDecimal(i++);

                // MeterLowLevel column, decimal(17,3), not null
                MeterLowLevel = reader.GetDecimal(i++);

                // MeterMethod column, nvarchar(1), not null
                MeterMethod = reader.GetString(i++);

                // MeterOn column, bit, not null
                MeterOn = reader.GetBoolean(i++);

                // MeterSlack column, decimal(17,3), not null
                MeterSlack = reader.GetDecimal(i++);

                // NextDueDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    NextDueDate = reader.GetDateTime(i);
                }
                else
                {
                    NextDueDate = DateTime.MinValue;
                }
                i++;
                // OnDemandGroup column, nvarchar(15), not null
                OnDemandGroup = reader.GetString(i++);

                // Priority column, nvarchar(15), not null
                Priority = reader.GetString(i++);

                // Scheduled column, bit, not null
                Scheduled = reader.GetBoolean(i++);

                // ScheduleType column, nvarchar(15), not null
                ScheduleType = reader.GetString(i++);

                // ScheduleWeeks column, nvarchar(52), not null
                ScheduleWeeks = reader.GetString(i++);

                // Section column, nvarchar(15), not null
                Section = reader.GetString(i++);

                // Shift column, nvarchar(15), not null
                Shift = reader.GetString(i++);

                // Type column, nvarchar(15), not null
                Type = reader.GetString(i++);

                // RIMEWorkClass column, int, not null
                RIMEWorkClass = reader.GetInt32(i++);

                // UpdateIndex column, int, not null
                UpdateIndex = reader.GetInt32(i++);

                if (false == reader.IsDBNull(i))
                {
                    AssignedTo_PersonnelClientLookupId = reader.GetString(i);
                }
                else
                {
                    AssignedTo_PersonnelClientLookupId = "";
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
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["PrevMaintSched_ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["PrevMaintSched_PrevMaintSchedId"].ToString(); }
                catch { missing.Append("PrevMaintSchedId "); }

                try { reader["PrevMaintSched_PrevMaintMasterId"].ToString(); }
                catch { missing.Append("PrevMaintMasterId "); }

                try { reader["PrevMaintSched_AssignedTo_PersonnelId"].ToString(); }
                catch { missing.Append("AssignedTo_PersonnelId "); }

                try { reader["PrevMaintSched_AssociationGroup"].ToString(); }
                catch { missing.Append("AssociationGroup "); }

                try { reader["PrevMaintSched_CalendarSlack"].ToString(); }
                catch { missing.Append("CalendarSlack "); }

                try { reader["PrevMaintSched_ChargeToId"].ToString(); }
                catch { missing.Append("ChargeToId "); }

                try { reader["PrevMaintSched_ChargeType"].ToString(); }
                catch { missing.Append("ChargeType "); }

                try { reader["PrevMaintSched_Crew"].ToString(); }
                catch { missing.Append("Crew "); }

                try { reader["PrevMaintSched_CurrentWOComplete"].ToString(); }
                catch { missing.Append("CurrentWOComplete "); }

                try { reader["PrevMaintSched_DownRequired"].ToString(); }
                catch { missing.Append("DownRequired "); }

                try { reader["PrevMaintSched_ExcludeDOW"].ToString(); }
                catch { missing.Append("ExcludeDOW "); }

                try { reader["PrevMaintSched_Frequency"].ToString(); }
                catch { missing.Append("Frequency "); }

                try { reader["PrevMaintSched_InactiveFlag"].ToString(); }
                catch { missing.Append("InactiveFlag "); }

                try { reader["PrevMaintSched_JobPlan"].ToString(); }
                catch { missing.Append("JobPlan "); }

                try { reader["PrevMaintSched_Last_WorkOrderId"].ToString(); }
                catch { missing.Append("Last_WorkOrderId "); }

                try { reader["PrevMaintSched_LastPerformed"].ToString(); }
                catch { missing.Append("LastPerformed "); }

                try { reader["PrevMaintSched_LastScheduled"].ToString(); }
                catch { missing.Append("LastScheduled "); }

                try { reader["PrevMaintSched_MeterHighLevel"].ToString(); }
                catch { missing.Append("MeterHighLevel "); }

                try { reader["PrevMaintSched_MeterId"].ToString(); }
                catch { missing.Append("MeterId "); }

                try { reader["PrevMaintSched_MeterInterval"].ToString(); }
                catch { missing.Append("MeterInterval "); }

                try { reader["PrevMaintSched_MeterLastDone"].ToString(); }
                catch { missing.Append("MeterLastDone "); }

                try { reader["PrevMaintSched_MeterLastDue"].ToString(); }
                catch { missing.Append("MeterLastDue "); }

                try { reader["PrevMaintSched_MeterLastReading"].ToString(); }
                catch { missing.Append("MeterLastReading "); }

                try { reader["PrevMaintSched_MeterLowLevel"].ToString(); }
                catch { missing.Append("MeterLowLevel "); }

                try { reader["PrevMaintSched_MeterMethod"].ToString(); }
                catch { missing.Append("MeterMethod "); }

                try { reader["PrevMaintSched_MeterOn"].ToString(); }
                catch { missing.Append("MeterOn "); }

                try { reader["PrevMaintSched_MeterSlack"].ToString(); }
                catch { missing.Append("MeterSlack "); }

                try { reader["PrevMaintSched_NextDueDate"].ToString(); }
                catch { missing.Append("NextDueDate "); }

                try { reader["PrevMaintSched_OnDemandGroup"].ToString(); }
                catch { missing.Append("OnDemandGroup "); }

                try { reader["PrevMaintSched_Priority"].ToString(); }
                catch { missing.Append("Priority "); }

                try { reader["PrevMaintSched_Scheduled"].ToString(); }
                catch { missing.Append("Scheduled "); }

                try { reader["PrevMaintSched_ScheduleType"].ToString(); }
                catch { missing.Append("ScheduleType "); }

                try { reader["PrevMaintSched_ScheduleWeeks"].ToString(); }
                catch { missing.Append("ScheduleWeeks "); }

                try { reader["PrevMaintSched_Section"].ToString(); }
                catch { missing.Append("Section "); }

                try { reader["PrevMaintSched_Shift"].ToString(); }
                catch { missing.Append("Shift "); }

                try { reader["PrevMaintSched_Type"].ToString(); }
                catch { missing.Append("Type "); }

                try { reader["PrevMaintSched_RIMEWorkClass"].ToString(); }
                catch { missing.Append("RIMEWorkClass "); }

                try { reader["PrevMaintSched_UpdateIndex"].ToString(); }
                catch { missing.Append("UpdateIndex "); }


                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
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
            data = new List<b_StoredProcValidationError>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_PrevMaintSched_ValidateByClientLookupId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        public void LoadFromDatabaseForForeignKeys(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // PrevMaintSchedId column, bigint, not null
                PrevMaintSchedId = reader.GetInt64(i++);

                // PrevMaintMasterId column, bigint, not null
                PrevMaintMasterId = reader.GetInt64(i++);

                // AssignedTo_PersonnelId column, bigint, not null
                AssignedTo_PersonnelId = reader.GetInt64(i++);

                // AssociationGroup column, nvarchar(15), not null
                AssociationGroup = reader.GetString(i++);

                // CalendarSlack column, int, not null
                CalendarSlack = reader.GetInt32(i++);

                // ChargeToId column, bigint, not null
                ChargeToId = reader.GetInt64(i++);

                // ChargeType column, nvarchar(15), not null
                ChargeType = reader.GetString(i++);

                ChargeToName = reader.GetString(i++);

                // Crew column, nvarchar(15), not null
                Crew = reader.GetString(i++);

                // CurrentWOComplete column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    CurrentWOComplete = reader.GetDateTime(i);
                }
                else
                {
                    CurrentWOComplete = DateTime.MinValue;
                }
                i++;
                // DownRequired column, bit, not null
                DownRequired = reader.GetBoolean(i++);

                // ExcludeDOW column, nvarchar(7), not null
                ExcludeDOW = reader.GetString(i++);

                // Frequency column, int, not null
                Frequency = reader.GetInt32(i++);

                // FrequencyType column, nvarchar(15), not null
                FrequencyType = reader.GetString(i++);

                // InactiveFlag column, bit, not null
                InactiveFlag = reader.GetBoolean(i++);

                // JobPlan column, nvarchar(MAX), not null
                JobPlan = reader.GetString(i++);

                // Last_WorkOrderId column, bigint, not null
                Last_WorkOrderId = reader.GetInt64(i++);

                // LastPerformed column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    LastPerformed = reader.GetDateTime(i);
                }
                else
                {
                    LastPerformed = DateTime.MinValue;
                }
                i++;
                // LastScheduled column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    LastScheduled = reader.GetDateTime(i);
                }
                else
                {
                    LastScheduled = DateTime.MinValue;
                }
                i++;
                // MeterHighLevel column, decimal(17,3), not null
                MeterHighLevel = reader.GetDecimal(i++);

                // MeterId column, bigint, not null
                MeterId = reader.GetInt64(i++);

                // MeterInterval column, decimal(17,3), not null
                MeterInterval = reader.GetDecimal(i++);

                // MeterLastDone column, decimal(17,3), not null
                MeterLastDone = reader.GetDecimal(i++);

                // MeterLastDue column, decimal(17,3), not null
                MeterLastDue = reader.GetDecimal(i++);

                // MeterLastReading column, decimal(17,3), not null
                MeterLastReading = reader.GetDecimal(i++);

                // MeterLowLevel column, decimal(17,3), not null
                MeterLowLevel = reader.GetDecimal(i++);

                // MeterMethod column, nvarchar(1), not null
                MeterMethod = reader.GetString(i++);

                // MeterOn column, bit, not null
                MeterOn = reader.GetBoolean(i++);

                // MeterSlack column, decimal(17,3), not null
                MeterSlack = reader.GetDecimal(i++);

                // NextDueDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    NextDueDate = reader.GetDateTime(i);
                }
                else
                {
                    NextDueDate = DateTime.MinValue;
                }
                i++;
                // OnDemandGroup column, nvarchar(15), not null
                OnDemandGroup = reader.GetString(i++);

                // Priority column, nvarchar(15), not null
                Priority = reader.GetString(i++);

                // Scheduled column, bit, not null
                Scheduled = reader.GetBoolean(i++);

                // ScheduleMethod column, nvarchar(15), not null
                ScheduleMethod = reader.GetString(i++);

                // ScheduleType column, nvarchar(15), not null
                ScheduleType = reader.GetString(i++);

                // ScheduleType_ column, nvarchar(15), not null
                ScheduleType_ = reader.GetString(i++);

                // ScheduleWeeks column, nvarchar(52), not null
                ScheduleWeeks = reader.GetString(i++);

                // Section column, nvarchar(15), not null
                Section = reader.GetString(i++);

                // Shift column, nvarchar(15), not null
                Shift = reader.GetString(i++);

                // Type column, nvarchar(15), not null
                Type = reader.GetString(i++);

                // RIMEWorkClass column, int, not null
                RIMEWorkClass = reader.GetInt32(i++);

                // Category column, nvarchar(15), not null
                Category = reader.GetString(i++);

                // UpdateIndex column, int, not null
                UpdateIndex = reader.GetInt32(i++);
                if ((false == reader.IsDBNull(i)))
                {
                    ChargeToClientLookupId = reader.GetString(i);
                }
                else
                {
                    ChargeToClientLookupId = "";
                }
                i++;

                if ((false == reader.IsDBNull(i)))
                {
                    AssignedTo_PersonnelClientLookupId = reader.GetString(i);
                }
                else
                {
                    AssignedTo_PersonnelClientLookupId = "";
                }
                i++;

                if ((false == reader.IsDBNull(i)))
                {
                    Meter_ClientLookupId = reader.GetString(i);
                }
                else
                {
                    Meter_ClientLookupId = "";
                }
                i++;

                if ((false == reader.IsDBNull(i)))
                {
                    WorkOrder_ClientLookupId = reader.GetString(i);
                }
                else
                {
                    WorkOrder_ClientLookupId = "";
                }
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ChargeToClientLookupId"].ToString(); }
                catch { missing.Append("ChargeToClientLookupId "); }

                try { reader["AssignedTo_PersonnelClientLookupId"].ToString(); }
                catch { missing.Append("AssignedTo_PersonnelClientLookupId "); }

                try { reader["Meter_ClientLookupId"].ToString(); }
                catch { missing.Append("Meter_ClientLookupId "); }

                try { reader["WorkOrder_ClientLookupId"].ToString(); }
                catch { missing.Append("WorkOrder_ClientLookupId "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }
                throw new Exception(msg.ToString(), ex);
            }
        }
        public void LoadFromDatabaseForForeignKeys_V2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // PrevMaintSchedId column, bigint, not null
                PrevMaintSchedId = reader.GetInt64(i++);

                // PrevMaintMasterId column, bigint, not null
                PrevMaintMasterId = reader.GetInt64(i++);

                // AssignedTo_PersonnelId column, bigint, not null
                AssignedTo_PersonnelId = reader.GetInt64(i++);

                // AssociationGroup column, nvarchar(15), not null
                AssociationGroup = reader.GetString(i++);

                // CalendarSlack column, int, not null
                CalendarSlack = reader.GetInt32(i++);

                // ChargeToId column, bigint, not null
                ChargeToId = reader.GetInt64(i++);

                // ChargeType column, nvarchar(15), not null
                ChargeType = reader.GetString(i++);

                ChargeToName = reader.GetString(i++);

                // Crew column, nvarchar(15), not null
                Crew = reader.GetString(i++);

                // CurrentWOComplete column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    CurrentWOComplete = reader.GetDateTime(i);
                }
                else
                {
                    CurrentWOComplete = DateTime.MinValue;
                }
                i++;
                // DownRequired column, bit, not null
                DownRequired = reader.GetBoolean(i++);

                // ExcludeDOW column, nvarchar(7), not null
                ExcludeDOW = reader.GetString(i++);

                // Frequency column, int, not null
                Frequency = reader.GetInt32(i++);

                // FrequencyType column, nvarchar(15), not null
                FrequencyType = reader.GetString(i++);

                // InactiveFlag column, bit, not null
                InactiveFlag = reader.GetBoolean(i++);

                // JobPlan column, nvarchar(MAX), not null
                JobPlan = reader.GetString(i++);

                // Last_WorkOrderId column, bigint, not null
                Last_WorkOrderId = reader.GetInt64(i++);

                // LastPerformed column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    LastPerformed = reader.GetDateTime(i);
                }
                else
                {
                    LastPerformed = DateTime.MinValue;
                }
                i++;
                // LastScheduled column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    LastScheduled = reader.GetDateTime(i);
                }
                else
                {
                    LastScheduled = DateTime.MinValue;
                }
                i++;
                // MeterHighLevel column, decimal(17,3), not null
                MeterHighLevel = reader.GetDecimal(i++);

                // MeterId column, bigint, not null
                MeterId = reader.GetInt64(i++);

                // MeterInterval column, decimal(17,3), not null
                MeterInterval = reader.GetDecimal(i++);

                // MeterLastDone column, decimal(17,3), not null
                MeterLastDone = reader.GetDecimal(i++);

                // MeterLastDue column, decimal(17,3), not null
                MeterLastDue = reader.GetDecimal(i++);

                // MeterLastReading column, decimal(17,3), not null
                MeterLastReading = reader.GetDecimal(i++);

                // MeterLowLevel column, decimal(17,3), not null
                MeterLowLevel = reader.GetDecimal(i++);

                // MeterMethod column, nvarchar(1), not null
                MeterMethod = reader.GetString(i++);

                // MeterOn column, bit, not null
                MeterOn = reader.GetBoolean(i++);

                // MeterSlack column, decimal(17,3), not null
                MeterSlack = reader.GetDecimal(i++);

                // NextDueDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    NextDueDate = reader.GetDateTime(i);
                }
                else
                {
                    NextDueDate = DateTime.MinValue;
                }
                i++;
                // OnDemandGroup column, nvarchar(15), not null
                OnDemandGroup = reader.GetString(i++);

                // Priority column, nvarchar(15), not null
                Priority = reader.GetString(i++);

                // Scheduled column, bit, not null
                Scheduled = reader.GetBoolean(i++);

                // ScheduleMethod column, nvarchar(15), not null
                ScheduleMethod = reader.GetString(i++);

                // ScheduleType column, nvarchar(15), not null
                ScheduleType = reader.GetString(i++);

                // ScheduleType_ column, nvarchar(15), not null
                ScheduleType_ = reader.GetString(i++);

                // ScheduleWeeks column, nvarchar(52), not null
                ScheduleWeeks = reader.GetString(i++);

                // Section column, nvarchar(15), not null
                Section = reader.GetString(i++);

                // Shift column, nvarchar(15), not null
                Shift = reader.GetString(i++);

                // Type column, nvarchar(15), not null
                Type = reader.GetString(i++);

                // RIMEWorkClass column, int, not null
                RIMEWorkClass = reader.GetInt32(i++);

                // Category column, nvarchar(15), not null
                Category = reader.GetString(i++);

                // RootCauseCode column, nvarchar(15), not null
                RootCauseCode = reader.GetString(i++);

                // ActionCode column, nvarchar(15), not null
                ActionCode = reader.GetString(i++);

                // FailureCode column, nvarchar(15), not null
                FailureCode = reader.GetString(i++);

                // Planner_PersonnelId column, bigint, not null
                Planner_PersonnelId = reader.GetInt64(i++);

                // PlanningRequired column, bit, not null   V2-1161
                PlanningRequired = reader.GetBoolean(i++);

                // PMSchedAssignRecords column, int, not null
                NumbersOfPMSchedAssignRecords = reader.GetInt32(i++);

                // UpdateIndex column, int, not null
                UpdateIndex = reader.GetInt32(i++);
                if ((false == reader.IsDBNull(i)))
                {
                    ChargeToClientLookupId = reader.GetString(i);
                }
                else
                {
                    ChargeToClientLookupId = "";
                }
                i++;

                if ((false == reader.IsDBNull(i)))
                {
                    AssignedTo_PersonnelClientLookupId = reader.GetString(i);
                }
                else
                {
                    AssignedTo_PersonnelClientLookupId = "";
                }
                i++;

                if ((false == reader.IsDBNull(i)))
                {
                    Meter_ClientLookupId = reader.GetString(i);
                }
                else
                {
                    Meter_ClientLookupId = "";
                }
                i++;

                if ((false == reader.IsDBNull(i)))
                {
                    WorkOrder_ClientLookupId = reader.GetString(i);
                }
                else
                {
                    WorkOrder_ClientLookupId = "";
                }
                i++;
                if ((false == reader.IsDBNull(i)))
                {
                    Planner_ClientLookupId = reader.GetString(i);
                }
                else
                {
                    Planner_ClientLookupId = "";
                }
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["PrevMaintSchedId"].ToString(); }
                catch { missing.Append("PrevMaintSchedId "); }

                try { reader["PrevMaintMasterId"].ToString(); }
                catch { missing.Append("PrevMaintMasterId "); }

                try { reader["AssignedTo_PersonnelId"].ToString(); }
                catch { missing.Append("AssignedTo_PersonnelId "); }

                try { reader["AssociationGroup"].ToString(); }
                catch { missing.Append("AssociationGroup "); }

                try { reader["CalendarSlack"].ToString(); }
                catch { missing.Append("CalendarSlack "); }

                try { reader["ChargeToId"].ToString(); }
                catch { missing.Append("ChargeToId "); }

                try { reader["ChargeType"].ToString(); }
                catch { missing.Append("ChargeType "); }

                try { reader["Crew"].ToString(); }
                catch { missing.Append("Crew "); }

                try { reader["CurrentWOComplete"].ToString(); }
                catch { missing.Append("CurrentWOComplete "); }

                try { reader["DownRequired"].ToString(); }
                catch { missing.Append("DownRequired "); }

                try { reader["ExcludeDOW"].ToString(); }
                catch { missing.Append("ExcludeDOW "); }

                try { reader["Frequency"].ToString(); }
                catch { missing.Append("Frequency "); }

                try { reader["FrequencyType"].ToString(); }
                catch { missing.Append("FrequencyType "); }

                try { reader["InactiveFlag"].ToString(); }
                catch { missing.Append("InactiveFlag "); }

                try { reader["JobPlan"].ToString(); }
                catch { missing.Append("JobPlan "); }

                try { reader["Last_WorkOrderId"].ToString(); }
                catch { missing.Append("Last_WorkOrderId "); }

                try { reader["LastPerformed"].ToString(); }
                catch { missing.Append("LastPerformed "); }

                try { reader["LastScheduled"].ToString(); }
                catch { missing.Append("LastScheduled "); }

                try { reader["MeterHighLevel"].ToString(); }
                catch { missing.Append("MeterHighLevel "); }

                try { reader["MeterId"].ToString(); }
                catch { missing.Append("MeterId "); }

                try { reader["MeterInterval"].ToString(); }
                catch { missing.Append("MeterInterval "); }

                try { reader["MeterLastDone"].ToString(); }
                catch { missing.Append("MeterLastDone "); }

                try { reader["MeterLastDue"].ToString(); }
                catch { missing.Append("MeterLastDue "); }

                try { reader["MeterLastReading"].ToString(); }
                catch { missing.Append("MeterLastReading "); }

                try { reader["MeterLowLevel"].ToString(); }
                catch { missing.Append("MeterLowLevel "); }

                try { reader["MeterMethod"].ToString(); }
                catch { missing.Append("MeterMethod "); }

                try { reader["MeterOn"].ToString(); }
                catch { missing.Append("MeterOn "); }

                try { reader["MeterSlack"].ToString(); }
                catch { missing.Append("MeterSlack "); }

                try { reader["NextDueDate"].ToString(); }
                catch { missing.Append("NextDueDate "); }

                try { reader["OnDemandGroup"].ToString(); }
                catch { missing.Append("OnDemandGroup "); }

                try { reader["Priority"].ToString(); }
                catch { missing.Append("Priority "); }

                try { reader["Scheduled"].ToString(); }
                catch { missing.Append("Scheduled "); }

                try { reader["ScheduleMethod"].ToString(); }
                catch { missing.Append("ScheduleMethod "); }

                try { reader["ScheduleType"].ToString(); }
                catch { missing.Append("ScheduleType "); }

                try { reader["ScheduleType_"].ToString(); }
                catch { missing.Append("ScheduleType_ "); }

                try { reader["ScheduleWeeks"].ToString(); }
                catch { missing.Append("ScheduleWeeks "); }

                try { reader["Section"].ToString(); }
                catch { missing.Append("Section "); }

                try { reader["Shift"].ToString(); }
                catch { missing.Append("Shift "); }

                try { reader["Type"].ToString(); }
                catch { missing.Append("Type "); }

                try { reader["RIMEWorkClass"].ToString(); }
                catch { missing.Append("RIMEWorkClass "); }

                try { reader["Category"].ToString(); }
                catch { missing.Append("Category "); }

                try { reader["RootCauseCode"].ToString(); }
                catch { missing.Append("RootCauseCode "); }

                try { reader["ActionCode"].ToString(); }
                catch { missing.Append("ActionCode "); }

                try { reader["FailureCode"].ToString(); }
                catch { missing.Append("FailureCode "); }

                try { reader["Planner_PersonnelId"].ToString(); }
                catch { missing.Append("Planner_PersonnelId "); }

                try { reader["PlanningRequired"].ToString(); } //V2-1161
                catch { missing.Append("PlanningRequired "); }

                try { reader["NumbersOfPMSchedAssignRecords"].ToString(); } //V2-1161
                catch { missing.Append("NumbersOfPMSchedAssignRecords "); }

                try { reader["UpdateIndex"].ToString(); }
                catch { missing.Append("UpdateIndex "); }

                try { reader["ChargeToClientLookupId"].ToString(); }
                catch { missing.Append("ChargeToClientLookupId "); }

                try { reader["AssignedTo_PersonnelClientLookupId"].ToString(); }
                catch { missing.Append("AssignedTo_PersonnelClientLookupId "); }

                try { reader["Meter_ClientLookupId"].ToString(); }
                catch { missing.Append("Meter_ClientLookupId "); }

                try { reader["WorkOrder_ClientLookupId"].ToString(); }
                catch { missing.Append("WorkOrder_ClientLookupId "); }
                
                try { reader["Planner_ClientLookupId"].ToString(); }
                catch { missing.Append("Planner_ClientLookupId "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }
                throw new Exception(msg.ToString(), ex);
            }
        }
        public void LoadFromDatabaseForSchedulingRecords(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                if ((false == reader.IsDBNull(i)))
                {
                    PrevMaintSchedId = reader.GetInt64(i);
                }
                else
                {
                    PrevMaintSchedId = 0;
                }
                i++;

                if ((false == reader.IsDBNull(i)))
                {
                    AssignedTo_PersonnelClientLookupId = reader.GetString(i);
                }
                else
                {
                    AssignedTo_PersonnelClientLookupId = "";
                }
                i++;

                if ((false == reader.IsDBNull(i)))
                {
                    ChargeToClientLookupId = reader.GetString(i);
                }
                else
                {
                    ChargeToClientLookupId = "";
                }
                i++;

                if ((false == reader.IsDBNull(i)))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = "";
                }
                i++;

                if ((false == reader.IsDBNull(i)))
                {
                    Description = reader.GetString(i);
                }
                else
                {
                    Description = "";
                }
                i++;

                if ((false == reader.IsDBNull(i)))
                {
                    ChargeToName = reader.GetString(i);
                }
                else
                {
                    ChargeToName = "";
                }
                i++;

                if ((false == reader.IsDBNull(i)))
                {
                    NextDueDate = reader.GetDateTime(i);
                }
                else
                {
                    NextDueDate = DateTime.MinValue;
                }
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["PrevMaintSchedId"].ToString(); }
                catch { missing.Append("PrevMaintSchedId "); }
                try { reader["AssignedTo_PersonnelClientLookupId"].ToString(); }
                catch { missing.Append("AssignedTo_PersonnelClientLookupId "); }

                try { reader["ChargeToClientLookupId"].ToString(); }
                catch { missing.Append("ChargeToClientLookupId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["ChargeToName"].ToString(); }
                catch { missing.Append("ChargeToName "); }

                try { reader["NextDueDate"].ToString(); }
                catch { missing.Append("NextDueDate "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }
                throw new Exception(msg.ToString(), ex);
            }
        }
        public static object ProcessRowForForeignKeys(SqlDataReader reader)
        {
            // Create instance of object
            b_PrevMaintSched obj = new b_PrevMaintSched();

            // Load the object from the database
            obj.LoadFromDatabaseForForeignKeys(reader);

            // Return result
            return (object)obj;
        }
        public static object ProcessRowForSchedulingRecords(SqlDataReader reader)
        {
            // Create instance of object
            b_PrevMaintSched obj = new b_PrevMaintSched();

            // Load the object from the database
            obj.LoadFromDatabaseForSchedulingRecords(reader);

            // Return result
            return (object)obj;
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
                Database.StoredProcedure.usp_PrevMaintSched_UpdateByPKForeignKeys.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public void UpdateByPKForeignKeysInDatabase_V2(
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
                Database.StoredProcedure.usp_PrevMaintSched_UpdateByPKForeignKeys_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public void UpdateAssignToPersonnelByPrevMaintSchedId(
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
                Database.StoredProcedure.usp_PrevMaintSched_UpdateAssignToPersonnelByPrevMaintSchedId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public void InsertByPKForeignKeysIntoDatabase(
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
                Database.StoredProcedure.usp_PrevMaintSched_CreateByPKForeignKeys.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public void InsertByPKForeignKeysIntoDatabase_V2(
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
                Database.StoredProcedure.usp_PrevMaintSched_CreateByPKForeignKeys_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public void RetrieveByPKForeignKeysFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName
        )
        {
            Database.SqlClient.ProcessRow<b_PrevMaintSched> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_PrevMaintSched>(reader => { this.LoadFromDatabaseForForeignKeys(reader); return this; });
                Database.StoredProcedure.usp_PrevMaintSched_RetrieveByPKForeignKeys.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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

        public void RetrieveByPKForeignKeysFromDatabase_V2(
    SqlConnection connection,
    SqlTransaction transaction,
    long callerUserInfoId,
    string callerUserName
)
        {
            Database.SqlClient.ProcessRow<b_PrevMaintSched> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_PrevMaintSched>(reader => { this.LoadFromDatabaseForForeignKeys_V2(reader); return this; });
                Database.StoredProcedure.usp_PrevMaintSched_RetrieveByPKForeignKeys_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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

        public void ValidateProcessFromDatabase(
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
            data = new List<b_StoredProcValidationError>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_PrevMaintSched_ValidateInsert.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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


        public bool HasColumn(SqlDataReader r, string columnName)
        {
            try
            {
                return r.GetOrdinal(columnName) >= 0;
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
        }

        //V2-739//
        public void RetrievePMCalendarForecastFromPrevMaintLibrary_ConsiderExcludeDOW(
      SqlConnection connection,
      SqlTransaction transaction,
      long callerUserInfoId,
      string callerUserName,
      ref List<b_PrevMaintSched> data ,ref string timeoutError)
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_PrevMaintSched> results = null;
            data = new List<b_PrevMaintSched>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_PrevMaintSched_CalendarForecastFromPrevMaintLibrary_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_PrevMaintSched>();
                }
            }
            catch (OperationCanceledException)
            {
                timeoutError = "Timeout";
            }
            catch (InvalidOperationException)
            {
                timeoutError = "Timeout";
            }
            catch (SqlException ex)
            {
                if (ex.Number == -2) // -2 represents TimeoutExpired error
                {
                    // Handle SQL timeout exception
                    timeoutError = "Timeout";
                }
                else
                {
                    // Handle other SQL exceptions
                    timeoutError = "SQL Exception: {ex.Message}";
                }
            }
            catch (Exception ex)
            {
                timeoutError = "An error occurred: {ex.Message}";
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
        #region V2-712
        public static object ProcessRowForForeignKeys_V2(SqlDataReader reader)
        {
            // Create instance of object
            b_PrevMaintSched obj = new b_PrevMaintSched();

            // Load the object from the database
            obj.LoadFromDatabaseForForeignKeysforPrevMaint_V2(reader);

            // Return result
            return (object)obj;
        }
        public void LoadFromDatabaseForForeignKeysforPrevMaint_V2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // PrevMaintSchedId column, bigint, not null
                PrevMaintSchedId = reader.GetInt64(i++);

                // PrevMaintMasterId column, bigint, not null
                PrevMaintMasterId = reader.GetInt64(i++);

                // AssignedTo_PersonnelId column, bigint, not null
                AssignedTo_PersonnelId = reader.GetInt64(i++);

                // AssociationGroup column, nvarchar(15), not null
                AssociationGroup = reader.GetString(i++);

                // CalendarSlack column, int, not null
                CalendarSlack = reader.GetInt32(i++);

                // ChargeToId column, bigint, not null
                ChargeToId = reader.GetInt64(i++);

                // ChargeType column, nvarchar(15), not null
                ChargeType = reader.GetString(i++);

                ChargeToName = reader.GetString(i++);

                // Crew column, nvarchar(15), not null
                Crew = reader.GetString(i++);

                // CurrentWOComplete column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    CurrentWOComplete = reader.GetDateTime(i);
                }
                else
                {
                    CurrentWOComplete = DateTime.MinValue;
                }
                i++;
                // DownRequired column, bit, not null
                DownRequired = reader.GetBoolean(i++);

                // ExcludeDOW column, nvarchar(7), not null
                ExcludeDOW = reader.GetString(i++);

                // Frequency column, int, not null
                Frequency = reader.GetInt32(i++);

                // FrequencyType column, nvarchar(15), not null
                FrequencyType = reader.GetString(i++);

                // InactiveFlag column, bit, not null
                InactiveFlag = reader.GetBoolean(i++);

                // JobPlan column, nvarchar(MAX), not null
                JobPlan = reader.GetString(i++);

                // Last_WorkOrderId column, bigint, not null
                Last_WorkOrderId = reader.GetInt64(i++);

                // LastPerformed column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    LastPerformed = reader.GetDateTime(i);
                }
                else
                {
                    LastPerformed = DateTime.MinValue;
                }
                i++;
                // LastScheduled column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    LastScheduled = reader.GetDateTime(i);
                }
                else
                {
                    LastScheduled = DateTime.MinValue;
                }
                i++;
                // MeterHighLevel column, decimal(17,3), not null
                MeterHighLevel = reader.GetDecimal(i++);

                // MeterId column, bigint, not null
                MeterId = reader.GetInt64(i++);

                // MeterInterval column, decimal(17,3), not null
                MeterInterval = reader.GetDecimal(i++);

                // MeterLastDone column, decimal(17,3), not null
                MeterLastDone = reader.GetDecimal(i++);

                // MeterLastDue column, decimal(17,3), not null
                MeterLastDue = reader.GetDecimal(i++);

                // MeterLastReading column, decimal(17,3), not null
                MeterLastReading = reader.GetDecimal(i++);

                // MeterLowLevel column, decimal(17,3), not null
                MeterLowLevel = reader.GetDecimal(i++);

                // MeterMethod column, nvarchar(1), not null
                MeterMethod = reader.GetString(i++);

                // MeterOn column, bit, not null
                MeterOn = reader.GetBoolean(i++);

                // MeterSlack column, decimal(17,3), not null
                MeterSlack = reader.GetDecimal(i++);

                // NextDueDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    NextDueDate = reader.GetDateTime(i);
                }
                else
                {
                    NextDueDate = DateTime.MinValue;
                }
                i++;
                // OnDemandGroup column, nvarchar(15), not null
                OnDemandGroup = reader.GetString(i++);

                // Priority column, nvarchar(15), not null
                Priority = reader.GetString(i++);

                // Scheduled column, bit, not null
                Scheduled = reader.GetBoolean(i++);

                // ScheduleMethod column, nvarchar(15), not null
                ScheduleMethod = reader.GetString(i++);

                // ScheduleType column, nvarchar(15), not null
                ScheduleType = reader.GetString(i++);

                // ScheduleType_ column, nvarchar(15), not null
                ScheduleType_ = reader.GetString(i++);

                // ScheduleWeeks column, nvarchar(52), not null
                ScheduleWeeks = reader.GetString(i++);

                // Section column, nvarchar(15), not null
                Section = reader.GetString(i++);

                // Shift column, nvarchar(15), not null
                Shift = reader.GetString(i++);

                // Type column, nvarchar(15), not null
                Type = reader.GetString(i++);

                // RIMEWorkClass column, int, not null
                RIMEWorkClass = reader.GetInt32(i++);

                // Category column, nvarchar(15), not null
                Category = reader.GetString(i++);

                // PlanningRequired column, bit, not null V2-1161
                PlanningRequired = reader.GetBoolean(i++);

                // UpdateIndex column, int, not null
                UpdateIndex = reader.GetInt32(i++);
                if ((false == reader.IsDBNull(i)))
                {
                    ChargeToClientLookupId = reader.GetString(i);
                }
                else
                {
                    ChargeToClientLookupId = "";
                }
                i++;

                if ((false == reader.IsDBNull(i)))
                {
                    AssignedTo_PersonnelClientLookupId = reader.GetString(i);
                }
                else
                {
                    AssignedTo_PersonnelClientLookupId = "";
                }
                i++;

                if ((false == reader.IsDBNull(i)))
                {
                    Meter_ClientLookupId = reader.GetString(i);
                }
                else
                {
                    Meter_ClientLookupId = "";
                }
                i++;

                if ((false == reader.IsDBNull(i)))
                {
                    WorkOrder_ClientLookupId = reader.GetString(i);
                }
                else
                {
                    WorkOrder_ClientLookupId = "";
                }
                i++;
                ChildCount = reader.GetInt32(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["PrevMaintSchedId"].ToString(); }
                catch { missing.Append("PrevMaintSchedId "); }

                try { reader["PrevMaintMasterId"].ToString(); }
                catch { missing.Append("PrevMaintMasterId "); }

                try { reader["AssignedTo_PersonnelId"].ToString(); }
                catch { missing.Append("AssignedTo_PersonnelId "); }

                try { reader["AssociationGroup"].ToString(); }
                catch { missing.Append("AssociationGroup "); }

                try { reader["CalendarSlack"].ToString(); }
                catch { missing.Append("CalendarSlack "); }

                try { reader["ChargeToId"].ToString(); }
                catch { missing.Append("ChargeToId "); }

                try { reader["ChargeType"].ToString(); }
                catch { missing.Append("ChargeType "); }

                try { reader["Crew"].ToString(); }
                catch { missing.Append("Crew "); }

                try { reader["CurrentWOComplete"].ToString(); }
                catch { missing.Append("CurrentWOComplete "); }

                try { reader["DownRequired"].ToString(); }
                catch { missing.Append("DownRequired "); }

                try { reader["ExcludeDOW"].ToString(); }
                catch { missing.Append("ExcludeDOW "); }

                try { reader["Frequency"].ToString(); }
                catch { missing.Append("Frequency "); }

                try { reader["FrequencyType"].ToString(); }
                catch { missing.Append("FrequencyType "); }

                try { reader["InactiveFlag"].ToString(); }
                catch { missing.Append("InactiveFlag "); }

                try { reader["JobPlan"].ToString(); }
                catch { missing.Append("JobPlan "); }

                try { reader["Last_WorkOrderId"].ToString(); }
                catch { missing.Append("Last_WorkOrderId "); }

                try { reader["LastPerformed"].ToString(); }
                catch { missing.Append("LastPerformed "); }

                try { reader["LastScheduled"].ToString(); }
                catch { missing.Append("LastScheduled "); }

                try { reader["MeterHighLevel"].ToString(); }
                catch { missing.Append("MeterHighLevel "); }

                try { reader["MeterId"].ToString(); }
                catch { missing.Append("MeterId "); }

                try { reader["MeterInterval"].ToString(); }
                catch { missing.Append("MeterInterval "); }

                try { reader["MeterLastDone"].ToString(); }
                catch { missing.Append("MeterLastDone "); }

                try { reader["MeterLastDue"].ToString(); }
                catch { missing.Append("MeterLastDue "); }

                try { reader["MeterLastReading"].ToString(); }
                catch { missing.Append("MeterLastReading "); }

                try { reader["MeterLowLevel"].ToString(); }
                catch { missing.Append("MeterLowLevel "); }

                try { reader["MeterMethod"].ToString(); }
                catch { missing.Append("MeterMethod "); }

                try { reader["MeterOn"].ToString(); }
                catch { missing.Append("MeterOn "); }

                try { reader["MeterSlack"].ToString(); }
                catch { missing.Append("MeterSlack "); }

                try { reader["NextDueDate"].ToString(); }
                catch { missing.Append("NextDueDate "); }

                try { reader["OnDemandGroup"].ToString(); }
                catch { missing.Append("OnDemandGroup "); }

                try { reader["Priority"].ToString(); }
                catch { missing.Append("Priority "); }

                try { reader["Scheduled"].ToString(); }
                catch { missing.Append("Scheduled "); }

                try { reader["ScheduleMethod"].ToString(); }
                catch { missing.Append("ScheduleMethod "); }

                try { reader["ScheduleType"].ToString(); }
                catch { missing.Append("ScheduleType "); }

                try { reader["ScheduleType_"].ToString(); }
                catch { missing.Append("ScheduleType_ "); }

                try { reader["ScheduleWeeks"].ToString(); }
                catch { missing.Append("ScheduleWeeks "); }

                try { reader["Section"].ToString(); }
                catch { missing.Append("Section "); }

                try { reader["Shift"].ToString(); }
                catch { missing.Append("Shift "); }

                try { reader["Type"].ToString(); }
                catch { missing.Append("Type "); }

                try { reader["RIMEWorkClass"].ToString(); }
                catch { missing.Append("RIMEWorkClass "); }

                try { reader["Category"].ToString(); }
                catch { missing.Append("Category "); }

                try { reader["PlanningRequired"].ToString(); }
                catch { missing.Append("PlanningRequired "); }

                try { reader["UpdateIndex"].ToString(); }
                catch { missing.Append("UpdateIndex "); }

                try { reader["ChargeToClientLookupId"].ToString(); }
                catch { missing.Append("ChargeToClientLookupId "); }

                try { reader["AssignedTo_PersonnelClientLookupId"].ToString(); }
                catch { missing.Append("AssignedTo_PersonnelClientLookupId "); }

                try { reader["Meter_ClientLookupId"].ToString(); }
                catch { missing.Append("Meter_ClientLookupId "); }

                try { reader["WorkOrder_ClientLookupId"].ToString(); }
                catch { missing.Append("WorkOrder_ClientLookupId"); }

                try { reader["ChildCount"].ToString(); }
                catch { missing.Append("ChildCount "); }


                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }
                throw new Exception(msg.ToString(), ex);
            }
        }

        public static object ProcessRowExtendedForOnDemandForecast_V2(SqlDataReader reader)
        {
            // Create instance of object
            b_PrevMaintSched obj = new b_PrevMaintSched();

            // Load the object from the database
            obj.LoadFromDatabaseExtendedForOnDemandForecast_V2(reader);

            // Return result
            return (object)obj;
        }
        public void LoadFromDatabaseExtendedForOnDemandForecast_V2(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                PMForeCastId = reader.GetInt32(i++);

                ClientId = reader.GetInt64(i++);

                SiteId = reader.GetInt64(i++);

                // Client lookup ID (From PM )
                ClientLookupId = reader.GetString(i++);

              
                if (false == reader.IsDBNull(i))
                {
                    AssignedTo_PersonnelName = reader.GetString(i);
                }
                else
                {
                    AssignedTo_PersonnelName = string.Empty;
                }
                i++;

                SchedueledDate = reader.GetDateTime(i++);

                RequiredDate = reader.GetDateTime(i++);

                // Description (From PM )
                Description = reader.GetString(i++);

                // Charge To Client Lookup Id
                if (false == reader.IsDBNull(i))
                {
                    ChargeToClientLookupId = reader.GetString(i);
                }
                else
                {
                    ChargeToClientLookupId = string.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    ChargeToName = reader.GetString(i);
                }
                else
                {
                    ChargeToName = string.Empty;
                }
                i++;

                Duration = reader.GetDecimal(i++);

                EstLaborHours = reader.GetDecimal(i++);

                EstLaborCost = reader.GetDecimal(i++);

                EstMaterialCost = reader.GetDecimal(i++);

                EstOtherCost = reader.GetDecimal(i++);

                PrevMaintSchedId = reader.GetInt64(i++);

                ChildCount = reader.GetInt32(i++);
                if (false == reader.IsDBNull(i))
                {
                    AssignedMultiple = reader.GetString(i);
                }
                else
                {
                    AssignedMultiple = string.Empty;
                }
                i++;
                Shift = reader.GetString(i++);
                DownRequired = reader.GetBoolean(i++);
                Type = reader.GetString(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["PMForeCastId"].ToString(); }
                catch { missing.Append("PMForeCastId "); }

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["AssignedTo_PersonnelName"].ToString(); }
                catch { missing.Append("AssignedTo_PersonnelName "); }

                try { reader["SchedueledDate"].ToString(); }
                catch { missing.Append("SchedueledDate "); }

                try { reader["RequiredDate"].ToString(); }
                catch { missing.Append("RequiredDate "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["ChargeToClientLookupId"].ToString(); }
                catch { missing.Append("ChargeToClientLookupId "); }

                try { reader["ChargeToName"].ToString(); }
                catch { missing.Append("ChargeToName "); }

                try { reader["Duration"].ToString(); }
                catch { missing.Append("Duration "); }

                try { reader["EstLaborHours"].ToString(); }
                catch { missing.Append("EstLaborHours "); }

                try { reader["EstLaborCost"].ToString(); }
                catch { missing.Append("EstLaborCost "); }

                try { reader["EstMaterialCost"].ToString(); }
                catch { missing.Append("EstMaterialCost "); }

                try { reader["EstOtherCost"].ToString(); }
                catch { missing.Append("EstOtherCost "); }

                try { reader["PrevMaintSchedId"].ToString(); }
                catch { missing.Append("PrevMaintSchedId "); }

                try { reader["AssignedMultiple"].ToString(); }
                catch { missing.Append("AssignedMultiple "); }

                try { reader["Shift"].ToString(); }
                catch { missing.Append("Shift "); }

                try { reader["DownRquired"].ToString(); }
                catch { missing.Append("DownRequired "); }

                try { reader["Type"].ToString(); }
                catch { missing.Append("Type "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }

        public void RetrievePMOnDemandForecastFromPrevMaintLibrary_V2(
 SqlConnection connection,
 SqlTransaction transaction,
 long callerUserInfoId,
 string callerUserName,
 ref List<b_PrevMaintSched> data,ref string timeoutError
 )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_PrevMaintSched> results = null;
            data = new List<b_PrevMaintSched>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_Prevmaintsched_OnDemandForecastFromPrevMaintLibrary_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_PrevMaintSched>();
                }
            }
            catch (OperationCanceledException)
            {
                timeoutError = "Timeout";
            }
            catch (InvalidOperationException)
            {
                timeoutError = "Timeout";
            }
            catch (SqlException ex)
            {
                if (ex.Number == -2) // -2 represents TimeoutExpired error
                {
                    // Handle SQL timeout exception
                    timeoutError = "Timeout";
                }
                else
                {
                    // Handle other SQL exceptions
                    timeoutError = "SQL Exception: {ex.Message}";
                }
            }
            catch (Exception ex)
            {
                timeoutError = "An error occurred: {ex.Message}";
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
        public static object ProcessRowExtendedForCalenderForecast_V2(SqlDataReader reader)
        {
            // Create instance of object
            b_PrevMaintSched obj = new b_PrevMaintSched();

            // Load the object from the database
            obj.LoadFromDatabaseExtendedForForecast_V2(reader);

            // Return result
            return (object)obj;
        }
        public void LoadFromDatabaseExtendedForForecast_V2(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                PMForeCastId = reader.GetInt32(i++);

                ClientId = reader.GetInt64(i++);

                SiteId = reader.GetInt64(i++);

                // Client lookup ID (From PM )
                ClientLookupId = reader.GetString(i++);

                
               // assigned to personnel name
                if (false == reader.IsDBNull(i))
                {
                    AssignedTo_PersonnelName = reader.GetString(i);
                }
                else
                {
                    AssignedTo_PersonnelName = string.Empty;
                }
                i++;

                SchedueledDate = reader.GetDateTime(i++);

                RequiredDate = reader.GetDateTime(i++);

                // Description (From PM )
                Description = reader.GetString(i++);

                // Charge To Client Lookup Id
                if (false == reader.IsDBNull(i))
                {
                    ChargeToClientLookupId = reader.GetString(i);
                }
                else
                {
                    ChargeToClientLookupId = string.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    ChargeToName = reader.GetString(i);
                }
                else
                {
                    ChargeToName = string.Empty;
                }
                i++;

                Duration = reader.GetDecimal(i++);

                EstLaborHours = reader.GetDecimal(i++);

                EstLaborCost = reader.GetDecimal(i++);

                EstMaterialCost = reader.GetDecimal(i++);

                EstOtherCost = reader.GetDecimal(i++);

                PrevMaintSchedId = reader.GetInt64(i++);

                ChildCount = reader.GetInt32(i++);

                if (false == reader.IsDBNull(i))
                {
                    AssignedMultiple = reader.GetString(i);
                }
                else
                {
                    AssignedMultiple = string.Empty;
                }
                i++;
                Shift = reader.GetString(i++);
                DownRequired = reader.GetBoolean(i++);
                //V2-1207
                if (false == reader.IsDBNull(i))
                {
                    Type = reader.GetString(i);
                }
                else
                {
                    Type = string.Empty;
                }
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["PMForeCastId"].ToString(); }
                catch { missing.Append("PMForeCastId "); }

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["AssignedTo_PersonnelName"].ToString(); }
                catch { missing.Append("AssignedTo_PersonnelName "); }

                try { reader["SchedueledDate"].ToString(); }
                catch { missing.Append("SchedueledDate "); }

                try { reader["RequiredDate"].ToString(); }
                catch { missing.Append("RequiredDate "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["ChargeToClientLookupId"].ToString(); }
                catch { missing.Append("ChargeToClientLookupId "); }

                try { reader["ChargeToName"].ToString(); }
                catch { missing.Append("ChargeToName "); }

                try { reader["Duration"].ToString(); }
                catch { missing.Append("Duration "); }

                try { reader["EstLaborHours"].ToString(); }
                catch { missing.Append("EstLaborHours "); }

                try { reader["EstLaborCost"].ToString(); }
                catch { missing.Append("EstLaborCost "); }

                try { reader["EstMaterialCost"].ToString(); }
                catch { missing.Append("EstMaterialCost "); }

                try { reader["EstOtherCost"].ToString(); }
                catch { missing.Append("EstOtherCost "); }

                try { reader["PrevMaintSchedId"].ToString(); }
                catch { missing.Append("PrevMaintSchedId "); }

                try { reader["ChildCount"].ToString(); }
                catch { missing.Append("ChildCount "); }

                try { reader["AssignedMultiple"].ToString(); }
                catch { missing.Append("AssignedMultiple "); }

                try { reader["Shift"].ToString(); }
                catch { missing.Append("Shift "); }

                try { reader["DownRequired"].ToString(); }
                catch { missing.Append("DownRequired "); }

                try { reader["Type"].ToString(); }
                catch { missing.Append("Type "); }
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

        #region V2-977
        public static object ProcessRowForSchedulingRecordsForMultipleAssignment(SqlDataReader reader)
        {
            // Create instance of object
            b_PrevMaintSched obj = new b_PrevMaintSched();

            // Load the object from the database
            obj.LoadFromDatabaseForSchedulingRecordsForMultipleAssignment(reader);

            // Return result
            return (object)obj;
        }

        public void LoadFromDatabaseForSchedulingRecordsForMultipleAssignment(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                if ((false == reader.IsDBNull(i)))
                {
                    PrevMaintSchedId = reader.GetInt64(i);
                }
                else
                {
                    PrevMaintSchedId = 0;
                }
                i++;

                if ((false == reader.IsDBNull(i)))
                {
                    AssignedTo_PersonnelClientLookupId = reader.GetString(i);
                }
                else
                {
                    AssignedTo_PersonnelClientLookupId = "";
                }
                i++;

                if ((false == reader.IsDBNull(i)))
                {
                    ChargeToClientLookupId = reader.GetString(i);
                }
                else
                {
                    ChargeToClientLookupId = "";
                }
                i++;

                if ((false == reader.IsDBNull(i)))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = "";
                }
                i++;

                if ((false == reader.IsDBNull(i)))
                {
                    Description = reader.GetString(i);
                }
                else
                {
                    Description = "";
                }
                i++;

                if ((false == reader.IsDBNull(i)))
                {
                    ChargeToName = reader.GetString(i);
                }
                else
                {
                    ChargeToName = "";
                }
                i++;

                if ((false == reader.IsDBNull(i)))
                {
                    NextDueDate = reader.GetDateTime(i);
                }
                else
                {
                    NextDueDate = DateTime.MinValue;
                }
                i++;
                #region V2-977
                if ((false == reader.IsDBNull(i)))
                {
                    PMSAssignId = reader.GetInt64(i);
                }
                else
                {
                    PMSAssignId = 0;
                }
                i++;
                if ((false == reader.IsDBNull(i)))
                {
                    IndexId = reader.GetInt64(i);
                }
                else
                {
                    IndexId = 0;
                }
                i++;
                //******
                TotalCount = reader.GetInt32(i++);
                #endregion
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["PrevMaintSchedId"].ToString(); }
                catch { missing.Append("PrevMaintSchedId "); }
                try { reader["AssignedTo_PersonnelClientLookupId"].ToString(); }
                catch { missing.Append("AssignedTo_PersonnelClientLookupId "); }

                try { reader["ChargeToClientLookupId"].ToString(); }
                catch { missing.Append("ChargeToClientLookupId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["ChargeToName"].ToString(); }
                catch { missing.Append("ChargeToName "); }

                try { reader["NextDueDate"].ToString(); }
                catch { missing.Append("NextDueDate "); }

                try { reader["PMSAssignId"].ToString(); }
                catch { missing.Append("PMSAssignId "); }

                try { reader["IndexId"].ToString(); }
                catch { missing.Append("IndexId "); }

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

        #region V2-1005
        public void PrevMaintSchedDelete_V2(
   SqlConnection connection,
      SqlTransaction transaction,
      long callerUserInfoId,
  string callerUserName
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
                Database.StoredProcedure.usp_PrevMaintSched_Delete_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        #endregion

        #region
        public void RetrieveByPrevMaintMasterIdChunckSearchFromDatabase_V2(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            ref List<b_PrevMaintSched> data
        )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_PrevMaintSched> results = null;
            data = new List<b_PrevMaintSched>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_PrevMaintSched_RetrieveByPrevMaintMasterIdChunkSearch_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_PrevMaintSched>();
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
        public static object ProcessRowForRetrieveByPrevMaintMasterIdForChunckSearch_V2(SqlDataReader reader)
        {
            // Create instance of object
            b_PrevMaintSched obj = new b_PrevMaintSched();

            // Load the object from the database
            obj.LoadFromDatabaseForRetrieveByPrevMaintMasterIdForChunckSearch_V2(reader);

            // Return result
            return (object)obj;
        }
        public void LoadFromDatabaseForRetrieveByPrevMaintMasterIdForChunckSearch_V2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // PrevMaintSchedId column, bigint, not null
                PrevMaintSchedId = reader.GetInt64(i++);

                // PrevMaintMasterId column, bigint, not null
                PrevMaintMasterId = reader.GetInt64(i++);

                // ChargeToClientLookupId column, nvarchar(31), not null
                ChargeToClientLookupId = reader.GetString(i++);

                ChargeToName = reader.GetString(i++);

                Frequency = reader.GetInt32(i++);

                FrequencyType = reader.GetString(i++);

                // NextDueDate column, datetime2
                if (false == reader.IsDBNull(i))
                {
                    NextDueDate = reader.GetDateTime(i);
                }
                else
                {
                    NextDueDate = DateTime.MinValue;
                }
                i++;

                if ((false == reader.IsDBNull(i)))
                {
                    WorkOrder_ClientLookupId = reader.GetString(i);
                }
                else
                {
                    WorkOrder_ClientLookupId = "";
                }
                i++;

                // LastScheduled column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    LastScheduled = reader.GetDateTime(i);
                }
                else
                {
                    LastScheduled = DateTime.MinValue;
                }
                i++;

                // LastPerformed column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    LastPerformed = reader.GetDateTime(i);
                }
                else
                {
                    LastPerformed = DateTime.MinValue;
                }
                i++;

                if ((false == reader.IsDBNull(i)))
                {
                    Meter_ClientLookupId = reader.GetString(i);
                }
                else
                {
                    Meter_ClientLookupId = "";
                }
                i++;

                // OnDemandGroup column, nvarchar(15), not null
                OnDemandGroup = reader.GetString(i++);

                // PlanningRequired column, bit, not null
                PlanningRequired = reader.GetBoolean(i++);

                ChildCount = reader.GetInt32(i++);

                TotalCount = reader.GetInt32(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["PrevMaintSchedId"].ToString(); }
                catch { missing.Append("PrevMaintSchedId "); }

                try { reader["PrevMaintMasterId"].ToString(); }
                catch { missing.Append("PrevMaintMasterId "); }

                try { reader["ChargeToClientLookupId"].ToString(); }
                catch { missing.Append("ChargeToClientLookupId "); }

                try { reader["ChargeToName"].ToString(); }
                catch { missing.Append("ChargeToName "); }

                try { reader["Frequency"].ToString(); }
                catch { missing.Append("Frequency "); }
                
                try { reader["FrequencyType"].ToString(); }
                catch { missing.Append("FrequencyType "); }

                try { reader["NextDueDate"].ToString(); }
                catch { missing.Append("NextDueDate "); }

                try { reader["WorkOrder_ClientLookupId"].ToString(); }
                catch { missing.Append("WorkOrder_ClientLookupId "); }

                try { reader["LastScheduled"].ToString(); }
                catch { missing.Append("LastScheduled "); }

                try { reader["LastPerformed"].ToString(); }
                catch { missing.Append("LastPerformed "); }

                try { reader["Meter_ClientLookupId"].ToString(); }
                catch { missing.Append("Meter_ClientLookupId "); }

                try { reader["OnDemandGroup"].ToString(); }
                catch { missing.Append("OnDemandGroup "); }

                try { reader["PlanningRequired"].ToString(); }
                catch { missing.Append("PlanningRequired "); }

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
        }
        #endregion
    }
}
