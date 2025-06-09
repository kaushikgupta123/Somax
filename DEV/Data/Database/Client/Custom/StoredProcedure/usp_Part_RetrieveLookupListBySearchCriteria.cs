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

using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

using Database;
using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    /// <summary>
    /// Access the usp_Part_RetrieveLookupListBySearchCriteria stored procedure.
    /// </summary>
    public class usp_Part_RetrieveLookupListBySearchCriteria
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_Part_RetrieveLookupListBySearchCriteria";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_Part_RetrieveLookupListBySearchCriteria()
        {
        }

        /// <summary>
        /// Static method to call the usp_Part_RetrieveLookupListBySearchCriteria stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="processRow">ProcessRow delegate containing method to call to process the row into an object.</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <returns>ArrayList containing the results of the query</returns>
        public static List<b_Part> CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            long clientId,
            string clientLookupId,
            string description,
            long siteId,
            string PartId,
            string Manufacturer,
            string ManufacturerId,
            string StockType,
            string UPCCode,
            int page,
            int resultsPerPage,
            string orderColumn,
            string orderDirection,
            out int ResultCount
        )
        {
            List<b_Part> records = new List<b_Part>();
            SqlDataReader reader = null;
            b_Part record = null;
            SqlParameter RETURN_CODE_parameter = null;

            int retCode = 0;

            ResultCount = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", clientId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ClientLookupId", clientLookupId, 70);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Description", description, 1073741823);
            //command.SetStringInputParameter(SqlDbType.NVarChar, "PartId", PartId, 63); 
            command.SetStringInputParameter(SqlDbType.NVarChar, "Manufacturer", Manufacturer, 63);// SOM-787-code uncomented on 31-08-2015
            command.SetStringInputParameter(SqlDbType.NVarChar, "ManufacturerId", ManufacturerId, 63);// SOM-1303 on 02-05-2017

            command.SetStringInputParameter(SqlDbType.NVarChar, "StockType", StockType, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "UPCCode", UPCCode, 31);
            
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
                    record = (b_Part)b_Part.ProcessRow(reader);

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
