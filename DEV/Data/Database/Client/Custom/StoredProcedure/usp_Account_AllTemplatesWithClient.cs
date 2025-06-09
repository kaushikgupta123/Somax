/*
**************************************************************************************************
* PROPRIETARY DATA 
**************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc. and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
**************************************************************************************************
* Copyright (c) 2016 by SOMAX Inc.. All rights reserved. 
**************************************************************************************************
* Date         JIRA Item Person                 Description
* ===========  ========= ====================== =================================================
* 2016-Jan-07  SOM-882   Roger Lawton           Removed ClientId parameter 
*                                               Added b_Account Parameter
**************************************************************************************************
*/
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_Account_AllTemplatesWithClient
    {
        private static string STOREDPROCEDURE_NAME = "usp_Account_AllTemplatesWithClient";

        public usp_Account_AllTemplatesWithClient()
        {
        }

        public static List<b_Account> CallStoredProcedure(
            SqlCommand command,
            Database.SqlClient.ProcessRow<b_Account> processRow,
            long callerUserInfoId,
            string callerUserName,
            b_Account Account
        )
        {
            List<b_Account> records = new List<b_Account>();
            SqlDataReader reader = null;
            b_Account record = null;
            SqlParameter RETURN_CODE_parameter = null;
            //SqlParameter        clientId_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", Account.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", Account.SiteId);
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
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);

            // Return the result
            return records;
        }



    }
}
