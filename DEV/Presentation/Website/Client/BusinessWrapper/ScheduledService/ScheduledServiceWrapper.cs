using Client.BusinessWrapper.Common;
using Client.Common;
using Client.Common.Constants;
using Client.Models.FleetScheduledService;
using Common.Constants;
using Common.Extensions;
using DataContracts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Client.BusinessWrapper.ScheduledService
{
    public class ScheduledServiceWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        List<string> errorMessage = new List<string>();
        public ScheduledServiceWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }
        #region Search GridData
        public List<FleetScheduledServiceSearchModel> GetFleetScheduledServiceGridData(int CustomQueryDisplayId, string orderbycol = "", string orderDir = "", int skip = 0, int length = 0, string clientLookupId = "", string name = "", string serviceTasksDescription = "", string searchText = "")
        {
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            FleetScheduledServiceSearchModel scServSearchModel;
            List<FleetScheduledServiceSearchModel> scServSearchModelList = new List<FleetScheduledServiceSearchModel>();
            List<DataContracts.ScheduledService> equipmentList = new List<DataContracts.ScheduledService>();
            DataContracts.ScheduledService scheduledService = new DataContracts.ScheduledService();
            scheduledService.ClientId = this.userData.DatabaseKey.Client.ClientId;
            scheduledService.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            scheduledService.CustomQueryDisplayId = CustomQueryDisplayId;
            scheduledService.OrderbyColumn = orderbycol;
            scheduledService.OrderBy = orderDir;
            scheduledService.OffSetVal = skip;
            scheduledService.NextRow = length;
            scheduledService.ClientLookupId = clientLookupId;
            scheduledService.Name = name;
            scheduledService.ServiceTasksDescription = serviceTasksDescription;
            scheduledService.SearchText = searchText;
            equipmentList = scheduledService.ScheduledServiceRetrieveChunkSearchV2(userData.DatabaseKey, userData.Site.TimeZone);
            bool ClientOnPremise = userData.DatabaseKey.Client.OnPremise;
            foreach (var item in equipmentList)
            {
                scServSearchModel = new FleetScheduledServiceSearchModel();
                scServSearchModel.EquipmentId = item.EquipmentId;
                scServSearchModel.ScheduledServiceId = item.ScheduledServiceId;
                scServSearchModel.ClientLookupId = item.ClientLookupId;
                scServSearchModel.Name = item.Name;
                scServSearchModel.ServiceTask = item.ServiceTasksDescription;
                scServSearchModel.TimeInterval = item.TimeInterval;
                scServSearchModel.Meter1Interval = item.Meter1Interval;
                scServSearchModel.Meter2Interval = item.Meter2Interval;
                scServSearchModel.TimeIntervalType = item.TimeIntervalType;
                scServSearchModel.Meter1Units = item.Meter1Units;
                scServSearchModel.Meter2Units = item.Meter2Units;
                scServSearchModel.NextDueDate = item.NextDueDate;
                scServSearchModel.NextDueMeter1 = item.NextDueMeter1;
                scServSearchModel.NextDueMeter2 = item.NextDueMeter2;
                scServSearchModel.LastPerformedMeter1 = item.LastPerformedMeter1;
                scServSearchModel.LastPerformedMeter2 = item.LastPerformedMeter2;
                if (item.LastPerformedDate != null && item.LastPerformedDate == default(DateTime))
                {
                    scServSearchModel.LastPerformedDate = null;
                }
                else
                {
                    scServSearchModel.LastPerformedDate = item.LastPerformedDate;
                }
                scServSearchModel.LastCompletedstr = item.LastCompletedstr;
                scServSearchModel.InactiveFlag = item.InactiveFlag;
                scServSearchModel.ImageUrl = !string.IsNullOrEmpty(item.ImageUrl) ? item.ImageUrl : commonWrapper.GetNoImageUrl();
                if (ClientOnPremise)
                {
                    scServSearchModel.ImageUrl = UtilityFunction.PhotoBase64ImgSrc(scServSearchModel.ImageUrl);
                }
                scServSearchModel.TotalCount = item.TotalCount;
                scServSearchModelList.Add(scServSearchModel);
            }
            return scServSearchModelList;
        }
        #endregion
        #region Add Or Edit 
        public DataContracts.ScheduledService AddOrEditScheduledService(string SS_ClientLookupId, FleetScheduledServiceVM objFTM)
        {
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            FleetScheduledServiceModel objFleetAsset = new FleetScheduledServiceModel();
            List<string> errList = new List<string>();
            Equipment equipment = new Equipment { ClientId = _dbKey.Client.ClientId, SiteId = _dbKey.User.DefaultSiteId, ClientLookupId = SS_ClientLookupId };
            equipment.RetrieveByClientLookupId(_dbKey);
            DataContracts.ScheduledService scheduledService = new DataContracts.ScheduledService();
            if (objFTM.ScheduledServiceModel.ScheduledServiceId == 0)
            {
                #region Insert in Fleet Scheduled  Table
                scheduledService.ClientId = this.userData.DatabaseKey.User.ClientId;
                scheduledService.SiteId = this.userData.DatabaseKey.User.DefaultSiteId; ;
                scheduledService.EquipmentId = equipment.EquipmentId;
                scheduledService.AreaId = equipment.AreaId;
                scheduledService.DepartmentId = equipment.DepartmentId;
                scheduledService.StoreroomId = equipment.StoreroomId;
                scheduledService.ImageUrl = !string.IsNullOrEmpty(equipment.EquipImage) ? equipment.EquipImage : commonWrapper.GetNoImageUrl();
                scheduledService.ServiceTaskId = objFTM.ScheduledServiceModel.ServiceTaskId;
                scheduledService.Meter1Interval = objFTM.ScheduledServiceModel.Meter1Interval;
                scheduledService.Meter2Interval = objFTM.ScheduledServiceModel.Meter2Interval;
                scheduledService.TimeInterval = objFTM.ScheduledServiceModel.TimeInterval;
                scheduledService.Last_ServiceOrderId = 0;
                scheduledService.LastPerformedMeter1 = 0;
                scheduledService.LastPerformedMeter2 = 0;
                scheduledService.TimeIntervalType = objFTM.ScheduledServiceModel.TimeIntervalType;
                scheduledService.Meter1Threshold = objFTM.ScheduledServiceModel.Meter1Threshold;
                scheduledService.Meter2Threshold = objFTM.ScheduledServiceModel.Meter2Threshold;
                scheduledService.TimeThreshold = objFTM.ScheduledServiceModel.TimeThreshold;
                scheduledService.TimeThresoldType = objFTM.ScheduledServiceModel.TimeThresoldType;
                scheduledService.NextDueDate = objFTM.ScheduledServiceModel.NextDueDate;
                scheduledService.NextDueMeter1 = objFTM.ScheduledServiceModel.NextDueMeter1;
                scheduledService.NextDueMeter2 = objFTM.ScheduledServiceModel.NextDueMeter2;
                scheduledService.LastPerformedDate = objFTM.ScheduledServiceModel.LastPerformedDate;
                scheduledService.RepairReason = objFTM.ScheduledServiceModel.RepairReason;
                scheduledService.VMRSSystem = objFTM.ScheduledServiceModel.System;
                scheduledService.VMRSAssembly = objFTM.ScheduledServiceModel.Assembly;
                scheduledService.CheckDuplicateScheduledService(this.userData.DatabaseKey);

                if (scheduledService.ErrorMessages == null || scheduledService.ErrorMessages.Count == 0)
                {
                    scheduledService.CreateCustom(this.userData.DatabaseKey);
                }
                #endregion
            }
            else
            {
                FleetScheduledServiceModel objSchServ = new FleetScheduledServiceModel();
                string emptyValue = string.Empty;
                scheduledService = new DataContracts.ScheduledService()
                {
                    ClientId = this.userData.DatabaseKey.Client.ClientId,
                    ScheduledServiceId = Convert.ToInt64(objFTM.ScheduledServiceModel.ScheduledServiceId),
                    EquipmentId = Convert.ToInt64(objFTM.ScheduledServiceModel.EquipmentId)
                };
                scheduledService.ImageUrl = equipment.ImageUrl;
                scheduledService.ServiceTaskId = objFTM.ScheduledServiceModel.ServiceTaskId;
                scheduledService.Meter1Interval = objFTM.ScheduledServiceModel.Meter1Interval;
                scheduledService.Meter2Interval = objFTM.ScheduledServiceModel.Meter2Interval;
                scheduledService.TimeInterval = objFTM.ScheduledServiceModel.TimeInterval;
                scheduledService.TimeIntervalType = objFTM.ScheduledServiceModel.TimeIntervalType;
                scheduledService.Meter1Threshold = objFTM.ScheduledServiceModel.Meter1Threshold;
                scheduledService.Meter2Threshold = objFTM.ScheduledServiceModel.Meter2Threshold;
                scheduledService.TimeThreshold = objFTM.ScheduledServiceModel.TimeThreshold;
                scheduledService.TimeThresoldType = objFTM.ScheduledServiceModel.TimeThresoldType;
                scheduledService.NextDueDate = objFTM.ScheduledServiceModel.NextDueDate;
                scheduledService.NextDueMeter1 = objFTM.ScheduledServiceModel.NextDueMeter1;
                scheduledService.NextDueMeter2 = objFTM.ScheduledServiceModel.NextDueMeter2;
                scheduledService.RepairReason = objFTM.ScheduledServiceModel.RepairReason;
                scheduledService.VMRSSystem = objFTM.ScheduledServiceModel.System;
                scheduledService.VMRSAssembly = objFTM.ScheduledServiceModel.Assembly;
                scheduledService.UpdateCustom(this.userData.DatabaseKey);
            }

            return scheduledService;
        }

        public FleetScheduledServiceModel GetEditScheduledServiceDetailsById(long EquipmentId, long ScheduledServiceId)
        {
            FleetScheduledServiceModel objScheduledServiceModel = new FleetScheduledServiceModel();
            DataContracts.ScheduledService scheduledService = new DataContracts.ScheduledService()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.User.DefaultSiteId,
                EquipmentId = EquipmentId,
                ScheduledServiceId = ScheduledServiceId
            };
            scheduledService.RetrieveByEquipmentIdandScheduledServiceId(_dbKey);
            objScheduledServiceModel = initializeDetailsControls(scheduledService);
            return objScheduledServiceModel;
        }

        public FleetScheduledServiceModel initializeDetailsControls(DataContracts.ScheduledService obj)
        {
            FleetScheduledServiceModel objScheduledService = new FleetScheduledServiceModel();
            objScheduledService.ClientId = obj.ClientId;
            objScheduledService.ScheduledServiceId = obj.ScheduledServiceId;
            objScheduledService.ServiceTaskId = obj.ServiceTaskId;
            objScheduledService.Last_ServiceOrderId = obj.Last_ServiceOrderId;
            objScheduledService.LastPerformedDate = obj.LastPerformedDate;
            objScheduledService.LastPerformedMeter1 = obj.LastPerformedMeter1;
            objScheduledService.LastPerformedMeter2 = obj.LastPerformedMeter2;
            objScheduledService.Meter1Interval = obj.Meter1Interval;
            objScheduledService.Meter1Threshold = obj.Meter1Threshold;
            objScheduledService.Meter2Interval = obj.Meter2Interval;
            objScheduledService.Meter2Threshold = obj.Meter2Threshold;
            objScheduledService.NextDueDate = obj.NextDueDate;
            objScheduledService.NextDueMeter1 = obj.NextDueMeter1;
            objScheduledService.NextDueMeter2 = obj.NextDueMeter2;
            objScheduledService.TimeInterval = obj.TimeInterval;
            objScheduledService.TimeIntervalType = obj.TimeIntervalType;
            objScheduledService.TimeThreshold = obj.TimeThreshold;
            objScheduledService.TimeThresoldType = obj.TimeThresoldType;
            objScheduledService.CreateBy = obj.CreateBy;
            objScheduledService.CreateDate = obj.CreateDate;
            objScheduledService.ModifyBy = obj.ModifyBy;
            objScheduledService.ModifyDate = obj.ModifyDate;
            objScheduledService.EquipmentId = obj.EquipmentId;
            objScheduledService.ClientLookupId = obj.ClientLookupId;
            objScheduledService.AreaId = obj.AreaId;
            objScheduledService.DepartmentId = obj.DepartmentId;
            objScheduledService.StoreroomId = obj.StoreroomId;
            objScheduledService.Meter1Type = obj.Meter1Type;
            objScheduledService.Meter2Type = obj.Meter2Type;
            objScheduledService.Meter1Units = obj.Meter1Units;
            objScheduledService.Meter2Units = obj.Meter2Units;
            objScheduledService.RepairReason = obj.RepairReason;
            objScheduledService.System = obj.VMRSSystem;
            objScheduledService.Assembly = obj.VMRSAssembly;

            return objScheduledService;
        }
        #endregion

        #region Active/InActive Fleet Scheduled Service
        public List<string> UpdateSchServiceActiveStatus(long sheduledServiceId, bool inActiveFlag)
        {
            List<string> errList = new List<string>();
            DataContracts.ScheduledService schService = new DataContracts.ScheduledService()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ScheduledServiceId = sheduledServiceId
            };
            schService.Retrieve(userData.DatabaseKey);
            schService.InactiveFlag = !inActiveFlag;
            schService.SiteId = this.userData.DatabaseKey.User.DefaultSiteId;
            // this function will only be called at the time of inactivate
            if (schService.InactiveFlag)
            {
                schService.ValidateForActivateInactivate(userData.DatabaseKey);
            }
            
            if (schService.ErrorMessages == null || schService.ErrorMessages.Count <= 0)
            {
                schService.Update(userData.DatabaseKey);
            }
            if (schService.ErrorMessages == null || schService.ErrorMessages.Count <= 0)
            {
                return errList;
            }
            else
            {
                return schService.ErrorMessages;
            }
        }
        #endregion
        #region Fleet Only
        public int GetCount()
        {
            int count = 0;
            DataContracts.ScheduledService schServices = new DataContracts.ScheduledService()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.Personnel.SiteId
            };
            var scheduledServices = schServices.RetrieveDashboardChart(userData.DatabaseKey, schServices);
            if (scheduledServices != null && scheduledServices.Count > 0)
            {
                count = scheduledServices[0].PastDueServiceCount;
            }
            return count;
        }
        #endregion

    }
}