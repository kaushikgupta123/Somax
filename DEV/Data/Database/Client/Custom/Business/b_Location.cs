/*
***************************************************************************************************
* PROPRIETARY DATA 
***************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
***************************************************************************************************
* Copyright (c) 2016 by SOMAX Inc.
* All rights reserved. 
***************************************************************************************************
* Date        Task ID   Person          Description
* =========== ======== ================ ===========================================================
* 2016-Aug-21 SOM-1049 Roger Lawton     Changed to use similar data retrieval functionality as
*                                       other pages 
***************************************************************************************************
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Database.Business
{
    /// <summary>
    /// Business object thay>t stores a record from the TechSpecs table.
    /// </summar
    public partial class b_Location
    {
        public int TotalCount { get; set; }
        public static b_Location ProcessRowForClientIdLookup(SqlDataReader reader)
        {
            // Create instance of object
            b_Location location = new b_Location();

            // Load the object from the database
            location.LoadFromDatabaseForClientIdLookup(reader);

            // Return result
            return location;
        }

        public void LoadFromDatabaseForClientIdLookup(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // LocationId column, bigint, not null
                LocationId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);

                //Name Column
                Name = reader.GetString(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["LocationId"].ToString(); }
                catch { missing.Append("LocationId "); }

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

        /// <summary>
        /// Retrieve User table records with specified primary key from the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public void RetrieveClientLookupIdBySearchCriteriaFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            ref List<b_Location> data
        )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_Location> results = null;
            data = new List<b_Location>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_Location_RetrieveClientLookupIdBySearchCriteria.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_Location>();
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

        public void ValidateByClientLookupId(
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
            //data = new List<b_StoredProcValidationError>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_Location_ValidateByClientLookupId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        // public void RetrieveAllClientLookupId(
        //    SqlConnection connection,
        //    SqlTransaction transaction,
        //    long callerUserInfoId,
        //    string callerUserName,
        //    ref List<b_Location> data
        //)
        // {

        //   SqlCommand command = null;
        //   string message = String.Empty;
        //   List<b_Location> results = null;
        //   data = new List<b_Location>();

        //   try
        //   {
        //     // Create the command to use in calling the stored procedures
        //     command = new SqlCommand();
        //     command.Connection = connection;
        //     command.Transaction = transaction;

        //     // Call the stored procedure to retrieve the data
        //     results = Database.StoredProcedure.usp_Location_RetrieveAllClientLookupId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

        //     if (results != null)
        //     {
        //       data = results;
        //     }
        //     else
        //     {
        //       data = new List<b_Location>();
        //     }
        //   }
        //   finally
        //   {
        //     if (null != command)
        //     {
        //       command.Dispose();
        //       command = null;
        //     }

        //     message = String.Empty;
        //     callerUserInfoId = 0;
        //     callerUserName = String.Empty;
        //   }
        // }
        public void RetrieveForSearch(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, ref b_Location[] data)
        {
            Database.SqlClient.ProcessRow<b_Location> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_Location[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_Location>(reader => { b_Location obj = new b_Location(); obj.LoadFromDatabase(reader); return obj; });
                results = Database.StoredProcedure.usp_Location_RetrieveForSearch.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

                // Extract the results
                if (null != results)
                {
                    data = (b_Location[])results.ToArray(typeof(b_Location));
                }
                else
                {
                    data = new b_Location[0];
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
        /// Retrieve all Location table records represented by this object in the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="data">b_Location[] that contains the results</param>
        public void RetrieveAll_V2(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName,
            ref b_Location[] data
        )
        {
            Database.SqlClient.ProcessRow<b_Location> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_Location[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_Location>(reader => { b_Location obj = new b_Location(); obj.LoadFromDatabase(reader); return obj; });
                results = Database.StoredProcedure.usp_Location_RetrieveAll_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, ClientId, SiteId);

                // Extract the results
                if (null != results)
                {
                    data = (b_Location[])results.ToArray(typeof(b_Location));
                }
                else
                {
                    data = new b_Location[0];
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

        #region V2 
        public static b_Location ProcessRowForClientIdLookupV2(SqlDataReader reader)
        {
            // Create instance of object
            b_Location location = new b_Location();

            // Load the object from the database
            location.LoadFromDatabaseForClientIdLookupV2(reader);

            // Return result
            return location;
        }

        public void LoadFromDatabaseForClientIdLookupV2(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // LocationId column, bigint, not null
                LocationId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);

                //Name Column
                Name = reader.GetString(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["LocationId"].ToString(); }
                catch { missing.Append("LocationId "); }

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

        public void RetrieveClientLookupIdBySearchCriteriaFromDatabaseV2(
       SqlConnection connection,
       SqlTransaction transaction,
       long callerUserInfoId,
       string callerUserName,
       ref List<b_Location> data
   )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_Location> results = null;
            data = new List<b_Location>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_Location_RetrieveClientLookupIdBySearchCriteria_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_Location>();
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

        public static object ProcessRowV2(SqlDataReader reader)
        {
            // Create instance of object
            b_Location obj = new b_Location();

            // Load the object from the database
            obj.LoadFromDatabaseV2(reader);

            // Return result
            return (object)obj;
        }

        /// <summary>
        /// Load the current row in the input SqlDataReader into a b_Location object.
        /// This routine should be applied to the usp_Location_RetrieveByPK stored procedure.
        /// This routine should be applied to the usp_Location_RetrieveAll. stored procedure.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        public int LoadFromDatabaseV2(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                ////ClientId column, bigint, not null
                //ClientId = reader.GetInt64(i++);

                //// LocationId column, bigint, not null
                LocationId = reader.GetInt64(i++);

                //// SiteId column, bigint, not null
                //SiteId = reader.GetInt64(i++);

                //// AreaId column, bigint, not null
                //AreaId = reader.GetInt64(i++);

                //// DepartmentId column, bigint, not null
                //DepartmentId = reader.GetInt64(i++);

                //// StoreroomId column, bigint, not null
                //StoreroomId = reader.GetInt64(i++);

                ////  ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);

                //// Address1 column, nvarchar(63), not null
                //Address1 = reader.GetString(i++);

                //// Address2 column, nvarchar(63), not null
                //Address2 = reader.GetString(i++);

                //// Address3 column, nvarchar(63), not null
                //Address3 = reader.GetString(i++);

                //// AddressCity column, nvarchar(63), not null
                //AddressCity = reader.GetString(i++);

                ////  AddressPostCode column, nvarchar(31), not null
                //AddressPostCode = reader.GetString(i++);

                //// AddressState column, nvarchar(63), not null
                //AddressState = reader.GetString(i++);

                //// AddressCountry column, nvarchar(63), not null
                //AddressCountry = reader.GetString(i++);

                //// Complex column, nvarchar(15), not null
                Complex = reader.GetString(i++);

                //// CurrentValue column, decimal(12, 2), not null
                //CurrentValue = reader.GetDecimal(i++);

                ////  DepreciationCode column, nvarchar(15), not null
                //DepreciationCode = reader.GetString(i++);

                ////  DepreciationLTD column, decimal(12, 2), not null
                //DepreciationLTD = reader.GetDecimal(i++);

                //// DepreciationPCT column, decimal(6, 3), not null
                //DepreciationPCT = reader.GetDecimal(i++);

                ////  DepreciationYTD column, decimal(12, 2), not null
                //DepreciationYTD = reader.GetDecimal(i++);

                ////  FacilitiyConditionIndex column, decimal(10, 3), not null
                //FacilitiyConditionIndex = reader.GetDecimal(i++);

                //// InactiveFlag column, bit, not null
                //InactiveFlag = reader.GetBoolean(i++);

                //// Labor_AccountId column, bigint, not null
                //Labor_AccountId = reader.GetInt64(i++);

                ////  LifeExpectMonths column, int, not null
                //LifeExpectMonths = reader.GetInt32(i++);

                //// LifeExpectYears column, int, not null
                //LifeExpectYears = reader.GetInt32(i++);

                ////  Material_AccountId column, bigint, not null
                //Material_AccountId = reader.GetInt64(i++);

                ////  Name column, nvarchar(63), not null
                Name = reader.GetString(i++);

                ////  OriginalCost column, decimal(12, 2), not null
                //OriginalCost = reader.GetDecimal(i++);

                ////  ParentId column, bigint, not null
                //ParentId = reader.GetInt64(i++);

                ////  ReplacementCost column, decimal(12, 2), not null
                //ReplacementCost = reader.GetDecimal(i++);

                ////  SquareFeet column, decimal(12, 2), not null
                //SquareFeet = reader.GetDecimal(i++);

                ////  Type column, nvarchar(15), not null
                Type = reader.GetString(i++);

                ////  Usage column, nvarchar(15), not null
                //Usage = reader.GetString(i++);

                ////  UpdateIndex column, int, not null
                //UpdateIndex = reader.GetInt32(i++);

                TotalCount = reader.GetInt32(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                //try { reader["ClientId"].ToString(); }
                //catch { missing.Append("ClientId "); }

                try { reader["LocationId"].ToString(); }
                catch { missing.Append("LocationId "); }

                //try { reader["SiteId"].ToString(); }
                //catch { missing.Append("SiteId "); }

                //try { reader["AreaId"].ToString(); }
                //catch { missing.Append("AreaId "); }

                //try { reader["DepartmentId"].ToString(); }
                //catch { missing.Append("DepartmentId "); }

                //try { reader["StoreroomId"].ToString(); }
                //catch { missing.Append("StoreroomId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                //try { reader["Address1"].ToString(); }
                //catch { missing.Append("Address1 "); }

                //try { reader["Address2"].ToString(); }
                //catch { missing.Append("Address2 "); }

                //try { reader["Address3"].ToString(); }
                //catch { missing.Append("Address3 "); }

                //try { reader["AddressCity"].ToString(); }
                //catch { missing.Append("AddressCity "); }

                //try { reader["AddressPostCode"].ToString(); }
                //catch { missing.Append("AddressPostCode "); }

                //try { reader["AddressState"].ToString(); }
                //catch { missing.Append("AddressState "); }

                //try { reader["AddressCountry"].ToString(); }
                //catch { missing.Append("AddressCountry "); }

                try { reader["Complex"].ToString(); }
                catch { missing.Append("Complex "); }

                //try { reader["CurrentValue"].ToString(); }
                //catch { missing.Append("CurrentValue "); }

                //try { reader["DepreciationCode"].ToString(); }
                //catch { missing.Append("DepreciationCode "); }

                //try { reader["DepreciationLTD"].ToString(); }
                //catch { missing.Append("DepreciationLTD "); }

                //try { reader["DepreciationPCT"].ToString(); }
                //catch { missing.Append("DepreciationPCT "); }

                //try { reader["DepreciationYTD"].ToString(); }
                //catch { missing.Append("DepreciationYTD "); }

                //try { reader["FacilitiyConditionIndex"].ToString(); }
                //catch { missing.Append("FacilitiyConditionIndex "); }

                //try { reader["InactiveFlag"].ToString(); }
                //catch { missing.Append("InactiveFlag "); }

                //try { reader["Labor_AccountId"].ToString(); }
                //catch { missing.Append("Labor_AccountId "); }

                //try { reader["LifeExpectMonths"].ToString(); }
                //catch { missing.Append("LifeExpectMonths "); }

                //try { reader["LifeExpectYears"].ToString(); }
                //catch { missing.Append("LifeExpectYears "); }

                //try { reader["Material_AccountId"].ToString(); }
                //catch { missing.Append("Material_AccountId "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append("Name "); }

                //try { reader["OriginalCost"].ToString(); }
                //catch { missing.Append("OriginalCost "); }

                //try { reader["ParentId"].ToString(); }
                //catch { missing.Append("ParentId "); }

                //try { reader["ReplacementCost"].ToString(); }
                //catch { missing.Append("ReplacementCost "); }

                //try { reader["SquareFeet"].ToString(); }
                //catch { missing.Append("SquareFeet "); }

                try { reader["Type"].ToString(); }
                catch { missing.Append("Type "); }

                //try { reader["Usage"].ToString(); }
                //catch { missing.Append("Usage "); }

                //try { reader["UpdateIndex"].ToString(); }
                //catch { missing.Append("UpdateIndex "); }


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
        #endregion
    }
}
