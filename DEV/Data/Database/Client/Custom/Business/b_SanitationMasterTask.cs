/*
**************************************************************************************************
* PROPRIETARY DATA 
**************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc. and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
**************************************************************************************************
* Copyright (c) 2014 by SOMAX Inc.. All rights reserved. 
**************************************************************************************************
* Date         JIRA Item Person                 Description
* ===========  ========= ====================== =================================================
* 2014-Aug-12  SOM-285   Roger Lawton           Added ChargeTo_ClientLookupId 
*                                               ProcedureMaster_ClientLookupId
*                                               LoadFromDatabase_Extended
**************************************************************************************************
*/

using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using Database.SqlClient;
using System.Collections.Generic;

namespace Database.Business
{
  public partial class b_SanitationMasterTask : DataBusinessBase
  {

    public string ChargeTo_ClientLookupId { get; set; }
    public string ProcedureMaster_ClientLookupId { get; set; }

    public void SanitationMasterTask_RetrieveBySanitationMasterId(
       SqlConnection connection,
       SqlTransaction transaction,
       long callerUserInfoId,
       string callerUserName,
       long ClientId,
       long SanitationMasterId,
       ref List<b_SanitationMasterTask> SanitationMasterTaskList
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
        SanitationMasterTaskList = Database.StoredProcedure.usp_SanitationMasterTask_RetrieveBySanitationMasterId.CallStoredProcedure(command, callerUserInfoId, callerUserName, ClientId, SanitationMasterId);// this);

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
    public void LoadFromDatabase_Extended(SqlDataReader reader)
    {
      // Execute the generated load from database
      int i = this.LoadFromDatabase(reader);
      // Add the additional items
      try
      {

        // Chargeto_ClientLookupId
       // ChargeTo_ClientLookupId = reader.GetString(i++);

        // ProcedureMaster_ClientLookupId
      //  ProcedureMaster_ClientLookupId = reader.GetString(i++);
      }
      catch (Exception ex)
      {
        // Diagnostics
        StringBuilder missing = new StringBuilder();

        //try { reader["Chargeto_ClientLookupId"].ToString(); }
        //catch { missing.Append("Chargeto_ClientLookupId "); }

        //try { reader["ProcedureMaster_ClientLookupId"].ToString(); }
        //catch { missing.Append("ProcedureMaster_ClientLookupId "); }

        StringBuilder msg = new StringBuilder();
        msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
        if (missing.Length > 0)
        {
          msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
        }

        throw new Exception(msg.ToString(), ex);
      }
    }

  }
}
