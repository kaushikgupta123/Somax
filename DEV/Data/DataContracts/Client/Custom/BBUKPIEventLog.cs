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
    public partial class BBUKPIEventLog : DataContractBase
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

        private List<BBUKPIEventLog> UpdateFromDatabaseObjectList(List<b_BBUKPIEventLog> dbObjs)
        {
            List<BBUKPIEventLog> result = new List<BBUKPIEventLog>();

            foreach (b_BBUKPIEventLog dbObj in dbObjs)
            {
                BBUKPIEventLog tmp = new BBUKPIEventLog();
                tmp.UpdateFromExtendedDatabaseObject(dbObj);
                result.Add(tmp);
            }
            return result;
        }

        public void UpdateFromExtendedDatabaseObject(b_BBUKPIEventLog dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.Personnel = dbObj.NameFirst + " " + dbObj.NameLast;
            this.PersonnelInitial = dbObj.PersonnelInitial;
            this.Events = dbObj.Event;
        }

        public b_BBUKPIEventLog ToExtendedDatabaseObject()
        {
            b_BBUKPIEventLog dbObj = this.ToDatabaseObject();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.ObjectId = this.ObjectId;
            return dbObj;
        }

        public List<BBUKPIEventLog> RetriveByBBUKPIId(DatabaseKey dbKey)
        {
            BBUKPIEventLog_RetrieveByBBUKPIId trans = new BBUKPIEventLog_RetrieveByBBUKPIId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.BBUKPIEventLog = ToExtendedDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.BBUKPIEventLogList);
        }
    }
}
