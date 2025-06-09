using System;
using System.Data;
using System.Data.SqlClient;

using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    /// <summary>
    /// Access the usp_SanitationJob_Create stored procedure.
    /// </summary>
    /// 
    public class usp_SanitationJob_CreateEXSanitRequest
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_SanitationJob_CreateEXSanitRequest";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_SanitationJob_CreateEXSanitRequest()
        {
        }

        /// <summary>
        /// Static method to call the usp_SanitationJob_Create stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public static void CallStoredProcedure (
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_SanitationJob obj
        )
        {
            SqlParameter        RETURN_CODE_parameter = null;
            int                 retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetOutputParameter(SqlDbType.BigInt, "SanitationJobId");
            command.SetInputParameter(SqlDbType.BigInt, "AreaId", obj.AreaId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetInputParameter(SqlDbType.BigInt, "DepartmentId", obj.DepartmentId);
            command.SetInputParameter(SqlDbType.BigInt, "StoreroomId", obj.StoreroomId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ClientLookupId", obj.ClientLookupId, 15);                    
            command.SetStringInputParameter(SqlDbType.NVarChar, "SourceType", obj.SourceType, 15);
            command.SetInputParameter(SqlDbType.BigInt, "SourceId", obj.SourceId);
            command.SetInputParameter(SqlDbType.Decimal, "ActualDuration", obj.ActualDuration);
            command.SetInputParameter(SqlDbType.BigInt, "AssignedTo_PersonnelId", obj.AssignedTo_PersonnelId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CancelReason", obj.CancelReason, 15);
            command.SetInputParameter(SqlDbType.BigInt, "ChargeToId", obj.ChargeToId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ChargeToClientLookupId", obj.ChargeToClientLookupId, 256);            
            command.SetStringInputParameter(SqlDbType.NVarChar, "ChargeType", obj.ChargeType, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ChargeTo_Name", obj.ChargeTo_Name, 63);
            command.SetInputParameter(SqlDbType.BigInt, "CompleteBy_PersonnelId", obj.CompleteBy_PersonnelId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CompleteComments", obj.CompleteComments, 1073741823);
            command.SetInputParameter(SqlDbType.DateTime2, "CompleteDate", obj.CompleteDate);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Description", obj.Description, 1073741823);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Shift", obj.Shift, 15);
            command.SetInputParameter(SqlDbType.Bit, "DownRequired", obj.DownRequired);
            command.SetInputParameter(SqlDbType.DateTime2, "ScheduledDate", obj.ScheduledDate);
            command.SetInputParameter(SqlDbType.Decimal, "ScheduledDuration", obj.ScheduledDuration);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Status", obj.Status, 15);
            command.SetInputParameter(SqlDbType.BigInt, "Creator_PersonnelId", obj.Creator_PersonnelId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Creator_PersonnelClientLookupId", obj.Creator_PersonnelClientLookupId, 63);
            command.SetInputParameter(SqlDbType.Int, "FlagSourceType", obj.FlagSourceType);
            command.SetInputParameter(SqlDbType.BigInt, "SanOnDemandMasterId", obj.SanOnDemandMasterId);
            command.SetInputParameter(SqlDbType.DateTime2, "RequiredDate", obj.RequiredDate);
            //command.SetInputParameter(SqlDbType.BigInt, "PlantLocationId", obj.PlantLocationId);

            // Execute stored procedure.
            command.ExecuteNonQuery();

            obj.SanitationJobId = (long)command.Parameters["@SanitationJobId"].Value;

            // Process the RETURN_CODE parameter value
            retCode = (int) RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}