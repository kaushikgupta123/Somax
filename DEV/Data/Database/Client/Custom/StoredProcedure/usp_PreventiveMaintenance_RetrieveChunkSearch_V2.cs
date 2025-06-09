

using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

using Database;
using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_PreventiveMaintenance_RetrieveChunkSearch_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_PreventiveMaintenance_RetrieveChunkSearch_V2";

        /// <summary>
        /// Constructor
        /// </summary>
        public usp_PreventiveMaintenance_RetrieveChunkSearch_V2()
        {

        }


        /// <summary>
        /// Static method to call the usp_PreventiveMaintenance_RetrieveToSearchCriteria stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="processRow">ProcessRow delegate containing method to call to process the row into an object.</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <returns>ArrayList containing the results of the query</returns>
        public static List<b_PrevMaintMaster> CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_PrevMaintMaster obj
        )
        {
            List<b_PrevMaintMaster> records = new List<b_PrevMaintMaster>();
            b_PrevMaintMaster b_PrevMaintMaster = new b_PrevMaintMaster();
            SqlDataReader reader = null;
            SqlParameter RETURN_CODE_parameter = null;

            int retCode = 0;
            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.Int, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.Int, "SiteId", obj.SiteId);
            command.SetInputParameter(SqlDbType.Int, "CaseNo",obj.CaseNo );
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderbyColumn", obj.orderbyColumn, 100);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderBy", obj.orderBy, 10);
            command.SetInputParameter(SqlDbType.Int, "offset1", obj.offset1);
            command.SetInputParameter(SqlDbType.Int, "nextrow", obj.nextrow );
            command.SetStringInputParameter(SqlDbType.NVarChar, "ClientLookupId", obj.ClientLookupId, 67);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Description", obj.Description, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ScheduleType", obj.ScheduleType, 67);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Type", obj.Type, 67);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Chargeto", obj.Chargeto, 67);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ChargetoName", obj.ChargetoName, 128);
            command.SetInputParameter(SqlDbType.Int, "FilterType", obj.FilterType);
            command.SetInputParameter(SqlDbType.Int, "FilterValue", obj.FilterValue );
            command.SetStringInputParameter(SqlDbType.NVarChar, "SearchText", obj.SearchText, 800);
            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    b_PrevMaintMaster = b_PrevMaintMaster.ProcessRowForChunkSearch(reader);
                    records.Add(b_PrevMaintMaster);
                }
            }
            catch (Exception ex)
            { }
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
