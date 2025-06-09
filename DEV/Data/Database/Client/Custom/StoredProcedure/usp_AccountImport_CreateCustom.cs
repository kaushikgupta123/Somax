using System;
using System.Data;
using System.Data.SqlClient;

using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
  /// <summary>
  /// Access the usp_AccountImport_CreateCustom stored procedure.
  /// </summary>
  public class usp_AccountImport_CreateCustom
  {
    /// <summary>
    /// Constants.
    /// </summary>
    private static string STOREDPROCEDURE_NAME = "usp_AccountImport_CreateCustom";

    /// <summary>
    /// Default constructor.
    /// </summary>
    public usp_AccountImport_CreateCustom()
    {
    }

    /// <summary>
    /// Static method to call the usp_AccountImport_CreateCustom stored procedure using SqlClient.
    /// </summary>
    /// <param name="command">SqlCommand object to use to call the stored procedure</param>
    /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
    /// <param name="callerUserName">string that identifies the user calling the database</param>
    public static void CallStoredProcedure(
        SqlCommand command,
        long callerUserInfoId,
        string callerUserName,
        b_AccountImport obj
    )
    {
      SqlParameter RETURN_CODE_parameter = null;
      int retCode = 0;

      // Setup command.
      command.SetProcName(STOREDPROCEDURE_NAME);
      RETURN_CODE_parameter = command.GetReturnCodeParameter();
      command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
      command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
      command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
      command.SetOutputParameter(SqlDbType.BigInt, "AccountImportId");
      command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
      command.SetStringInputParameter(SqlDbType.NVarChar, "ClientLookupId", obj.ClientLookupId, 31);
      command.SetInputParameter(SqlDbType.BigInt, "ExAccountId", obj.ExAccountId);
      command.SetStringInputParameter(SqlDbType.NVarChar, "Name", obj.Name, 63);
      command.SetStringInputParameter(SqlDbType.NVarChar, "Enabled", obj.Enabled, 50);
      command.SetStringInputParameter(SqlDbType.NVarChar, "ErrorCodes", obj.ErrorCodes, 127);
      command.SetStringInputParameter(SqlDbType.NVarChar, "ErrorMessage", obj.ErrorMessage, 511);
      command.SetInputParameter(SqlDbType.DateTime2, "LastProcess", obj.LastProcess);

      // Execute stored procedure.
      command.ExecuteNonQuery();

      obj.AccountImportId = (long)command.Parameters["@AccountImportId"].Value;

      // Process the RETURN_CODE parameter value
      retCode = (int)RETURN_CODE_parameter.Value;
      AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
    }
  }
}