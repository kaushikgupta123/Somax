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
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using Database.SqlClient;
namespace Database.StoredProcedure
{
  public class usp_PartMasterImport_ProcessInterface
  {
    /// <summary>
    /// Constants.
    /// </summary>
    private static string STOREDPROCEDURE_NAME = "usp_PartMasterImport_ProcessInterface";

    /// <summary>
    /// Default constructor.
    /// </summary>
    public usp_PartMasterImport_ProcessInterface()
    {

    }


    public static void CallStoredProcedure(
        SqlCommand command,
        long CallerUserPersonnelId,
        string callerUserName,
        b_PartMasterImport obj
    )
    {
      SqlParameter RETURN_CODE_parameter = null;
      SqlParameter error_message_count = null;
      int retCode = 0;

      // Setup command.
      command.SetProcName(STOREDPROCEDURE_NAME);
      RETURN_CODE_parameter = command.GetReturnCodeParameter();
      command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", CallerUserPersonnelId);
      command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
      command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
      command.SetInputParameter(SqlDbType.BigInt, "PartMasterImportId", obj.PartMasterImportId);

      // Setup error_message_count parameter.
      error_message_count = command.Parameters.Add("@error_message_count", SqlDbType.Int);
      error_message_count.Direction = ParameterDirection.Output;

      // Execute stored procedure.
      command.ExecuteNonQuery();

      obj.error_message_count = (int)error_message_count.Value;

      // Process the RETURN_CODE parameter value
      retCode = (int)RETURN_CODE_parameter.Value;
      AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
    }
  }
}
