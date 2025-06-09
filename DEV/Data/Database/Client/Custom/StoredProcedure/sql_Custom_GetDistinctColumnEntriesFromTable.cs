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

using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Database.StoredProcedure
{
    /// <summary>
    /// Access the usp_Account_RetrieveBySearchCriteria stored procedure.
    /// </summary>
    public class sql_Custom_GetDistinctColumnEntriesFromTable
    {
        public static List<string> CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            long clientId,
            string table,
            string column
        )
        {
            List<string> results = new List<string>();
            SqlDataReader reader = null;

            command.CommandText = string.Format("SELECT DISTINCT {0} FROM {1} WHERE ClientId={2}", column, table, clientId);
            command.CommandType = CommandType.Text;

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    //// Add the record to the list.
                    results.Add(reader.GetValue(0).ToString());
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

            return results;
        }
    }
}
