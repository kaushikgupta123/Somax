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
using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_SanitationJob_UpdateForComplete
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_SanitationJob_UpdateForComplete";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_SanitationJob_UpdateForComplete()
        {

        }


        public static void CallStoredProcedure(
            SqlCommand command,
            long CallerUserPersonnelId,
            string callerUserName,
            b_SanitationJob obj
        )
        {
            SqlParameter RETURN_CODE_parameter = null;
            SqlParameter updateIndexOut_parameter = null;
            int retCode = 0;
            
            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserPersonnelId", CallerUserPersonnelId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetInputParameter(SqlDbType.BigInt, "SanitationJobId", obj.SanitationJobId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ChargeType", obj.ChargeType, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ChargeTo_ClientLookupId", obj.ChargeTo_ClientLookupId, 31);
            command.SetInputParameter(SqlDbType.BigInt, "PlantLocationId", obj.PlantLocationId);//SOM-1348 
            command.SetInputParameter(SqlDbType.Decimal, "ActualDuration", obj.ActualDuration);
            command.SetInputParameter(SqlDbType.DateTime2, "CompleteDate", obj.CompleteDate);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CompleteComments", obj.CompleteComments, 1073741823);
            command.SetInputParameter(SqlDbType.BigInt, "CompleteBy_PersonnelId", obj.CompleteBy_PersonnelId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Description", obj.Description, 1073741823);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Status", obj.Status, 15);
            command.SetInputParameter(SqlDbType.BigInt, "UpdateIndex", obj.UpdateIndex);

            // Setup updateIndexOut parameter.
            updateIndexOut_parameter = command.Parameters.Add("@UpdateIndexOut", SqlDbType.Int);
            updateIndexOut_parameter.Direction = ParameterDirection.Output;

            // Execute stored procedure.
            command.ExecuteNonQuery();

            obj.UpdateIndex = (int)updateIndexOut_parameter.Value;

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}
