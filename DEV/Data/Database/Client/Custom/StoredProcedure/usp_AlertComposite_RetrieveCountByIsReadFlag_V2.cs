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
    public class usp_AlertComposite_RetrieveCountByIsReadFlag_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_AlertComposite_RetrieveCountByIsReadFlag_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_AlertComposite_RetrieveCountByIsReadFlag_V2()
        {
        }

        public static int CallStoredProcedure(SqlCommand command,
            //long callerUserInfoId,
            long clientId,
            long personnelId,
            bool IsRead,
           out int resultMaintenanceCount,
           out int resultInventoryCount,
           out int resultProcurementCount,
           out int resultSystemCount
            )
        {
           SqlDataReader reader = null;
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;
            int retTotalCount = 0;
            resultMaintenanceCount = 0;
            resultInventoryCount = 0;
            resultProcurementCount = 0;
            resultSystemCount = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", clientId);
            command.SetInputParameter(SqlDbType.BigInt, "PersonnelId", personnelId);
            command.SetInputParameter(SqlDbType.Bit, "IsRead", IsRead);
            try
            {// Execute stored procedure.
                reader = command.ExecuteReader();

                // Get the total count
                while (reader.Read())
                {
                    retTotalCount = reader.GetInt32(0);
                    resultMaintenanceCount = reader.GetInt32(1);
                    resultInventoryCount = reader.GetInt32(2);
                    resultProcurementCount = reader.GetInt32(3);
                    resultSystemCount = reader.GetInt32(4);
                }

                //reader.NextResult();

                //// Get the total Maintenance count
                //while (reader.Read())
                //{
                //    resultMaintenanceCount = reader.GetInt32(1);
                //}
                //reader.NextResult();

                //// Get the total Inventory count
                //while (reader.Read())
                //{
                //    resultInventoryCount = reader.GetInt32(2);
                //}

                //reader.NextResult();

                //// Get the total Procurement count
                //while (reader.Read())
                //{
                //    resultProcurementCount = reader.GetInt32(3);
                //}

                //reader.NextResult();

                //// Get the total System count
                //while (reader.Read())
                //{
                //    resultSystemCount = reader.GetInt32(4);
                //}
                //retCount = (Int32)command.ExecuteScalar();
            }
            finally
            {
                //if (null != reader)
                //{
                //    if (false == reader.IsClosed)
                //    {
                //        reader.Close();
                //    }
                //    reader = null;
                //}
            }

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);

            // Return the result
            return retTotalCount;
        }
    }
}
