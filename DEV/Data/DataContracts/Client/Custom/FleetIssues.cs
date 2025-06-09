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
    public partial class FleetIssues : DataContractBase, IStoredProcedureValidation
    {
        #region Properties
        public int CustomQueryDisplayId { get; set; }
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int OffSetVal { get; set; }
        public int NextRow { get; set; }
        public string SerialNo { get; set; }
        public string ModelNo { get; set; }
        public string SearchText { get; set; }
        public string EquipmentClientLookupId { get; set; }
        public string ServiceOrderClientLookupId { get; set; }
        public string CreateStartDateVw { get; set; }
        public string CreateEndDateVw { get; set; }
        public string RecordStartDate { get; set; }
        public string RecordEndDate { get; set; }
        public Int32 TotalCount { get; set; }
        public string EquipImage { get; set; }


        public string ClientLookupId { get; set; }

        public string Name { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }
        public string VIN { get; set; }

        public long PrevFleeissueId { get; set; }
        public string ValidateFor = string.Empty;

        #region Fleet Only
        public int FleetIssuesCount { get; set; }
        #endregion
        #endregion
        #region Transaction
        public List<FleetIssues> FleetIssueRetrieveChunkSearchV2(DatabaseKey dbKey, string TimeZone)
        {
            FleetIssues_RetrieveFleetIssueChunkSearchV2 trans = new FleetIssues_RetrieveFleetIssueChunkSearchV2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.FleetIssues = this.ToDateBaseObjectForChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<FleetIssues> FleetIssueslist = new List<FleetIssues>();
            foreach (b_FleetIssues FleetIssues in trans.FleetIssues.listOfFleetIssues)
            {
                FleetIssues tmpFleetIssues = new FleetIssues();

                tmpFleetIssues.UpdateFromDatabaseObjectForFleetIssueChunkSearch(FleetIssues, TimeZone);
                FleetIssueslist.Add(tmpFleetIssues);
            }
            return FleetIssueslist;
        }
        public void UpdateFromDatabaseObjectForFleetIssueChunkSearch(b_FleetIssues dbObj, string TimeZone)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.TotalCount = dbObj.TotalCount;
            this.RecordDate = dbObj.RecordDate;
            this.FleetIssuesId = dbObj.FleetIssuesId;
            this.Description = dbObj.Description;
            this.EquipImage = dbObj.EquipImage;
            this.Status = dbObj.Status;
            this.Defects = dbObj.Defects;
            this.CompleteDate = dbObj.CompleteDate;
            this.ClientLookupId = dbObj.ClientLookupId;
            this.Name = dbObj.Name;
            this.ServiceOrderClientLookupId = dbObj.ServiceOrderClientLookupId;
        }
        public void RetrieveByFleetIssuesId(DatabaseKey dbKey)
        {
            FleetIssues_RetrieveByFleetIssuesIdFromDatabase trans = new FleetIssues_RetrieveByFleetIssuesIdFromDatabase()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.FleetIssues = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.FleetIssues);
            this.EquipmentClientLookupId = trans.FleetIssues.EquipmentClientLookupId;

            trans.FleetIssues.EquipmentClientLookupId = this.EquipmentClientLookupId;


        }

        public b_FleetIssues ToDateBaseObjectForChunkSearch()
        {
            b_FleetIssues dbObj = this.ToDatabaseObject();
            dbObj.CustomQueryDisplayId = this.CustomQueryDisplayId;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;
            dbObj.ClientLookupId = this.ClientLookupId;
            dbObj.Name = this.Name;
            dbObj.Make = this.Make;
            dbObj.Model = this.Model;
            dbObj.VIN = this.VIN;
            dbObj.Defects = this.Defects;
            dbObj.RecordStartDate = RecordStartDate;
            dbObj.RecordEndDate = RecordEndDate;
            dbObj.CreateStartDateVw = CreateStartDateVw;
            dbObj.CreateEndDateVw = CreateEndDateVw;
            dbObj.SearchText = this.SearchText;
            return dbObj;
        }
        #endregion

        #region Fleet Only

        public List<FleetIssues> RetrieveDashboardChart(DatabaseKey dbKey, FleetIssues sj)
        {
            FleetIssues_RetrieveDashboardChart trans = new FleetIssues_RetrieveDashboardChart()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.FleetIssues = sj.ToDatabaseObjectDashBoardChart();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return FleetIssues.UpdateFromDatabaseObjectList(trans.FleetIssuesList);
        }
        public b_FleetIssues ToDatabaseObjectDashBoardChart()
        {
            b_FleetIssues dbObj = new b_FleetIssues();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            return dbObj;
        }
        public static List<FleetIssues> UpdateFromDatabaseObjectList(List<b_FleetIssues> dbObjs)
        {
            List<FleetIssues> result = new List<FleetIssues>();

            foreach (b_FleetIssues dbObj in dbObjs)
            {
                FleetIssues tmp = new FleetIssues();
                tmp.UpdateFromDatabaseObjectExtended(dbObj);
                result.Add(tmp);
            }
            return result;
        }
        public void UpdateFromDatabaseObjectExtended(b_FleetIssues dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.FleetIssuesCount = dbObj.FleetIssuesCount;

        }
        #endregion

        #region Retrieve By Equipment Id
        public List<FleetIssues> FleetIssueRetrieveByEquipmentIdV2(DatabaseKey dbKey, string TimeZone)
        {
            FleetIssues_RetrieveFleetIssueByEquipmentIdV2 trans = new FleetIssues_RetrieveFleetIssueByEquipmentIdV2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.FleetIssues = this.ToDateBaseObjectForRetrieveByEquipmentId();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<FleetIssues> FleetIssueslist = new List<FleetIssues>();
            foreach (b_FleetIssues FleetIssues in trans.FleetIssuesList)
            {
                FleetIssues tmpFleetIssues = new FleetIssues();

                tmpFleetIssues.UpdateFromDatabaseObjectForFleetIssueRetrieveByEquipmentId(FleetIssues, TimeZone);
                FleetIssueslist.Add(tmpFleetIssues);
            }
            return FleetIssueslist;
        }

        public b_FleetIssues ToDateBaseObjectForRetrieveByEquipmentId()
        {
            b_FleetIssues dbObj = this.ToDatabaseObject();
            dbObj.FleetIssuesId = this.FleetIssuesId;
            dbObj.EquipmentId = this.EquipmentId;
            dbObj.EquipmentClientLookupId = this.EquipmentClientLookupId;
            dbObj.RecordDate = this.RecordDate;
            dbObj.Defects = this.Defects;
            dbObj.Description = this.Description;
            dbObj.DriverName = this.DriverName;
            dbObj.Status = this.Status;
            dbObj.CompleteDate = this.CompleteDate;
            dbObj.ServiceOrderClientLookupId = this.ServiceOrderClientLookupId;

            return dbObj;
        }

        public void UpdateFromDatabaseObjectForFleetIssueRetrieveByEquipmentId(b_FleetIssues dbObj, string TimeZone)
        {
            this.UpdateFromDatabaseObject(dbObj);

            this.FleetIssuesId = dbObj.FleetIssuesId;
            this.EquipmentId = dbObj.EquipmentId;
            this.EquipmentClientLookupId = dbObj.EquipmentClientLookupId;
            this.RecordDate = dbObj.RecordDate;
            this.Defects = dbObj.Defects;
            this.Description = dbObj.Description;
            this.DriverName = dbObj.DriverName;
            this.Status = dbObj.Status;
            this.CompleteDate = dbObj.CompleteDate;
            this.ServiceOrderClientLookupId = dbObj.ServiceOrderClientLookupId;
        }
        #endregion



        public void CheckIfServiceOrderExist(DatabaseKey dbKey)
        {
            ValidateFor = "CheckIfServiceOrderExist";
            Validate<FleetIssues>(dbKey);
        }


        public List<FleetIssues> RetrieveByServiceOrderId_V2(DatabaseKey dbKey)
        {
            FleetIssues_RetrieveByServiceOrderId_V2 trans = new FleetIssues_RetrieveByServiceOrderId_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.FleetIssues = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            return (UpdateFromDatabaseList(trans.FIList));
        }


        public static List<FleetIssues> UpdateFromDatabaseList(List<b_FleetIssues> dbObjs)
        {
            List<FleetIssues> result = new List<FleetIssues>();

            foreach (b_FleetIssues dbObj in dbObjs)
            {
                FleetIssues tmp = new FleetIssues();
                tmp.UpdateFromDatabaseObject(dbObj);
                result.Add(tmp);
            }
            return result;
        }

        public void FleetIssuesUpdateforPrevandNewFleetissues(DatabaseKey dbKey)
        {
            FleetIssues_UpdateforPrevandNewFleetissues trans = new FleetIssues_UpdateforPrevandNewFleetissues()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.FleetIssues = this.ToDatabaseObject();
            trans.FleetIssues.PrevFleeissueId = this.PrevFleeissueId;
            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.FleetIssues);
        }

        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();
            if (ValidateFor == "CheckIfServiceOrderExist")
            {
                FleetIssues_IfServiceOrdrExist trans = new FleetIssues_IfServiceOrdrExist()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.FleetIssues = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                errors = StoredProcValidationError.UpdateFromDatabaseObjects(trans.StoredProcValidationErrorList);
            }
            return errors;
        }
    }
}
