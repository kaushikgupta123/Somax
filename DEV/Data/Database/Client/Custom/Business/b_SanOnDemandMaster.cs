/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2012 by SOMAX Inc.
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
using System.Collections.Generic;

namespace Database.Business
{
    /// <summary>
    /// Business object that stores a record from the SanOnDemandMaster table.
    /// </summary>
    public partial class b_SanOnDemandMaster
    {
        #region Properties
        public DateTime CreateDate { get; set; }
        #endregion

        public void RetrieveAllForSearchPage(
      SqlConnection connection,
      SqlTransaction transaction,
      long callerUserInfoId,
      string callerUserName,
      ref List<b_SanOnDemandMaster> results
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

               results = Database.StoredProcedure.usp_SanOnDemandMaster_RetriveByFilterSearch.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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


        public void RetrieveAllForSearchPage_V2(
              SqlConnection connection,
              SqlTransaction transaction,
              long callerUserInfoId,
              string callerUserName,
              ref List<b_SanOnDemandMaster> results
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

                results = Database.StoredProcedure.usp_SanOnDemandMaster_RetriveByFilterSearch_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public void UpdateBy_SanOnDemandMasterId(
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
               Database.StoredProcedure.usp_SanOnDemandMaster_UpdateBy_SanOnDemandMasterId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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


        public void RetrieveBy_SanOnDemandMasterId(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
  string callerUserName
    )
        {
            Database.SqlClient.ProcessRow<b_SanOnDemandMaster> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_SanOnDemandMaster>(reader => { this.LoadFromDatabase(reader); return this; });
                Database.StoredProcedure.usp_SanOnDemandMaster_RetrieveBy_SanOnDemandMasterId.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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
        public void InsertByForeignKeysIntoDatabase(
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
                Database.StoredProcedure.usp_SanOnDemandMaster_CreateByForeignKeys.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
            data = new List<b_StoredProcValidationError>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_SanOnDemandMaster_ValidateByClientLookupId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        public static b_SanOnDemandMaster ProcessRowForSanOnDemandMasterRetriveAllForSearch(SqlDataReader reader)
        {
            // Create instance of object
            b_SanOnDemandMaster SanOnDemandMaster = new b_SanOnDemandMaster();

            SanOnDemandMaster.LoadFromDatabaseForSanOnDemandMasterRetriveAllForSearch(reader);

            return SanOnDemandMaster;
        }

        public int LoadFromDatabaseForSanOnDemandMasterRetriveAllForSearch(SqlDataReader reader)
        {
            int i = this.LoadFromDatabase(reader);
            try
            {

                // CreateDate column, DateTime2, null
                if (false == reader.IsDBNull(i))
                {
                    this.CreateDate = reader.GetDateTime(i);
                }
                else
                {
                    this.CreateDate = DateTime.MinValue;
                }
                i++;


            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

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

        public void RetrieveAllForActiveFlag(
      SqlConnection connection,
      SqlTransaction transaction,
      long callerUserInfoId,
      string callerUserName,
      ref List<b_SanOnDemandMaster> results
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

                results = Database.StoredProcedure.usp_SanOnDemandMaster_RetriveByActiveFlag.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
