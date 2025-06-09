using Database.Business;
using Database;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    public partial class VendorAssetMgt
    {
        #region Property
        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public string offset1 { get; set; }
        public string nextrow { get; set; }
        public int TotalCount { get; set; }
        public long AssetMgtSource { get; set; }
        public bool AssetMgtRequired { get; set; }
        public DateTime? AssetMgtExpireDate { get; set; }
        public bool AssetMgtOverride { get; set; }
        public DateTime? AssetMgtOverrideDate { get; set; }
        #endregion

        public List<VendorAssetMgt> RetrieveChunkSearchByVendorId(DatabaseKey dbKey)
        {
            VendorAssetMgt_RetrieveChunkSearchByVendorId trans = new VendorAssetMgt_RetrieveChunkSearchByVendorId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.VendorAssetMgt = this.ToDateBaseObjectForRetrieveChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<VendorAssetMgt> VendorAssetMgtlist = new List<VendorAssetMgt>();

            foreach (b_VendorAssetMgt VendorAssetMgt in trans.VendorAssetMgtList)
            {
                VendorAssetMgt tmpVendorAssetMgt = new VendorAssetMgt();
                tmpVendorAssetMgt.UpdateFromDatabaseObjectForRetriveAllForSearch(VendorAssetMgt);
                VendorAssetMgtlist.Add(tmpVendorAssetMgt);
            }
            return VendorAssetMgtlist;
        }

        public b_VendorAssetMgt ToDateBaseObjectForRetrieveChunkSearch()
        {
            b_VendorAssetMgt dbObj = new b_VendorAssetMgt();
            dbObj.ClientId = this.ClientId;
            dbObj.VendorId = this.VendorId;

            dbObj.orderbyColumn = this.orderbyColumn;
            dbObj.orderBy = this.orderBy;
            dbObj.offset1 = this.offset1;
            dbObj.nextrow = this.nextrow;
            return dbObj;
        }

        public void UpdateFromDatabaseObjectForRetriveAllForSearch(b_VendorAssetMgt dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.VendorAssetMgtId = dbObj.VendorAssetMgtId;
            this.Company = dbObj.Company;
            this.Contact = dbObj.Contact;
            this.Contract = dbObj.Contract;
            this.ExpireDate = dbObj.ExpireDate;
            this.AssetMgtSource= dbObj.AssetMgtSource;
            this.TotalCount = dbObj.TotalCount;
        }

        #region Header
        public VendorAssetMgt RetrieveAssetMgtHederByVendorId(DatabaseKey dbKey)
        {
            VendorAssetMgt_RetrieveHeaderByVendorId trans = new VendorAssetMgt_RetrieveHeaderByVendorId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.VendorAssetMgt = this.ToDateBaseObjectForRetrieveHeader();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            VendorAssetMgt VendorAssetMgtlist = new VendorAssetMgt();

            foreach (b_VendorAssetMgt VendorAssetMgt in trans.VendorAssetMgtList)
            {
                VendorAssetMgtlist.UpdateFromDatabaseObjectForRetriveHeader(VendorAssetMgt);
            }
            return VendorAssetMgtlist;
        }
        public b_VendorAssetMgt ToDateBaseObjectForRetrieveHeader()
        {
            b_VendorAssetMgt dbObj = new b_VendorAssetMgt();
            dbObj.ClientId = this.ClientId;
            dbObj.VendorId = this.VendorId;
            return dbObj;
        }
        public void UpdateFromDatabaseObjectForRetriveHeader(b_VendorAssetMgt dbObj)
        {
            this.VendorId = dbObj.VendorId;
            this.AssetMgtRequired = dbObj.AssetMgtRequired;
            this.AssetMgtExpireDate = dbObj.AssetMgtExpireDate;
            this.AssetMgtOverride = dbObj.AssetMgtOverride;
            this.AssetMgtOverrideDate = dbObj.AssetMgtOverrideDate;
            this.Company = dbObj.Company;
            this.Contact = dbObj.Contact;
            this.Contract = dbObj.Contract;
            this.VendorAssetMgtId = dbObj.VendorAssetMgtId;
        }
        #endregion
    }
}
