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
* 2014-Oct-26  SOM-384   Roger Lawton           Modified - changed to four types of users
*                                               Not keeping track of phone or tablet users
**************************************************************************************************
*/

using System.Data;
using System.Data.SqlClient;
using Database.Business;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_UserData_ValidateUserUpdate
    {
         private static string STOREDPROCEDURE_NAME = "usp_UserData_ValidateUserUpdate";

         public usp_UserData_ValidateUserUpdate()
          {

          }
         public static List<b_StoredProcValidationError> CallStoredProcedure(
           SqlCommand command,
           long callerUserInfoId,
           string callerUserName,
           b_UserDetails obj
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
             command.SetInputParameter(SqlDbType.BigInt, "UserInfoId", obj.UserInfoId);
             command.SetInputParameter(SqlDbType.Bit, "SiteControled", obj.SiteControlled);
             command.SetInputParameter(SqlDbType.Bit, "IsSuperUser", obj.IsSuperUser);
             command.SetStringInputParameter(SqlDbType.NVarChar, "UserName", obj.UserName, 63);
             command.SetStringInputParameter(SqlDbType.NVarChar, "UserType", obj.UserType, 31);


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
             retCode = (int)RETURN_CODE_parameter.Value;
             AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);

             // Return the result
             return records;
         }



    }
}
