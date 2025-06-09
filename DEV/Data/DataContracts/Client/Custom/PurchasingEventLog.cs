/*
**************************************************************************************************
* PROPRIETARY DATA 
**************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc. and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
**************************************************************************************************
* Copyright (c) 2018-2019 by SOMAX Inc.. All rights reserved. 
**************************************************************************************************
* Date        JIRA Item Person          Description
* =========== ========= =============== ==========================================================
* 2019-May-10 SOM-1680  Roger Lawton    Added Timezone as a parameter to convert transaction date
**************************************************************************************************
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using Database;
using Database.Business;
using Database.Client.Custom.Transactions;
using Newtonsoft.Json;
using Common.Constants;
using Common.Extensions;

namespace DataContracts
{
    [JsonObject]
    public partial class PurchasingEventLog : DataContractBase
    {
        #region Properties

        public string NameFirst { get; set; }
        public string NameMiddle { get; set; }
        public string NameLast { get; set; }
        public string Personnel { get; set; }
        public string Events { get; set; }
        public string NameFull
        {
            get
            {
                string name = NameLast.Trim() + ", " + NameFirst.Trim() + " " + NameMiddle.Trim();
                return (string.Compare(",", name.Trim()) == 0) ? "" : name;  // Ensure "," won't be returned if _NameLast, _NameFirst, and _NameMiddle are empty
            }
        }
        public string FullName
        {
            get
            {
                string name = NameFirst.Trim() + " " + NameLast.Trim();
                return (string.Compare(",", name.Trim()) == 0) ? "" : name;  // Ensure "," won't be returned if _NameLast, _NameFirst, and _NameMiddle are empty
            }
        }

        #endregion

        private List<PurchasingEventLog> UpdateFromDatabaseObjectList(List<b_PurchasingEventLog> dbObjs, string timezone)
        {
            List<PurchasingEventLog> result = new List<PurchasingEventLog>();

            foreach (b_PurchasingEventLog dbObj in dbObjs)
            {
                PurchasingEventLog tmp = new PurchasingEventLog();
                tmp.UpdateFromExtendedDatabaseObject(dbObj,timezone);
                result.Add(tmp);
            }
            return result;
        }

        public void UpdateFromExtendedDatabaseObject(b_PurchasingEventLog dbObj,  string timezone)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.Personnel = dbObj.NameFirst + " " + dbObj.NameLast;
            // SOM-1680 
            // Convert Transaction Date to User's Time Zone
            this.TransactionDate = this.TransactionDate.ToUserTimeZone(timezone);
            switch (this.Event)
            {
                case PurchasingEvents.Approved:
                    this.Events = PurchasingEventLogFunction.Approved;
                    break;
                case PurchasingEvents.Archived:
                    this.Events = PurchasingEventLogFunction.Archived;
                    break;
                case PurchasingEvents.Canceled:
                    this.Events = PurchasingEventLogFunction.Canceled;
                    break;
                case PurchasingEvents.ReceiptComplete:
                    this.Events = PurchasingEventLogFunction.ReceiptComplete;
                    break;
                //case Data.Common.Constants.PurchasingEvents.POCreate:
                //    this.Events = loc.PurchasingEventLogFunction.POCreate;
                //    break;
                //case Data.Common.Constants.PurchasingEvents.PRCreate:
                //  this.Events = loc.PurchasingEventLogFunction.PRCreate;
                //  break;
                case PurchasingEvents.Denied:
                    this.Events = PurchasingEventLogFunction.Denied;
                    break;
                case PurchasingEvents.Void:
                    this.Events = PurchasingEventLogFunction.Void;
                    break;
                case PurchasingEvents.POOpen:
                    this.Events = PurchasingEventLogFunction.POOpen;
                    break;
                case PurchasingEvents.PROpen:
                    this.Events = PurchasingEventLogFunction.PROpen;
                    break;
                case PurchasingEvents.ReceiptPartial:
                    this.Events = PurchasingEventLogFunction.ReceiptPartial;
                    break;
                case PurchasingEvents.Resubmit:
                    this.Events = PurchasingEventLogFunction.Resubmit;
                    break;
                case PurchasingEvents.EmailToVendor:
                    this.Events = PurchasingEventLogFunction.EmailToVendor;
                    break;
                case PurchasingEvents.SendForApproval:
                    this.Events = PurchasingEventLogFunction.SendForApproval;
                    break;
                case PurchasingEvents.Import:
                    this.Events = PurchasingEventLogFunction.Import;
                    break;
                case PurchasingEvents.ImportUpdate:
                    this.Events = PurchasingEventLogFunction.ImportUpdate;
                    break;
            }

        }

        public b_PurchasingEventLog ToExtendedDatabaseObject()
        {
            b_PurchasingEventLog dbObj = this.ToDatabaseObject();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.ObjectId = this.ObjectId;
            return dbObj;
        }

        public List<PurchasingEventLog> RetriveByObjectId(DatabaseKey dbKey, string timezone)
        {
            PurchasingEventLogTransactions_RetrieveByObjectId trans = new PurchasingEventLogTransactions_RetrieveByObjectId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.PurchasingEventLog = ToExtendedDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<PurchasingEventLog> logList = new List<PurchasingEventLog>();
            logList = UpdateFromDatabaseObjectList(trans.PurchasingEventLogList, timezone);
           
            return logList;
        }

       

    }
}
