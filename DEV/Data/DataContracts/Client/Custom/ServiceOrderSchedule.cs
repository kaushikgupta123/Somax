using Database.Business;
using Database.Client.Custom.Transactions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    [JsonObject]
    public partial class ServiceOrderSchedule : DataContractBase, IStoredProcedureValidation
    {
        #region Property
        public string ServiceAssigned_Name { get; set; }
        public List<b_ServiceOrderSchedule> ServiceOrderSchduleList { get; set; }
        public string ClientLookupId { get; set; }
        public string NameFirst { get; set; }
        public string NameLast { get; set; }
        public string NameMiddle { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool Flag { get; set; }
        public long SiteId { get; set; } // RKL 2020-May-06
        public List<List<ServiceOrderSchedule>> TotalRecords { get; set; }

        #endregion
        public List<b_ServiceOrderSchedule> ToDatabaseObjectPersonnelList()
        {
            List<b_ServiceOrderSchedule> dbObj = new List<b_ServiceOrderSchedule>();
            dbObj = this.ServiceOrderSchduleList;
            return dbObj;
        }
        public b_ServiceOrderSchedule ToDatabaseObjectPersonnel()
        {
            b_ServiceOrderSchedule dbObj = new b_ServiceOrderSchedule();
            dbObj.ServiceOrderScheduleId = this.ServiceOrderScheduleId;
            dbObj.ClientId = this.ClientId;
            dbObj.ServiceOrderId = this.ServiceOrderId;
            dbObj.PersonnelId = this.PersonnelId;
            dbObj.ScheduledDate = this.ScheduledDate;
            dbObj.ScheduledHours = this.ScheduledHours;
            dbObj.Shift = this.Shift;
            dbObj.Flag = this.Flag;
            dbObj.SiteId = this.SiteId;
            return dbObj;
        }
        public static List<ServiceOrderSchedule> UpdateFromDatabaseObjectPersonnelList(List<b_ServiceOrderSchedule> dbObjs)
        {
            List<ServiceOrderSchedule> result = new List<ServiceOrderSchedule>();

            foreach (b_ServiceOrderSchedule dbObj in dbObjs)
            {
                ServiceOrderSchedule tmp = new ServiceOrderSchedule();
                tmp.UpdateFromExtendedDatabaseObjectPersonnel(dbObj);
                result.Add(tmp);
            }
            return result;
        }
        public void UpdateFromExtendedDatabaseObjectPersonnel(b_ServiceOrderSchedule dbObj)
        {
            this.PersonnelId = dbObj.PersonnelId;
            this.NameFirst = string.IsNullOrEmpty(dbObj.NameFirst) ? "" : dbObj.NameFirst;
            this.NameLast = string.IsNullOrEmpty(dbObj.NameLast) ? "" : dbObj.NameLast;
            this.NameMiddle = string.IsNullOrEmpty(dbObj.NameMiddle) ? "" : dbObj.NameMiddle;
            this.FullName = string.IsNullOrEmpty(dbObj.FullName) ? "" : dbObj.FullName;
            this.ServiceAssigned_Name = string.IsNullOrEmpty(dbObj.UserName) ? "" : dbObj.UserName;
            this.Email = string.IsNullOrEmpty(dbObj.Email) ? "" : dbObj.Email;
        }
        public List<List<ServiceOrderSchedule>> RetrievePersonnel(DatabaseKey dbKey)
        {
            ServiceOrderSchdule_RetrievePersonnel trans = new ServiceOrderSchdule_RetrievePersonnel()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.ServiceOrderScheduleList = this.ToDatabaseObjectPersonnelList();
            trans.ServiceOrderSchedule = this.ToDatabaseObjectPersonnel();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            TotalRecords = new List<List<ServiceOrderSchedule>>();
            this.TotalRecords.Add(ServiceOrderSchedule.UpdateFromDatabaseObjectPersonnelList(trans.ServiceOrderSchedulePersonnelList[0]));
            this.TotalRecords.Add(ServiceOrderSchedule.UpdateFromDatabaseObjectPersonnelList(trans.ServiceOrderSchedulePersonnelList[1]));

            return this.TotalRecords;
        }
      

        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            throw new NotImplementedException();
        }
    }
}
