using Data.Database;

using Database.Business;
using Database.StoredProcedure;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    public partial class ClientUserInfoList : DataContractBase
    {
        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public Int32 offset1 { get; set; }
        public Int32 nextrow { get; set; }
        public int totalCount { get; set; }
        public List<ClientUserInfoList> RetrieveChunkSearchLookupList(DatabaseKey databaseKey)
        {
            ClientUserInfoList_RetrieveChunkSearchLookupList trans = new ClientUserInfoList_RetrieveChunkSearchLookupList()
            {
                CallerUserInfoId = databaseKey.User.UserInfoId,
                CallerUserName = databaseKey.UserName
            };

            trans.dbKey = databaseKey.ToTransDbKey();
            trans.ClientUserInfoList = this.ToDatabaseRetrieveChunkSearchlookupListObject();
            trans.Execute();
            return UpdateFromDatabaseObjectLookupList(trans.clientUserInfoList);
        }
        public static List<ClientUserInfoList> UpdateFromDatabaseObjectLookupList(List<b_ClientUserInfoList> dbObjs)
        {
            List<ClientUserInfoList> result = new List<ClientUserInfoList>();

            foreach (b_ClientUserInfoList dbObj in dbObjs)
            {
                ClientUserInfoList tmp = new ClientUserInfoList();
                tmp.UpdateFromDatabaseRetrieveChunkSearchObject(dbObj);
                result.Add(tmp);
            }
            return result;
        }
        public void UpdateFromDatabaseRetrieveChunkSearchObject(b_ClientUserInfoList dbObj)
        {
            this.ClientUserInfoListID = dbObj.ClientUserInfoListID;
            this.SiteName = dbObj.SiteName;
            this.ClientName = dbObj.ClientName;
            this.ClientId = dbObj.ClientId;
            this.DefaultSiteId = dbObj.DefaultSiteId;
            this.totalCount = dbObj.totalCount;
        }
        public b_ClientUserInfoList ToDatabaseRetrieveChunkSearchlookupListObject()
        {
            b_ClientUserInfoList dbObj = new b_ClientUserInfoList();
            dbObj.ClientName = this.ClientName;
            dbObj.SiteName = this.SiteName;
            dbObj.UserInfoId = this.UserInfoId;
            dbObj.orderbyColumn = this.orderbyColumn;
            dbObj.orderBy = this.orderBy;
            dbObj.offset1 = this.offset1;
            dbObj.nextrow = this.nextrow;
            return dbObj;
        }
    }
}
