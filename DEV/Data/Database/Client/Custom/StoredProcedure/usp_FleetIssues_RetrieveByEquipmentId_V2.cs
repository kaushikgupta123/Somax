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
    class usp_FleetIssues_RetrieveByEquipmentId_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_FleetIssues_RetrieveByEquipmentId_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// 
        public usp_FleetIssues_RetrieveByEquipmentId_V2()
        {

        }

        public static List<b_FleetIssues> CallStoredProcedure(
           SqlCommand command,
           long callerUserInfoId,
           string callerUserName,
           b_FleetIssues obj
       )
        {
            List<b_FleetIssues> records = new List<b_FleetIssues>();
            SqlDataReader reader = null;

            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "EquipmentId", obj.EquipmentId);
            try
            {

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    b_FleetIssues tmpFleetIssue = b_FleetIssues.ProcessRetrieveForFleetIssueByEquipmentIdV2(reader);
                    tmpFleetIssue.ClientId = obj.ClientId;
                    records.Add(tmpFleetIssue);
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
            return records;
        }
    }
}
