/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2014 by SOMAX Inc.
* All rights reserved. 
****************************************************************************************************
* NOTES
* 2014-Nov-23 - This method should NOT be needed - Lookup lists should not have to be retrieved 
*               by callbacks as they should be retrieved completely the first time  
****************************************************************************************************
* Date        Task ID   Person             Description
* =========== ======== =================== =========================================================
* 2014-Nov-23 SOM-453  Roger Lawton        Added method to retrieve list with inactive items excluded
****************************************************************************************************
 */

using System;
using System.Collections;
using System.Data.SqlClient;
using System.Collections.Generic;


namespace Database.Business
{
   public partial class b_LookupList
    {
       public string FilterText { get; set; }
       public int FilterStartIndex { get; set; }
       public int FilterEndIndex { get; set; }

        public static KeyValuePair<string,string> ProcessRowForFilterLookUpList(SqlDataReader reader)
        {
            int i=0;

            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(reader.GetString(i++), reader.GetString(i++));

            return tmp;
        }

        public void RetrieveLookupListByFilterText(
         SqlConnection connection,
         SqlTransaction transaction,
         long callerUserInfoId,
         string callerUserName,
         long ClientId,
        ref List<KeyValuePair<String,string>> results            
        )
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data

                results = Database.StoredProcedure.usp_LookupList_RetrieveByFilterText.CallStoredProcedure(command, callerUserInfoId, callerUserName, ClientId, FilterText, FilterStartIndex, FilterEndIndex,Filter, ListName);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }

        public void CreateTemplate(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
           string callerUserName
        )
        {
            SqlCommand command = null;
            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                 Database.StoredProcedure.usp_LookupList_CreateTemplate.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
            }

          
        }

        // SOM-453
        /// <summary>
        /// Retrieve all LookupList table records represented by this object in the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="data">b_LookupList[] that contains the results</param>
        public void RetrieveAllActiveFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName,
            ref b_LookupList[] data
        )
        {
          Database.SqlClient.ProcessRow<b_LookupList> processRow = null;
          ArrayList results = null;
          SqlCommand command = null;
          string message = String.Empty;

          // Initialize the results
          data = new b_LookupList[0];

          try
          {
            // Create the command to use in calling the stored procedures
            command = new SqlCommand();
            command.Connection = connection;
            command.Transaction = transaction;

            // Call the stored procedure to retrieve the data
            processRow = new Database.SqlClient.ProcessRow<b_LookupList>(reader => { b_LookupList obj = new b_LookupList(); obj.LoadFromDatabase(reader); return obj; });
            results = Database.StoredProcedure.usp_LookupList_RetrieveAllActive.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, ClientId, SiteId);

            // Extract the results
            if (null != results)
            {
              data = (b_LookupList[])results.ToArray(typeof(b_LookupList));
            }
            else
            {
              data = new b_LookupList[0];
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

        public void GetLookUpListByListName(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            string ListName,
            ref b_LookupList[] data
        )
        {
          Database.SqlClient.ProcessRow<b_LookupList> processRow = null;
          ArrayList results = null;
          SqlCommand command = null;
          string message = String.Empty;

          // Initialize the results
          data = new b_LookupList[0];

          try
          {
            // Create the command to use in calling the stored procedures
            command = new SqlCommand();
            command.Connection = connection;
            command.Transaction = transaction;

            // Call the stored procedure to retrieve the data
            processRow = new Database.SqlClient.ProcessRow<b_LookupList>(reader => { b_LookupList obj = new b_LookupList(); obj.LoadFromDatabase(reader); return obj; });
            results = Database.StoredProcedure.usp_LookUpList_GetLookUpListByListName.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, ClientId, SiteId, ListName);

            // Extract the results
            if (null != results)
            {
              data = (b_LookupList[])results.ToArray(typeof(b_LookupList));
            }
            else
            {
              data = new b_LookupList[0];
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

        public int DeleteByLookupListId_V2(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
           string callerUserName
        )
        {
            int count = 0;
            SqlCommand command = null;
            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                count = Database.StoredProcedure.usp_LookupList_DeleteByLookupListId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
            }

            return count;
        }
    }
}
