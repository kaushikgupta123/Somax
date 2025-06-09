using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using System.Collections.Generic;
using System.Globalization;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_PrevMaintBatchEntry_ForWorkOrderFromPrevMaintLibrary_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_PrevMaintBatchEntry_ForWorkOrderFromPrevMaintLibrary_V2";
        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_PrevMaintBatchEntry_ForWorkOrderFromPrevMaintLibrary_V2()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="callerUserInfoId"></param>
        /// <param name="callerUserName"></param>
        /// <param name="clientId"></param>
        /// <param name="siteid"></param>
        /// <param name="ScheduleType"></param>
        /// <param name="ScheduleThroughDate"></param>
        /// <param name="OnDemandgroup"></param>
        /// <param name="PrintWorkOrders"></param>
        /// <param name="PrintAttachments"></param>
        /// <returns></returns>
        /// 
        public static List<b_PrevMaintBatchEntry> CallStoredProcedure(
           SqlCommand command,
           long callerUserInfoId,
           string callerUserName,
           long clientId,
           long siteid,
           string ScheduleType,
           DateTime ScheduleThroughDate,
           string OnDemandgroup,
           bool PrintWorkOrders,
           bool PrintAttachments,
           string AssetGroup1Ids,
           string AssetGroup2Ids,
          string AssetGroup3Ids,
         string PrevMaintSchedType,
          string PrevMaintMasterType

       )
        {
            List<b_PrevMaintBatchEntry> records = new List<b_PrevMaintBatchEntry>();
            SqlDataReader reader = null;
            b_PrevMaintBatchEntry record = null;
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);

            // Setup clientId parameter.
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", clientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", siteid);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ScheduleType", ScheduleType, 25);
            command.SetInputParameter(SqlDbType.DateTime2, "ScheduleThroughDate", ScheduleThroughDate);
            command.SetStringInputParameter(SqlDbType.NVarChar, "OnDemandgroup", OnDemandgroup, 25);
            command.SetInputParameter(SqlDbType.Bit, "PrintWorkOrders", PrintWorkOrders);
            command.SetInputParameter(SqlDbType.Bit, "PrintAttachments", PrintAttachments);
            command.SetInputParameter(SqlDbType.DateTime, "CurrentDate", DateTime.Now);
            command.SetStringInputParameter(SqlDbType.NVarChar, "AssetGroup1Ids", AssetGroup1Ids,500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "AssetGroup2Ids", AssetGroup2Ids,500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "AssetGroup3Ids", AssetGroup3Ids,500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "PrevMaintSchedType", PrevMaintSchedType, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "PrevMaintMasterType", PrevMaintMasterType, 500);

            try
            {
                command.CommandTimeout = 0;                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Process the current row into a record
                    record = b_PrevMaintBatchEntry.ProcessRowAfterBatchEntry(reader);

                    // Add the record to the list.
                    records.Add(record);
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
