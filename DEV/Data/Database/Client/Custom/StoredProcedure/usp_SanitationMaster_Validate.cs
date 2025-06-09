/*
***************************************************************************************************
* PROPRIETARY DATA 
***************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
***************************************************************************************************
* Copyright (c) 2014 by SOMAX Inc.
* All rights reserved. 
***************************************************************************************************
* Date        Task ID   Person             Description
* =========== ======== =================== ========================================================
* 2015-Mar-12 SOM-585  Roger Lawton        Validate Sanitation Master Add or Save
***************************************************************************************************
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
    public class usp_SanitationMaster_Validate

    {
        private static string STOREDPROCEDURE_NAME = "usp_SanitationMaster_Validate";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_SanitationMaster_Validate()
        {
        }

        
        public static List<b_StoredProcValidationError> CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_SanitationMaster obj
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
            command.SetInputParameter(SqlDbType.BigInt, "SanitationMasterId", obj.SanitationMasterId);
            command.SetInputParameter(SqlDbType.BigInt, "AssignTo_PeronnelId", obj.Assignto_PersonnelId);
            command.SetInputParameter(SqlDbType.BigInt, "PlantLocationId", obj.PlantLocationId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ChargeType", obj.ChargeType, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ChargeToClientLookupId", obj.ChargeToClientLookupId, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Description", obj.Description, 1000000);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ExclusionDays", obj.ExclusionDays, 7);
            command.SetInputParameter(SqlDbType.Int, "Frequency", obj.Frequency);
            command.SetInputParameter(SqlDbType.DateTime2, "LastScheduled", obj.LastScheduled);
            command.SetInputParameter(SqlDbType.DateTime2, "NextDue", obj.NextDue);
            command.SetStringInputParameter(SqlDbType.NVarChar, "OnDemandGroup", obj.OnDemandGroup, 15);
            command.SetInputParameter(SqlDbType.Decimal, "ScheduledDuration", obj.ScheduledDuration);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ScheduledType", obj.ScheduledType, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Shift", obj.Shift, 15);
            command.SetInputParameter(SqlDbType.Bit, "InactiveFlag", obj.InactiveFlag);
  
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
