using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.Business;
using Database.SqlClient;

namespace Data.Database.Business
{
    public partial class b_ManufacturerMaster
    {
        public void PartMaster_RetrieveAll_ByInactiveFlag(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName,
        ref b_ManufacturerMaster[] data
    )
        {
            ProcessRow<b_ManufacturerMaster> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_ManufacturerMaster[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new ProcessRow<b_ManufacturerMaster>(reader => { b_ManufacturerMaster obj = new b_ManufacturerMaster(); obj.LoadFromDatabase(reader); return obj; });
                results = StoredProcedure.usp_ManufacturerMaster_RetrieveAll_ByInactiveFlag.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

                // Extract the results
                if (null != results)
                {
                    data = (b_ManufacturerMaster[])results.ToArray(typeof(b_ManufacturerMaster));
                }
                else
                {
                    data = new b_ManufacturerMaster[0];
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

        public void ValidateByClientLookupId(SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            ref List<b_StoredProcValidationError> data)
        {
            SqlCommand command = null;
            string message = String.Empty;
            List<b_StoredProcValidationError> results = null;
            data = new List<b_StoredProcValidationError>();
            try
            {
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;
                results = Data.Database.StoredProcedure.usp_ManufacturerMaster_ValidateByClientLookupId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
