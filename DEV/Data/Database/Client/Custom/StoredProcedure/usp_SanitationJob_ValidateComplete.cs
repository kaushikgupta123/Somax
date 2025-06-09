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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database.Business;
using System.Data.SqlClient;
using System.Data;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_SanitationJob_ValidateComplete
    {
        private static string STOREDPROCEDURE_NAME = "usp_SanitationJob_ValidateComplete";
         /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_SanitationJob_ValidateComplete()
        {
        }

        
        public static List<b_StoredProcValidationError> CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_SanitationJob obj
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
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetInputParameter(SqlDbType.BigInt, "SanitationJobId", obj.SanitationJobId);
            command.SetInputParameter(SqlDbType.DateTime2, "CompleteDate", obj.CompleteDate);
            command.SetInputParameter(SqlDbType.BigInt, "CompleteBy_PersonnelId", obj.CompleteBy_PersonnelId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ChargeType", obj.ChargeType, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ChargeTo_ClientLookupId", obj.ChargeTo_ClientLookupId, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Description", obj.Description, 1000000);
            command.SetInputParameter(SqlDbType.BigInt, "AssignedTo_PersonnelId", obj.AssignedTo_PersonnelId);
            command.SetInputParameter(SqlDbType.Decimal, "ActualDuration", obj.ActualDuration);

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
            catch
            {
              Exception ex;
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
