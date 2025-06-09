using Database.Business;
using Database.Client.Custom.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    public partial class ServiceOrder : DataContractBase, IStoredProcedureValidation
    {
        #region Properties
        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public string offset1 { get; set; }
        public string nextrow { get; set; }
        public string SearchText { get; set; }
        public int TotalCount { get; set; }
        public int CustomQueryDisplayId { get; set; }
        public string EquipmentClientLookupId { get; set; }
        public string AssetName { get; set; }
        public string VIN { get; set; }
        public string PersonnelList { get; set; }
        public UtilityAdd utilityAdd { get; set; }
        public DateTime CreateDate { get; set; }
        public string Assigned { get; set; }
        public int ChildCount { get; set; }
        public string Meter1Type { get; set; }
        public decimal Meter1CurrentReading { get; set; }
        public string Meter2Type { get; set; }
        public decimal Meter2CurrentReading { get; set; }
        public string CompletedByPersonnels { get; set; }
        public string CancelledByPersonnels { get; set; }
        //public string EquipmentName { get; set; }
        public decimal ScheduledHours { get; set; }
        public bool IsDeleteFlag { get; set; }
        public string CreateStartDateVw { get; set; }
        public string CreateEndDateVw { get; set; }
        public string CompleteStartDateVw { get; set; }
        public string CompleteEndDateVw { get; set; }
        #region Fleet Only
        public int ServiceOrderCount { get; set; }
        #endregion
        public DateTime? Meter1CurrentReadingDate { get; set; }
        public DateTime? Meter2CurrentReadingDate { get; set; }
        public string Meter1Units { get; set; }
        public string Meter2Units { get; set; }
        public decimal LaborTotal { get; set; }
        public decimal PartTotal { get; set; }
        public decimal OtherTotal { get; set; }
        public decimal GrandTotal { get; set; }
        public string ShiftDesc { get; set; }
        public string TypeDesc { get; set; }
        public string CreateBy { get; set; }
        #endregion
        public List<ServiceOrder> RetrieveChunkSearch(DatabaseKey dbKey)
        {
            ServiceOrder_RetrieveChunkSearch trans = new ServiceOrder_RetrieveChunkSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.ServiceOrder = this.ToDateBaseObjectForRetrieveChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();          

            List<ServiceOrder> ServiceOrderlist = new List<ServiceOrder>();

            foreach (b_ServiceOrder serviceorder in trans.ServiceOrderList)
            {
                ServiceOrder tmpServiceOrder = new ServiceOrder();
                tmpServiceOrder.UpdateFromDatabaseObjectForRetriveAllForSearch(serviceorder);
                ServiceOrderlist.Add(tmpServiceOrder);
            }
            return ServiceOrderlist;
        }
        public b_ServiceOrder ToDateBaseObjectForRetrieveChunkSearch()
        {
            b_ServiceOrder dbObj = this.ToDatabaseObject();
            dbObj.CustomQueryDisplayId = this.CustomQueryDisplayId;
            dbObj.orderbyColumn = this.orderbyColumn;
            dbObj.orderBy = this.orderBy;
            dbObj.CustomQueryDisplayId = this.CustomQueryDisplayId;
            dbObj.offset1 = this.offset1;
            dbObj.nextrow = this.nextrow;
            dbObj.ClientLookupId = this.ClientLookupId;
            dbObj.EquipmentClientLookupId = this.EquipmentClientLookupId;
            dbObj.AssetName = this.AssetName;
            dbObj.VIN = this.VIN;
            dbObj.Shift = this.Shift;
            dbObj.Description = this.Description;
            dbObj.Status = this.Status;
            dbObj.CreateDate = this.CreateDate;
            dbObj.Type = this.Type;
            dbObj.Assigned = this.Assigned;
            dbObj.Assign_PersonnelId = this.Assign_PersonnelId;
            dbObj.ScheduleDate = this.ScheduleDate;
            dbObj.CompleteDate = this.CompleteDate;
            dbObj.CreateStartDateVw = this.CreateStartDateVw;
            dbObj.CreateEndDateVw = this.CreateEndDateVw;
            dbObj.CompleteStartDateVw = this.CompleteStartDateVw;
            dbObj.CompleteEndDateVw = this.CompleteEndDateVw;
            dbObj.PersonnelList = this.PersonnelList;
            dbObj.SearchText = this.SearchText;


            return dbObj;
        }
        public void UpdateFromDatabaseObjectForRetriveAllForSearch(b_ServiceOrder dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.CustomQueryDisplayId = dbObj.CustomQueryDisplayId;
            this.CreateDate = dbObj.CreateDate;
            this.EquipmentClientLookupId = dbObj.EquipmentClientLookupId;
            this.AssetName = dbObj.AssetName;
            this.VIN = dbObj.VIN;
            this.Assigned = dbObj.Assigned;
            this.Assign_PersonnelId = dbObj.Assign_PersonnelId;
            this.orderbyColumn = dbObj.orderbyColumn;
            this.orderBy = dbObj.orderBy;
            this.offset1 = dbObj.offset1;
            this.nextrow = dbObj.nextrow;
            this.SearchText = dbObj.SearchText;
            this.ChildCount = dbObj.ChildCount;
            this.TotalCount = dbObj.TotalCount;

        }

        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            throw new NotImplementedException();
        }

        #region Details
        public ServiceOrder RetrieveByServiceOrderId(DatabaseKey dbKey)
        {
            ServiceOrder_RetrieveByServiceOrderId trans = new ServiceOrder_RetrieveByServiceOrderId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,

            };
            trans.ServiceOrder = this.ToDateBaseObjectForRetrieveByServiceOrderId();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            ServiceOrder objServiceOrder = new ServiceOrder();
            objServiceOrder.UpdateFromRetrieveByServiceOrderId(trans.objServiceOrder);
            return objServiceOrder;
        }
        public b_ServiceOrder ToDateBaseObjectForRetrieveByServiceOrderId()
        {
            b_ServiceOrder dbObj = this.ToDatabaseObject();
            dbObj.Status = this.Status;
            dbObj.EquipmentClientLookupId = this.EquipmentClientLookupId;
            dbObj.EquipmentId = this.EquipmentId;
            dbObj.AssetName = this.AssetName;
            dbObj.Meter1Type = this.Meter1Type;
            dbObj.Meter1CurrentReading = this.Meter1CurrentReading;
            dbObj.Meter2Type = this.Meter2Type;
            dbObj.Meter2CurrentReading = this.Meter2CurrentReading;
            dbObj.Assigned = this.Assigned;
            dbObj.CompletedByPersonnels = this.CompletedByPersonnels;
            dbObj.CancelledByPersonnels = this.CancelledByPersonnels;
            dbObj.Meter1CurrentReadingDate = this.Meter1CurrentReadingDate;
            dbObj.Meter2CurrentReadingDate = this.Meter2CurrentReadingDate;
            dbObj.Meter1Units = this.Meter1Units;
            dbObj.Meter2Units = this.Meter2Units;
            dbObj.LaborTotal = this.LaborTotal;
            dbObj.PartTotal = this.PartTotal;
            dbObj.OtherTotal = this.OtherTotal;
            dbObj.GrandTotal = this.GrandTotal;
            dbObj.ShiftDesc = this.ShiftDesc;
            dbObj.TypeDesc = this.TypeDesc;
            return dbObj;
        }
        public b_ServiceOrder ToDateBaseObjectForRetrievePersonnelInitial()
        {
            b_ServiceOrder dbObj = this.ToDatabaseObject();
            dbObj.Assigned = this.Assigned;
            return dbObj;
        }
        public void UpdateFromRetrieveByServiceOrderId(b_ServiceOrder dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.Status = dbObj.Status;
            this.EquipmentClientLookupId = dbObj.EquipmentClientLookupId;
            this.EquipmentId = dbObj.EquipmentId;
            this.AssetName = dbObj.AssetName;
            this.Meter1Type = dbObj.Meter1Type;
            this.Meter1CurrentReading = dbObj.Meter1CurrentReading;
            this.Meter2Type = dbObj.Meter2Type;
            this.Meter2CurrentReading = dbObj.Meter2CurrentReading;
            this.Assigned = dbObj.Assigned;
            this.CompletedByPersonnels = dbObj.CompletedByPersonnels;
            this.CancelledByPersonnels = dbObj.CancelledByPersonnels;
            this.Meter1CurrentReadingDate = dbObj.Meter1CurrentReadingDate;
            this.Meter2CurrentReadingDate = dbObj.Meter2CurrentReadingDate;
            this.Meter1Units = dbObj.Meter1Units;
            this.Meter2Units = dbObj.Meter2Units;
            this.LaborTotal = dbObj.LaborTotal;
            this.PartTotal = dbObj.PartTotal;
            this.OtherTotal = dbObj.OtherTotal;
            this.GrandTotal = dbObj.GrandTotal;
            this.ShiftDesc = dbObj.ShiftDesc;
            this.TypeDesc = dbObj.TypeDesc;
            //switch (this.Status)
            //{
            //    case Common.Constants.WorkOrderStatusConstants.Approved:
            //        this.Status_Display = "Approved";
            //        break;
            //    case Common.Constants.WorkOrderStatusConstants.AwaitingApproval:
            //        this.Status_Display = "AwaitingApproval";
            //        break;
            //    case Common.Constants.WorkOrderStatusConstants.Canceled:
            //        this.Status_Display = "Canceled";
            //        break;
            //    case Common.Constants.WorkOrderStatusConstants.Complete:
            //        this.Status_Display = "Complete";
            //        break;
            //    default:
            //        this.Status_Display = string.Empty;
            //        break;
            //}
        }


        public void UpdateFromRetrievePersonnelInitial(b_ServiceOrder dbObj)
        {
            
            this.Assigned = dbObj.Assigned;
            
        }
        public ServiceOrder RetrievePersonnelInitial(DatabaseKey dbKey)
        {
            ServiceOrder_RetrievePersonnelInitial trans = new ServiceOrder_RetrievePersonnelInitial()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.ServiceOrder = this.ToDateBaseObjectForRetrievePersonnelInitial();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            ServiceOrder objServiceOrder = new ServiceOrder();
            objServiceOrder.UpdateFromRetrievePersonnelInitial(trans.objServiceOrder);
            return objServiceOrder;
        }
        public void AddRemoveScheduleRecord(DatabaseKey dbKey)
        {
            ServiceOrder_AddRemoveScheduleRecord trans = new ServiceOrder_AddRemoveScheduleRecord()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.ServiceOrder = this.ToDatabaseObject();
            trans.ServiceOrder.ServiceOrderId = this.ServiceOrderId;
            trans.ServiceOrder.ScheduleDate = this.ScheduleDate;
            trans.ServiceOrder.ScheduledHours = this.ScheduledHours;
            trans.ServiceOrder.PersonnelList = this.PersonnelList;
            trans.ServiceOrder.IsDeleteFlag = this.IsDeleteFlag;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.ServiceOrder);
        }
        #endregion

        #region Fleet Only

        public List<ServiceOrder> RetrieveDashboardChart(DatabaseKey dbKey, ServiceOrder sj)
        {
            ServiceOrder_RetrieveDashboardChart trans = new ServiceOrder_RetrieveDashboardChart()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.ServiceOrder = sj.ToDatabaseObjectDashBoardChart();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return ServiceOrder.UpdateFromDatabaseObjectList(trans.ServiceOrderList);
        }
        public b_ServiceOrder ToDatabaseObjectDashBoardChart()
        {
            b_ServiceOrder dbObj = new b_ServiceOrder();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;            
            return dbObj;
        }
        public static List<ServiceOrder> UpdateFromDatabaseObjectList(List<b_ServiceOrder> dbObjs)
        {
            List<ServiceOrder> result = new List<ServiceOrder>();

            foreach (b_ServiceOrder dbObj in dbObjs)
            {
                ServiceOrder tmp = new ServiceOrder();
                tmp.UpdateFromDatabaseObjectExtended(dbObj);
                result.Add(tmp);
            }
            return result;
        }
        public void UpdateFromDatabaseObjectExtended(b_ServiceOrder dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.Assigned = dbObj.Assigned;
            this.CreateDate = dbObj.CreateDate;
            this.ServiceOrderCount = dbObj.ServiceOrderCount;         
        }
        #endregion

        #region Retrieve By Equipment Id
        //public ServiceOrder RetrieveByEquipmentId1(DatabaseKey dbKey)
        //{
        //    ServiceOrder_RetrieveByEquipmentId trans = new ServiceOrder_RetrieveByEquipmentId()
        //    {
        //        CallerUserInfoId = dbKey.User.UserInfoId,
        //        CallerUserName = dbKey.UserName,

        //    };
        //    trans.ServiceOrder = this.ToDateBaseObjectForRetrieveByEquipmentId();
        //    trans.dbKey = dbKey.ToTransDbKey();
        //    trans.Execute();
        //    ServiceOrder objServiceOrder = new ServiceOrder();
        //    objServiceOrder.UpdateFromRetrieveByServiceOrderId(trans.objServiceOrder);
        //    return objServiceOrder;
        //}

        public List<ServiceOrder> RetrieveByEquipmentId(DatabaseKey dbKey)
        {
            ServiceOrder_RetrieveByEquipmentId trans = new ServiceOrder_RetrieveByEquipmentId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.ServiceOrder = this.ToDateBaseObjectForRetrieveByEquipmentId();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
           
            List<ServiceOrder> ServiceOrderlist = new List<ServiceOrder>();

            foreach (b_ServiceOrder serviceorder in trans.ServiceOrderList)
            {
                ServiceOrder tmpServiceOrder = new ServiceOrder();
                tmpServiceOrder.UpdateFromRetrieveByEquipmentId(serviceorder);
                ServiceOrderlist.Add(tmpServiceOrder);
            }
            return ServiceOrderlist;
        }

        public b_ServiceOrder ToDateBaseObjectForRetrieveByEquipmentId()
        {
            b_ServiceOrder dbObj = this.ToDatabaseObject();
            dbObj.ServiceOrderId = this.ServiceOrderId;
            dbObj.ClientLookupId = this.ClientLookupId;
            dbObj.EquipmentClientLookupId = this.EquipmentClientLookupId;
            dbObj.AssetName = this.AssetName;
            dbObj.Status = this.Status;
            dbObj.Type = this.Type;
            dbObj.CreateDate = this.CreateDate;
            dbObj.CompleteDate = this.CompleteDate;
            dbObj.ChildCount = this.ChildCount;
            return dbObj;
        }

        public void UpdateFromRetrieveByEquipmentId(b_ServiceOrder dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);

            this.EquipmentClientLookupId = dbObj.EquipmentClientLookupId;
            this.AssetName = dbObj.AssetName;
            this.Status = dbObj.Status;
            this.Type = dbObj.Type;
            this.CreateDate = dbObj.CreateDate;
            this.CompleteDate = dbObj.CompleteDate;
            this.ChildCount = dbObj.ChildCount;
        }
        #endregion

        #region Service order history
        public List<ServiceOrder> RetrieveServiceOrderHistory(DatabaseKey dbKey)
        {
            ServiceOrder_ServiceOrderHistory trans = new ServiceOrder_ServiceOrderHistory()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.ServiceOrder = this.ToDateBaseObjectForRetrieveChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<ServiceOrder> ServiceOrderlist = new List<ServiceOrder>();

            foreach (b_ServiceOrder serviceorder in trans.ServiceOrderList)
            {
                ServiceOrder tmpServiceOrder = new ServiceOrder();
                tmpServiceOrder.UpdateFromDatabaseObjectForRetriveAllForSearch(serviceorder);
                ServiceOrderlist.Add(tmpServiceOrder);
            }
            return ServiceOrderlist;
        }
        #endregion
    }
}
