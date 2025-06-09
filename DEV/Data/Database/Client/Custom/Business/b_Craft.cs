/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2016 by SOMAX Inc.
* PreventiveMaintenanceDetails.aspx.cs
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID  Person             Description
* =========== ======== ================== ==========================================================
* 2014-Oct-02 SOM-354  Roger Lawton       Changed method name from RetrieveBySiteNameAllFromDatabase 
*                                         to RetrieveForSite
*                                         Changed to call usp_Craft_RetrieveForSite instead of
*                                         usp_Craft_RetrieveBySiteName
 *                                        Removed SiteName - not needed
*                                         Removed LoadFromDatabaseBySiteName (use LoadFromDatabase)
*                                         Added Validation Methods 
* 2015-Nov-05 SOM-844  Roger Lawton       Added DeleteInactivate Method
*                                         Cleaned Up
****************************************************************************************************
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Database.Business
{

  public partial class b_Craft
  {

    #region properties

    public string FilterText { get; set; }
    public int FilterStartIndex { get; set; }
    public int FilterEndIndex { get; set; }

    #endregion
    public static b_Craft ProcessRowForRetrieveByFilterText(SqlDataReader reader)
    {
      // Create instance of object
      b_Craft crafts = new b_Craft();

      // Load the object from the database
      crafts.LoadFromDatabaseForRetrieveByFilterText(reader);

      // Return result
      return crafts;
    }
    public void LoadFromDatabaseForRetrieveByFilterText(SqlDataReader reader)
    {
      int i = 0;
      try
      {

        // PartsId column, bigint, not null
        CraftId = reader.GetInt64(i++);

        // ClientLookupId column, nvarchar(31), not null
        ClientLookupId = reader.GetString(i++);

        // Description column
        Description = reader.GetString(i++);

      }
      catch (Exception ex)
      {
        // Diagnostics
        StringBuilder missing = new StringBuilder();

        try { reader["CraftId"].ToString(); }
        catch { missing.Append("PartId "); }

        try { reader["ClientLookupId"].ToString(); }
        catch { missing.Append("ClientLookupId "); }

        try { reader["Description"].ToString(); }
        catch { missing.Append("Description "); }

        StringBuilder msg = new StringBuilder();
        msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
        if (missing.Length > 0)
        {
          msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
        }

        throw new Exception(msg.ToString(), ex);
      }
    }

    // SOM-354
    public void RetrieveForSite(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, ref b_Craft[] data)
    {
      Database.SqlClient.ProcessRow<b_Craft> processRow = null;
      ArrayList results = null;
      SqlCommand command = null;
      string message = String.Empty;

      // Initialize the results
      data = new b_Craft[0];

      try
      {
        // Create the command to use in calling the stored procedures
        command = new SqlCommand();
        command.Connection = connection;
        command.Transaction = transaction;

        // Call the stored procedure to retrieve the data
        processRow = new Database.SqlClient.ProcessRow<b_Craft>(reader => { b_Craft obj = new b_Craft(); obj.LoadFromDatabase(reader); return obj; });
        results = Database.StoredProcedure.usp_Craft_RetrieveForSite.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

        // Extract the results
        if (null != results)
        {
          data = (b_Craft[])results.ToArray(typeof(b_Craft));
        }
        else
        {
          data = new b_Craft[0];
        }

        // Clear the results collection
        if (null != results)
        {
          results.Clear();
          results = null;
        }
      }
      finally
      {
        if (null != command)
        {
          command.Dispose();
          command = null;
        }
        processRow = null;
        results = null;
        message = String.Empty;
        callerUserInfoId = 0;
        callerUserName = String.Empty;
      }
    }
    // SOM-354
    public void ValidateInsert(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, ref List<b_StoredProcValidationError> data)
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
        results = Database.StoredProcedure.usp_Craft_ValidateInsert.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

    // SOM-354
    public void ValidateSave(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, ref List<b_StoredProcValidationError> data)
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
        results = Database.StoredProcedure.usp_Craft_ValidateSave.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
    public void DeleteInactivate(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, ref List<b_StoredProcValidationError> data)
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

        // Call the stored procedure to delete or inactivate 
        results = Database.StoredProcedure.usp_Craft_DeleteInactivate.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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