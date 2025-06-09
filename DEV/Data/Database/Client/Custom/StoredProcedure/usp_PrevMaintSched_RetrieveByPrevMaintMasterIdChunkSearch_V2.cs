using Database.Business;
using Database.SqlClient;

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_PrevMaintSched_RetrieveByPrevMaintMasterIdChunkSearch_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_PrevMaintSched_RetrieveByPrevMaintMasterIdChunkSearch_V2";

        public usp_PrevMaintSched_RetrieveByPrevMaintMasterIdChunkSearch_V2()
        {
        }
        public static List<b_PrevMaintSched> CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_PrevMaintSched obj
        )
        {
            List<b_PrevMaintSched> records = new List<b_PrevMaintSched>();
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
            command.SetInputParameter(SqlDbType.BigInt, "PrevMaintMasterId", obj.PrevMaintMasterId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderbyColumn", obj.OrderbyColumn, 100);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderBy", obj.OrderBy, 10);
            command.SetInputParameter(SqlDbType.Int, "offset1", obj.Offset);
            command.SetInputParameter(SqlDbType.Int, "nextrow", obj.Nextrow);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ChargeToClientLookupId", obj.ChargeToClientLookupId, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ChargeToName", obj.ChargeToName, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "FrequencyWithType", obj.FrequencyWithType, 35);
            command.SetInputParameter(SqlDbType.DateTime2, "NextDueDate", obj.NextDueDate);
            command.SetStringInputParameter(SqlDbType.NVarChar, "WorkOrder_ClientLookupId", obj.WorkOrder_ClientLookupId, 31);
            command.SetInputParameter(SqlDbType.DateTime2, "LastScheduled", obj.LastScheduled);
            command.SetInputParameter(SqlDbType.DateTime2, "LastPerformed", obj.LastPerformed);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Meter_ClientLookupId", obj.Meter_ClientLookupId, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "OnDemandGroup", obj.OnDemandGroup, 15);

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.
                    records.Add((b_PrevMaintSched)b_PrevMaintSched.ProcessRowForRetrieveByPrevMaintMasterIdForChunckSearch_V2(reader));
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
