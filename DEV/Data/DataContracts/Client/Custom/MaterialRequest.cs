using Database.Client.Custom.Transactions;

using System;
using System.Collections.Generic;
using Database.Business;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    public partial class MaterialRequest
    {
        #region Property
        public DateTime? CreateDate { get; set; }
        public string AdvRequiredDate { get; set; }
        public string AccountClientLookupId { get; set; }
        public string AdvCreateDate { get; set; }
        public string AdvCompleteDate { get; set; }
        public int CustomQueryDisplayId { get; set; }
        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public string offset1 { get; set; }
        public string nextrow { get; set; }
        public string SearchText { get; set; }
        public int TotalCount { get; set; }
        public Int32 ChildCount { get; set; }
        public string Personnel_NameFirst { get; set; }
        public string Personnel_NameLast { get; set; }
        #endregion

        public List<MaterialRequest> RetrieveChunkSearch(DatabaseKey dbKey)
        {
            MaterialRequest_RetrieveChunkSearch trans = new MaterialRequest_RetrieveChunkSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.MaterialRequest = this.ToDateBaseObjectForRetrieveChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<MaterialRequest> MaterialRequestlist = new List<MaterialRequest>();

            foreach (b_MaterialRequest MaterialRequest in trans.MaterialRequestList)
            {
                MaterialRequest tmpmaterialRequest = new MaterialRequest();
                tmpmaterialRequest.UpdateFromDatabaseObjectForRetriveAllForSearch(MaterialRequest);
                MaterialRequestlist.Add(tmpmaterialRequest);
            }
            return MaterialRequestlist;
        }

        public b_MaterialRequest ToDateBaseObjectForRetrieveChunkSearch()
        {
            b_MaterialRequest dbObj = new b_MaterialRequest();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId= this.SiteId;
            //dbObj.SiteId= 0;
            dbObj.MaterialRequestId = this.MaterialRequestId;

            dbObj.orderbyColumn = this.orderbyColumn;
            dbObj.orderBy = this.orderBy;
            dbObj.CustomQueryDisplayId = this.CustomQueryDisplayId;
            dbObj.offset1 = this.offset1;
            dbObj.nextrow = this.nextrow;
            dbObj.AccountClientLookupId = this.AccountClientLookupId;
            dbObj.Description = this.Description;
            dbObj.Status = this.Status;
            dbObj.RequiredDate = this.RequiredDate;
            dbObj.CreateDate = this.CreateDate;
            dbObj.CompleteDate = this.CompleteDate;
            dbObj.SearchText = this.SearchText;
            return dbObj;
        }

        public void UpdateFromDatabaseObjectForRetriveAllForSearch(b_MaterialRequest dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.MaterialRequestId = dbObj.MaterialRequestId;
            this.CustomQueryDisplayId = dbObj.CustomQueryDisplayId;
            this.AccountClientLookupId = dbObj.AccountClientLookupId;
            this.Description = dbObj.Description;
            this.Status = dbObj.Status;
            this.RequiredDate= dbObj.RequiredDate;
            this.CreateDate = dbObj.CreateDate;
            this.CompleteDate = dbObj.CompleteDate;
            this.orderbyColumn = dbObj.orderbyColumn;
            this.orderBy = dbObj.orderBy;
            this.offset1 = dbObj.offset1;
            this.nextrow = dbObj.nextrow;
            this.SearchText = dbObj.SearchText;
            this.ChildCount = dbObj.ChildCount;
            this.TotalCount = dbObj.TotalCount;
        }


        public void RetriveByMaterialRequestId(DatabaseKey dbKey)
        {
            MaterialRequest_RetrieveByMaterialRequestId trans = new MaterialRequest_RetrieveByMaterialRequestId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.MaterialRequest = this.ToDatabaseObjectRetriveByMaterialRequestId();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObjectExtendedByPartid_V2(trans.MaterialRequest);
        }
        
        public b_MaterialRequest ToDatabaseObjectRetriveByMaterialRequestId()
        {
            b_MaterialRequest dbObj = new b_MaterialRequest();
            dbObj.ClientId = this.ClientId;
            dbObj.MaterialRequestId = this.MaterialRequestId;
            return dbObj;
        }      
        public void UpdateFromDatabaseObjectExtendedByPartid_V2(b_MaterialRequest dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.MaterialRequestId = dbObj.MaterialRequestId;
            this.SiteId = dbObj.SiteId;
            this.AreaId = dbObj.AreaId;
            this.DepartmentId = dbObj.DepartmentId;
            this.StoreroomId = dbObj.StoreroomId;
            this.AccountId= dbObj.AccountId;
            this.Status = dbObj.Status;
            this.Personnel_NameFirst = dbObj.Personnel_NameFirst;
            this.Personnel_NameLast = dbObj.Personnel_NameLast;
            this.CreateDate = dbObj.CreateDate;
            this.RequiredDate = dbObj.RequiredDate;
            this.CompleteDate = dbObj.CompleteDate;
            this.AccountClientLookupId = dbObj.AccountClientLookupId;
            this.Description = dbObj.Description;
            this.Requestor_PersonnelId= dbObj.Requestor_PersonnelId;
        }
    }
}
