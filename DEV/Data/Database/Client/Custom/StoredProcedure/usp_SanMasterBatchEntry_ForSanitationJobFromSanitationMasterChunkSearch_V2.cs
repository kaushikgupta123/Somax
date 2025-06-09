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
    public class usp_SanMasterBatchEntry_ForSanitationJobFromSanitationMasterChunkSearch_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_SanMasterBatchEntry_ForSanitationJobFromSanitationMasterChunkSearch_V2";
        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_SanMasterBatchEntry_ForSanitationJobFromSanitationMasterChunkSearch_V2()
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
        public static b_SanMasterBatchEntry CallStoredProcedure(
       SqlCommand command,
       long callerUserInfoId,
       string callerUserName,
         b_SanMasterBatchEntry obj

   )
        {

            SqlDataReader reader = null;
            b_SanMasterBatchEntry record = null;
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ScheduleType", obj.ScheduleType, 25);
            // Setup clientId parameter.
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.siteid);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderbyColumn", obj.OrderbyColumn, 100);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderBy", obj.OrderBy, 10);
            command.SetInputParameter(SqlDbType.Int, "offset1", obj.OffSetVal);
            command.SetInputParameter(SqlDbType.Int, "nextrow", obj.NextRow);
            command.SetInputParameter(SqlDbType.DateTime2, "ScheduleThroughDate", obj.ScheduleThroughDate);
            command.SetStringInputParameter(SqlDbType.NVarChar, "OnDemandgroup", obj.OnDemandgroup, 25);
            command.SetInputParameter(SqlDbType.Bit, "PrintSanitationJobs", obj.PrintSanitationJobs);
            command.SetInputParameter(SqlDbType.Bit, "PrintAttachments", obj.PrintAttachments);
            command.SetInputParameter(SqlDbType.DateTime, "CurrentDate", DateTime.Now);
            command.SetStringInputParameter(SqlDbType.NVarChar, "AssetGroup1Ids", obj.AssetGroup1Ids, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "AssetGroup2Ids", obj.AssetGroup2Ids, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "AssetGroup3Ids", obj.AssetGroup3Ids, 500);
            command.SetInputParameter(SqlDbType.DateTime2, "SanitationBEDueDate", obj.DueDate);
            command.SetStringInputParameter(SqlDbType.NVarChar, "EquipmentClientLookupId", obj.EquipmentClientLookupId, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "EquipmentName", obj.EquipmentName, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "SanitationMasterDescription", obj.MasterDescription, -1);
            command.SetStringInputParameter(SqlDbType.NVarChar, "SanitationMasterShift", obj.Shift, 15);
            command.SetInputParameter(SqlDbType.BigInt, "ReturnSanMasterBatchHeaderId", obj.SanMasterBatchHeaderId);

            try
            {
                // List<b_PrevMaintBatchEntry> records = new List<b_PrevMaintBatchEntry>();
                command.CommandTimeout = 0;                // Execute stored procedure.
                reader = command.ExecuteReader();
                obj.listOfSanMasterBatchEntries = new List<b_SanMasterBatchEntry>();
                // Loop through dataset.
                while (reader.Read())
                {
                    // Process the current row into a record
                    record = b_SanMasterBatchEntry.ProcessRowAfterBatchEntryChunkSearch(reader);
                    // Add the record to the list.
                    obj.listOfSanMasterBatchEntries.Add(record);
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
            return obj;
            // Return the result
            // return records;
        }
    }
}
