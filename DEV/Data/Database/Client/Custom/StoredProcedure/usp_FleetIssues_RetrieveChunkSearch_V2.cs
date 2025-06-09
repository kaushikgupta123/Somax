using System.Data;
using System.Data.SqlClient;
using Database.Business;
using Database.SqlClient;
using System.Collections.Generic;

namespace Database.StoredProcedure
{
    public class usp_FleetIssues_RetrieveChunkSearch_V2
    { /// <summary>
      /// Constants.
      /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_FleetIssues_RetrieveChunkSearch_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_FleetIssues_RetrieveChunkSearch_V2()
        {
        }

        /// <summary>
        /// Static method to call the usp_Equipment_RetrieveChunkSearch_V2 stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public static b_FleetIssues CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_FleetIssues obj
        )
        {
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
            command.SetInputParameter(SqlDbType.Int, "CaseNo", obj.CustomQueryDisplayId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderbyColumn", obj.OrderbyColumn, 100);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderBy", obj.OrderBy, 10);
            command.SetInputParameter(SqlDbType.Int, "offset1", obj.OffSetVal);
            command.SetInputParameter(SqlDbType.Int, "nextrow", obj.NextRow);
            command.SetStringInputParameter(SqlDbType.NVarChar, "EquipmentId", obj.ClientLookupId, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Name", obj.Name, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Make", obj.Make, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Model", obj.Model, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "VIN", obj.VIN, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "RecordStartDate", obj.RecordStartDate, 60);
            command.SetStringInputParameter(SqlDbType.NVarChar, "RecordEndDate", obj.RecordEndDate, 60);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CreateStartDateVw", obj.CreateStartDateVw, 60);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CreateEndDateVw", obj.CreateEndDateVw, 60);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Defects", obj.Defects, 4000);
            command.SetStringInputParameter(SqlDbType.NVarChar, "SearchText ", obj.SearchText, 800);

            try
            {

                List<b_FleetIssues> records = new List<b_FleetIssues>();
                reader = command.ExecuteReader();
                obj.listOfFleetIssues = new List<b_FleetIssues>();
                while (reader.Read())
                {
                    b_FleetIssues tmpFleetIssues = b_FleetIssues.ProcessRetrieveForFleetIssueChunkV2(reader);
                    obj.listOfFleetIssues.Add(tmpFleetIssues);
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
            return obj;
        }
    }
}
