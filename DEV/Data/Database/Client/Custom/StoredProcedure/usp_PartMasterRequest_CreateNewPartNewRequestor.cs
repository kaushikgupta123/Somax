/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2012-2022 by SOMAX Inc.
 * All rights reserved. 
 ******************************************************************************
 * Date        Log Id     Person               Description
 * =========== ========== ==================== =================================
 * 2022-Jan-04            Roger Lawton         Added parameters 
 ******************************************************************************
 */

using System;
using System.Data;
using System.Data.SqlClient;

using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    /// <summary>
    /// Access the usp_PartMasterRequest_Create stored procedure.
    /// </summary>
    public class usp_PartMasterRequest_CreateNewPartMasterRequest
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_PartMasterRequest_CreateNewPartMasterRequest";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_PartMasterRequest_CreateNewPartMasterRequest()
        {
        }

        /// <summary>
        /// Static method to call the usp_PartMasterRequest_Create stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public static void CallStoredProcedure (
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_PartMasterRequest obj
        )
        {
            SqlParameter        RETURN_CODE_parameter = null;
            int                 retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetOutputParameter(SqlDbType.BigInt, "PartMasterRequestId");
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetInputParameter(SqlDbType.Bit, "Critical", obj.Critical);
            command.SetStringInputParameter(SqlDbType.NVarChar, "PurchaseFreq", obj.PurchaseFreq, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "PurchaseLeadTime", obj.PurchaseLeadTime, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "PurchaseCost", obj.PurchaseCost, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Justification", obj.Justification, -1);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Description", obj.Description,-1);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Manufacturer", obj.Manufacturer, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ManufacturerId", obj.ManufacturerId, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "UnitOfMeasure", obj.UnitOfMeasure, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Status", obj.Status, 15);
            command.SetInputParameter(SqlDbType.BigInt, "PartMasterId", obj.PartMasterId);
            command.SetInputParameter(SqlDbType.BigInt, "CompleteBy_PersonnelId", obj.CompleteBy_PersonnelId);
            command.SetInputParameter(SqlDbType.DateTime2, "CompleteDate", obj.CompleteDate);
            command.SetInputParameter(SqlDbType.BigInt, "CreatedBy_PersonnelId", obj.CreatedBy_PersonnelId);
            command.SetInputParameter(SqlDbType.BigInt, "ApproveDenyBy_PersonnelId", obj.ApproveDenyBy_PersonnelId);
            command.SetInputParameter(SqlDbType.DateTime2, "ApproveDeny_Date", obj.ApproveDeny_Date);
            command.SetInputParameter(SqlDbType.BigInt, "LastReviewedBy_PersonnelId", obj.LastReviewedBy_PersonnelId);
            command.SetInputParameter(SqlDbType.DateTime2, "LastReviewed_Date", obj.LastReviewed_Date);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ImageURL", obj.ImageURL, 255);
            command.SetStringInputParameter(SqlDbType.NVarChar, "RequestType", obj.RequestType, 15);
            //command.SetInputParameter(SqlDbType.BigInt, "ExportLogId", obj.ExportLogId);        // 2022-01-04-RKL 2022-10-13-INT we have not found the column in SP
            command.SetInputParameter(SqlDbType.Decimal, "UnitCost", obj.UnitCost);             // 2022-01-04-RKL
            command.SetStringInputParameter(SqlDbType.NVarChar, "Location", obj.Location, 31);  // 2022-01-04-RKL
            command.SetInputParameter(SqlDbType.Decimal, "QtyMinimum", obj.QtyMinimum);         // 2022-01-04-RKL
            command.SetInputParameter(SqlDbType.Decimal, "QtyMaximum", obj.QtyMaximum);         // 2022-01-04-RKL
            command.SetStringInputParameter(SqlDbType.NVarChar, "Part_ClientLookupId", obj.Part_ClientLookupId,70);

            // Execute stored procedure.
            command.ExecuteNonQuery();

            obj.PartMasterRequestId = (long)command.Parameters["@PartMasterRequestId"].Value;

            // Process the RETURN_CODE parameter value
            retCode = (int) RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}