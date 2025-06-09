/*
 ******************************************************************************
 * PROPRIETARY DATA 
 **************************************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 **************************************************************************************************
 * Copyright (c) 2011 by SOMAX Inc.
 * All rights reserved. 
 **************************************************************************************************
 * Date        Task ID   Person             Description
 * =========== ======== =================== =======================================================
 * 2014-Aug-28 SOM-304  Roger Lawton        Modified LoadFromDatabaseForSearchCriteria
 * 2020-Apr-06 SOM-1737 Roger Lawton        Copy from PM Library instead of Reference
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
    /// Business object thay>t stores a record from the PrevMaintMaster table.
    /// </summar
    public partial class b_PrevMaintMaster
    {
        #region Property
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }

        public Int32  CaseNo { get; set; }
        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public Int32 offset1 { get; set; }
        public Int32 nextrow { get; set; }
        public string Chargeto { get; set; }
        public string ChargetoName { get; set; }
        public Int32 FilterType { get; set; }
        public Int64 FilterValue { get; set; }
        public string SearchText { get; set; }
        public Int32 UpdateIndexOut { get; set; }
        public Int32 TotalCount { get; set; }
        public Int32 ChildCount { get; set; }

        #endregion
        public static b_PrevMaintMaster ProcessRowForChunkSearch(SqlDataReader reader)
        {
            // Create instance of object
            b_PrevMaintMaster obj = new b_PrevMaintMaster();

            // Load the object from the database
            obj.LoadFromDatabaseForChunkSearch(reader);

            // Return result
            return obj;
        }
        public static b_PrevMaintMaster ProcessRowForSearchCriteria(SqlDataReader reader)
        {
            // Create instance of object
            b_PrevMaintMaster obj = new b_PrevMaintMaster();

            // Load the object from the database
            obj.LoadFromDatabaseForSearchCriteria(reader);

            // Return result
            return obj;
        }

        public void LoadFromDatabaseForSearchCriteria(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // PrevMaintMasterId column, bigint, not null
                PrevMaintMasterId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);

                // Description column, nvarchar(255), not null
                Description = reader.GetString(i++);

                // Scheduled Type, nvarchar(15), not null
                ScheduleType = reader.GetString(i++);

                // Inactive Flag
                InactiveFlag = reader.GetBoolean(i++);

                //JobDuration-----------
                JobDuration = reader.GetDecimal(i++);

                // Scheduled , nvarchar(15), not null
                Type = reader.GetString(i++);

                // PrevMaintLibraryId , Int64, 0
                PrevMaintLibraryId = reader.GetInt64(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["PrevMaintMasterId"].ToString(); }
                catch { missing.Append("PrevMaintMasterId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["ScheduleType"].ToString(); }
                catch { missing.Append("ScheduleType "); }

                try { reader["InactiveFlag"].ToString(); }
                catch { missing.Append("InactiveFlag "); }

                try { reader["JobDuration"].ToString(); }
                catch { missing.Append("JobDuration "); }

                try { reader["Type"].ToString(); }
                catch { missing.Append("Type "); }

                try { reader["PrevMaintLibraryId"].ToString(); }
                catch { missing.Append("PrevMaintLibraryId "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }

        public void LoadFromDatabaseForChunkSearch(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // PrevMaintMasterId column, bigint, not null
                PrevMaintMasterId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);

                // Description column, nvarchar(255), not null
                Description = reader.GetString(i++);

                // Scheduled Type, nvarchar(15), not null
                ScheduleType = reader.GetString(i++);

                // Inactive Flag
                InactiveFlag = reader.GetBoolean(i++);

                //JobDuration-----------
                JobDuration = reader.GetDecimal(i++);

                // Scheduled , nvarchar(15), not null
                Type = reader.GetString(i++);

                // PrevMaintLibraryId , Int64, 0
                PrevMaintLibraryId = reader.GetInt64(i++);

                //ChildCount
                ChildCount = reader.GetInt32(i++);

                //TotalCount
                TotalCount = reader.GetInt32(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["PrevMaintMasterId"].ToString(); }
                catch { missing.Append("PrevMaintMasterId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["ScheduleType"].ToString(); }
                catch { missing.Append("ScheduleType "); }

                try { reader["InactiveFlag"].ToString(); }
                catch { missing.Append("InactiveFlag "); }

                try { reader["JobDuration"].ToString(); }
                catch { missing.Append("JobDuration "); }

                try { reader["Type"].ToString(); }
                catch { missing.Append("Type "); }

                try { reader["PrevMaintLibraryId"].ToString(); }
                catch { missing.Append("PrevMaintLibraryId "); }

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

        /* RKL - 2014-Aug-31 - This is not used anywhere
        public void RetrievePreventiveMaintenanceBySearchCriteriaFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            int ClientId,
            int SiteId,
            int FilterType,
            int FilterValue,
            ref List<b_PrevMaintMaster> data
        )
        {

          SqlCommand command = null;
          string message = String.Empty;
          List<b_PrevMaintMaster> results = null;
          data = new List<b_PrevMaintMaster>();
          int ResultCount;

          try
          {
            // Create the command to use in calling the stored procedures
            command = new SqlCommand();
            command.Connection = connection;
            command.Transaction = transaction;


            // Call the stored procedure to retrieve the data
            results = Database.StoredProcedure.usp_PreventiveMaintenance_RetrieveToSearchCriteria.CallStoredProcedure(command, callerUserInfoId, callerUserName, 
              ClientId, SiteId, FilterType, FilterValue, out ResultCount);

            if (results != null)
            {
              data = results;
            }
            else
            {
              data = new List<b_PrevMaintMaster>();
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

        */
        public void ValidateProcessLinkFromDatabase(
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
                results = Database.StoredProcedure.usp_PrevMaintMaster_ValidateByClientLookupId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        public void ValidateProcessAddFromDatabase(
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
                results = Database.StoredProcedure.usp_PrevMaintMaster_ValidateByClientLookupId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public void DeletePreventiveMaintenanceMasterChild(SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName)
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
                Database.StoredProcedure.usp_PrevMaintMaster_Delete_Master_Childs.CallStoredProcedure(command, callerUserInfoId, callerUserName, this.PrevMaintMasterId);

            }
            finally
            {
                if (null != command)
                {
                    //command.Dispose();
                    //command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;

            }
        }
      
        public void PreventiveMaintenance_CreateFromPMLibrary(SqlConnection connection,
             SqlTransaction transaction,
             long callerUserInfoId,
             string callerUserName)
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
                Database.StoredProcedure.usp_PrevMaintMaster_CreateFromPMLibrary.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        // SOM-1737
        public void CreatePrevMaintMaster_CopyFromPMLibrary(SqlConnection connection,
             SqlTransaction transaction,
             long callerUserInfoId,
             string callerUserName)
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
            Database.StoredProcedure.usp_PrevMaintMaster_CopyFromPMLibrary.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

    public void PreventiveMaintenance_RetrieveByForeignKey(SqlConnection connection,
       SqlTransaction transaction,
       long callerUserInfoId,
       string callerUserName)
        {
            Database.SqlClient.ProcessRow<b_PrevMaintMaster> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_PrevMaintMaster>(reader => { this.LoadFromDatabaseByPKForeignKey(reader); return this; });
                Database.StoredProcedure.usp_PrevMaintMaster_RetrieveCreateModifyDetails.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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

        private void LoadFromDatabaseByPKForeignKey(SqlDataReader reader)
        {
            int i = 0;//this.LoadFromDatabaseForeignKey(reader);

            try
            {

                //- SOM: 937
                if (false == reader.IsDBNull(i))
                {
                    CreateBy = reader.GetString(i);
                }
                else
                {
                    CreateBy = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    CreateDate = reader.GetDateTime(i);
                }
                else
                {
                    CreateDate = DateTime.Now;
                }
                i++;

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


            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["CreateBy"].ToString(); }
                catch { missing.Append("CreateBy"); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate"); }

                try { reader["ModifyBy"].ToString(); }
                catch { missing.Append("ModifyBy"); }

                try { reader["ModifyDate"].ToString(); }
                catch { missing.Append("ModifyDate"); }



                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }

        public int LoadFromDatabaseForeignKey(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                CreateBy = reader.GetString(i++);
                CreateDate = reader.GetDateTime(i++);
                ModifyBy = reader.GetString(i++);
                ModifyDate = reader.GetDateTime(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["CreateBy"].ToString(); }
                catch { missing.Append("CreateBy "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["ModifyBy"].ToString(); }
                catch { missing.Append("ModifyBy "); }

                try { reader["ModifyDate"].ToString(); }
                catch { missing.Append("ModifyDate "); }

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

        public void ValidateByClientLookupIdForChange(SqlConnection connection,
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
                results = Database.StoredProcedure.usp_PreventiveMaintenance_ValidateClientLookupId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public void RetrieveChunkSearch(
    SqlConnection connection,
    SqlTransaction transaction,
    long callerUserInfoId,
    string callerUserName,
    ref List<b_PrevMaintMaster> results
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

                results = Database.StoredProcedure.usp_PreventiveMaintenance_RetrieveChunkSearch_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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


        public void UpdateForClientLookupId_V2(
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
                Database.StoredProcedure.usp_PrevMaintMaster_UpdateForClientLookupId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
    }
}
