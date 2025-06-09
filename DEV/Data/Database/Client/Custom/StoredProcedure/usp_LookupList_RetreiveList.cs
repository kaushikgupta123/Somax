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
    /// Access the usp_LookupList_RetreiveList stored procedure.
    /// </summary>
    public class usp_LookupList_RetreiveList
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_LookupList_RetreiveList";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_LookupList_RetreiveList()
        {
        }

        /// <summary>
        /// Static method to call the usp_UserData_RetrieveByUserName stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="processRow">ProcessRow delegate containing method to call to process the row into an object.</param>
        /// <param name="callerId">string that identifies the user calling the database</param>
        /// <param name="clientId">long that contains the value of the @ClientId parameter</param>
        /// <param name="sessionId">System.Guid that contains the value of the @SessionId parameter</param>
        /// <returns>ArrayList containing the results of the query</returns>
        public static List<b_LookupList> CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            long clientId,
            string listname,
            string listfilter,
            long ParentSiteID,
            string txtSearch
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
            command.SetStringInputParameter(SqlDbType.NVarChar, "LookupList", listname, 25);
            command.SetStringInputParameter(SqlDbType.NVarChar, "LookupFilter", listfilter, 15);
            //command.SetInputParameter(SqlDbType.BigInt, "LookupFilter", listfilter);
            command.SetInputParameter(SqlDbType.BigInt, "ParentSiteID", ParentSiteID);
            command.SetStringInputParameter(SqlDbType.NVarChar, "txtSearch", txtSearch, 15);

            List<b_LookupList> result = new List<b_LookupList>();

            try
            {

                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Retrieve List
                while (reader.Read())
                {
                    // Add the record to the list.
                    result.Add((b_LookupList)b_LookupList.ProcessRow(reader));
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