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
using System.Text;
using System.Data.SqlClient;
using Database.SqlClient;

namespace Database.Business
{
    /// <summary>
    /// Business object that stores a record from the LocalizedList table.InsertIntoDatabase
    /// </summary>
    public partial class b_LocalizedList : DataBusinessBase
    {
        public long SiteId { get; set; }
        #region @APM,@CMMS,@Sanit
        public bool? APM { get; set; }
        public bool? CMMS { get; set; }

        public bool? Sanit { get; set; }
        public bool? Fleet { get; set; }        // V2-475
        public bool? UsePartMaster { get; set; }

        public bool? UseShoppingCart { get; set; }
        public string PackageLevel { get; set; }
        #endregion

        /// <summary>
        /// Retrieve all LocalizedList table records for a specific language and culture
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="data">b_LocalizedList[] that contains the results</param>
        public void RetrieveLanguageSpecificLocalizedList(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, ref b_LocalizedList[] data)
        {
            Database.SqlClient.ProcessRow<b_LocalizedList> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_LocalizedList[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_LocalizedList>(reader => { b_LocalizedList obj = new b_LocalizedList(); obj.LoadFromDatabase(reader); return obj; });
                results = Database.StoredProcedure.usp_LocalizedList_RetrieveLanguageSpecific.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

                // Extract the results
                if (null != results)
                {
                    data = (b_LocalizedList[])results.ToArray(typeof(b_LocalizedList));
                }
                else
                {
                    data = new b_LocalizedList[0];
                }

                // Clear the results collection
                if (null != results)
                {
                    results.Clear();
                    results = null;
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
                processRow = null;
                results = null;
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
    }
}
