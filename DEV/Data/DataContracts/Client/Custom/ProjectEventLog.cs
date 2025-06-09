using Common.Constants;
using Database;
using Database.Business;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    [JsonObject]
    public partial class ProjectEventLog : DataContractBase
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
        private List<ProjectEventLog> UpdateFromDatabaseObjectList(List<b_ProjectEventLog> dbObjs)
        {
            List<ProjectEventLog> result = new List<ProjectEventLog>();

            foreach (b_ProjectEventLog dbObj in dbObjs)
            {
                ProjectEventLog tmp = new ProjectEventLog();
                tmp.UpdateFromExtendedDatabaseObject(dbObj);
                result.Add(tmp);
            }
            return result;
        }

        public void UpdateFromExtendedDatabaseObject(b_ProjectEventLog dbObj)
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

        public b_ProjectEventLog ToExtendedDatabaseObject()
        {
            b_ProjectEventLog dbObj = this.ToDatabaseObject();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
          //  dbObj.WorkOrderId = this.WorkOrderId;
            return dbObj;
        }

        public List<ProjectEventLog> RetriveByProjectId(DatabaseKey dbKey)
        {
            ProjectEventLog_RetrieveByProjectId trans = new ProjectEventLog_RetrieveByProjectId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.ProjectEventLog = ToExtendedDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.ProjectEventLogList);
        }

    }
}
