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
    public partial class WorkOrderPlanEventLog : DataContractBase
    {
        #region Properties

        public string NameFirst { get; set; }
        public string NameMiddle { get; set; }
        public string NameLast { get; set; }
        public string Personnel { get; set; }
        public string Events { get; set; }
        public string PersonnelInitial { get; set; }

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

        private List<WorkOrderPlanEventLog> UpdateFromDatabaseObjectList(List<b_WorkOrderPlanEventLog> dbObjs)
        {
            List<WorkOrderPlanEventLog> result = new List<WorkOrderPlanEventLog>();

            foreach (b_WorkOrderPlanEventLog dbObj in dbObjs)
            {
                WorkOrderPlanEventLog tmp = new WorkOrderPlanEventLog();
                tmp.UpdateFromExtendedDatabaseObject(dbObj);
                result.Add(tmp);
            }
            return result;
        }

        public void UpdateFromExtendedDatabaseObject(b_WorkOrderPlanEventLog dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.Personnel = dbObj.NameFirst + " " + dbObj.NameLast;
            this.PersonnelInitial = dbObj.PersonnelInitial;
            switch (this.Event)
            {
                case WorkOrderEvents.Approved:
                    this.Events = WorkOrderEventLogFunction.Approved;
                    break;
                case WorkOrderEvents.Archived:
                    this.Events = WorkOrderEventLogFunction.Archived;
                    break;
                case WorkOrderEvents.Canceled:
                    this.Events = WorkOrderEventLogFunction.Canceled;
                    break;
                case WorkOrderEvents.Complete:
                    this.Events = WorkOrderEventLogFunction.Complete;
                    break;
                case WorkOrderEvents.Create:
                    this.Events = WorkOrderEventLogFunction.Create;
                    break;
                case WorkOrderEvents.Denied:
                    this.Events = WorkOrderEventLogFunction.Denied;
                    break;
                case WorkOrderEvents.Scheduled:
                    this.Events = WorkOrderEventLogFunction.Scheduled;
                    break;
                case WorkOrderEvents.WorkRequest:
                    this.Events = WorkOrderEventLogFunction.WorkRequest;
                    break;
            }
        }

        public b_WorkOrderPlanEventLog ToExtendedDatabaseObject()
        {
            b_WorkOrderPlanEventLog dbObj = this.ToDatabaseObject();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.WorkOrderPlanId = this.WorkOrderPlanId;
            return dbObj;
        }

        public List<WorkOrderPlanEventLog> RetriveByWorkOrderPlanId(DatabaseKey dbKey)
        {
            WorkOrderPlanEventLog_RetrieveByWorkOrderPlanId trans = new WorkOrderPlanEventLog_RetrieveByWorkOrderPlanId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.WorkOrderPlanEventLog = ToExtendedDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.WorkOrderPlanEventLogList);
        }


    }

}
