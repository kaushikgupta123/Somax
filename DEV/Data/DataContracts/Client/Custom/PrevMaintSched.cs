/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2014 by SOMAX Inc.
* PreventiveMaintenanceDetails.aspx.cs
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID  Person             Description
* =========== ======== ================== =========================================================
* 2014-Aug-29 SOM-304  Roger Lawton       Structure Change and Screen Modification
* 2014-Sep-12 SOM-317  Roger Lawton       Need WorkOrder.ClientLookupId
* 2014-Sep-25 SOM-338  Roger Lawton       Modified Retrieve Methods 
****************************************************************************************************
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using Database;
using Database.Business;
using Database.Client.Custom.Transactions;

namespace DataContracts
{
    public partial class PrevMaintSched : DataContractBase, IStoredProcedureValidation
    {
        #region Private Variables
        private int m_ValidateType;   // 1 - Add, 2 - Save
        #endregion
        #region constants
        private const int Validate_Add = 1;
        private const int Validate_Save = 2;
        #endregion
        #region Properties
        [DataMember]
        public string ClientLookupId { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public decimal EstimatedTotalCosts { get; set; }

        [DataMember]
        public decimal jobDuration { get; set; }

        [DataMember]
        public decimal EstLaborHours { get; set; }
        [DataMember]
        public decimal EstLaborCost { get; set; }

        [DataMember]
        public decimal EstOtherCost { get; set; }
        [DataMember]
        public decimal Duration { get; set; }

        [DataMember]
        public decimal EstMaterialCost { get; set; }

        [DataMember]
        public string AssignedTo_PersonnelClientLookupId { get; set; }

        [DataMember]
        public string ChargeToClientLookupId { get; set; }
        [DataMember]
        public DateTime SchedueledDate { get; set; }
        [DataMember]
        public DateTime CurrentDate { get; set; }
        [DataMember]
        public string ChargeToName { get; set; }

        [DataMember]
        public string AssignedTo_PersonnelName { get; set; }

        [DataMember]
        public string Meter_ClientLookupId { get; set; }

        [DataMember]
        public string WorkOrder_ClientLookupId { get; set; }
        [DataMember]
        public string Planner_ClientLookupId { get; set; } //V2-950

        [DataMember]
        public int PMForeCastId { get; set; }
        public long SiteId { get; set; }
        public int ChildCount { get; set; }
        public int NumbersOfPMSchedAssignRecords { get; set; } //V2-1161
        #region V2-1111
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int Offset { get; set; }
        public int Nextrow { get; set; }
        public string AssignedTo { get; set; }
        public string PMID { get; set; }
        public string NextDue { get; set; }
        public int TotalCount { get; set; }
        #endregion
        #region V2-977
        public long PMSAssignId { get; set; }
        public long IndexId { get; set; }
        public DateTime RequiredDate { get; set; }
        public string AssignedTo_PersonnelIds { get; set; }
        public string PMForeCastRequiredDate { get; set; }
        public string AssignedMultiple { get; set; }
        public bool? ForecastDownRequired { get; set; }   //1082
        #endregion
        public string FrequencyWithType { get; set; } //V2-1212
        #endregion

        public static List<PrevMaintSched> UpdateFromDatabaseObjectList(List<b_PrevMaintSched> dbObjs)
        {
            List<PrevMaintSched> result = new List<PrevMaintSched>();

            foreach (b_PrevMaintSched dbObj in dbObjs)
            {
                PrevMaintSched tmp = new PrevMaintSched();
                tmp.UpdateFromExtendedDatabaseObject(dbObj);
                result.Add(tmp);
            }
            return result;
        }
        //SOM-972
        public static List<PrevMaintSched> UpdateFromDatabaseObjectListForSchedulingRecords(List<b_PrevMaintSched> dbObjs)
        {
            List<PrevMaintSched> result = new List<PrevMaintSched>();

            foreach (b_PrevMaintSched dbObj in dbObjs)
            {
                PrevMaintSched tmp = new PrevMaintSched();
                tmp.UpdateFromExtendedDatabaseObjectForSchedulingRecords(dbObj);
                result.Add(tmp);
            }
            return result;
        }
        //SOM-1018
        public static List<PrevMaintSched> UpdateFromDBObjectListForForecast(List<b_PrevMaintSched> dbObjs)
        {
            List<PrevMaintSched> result = new List<PrevMaintSched>();

            foreach (b_PrevMaintSched dbObj in dbObjs)
            {
                PrevMaintSched tmp = new PrevMaintSched();
                tmp.UpdateFromExtendedDatabaseObjectForCalendarForecast(dbObj);
                result.Add(tmp);
            }
            return result;
        }
        public void InitializeExtended()
        {
            ClientLookupId = string.Empty;
            Description = string.Empty;
            EstimatedTotalCosts = 0.0M;
            AssignedTo_PersonnelClientLookupId = string.Empty;
            ChargeToClientLookupId = string.Empty;
            AssignedTo_PersonnelName = string.Empty;
            Meter_ClientLookupId = string.Empty;
            WorkOrder_ClientLookupId = string.Empty;
            SiteId = 0;
        }

        public void UpdateFromCompositeDatabaseObject(b_PrevMaintSched dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.AssignedTo_PersonnelClientLookupId = dbObj.AssignedTo_PersonnelClientLookupId;
            this.ChargeToClientLookupId = dbObj.ChargeToClientLookupId;
        }

        public void UpdateFromExtendedDatabaseObject(b_PrevMaintSched dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.SiteId = dbObj.SiteId;
            this.ClientLookupId = dbObj.ClientLookupId;
            this.Description = dbObj.Description;
            this.EstimatedTotalCosts = dbObj.EstimatedTotalCosts;
            this.ChargeToName = dbObj.ChargeToName;
            this.AssignedTo_PersonnelClientLookupId = dbObj.AssignedTo_PersonnelClientLookupId;
            this.ChargeToClientLookupId = dbObj.ChargeToClientLookupId;
            this.AssignedTo_PersonnelName = dbObj.AssignedTo_PersonnelName;
            this.Meter_ClientLookupId = dbObj.Meter_ClientLookupId;
            this.WorkOrder_ClientLookupId = dbObj.WorkOrder_ClientLookupId;
            this.Planner_ClientLookupId = dbObj.Planner_ClientLookupId;
            this.ChildCount = dbObj.ChildCount;
            this.NumbersOfPMSchedAssignRecords = dbObj.NumbersOfPMSchedAssignRecords; //V2-1161
        }
        public void UpdateFromExtendedDatabaseObjectForSchedulingRecords(b_PrevMaintSched dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.SiteId = dbObj.SiteId;
            this.ClientLookupId = dbObj.ClientLookupId;
            this.Description = dbObj.Description;
            this.PrevMaintSchedId = dbObj.PrevMaintSchedId;
            this.AssignedTo_PersonnelClientLookupId = dbObj.AssignedTo_PersonnelClientLookupId;
            this.ChargeToClientLookupId = dbObj.ChargeToClientLookupId;
            this.AssignedTo_PersonnelName = dbObj.AssignedTo_PersonnelName;
            this.NextDueDate = dbObj.NextDueDate;
            this.ChargeToName = dbObj.ChargeToName;
            this.AssignedTo_PersonnelId = dbObj.AssignedTo_PersonnelId;
            this.PMSAssignId = dbObj.PMSAssignId;
            this.IndexId = dbObj.IndexId;
            this.TotalCount = dbObj.TotalCount;

        }
        //SOM-1018
        public void UpdateFromExtendedDatabaseObjectForCalendarForecast(b_PrevMaintSched dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.SiteId = dbObj.SiteId;
            this.ClientLookupId = dbObj.ClientLookupId;
            this.Description = dbObj.Description;
            this.PMForeCastId = dbObj.PMForeCastId;
            this.AssignedTo_PersonnelClientLookupId = dbObj.AssignedTo_PersonnelClientLookupId;
            this.ChargeToClientLookupId = dbObj.ChargeToClientLookupId;
            this.AssignedTo_PersonnelName = dbObj.AssignedTo_PersonnelName;
            this.Duration = dbObj.Duration;
            this.EstLaborHours = dbObj.EstLaborHours;
            this.EstLaborCost = dbObj.EstLaborCost;
            this.EstOtherCost = dbObj.EstOtherCost;
            this.EstMaterialCost = dbObj.EstMaterialCost;
            this.ChargeToName = dbObj.ChargeToName;
            this.AssignedTo_PersonnelId = dbObj.AssignedTo_PersonnelId;
            this.SchedueledDate = dbObj.SchedueledDate;
            this.CurrentDate = dbObj.CurrentDate;
            this.OnDemandGroup = dbObj.OnDemandGroup;
            this.PrevMaintSchedId = dbObj.PrevMaintSchedId;
            this.RequiredDate = dbObj.RequiredDate;
            this.AssignedTo_PersonnelName = dbObj.AssignedTo_PersonnelName;
            this.ChildCount = dbObj.ChildCount;
            this.AssignedMultiple = dbObj.AssignedMultiple;
            this.DownRequired = dbObj.DownRequired;
            this.PlanningRequired = dbObj.PlanningRequired; //V2-1161
            this.Shift = dbObj.Shift;
            this.Type = dbObj.Type; //V2-1207
        }

        public static List<b_PrevMaintSched> ToDatabaseObjectList(List<PrevMaintSched> objs)
        {
            List<b_PrevMaintSched> result = new List<b_PrevMaintSched>();
            foreach (PrevMaintSched obj in objs)
            {
                result.Add(obj.ToExtendedDatabaseObject());
            }

            return result;
        }

        public b_PrevMaintSched ToExtendedDatabaseObject()
        {
            b_PrevMaintSched dbObj = this.ToDatabaseObject();
            dbObj.SiteId = this.SiteId;
            dbObj.ClientLookupId = this.ClientLookupId;
            dbObj.Description = this.Description;
            dbObj.EstimatedTotalCosts = this.EstimatedTotalCosts;
            dbObj.ChargeToName = this.ChargeToName;
            dbObj.ChargeToClientLookupId = this.ChargeToClientLookupId;
            dbObj.AssignedTo_PersonnelClientLookupId = this.AssignedTo_PersonnelClientLookupId;
            dbObj.AssignedTo_PersonnelName = this.AssignedTo_PersonnelName;
            dbObj.Meter_ClientLookupId = this.Meter_ClientLookupId;
            dbObj.WorkOrder_ClientLookupId = this.WorkOrder_ClientLookupId;
            dbObj.AssignedTo_PersonnelId = this.AssignedTo_PersonnelId;
            dbObj.PrevMaintSchedId = this.PrevMaintSchedId;

            return dbObj;
        }

        public b_PrevMaintSched ToExtendedAssignDatabaseObject()
        {
            b_PrevMaintSched dbObj = new b_PrevMaintSched();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.PMID = this.PMID;
            dbObj.Description = this.Description;
            dbObj.ChargeToName = this.ChargeToName;
            dbObj.ChargeToClientLookupId = this.ChargeToClientLookupId;
            dbObj.NextDue = this.NextDue;
            dbObj.AssignedTo = this.AssignedTo;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.Offset = this.Offset;
            dbObj.Nextrow = this.Nextrow;

            return dbObj;
        }
  
        public b_PrevMaintSched ToExtendedForecastDatabaseObject()
        {
            b_PrevMaintSched dbObj = this.ToDatabaseObject();

            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.ClientLookupId = this.ClientLookupId;
            dbObj.Description = this.Description;
            dbObj.PMForeCastId = this.PMForeCastId;
            dbObj.AssignedTo_PersonnelClientLookupId = this.AssignedTo_PersonnelClientLookupId;
            dbObj.ChargeToClientLookupId = this.ChargeToClientLookupId;
            dbObj.AssignedTo_PersonnelName = this.AssignedTo_PersonnelName;
            dbObj.jobDuration = this.jobDuration;
            dbObj.EstLaborHours = this.EstLaborHours;
            dbObj.EstLaborCost = this.EstLaborCost;
            dbObj.EstOtherCost = this.EstOtherCost;
            dbObj.EstMaterialCost = this.EstMaterialCost;
            dbObj.ChargeToName = this.ChargeToName;
            dbObj.AssignedTo_PersonnelId = this.AssignedTo_PersonnelId;
            dbObj.SchedueledDate = this.SchedueledDate;
            dbObj.CurrentDate = this.CurrentDate;
            dbObj.PrevMaintSchedId = this.PrevMaintSchedId;
            dbObj.ChildCount = this.ChildCount;
            dbObj.PMForeCastRequiredDate = this.PMForeCastRequiredDate;
            dbObj.AssignedTo_PersonnelIds = this.AssignedTo_PersonnelIds;
            dbObj.AssignedMultiple = this.AssignedMultiple;
            dbObj.ForecastDownRequired = this.ForecastDownRequired;
            dbObj.Shift = this.Shift;
            dbObj.Type = this.Type; //V2-1207
            return dbObj;
        }

        public static List<PrevMaintSched> RetriveByEquipmentId(DatabaseKey dbKey, PrevMaintSched pm)
        {
            PrevMaintSched_RetrieveByEquipmentId trans = new PrevMaintSched_RetrieveByEquipmentId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.PrevMaintSched = pm.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return PrevMaintSched.UpdateFromDatabaseObjectList(trans.PrevMaintSchedList);

        }
        public static List<PrevMaintSched> RetriveByBIMGuid(DatabaseKey dbKey, PrevMaintSched pm, Guid BIMGuid)
        {
            PrevMaintSched_RetrieveByBIMGuid trans = new PrevMaintSched_RetrieveByBIMGuid()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
                BIMGuid = BIMGuid
            };

            trans.PrevMaintSched = pm.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return PrevMaintSched.UpdateFromDatabaseObjectList(trans.PrevMaintSchedList);
        }


