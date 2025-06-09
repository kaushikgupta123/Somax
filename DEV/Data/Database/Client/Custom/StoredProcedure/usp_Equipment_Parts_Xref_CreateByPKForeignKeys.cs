/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2015 by SOMAX Inc.
* PreventiveMaintenanceDetails.aspx.cs
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID  Person             Description
* =========== ======== ================== ==========================================================
* 2015-Feb-17 SOM-562  Roger Lawton       Part/Vendor Cross-References not showing up
****************************************************************************************************
*/
using System;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    /// <summary>
    /// Access the usp_Equipment_Parts_Xref_Create stored procedure.
    /// </summary>
    public class usp_Equipment_Parts_Xref_CreateByPKForeignKeys
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_Equipment_Parts_Xref_CreateByPKForeignKeys";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_Equipment_Parts_Xref_CreateByPKForeignKeys()
        {
        }

        /// <summary>
        /// Static method to call the usp_Equipment_Parts_Xref_CreateByPKForeignKeys stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="clientId">long that contains the value of the @ClientId parameter</param>
        /// <param name="equipment_Parts_XrefId">long that contains the value of the @Equipment_Parts_XrefId parameter</param>
        /// <param name="equipmentID">long that contains the value of the @EquipmentID parameter</param>
        /// <param name="partID">long that contains the value of the @PartID parameter</param>
        /// <param name="comment">string that contains the value of the @Comment parameter</param>
        /// <param name="quantityNeeded">decimal that contains the value of the @QuantityNeeded parameter</param>
        /// <param name="quantityUsed">decimal that contains the value of the @QuantityUsed parameter</param>
        public static void CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_Equipment_Parts_Xref obj
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
            command.SetInputParameter(SqlDbType.BigInt, "ParentSiteId", obj.ParentSiteId);
            command.SetOutputParameter(SqlDbType.BigInt, "Equipment_Parts_XrefId");
            command.SetInputParameter(SqlDbType.BigInt, "EquipmentId", obj.EquipmentId);
            command.SetInputParameter(SqlDbType.BigInt, "PartId", obj.PartId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Comment", obj.Comment, 2047);
            command.SetInputParameter(SqlDbType.Decimal, "QuantityNeeded", obj.QuantityNeeded);
            command.SetInputParameter(SqlDbType.Decimal, "QuantityUsed", obj.QuantityUsed);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Part_ClientLookupId", obj.Part_ClientLookupId, 70);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Equipment_ClientLookupId", obj.Equipment_ClientLookupId, 31);


            // Execute stored procedure.
            command.ExecuteNonQuery();

            obj.Equipment_Parts_XrefId = (long)command.Parameters["@Equipment_Parts_XrefId"].Value;

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}