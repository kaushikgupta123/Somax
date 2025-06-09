using Database.Business;
using Database.Client.Custom.Transactions;
using Database.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    public partial class ClientEventLog: DataContractBase
    {
        #region Properties
        public string order { get; set; }
        public string orderdir { get; set; }
        public int offset { get; set; }
        public int next { get; set; }
        public string SiteName { get; set; }
        public int TotalCount { get; set; }
        #endregion

        private List<ClientEventLog> UpdateFromDatabaseObjectList(List<b_ClientEventLog> dbObjs)
        {
            List<ClientEventLog> result = new List<ClientEventLog>();

            foreach (b_ClientEventLog dbObj in dbObjs)
            {
                ClientEventLog tmp = new ClientEventLog();
                tmp.UpdateFromExtendedDatabaseObject(dbObj);
                result.Add(tmp);
            }
            return result;
        }

        public void UpdateFromExtendedDatabaseObject(b_ClientEventLog dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.SiteName = dbObj.SiteName;
            this.TotalCount = dbObj.TotalCount;
        }

        public b_ClientEventLog ToExtendedDatabaseObject()
        {
            b_ClientEventLog dbObj = new b_ClientEventLog();
            dbObj.ClientId = this.ClientId;
            dbObj.OrderBy = this.order;
            dbObj.OrderbyColumn=this.orderdir;
            dbObj.OffSetVal = this.offset;
            dbObj.NextRow = this.next;
            return dbObj;
        }

        public List<ClientEventLog> RetriveByClientId(DatabaseKey dbKey)
        {
            ClientEventLog_ReteriveByClientEventLog trans = new ClientEventLog_ReteriveByClientEventLog()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.ClientEventLog = ToExtendedDatabaseObject();
            trans.SearchClientId = this.ClientId;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.ClientEventLogList);
        }
        public void CreateFromAdmin(DatabaseKey dbKey)
        {
            ClientEventLog_CreateFromAdmin trans = new ClientEventLog_CreateFromAdmin();
            trans.ClientEventLog = this.ToDatabaseObject();
            trans.SearchClientId = this.ClientId;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.ClientEventLog);
        }
    }
}