        public static List<PrevMaintSched> RetrieveByLocationId(DatabaseKey dbKey, PrevMaintSched pm)
        {
            PrevMaintSched_RetrieveByLocationId trans = new PrevMaintSched_RetrieveByLocationId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.PrevMaintSched = pm.ToExtendedDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return PrevMaintSched.UpdateFromDatabaseObjectList(trans.PrevMaintSchedList);
        }

        public static List<PrevMaintSched> RetrieveByPrevMaintMasterId(DatabaseKey dbKey, PrevMaintSched pm)
        {
            PrevMaintSched_RetrieveByPrevMaintMasterId trans = new PrevMaintSched_RetrieveByPrevMaintMasterId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.PrevMaintSched = pm.ToExtendedDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return PrevMaintSched.UpdateFromDatabaseObjectList(trans.PrevMaintSchedList);
        }

        public static List<PrevMaintSched> RetrieveByPrevMaintMasterId_V2(DatabaseKey dbKey, PrevMaintSched pm)
        {
            PrevMaintSched_RetrieveByPrevMaintMasterId_V2 trans = new PrevMaintSched_RetrieveByPrevMaintMasterId_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.PrevMaintSched = pm.ToExtendedDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return PrevMaintSched.UpdateFromDatabaseObjectList(trans.PrevMaintSchedList);
        }

