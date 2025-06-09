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

using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    /// <summary>
    /// Access the usp_Site_RetrieveList stored procedure.
    /// </summary>
    public class usp_Site_RetrieveList
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_Site_RetrieveList";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_Site_RetrieveList()
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
        public static List<b_Site> CallStoredProcedure(
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
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", clientId);

            // Setup the return object
            List<b_Site> result = new List<b_Site>();

            try
            {

                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Retrieve List
                while (reader.Read())
                {
                    // Add the record to the list.
                    result.Add((b_Site)b_Site.ProcessRow(reader));
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
            return result;
        }
    }
}