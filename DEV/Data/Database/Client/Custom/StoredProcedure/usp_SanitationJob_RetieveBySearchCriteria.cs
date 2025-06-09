/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2015 by SOMAX Inc.
* All rights reserved. 
****************************************************************************************************
* Date        Task ID   Person             Description
* =========== ======== =================== =======================================================
* 2015-Mar-21 SOM-585  Roger Lawton        Changed Parameters
****************************************************************************************************
 */
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

using Database;
using Database.Business;
using System.Collections;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_SanitationJob_RetieveBySearchCriteria
    {
        private static string STOREDPROCEDURE_NAME = "usp_SanitationJob_RetieveBySearchCriteria";

        /// <summary>
        /// Constructor
        /// </summary>
        public usp_SanitationJob_RetieveBySearchCriteria()
        {
        }


        public static ArrayList CallStoredProcedure(
            SqlCommand command,
            Database.SqlClient.ProcessRow<b_SanitationJob> processRow,
            long callerUserInfoId,
            string callerUserName,
            b_SanitationJob b_SanitationJob
        )
        {
            ArrayList records = new ArrayList();
            SqlDataReader reader = null;
            b_SanitationJob record = null;
            SqlParameter RETURN_CODE_parameter = null;
            //SqlParameter        clientId_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", b_SanitationJob.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", b_SanitationJob.SiteId);
            command.SetInputParameter(SqlDbType.BigInt, "AssignedTo_PersonnelId", b_SanitationJob.AssignedTo_PersonnelId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Shift", b_SanitationJob.Shift, 15);
            command.SetInputParameter(SqlDbType.DateTime2, "ScheduledDate", b_SanitationJob.ScheduledDate);

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
            catch (Exception ex) { }
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
