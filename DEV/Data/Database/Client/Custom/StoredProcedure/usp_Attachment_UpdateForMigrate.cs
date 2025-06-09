/*
**************************************************************************************************
* PROPRIETARY DATA 
**************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc. and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
**************************************************************************************************
* b_Attachment.cs - Custom Business Object
**************************************************************************************************
* Copyright (c) 2013-2019 by SOMAX Inc.. All rights reserved. 
**************************************************************************************************
* Date        JIRA Item Person         Description
* =========== ========= ============= ==========================================================
* 2019-Apr-15 SOM-1650  Roger Lawton  Update the Attachment record for Migration
**************************************************************************************************
*/

using System;
using System.Data;
using System.Data.SqlClient;

using Database.Business;
using Database.SqlClient;
namespace Database.StoredProcedure
{
  /// <summary>
  /// Access the usp_Attachment_UpdateForMigrate stored procedure.
  /// </summary>
  public class usp_Attachment_UpdateForMigrate
  {
    /// <summary>
    /// Constants.
    /// </summary>
    private static string STOREDPROCEDURE_NAME = "usp_Attachment_UpdateForMigrate";

    /// <summary>
    /// Default constructor.
    /// </summary>
    public usp_Attachment_UpdateForMigrate()
    {
    }

    /// <summary>
    /// Static method to call the usp_Attachment_UpdateForMigrate stored procedure using SqlClient.
    /// </summary>
    /// <param name="command">SqlCommand object to use to call the stored procedure</param>
    /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
    /// <param name="callerUserName">string that identifies the user calling the database</param>
    public static void CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_Attachment obj
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
     //command.SetInputParameter(SqlDbType.BigInt, "FileInfoId", obj.FileInfoId);
      command.SetStringInputParameter(SqlDbType.NVarChar, "URL", obj.AttachmentURL, 511);

      // Execute stored procedure.
      command.ExecuteNonQuery();

      // Process the RETURN_CODE parameter value
      retCode = (int)RETURN_CODE_parameter.Value;
      AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
    }
  }
}
