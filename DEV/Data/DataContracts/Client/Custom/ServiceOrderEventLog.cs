using Common.Constants;
using Database.Business;
using Database.Transactions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    [JsonObject]
    public partial class ServiceOrderEventLog
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

        private List<ServiceOrderEventLog> UpdateFromDatabaseObjectList(List<b_ServiceOrderEventLog> dbObjs)
        {
            List<ServiceOrderEventLog> result = new List<ServiceOrderEventLog>();

            foreach (b_ServiceOrderEventLog dbObj in dbObjs)
            {
                ServiceOrderEventLog tmp = new ServiceOrderEventLog();
                tmp.UpdateFromExtendedDatabaseObject(dbObj);
                result.Add(tmp);
            }
            return result;
        }

        public void UpdateFromExtendedDatabaseObject(b_ServiceOrderEventLog dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.Personnel = dbObj.NameFirst + " " + dbObj.NameLast;
            this.PersonnelInitial = dbObj.PersonnelInitial;
            this.Event = dbObj.Event;
            //switch (this.Event)
            //{
            //    case Serviceorder.Approved:
            //        this.Events = WorkOrderEventLogFunction.Approved;
            //        break;
            //    case WorkOrderEvents.Archived:
            //        this.Events = WorkOrderEventLogFunction.Archived;
            //        break;
            //    case WorkOrderEvents.Canceled:
            //        this.Events = WorkOrderEventLogFunction.Canceled;
            //        break;
            //    case WorkOrderEvents.Complete:
            //        this.Events = WorkOrderEventLogFunction.Complete;
            //        break;
            //    case WorkOrderEvents.Create:
            //        this.Events = WorkOrderEventLogFunction.Create;
            //        break;
            //    case WorkOrderEvents.Denied:
            //        this.Events = WorkOrderEventLogFunction.Denied;
            //        break;
            //    case WorkOrderEvents.Scheduled:
            //        this.Events = WorkOrderEventLogFunction.Scheduled;
            //        break;
            //    case WorkOrderEvents.WorkRequest:
            //        this.Events = WorkOrderEventLogFunction.WorkRequest;
            //        break;
            //}
        }

        public b_ServiceOrderEventLog ToExtendedDatabaseObject()
        {
            b_ServiceOrderEventLog dbObj = this.ToDatabaseObject();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.ServiceOrderId = this.ServiceOrderId;
            return dbObj;
        }

        public List<ServiceOrderEventLog> RetrieveByServiceOrderId(DatabaseKey dbKey)
        {
            ServiceOrderEventLog_RetrieveByServiceOrderId trans = new ServiceOrderEventLog_RetrieveByServiceOrderId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.ServiceOrderEventLog = ToExtendedDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.ServiceOrderEventLogList);
        }
    }
}
