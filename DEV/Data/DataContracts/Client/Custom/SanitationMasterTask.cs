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
*                                               UpdateFromDatabaseObject_Extended
*                                               ToDatabaseObjectExtended
**************************************************************************************************
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using Database;
using Database.Business;

namespace DataContracts
{
  public partial class SanitationMasterTask : DataContractBase //, IStoredProcedureValidation
  {
    #region Properties
    public string ChargeTo_ClientLookupId { get; set; }
    public string OrderNumber { get; set; }

    public string ProcedureMaster_ClientLookupId { get; set; }
    #endregion

    public List<SanitationMasterTask> RetrieveAll(DatabaseKey dbKey)
    {
      SanitationMasterTask_RetrieveAll trans = new SanitationMasterTask_RetrieveAll()
      {
        CallerUserInfoId = dbKey.User.UserInfoId,
        CallerUserName = dbKey.UserName,

      };
      trans.dbKey = dbKey.ToTransDbKey();
      trans.Execute();
      return UpdateFromDatabaseObjectList(trans.SanitationMasterTaskList);
    }

    public List<SanitationMasterTask> SanitationMasterTaskRetrieveBySanitationMasterId(DatabaseKey dbKey, long SanitationMasterId)
    {
      List<SanitationMasterTask> SanitationMasterTaskListInContract = new List<SanitationMasterTask>();

      Database.SanitationMasterTask_RetrieveBySanitationMasterId trans =
          new Database.SanitationMasterTask_RetrieveBySanitationMasterId();


      trans.SanitationMasterTask = new b_SanitationMasterTask();
      trans.ClientId = dbKey.Personnel.ClientId;
      trans.SanitationMasterId = SanitationMasterId;
      trans.CallerUserInfoId = dbKey.User.UserInfoId;
      trans.CallerUserName = dbKey.UserName;
      trans.dbKey = dbKey.ToTransDbKey();
      trans.Execute();
      int i = 0;
      SanitationMasterTask task;
      trans.SanitationMasterTaskList.ForEach(b_obj =>
      {
        task = new SanitationMasterTask();
        task.UpdateFromDatabaseObject_Extended(b_obj);
        i++;
        if(i<10)
            task.OrderNumber ="00"+ i.ToString();
        else if(i<100)
            task.OrderNumber = "0" + i.ToString();
        else
            task.OrderNumber = i.ToString();
        SanitationMasterTaskListInContract.Add(task);
      });

      return SanitationMasterTaskListInContract;
    }

    public void UpdateFromDatabaseObject_Extended(b_SanitationMasterTask dbojb)
    {
      this.UpdateFromDatabaseObject(dbojb);
      this.ChargeTo_ClientLookupId = dbojb.ChargeTo_ClientLookupId;
      this.ProcedureMaster_ClientLookupId = dbojb.ProcedureMaster_ClientLookupId;
    }

    public b_SanitationMasterTask ToDatabaseObjectExtended()
    {
      b_SanitationMasterTask dbObj = this.ToDatabaseObject();
      dbObj.ChargeTo_ClientLookupId = this.ChargeTo_ClientLookupId;
      dbObj.ProcedureMaster_ClientLookupId = this.ProcedureMaster_ClientLookupId;
      return dbObj;
    }

    public static List<SanitationMasterTask> UpdateFromDatabaseObjectList(List<b_SanitationMasterTask> dbObjs)
    {
      List<SanitationMasterTask> result = new List<SanitationMasterTask>();

      foreach (b_SanitationMasterTask dbObj in dbObjs)
      {
        SanitationMasterTask tmp = new SanitationMasterTask();
        tmp.UpdateFromDatabaseObject(dbObj);
        result.Add(tmp);
      }
      return result;
    }
  }
}
