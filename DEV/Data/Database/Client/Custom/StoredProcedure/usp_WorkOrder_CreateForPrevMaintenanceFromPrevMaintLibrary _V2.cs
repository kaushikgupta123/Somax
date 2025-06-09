using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

using Database;
using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_WorkOrder_CreateForPrevMaintenanceFromPrevMaintLibrary_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_WorkOrder_CreateForPrevMaintenanceFromPrevMaintLibrary_V2";

        public static void CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            ref b_WorkOrder b_workorder

        )
        {
            //x.Requestor_PersonnelId
            //x.ClientLookupId
            //x.PrevMaintBatchId
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", b_workorder.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", b_workorder.SiteId);
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "PrevMaintbatchEntryId", b_workorder.PrevMaintBatchId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "WORKORDER_CLIENTLOOKUPID", b_workorder.ClientLookupId, 25);
            command.SetInputParameter(SqlDbType.BigInt, "PersonnelID", b_workorder.Requestor_PersonnelId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "AssetGroup1Ids", b_workorder.AssetGroup1Ids,500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "AssetGroup2Ids", b_workorder.AssetGroup2Ids,500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "AssetGroup3Ids", b_workorder.AssetGroup3Ids,500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "PrevMaintSchedType", b_workorder.PrevMaintSchedType, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "PrevMaintMasterType", b_workorder.PrevMaintMasterType, 500);
            command.SetOutputParameter(SqlDbType.BigInt, "LastGeneratedWorkOrderId");
            command.SetOutputParameter(SqlDbType.BigInt, "lastWorkAssignedPersonnelId");

            try
            {
                // Execute stored procedure.
                command.ExecuteNonQuery();

                b_workorder.WorkOrderId = Convert.ToInt32(command.Parameters["@LastGeneratedWorkOrderId"].Value.ToString());
                b_workorder.WorkAssigned_PersonnelId = Convert.ToInt32(command.Parameters["@LastWorkAssignedPersonnelId"].Value.ToString());
            }
            catch (Exception ex) { }
            finally
            {


                // Process the RETURN_CODE parameter value
                retCode = (int)RETURN_CODE_parameter.Value;
                AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);

                // Return the result
                //return result;
            }
        }
    }
}
