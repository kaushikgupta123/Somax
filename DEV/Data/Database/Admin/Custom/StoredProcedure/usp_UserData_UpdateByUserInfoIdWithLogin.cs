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
* Date         JIRA Item Person                 Description
* ===========  ========= ====================== =================================================
* 2014-Oct-26  SOM-384   Roger Lawton           Modified - changed to four types of users
*                                               Not keeping track of phone or tablet users
**************************************************************************************************
*/

using System.Data;
using System.Data.SqlClient;

using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_UserData_UpdateByUserInfoIdWithLogin
    {
        private static string STOREDPROCEDURE_NAME = "usp_UserData_UpdateByUserInfoIdWithLogin";

        public usp_UserData_UpdateByUserInfoIdWithLogin()
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
            command.SetInputParameter(SqlDbType.BigInt, "UserInfoId",obj.UserInfoId);
            command.SetInputParameter(SqlDbType.BigInt, "DefaultSiteId", obj.DefaultSiteId);
            command.SetStringInputParameter(SqlDbType.VarChar, "FirstName", obj.FirstName, 63);
            command.SetStringInputParameter(SqlDbType.VarChar, "LastName", obj.LastName, 63);
            command.SetStringInputParameter(SqlDbType.VarChar, "MiddleName", obj.MiddleName, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Email", obj.Email, 255);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Localization", obj.Localization, 255);
            command.SetStringInputParameter(SqlDbType.NVarChar, "UIConfiguration", obj.UIConfiguration, 255);
            command.SetStringInputParameter(SqlDbType.NVarChar, "TimeZone", obj.TimeZone, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "UserType", obj.UserType, 31);
            command.SetInputParameter(SqlDbType.BigInt, "SecurityProfileId", obj.SecurityProfileId);
            command.SetInputParameter(SqlDbType.Int, "ResultsPerPage", obj.ResultsPerPage);
            command.SetInputParameter(SqlDbType.Bit, "IsSuperUser", obj.IsSuperUser);
            command.SetInputParameter(SqlDbType.Bit, "IsPasswordTemporary", obj.IsPasswordTemporary);
            command.SetInputParameter(SqlDbType.BigInt, "UserUpdateIndex", obj.UserUpdateIndex);
            command.SetOutputParameter(SqlDbType.BigInt, "UserUpdateIndexOut");

            // Setup command.          
            command.SetInputParameter(SqlDbType.BigInt, "LoginInfoId",obj.LoginInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "UserName", obj.UserName, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "TempPassword", obj.TempPassword, 255);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Password", obj.Password, 255);
            command.SetStringInputParameter(SqlDbType.VarChar, "SecurityQuestion", obj.SecurityQuestion, 511);
            command.SetStringInputParameter(SqlDbType.VarChar, "SecurityResponse", obj.SecurityResponse, 15);
            command.SetInputParameter(SqlDbType.Bit, "IsActive", obj.IsActive);
            command.SetInputParameter(SqlDbType.BigInt, "LoginUpdateIndex", obj.LoginUpdateIndex);
            command.SetOutputParameter(SqlDbType.BigInt, "LoginUpdateIndexOut");

            command.SetInputParameter(SqlDbType.BigInt, "ClientUpdateIndex", obj.ClientUpdateIndex);
            command.SetOutputParameter(SqlDbType.BigInt, "ClientUpdateIndexOut");

            command.SetOutputParameter(SqlDbType.BigInt, "AppUserOut");
            command.SetOutputParameter(SqlDbType.BigInt, "LimitedUserOut");
            command.SetOutputParameter(SqlDbType.BigInt, "WorkRequestUserOut");
            command.SetOutputParameter(SqlDbType.BigInt, "SanitationUserOut");
            command.SetOutputParameter(SqlDbType.BigInt, "SuperUserOut");

            // Execute stored procedure.
            command.ExecuteNonQuery();

            obj.UserUpdateIndex = (long)command.Parameters["@UserUpdateIndexOut"].Value;
            obj.LoginUpdateIndex = (long)command.Parameters["@LoginUpdateIndexOut"].Value;
            obj.ClientUpdateIndex = (long)command.Parameters["@ClientUpdateIndexOut"].Value;
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
