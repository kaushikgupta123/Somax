/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2011 by SOMAX Inc.
 * All rights reserved. 
 ******************************************************************************
 * Date        Task ID   Person             Description
 * =========== ======== =================== ===================================
 * 2012-Mar-19          Roger Lawton        Created
 ******************************************************************************
 */

using System.Data;
using System.Data.SqlClient;
using Database.SqlClient;
using Database.Business;
using System.Collections.Generic;

namespace Database.StoredProcedure
{
  /// <summary>
  /// Access the usp_Equipment_ValidateByClientId stored procedure.
  /// </summary>
  class usp_Personnel_ValidateExtended
  {
    /// <summary>
    /// Constants.
    /// </summary>
    private static string STOREDPROCEDURE_NAME = "usp_Personnel_ValidateExtended";

    /// <summary>
    /// Default constructor.
    /// </summary>
    public usp_Personnel_ValidateExtended()
    {
    }

    /// <summary>
    /// Static method to call the usp_Personnel_ValidateExtended stored procedure using SqlClient.
    /// </summary>
    /// <param name="command">SqlCommand object to use to call the stored procedure</param>
    /// <param name="processRow">ProcessRow delegate containing method to call to process the row into an object.</param>
    /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
    /// <param name="callerUserName">string that identifies the user calling the database</param>
    /// <param name="clientId">long that contains the value of the @ClientId parameter</param>
    /// <param name="sessionId">System.Guid that contains the value of the @SessionId parameter</param>
    /// <returns>ArrayList containing the results of the query</returns>
    public static List<b_StoredProcValidationError> CallStoredProcedure(
        SqlCommand command,
        long callerUserInfoId,
        string callerUserName,
        b_Personnel obj,
        string Supervisor_ClientLookupId,
        string DefaultStoreroom_Name,
        bool createMode,
        DataTable lulist
    )
    {
      List<b_StoredProcValidationError> records = new List<b_StoredProcValidationError>();
      SqlDataReader reader = null;
      SqlParameter RETURN_CODE_parameter = null;
      int retCode = 0;

      // Setup command.
      command.SetProcName(STOREDPROCEDURE_NAME);
      RETURN_CODE_parameter = command.GetReturnCodeParameter();
      command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
      command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
      command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
      command.SetInputParameter(SqlDbType.BigInt, "PersonnelId", obj.PersonnelId);
      command.SetStringInputParameter(SqlDbType.NVarChar, "ClientLookupId", obj.ClientLookupId, 63);
      command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
      command.SetInputParameter(SqlDbType.Bit, "CreateMode", createMode);
      command.SetStringInputParameter(SqlDbType.NVarChar, "Supervisor_ClientLookupId", Supervisor_ClientLookupId, 63);
      command.SetStringInputParameter(SqlDbType.NVarChar, "DefaultStoreroom_Name", DefaultStoreroom_Name, 31);
      command.SetInputParameter(SqlDbType.Structured, "LookupTable", lulist);
      try
      {
        // Execute stored procedure.
        reader = command.ExecuteReader();

        // Loop through dataset.
        while (reader.Read())
        {
          // Add the record to the list.
          records.Add((b_StoredProcValidationError)b_StoredProcValidationError.ProcessRow(reader));
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