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
* 2014-Jul-30  SOM-263   Roger Lawton           Corrected creation/updating of schedule records
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

namespace DataContracts
{
    [JsonObject]
    public partial class WorkOrderSchedule : DataContractBase, IStoredProcedureValidation
    {
        #region Property
        public string AssignedTo_PersonnelClientLookupId { get; set; }
        public string WorkAssigned_Name { get; set; }
        public List<b_WorkOrderSchedule> WorkOrderSchduleList { get; set; }
        public string ClientLookupId { get; set; }
        public string NameFirst { get; set; }
        public string NameLast { get; set; }
        public string NameMiddle { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool Flag { get; set; }
        public long SiteId { get; set; } // RKL 2020-May-06
        public List<List<WorkOrderSchedule>> TotalRecords { get; set; }
        public string Description { get; set; }
        public long CaseNo { get; set; }
        public int TotalCount { get; set; }
        #endregion



        public static List<WorkOrderSchedule> UpdateFromDatabaseObjectList(List<b_WorkOrderSchedule> dbObjs)
        {
            List<WorkOrderSchedule> result = new List<WorkOrderSchedule>();

            foreach (b_WorkOrderSchedule dbObj in dbObjs)
            {
                WorkOrderSchedule tmp = new WorkOrderSchedule();
                tmp.UpdateFromExtendedDatabaseObject(dbObj);
                result.Add(tmp);
            }
            return result;
        }

        public b_WorkOrderSchedule ToExtendedDatabaseObject()
        {
            b_WorkOrderSchedule dbObj = this.ToDatabaseObject();
            dbObj.AssignedTo_PersonnelClientLookupId = this.AssignedTo_PersonnelClientLookupId;
            dbObj.WorkAssigned_Name = this.WorkAssigned_Name;
            return dbObj;
        }

        public void UpdateFromExtendedDatabaseObject(b_WorkOrderSchedule dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.AssignedTo_PersonnelClientLookupId = string.IsNullOrEmpty(dbObj.AssignedTo_PersonnelClientLookupId) ? "" : dbObj.AssignedTo_PersonnelClientLookupId;
            this.WorkAssigned_Name = string.IsNullOrEmpty(dbObj.WorkAssigned_Name) ? "" : dbObj.WorkAssigned_Name;
        }

        public static List<b_WorkOrderSchedule> ToDatabaseObjectList(List<WorkOrderSchedule> objs)
        {
            List<b_WorkOrderSchedule> result = new List<b_WorkOrderSchedule>();

            foreach (WorkOrderSchedule obj in objs)
            {
                result.Add(obj.ToExtendedDatabaseObject());
            }

            return result;
        }

        public List<b_WorkOrderSchedule> ToDatabaseObjectList()
        {
            List<b_WorkOrderSchedule> dbObj = new List<b_WorkOrderSchedule>();
            dbObj = this.WorkOrderSchduleList;
            return dbObj;
        }


        public List<WorkOrderSchedule> RetriveByWorkOrderId(DatabaseKey dbKey)
        {
            WorkOrderSchdule_RetrieveByWorkOrderId trans = new WorkOrderSchdule_RetrieveByWorkOrderId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.WorkOrderScheduleList = this.ToDatabaseObjectList();
            trans.WorkOrderSchedule = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return WorkOrderSchedule.UpdateFromDatabaseObjectList(trans.WorkOrderScheduleList);
        }

        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            //WorkOrderSchedule_ValidateByClientLookupId trans = new WorkOrderSchedule_ValidateByClientLookupId()
            //{
            //    CallerUserInfoId = dbKey.User.UserInfoId,
            //    CallerUserName = dbKey.UserName,
            //};
            //trans.WorkOrderTask = this.ToDatabaseObject();            
            //trans.dbKey = dbKey.ToTransDbKey();
            //trans.Execute();

            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();

            //if (trans.StoredProcValidationErrorList != null)
            //{
            //    foreach (b_StoredProcValidationError error in trans.StoredProcValidationErrorList)
            //    {
            //        StoredProcValidationError tmp = new StoredProcValidationError();
            //        tmp.UpdateFromDatabaseObject(error);
            //        errors.Add(tmp);
            //    }
            //}

            return errors;
        }

