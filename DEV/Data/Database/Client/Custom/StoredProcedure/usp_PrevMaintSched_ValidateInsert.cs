using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_PrevMaintSched_ValidateInsert
    {
        private static string STOREDPROCEDURE_NAME = "usp_PrevMaintSched_ValidateInsert";

        public usp_PrevMaintSched_ValidateInsert()
        {
        }

        public static List<b_StoredProcValidationError> CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_PrevMaintSched obj
        )
        {
            List<b_StoredProcValidationError> records = new List<b_StoredProcValidationError>();
            SqlDataReader reader = null;
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;


            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt,"SiteId",obj.SiteId);
            command.SetInputParameter(SqlDbType.BigInt, "PrevMaintMasterId", obj.PrevMaintMasterId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ChargeType", obj.ChargeType, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ChargeToClientLookupId", obj.ChargeToClientLookupId,31);
            command.SetStringInputParameter(SqlDbType.NVarChar,"FrequencyType",obj.FrequencyType,15);
            command.SetInputParameter(SqlDbType.Int,"Frequency",obj.Frequency);
            command.SetInputParameter(SqlDbType.BigInt,"MeterId",obj.MeterId);
            command.SetStringInputParameter(SqlDbType.NVarChar,"MeterMethod",obj.MeterMethod,15);
            command.SetInputParameter(SqlDbType.Decimal,"MeterInterval",obj.MeterInterval);
            command.SetStringInputParameter(SqlDbType.NVarChar,"OnDemandGroup",obj.OnDemandGroup,15);
            command.SetInputParameter(SqlDbType.BigInt, "AssignedTo_PersonnelId", obj.AssignedTo_PersonnelId);

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.
                    records.Add((b_StoredProcValidationError)b_StoredProcValidationError.ProcessRow(reader));
                }

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

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);

            // Return the result
            return records;
        }
    }
}
