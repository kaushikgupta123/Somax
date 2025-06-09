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
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using System.Collections.Generic;
using Database.SqlClient;

namespace
    Database.StoredProcedure
{
    /// <summary>
    /// Access the usp_Part_Vendor_Xref_Create_Update_Punchout_V2 stored procedure.
    /// </summary>
    public class usp_Part_Vendor_Xref_Create_Update_Punchout_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_Part_Vendor_Xref_Create_Update_Punchout_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_Part_Vendor_Xref_Create_Update_Punchout_V2()
        {
        }

        /// <summary>
        /// Static method to call the usp_Part_Vendor_Xref_Create_Update_Punchout_V2 stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>

        public static void CallStoredProcedure(
          SqlCommand command,
          long callerUserInfoId,
          string callerUserName,
          b_Part_Vendor_Xref obj
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
            command.SetInputParameter(SqlDbType.BigInt, "VendorId", obj.VendorId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CatalogNumber", obj.CatalogNumber, 31);
            command.SetInputParameter(SqlDbType.Decimal, "IssueOrder", obj.IssueOrder);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Manufacturer", obj.Manufacturer, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ManufacturerId", obj.ManufacturerId, 63);
            command.SetInputParameter(SqlDbType.Int, "OrderQuantity", obj.OrderQuantity);
            command.SetStringInputParameter(SqlDbType.NVarChar, "OrderUnit", obj.OrderUnit, 15);
            command.SetInputParameter(SqlDbType.Decimal, "Price", obj.Price);
            command.SetInputParameter(SqlDbType.Bit, "UOMConvRequired", obj.UOMConvRequired);
            command.SetInputParameter(SqlDbType.Bit, "Punchout", obj.Punchout);
            // Execute stored procedure.
            command.ExecuteNonQuery();
            

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}