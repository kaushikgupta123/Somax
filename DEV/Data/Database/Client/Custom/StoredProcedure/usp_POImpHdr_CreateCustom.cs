using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
  public class usp_POImpHdr_CreateCustom
  {
    private static string STOREDPROCEDURE_NAME = "usp_POImpHdr_CreateCustom";

    public usp_POImpHdr_CreateCustom()
    {

    }
    public static void CallStoredProcedure(
       SqlCommand command,     
       long callerUserInfoId,
       string callerUserName,
       b_POImpHdr obj
   )
    {
      ArrayList records = new ArrayList();
      SqlDataReader reader = null;
      b_POImpHdr record = null;
      SqlParameter RETURN_CODE_parameter = null;
      int retCode = 0;

      // Setup command.
      command.SetProcName(STOREDPROCEDURE_NAME);
      RETURN_CODE_parameter = command.GetReturnCodeParameter();
      command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
      command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
      command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
      command.SetOutputParameter(SqlDbType.BigInt, "POImpHdrId");
      command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
      command.SetStringInputParameter(SqlDbType.NVarChar, "PONumber", obj.PONumber, 15);
      command.SetInputParameter(SqlDbType.Int, "Revision", obj.Revision);
      command.SetInputParameter(SqlDbType.BigInt, "EXPOID", obj.EXPOID);
      command.SetInputParameter(SqlDbType.BigInt, "EXPRID", obj.EXPRID);
      command.SetStringInputParameter(SqlDbType.NVarChar, "SOMAXPRNumber", obj.SOMAXPRNumber, 15);
      command.SetInputParameter(SqlDbType.BigInt, "SOMAXPRID", obj.SOMAXPRID);
      command.SetInputParameter(SqlDbType.DateTime2, "POCreateDate", obj.POCreateDate);
      command.SetStringInputParameter(SqlDbType.NVarChar, "Currency", obj.Currency, 15);
      command.SetStringInputParameter(SqlDbType.NVarChar, "EXVendor", obj.EXVendor, 31);
      command.SetInputParameter(SqlDbType.BigInt, "EXVendorId", obj.EXVendorId);
      command.SetInputParameter(SqlDbType.DateTime2, "RequiredDate", obj.RequiredDate);
      command.SetStringInputParameter(SqlDbType.NVarChar, "PaymentTerms", obj.PaymentTerms, 50);
      command.SetStringInputParameter(SqlDbType.NVarChar, "Status", obj.Status, 31);
      command.SetStringInputParameter(SqlDbType.NVarChar, "ErrorCodes", obj.ErrorCodes, 127);
      command.SetStringInputParameter(SqlDbType.NVarChar, "ErrorMessage", obj.ErrorMessage, 511);
      command.SetInputParameter(SqlDbType.DateTime2, "LastProcess", obj.LastProcess);


      try
      {
        // Execute stored procedure.
        command.ExecuteNonQuery();

        obj.POImpHdrId = (long)command.Parameters["@POImpHdrId"].Value;

        // Process the RETURN_CODE parameter value
        retCode = (int)RETURN_CODE_parameter.Value;
        AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
      }
      catch (Exception ex)
      { }
    }
  }
}


