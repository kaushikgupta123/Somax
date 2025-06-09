using Database.Business;
using Database.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Database.StoredProcedure
{
    /// <summary>
    /// Access the usp_RepairableSpareLog_RetrieveByEquipmentId_V2 stored procedure.
    /// </summary>
    public class usp_RepairableSpareLog_RetrieveByEquipmentId_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_RepairableSpareLog_RetrieveByEquipmentId_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_RepairableSpareLog_RetrieveByEquipmentId_V2()
        {
        }
        /// <summary>
        /// Static method to call the usp_RepairableSpareLog_RetrieveByEquipmentId_V2 stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="processRow">ProcessRow delegate containing method to call to process the row into an object.</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <returns>ArrayList containing the results of the query</returns>
        public static List<b_RepairableSpareLog> CallStoredProcedure(
            SqlCommand command,
            Database.SqlClient.ProcessRow<b_RepairableSpareLog> processRow,
            long callerUserInfoId,
            string callerUserName,
            b_RepairableSpareLog obj
        )
        {
            List<b_RepairableSpareLog> records = new List<b_RepairableSpareLog>();
            b_RepairableSpareLog repairableSpareLog = new b_RepairableSpareLog();
            SqlDataReader reader = null;
            b_RepairableSpareLog record = null;
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "EquipmentId", obj.EquipmentId);
            command.SetStringInputParameter(SqlDbType.VarChar, "TransactionDate", obj.TransactionDateS, 30);
            command.SetStringInputParameter(SqlDbType.VarChar, "Status", obj.Status, 30);
            command.SetStringInputParameter(SqlDbType.VarChar, "PersonnelName", obj.PersonnelName, 256);
            command.SetStringInputParameter(SqlDbType.VarChar, "AssignedClientLookupId", obj.AssignedClientLookupId, 256);
            command.SetStringInputParameter(SqlDbType.VarChar, "Location", obj.Location, 256);
            command.SetStringInputParameter(SqlDbType.VarChar, "ParentClientLookupId", obj.ParentClientLookupId, 256);
            command.SetStringInputParameter(SqlDbType.VarChar, "AssetGroup1Name", obj.AssetGroup1Name, 30);
            command.SetStringInputParameter(SqlDbType.VarChar, "AssetGroup2Name", obj.AssetGroup2Name, 30);
            command.SetStringInputParameter(SqlDbType.VarChar, "AssetGroup3Name", obj.AssetGroup3Name, 30);
            command.SetInputParameter(SqlDbType.Int, "Page", obj.OffSetVal);
            command.SetInputParameter(SqlDbType.Int, "ResultsPerPage", obj.NextRow);
            command.SetStringInputParameter(SqlDbType.VarChar, "OrderColumn", obj.OrderbyColumn, 256);
            command.SetStringInputParameter(SqlDbType.VarChar, "OrderDirection", obj.OrderBy, 256);

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Process the current row into a record
                    repairableSpareLog = b_RepairableSpareLog.ProcessRowForRepSpareAssetByEquipmentId(reader);
                    // Add the record to the list.
                    records.Add(repairableSpareLog);
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
