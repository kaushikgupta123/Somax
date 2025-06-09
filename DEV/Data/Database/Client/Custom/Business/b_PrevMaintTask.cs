/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2014 by SOMAX Inc.
* b_PrevMaintTask.cs (Data Object)
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID  Person             Description
* =========== ======== ================== =========================================================
* 2014-Jul-29 SOM-259  Roger Lawton       Added AssignedTo_ClientLookupId and ChargeTo_ClientLookupId
*                                         Properties
*                                         Added LoadFromDatabaseExtended method
*                                         Modified PrevMaintTaskRetrieveByPrevMaintMasterId method
*                                         by removing the ProcessRow delegate - was not working as
*                                         expected (see change log in class
*                                         usp_PrevMaintTask_RetrieveByPrevMaintMasterId)
* 2014-Sep-05 SOM-304  Roger Lawton       Added Validation, Creat and Update Methods
****************************************************************************************************
*/

using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.Business
{

  public partial class b_PrevMaintTask
  {
    #region properties
    public string AssignedTo_ClientLookupId { get; set; }
    public string ChargeToClientLookupId { get; set; }
    public long SiteId { get; set; }
    #endregion properties

    public void PrevMaintTask_RetrieveByPrevMaintMasterId(
         SqlConnection connection,
         SqlTransaction transaction,
         long callerUserInfoId,
         string callerUserName,
         long ClientId,
         long PrevMaintMasterId,
         ref List<b_PrevMaintTask> prevMaintTaskList
     )
    {
      SqlCommand command = null;
      string message = String.Empty;

      try
      {
        // Create the command to use in calling the stored procedures
        command = new SqlCommand();
        command.Connection = connection;
        command.Transaction = transaction;
        // Call the stored procedure to retrieve the data
        prevMaintTaskList = Database.StoredProcedure.usp_PrevMaintTask_RetrieveByPrevMaintMasterId.CallStoredProcedure(command, callerUserInfoId, callerUserName, ClientId, PrevMaintMasterId);

      }
      finally
      {
        if (null != command)
        {
          command.Dispose();
          command = null;
        }
        message = String.Empty;
        callerUserInfoId = 0;
        callerUserName = String.Empty;
        ClientId = 0;
      }
    }
    public void LoadFromDatabaseExtended(SqlDataReader reader)
    {
      int i = this.LoadFromDatabase(reader);
      try
      {
        // EquipmentId column, bigint, not null
        this.AssignedTo_ClientLookupId = reader.GetString(i++);

        // ClientLookupId column, nvarchar(31), not null
        this.ChargeToClientLookupId = reader.GetString(i++);
      }
      catch (Exception ex)
      {
        // Diagnostics
        StringBuilder missing = new StringBuilder();

        try { reader["AssignedTo_ClientLookupId"].ToString(); }
        catch { missing.Append("AssignedTo_ClientLookupId "); }

        try { reader["ChargeToClientLookupId"].ToString(); }
        catch { missing.Append("ChargeToClientLookupId "); }

        StringBuilder msg = new StringBuilder();
        msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
        if (missing.Length > 0)
        {
          msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
        }

        throw new Exception(msg.ToString(), ex);
      }
    }
    public void RetrieveExtendedInformation(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName
    )
    {
      Database.SqlClient.ProcessRow<b_PrevMaintTask> processRow = null;
      SqlCommand command = null;
      string message = String.Empty;

      try
      {
        // Create the command to use in calling the stored procedures
        command = new SqlCommand();
        command.Connection = connection;
        command.Transaction = transaction;

        // Call the stored procedure to retrieve the data
        processRow = new Database.SqlClient.ProcessRow<b_PrevMaintTask>(reader => { this.LoadFromDatabaseExtended(reader); return this; });
        Database.StoredProcedure.usp_PrevMaintTask_RetrieveExtendedInformation.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

      }
      finally
      {
        if (null != command)
        {
          command.Dispose();
          command = null;
        }
        processRow = null;
        message = String.Empty;
        callerUserInfoId = 0;
        callerUserName = String.Empty;
      }
    }

    // Create using lookup id
    public void CreatyByPKForeignKeys(SqlConnection connection,SqlTransaction transaction,long callerUserInfoId,string callerUserName)
    {
      SqlCommand command = null;
      try
      {
        command = connection.CreateCommand();
        if (null != transaction)
        {
          command.Transaction = transaction;
        }
        Database.StoredProcedure.usp_PrevMaintTask_CreateByPKForeignKeys.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
      }
      finally
      {
        if (null != command)
        {
          command.Dispose();
          command = null;
        }
      }
    }

    //Update using lookup id
    public void UpdateByPKForeignKeys(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName)
    {
      SqlCommand command = null;
      try
      {
        command = connection.CreateCommand();
        if (null != transaction)
        {
          command.Transaction = transaction;
        }
        Database.StoredProcedure.usp_PrevMaintTask_UpdateByPKForeignKeys.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
      }
      finally
      {
        if (null != command)
        {
          command.Dispose();
          command = null;
        }
      }
    }

    /// <summary>
    /// Delete the PrevMaintTask table record and renumber the remaining items if any
    /// </summary>
    /// <param name="connection">SqlConnection containing the database connection</param>
    /// <param name="transaction">SqlTransaction containing the database transaction</param>
    /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
    /// <param name="callerUserName">string that identifies the user calling the database</param>
    public void DeleteAndRenumber(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName)
    {
      SqlCommand command = null;

      try
      {
        command = connection.CreateCommand();
        if (null != transaction)
        {
          command.Transaction = transaction;
        }
        Database.StoredProcedure.usp_PrevMaintTask_DeleteRenumber.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
      }
      finally
      {
        if (null != command)
        {
          command.Dispose();
          command = null;
        }
      }
    }

    public void ValidateProcessOnInsert(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, ref List<b_StoredProcValidationError> data)
    {
      SqlCommand command = null;
      string message = String.Empty;
      List<b_StoredProcValidationError> results = null;
      data = new List<b_StoredProcValidationError>();

      try
      {
        // Create the command to use in calling the stored procedures
        command = new SqlCommand();
        command.Connection = connection;
        command.Transaction = transaction;

        // Call the stored procedure to retrieve the data
        results = Database.StoredProcedure.usp_PrevMaintTask_ValidateOnInsert.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

        if (results != null)
        {
          data = results;
        }
        else
        {
          data = new List<b_StoredProcValidationError>();
        }
      }
      finally
      {
        if (null != command)
        {
          command.Dispose();
          command = null;
        }

        message = String.Empty;
        callerUserInfoId = 0;
        callerUserName = String.Empty;
      }
    }

    public void ValidateProcessOnSave(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, ref List<b_StoredProcValidationError> data)
    {
      SqlCommand command = null;
      string message = String.Empty;
      List<b_StoredProcValidationError> results = null;
      data = new List<b_StoredProcValidationError>();

      try
      {
        // Create the command to use in calling the stored procedures
        command = new SqlCommand();
        command.Connection = connection;
        command.Transaction = transaction;

        // Call the stored procedure to retrieve the data
        results = Database.StoredProcedure.usp_PrevMaintTask_ValidateOnSave.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

        if (results != null)
        {
          data = results;
        }
        else
        {
          data = new List<b_StoredProcValidationError>();
        }
      }
      finally
      {
        if (null != command)
        {
          command.Dispose();
          command = null;
        }

        message = String.Empty;
        callerUserInfoId = 0;
        callerUserName = String.Empty;
      }
    }
  }
}
