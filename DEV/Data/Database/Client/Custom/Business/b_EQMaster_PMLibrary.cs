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
* 2014-Sep-18 SOM-106  Roger Lawton       Added 
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
    /// Business object that stores a record from the Part_Vendor_Xref table.
    /// </summary>
    public partial class b_EQMaster_PMLibrary
    {
        public string ClientLookupId { get; set; }
        public int flag { get; set; }
        public string Description { get; set; }
        public int Frequency { get; set; }
        public string FrequencyType { get; set; }

        /// <summary>
        /// Process the current row in the input SqlDataReader into a b_EQMaster_PTMaster_XRef object.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        /// <returns>object cast of the b_EQMaster_PTMaster_XRef object</returns>
        public static object ProcessRowExtended(SqlDataReader reader)
        {
            // Create instance of object
            b_EQMaster_PMLibrary obj = new b_EQMaster_PMLibrary();

            // Load the object from the database
            obj.LoadFromDatabaseExtended(reader);

            // Return result
            return (object)obj;
        }

        /// <summary>
        /// Load the current row in the input SqlDataReader into a b_Part_Vendor_Xref object.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        public void LoadFromDatabaseExtended(SqlDataReader reader)
        {
            int i = this.LoadFromDatabase(reader);
            try
            {
                ClientLookupId = reader.GetString(i++);

                Description = reader.GetString(i++);

                Frequency = reader.GetInt32(i++);

                FrequencyType = reader.GetString(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                try { reader["ClienLookupId"].ToString(); }
                catch { missing.Append("ClienLookupId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["Frequency"].ToString(); }
                catch { missing.Append("Frequency "); }

                try { reader["FrequencyType"].ToString(); }
                catch { missing.Append("FrequencyType "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }
        public void RetrieveListByEQMasterId(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, ref List<b_EQMaster_PMLibrary> data)
        {
            SqlCommand command = null;
            string message = String.Empty;
            List<b_EQMaster_PMLibrary> results = null;
            data = new List<b_EQMaster_PMLibrary>();
            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_EQMaster_PMLibrary_RetrieveByEQMasterId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_EQMaster_PMLibrary>();
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

        public void CreateByFK(
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
                Database.StoredProcedure.usp_EQMaster_PMLibrary_CreateByFK.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public void UpdateByFK(
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
                Database.StoredProcedure.usp_EQMaster_PMLibrary_UpdateByFK.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public void ValidateByClientLookupId(SqlConnection connection,
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
                results = Database.StoredProcedure.usp_EQMaster_PMLibrary_ValidateByClientLookupId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
