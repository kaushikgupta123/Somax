using Common.Structures;
using Database;
using Database.Business;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    public partial class ScheduledService : DataContractBase, IStoredProcedureValidation
    {
        #region Properties
        public string CreateBy { get; set; }
        public string ModifyBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public string ValidateFor = string.Empty;
        public string ImageUrl { get; set; }
        public int CustomQueryDisplayId { get; set; }
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int OffSetVal { get; set; }
        public int NextRow { get; set; }      
        public string ClientLookupId { get; set; }
        public string Name { get; set; }
        public string ServiceTasksDescription { get; set; }
        public string Schedule { get; set; }
        public string NextDue { get; set; }
        public string LastCompleted { get; set; }
        public string ServiceTaskDesc { get; set; }
       
        public string LastCompletedLine1 { get; set; }
        public string LastCompletedLine2 { get; set; }
        public string LastCompletedstr { get; set; }      
        public List<ScheduledService> listOfScheduledService { get; set; }
        public Int32 TotalCount { get; set; }
        public string SearchText { get; set; }
        public string Meter1Type { get; set; }
        public string Meter2Type { get; set; }
        public string Meter1Units { get; set; }
        public string Meter2Units { get; set; }

        #region Fleet Only
        public int PastDueServiceCount { get; set; }
        #endregion
        #endregion

        #region Retrieve Search
        public List<ScheduledService> ScheduledServiceRetrieveChunkSearchV2(DatabaseKey dbKey, string TimeZone)
        {
            ScheduledService_RetrievecheduledServiceChunkSearchV2 trans = new ScheduledService_RetrievecheduledServiceChunkSearchV2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.ScheduledService = this.ToDateBaseObjectForChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            this.listOfScheduledService = new List<ScheduledService>();
            List<ScheduledService> ScheduledServicelist = new List<ScheduledService>();
            foreach (b_ScheduledService ScheduledService in trans.ScheduledService.listOfScheduledService)
            {
                ScheduledService tmpScheduledService = new ScheduledService();

                tmpScheduledService.UpdateFromDatabaseObjectForScheduledServiceChunkSearch(ScheduledService, TimeZone);
                ScheduledServicelist.Add(tmpScheduledService);
            }
            return ScheduledServicelist;
        }
        public b_ScheduledService ToDateBaseObjectForChunkSearch()
        {
            b_ScheduledService dbObj = this.ToDatabaseObject();
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;
            dbObj.CustomQueryDisplayId = this.CustomQueryDisplayId;
            dbObj.Name = this.Name;
            dbObj.ClientLookupId = this.ClientLookupId;
            dbObj.ServiceTaskDesc = this.ServiceTaskDesc;
            dbObj.SearchText = this.SearchText;
            dbObj.ServiceTasksDescription = this.ServiceTasksDescription;
            dbObj.TimeInterval = this.TimeInterval;
            dbObj.Meter1Interval = this.Meter1Interval;
            dbObj.Meter2Interval = this.Meter2Interval;
            dbObj.TimeIntervalType = this.TimeIntervalType;
            dbObj.Meter1Units = this.Meter1Units;
            dbObj.Meter2Units = this.Meter2Units;
            dbObj.NextDueDate = this.NextDueDate;
            dbObj.NextDueMeter1 = this.NextDueMeter1;
            dbObj.NextDueMeter2 = this.NextDueMeter2;
            dbObj.LastPerformedDate = this.LastPerformedDate;
            dbObj.LastPerformedMeter1 = this.LastPerformedMeter1;
            dbObj.LastPerformedMeter2 = this.LastPerformedMeter2;
            dbObj.ScheduledServiceId = this.ScheduledServiceId;
            return dbObj;
        }

        public void UpdateFromDatabaseObjectForScheduledServiceChunkSearch(b_ScheduledService dbObj, string TimeZone)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.TotalCount = dbObj.TotalCount;

            this.ClientId = dbObj.ClientId;
            this.ServiceTaskId = dbObj.ServiceTaskId;
            this.EquipmentId = dbObj.EquipmentId;
            this.ClientLookupId = dbObj.ClientLookupId;
            this.Name = dbObj.Name;
            this.ServiceTasksDescription = dbObj.ServiceTasksDescription;
            this.Schedule = dbObj.Schedule;
            this.NextDue = dbObj.NextDue;
            this.ImageUrl = dbObj.ImageUrl;
            this.ServiceTasksDescription = dbObj.ServiceTasksDescription;
            this.TimeInterval = dbObj.TimeInterval;
            this.Meter1Interval = dbObj.Meter1Interval;
            this.Meter2Interval = dbObj.Meter2Interval;
            this.TimeIntervalType = dbObj.TimeIntervalType;
            this.Meter1Units = dbObj.Meter1Units;
            this.Meter2Units = dbObj.Meter2Units;
            this.NextDueDate = dbObj.NextDueDate;
            this.NextDueMeter1 = dbObj.NextDueMeter1;
            this.NextDueMeter2 = dbObj.NextDueMeter2;
            this.LastPerformedDate = dbObj.LastPerformedDate;
            this.LastPerformedMeter1 = dbObj.LastPerformedMeter1;
            this.LastPerformedMeter2 = dbObj.LastPerformedMeter2;
            this.ScheduledServiceId = dbObj.ScheduledServiceId;
            this.InactiveFlag = dbObj.InactiveFlag;
            this.LastCompletedstr = dbObj.LastCompletedstr;
        }
        #endregion
        #region Edit By id
        public void RetrieveByEquipmentIdandScheduledServiceId(DatabaseKey dbKey)
        {
            ScheduledService_RetrieveByEquipmentIdandScheduledServiceId trans = new ScheduledService_RetrieveByEquipmentIdandScheduledServiceId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,                
            };
            trans.ScheduledService = this.ToDatabaseObject();
            trans.ScheduledService.ScheduledServiceId = this.ScheduledServiceId;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromEditDatabaseObject(trans.ScheduledService);
            trans.ScheduledService.ServiceTaskId = this.ServiceTaskId;
            trans.ScheduledService.Last_ServiceOrderId = this.Last_ServiceOrderId;
            trans.ScheduledService.LastPerformedDate = this.LastPerformedDate;
            trans.ScheduledService.LastPerformedMeter1 = this.LastPerformedMeter1;
            trans.ScheduledService.LastPerformedMeter2 = this.LastPerformedMeter2;
            trans.ScheduledService.Meter1Interval = this.Meter1Interval;
            trans.ScheduledService.Meter1Threshold = this.Meter1Threshold;
            trans.ScheduledService.Meter2Interval = this.Meter2Interval;
            trans.ScheduledService.Meter2Threshold = this.Meter2Threshold;
            trans.ScheduledService.NextDueDate = this.NextDueDate;
            trans.ScheduledService.NextDueMeter1 = this.NextDueMeter1;
            trans.ScheduledService.NextDueMeter2 = this.NextDueMeter2;
            trans.ScheduledService.TimeInterval = this.TimeInterval;
            trans.ScheduledService.TimeIntervalType = this.TimeIntervalType;
            trans.ScheduledService.TimeThreshold = this.TimeThreshold;
            trans.ScheduledService.TimeThresoldType = this.TimeThresoldType;
            trans.ScheduledService.CreateBy = this.CreateBy;
            trans.ScheduledService.CreateDate = this.CreateDate;
            trans.ScheduledService.ModifyBy = this.ModifyBy;
            trans.ScheduledService.ModifyDate = this.ModifyDate;
            trans.ScheduledService.EquipmentId = this.EquipmentId;
            trans.ScheduledService.ClientLookupId = this.ClientLookupId;
            trans.ScheduledService.AreaId = this.AreaId;
            trans.ScheduledService.DepartmentId = this.DepartmentId;
            trans.ScheduledService.StoreroomId = this.StoreroomId;
            trans.ScheduledService.Meter1Type = this.Meter1Type;
            trans.ScheduledService.Meter2Type = this.Meter2Type;
            trans.ScheduledService.Meter1Units = this.Meter1Units;
            trans.ScheduledService.Meter2Units = this.Meter2Units;
            trans.ScheduledService.RepairReason = this.RepairReason;
            trans.ScheduledService.VMRSSystem = this.VMRSSystem;
            trans.ScheduledService.VMRSAssembly = this.VMRSAssembly;
        }

        public void UpdateFromEditDatabaseObject(b_ScheduledService dbObj)
        {
            this.ScheduledServiceId = dbObj.ScheduledServiceId;
            this.ClientId = dbObj.ClientId;
            this.SiteId = dbObj.SiteId;
            this.AreaId = dbObj.AreaId;
            this.DepartmentId = dbObj.DepartmentId;
            this.StoreroomId = dbObj.StoreroomId;
            this.ServiceTaskId = dbObj.ServiceTaskId;
            this.EquipmentId = dbObj.EquipmentId;
            this.InactiveFlag = dbObj.InactiveFlag;
            this.Last_ServiceOrderId = dbObj.Last_ServiceOrderId;
            this.LastPerformedDate = dbObj.LastPerformedDate;
            this.LastPerformedMeter1 = dbObj.LastPerformedMeter1;
            this.LastPerformedMeter2 = dbObj.LastPerformedMeter2;
            this.Meter1Interval = dbObj.Meter1Interval;
            this.Meter1Threshold = dbObj.Meter1Threshold;
            this.Meter2Interval = dbObj.Meter2Interval;
            this.Meter2Threshold = dbObj.Meter2Threshold;
            this.NextDueDate = dbObj.NextDueDate;
            this.NextDueMeter1 = dbObj.NextDueMeter1;
            this.NextDueMeter2 = dbObj.NextDueMeter2;
            this.TimeInterval = dbObj.TimeInterval;
            this.TimeIntervalType = dbObj.TimeIntervalType;
            this.TimeThreshold = dbObj.TimeThreshold;
            this.TimeThresoldType = dbObj.TimeThresoldType;
            this.ClientLookupId = dbObj.ClientLookupId;
            this.Meter1Type = dbObj.Meter1Type;
            this.Meter2Type = dbObj.Meter2Type;
            this.Meter1Units = dbObj.Meter1Units;
            this.Meter2Units = dbObj.Meter2Units;
            this.RepairReason = dbObj.RepairReason;
            this.VMRSSystem = dbObj.VMRSSystem;
            this.VMRSAssembly = dbObj.VMRSAssembly;
            AuditEnabled = true;
        }
        #endregion

        #region Issert Data
        public void CreateCustom(DatabaseKey dbKey)
        {
            ScheduledService_CreateCustom trans = new ScheduledService_CreateCustom();
            trans.ScheduledService= this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.ScheduledService);
        }
        #endregion

        #region Update Data
        public void UpdateCustom(DatabaseKey dbKey)
        {
            ScheduledService_UpdateCustom trans = new ScheduledService_UpdateCustom();
            trans.ScheduledService = this.ToDatabaseObject();
            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.ScheduledService);
        }
        #endregion
        #region Active/InActive Schduled Service
        public void RetrieveByPKForeignKeys_V2(DatabaseKey dbKey)
        {
            ScheduledService_RetrieveByForeignKeys_V2 trans = new ScheduledService_RetrieveByForeignKeys_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.ScheduledService = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObject(trans.ScheduledService);
            this.ScheduledServiceId = trans.ScheduledService.ScheduledServiceId;
            this.ClientId = trans.ScheduledService.ClientId;
            this.SiteId = trans.ScheduledService.SiteId;
            this.AreaId = trans.ScheduledService.AreaId;
            this.DepartmentId = trans.ScheduledService.DepartmentId;
            this.StoreroomId = trans.ScheduledService.StoreroomId;
            this.ServiceTaskId = trans.ScheduledService.ServiceTaskId;
            this.EquipmentId = trans.ScheduledService.EquipmentId;
            this.InactiveFlag = trans.ScheduledService.InactiveFlag;
            this.Last_ServiceOrderId = trans.ScheduledService.Last_ServiceOrderId;
            this.LastPerformedDate = trans.ScheduledService.LastPerformedDate;
            this.LastPerformedMeter1 = trans.ScheduledService.LastPerformedMeter1;
            this.LastPerformedMeter2 = trans.ScheduledService.LastPerformedMeter2;
            this.Meter1Interval = trans.ScheduledService.Meter1Interval;
            this.Meter1Threshold = trans.ScheduledService.Meter1Threshold;
            this.Meter2Interval = trans.ScheduledService.Meter2Interval;
            this.Meter2Threshold = trans.ScheduledService.Meter2Threshold;
            this.NextDueDate = trans.ScheduledService.NextDueDate;
            this.NextDueMeter1 = trans.ScheduledService.NextDueMeter1;
            this.NextDueMeter2 = trans.ScheduledService.NextDueMeter2;
            this.TimeInterval = trans.ScheduledService.TimeInterval;
            this.TimeIntervalType = trans.ScheduledService.TimeIntervalType;
            this.TimeThreshold = trans.ScheduledService.TimeThreshold;
            this.TimeThresoldType = trans.ScheduledService.TimeThresoldType;

        }

        public void UpdateByPKForeignKeys_V2(DatabaseKey dbKey)
        {
            
            if (IsValid)
            {

                ScheduledService_UpdateByForeignKeys_V2 trans = new ScheduledService_UpdateByForeignKeys_V2()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };

                trans.ScheduledService = this.ToDatabaseObject();

            

                trans.ChangeLog = GetChangeLogObject(dbKey);
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                // The create procedure changed the Update Index.
                UpdateFromDatabaseObject(trans.ScheduledService);
            }
        }

        #endregion
        #region Validate Equipment and Service Task Id
        public void CheckDuplicateScheduledService(DatabaseKey dbKey)
        {
            ValidateFor = "CheckDuplicate";
            Validate<ScheduledService>(dbKey);
        }
        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();
            //--------Check For Equipment is Parent of another-------------------------------------------------
            if (ValidateFor == "CheckDuplicate")
            {
                ScheduledService_ValidateByEquipmentAndServiceTaskId trans = new ScheduledService_ValidateByEquipmentAndServiceTaskId()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.ScheduledService = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                errors = StoredProcValidationError.UpdateFromDatabaseObjects(trans.StoredProcValidationErrorList);


            }
            else if (ValidateFor == "ActivateInactivate")
            {
                ScheduledService_ValidateInactivateOrActivate trans = new ScheduledService_ValidateInactivateOrActivate()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.ScheduledService = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                errors = StoredProcValidationError.UpdateFromDatabaseObjects(trans.StoredProcValidationErrorList);

            }
            return errors;
        }
        #endregion

        #region Fleet Only

        public List<ScheduledService> RetrieveDashboardChart(DatabaseKey dbKey, ScheduledService sj)
        {
            ScheduledService_RetrieveDashboardChart trans = new ScheduledService_RetrieveDashboardChart()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.ScheduledService = sj.ToDatabaseObjectDashBoardChart();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return ScheduledService.UpdateFromDatabaseObjectList(trans.ScheduledServiceList);
        }
        public b_ScheduledService ToDatabaseObjectDashBoardChart()
        {
            b_ScheduledService dbObj = new b_ScheduledService();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            return dbObj;
        }
        public static List<ScheduledService> UpdateFromDatabaseObjectList(List<b_ScheduledService> dbObjs)
        {
            List<ScheduledService> result = new List<ScheduledService>();
            foreach (b_ScheduledService dbObj in dbObjs)
            {
                ScheduledService tmp = new ScheduledService();
                tmp.UpdateFromDatabaseObjectExtended(dbObj);
                result.Add(tmp);
            }
            return result;
        }
        public void UpdateFromDatabaseObjectExtended(b_ScheduledService dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.CreateDate = dbObj.CreateDate;
            this.PastDueServiceCount = dbObj.PastDueServiceCount;          
        }
        #endregion

        #region Scheduled Service Retrieve By Equipment Id
        public List<ScheduledService> ScheduledServiceRetrieveByEquipmentIdV2(DatabaseKey dbKey, string TimeZone)
        {
            ScheduledService_RetrieveByEquipmentIdV2 trans = new ScheduledService_RetrieveByEquipmentIdV2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.ScheduledService = this.ToDateBaseObjectForRetrieveByEquipmentId();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            this.listOfScheduledService = new List<ScheduledService>();
            List<ScheduledService> ScheduledServicelist = new List<ScheduledService>();
            foreach (b_ScheduledService ScheduledService in trans.ScheduledServiceList)
            {
                ScheduledService tmpScheduledService = new ScheduledService();

                tmpScheduledService.UpdateFromDatabaseObjectForScheduledServiceRetrieveByEquipmentId(ScheduledService, TimeZone);
                ScheduledServicelist.Add(tmpScheduledService);
            }
            return ScheduledServicelist;
        }

        public b_ScheduledService ToDateBaseObjectForRetrieveByEquipmentId()
        {
            b_ScheduledService dbObj = this.ToDatabaseObject();
            dbObj.ClientId = this.ClientId;
            dbObj.ScheduledServiceId = this.ScheduledServiceId;
            dbObj.ServiceTaskId = this.ServiceTaskId;
            dbObj.ServiceTasksDescription = this.ServiceTasksDescription;
            dbObj.TimeInterval = this.TimeInterval;
            dbObj.TimeIntervalType = this.TimeIntervalType;
            dbObj.Meter1Interval = this.Meter1Interval;
            dbObj.Meter1Units = this.Meter1Units;
            dbObj.Meter2Interval = this.Meter2Interval;
            dbObj.Meter2Units = this.Meter2Units;
            dbObj.NextDueDate = this.NextDueDate;
            dbObj.TimeThresoldType = this.TimeThresoldType;
            dbObj.LastPerformedDate = this.LastPerformedDate;
            dbObj.LastPerformedMeter1 = this.LastPerformedMeter1;
            dbObj.LastPerformedMeter2 = this.LastPerformedMeter2;
            dbObj.NextDueMeter1 = this.NextDueMeter1;
            dbObj.NextDueMeter2 = this.NextDueMeter2;
            dbObj.Meter1Type = this.Meter1Type;
            dbObj.Meter2Type = this.Meter2Type;
            return dbObj;
        }

        public void UpdateFromDatabaseObjectForScheduledServiceRetrieveByEquipmentId(b_ScheduledService dbObj, string TimeZone)
        {
            this.UpdateFromDatabaseObject(dbObj);

            this.ClientId = dbObj.ClientId;
            this.ServiceTaskId = dbObj.ServiceTaskId;
            this.ServiceTasksDescription = dbObj.ServiceTasksDescription;
            this.TimeInterval = dbObj.TimeInterval;
            this.TimeIntervalType = dbObj.TimeIntervalType;
            this.Meter1Interval = dbObj.Meter1Interval;
            this.Meter1Units = dbObj.Meter1Units;
            this.Meter2Interval = dbObj.Meter2Interval;
            this.Meter2Units = dbObj.Meter2Units;
            this.NextDueDate = dbObj.NextDueDate;
            this.TimeThresoldType = dbObj.TimeThresoldType;
            this.LastPerformedDate = dbObj.LastPerformedDate;
            this.LastPerformedMeter1 = dbObj.LastPerformedMeter1;
            this.LastPerformedMeter2 = dbObj.LastPerformedMeter2;
            this.NextDueMeter1 = dbObj.NextDueMeter1;
            this.NextDueMeter2 = dbObj.NextDueMeter2;
            this.Meter1Type = dbObj.Meter1Type;
            this.Meter2Type = dbObj.Meter2Type;
            
        }
        #endregion

        #region Schduled service for Service order
        public List<ScheduledService> RetrieveScheduledServiceForServiceOrder(DatabaseKey dbKey, string TimeZone)
        {
            ScheduledService_RetrieveScheduledServiceForServiceOrder trans = new ScheduledService_RetrieveScheduledServiceForServiceOrder()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.ScheduledService = this.ToDateBaseObjectForChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            this.listOfScheduledService = new List<ScheduledService>();
            List<ScheduledService> ScheduledServicelist = new List<ScheduledService>();
            foreach (b_ScheduledService ScheduledService in trans.ScheduledService.listOfScheduledService)
            {
                ScheduledService tmpScheduledService = new ScheduledService();

                tmpScheduledService.UpdateFromDatabaseObjectForScheduledServiceChunkSearch(ScheduledService, TimeZone);
                ScheduledServicelist.Add(tmpScheduledService);
            }
            return ScheduledServicelist;
        }
        #endregion

        #region Validate for activate inactivate
        public void ValidateForActivateInactivate(DatabaseKey dbKey)
        {
            ValidateFor = "ActivateInactivate";
            Validate<ScheduledService>(dbKey);
        }
        #endregion
    }
}
