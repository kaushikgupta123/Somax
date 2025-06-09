using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

using Data.Database;
using Data.Database.Business;
using System.Collections;
using Database.SqlClient;
using Database;

namespace Data.Database.StoredProcedure
{

    public class usp_PartCategoryMaster_RetrieveByInactiveFlag
    {
        private static string STOREDPROCEDURE_NAME = "usp_PartCategoryMaster_RetrieveByInactiveFlag";

        /// <summary>
        /// Constructor
        /// </summary>
        public usp_PartCategoryMaster_RetrieveByInactiveFlag()
        {
        }


        public static ArrayList CallStoredProcedure(
            SqlCommand command,
            ProcessRow<b_PartCategoryMaster> processRow,
            long callerUserInfoId,
            string callerUserName,
            b_PartCategoryMaster b_PartCategoryMaster
        )
        {
            ArrayList records = new ArrayList();
            SqlDataReader reader = null;
            b_PartCategoryMaster record = null;
            SqlParameter RETURN_CODE_parameter = null;
          
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", b_PartCategoryMaster.ClientId);
            command.SetInputParameter(SqlDbType.Bit, "InactiveFlag", b_PartCategoryMaster.InactiveFlag);

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Process the current row into a record
                    record = processRow(reader);

                    // Add the record to the list.
                    records.Add(record);
                }

            }
            catch (Exception ex) { }
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

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);

            // Return the result
            return records;
        }
    }
}
