using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.SqlClient;
using Database.Business;
using System.Collections.Generic;
using System;

namespace Database.StoredProcedure
{
    public class usp_WOPlanLineItem_PlannedWorkorderHoursByAssigned_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_WOPlanLineItem_PlannedWorkorderHoursByAssigned_V2";

        public usp_WOPlanLineItem_PlannedWorkorderHoursByAssigned_V2()
        {
        }

        public static List<Tuple<string, decimal, string>> CallStoredProcedure(
        SqlCommand command,
        long callerUserInfoId,
        string callerUserName,
        long ClientId,
        long SiteId,
        long WorkOrderPlanId
    )
        {
            List<Tuple<string,decimal, string>> records = new List<Tuple<string, decimal, string>>();

             SqlDataReader reader = null;
            //b_Account record = null;
            SqlParameter RETURN_CODE_parameter = null;

            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", SiteId);
            command.SetInputParameter(SqlDbType.BigInt, "WorkOrderPlanId", WorkOrderPlanId);

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Get the result count
                while (reader.Read())
                {
                    string seriesname = reader.GetString(0);
                    decimal total = !reader.IsDBNull(1) ? reader.GetDecimal(1) : default(decimal);
                    string name = reader.GetString(2);
                    records.Add(Tuple.Create(seriesname,total, name));
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
