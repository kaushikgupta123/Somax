/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2016 by SOMAX Inc.
* usp_Personnel_UpdateFromUserInfo
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID  Person             Description
* =========== ======== ================== =========================================================
* 2016-Oct-31 SOM-642  Roger Lawton       Update personnel info if appropriate
****************************************************************************************************
*/


using System.Data;
using System.Data.SqlClient;
using Database.SqlClient;
using Database.Business;

namespace Database.StoredProcedure
{

  /// <summary>
  /// Access the usp_Personnel_UpdateFromUserInfo stored procedure.
  /// </summary>
  public class usp_Personnel_UpdateFromUserInfo
  {
    /// <summary>
    /// Constants.
    /// </summary>
    private static string STOREDPROCEDURE_NAME = "usp_Personnel_UpdateFromUserInfo";

    /// <summary>
    /// Default constructor.
    /// </summary>
    public usp_Personnel_UpdateFromUserInfo()
    {
    }

    /// <summary>
    /// Static method to call the usp_Personnel_UpdateByPK stored procedure using SqlClient.
    /// </summary>
    /// <param name="command">SqlCommand object to use to call the stored procedure</param>
    /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
    /// <param name="callerUserName">string that identifies the user calling the database</param>
    /// <param name="updateIndexOut">int that contains the value of the @UpdateIndexOut parameter</param>
    public static void CallStoredProcedure(
        SqlCommand command,
        long callerUserInfoId,
        string callerUserName,
        b_Personnel obj
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
      command.SetInputParameter(SqlDbType.BigInt, "UserInfoId", obj.UserInfoId);
      command.SetStringInputParameter(SqlDbType.NVarChar, "NameFirst", obj.NameFirst, 31);
      command.SetStringInputParameter(SqlDbType.NVarChar, "NameLast", obj.NameLast, 31);
      command.SetStringInputParameter(SqlDbType.NVarChar, "NameMiddle", obj.NameMiddle, 31);
      command.SetStringInputParameter(SqlDbType.NVarChar, "Email", obj.Email, 255);
      command.SetInputParameter(SqlDbType.Bit,"InactiveFlag",obj.InactiveFlag);
      // Execute stored procedure.
      command.ExecuteNonQuery();
      retCode = (int)RETURN_CODE_parameter.Value;
      AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
    }
  }
}