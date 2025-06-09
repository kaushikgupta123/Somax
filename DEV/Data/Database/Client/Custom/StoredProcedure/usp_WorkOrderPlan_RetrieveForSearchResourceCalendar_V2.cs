using System;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using Database.SqlClient;
using System.Data.Common;
using System.Collections.Generic;

namespace Database.StoredProcedure
{
    public class usp_WorkOrderPlan_RetrieveForSearchResourceCalendar_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_WorkOrderPlan_RetrieveForSearchResourceCalendar_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_WorkOrderPlan_RetrieveForSearchResourceCalendar_V2()
        {
        }

        /// <summary>
        /// Static method to call the usp_WorkOrderPlan_RetrieveForSearchResourceCalendar_V2 stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// 
        public static List<b_WorkOrderPlan> CallStoredProcedure(
         SqlCommand command,
         long callerUserInfoId,
         string callerUserName,
         b_WorkOrderPlan obj
     )
        {
            List<b_WorkOrderPlan> records = new List<b_WorkOrderPlan>();
            b_WorkOrderPlan b_WorkOrderPlan = new b_WorkOrderPlan();
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
            command.SetStringInputParameter(SqlDbType.NVarChar, "CalendarStart", obj.ScheduledDateStart, 50);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CalendarEnd", obj.ScheduledDateEnd, 50);
            command.SetInputParameter(SqlDbType.BigInt, "WorkOrderPlanId", obj.WorkOrderPlanId);            

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    b_WorkOrderPlan = b_WorkOrderPlan.ProcessRowForResourceCalendarChunkSearch(reader);
                    records.Add(b_WorkOrderPlan);
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
