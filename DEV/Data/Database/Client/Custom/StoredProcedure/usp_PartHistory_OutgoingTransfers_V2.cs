using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_PartHistory_OutgoingTransfers_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_PartHistory_OutgoingTransfers_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_PartHistory_OutgoingTransfers_V2()
        {
        }
        /// 
        public static void CallStoredProcedure(
          SqlCommand command,
          long callerUserInfoId,
          string callerUserName,
          b_PartHistory obj
            )
        {
            List<b_PartHistory> records = new List<b_PartHistory>();
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
            command.SetInputParameter(SqlDbType.BigInt, "PerformedById", obj.PerformedById);
            command.SetOutputParameter(SqlDbType.BigInt, "PartHistoryId");
            command.SetInputParameter(SqlDbType.Structured, "StoreroomTransferListTable", obj.StoreroomTransferList);
            try
            {
                command.ExecuteNonQuery();

                obj.PartHistoryId = command.Parameters["@PartHistoryId"].Value == null ? 0 : (long)command.Parameters["@PartHistoryId"].Value;
                // Execute stored procedure.
                //reader = command.ExecuteReader();

                //// Loop through dataset.
                //while (reader.Read())
                //{
                //    // Add the record to the list.
                //    //records.Add((b_UserReportGridDefintion)b_UserReportGridDefintion.ProcessRowForUserReport(reader));
                //}

                //reader.NextResult();

                //while (reader.Read())
                //{

                //}

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
            //  return records;
        }
    }
}
