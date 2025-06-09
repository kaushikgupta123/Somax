using Client.BusinessWrapper.Common;
using Client.Models.FleetIssue;
using DataContracts;
using System;
using System.Collections.Generic;
using System.Globalization;
using Common.Constants;
using Common.Extensions;
using Client.Common;

namespace Client.BusinessWrapper.FleetIssue
{
    public class FleetIssueWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        List<string> errorMessage = new List<string>();
        public FleetIssueWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }
        #region Search
        public List<FleetIssueSearchModel> GetFleetIssueGridData(int CustomQueryDisplayId, string CreateStartDateVw = "", string CreateEndDateVw = "", string orderbycol = "", string orderDir = "", int skip = 0, int length = 0, string clientLookupId = "", string name = "", string make = "", string model = "", string vin = "", string startrecordDate = "", string endrecordDate = "", List<string> Defects = null, string searchText = "")
        {
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            FleetIssueSearchModel fleetIssueSearchModel;
            List<FleetIssueSearchModel> fleetIssueSearchModelList = new List<FleetIssueSearchModel>();
            List<FleetIssues> FleetIssuesList = new List<FleetIssues>();
            FleetIssues fleetIssues = new FleetIssues();
            fleetIssues.CustomQueryDisplayId = CustomQueryDisplayId;
            fleetIssues.ClientId = this.userData.DatabaseKey.Client.ClientId;
            fleetIssues.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            fleetIssues.OrderbyColumn = orderbycol;
            fleetIssues.OrderBy = orderDir;
            fleetIssues.OffSetVal = skip;
            fleetIssues.NextRow = length;
            fleetIssues.ClientLookupId = clientLookupId;
            fleetIssues.Name = name;
            fleetIssues.Make = make;
            fleetIssues.Model = model;
            fleetIssues.VIN = vin;
            fleetIssues.Defects = Defects != null && Defects.Count > 0 ? string.Join(",", Defects) : string.Empty;
            fleetIssues.RecordStartDate = startrecordDate;
            fleetIssues.RecordEndDate = endrecordDate;
            fleetIssues.CreateStartDateVw = CreateStartDateVw;
            fleetIssues.CreateEndDateVw = CreateEndDateVw;
            fleetIssues.SearchText = searchText;
            FleetIssuesList = fleetIssues.FleetIssueRetrieveChunkSearchV2(userData.DatabaseKey, userData.Site.TimeZone);
            bool ClientOnPremise = userData.DatabaseKey.Client.OnPremise;
            foreach (var item in FleetIssuesList)
            {
                fleetIssueSearchModel = new FleetIssueSearchModel();
                fleetIssueSearchModel.EquipmentId = item.EquipmentId;
                fleetIssueSearchModel.FleetIssuesId = item.FleetIssuesId;
                fleetIssueSearchModel.ClientLookupId = item.ClientLookupId;
                fleetIssueSearchModel.ImageUrl = !string.IsNullOrEmpty(item.EquipImage) ? item.EquipImage : commonWrapper.GetNoImageUrl();
                if (ClientOnPremise)
                {
                    fleetIssueSearchModel.ImageUrl = UtilityFunction.PhotoBase64ImgSrc(fleetIssueSearchModel.ImageUrl);
                }

                fleetIssueSearchModel.Name = item.Name;
                fleetIssueSearchModel.VIN = item.VIN;
                fleetIssueSearchModel.Make = item.Make;
                fleetIssueSearchModel.Model = item.Model;
                fleetIssueSearchModel.RecordDate = item.RecordDate;
                fleetIssueSearchModel.Description = item.Description;
                fleetIssueSearchModel.Status = item.Status;
                fleetIssueSearchModel.Defects = item.Defects;
                if (item.CompleteDate != null && item.CompleteDate == default(DateTime))
                {
                    fleetIssueSearchModel.CompleteDate = null;
                }
                else
                {
                    fleetIssueSearchModel.CompleteDate = item.CompleteDate;
                }
                fleetIssueSearchModel.TotalCount = item.TotalCount;
                fleetIssueSearchModel.ServiceOrderClientLookupId = item.ServiceOrderClientLookupId;
                fleetIssueSearchModelList.Add(fleetIssueSearchModel);
            }

            return fleetIssueSearchModelList;
        }
        #endregion
        #region Add Or Edit 
        public FleetIssues AddOrEditFleetIssue(string FI_ClientLookupId, FleetIssueVM objFIVM)
        {
            Equipment equipment = new Equipment { ClientId = _dbKey.Client.ClientId, SiteId = _dbKey.User.DefaultSiteId, ClientLookupId = FI_ClientLookupId };
            equipment.RetrieveByClientLookupId(_dbKey);
            var date = objFIVM.FleetIssueModel.RecordDate;
            string emptyValue = string.Empty;
            DateTime RecordDateTime = DateTime.ParseExact(date + " " + objFIVM.FleetIssueModel.RecordTime, "MM/dd/yyyy h:mm tt", CultureInfo.InvariantCulture);
            List<string> errList = new List<string>();
            FleetIssues fleetIssues = new FleetIssues();
            string DefectsList = String.Empty;
            if (objFIVM.FleetIssueModel.FleetIssuesId == 0)
            {
                fleetIssues.ClientId = this.userData.DatabaseKey.User.ClientId;
                fleetIssues.SiteId = this.userData.DatabaseKey.User.DefaultSiteId;
                fleetIssues.AreaId = 0;
                fleetIssues.DepartmentId = 0;
                fleetIssues.StoreroomId = 0;
                fleetIssues.CompleteDate = null;
                fleetIssues.ServiceOrderId = 0;
                fleetIssues.Status = IssueStatusConstants.Open;
                if (objFIVM.FleetIssueModel.DefectsIds != null && objFIVM.FleetIssueModel.DefectsIds.Count > 0)
                {
                    foreach (var item in objFIVM.FleetIssueModel.DefectsIds)
                    {
                        DefectsList += item + ",";
                    }
                }
                fleetIssues.Defects = DefectsList.TrimEnd(',');
                fleetIssues.Description = !string.IsNullOrEmpty(objFIVM.FleetIssueModel.Description) ? objFIVM.FleetIssueModel.Description.Trim() : emptyValue;
                fleetIssues.DriverName = !string.IsNullOrEmpty(objFIVM.FleetIssueModel.DriverName) ? objFIVM.FleetIssueModel.DriverName.Trim() : emptyValue;
                fleetIssues.EquipmentId = equipment.EquipmentId;
                fleetIssues.RecordDate = RecordDateTime;
                fleetIssues.Create(this.userData.DatabaseKey);
            }
            else
            {
                fleetIssues.FleetIssuesId = objFIVM.FleetIssueModel.FleetIssuesId;
                fleetIssues.Retrieve(this.userData.DatabaseKey);
                if (objFIVM.FleetIssueModel.DefectsIds != null && objFIVM.FleetIssueModel.DefectsIds.Count > 0)
                {
                    foreach (var item in objFIVM.FleetIssueModel.DefectsIds)
                    {
                        DefectsList += item + ",";
                    }
                }
                fleetIssues.Defects = DefectsList.TrimEnd(',');
                fleetIssues.Description = !string.IsNullOrEmpty(objFIVM.FleetIssueModel.Description) ? objFIVM.FleetIssueModel.Description.Trim() : emptyValue;
                fleetIssues.DriverName = !string.IsNullOrEmpty(objFIVM.FleetIssueModel.DriverName) ? objFIVM.FleetIssueModel.DriverName.Trim() : emptyValue;
                fleetIssues.EquipmentId = equipment.EquipmentId;
                fleetIssues.RecordDate = RecordDateTime;
                fleetIssues.Update(this.userData.DatabaseKey);

            }
            return fleetIssues;
        }
        public FleetIssueModel GetEditFleetIssueDetailsById(long FleetIssuesId)
        {
            FleetIssueModel objFleetIssueModel = new FleetIssueModel();
            DataContracts.FleetIssues fleetIssues = new DataContracts.FleetIssues()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                FleetIssuesId = FleetIssuesId
            };
            fleetIssues.RetrieveByFleetIssuesId(_dbKey);
            objFleetIssueModel = initializeDetailsControls(fleetIssues);
            return objFleetIssueModel;
        }
        public FleetIssueModel initializeDetailsControls(FleetIssues obj)
        {
            FleetIssueModel objFleetIssueFuel = new FleetIssueModel();
            objFleetIssueFuel.FleetIssuesId = obj.FleetIssuesId;
            objFleetIssueFuel.EquipmentID = Convert.ToString(obj.EquipmentId);
            objFleetIssueFuel.ClientLookupId = obj.EquipmentClientLookupId;
            objFleetIssueFuel.DriverName = obj.DriverName;
            objFleetIssueFuel.DefectsIds = new List<string>(obj.Defects.Split(','));
            objFleetIssueFuel.Description = obj.Description;
            DateTime dateAndTime = Convert.ToDateTime(obj.RecordDate);
            string onlyDate = dateAndTime.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            string onlyTime = dateAndTime.ToString("HH:mm:ss");
            var time = DateTime.ParseExact(onlyTime, "HH:mm:ss", null).ToString("hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
            objFleetIssueFuel.RecordDate = onlyDate;
            objFleetIssueFuel.RecordTime = time;
            return objFleetIssueFuel;
        }
        #endregion

        #region Delete FuelTracking
        public FleetIssues DeleteFleetIssue(long fleetIssuesId)
        { 
            
            FleetIssues FleetIssues = new FleetIssues()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId= this.userData.DatabaseKey.User.DefaultSiteId,
                FleetIssuesId = fleetIssuesId
            };
            FleetIssues.CheckIfServiceOrderExist(this.userData.DatabaseKey);
            if(FleetIssues.ErrorMessages == null || FleetIssues.ErrorMessages.Count == 0)
            {
                FleetIssues.Delete(this.userData.DatabaseKey);
            }
            
            return FleetIssues;
        }
        #endregion

        #region Fleet Only
        public int GetCount()
        {
            int count = 0;
            FleetIssues fIssues = new FleetIssues()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.Personnel.SiteId
            };
            var fleetIssues = fIssues.RetrieveDashboardChart(userData.DatabaseKey, fIssues);
            if (fleetIssues != null && fleetIssues.Count > 0)
            {
                count = fleetIssues[0].FleetIssuesCount;
            }
            return count;
        }
        #endregion
    }
}