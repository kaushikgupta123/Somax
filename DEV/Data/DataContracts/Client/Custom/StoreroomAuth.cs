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
    public partial class StoreroomAuth : DataContractBase, IStoredProcedureValidation
    {
        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public Int32 offset1 { get; set; }
        public Int32 nextrow { get; set; }
        public string SiteName { get; set; }
        public string StoreroomName { get; set; }
        public int totalCount { get; set; }
        public string ValidateFor = string.Empty;

        public List<StoreroomAuth> RetrieveUserStoreroomDetailsByClientId(DatabaseKey dbKey)
        {
            StoreroomAuth_RetrieveChunkSearch trans = new StoreroomAuth_RetrieveChunkSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.StoreroomAuth = this.ToDatabaseRetrieveChunkSearchObject();
        
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<StoreroomAuth> StoreroomAuthlist = new List<StoreroomAuth>();

            foreach (b_StoreroomAuth v in trans.StoreroomAuthList)
            {
                StoreroomAuth tmpStoreroomAuth = new StoreroomAuth();
                tmpStoreroomAuth.UpdateFromDatabaseRetrieveChunkSearchObject(v);
              
                StoreroomAuthlist.Add(tmpStoreroomAuth);
            }
            return StoreroomAuthlist;

        }

        public b_StoreroomAuth ToDatabaseRetrieveChunkSearchObject()
        {
            b_StoreroomAuth dbObj = new b_StoreroomAuth();
            dbObj.ClientId = this.ClientId;
            dbObj.PersonnelId = this.PersonnelId;
            dbObj.orderbyColumn = this.orderbyColumn;
            dbObj.orderBy = this.orderBy;
            dbObj.offset1 = this.offset1;
            dbObj.nextrow = this.nextrow;
            return dbObj;
        }

        public void UpdateFromDatabaseRetrieveChunkSearchObject(b_StoreroomAuth dbObj)
        {
            this.StoreroomAuthId = dbObj.StoreroomAuthId;
            this.SiteName = dbObj.SiteName;
            this.StoreroomName = dbObj.StoreroomName;
            this.totalCount = dbObj.totalCount;
        }
        public List<StoreroomAuth> RetrieveAllStoreroomBySiteId(DatabaseKey databaseKey)
        {
            StoreroomAuth_RetrieveAllStoreroomBySiteId trans = new StoreroomAuth_RetrieveAllStoreroomBySiteId()
            {
                CallerUserInfoId = databaseKey.User.UserInfoId,
                CallerUserName = databaseKey.UserName
            };

            trans.dbKey = databaseKey.ToTransDbKey();
            trans.StoreroomAuth = this.ToDatabaseObject();
            
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.StoreroomList);
        }
        public static List<StoreroomAuth> UpdateFromDatabaseObjectList(List<b_StoreroomAuth> dbObjs)
        {
            List<StoreroomAuth> result = new List<StoreroomAuth>();

            foreach (b_StoreroomAuth dbObj in dbObjs)
            {
                StoreroomAuth tmp = new StoreroomAuth();
                tmp.UpdateFromDatabaseObject(dbObj);
                tmp.StoreroomId = dbObj.StoreroomId;
                tmp.StoreroomName = dbObj.StoreroomName;
                result.Add(tmp);
            }
            return result;
        }
        
        public void RetrieveByStoreroomAuthId(DatabaseKey dbKey)
        {
            StoreroomAuth_RetrieveByStoreroomAuthId_V2 trans = new StoreroomAuth_RetrieveByStoreroomAuthId_V2();
            trans.StoreroomAuth = this.ToRetrieveByStoreroomAuthIdDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateRetrieveByStoreroomAuthIdFromDatabaseObject(trans.StoreroomAuth);
        }
        public b_StoreroomAuth ToRetrieveByStoreroomAuthIdDatabaseObject()
        {
            b_StoreroomAuth dbObj = new b_StoreroomAuth();
            dbObj.StoreroomAuthId = this.StoreroomAuthId;
            dbObj.ClientId = this.ClientId;
            return dbObj;
        }
        public void UpdateRetrieveByStoreroomAuthIdFromDatabaseObject(b_StoreroomAuth dbObj)
        {
            this.StoreroomAuthId = dbObj.StoreroomAuthId;
            this.ClientId = dbObj.ClientId;
            this.SiteId = dbObj.SiteId;
            this.PersonnelId = dbObj.PersonnelId;
            this.StoreroomId = dbObj.StoreroomId;
            this.Maintain = dbObj.Maintain;
            this.Issue = dbObj.Issue;
            this.IssueTransfer = dbObj.IssueTransfer;
            this.ReceiveTransfer = dbObj.ReceiveTransfer;
            this.PhysicalInventory = dbObj.PhysicalInventory;
            this.Purchase = dbObj.Purchase;
            this.ReceivePurchase = dbObj.ReceivePurchase;
            this.SiteName = dbObj.SiteName;
            this.StoreroomName = dbObj.StoreroomName;
        }

        public void CheckDuplicateSite(DatabaseKey dbKey)
        {          
            Validate<StoreroomAuth>(dbKey);
        }

        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
           List<StoredProcValidationError> errors = new List<StoredProcValidationError>();            
                StoreroomAuth_ValidateSiteId trans = new StoreroomAuth_ValidateSiteId()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.StoreroomAuth = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                errors = StoredProcValidationError.UpdateFromDatabaseObjects(trans.StoredProcValidationErrorList);        
            return errors;
        }

}
}
