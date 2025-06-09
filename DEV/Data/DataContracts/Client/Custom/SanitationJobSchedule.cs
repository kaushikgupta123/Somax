using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using Database;
using Database.Business;

namespace DataContracts
{
    public partial class SanitationJobSchedule
    {
        #region Properties
        public long SiteId { get; set; }
        public string ClientLookupId { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }

        #endregion
        public List<SanitationJobSchedule> RetrieveAllBy_SanitationJobId(DatabaseKey dbKey)
        {
            SanitationJobSchedule_RetriveAllBySanitationJobId trans = new SanitationJobSchedule_RetriveAllBySanitationJobId();
            trans.SanitationJobSchedule = this.ToDatabaseObjectForSanitationJobSchedule();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.SanitationJobScheduleList);
        }

        public void RetrieveSingleBy_SanitationJobId(DatabaseKey dbKey)
        {
            SanitationJobSchedule_RetrieveSingleBySanitationJobId trans = new SanitationJobSchedule_RetrieveSingleBySanitationJobId();
            trans.SanitationJobSchedule = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObjectSanitationJobSchedule(trans.SanitationJobSchedule);
        }


        public b_SanitationJobSchedule ToDatabaseObjectForSanitationJobSchedule()
        {
            b_SanitationJobSchedule dbObj = new b_SanitationJobSchedule();
            dbObj.ClientId = this.ClientId;
            dbObj.SanitationJobScheduleId = this.SanitationJobScheduleId;
            dbObj.SanitationJobId = this.SanitationJobId;
            dbObj.PersonnelId = this.PersonnelId;
            dbObj.SiteId = this.SiteId;           
           
            return dbObj;
        }      

        public static List<SanitationJobSchedule> UpdateFromDatabaseObjectList(List<b_SanitationJobSchedule> dbObjs)
        {
            List<SanitationJobSchedule> result = new List<SanitationJobSchedule>();

            foreach (b_SanitationJobSchedule dbObj in dbObjs)
            {
                SanitationJobSchedule tmp = new SanitationJobSchedule();
                tmp.UpdateFromDatabaseObjectSanitationJobSchedule(dbObj);
                result.Add(tmp);
            }
            return result;
        }

        public void UpdateFromDatabaseObjectSanitationJobSchedule(b_SanitationJobSchedule dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.SanitationJobScheduleId = dbObj.SanitationJobScheduleId;
            this.SanitationJobId = dbObj.SanitationJobId;
            this.PersonnelId = dbObj.PersonnelId;
            this.ClientLookupId = dbObj.ClientLookupId;
            this.ScheduledStartDate = dbObj.ScheduledStartDate;
            this.ScheduledHours = dbObj.ScheduledHours;
            this.CreatedBy = dbObj.CreatedBy;
            this.Name = dbObj.Name;
            this.CreateDate = dbObj.CreateDate;
            this.ModifyBy = dbObj.ModifyBy;
            this.ModifyDate = dbObj.ModifyDate;
            this.UpdateIndex = dbObj.UpdateIndex;
            this.Del = dbObj.Del;
            

            // Turn on auditing
            AuditEnabled = true;
        }

        #region Insert,UpDate,Delete Work

        public void CreateForSanitationJob(DatabaseKey dbKey)
        {
            SanitationJobSchedule_CreateForSanitationJob trans = new SanitationJobSchedule_CreateForSanitationJob();
            trans.SanitationJobSchedule = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.SanitationJobSchedule);
        }

        public void UpdateForSanitationJob(DatabaseKey dbKey)
        {
            SanitationJobSchedule_UpdateForSanitationJob trans = new SanitationJobSchedule_UpdateForSanitationJob();
            trans.SanitationJobSchedule = this.ToDatabaseObject();
            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.SanitationJobSchedule);
        }

        public void DeleteForSanitationJob(DatabaseKey dbKey)
        {
            SanitationJobSchedule_DeleteForSanitationJob trans = new SanitationJobSchedule_DeleteForSanitationJob();
            trans.SanitationJobSchedule = this.ToDatabaseObject();
            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.SanitationJobSchedule);
        }



        #endregion
    }
}
