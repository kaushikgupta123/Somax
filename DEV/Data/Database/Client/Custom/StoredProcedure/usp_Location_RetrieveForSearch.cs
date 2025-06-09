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

using Database.Business;
using Database.SqlClient;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_Location_RetrieveForSearch
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_Location_RetrieveForSearch";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_Location_RetrieveForSearch()
        {
        }

        /// <summary>
        /// Static method to call the usp_Location_RetrieveForSearch stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="processRow">ProcessRow delegate containing method to call to process the row into an object.</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <returns>ArrayList containing the results of the query</returns>
        public static ArrayList CallStoredProcedure(
            SqlCommand command,
            Database.SqlClient.ProcessRow<b_Location> processRow,
            long callerUserInfoId,
            string callerUserName,
            b_Location obj)
        {
            ArrayList records = new ArrayList();
            SqlDataReader reader = null;
            b_Location record = null;
            SqlParameter RETURN_CODE_parameter = null;
            //SqlParameter        clientId_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            //command.SetInputParameter(SqlDbType.BigInt, "CustomQueryDisplayId", obj.CustomQueryDisplayId);
            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read()) 
                {
                    // Process the current row into a record
                    record = processRow(reader);

                    // Add the record to the list.
                    records.Add(record);
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
            retCode = (int) RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);

            // Return the result
            return records;
        }
    }
}
