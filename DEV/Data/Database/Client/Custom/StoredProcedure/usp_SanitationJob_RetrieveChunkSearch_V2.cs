/*
**************************************************************************************************
* PROPRIETARY DATA 
**************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc. and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
**************************************************************************************************
* Copyright (c) 2020 by SOMAX Inc.. All rights reserved. 
**************************************************************************************************
* Date         JIRA Item Person                 Description
* ===========  ========= ====================== =================================================
**************************************************************************************************
*/

using System;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using Database.SqlClient;
using System.Data.Common;
using System.Collections.Generic;

namespace Database.StoredProcedure
{
    public class usp_SanitationJob_RetrieveChunkSearch_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_SanitationJob_RetrieveChunkSearch_V2";
       
        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_SanitationJob_RetrieveChunkSearch_V2()
        {
        }

        /// <summary>
        /// Static method to call the usp_WorkOrder_CompleteWorkOrder stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public static b_SanitationJob CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_SanitationJob obj
        )
        {
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
            command.SetInputParameter(SqlDbType.BigInt, "LoggedInUserPEID", obj.LoggedInUserPEID);
            command.SetInputParameter(SqlDbType.Int, "CaseNo", obj.CustomQueryDisplayId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderbyColumn", obj.orderbyColumn, 100);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderBy", obj.orderBy, 10);
            command.SetInputParameter(SqlDbType.Int, "offset1", obj.offset1);
            command.SetInputParameter(SqlDbType.Int, "nextrow", obj.nextrow);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ClientLookupId", obj.ClientLookupId, 67);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Description", obj.Description, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Chargeto", obj.ChargeTo, 67);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ChargetoName", obj.ChargeToName, 67);
            command.SetStringInputParameter(SqlDbType.NVarChar, "AssetLocation", obj.AssetLocation, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Status", obj.Status, 67);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Shift", obj.Shift, 67);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CreateBy", obj.CreateByName, 67);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CreatedDate", obj.CreatedDate, 67);
            command.SetStringInputParameter(SqlDbType.NVarChar, "AssignedBy", obj.AssignedBy, 67);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CompleteDate", obj.CompletedDate, 67);
            command.SetStringInputParameter(SqlDbType.NVarChar, "VerifiedBy", obj.VerifiedBy, 67);
            command.SetStringInputParameter(SqlDbType.NVarChar, "VerifiedDate", obj.VerifyDate, 67);
            command.SetInputParameter(SqlDbType.Bit, "Extracted", obj.Extracted);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ScheduledDate", obj.ScheduleDate, 67);
            command.SetStringInputParameter(SqlDbType.NVarChar, "AssetGroup1_ClientLookUpId", obj.AssetGroup1_ClientLookUpId, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "AssetGroup2_ClientLookUpId", obj.AssetGroup2_ClientLookUpId, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "AssetGroup3_ClientLookUpId", obj.AssetGroup3_ClientLookUpId, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "SearchText", obj.SearchText, 800);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CreateStartDateVw", obj.CreateStartDateVw, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CreateEndDateVw", obj.CreateEndDateVw, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CompleteStartDateVw", obj.CompleteStartDateVw, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CompleteEndDateVw", obj.CompleteEndDateVw, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "FailedStartDateVw", obj.FailedStartDateVw, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "FailedEndDateVw", obj.FailedEndDateVw, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "PassedStartDateVw", obj.PassedStartDateVw, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "PassedEndDateVw", obj.PassedEndDateVw, 500);
            try
            {

                List<b_SanitationJob> records = new List<b_SanitationJob>();
                // Execute stored procedure.
                obj.utilityAdd = new UtilityAdd();
                obj.utilityAdd.list1 = new List<string>();
                reader = command.ExecuteReader();
               
                obj.ListSanJob = new List<b_SanitationJob>();
                while (reader.Read())
                {
                    b_SanitationJob tmpSanJob = b_SanitationJob.ProcessRowForSanitationJobRetriveAllForSearch(reader);
                    obj.ListSanJob.Add(tmpSanJob);
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
            return obj;
        }
    }
}