        // SOM-263
        public void CreateForWorkOrder(DatabaseKey dbKey)
        {
            WorkOrderSchedule_CreateForWorkOrder trans = new WorkOrderSchedule_CreateForWorkOrder();
            trans.WorkOrderSchedule = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromExtendedDatabaseObject(trans.WorkOrderSchedule);
        }

        // SOM-263
        public void UpdateForWorkOrder(DatabaseKey dbKey)
        {
            WorkOrderSchedule_UpdateForWorkOrder trans = new WorkOrderSchedule_UpdateForWorkOrder();
            trans.WorkOrderSchedule = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromExtendedDatabaseObject(trans.WorkOrderSchedule);

        }
        // SOM-1144
        public void RemoveRecord(DatabaseKey dbKey)
        {
            WorkOrderSchedule_RemoveRecord trans = new WorkOrderSchedule_RemoveRecord();
            trans.WorkOrderSchedule = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromExtendedDatabaseObject(trans.WorkOrderSchedule);

        }
        public void UpdateForWorkOrderScheduled(DatabaseKey dbKey)
        {
            WorkOrderSchedule_EditandSaveScheduled trans = new WorkOrderSchedule_EditandSaveScheduled();
            trans.WorkOrderSchedule = this.ToDatabaseObject();
            trans.WorkOrderSchedule.WorkOrderSchedId = this.WorkOrderSchedId;
            trans.WorkOrderSchedule.ScheduledStartDate = this.ScheduledStartDate;
            trans.WorkOrderSchedule.ScheduledHours = this.ScheduledHours;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromExtendedDatabaseObject(trans.WorkOrderSchedule);

        }
        public void DeleteScheduledWorOrder(DatabaseKey dbKey)
        {
            WorkOrderSchedule_DeleteScheduledWorOrder trans = new WorkOrderSchedule_DeleteScheduledWorOrder();
            trans.WorkOrderSchedule = this.ToDatabaseObject();
            trans.WorkOrderSchedule.WorkOrderSchedId = this.WorkOrderSchedId;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromExtendedDatabaseObject(trans.WorkOrderSchedule);

        }

