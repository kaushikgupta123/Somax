/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2011 by SOMAX Inc.
 * All rights reserved. 
 ******************************************************************************
 * Date         Log Entry   Person                  Description
 * ===========  =========   ======================= ===========================
 * 2011-Nov-22  201100000   Roger Lawton            Created
 ******************************************************************************
 */
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

using Database;
using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    /// <summary>
    /// Access the usp_PermissionLists_Retrieve stored procedure.
    /// </summary>
    public class usp_PermissionLists_Retrieve
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_PermissionLists_Retrieve";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_PermissionLists_Retrieve()
        {
        }

        /// <summary>
        /// Static method to call the usp_Site_RetrieveList stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">integer that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the datebase</param>
        /// <param name="clientId">long that contains the user's client id</param>
        /// <returns>Array of business objects containing the results of the query</returns>
        public static b_PermissionLists CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            long clientId
            )
        {
            SqlDataReader reader = null;
            SqlParameter RETURN_CODE_parameter = null;

            int retCode = 0;


            // Setup command.
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = STOREDPROCEDURE_NAME;
            command.Parameters.Clear();

            // Setup RETURN_CODE parameter.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", clientId);

            // Setup the return object
            b_PermissionLists results = new b_PermissionLists();

            try
            {
                results.Sites = new List<b_Site>();
                results.Areas = new List<b_Area>();
                results.Departments = new List<b_Department>();
                results.Storerooms = new List<b_Storeroom>();
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through returned rowsets
                // Sites
                while (reader.Read())
                {
                    results.Sites.Add((b_Site)b_Site.ProcessRow(reader));
                }

                // Areas
                reader.NextResult();
                while (reader.Read())
                {
                    results.Areas.Add((b_Area)b_Area.ProcessRow(reader));
                }

                // Departments
                reader.NextResult();
                while (reader.Read())
                {
                    results.Departments.Add((b_Department)b_Department.ProcessRow(reader));
                }

                // Storerooms
                reader.NextResult();
                while (reader.Read())
                {
                    results.Storerooms.Add((b_Storeroom)b_Storeroom.ProcessRow(reader));
                }

            }
            finally
            {
                if (null != reader)
                {
                    if (false == reader.IsClosed)
                    {
                        reader.Close();
                    }
                    reader = null;
                }
            }

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);

            // Return the result
            //return records;
            return results;
        }
    }
}