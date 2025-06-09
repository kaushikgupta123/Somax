using Common.Constants;
using DataContracts;
using System;
using System.Linq;
using System.Collections.Generic;
using Client.Models.EventInfo;
using Client.Common;
using Client.Models.Dashboard;
using Client.Models;

namespace Client.BusinessWrapper.EventInfo
{
    public class EventInfoWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        List<string> errorMessage = new List<string>();
        string BodyHeader = string.Empty;
        string BodyContent = string.Empty;
        string FooterSignature = string.Empty;
        public EventInfoWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        internal List<KeyValuePair<string, string>> populateListDetails()
        {
            List<KeyValuePair<string, string>> customList = new List<KeyValuePair<string, string>>();
            customList = CustomQueryDisplay.RetrieveQueryItemsByTableAndLanguage(userData.DatabaseKey, "EventInfo", userData.Site.LocalizationLanguage, userData.Site.LocalizationCulture);
            if (customList.Count > 0)
            {
                customList.Insert(0, new KeyValuePair<string, string>("0", "-- Select All --"));
            }
            return customList;
        }
        public List<EventInfoModel> EventInfoList(int SearchTextDropID)
        {

            DataContracts.EventInfo eventinfo = new DataContracts.EventInfo()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                CustomQueryDisplayId = SearchTextDropID,
                SiteId=userData.DatabaseKey.User.DefaultSiteId
            };
            List<DataContracts.EventInfo> eventinfoList = eventinfo.RetrieveAllForSearchNew(this.userData.DatabaseKey, userData.Site.TimeZone);
            List<EventInfoModel> eventList = InitializeModel(eventinfoList);
            return eventList;
        }
        List<EventInfoModel> InitializeModel(List<DataContracts.EventInfo> eventinfoList)
        {
            List<EventInfoModel> eventList = new List<EventInfoModel>();
            EventInfoModel model;

            foreach (var item in eventinfoList)
            {
                model = new EventInfoModel();
                model.ClientId = item.ClientId;
                model.EventInfoId = item.EventInfoId;
                model.EquipmentId = item.EquipmentId;
                model.SourceType = item.SourceType;
                model.EventType = item.EventType;
                model.Description = item.Description;
                model.Status = item.Status;
                model.ProcessBy_PersonnelId = item.ProcessBy_PersonnelId;
                model.ProcessDate = item.ProcessDate;
                model.Disposition = item.Disposition;
                model.DismissReason = item.DismissReason;
                model.WorkOrderId = item.WorkOrderId;
                model.WOClientLookupId = item.WOClientLookupId;
                model.FaultCode = item.FaultCode;
                model.Comments = item.Comments;
                model.SensorId = item.SensorId;
                model.CreatedBy = item.CreatedBy;
                model.CreateDate = item.CreateDate;
                model.ProcessBy_Personnel = item.ProcessBy_Personnel;
                eventList.Add(model);
            }
            return eventList;
        }
        public EventInfoModel EventRetriveById(long EventInfoId)
        {
            EventInfoModel model = new EventInfoModel();
            DataContracts.EventInfo ei = new DataContracts.EventInfo()
            {
                EventInfoId = EventInfoId,
                ClientId = userData.DatabaseKey.Client.ClientId
            };
            ei.RetrieveByPKForeignkey(userData.DatabaseKey, userData.DatabaseKey.User.TimeZone); // change to retrieve by PK
            model.ClientId = ei.ClientId;
            model.EventInfoId = ei.EventInfoId;
            model.EquipmentId = ei.EquipmentId;
            model.EquipClientLookupId = ei.EquipClientLookupId;
            model.SourceType = ei.SourceType;
            model.EventType = ei.EventType;
            model.Description = ei.Description;
            model.Status = ei.Status;
            model.ProcessBy_PersonnelId = ei.ProcessBy_PersonnelId;
            model.ProcessBy_Personnel = ei.ProcessBy_Personnel;
            model.Disposition = ei.Disposition;
            model.DismissReason = ei.DismissReason;
            model.WorkOrderId = ei.WorkOrderId;
            model.WOClientLookupId = ei.WOClientLookupId;
            var LanguageList = UtilityFunction.LocalizationTypes();
            model.FaultCode = ei.FaultCode;
            model.Comments = ei.Comments;
            model.SensorId = ei.SensorId;
            model.CreatedBy = ei.CreatedBy;

            if (ei.CreateDate != null && ei.CreateDate == default(DateTime))
            {
                model.CreateDate = null;
            }
            else
            {
                model.CreateDate = ei.CreateDate;
            }
            if (ei.ProcessDate != null && ei.ProcessDate == default(DateTime))
            {
                model.ProcessDate = null;
            }
            else
            {
                model.ProcessDate = ei.ProcessDate;
            }
            return model;
        }
        public List<string> DismissEvent(DismissModel dismissModel, ref string Type)
        {
            DataContracts.EventInfo ei = new DataContracts.EventInfo();
            ei.ClientId = userData.DatabaseKey.Client.ClientId;
            ei.EventInfoId = dismissModel.EventInfoId;
            ei.Retrieve(userData.DatabaseKey);
            ei.Disposition = "Dismiss";
            ei.DismissReason = dismissModel.DismissReason;
            ei.Comments = dismissModel.Comments;
            ei.ProcessBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            ei.ProcessDate = DateTime.UtcNow;
            ei.Update(userData.DatabaseKey);
            Type = ei.Disposition;
            return ei.ErrorMessages;
        }
        public List<string> AcknowledgeEvent(AcknowledgeModel acknowledgeModel, ref string Type)
        {
            DataContracts.EventInfo ei = new DataContracts.EventInfo();
            ei.ClientId = userData.DatabaseKey.Client.ClientId;
            ei.EventInfoId = acknowledgeModel.EventInfoId;
            ei.Retrieve(userData.DatabaseKey);
            ei.Disposition = "Acknowledge";
            string[] values = acknowledgeModel.FaultCode.Split('|').Select(sValue => sValue.Trim()).ToArray();
            ei.FaultCode = values[0] != string.Empty ? values[0].Trim() : "";
            ei.Comments = acknowledgeModel.Comments;
            ei.ProcessBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            ei.ProcessDate = DateTime.UtcNow;
            ei.Update(userData.DatabaseKey);
            Type = ei.Disposition;
            return ei.ErrorMessages;
        }

        #region Event Describe

        public WorkOrder Event_Describe(EventDescribeModel EventDescribeModel)
        {
            DataContracts.EventInfo ei = new DataContracts.EventInfo();
            DataContracts.WorkOrderSchedule workorderSchedule = new DataContracts.WorkOrderSchedule();

            DataContracts.WorkOrder workOrder = new DataContracts.WorkOrder
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId
            };

            string newClientlookupId = "";
            if (AutoGenerateKey.WorkOrder_FromWorkOrder_AutoGenerateEnabled)
            {
                newClientlookupId = CustomSequentialId.GetNextId(userData.DatabaseKey, AutoGenerateKey.WorkOrder_FromWorkOrder_AutoGenerateKey, userData.DatabaseKey.User.DefaultSiteId, "");
            }

            workOrder.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            workOrder.ClientLookupId = newClientlookupId;
            workOrder.ApproveDate = DateTime.UtcNow;
            workOrder.Status = WorkOrderStatusConstants.Approved;
            workOrder.SourceType = WorkOrderSourceTypes.Emergency;
            // Type
            workOrder.Type = EventDescribeModel.Type;
            workOrder.ChargeType = "Equipment";
            workOrder.ChargeToClientLookupId = EventDescribeModel.ChargeToClientLookupId;
            workOrder.Description = EventDescribeModel.Description;
            workOrder.PersonnelId = EventDescribeModel.PersonnelId ?? 0;
            //--WorkOrderSchedule--//
            workorderSchedule.ClientId = userData.DatabaseKey.Client.ClientId;
            workorderSchedule.PersonnelId = EventDescribeModel.PersonnelId ?? 0;
            workorderSchedule.ScheduledStartDate = DateTime.Parse(EventDescribeModel.Date.ToString());
            workorderSchedule.ScheduledHours = EventDescribeModel.Hours ?? 0;
            ei.ClientId = userData.DatabaseKey.Client.ClientId;
            ei.EventInfoId = EventDescribeModel.EventInfoId;
            ei.Retrieve(userData.DatabaseKey);
            ei.ProcessBy_PersonnelId = EventDescribeModel.PersonnelId ?? 0;
            ei.ProcessDate = DateTime.UtcNow;
            ei.Disposition = EventDispositionConstants.WorkOrder;

            //   RetrieveEventInfoWorkOrder(workOrder, workorderSchedule, ei);
            //--Creating WorkOrder Record--//           
            workOrder.CreateEmergencyWOByPKForeignKeys(this.userData.DatabaseKey, userData.Site.TimeZone);
            //--Creating Schedule Record--//
            workorderSchedule.WorkOrderId = workOrder.WorkOrderId;
            //workorderSchedule.Create(UserData.DatabaseKey);
            if (EventDescribeModel.PersonnelId > 0)
            {
                workorderSchedule.CreateForWorkOrder(this.userData.DatabaseKey);
                CreateEventLog(workOrder.WorkOrderId, WorkOrderEvents.Scheduled);
            }
            //--Updating EventInfo record--//
            ei.WorkOrderId = workOrder.WorkOrderId;
            ei.Update(userData.DatabaseKey);
            //--Creating EventLog--//
            CreateEventLog(workOrder.WorkOrderId, WorkOrderEvents.Create);

            return workOrder;
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
        private void CreateEventLog(Int64 WOId, string Status, string comment)
        {
            WorkOrderEventLog log = new WorkOrderEventLog();
            log.ClientId = userData.DatabaseKey.Client.ClientId;
            log.SiteId = userData.DatabaseKey.Personnel.SiteId;
            log.WorkOrderId = WOId;
            log.Event = Status;
            log.TransactionDate = DateTime.UtcNow;
            log.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            log.Comments = comment;
            log.SourceId = 0;
            log.Create(userData.DatabaseKey);
        }
        #endregion

        #region Event OnDemand Work Order
        public List<DataContracts.MaintOnDemandMaster> GetOndemandList()
        {
            DataContracts.MaintOnDemandMaster maintOnDemandMaster = new DataContracts.MaintOnDemandMaster();
            maintOnDemandMaster.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            List<DataContracts.MaintOnDemandMaster> maintOnDemandMasterList = maintOnDemandMaster.RetrieveAllBySiteId(this.userData.DatabaseKey, this.userData.Site.TimeZone).Where(a => a.InactiveFlag == false).ToList();
            return maintOnDemandMasterList;
        }
        public List<string> CreateforEventInfo(EventOnDemandModel eventOnDemandModel)
        {
            DataContracts.EventInfo ei = new DataContracts.EventInfo();
            DataContracts.WorkOrderSchedule workorderSchedule = new DataContracts.WorkOrderSchedule();

            DataContracts.WorkOrder workOrder = new DataContracts.WorkOrder
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId
            };

            RetrieveEventInfoWorkOrder_OD(workOrder, workorderSchedule, ei, eventOnDemandModel);
            //--Creating WorkOrder Record--//           
            workOrder.CreateFromOnDemandMaster(this.userData.DatabaseKey, userData.Site.TimeZone);//---WorkOrder Created--//

            //--Creating Schedule Record--//
            workorderSchedule.WorkOrderId = workOrder.WorkOrderId;
            //workorderSchedule.Create(UserData.DatabaseKey);
            workorderSchedule.CreateForWorkOrder(this.userData.DatabaseKey);
            //--Updating EventInfo record--//          
            ei.WorkOrderId = workOrder.WorkOrderId;
            ei.Update(userData.DatabaseKey);
            //--Creating EventLog--//
            CreateEventLog(workOrder.WorkOrderId, WorkOrderEvents.Create);
            CreateEventLog(workOrder.WorkOrderId, WorkOrderEvents.Scheduled);
            List<Int64> createdWorkOrderList = new List<Int64>();
            createdWorkOrderList.Add(workOrder.WorkOrderId);
            // AlertCreate(workOrder, AlertTypeEnum.WorkOrderScheduleAssignedUser, createdWorkOrderList);
            return workOrder.ErrorMessages;
        }
        private void RetrieveEventInfoWorkOrder_OD(DataContracts.WorkOrder workOrder, DataContracts.WorkOrderSchedule workorderSchedule, DataContracts.EventInfo ei, EventOnDemandModel eventOnDemandModel)
        {
            // Generate the new number  WorkOrder_FromWorkOrder_AutoGenerateEnabled
            string newClientlookupId = "";
            if (AutoGenerateKey.WorkOrder_FromWorkOrder_AutoGenerateEnabled)
            {
                newClientlookupId = CustomSequentialId.GetNextId(userData.DatabaseKey, AutoGenerateKey.WorkOrder_FromWorkOrder_AutoGenerateKey, userData.DatabaseKey.User.DefaultSiteId, "");
            }

            // MaintDemandMaster

            workOrder.MaintOnDemandClientLookUpId = eventOnDemandModel.OnDemandID;
            workOrder.Type = eventOnDemandModel.Type;

            workOrder.ClientLookupId = newClientlookupId;
            workOrder.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            workOrder.Status = WorkOrderStatusConstants.Approved;
            workOrder.SourceType = WorkOrderSourceTypes.OnDemand;
            workOrder.ApproveDate = DateTime.UtcNow;
            workOrder.ChargeType = "Equipment";
            workOrder.ChargeToClientLookupId = eventOnDemandModel.ChargeToClientLookupId;
            workOrder.PersonnelId = eventOnDemandModel.PersonnelId ?? 0;
            //--WorkOrderSchedule--//
            workorderSchedule.ClientId = userData.DatabaseKey.Client.ClientId;
            workorderSchedule.PersonnelId = eventOnDemandModel.PersonnelId ?? 0;
            workorderSchedule.ScheduledStartDate = eventOnDemandModel.Date ?? default(DateTime);
            workorderSchedule.ScheduledHours = eventOnDemandModel.Hours ?? 0;
            //--Event Info--//
            ei.ClientId = userData.DatabaseKey.Client.ClientId;
            ei.EventInfoId = eventOnDemandModel.EventInfoId;
            ei.Retrieve(userData.DatabaseKey);
            ei.ProcessBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            ei.ProcessDate = DateTime.UtcNow;
            ei.Disposition = EventDispositionConstants.WorkOrder;
        }
        #endregion Event Ondemand work Order

        public int GetEventInfoCountByStatus(string status)
        {
            DataContracts.EventInfo eventinfo = new DataContracts.EventInfo()
            {
                Status = status
            };

            int count = eventinfo.RetrieveEventInfoCountByStatus(userData.DatabaseKey);
            return count;
        }
        public APMCountHozBarModel GetAPMCountHozBar()
        {
            APMCountHozBarModel aPMCountHozBarModel = new APMCountHozBarModel();
            DataContracts.EventInfo eventinfo = new DataContracts.EventInfo();
            var count = eventinfo.RetrieveAPMCountHozBarV2(userData.DatabaseKey);
            aPMCountHozBarModel.TotalOpenCount = count.TotalOpenCount;
            aPMCountHozBarModel.OpenAssetCount = count.OpenAssetCount;
            aPMCountHozBarModel.MonitoredAssetCount = count.MonitoredAssetCount;
            return aPMCountHozBarModel;
        }
        public Chart GetAPMBarChartData(int QueryId)
        {
            DataContracts.EventInfo eventinfo = new DataContracts.EventInfo();
            List<string> ColorList = new List<string>();
            Chart _chart = new Chart();
            eventinfo.QueryId = QueryId;
            var chartData = eventinfo.RetrieveAPMBarChart(userData.DatabaseKey);
            if (chartData != null && chartData.Count > 0)
            {
                _chart.labels = chartData.Select(x => x.FaultCode).ToArray();
                _chart.datasets = new List<Datasets>();
                List<Datasets> _dataSet = new List<Datasets>();
                _dataSet.Add(new Datasets()
                {
                    data = chartData.Select(x => Convert.ToInt64(x.EventCount)).ToArray(),
                });
                if (_dataSet != null)
                {
                    _dataSet[0].backgroundColor = ColorList.ToArray();
                    _dataSet[0].borderColor = ColorList.ToArray();
                }

                _chart.datasets = _dataSet;
            }
            return _chart;
        }
        public List<DataContracts.EventInfo> GetAPMDoughChart(int QueryId)
        {
            DataContracts.EventInfo eventinfo = new DataContracts.EventInfo();
            eventinfo.QueryId = QueryId;
            return eventinfo.RetrieveAPMDoughChart(userData.DatabaseKey);
        }


       
    }
}