        public List<List<WorkOrderSchedule>> RetrievePersonnel(DatabaseKey dbKey)
        {
            WorkOrderSchdule_RetrievePersonnel trans = new WorkOrderSchdule_RetrievePersonnel()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.WorkOrderScheduleList = this.ToDatabaseObjectPersonnelList();
            trans.WorkOrderSchedule = this.ToDatabaseObjectPersonnel();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            TotalRecords = new List<List<WorkOrderSchedule>>();
            this.TotalRecords.Add(WorkOrderSchedule.UpdateFromDatabaseObjectPersonnelList(trans.WorkOrderSchedulePersonnelList[0]));
            this.TotalRecords.Add(WorkOrderSchedule.UpdateFromDatabaseObjectPersonnelList(trans.WorkOrderSchedulePersonnelList[1]));

            return this.TotalRecords;
        }
        public List<List<WorkOrderSchedule>> RetrievePersonnelByAssetGroupMasterQuery(DatabaseKey dbKey)
        {
            WorkOrderSchdule_RetrievePersonnelByAssetGroupMasterQuery trans = new WorkOrderSchdule_RetrievePersonnelByAssetGroupMasterQuery()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.WorkOrderScheduleList = this.ToDatabaseObjectPersonnelList();
            trans.WorkOrderSchedule = this.ToDatabaseObjectPersonnel();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            TotalRecords = new List<List<WorkOrderSchedule>>();
            this.TotalRecords.Add(WorkOrderSchedule.UpdateFromDatabaseObjectPersonnelList(trans.WorkOrderSchedulePersonnelList[0]));
            this.TotalRecords.Add(WorkOrderSchedule.UpdateFromDatabaseObjectPersonnelList(trans.WorkOrderSchedulePersonnelList[1]));

            return this.TotalRecords;
        }
        public b_WorkOrderSchedule ToDatabaseObjectPersonnel()
        {
            b_WorkOrderSchedule dbObj = new b_WorkOrderSchedule();
            dbObj.ClientId = this.ClientId;
            dbObj.WorkOrderSchedId = this.WorkOrderSchedId;
            dbObj.WorkOrderId = this.WorkOrderId;
            dbObj.PersonnelId = this.PersonnelId;
            dbObj.Craft = this.Craft;
            dbObj.Crew = this.Crew;
            dbObj.Rescheduled = this.Rescheduled;
            dbObj.ScheduledStartDate = this.ScheduledStartDate;
            dbObj.ScheduledHours = this.ScheduledHours;
            dbObj.Shift = this.Shift;
            dbObj.WorkOrderCompleted = this.WorkOrderCompleted;
            dbObj.UpdateIndex = this.UpdateIndex;
            dbObj.Flag = this.Flag;
            dbObj.SiteId = this.SiteId;
            return dbObj;
        }
        public List<b_WorkOrderSchedule> ToDatabaseObjectPersonnelList()
        {
            List<b_WorkOrderSchedule> dbObj = new List<b_WorkOrderSchedule>();
            dbObj = this.WorkOrderSchduleList;
            return dbObj;
        }
        public static List<WorkOrderSchedule> UpdateFromDatabaseObjectPersonnelList(List<b_WorkOrderSchedule> dbObjs)
        {
            List<WorkOrderSchedule> result = new List<WorkOrderSchedule>();

            foreach (b_WorkOrderSchedule dbObj in dbObjs)
            {
                WorkOrderSchedule tmp = new WorkOrderSchedule();
                tmp.UpdateFromExtendedDatabaseObjectPersonnel(dbObj);
                result.Add(tmp);
            }
            return result;
        }
        public void UpdateFromExtendedDatabaseObjectPersonnel(b_WorkOrderSchedule dbObj)
        {
            this.PersonnelId = dbObj.PersonnelId;
            this.NameFirst = string.IsNullOrEmpty(dbObj.NameFirst) ? "" : dbObj.NameFirst;
            this.NameLast = string.IsNullOrEmpty(dbObj.NameLast) ? "" : dbObj.NameLast;
            this.NameMiddle = string.IsNullOrEmpty(dbObj.NameMiddle) ? "" : dbObj.NameMiddle;
            this.FullName = string.IsNullOrEmpty(dbObj.FullName) ? "" : dbObj.FullName;
            this.WorkAssigned_Name = string.IsNullOrEmpty(dbObj.UserName) ? "" : dbObj.UserName;
            this.Email = string.IsNullOrEmpty(dbObj.Email) ? "" : dbObj.Email;
        }
        public void RetrieveByWorkOrderIdAndSchdeuleId(DatabaseKey dbKey)
        {
            WorkOrderSchdule_RetrieveByWorkOrderIdAndSchdeuleId trans = new WorkOrderSchdule_RetrieveByWorkOrderIdAndSchdeuleId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            //trans.WorkOrderScheduleList = this.ToDatabaseObjectList();
            trans.WorkOrderSchedule = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            this.UpdateFromDatabaseObjectRetrieveByWorkOrderIdAndSchdeuleId(trans.results);
        }
        public void UpdateFromDatabaseObjectRetrieveByWorkOrderIdAndSchdeuleId(b_WorkOrderSchedule dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.NameFirst = dbObj.NameFirst;
            this.NameLast = dbObj.NameLast;
            this.Description = dbObj.Description;
            this.ClientLookupId = dbObj.ClientLookupId;
        }
        public void RemoveWorkOrderScheduleForLaborScheduling(DatabaseKey dbKey)
        {
            WorkOrderSchedule_RemoveWorkOrderScheduleForLaborScheduling trans = new WorkOrderSchedule_RemoveWorkOrderScheduleForLaborScheduling()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.WorkOrderSchedule = this.ToDatabaseObject();
            //trans.WorkOrderSchedule.WorkOrderSchedId = this.WorkOrderSchedId;
            //trans.WorkOrderSchedule.ScheduledStartDate = this.ScheduledStartDate;
            //trans.WorkOrderSchedule.ScheduledHours = this.ScheduledHours;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.WorkOrderSchedule);
        }

