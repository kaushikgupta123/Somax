using System;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using Database.SqlClient;
using System.Data.Common;
using System.Collections.Generic;

namespace Database.StoredProcedure
{
    public class usp_List_RetrieveForSearchByLaborScheduling_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_List_RetrieveForSearchByLaborScheduling_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_List_RetrieveForSearchByLaborScheduling_V2()
        {
        }

        /// <summary>
        /// Static method to call the usp_List_RetrieveForSearchByLaborScheduling_V2 stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// 
        public static List<b_WorkOrder> CallStoredProcedure(
         SqlCommand command,
         long callerUserInfoId,
         string callerUserName,
         b_WorkOrder obj
     )
        {
            List<b_WorkOrder> records = new List<b_WorkOrder>();
            b_WorkOrder b_WorkOrder = new b_WorkOrder();
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
            command.SetStringInputParameter(SqlDbType.NVarChar, "PersonnelList", obj.PersonnelList.TrimStart(','), 1073741823);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ScheduledDateStart", obj.ScheduledDateStart, 50);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ScheduledDateEnd", obj.ScheduledDateEnd, 50);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderbyColumn", obj.OrderbyColumn, 100);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderBy", obj.OrderBy, 10);
            command.SetInputParameter(SqlDbType.Int, "offset1", obj.OffSetVal);
            command.SetInputParameter(SqlDbType.Int, "nextrow", obj.NextRow);
            command.SetStringInputParameter(SqlDbType.NVarChar, "EquipmentId", obj.ClientLookupId, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ChargeToName", obj.ChargeTo_Name, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Description", obj.Description, 1073741823);
            command.SetStringInputParameter(SqlDbType.NVarChar, "RequireDate", obj.RequireDate, 67);            
            command.SetStringInputParameter(SqlDbType.NVarChar, "Type", obj.Type, 30);
            command.SetInputParameter(SqlDbType.Int, "CaseNo", obj.CustomQueryDisplayId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "SearchText ", obj.SearchText, 50);

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    b_WorkOrder = b_WorkOrder.ProcessRowForListLaborSchedulingChunkSearch(reader);
                    records.Add(b_WorkOrder);
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