        public void CreateByForeignKeys(DatabaseKey dbKey)
        {
            PrevMaintSched_CreateByPKForeignKeys trans = new PrevMaintSched_CreateByPKForeignKeys();
            trans.PrevMaintSched = this.ToExtendedDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure may have populated an auto-incremented key. 
            UpdateFromExtendedDatabaseObject(trans.PrevMaintSched);
        }
        public void CreateByForeignKeys_V2(DatabaseKey dbKey)
        {
            PrevMaintSched_CreateByPKForeignKeys_V2 trans = new PrevMaintSched_CreateByPKForeignKeys_V2();
            trans.PrevMaintSched = this.ToExtendedDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure may have populated an auto-incremented key. 
            UpdateFromExtendedDatabaseObject(trans.PrevMaintSched);
        }

        public void RetrieveByForeignKeys(DatabaseKey dbKey)
        {
            PrevMaintSched_RetrieveByPKForeignKeys trans = new PrevMaintSched_RetrieveByPKForeignKeys();
            trans.PrevMaintSched = this.ToExtendedDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromExtendedDatabaseObject(trans.PrevMaintSched);
        }
        public void RetrieveByForeignKeys_V2(DatabaseKey dbKey)
        {
            PrevMaintSched_RetrieveByPKForeignKeys_V2 trans = new PrevMaintSched_RetrieveByPKForeignKeys_V2();
            trans.PrevMaintSched = this.ToExtendedDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromExtendedDatabaseObject(trans.PrevMaintSched);
        }