        public void RemoveWorkOrderScheduleForResourceList(DatabaseKey dbKey)
        {
            WorkOrderSchedule_RemoveWorkOrderScheduleForResourceList trans = new WorkOrderSchedule_RemoveWorkOrderScheduleForResourceList()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.WorkOrderSchedule = this.ToDatabaseObject();
            //trans.WorkOrderSchedule.WorkOrderSchedId = this.WorkOrderSchedId;
            //trans.WorkOrderSchedule.ScheduledStartDate = this.ScheduledStartDate;
            //trans.WorkOrderSchedule.ScheduledHours = this.ScheduledHours;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.WorkOrderSchedule);
        }
        public void DragWorkOrderScheduleForLaborScheduling(DatabaseKey dbKey)
        {
            WorkOrderSchedule_DragWorkOrderScheduleForLaborScheduling trans = new WorkOrderSchedule_DragWorkOrderScheduleForLaborScheduling()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.WorkOrderSchedule = this.ToDatabaseObject();
            //trans.WorkOrderSchedule.WorkOrderSchedId = this.WorkOrderSchedId;
            //trans.WorkOrderSchedule.ScheduledStartDate = this.ScheduledStartDate;
            //trans.WorkOrderSchedule.ScheduledHours = this.ScheduledHours;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.WorkOrderSchedule);
        }

        #region Dashboard
               
        public List<WorkOrderSchedule> RetrieveCountWorkorderScheduleByComplete(DatabaseKey dbKey)
        {
            WorkOrderSchedule_RetrieveCountWorkOrderScheduleByComplete trans = new WorkOrderSchedule_RetrieveCountWorkOrderScheduleByComplete()
            {                
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.WorkOrderSchedule = this.ToDatabaseObjectRecordsCountByCreateDate();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseObjectRecordsCountByCreateDate(trans.WorkOrderScheduleList);
        }
        public b_WorkOrderSchedule ToDatabaseObjectRecordsCountByCreateDate()
        {
            b_WorkOrderSchedule dbObj = this.ToDatabaseObject();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;            
            dbObj.CaseNo = this.CaseNo;
            dbObj.PersonnelId = this.PersonnelId;
            return dbObj;
        }

       
        public List<WorkOrderSchedule> UpdateFromDatabaseObjectRecordsCountByCreateDate(List<b_WorkOrderSchedule> dbObjs)
        {
            List<WorkOrderSchedule> result = new List<WorkOrderSchedule>();

            foreach (b_WorkOrderSchedule dbObj in dbObjs)
            {
               WorkOrderSchedule tmp = new WorkOrderSchedule();
                tmp.UpdateFromExtendedDatabastRetriveByWorkOrderIdV2eObject(dbObj);
                result.Add(tmp);
              
            }
            return result;
        }
        public void UpdateFromExtendedDatabastRetriveByWorkOrderIdV2eObject(b_WorkOrderSchedule dbObj)
        {
            this.ScheduledStartDate = dbObj.ScheduledStartDate;            
            this.TotalCount = dbObj.TotalCount;

        }
        public List<WorkOrderSchedule> RetrieveCountWorkorderScheduleByInComplete(DatabaseKey dbKey)
        {
            WorkOrderSchedule_RetrieveCountWorkOrderScheduleByInComplete trans = new WorkOrderSchedule_RetrieveCountWorkOrderScheduleByInComplete()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.WorkOrderSchedule = this.ToDatabaseObjectRecordsCountByCreateDate();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseObjectRecordsCountByCreateDate(trans.WorkOrderScheduleList);
        }
        #endregion

        #region V2-944
        public void UpdateFromDatabaseObjectPrintWorkOrderScheduleExtended(b_WorkOrderSchedule dbObj)
        {
            this.WorkOrderId = dbObj.WorkOrderId;
            this.ScheduledStartDate = dbObj.ScheduledStartDate;
            this.ScheduledHours = dbObj.ScheduledHours;
            this.NameFirst = dbObj.NameFirst;
            this.NameLast = dbObj.NameLast;
        }
        #endregion
    }
}
