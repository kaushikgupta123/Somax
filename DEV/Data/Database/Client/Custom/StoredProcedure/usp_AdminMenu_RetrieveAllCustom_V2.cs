using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

using Database.Business;
using Database.SqlClient;
using System.Collections.Generic;

namespace Database.StoredProcedure
{
    /// <summary>
    /// Access the usp_AdminMenu_RetrieveAllCustom_V2 stored procedure.
    /// </summary>
    public class usp_AdminMenu_RetrieveAllCustom_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_AdminMenu_RetrieveAllCustom_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_AdminMenu_RetrieveAllCustom_V2()
        {
        }

        /// <summary>
        /// Static method to call the usp_Menu_RetrieveAll stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="processRow">ProcessRow delegate containing method to call to process the row into an object.</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="UserType">string that identifies the user type the database</param>
        /// <returns>ArrayList containing the results of the query</returns>
        public static List<b_Menu> CallStoredProcedure(
            SqlCommand command,
           long callerUserInfoId,
           string callerUserName,
            b_Menu bMenu,           
            string UserType
        )
        {
            List<b_Menu> records = new List<b_Menu>();
            SqlDataReader reader = null;
            b_Menu record = null;
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;



            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);

            // Setup clientId parameter.
            //command.SetInputParameter(SqlDbType.BigInt, "ClientId", bMenu.ClientId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ResourceSet", bMenu.ResourceSet, 512);
            command.SetStringInputParameter(SqlDbType.NVarChar, "LocaleId", bMenu.LocaleId, 10);
            command.SetStringInputParameter(SqlDbType.NVarChar, "MenuType", bMenu.MenuType, 64);
            //command.SetInputParameter(SqlDbType.BigInt, "SiteId", bMenu.SiteId);
            command.SetInputParameter(SqlDbType.BigInt, "UserInfoId", bMenu.UserInfoId);
            //command.SetStringInputParameter(SqlDbType.NVarChar, "UserType", UserType, 31);
            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    // Process the current row into a record
                    record = (b_Menu)b_Menu.ProcessRowGetALLCustomAdmin(reader);

                    // Add the record to the list.
                    records.Add(record);
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