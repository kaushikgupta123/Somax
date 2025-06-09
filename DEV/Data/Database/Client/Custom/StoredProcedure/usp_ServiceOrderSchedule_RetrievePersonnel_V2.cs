using Database.Business;
using Database.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.StoredProcedure
{
    public class usp_ServiceOrderSchedule_RetrievePersonnel_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_ServiceOrderSchedule_RetrievePersonnel_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        ///
        public usp_ServiceOrderSchedule_RetrievePersonnel_V2()
        {

        }
        /// <summary>
        /// Static method to call the usp_WorkOrder_RetrieveByEquipmentId stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="processRow">ProcessRow delegate containing method to call to process the row into an object.</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="clientId">long that contains the value of the @ClientId parameter</param>
        /// <param name="sessionId">System.Guid that contains the value of the @SessionId parameter</param>
        /// <returns>ArrayList containing the results of the query</returns>
        /// 
        public static List<List<b_ServiceOrderSchedule>> CallStoredProcedure(
           SqlCommand command,
           long callerUserInfoId,
           string callerUserName,
           b_ServiceOrderSchedule obj
       )
        {

            List<b_ServiceOrderSchedule> AllRecords = new List<b_ServiceOrderSchedule>();
            List<b_ServiceOrderSchedule> ScheduledRecords = new List<b_ServiceOrderSchedule>();
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
            command.SetInputParameter(SqlDbType.BigInt, "ServiceOrderId", obj.ServiceOrderId);

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.
                    AllRecords.Add((b_ServiceOrderSchedule)b_ServiceOrderSchedule.ProcessRowRetrievePersonnel(reader));
                }
                reader.NextResult();
                while (reader.Read())
                {
                    // Add the record to the list.
                    ScheduledRecords.Add((b_ServiceOrderSchedule)b_ServiceOrderSchedule.ProcessRowRetrievePersonnel(reader));
                }
                obj.TotalRecords = new List<List<b_ServiceOrderSchedule>>();
                obj.TotalRecords.Add(AllRecords);
                obj.TotalRecords.Add(ScheduledRecords);
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
            return obj.TotalRecords;
        }
    }
}
