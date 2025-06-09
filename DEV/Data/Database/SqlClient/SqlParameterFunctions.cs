/*
***************************************************************************************************
* PROPRIETARY DATA 
***************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc. and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
***************************************************************************************************
* Copyright (c) 2016 by SOMAX Inc.. All rights reserved. 
***************************************************************************************************
* Date        JIRA No  Person         Description
* =========== ======== ============== =============================================================
* 2016-Sep-01 SOM-1081 Roger Lawton   Change SetStringInputParameter to allow -1 for size 
*                                       to support NvarChar(MAX)
***************************************************************************************************
*/
using System;
using System.Data;
using System.Data.SqlClient;

namespace Database.SqlClient
{
    public static class SqlParameterFunctions
    {
        public static void SetProcName(this SqlCommand command, string procName)
        {
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = procName;
            command.Parameters.Clear();
        }

        public static SqlParameter GetReturnCodeParameter(this SqlCommand command)
        {
            SqlParameter param = command.Parameters.Add("RETURN_CODE", SqlDbType.Int);
            param.Direction = ParameterDirection.ReturnValue;
            param.Value = 0;
            return param;
        }

        /// <summary>
        /// Sets a non-string input parameter to be passed to a stored procedure. Note that the "@" symbol is automatically prepended.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="val"></param>
        public static void SetInputParameter(this SqlCommand command, SqlDbType type, string name, object val)
        {
            if (type == SqlDbType.VarChar || type == SqlDbType.NVarChar || type == SqlDbType.Text || type == SqlDbType.NText)
            {
                throw new Exception("SetInputParameter should not be used on strings. Use SetStringInputParameter instead.");
            }

            SqlParameter param = command.Parameters.Add("@" + name, type);
            param.Direction = ParameterDirection.Input;
            param.Value = val ?? DBNull.Value;
        }

        /// <summary>
        /// Sets a string input parameter to be passed to a stored procedure. Note that the "@" symbol is automatically prepended.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <param name="maxLength"></param>
        public static void SetStringInputParameter(this SqlCommand command, SqlDbType type, string name, string val, int maxLength)
        {
            SqlParameter param = command.Parameters.Add("@" + name, type);
            param.Direction = ParameterDirection.Input;
            if (string.IsNullOrWhiteSpace(val))
            {
                param.Value = "";
            }
            // SOM-1081
            else if (maxLength == -1) // NvarChar(Max)
            {
              param.Value = val.Trim(); 
            }
            else
            {
                param.Size = System.Math.Min(val.Length, maxLength);
                param.Value = val.Substring(0, System.Math.Min(val.Length, maxLength));
            }
        }

        /// <summary>
        /// Returns an output parameter object to catch 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static void SetOutputParameter(this SqlCommand command, SqlDbType type, string name)
        {
            SqlParameter param = command.Parameters.Add("@" + name, type);
            param.Direction = ParameterDirection.Output;
        }


    }
}
