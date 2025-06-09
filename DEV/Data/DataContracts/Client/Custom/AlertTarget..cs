/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2016 by SOMAX Inc.
* PopupAddWorkRequest
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID  Person             Description
* =========== ======== ================== ==========================================================
* 2016-Sep-01 SOM-1037 Roger Lawton       Added RetrieveTargetList
*                                         Added strings for first name, last name and 
*                                         personnel_clientlookupid     
* 2016-Oct-31 SOM-652  Roger Lawton       Added email for target                                     
****************************************************************************************************
 */

using System.Collections.Generic;
using Database.Business;
using Database.Transactions;

namespace DataContracts
{
  public partial class AlertTarget
  {
    #region properties
    public string FirstName { get; set; }
    public string LastName {get; set; }
    public string Personnel_ClientLookupId { get; set; }
    public string email_address { get; set; }
    public bool include_inactive { get; set; }
    public string UserName { get; set; }
        #endregion properties
        public List<AlertTarget> RetreiveTargetList(DatabaseKey dbKey)
    {
      AlertTarget_RetrieveTargetList trans = new AlertTarget_RetrieveTargetList()
      {
        CallerUserInfoId = dbKey.User.UserInfoId,
        CallerUserName = dbKey.UserName,
      };
      trans.AlertSetupId = this.AlertSetupId;
      trans.Include_Inactive = include_inactive;
      trans.dbKey = dbKey.ToTransDbKey();
      trans.Execute();
      return UpdateFromDatabaseObjectList(trans.AlertTargetList);
    }

    public List<AlertTarget> RetrieveAll(DatabaseKey dbKey)
    {
      AlertTarget_RetrieveAll trans = new AlertTarget_RetrieveAll()
      {
        CallerUserInfoId = dbKey.User.UserInfoId,
        CallerUserName = dbKey.UserName,

      };
      trans.dbKey = dbKey.ToTransDbKey();
      trans.Execute();
      return UpdateFromDatabaseObjectList(trans.AlertTargetList);
    }

    public void UpdateFromDatabaseObjectExtended(b_AlertTarget dbObj)
    {
      this.UpdateFromDatabaseObject(dbObj);
      // Entended Information
      this.FirstName = dbObj.FirstName;
      this.LastName = dbObj.LastName;
      this.Personnel_ClientLookupId = dbObj.Personnel_ClientLookupId;
      this.email_address = dbObj.email_address;
      this.UserName = dbObj.UserName;
      this.UserInfoId = dbObj.UserInfoId;
    }

    public static List<AlertTarget> UpdateFromDatabaseObjectList(List<b_AlertTarget> dbObjs)
    {
      List<AlertTarget> result = new List<AlertTarget>();

      foreach (b_AlertTarget dbObj in dbObjs)
      {
        AlertTarget tmp = new AlertTarget();
        tmp.UpdateFromDatabaseObjectExtended(dbObj);
        result.Add(tmp);
      }
      return result;
    }
  }
}
