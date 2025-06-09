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
    public class usp_WOPlanLineItem_RetrieveByWorkOrderPlanId_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_WOPlanLineItem_RetrieveByWorkOrderPlanId_V2";

        public usp_WOPlanLineItem_RetrieveByWorkOrderPlanId_V2()
        {

        }

        public static List<b_WOPlanLineItem> CallStoredProcedure(
   SqlCommand command,
   long callerUserInfoId,
   string callerUserName,
   b_WOPlanLineItem obj
   )
        {
            List<b_WOPlanLineItem> records = new List<b_WOPlanLineItem>();
            SqlDataReader reader = null;
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "WorkOrderPlanId", obj.WorkOrderPlanId);

            try
            {

                // Execute stored procedure.
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    // Add the record to the list.0
                    b_WOPlanLineItem tmpServiceOrder = b_WOPlanLineItem.ProcessRowForWOPlanLineItemRetriveByWorkOrderPlanId(reader);
                    tmpServiceOrder.ClientId = obj.ClientId;
                    records.Add(tmpServiceOrder);
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
