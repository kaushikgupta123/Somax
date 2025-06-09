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
 */

using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    /// <summary>
    /// Access the usp_Personnel_RetrieveLookupListBySearchCriteria stored procedure.
    /// </summary>
    public class usp_Personnel_RetrieveLookupListBySearchCriteria
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_Personnel_RetrieveLookupListBySearchCriteria";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_Personnel_RetrieveLookupListBySearchCriteria()
        {
        }

        /// <summary>
        /// Static method to call the usp_Personnel_RetrieveLookupListBySearchCriteria stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="processRow">ProcessRow delegate containing method to call to process the row into an object.</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <returns>ArrayList containing the results of the query</returns>
        public static List<b_Personnel> CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            long clientId,
            string clientLookupId,
            string nameFull,
            string email,
            long siteId,
            int page,
            int resultsPerPage,
            string orderColumn,
            string orderDirection,
            out int ResultCount
        )
        {
            List<b_Personnel> records = new List<b_Personnel>();
            SqlDataReader reader = null;
            b_Personnel record = null;
            SqlParameter RETURN_CODE_parameter = null;

            int retCode = 0;

            ResultCount = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", clientId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ClientLookupId", clientLookupId, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "NameFull", nameFull, 127);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Email", email, 255);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", siteId);
            command.SetInputParameter(SqlDbType.Int, "Page", page);
            command.SetInputParameter(SqlDbType.Int, "ResultsPerPage", resultsPerPage);
            command.SetStringInputParameter(SqlDbType.VarChar, "OrderColumn", orderColumn, 256);
            command.SetStringInputParameter(SqlDbType.VarChar, "OrderDirection", orderDirection, 256);

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Get the result count
                while (reader.Read())
                {
                    ResultCount = reader.GetInt32(0);
                }

                reader.NextResult();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Process the current row into a record
                    record = (b_Personnel)b_Personnel.ProcessRow(reader);

                    //// Add the record to the list.
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