        public void UpdateByForeignKeys(DatabaseKey dbKey)
        {

            PrevMaintSched_UpdateByPKForeignKeys trans = new PrevMaintSched_UpdateByPKForeignKeys();
            trans.PrevMaintSched = this.ToExtendedDatabaseObject();
            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromExtendedDatabaseObject(trans.PrevMaintSched);
        }

        public void UpdateByForeignKeys_V2(DatabaseKey dbKey)
        {

            PrevMaintSched_UpdateByPKForeignKeys_V2 trans = new PrevMaintSched_UpdateByPKForeignKeys_V2();
            trans.PrevMaintSched = this.ToExtendedDatabaseObject();
            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromExtendedDatabaseObject(trans.PrevMaintSched);
        }


        /*Added by Indusnet Technologies*/

        public void ValidateAdd(DatabaseKey dbKey)
        {
            m_ValidateType = Validate_Add;
            Validate<PrevMaintSched>(dbKey);

        }

        public void ValidateSave(DatabaseKey dbKey)
        {
            m_ValidateType = Validate_Save;
            Validate<PrevMaintSched>(dbKey);

        }


        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();
            // Add
            if (m_ValidateType == Validate_Add)
            {
                PrevMaintSched_ValidateInsert trans = new PrevMaintSched_ValidateInsert()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.PrevMaintSched = this.ToExtendedDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();


                if (trans.StoredProcValidationErrorList != null)
                {
                    foreach (b_StoredProcValidationError error in trans.StoredProcValidationErrorList)
                    {
                        StoredProcValidationError tmp = new StoredProcValidationError();
                        tmp.UpdateFromDatabaseObject(error);
                        errors.Add(tmp);
                    }
                }
            }
            // Save 
            if (m_ValidateType == Validate_Save)
            {
                PrevMaintSched_ValidateByClientLookupId trans = new PrevMaintSched_ValidateByClientLookupId()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.PrevMaintSched = this.ToExtendedDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();


                if (trans.StoredProcValidationErrorList != null)
                {
                    foreach (b_StoredProcValidationError error in trans.StoredProcValidationErrorList)
                    {
                        StoredProcValidationError tmp = new StoredProcValidationError();
                        tmp.UpdateFromDatabaseObject(error);
                        errors.Add(tmp);
                    }
                }
            }

