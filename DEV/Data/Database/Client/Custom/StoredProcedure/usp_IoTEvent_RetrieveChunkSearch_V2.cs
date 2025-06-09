using Database.Business;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_IoTEvent_RetrieveChunkSearch_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_IoTEvent_RetrieveChunkSearch_V2";
        public usp_IoTEvent_RetrieveChunkSearch_V2()
        {
            
        }
        /// <summary>
        /// Static method to call the usp_IoTEvent_RetrieveChunkSearch_V2 stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <returns>ArrayList containing the results of the query</returns>
        public static List<b_IoTEvent> CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            long clientId,
            b_IoTEvent obj
        )
        {
            List<b_IoTEvent> records = new List<b_IoTEvent>();
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
            command.SetInputParameter(SqlDbType.Int, "CaseNo", obj.CustomQueryDisplayId);

            command.SetStringInputParameter(SqlDbType.VarChar, "orderbyColumn", obj.OrderbyColumn, 100);
            command.SetStringInputParameter(SqlDbType.VarChar, "orderBy", obj.OrderBy, 10);
            command.SetInputParameter(SqlDbType.Int, "offset1", obj.OffSetVal);
            command.SetInputParameter(SqlDbType.Int, "nextrow", obj.NextRow);
            command.SetStringInputParameter(SqlDbType.NVarChar, "SourceType", obj.SourceType, 67);
            command.SetStringInputParameter(SqlDbType.NVarChar, "EventType", obj.EventType, 67);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Status", obj.Status, 67);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Disposition", obj.Disposition, 67);
            command.SetStringInputParameter(SqlDbType.NVarChar, "WOClientLookupId", obj.WOClientLookupId, 67);
            command.SetStringInputParameter(SqlDbType.NVarChar, "FaultCode", obj.FaultCode, 67);
            command.SetInputParameter(SqlDbType.DateTime2, "CreateDate", obj.CreateDate);
            command.SetStringInputParameter(SqlDbType.NVarChar, "IoTDeviceClientLookupId", obj.IoTDeviceClientLookupId, 67);
            command.SetStringInputParameter(SqlDbType.NVarChar, "AssetId", obj.AssetId, 67);
            command.SetStringInputParameter(SqlDbType.NVarChar, "SearchText ", obj.SearchText, 800);

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    records.Add(b_IoTEvent.ProcessRowForRetrieveChunkSearchFromDetails(reader));
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

            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);

            // Return the result
            return records;
        }
    }
}
