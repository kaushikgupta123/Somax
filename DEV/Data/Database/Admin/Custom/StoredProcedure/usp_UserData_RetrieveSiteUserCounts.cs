/*
**************************************************************************************************
* PROPRIETARY DATA 
**************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc. and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
**************************************************************************************************
* Copyright (c) 2014 by SOMAX Inc.. All rights reserved. 
**************************************************************************************************
* Date         JIRA Item Person           Description
* ===========  ========= ================ ========================================================
* 2014-Oct-21  SOM-384   Roger Lawton     Added to retrievesiteusercounts
**************************************************************************************************
*/

using System.Data;
using System.Data.SqlClient;

using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
  public class usp_UserData_RetrieveSiteUserCounts
  {
    private static string STOREDPROCEDURE_NAME = "usp_UserData_RetrieveSiteUserCounts";

    public usp_UserData_RetrieveSiteUserCounts()
    {

    }
    public static void CallStoredProcedure(
        SqlCommand command,
        long callerUserInfoId,
        string callerUserName,
        b_UserDetails obj
    )
    {
      SqlParameter RETURN_CODE_parameter = null;
      int retCode = 0;

      command.SetProcName(STOREDPROCEDURE_NAME);
      RETURN_CODE_parameter = command.GetReturnCodeParameter();
      command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
      command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
      command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
      command.SetInputParameter(SqlDbType.BigInt, "DefaultSiteId", obj.DefaultSiteId);
      // Output parameters
      //command.SetOutputParameter(SqlDbType.BigInt, "TabletUserOut");
      //command.SetOutputParameter(SqlDbType.BigInt, "PhoneUserOut");
      command.SetOutputParameter(SqlDbType.BigInt, "AppUserOut");
      command.SetOutputParameter(SqlDbType.BigInt, "LimitedUserOut");
      command.SetOutputParameter(SqlDbType.BigInt, "WorkRequestUserOut");
      command.SetOutputParameter(SqlDbType.BigInt, "SanitationUserOut");
      command.SetOutputParameter(SqlDbType.BigInt, "SuperUserOut");

      // Execute stored procedure.
      command.ExecuteNonQuery();

      // Fill in the counts
      //obj.CountTabletUser = (long)command.Parameters["@TabletUserOut"].Value;
      //obj.CountPhoneUser = (long)command.Parameters["@PhoneUserOut"].Value;
      obj.CountWebAppUser = (long)command.Parameters["@AppUserOut"].Value;
      obj.CountLimitedUser = (long)command.Parameters["@LimitedUserOut"].Value;
      obj.CountWorkRequestUser = (long)command.Parameters["@WorkRequestUserOut"].Value;
      obj.CountSanitationUser = (long)command.Parameters["@SanitationUserOut"].Value;
      obj.CountSuperUser = (long)command.Parameters["@SuperUserOut"].Value;

      // Process the RETURN_CODE parameter value
      retCode = (int)RETURN_CODE_parameter.Value;
      AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
    }
  }
}
