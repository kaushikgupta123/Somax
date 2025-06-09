using Database.Business;
using Database.SqlClient;

using System;
using System.Data;
using System.Data.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_PrevMaintSched_UpdateByPKForeignKeys_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_PrevMaintSched_UpdateByPKForeignKeys_V2";

        public usp_PrevMaintSched_UpdateByPKForeignKeys_V2()
        {

        }
        public static void CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_PrevMaintSched obj
        )
        {
            SqlParameter RETURN_CODE_parameter = null;
            SqlParameter updateIndexOut_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetInputParameter(SqlDbType.BigInt, "PrevMaintSchedId", obj.PrevMaintSchedId);
            command.SetInputParameter(SqlDbType.BigInt, "PrevMaintMasterId", obj.PrevMaintMasterId);
            command.SetInputParameter(SqlDbType.BigInt, "AssignedTo_PersonnelId", obj.AssignedTo_PersonnelId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "AssociationGroup", obj.AssociationGroup, 15);
            command.SetInputParameter(SqlDbType.Int, "CalendarSlack", obj.CalendarSlack);
            command.SetInputParameter(SqlDbType.BigInt, "ChargeToId", obj.ChargeToId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ChargeType", obj.ChargeType, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Crew", obj.Crew, 15);
            command.SetInputParameter(SqlDbType.DateTime2, "CurrentWOComplete", (obj.CurrentWOComplete == DateTime.MinValue) ? null : obj.CurrentWOComplete);
            command.SetInputParameter(SqlDbType.Bit, "DownRequired", obj.DownRequired);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ExcludeDOW", obj.ExcludeDOW, 7);
            command.SetInputParameter(SqlDbType.Int, "Frequency", obj.Frequency);
            command.SetStringInputParameter(SqlDbType.NVarChar, "FrequencyType", obj.FrequencyType, 15);
            command.SetInputParameter(SqlDbType.Bit, "InactiveFlag", obj.InactiveFlag);
            command.SetStringInputParameter(SqlDbType.NVarChar, "JobPlan", obj.JobPlan, 1073741823);
            command.SetInputParameter(SqlDbType.BigInt, "Last_WorkOrderId", obj.Last_WorkOrderId);
            command.SetInputParameter(SqlDbType.DateTime2, "LastPerformed", (obj.LastPerformed == DateTime.MinValue) ? null : obj.LastPerformed);
            command.SetInputParameter(SqlDbType.DateTime2, "LastScheduled", (obj.LastScheduled == DateTime.MinValue) ? null : obj.LastScheduled);
            command.SetInputParameter(SqlDbType.Decimal, "MeterHighLevel", obj.MeterHighLevel);
            command.SetInputParameter(SqlDbType.BigInt, "MeterId", obj.MeterId);
            command.SetInputParameter(SqlDbType.Decimal, "MeterInterval", obj.MeterInterval);
            command.SetInputParameter(SqlDbType.Decimal, "MeterLastDone", obj.MeterLastDone);
            command.SetInputParameter(SqlDbType.Decimal, "MeterLastDue", obj.MeterLastDue);
            command.SetInputParameter(SqlDbType.Decimal, "MeterLastReading", obj.MeterLastReading);
            command.SetInputParameter(SqlDbType.Decimal, "MeterLowLevel", obj.MeterLowLevel);
            command.SetStringInputParameter(SqlDbType.NVarChar, "MeterMethod", obj.MeterMethod, 1);
            command.SetInputParameter(SqlDbType.Bit, "MeterOn", obj.MeterOn);
            command.SetInputParameter(SqlDbType.Decimal, "MeterSlack", obj.MeterSlack);
            command.SetInputParameter(SqlDbType.DateTime2, "NextDueDate", (obj.NextDueDate == DateTime.MinValue) ? null : obj.NextDueDate);
            command.SetStringInputParameter(SqlDbType.NVarChar, "OnDemandGroup", obj.OnDemandGroup, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Priority", obj.Priority, 15);
            command.SetInputParameter(SqlDbType.Bit, "Scheduled", obj.Scheduled);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ScheduleMethod", obj.ScheduleMethod, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ScheduleType", obj.ScheduleType, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ScheduleWeeks", obj.ScheduleWeeks, 52);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Section", obj.Section, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Shift", obj.Shift, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Type", obj.Type, 15);
            command.SetInputParameter(SqlDbType.Int, "RIMEWorkClass", obj.RIMEWorkClass);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Category", obj.Category, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "RootCauseCode", obj.RootCauseCode, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ActionCode", obj.ActionCode, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "FailureCode", obj.FailureCode, 15);
            command.SetInputParameter(SqlDbType.BigInt, "Planner_PersonnelId", obj.Planner_PersonnelId);
            command.SetInputParameter(SqlDbType.Int, "UpdateIndex", obj.UpdateIndex);
            command.SetInputParameter(SqlDbType.Bit, "PlanningRequired", obj.PlanningRequired); //V2-1161
            command.SetStringInputParameter(SqlDbType.NVarChar, "ChargeToClientLookupId", obj.ChargeToClientLookupId, 31);

            // Setup updateIndexOut parameter.
            updateIndexOut_parameter = command.Parameters.Add("@UpdateIndexOut", SqlDbType.Int);
            updateIndexOut_parameter.Direction = ParameterDirection.Output;

            // Execute stored procedure.
            command.ExecuteNonQuery();

            obj.UpdateIndex = (int)updateIndexOut_parameter.Value;

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}
