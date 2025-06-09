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
* 2015-Mar-21 SOM-585  Roger Lawton        Review Changes
****************************************************************************************************
 */
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

using Database;
using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_SanitationJobGeneration
    {
        private static string STOREDPROCEDURE_NAME = "usp_SanitationJob_Generation";

        public static void CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            long SanitationJobBatchEntryId,
            ref b_SanitationJob b_SanitationJob
        )
        {
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "SanitationJobBatchEntryId", SanitationJobBatchEntryId);
            //command.SetStringInputParameter(SqlDbType.NVarChar, "SanitationJob_ClientLookUpId", b_SanitationJob.ClientLookupId, 25);
            //command.SetInputParameter(SqlDbType.BigInt, "PersonnelId", b_SanitationJob.Requestor_PersonnelId);
            command.SetOutputParameter(SqlDbType.BigInt, "LastGeneratedJobId");

            try
            {
                // Execute stored procedure.
                command.ExecuteNonQuery();

                b_SanitationJob.SanitationJobId = Convert.ToInt32(command.Parameters["@LastGeneratedJobId"].Value.ToString());

            }
            catch (Exception ex) { }
            finally
            {


                // Process the RETURN_CODE parameter value
                retCode = (int)RETURN_CODE_parameter.Value;
                AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);

                // Return the result
                //return result;
            }
        }
    }
}