            return errors;
        }


        public List<PrevMaintSched> RetrievePMSchedulingRecord(DatabaseKey databaseKey)
        {
            PrevMaintSched_RetrievePMSchedulingRecords trans = new PrevMaintSched_RetrievePMSchedulingRecords()
            {
                CallerUserInfoId = databaseKey.User.UserInfoId,
                CallerUserName = databaseKey.UserName,
            };
            trans.PrevMaintSched = this.ToExtendedAssignDatabaseObject();
            trans.dbKey = databaseKey.ToTransDbKey();
            trans.Execute();
            return PrevMaintSched.UpdateFromDatabaseObjectListForSchedulingRecords(trans.PrevMaintSchedList);
        }
        //SOM-1018
        public List<PrevMaintSched> RetrievePMCalendarForecast(DatabaseKey databaseKey)
        {
            PrevMaintSched_PMCalendarForecast trans = new PrevMaintSched_PMCalendarForecast()
            {
                CallerUserInfoId = databaseKey.User.UserInfoId,
                CallerUserName = databaseKey.UserName,
            };
            trans.PrevMaintSched = this.ToExtendedForecastDatabaseObject();
            trans.dbKey = databaseKey.ToTransDbKey();
            trans.Execute();
            return PrevMaintSched.UpdateFromDBObjectListForForecast(trans.PrevMaintSchedList);
        }
        //--SOM-1669--//
        public List<PrevMaintSched> RetrievePMCalendarForecastFromPrevMaintLibrary(DatabaseKey databaseKey)
        {
            PrevMaintSched_PMCalendarForecastFromPrevMaintLibrary trans = new PrevMaintSched_PMCalendarForecastFromPrevMaintLibrary()
            {
                CallerUserInfoId = databaseKey.User.UserInfoId,
                CallerUserName = databaseKey.UserName,
            };
            trans.PrevMaintSched = this.ToExtendedForecastDatabaseObject();
            trans.dbKey = databaseKey.ToTransDbKey();
            trans.Execute();
            return PrevMaintSched.UpdateFromDBObjectListForForecast(trans.PrevMaintSchedList);
        }
        public List<PrevMaintSched> RetrievePMOnDemandForecast(DatabaseKey databaseKey)
        {
            PrevMaintSched_PMOnDemandForecast trans = new PrevMaintSched_PMOnDemandForecast()
            {
                CallerUserInfoId = databaseKey.User.UserInfoId,
                CallerUserName = databaseKey.UserName,
            };
            trans.PrevMaintSched = this.ToExtendedForecastDatabaseObject();
            trans.dbKey = databaseKey.ToTransDbKey();
            trans.Execute();
            return PrevMaintSched.UpdateFromDBObjectListForForecast(trans.PrevMaintSchedList);
        }
        //--SOM-1669--//
        public List<PrevMaintSched> RetrievePMOnDemandForecastFromPrevMaintLibrary(DatabaseKey databaseKey)
        {
            PrevMaintSched_PMOnDemandForecastFromPrevMaintLibrary trans = new PrevMaintSched_PMOnDemandForecastFromPrevMaintLibrary()
            {
                CallerUserInfoId = databaseKey.User.UserInfoId,
                CallerUserName = databaseKey.UserName,
            };
            trans.PrevMaintSched = this.ToExtendedForecastDatabaseObject();
            trans.dbKey = databaseKey.ToTransDbKey();
            trans.Execute();
            return PrevMaintSched.UpdateFromDBObjectListForForecast(trans.PrevMaintSchedList);
        }
        public void UpdateAssignToPersonnelByPrevMaintSchedId(DatabaseKey databaseKey)
        {
            PrevMaintSched_UpdateAssignToPersonnelByPrevMaintSchedId trans = new PrevMaintSched_UpdateAssignToPersonnelByPrevMaintSchedId();
            trans.PrevMaintSched = this.ToExtendedDatabaseObject();
            trans.ChangeLog = GetChangeLogObject(databaseKey);
            trans.dbKey = databaseKey.ToTransDbKey();
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromExtendedDatabaseObjectForSchedulingRecords(trans.PrevMaintSched);
        }
        //V2-739//
        public Tuple<List<PrevMaintSched>,string> RetrievePMCalendarForecastFromPrevMaintLibrary_ConsiderExcludeDOW(DatabaseKey databaseKey)
        {
            PrevMaintSched_PMCalendarForecastFromPrevMaintLibrary_ConsiderExcludeDOW trans = new PrevMaintSched_PMCalendarForecastFromPrevMaintLibrary_ConsiderExcludeDOW()
            {
                CallerUserInfoId = databaseKey.User.UserInfoId,
                CallerUserName = databaseKey.UserName,
            };
            trans.PrevMaintSched = this.ToExtendedForecastDatabaseObject();
            trans.dbKey = databaseKey.ToTransDbKey();
            trans.Execute();
            return Tuple.Create(PrevMaintSched.UpdateFromDBObjectListForForecast(trans.PrevMaintSchedList), trans.timeoutErrors);
        }
        #region V2-712
        public Tuple<List<PrevMaintSched>, string> RetrievePMOnDemandForecastFromPrevMaintLibrary_V2(DatabaseKey databaseKey)
        {
            PrevMaintSched_PMOnDemandForecastFromPrevMaintLibrary_V2 trans = new PrevMaintSched_PMOnDemandForecastFromPrevMaintLibrary_V2()
            {
                CallerUserInfoId = databaseKey.User.UserInfoId,
                CallerUserName = databaseKey.UserName,
            };
            trans.PrevMaintSched = this.ToExtendedForecastDatabaseObject_V2();
            trans.dbKey = databaseKey.ToTransDbKey();
            trans.Execute();
            return Tuple.Create(PrevMaintSched.UpdateFromDBObjectListForForecast_V2(trans.PrevMaintSchedList),trans.timeoutErrors);
        }
        public b_PrevMaintSched ToExtendedForecastDatabaseObject_V2()
        {
            b_PrevMaintSched dbObj = this.ToDatabaseObject();

            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.ClientLookupId = this.ClientLookupId;
            dbObj.Description = this.Description;
            dbObj.PMForeCastId = this.PMForeCastId;
            dbObj.AssignedTo_PersonnelClientLookupId = this.AssignedTo_PersonnelClientLookupId;
            dbObj.ChargeToClientLookupId = this.ChargeToClientLookupId;
            dbObj.AssignedTo_PersonnelName = this.AssignedTo_PersonnelName;
            dbObj.jobDuration = this.jobDuration;
            dbObj.EstLaborHours = this.EstLaborHours;
            dbObj.EstLaborCost = this.EstLaborCost;
            dbObj.EstOtherCost = this.EstOtherCost;
            dbObj.EstMaterialCost = this.EstMaterialCost;
            dbObj.ChargeToName = this.ChargeToName;
            dbObj.AssignedTo_PersonnelId = this.AssignedTo_PersonnelId;
            dbObj.SchedueledDate = this.SchedueledDate;
            dbObj.CurrentDate = this.CurrentDate;
            dbObj.PrevMaintSchedId = this.PrevMaintSchedId;
            dbObj.PMForeCastRequiredDate = this.PMForeCastRequiredDate;
            dbObj.AssignedTo_PersonnelIds = this.AssignedTo_PersonnelIds;
            dbObj.ChildCount = this.ChildCount;
            dbObj.AssignedMultiple = this.AssignedMultiple;
            dbObj.ForecastDownRequired = this.ForecastDownRequired;
            dbObj.Shift = this.Shift;
            dbObj.Type = this.Type; //V2-1207
            return dbObj;
        }
        public static List<PrevMaintSched> UpdateFromDBObjectListForForecast_V2(List<b_PrevMaintSched> dbObjs)
        {
            List<PrevMaintSched> result = new List<PrevMaintSched>();

            foreach (b_PrevMaintSched dbObj in dbObjs)
            {
                PrevMaintSched tmp = new PrevMaintSched();
                tmp.UpdateFromExtendedDatabaseObjectForCalendarForecast_V2(dbObj);
                result.Add(tmp);
            }
            return result;
        }
        public void UpdateFromExtendedDatabaseObjectForCalendarForecast_V2(b_PrevMaintSched dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.SiteId = dbObj.SiteId;
            this.ClientLookupId = dbObj.ClientLookupId;
            this.Description = dbObj.Description;
            this.PMForeCastId = dbObj.PMForeCastId;
            this.AssignedTo_PersonnelClientLookupId = dbObj.AssignedTo_PersonnelClientLookupId;
            this.ChargeToClientLookupId = dbObj.ChargeToClientLookupId;
            this.AssignedTo_PersonnelName = dbObj.AssignedTo_PersonnelName;
            this.Duration = dbObj.Duration;
            this.EstLaborHours = dbObj.EstLaborHours;
            this.EstLaborCost = dbObj.EstLaborCost;
            this.EstOtherCost = dbObj.EstOtherCost;
            this.EstMaterialCost = dbObj.EstMaterialCost;
            this.ChargeToName = dbObj.ChargeToName;
            this.AssignedTo_PersonnelId = dbObj.AssignedTo_PersonnelId;
            this.SchedueledDate = dbObj.SchedueledDate;
            this.CurrentDate = dbObj.CurrentDate;
            this.OnDemandGroup = dbObj.OnDemandGroup;
            this.PrevMaintSchedId = dbObj.PrevMaintSchedId;
            this.RequiredDate = dbObj.RequiredDate;
            this.AssignedTo_PersonnelName = dbObj.AssignedTo_PersonnelName;
            this.ChildCount = dbObj.ChildCount;
            this.AssignedMultiple = dbObj.AssignedMultiple;
            this.DownRequired = dbObj.DownRequired;
            this.Shift = dbObj.Shift;
            this.Type = dbObj.Type; //V2-1207
        }

