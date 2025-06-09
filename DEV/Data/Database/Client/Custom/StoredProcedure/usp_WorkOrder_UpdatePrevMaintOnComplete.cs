/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2014 by SOMAX Inc.
* All rights reserved. 
****************************************************************************************************
* Date        Task ID   Person             Description
* =========== ======== =================== =======================================================
* 2014-Sep-09 SOM-304  Roger Lawton        Update Prev Maint Sched record on Complete
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
  /// Access the usp_WorkOrder_UpdatePrevMaintOnComplete stored procedure.
  /// </summary>
  public class usp_WorkOrder_UpdatePrevMaintOnComplete
  {
    /// <summary>
    /// Constants.
    /// </summary>
    private static string STOREDPROCEDURE_NAME = "usp_WorkOrder_UpdatePrevMaintOnComplete";

    /// <summary>
    /// Default constructor.
    /// </summary>
    public usp_WorkOrder_UpdatePrevMaintOnComplete()
    {
    }

    /// <summary>
    /// Static method to call the usp_WorkOrder_UpdateByForeignKeys stored procedure using SqlClient.
    /// </summary>
    /// <param name="command">SqlCommand object to use to call the stored procedure</param>
    /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
    /// <param name="callerUserName">string that identifies the user calling the database</param>
    public static void CallStoredProcedure(
        SqlCommand command,
        long callerUserInfoId,
        string callerUserName,
        b_WorkOrder obj
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
      command.SetInputParameter(SqlDbType.BigInt, "WorkOrderId", obj.WorkOrderId);

      // Execute stored procedure.
      command.ExecuteNonQuery();

      retCode = (int)RETURN_CODE_parameter.Value;
      AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
    }
  }
}