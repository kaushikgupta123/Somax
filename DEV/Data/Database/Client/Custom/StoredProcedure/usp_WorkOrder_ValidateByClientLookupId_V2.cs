/*
***************************************************************************************************
* PROPRIETARY DATA 
***************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc. and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
***************************************************************************************************
* Copyright (c) 2014 by SOMAX Inc.. All rights reserved. 
***************************************************************************************************
* Change Log
***************************************************************************************************
Date         JIRA Item Entry Person                  Description
===========  ========= ======================= ====================================================
2014-Jul-20  SOM-194   Roger Lawton            Modified and cleaned up - not working with shared 
                                               Lookup List Items (Siteid = 0)
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
  /// <summary>
  /// Access the usp_WorkOrder_ValidateByClientLookupId_V2 stored procedure.
  /// </summary>
  public class usp_WorkOrder_ValidateByClientLookupId_V2
  {
    /// <summary>
    /// Constants.
    /// </summary>
    private static string STOREDPROCEDURE_NAME = "usp_WorkOrder_ValidateByClientLookupId_V2";

    /// <summary>
    /// Default constructor.
    /// </summary>
    public usp_WorkOrder_ValidateByClientLookupId_V2()
    {
    }

    /// <summary>
    /// Static method to call the usp_WorkOrder_ValidateByClientLookupId_V2 stored procedure using SqlClient.
    /// </summary>
    /// <param name="command">SqlCommand object to use to call the stored procedure</param>
    /// <param name="processRow">ProcessRow delegate containing method to call to process the row into an object.</param>
    /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
    /// <param name="callerUserName">string that identifies the user calling the database</param>
    /// <param name="clientId">long that contains the value of the @ClientId parameter</param>
    public static List<b_StoredProcValidationError> CallStoredProcedure(
        SqlCommand command,
        long callerUserInfoId,
        string callerUserName,
        b_WorkOrder obj,
        bool createMode
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
      command.SetInputParameter(SqlDbType.BigInt, "WorkOrderId", obj.WorkOrderId);
      command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
      command.SetStringInputParameter(SqlDbType.NVarChar, "ClientLookupId", obj.ClientLookupId, 31);
      command.SetInputParameter(SqlDbType.Bit, "CreateMode", createMode);
      command.SetStringInputParameter(SqlDbType.NVarChar, "ChargeToClientLookupId", obj.ChargeToClientLookupId, 31);
      command.SetStringInputParameter(SqlDbType.NVarChar, "ActionCode", obj.ActionCode, 15);
      command.SetStringInputParameter(SqlDbType.NVarChar, "Category", obj.Category, 15);
      command.SetStringInputParameter(SqlDbType.NVarChar, "ChargeType", obj.ChargeType, 15);
      command.SetStringInputParameter(SqlDbType.NVarChar, "FailureCode", obj.FailureCode, 15);
      command.SetStringInputParameter(SqlDbType.NVarChar, "Priority", obj.Priority, 15);
      command.SetStringInputParameter(SqlDbType.NVarChar, "ReasonNotDone", obj.ReasonNotDone, 15);
      command.SetStringInputParameter(SqlDbType.NVarChar, "Section", obj.Section, 15);
      command.SetStringInputParameter(SqlDbType.NVarChar, "Type", obj.Type, 15);
      command.SetStringInputParameter(SqlDbType.NVarChar, "ApproveBy_PersonnelClientLookupId", obj.ApproveBy_PersonnelClientLookupId, 63);
      command.SetStringInputParameter(SqlDbType.NVarChar, "CloseBy_PersonnelClientLookupId", obj.CloseBy_PersonnelClientLookupId, 31);
      command.SetStringInputParameter(SqlDbType.NVarChar, "CompleteBy_PersonnelClientLookupId", obj.CompleteBy_PersonnelClientLookupId, 63);
      command.SetStringInputParameter(SqlDbType.NVarChar, "Creator_PersonnelClientLookupId", obj.Creator_PersonnelClientLookupId, 63);
      command.SetStringInputParameter(SqlDbType.NVarChar, "Planner_PersonnelClientLookupId", obj.Planner_PersonnelClientLookupId, 31);
      command.SetStringInputParameter(SqlDbType.NVarChar, "ReleaseBy_PersonnelClientLookupId", obj.ReleaseBy_PersonnelClientLookupId, 31);
      command.SetStringInputParameter(SqlDbType.NVarChar, "Requestor_PersonnelClientLookupId", obj.Requestor_PersonnelClientLookupId, 31);
      command.SetStringInputParameter(SqlDbType.NVarChar, "Scheduler_PersonnelClientLookupId", obj.Scheduler_PersonnelClientLookupId, 31);
      command.SetStringInputParameter(SqlDbType.NVarChar, "SignoffBy_PersonnelClientLookupId", obj.SignoffBy_PersonnelClientLookupId, 63);
      command.SetStringInputParameter(SqlDbType.NVarChar, "WorkAssigned_PersonnelClientLookupId", obj.WorkAssigned_PersonnelClientLookupId,63);


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
      // Process the RETURN_CODE parameter value
      retCode = (int)RETURN_CODE_parameter.Value;
      AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);

      // Return the result
      return records;
    }
  }
}