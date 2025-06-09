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
* Date         JIRA Item Person                 Description
* ===========  ========= ====================== =================================================
* 2014-Aug-12  SOM-285   Roger Lawton           Removed ProcessRow
*                                               Use LoadFromDatabase_Extended
**************************************************************************************************
*/
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

using Database.Business;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_SanitationMasterTask_RetrieveBySanitationMasterId
    {
        private static string STOREDPROCEDURE_NAME = "usp_SanitationMasterTask_RetrieveBySanitationMasterId";

        public usp_SanitationMasterTask_RetrieveBySanitationMasterId()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="callerUserInfoId"></param>
        /// <param name="callerUserName"></param>
        /// <param name="clientId"></param>
        /// <param name="PrevMaintMasterId"></param>
        /// <returns></returns>
        public static List<b_SanitationMasterTask> CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            long clientId,
            long SanitationMasterId
        )
        {
            List<b_SanitationMasterTask> records = new List<b_SanitationMasterTask>();
            SqlDataReader reader = null;
            b_SanitationMasterTask record = null;
            SqlParameter RETURN_CODE_parameter = null;
            //SqlParameter        clientId_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", clientId);
            command.SetInputParameter(SqlDbType.BigInt, "SanitationMasterId", SanitationMasterId);
            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {

                    // Process the current row into a record
                    record = new b_SanitationMasterTask();
                    record.LoadFromDatabase_Extended(reader);

                    // Add the record to the list.
                    records.Add(record);
                }
            }
            catch (Exception ex)
            {
                throw;
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
