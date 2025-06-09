using Database;
using Database.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    public partial class ApprovalRoute : DataContractBase
    {
        public long PurchaseRequestId { get; set; }
        public long WorkOrderId { get; set; }
        public long SiteId { get; set; }
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int Offset { get; set; }
        public int Nextrow { get; set; }
        public string ClientLookupId { get; set; }
        public string VendorName { get; set; }
        public string ChargeTo { get; set; }
        public string ChargeToName { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public DateTime? Date { get; set; }
        public int TotalCount { get; set; }
        public string FilterTypeCase { get; set; }
        public bool IsFinalApprover { get; set; }
        #region Material request v2 769
        public long EstimatedCostsId { get; set; }
        public long MaterialRequestId { get; set; }
        public decimal UnitCost { get; set; }
        public decimal Quantity { get; set; }
        public decimal TotalCost { get; set; }
        #endregion Material request
        #region PurchaseRequest

        public static List<ApprovalRoute> ApprovalRoute_RetrieveForPurchaseRequest_V2(DatabaseKey dbKey, ApprovalRoute approveroute)
        {
            ApprovalRoute_RetrieveForPurchaseRequest_V2 trans = new ApprovalRoute_RetrieveForPurchaseRequest_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.ApprovalRoute = approveroute.ToDateBaseObjectRetrive_V2();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return ApprovalRouteUpdateFromDatabaseObjectListByPR(trans.ApprovalRouteList);
        }
        public b_ApprovalRoute ToDateBaseObjectRetrive_V2()
        {
            b_ApprovalRoute dbObj = this.ToDatabaseObject();
            dbObj.FilterTypeCase = this.FilterTypeCase;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.Offset = this.Offset;
            dbObj.Nextrow = this.Nextrow;
            return dbObj;
        }

        public static List<ApprovalRoute> ApprovalRouteUpdateFromDatabaseObjectListByPR(List<b_ApprovalRoute> dbObjs)
        {
            List<ApprovalRoute> result = new List<ApprovalRoute>();

            foreach (b_ApprovalRoute dbObj in dbObjs)
            {
                ApprovalRoute tmp = new ApprovalRoute()
                {
                    ApprovalRouteId = dbObj.ApprovalRouteId,
                    ClientId = dbObj.ClientId,
                    PurchaseRequestId = dbObj.PurchaseRequestId,
                    ClientLookupId = dbObj.ClientLookupId,
                    VendorName = dbObj.VendorName,
                    Date = dbObj.Date,
                    Comments = dbObj.Comments,
                    ApprovalGroupId=dbObj.ApprovalGroupId,
                    TotalCount = dbObj.TotalCount,

                };
                result.Add(tmp);
            }
            return result;
        }
        
        #endregion
        #region WorkRequest

        public static List<ApprovalRoute> ApprovalRoute_RetrieveForWorkRequest_V2(DatabaseKey dbKey, ApprovalRoute approveroute)
        {
            ApprovalRoute_RetrieveForWorkRequest_V2 trans = new ApprovalRoute_RetrieveForWorkRequest_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.ApprovalRoute = approveroute.ToDateBaseObjectRetrive_V2();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return ApprovalRouteUpdateFromDatabaseObjectListByWR(trans.ApprovalRouteList);
        }

        public static List<ApprovalRoute> ApprovalRouteUpdateFromDatabaseObjectListByWR(List<b_ApprovalRoute> dbObjs)
        {
            List<ApprovalRoute> result = new List<ApprovalRoute>();

            foreach (b_ApprovalRoute dbObj in dbObjs)
            {
                ApprovalRoute tmp = new ApprovalRoute()
                {
                    ApprovalRouteId = dbObj.ApprovalRouteId,
                    WorkOrderId = dbObj.WorkOrderId,
                    ClientLookupId = dbObj.ClientLookupId,
                    ChargeTo = dbObj.ChargeTo,
                    ChargeToName = dbObj.ChargeToName,
                    Description = dbObj.Description,
                    Type = dbObj.Type,
                    Date = dbObj.Date,
                    Comments = dbObj.Comments,
                    ApprovalGroupId = dbObj.ApprovalGroupId,
                    TotalCount = dbObj.TotalCount,

                };
                result.Add(tmp);
            }
            return result;
        }
        #endregion

        #region V2-730
        public static List<ApprovalRoute> ApprovalRoute_RetrievebyObjectId_V2(DatabaseKey dbKey, ApprovalRoute approveroute)
        {
            ApprovalRoute_RetrievebyObjectId_V2 trans = new ApprovalRoute_RetrievebyObjectId_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.ApprovalRoute = approveroute.ToDateBaseObjectbyObjectId_V2();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return ApprovalRoute.ApprovalRouteUpdateFromDatabasebyObjectIdList(trans.ApprovalRouteList);
        }
        public b_ApprovalRoute ToDateBaseObjectbyObjectId_V2()
        {
            b_ApprovalRoute dbObj = new b_ApprovalRoute();
            dbObj.ClientId = this.ClientId;
            dbObj.ApproverId = this.ApproverId;
            dbObj.ObjectId = this.ObjectId;
            dbObj.RequestType = this.RequestType;
            return dbObj;
        }

        public static List<ApprovalRoute> ApprovalRouteUpdateFromDatabasebyObjectIdList(List<b_ApprovalRoute> dbObjs)
        {
            List<ApprovalRoute> result = new List<ApprovalRoute>();

            foreach (b_ApprovalRoute dbObj in dbObjs)
            {
                ApprovalRoute tmp = new ApprovalRoute();
                tmp.UpdateFromDatabasebyObjectId_V2(dbObj);
                result.Add(tmp);
            }
            return result;
        }
        public void UpdateFromDatabasebyObjectId_V2(b_ApprovalRoute dbObj)
        {
            this.ApprovalRouteId = dbObj.ApprovalRouteId;
            this.ApprovalGroupId = dbObj.ApprovalGroupId;
        }

        public void UpdateByObjectId_V2(DatabaseKey dbKey)
        {
            ApprovalRoute_UpdateByForeignKeys_V2 trans = new ApprovalRoute_UpdateByForeignKeys_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.ApprovalRoute = this.ToDatabaseObject();

            trans.ApprovalRoute.ClientId = this.ClientId;
            trans.ApprovalRoute.ObjectId = this.ObjectId;
            trans.ApprovalRoute.ApprovalGroupId = this.ApprovalGroupId;
            trans.ApprovalRoute.ProcessResponse = this.ProcessResponse;
            trans.ApprovalRoute.ApproverId = this.ApproverId;

            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.ApprovalRoute);

        }


        #endregion
        #region Material Request
        public List<ApprovalRoute> ApprovalRoute_RetrieveForMaterialRequest(DatabaseKey dbKey, ApprovalRoute approveroute)
        {
            ApprovalRoute_RetrieveForMaterialRequest_V2 trans = new ApprovalRoute_RetrieveForMaterialRequest_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.ApprovalRoute = approveroute.ToDateBaseObjectForRetriveForMaterialRequest_V2();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return ApprovalRoute.ApprovalRouteUpdateFromDatabaseObjectListForMaterialRequest(trans.ApprovalRouteListForMaterialRequest);
        }
        public b_ApprovalRoute ToDateBaseObjectForRetriveForMaterialRequest_V2()
        {
            b_ApprovalRoute dbObj = this.ToDatabaseObject();
            dbObj.SiteId = this.SiteId;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.Offset = this.Offset;
            dbObj.Nextrow = this.Nextrow;
            dbObj.FilterTypeCase = this.FilterTypeCase;
            return dbObj;
        }

        public static List<ApprovalRoute> ApprovalRouteUpdateFromDatabaseObjectListForMaterialRequest(List<b_ApprovalRoute> dbObjs)
        {
            List<ApprovalRoute> result = new List<ApprovalRoute>();

            foreach (b_ApprovalRoute dbObj in dbObjs)
            {
                ApprovalRoute tmp = new ApprovalRoute();
                tmp.UpdateFromDatabaseObjectForMaterialRequest_V2(dbObj);
                result.Add(tmp);
            }
            return result;
        }
        public void UpdateFromDatabaseObjectForMaterialRequest_V2(b_ApprovalRoute dbObj)
        {
            this.ApprovalRouteId = dbObj.ApprovalRouteId;
            this.EstimatedCostsId = dbObj.EstimatedCostsId;
            this.MaterialRequestId = dbObj.MaterialRequestId;

            this.ClientId = dbObj.ClientId;
            this.ClientLookupId = dbObj.ClientLookupId;
            this.Description = dbObj.Description;
            this.UnitCost = dbObj.UnitCost;
            this.Quantity = dbObj.Quantity;
            this.TotalCost = dbObj.TotalCost;
            this.Date = dbObj.Date;
            this.Comments = dbObj.Comments;
            this.ApprovalGroupId = dbObj.ApprovalGroupId;
            this.TotalCount = dbObj.TotalCount;
            // Turn on auditing
            AuditEnabled = true;
        }
        #endregion
    }
}
