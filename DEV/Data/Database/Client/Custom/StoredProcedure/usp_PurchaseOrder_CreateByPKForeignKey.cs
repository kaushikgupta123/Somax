/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2014 by SOMAX Inc.
* b_PurchaseOrder.cs (Data Object)
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID  Person             Description
* =========== ======== ================== =========================================================
****************************************************************************************************
*/

using System;
using System.Data;
using System.Data.SqlClient;
using Database.SqlClient;
using Database.Business;

namespace Database.StoredProcedure
{
  /// <summary>
    /// Access the usp_PurchaseOrder_CreateByPKForeignKey stored procedure.
  /// </summary>
  public class usp_PurchaseOrder_CreateByPKForeignKey
  {
    /// <summary>
    /// Constants.
    /// </summary>
    private static string STOREDPROCEDURE_NAME = "usp_PurchaseOrder_CreateByPKForeignKey";

    /// <summary>
    /// Default constructor.
    /// </summary>
    public usp_PurchaseOrder_CreateByPKForeignKey()
    {
    }

    /// <summary>
    /// Static method to call the usp_PrevMaintSched_Create stored procedure using SqlClient.
    /// </summary>
    /// <param name="command">SqlCommand object to use to call the stored procedure</param>
    /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
    /// <param name="callerUserName">string that identifies the user calling the database</param>
    public static void CallStoredProcedure(SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_PurchaseOrder obj)
    {
      SqlParameter RETURN_CODE_parameter = null;
      int retCode = 0;

      // Setup command.
      command.SetProcName(STOREDPROCEDURE_NAME);
      RETURN_CODE_parameter = command.GetReturnCodeParameter();
      command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
      command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
      command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
      command.SetOutputParameter(SqlDbType.BigInt, "PurchaseOrderId");
      command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
      command.SetInputParameter(SqlDbType.BigInt, "DepartmentId", obj.DepartmentId);
      command.SetInputParameter(SqlDbType.BigInt, "AreaId", obj.AreaId);
      command.SetInputParameter(SqlDbType.BigInt, "StoreroomId", obj.StoreroomId);
      command.SetStringInputParameter(SqlDbType.NVarChar, "ClientLookupId", obj.ClientLookupId, 31);
      command.SetStringInputParameter(SqlDbType.NVarChar, "Attention", obj.Attention, 63);
      command.SetInputParameter(SqlDbType.BigInt, "Buyer_PersonnelId", obj.Buyer_PersonnelId);
      command.SetStringInputParameter(SqlDbType.NVarChar, "Carrier", obj.Carrier, 15);
      command.SetInputParameter(SqlDbType.BigInt, "CompleteBy_PersonnelId", obj.CompleteBy_PersonnelId);
      command.SetInputParameter(SqlDbType.DateTime2, "CompleteDate", obj.CompleteDate);
      command.SetInputParameter(SqlDbType.BigInt, "Creator_PersonnelId", obj.Creator_PersonnelId);
      command.SetStringInputParameter(SqlDbType.NVarChar, "FOB", obj.FOB, 15);
      command.SetStringInputParameter(SqlDbType.NVarChar, "Status", obj.Status, 15);
      command.SetStringInputParameter(SqlDbType.NVarChar, "Terms", obj.Terms, 15);
      command.SetInputParameter(SqlDbType.BigInt, "VendorId", obj.VendorId);
      command.SetInputParameter(SqlDbType.BigInt, "VoidBy_PersonnelId", obj.VoidBy_PersonnelId);
      command.SetInputParameter(SqlDbType.DateTime2, "VoidDate", obj.VoidDate);
      command.SetStringInputParameter(SqlDbType.NVarChar, "VoidReason", obj.VoidReason, 15);
      command.SetStringInputParameter(SqlDbType.NVarChar, "Reason", obj.Reason, 255);
      // Execute stored procedure.
      command.ExecuteNonQuery();

      obj.PurchaseOrderId = (long)command.Parameters["@PurchaseOrderId"].Value;

      // Process the RETURN_CODE parameter value
      retCode = (int)RETURN_CODE_parameter.Value;
      AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
    }
  }
}