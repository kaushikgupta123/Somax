/*
**************************************************************************************************
* PROPRIETARY DATA 
**************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc. and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
**************************************************************************************************
* Copyright (c) 2014 by SOMAX Inc.. All rights reserved. 
**************************************************************************************************
* Date         JIRA Item Person            Description
* ===========  ========= ================= =======================================================
* 2014-Oct-18  SOM-376   Roger Lawton      Added RetrieveByUserInfoId
**************************************************************************************************
*/

using System;
using System.Collections;
using System.Data.SqlClient;
using System.Text;

namespace Database.Business
{
    /// <summary>
    /// Business object that stores a record from the LoginInfo table.
    /// </summary>
    public partial class b_LoginInfo
    {

        public string Email { get; set; }

        public void RetrieveByClientIdFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            ref b_LoginInfo[] data
        )
        {
            Database.SqlClient.ProcessRow<b_LoginInfo> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_LoginInfo[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_LoginInfo>(reader => { b_LoginInfo obj = new b_LoginInfo(); obj.LoadFromDatabase(reader); return obj; });
                results = Database.StoredProcedure.usp_LoginInfo_RetrieveByClientId.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

                // Extract the results
                if (null != results)
                {
                    data = (b_LoginInfo[])results.ToArray(typeof(b_LoginInfo));
                }
                else
                {
                    data = new b_LoginInfo[0];
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

        public void RetrieveByResetPasswordCodeFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName
        )
        {
            Database.SqlClient.ProcessRow<b_LoginInfo> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_LoginInfo>(reader => { this.LoadFromDatabase(reader); return this; });
                Database.StoredProcedure.usp_LoginInfo_RetrieveByResetPasswordCode.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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

        public void RetrieveByUserInfoId(
      SqlConnection connection,
      SqlTransaction transaction,
      long callerUserInfoId,
      string callerUserName
  )
        {
            Database.SqlClient.ProcessRow<b_LoginInfo> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_LoginInfo>(reader => { this.LoadFromDatabase(reader); return this; });
                Database.StoredProcedure.usp_LoginInfo_RetrieveByUserInfoId.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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


        public void RetrieveBySearchCriteriaFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName
        )
        {
            Database.SqlClient.ProcessRow<b_LoginInfo> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_LoginInfo>(reader => { this.LoadFromDatabase(reader); return this; });
                Database.StoredProcedure.usp_LoginInfo_RetrieveBySearchCriteria.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, UserName, Email);

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

        public void RetrieveBySearchCriteriaFromDatabaseAdmin(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName
        )
        {
            Database.SqlClient.ProcessRow<b_LoginInfo> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_LoginInfo>(reader => { this.LoadFromDatabase(reader); return this; });
                Database.StoredProcedure.usp_AdminLoginInfo_RetrieveBySearchCriteria_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, UserName, Email);

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

        public void UpdateInDatabaseCustom(
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
                Database.StoredProcedure.usp_LoginInfo_UpdateByPK_Custom_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public void RetrieveByPKFromDatabaseAdmin(
    SqlConnection connection,
    SqlTransaction transaction,
    long callerUserInfoId,
string callerUserName
)
        {
            Database.SqlClient.ProcessRow<b_LoginInfo> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_LoginInfo>(reader => { this.LoadFromDatabase(reader); return this; });
                Database.StoredProcedure.usp_AdminLoginInfo_RetrieveByPK_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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
        public void UpdateByPKAdmin(
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
                Database.StoredProcedure.usp_AdminLoginInfo_UpdateByPK_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public void RetrieveLoginInfoByLoginInfoId(
  SqlConnection connection,
  SqlTransaction transaction,
  long callerUserInfoId,
string callerUserName
)
        {
            Database.SqlClient.ProcessRow<b_LoginInfo> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_LoginInfo>(reader => { this.LoadFromDatabase(reader); return this; });
                Database.StoredProcedure.usp_LoginInfo_RetrieveByLoginInfoId_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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
