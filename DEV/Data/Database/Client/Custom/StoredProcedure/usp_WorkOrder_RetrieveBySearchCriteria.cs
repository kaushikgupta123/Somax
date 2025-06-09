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
 * 2012-Feb-02 20120001 Roger Lawton        Changed to support additional columns on search
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
    /// Access the usp_WorkOrder_RetrieveBySearchCriteria stored procedure.
    /// </summary>
    public class usp_WorkOrder_RetrieveBySearchCriteria
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_WorkOrder_RetrieveBySearchCriteria";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_WorkOrder_RetrieveBySearchCriteria()
        {
        }

        /// <summary>
        /// Static method to call the usp_Equipment_RetrieveAll stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="processRow">ProcessRow delegate containing method to call to process the row into an object.</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <returns>ArrayList containing the results of the query</returns>
        public static List<b_WorkOrder> CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            long clientId,
            string query,
            string site,
            string area,
            string department,
            string source,
            string type,
            string status,
            string priority,
            string shift,
            string dateSelection,
            DateTime dateStart,
            DateTime dateEnd,
            string column,
            string searchText,
            int page,
            int resultsPerPage,
            bool useLike,
            string orderColumn,
            string orderDirection,
            out int ResultCount
        )
        {
            List<b_WorkOrder> records = new List<b_WorkOrder>();
            SqlDataReader reader = null;
            b_WorkOrder record = null;
            SqlParameter RETURN_CODE_parameter = null;

            int retCode = 0;

            ResultCount = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", clientId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Query", query, 256);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Site", site, 256);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Area", area, 256);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Department", department, 256);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Source", source, 256);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Type", type, 256);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Status", status, 256);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Priority", priority, 256);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Shift", shift, 256);
            command.SetStringInputParameter(SqlDbType.NVarChar, "DateSelection", dateSelection, 256);
            command.SetInputParameter(SqlDbType.DateTime2, "DateStart", dateStart);
            command.SetInputParameter(SqlDbType.DateTime2, "DateEnd", dateEnd);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Column", column, 256);
            command.SetStringInputParameter(SqlDbType.NVarChar, "SearchText", searchText, 256);
            command.SetInputParameter(SqlDbType.Int, "Page", page);
            command.SetInputParameter(SqlDbType.Int, "ResultsPerPage", resultsPerPage);
            command.SetInputParameter(SqlDbType.Bit, "UseLike", ((useLike) ? (0x1) : (0x0)));
            command.SetStringInputParameter(SqlDbType.NVarChar, "OrderColumn", orderColumn, 256);
            command.SetStringInputParameter(SqlDbType.NVarChar, "OrderDirection", orderDirection, 256);

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
                    record = (b_WorkOrder)b_WorkOrder.ProcessRow(reader);

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
