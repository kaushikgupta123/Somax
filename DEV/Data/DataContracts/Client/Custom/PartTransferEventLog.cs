/*
**************************************************************************************************
* PROPRIETARY DATA 
**************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc. and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
**************************************************************************************************
* Copyright (c) 2018 by SOMAX Inc.. All rights reserved. 
**************************************************************************************************
* Date         JIRA Item Person            Description
* ===========  ========= ================= =======================================================
*
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
using Common.Extensions;

namespace DataContracts
{
    [JsonObject]
    public partial class PartTransferEventLog : DataContractBase
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

        private List<PartTransferEventLog> UpdateFromDatabaseObjectList(List<b_PartTransferEventLog> dbObjs)
        {
            List<PartTransferEventLog> result = new List<PartTransferEventLog>();

            foreach (b_PartTransferEventLog dbObj in dbObjs)
            {
                PartTransferEventLog tmp = new PartTransferEventLog();
                tmp.UpdateFromExtendedDatabaseObject(dbObj);
                result.Add(tmp);
            }
            return result;
        }

        public void UpdateFromExtendedDatabaseObject(b_PartTransferEventLog dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.Personnel = dbObj.NameFirst + " " + dbObj.NameLast;
            //switch (this.Event)
            //{
            //    case Common.Constants.PartTransferEvents.Canceled:
            //        this.Events = "Canceled";
            //        break;
            //    case Common.Constants.PartTransferEvents.Complete:
            //        this.Events = "Complete";
            //        break;
            //    case Common.Constants.PartTransferEvents.Created:
            //        this.Events = "Created";
            //        break;
            //    case Common.Constants.PartTransferEvents.Denied:
            //        this.Events = "Denied;
            //        break;
            //    case Common.Constants.PartTransferEvents.ForceComplete:
            //        this.Events = "ForceComplete;
            //        break;
            //    case Common.Constants.PartTransferEvents.ForceCompPend:
            //        this.Events = "ForceCompPend;
            //        break;
            //    case Common.Constants.PartTransferEvents.Issue:
            //        this.Events = "Issue;
            //        break;
            //    case Common.Constants.PartTransferEvents.Receipt:
            //        this.Events = "Receipt;
            //        break;
            //    case Common.Constants.PartTransferEvents.Sent:
            //        this.Events = "Sent;
            //        break;
            //}
        }

        public b_PartTransferEventLog ToExtendedDatabaseObject()
        {
            b_PartTransferEventLog dbObj = this.ToDatabaseObject();
            dbObj.ClientId = this.ClientId;
            dbObj.PartTransferId = this.PartTransferId;
            return dbObj;
        }

        public List<PartTransferEventLog> RetriveByPartTransferId(DatabaseKey dbKey)
        {
            PartTransferEventLog_ReteriveByPartTransferId trans = new PartTransferEventLog_ReteriveByPartTransferId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.PartTransferEventLog = ToExtendedDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.PartTransferEventLogList);
        }
        public void RetrieveForAlert(DatabaseKey dbKey,  string timezone)
        {
            PartTransferEventLog_RetrieveForAlert trans = new PartTransferEventLog_RetrieveForAlert();
            trans.PartTransferEventLog = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromExtendedDatabaseObject(trans.PartTransferEventLog,  timezone);
        }


        public void UpdateFromExtendedDatabaseObject(b_PartTransferEventLog dbObj, string timezone)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.Personnel = dbObj.NameFirst + " " + dbObj.NameLast;
            this.NameFirst = dbObj.NameFirst;
            this.NameLast = dbObj.NameLast;
            this.NameMiddle = dbObj.NameMiddle;
            this.TransactionDate = dbObj.TransactionDate.ToUserTimeZone(timezone);
            //switch (this.Event)
            //{
            //    case Common.Constants.PartTransferEvents.Canceled:
            //        this.Events = loc.PartTransferEventLogFunction.Canceled;
            //        break;
            //    case Common.Constants.PartTransferEvents.Complete:
            //        this.Events = loc.PartTransferEventLogFunction.Complete;
            //        break;
            //    case Common.Constants.PartTransferEvents.Created:
            //        this.Events = loc.PartTransferEventLogFunction.Created;
            //        break;
            //    case Common.Constants.PartTransferEvents.Denied:
            //        this.Events = loc.PartTransferEventLogFunction.Denied;
            //        break;
            //    case Common.Constants.PartTransferEvents.ForceComplete:
            //        this.Events = loc.PartTransferEventLogFunction.ForceComplete;
            //        break;
            //    case Common.Constants.PartTransferEvents.ForceCompPend:
            //        this.Events = loc.PartTransferEventLogFunction.ForceCompPend;
            //        break;
            //    case Common.Constants.PartTransferEvents.Issue:
            //        this.Events = loc.PartTransferEventLogFunction.Issue;
            //        break;
            //    case Common.Constants.PartTransferEvents.Receipt:
            //        this.Events = loc.PartTransferEventLogFunction.Receipt;
            //        break;
            //    case Common.Constants.PartTransferEvents.Sent:
            //        this.Events = loc.PartTransferEventLogFunction.Sent;
            //        break;
            //}
        }
    }

}
