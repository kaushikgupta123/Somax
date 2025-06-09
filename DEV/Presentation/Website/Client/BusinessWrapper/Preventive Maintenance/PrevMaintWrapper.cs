using Client.BusinessWrapper.Common;
using Client.Common;
using Client.Common.Constants;
using Client.Localization;
using Client.Models;
using Client.Models.Common;
using Client.Models.PreventiveMaintenance;
using Client.Models.PreventiveMaintenance.UIConfiguration;
using Common.Constants;
using Common.Enumerations;
using DataContracts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2018-2019 by SOMAX Inc.
* Client.BusinessWrapper.PrevMaintWrapper
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID Person        Description
* =========== ======= ============= ================================================================
* 2019-Jun-25 V2-206  Roger Lawton  Changed to use the method for the PM Library 
****************************************************************************************************
*/

namespace Client.BusinessWrapper.PrevMaintWrapper
{
    public class PrevMaintWrapper : CommonWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        List<string> errorMessage = new List<string>();

        #region Page Constants
        private const int FILTER_TYPE_NONE = 0;
        private const int FILTER_TYPE_EQUIPMENT = 1;
        private const int FILTER_TYPE_LOCATION = 2;
        private const int FILTER_TYPE_ASSIGNED = 3;
        #endregion

        public PrevMaintWrapper(UserData userData) : base(userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }
        #region Search
        public List<PreventiveMaintenanceModel> populatePrevMaint(bool inactiveFalg, int equipmentId = 0, int locationId = 0, int assignedId = 0)
        {
            PrevMaintMaster prevMaintMaster = new PrevMaintMaster();
            PreventiveMaintenanceModel PrevModel;
            List<PreventiveMaintenanceModel> PartsModelList = new List<PreventiveMaintenanceModel>();

            int filtertype;
            int filtervalue;

            if (equipmentId != 0)  //for equipment search
            {
                filtertype = FILTER_TYPE_EQUIPMENT;
                filtervalue = equipmentId;
            }
            else if (locationId != 0)  //for location search
            {
                filtertype = FILTER_TYPE_LOCATION;
                filtervalue = locationId;
            }
            else if (assignedId != 0)  //for assigned search
            {
                filtertype = FILTER_TYPE_ASSIGNED;
                filtervalue = assignedId;
            }
            else  //without location, equipment or assigned only considering SiteId in search
            {
                filtertype = FILTER_TYPE_NONE;
                filtervalue = 0;
            }
            List<PrevMaintMaster> PrevMaintMasterList = prevMaintMaster.PreventiveMaintenanceRetrieveToSearchCriteria
              (_dbKey, filtertype, filtervalue);

            if (PrevMaintMasterList != null && PrevMaintMasterList.Count > 0)
            {
                PrevMaintMasterList = PrevMaintMasterList.Where(x => x.InactiveFlag.Equals(inactiveFalg)).ToList();
            }

            foreach (var p in PrevMaintMasterList)
            {
                PrevModel = new PreventiveMaintenanceModel();
                PrevModel.PrevMaintMasterId = p.PrevMaintMasterId;
                PrevModel.ClientLookupId = p.ClientLookupId;
                PrevModel.Description = p.Description;
                PrevModel.JobDuration = p.JobDuration;
                PrevModel.ScheduleType = p.ScheduleType;
                PrevModel.Type = p.Type;
                PrevModel.InactiveFlag = p.InactiveFlag;
                PartsModelList.Add(PrevModel);
            }
            return PartsModelList;
        }

        public List<PreventiveMaintenanceModel> populateChunkSearch(PreventiveMaintenanceSearchCriteriaModel pm)
        {
            PrevMaintMaster prevMaintMaster = new PrevMaintMaster();
            PreventiveMaintenanceModel PrevModel;
            List<PreventiveMaintenanceModel> prevMaintModelList = new List<PreventiveMaintenanceModel>();


            if (pm.EquipmentId != 0)  //for equipment search
            {
                prevMaintMaster.FilterType = FILTER_TYPE_EQUIPMENT;
                prevMaintMaster.FilterValue = pm.EquipmentId;
            }
            else if (pm.LocationId != 0)  //for location search
            {
                prevMaintMaster.FilterType = FILTER_TYPE_LOCATION;
                prevMaintMaster.FilterValue = pm.LocationId;
            }
            else if (pm.AssignedId != 0)  //for assigned search
            {
                prevMaintMaster.FilterType = FILTER_TYPE_ASSIGNED;
                prevMaintMaster.FilterValue = pm.AssignedId;
            }
            else  //without location, equipment or assigned only considering SiteId in search
            {
                prevMaintMaster.FilterType = FILTER_TYPE_NONE;
                prevMaintMaster.FilterValue = 0;
            }
            prevMaintMaster.CaseNo = pm.CaseNo;
            prevMaintMaster.orderbyColumn = pm.orderbyColumn;
            prevMaintMaster.offset1 = pm.offset1;
            prevMaintMaster.nextrow = pm.nextrow;
            prevMaintMaster.ClientLookupId = pm.ClientLookupId;
            prevMaintMaster.Description = pm.Description;
            prevMaintMaster.ScheduleType = pm.ScheduleType;
            prevMaintMaster.Type = pm.Type;
            prevMaintMaster.Chargeto = pm.Chargeto;
            prevMaintMaster.ChargetoName = pm.ChargetoName;
            prevMaintMaster.SearchText = pm.SearchText;
            prevMaintMaster.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            prevMaintMaster.orderBy = pm.orderBy;
            List<PrevMaintMaster> PrevMaintMasterList = prevMaintMaster.RetrieveChunkSearch(_dbKey);

            foreach (var p in PrevMaintMasterList)
            {
                PrevModel = new PreventiveMaintenanceModel();
                PrevModel.PrevMaintMasterId = p.PrevMaintMasterId;
                PrevModel.ClientLookupId = p.ClientLookupId;
                PrevModel.Description = p.Description;
                PrevModel.JobDuration = p.JobDuration;
                PrevModel.ScheduleType = p.ScheduleType;
                PrevModel.Type = p.Type;
                PrevModel.InactiveFlag = p.InactiveFlag;
                PrevModel.TotalCount = p.TotalCount;
                PrevModel.ChildCount = p.ChildCount;
                prevMaintModelList.Add(PrevModel);
            }
            return prevMaintModelList;
        }
        #endregion

        #region Add-Edit
        public PrevMaintMaster AddPrevMaint(PreventiveMaintenanceModel _PrevMaintModel)
        {
            PrevMaintMaster prevMaintMaster = new PrevMaintMaster()
            {
                ClientId = _dbKey.Client.ClientId,
                SiteId = _dbKey.User.DefaultSiteId
            };
            prevMaintMaster.ClientLookupId = _PrevMaintModel.ClientLookupId;
            prevMaintMaster.ScheduleType = _PrevMaintModel.ScheduleType;
            prevMaintMaster.Description = _PrevMaintModel.Description;
            prevMaintMaster.JobDuration = _PrevMaintModel.JobDuration ?? 0;
            prevMaintMaster.Type = _PrevMaintModel.Type;
            prevMaintMaster.InactiveFlag = _PrevMaintModel.InactiveFlag;

            prevMaintMaster.ValidateAdd(userData.DatabaseKey);
            if (prevMaintMaster.ErrorMessages != null && prevMaintMaster.ErrorMessages.Count == 0)
            {
                prevMaintMaster.Create(_dbKey);
            }

            return prevMaintMaster;
        }
        public PrevMaintMaster EditPrevMaint(PreventiveMaintenanceModel _PrevMaintModel)
        {
            PrevMaintMaster prevMain = new PrevMaintMaster()
            {
                ClientId = _dbKey.Client.ClientId,
                PrevMaintMasterId = _PrevMaintModel.PrevMaintMasterId
            };
            prevMain.Retrieve(_dbKey);

            prevMain.ScheduleType = _PrevMaintModel?.ScheduleType ?? string.Empty;
            prevMain.Description = _PrevMaintModel?.Description ?? string.Empty;
            prevMain.JobDuration = _PrevMaintModel?.JobDuration ?? 0;
            prevMain.Type = _PrevMaintModel?.Type ?? string.Empty;
            prevMain.InactiveFlag = _PrevMaintModel?.InactiveFlag ?? false;
            prevMain.Update(_dbKey);
            return prevMain;

        }
        public List<String> DeletePM(long _PMid)
        {
            List<string> EMsg = new List<string>();
            if (_PMid > 0)
            {
                PrevMaintMaster master = new PrevMaintMaster();
                master.PrevMaintMasterId = _PMid;
                master.ClientId = userData.DatabaseKey.Personnel.ClientId;
                master.DeletePreventiveMaintenanceMasterDetails(userData.DatabaseKey);
                if (master.ErrorMessages != null && master.ErrorMessages.Count > 0)
                {
                    EMsg = master.ErrorMessages;
                }
            }
            return EMsg;
        }
        #endregion

        #region Details 
        public PreventiveMaintenanceModel populateMaintenanceDetails(long prevMaintMasterId)
        {
            PreventiveMaintenanceModel objPrev = new PreventiveMaintenanceModel();
            DataContracts.PrevMaintLibrary prevMaintLibrary = new DataContracts.PrevMaintLibrary();
            PrevMaintMaster obj = new PrevMaintMaster()
            {
                ClientId = _dbKey.Client.ClientId,
                SiteId = _dbKey.User.DefaultSiteId,
                PrevMaintMasterId = prevMaintMasterId
            };
            obj.Retrieve(userData.DatabaseKey);
            objPrev = initializeControls(obj);
            if (objPrev != null && objPrev.PrevMaintLibraryId > 0)
            {
                prevMaintLibrary.PrevMaintLibraryId = objPrev.PrevMaintLibraryId;
                prevMaintLibrary.Retrieve(userData.DatabaseKey);
                objPrev.PrevMaintLibraryId = prevMaintLibrary.PrevMaintLibraryId;
                objPrev.ClientLookupId = prevMaintLibrary.ClientLookupId;
                objPrev.Description = prevMaintLibrary.Description;
                objPrev.DownRequired = Convert.ToString(prevMaintLibrary.DownRequired);
                objPrev.FrequencyType = prevMaintLibrary.FrequencyType;
                objPrev.JobDuration = prevMaintLibrary.JobDuration;
                objPrev.Type = prevMaintLibrary.Type;
                objPrev.ScheduleMethod = prevMaintLibrary.ScheduleMethod;
                objPrev.ScheduleType = prevMaintLibrary.ScheduleType;
                objPrev.InactiveFlag = prevMaintLibrary.InactiveFlag;
            }

            return objPrev;
        }


        public PreventiveMaintenanceModel initializeControls(PrevMaintMaster obj)
        {
            PreventiveMaintenanceModel objPrev = new PreventiveMaintenanceModel();

            objPrev.PrevMaintMasterId = obj.PrevMaintMasterId;
            objPrev.ClientLookupId = obj?.ClientLookupId ?? string.Empty;
            objPrev.ScheduleType = obj?.ScheduleType ?? string.Empty;
            objPrev.Description = obj?.Description ?? string.Empty;
            objPrev.JobDuration = obj?.JobDuration ?? 0;
            objPrev.Type = obj?.Type ?? string.Empty;
            objPrev.InactiveFlag = obj?.InactiveFlag ?? false;
            objPrev.PrevMaintLibraryId = obj.PrevMaintLibraryId;
            return objPrev;
        }
        public CreatedLastUpdatedPMModel createdLastUpdatedModel(long _PMId)
        {
            CreatedLastUpdatedPMModel _CreatedLastUpdatedModel = new CreatedLastUpdatedPMModel();
            DataContracts.PrevMaintMaster prevMaintMaster = new DataContracts.PrevMaintMaster();
            prevMaintMaster.PrevMaintMasterId = _PMId;
            prevMaintMaster.RetrieveByForeignKey(this.userData.DatabaseKey);
            _CreatedLastUpdatedModel.CreatedDateValue = prevMaintMaster.CreateDate.ToString();
            _CreatedLastUpdatedModel.CreatedUserValue = !string.IsNullOrEmpty(prevMaintMaster.CreateBy) ? prevMaintMaster.CreateBy.ToString() : null;
            _CreatedLastUpdatedModel.ModifyUserValue = !string.IsNullOrEmpty(prevMaintMaster.ModifyBy) ? prevMaintMaster.ModifyBy.ToString() : null;
            _CreatedLastUpdatedModel.ModifyDatevalue = prevMaintMaster.ModifyDate.ToString() == "0001-01-01 00:00:00.0000000" ? "" : prevMaintMaster.ModifyDate.ToString();
            return _CreatedLastUpdatedModel;
        }

        #endregion

