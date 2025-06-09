using Database.Business;
using Database.SqlClient;
using System.Data;
using System.Data.SqlClient;

namespace Database.StoredProcedure
{
    class usp_ServiceOrder_RetrieveByServiceOrderId_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_ServiceOrder_RetrieveByServiceOrderId_V2";

        public usp_ServiceOrder_RetrieveByServiceOrderId_V2()
        {

        }

        public static b_ServiceOrder CallStoredProcedure(
       SqlCommand command,
       long callerUserInfoId,
       string callerUserName,
       b_ServiceOrder obj
       )
        {
            b_ServiceOrder records = new b_ServiceOrder();
            SqlDataReader reader = null;
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetInputParameter(SqlDbType.BigInt, "ServiceOrderId", obj.ServiceOrderId);
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            try
            {
                //obj.utilityAdd = new UtilityAdd();
                //obj.utilityAdd.list1 = new List<string>();
                // Execute stored procedure.
                reader = command.ExecuteReader();
                //while (reader.Read())
                //{

                //    obj.utilityAdd.list1.Add(reader.GetValue(0).ToString());
                //}
                //reader.NextResult();
                //obj.utilityAdd.list2 = new List<string>();
                //while (reader.Read())
                //{
                //    obj.utilityAdd.list2.Add(reader.GetValue(0).ToString());
                //}
                //reader.NextResult();
                //// Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.0
                    records = b_ServiceOrder.ProcessRowForRetrieveByServiceOrderId(reader);
                    //tmpServiceOrder.ClientId = obj.ClientId;
                    //records.Add(tmpServiceOrder);
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
