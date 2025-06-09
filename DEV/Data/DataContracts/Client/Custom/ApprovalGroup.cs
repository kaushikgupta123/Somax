using Database;
using Database.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
   
        public partial class ApprovalGroup : DataContractBase
        {
        #region property
        public List<ApprovalGroup> listOfApprovalGroup { get; set; }
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int OffSetVal { get; set; }
        public int NextRow { get; set; }
        public string AssetGroup1ClientLookupId { get; set; }
        public string AssetGroup2ClientLookupId { get; set; }
        public string AssetGroup3ClientLookupId { get; set; }
        public long AssetGroup1Id { get; set; }
        public long AssetGroup2Id { get; set; }
        public long AssetGroup3Id { get; set; }
        public int TotalCount { get; set; }
        public int ChildCount { get; set; }
        public string SearchText { get; set; }
        #endregion
        public ApprovalGroup ApprovalGroupRetrieveChunkSearchV2(DatabaseKey dbKey, string TimeZone)
        {
            ApprovalGroup_RetrieveChunkSearchV2 trans = new ApprovalGroup_RetrieveChunkSearchV2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.ApprovalGroup = this.ToDateBaseObjectForChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            this.listOfApprovalGroup = new List<ApprovalGroup>();


            List<ApprovalGroup> ApprovalGrouplist = new List<ApprovalGroup>();
            // Moved this to UpdateFromDatabaseObjectForRetriveV2
            foreach (b_ApprovalGroup ApprovalGroup in trans.ApprovalGroup.listOfApprovalGroup)
            {
                ApprovalGroup tmpApprovalGroup = new ApprovalGroup();

                tmpApprovalGroup.UpdateFromDatabaseObjectForChunkSearch(ApprovalGroup);
                ApprovalGrouplist.Add(tmpApprovalGroup);
            }
            this.listOfApprovalGroup.AddRange(ApprovalGrouplist);
            return this;
        }
        public b_ApprovalGroup ToDateBaseObjectForChunkSearch()
        {
            b_ApprovalGroup dbObj = new b_ApprovalGroup();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;
            dbObj.ApprovalGroupId=this.ApprovalGroupId;
            dbObj.RequestType=this.RequestType;
            dbObj.Description=this.Description;
            //dbObj.AssetGroup1ClientLookupId = this.AssetGroup1ClientLookupId;
            //dbObj.AssetGroup2ClientLookupId = this.AssetGroup2ClientLookupId;
            //dbObj.AssetGroup3ClientLookupId = this.AssetGroup3ClientLookupId;
            dbObj.AssetGroup1Id = this.AssetGroup1Id;
            dbObj.AssetGroup2Id = this.AssetGroup2Id;
            dbObj.AssetGroup3Id = this.AssetGroup3Id;
            dbObj.SearchText = this.SearchText;
            return dbObj;
        }

        public void UpdateFromDatabaseObjectForChunkSearch(b_ApprovalGroup dbObj)
        {
            this.ClientId=dbObj.ClientId;
            this.ApprovalGroupId=dbObj.ApprovalGroupId;
            this.RequestType=dbObj.RequestType;
            this.Description=dbObj.Description;
            this.AssetGroup1=dbObj.AssetGroup1;
            this.AssetGroup2=dbObj.AssetGroup2;
            this.AssetGroup3=dbObj.AssetGroup3;
            this.AssetGroup1ClientLookupId = dbObj.AssetGroup1ClientLookupId;
            this.AssetGroup2ClientLookupId = dbObj.AssetGroup2ClientLookupId;
            this.AssetGroup3ClientLookupId = dbObj.AssetGroup3ClientLookupId;
            this.AssetGroup1Id=dbObj.AssetGroup1Id;
            this.AssetGroup2Id=dbObj.AssetGroup2Id;
            this.AssetGroup3Id=dbObj.AssetGroup3Id;
            this.ChildCount=dbObj.ChildCount;
            this.TotalCount = dbObj.TotalCount;
        }

        public void RetrieveById_V2(DatabaseKey dbKey)
        {
            ApprovalGroup_RetrieveById_V2 trans = new ApprovalGroup_RetrieveById_V2();
            trans.ApprovalGroup = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObjectById(trans.ApprovalGroup);
        }
        public void UpdateFromDatabaseObjectById(b_ApprovalGroup dbObj)
        {
            UpdateFromDatabaseObject(dbObj);
            this.AssetGroup1ClientLookupId = dbObj.AssetGroup1ClientLookupId;
            this.AssetGroup2ClientLookupId = dbObj.AssetGroup2ClientLookupId;
            this.AssetGroup3ClientLookupId = dbObj.AssetGroup3ClientLookupId;

        }
    }
}
