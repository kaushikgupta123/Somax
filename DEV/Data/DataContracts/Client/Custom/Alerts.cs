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
* 2017-Dec-05 SOM-1515  Roger Lawton      Add ClearAlert Method
**************************************************************************************************
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Reflection;

using Database;
using Database.Business;

namespace DataContracts
{
    
    public partial class Alerts : DataContractBase 
    {
    #region properties
    public string AlertName;
    public long PersonnelId;

    #endregion properties
    #region Transaction Methods



    public void UpdateAlert(DatabaseKey dbKey) 
        {
            Alerts_UpdateClear trans = new Alerts_UpdateClear();
            trans.Alerts = this.ToDatabaseObject();
			      trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            
            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.Alerts);
        }
        
        public void AlertClear(DatabaseKey dbKey)
        {
            Alerts_RetrieveList trans = new Alerts_RetrieveList();
            trans.Alerts = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.Alerts);
        }

        /// <summary>
        /// ClearAlert
        /// Set the alertname, objectname, objectid and optionally personnelid
        /// sets all alertuser records to "isread" for the alertname, objectname and objectid 
        /// if personnelid is not zero - only updates the items for that person
        /// if personnelid is zero - updates all for the alertname, objectname and objectid 
        /// </summary>
        /// <param name="dbKey"></param>
        public void ClearAlert(DatabaseKey dbKey)
        {
          Alerts_ClearAlert trans = new Alerts_ClearAlert();
          trans.Alerts = this.ToDatabaseObject();
          trans.Alerts.AlertName = this.AlertName;
          trans.Alerts.PersonnelId = this.PersonnelId;
          trans.dbKey = dbKey.ToTransDbKey();
          trans.Execute();
          UpdateFromDatabaseObject(trans.Alerts);
        }

    #endregion




  }
}
