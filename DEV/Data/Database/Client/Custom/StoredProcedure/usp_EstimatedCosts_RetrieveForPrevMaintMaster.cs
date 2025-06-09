/*
Added By Indusnet technologies
 */

using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

using Database;
using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
  public class usp_EstimatedCosts_RetrieveForPrevMaintMaster
  {
    /// <summary>
    /// Constants.
    /// </summary>
    private static string STOREDPROCEDURE_NAME = "usp_EstimatedCosts_RetrieveForPrevMaintMaster";

    /// <summary>
    /// Default constructor.
    /// </summary>
    public usp_EstimatedCosts_RetrieveForPrevMaintMaster()
    {
    }

    /// <summary>
    /// Static method to call the usp_Equipment_RetrieveAll stored procedure using SqlClient.
    /// </summary>
    /// <param name="command">SqlCommand object to use to call the stored procedure</param>
    /// <param name="processRow">ProcessRow delegate containing method to call to process the row into an object.</param>
    /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
    /// <param name="callerUserName">string that identifies the user calling the database</param>
    /// <returns>ArrayList containing the results of the query</returns>
    public static List<b_EstimatedCosts> CallStoredProcedure(
       SqlCommand command,
       long callerUserInfoId,
       string callerUserName,
       long clientId,
       long PrevMaintMasterId,
       string Category
   )
    {
      List<b_EstimatedCosts> records = new List<b_EstimatedCosts>();
      SqlDataReader reader = null;
      b_EstimatedCosts record = null;
      SqlParameter RETURN_CODE_parameter = null;

      int retCode = 0;

      // Setup command.
      command.SetProcName(STOREDPROCEDURE_NAME);
      RETURN_CODE_parameter = command.GetReturnCodeParameter();
      command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
      command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
      command.SetInputParameter(SqlDbType.BigInt, "ClientId", clientId);
      command.SetInputParameter(SqlDbType.BigInt, "PrevMaintMasterId", PrevMaintMasterId);
      command.SetStringInputParameter(SqlDbType.NVarChar, "Category", Category, 15);

      try
      {
        // Execute stored procedure.
        reader = command.ExecuteReader();

        // Loop through dataset.
        while (reader.Read())
        {
          // Process the current row into a record
          record = (b_EstimatedCosts)b_EstimatedCosts.ProcessRowForPrevMaint(reader);
          //  record = (b_EstimatedCosts)b_EstimatedCosts.ProcessRow(reader);

          //// Add the record to the list.
          records.Add(record);
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
      return records;
    }
  }
}
