/*
******************************************************************************
* PROPRIETARY DATA 
******************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
******************************************************************************
* Copyright (c) 2014 by SOMAX Inc.
* All rights reserved. 
******************************************************************************
* Date        JIRA ID Person            Description
* =========== ======= ================= ===================================
* 2014-Aug-01 SOM-246 Roger Lawton      Added Class
* ******************************************************************************
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using Database;
using Database.Business;
using Database.Client.Custom.Transactions;

namespace DataContracts
{
    public partial class LocalizedList:DataContractBase
    {
        public long SiteId { get; set; }
        #region @APM,@CMMS,@Sanit
        public bool? APM { get; set; }
        public bool? CMMS { get; set; }
        public bool? Sanit { get; set; }
        public bool? Fleet { get; set; }    // V2475

        public bool? UsePartMaster { get; set; }

        public bool? UseShoppingCart { get; set; }
        public string PackageLevel { get; set; }
        #endregion

        #region Transaction Methods

        public List<LocalizedList> RetrieveLanguageSpecificList(DatabaseKey dbKey)
        {
            RetrieveLanguageSpecificLocalizedList trans = new RetrieveLanguageSpecificLocalizedList();
            trans.LocalizedList = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.LocalizedList.SiteId = this.SiteId;
            trans.LocalizedList.APM = this.APM;
            trans.LocalizedList.CMMS = this.CMMS;
            trans.LocalizedList.Sanit = this.Sanit;
            trans.LocalizedList.Fleet = this.Fleet;
            trans.LocalizedList.UsePartMaster = this.UsePartMaster;
            trans.LocalizedList.UseShoppingCart = this.UseShoppingCart;
            trans.LocalizedList.PackageLevel = this.PackageLevel;
            trans.Execute();

            List<b_LocalizedList> lookup = trans.LocalizedListList;
            List<LocalizedList> result = UpdateFromDatabaseObjectList(lookup);     
            
            return result;
            
        }
        public static List<LocalizedList> UpdateFromDatabaseObjectList(List<b_LocalizedList> dbObjs)
        {
          List<LocalizedList> result = new List<LocalizedList>();

          foreach (b_LocalizedList dbObj in dbObjs)
          {
            LocalizedList tmp = new LocalizedList();
            tmp.UpdateFromDatabaseObject(dbObj);
            result.Add(tmp);
          }
          return result;
        }

        #endregion
    }
}
