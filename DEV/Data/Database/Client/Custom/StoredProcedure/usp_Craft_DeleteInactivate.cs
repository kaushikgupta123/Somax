/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2016 by SOMAX Inc.
* PreventiveMaintenanceDetails.aspx.cs
* All rights reserved. 
****************************************************************************************************
* This transaction returns at least one error item even if successful
* If the process deletes the craft then 
*   Code="1107">Craft {0} has been deleted 
* is returned
* If the process inactivated the craft then  
*   Code="1108">Craft {0} is referenced in the system.  It has been inactivated instead of deleted.
* is returned
****************************************************************************************************
* Date        JIRA-ID  Person             Description
* =========== ======== ================== ==========================================================
* 2015-Nov-05 SOM-844  Roger Lawton       Added Craft_DeleteInactivate Stored Procedure
*                                         
****************************************************************************************************
 */

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_Craft_DeleteInactivate
    {
        private static string STOREDPROCEDURE_NAME = "usp_Craft_DeleteInactivate";

        public usp_Craft_DeleteInactivate()
        {
        }

        public static List<b_StoredProcValidationError> CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_Craft obj
        )
        {
            List<b_StoredProcValidationError> records = new List<b_StoredProcValidationError>();
            SqlDataReader reader = null;
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;


            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "CraftId", obj.CraftId);

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.
                    records.Add((b_StoredProcValidationError)b_StoredProcValidationError.ProcessRow(reader));
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