        #region ScheduleRecord
        public List<ScheduleRecords> GetScheduleRecords(long PrevMasterID)
        {
            List<ScheduleRecords> ScheduleRecordsList = new List<ScheduleRecords>();
            ScheduleRecords objScheduleRecords;

            PrevMaintSched pms = new PrevMaintSched()
            {
                PrevMaintMasterId = PrevMasterID,
                SiteId = userData.DatabaseKey.User.DefaultSiteId
            };
            List<PrevMaintSched> pmsList = PrevMaintSched.RetrieveByPrevMaintMasterId(userData.DatabaseKey, pms);

            foreach (var pm in pmsList)
            {
                objScheduleRecords = new ScheduleRecords();
                objScheduleRecords.PrevMaintScheId = pm.PrevMaintSchedId;
                objScheduleRecords.ChargeToClientLookupId = pm.ChargeToClientLookupId;
                objScheduleRecords.ChargeToName = pm.ChargeToName;
                objScheduleRecords.AssignedTo_PersonnelClientLookupId = pm.AssignedTo_PersonnelClientLookupId;
                objScheduleRecords.Meter_ClientLookupId = pm.Meter_ClientLookupId;
                objScheduleRecords.Frequency = pm.Frequency;
                objScheduleRecords.FrequencyType = pm.FrequencyType;
                if (pm.LastPerformed != null && pm.LastPerformed == default(DateTime))
                {
                    objScheduleRecords.LastPerformed = null;
                }
                else
                {
                    objScheduleRecords.LastPerformed = pm.LastPerformed;
                }

                if (pm.NextDueDate != null && pm.NextDueDate == default(DateTime))
                {
                    objScheduleRecords.NextDueDate = null;
                    objScheduleRecords.NextDueDateString = string.Empty;
                }
                else
                {
                    objScheduleRecords.NextDueDate = pm.NextDueDate;
                    objScheduleRecords.NextDueDateString = pm.NextDueDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                objScheduleRecords.Last_WorkOrderId = pm.Last_WorkOrderId;
                objScheduleRecords.OnDemandGroup = pm.OnDemandGroup;
                ScheduleRecordsList.Add(objScheduleRecords);
            }
            return ScheduleRecordsList;
        }
        public List<ScheduleRecords> GetScheduleRecords_V2(long PrevMasterID)
        {
            List<ScheduleRecords> ScheduleRecordsList = new List<ScheduleRecords>();
            ScheduleRecords objScheduleRecords;

            PrevMaintSched pms = new PrevMaintSched()
            {
                PrevMaintMasterId = PrevMasterID,
                SiteId = userData.DatabaseKey.User.DefaultSiteId
            };
            List<PrevMaintSched> pmsList = PrevMaintSched.RetrieveByPrevMaintMasterId_V2(userData.DatabaseKey, pms);
            var FrequencyTypeList = UtilityFunction.populateFrequencyTypeList();
            foreach (var pm in pmsList)
            {
                objScheduleRecords = new ScheduleRecords();
                objScheduleRecords.ChargeType = pm.ChargeType;
                objScheduleRecords.Scheduled = pm.Scheduled;
                objScheduleRecords.PrevMaintScheId = pm.PrevMaintSchedId;
                objScheduleRecords.ChargeToClientLookupId = pm.ChargeToClientLookupId;
                objScheduleRecords.ChargeToName = pm.ChargeToName;
                objScheduleRecords.AssignedTo_PersonnelClientLookupId = pm.AssignedTo_PersonnelClientLookupId;
                objScheduleRecords.Meter_ClientLookupId = pm.Meter_ClientLookupId;
                objScheduleRecords.Frequency = pm.Frequency;
                objScheduleRecords.FrequencyType = pm.FrequencyType;
                //V2-842
                //V2-842 - RKL - 2023-May-02
                // If this is a meter or OnDemand then the frequency type is filled in by 
                //  Meter - Meter.ClientLookupid
                //  On-Demand - OnDemandGroup
                // These values will NOT be in the Frequency type lookup list
                if (!string.IsNullOrWhiteSpace(objScheduleRecords.FrequencyType))
                {
                    string ft = FrequencyTypeList.FirstOrDefault(x => x.value == objScheduleRecords.FrequencyType)?.text;
                    if (string.IsNullOrWhiteSpace(ft))
                    {
                        ft = objScheduleRecords.FrequencyType;
                    }
                    objScheduleRecords.FrequencyType = ft;
                }

                if (pm.LastPerformed != null && pm.LastPerformed == default(DateTime))
                {
                    objScheduleRecords.LastPerformed = null;
                }
                else
                {
                    objScheduleRecords.LastPerformed = pm.LastPerformed;
                }

                if (pm.NextDueDate != null && pm.NextDueDate == default(DateTime))
                {
                    objScheduleRecords.NextDueDate = null;
                    objScheduleRecords.NextDueDateString = string.Empty;
                }
                else
                {
                    objScheduleRecords.NextDueDate = pm.NextDueDate;
                    objScheduleRecords.NextDueDateString = pm.NextDueDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                objScheduleRecords.Last_WorkOrderId = pm.Last_WorkOrderId;
                objScheduleRecords.OnDemandGroup = pm.OnDemandGroup;
                //V2-758
                objScheduleRecords.WorkOrder_ClientLookupId = pm.WorkOrder_ClientLookupId;
                //V2-842
                if (pm.LastScheduled != null && pm.LastScheduled == default(DateTime))
                {
                    objScheduleRecords.LastScheduled = null;
                }
                else
                {
                    objScheduleRecords.LastScheduled = pm.LastScheduled;
                }
                objScheduleRecords.ChildCount = pm.ChildCount; //V2-712
                objScheduleRecords.PlanningRequired = pm.PlanningRequired; //V2-1161

                ScheduleRecordsList.Add(objScheduleRecords);
            }
            return ScheduleRecordsList;
        }
        public ScheduleRecords PoupalateScheduleDetailsbyPK(long PrevMaintMasterId, long PrevMaintScheId)
        {
            ScheduleRecords objSch = new ScheduleRecords();
            PrevMaintSched prevmaintsched = new PrevMaintSched()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                PrevMaintMasterId = PrevMaintMasterId,
                PrevMaintSchedId = PrevMaintScheId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId
            };
            prevmaintsched.RetrieveByForeignKeys_V2(userData.DatabaseKey);
            objSch = initializeScheduleControls(prevmaintsched);
            Equipment equipment = new Equipment
            {
                ClientId = _dbKey.Client.ClientId,
                SiteId = _dbKey.User.DefaultSiteId,
                ClientLookupId = prevmaintsched.ChargeToClientLookupId
            };
            objSch.ExclusionDaysString = GetExclusionDaysArray(objSch.ExcludeDOW);
            equipment.RetrieveByClientLookupId(_dbKey);
            objSch.ChargeToIdStatus = equipment.InactiveFlag;
            return objSch;
        }
        private string[] GetExclusionDaysArray(string days)
        {
            var daysCharArray = days.ToCharArray();
            List<string> tmpList = new List<string>();
            for (int i = 0; i < daysCharArray.Count(); i++)
            {
                if (daysCharArray[i] == '1')
                {
                    tmpList.Add(i.ToString());
                }
            }
            return tmpList.ToArray();
        }
        public PrevMaintLibrary GetPrevMaintSchedFromLibray(long PrevMaintLibraryId)
        {
            PrevMaintLibrary pms = new PrevMaintLibrary()
            {
                PrevMaintLibraryId = PrevMaintLibraryId,
                ClientId = userData.DatabaseKey.Client.ClientId
            };
            pms.Retrieve(userData.DatabaseKey);
            return pms;
        }
        public PrevMaintSched EditPrevMaintSched(ScheduleRecords _scheduleRecords)
        {
            string ExclusionDays = "0000000";
            PrevMaintWrapper _PrevMaintObj = new PrevMaintWrapper(userData);
            PrevMaintSched prevmaintsched = new PrevMaintSched()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                PrevMaintMasterId = _scheduleRecords.PrevMaintMasterId,
                PrevMaintSchedId = _scheduleRecords.PrevMaintScheId
            };

            prevmaintsched.RetrieveByForeignKeys_V2(userData.DatabaseKey);

            prevmaintsched.ChargeToClientLookupId = _scheduleRecords?.ChargeToClientLookupId ?? string.Empty;
            prevmaintsched.ScheduleMethod = _scheduleRecords?.ScheduleMethod ?? string.Empty;
            if (_scheduleRecords.PrevMaintLibraryID > 0)
            {
                var allFreqTypes = UtilityFunction.populateFrequencyTypeList();
                if (allFreqTypes.Any(x => x.text.Equals(_scheduleRecords?.FrequencyType)))
                {
                    prevmaintsched.FrequencyType = allFreqTypes.Where(x => x.text.Equals(_scheduleRecords?.FrequencyType)).FirstOrDefault().value;
                }
                else
                {
                    prevmaintsched.FrequencyType = _scheduleRecords?.FrequencyType ?? string.Empty;
                }
                if (!string.IsNullOrEmpty(_scheduleRecords.ChargeType))
                {
                    var ScheduleChargeTypeList = UtilityFunction.populateChargeType();
                    if (ScheduleChargeTypeList != null && ScheduleChargeTypeList.Count() > 0)
                    {
                        prevmaintsched.ChargeType = ScheduleChargeTypeList.Where(x => x.value.Equals(_scheduleRecords?.ChargeType)).FirstOrDefault().value;
                    }
                }
                else
                {
                    prevmaintsched.ChargeType = _scheduleRecords.ChargeType;
                }
                if (!string.IsNullOrEmpty(_scheduleRecords.OnDemandGroup))
                {
                    var AllLookUp = _PrevMaintObj.GetAllLookUpList();
                    if (AllLookUp != null)
                    {
                        var typeList = AllLookUp.Where(x => x.ListName == LookupListConstants.Preventive_Maint_Ondemand_Grp).ToList();
                        if (typeList != null && typeList.Count() > 0)
                        {
                            prevmaintsched.OnDemandGroup = typeList.Where(x => x.ListValue.Equals(_scheduleRecords?.OnDemandGroup)).FirstOrDefault().ListValue;
                        }
                    }
                }
                else
                {
                    prevmaintsched.OnDemandGroup = _scheduleRecords?.OnDemandGroup ?? string.Empty;
                }
            }
            else
            {
                prevmaintsched.FrequencyType = _scheduleRecords?.FrequencyType ?? string.Empty;
                prevmaintsched.OnDemandGroup = _scheduleRecords?.OnDemandGroup ?? string.Empty;
                prevmaintsched.ChargeType = _scheduleRecords.ChargeType;
            }

            prevmaintsched.Frequency = _scheduleRecords?.Frequency ?? 0;
            prevmaintsched.CalendarSlack = _scheduleRecords?.CalendarSlack ?? 0;
            prevmaintsched.NextDueDate = _scheduleRecords.NextDueDate;
            prevmaintsched.InactiveFlag = _scheduleRecords?.InactiveFlag ?? false;
            prevmaintsched.Scheduled = _scheduleRecords?.Scheduled ?? false;
            prevmaintsched.DownRequired = _scheduleRecords?.DownRequired ?? false;
            prevmaintsched.MeterId = _scheduleRecords?.MeterId ?? 0;
            prevmaintsched.MeterInterval = _scheduleRecords?.MeterInterval ?? 0;
            prevmaintsched.MeterSlack = _scheduleRecords?.MeterSlack ?? 0;
            prevmaintsched.MeterMethod = _scheduleRecords?.MeterMethod ?? string.Empty;
            prevmaintsched.MeterLastDue = _scheduleRecords?.MeterLastDue ?? 0;
            prevmaintsched.MeterLastDone = _scheduleRecords?.MeterLastDone ?? 0;
            prevmaintsched.AssignedTo_PersonnelId = _scheduleRecords?.AssignedTo_PersonnelId ?? 0;
            prevmaintsched.Type = _scheduleRecords?.Type ?? string.Empty;
            prevmaintsched.Section = _scheduleRecords?.Section ?? string.Empty;
            prevmaintsched.Priority = _scheduleRecords?.Priority ?? string.Empty;
            prevmaintsched.Category = _scheduleRecords?.Category ?? string.Empty;
            prevmaintsched.Shift = _scheduleRecords?.Shift ?? string.Empty;
            prevmaintsched.Planner_PersonnelId = _scheduleRecords?.Planner_PersonnelId ?? 0;
            prevmaintsched.FailureCode = _scheduleRecords?.FailureCode ?? string.Empty;
            prevmaintsched.ActionCode = _scheduleRecords?.ActionCode ?? string.Empty;
            prevmaintsched.RootCauseCode = _scheduleRecords?.RootCauseCode ?? string.Empty;

            if (_scheduleRecords.ExclusionDaysString != null)
            {
                ExclusionDays = GetExclusionDaysString(_scheduleRecords.ExclusionDaysString);

            }
            prevmaintsched.ExcludeDOW = ExclusionDays;
            prevmaintsched.ValidateSave(userData.DatabaseKey);

            if (prevmaintsched.ErrorMessages.Count != 0)
            {
                if (prevmaintsched.ErrorMessages[0].ToString() != ErrorMessageConstants.Schedule_Record_Exists)
                {
                    return prevmaintsched;
                }
                else
                {
                    prevmaintsched.UpdateByForeignKeys_V2(userData.DatabaseKey);
                }
            }
            else
            {
                prevmaintsched.UpdateByForeignKeys_V2(userData.DatabaseKey);
            }
            return prevmaintsched;
        }
        private string GetExclusionDaysString(string[] days)
        {
            var defaultExclusiondays = "0000000";
            char[] array = defaultExclusiondays.ToCharArray();
            if (days != null && days.Length > 0)
            {
                foreach (var day in days)
                {
                    array[Convert.ToInt32(day)] = '1';
                }
            }
            return new string(array);
        }
        public PrevMaintSched AddPrevMaintSched(ScheduleRecords _scheduleRecords)
        {
            PrevMaintSched prevmaintsched = new PrevMaintSched()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                PrevMaintMasterId = _scheduleRecords.PrevMaintMasterId
            };
            prevmaintsched.ChargeType = _scheduleRecords.ChargeType;
            prevmaintsched.ChargeToClientLookupId = _scheduleRecords?.ChargeToClientLookupId ?? string.Empty;
            prevmaintsched.ChargeToName = _scheduleRecords?.ChargeToName ?? string.Empty;
            if (_scheduleRecords.PrevMaintLibraryID > 0)
            {
                var allFreqTypes = UtilityFunction.populateFrequencyTypeList();
                if (allFreqTypes.Any(x => x.text.Equals(_scheduleRecords?.FrequencyType)))
                {
                    prevmaintsched.FrequencyType = allFreqTypes.Where(x => x.text.Equals(_scheduleRecords?.FrequencyType)).FirstOrDefault().value;
                }
                else
                {
                    prevmaintsched.FrequencyType = _scheduleRecords?.FrequencyType ?? string.Empty;
                }
                var ScheduleMethodTypes = UtilityFunction.populateScheduleMethodList();
                if (ScheduleMethodTypes.Any(x => x.text.Equals(_scheduleRecords?.ScheduleMethod)))
                {
                    prevmaintsched.ScheduleMethod = ScheduleMethodTypes.Where(x => x.text.Equals(_scheduleRecords?.ScheduleMethod)).FirstOrDefault().value;
                }
                else
                {
                    prevmaintsched.ScheduleMethod = _scheduleRecords?.ScheduleMethod ?? string.Empty;
                }
            }
            else
            {
                prevmaintsched.FrequencyType = _scheduleRecords?.FrequencyType ?? string.Empty;
                prevmaintsched.ScheduleMethod = _scheduleRecords?.ScheduleMethod ?? string.Empty;
            }
            prevmaintsched.Frequency = _scheduleRecords?.Frequency ?? 0;
            prevmaintsched.CalendarSlack = _scheduleRecords?.CalendarSlack ?? 0;
            prevmaintsched.NextDueDate = _scheduleRecords.NextDueDate;
            prevmaintsched.OnDemandGroup = _scheduleRecords?.OnDemandGroup ?? string.Empty;
            prevmaintsched.InactiveFlag = _scheduleRecords?.InactiveFlag ?? false;
            prevmaintsched.LastPerformed = _scheduleRecords?.LastPerformed;
            prevmaintsched.LastScheduled = _scheduleRecords?.LastScheduled;
            prevmaintsched.Scheduled = _scheduleRecords?.Scheduled ?? false;
            prevmaintsched.DownRequired = _scheduleRecords?.DownRequired ?? false;
            prevmaintsched.WorkOrder_ClientLookupId = _scheduleRecords?.WorkOrder_ClientLookupId ?? string.Empty;
            prevmaintsched.CurrentWOComplete = _scheduleRecords?.CurrentWOComplete;
            prevmaintsched.MeterId = _scheduleRecords?.MeterId ?? 0;
            prevmaintsched.MeterInterval = _scheduleRecords?.MeterInterval ?? 0;
            prevmaintsched.MeterSlack = _scheduleRecords?.MeterSlack ?? 0;
            prevmaintsched.MeterMethod = _scheduleRecords?.MeterMethod ?? string.Empty;
            prevmaintsched.MeterLastDue = _scheduleRecords?.MeterLastDue ?? 0;
            prevmaintsched.MeterLastDone = _scheduleRecords?.MeterLastDone ?? 0;
            prevmaintsched.AssignedTo_PersonnelId = _scheduleRecords?.AssignedTo_PersonnelId ?? 0;
            prevmaintsched.Type = _scheduleRecords?.Type ?? string.Empty;
            prevmaintsched.Section = _scheduleRecords?.Section ?? string.Empty;
            prevmaintsched.Priority = _scheduleRecords?.Priority ?? string.Empty;
            prevmaintsched.Category = _scheduleRecords?.Category ?? string.Empty;
            prevmaintsched.Shift = _scheduleRecords?.Shift ?? string.Empty;
            prevmaintsched.Planner_PersonnelId = _scheduleRecords?.Planner_PersonnelId ?? 0;
            prevmaintsched.FailureCode = _scheduleRecords?.FailureCode ?? string.Empty;
            prevmaintsched.ActionCode = _scheduleRecords?.ActionCode ?? string.Empty;
            prevmaintsched.RootCauseCode = _scheduleRecords?.RootCauseCode ?? string.Empty;
            if (_scheduleRecords.ExclusionDaysString != null)
            {
                string ExclusionDays = GetExclusionDaysString(_scheduleRecords.ExclusionDaysString);
                prevmaintsched.ExcludeDOW = ExclusionDays;
            }

            prevmaintsched.ValidateAdd(userData.DatabaseKey);

