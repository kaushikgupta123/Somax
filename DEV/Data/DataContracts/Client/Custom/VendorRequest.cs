
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Reflection;
using System.Text;
using System.Data;

using Database;
using Database.Business;
using Newtonsoft.Json;
using Common.Structures;

namespace DataContracts
{
    [JsonObject]
    public partial class VendorRequest : DataContractBase
    {
        #region  Properties
        public int CustomQueryDisplayId { get; set; }

        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public Int32 offset1 { get; set; }
        public Int32 nextrow { get; set; }
        public string SearchText { get; set; }
        public int totalCount { get; set; }

        #endregion


        #region Vendor Request Chunk Search
        public List<VendorRequest> VendorRequestChunkSearch(DatabaseKey dbKey, VendorRequest VendorRequest)
        {
            VendorRequest_ChunkSearchV2 trans = new VendorRequest_ChunkSearchV2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.VendorRequest = VendorRequest.ToDateBaseObjectForRetrive_V2();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return VendorRequest.UpdateFromDatabaseObjectList(trans.RetVendorRequestList);
        }
        public b_VendorRequest ToDateBaseObjectForRetrive_V2()
        {
            b_VendorRequest dbObj = this.ToDatabaseObject();
            dbObj.SiteId = this.SiteId;
            dbObj.ClientId = this.ClientId;
            dbObj.orderbyColumn = this.orderbyColumn;
            dbObj.orderBy = this.orderBy;
            dbObj.offset1 = this.offset1;
            dbObj.nextrow = this.nextrow;
            dbObj.SearchText = this.SearchText;
            dbObj.CustomQueryDisplayId = this.CustomQueryDisplayId;
            return dbObj;
        }

        public static List<VendorRequest> UpdateFromDatabaseObjectList(List<b_VendorRequest> dbObjs)
        {
            List<VendorRequest> result = new List<VendorRequest>();

            foreach (b_VendorRequest dbObj in dbObjs)
            {
                VendorRequest tmp = new VendorRequest();
                tmp.UpdateFromDatabaseObjectForMaterialRequest_V2(dbObj);
                result.Add(tmp);
            }
            return result;
        }
        public void UpdateFromDatabaseObjectForMaterialRequest_V2(b_VendorRequest dbObj)
        {
            this.VendorRequestId = dbObj.VendorRequestId;
            this.Name = dbObj.Name;
            this.AddressCity = dbObj.AddressCity;
            this.AddressState = dbObj.AddressState;
            this.Type = dbObj.Type;
            this.Status = dbObj.Status;
            this.totalCount = dbObj.totalCount;
            // Turn on auditing
            AuditEnabled = true;
        }
        #endregion
    }

}
