/*
***************************************************************************************************
* PROPRIETARY DATA 
***************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
***************************************************************************************************
* Copyright (c) 2014 by SOMAX Inc.
* All rights reserved. 
***************************************************************************************************
* Date        Task ID   Person             Description
* =========== ======== =================== ========================================================
* 2015-Mar-12 SOM-585  Roger Lawton        Added Items to support sanitation
***************************************************************************************************
*/
using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.Business
{

    public partial class b_SanitationMaster
    {
        #region  Properties
        public string ChargeToClientLookupId { get; set; }
        public string ChargeToName { get; set; }
        public string Assigned { get; set; }
        public long PlantLocationId { get; set; }

        #endregion
        public static b_SanitationMaster ProcessRowForSearchCriteria(SqlDataReader reader)
        {
            // Create instance of object
            b_SanitationMaster obj = new b_SanitationMaster();

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
                SanitationMasterId = reader.GetInt64(i++);

                //Description column, nvarchar(31), not null
                Description = reader.GetString(i++);

                //Assignto_PersonnelId column, bigint, not null
                Assignto_PersonnelId = reader.GetInt64(i++);

                //Assigned column, nvarchar(31), not null
                Assigned = reader.GetString(i++);

                //ChargeType column, nvarchar(31), not null
                ChargeType = reader.GetString(i++);

                //ChargeToId column, bigint, not null
                ChargeToId = reader.GetInt64(i++);

                //ChargeToClientLookupId column, nvarchar(31), not null
                ChargeToClientLookupId = reader.GetString(i++);

                //Frequency column, bigint, not null
                Frequency = reader.GetInt32(i++);

                //OnDemandGroup column, nvarchar(31), not null
                OnDemandGroup = reader.GetString(i++);

                //ScheduledType column, nvarchar(31), not null
                ScheduledType = reader.GetString(i++);

                // Shift column, nvarchar(255), not null
                Shift = reader.GetString(i++);

                // ScheduledDuration column, decimal, not null
                ScheduledDuration = reader.GetDecimal(i++);

                // InactiveFlag column, boolean, not null
                InactiveFlag = reader.GetBoolean(i++);

                // NextDue column, DateTime, not null

                NextDue = reader.GetDateTime(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["SanitationMasterId"].ToString(); }
                catch { missing.Append("SanitationMasterId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["Assignto_PersonnelId"].ToString(); }
                catch { missing.Append("Assignto_PersonnelId "); }

                try { reader["Assigned"].ToString(); }
                catch { missing.Append("Assigned "); }

                try { reader["ChargeType"].ToString(); }
                catch { missing.Append("ChargeType "); }

                try { reader["ChargeToId"].ToString(); }
                catch { missing.Append("ChargeToId "); }

                try { reader["ChargeToClientLookupId"].ToString(); }
                catch { missing.Append("ChargeToClientLookupId "); }

                try { reader["Frequency"].ToString(); }
                catch { missing.Append("Frequency "); }

                try { reader["OnDemandGroup"].ToString(); }
                catch { missing.Append("OnDemandGroup "); }

                try { reader["ScheduledType"].ToString(); }
                catch { missing.Append("ScheduledType "); }

                try { reader["Shift"].ToString(); }
                catch { missing.Append("Shift "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }


        public void SanitationMaster_SaveAs(SqlConnection connection,
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
                Database.StoredProcedure.usp_SanitationMaster_SaveAs.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        public void SanitationMaster_Delete(SqlConnection connection,
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
                Database.StoredProcedure.usp_Sanitation_Delete.CallStoredProcedure(command, callerUserInfoId, callerUserName, this.SanitationMasterId);

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
        // RKL
        // Not used anywhere that I can find
        /*
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
                results = Database.StoredProcedure.usp_SanitationMaster_ValidateByClientLookupId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        */
        public void SanitationMaster_CreateByFK(
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
                Database.StoredProcedure.usp_SanitationMaster_CreateByFK.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
                
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

        public  void SanitationMaster_RetrieveByFK(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName
        )
        {
            Database.SqlClient.ProcessRow<b_SanitationMaster> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_SanitationMaster>(reader => { this.LoadFromDatabaseByFk(reader); return this; });
                Database.StoredProcedure.usp_SanitationMaster_RetrieveByFK.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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
        public int LoadFromDatabaseByFk(SqlDataReader reader)
        {
            int i = this.LoadFromDatabase(reader);
            try
            {
                // ChargeToClientLookupId column, nvarchar(MAX), not null
                if (false == reader.IsDBNull(i))
                {
                    ChargeToClientLookupId = reader.GetString(i);
                }
                else
                {
                    ChargeToClientLookupId = "";
                }
                i++;
                // ChargeToName
                if (false == reader.IsDBNull(i))
                {
                  ChargeToName = reader.GetString(i);
                }
                else
                {
                  ChargeToName = "";
                }
                i++;
                //// Assigned
                //if (false == reader.IsDBNull(i))
                //{
                //  Assigned = reader.GetString(i);
                //}
                //else
                //{
                //  Assigned = "";
                //}
                //i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ChargeToClientLookupId"].ToString(); }
                catch { missing.Append("ChargeToClientLookupId "); }

                try { reader["ChargeToName"].ToString(); }
                catch { missing.Append("ChargeToName "); }

                //try { reader["Assigned"].ToString(); }
                //catch { missing.Append("Assigned "); }

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


        public void SanitationMaster_UpdateByFK(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName)
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                Database.StoredProcedure.usp_SanitationMaster_UpdateByFK.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public void ValidateSanitationMaster(
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
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;
                results = Database.StoredProcedure.usp_SanitationMaster_Validate.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
    }
}