            if (prevmaintsched.ErrorMessages.Count != 0)
            {   //SOM-1244  Multiple row for same chargeTo entry in schedule records
                if (prevmaintsched.ErrorMessages[0].ToString() != ErrorMessageConstants.Schedule_Record_Exists)
                {
                    return prevmaintsched;
                }
                else
                {
                    prevmaintsched.CreateByForeignKeys_V2(userData.DatabaseKey);
                }
            }
            else
            {
                prevmaintsched.CreateByForeignKeys_V2(userData.DatabaseKey);
            }
            return prevmaintsched;
        }
        public bool DeletePrevMaintSched(long prevMaintScheId)
        {
            try
            {
                PrevMaintSched pms = new PrevMaintSched()
                {
                    PrevMaintSchedId = prevMaintScheId,
                    ClientId = userData.DatabaseKey.Client.ClientId
                };
                pms.PrevMaintSchedDelete_V2(userData.DatabaseKey);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ScheduleRecords initializeScheduleControls(PrevMaintSched obj)
        {
            ScheduleRecords objPrev = new ScheduleRecords();

            objPrev.BusinessType = this.userData.DatabaseKey.Client.BusinessType.ToLower();
            objPrev.PrevMaintMasterId = obj.PrevMaintMasterId;
            objPrev.PrevMaintScheId = obj.PrevMaintSchedId;
            objPrev.ChargeType = obj.ChargeType;
            objPrev.ChargeToClientLookupId = obj?.ChargeToClientLookupId ?? string.Empty;
            objPrev.ChargeToName = obj?.ChargeToName ?? string.Empty;
            objPrev.ScheduleMethod = obj?.ScheduleMethod ?? string.Empty;
            objPrev.FrequencyType = obj?.FrequencyType ?? string.Empty;
            objPrev.Frequency = obj?.Frequency ?? 0;
            objPrev.CalendarSlack = obj?.CalendarSlack ?? 0;
            objPrev.NextDueDate = obj.NextDueDate;
            objPrev.OnDemandGroup = obj?.OnDemandGroup ?? string.Empty;
            objPrev.InactiveFlag = obj?.InactiveFlag ?? false;
            objPrev.LastPerformed = obj?.LastPerformed;
            objPrev.LastScheduled = obj?.LastScheduled;
            objPrev.Scheduled = obj?.Scheduled ?? false;
            objPrev.DownRequired = obj?.DownRequired ?? false;
            objPrev.WorkOrder_ClientLookupId = obj?.WorkOrder_ClientLookupId ?? string.Empty;
            objPrev.CurrentWOComplete = obj?.CurrentWOComplete;
            objPrev.MeterId = obj?.MeterId ?? 0;
            objPrev.MeterInterval = obj?.MeterInterval ?? 0;
            objPrev.MeterSlack = obj?.MeterSlack ?? 0;
            objPrev.MeterMethod = obj?.MeterMethod ?? string.Empty;
            objPrev.MeterLastDue = obj?.MeterLastDue ?? 0;
            objPrev.MeterLastDone = obj?.MeterLastDone ?? 0;
            objPrev.AssignedTo_PersonnelId = obj?.AssignedTo_PersonnelId ?? 0;
            objPrev.Type = obj?.Type ?? string.Empty;
            objPrev.Section = obj?.Section ?? string.Empty;
            objPrev.Priority = obj?.Priority ?? string.Empty;
            objPrev.Category = obj?.Category ?? string.Empty;
            objPrev.Shift = obj?.Shift ?? string.Empty;
            objPrev.Planner_PersonnelId = obj?.Planner_PersonnelId ?? 0;
            objPrev.FailureCode = obj?.FailureCode ?? string.Empty;
            objPrev.ActionCode = obj?.ActionCode ?? string.Empty;
            objPrev.RootCauseCode = obj?.RootCauseCode ?? string.Empty;
            objPrev.ExcludeDOW = obj?.ExcludeDOW ?? string.Empty;

            return objPrev;
        }
        #endregion

        #region Task
        public List<PrevMaintTaskModel> populateTaskList(long PrevMaintMasterId, long prevMaintLibraryId)
        {
            PrevMaintTaskModel prevMaintTaskModel;
            List<PrevMaintTaskModel> prevMaintTaskModelList = new List<PrevMaintTaskModel>();
            if (prevMaintLibraryId == 0)
            {
                PrevMaintTask prevMaintTask = new PrevMaintTask();
                List<PrevMaintTask> prevMaintTasklist = prevMaintTask.PrevMaintTaskRetrieveByPrevMaintMasterId(userData.DatabaseKey, PrevMaintMasterId);
                foreach (var p in prevMaintTasklist)
                {
                    prevMaintTaskModel = new PrevMaintTaskModel();
                    prevMaintTaskModel.PrevMaintMasterId = p.PrevMaintMasterId;
                    prevMaintTaskModel.PrevMaintTaskId = p.PrevMaintTaskId;
                    prevMaintTaskModel.TaskNumber = p.TaskNumber;
                    prevMaintTaskModel.Description = p.Description;
                    prevMaintTaskModel.ChargeType = p.ChargeType;
                    prevMaintTaskModel.ChargeToClientLookupId = p.ChargeToClientLookupId;
                    prevMaintTaskModelList.Add(prevMaintTaskModel);
                }
            }
            else if (prevMaintLibraryId > 0)
            {
                DataContracts.PrevMaintLibraryTask task = new DataContracts.PrevMaintLibraryTask();
                List<PrevMaintLibraryTask> listTask = new List<PrevMaintLibraryTask>();
                task.ClientId = userData.DatabaseKey.Client.ClientId;
                task.PrevMaintLibraryId = prevMaintLibraryId;
                listTask = task.RetrieveAllTaskByPrevMaintLibraryId(userData.DatabaseKey);
                foreach (var p in listTask)
                {
                    prevMaintTaskModel = new PrevMaintTaskModel();
                    prevMaintTaskModel.TaskNumber = p.TaskId;
                    prevMaintTaskModel.Description = p.Description;
                    prevMaintTaskModelList.Add(prevMaintTaskModel);
                }
            }

            return prevMaintTaskModelList;
        }
        public PrevMaintTask addPrevMaintTask(PrevMaintTaskModel objPrevMaintTask)
        {
            PrevMaintTask pmtask = new PrevMaintTask()
            {
                PrevMaintMasterId = objPrevMaintTask.PrevMaintMasterId,
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId
            };

            pmtask.CreateByForeignKeys(userData.DatabaseKey);
            return pmtask;
        }

        public PrevMaintTask AddOrUpdatePrev(PrevMaintVM prevMaintVM, ref List<String> errorList, out string Mode)
        {
            PrevMaintTask pmtask = new PrevMaintTask()
            {
                PrevMaintMasterId = prevMaintVM.prevMaintTaskModel.PrevMaintMasterId,
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                Description = prevMaintVM.prevMaintTaskModel.Description,
                TaskNumber = prevMaintVM.prevMaintTaskModel.TaskNumber,
                ChargeType = prevMaintVM.prevMaintTaskModel.ChargeType,
                ChargeToClientLookupId = prevMaintVM.prevMaintTaskModel.ChargeToClientLookupId,
                PrevMaintTaskId = prevMaintVM.prevMaintTaskModel.PrevMaintTaskId
            };

            if (prevMaintVM.prevMaintTaskModel.PrevMaintTaskId == 0)
            {
                Mode = "add";
                pmtask.ValidateAdd(userData.DatabaseKey);

                if (pmtask.ErrorMessages != null && pmtask.ErrorMessages.Count > 0)
                {
                    errorList = pmtask.ErrorMessages;
                }
                else
                {
                    pmtask.CreateByForeignKeys(userData.DatabaseKey);
                }
            }
            else
            {
                Mode = "update";
                pmtask.ValidateSave(userData.DatabaseKey);
                if (pmtask.ErrorMessages != null && pmtask.ErrorMessages.Count != 0)
                {
                    errorList = pmtask.ErrorMessages;
                }
                else
                {
                    pmtask.UpdateByForeignKeys(userData.DatabaseKey);
                }
            }
            return pmtask;
        }

        public PrevMaintTaskModel GaetTask(long PrevMasterID, long _taskId, string ClientLookupId)
        {
            PrevMaintTaskModel returnPrevTask = new PrevMaintTaskModel();

            PrevMaintTask pmTask = new PrevMaintTask()
            {
                PrevMaintTaskId = _taskId,
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId
            };
            pmTask.RetrieveExtendedInformation(userData.DatabaseKey);
            returnPrevTask.PrevMaintTaskId = _taskId;
            returnPrevTask.Description = pmTask.Description;
            returnPrevTask.TaskNumber = pmTask.TaskNumber;
            returnPrevTask.ChargeType = pmTask.ChargeType;
            returnPrevTask.PrevMaintMasterId = pmTask.PrevMaintMasterId;
            returnPrevTask.ChargeToClientLookupId = pmTask.ChargeToClientLookupId;
            returnPrevTask.PrevmaintClientlookUp = ClientLookupId;

            return returnPrevTask;
        }
        public bool DeletePrevMaintTask(long taskNumber)
        {
            try
            {
                PrevMaintTask pmtask = new PrevMaintTask()
                {
                    PrevMaintTaskId = taskNumber
                };

                pmtask.Delete(userData.DatabaseKey);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Estimated Grid Population
        public List<EstimatedCostModel> populateEstimatedParts(long PrevMaintMasterId)
        {
            EstimatedCostModel estimatedCostModel;
            List<EstimatedCostModel> estimatedCostModelList = new List<EstimatedCostModel>();

            EstimatedCosts estimatecost = new EstimatedCosts()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ObjectId = PrevMaintMasterId,
                ObjectType = SearchCategoryConstants.TBL_PREVMAINTMASTER,
                Category = "Parts"
            };
            List<EstimatedCosts> list = estimatecost.RetriveByObjectId(this.userData.DatabaseKey);
            foreach (var ec in list)
            {
                estimatedCostModel = new EstimatedCostModel();
                estimatedCostModel.EstimatedCostsId = ec.EstimatedCostsId;
                estimatedCostModel.ClientLookupId = ec.ClientLookupId;
                estimatedCostModel.Description = ec.Description;
                estimatedCostModel.UnitCost = ec.UnitCost;
                estimatedCostModel.Quantity = ec.Quantity;
                estimatedCostModel.TotalCost = ec.TotalCost;
                estimatedCostModel.CategoryId = ec.CategoryId;
                estimatedCostModelList.Add(estimatedCostModel);
            }

            return estimatedCostModelList;
        }
        public List<EstimatedCostModel> populateEstimatedLabors(long PrevMaintMasterId)
        {
            EstimatedCostModel estimatedCostModel;
            List<EstimatedCostModel> estimatedCostModelList = new List<EstimatedCostModel>();

            EstimatedCosts estimatecost = new EstimatedCosts()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ObjectId = PrevMaintMasterId,
                ObjectType = SearchCategoryConstants.TBL_PREVMAINTMASTER,
                Category = SearchCategoryConstants.TBL_CRAFT
            };
            List<EstimatedCosts> list = estimatecost.RetriveByObjectId(this.userData.DatabaseKey);
            foreach (var ec in list)
            {
                estimatedCostModel = new EstimatedCostModel();
                estimatedCostModel.EstimatedCostsId = ec.EstimatedCostsId;
                estimatedCostModel.ClientLookupId = ec.ClientLookupId;
                estimatedCostModel.Description = ec.Description;
                estimatedCostModel.UnitCost = ec.UnitCost;
                estimatedCostModel.Quantity = ec.Quantity;
                estimatedCostModel.Duration = ec.Duration;
                estimatedCostModel.TotalCost = ec.TotalCost;
                estimatedCostModelList.Add(estimatedCostModel);
            }

            return estimatedCostModelList;
        }
        public List<EstimatedCostModel> populateEstimatedOthers(long PrevMaintMasterId)
        {
            EstimatedCostModel estimatedCostModel;
            List<EstimatedCostModel> estimatedCostModelList = new List<EstimatedCostModel>();

            EstimatedCosts estimatecost = new EstimatedCosts()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ObjectId = PrevMaintMasterId,
                ObjectType = SearchCategoryConstants.TBL_PREVMAINTMASTER,
                Category = "Other"
            };
            List<EstimatedCosts> list = estimatecost.RetriveByObjectId(this.userData.DatabaseKey);
            foreach (var ec in list)
            {
                estimatedCostModel = new EstimatedCostModel();
                estimatedCostModel.EstimatedCostsId = ec.EstimatedCostsId;
                estimatedCostModel.Source = ec.Source;
                estimatedCostModel.VendorClientLookupId = ec.VendorClientLookupId;
                estimatedCostModel.Description = ec.Description;
                estimatedCostModel.UnitCost = ec.UnitCost;
                estimatedCostModel.Quantity = ec.Quantity;
                estimatedCostModel.TotalCost = ec.TotalCost;
                estimatedCostModelList.Add(estimatedCostModel);
            }

            return estimatedCostModelList;
        }
        public List<EstimatedCostModel> populateEstimatedSummery(long PrevMaintMasterId)
        {
            EstimatedCostModel estimatedCostModel;
            List<EstimatedCostModel> estimatedCostModelList = new List<EstimatedCostModel>();

            EstimatedCosts estimatecost = new EstimatedCosts()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ObjectId = PrevMaintMasterId,
                ObjectType = SearchCategoryConstants.TBL_PREVMAINTMASTER,
            };
            List<EstimatedCosts> list = estimatecost.SummeryRetriveByObjectId(this.userData.DatabaseKey);
            foreach (var ec in list)
            {
                estimatedCostModel = new EstimatedCostModel();
                estimatedCostModel.EstimatedCostsId = ec.EstimatedCostsId;
                estimatedCostModel.TotalPartCost = ec.TotalPartCost;
                estimatedCostModel.TotalCraftCost = ec.TotalCraftCost;
                estimatedCostModel.TotalLaborHours = ec.TotalLaborHours;
                estimatedCostModel.TotalExternalCost = ec.TotalExternalCost;
                estimatedCostModel.TotalInternalCost = ec.TotalInternalCost;
                estimatedCostModel.TotalSummeryCost = ec.TotalSummeryCost;
                estimatedCostModelList.Add(estimatedCostModel);
            }

            return estimatedCostModelList;
        }
        #endregion

        #region Estimate 
        #region Part
        public EstimatedCosts AddPart(EstimatedCostModel estimatedCostModel)
        {
            EstimatedCosts estimatecost = new EstimatedCosts()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ObjectId = estimatedCostModel.PrevMaintMasterId,
                ObjectType = SearchCategoryConstants.TBL_PREVMAINTMASTER,
                Category = "Parts"
            };

            string part_ClientLookupId = estimatedCostModel.ClientLookupId;
            Part pt = new Part { ClientId = _dbKey.Client.ClientId, SiteId = _dbKey.User.DefaultSiteId, ClientLookupId = part_ClientLookupId };
            pt.RetrieveByClientLookUpIdNUPCCode(_dbKey);

            estimatecost.CategoryId = pt.PartId;
            estimatecost.Description = pt.Description;
            estimatecost.UnitCost = pt.AppliedCost;
            estimatecost.Quantity = estimatedCostModel.Quantity ?? 0;
            estimatecost.Source = "Internal";
            estimatecost.CheckDuplicateCraftForAdd(userData.DatabaseKey);
            if (estimatecost != null && estimatecost.ErrorMessages.Count > 0)
            {
                return estimatecost;
            }
            estimatecost.Create(this.userData.DatabaseKey);
            return estimatecost;
        }
        public EstimatedCosts EditPart(EstimatedCostModel estimatedCostModel)
        {
            EstimatedCosts estimatecost = new EstimatedCosts()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ObjectId = estimatedCostModel.PrevMaintMasterId,
                EstimatedCostsId = estimatedCostModel.EstimatedCostsId,
            };

            estimatecost.Retrieve(this.userData.DatabaseKey);

            estimatecost.Quantity = estimatedCostModel.Quantity ?? 0;
            estimatecost.CheckDuplicateCraftForUpdate(userData.DatabaseKey);
            if (estimatecost != null && estimatecost.ErrorMessages.Count > 0)
            {
                return estimatecost;
            }
            estimatecost.Update(this.userData.DatabaseKey);
            return estimatecost;
        }
        #endregion

        #region Other
        public EstimatedCosts AddOther(EstimatedCostModel estimatedCostModel)
        {
            EstimatedCosts estimatecost = new EstimatedCosts()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ObjectId = estimatedCostModel.PrevMaintMasterId,
                ObjectType = SearchCategoryConstants.TBL_PREVMAINTMASTER,
                Category = "Other"
            };

            if (userData.Site.UseVendorMaster)
            {
                estimatecost.VendorId = 0;
            }
            else
            {
                estimatecost.VendorId = estimatedCostModel.VendorId;
            }

            estimatecost.CategoryId = 0;
            estimatecost.Description = estimatedCostModel.Description;
            estimatecost.UnitCost = estimatedCostModel.UnitCost ?? 0;
            estimatecost.Quantity = estimatedCostModel.Quantity ?? 0;
            estimatecost.Source = estimatedCostModel.Source;
            estimatecost.Create(this.userData.DatabaseKey);
            return estimatecost;
        }
        public EstimatedCosts EditOther(EstimatedCostModel estimatedCostModel)
        {
            EstimatedCosts estimatecost = new EstimatedCosts()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ObjectId = estimatedCostModel.PrevMaintMasterId,
                EstimatedCostsId = estimatedCostModel.EstimatedCostsId,
            };

            estimatecost.Retrieve(this.userData.DatabaseKey);

            if (userData.Site.UseVendorMaster)
            {
                estimatecost.VendorId = 0;
            }
            else
            {
                estimatecost.VendorId = estimatedCostModel.VendorId;
            }
            estimatecost.ObjectType = SearchCategoryConstants.TBL_PREVMAINTMASTER;
            estimatecost.Category = "Other";
            estimatecost.CategoryId = 0;
            estimatecost.Description = estimatedCostModel.Description;
            estimatecost.UnitCost = estimatedCostModel.UnitCost ?? 0;
            estimatecost.Quantity = estimatedCostModel.Quantity ?? 0;
            estimatecost.Source = estimatedCostModel.Source;
            estimatecost.Update(this.userData.DatabaseKey);
            return estimatecost;
        }

        #endregion

        #region Labor
        public EstimatedCosts AddLabor(EstimatedCostModel estimatedCostModel)
        {

            EstimatedCosts estimatecost = new EstimatedCosts()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ObjectId = estimatedCostModel.PrevMaintMasterId,
                ObjectType = SearchCategoryConstants.TBL_PREVMAINTMASTER,
                Category = SearchCategoryConstants.TBL_CRAFT
            };

            Craft craft = new Craft();

            craft.ClientId = userData.DatabaseKey.Personnel.ClientId;
            craft.CraftId = estimatedCostModel.CraftId;
            craft.Retrieve(userData.DatabaseKey);

            estimatecost.CategoryId = craft.CraftId;
            estimatecost.Description = craft.Description;
            estimatecost.UnitCost = craft.ChargeRate;
            estimatecost.Quantity = estimatedCostModel?.Quantity ?? 0;
            estimatecost.Duration = estimatedCostModel.Duration ?? 0;
            estimatecost.Source = "Internal";
            estimatecost.CheckDuplicateCraftForAdd(userData.DatabaseKey);
            if (estimatecost.ErrorMessages != null && estimatecost.ErrorMessages.Count > 0)
            {
                return estimatecost;
            }
            estimatecost.Create(this.userData.DatabaseKey);
            return estimatecost;
        }
        public EstimatedCosts EditLabor(EstimatedCostModel estimatedCostModel)
        {
            EstimatedCosts estimatecost = new EstimatedCosts()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ObjectId = estimatedCostModel.PrevMaintMasterId,
                EstimatedCostsId = estimatedCostModel.EstimatedCostsId,
            };

            estimatecost.Retrieve(this.userData.DatabaseKey);

            Craft craft = new Craft();

            craft.ClientId = userData.DatabaseKey.Personnel.ClientId;
            craft.CraftId = estimatedCostModel.CraftId;
            craft.Retrieve(userData.DatabaseKey);

            estimatecost.ObjectType = SearchCategoryConstants.TBL_PREVMAINTMASTER;
            estimatecost.Category = SearchCategoryConstants.TBL_CRAFT;
            estimatecost.CategoryId = craft.CraftId;
            estimatecost.Description = craft.Description;
            estimatecost.UnitCost = craft.ChargeRate;
            estimatecost.Quantity = estimatedCostModel?.Quantity ?? 0;
            estimatecost.Duration = estimatedCostModel.Duration ?? 0;
            estimatecost.Source = "Internal";
            estimatecost.CheckDuplicateCraftForUpdate(userData.DatabaseKey);
            if (estimatecost != null && estimatecost.ErrorMessages.Count > 0)
            {
                return estimatecost;
            }
            estimatecost.Update(this.userData.DatabaseKey);
            return estimatecost;
        }
        #endregion

        public bool DeleteEstimate(long estimatedCostId)
        {
            try
            {
                EstimatedCosts estimatecost = new EstimatedCosts()
                {
                    ClientId = this.userData.DatabaseKey.Client.ClientId,
                    EstimatedCostsId = estimatedCostId
                };
                estimatecost.Delete(this.userData.DatabaseKey);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region PM Scheduling Reassign
        public List<PMSchedulingReassignModel> populatePMSchedulingReassign( string orderbycol = "", string orderDir = "", int skip = 0, int length = 0, string AssignedTo = "", string PMID = "", string Description = "", string ChargeToClientLookupId = "", string ChargeToName = "", string NextDue = "")
        {
            PrevMaintMaster prevMaintMaster = new PrevMaintMaster();
            PMSchedulingReassignModel PMSchedulingReassignModel;
            List<PMSchedulingReassignModel> PMSchedulingReassignModelList = new List<PMSchedulingReassignModel>();

            PrevMaintSched PrevMaintSchedl = new PrevMaintSched()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.Personnel.SiteId,
                CallerUserInfoId = this.userData.DatabaseKey.User.CallerUserInfoId,
                OrderbyColumn = orderbycol,
                OrderBy = orderDir,
                Offset = skip,
                Nextrow = length,
                AssignedTo = AssignedTo,
                PMID = PMID,
                Description = Description,
                ChargeToClientLookupId = ChargeToClientLookupId,
                ChargeToName = ChargeToName,
                NextDue = NextDue
            };

            List<PrevMaintSched> PrevMaintSchedList = PrevMaintSchedl.RetrievePMSchedulingRecord(this.userData.DatabaseKey);

            foreach (var p in PrevMaintSchedList)
            {
                PMSchedulingReassignModel = new PMSchedulingReassignModel();
                PMSchedulingReassignModel.PrevMaintSchedId = p.PrevMaintSchedId;
                PMSchedulingReassignModel.AssignedTo = p.AssignedTo_PersonnelClientLookupId;
                PMSchedulingReassignModel.PMID = p.ClientLookupId;
                PMSchedulingReassignModel.Description = p.Description;
                PMSchedulingReassignModel.ChargeTo = p.ChargeToClientLookupId;
                PMSchedulingReassignModel.ChargeToName = p.ChargeToName;
                if (p.NextDueDate != null && p.NextDueDate != default(DateTime))
                {
                    PMSchedulingReassignModel.NextDue = p.NextDueDate;
                }
                else
                {
                    PMSchedulingReassignModel.NextDue = null;
                }
                #region V2-977
                PMSchedulingReassignModel.PMSAssignId = p.PMSAssignId;
                PMSchedulingReassignModel.IndexId = p.IndexId;
                #endregion
                PMSchedulingReassignModel.TotalCount = p.TotalCount;
                PMSchedulingReassignModelList.Add(PMSchedulingReassignModel);
            }
            return PMSchedulingReassignModelList;

        }

        public PrevMaintSched UpdatePMSchedulingReassign(long PersonnelId, string[] PrevMainSchdId)
        {
            PrevMaintSched _PrevMaintSched = new PrevMaintSched();
            foreach (var item in PrevMainSchdId)
            {
                _PrevMaintSched.ClientId = this.userData.DatabaseKey.Client.ClientId;
                _PrevMaintSched.PrevMaintSchedId = Convert.ToInt64(item);
                _PrevMaintSched.AssignedTo_PersonnelId = PersonnelId;
                _PrevMaintSched.UpdateAssignToPersonnelByPrevMaintSchedId(userData.DatabaseKey);
            }
            return _PrevMaintSched;
        }
        #endregion

        #region Preventive  Maintenance Forcast
        public  List<PMForcastModel> populatePrevMaintForcast(DateTime ForCastDate, string ScheduleType, string OnDemandGroup,ref string timeoutError, List<string> assignedPMS = null, string requiredDate = "", bool? downrequired = null, List<string> shift = null)
        {
            PrevMaintMaster prevMaintMaster = new PrevMaintMaster();
            List<PrevMaintSched> PrevMaintSchedList = new List<PrevMaintSched>();
            PrevMaintSched PrevMaintSchedl = new PrevMaintSched();
            List<PMForcastModel> PrevMaintForcastList = new List<PMForcastModel>();
            string timeoutErrors = string.Empty;
            PMForcastModel pMForcastModel;
            PrevMaintSchedl.CallerUserName = this.userData.DatabaseKey.User.CallerUserName;
            PrevMaintSchedl.ClientId = this.userData.DatabaseKey.User.ClientId;
            PrevMaintSchedl.SiteId = this.userData.DatabaseKey.Personnel.SiteId;
            PrevMaintSchedl.CallerUserInfoId = this.userData.DatabaseKey.User.CallerUserInfoId;
            PrevMaintSchedl.SchedueledDate = ForCastDate;
            PrevMaintSchedl.CurrentDate = DateTime.Today;
            PrevMaintSchedl.OnDemandGroup = OnDemandGroup;
            PrevMaintSchedl.AssignedTo_PersonnelIds = string.Join(",", assignedPMS ?? new List<string>());
            PrevMaintSchedl.PMForeCastRequiredDate = requiredDate;
            PrevMaintSchedl.ForecastDownRequired = downrequired;
            PrevMaintSchedl.Shift = string.Join(",", shift ?? new List<string>());
            string ScheduleTypeVal = ScheduleType;
            if (ScheduleTypeVal.ToString() == ScheduleTypeConstants.Calendar)
            {
                var calanderDetails = PrevMaintSchedl.RetrievePMCalendarForecastFromPrevMaintLibrary_ConsiderExcludeDOW(this.userData.DatabaseKey);
                PrevMaintSchedList = calanderDetails.Item1;
                timeoutError = calanderDetails.Item2;
            }
            else if (ScheduleTypeVal.ToString() == ScheduleTypeConstants.OnDemand)
            {
                var OnDemandRecords = PrevMaintSchedl.RetrievePMOnDemandForecastFromPrevMaintLibrary_V2(this.userData.DatabaseKey);
                PrevMaintSchedList = OnDemandRecords.Item1;
                timeoutError = OnDemandRecords.Item2;
            }
            if (PrevMaintSchedList != null)
            {
                CommonWrapper commonWrapper = new CommonWrapper(userData);
                List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
                List<DataContracts.LookupList> Shift = new List<DataContracts.LookupList>();
                AllLookUps = commonWrapper.GetAllLookUpList();
                if (AllLookUps != null)
                {
                    Shift = AllLookUps.Where(x => x.ListName == LookupListConstants.Shift).ToList();
                }
                foreach (var p in PrevMaintSchedList)
                {
                    pMForcastModel = new PMForcastModel();
                    pMForcastModel.ClientLookupId = p.ClientLookupId;
                    pMForcastModel.Description = p.Description;
                    pMForcastModel.SchedueledDate = p.SchedueledDate;
                    pMForcastModel.AssignedTo_PersonnelClientLookupId = p.AssignedTo_PersonnelClientLookupId;
                    pMForcastModel.ChargeToClientLookupId = p.ChargeToClientLookupId;
                    pMForcastModel.ChargeToName = p.ChargeToName;
                    pMForcastModel.Duration = p.Duration;
                    pMForcastModel.EstLaborHours = p.EstLaborHours;
                    pMForcastModel.EstLaborCost = p.EstLaborCost;
                    pMForcastModel.EstOtherCost = p.EstOtherCost;
                    pMForcastModel.EstMaterialCost = p.EstMaterialCost;
                    pMForcastModel.PrevMaintSchedId = p.PrevMaintSchedId;
                    pMForcastModel.RequiredDate = p.RequiredDate;
                    pMForcastModel.Assigned = p.AssignedTo_PersonnelName;
                    pMForcastModel.ChildCount = p.ChildCount;
                    pMForcastModel.AssignedMultiple = p.AssignedMultiple;
                    pMForcastModel.DownRequired = p.DownRequired;
                    if (Shift != null && Shift.Any(cus => cus.ListValue == p.Shift))
                    {
                        pMForcastModel.Shift = Shift.Where(x => x.ListValue == p.Shift).Select(x => x.Description).First();
                    }
                    else
                    {
                        pMForcastModel.Shift = string.Empty;
                    }
                    pMForcastModel.Type = p.Type;//V2-1207
                    PrevMaintForcastList.Add(pMForcastModel);
                }
            }
            return PrevMaintForcastList;
        }
        #endregion

        #region GenerateWO
        public string CreatePMWorkOrder(PMGenerateWorkOrdersModel pMWoModel, ref List<Int64> createdWorkOrderList)
        {
            long site = userData.DatabaseKey.User.DefaultSiteId;
            string scheduleType = pMWoModel.ScheduleType;
            int WOAllowedPrintNumber = 50;
            string onDemandGroup = pMWoModel.OnDemand;
            bool printGeneratedWO = pMWoModel.chkPrintWorkOrder;
            DateTime genarateDate = pMWoModel?.GenerateThrough ?? DateTime.MinValue;
            bool printAttachment = false;
            string AssetGroup1Ids = string.Join(",", pMWoModel.AssetGroup1Ids ?? new List<string>());
            string AssetGroup2Ids = string.Join(",", pMWoModel.AssetGroup2Ids ?? new List<string>());
            string AssetGroup3Ids = string.Join(",", pMWoModel.AssetGroup3Ids ?? new List<string>());
            string PrevMaintSchedType = string.Join(",", pMWoModel.PrevMaintSchedType ?? new List<string>());
            string PrevMaintMasterType = string.Join(",", pMWoModel.PrevMaintMasterType ?? new List<string>());

            DataContracts.PrevMaintBatchEntry be = new PrevMaintBatchEntry()
            {
                CallerUserInfoId = this.userData.DatabaseKey.User.UserInfoId,
                CallerUserName = this.userData.DatabaseKey.UserName,
            };
            List<PrevMaintBatchEntry> PrevMaintBatchEntryList = be.PrevMaintBatchEntry_ForWorkOrderFromPrevMaintLibrary_V2(this.userData.DatabaseKey, this.userData.DatabaseKey.Client.ClientId, site, scheduleType, genarateDate, onDemandGroup, printGeneratedWO, printAttachment, AssetGroup1Ids, AssetGroup2Ids, AssetGroup3Ids, PrevMaintSchedType, PrevMaintMasterType);
            if (PrevMaintBatchEntryList.Count > 0)
            {
                if (PrevMaintBatchEntryList.Count > WOAllowedPrintNumber && pMWoModel.chkPrintWorkOrder)
                {
                    return ("Printing more than " + WOAllowedPrintNumber + " records is not allowed in the current system.Please use Preview before Generation mode to restrict the number of records before printing.");
                }
                else
                {
                    try
                    {
                        long PrevMaintBatchHeaderId = 0;
                        foreach (PrevMaintBatchEntry batchEntry in PrevMaintBatchEntryList)
                        {
                            long _workOrderid = 0;
                            string newClientLookupId = CustomSequentialId.GetNextId(this.userData.DatabaseKey, AutoGenerateKey.WO_Annual, this.userData.DatabaseKey.User.DefaultSiteId, string.Empty);

                            DataContracts.WorkOrder workOrderContract = new DataContracts.WorkOrder();

                            _workOrderid = workOrderContract.WorkOrder_CreateForPrevMaintenanceFromPrevMaintLibrary_V2
                                (this.userData.DatabaseKey, batchEntry.PrevMaintBatchEntryId, newClientLookupId, this.userData.DatabaseKey.Personnel.PersonnelId, AssetGroup1Ids, AssetGroup2Ids, AssetGroup3Ids, PrevMaintSchedType, PrevMaintMasterType, out long LastWorkAssignedPersonnelId);

                            createdWorkOrderList.Add(_workOrderid);
                            if (LastWorkAssignedPersonnelId == 0)  // 640
                            {
                                CreateEventLog(_workOrderid, WorkOrderEvents.Approved);

                            }
                            else
                            {
                                CreateEventLog(_workOrderid, WorkOrderEvents.Scheduled);
                            }
                            PrevMaintBatchHeaderId = batchEntry.PrevMaintBatchHeaderId;
                        }

                        if (createdWorkOrderList.Count > 0)
                        {
                            string commaSeparetedWorkOrders = string.Empty;
                            createdWorkOrderList.ForEach(x =>
                            {
                                if (x != 0)
                                    commaSeparetedWorkOrders += x.ToString() + ",";
                            });

                            //Update The PrevMaintBatchHeader 
                            Update_PrevMaintBatchHeader(createdWorkOrderList.Count(), PrevMaintBatchHeaderId);

                            DataContracts.WorkOrder workOrder = new DataContracts.WorkOrder();
                            AlertCreate(workOrder, AlertTypeEnum.WorkOrderSchedule, createdWorkOrderList);
                        }
                        return (Convert.ToString(createdWorkOrderList.Count()));
                    }
                    catch (Exception ex)
                    {
                        return (ErrorMessageConstants.Not_All_Work_Orders_Generated_Successfully);
                    }
                }


            }
            else
            {
                return (ErrorMessageConstants.The_Search_Returned_No_Results);
            }
        }

        public string CreatePMWorkOrderPreview(PrevBatchEntryModel PrevBatchEntryModellist, ref List<Int64> createdWorkOrderList)
        {
            string AssetGroup1Ids = string.Join(",", PrevBatchEntryModellist.AssetGroup1Ids ?? new List<string>());
            string AssetGroup2Ids = string.Join(",", PrevBatchEntryModellist.AssetGroup2Ids ?? new List<string>());
            string AssetGroup3Ids = string.Join(",", PrevBatchEntryModellist.AssetGroup3Ids ?? new List<string>());
            string PrevMaintSchedType = string.Join(",", PrevBatchEntryModellist.PrevMaintSchedType ?? new List<string>());
            string PrevMaintMasterType = string.Join(",", PrevBatchEntryModellist.PrevMaintMasterType ?? new List<string>());
            bool printGeneratedWO = PrevBatchEntryModellist.chkPrintWorkOrder;
            if (PrevBatchEntryModellist.list.Count > 0)
            {
                try
                {
                    long PrevMaintBatchHeaderId = 0;
                    foreach (PrevBatchEntryListModel obj in PrevBatchEntryModellist.list)
                    {
                        long _workOrderid = 0;
                        string newClientLookupId = CustomSequentialId.GetNextId(this.userData.DatabaseKey, AutoGenerateKey.WO_Annual, this.userData.DatabaseKey.User.DefaultSiteId, string.Empty);

                        DataContracts.WorkOrder workOrderContract = new DataContracts.WorkOrder();
                        _workOrderid = workOrderContract.WorkOrder_CreateForPrevMaintenanceFromPrevMaintLibrary_V2
                                            (this.userData.DatabaseKey, obj.PrevMaintBatchEntryId, newClientLookupId, this.userData.DatabaseKey.Personnel.PersonnelId, AssetGroup1Ids, AssetGroup2Ids, AssetGroup3Ids, PrevMaintSchedType, PrevMaintMasterType, out long LastWorkAssignedPersonnelId);

                        createdWorkOrderList.Add(_workOrderid);
                        if (userData.Site.UsePlanning && obj.PlanningRequired == true)
                        {

                            CreateEventLog(_workOrderid, WorkOrderEvents.Planning);
                        }
                        else
                        {
                            if (LastWorkAssignedPersonnelId == 0)  // 640
                            {
                                CreateEventLog(_workOrderid, WorkOrderEvents.Approved);

                            }
                            else
                            {
                                CreateEventLog(_workOrderid, WorkOrderEvents.Scheduled);
                            }
                        }

                        PrevMaintBatchHeaderId = obj.PrevMaintBatchHeaderId;
                    }

                    if (createdWorkOrderList.Count > 0)
                    {
                        string commaSeparetedWorkOrders = string.Empty;
                        createdWorkOrderList.ForEach(x =>
                                                {
                                                    if (x != 0)
                                                        commaSeparetedWorkOrders += x.ToString() + ",";
                                                });


                        //Update The PrevMaintBatchHeader 
                        Update_PrevMaintBatchHeader(createdWorkOrderList.Count(), PrevMaintBatchHeaderId);

                        DataContracts.WorkOrder workOrder = new DataContracts.WorkOrder();
                        AlertCreate(workOrder, AlertTypeEnum.WorkOrderSchedule, createdWorkOrderList);
                    }
                    return (Convert.ToString(createdWorkOrderList.Count()));
                }
                catch (Exception ex)
                {
                    return (ErrorMessageConstants.Not_All_Work_Orders_Generated_Successfully);
                }
            }
            return "";
        }

        private void Update_PrevMaintBatchHeader(int count, long PrevMaintBatchHeaderId)
        {
            PrevMaintBatchHeader PrevMaintBatchHeaderObj = new PrevMaintBatchHeader()
            {
                CallerUserInfoId = this.userData.DatabaseKey.User.UserInfoId,
                CallerUserName = this.userData.DatabaseKey.UserName,
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                PrevMaintBatchHeaderId = PrevMaintBatchHeaderId,
            };
            PrevMaintBatchHeaderObj.Retrieve(this.userData.DatabaseKey);

            PrevMaintBatchHeaderObj.BatchEntries = count;
            PrevMaintBatchHeaderObj.Update(this.userData.DatabaseKey);
        }

        private void AlertCreate(DataContracts.WorkOrder workOrder, AlertTypeEnum alertTypeEnum, List<Int64> ListWorkOrderId) //Process Alert
        {
            DataContracts.WorkOrder workorder = new DataContracts.WorkOrder();
            ProcessAlert objAlert = new ProcessAlert(this.userData);

            List<object> lstWOID = new List<object>();
            foreach (Int64 objItem in ListWorkOrderId)
            {
                lstWOID.Add(objItem);
            }
            try
            {
                objAlert.CreateAlert<DataContracts.WorkOrder>(this.userData, workorder, alertTypeEnum, this.userData.DatabaseKey.User.UserInfoId, lstWOID);

            }
            catch (Exception ex)
            {
                throw;
            }

        }


        #region Search
        public List<PMGenerateWorkOrdersSearchModel> GetPMGenerateWorkOrdersGridData(string orderbycol = "", string orderDir = "", int skip = 0, int length = 0, string ScheduleType = "", DateTime? ScheduleThroughDate = null, string OnDemandGroup = "", List<string> AssetGroup1Ids = null, List<string> AssetGroup2Ids = null, List<string> AssetGroup3Ids = null, List<string> WOType = null, List<string> PMType = null, DateTime? PrevBEDueDate = null, string EquipmentClientLookupId = "", string EquipmentName = "", string PrevMaintMasterClientLookupId = "", string PrevMaintMasterDescription = "", long ReturnPrevMaintBatchHeaderId = 0, bool? downRequired = null, List<string> shift = null)
        {
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            PMGenerateWorkOrdersSearchModel _PMGenerateWorkOrdersSearchModel;
            List<PMGenerateWorkOrdersSearchModel> PMGenerateWorkOrdersSearchModelList = new List<PMGenerateWorkOrdersSearchModel>();
            List<PrevMaintBatchEntry> PMGenerateWorkOrdersList = new List<PrevMaintBatchEntry>();
            PrevMaintBatchEntry PMGenerateWorkOrders = new PrevMaintBatchEntry();
            PMGenerateWorkOrders.ClientId = this.userData.DatabaseKey.Client.ClientId;
            PMGenerateWorkOrders.siteid = userData.DatabaseKey.User.DefaultSiteId;
            PMGenerateWorkOrders.OrderbyColumn = orderbycol;
            PMGenerateWorkOrders.OrderBy = orderDir;
            PMGenerateWorkOrders.OffSetVal = skip;
            PMGenerateWorkOrders.NextRow = length;
            PMGenerateWorkOrders.ScheduleType = ScheduleType;
            PMGenerateWorkOrders.ScheduleThroughDate = ScheduleThroughDate;
            PMGenerateWorkOrders.OnDemandgroup = OnDemandGroup;
            PMGenerateWorkOrders.AssetGroup1Ids = string.Join(",", AssetGroup1Ids ?? new List<string>());
            PMGenerateWorkOrders.AssetGroup2Ids = string.Join(",", AssetGroup2Ids ?? new List<string>());
            PMGenerateWorkOrders.AssetGroup3Ids = string.Join(",", AssetGroup3Ids ?? new List<string>());
            PMGenerateWorkOrders.PrevMaintSchedType = string.Join(",", WOType ?? new List<string>());
            PMGenerateWorkOrders.PrevMaintMasterType = string.Join(",", PMType ?? new List<string>());

            PMGenerateWorkOrders.PrevBEDueDate = PrevBEDueDate;
            PMGenerateWorkOrders.EquipmentClientLookupId = EquipmentClientLookupId;
            PMGenerateWorkOrders.EquipmentName = EquipmentName;
            PMGenerateWorkOrders.PrevMaintMasterClientLookupId = PrevMaintMasterClientLookupId;
            PMGenerateWorkOrders.PrevMaintMasterDescription = PrevMaintMasterDescription;
            PMGenerateWorkOrders.PrevMaintBatchHeaderId = ReturnPrevMaintBatchHeaderId;
            PMGenerateWorkOrders.Shift = string.Join(",", shift ?? new List<string>());
            PMGenerateWorkOrders.DownRequired = downRequired;
            PMGenerateWorkOrdersList = PMGenerateWorkOrders.PrevMaintBatchEntry_ForWorkOrderFromPrevMaintLibraryChunkSearch_V2(userData.DatabaseKey, userData.Site.TimeZone);

            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> Shift = new List<DataContracts.LookupList>();
            AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                Shift = AllLookUps.Where(x => x.ListName == LookupListConstants.Shift).ToList();
            }
            foreach (var item in PMGenerateWorkOrdersList)
            {
                _PMGenerateWorkOrdersSearchModel = new PMGenerateWorkOrdersSearchModel();
                _PMGenerateWorkOrdersSearchModel.PrevMaintBatchEntryId = item.PrevMaintBatchEntryId;
                _PMGenerateWorkOrdersSearchModel.PrevMaintBatchHeaderId = item.PrevMaintBatchHeaderId;
                if (item.DueDate != null && item.DueDate == default(DateTime))
                {
                    _PMGenerateWorkOrdersSearchModel.DueDate = null;
                }
                else
                {
                    _PMGenerateWorkOrdersSearchModel.DueDate = item.DueDate;
                }
                _PMGenerateWorkOrdersSearchModel.EquipmentClientLookupId = item.EquipmentClientLookupId;
                _PMGenerateWorkOrdersSearchModel.EquipmentName = item.EquipmentName;
                _PMGenerateWorkOrdersSearchModel.PrevMaintMasterClientLookupId = item.PrevMaintMasterClientLookupId;
                _PMGenerateWorkOrdersSearchModel.PrevMaintMasterDescription = item.PrevMaintMasterDescription;
                _PMGenerateWorkOrdersSearchModel.TotalCount = item.TotalCount;
                //V2-1014
                _PMGenerateWorkOrdersSearchModel.ChildCount = item.ChildCount;
                _PMGenerateWorkOrdersSearchModel.PMRequiredDate = item.PMRequiredDate;
                _PMGenerateWorkOrdersSearchModel.AssignedMultiple = item.AssignedMultiple;
                _PMGenerateWorkOrdersSearchModel.AssignedTo_Name = item.AssignedTo_Name;
                //V2-1082
                _PMGenerateWorkOrdersSearchModel.DownRequired = item.DownRequired;
                if (Shift != null && Shift.Any(cus => cus.ListValue == item.Shift))
                {
                    _PMGenerateWorkOrdersSearchModel.Shift = Shift.Where(x => x.ListValue == item.Shift).Select(x => x.Description).First();
                }
                else
                {
                    _PMGenerateWorkOrdersSearchModel.Shift = string.Empty;
                }
                _PMGenerateWorkOrdersSearchModel.PrevMaintSchedId = item.PrevMaintSchedId;
                _PMGenerateWorkOrdersSearchModel.PlanningRequired = item.PlanningRequired;
                PMGenerateWorkOrdersSearchModelList.Add(_PMGenerateWorkOrdersSearchModel);
            }

            return PMGenerateWorkOrdersSearchModelList;
        }

        private void CreateEventLog(Int64 WOId, string Status)
        {
            WorkOrderEventLog log = new WorkOrderEventLog();
            log.ClientId = userData.DatabaseKey.Client.ClientId;
            log.SiteId = userData.DatabaseKey.Personnel.SiteId;
            log.WorkOrderId = WOId;
            log.Event = Status;
            log.TransactionDate = DateTime.UtcNow;
            log.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            log.Comments = "";
            log.SourceId = 0;
            log.Create(userData.DatabaseKey);
        }
        #endregion
        #endregion

        #region Select PM Procedure
        public List<PreventiveMaintenanceLibraryModel> PopulatePmProcedure()
        {
            DataContracts.PrevMaintLibrary pml = new DataContracts.PrevMaintLibrary();
            pml.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            pml.ClientId = userData.DatabaseKey.Client.ClientId;
            List<DataContracts.PrevMaintLibrary> pmlList = pml.RetrieveAllCustom(this.userData.DatabaseKey);
            PreventiveMaintenanceLibraryModel objPmLib;
            List<PreventiveMaintenanceLibraryModel> PmLibList = new List<PreventiveMaintenanceLibraryModel>();
            foreach (var v in pmlList)
            {
                objPmLib = new PreventiveMaintenanceLibraryModel();
                objPmLib.PrevMaintLibraryId = v.PrevMaintLibraryId;
                objPmLib.ClientLookupId = v.ClientLookupId;
                objPmLib.Description = v.Description;
                objPmLib.JobDuration = v.JobDuration;
                objPmLib.FrequencyType = v.FrequencyType;
                objPmLib.Frequency = v.Frequency;
                PmLibList.Add(objPmLib);
            }
            return PmLibList;
        }
        //*** V2-694
        public List<PreventiveMaintenanceLibraryModel> PopulatePmProcedure_InactiveFlag(int InactiveFlag)
        {
            DataContracts.PrevMaintLibrary pml = new DataContracts.PrevMaintLibrary();
            pml.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            pml.ClientId = userData.DatabaseKey.Client.ClientId;
            pml.InactiveFlg = InactiveFlag;
            List<DataContracts.PrevMaintLibrary> pmlList = pml.RetrieveByInactiveFlag(this.userData.DatabaseKey);
            PreventiveMaintenanceLibraryModel objPmLib;
            List<PreventiveMaintenanceLibraryModel> PmLibList = new List<PreventiveMaintenanceLibraryModel>();
            foreach (var v in pmlList)
            {
                objPmLib = new PreventiveMaintenanceLibraryModel();
                objPmLib.PrevMaintLibraryId = v.PrevMaintLibraryId;
                objPmLib.ClientLookupId = v.ClientLookupId;
                objPmLib.Description = v.Description;
                objPmLib.JobDuration = v.JobDuration;
                objPmLib.FrequencyType = v.FrequencyType;
                objPmLib.Frequency = v.Frequency;
                PmLibList.Add(objPmLib);
            }
            return PmLibList;
        }
        public PrevMaintMaster AddPreventiveProc(long pr_id, out bool LibraryActivationStatus)
        {
            long ObjectId = 0;
            PrevMaintMaster pmm = new PrevMaintMaster();

            PrevMaintLibrary l = new PrevMaintLibrary();
            l.ClientId = userData.DatabaseKey.Client.ClientId; ;
            l.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            l.PrevMaintLibraryId = pr_id;
            l.Retrieve(userData.DatabaseKey);
            LibraryActivationStatus = l.InactiveFlag;
            pmm.ClientId = userData.DatabaseKey.Client.ClientId;
            pmm.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            pmm.PrevMaintLibraryId = Convert.ToInt64(pr_id);
            pmm.ClientLookupId = l.ClientLookupId;
            pmm.ValidateLink(userData.DatabaseKey);
            if (pmm.ErrorMessages.Count == 0)
            {
                // SOM-1737 - RKL - 2020-Apr-06
                if (userData.DatabaseKey.Client.PMLibCopy)
                {
                    pmm.CreatePrevMaintMaster_CopyFromPMLibrary(userData.DatabaseKey);
                }
                else
                {
                    pmm.CreatePrevMaintMaster_CreateFromPMLibrary(userData.DatabaseKey);
                }
                ObjectId = pmm.PrevMaintMasterId;
            }
            return pmm;
        }
        #endregion Select PM Procedure 

        #region ChangePreventive Maintenance Id

        public List<String> ChangePrevMaintId(ChangePreventiveIDModel _ChangePreventiveIDModel)
        {
            PrevMaintVM objprevMaintVM = new PrevMaintVM();
            PreventiveMaintenanceModel preventiveMaintenanceModel = new PreventiveMaintenanceModel();
            List<string> PMsg = new List<string>();
            long _pvId = _ChangePreventiveIDModel.PrevMaintMasterId;
            if (_pvId > 0)
            {
                PrevMaintMaster prevmaint = new PrevMaintMaster();
                prevmaint.PrevMaintMasterId = _pvId;
                prevmaint.ClientLookupId = _ChangePreventiveIDModel.ClientLookupId;
                prevmaint.SiteId = userData.DatabaseKey.User.DefaultSiteId;
                prevmaint.ValidateChangeLookupId(userData.DatabaseKey);
                if (prevmaint.ErrorMessages.Count == 0)
                {
                    prevmaint.Retrieve(userData.DatabaseKey);
                    prevmaint.PrevMaintMasterId = _ChangePreventiveIDModel.PrevMaintMasterId;
                    prevmaint.ClientLookupId = _ChangePreventiveIDModel.ClientLookupId;
                    prevmaint.UpdateIndex = prevmaint.UpdateIndex;
                    prevmaint.UpdateForClientLookupId(userData.DatabaseKey);
                }
                else
                {
                    PMsg = prevmaint.ErrorMessages;
                }
            }
            return PMsg;
        }
        #endregion

        #region V2-949
        public AttachmentModel EditPMAttachment(long PrevMainMasterId, long FileAttachmentId)
        {
            AttachmentModel objAttachmentModel = new AttachmentModel();
            Attachment attachment = new Attachment()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                AttachmentId = FileAttachmentId,
            };
            attachment.Retrieve(userData.DatabaseKey);
            objAttachmentModel.AttachmentId = attachment.AttachmentId;
            objAttachmentModel.Subject = attachment.Description;
            objAttachmentModel.PrintwithForm = attachment.PrintwithForm;
            objAttachmentModel.PrevMaintMasterId = PrevMainMasterId;
            objAttachmentModel.FileType= attachment.FileType;
            return objAttachmentModel;
        }


        #endregion

        #region V2-712
        public List<PMSchedAssignModel> GetPMScheduleAssignGrid_V2(string order, string orderDir, int length, int offset1, long PrevMasterID)
        {
            List<PMSchedAssignModel> PMSchedAssignList = new List<PMSchedAssignModel>();
            PMSchedAssignModel objPMSchedAssign;

            PMSchedAssign pmsa = new PMSchedAssign()
            {
                PrevMaintSchedId = PrevMasterID,
                orderbyColumn = order,
                offset1 = offset1,
                nextrow = length,
                orderBy = orderDir,
                SiteId = this.userData.DatabaseKey.Personnel.SiteId

            };
            List<PMSchedAssign> pmSchedAssignList = pmsa.RetrivePMSchedAssignByPMSchedId(userData.DatabaseKey);
            foreach (var pm in pmSchedAssignList)
            {
                objPMSchedAssign = new PMSchedAssignModel();
                objPMSchedAssign.PMSchedAssignId = pm.PMSchedAssignId;
                objPMSchedAssign.PrevMaintSchedId = pm.PrevMaintSchedId;
                objPMSchedAssign.ClientLookupId = pm.ClientLookupId;
                objPMSchedAssign.PersonnelFullName = pm.PersonnelFullName;
                objPMSchedAssign.ScheduledHours = pm.ScheduledHours;
                objPMSchedAssign.TotalCount = pm.TotalCount;
                PMSchedAssignList.Add(objPMSchedAssign);
            }
            return PMSchedAssignList;
        }

        public PMSchedAssignModel PopulatePmSchedAssignWidgetDetails(long PmSchedAssignId)
        {
            PMSchedAssign pmschedassign = new PMSchedAssign()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                PMSchedAssignId = PmSchedAssignId,
            };
            pmschedassign.Retrieve(userData.DatabaseKey);
            PMSchedAssignModel objPMSchedAssign = new PMSchedAssignModel()
            {
                PMSchedAssignId = pmschedassign.PMSchedAssignId,
                PrevMaintSchedId = pmschedassign.PrevMaintSchedId,
                PersonnelId = pmschedassign.PersonnelId,
                ScheduledHours = pmschedassign.ScheduledHours,
            };
            return objPMSchedAssign;
        }

        public PMSchedAssign CreatePMSchedAssignment(PMSchedAssignModel objpmschedassign)
        {
            PMSchedAssign pMSchedAssign = new PMSchedAssign()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                PrevMaintSchedId = objpmschedassign.PrevMaintSchedId,
                PersonnelId = objpmschedassign.PersonnelId,
                ScheduledHours = objpmschedassign.ScheduledHours ?? 0,
            };
            pMSchedAssign.Create(userData.DatabaseKey);
            return pMSchedAssign;
        }
        public List<PersonnelLookUpModel> GetPMAssignPersonnelLookupList(string orderbycol = "", string orderDir = "", int skip = 0, int length = 0, string clientLookupId = "", string NameFirst = "", string NameLast = "")
        {

            PersonnelLookUpModel personnelLookUpModel;
            List<PersonnelLookUpModel> personnelLookUpModelList = new List<PersonnelLookUpModel>();
            List<Personnel> PersonnelList = new List<Personnel>();
            Personnel personnel = new Personnel();
            personnel.ClientId = this.userData.DatabaseKey.Client.ClientId;
            personnel.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            personnel.OrderbyColumn = orderbycol;
            personnel.OrderBy = orderDir;
            personnel.OffSetVal = skip;
            personnel.NextRow = length;
            personnel.ClientLookupId = clientLookupId;
            personnel.NameFirst = NameFirst;
            personnel.NameLast = NameLast;

            PersonnelList = personnel.RetrievePMAssignPersonneLookupList(userData.DatabaseKey);

            foreach (var item in PersonnelList)
            {
                personnelLookUpModel = new PersonnelLookUpModel();
                personnelLookUpModel.PersonnelId = item.PersonnelId;
                personnelLookUpModel.ClientLookupId = item.ClientLookupId;
                personnelLookUpModel.NameFirst = item.NameFirst;
                personnelLookUpModel.NameLast = item.NameLast;
                personnelLookUpModel.TotalCount = item.TotalCount;

                personnelLookUpModelList.Add(personnelLookUpModel);
            }

            return personnelLookUpModelList;
        }

        public PMSchedAssign UpdatePmScheduleAssign(long PMSchedAssignId, decimal hours)
        {
            PMSchedAssign pmschedassign = new PMSchedAssign()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                PMSchedAssignId = PMSchedAssignId,
            };
            pmschedassign.Retrieve(userData.DatabaseKey);
            pmschedassign.ScheduledHours = hours;
            pmschedassign.Update(userData.DatabaseKey);
            return pmschedassign;
        }
        public bool DeletePmSchedassign(long pmSchedAssignId)
        {
            try
            {
                PMSchedAssign pMSchedAssign = new PMSchedAssign()
                {
                    ClientId = this.userData.DatabaseKey.Client.ClientId,
                    PMSchedAssignId = pmSchedAssignId
                };
                pMSchedAssign.Delete(this.userData.DatabaseKey);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<InnerGridPMSchedAssignModel> GetPrevMaintInnerGridData(long PrevMaintSchedId)
        {
            InnerGridPMSchedAssignModel _InnerGridPMSchedAssignModel;
            List<InnerGridPMSchedAssignModel> InnerGridPMSchedAssignModelList = new List<InnerGridPMSchedAssignModel>();
            List<PMSchedAssign> PMSchedAssignList = new List<PMSchedAssign>();
            PMSchedAssign pmSchedAssign = new PMSchedAssign();
            pmSchedAssign.ClientId = this.userData.DatabaseKey.Client.ClientId;
            pmSchedAssign.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            pmSchedAssign.orderbyColumn = "1";
            pmSchedAssign.orderBy = "asc";
            pmSchedAssign.offset1 = 0;
            pmSchedAssign.nextrow = 100000;
            pmSchedAssign.PrevMaintSchedId = PrevMaintSchedId;
            PMSchedAssignList = pmSchedAssign.RetrivePMSchedAssignByPMSchedId(userData.DatabaseKey);
            foreach (var item in PMSchedAssignList)
            {
                _InnerGridPMSchedAssignModel = new InnerGridPMSchedAssignModel();
                _InnerGridPMSchedAssignModel.ClientLookupId = item.ClientLookupId;
                _InnerGridPMSchedAssignModel.PersonnelFullName = item.PersonnelFullName;
                _InnerGridPMSchedAssignModel.PMSchedAssignId = item.PMSchedAssignId;
                _InnerGridPMSchedAssignModel.ScheduledHours = item.ScheduledHours;
                _InnerGridPMSchedAssignModel.TotalCount = item.TotalCount;
                InnerGridPMSchedAssignModelList.Add(_InnerGridPMSchedAssignModel);
            }
            return InnerGridPMSchedAssignModelList;
        }
        #endregion

        #region V2-950
        public PrevMaintSched AddPrevMaintSchedDynamicCalendar(PrevMaintVM prevMaintVM)
        {
            PrevMaintSched prevmaintsched = new PrevMaintSched();
            prevmaintsched.ClientId = userData.DatabaseKey.Client.ClientId;
            prevmaintsched.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            prevmaintsched.PrevMaintMasterId = prevMaintVM.AddPMSRecordsModelDynamic_Calendar.PrevMaintMasterId ?? 0;
            prevmaintsched.ChargeType = ChargeType.Equipment;
            PropertyInfo getpropertyInfo, setpropertyInfo;
            List<UIConfigurationDetailsForModelValidation> configDetails = new List<UIConfigurationDetailsForModelValidation> { };

            configDetails = new RetrieveDataForUIConfiguration().Retrieve(DataDictionaryViewNameConstant.AddScheduleRecords_Calendar, userData);

            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == false && x.Section == false);

            foreach (var item in ColumnDetails)
            {
                getpropertyInfo = prevMaintVM.AddPMSRecordsModelDynamic_Calendar.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(prevMaintVM.AddPMSRecordsModelDynamic_Calendar);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }

                setpropertyInfo = prevmaintsched.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(prevmaintsched, val);
            }

            prevmaintsched.ValidateAdd(userData.DatabaseKey);

            if (prevmaintsched.ErrorMessages.Count != 0)
            {   //SOM-1244  Multiple row for same chargeTo entry in schedule records
                if (prevmaintsched.ErrorMessages[0].ToString() != ErrorMessageConstants.Schedule_Record_Exists)
                {
                    return prevmaintsched;
                }
                else
                {
                    prevmaintsched.CreateByForeignKeys_V2(userData.DatabaseKey);
                }
            }
            else
            {
                prevmaintsched.CreateByForeignKeys_V2(userData.DatabaseKey);
            }
            if (prevmaintsched.ErrorMessages.Count == 0)
            {
                if (configDetails.Any(x => x.Display == true && x.UDF == true && x.Section == false))
                {
                    IEnumerable<string> errors = AddPrevMaintSchedUDFDynamicCalendar(prevMaintVM.AddPMSRecordsModelDynamic_Calendar, prevmaintsched.PrevMaintSchedId, configDetails);
                    if (errors != null && errors.Count() > 0)
                    {
                        prevmaintsched.ErrorMessages.AddRange(errors);
                    }
                }
            }
            return prevmaintsched;
        }
        public List<string> AddPrevMaintSchedUDFDynamicCalendar(AddPMSRecordsModelDynamic_Calendar PrevMaintSched, long PrevMaintSchedId,
            List<UIConfigurationDetailsForModelValidation> configDetails)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            PMSchedUDF pMSchedUDF = new PMSchedUDF();
            pMSchedUDF.ClientId = userData.DatabaseKey.Client.ClientId;
            pMSchedUDF.PrevMaintSchedId = PrevMaintSchedId;
            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == true && x.Section == false);
            foreach (var item in ColumnDetails)
            {
                getpropertyInfo = PrevMaintSched.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(PrevMaintSched);
                Type t = getpropertyInfo.PropertyType;
                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                setpropertyInfo = pMSchedUDF.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(pMSchedUDF, val);
            }

            pMSchedUDF.Create(_dbKey);
            return pMSchedUDF.ErrorMessages;
        }
        public PrevMaintSched AddPrevMaintSchedDynamicMeter(PrevMaintVM prevMaintVM)
        {
            PrevMaintSched prevmaintsched = new PrevMaintSched();
            prevmaintsched.ClientId = userData.DatabaseKey.Client.ClientId;
            prevmaintsched.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            prevmaintsched.PrevMaintMasterId = prevMaintVM.AddPMSRecordsModelDynamic_Meter.PrevMaintMasterId ?? 0;
            prevmaintsched.ChargeType = ChargeType.Equipment;
            PropertyInfo getpropertyInfo, setpropertyInfo;
            List<UIConfigurationDetailsForModelValidation> configDetails = new List<UIConfigurationDetailsForModelValidation> { };

            configDetails = new RetrieveDataForUIConfiguration().Retrieve(DataDictionaryViewNameConstant.AddScheduleRecords_Meter, userData);

            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == false && x.Section == false);

            foreach (var item in ColumnDetails)
            {
                getpropertyInfo = prevMaintVM.AddPMSRecordsModelDynamic_Meter.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(prevMaintVM.AddPMSRecordsModelDynamic_Meter);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }

                setpropertyInfo = prevmaintsched.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(prevmaintsched, val);
            }

            prevmaintsched.ValidateAdd(userData.DatabaseKey);

            if (prevmaintsched.ErrorMessages.Count != 0)
            {
                if (prevmaintsched.ErrorMessages[0].ToString() != ErrorMessageConstants.Schedule_Record_Exists)
                {
                    return prevmaintsched;
                }
                else
                {
                    prevmaintsched.CreateByForeignKeys_V2(userData.DatabaseKey);
                }
            }
            else
            {
                prevmaintsched.CreateByForeignKeys_V2(userData.DatabaseKey);
            }
            if (prevmaintsched.ErrorMessages.Count == 0)
            {
                if (configDetails.Any(x => x.Display == true && x.UDF == true && x.Section == false))
                {
                    IEnumerable<string> errors = AddPrevMaintSchedUDFDynamicMeter(prevMaintVM.AddPMSRecordsModelDynamic_Meter, prevmaintsched.PrevMaintSchedId, configDetails);
                    if (errors != null && errors.Count() > 0)
                    {
                        prevmaintsched.ErrorMessages.AddRange(errors);
                    }
                }
            }
            return prevmaintsched;
        }
        public List<string> AddPrevMaintSchedUDFDynamicMeter(AddPMSRecordsModelDynamic_Meter PrevMaintSched, long PrevMaintSchedId,
            List<UIConfigurationDetailsForModelValidation> configDetails)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            PMSchedUDF pMSchedUDF = new PMSchedUDF();
            pMSchedUDF.ClientId = userData.DatabaseKey.Client.ClientId;
            pMSchedUDF.PrevMaintSchedId = PrevMaintSchedId;

            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == true && x.Section == false);
            foreach (var item in ColumnDetails)
            {
                getpropertyInfo = PrevMaintSched.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(PrevMaintSched);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                setpropertyInfo = pMSchedUDF.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(pMSchedUDF, val);
            }

            pMSchedUDF.Create(_dbKey);
            return pMSchedUDF.ErrorMessages;
        }
        public PrevMaintSched AddPrevMaintSchedDynamicOnDemand(PrevMaintVM prevMaintVM)
        {
            PrevMaintSched prevmaintsched = new PrevMaintSched();
            prevmaintsched.ClientId = userData.DatabaseKey.Client.ClientId;
            prevmaintsched.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            prevmaintsched.PrevMaintMasterId = prevMaintVM.AddPMSRecordsModelDynamic_OnDemand.PrevMaintMasterId ?? 0;
            prevmaintsched.ChargeType = ChargeType.Equipment;
            PropertyInfo getpropertyInfo, setpropertyInfo;
            List<UIConfigurationDetailsForModelValidation> configDetails = new List<UIConfigurationDetailsForModelValidation> { };

            configDetails = new RetrieveDataForUIConfiguration().Retrieve(DataDictionaryViewNameConstant.AddScheduleRecords_OnDemand, userData);

            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == false && x.Section == false);

            foreach (var item in ColumnDetails)
            {
                getpropertyInfo = prevMaintVM.AddPMSRecordsModelDynamic_OnDemand.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(prevMaintVM.AddPMSRecordsModelDynamic_OnDemand);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }

                setpropertyInfo = prevmaintsched.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(prevmaintsched, val);
            }

            prevmaintsched.ValidateAdd(userData.DatabaseKey);

            if (prevmaintsched.ErrorMessages.Count != 0)
            {
                if (prevmaintsched.ErrorMessages[0].ToString() != ErrorMessageConstants.Schedule_Record_Exists)
                {
                    return prevmaintsched;
                }
                else
                {
                    prevmaintsched.CreateByForeignKeys_V2(userData.DatabaseKey);
                }
            }
            else
            {
                prevmaintsched.CreateByForeignKeys_V2(userData.DatabaseKey);
            }
            if (prevmaintsched.ErrorMessages.Count == 0)
            {
                if (configDetails.Any(x => x.Display == true && x.UDF == true && x.Section == false))
                {
                    IEnumerable<string> errors = AddPrevMaintSchedUDFDynamicOnDemand(prevMaintVM.AddPMSRecordsModelDynamic_OnDemand, prevmaintsched.PrevMaintSchedId, configDetails);
                    if (errors != null && errors.Count() > 0)
                    {
                        prevmaintsched.ErrorMessages.AddRange(errors);
                    }
                }
            }
            return prevmaintsched;
        }
        public List<string> AddPrevMaintSchedUDFDynamicOnDemand(AddPMSRecordsModelDynamic_OnDemand PrevMaintSched, long PrevMaintSchedId,
            List<UIConfigurationDetailsForModelValidation> configDetails)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            PMSchedUDF pMSchedUDF = new PMSchedUDF();
            pMSchedUDF.ClientId = userData.DatabaseKey.Client.ClientId;
            pMSchedUDF.PrevMaintSchedId = PrevMaintSchedId;

            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == true && x.Section == false);
            foreach (var item in ColumnDetails)
            {
                getpropertyInfo = PrevMaintSched.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(PrevMaintSched);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                setpropertyInfo = pMSchedUDF.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(pMSchedUDF, val);
            }

            pMSchedUDF.Create(_dbKey);
            return pMSchedUDF.ErrorMessages;
        }
        private void AssignDefaultOrNullValue(ref object val, Type t)
        {
            if (t.Equals(typeof(long?)))
            {
                val = val ?? 0;
            }
            else if (t.Equals(typeof(DateTime?)))
            {
                //val = val ?? null;
            }
            else if (t.Equals(typeof(decimal?)))
            {
                val = val ?? 0M;
            }
            else if (t.Name == "String")
            {
                val = val ?? string.Empty;
            }
        }
        public PrevMaintSched getPrevMaintSchedDetailsByPMSchedId(long PrevMaintMasterId, long PrevMaintScheId)
        {
            PrevMaintSched prevmaintsched = new PrevMaintSched()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                PrevMaintMasterId = PrevMaintMasterId,
                PrevMaintSchedId = PrevMaintScheId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId
            };
            prevmaintsched.RetrieveByForeignKeys_V2(userData.DatabaseKey);
            return prevmaintsched;
        }
        public PMSchedUDF getPMSchedUDFByPrevMaintSchedId(long PMSchedId)
        {
            PMSchedUDF pMSchedUDF = new PMSchedUDF()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                PrevMaintSchedId = PMSchedId
            };

            pMSchedUDF = pMSchedUDF.RetrivePMSchedUDFByPrevMaintSchedId(this.userData.DatabaseKey);
            return pMSchedUDF;
        }
        public EditPMSRecordsModelDynamic_Calendar getEditPMScheduleRecordsDynamicCalendar(long PrevMaintMasterId, long PMSchedId)
        {
            EditPMSRecordsModelDynamic_Calendar editDynamic = new EditPMSRecordsModelDynamic_Calendar();
            PrevMaintSched PMS = getPrevMaintSchedDetailsByPMSchedId(PrevMaintMasterId, PMSchedId);
            PMSchedUDF PMSUDF = getPMSchedUDFByPrevMaintSchedId(PMSchedId);

            editDynamic = MapPrevMaintSchedDataCalendar(editDynamic, PMS);
            editDynamic = MapPMSchedUDFDataCalendar(editDynamic, PMSUDF);

            return editDynamic;
        }
        public EditPMSRecordsModelDynamic_Calendar MapPrevMaintSchedDataCalendar(EditPMSRecordsModelDynamic_Calendar objPrev, PrevMaintSched obj)
        {
            objPrev.PrevMaintMasterId = obj.PrevMaintMasterId;
            objPrev.PrevMaintSchedId = obj.PrevMaintSchedId;
            objPrev.ChargeType = obj.ChargeType;
            objPrev.ChargeToId = obj.ChargeToId;
            objPrev.ChargeToClientLookupId = obj?.ChargeToClientLookupId ?? string.Empty;
            objPrev.ChargeToName = obj?.ChargeToName ?? string.Empty;
            objPrev.ScheduleMethod = obj?.ScheduleMethod ?? string.Empty;
            objPrev.FrequencyType = obj?.FrequencyType ?? string.Empty;
            objPrev.Frequency = obj?.Frequency ?? 0;
            objPrev.CalendarSlack = obj?.CalendarSlack ?? 0;
            objPrev.OnDemandGroup = obj?.OnDemandGroup ?? string.Empty;
            objPrev.InactiveFlag = obj?.InactiveFlag ?? false;
            if (obj.NextDueDate != null && obj.NextDueDate != default(DateTime))
            {
                objPrev.NextDueDate = obj.NextDueDate;
            }
            else
            {
                objPrev.NextDueDate = null;
            }
            if (obj.LastPerformed != null && obj.LastPerformed != default(DateTime))
            {
                objPrev.LastPerformed = obj.LastPerformed;
            }
            else
            {
                objPrev.LastPerformed = null;
            }
            if (obj.LastScheduled != null && obj.LastScheduled != default(DateTime))
            {
                objPrev.LastScheduled = obj.LastScheduled;
            }
            else
            {
                objPrev.LastScheduled = null;
            }
            if (obj.CurrentWOComplete != null && obj.CurrentWOComplete != default(DateTime))
            {
                objPrev.CurrentWOComplete = obj.CurrentWOComplete;
            }
            else
            {
                objPrev.CurrentWOComplete = null;
            }
            objPrev.Scheduled = obj?.Scheduled ?? false;
            objPrev.DownRequired = obj?.DownRequired ?? false;
            objPrev.MeterId = obj?.MeterId ?? 0;
            objPrev.MeterInterval = obj?.MeterInterval ?? 0;
            objPrev.MeterSlack = obj?.MeterSlack ?? 0;
            objPrev.MeterMethod = obj?.MeterMethod ?? string.Empty;
            objPrev.MeterLastDue = obj?.MeterLastDue ?? 0;
            objPrev.MeterLastDone = obj?.MeterLastDone ?? 0;
            objPrev.AssignedTo_PersonnelId = obj?.AssignedTo_PersonnelId ?? 0;
            objPrev.Type = obj?.Type ?? string.Empty;
            objPrev.Section = obj?.Section ?? string.Empty;
            objPrev.Priority = obj?.Priority ?? string.Empty;
            objPrev.Category = obj?.Category ?? string.Empty;
            objPrev.Shift = obj?.Shift ?? string.Empty;
            if (obj.Planner_PersonnelId == 0)
            {
                objPrev.Planner_PersonnelId = null;
            }
            else
            {
                objPrev.Planner_PersonnelId = obj.Planner_PersonnelId;
            }
            objPrev.FailureCode = obj?.FailureCode ?? string.Empty;
            objPrev.ActionCode = obj?.ActionCode ?? string.Empty;
            objPrev.RootCauseCode = obj?.RootCauseCode ?? string.Empty;
            objPrev.ExcludeDOW = obj?.ExcludeDOW ?? string.Empty;
            objPrev.ExclusionDaysString = GetExclusionDaysArray(objPrev.ExcludeDOW);
            objPrev.Planner_ClientLookupId = obj.Planner_ClientLookupId ?? string.Empty;
            objPrev.PlanningRequired = obj?.PlanningRequired ?? false;
            objPrev.NumbersOfPMSchedAssignRecords = obj.NumbersOfPMSchedAssignRecords; //V2-1161
            return objPrev;
        }
        public EditPMSRecordsModelDynamic_Calendar MapPMSchedUDFDataCalendar(EditPMSRecordsModelDynamic_Calendar objPrev, PMSchedUDF obj)
        {
            if (obj != null)
            {
                objPrev.PMSchedUDFId = obj.PMSchedUDFId;

                objPrev.Text1 = obj.Text1;
                objPrev.Text2 = obj.Text2;
                objPrev.Text3 = obj.Text3;
                objPrev.Text4 = obj.Text4;

                if (obj.Date1 != null && obj.Date1 == DateTime.MinValue)
                {
                    objPrev.Date1 = null;
                }
                else
                {
                    objPrev.Date1 = obj.Date1;
                }
                if (obj.Date2 != null && obj.Date2 == DateTime.MinValue)
                {
                    objPrev.Date2 = null;
                }
                else
                {
                    objPrev.Date2 = obj.Date2;
                }
                if (obj.Date3 != null && obj.Date3 == DateTime.MinValue)
                {
                    objPrev.Date3 = null;
                }
                else
                {
                    objPrev.Date3 = obj.Date3;
                }
                if (obj.Date4 != null && obj.Date4 == DateTime.MinValue)
                {
                    objPrev.Date4 = null;
                }
                else
                {
                    objPrev.Date4 = obj.Date4;
                }

                objPrev.Bit1 = obj.Bit1;
                objPrev.Bit2 = obj.Bit2;
                objPrev.Bit3 = obj.Bit3;
                objPrev.Bit4 = obj.Bit4;

                objPrev.Numeric1 = obj.Numeric1;
                objPrev.Numeric2 = obj.Numeric2;
                objPrev.Numeric3 = obj.Numeric3;
                objPrev.Numeric4 = obj.Numeric4;

                objPrev.Select1 = obj.Select1;
                objPrev.Select2 = obj.Select2;
                objPrev.Select3 = obj.Select3;
                objPrev.Select4 = obj.Select4;
            }

            return objPrev;
        }

        public EditPMSRecordsModelDynamic_Meter getEditPMScheduleRecordsDynamicMeter(long PrevMaintMasterId, long PMSchedId)
        {
            EditPMSRecordsModelDynamic_Meter editDynamic = new EditPMSRecordsModelDynamic_Meter();
            PrevMaintSched PMS = getPrevMaintSchedDetailsByPMSchedId(PrevMaintMasterId, PMSchedId);
            PMSchedUDF PMSUDF = getPMSchedUDFByPrevMaintSchedId(PMSchedId);

            editDynamic = MapPrevMaintSchedDataMeter(editDynamic, PMS);
            editDynamic = MapPMSchedUDFDataMeter(editDynamic, PMSUDF);

            return editDynamic;
        }
        public EditPMSRecordsModelDynamic_Meter MapPrevMaintSchedDataMeter(EditPMSRecordsModelDynamic_Meter objPrev, PrevMaintSched obj)
        {
            objPrev.PrevMaintMasterId = obj.PrevMaintMasterId;
            objPrev.PrevMaintSchedId = obj.PrevMaintSchedId;
            objPrev.ChargeType = obj.ChargeType;
            objPrev.ChargeToId = obj.ChargeToId;
            objPrev.ChargeToClientLookupId = obj?.ChargeToClientLookupId ?? string.Empty;
            objPrev.Meter_ClientLookupId = obj?.Meter_ClientLookupId ?? string.Empty;
            objPrev.ChargeToName = obj?.ChargeToName ?? string.Empty;
            objPrev.ScheduleMethod = obj?.ScheduleMethod ?? string.Empty;
            objPrev.FrequencyType = obj?.FrequencyType ?? string.Empty;
            objPrev.Frequency = obj?.Frequency ?? 0;
            objPrev.CalendarSlack = obj?.CalendarSlack ?? 0;
            objPrev.OnDemandGroup = obj?.OnDemandGroup ?? string.Empty;
            objPrev.InactiveFlag = obj?.InactiveFlag ?? false;
            if (obj.NextDueDate != null && obj.NextDueDate != default(DateTime))
            {
                objPrev.NextDueDate = obj.NextDueDate;
            }
            else
            {
                objPrev.NextDueDate = null;
            }
            if (obj.LastPerformed != null && obj.LastPerformed != default(DateTime))
            {
                objPrev.LastPerformed = obj.LastPerformed;
            }
            else
            {
                objPrev.LastPerformed = null;
            }
            if (obj.LastScheduled != null && obj.LastScheduled != default(DateTime))
            {
                objPrev.LastScheduled = obj.LastScheduled;
            }
            else
            {
                objPrev.LastScheduled = null;
            }
            if (obj.CurrentWOComplete != null && obj.CurrentWOComplete != default(DateTime))
            {
                objPrev.CurrentWOComplete = obj.CurrentWOComplete;
            }
            else
            {
                objPrev.CurrentWOComplete = null;
            }
            objPrev.Scheduled = obj?.Scheduled ?? false;
            objPrev.DownRequired = obj?.DownRequired ?? false;
            objPrev.MeterId = obj?.MeterId ?? 0;
            objPrev.MeterInterval = obj?.MeterInterval ?? 0;
            objPrev.MeterSlack = obj?.MeterSlack ?? 0;
            objPrev.MeterMethod = obj?.MeterMethod ?? string.Empty;
            objPrev.MeterLastDue = obj?.MeterLastDue ?? 0;
            objPrev.MeterLastDone = obj?.MeterLastDone ?? 0;
            objPrev.AssignedTo_PersonnelId = obj?.AssignedTo_PersonnelId ?? 0;
            objPrev.Type = obj?.Type ?? string.Empty;
            objPrev.Section = obj?.Section ?? string.Empty;
            objPrev.Priority = obj?.Priority ?? string.Empty;
            objPrev.Category = obj?.Category ?? string.Empty;
            objPrev.Shift = obj?.Shift ?? string.Empty;
            if (obj.Planner_PersonnelId == 0)
            {
                objPrev.Planner_PersonnelId = null;
            }
            else
            {
                objPrev.Planner_PersonnelId = obj.Planner_PersonnelId;
            }
            objPrev.FailureCode = obj?.FailureCode ?? string.Empty;
            objPrev.ActionCode = obj?.ActionCode ?? string.Empty;
            objPrev.RootCauseCode = obj?.RootCauseCode ?? string.Empty;
            objPrev.ExcludeDOW = obj?.ExcludeDOW ?? string.Empty;
            objPrev.Planner_ClientLookupId = obj.Planner_ClientLookupId ?? string.Empty;
            objPrev.PlanningRequired = obj?.PlanningRequired ?? false; //V2-1161
            objPrev.NumbersOfPMSchedAssignRecords = obj.NumbersOfPMSchedAssignRecords; //V2-1161
            return objPrev;
        }
        public EditPMSRecordsModelDynamic_Meter MapPMSchedUDFDataMeter(EditPMSRecordsModelDynamic_Meter objPrev, PMSchedUDF obj)
        {
            if (obj != null)
            {
                objPrev.PMSchedUDFId = obj.PMSchedUDFId;

                objPrev.Text1 = obj.Text1;
                objPrev.Text2 = obj.Text2;
                objPrev.Text3 = obj.Text3;
                objPrev.Text4 = obj.Text4;

                if (obj.Date1 != null && obj.Date1 == DateTime.MinValue)
                {
                    objPrev.Date1 = null;
                }
                else
                {
                    objPrev.Date1 = obj.Date1;
                }
                if (obj.Date2 != null && obj.Date2 == DateTime.MinValue)
                {
                    objPrev.Date2 = null;
                }
                else
                {
                    objPrev.Date2 = obj.Date2;
                }
                if (obj.Date3 != null && obj.Date3 == DateTime.MinValue)
                {
                    objPrev.Date3 = null;
                }
                else
                {
                    objPrev.Date3 = obj.Date3;
                }
                if (obj.Date4 != null && obj.Date4 == DateTime.MinValue)
                {
                    objPrev.Date4 = null;
                }
                else
                {
                    objPrev.Date4 = obj.Date4;
                }

                objPrev.Bit1 = obj.Bit1;
                objPrev.Bit2 = obj.Bit2;
                objPrev.Bit3 = obj.Bit3;
                objPrev.Bit4 = obj.Bit4;

                objPrev.Numeric1 = obj.Numeric1;
                objPrev.Numeric2 = obj.Numeric2;
                objPrev.Numeric3 = obj.Numeric3;
                objPrev.Numeric4 = obj.Numeric4;

                objPrev.Select1 = obj.Select1;
                objPrev.Select2 = obj.Select2;
                objPrev.Select3 = obj.Select3;
                objPrev.Select4 = obj.Select4;
            }

            return objPrev;
        }

        public EditPMSRecordsModelDynamic_OnDemand getEditPMScheduleRecordsDynamicOnDemand(long PrevMaintMasterId, long PMSchedId)
        {
            EditPMSRecordsModelDynamic_OnDemand editDynamic = new EditPMSRecordsModelDynamic_OnDemand();
            PrevMaintSched PMS = getPrevMaintSchedDetailsByPMSchedId(PrevMaintMasterId, PMSchedId);
            PMSchedUDF PMSUDF = getPMSchedUDFByPrevMaintSchedId(PMSchedId);

            editDynamic = MapPrevMaintSchedDataOnDemand(editDynamic, PMS);
            editDynamic = MapPMSchedUDFDataOnDemand(editDynamic, PMSUDF);

            return editDynamic;
        }
        public EditPMSRecordsModelDynamic_OnDemand MapPrevMaintSchedDataOnDemand(EditPMSRecordsModelDynamic_OnDemand objPrev, PrevMaintSched obj)
        {
            objPrev.PrevMaintMasterId = obj.PrevMaintMasterId;
            objPrev.PrevMaintSchedId = obj.PrevMaintSchedId;
            objPrev.ChargeType = obj.ChargeType;
            objPrev.ChargeToId = obj.ChargeToId;
            objPrev.ChargeToClientLookupId = obj?.ChargeToClientLookupId ?? string.Empty;
            objPrev.ChargeToName = obj?.ChargeToName ?? string.Empty;
            objPrev.ScheduleMethod = obj?.ScheduleMethod ?? string.Empty;
            objPrev.FrequencyType = obj?.FrequencyType ?? string.Empty;
            objPrev.Frequency = obj?.Frequency ?? 0;
            objPrev.CalendarSlack = obj?.CalendarSlack ?? 0;
            objPrev.OnDemandGroup = obj?.OnDemandGroup ?? string.Empty;
            objPrev.InactiveFlag = obj?.InactiveFlag ?? false;
            if (obj.NextDueDate != null && obj.NextDueDate != default(DateTime))
            {
                objPrev.NextDueDate = obj.NextDueDate;
            }
            else
            {
                objPrev.NextDueDate = null;
            }
            if (obj.LastPerformed != null && obj.LastPerformed != default(DateTime))
            {
                objPrev.LastPerformed = obj.LastPerformed;
            }
            else
            {
                objPrev.LastPerformed = null;
            }
            if (obj.LastScheduled != null && obj.LastScheduled != default(DateTime))
            {
                objPrev.LastScheduled = obj.LastScheduled;
            }
            else
            {
                objPrev.LastScheduled = null;
            }
            if (obj.CurrentWOComplete != null && obj.CurrentWOComplete != default(DateTime))
            {
                objPrev.CurrentWOComplete = obj.CurrentWOComplete;
            }
            else
            {
                objPrev.CurrentWOComplete = null;
            }
            objPrev.Scheduled = obj?.Scheduled ?? false;
            objPrev.DownRequired = obj?.DownRequired ?? false;
            objPrev.MeterId = obj?.MeterId ?? 0;
            objPrev.MeterInterval = obj?.MeterInterval ?? 0;
            objPrev.MeterSlack = obj?.MeterSlack ?? 0;
            objPrev.MeterMethod = obj?.MeterMethod ?? string.Empty;
            objPrev.MeterLastDue = obj?.MeterLastDue ?? 0;
            objPrev.MeterLastDone = obj?.MeterLastDone ?? 0;
            objPrev.AssignedTo_PersonnelId = obj?.AssignedTo_PersonnelId ?? 0;
            objPrev.Type = obj?.Type ?? string.Empty;
            objPrev.Section = obj?.Section ?? string.Empty;
            objPrev.Priority = obj?.Priority ?? string.Empty;
            objPrev.Category = obj?.Category ?? string.Empty;
            objPrev.Shift = obj?.Shift ?? string.Empty;
            if (obj.Planner_PersonnelId == 0)
            {
                objPrev.Planner_PersonnelId = null;
            }
            else
            {
                objPrev.Planner_PersonnelId = obj.Planner_PersonnelId;
            }
            objPrev.FailureCode = obj?.FailureCode ?? string.Empty;
            objPrev.ActionCode = obj?.ActionCode ?? string.Empty;
            objPrev.RootCauseCode = obj?.RootCauseCode ?? string.Empty;
            objPrev.ExcludeDOW = obj?.ExcludeDOW ?? string.Empty;
            objPrev.Planner_ClientLookupId = obj.Planner_ClientLookupId ?? string.Empty;
            objPrev.PlanningRequired = obj?.PlanningRequired ?? false; //V2-1161
            objPrev.NumbersOfPMSchedAssignRecords = obj.NumbersOfPMSchedAssignRecords; //V2-1161
            return objPrev;
        }
        public EditPMSRecordsModelDynamic_OnDemand MapPMSchedUDFDataOnDemand(EditPMSRecordsModelDynamic_OnDemand objPrev, PMSchedUDF obj)
        {
            if (obj != null)
            {
                objPrev.PMSchedUDFId = obj.PMSchedUDFId;

                objPrev.Text1 = obj.Text1;
                objPrev.Text2 = obj.Text2;
                objPrev.Text3 = obj.Text3;
                objPrev.Text4 = obj.Text4;

                if (obj.Date1 != null && obj.Date1 == DateTime.MinValue)
                {
                    objPrev.Date1 = null;
                }
                else
                {
                    objPrev.Date1 = obj.Date1;
                }
                if (obj.Date2 != null && obj.Date2 == DateTime.MinValue)
                {
                    objPrev.Date2 = null;
                }
                else
                {
                    objPrev.Date2 = obj.Date2;
                }
                if (obj.Date3 != null && obj.Date3 == DateTime.MinValue)
                {
                    objPrev.Date3 = null;
                }
                else
                {
                    objPrev.Date3 = obj.Date3;
                }
                if (obj.Date4 != null && obj.Date4 == DateTime.MinValue)
                {
                    objPrev.Date4 = null;
                }
                else
                {
                    objPrev.Date4 = obj.Date4;
                }

                objPrev.Bit1 = obj.Bit1;
                objPrev.Bit2 = obj.Bit2;
                objPrev.Bit3 = obj.Bit3;
                objPrev.Bit4 = obj.Bit4;

                objPrev.Numeric1 = obj.Numeric1;
                objPrev.Numeric2 = obj.Numeric2;
                objPrev.Numeric3 = obj.Numeric3;
                objPrev.Numeric4 = obj.Numeric4;

                objPrev.Select1 = obj.Select1;
                objPrev.Select2 = obj.Select2;
                objPrev.Select3 = obj.Select3;
                objPrev.Select4 = obj.Select4;
            }

            return objPrev;
        }
        public PrevMaintSched editPMSRecordsDynamicCalendar(PrevMaintVM objVM)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            string ExclusionDays = "0000000";
            PrevMaintWrapper _PrevMaintObj = new PrevMaintWrapper(userData);
            PrevMaintSched prevmaintsched = new PrevMaintSched()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                PrevMaintMasterId = objVM.EditPMSRecordsModelDynamic_Calendar.PrevMaintMasterId ?? 0,
                PrevMaintSchedId = objVM.EditPMSRecordsModelDynamic_Calendar.PrevMaintSchedId
            };

            prevmaintsched.RetrieveByForeignKeys_V2(userData.DatabaseKey);

            List<UIConfigurationDetailsForModelValidation> configDetails = new List<UIConfigurationDetailsForModelValidation> { };

            configDetails = new RetrieveDataForUIConfiguration().Retrieve(DataDictionaryViewNameConstant.ScheduleRecordsEdit_Calendar, userData);
            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == false && x.Section == false && x.ViewOnly == false);
            foreach (var item in ColumnDetails)
            {
                getpropertyInfo = objVM.EditPMSRecordsModelDynamic_Calendar.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(objVM.EditPMSRecordsModelDynamic_Calendar);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }

                setpropertyInfo = prevmaintsched.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(prevmaintsched, val);
            }
            prevmaintsched.ChargeType = ChargeType.Equipment;
            prevmaintsched.ChargeToClientLookupId = objVM.EditPMSRecordsModelDynamic_Calendar.ChargeToClientLookupId ?? string.Empty;
            if (objVM.EditPMSRecordsModelDynamic_Calendar.ExclusionDaysString != null)
            {
                ExclusionDays = GetExclusionDaysString(objVM.EditPMSRecordsModelDynamic_Calendar.ExclusionDaysString);

            }
            prevmaintsched.ExcludeDOW = ExclusionDays;
            prevmaintsched.ValidateSave(userData.DatabaseKey);

            if (prevmaintsched.ErrorMessages.Count != 0)
            {
                if (prevmaintsched.ErrorMessages[0].ToString() != ErrorMessageConstants.Schedule_Record_Exists)
                {
                    return prevmaintsched;
                }
                else
                {
                    prevmaintsched.UpdateByForeignKeys_V2(userData.DatabaseKey);
                }
            }
            else
            {
                prevmaintsched.UpdateByForeignKeys_V2(userData.DatabaseKey);
            }
            if (((prevmaintsched.ErrorMessages != null && prevmaintsched.ErrorMessages.Count == 0) || prevmaintsched.ErrorMessages[0].ToString() == ErrorMessageConstants.Schedule_Record_Exists &&
                configDetails.Any(x => x.Display == true && x.UDF == true && x.Section == false && x.ViewOnly == false)))
            {
                IEnumerable<string> errors = EditPMSchedUDFDynamicCalendar(objVM.EditPMSRecordsModelDynamic_Calendar, configDetails);
                if (errors != null && errors.Count() > 0)
                {
                    prevmaintsched.ErrorMessages.AddRange(errors);
                }
            }

            return prevmaintsched;
        }
        public List<string> EditPMSchedUDFDynamicCalendar(EditPMSRecordsModelDynamic_Calendar PMSDyn, List<UIConfigurationDetailsForModelValidation> configDetails)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            PMSchedUDF pMSchedUDF = new PMSchedUDF()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                PrevMaintSchedId = PMSDyn.PrevMaintSchedId
            };
            pMSchedUDF = pMSchedUDF.RetrivePMSchedUDFByPrevMaintSchedId(_dbKey);

            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == true && x.Section == false && x.ViewOnly == false);
            foreach (var item in ColumnDetails)
            {
                getpropertyInfo = PMSDyn.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(PMSDyn);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                setpropertyInfo = pMSchedUDF.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(pMSchedUDF, val);
            }
            if (pMSchedUDF.PrevMaintSchedId == 0)
            {
                pMSchedUDF.PrevMaintSchedId = PMSDyn.PrevMaintSchedId;
                pMSchedUDF.Create(_dbKey);
            }
            else
            {
                pMSchedUDF.Update(_dbKey);
            }

            return pMSchedUDF.ErrorMessages;
        }
        public PrevMaintSched editPMSRecordsDynamicMeter(PrevMaintVM objVM)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            PrevMaintWrapper _PrevMaintObj = new PrevMaintWrapper(userData);
            PrevMaintSched prevmaintsched = new PrevMaintSched()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                PrevMaintMasterId = objVM.EditPMSRecordsModelDynamic_Meter.PrevMaintMasterId ?? 0,
                PrevMaintSchedId = objVM.EditPMSRecordsModelDynamic_Meter.PrevMaintSchedId
            };

            prevmaintsched.RetrieveByForeignKeys_V2(userData.DatabaseKey);

            List<UIConfigurationDetailsForModelValidation> configDetails = new List<UIConfigurationDetailsForModelValidation> { };

            configDetails = new RetrieveDataForUIConfiguration().Retrieve(DataDictionaryViewNameConstant.ScheduleRecordsEdit_Meter, userData);
            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == false && x.Section == false && x.ViewOnly == false);
            foreach (var item in ColumnDetails)
            {
                getpropertyInfo = objVM.EditPMSRecordsModelDynamic_Meter.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(objVM.EditPMSRecordsModelDynamic_Meter);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }

                setpropertyInfo = prevmaintsched.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(prevmaintsched, val);
            }
            prevmaintsched.ChargeType = ChargeType.Equipment;
            prevmaintsched.ChargeToClientLookupId = objVM.EditPMSRecordsModelDynamic_Meter.ChargeToClientLookupId ?? string.Empty;
            prevmaintsched.ValidateSave(userData.DatabaseKey);

            if (prevmaintsched.ErrorMessages.Count != 0)
            {
                if (prevmaintsched.ErrorMessages[0].ToString() != ErrorMessageConstants.Schedule_Record_Exists)
                {
                    return prevmaintsched;
                }
                else
                {
                    prevmaintsched.UpdateByForeignKeys_V2(userData.DatabaseKey);
                }
            }
            else
            {
                prevmaintsched.UpdateByForeignKeys_V2(userData.DatabaseKey);
            }
            if (((prevmaintsched.ErrorMessages != null && prevmaintsched.ErrorMessages.Count == 0) || prevmaintsched.ErrorMessages[0].ToString() == ErrorMessageConstants.Schedule_Record_Exists) &&
                configDetails.Any(x => x.Display == true && x.UDF == true && x.Section == false && x.ViewOnly == false))
            {
                IEnumerable<string> errors = EditPMSchedUDFDynamicMeter(objVM.EditPMSRecordsModelDynamic_Meter, configDetails);
                if (errors != null && errors.Count() > 0)
                {
                    prevmaintsched.ErrorMessages.AddRange(errors);
                }
            }

            return prevmaintsched;
        }
        public List<string> EditPMSchedUDFDynamicMeter(EditPMSRecordsModelDynamic_Meter PMSDyn, List<UIConfigurationDetailsForModelValidation> configDetails)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            PMSchedUDF pMSchedUDF = new PMSchedUDF()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                PrevMaintSchedId = PMSDyn.PrevMaintSchedId
            };
            pMSchedUDF = pMSchedUDF.RetrivePMSchedUDFByPrevMaintSchedId(_dbKey);

            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == true && x.Section == false && x.ViewOnly == false);
            foreach (var item in ColumnDetails)
            {
                getpropertyInfo = PMSDyn.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(PMSDyn);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                setpropertyInfo = pMSchedUDF.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(pMSchedUDF, val);
            }
            if (pMSchedUDF.PrevMaintSchedId == 0)
            {
                pMSchedUDF.PrevMaintSchedId = PMSDyn.PrevMaintSchedId;
                pMSchedUDF.Create(_dbKey);
            }
            else
            {
                pMSchedUDF.Update(_dbKey);
            }

            return pMSchedUDF.ErrorMessages;
        }
        public PrevMaintSched editPMSRecordsDynamicOnDemand(PrevMaintVM objVM)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            PrevMaintWrapper _PrevMaintObj = new PrevMaintWrapper(userData);
            PrevMaintSched prevmaintsched = new PrevMaintSched()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                PrevMaintMasterId = objVM.EditPMSRecordsModelDynamic_OnDemand.PrevMaintMasterId ?? 0,
                PrevMaintSchedId = objVM.EditPMSRecordsModelDynamic_OnDemand.PrevMaintSchedId
            };

            prevmaintsched.RetrieveByForeignKeys_V2(userData.DatabaseKey);

            List<UIConfigurationDetailsForModelValidation> configDetails = new List<UIConfigurationDetailsForModelValidation> { };

            configDetails = new RetrieveDataForUIConfiguration().Retrieve(DataDictionaryViewNameConstant.ScheduleRecordsEdit_OnDemand, userData);
            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == false && x.Section == false && x.ViewOnly == false);
            foreach (var item in ColumnDetails)
            {
                getpropertyInfo = objVM.EditPMSRecordsModelDynamic_OnDemand.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(objVM.EditPMSRecordsModelDynamic_OnDemand);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }

                setpropertyInfo = prevmaintsched.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(prevmaintsched, val);
            }
            prevmaintsched.ChargeType = ChargeType.Equipment;
            prevmaintsched.ChargeToClientLookupId = objVM.EditPMSRecordsModelDynamic_OnDemand.ChargeToClientLookupId ?? string.Empty;
            prevmaintsched.ValidateSave(userData.DatabaseKey);

            if (prevmaintsched.ErrorMessages.Count != 0)
            {
                if (prevmaintsched.ErrorMessages[0].ToString() != ErrorMessageConstants.Schedule_Record_Exists)
                {
                    return prevmaintsched;
                }
                else
                {
                    prevmaintsched.UpdateByForeignKeys_V2(userData.DatabaseKey);
                }
            }
            else
            {
                prevmaintsched.UpdateByForeignKeys_V2(userData.DatabaseKey);
            }
            if (((prevmaintsched.ErrorMessages != null && prevmaintsched.ErrorMessages.Count == 0) || prevmaintsched.ErrorMessages[0].ToString() == ErrorMessageConstants.Schedule_Record_Exists) &&
                configDetails.Any(x => x.Display == true && x.UDF == true && x.Section == false && x.ViewOnly == false))
            {
                IEnumerable<string> errors = EditPMSchedUDFDynamicOnDemand(objVM.EditPMSRecordsModelDynamic_OnDemand, configDetails);
                if (errors != null && errors.Count() > 0)
                {
                    prevmaintsched.ErrorMessages.AddRange(errors);
                }
            }

            return prevmaintsched;
        }
        public List<string> EditPMSchedUDFDynamicOnDemand(EditPMSRecordsModelDynamic_OnDemand PMSDyn, List<UIConfigurationDetailsForModelValidation> configDetails)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            PMSchedUDF pMSchedUDF = new PMSchedUDF()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                PrevMaintSchedId = PMSDyn.PrevMaintSchedId
            };
            pMSchedUDF = pMSchedUDF.RetrivePMSchedUDFByPrevMaintSchedId(_dbKey);

            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == true && x.Section == false && x.ViewOnly == false);
            foreach (var item in ColumnDetails)
            {
                getpropertyInfo = PMSDyn.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(PMSDyn);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                setpropertyInfo = pMSchedUDF.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(pMSchedUDF, val);
            }
            if (pMSchedUDF.PrevMaintSchedId == 0)
            {
                pMSchedUDF.PrevMaintSchedId = PMSDyn.PrevMaintSchedId;
                pMSchedUDF.Create(_dbKey);
            }
            else
            {
                pMSchedUDF.Update(_dbKey);
            }

            return pMSchedUDF.ErrorMessages;
        }
        public PrevMaintSched SavePrevMaintSchedExcludeDOW(long PrevMaintScheId, string[] ExclusionDaysString)
        {
            string ExclusionDays = "0000000";
            PrevMaintSched prevmaintsched = new PrevMaintSched()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                PrevMaintSchedId = PrevMaintScheId
            };

            prevmaintsched.Retrieve(userData.DatabaseKey);
            if (ExclusionDaysString != null)
            {
                ExclusionDays = GetExclusionDaysString(ExclusionDaysString);
            }
            prevmaintsched.ExcludeDOW = ExclusionDays;
            prevmaintsched.Update(userData.DatabaseKey);
            return prevmaintsched;
        }
        #endregion

        #region V2-977
        public PMSchedAssign UpdatePMSchedulingReassignForMultipleAssignment(long PersonnelId, string[] PMSchedAssignId, string[] PMSchedId)
        {
            PMSchedAssign _PMSchedAssign = new PMSchedAssign();
            for (var item = 0; item < PMSchedAssignId.Length; item++)
            {
                if (Convert.ToInt64(PMSchedAssignId[item]) > 0)
                {
                    _PMSchedAssign.ClientId = this.userData.DatabaseKey.Client.ClientId;
                    _PMSchedAssign.PMSchedAssignId = Convert.ToInt64(PMSchedAssignId[item]);
                    _PMSchedAssign.Retrieve(userData.DatabaseKey);
                    _PMSchedAssign.PersonnelId = PersonnelId;
                    _PMSchedAssign.Update(userData.DatabaseKey);
                }
                else
                {
                    _PMSchedAssign.ClientId = this.userData.DatabaseKey.Client.ClientId;
                    _PMSchedAssign.PMSchedAssignId = 0;
                    _PMSchedAssign.PersonnelId = PersonnelId;
                    _PMSchedAssign.PrevMaintSchedId = Convert.ToInt64(PMSchedId[item]);
                    _PMSchedAssign.Create(userData.DatabaseKey);
                }
            }
            return _PMSchedAssign;
        }
        #endregion

        #region V2-1151 Material Request Support
        public EstimatedCosts AddMaterialRequestPartNotInInventory(PrevMaintVM WoVM)
        {
            EstimatedCosts estimatecost = new EstimatedCosts()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ObjectId = WoVM.estimatePart.PrevMaintMasterId,
                ObjectType = SearchCategoryConstants.TBL_PREVMAINTMASTER,
                Category = GlobalConstants.Parts,
                CategoryId = 0,
                Description = WoVM.estimatePart.Description,
                UnitCost = WoVM.estimatePart.UnitCost??0,
                Quantity = WoVM.estimatePart.Quantity ?? 0,
                VendorId=WoVM.estimatePart.VendorId??0,
                AccountId=WoVM.estimatePart.AccountId??0,
                Source = SourceTypeConstant.Internal,
                UnitOfMeasure=WoVM.estimatePart.Unit
            };
            estimatecost.Create(this.userData.DatabaseKey);
            return estimatecost;
        }
        public EstimatedCosts EditMaterialRequest(PrevMaintVM WoVM)
        {
            EstimatedCosts estimatecost = new EstimatedCosts()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ObjectId = WoVM.PartNotInInventoryModel.PrevMaintMasterId,
                EstimatedCostsId = WoVM.PartNotInInventoryModel.EstimatedCostsId,
            };
            estimatecost.Retrieve(this.userData.DatabaseKey);
            estimatecost.Description = WoVM.PartNotInInventoryModel.Description;
            estimatecost.Quantity = WoVM.PartNotInInventoryModel.Quantity ?? 0;
            estimatecost.UnitCost = WoVM.PartNotInInventoryModel.UnitCost ?? 0;
            estimatecost.UnitOfMeasure = WoVM.PartNotInInventoryModel.Unit ?? string.Empty;
            estimatecost.AccountId = WoVM.PartNotInInventoryModel?.AccountId ?? 0;
            estimatecost.AccountClientLookupId = WoVM.PartNotInInventoryModel.AccountClientLookupId;
            estimatecost.VendorId = WoVM.PartNotInInventoryModel?.VendorId ?? 0;
            estimatecost.VendorClientLookupId = WoVM.PartNotInInventoryModel.VendorClientLookupId;
            if (userData.Site.ShoppingCart == true)
            {
                estimatecost.UNSPSC = WoVM.PartNotInInventoryModel?.PartCategoryMasterId ?? 0;
                estimatecost.PartClientLookupId = WoVM.PartNotInInventoryModel.PartCategoryClientLookupId;
            }
            else
            {
                estimatecost.UNSPSC = 0;
                estimatecost.PartClientLookupId = null;
            }
            estimatecost.Update(this.userData.DatabaseKey);
            return estimatecost;
        }
        public EditMaterialRequestModel RetrieveEstimateCostsByObjectId(long EstimatedCostsId, long PrevMaintMasterID)
        {
            EditMaterialRequestModel objPartNotInInventoryModel = new EditMaterialRequestModel();

            EstimatedCosts estimatedCosts = new EstimatedCosts()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ObjectId = PrevMaintMasterID,
                ObjectType = SearchCategoryConstants.TBL_PREVMAINTMASTER
            };
            List<EstimatedCosts> EstmatedCostsItems = EstimatedCosts.EstimatedCostsRetrieveByObjectId_ForChild(this.userData.DatabaseKey, estimatedCosts);

            var selectedEstmatedCostsItems = EstmatedCostsItems != null ? EstmatedCostsItems.Where(x => x.EstimatedCostsId == EstimatedCostsId).SingleOrDefault() : null;
            objPartNotInInventoryModel.EstimatedCostsId = selectedEstmatedCostsItems.EstimatedCostsId;
            objPartNotInInventoryModel.ClientId = selectedEstmatedCostsItems.ClientId;
            objPartNotInInventoryModel.Quantity = selectedEstmatedCostsItems.Quantity;
            objPartNotInInventoryModel.ObjectId = selectedEstmatedCostsItems.ObjectId;
            objPartNotInInventoryModel.Description = selectedEstmatedCostsItems.Description;
            objPartNotInInventoryModel.CategoryId = selectedEstmatedCostsItems.CategoryId;
            objPartNotInInventoryModel.PartClientLookupId = selectedEstmatedCostsItems.PartClientLookupId;
            objPartNotInInventoryModel.UnitCost = selectedEstmatedCostsItems?.UnitCost ?? 00;

            objPartNotInInventoryModel.Unit = selectedEstmatedCostsItems.Unit;
            objPartNotInInventoryModel.AccountId = selectedEstmatedCostsItems.AccountId;
            objPartNotInInventoryModel.AccountClientLookupId = selectedEstmatedCostsItems.AccountClientLookupId;
            objPartNotInInventoryModel.VendorId = selectedEstmatedCostsItems.VendorId;
            objPartNotInInventoryModel.VendorClientLookupId = selectedEstmatedCostsItems.VendorClientLookupId;
            objPartNotInInventoryModel.PartCategoryMasterId = selectedEstmatedCostsItems.UNSPSC;
            objPartNotInInventoryModel.PartCategoryClientLookupId = selectedEstmatedCostsItems.PartCategoryClientLookupId;
            objPartNotInInventoryModel.PrevMaintMasterId = PrevMaintMasterID;
            return objPartNotInInventoryModel;
        }
        #endregion

        #region V2-1204 PrevMaint Model

        public PrevMaintMaster AddPrevMaintModel(PreventiveMaintenanceModel _PrevMaintModel)
        {
            PrevMaintMaster prevMaintMaster = new PrevMaintMaster()
            {
                ClientId = _dbKey.Client.ClientId,
                SiteId = _dbKey.User.DefaultSiteId
            };
            prevMaintMaster.ClientLookupId = _PrevMaintModel.ClientLookupId;
            prevMaintMaster.ScheduleType = _PrevMaintModel.ScheduleType;
            prevMaintMaster.Description = _PrevMaintModel.Description;
            prevMaintMaster.JobDuration = _PrevMaintModel.JobDuration ?? 0;
            prevMaintMaster.Type = _PrevMaintModel.Type;
            prevMaintMaster.InactiveFlag = _PrevMaintModel.InactiveFlag;

            prevMaintMaster.ValidateAdd(userData.DatabaseKey);
            if (prevMaintMaster.ErrorMessages != null && prevMaintMaster.ErrorMessages.Count == 0)
            {
                prevMaintMaster.Create(_dbKey);
                if (prevMaintMaster.ErrorMessages == null || prevMaintMaster.ErrorMessages.Count == 0)
                {
                    // --- Create PrevMaintSched ---
                    var pms = new PrevMaintSched
                    {
                        PrevMaintMasterId = _PrevMaintModel.PrevMaintMasterId,
                        SiteId = userData.DatabaseKey.User.DefaultSiteId
                    };
                    var pmsList = PrevMaintSched.RetrieveByPrevMaintMasterId_V2(userData.DatabaseKey, pms);
                    if (pmsList != null)
                    {
                        foreach (var pmdata in pmsList)
                        {
                            var prevMaintSched = new PrevMaintSched
                            {
                                PrevMaintMasterId = prevMaintMaster.PrevMaintMasterId,
                                CalendarSlack = pmdata?.CalendarSlack ?? 0,
                                ChargeToId = pmdata?.ChargeToId ?? 0,
                                ChargeType = pmdata?.ChargeType,
                                Crew = pmdata?.Crew,
                                CurrentWOComplete = (pmdata?.CurrentWOComplete!=null && pmdata?.CurrentWOComplete==DateTime.MinValue)?null: pmdata?.CurrentWOComplete,
                                DownRequired = pmdata?.DownRequired ?? false,
                                ExcludeDOW = pmdata?.ExcludeDOW,
                                Frequency = pmdata?.Frequency ?? 0,
                                FrequencyType = pmdata?.FrequencyType,
                                InactiveFlag = pmdata?.InactiveFlag ?? false,
                                JobPlan = pmdata?.JobPlan,
                                Last_WorkOrderId = 0,
                                MeterHighLevel = pmdata?.MeterHighLevel ?? 0,
                                MeterId = pmdata?.MeterId ?? 0,
                                MeterInterval = pmdata?.MeterInterval ?? 0,
                                MeterSlack = pmdata?.MeterSlack ?? 0,
                                MeterMethod = pmdata?.MeterMethod ?? string.Empty,
                                MeterLastDue = 0,
                                MeterLastDone = 0,
                                MeterLastReading = 0,
                                MeterLowLevel = pmdata?.MeterLowLevel ?? 0,
                                NextDueDate = pmdata.NextDueDate,
                                OnDemandGroup = string.Empty,
                                Priority = pmdata?.Priority ?? string.Empty,
                                ScheduleMethod = pmdata?.ScheduleMethod,
                                ScheduleType = pmdata?.ScheduleType,
                                ScheduleWeeks = pmdata?.ScheduleWeeks,
                                Section = pmdata?.Section ?? string.Empty,
                                Shift = pmdata?.Shift ?? string.Empty,
                                Type = pmdata?.Type ?? string.Empty,
                                Category = pmdata?.Category ?? string.Empty,
                                RootCauseCode = pmdata?.RootCauseCode ?? string.Empty,
                                ActionCode = pmdata?.ActionCode ?? string.Empty,
                                FailureCode = pmdata?.FailureCode ?? string.Empty,
                                Planner_PersonnelId = pmdata?.Planner_PersonnelId ?? 0
                            };
                            prevMaintSched.CreateByForeignKeys_V2(userData.DatabaseKey);

                            // --- Create PMSchedUDF ---
                            if (prevMaintSched.ErrorMessages == null)
                            {
                                var pMSchedUDFResult = new PMSchedUDF
                                {
                                    ClientId = userData.DatabaseKey.Client.ClientId,
                                    PrevMaintSchedId = pmdata.PrevMaintSchedId
                                }.RetrivePMSchedUDFByPrevMaintSchedId(userData.DatabaseKey);

                                if (pMSchedUDFResult.PrevMaintSchedId == pmdata.PrevMaintSchedId)
                                {
                                    var pMSchedUDF = new PMSchedUDF
                                    {
                                        Text1 = pMSchedUDFResult.Text1,
                                        Text2 = pMSchedUDFResult.Text2,
                                        Text3 = pMSchedUDFResult.Text3,
                                        Text4 = pMSchedUDFResult.Text4,
                                        Date1 = (pMSchedUDFResult.Date1 != null && pMSchedUDFResult.Date1 == DateTime.MinValue) ? null : pMSchedUDFResult.Date1,
                                        Date2 = (pMSchedUDFResult.Date2 != null && pMSchedUDFResult.Date2 == DateTime.MinValue) ? null : pMSchedUDFResult.Date2,
                                        Date3 = (pMSchedUDFResult.Date3 != null && pMSchedUDFResult.Date3 == DateTime.MinValue) ? null : pMSchedUDFResult.Date3,
                                        Date4 = (pMSchedUDFResult.Date4 != null && pMSchedUDFResult.Date4 == DateTime.MinValue) ? null : pMSchedUDFResult.Date4,
                                        Bit1 = pMSchedUDFResult.Bit1,
                                        Bit2 = pMSchedUDFResult.Bit2,
                                        Bit3 = pMSchedUDFResult.Bit3,
                                        Bit4 = pMSchedUDFResult.Bit4,
                                        Numeric1 = pMSchedUDFResult.Numeric1,
                                        Numeric2 = pMSchedUDFResult.Numeric2,
                                        Numeric3 = pMSchedUDFResult.Numeric3,
                                        Numeric4 = pMSchedUDFResult.Numeric4,
                                        Select1 = pMSchedUDFResult.Select1,
                                        Select2 = pMSchedUDFResult.Select2,
                                        Select3 = pMSchedUDFResult.Select3,
                                        Select4 = pMSchedUDFResult.Select4,
                                        PrevMaintSchedId = prevMaintSched.PrevMaintSchedId
                                    };
                                    pMSchedUDF.Create(_dbKey);
                                }
                                //---Create PMSchedAssign---//
                                PMSchedAssign pMSchedAssign = new PMSchedAssign
                                {
                                    ClientId = userData.DatabaseKey.Client.ClientId,
                                    PrevMaintSchedId = pmdata.PrevMaintSchedId
                                };
                                var pMSchedAssignList = pMSchedAssign.RetrieveByPMSchedId_V2(userData.DatabaseKey);
                                if (pMSchedAssignList != null)
                                {
                                    foreach (var data in pMSchedAssignList)
                                    {
                                        pMSchedAssign = new PMSchedAssign
                                        {
                                            ClientId = userData.DatabaseKey.Client.ClientId,
                                            PrevMaintSchedId = prevMaintSched.PrevMaintSchedId,
                                            PersonnelId = data.PersonnelId,
                                            ScheduledHours = data.ScheduledHours
                                        };
                                        pMSchedAssign.Create(userData.DatabaseKey);
                                    }
                                }
                            }
                        }
                    }

                    // --- Create PrevMaintTask ---
                    if (_PrevMaintModel.Copy_Tasks)
                    {
                        var prevMaintTasklist = new PrevMaintTask().PrevMaintTaskRetrieveByPrevMaintMasterId(userData.DatabaseKey, _PrevMaintModel.PrevMaintMasterId);
                        if (prevMaintTasklist != null)
                        {
                            foreach (var pmTaskData in prevMaintTasklist)
                            {
                                var pmtask = new PrevMaintTask
                                {
                                    PrevMaintMasterId = prevMaintMaster.PrevMaintMasterId,
                                    ClientId = userData.DatabaseKey.Client.ClientId,
                                    SiteId = userData.DatabaseKey.User.DefaultSiteId,
                                    Description = pmTaskData.Description,
                                    TaskNumber = pmTaskData.TaskNumber
                                };
                                pmtask.CreateByForeignKeys(userData.DatabaseKey);
                            }
                        }
                    }

                    // --- Create EstimatedCosts ---
                    if (_PrevMaintModel.Copy_EstimatedCost)
                    {
                        var estimatecost = new EstimatedCosts
                        {
                            ClientId = userData.DatabaseKey.Client.ClientId,
                            ObjectId = _PrevMaintModel.PrevMaintMasterId,
                            ObjectType = SearchCategoryConstants.TBL_PREVMAINTMASTER
                        };
                        var ecList = estimatecost.RetriveByObjectId_V2(userData.DatabaseKey);
                        if (ecList != null)
                        {
                            foreach (var ecData in ecList)
                            {
                                var newEstimateCost = new EstimatedCosts
                                {
                                    ClientId = userData.DatabaseKey.Client.ClientId,
                                    ObjectId = prevMaintMaster.PrevMaintMasterId,
                                    ObjectType = SearchCategoryConstants.TBL_PREVMAINTMASTER,
                                    Category = ecData.Category,
                                    CategoryId = ecData.CategoryId,
                                    Description = ecData.Description,
                                    Duration = ecData.Duration,
                                    UnitCost = ecData.UnitCost,
                                    Quantity = ecData.Quantity,
                                    Source = ecData.Source,
                                    VendorId = ecData.VendorId
                                };
                                newEstimateCost.Create(userData.DatabaseKey);
                            }
                        }
                    }

                    // --- Create Notes ---
                    if (_PrevMaintModel.Copy_Notes)
                    {
                        var note = new Notes
                        {
                            ObjectId = _PrevMaintModel.PrevMaintMasterId,
                            TableName = AttachmentTableConstant.PreventiveMaintenance
                        };
                        var NotesList = note.RetrieveByObjectId(userData.DatabaseKey, userData.Site.TimeZone);
                        if (NotesList != null)
                        {
                            foreach (var noteData in NotesList)
                            {
                                var newNote = new Notes
                                {
                                    OwnerId = noteData.OwnerId,
                                    OwnerName = noteData.OwnerName,
                                    Subject = noteData.Subject,
                                    Content = noteData.Content,
                                    Type = noteData.Type,
                                    ObjectId = prevMaintMaster.PrevMaintMasterId,
                                    TableName = AttachmentTableConstant.PreventiveMaintenance
                                };
                                newNote.Create(userData.DatabaseKey);
                            }
                        }
                    }
                }
            }

            return prevMaintMaster;
        }
        #endregion

        #region

        public List<ScheduleRecords> GetScheduleRecordsChunkSearch_V2(long PrevMasterID, int skip = 0, int length = 0, string orderbycol = "", string orderDir = "", string ChargeToClientLookupId = "", string ChargeToName = "", string Frequency = "", DateTime? NextDueDate = null, string WorkOrder_ClientLookupId = "", DateTime? LastScheduled = null, DateTime? LastPerformed = null, string Meter_ClientLookupId = "", string OnDemandGroup = "")
        {
            List<ScheduleRecords> ScheduleRecordsList = new List<ScheduleRecords>();
            ScheduleRecords objScheduleRecords;

            PrevMaintSched pms = new PrevMaintSched()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                PrevMaintMasterId = PrevMasterID,
                SiteId = userData.DatabaseKey.User.DefaultSiteId
            };
            pms.Offset = skip;
            pms.Nextrow = length;
            pms.OrderbyColumn = orderbycol;
            pms.OrderBy = orderDir;
            pms.ChargeToClientLookupId = ChargeToClientLookupId;
            pms.ChargeToName = ChargeToName;
            pms.FrequencyWithType = Frequency;
            pms.NextDueDate = NextDueDate;
            pms.WorkOrder_ClientLookupId = WorkOrder_ClientLookupId;
            pms.LastScheduled = LastScheduled;
            pms.LastPerformed = LastPerformed;
            pms.Meter_ClientLookupId = Meter_ClientLookupId;
            pms.OnDemandGroup = OnDemandGroup;
            List<PrevMaintSched> pmsList = PrevMaintSched.RetrieveByPrevMaintMasterIdChunkSearch_V2(userData.DatabaseKey, pms);
            var FrequencyTypeList = UtilityFunction.populateFrequencyTypeList();
            foreach (var pm in pmsList)
            {
                objScheduleRecords = new ScheduleRecords();
                objScheduleRecords.ChargeType = pm.ChargeType;
                objScheduleRecords.Scheduled = pm.Scheduled;
                objScheduleRecords.PrevMaintScheId = pm.PrevMaintSchedId;
                objScheduleRecords.ChargeToClientLookupId = pm.ChargeToClientLookupId;
                objScheduleRecords.ChargeToName = pm.ChargeToName;
                objScheduleRecords.AssignedTo_PersonnelClientLookupId = pm.AssignedTo_PersonnelClientLookupId;
                objScheduleRecords.Meter_ClientLookupId = pm.Meter_ClientLookupId;
                objScheduleRecords.Frequency = pm.Frequency;
                objScheduleRecords.FrequencyType = pm.FrequencyType;
                if (!string.IsNullOrWhiteSpace(objScheduleRecords.FrequencyType))
                {
                    string ft = FrequencyTypeList.FirstOrDefault(x => x.value == objScheduleRecords.FrequencyType)?.text;
                    if (string.IsNullOrWhiteSpace(ft))
                    {
                        ft = objScheduleRecords.FrequencyType;
                    }
                    objScheduleRecords.FrequencyType = ft;
                }

                if (pm.LastPerformed != null && pm.LastPerformed == default(DateTime))
                {
                    objScheduleRecords.LastPerformed = null;
                }
                else
                {
                    objScheduleRecords.LastPerformed = pm.LastPerformed;
                }

                if (pm.NextDueDate != null && pm.NextDueDate == default(DateTime))
                {
                    objScheduleRecords.NextDueDate = null;
                    objScheduleRecords.NextDueDateString = string.Empty;
                }
                else
                {
                    objScheduleRecords.NextDueDate = pm.NextDueDate;
                    objScheduleRecords.NextDueDateString = pm.NextDueDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                objScheduleRecords.Last_WorkOrderId = pm.Last_WorkOrderId;
                objScheduleRecords.OnDemandGroup = pm.OnDemandGroup;
                objScheduleRecords.WorkOrder_ClientLookupId = pm.WorkOrder_ClientLookupId;
                if (pm.LastScheduled != null && pm.LastScheduled == default(DateTime))
                {
                    objScheduleRecords.LastScheduled = null;
                }
                else
                {
                    objScheduleRecords.LastScheduled = pm.LastScheduled;
                }
                objScheduleRecords.ChildCount = pm.ChildCount;
                objScheduleRecords.PlanningRequired = pm.PlanningRequired;
                objScheduleRecords.TotalCount = pm.TotalCount;

                ScheduleRecordsList.Add(objScheduleRecords);
            }
            return ScheduleRecordsList;
        }
        #endregion
    }
}