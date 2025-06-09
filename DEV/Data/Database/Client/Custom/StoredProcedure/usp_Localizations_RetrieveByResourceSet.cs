using System;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_Localizations_RetrieveByResourceSet
    {
        private static string STOREDPROCEDURE_NAME = "usp_Localizations_RetrieveByResourceSet";

        public usp_Localizations_RetrieveByResourceSet()
        {

        }
        public static List<b_Localizations> CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_Localizations obj

        )
        {
            List<b_Localizations> records = new List<b_Localizations>();
            SqlDataReader reader = null;
            b_Localizations record = null;
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ResourceSet", obj.ResourceSet, 512);
            command.SetStringInputParameter(SqlDbType.NVarChar, "LocaleId", obj.LocaleId, 10);
            
            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    record = new b_Localizations();
                    record.LoadFromDbExtended(reader);
                    records.Add(record);
                }
            }
            catch (Exception)
            {
                throw;
            }
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

            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);

            // Return the result
            return records;
        }
    }
}
