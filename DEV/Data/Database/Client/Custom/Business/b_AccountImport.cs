
using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.Business
{
    public partial class b_AccountImport
    {
        public string AccountNumber { get; set; }
        public string Status { get; set; }
        public int spErrCode { get; set; }
        public void AccountImportValidation(
      SqlConnection connection,
      SqlTransaction transaction,
      long callerUserInfoId,
      string callerUserName,
      ref List<b_StoredProcValidationError> data

  )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_StoredProcValidationError> results = null;
            data = new List<b_StoredProcValidationError>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_AccountImport_Validation.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_StoredProcValidationError>();
                }

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



        public void AccountImportProcessImport(
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
                Database.StoredProcedure.usp_AccountImport_Process.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        //public void AccountImportRetrieveByAccountNumber(SqlConnection connection,
        //   SqlTransaction transaction,
        //    long callerUserInfoId,
        //    string callerUserName)
        //{
        //    Database.SqlClient.ProcessRow<b_AccountImport> processRow = null;
        //    SqlCommand command = null;
        //    string message = String.Empty;
        //    try
        //    {
        //        // Create the command to use in calling the stored procedures
        //        command = new SqlCommand();
        //        command.Connection = connection;
        //        command.Transaction = transaction;

        //        // Call the stored procedure to retrieve the data
        //        processRow = new Database.SqlClient.ProcessRow<b_AccountImport>(reader => { this.LoadFromDatabase(reader); return this; });
        //        StoredProcedure.usp_AccountImport_RetrieveByAccountNumber.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

        //    }
        //    finally
        //    {
        //        if (null != command)
        //        {
        //            command.Dispose();
        //            command = null;
        //        }
        //        processRow = null;
        //        message = String.Empty;
        //        callerUserInfoId = 0;
        //        callerUserName = String.Empty;
        //    }
        //}


        public void AccountImportRetrieveByExAccountId(SqlConnection connection,
           SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName)
        {
            Database.SqlClient.ProcessRow<b_AccountImport> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;
            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_AccountImport>(reader => { this.LoadFromDatabase(reader); return this; });
                StoredProcedure.usp_AccountImport_RetrieveByExAccountId.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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


        public void InsertIntoDatabaseCustom(
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
                Database.StoredProcedure.usp_AccountImport_CreateCustom.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
                Database.StoredProcedure.usp_AccountImport_UpdateByPKCustom.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

    }
}

