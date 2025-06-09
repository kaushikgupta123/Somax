/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2011 by SOMAX Inc.
 * All rights reserved. 
 ******************************************************************************
 * Date        Task ID   Person             Description
 * =========== ======== =================== ===================================
 * 2015-Oct-14          Indus Net        Multiple Site User
 ******************************************************************************
 */

using System.Data;
using System.Data.SqlClient;
using Database.SqlClient;
using Database.Business;

namespace Database.StoredProcedure
{

  /// <summary>
  /// Access the usp_Personnel_UpdateExtended stored procedure.
  /// </summary>
  public class usp_Personnel_UpdateForMultiUserSite
  {
    /// <summary>
    /// Constants.
    /// </summary>
      private static string STOREDPROCEDURE_NAME = "usp_Personnel_UpdateForMultiUserSite";

    /// <summary>
    /// Default constructor.
    /// </summary>
      public usp_Personnel_UpdateForMultiUserSite()
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
      SqlParameter updateIndexOut_parameter = null;
      int retCode = 0;

      // Setup command.
      command.SetProcName(STOREDPROCEDURE_NAME);
      RETURN_CODE_parameter = command.GetReturnCodeParameter();
      command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
      command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
      command.SetInputParameter(SqlDbType.BigInt, "UserInfoId", obj.UserInfoId);
      command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);      
      command.SetStringInputParameter(SqlDbType.NVarChar, "ClientLookupId", obj.ClientLookupId, 63);
      command.SetStringInputParameter(SqlDbType.NVarChar, "NameFirst", obj.NameFirst, 31);
      command.SetStringInputParameter(SqlDbType.NVarChar, "NameLast", obj.NameLast, 31);
      command.SetStringInputParameter(SqlDbType.NVarChar, "NameMiddle", obj.NameMiddle, 31);
      command.SetInputParameter(SqlDbType.Int, "UpdateIndex", obj.UpdateIndex);
    
      // Setup updateIndexOut parameter.
      updateIndexOut_parameter = command.Parameters.Add("@UpdateIndexOut", SqlDbType.Int);
      updateIndexOut_parameter.Direction = ParameterDirection.Output;

      // Execute stored procedure.
      command.ExecuteNonQuery();

      obj.UpdateIndex = (int)updateIndexOut_parameter.Value;

      // Process the RETURN_CODE parameter value
      retCode = (int)RETURN_CODE_parameter.Value;
      AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
    }
  }
}