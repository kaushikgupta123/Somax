/*
***************************************************************************************************
* PROPRIETARY DATA 
***************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
***************************************************************************************************
* Copyright (c) 2011 by SOMAX Inc.
* All rights reserved. 
***************************************************************************************************
* Date        Task ID   Person             Description
* =========== ======== =================== ========================================================
* 2014-Aug-01 SOM_246   Roger Lawton       Added 
***************************************************************************************************
*/

using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    /// <summary>
    /// Access the usp_LocalizedList_RetrieveLanguageSpecific stored procedure.
    /// </summary>
    public class usp_LocalizedList_RetrieveLanguageSpecific
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_LocalizedList_RetrieveLanguageSpecific_V2";
        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_LocalizedList_RetrieveLanguageSpecific()
        {
        }

        /// <summary>
        /// Static method to call the usp_LocalizedList_RetrieveAll stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="processRow">ProcessRow delegate containing method to call to process the row into an object.</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <returns>ArrayList containing the results of the query</returns>
        public static ArrayList CallStoredProcedure (
            SqlCommand command,
            Database.SqlClient.ProcessRow<b_LocalizedList> processRow,
            long callerUserInfoId,
            string callerUserName,
            b_LocalizedList obj
        )
        {
            ArrayList records = new ArrayList();
            SqlDataReader reader = null;
            b_LocalizedList record = null;
            SqlParameter RETURN_CODE_parameter = null;
            //SqlParameter        clientId_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Language", obj.Language, 3);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Culture", obj.Culture, 3);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetInputParameter(SqlDbType.Bit, "APM", obj.APM);
            command.SetInputParameter(SqlDbType.Bit, "CMMS", obj.CMMS);
            command.SetInputParameter(SqlDbType.Bit, "Sanit", obj.Sanit);
            command.SetInputParameter(SqlDbType.Bit, "Fleet", obj.Fleet);
            command.SetInputParameter(SqlDbType.Bit, "UsePartMaster", obj.UsePartMaster);
            command.SetInputParameter(SqlDbType.Bit, "UseShoppingCart", obj.UseShoppingCart);
            command.SetStringInputParameter(SqlDbType.NVarChar, "PackageLevel", obj.PackageLevel, 15);
            
            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read()) 
                {
                    // Process the current row into a record
                    record = processRow(reader);

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
            retCode = (int) RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);

            // Return the result
            return records;
        }
    }
}