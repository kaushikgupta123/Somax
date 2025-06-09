using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

using Database;
using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
  public class usp_WorkOrder_CreateForSensorReading
  {
    private static string STOREDPROCEDURE_NAME = "usp_WorkOrder_CreateForSensorReading";

    public static void CallStoredProcedure(
        SqlCommand command,
        long callerUserInfoId,
        string callerUserName,
        b_WorkOrder b_workorder)
    {
      //x.Requestor_PersonnelId
      //x.ClientLookupId
      //x.PrevMaintBatchId
      SqlParameter RETURN_CODE_parameter = null;
      int retCode = 0;

      // Setup command.
      command.SetProcName(STOREDPROCEDURE_NAME);
      RETURN_CODE_parameter = command.GetReturnCodeParameter();
      command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
      command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
      command.SetInputParameter(SqlDbType.BigInt, "ClientId", b_workorder.ClientId);
      command.SetInputParameter(SqlDbType.BigInt, "SiteId", b_workorder.SiteId);
      command.SetInputParameter(SqlDbType.BigInt, "SensorReadingId", b_workorder.SourceId); // NOTE PASSING SENSOR READING ID IN THE SOURCEID
      command.SetInputParameter(SqlDbType.BigInt, "PersonnelId", b_workorder.Creator_PersonnelId);
      command.SetStringInputParameter(SqlDbType.NVarChar, "TriggerMessage", b_workorder.Description, 511);
      command.SetInputParameter(SqlDbType.DateTime, "Today_Local",b_workorder.ScheduledStartDate);
      command.SetStringInputParameter(SqlDbType.NVarChar, "WorkOrder_ClientLookupId", b_workorder.ClientLookupId, 15);
      command.SetOutputParameter(SqlDbType.BigInt, "LastGeneratedWorkOrderId");
            try
      {
        // Execute stored procedure.
        command.ExecuteNonQuery();

        b_workorder.WorkOrderId = Convert.ToInt32(command.Parameters["@LastGeneratedWorkOrderId"].Value.ToString());

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
