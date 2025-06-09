/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2016 by SOMAX Inc.
* Grid Layout 
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID  Person             Description
* =========== ======== ================== ==========================================================
* 2016-Aug-07 SOM-1049 Roger Lawton       Added new class to store/retrieve grid layout information
****************************************************************************************************
 */

using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using Database.SqlClient;

namespace Database.Business
{
    /// <summary>
    /// Business object that stores a record from the GridDataLayout table.InsertIntoDatabase
    /// </summary>
    public partial class b_GridDataLayout : DataBusinessBase
    {
        /// <summary>
        /// Retrieve GridDataLayout table record with specified client, site, grid name and user from the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="key">System.Guid that contains the key to use in the lookup</param>
        /// <param name="data">b_GridDataLayout[] that contains the results</param>
      public void RetrieveByGridSiteUser(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName )
        {
            Database.SqlClient.ProcessRow<b_GridDataLayout> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_GridDataLayout>(reader => { this.LoadFromDatabase(reader); return this; });
                Database.StoredProcedure.usp_GridDataLayout_RetrieveByGridSiteUser.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
                processRow = null;
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
    }
}
