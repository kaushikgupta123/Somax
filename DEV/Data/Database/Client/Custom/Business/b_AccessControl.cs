/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2018 by SOMAX Inc.
* PreventiveMaintenanceDetails.aspx.cs
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID  Person             Description
* =========== ======== ================== =========================================================

****************************************************************************************************
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
    /// 
    [Serializable()]
    public partial class b_AccessControl
    {
        #region  Properties
        public Int64 ClientId { get; set; }
        public Int64 AccessControlId { get; set; }
        public Int64 SiteId { get; set; }
        public Int64 UserInfoId { get; set; }
        public Int64 PersonnelId { get; set; }
        public string ControlType { get; set; }
        public bool Enabled { get; set; }
        public bool IsAuthorized { get; set; }

        #endregion

        public void IsUserAuthorized(
         SqlConnection connection,
         SqlTransaction transaction,
         long callerUserInfoId,
         string callerUserName,
         ref b_AccessControl results
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
                results = Database.StoredProcedure.usp_AccessControl_IsUserAuthorized.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public static b_AccessControl ProcessRowForIsUserAuthorized(SqlDataReader reader)
        {
            // Create instance of object
            b_AccessControl ac = new b_AccessControl();

            // Load the object from the database
            ac.LoadFromDatabaseForIsUserAuthorized(reader);

            // Return result
            return ac;
        }
        public void LoadFromDatabaseForIsUserAuthorized(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // ClientId column, bigint, not null              
                if (false == reader.IsDBNull(i))
                {
                    ClientId = reader.GetInt64(i);
                }
                else
                {
                    ClientId = 0;
                }
                i++;

                // AccessControlId column, bigint, not null              
                if (false == reader.IsDBNull(i))
                {
                    AccessControlId = reader.GetInt64(i);
                }
                else
                {
                    AccessControlId = 0;
                }
                i++;

                // SiteId column, bigint, not null               
                if (false == reader.IsDBNull(i))
                {
                    SiteId = reader.GetInt64(i);
                }
                else
                {
                    SiteId = 0;
                }
                i++;

                // UserInfoId column, bigint, not null
                if (false == reader.IsDBNull(i))
                {
                    UserInfoId = reader.GetInt64(i);
                }
                else
                {
                    UserInfoId = 0;
                }
                i++;

                // PersonnelId column, bigint, not null
                if (false == reader.IsDBNull(i))
                {
                    PersonnelId = reader.GetInt64(i);
                }
                else
                {
                    PersonnelId = 0;
                }
                i++;

                // ControlType column, string , not empty
                if (false == reader.IsDBNull(i))
                {
                    ControlType = reader.GetString(i);
                }
                else
                {
                    ControlType = string.Empty;
                }
                i++;

                // Enabled column, string , not empty
                if (false == reader.IsDBNull(i))
                {
                    Enabled = reader.GetBoolean(i);
                }
                else
                {
                    Enabled = false;
                }
                i++;

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["AccessControlId"].ToString(); }
                catch { missing.Append("AccessControlId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["UserInfoId"].ToString(); }
                catch { missing.Append("UserInfoId "); }

                try { reader["PersonnelId"].ToString(); }
                catch { missing.Append("PersonnelId "); }

                try { reader["ControlType"].ToString(); }
                catch { missing.Append("ControlType "); }

                try { reader["Enabled"].ToString(); }
                catch { missing.Append("Enabled "); }


                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }

    }
}

/*****************************End Added By Indusnet Technologies*****************************/






