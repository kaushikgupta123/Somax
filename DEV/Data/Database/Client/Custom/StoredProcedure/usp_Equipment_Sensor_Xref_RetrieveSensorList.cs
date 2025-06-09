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


    public class usp_Equipment_Sensor_Xref_RetrieveSensorList
    {
        private static string STOREDPROCEDURE_NAME = "usp_Equipment_Sensor_Xref_RetrieveSensorList";
        public usp_Equipment_Sensor_Xref_RetrieveSensorList()
        {
        }
       
        public static List<b_Equipment_Sensor_Xref> CallStoredProcedure(
          SqlCommand command,
          long callerUserInfoId,
          string callerUserName,
          b_Equipment_Sensor_Xref obj
      )
        {
            List<b_Equipment_Sensor_Xref> records = new List<b_Equipment_Sensor_Xref>();
            SqlDataReader reader = null;
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.
                    records.Add((b_Equipment_Sensor_Xref)b_Equipment_Sensor_Xref.ProcessRowForEquipmentSensor(reader));
                }

                reader.NextResult();

                while (reader.Read())
                {
                    // Add the record to the list.
                    //records.Add(reader.GetString(0));
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
