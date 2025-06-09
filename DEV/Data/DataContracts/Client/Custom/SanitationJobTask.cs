/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2015 by SOMAX Inc.
* All rights reserved. 
****************************************************************************************************
* Date        Task ID   Person             Description
* =========== ======== =================== =======================================================
* 2015-Mar-21 SOM-585  Roger Lawton        Changed from UTC to current date 
****************************************************************************************************
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Database;
using Database.Business;
using Common.Extensions;

namespace DataContracts
{
    public partial class SanitationJobTask
    {
        public long SiteId { get; set; }
        public string CompleteBy { get; set; }

        

        public static List<SanitationJobTask> UpdateFromDatabaseObjectList(List<b_SanitationJobTask> dbObjs)
        {
            List<SanitationJobTask> result = new List<SanitationJobTask>();

            foreach (b_SanitationJobTask dbObj in dbObjs)
            {
                SanitationJobTask tmp = new SanitationJobTask();
                tmp.UpdateFromDatabaseObject(dbObj);
                //tmp.UpdateFromDatabaseObjectExtended(dbObj);
                result.Add(tmp);
            }
            return result;
        }


        public List<SanitationJobTask> RetrieveAll(DatabaseKey dbKey)
        {
            SanitationJobTask_RetrieveAll trans = new SanitationJobTask_RetrieveAll()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,

            };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.SanitationJobTaskList);
        }

       

        public void UpdateBySanitationJobTaskId(DatabaseKey dbKey)
        {
            SanitationJobTask_UpdateBySanitationJobTaskId trans = new SanitationJobTask_UpdateBySanitationJobTaskId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            //Why do we need the "Custom" one 
            //trans.SanitationJobTask = this.ToDatabaseObjectCustom();
            trans.SanitationJobTask = this.ToDatabaseObject();
            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.SanitationJobTask);
        }

        public List<SanitationJobTask> SanitationJobTask_RetrieveForWorkBench(DatabaseKey dbKey)
        {
            SanitationJobTaskTransactions_RetrieveForWorkBench trans = new SanitationJobTaskTransactions_RetrieveForWorkBench()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.SanitationJobTask = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.SanitationJobTaskList);
        }

        public List<SanitationJobTask> RetrieveByJob(DatabaseKey dbKey, SanitationJobTask sanitationJobTask)
        {

            SanitationJobTask_RetrieveBySanitationJob trans = new SanitationJobTask_RetrieveBySanitationJob()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.SanitationJobTask = sanitationJobTask.ToDatabaseObject();
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.SanitationJobTaskList);
        }
        public List<SanitationJobTask> SanitationJobTask_RetrieveBy_SanitationJobId(DatabaseKey dbKey)
        {
            List<SanitationJobTask> SanitationMasterTaskListInContract = new List<SanitationJobTask>();

            SanitationJobTask_RetrieveBy_SanitationJobId trans = new SanitationJobTask_RetrieveBy_SanitationJobId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,

            };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.SanitationJobTask = ToDatabaseForSanitationTask();
            trans.Execute();
            return UpdateFromDatabaseObjectListForSanitation(trans.SanitationJobTaskList);

        }
        public b_SanitationJobTask ToDatabaseForSanitationTask()
        {
            b_SanitationJobTask dbObj = new b_SanitationJobTask();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.SanitationJobTaskId = this.SanitationJobTaskId;
            dbObj.SanitationJobId = this.SanitationJobId;
            return dbObj;
        }
        public static List<SanitationJobTask> UpdateFromDatabaseObjectListForSanitation(List<b_SanitationJobTask> dbObjs)
        {
            List<SanitationJobTask> result = new List<SanitationJobTask>();

            foreach (b_SanitationJobTask dbObj in dbObjs)
            {
                SanitationJobTask tmp = new SanitationJobTask();
                tmp.UpdateFromDatabaseObjectForSanitation(dbObj);
                //tmp.UpdateFromDatabaseObjectExtended(dbObj);
                result.Add(tmp);
            }
            return result;
        }
        public void UpdateFromDatabaseObjectForSanitation(b_SanitationJobTask dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.SanitationJobTaskId = dbObj.SanitationJobTaskId;
            this.SanitationJobId = dbObj.SanitationJobId;
            this.SanitationMasterTaskId = dbObj.SanitationMasterTaskId;
            this.CancelReason = dbObj.CancelReason;
            this.CompleteBy_PersonnelId = dbObj.CompleteBy_PersonnelId;
            this.CompleteBy = dbObj.CompleteBy;
            this.CompleteComments = dbObj.CompleteComments;
            this.CompleteDate = dbObj.CompleteDate;
            this.Description = dbObj.Description;
            this.Status = dbObj.Status;
            this.TaskId = dbObj.TaskId;
            this.RecordedValue = dbObj.RecordedValue;
            this.PerformTime = dbObj.PerformTime;
            this.UpdateIndex = dbObj.UpdateIndex;

            // Turn on auditing
            //AuditEnabled = true;
        }
        public void CreateNew_SanitationJobTask(DatabaseKey dbKey, string TimeZone)
        {
            CreateNew_SanitationJobTask trans = new CreateNew_SanitationJobTask();
            trans.SanitationJobTask = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObjectList(trans.SanitationJobTask, TimeZone);
        }
        public void UpdateFromDatabaseObjectList(b_SanitationJobTask dbObj, string TimeZone)
        {
            this.ClientId = dbObj.ClientId;
            this.SanitationJobTaskId = dbObj.SanitationJobTaskId;
            this.SanitationJobId = dbObj.SanitationJobId;
            this.SanitationMasterTaskId = dbObj.SanitationMasterTaskId;
            this.CancelReason = dbObj.CancelReason;
            this.CompleteBy_PersonnelId = dbObj.CompleteBy_PersonnelId;
            this.CompleteComments = dbObj.CompleteComments;
            this.CompleteDate = dbObj.CompleteDate;
            if (dbObj.CompleteDate != null && dbObj.CompleteDate != DateTime.MinValue)
            {
                this.CompleteDate = dbObj.CompleteDate.ToUserTimeZone(TimeZone);
            }
            else
            {
                this.CompleteDate = dbObj.CompleteDate;
            }
            this.Description = dbObj.Description;
            this.Status = dbObj.Status;
            this.TaskId = dbObj.TaskId;
            this.RecordedValue = dbObj.RecordedValue;
            this.PerformTime = dbObj.PerformTime;
            this.UpdateIndex = dbObj.UpdateIndex;

            // Turn on auditing
            AuditEnabled = true;
        }
        public void Update_SanitationJobTask(DatabaseKey dbKey)
        {
            Update_SanitationJobTaskBy_SanitationJobTaskId trans = new Update_SanitationJobTaskBy_SanitationJobTaskId();
            trans.SanitationJobTask = this.ToDatabaseObject();
            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.SanitationJobTask);
        }
        public void SanitationJobTask_RetrieveSingleBy_SanitationJobId(DatabaseKey dbKey)
        {
            SanitationJobTask_RetrieveSingleBy_SanitationJobId trans = new SanitationJobTask_RetrieveSingleBy_SanitationJobId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.SanitationJobTask = this.ToDatabaseForSanitationTask();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObjectForSanitation(trans.SanitationJobTask);
        }

    }
}
