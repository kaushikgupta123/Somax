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
    class usp_FuelTracking_RetrieveByEquipmentId_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_FuelTracking_RetrieveByEquipmentId_V2";

        public usp_FuelTracking_RetrieveByEquipmentId_V2()
        {

        }

        public static List<b_Equipment> CallStoredProcedure(
   SqlCommand command,
   long callerUserInfoId,
   string callerUserName,
   b_Equipment obj
   )
        {
            List<b_Equipment> records = new List<b_Equipment>();
            SqlDataReader reader = null;
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "EquipmentId", obj.EquipmentId);
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            try
            {

                // Execute stored procedure.
                reader = command.ExecuteReader();

                //// Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.0
                    b_Equipment tmpFuelTracking = b_Equipment.ProcessRetrieveForFleetFuelByEquipmentIdV2(reader);
                    tmpFuelTracking.ClientId = obj.ClientId;
                    records.Add(tmpFuelTracking);
                    //records = b_ServiceOrder.ProcessRowForRetrieveByEquipmentId(reader);

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
