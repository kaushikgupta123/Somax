using Database;
using Database.Business;
using System;
using System.Collections.Generic;

namespace DataContracts
{
    public partial class VendorCatalogItem : DataContractBase
    {
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int offset1 { get; set; }
        public int nextrow { get; set; }
        public int TotalCount { get; set; }
        public string VendorName { get; set; }
        public static List<VendorCatalogItem> RetrieveByPartMasterId_V2(DatabaseKey dbKey, VendorCatalogItem down)
        {
            VendorCatalogItem_RetrieveByPartMasterId_V2 trans = new VendorCatalogItem_RetrieveByPartMasterId_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.VendorCatalogItem = down.ToDateBaseObjectForRetrieveByPartMasterId_V2();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return VendorCatalogItem.VendorCatalogItemUpdateFromDatabaseObjectList(trans.VendorCatalogItemList);
        }
        public static List<VendorCatalogItem> VendorCatalogItemUpdateFromDatabaseObjectList(List<b_VendorCatalogItem> dbObjs)
        {
            List<VendorCatalogItem> result = new List<VendorCatalogItem>();

            foreach (b_VendorCatalogItem dbObj in dbObjs)
            {
                VendorCatalogItem tmp = new VendorCatalogItem();
                tmp.VendorCatalogItemUpdateFromExtendedDatabaseObject(dbObj);
                result.Add(tmp);
            }
            return result;
        }

        public void VendorCatalogItemUpdateFromExtendedDatabaseObject(b_VendorCatalogItem dbObj)
        {
            //this.UpdateFromDatabaseObject(dbObj);
            this.ClientId = dbObj.ClientId;
            this.VendorCatalogItemId = dbObj.VendorCatalogItemId;
            this.VendorName = dbObj.VendorName;
            this.UnitCost = dbObj.UnitCost;
            this.PurchaseUOM = dbObj.PurchaseUOM;
            this.TotalCount = dbObj.TotalCount;
        }

        public b_VendorCatalogItem ToDateBaseObjectForRetrieveByPartMasterId_V2()
        {
            b_VendorCatalogItem dbObj = this.ToDatabaseObject();
            dbObj.ClientId = this.ClientId;
            dbObj.PartMasterId = this.PartMasterId;
            dbObj.orderbyColumn = this.OrderbyColumn;
            dbObj.orderBy = this.OrderBy;
            dbObj.offset1 = this.offset1;
            dbObj.nextrow = this.nextrow;
            return dbObj;
        }
    }
}