        #endregion
        #region V2-1005
        public void PrevMaintSchedDelete_V2(DatabaseKey dbKey)
        {
            PrevMaintSched_Delete_V2 trans = new PrevMaintSched_Delete_V2();

            trans.PrevMaintSched = this.ToDeleteDataDatabaseObject();


            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.PrevMaintSched);
        }
        public b_PrevMaintSched ToDeleteDataDatabaseObject()
        {
            b_PrevMaintSched dbObj = new b_PrevMaintSched();
            dbObj.ClientId = this.ClientId;
            dbObj.PrevMaintSchedId = this.PrevMaintSchedId;
            return dbObj;
        }
        #endregion

        #region V2-1212
        public static List<PrevMaintSched> RetrieveByPrevMaintMasterIdChunkSearch_V2(DatabaseKey dbKey, PrevMaintSched pm)
        {
            PrevMaintSched_RetrieveByPrevMaintMasterIdChunkSeaarch_V2 trans = new PrevMaintSched_RetrieveByPrevMaintMasterIdChunkSeaarch_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.PrevMaintSched = pm.ToExtendedDatabaseObjectForChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return PrevMaintSched.UpdateFromDatabaseObjectListForChunkSearch(trans.PrevMaintSchedList);
        }

        public b_PrevMaintSched ToExtendedDatabaseObjectForChunkSearch()
        {
            b_PrevMaintSched dbObj = new b_PrevMaintSched();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.PrevMaintMasterId = this.PrevMaintMasterId;
            dbObj.Offset = this.Offset;
            dbObj.Nextrow = this.Nextrow;
            dbObj.OrderBy = this.OrderBy;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.ChargeToName = this.ChargeToName;
            dbObj.ChargeToClientLookupId = this.ChargeToClientLookupId;
            dbObj.Meter_ClientLookupId = this.Meter_ClientLookupId;
            dbObj.WorkOrder_ClientLookupId = this.WorkOrder_ClientLookupId;
            dbObj.NextDueDate = this.NextDueDate;
            dbObj.FrequencyWithType = this.FrequencyWithType;
            dbObj.LastScheduled = this.LastScheduled;
            dbObj.LastPerformed = this.LastPerformed;
            dbObj.OnDemandGroup = this.OnDemandGroup;

            return dbObj;
        }
        public static List<PrevMaintSched> UpdateFromDatabaseObjectListForChunkSearch(List<b_PrevMaintSched> dbObjs)
        {
            List<PrevMaintSched> result = new List<PrevMaintSched>();

            foreach (b_PrevMaintSched dbObj in dbObjs)
            {
                PrevMaintSched tmp = new PrevMaintSched();
                tmp.UpdateFromExtendedDatabaseObjectForChunkSearch(dbObj);
                result.Add(tmp);
            }
            return result;
        }
        public void UpdateFromExtendedDatabaseObjectForChunkSearch(b_PrevMaintSched dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.PrevMaintSchedId = dbObj.PrevMaintSchedId;
            this.PrevMaintMasterId = dbObj.PrevMaintMasterId;
            this.ChargeToClientLookupId = dbObj.ChargeToClientLookupId;
            this.ChargeToName = dbObj.ChargeToName;
            this.Frequency = dbObj.Frequency;
            this.FrequencyType = dbObj.FrequencyType;
            this.NextDueDate = dbObj.NextDueDate;
            this.Meter_ClientLookupId = dbObj.Meter_ClientLookupId;
            this.WorkOrder_ClientLookupId = dbObj.WorkOrder_ClientLookupId;
            this.LastScheduled = dbObj.LastScheduled;
            this.LastPerformed = dbObj.LastPerformed;
            this.OnDemandGroup = dbObj.OnDemandGroup;
            this.PlanningRequired = dbObj.PlanningRequired;
            this.ChildCount = dbObj.ChildCount;
            this.TotalCount = dbObj.TotalCount;
        }
        #endregion
    }
}
