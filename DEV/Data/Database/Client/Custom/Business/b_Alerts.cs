/*
**************************************************************************************************
* PROPRIETARY DATA 
**************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc. and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
**************************************************************************************************
* Copyright (c) 2014-2017 by SOMAX Inc.. All rights reserved. 
**************************************************************************************************
* Date        JIRA Item Person            Description
* =========== ========= ================= =======================================================
* 2017-Dec-05 SOM-1515  Roger Lawton      Add AlertName Property and AlertClear Method
**************************************************************************************************
*/

using System.Data.SqlClient;

namespace Database.Business
{
  /// <summary>
  /// Business object that stores a record from the Alerts table.InsertIntoDatabase
  /// </summary>
  public partial class b_Alerts : DataBusinessBase
  {
    #region properties
    public string AlertName;
    public long PersonnelId;
    #endregion  
    public void InsertAndClear(SqlConnection connection,SqlTransaction transaction,long callerUserInfoId, string callerUserName)
    {
      SqlCommand command = null;

      try
      {
        command = connection.CreateCommand();
        if (null != transaction)
        {
          command.Transaction = transaction;
        }
        Database.StoredProcedure.usp_Alerts_CreateAndClear.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

    public void AlertClear(
       SqlConnection connection,
       SqlTransaction transaction,
       long callerUserInfoId,
       string callerUserName
   )
    {
      SqlCommand command = null;

      try
      {
        command = connection.CreateCommand();
        if (null != transaction)
        {
          command.Transaction = transaction;
        }
        Database.StoredProcedure.usp_Alerts_UpdateAlertIsClear.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
    public void ClearAlert(SqlConnection connection,SqlTransaction transaction,long callerUserInfoId,string callerUserName)
    {
      SqlCommand command = null;

      try
      {
        command = connection.CreateCommand();
        if (null != transaction)
        {
          command.Transaction = transaction;
        }
        Database.StoredProcedure.usp_Alerts_ClearAlert.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

  }
}
