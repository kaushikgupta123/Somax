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
using Common.Constants;

namespace DataContracts
{
    [JsonObject]
    public partial class SanitationEventLog : DataContractBase
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

        private List<SanitationEventLog> UpdateFromDatabaseObjectList(List<b_SanitationEventLog> dbObjs)
        {
            List<SanitationEventLog> result = new List<SanitationEventLog>();

            foreach (b_SanitationEventLog dbObj in dbObjs)
            {
                SanitationEventLog tmp = new SanitationEventLog();
                tmp.UpdateFromExtendedDatabaseObject(dbObj);
                result.Add(tmp);
            }
            return result;
        }

        public void UpdateFromExtendedDatabaseObject(b_SanitationEventLog dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.Personnel = dbObj.NameFirst + " " + dbObj.NameLast;
            switch (this.Event)
            {
                case SanitationEvents.Approved:
                    this.Events = SanitationEventLogFunction.Approved;
                    break;
                case SanitationEvents.Archived:
                    this.Events = SanitationEventLogFunction.Archived;
                    break;
                case SanitationEvents.Canceled:
                    this.Events = SanitationEventLogFunction.Canceled;
                    break;
                case SanitationEvents.Complete:
                    this.Events = SanitationEventLogFunction.Complete;
                    break;
                case SanitationEvents.Create:
                    this.Events = SanitationEventLogFunction.Create;
                    break;
                case SanitationEvents.Denied:
                    this.Events = SanitationEventLogFunction.Denied;
                    break;
                case SanitationEvents.Fail:
                    this.Events = SanitationEventLogFunction.Fail;
                    break;
                case SanitationEvents.JobRequest:
                    this.Events = SanitationEventLogFunction.JobRequest;
                    break;
                case SanitationEvents.Passed:
                    this.Events = SanitationEventLogFunction.Passed;
                    break;
                case SanitationEvents.Scheduled:
                    this.Events = SanitationEventLogFunction.Scheduled;
                    break;
            }
        }

        public b_SanitationEventLog ToExtendedDatabaseObject()
        {
            b_SanitationEventLog dbObj = this.ToDatabaseObject();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.ObjectId = this.ObjectId;
            return dbObj;
        }

        public List<SanitationEventLog> RetriveBySanitationJobId(DatabaseKey dbKey)
        {
            SanitationEventLog_RetrieveBySanitationId trans = new SanitationEventLog_RetrieveBySanitationId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.SanitationEventLog = ToExtendedDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<SanitationEventLog> logList = new List<SanitationEventLog>();
            logList = UpdateFromDatabaseObjectList(trans.SanitationEventLogList);
           
            return logList;
        }

       

    }
}
