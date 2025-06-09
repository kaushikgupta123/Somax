using System;
using System.Data;
using System.Data.SqlClient;

using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    /// <summary>
    /// Access the usp_VendorMasterImport_CreateCustom stored procedure.
    /// </summary>
    public class usp_FleetScheduledService_CreateCustom_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_FleetScheduledService_CreateCustom_V2";
        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_FleetScheduledService_CreateCustom_V2()
        {
        }
        /// <summary>
        /// Static method to call the usp_VendorMasterImport_CreateCustom stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public static void CallStoredProcedure(
           SqlCommand command,
           long callerUserInfoId,
           string callerUserName,
           b_ScheduledService obj
       )
        {
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;
            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetOutputParameter(SqlDbType.BigInt, "ScheduledServiceId");
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetInputParameter(SqlDbType.BigInt, "AreaId", obj.AreaId);
            command.SetInputParameter(SqlDbType.BigInt, "DepartmentId", obj.DepartmentId);
            command.SetInputParameter(SqlDbType.BigInt, "StoreroomId", obj.StoreroomId);
            command.SetInputParameter(SqlDbType.BigInt, "ServiceTaskId", obj.ServiceTaskId);
            command.SetInputParameter(SqlDbType.BigInt, "EquipmentId", obj.EquipmentId);
            command.SetInputParameter(SqlDbType.Bit, "InactiveFlag", obj.InactiveFlag);
            command.SetInputParameter(SqlDbType.BigInt, "Last_ServiceOrderId", obj.Last_ServiceOrderId);
            command.SetInputParameter(SqlDbType.DateTime2, "LastPerformedDate", obj.LastPerformedDate);
            command.SetInputParameter(SqlDbType.Decimal, "LastPerformedMeter1", obj.LastPerformedMeter1);
            command.SetInputParameter(SqlDbType.Decimal, "LastPerformedMeter2", obj.LastPerformedMeter2);
            command.SetInputParameter(SqlDbType.Decimal, "Meter1Interval", obj.Meter1Interval);
            command.SetInputParameter(SqlDbType.Decimal, "Meter1Threshold", obj.Meter1Threshold);
            command.SetInputParameter(SqlDbType.Decimal, "Meter2Interval", obj.Meter2Interval);
            command.SetInputParameter(SqlDbType.Decimal, "Meter2Threshold", obj.Meter2Threshold);
            command.SetInputParameter(SqlDbType.Int, "TimeInterval", obj.TimeInterval);
            command.SetStringInputParameter(SqlDbType.NVarChar, "TimeIntervalType", obj.TimeIntervalType, 9);
            command.SetInputParameter(SqlDbType.Int, "TimeThreshold", obj.TimeThreshold);
            command.SetStringInputParameter(SqlDbType.NVarChar, "TimeThresoldType", obj.TimeThresoldType, 9);
            command.SetStringInputParameter(SqlDbType.NVarChar, "RepairReason", obj.RepairReason, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "VMRSSystem", obj.VMRSSystem, 3);
            command.SetStringInputParameter(SqlDbType.NVarChar, "VMRSAssembly", obj.VMRSAssembly, 3);
            // Execute stored procedure.
            command.ExecuteNonQuery();
            obj.ScheduledServiceId = (long)command.Parameters["@ScheduledServiceId"].Value;

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}