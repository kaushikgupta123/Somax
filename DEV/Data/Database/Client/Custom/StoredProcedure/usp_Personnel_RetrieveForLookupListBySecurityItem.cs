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
* ===========  ========= ================ =======================================================
* 2015-Feb-09  SOM-536   Roger Lawton     Change method of retrieving personnel
*                                         Added the UserSecurity parameter
*                                         Cannot read from the admin database in a client 
*                                         stored procedure
* 2021-Feb-09  V2-478    Roger Lawton     Changed to call a V2 stored procedure
*                                         Changed parameters
**************************************************************************************************
*/

using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.SqlClient;
using Database.Business;


namespace Database.StoredProcedure
{
    public class usp_Personnel_RetrieveForLookupListBySecurityItem
    {
        private static string STOREDPROCEDURE_NAME = "usp_Personnel_RetrieveForLookupListBySecurityItem_V2";

        public usp_Personnel_RetrieveForLookupListBySecurityItem()
        {
        }

        public static List<b_Personnel> CallStoredProcedure(SqlCommand command,long callerUserInfoId,string callerUserName,b_Personnel obj)
        {
            List<b_Personnel> records = new List<b_Personnel>();
            SqlDataReader reader = null;
            SqlParameter RETURN_CODE_parameter = null;

            int retCode = 0;
            // Setup command
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetStringInputParameter(SqlDbType.NVarChar,"Items",obj.SecItems,-1);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Props",obj.SecProps, -1);
            //command.SetInputParameter(SqlDbType.Bit, "ItemAccess", obj.ItemAccess);
            //command.SetInputParameter(SqlDbType.Bit, "ItemCreate", obj.ItemCreate);
            //command.SetInputParameter(SqlDbType.Bit, "ItemEdit", obj.ItemEdit);
            //command.SetInputParameter(SqlDbType.Bit, "ItemDelete", obj.ItemDelete);
            //command.SetStringInputParameter(SqlDbType.NVarChar, "ItemName", obj.ItemName, -1);
            //command.SetInputParameter(SqlDbType.Structured, "UserSecurity", obj.UserSec);
            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    //// Add the record to the list.
                    records.Add(b_Personnel.ProcessRowForFilterPersonnel(reader));
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
