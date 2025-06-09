using System;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    /// <summary>
    /// Access the usp_WorkOrder_CreateFromOnDemandMaster stored procedure.
    /// </summary>
    public class usp_WorkOrder_CreateFromOnDemandMaster_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_WorkOrder_CreateFromOnDemandMaster_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_WorkOrder_CreateFromOnDemandMaster_V2()
        {
        }

        /// <summary>
        /// Static method to call the usp_WorkOrder_CreateFromOnDemandMaster_V2 stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public static void CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_WorkOrder obj

        )
        {
            SqlParameter RETURN_CODE_parameter = null;
            SqlParameter chargeTo_Name_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetOutputParameter(SqlDbType.BigInt, "WorkOrderId");
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetInputParameter(SqlDbType.BigInt, "DepartmentId", obj.DepartmentId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ClientLookupId", obj.ClientLookupId, 15);
            command.SetInputParameter(SqlDbType.BigInt, "ApproveBy_PersonnelId", obj.ApproveBy_PersonnelId);
            command.SetInputParameter(SqlDbType.DateTime2, "ApproveDate", obj.ApproveDate);
            command.SetInputParameter(SqlDbType.BigInt, "ChargeToId", obj.ChargeToId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ChargeType", obj.ChargeType, 15);
            command.SetInputParameter(SqlDbType.BigInt, "Creator_PersonnelId", obj.Creator_PersonnelId);
            command.SetInputParameter(SqlDbType.DateTime2, "ScheduledStartDate", obj.ScheduledStartDate);
            command.SetInputParameter(SqlDbType.BigInt, "SourceId", obj.SourceId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "SourceType", obj.SourceType, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Status", obj.Status, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Type", obj.Type, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Creator_PersonnelClientLookupId", obj.Creator_PersonnelClientLookupId, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ChargeToClientLookupId", obj.ChargeToClientLookupId, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ApproveBy_PersonnelClientLookupId", obj.ApproveBy_PersonnelClientLookupId, 63);
            chargeTo_Name_parameter = command.Parameters.Add("@ChargeTo_Name", SqlDbType.NVarChar, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "MaintOnDemandClientLookUpId", obj.MaintOnDemandClientLookUpId, 31);

            command.SetStringInputParameter(SqlDbType.NVarChar, "Priority", obj.Priority, 15);
            command.SetInputParameter(SqlDbType.DateTime2, "RequiredDate", obj.RequiredDate);

            chargeTo_Name_parameter.Direction = ParameterDirection.Output;


            // Execute stored procedure.
            command.ExecuteNonQuery();

            obj.ChargeTo_Name = (string)chargeTo_Name_parameter.Value;

            if (!string.IsNullOrEmpty(command.Parameters["@WorkOrderId"].Value.ToString()))
            {
                obj.WorkOrderId = (long)command.Parameters["@WorkOrderId"].Value;
            }

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}