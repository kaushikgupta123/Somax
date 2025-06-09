/*
 *  Added By Indusnet Technologies
 */

using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Database.Business
{
    public partial class b_Department : DataBusinessBase
    {
        public bool Flag { get; set; }
                public void RetrieveAllTemplatesWithClient(
                SqlConnection connection,
                SqlTransaction transaction,
                long callerUserInfoId,
                string callerUserName,
                ref List<b_Department> data
                    )
                            {
                                Database.SqlClient.ProcessRow<b_Department> processRow = null;
                                List<b_Department> results = null;
                                SqlCommand command = null;
                                string message = String.Empty;

                                // Initialize the results
                                data = new List<b_Department>();

                                try
                                {
                                    // Create the command to use in calling the stored procedures
                                    command = new SqlCommand();
                                    command.Connection = connection;
                                    command.Transaction = transaction;

                                    // Call the stored procedure to retrieve the data
                                    processRow = new Database.SqlClient.ProcessRow<b_Department>(reader => { b_Department obj = new b_Department(); obj.LoadFromDatabase(reader); return obj; });
                                    results = Database.StoredProcedure.usp_Department_AllTemplatesWithClient.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, ClientId);

                                    // Extract the results
                                    if (null != results)
                                    {
                                        data = results;
                                    }
                                    else
                                    {
                                        data = new List<b_Department>();
                                    }

                                    // Clear the results collection
                                    //if (null != results)
                                    //{
                                    //    results.Clear();
                                    //    results = null;
                                    //}
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

        public void CheckKeyAndDeleteByPK(
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
                Database.StoredProcedure.usp_Department_CheckKeyDepartment.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public void RetrieveByClientIdSiteId(SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            ref List<b_Department> data)
        {
            SqlCommand command = null;
            string message = String.Empty;
            List<b_Department> results = null;
            data = new List<b_Department>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_Department_RetrieveByClientIdSiteId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_Department>();
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
       
        public void RetrieveDepartmentId(
                SqlConnection connection,
                SqlTransaction transaction,
                long callerUserInfoId,
                string callerUserName
            )
        {
            Database.SqlClient.ProcessRow<b_Department> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_Department>(reader => { this.LoadFromDatabase(reader); return this; });
                Database.StoredProcedure.usp_Department_RetrieveByDepartmentId.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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


        public void RetrieveInActiveFlag(SqlConnection connection,
          SqlTransaction transaction,
          long callerUserInfoId,
          string callerUserName,
          ref List<b_Department> data)
        {
            SqlCommand command = null;
            string message = String.Empty;
            List<b_Department> results = null;
            data = new List<b_Department>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_Department_RetrieveByInActiveFlag_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_Department>();
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
       
        public static object ProcessRowForDepartment(SqlDataReader reader)
        {
            b_Department obj = new b_Department();
            obj.LoadFromDatabase(reader);
            return (object)obj;
        }

        public void ValidateByNewClientLookupId(
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
            //data = new List<b_StoredProcValidationError>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_Department_ValidateNewClientLookupId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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


        public void ValidateByOldClientLookupId(
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
            //data = new List<b_StoredProcValidationError>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_Department_ValidateOldClientLookupId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
    }


}
