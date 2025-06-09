/*
**************************************************************************************************
* PROPRIETARY DATA 
**************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc. and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
**************************************************************************************************
* Copyright (c) 2014 by SOMAX Inc.. All rights reserved. 
**************************************************************************************************
* Date         JIRA Item Person           Description
* ===========  ========= ================ ========================================================
* 2014-Oct-27  SOM-384   Roger Lawton     Added Class
**************************************************************************************************
*/
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

using Database.Business;
using Database.SqlClient;
using System.Collections.Generic;

namespace Database.StoredProcedure
{
    /// <summary>
    /// Access the usp_SecurityProfile_RetrieveByName stored procedure.
    /// </summary>
    public class usp_SecurityProfile_RetrieveAllProfilesForEnterPrise_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_SecurityProfile_RetrieveAllProfilesForEnterPrise_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_SecurityProfile_RetrieveAllProfilesForEnterPrise_V2()
        {
        }

        /// <summary>
        /// Static method to call the usp_SecurityProfile_RetrieveByName stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="processRow">ProcessRow delegate containing method to call to process the row into an object.</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <returns>ArrayList containing the results of the query</returns>
        public static List<b_SecurityProfile> CallStoredProcedure(
            SqlCommand command,
            Database.SqlClient.ProcessRow<b_SecurityProfile> processRow,
            long callerUserInfoId,
            string callerUserName,
            long clientId,
            b_SecurityProfile obj
        )
        {
            List<b_SecurityProfile> records = new List<b_SecurityProfile>();
            SqlDataReader reader = null;
            b_SecurityProfile record = null;
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", clientId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "PackageLevel", obj.PackageLevel, 15);
           


            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();
                // Loop through dataset.
                while (reader.Read())
                {
                    // Process the current row into a record
                    record = b_SecurityProfile.LoadFromDatabaseRetrieveByPackageLevel_V2(reader);

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
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);

            // Return the result
            return records;
        }
    }
}