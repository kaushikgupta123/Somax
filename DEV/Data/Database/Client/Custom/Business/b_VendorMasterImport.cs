
using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.Business
{
    public partial class b_VendorMasterImport
    {

        public string VendorNumber { get; set; }
        public string Status { get; set; }
        public int error_message_count { get; set; }
        public void VendorMasterImportValidate(
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
                results = Database.StoredProcedure.usp_VendorMasterImport_Validation.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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



        public void VendorMasterImportProcess(
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
                Database.StoredProcedure.usp_VendorMasterImport_Process.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        //public void VendorImportRetrieveByVendorNumber(SqlConnection connection,
        //  SqlTransaction transaction,
        //   long callerUserInfoId,
        //   string callerUserName)
        //{
        //  Database.SqlClient.ProcessRow<b_VendorMasterImport> processRow = null;
        //  SqlCommand command = null;
        //  string message = String.Empty;
        //  try
        //  {
        //    // Create the command to use in calling the stored procedures
        //    command = new SqlCommand();
        //    command.Connection = connection;
        //    command.Transaction = transaction;

        //    // Call the stored procedure to retrieve the data
        //    processRow = new Database.SqlClient.ProcessRow<b_VendorMasterImport>(reader => { this.LoadFromDatabase(reader); return this; });
        //    StoredProcedure.usp_VendorMasterImport_RetrieveByVendorNumber.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

        //  }
        //  finally
        //  {
        //    if (null != command)
        //    {
        //      command.Dispose();
        //      command = null;
        //    }
        //    processRow = null;
        //    message = String.Empty;
        //    callerUserInfoId = 0;
        //    callerUserName = String.Empty;
        //  }
        //}

        public void VendorImportRetrieveByExVendorId(SqlConnection connection,
         SqlTransaction transaction,
          long callerUserInfoId,
          string callerUserName)
        {
            Database.SqlClient.ProcessRow<b_VendorMasterImport> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;
            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_VendorMasterImport>(reader => { this.LoadFromDatabase(reader); return this; });
                StoredProcedure.usp_VendorMasterImport_RetrieveByExVendorId.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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
                Database.StoredProcedure.usp_VendorMasterImport_CreateCustom.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
                Database.StoredProcedure.usp_VendorMasterImport_UpdateByPKCustom.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public void vendorMasterImport_ProcessInterface(
           SqlConnection connection,
           SqlTransaction transaction,
           long CallerUserPersonnelId,
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
                Database.StoredProcedure.usp_VendorMasterImport_ProcessInterface.CallStoredProcedure(command, CallerUserPersonnelId, callerUserName, this);
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



        public void RetrieveByClientIdVendorExIdVendorExSiteId(
              SqlConnection connection,
              SqlTransaction transaction,
              long callerUserInfoId,
              string callerUserName,
              string _ClientLookupID)
        {
            Database.SqlClient.ProcessRow<b_VendorMasterImport> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_VendorMasterImport>(reader => { this.LoadFromDatabase(reader); return this; });
                Database.StoredProcedure.usp_VendorMasterImport_RetrieveByClientIdVendorExIdVendorExSiteId.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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

