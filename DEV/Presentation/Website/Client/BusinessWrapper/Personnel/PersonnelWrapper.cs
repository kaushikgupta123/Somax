using Client.Models.Personnel;

using DataContracts;
using System.Linq;
using System;
using System.Collections.Generic;

namespace Client.BusinessWrapper
{
    public class PersonnelWrapper
    {
        private DatabaseKey _dbKey;
        private UserData _userData;
        List<string> errorMessage = new List<string>();

        public PersonnelWrapper(UserData userData)
        {
            _userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        #region Search
        internal List<PersonnelModel> GetPersonnelGridData(int skip = 0, int length = 0, string orderbycol = "", string orderDir = "", string ClientLookUpId = "",
            string Name = "", string Shift = "", string searchText = "", int inactiveFlag = 0,int AssignedAssetGroup1 = 0, int AssignedAssetGroup2 = 0, int AssignedAssetGroup3 = 0)
        {
            PersonnelModel personnelModel;
            List<PersonnelModel> personnelList = new List<PersonnelModel>();

            Personnel personnel = new Personnel();
            personnel.SiteId = _userData.DatabaseKey.User.DefaultSiteId;
            personnel.OffSetVal = skip;
            personnel.NextRow = length;
            personnel.OrderbyColumn = orderbycol;
            personnel.OrderBy = orderDir;
            personnel.ClientLookupId = ClientLookUpId;
            personnel.Name = Name;
            personnel.Shift = Shift;
            //personnel.ScheduleGroup = ScheduleGroup;
            personnel.SearchText = searchText;
            personnel.InActiveStatus = inactiveFlag; //V2-1098
            #region 1108
            personnel.AssignedAssetGroup1 = AssignedAssetGroup1;
            personnel.AssignedAssetGroup2 = AssignedAssetGroup2;
            personnel.AssignedAssetGroup3 = AssignedAssetGroup3;
            #endregion 
            var result = personnel.RetrieveChunkSearchV2(_dbKey);

            foreach (var item in result.listOfPersonnel)
            {
                personnelModel = new PersonnelModel();
                personnelModel.PersonnelId = item.PersonnelId;
                personnelModel.ClientLookupId = item.ClientLookupId;
                personnelModel.FirstName = item.NameFirst;
                personnelModel.LastName = item.NameLast;
                personnelModel.CraftId = item.CraftId;
                personnelModel.Shift = item.Shift;
                personnelModel.ShiftDescription = item.SchiptDescription?.Trim();
                personnelModel.Crew = item.Crew;
                personnelModel.CrewDescription = item.CrewDescription?.Trim();
                personnelModel.CraftClientLookupId = item.CraftClientLookupId.Trim();
                personnelModel.ScheduleGroup = item.ScheduleGroup;
                personnelModel.ScheduleGroupDescription = item.ScheduleGroupDescription?.Trim();
                personnelModel.TotalCount = item.TotalCount;
                #region 1108
                personnelModel.AssetGroup1Names = item.AssignedAssetGroup1ClientlookupId;
                personnelModel.AssetGroup2Names = item.AssignedAssetGroup2ClientlookupId;
                personnelModel.AssetGroup3Names = item.AssignedAssetGroup3ClientlookupId;
                #endregion 
                personnelList.Add(personnelModel);
            }
            return personnelList;
        }

        #endregion

        #region Details
        public PersonnelModel getPersonnelDetailsById(long PersonnelId, out AuxiliaryInformationModel informationModel, out AssetGroupMasterQuery assetGroupMasterQuery)
        {
            PersonnelModel personnelModel = new PersonnelModel();
            Personnel personnel = new Personnel()
            {
                ClientId = _dbKey.Client.ClientId,
                PersonnelId = PersonnelId,
                SiteId = _userData.DatabaseKey.User.DefaultSiteId
            };
            personnel.RetrieveByPKForeignKeys_V2(this._userData.DatabaseKey);
            personnelModel = initializeControls(personnel, out informationModel, out assetGroupMasterQuery);
            return personnelModel;
        }
        public PersonnelModel initializeControls(Personnel obj, out AuxiliaryInformationModel information, out AssetGroupMasterQuery assetGroupMasterQuery)
        {
            PersonnelModel objpersonnel = new PersonnelModel();
            AuxiliaryInformationModel auxiliaryInformation = new AuxiliaryInformationModel();
            assetGroupMasterQuery = new AssetGroupMasterQuery();
            objpersonnel.PersonnelId = obj.PersonnelId;
            objpersonnel.ClientLookupId = obj.ClientLookupId;
            objpersonnel.FirstName = obj.NameFirst;
            objpersonnel.MiddleName = obj?.NameMiddle.Trim() ?? string.Empty;
            objpersonnel.LastName = obj.NameLast;
            objpersonnel.DepartmentDescription = obj?.DepartmentDescription.Trim() ?? string.Empty;
            objpersonnel.Deptid = obj?.DepartmentId ?? 0;
            objpersonnel.CraftDescription = obj?.CraftDescription.Trim() ?? string.Empty;
            objpersonnel.CraftId = obj?.CraftId ?? 0;
            objpersonnel.Shift = obj?.Shift ?? string.Empty;
            objpersonnel.Crew = obj?.Crew ?? string.Empty;
            objpersonnel.CrewDescription = obj?.CrewDescription.Trim() ?? string.Empty;
            objpersonnel.ScheduleGroup = obj?.ScheduleGroup ?? string.Empty;
            objpersonnel.Planner = obj.Planner;
            objpersonnel.ShiftDescription = obj.SchiptDescription?.Trim() ?? string.Empty;
            objpersonnel.CraftDescription = obj.CraftDescription?.Trim() ?? string.Empty;
            objpersonnel.ScheduleGroupDescription = obj.ScheduleGroupDescription?.Trim() ?? string.Empty;
            objpersonnel.AssetGroup1Names = obj.AssetGroup1Names;
            objpersonnel.AssetGroup2Names = obj.AssetGroup2Names;
            objpersonnel.AssetGroup3Names = obj.AssetGroup3Names;
            objpersonnel.ScheduleEmployee = obj.ScheduleEmployee;
            objpersonnel.ExternalId = obj.ExOracleUserId?.Trim() ?? string.Empty; //V2-831
            objpersonnel.InactiveFlag= obj.InactiveFlag; //V2-1098
            #region 1108
            objpersonnel.AssignedAssetGroup1 = obj.AssignedAssetGroup1;
            objpersonnel.AssignedAssetGroup2 = obj.AssignedAssetGroup2;
            objpersonnel.AssignedAssetGroup3 = obj.AssignedAssetGroup3;
            objpersonnel.AssignedAssetGroup1Names = obj.AssignedAssetGroup1Names;
            objpersonnel.AssignedAssetGroup2Names = obj.AssignedAssetGroup2Names;
            objpersonnel.AssignedAssetGroup3Names = obj.AssignedAssetGroup3Names;
            #endregion
            objpersonnel.Default_StoreroomId = obj?.Default_StoreroomId??0; //V2-1178  
            objpersonnel.DefaultStoreroom = obj.DefaultStoreroom; //V2-1178
            if (obj.StartDate != null && obj.StartDate.Value != default(DateTime))
            {
                auxiliaryInformation.StartDate = obj.StartDate;
            }
            else
            {
                auxiliaryInformation.StartDate = null;
            }
            if (obj.LastSalaryReview != null && obj.LastSalaryReview.Value != default(DateTime))
            {
                auxiliaryInformation.LastSalaryReview = obj.LastSalaryReview;
            }
            else
            {
                auxiliaryInformation.LastSalaryReview = null;
            }
            auxiliaryInformation.BasePay = obj.BasePay;
            auxiliaryInformation.InitialPay = obj.InitialPay;
            auxiliaryInformation.PersonnelId = obj.PersonnelId;
            information = auxiliaryInformation;

            assetGroupMasterQuery.PersonnelId = obj.PersonnelId;
            assetGroupMasterQuery.AssetGroup1Ids = obj.AssetGroup1.Split(',').ToList();
            assetGroupMasterQuery.AssetGroup2Ids = obj.AssetGroup2.Split(',').ToList();
            assetGroupMasterQuery.AssetGroup3Ids = obj.AssetGroup3.Split(',').ToList();           

            return objpersonnel;
        }
        #endregion

        #region Edit
        internal List<string> UpdatePersonnel(PersonnelModel obj)
        {
            Personnel personnel = new Personnel()
            {
                ClientId = this._userData.DatabaseKey.Client.ClientId,
                PersonnelId = obj.PersonnelId
            };
            personnel.Retrieve(this._userData.DatabaseKey);

            personnel.ClientLookupId = personnel.ClientLookupId;
            personnel.NameFirst = obj.FirstName ?? string.Empty;
            personnel.NameMiddle = obj?.MiddleName ?? string.Empty;
            personnel.NameLast = obj?.LastName ?? string.Empty;            
            personnel.DepartmentId = obj?.Deptid ?? 0;
            personnel.CraftId = obj?.CraftId ?? 0;
            personnel.Shift = obj?.Shift ?? string.Empty;
            personnel.Crew = obj?.Crew ?? string.Empty;
            personnel.ScheduleGroup = obj?.ScheduleGroup ?? string.Empty;
            personnel.Planner = obj.Planner;
            personnel.ScheduleEmployee = obj.ScheduleEmployee;
            personnel.ExOracleUserId = obj.ExternalId ?? string.Empty; ; //V2-831         
            //personnel.UpdateIndex = obj.UpdateIndex;
            #region V2-1108
            personnel.AssignedAssetGroup1 = obj?.AssignedAssetGroup1??0;
            personnel.AssignedAssetGroup2 = obj?.AssignedAssetGroup2??0;
            personnel.AssignedAssetGroup3 = obj?.AssignedAssetGroup3??0;
            #endregion           
            personnel.Default_StoreroomId = obj.Default_StoreroomId??0;//V2-1178
            personnel.Update(this._userData.DatabaseKey);
            return personnel.ErrorMessages;
        }
        #endregion

        #region Events
        public List<EventsModel> PopulateEventsList(long PersonnelId)
        {
            List<EventsModel> eventList = new List<EventsModel>();
            EventsModel eventmodel;
            Personnel personnel = new Personnel()
            {
                ClientId = _dbKey.Client.ClientId,
                PersonnelId = PersonnelId,
                SiteId = _userData.DatabaseKey.User.DefaultSiteId
            };
            List<Personnel> lst = Personnel.RetriveEventsByPersonnelId(this._userData.DatabaseKey, personnel);

            foreach (var item in lst)
            {
                eventmodel = new EventsModel();
                eventmodel.EventsId = item.EventsId;
                eventmodel.Type = item.Type;
                eventmodel.Description = item.Description;
                eventmodel.CompleteDate = item.CompleteDate;
                eventmodel.ExpireDate = item.ExpireDate;
                eventList.Add(eventmodel);
            }
            return eventList;
        }

        public List<string> AddEvent(EventsModel EventsModel)
        {
            Events events = new Events()
            {
                ClientId = _dbKey.Client.ClientId,
                SiteId = _userData.DatabaseKey.User.DefaultSiteId,
                PersonnelId = EventsModel.PersonnelId,
                Type = EventsModel.Type,
                Description = EventsModel.Description,
                CompleteDate = EventsModel.CompleteDate,
                ExpireDate = EventsModel.ExpireDate ?? null
            };

            events.Create(this._userData.DatabaseKey);
            return events.ErrorMessages;
        }

        public EventsModel RetriveEventById(long? EventsId, long personnelId)
        {
            EventsModel retObj = new EventsModel();
            Events events = new Events()
            {
                EventsId = EventsId ?? 0
            };
            events.Retrieve(this._userData.DatabaseKey);
            retObj = PopulateEventModel(events, personnelId);
            return retObj;
        }

        internal EventsModel PopulateEventModel(Events aobj, long personnelId)
        {
            EventsModel oModel = new EventsModel();
            oModel.EventsId = aobj.EventsId;
            oModel.Type = aobj.Type;
            oModel.Description = aobj.Description;
            oModel.CompleteDate = aobj.CompleteDate;
            oModel.ExpireDate = aobj.ExpireDate ?? null;
            oModel.PersonnelId = personnelId;
            return oModel;
        }

        public List<String> UpdateEvent(EventsModel objEvent)
        {
            Events events = new Events()
            {
                EventsId = objEvent.EventsId
            };
            events.Retrieve(this._userData.DatabaseKey);


            events.Type = objEvent.Type;
            events.Description = objEvent.Description ?? string.Empty;
            events.CompleteDate = objEvent.CompleteDate;
            events.ExpireDate = objEvent.ExpireDate ?? null;

            events.Update(this._userData.DatabaseKey);
            return events.ErrorMessages;
        }

        public bool DeleteEvent(long EventsId)
        {
            try
            {
                Events Events = new Events() { EventsId = EventsId };
                Events.Delete(this._userData.DatabaseKey);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Availability
        internal List<PersonnelAvailabilityModel> GetPersonnelAvailabilityGridData(long personnelId)
        {
            PersonnelAvailabilityModel personnelAvailabilityModel;
            List<PersonnelAvailabilityModel> personnelAvailabilityList = new List<PersonnelAvailabilityModel>();

            Personnel personnel = new Personnel();
            personnel.SiteId = _userData.DatabaseKey.User.DefaultSiteId;
            personnel.ClientId = _userData.DatabaseKey.User.ClientId;
            personnel.PersonnelId = personnelId;


            var result = Personnel.RetrivePersonnelAvailabilityByPersonnelId(_userData.DatabaseKey, personnel);

            foreach (var item in result)
            {
                personnelAvailabilityModel = new PersonnelAvailabilityModel();
                personnelAvailabilityModel.PersonnelAvailabilityId = item.PersonnelAvailabilityId;
                personnelAvailabilityModel.PersonnelId = item.PersonnelId;
                personnelAvailabilityModel.PersonnelAvailabilityDate = item.PersonnelAvailabilityDate;
                personnelAvailabilityModel.PAHours = item.PAHours;
                personnelAvailabilityModel.PAShift = item.PAShift;
                personnelAvailabilityList.Add(personnelAvailabilityModel);
            }
            return personnelAvailabilityList;
        }

        public List<string> AddPersonnelAvailability(PersonnelAvailabilityModel personnelAvailabilityModel)
        {
            PersonnelAvailability personnelAvailability = new PersonnelAvailability();
            personnelAvailability.CallerUserInfoId = _userData.DatabaseKey.User.CallerUserInfoId;
            personnelAvailability.CallerUserName = _userData.DatabaseKey.User.CallerUserName;
            personnelAvailability.PersonnelAvailabilityId = personnelAvailabilityModel.PersonnelAvailabilityId ?? 0;
            personnelAvailability.ClientId = _userData.DatabaseKey.User.ClientId;
            personnelAvailability.SiteId = _userData.DatabaseKey.User.DefaultSiteId;
            personnelAvailability.PersonnelId = personnelAvailabilityModel.PersonnelId;
            personnelAvailability.Date = personnelAvailabilityModel.PersonnelAvailabilityDate;
            personnelAvailability.Hours = personnelAvailabilityModel.PAHours;
            personnelAvailability.Shift = personnelAvailabilityModel.PAShift;
            personnelAvailability.Create(_userData.DatabaseKey);
            return personnelAvailability.ErrorMessages;
        }

        public List<string> UpdatePersonnelAvailability(PersonnelAvailabilityModel personnelAvailabilityModel)
        {
            PersonnelAvailability personnelAvailability = new PersonnelAvailability();
            personnelAvailability.CallerUserInfoId = _userData.DatabaseKey.User.CallerUserInfoId;
            personnelAvailability.CallerUserName = _userData.DatabaseKey.User.CallerUserName;
            personnelAvailability.PersonnelAvailabilityId = personnelAvailabilityModel.PersonnelAvailabilityId ?? 0;
            personnelAvailability.ClientId = _userData.DatabaseKey.User.ClientId;
            personnelAvailability.SiteId = _userData.DatabaseKey.User.DefaultSiteId;
            personnelAvailability.PersonnelId = personnelAvailabilityModel.PersonnelId;
            personnelAvailability.Date = personnelAvailabilityModel.PersonnelAvailabilityDate;
            personnelAvailability.Hours = personnelAvailabilityModel.PAHours;
            personnelAvailability.Shift = personnelAvailabilityModel.PAShift;
            personnelAvailability.PersonnelAvailabilityId = personnelAvailabilityModel.PersonnelAvailabilityId ?? 0;
            personnelAvailability.Update(_userData.DatabaseKey);
            return personnelAvailability.ErrorMessages;
        }

        public PersonnelAvailability RetrievePersonnelAvailability(PersonnelAvailabilityModel personnelAvailabilityModel)
        {
            PersonnelAvailability personnelAvailability = new PersonnelAvailability();
            personnelAvailability.PersonnelAvailabilityId = personnelAvailabilityModel.PersonnelAvailabilityId ?? 0;
            personnelAvailability.ClientId = _userData.DatabaseKey.User.ClientId;
            personnelAvailability.Retrieve(_userData.DatabaseKey);
            return personnelAvailability;
        }

        public List<string> DeletePersonnelAvailability(PersonnelAvailabilityModel personnelAvailabilityModel)
        {
            PersonnelAvailability personnelAvailability = new PersonnelAvailability();
            personnelAvailability.PersonnelAvailabilityId = personnelAvailabilityModel.PersonnelAvailabilityId ?? 0;
            personnelAvailability.ClientId = _userData.DatabaseKey.User.ClientId;
            personnelAvailability.Delete(_userData.DatabaseKey);
            return personnelAvailability.ErrorMessages;
        }
        #endregion

        #region Labor
        public List<LaborModel> PopulateLaborList(long PersonnelId)
        {
            List<LaborModel> laborList = new List<LaborModel>();
            LaborModel labormodel;
            Personnel personnel = new Personnel()
            {
                ClientId = _dbKey.Client.ClientId,
                PersonnelId = PersonnelId,
                SiteId = _userData.DatabaseKey.User.DefaultSiteId
            };
            List<Personnel> lst = Personnel.RetriveLaborsByPersonnelId(this._userData.DatabaseKey, personnel);

            foreach (var item in lst)
            {
                labormodel = new LaborModel();
                labormodel.TimecardId = item.TimecardId;
                labormodel.ChargeTo = item.WOClientLookupId;
                labormodel.Date = item.laborstartdate;
                labormodel.Hours = item.Hours;
                labormodel.Cost = item.Value;

                laborList.Add(labormodel);
            }
            return laborList;
        }
        #endregion

        #region Attendance
        public List<PersonnelAttendanceModel> PopulateAttendance(long PersonnelId)
        {
            PersonnelAttendanceModel objPersonnelAttendanceModel;
            List<PersonnelAttendanceModel> PersonnelAttendanceModelList = new List<PersonnelAttendanceModel>();
            Personnel personnel = new Personnel();
            personnel.SiteId = _userData.DatabaseKey.User.DefaultSiteId;
            personnel.PersonnelId = PersonnelId;

            List<Personnel> personnelList = Personnel.RetrivePersonnelAttendanceByPersonnelId(_userData.DatabaseKey, personnel);
            foreach (var item in personnelList)
            {
                objPersonnelAttendanceModel = new PersonnelAttendanceModel();
                objPersonnelAttendanceModel.PersonnelAttendanceId = item.PersonnelAttendanceId;
                if (item.PersonnelAttendDate != null && item.PersonnelAttendDate != default(DateTime))
                {
                    objPersonnelAttendanceModel.Date = item.PersonnelAttendDate;
                }
                else
                {
                    objPersonnelAttendanceModel.Date = null;
                }
                objPersonnelAttendanceModel.Hours = item.PersonnelAttendHours;
                objPersonnelAttendanceModel.Shift = item.PersonnelAttendShift;
                objPersonnelAttendanceModel.ShiftDescription = item.PersonnelAttendShiftDecription;
                PersonnelAttendanceModelList.Add(objPersonnelAttendanceModel);
            }
            return PersonnelAttendanceModelList;
        }
        public List<string> AddAttendance(PersonnelAttendanceModel PersonnelAttendanceModel)
        {
            PersonnelAttendance personnelAttendance = new PersonnelAttendance()
            {
                ClientId = _dbKey.Client.ClientId,
                SiteId = _userData.DatabaseKey.User.DefaultSiteId,
                PersonnelId = PersonnelAttendanceModel.PersonnelId,
                Date = PersonnelAttendanceModel.Date,
                Hours = PersonnelAttendanceModel.Hours,
                Shift = PersonnelAttendanceModel.Shift,
            };

            personnelAttendance.Create(this._userData.DatabaseKey);
            return personnelAttendance.ErrorMessages;
        }
        public PersonnelAttendanceModel RetrivePersonnelAttendaceById(long? PersonnelAttendanceId, long personnelId)
        {
            PersonnelAttendanceModel retObj = new PersonnelAttendanceModel();
            PersonnelAttendance personnelAttendance = new PersonnelAttendance()
            {
                PersonnelAttendanceId = PersonnelAttendanceId ?? 0
            };
            personnelAttendance.Retrieve(this._userData.DatabaseKey);
            retObj = PopulateAttendanceModel(personnelAttendance, personnelId);
            return retObj;
        }
        internal PersonnelAttendanceModel PopulateAttendanceModel(PersonnelAttendance aobj, long personnelId)
        {
            PersonnelAttendanceModel oModel = new PersonnelAttendanceModel();
            oModel.PersonnelAttendanceId = aobj.PersonnelAttendanceId;
            if (aobj.Date != null && aobj.Date != default(DateTime))
            {
                oModel.Date = aobj.Date;
            }
            else
            {
                oModel.Date = null;
            }
            oModel.Hours = aobj.Hours;
            oModel.Shift = aobj.Shift;
            oModel.PersonnelId = personnelId;
            return oModel;
        }
        public List<String> UpdateAttendance(PersonnelAttendanceModel personnelAttendanceModel)
        {
            PersonnelAttendance personnelAttendance = new PersonnelAttendance()
            {
                PersonnelAttendanceId = personnelAttendanceModel.PersonnelAttendanceId
            };
            personnelAttendance.Retrieve(this._userData.DatabaseKey);


            personnelAttendance.Date = personnelAttendanceModel.Date;
            personnelAttendance.Hours = personnelAttendanceModel.Hours;
            personnelAttendance.Shift = personnelAttendanceModel.Shift;
            personnelAttendance.Update(this._userData.DatabaseKey);
            return personnelAttendance.ErrorMessages;
        }

        public bool DeleteAttendance(long PersonnelAttendanceId)
        {
            try
            {
                PersonnelAttendance personnelAttendance = new PersonnelAttendance() { PersonnelAttendanceId = PersonnelAttendanceId };
                personnelAttendance.Delete(this._userData.DatabaseKey);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Auxiliary-Information
        public List<string> UpdateAuxiliaryInformation(AuxiliaryInformationModel obj)
        {

            Personnel personnel = new Personnel()
            {
                ClientId = _dbKey.Client.ClientId,
                PersonnelId = obj.PersonnelId,
                SiteId = _userData.DatabaseKey.User.DefaultSiteId
            };
            personnel.Retrieve(this._userData.DatabaseKey);
            personnel.StartDate = obj.StartDate;
            personnel.LastSalaryReview = obj.LastSalaryReview;
            personnel.BasePay = obj.BasePay ?? default(decimal);
            personnel.InitialPay = obj.InitialPay ?? default(decimal);
            personnel.Update(this._userData.DatabaseKey);
            return personnel.ErrorMessages;

        }
        #endregion

        #region Asset Group Master Query
        public List<string> UpdateAssetGroupMasterQuery(AssetGroupMasterQuery assetGroupMasterQuery)
        {
            Personnel personnel = new Personnel()
            {
                ClientId = _dbKey.Client.ClientId,
                PersonnelId = assetGroupMasterQuery.PersonnelId,
                SiteId = _userData.DatabaseKey.User.DefaultSiteId
            };
            personnel.Retrieve(_userData.DatabaseKey);
            if (assetGroupMasterQuery.AssetGroup1Ids != null && assetGroupMasterQuery.AssetGroup1Ids.Count > 0)
            {
                personnel.AssetGroup1 = string.Join(",", assetGroupMasterQuery.AssetGroup1Ids.ToArray());
            }
            else
            {
                personnel.AssetGroup1 = "";
            }
            if (assetGroupMasterQuery.AssetGroup2Ids != null && assetGroupMasterQuery.AssetGroup2Ids.Count > 0)
            {
                personnel.AssetGroup2 = string.Join(",", assetGroupMasterQuery.AssetGroup2Ids.ToArray());
            }
            else
            {
                personnel.AssetGroup2 = "";
            }
            if (assetGroupMasterQuery.AssetGroup3Ids != null && assetGroupMasterQuery.AssetGroup3Ids.Count > 0)
            {
                personnel.AssetGroup3 = string.Join(",", assetGroupMasterQuery.AssetGroup3Ids.ToArray());
            }
            else
            {
                personnel.AssetGroup3 = "";
            }
            personnel.Update(_userData.DatabaseKey);
            return personnel.ErrorMessages;
        }
        #endregion

        #region Retrieve for WorkOrder completion wizard tab
        public Personnel RetrieveForWorkOrderCompletionWizard(long PersonnelId,decimal Hours)
        {
            //PersonnelModel objPersonnelModel;

            Personnel personnel = new Personnel()
            {
                PersonnelId = PersonnelId,
                ClientId = _userData.DatabaseKey.Client.ClientId,
                SiteId = _userData.Site.SiteId,
                Hours = Hours
            };
            personnel.RetrieveForWorkOrderCompletionWizard(_userData.DatabaseKey);
            return personnel;
        }
        #endregion
    }
}