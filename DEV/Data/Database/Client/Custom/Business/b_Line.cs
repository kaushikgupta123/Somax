using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.Business
{
    public partial class b_Line
    { 
            public void RetrieveByClientIdSiteId(SqlConnection connection,
             SqlTransaction transaction,
             long callerUserInfoId,
             string callerUserName,
             ref List<b_Line> data)
            {
                SqlCommand command = null;
                string message = String.Empty;
                List<b_Line> results = null;
                data = new List<b_Line>();

                try
                {
                    // Create the command to use in calling the stored procedures
                    command = new SqlCommand();
                    command.Connection = connection;
                    command.Transaction = transaction;

                    // Call the stored procedure to retrieve the data
                    results = Database.StoredProcedure.usp_Line_RetrieveByClientIdSiteId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                    if (results != null)
                    {
                        data = results;
                    }
                    else
                    {
                        data = new List<b_Line>();
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
           
            public void RetrieveByLineId(
                SqlConnection connection,
                SqlTransaction transaction,
                long callerUserInfoId,
                string callerUserName
            )
            {
                Database.SqlClient.ProcessRow<b_Line> processRow = null;
                SqlCommand command = null;
                string message = String.Empty;

                try
                {
                    // Create the command to use in calling the stored procedures
                    command = new SqlCommand();
                    command.Connection = connection;
                    command.Transaction = transaction;

                    // Call the stored procedure to retrieve the data
                    processRow = new Database.SqlClient.ProcessRow<b_Line>(reader => { this.LoadFromDatabase(reader); return this; });
                    Database.StoredProcedure.usp_Line_RetrieveByLineId.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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

        public void RetrieveByInActiveFlag(SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName,
        ref List<b_Line> data)
        {
            SqlCommand command = null;
            string message = String.Empty;
            List<b_Line> results = null;
            data = new List<b_Line>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_Line_RetrieveByInActiveFlag_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_Line>();
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


        public static object ProcessRowForLine(SqlDataReader reader)
            {
                b_Line obj = new b_Line();
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
                results = Database.StoredProcedure.usp_Line_ValidateNewClientLookupId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
                results = Database.StoredProcedure.usp_Line_ValidateOldClientLookupId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
