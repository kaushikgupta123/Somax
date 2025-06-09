using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

using Database;
using Database.Business;
using System.Collections;
using Database.SqlClient;

namespace Database.StoredProcedure
{

    public class usp_PartMaster_RetrieveAll_ByInactiveFlag
    {
        private static string STOREDPROCEDURE_NAME = "usp_PartMaster_RetrieveAll_ByInactiveFlag";

        /// <summary>
        /// Constructor
        /// </summary>
        public usp_PartMaster_RetrieveAll_ByInactiveFlag()
        {
        }


        public static ArrayList CallStoredProcedure(
            SqlCommand command,
            Database.SqlClient.ProcessRow<b_PartMaster> processRow,
            long callerUserInfoId,
            string callerUserName,
            b_PartMaster b_PartMaster,
            bool InactiveFlag
        )
        {
            ArrayList records = new ArrayList();
            SqlDataReader reader = null;
            b_PartMaster record = null;
            SqlParameter RETURN_CODE_parameter = null;
          
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", b_PartMaster.ClientId);            
            command.SetInputParameter(SqlDbType.Bit, "InactiveFlag", InactiveFlag);

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
