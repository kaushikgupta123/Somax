using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.Business;
using Database.SqlClient;
using System;

namespace Database.StoredProcedure
{
    public class usp_Personnel_RetrieveChunkSearchLookupListForApprovalGroup_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_Personnel_RetrieveChunkSearchLookupListForApprovalGroup_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_Personnel_RetrieveChunkSearchLookupListForApprovalGroup_V2()
        {
        }
        public static List<b_Personnel> CallStoredProcedure(SqlCommand command, long callerUserInfoId,
                                         string callerUserName, b_Personnel obj)
        {
            List<b_Personnel> records = new List<b_Personnel>();
            b_Personnel b_Personnel = new b_Personnel();
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
            command.SetStringInputParameter(SqlDbType.NVarChar, "ClientLookupId", obj.ClientLookupId, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Name", obj.FullName, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "AssetGroup1", obj.AssetGroup1, 1073741823);
            command.SetStringInputParameter(SqlDbType.NVarChar, "AssetGroup2", obj.AssetGroup2, 1073741823);
            command.SetStringInputParameter(SqlDbType.NVarChar, "AssetGroup3", obj.AssetGroup3, 1073741823);
            command.SetStringInputParameter(SqlDbType.NVarChar, "RequestType", obj.requestType, 63);
            command.SetInputParameter(SqlDbType.BigInt, "ApprovalGroupId", obj.approvalGroupId);
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
                    b_Personnel = b_Personnel.ProcessRowOfPersonnelLookupListForApprovalGroup(reader);
                    records.Add(b_Personnel);
                }
            }
            catch (Exception ex)
            {

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