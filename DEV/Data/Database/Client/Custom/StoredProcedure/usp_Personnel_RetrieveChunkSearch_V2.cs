using Database.Business;
using Database.SqlClient;

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_Personnel_RetrieveChunkSearch_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_Personnel_RetrieveChunkSearch_V2";

        public usp_Personnel_RetrieveChunkSearch_V2()
        {

        }
        
        public static b_Personnel CallStoredProcedure(
           SqlCommand command,
           long callerUserInfoId,
           string callerUserName,
          b_Personnel obj

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
            command.SetStringInputParameter(SqlDbType.NVarChar, "PersonnelId", obj.ClientLookupId, 100);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderbyColumn", obj.OrderbyColumn, 100);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderBy", obj.OrderBy, 10);
            command.SetInputParameter(SqlDbType.Int, "offset1", obj.OffSetVal);
            command.SetInputParameter(SqlDbType.Int, "nextrow", obj.NextRow);
            command.SetStringInputParameter(SqlDbType.NVarChar, "SearchText ", obj.SearchText, 800);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Name", obj.Name, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Shift", obj.Shift, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ScheduleGroup", obj.ScheduleGroup, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CraftClientLookupId", obj.CraftClientLookupId, 15);
            command.SetInputParameter(SqlDbType.Int, "InActiveStatus", obj.InActiveStatus);
            //#region 1108
            command.SetInputParameter(SqlDbType.BigInt, "AssignedAssetGroup1Id", obj.AssignedAssetGroup1Id);
            command.SetInputParameter(SqlDbType.BigInt, "AssignedAssetGroup2Id", obj.AssignedAssetGroup2Id);
            command.SetInputParameter(SqlDbType.BigInt, "AssignedAssetGroup3Id", obj.AssignedAssetGroup3Id);
            //#endregion
            try
            {
                List<b_Personnel> records = new List<b_Personnel>();
                // Execute stored procedure.
                reader = command.ExecuteReader();
                obj.listOfPersonnels = new List<b_Personnel>();

                // Loop through dataset.
                while (reader.Read())
                {
                    b_Personnel tmpPersonnels = b_Personnel.ProcessChunkSearchV2(reader);
                    obj.listOfPersonnels.Add(tmpPersonnels);
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
            return obj;
        }
    }
}
