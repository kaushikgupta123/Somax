/*
***************************************************************************************************
* PROPRIETARY DATA 
***************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as a Trade Secret. 
***************************************************************************************************
* Copyright (c) 2015 by SOMAX Inc.
* All rights reserved. 
***************************************************************************************************
* Date        Task ID   Person             Description
* =========== ======== =================== =======================================================
* 2015-Apr-02 SOM-619  Roger Lawton        Changed Parameter
***************************************************************************************************
*/
using System;
using System.Collections.Generic;

using System.Data;
using System.Data.SqlClient;
using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_PartHistory_PhysicalInventory
    {
        /// <summary>
        /// Constants
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_PartHistory_PhysicalInventory";

        /// <summary>
        /// Constructor
        /// </summary>
        public usp_PartHistory_PhysicalInventory()
        {

        }

        public static void CallStoredProcedure(
         SqlCommand command,
         long callerUserInfoId,
         string callerUserName,
         b_PartHistory obj
         )
        {
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetInputParameter(SqlDbType.BigInt, "PartId", obj.PartId);
            command.SetInputParameter(SqlDbType.BigInt, "PerformById", obj.PerformedById);
            command.SetInputParameter(SqlDbType.Decimal, "QuantityCounted", obj.PartQtyCounted);
            command.SetOutputParameter(SqlDbType.BigInt, "PartHistoryId");


            // Execute stored procedure.
            command.ExecuteNonQuery();

            obj.PartHistoryId = (long)command.Parameters["@PartHistoryId"].Value;

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }



    }
}
