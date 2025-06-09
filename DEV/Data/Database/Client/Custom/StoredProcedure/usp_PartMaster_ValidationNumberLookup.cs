
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_PartMaster_ValidationNumberLookup
    {
        private static string STOREDPROCEDURE_NAME = "usp_PartMaster_ValidationNumberLookup";
        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_PartMaster_ValidationNumberLookup()
        {
        }


        public static List<b_StoredProcValidationError> CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_PartMasterRequest obj
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
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetInputParameter(SqlDbType.BigInt, "PartMasterRequestId", obj.PartMasterRequestId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ClientLookUpId", obj.PartMaster_ClientLookupId,70);
            command.SetStringInputParameter(SqlDbType.NVarChar, "RequestType", obj.RequestType, 15);
            command.SetInputParameter(SqlDbType.Decimal, "UnitCost", obj.UnitCost);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Location", obj.Location, 31);
            command.SetInputParameter(SqlDbType.Decimal, "QtyMinimum", obj.QtyMinimum);
            command.SetInputParameter(SqlDbType.Decimal, "QtyMaximum", obj.QtyMaximum);
            //command.SetInputParameter(SqlDbType.BigInt, "PartMasterId", obj.PartMasterId);
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
            catch
            {
                Exception ex;
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
