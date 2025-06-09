using Client.BusinessWrapper.Common;
using Client.Common.Constants;
using Client.Models;
using Client.Models.Common;
using Client.Models.FleetScheduledService;
using Client.Models.FleetService;
using Common.Constants;
using Common.Enumerations;
using Common.Extensions;
using DataContracts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Client.BusinessWrapper.FleetService
{
    public class FleetServiceWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        List<string> errorMessage = new List<string>();
        public FleetServiceWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }
        #region Search Master
        public List<FleetServiceModel> GetFleetServiceGridData(int CustomQueryDisplayId,string orderbycol = "", string orderDir = "", int skip = 0, int length = 0,
             DateTime? CreateStartDateVw = null, DateTime? CreateEndDateVw = null, DateTime? CompleteStartDateVw = null, DateTime? CompleteEndDateVw = null, string personnelList = "", string AssetID = "", string Name = "", string Description = "", string Shift = "", string Type = "", string VIN = "", string searchText = "")
        {
            ServiceOrder ServiceOrder = new ServiceOrder();
            FleetServiceModel SModel;
            List<FleetServiceModel> fleetServiceModelList = new List<FleetServiceModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<string> StatusList = new List<string>();
            List<string> AssetIdList = new List<string>();
            ServiceOrder.ClientId = userData.DatabaseKey.Client.ClientId;
            ServiceOrder.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            ServiceOrder.CustomQueryDisplayId = CustomQueryDisplayId;
            ServiceOrder.orderbyColumn = orderbycol;
            ServiceOrder.orderBy = orderDir;
            ServiceOrder.offset1 = Convert.ToString(skip);
            ServiceOrder.nextrow = Convert.ToString(length);

            ServiceOrder.EquipmentClientLookupId = AssetID;
            ServiceOrder.AssetName = Name;
            ServiceOrder.Description = Description;
            ServiceOrder.Shift = Shift;
            ServiceOrder.Type = Type;
            ServiceOrder.VIN = VIN;
            ServiceOrder.CreateStartDateVw = CreateStartDateVw.HasValue ? CreateStartDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            ServiceOrder.CreateEndDateVw = CreateEndDateVw.HasValue ? CreateEndDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            ServiceOrder.CompleteStartDateVw = CompleteStartDateVw.HasValue ? CompleteStartDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            ServiceOrder.CompleteEndDateVw = CompleteEndDateVw.HasValue ? CompleteEndDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            ServiceOrder.PersonnelList = personnelList;
            ServiceOrder.SearchText = searchText;

            List<ServiceOrder> ServiceOrderList = ServiceOrder.RetrieveChunkSearch(this.userData.DatabaseKey);            

            if (ServiceOrderList != null)
            {
                foreach (var item in ServiceOrderList)
                {
                    SModel = new FleetServiceModel();
                    SModel.ServiceOrderId = item.ServiceOrderId;
                    SModel.ClientLookupId = item.ClientLookupId;
                    SModel.EquipmentClientLookupId = item.EquipmentClientLookupId;
                    SModel.AssetName = item.AssetName;
                    SModel.Status = item.Status;
                    SModel.Type = item.Type;
                    if (item.CreateDate != null && item.CreateDate == default(DateTime))
                    {
                        SModel.CreateDate = null;
                    }
                    else
                    {
                        SModel.CreateDate = item.CreateDate;
                    }

                    SModel.Assigned = item.Assigned;

                    if (item.ScheduleDate != null && item.ScheduleDate == default(DateTime))
                    {
                        SModel.ScheduleDate = null;
                    }
                    else
                    {
                        SModel.ScheduleDate = item.ScheduleDate;
                    }

                    if (item.CompleteDate != null && item.CompleteDate == default(DateTime))
                    {
                        SModel.CompleteDate = null;
                    }
                    else
                    {
                        SModel.CompleteDate = item.CompleteDate;
                    }

                    SModel.Assign_PersonnelId = item.Assign_PersonnelId;
                    SModel.ChildCount = item.ChildCount;
                    SModel.TotalCount = item.TotalCount;
                    fleetServiceModelList.Add(SModel);
                }
            }



            return fleetServiceModelList;
        }
        #endregion

        #region Search Line Item
        internal List<FleetServiceLineItemModel> PopulateLineitems(long ServiceOrderId)
        {
            FleetServiceLineItemModel objLineItem;
            List<FleetServiceLineItemModel> LineItemList = new List<FleetServiceLineItemModel>();

            ServiceOrderLineItem serviceOrderLineItem = new ServiceOrderLineItem()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ServiceOrderId = ServiceOrderId
            };
            List<ServiceOrderLineItem> ServiceOrderLineItemList = ServiceOrderLineItem.ServiceOrderLineItemRetrieveByServiceOrderId(this.userData.DatabaseKey, serviceOrderLineItem);

            if (ServiceOrderLineItemList != null)
            {
                foreach (var item in ServiceOrderLineItemList)
                {
                    objLineItem = new FleetServiceLineItemModel();
                    objLineItem.ServiceOrderLineItemId = item.ServiceOrderLineItemId;
                    objLineItem.ServiceOrderId = item.ServiceOrderId;
                    objLineItem.Description = item.Description;
                    objLineItem.Labor = Math.Round(decimal.Parse(item.Labor.ToString()), 2).ToString();
                    objLineItem.Materials = Math.Round(decimal.Parse(item.Materials.ToString()), 2).ToString();
                    objLineItem.Others = Math.Round(decimal.Parse(item.Other.ToString()), 2);
                    objLineItem.Total = Math.Round(item.Total, 2);
                    objLineItem.DetailsTotal = objLineItem.Others + objLineItem.Total;
                    objLineItem.RepairReason = item.RepairReason;
                    objLineItem.Comment = item.Comment;
                    objLineItem.ServiceTaskId = item.ServiceTaskId;
                    objLineItem.EquipmentId = item.EquipmentId;
                    objLineItem.FleetIssuesId = item.FleetIssueId;
                    objLineItem.FIDescription = item.FIDescription;
                    objLineItem.SchedServiceId = item.SchedServiceId;
                    objLineItem.System = item.VMRSSystem;
                    objLineItem.Assembly = item.VMRSAssembly;
                    objLineItem.Status = item.Status;
                    LineItemList.Add(objLineItem);
                }
            }

            return LineItemList;
        }
        #endregion

        #region Details
        public FleetServiceModel RetrieveByServiceOrderId(long ServiceOrderId)
        {
            ServiceOrder objServiceOrder = new ServiceOrder();
            FleetServiceModel objFleetServiceModel = new FleetServiceModel();
            CompleteServiceOrderModel objCompleteServiceOrderModel = new CompleteServiceOrderModel();

            objServiceOrder.ClientId = userData.DatabaseKey.Client.ClientId;
            objServiceOrder.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            objServiceOrder.ServiceOrderId = ServiceOrderId;

            objServiceOrder = objServiceOrder.RetrieveByServiceOrderId(this.userData.DatabaseKey);

            if (objServiceOrder != null)
            {
                objFleetServiceModel.ServiceOrderId = objServiceOrder.ServiceOrderId;
                objFleetServiceModel.ClientLookupId = objServiceOrder.ClientLookupId;
                objFleetServiceModel.Status = objServiceOrder.Status;
                objFleetServiceModel.EquipmentClientLookupId = objServiceOrder.EquipmentClientLookupId;
                objFleetServiceModel.AssetName = objServiceOrder.AssetName;
                objFleetServiceModel.EquipmentId = objServiceOrder.EquipmentId;
                objFleetServiceModel.Meter1Type = objServiceOrder.Meter1Type;
                objFleetServiceModel.Meter1CurrentReading = objServiceOrder.Meter1CurrentReading;
                objFleetServiceModel.Meter2Type = objServiceOrder.Meter2Type;
                objFleetServiceModel.Meter2CurrentReading = objServiceOrder.Meter2CurrentReading;
                objFleetServiceModel.LaborTotal = Math.Round(objServiceOrder.LaborTotal, 2);
                objFleetServiceModel.PartTotal = Math.Round(objServiceOrder.PartTotal, 2);
                objFleetServiceModel.OtherTotal = Math.Round(objServiceOrder.OtherTotal, 2);
                objFleetServiceModel.GrandTotal = Math.Round(objServiceOrder.GrandTotal, 2);
                if (objServiceOrder.ScheduleDate != null && objServiceOrder.ScheduleDate == DateTime.MinValue)
                {
                    objFleetServiceModel.ScheduleDate = null;
                }
                else
                {
                    objFleetServiceModel.ScheduleDate = objServiceOrder.ScheduleDate;
                }
                objFleetServiceModel.Assigned = objServiceOrder.Assigned;
                objFleetServiceModel.Assign_PersonnelId = objServiceOrder.Assign_PersonnelId;
                if (objServiceOrder.CompleteDate != null && objServiceOrder.CompleteDate == DateTime.MinValue)
                {
                    objFleetServiceModel.CompleteDate = null;
                }
                else
                {
                    objFleetServiceModel.CompleteDate = objServiceOrder.CompleteDate;
                }
                objFleetServiceModel.Description = objServiceOrder.Description;
                objFleetServiceModel.Shift = objServiceOrder.Shift;
                objFleetServiceModel.Type = objServiceOrder.Type;
                objFleetServiceModel.CompleteBy_PersonnelId = objServiceOrder.CompleteBy_PersonnelId;
                objFleetServiceModel.CompletedByPersonnels = objServiceOrder.CompletedByPersonnels;
                if (objServiceOrder.CancelDate != null && objServiceOrder.CancelDate == default(DateTime))
                {
                    objFleetServiceModel.CancelDate = null;
                }
                else
                {
                    objFleetServiceModel.CancelDate = objServiceOrder.CancelDate;
                }
                objFleetServiceModel.CancelBy_PersonnelId = objServiceOrder.CancelBy_PersonnelId;
                objFleetServiceModel.CancelledByPersonnels = objServiceOrder.CancelledByPersonnels;
                objFleetServiceModel.CancelReason = objServiceOrder.CancelReason;
                objFleetServiceModel.Meter1CurrentReadingDate = objServiceOrder.Meter1CurrentReadingDate;
                objFleetServiceModel.Meter2CurrentReadingDate = objServiceOrder.Meter2CurrentReadingDate;
                objFleetServiceModel.Meter1Units = objServiceOrder.Meter1Units;
                objFleetServiceModel.Meter2Units = objServiceOrder.Meter2Units;
                objFleetServiceModel.ShiftDesc = objServiceOrder.ShiftDesc;
                objFleetServiceModel.TypeDesc = objServiceOrder.TypeDesc;
                objFleetServiceModel.ClientOnPremise = userData.DatabaseKey.Client.OnPremise;
            }
            return objFleetServiceModel;
        }
        public string RetrievePersonnelInitial(long ServiceOrderId)
        {
            ServiceOrder objServiceOrder = new ServiceOrder();

            objServiceOrder.ClientId = userData.DatabaseKey.Client.ClientId;
            objServiceOrder.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            objServiceOrder.ServiceOrderId = ServiceOrderId;

            objServiceOrder = objServiceOrder.RetrievePersonnelInitial(this.userData.DatabaseKey);
            string Assigned = string.Empty;
            if (!string.IsNullOrEmpty(objServiceOrder.Assigned))
            {
                Assigned = objServiceOrder.Assigned;
            }

            return Assigned;
        }
        public ServiceOrder AddRemoveScheduleRecord(ServiceOrderScheduleModel objServiceOrderSchedule, ref string Statusmsg, string ScheduleType = "Add")
        {
            string PersonnelList = String.Empty;
            ProcessAlert objAlert = new ProcessAlert(this.userData);
            ServiceOrder objServiceOrder = new ServiceOrder();
            objServiceOrder.ClientId = userData.DatabaseKey.Client.ClientId;
            objServiceOrder.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            objServiceOrder.ServiceOrderId = objServiceOrderSchedule.ServiceOrderId;
            objServiceOrder.ScheduledHours = objServiceOrderSchedule.ScheduledDuration;
            objServiceOrder.ScheduleDate = objServiceOrderSchedule.Schedulestartdate;

            CommonWrapper coWrapper = new CommonWrapper(userData);
            var namelist = coWrapper.MentionList("");
            List<UserMentionDataModel.UserMentionData> userMentionDataList = new List<UserMentionDataModel.UserMentionData>();
            UserMentionDataModel.UserMentionData objUserMentionData;
            List<long> nos = new List<long>() { objServiceOrderSchedule.ServiceOrderId };
            if (objServiceOrderSchedule.PersonnelIds != null && objServiceOrderSchedule.PersonnelIds.Count > 0)
            {
                foreach (var item in objServiceOrderSchedule.PersonnelIds)
                {
                    PersonnelList += item + ",";
                    objUserMentionData = new UserMentionDataModel.UserMentionData();
                    objUserMentionData.userId = Convert.ToInt64(item);
                    objUserMentionData.userName = namelist.Where(x => x.PersonnelId == Convert.ToInt64(item)).Select(y => y.UserName).FirstOrDefault();
                    objUserMentionData.emailId = namelist.Where(x => x.PersonnelId == Convert.ToInt64(item)).Select(y => y.Email).FirstOrDefault();
                    userMentionDataList.Add(objUserMentionData);
                }
            }
            List<long> userIds = new List<long>();
            var UserList = new List<Tuple<long, string, string>>();
            if (userMentionDataList != null && userMentionDataList.Count > 0)
            {
                foreach (var item in userMentionDataList)
                {
                    UserList.Add
                   (
                        Tuple.Create(Convert.ToInt64(item.userId), item.userName, item.emailId)
                  );
                }

            }

            objServiceOrder.PersonnelList = (!String.IsNullOrEmpty(PersonnelList)) ? PersonnelList.TrimEnd(',') : string.Empty;
            if ((objServiceOrderSchedule.PersonnelIds != null && objServiceOrderSchedule.PersonnelIds.Count > 0) && (objServiceOrderSchedule.Schedulestartdate != null && objServiceOrderSchedule.Schedulestartdate != default(DateTime)))
            {
                objServiceOrder.IsDeleteFlag = false;
            }
            else
            {
                objServiceOrder.IsDeleteFlag = true;
            }

            objServiceOrder.AddRemoveScheduleRecord(userData.DatabaseKey);
            if (objServiceOrder.ErrorMessages == null)
            {
                //Add in ServiceOrderEventLog table
                if (ScheduleType == "Add")
                {
                    CreateEventLog(objServiceOrderSchedule.ServiceOrderId, ServiceOrderStatusConstant.Scheduled);
                    objAlert.CreateAlert<DataContracts.ServiceOrder>(AlertTypeEnum.ServiceOrderAssign, nos, UserList);
                }
                Statusmsg = "success";
            }
            else
            {
                Statusmsg = "error";
            }
            return objServiceOrder;
        }
        public void RemoveScheduleRecord(long ServiceOrderId, ref string Statusmsg)
        {
            ServiceOrder objServiceOrder = new ServiceOrder();
            objServiceOrder.ClientId = userData.DatabaseKey.Client.ClientId;
            objServiceOrder.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            objServiceOrder.ServiceOrderId = ServiceOrderId;
            objServiceOrder.IsDeleteFlag = true;
            objServiceOrder.AddRemoveScheduleRecord(this.userData.DatabaseKey/*, userData.Site.TimeZone*/);
            if (objServiceOrder.ErrorMessages.Count == 0)
            {
                Statusmsg = "success";
            }
            else
            {
                Statusmsg = "error";
            }
        }
        public List<ServiceOrderIssuePartModel> RetrievePartByServiceOrderId(long ServiceOrderId, long ServiceOrderLineItemId)
        {
            ServiceOrderIssuePartModel partHistoryModel;
            List<ServiceOrderIssuePartModel> plist = new List<ServiceOrderIssuePartModel>();
            PartHistory parthistory = new PartHistory()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ChargeType_Primary = "ServiceOrder",
                ChargeToId_Primary = ServiceOrderId,
                ChargeType_Secondary = "SOLineItem",
                ChargeToId_Secondary = ServiceOrderLineItemId,
            };
            List<PartHistory> phList = parthistory.RetriveByServiceOrderId(this.userData.DatabaseKey);

            foreach (var item in phList)
            {
                partHistoryModel = new ServiceOrderIssuePartModel();
                partHistoryModel.PartClientLookupId = item.PartClientLookupId;
                partHistoryModel.Description = item.Description;
                partHistoryModel.TransactionQuantity = item.TransactionQuantity;
                partHistoryModel.Cost = item.Cost;
                partHistoryModel.TotalCost = item.TotalCost;
                partHistoryModel.UnitofMeasure = item.UnitofMeasure;
                if (item.TransactionDate != null && item.TransactionDate == default(DateTime))
                {
                    partHistoryModel.TransactionDate = null;
                }
                else
                {
                    partHistoryModel.TransactionDate = item.TransactionDate.ToUserTimeZone(this.userData.Site.TimeZone);
                }
                partHistoryModel.VMRSFailure = item.VMRSFailure;
                partHistoryModel.VMRSFailureCode = item.VMRSFailureCode;
                plist.Add(partHistoryModel);
            }
            return plist;
        }
        public List<ServiceOrderLabourModel> RetrieveLabourByServiceOrderId(long ServiceOrderId, long ServiceOrderLineItemId)
        {
            List<ServiceOrderLabourModel> LaborList = new List<ServiceOrderLabourModel>();
            ServiceOrderLabourModel objLabor;

            Timecard timecard = new Timecard()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ChargeType_Primary = "ServiceOrder",
                ChargeToId_Primary = ServiceOrderId,
                ChargeType_Secondary = "SOLineItem",
                ChargeToId_Secondary = ServiceOrderLineItemId
            };

            var ActuallaborData = Timecard.RetriveByServiceOrderId(this.userData.DatabaseKey, timecard);

            if (ActuallaborData != null)
            {
                foreach (var item in ActuallaborData)
                {
                    objLabor = new ServiceOrderLabourModel();
                    objLabor.Name = item.NameLast + " " + item.NameFirst;
                    if (item.StartDate != null && item.StartDate == default(DateTime))
                    {
                        objLabor.StartDate = null;
                    }
                    else
                    {
                        objLabor.StartDate = item.StartDate;
                    }
                    objLabor.Hours = item.Hours;
                    objLabor.Cost = item.TCValue;
                    objLabor.TimecardId = item.TimecardId;
                    objLabor.ServiceOrderLineItemId = item.ChargeToId_Secondary;
                    objLabor.TimecardId = item.TimecardId;
                    objLabor.VMRSWorkAccomplished = item.VMRSWorkAccomplished;
                    objLabor.VMRSWorkAccomplishedCode = item.VMRSWorkAccomplishedCode;
                    LaborList.Add(objLabor);
                }
            }
            return LaborList;
        }
        public List<ServiceOrderOthers> PopulateOthers(long ServiceOrderId, long ServiceOrderLineItemId)
        {
            List<ServiceOrderOthers> OtherList = new List<ServiceOrderOthers>();
            ServiceOrderOthers objOther;
            OtherCosts othercost = new OtherCosts()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ObjectId = ServiceOrderId,
                ObjectType = "ServiceOrder",
                ObjectType_Secondary = "SOLineItem",
                ObjectId_Secondary = ServiceOrderLineItemId
            };

            var ActualOthersData = othercost.RetriveByTypeAndObjectId(this.userData.DatabaseKey);
            if (ActualOthersData != null)
            {
                foreach (var item in ActualOthersData)
                {
                    objOther = new ServiceOrderOthers();
                    objOther.OtherCostsId = item.OtherCostsId;
                    objOther.ServiceOrderLineItemId = ServiceOrderLineItemId;
                    objOther.Source = item.Source;
                    objOther.VendorClientLookupId = item.VendorClientLookupId;
                    objOther.VendorId = item.VendorId;
                    objOther.Description = item.Description;
                    objOther.UnitCost = item.UnitCost;
                    objOther.Quantity = item.Quantity;
                    objOther.TotalCost = item.TotalCost;

                    OtherList.Add(objOther);
                }
            }
            return OtherList;
        }
        public List<EventLogModel> PopulateEventLog(long ServiceOrderId)
        {
            EventLogModel objEventLogModel;
            List<EventLogModel> EventLogModelList = new List<EventLogModel>();
            ServiceOrderEventLog log = new ServiceOrderEventLog();
            List<ServiceOrderEventLog> data = new List<ServiceOrderEventLog>();

            log.ClientId = userData.DatabaseKey.Client.ClientId;
            log.SiteId = userData.DatabaseKey.Personnel.SiteId;
            log.ServiceOrderId = ServiceOrderId;
            data = log.RetrieveByServiceOrderId(userData.DatabaseKey);

            if (data != null)
            {
                foreach (var item in data)
                {
                    objEventLogModel = new EventLogModel();
                    objEventLogModel.ClientId = item.ClientId;
                    objEventLogModel.SiteId = item.SiteId;
                    objEventLogModel.EventLogId = item.ServiceOrderEventLogId;
                    objEventLogModel.ObjectId = item.ServiceOrderId;
                    if (item.TransactionDate != null && item.TransactionDate == default(DateTime))
                    {
                        objEventLogModel.TransactionDate = null;
                    }
                    else
                    {
                        objEventLogModel.TransactionDate = item.TransactionDate.ToUserTimeZone(userData.Site.TimeZone);
                    }
                    objEventLogModel.Event = item.Event;
                    objEventLogModel.Comments = item.Comments;
                    objEventLogModel.SourceId = item.SourceId;
                    objEventLogModel.Personnel = item.Personnel;
                    objEventLogModel.Events = item.Events;
                    objEventLogModel.PersonnelInitial = item.PersonnelInitial;
                    EventLogModelList.Add(objEventLogModel);
                }
            }
            return EventLogModelList;
        }
        public List<SelectListItem> PopulateServiceTask()
        {
            ServiceTasks ServiceTasks = new ServiceTasks();
            ServiceTasks.ClientId = userData.DatabaseKey.Client.ClientId;
            List<ServiceTasks> lstServiceTasks = ServiceTasks.RetrieveAllCustom(userData.DatabaseKey);
            var ServTasks = lstServiceTasks.Select(x => new SelectListItem { Text = x.Description, Value = x.ServiceTasksId.ToString() }).ToList();
            return ServTasks;
        }

        #region Line item
        public ServiceOrderLineItem AddLineItem(FleetServiceLineItemModel objLineItem)
        {
            ServiceOrderLineItem serviceOrderLineItem = new ServiceOrderLineItem()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ServiceOrderId = objLineItem.ServiceOrderId,
                ServiceTaskId = objLineItem.ServiceTaskId,
                RepairReason = objLineItem.RepairReason,
                Comment = objLineItem.Comment,
                VMRSSystem = objLineItem.System,
                VMRSAssembly = objLineItem.Assembly
            };
            serviceOrderLineItem.Create(this.userData.DatabaseKey);
            return serviceOrderLineItem;
        }
        public ServiceOrderLineItem EditLineItem(FleetServiceLineItemModel objLineItem)
        {

            ServiceOrderLineItem serviceOrderLineItem = new ServiceOrderLineItem()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ServiceOrderLineItemId = objLineItem.ServiceOrderLineItemId
            };

            serviceOrderLineItem.Retrieve(this.userData.DatabaseKey);
            if (serviceOrderLineItem.FleetIssueId == 0)
            {
                serviceOrderLineItem.RepairReason = objLineItem.RepairReason ?? "";
                serviceOrderLineItem.Comment = objLineItem.Comment ?? "";
                serviceOrderLineItem.FleetIssueId = objLineItem.FleetIssuesId;
                serviceOrderLineItem.VMRSSystem = objLineItem.System;
                serviceOrderLineItem.VMRSAssembly = objLineItem.Assembly;

                serviceOrderLineItem.Update(this.userData.DatabaseKey);

                if (objLineItem.FleetIssuesId > 0)
                {
                    FleetIssues objFI = new FleetIssues()
                    {
                        ClientId = this.userData.DatabaseKey.Client.ClientId,
                        FleetIssuesId = objLineItem.FleetIssuesId,
                    };
                    objFI.Retrieve(this.userData.DatabaseKey);
                    objFI.ServiceOrderId = objLineItem.ServiceOrderId;
                    objFI.Update(this.userData.DatabaseKey);
                }
            }
            else
            {
                var prevFleetissueid = serviceOrderLineItem.FleetIssueId;
                serviceOrderLineItem.RepairReason = objLineItem.RepairReason ?? "";
                serviceOrderLineItem.Comment = objLineItem.Comment ?? "";
                serviceOrderLineItem.FleetIssueId = objLineItem.FleetIssuesId;
                serviceOrderLineItem.VMRSSystem = objLineItem.System;
                serviceOrderLineItem.VMRSAssembly = objLineItem.Assembly;

                serviceOrderLineItem.Update(this.userData.DatabaseKey);

                FleetIssues objFI = new FleetIssues()
                {
                    ClientId = this.userData.DatabaseKey.Client.ClientId,
                    FleetIssuesId = objLineItem.FleetIssuesId,
                };
                objFI.Retrieve(this.userData.DatabaseKey);
                objFI.ServiceOrderId = objLineItem.ServiceOrderId;
                objFI.PrevFleeissueId = prevFleetissueid;
                objFI.FleetIssuesUpdateforPrevandNewFleetissues(this.userData.DatabaseKey);
            }
            return serviceOrderLineItem;
        }


        public bool DeleteLineItem(long ServiceOrderLineItemId)
        {
            ServiceOrderLineItem serviceOrderLineItem = new ServiceOrderLineItem()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ServiceOrderLineItemId = ServiceOrderLineItemId
            };
            try
            {
                serviceOrderLineItem.ServiceOrderLineItemDeleteCustom(this.userData.DatabaseKey);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public LineItemDeleteParameter ValidateForDeleteLineItem(long ServiceOrderId, long ServiceOrderLineItemId)
        {
            LineItemDeleteParameter objParam = null;
            ServiceOrderLineItem serviceOrderLineItem = new ServiceOrderLineItem()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ServiceOrderLineItemId = ServiceOrderLineItemId,
                ServiceOrderId = ServiceOrderId
            };
            serviceOrderLineItem = serviceOrderLineItem.ValidateForDeleteLineItem(this.userData.DatabaseKey, serviceOrderLineItem);
            if (serviceOrderLineItem != null)
            {
                objParam = new LineItemDeleteParameter();
                objParam.DeleteAllowed = serviceOrderLineItem.DeleteAllowed;
                objParam.LabourExists = serviceOrderLineItem.LabourExists;
                objParam.PartExists = serviceOrderLineItem.PartExists;
                objParam.OthersExists = serviceOrderLineItem.OthersExists;
            }
            return objParam;

        }
        #endregion

        #region Labour
        public Timecard AddLabour(ServiceOrderLabourModel objServiceOrderLabour)
        {
            Timecard timecard = new Timecard()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                ChargeType_Primary = "ServiceOrder",
                ChargeToId_Primary = objServiceOrderLabour.ServiceOrderId,
                ChargeType_Secondary = "SOLineItem",
                ChargeToId_Secondary = objServiceOrderLabour.ServiceOrderLineItemId,
                VMRSWorkAccomplished = objServiceOrderLabour.VMRSWorkAccomplished ?? ""
            };
            timecard.PersonnelId = objServiceOrderLabour.PersonnelID ?? 0;
            timecard.Hours = objServiceOrderLabour.Hours;
            timecard.StartDate = objServiceOrderLabour.StartDate;

            timecard.CreateByPKForeignKeys_V2(this.userData.DatabaseKey);

            return timecard;
        }
        public Timecard EditLabour(ServiceOrderLabourModel objServiceOrderLabour)
        {
            Timecard timecard = new Timecard()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                ChargeType_Primary = "ServiceOrder",
                ChargeToId_Primary = objServiceOrderLabour.ServiceOrderId,
                ChargeType_Secondary = "SOLineItem",
                ChargeToId_Secondary = objServiceOrderLabour.ServiceOrderLineItemId,
                TimecardId = objServiceOrderLabour.TimecardId
            };

            timecard.Retrieve(this.userData.DatabaseKey);
            timecard.PersonnelId = objServiceOrderLabour.PersonnelID ?? 0;
            timecard.Hours = objServiceOrderLabour.Hours;
            timecard.StartDate = objServiceOrderLabour.StartDate;
            timecard.VMRSWorkAccomplished = objServiceOrderLabour.VMRSWorkAccomplished;

            timecard.UpdateByPKForeignKeys_V2(this.userData.DatabaseKey);
            return timecard;
        }
        public bool DeleteLabour(long TimecardId)
        {
            Timecard timecard = new Timecard()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                TimecardId = TimecardId
            };
            try
            {
                timecard.Delete(this.userData.DatabaseKey);
                return true;
            }
            catch
            {
                return false;
            }

        }
        public ServiceOrderLabourModel RetrieveByTimecardid(long TimecardId, long ServiceOrderId, long ServiceOrderLineItemId)
        {
            ServiceOrderLabourModel objServiceOrderLabour = new ServiceOrderLabourModel();
            Timecard timecard = new Timecard()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                ChargeType_Primary = "ServiceOrder",
                ChargeToId_Primary = ServiceOrderId,
                ChargeType_Secondary = "SOLineItem",
                ChargeToId_Secondary = ServiceOrderLineItemId,
                TimecardId = TimecardId
            };

            var ActuallaborData = Timecard.RetriveByServiceOrderId(this.userData.DatabaseKey, timecard);
            foreach (var item in ActuallaborData)
            {
                objServiceOrderLabour.PersonnelID = item.PersonnelId;
                objServiceOrderLabour.StartDate = item.StartDate;
                objServiceOrderLabour.Hours = item.Hours;
                objServiceOrderLabour.ServiceOrderId = item.ChargeToId_Primary;
                objServiceOrderLabour.ServiceOrderLineItemId = item.ChargeToId_Secondary;
                objServiceOrderLabour.Name = item.NameFirst + " " + item.NameLast;
                objServiceOrderLabour.VMRSWorkAccomplished = item.VMRSWorkAccomplished;
            }
            return objServiceOrderLabour;
        }
        #endregion

        #region Issue Part
        public PartHistory AddIssuePart(ServiceOrderIssuePartModel objServiceOrderPart)
        {
            PartHistory partHistory = new PartHistory()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                ChargeType_Primary = "ServiceOrder",
                ChargeToId_Primary = objServiceOrderPart.ServiceOrderId,
                ChargeType_Secondary = "SOLineItem",
                ChargeToId_Secondary = objServiceOrderPart.ServiceOrderLineItemId,
                PartId = objServiceOrderPart.PartId,
                VMRSFailure = objServiceOrderPart.VMRSFailure,

                IssueToClientLookupId = this.userData.DatabaseKey.Personnel.ClientLookupId,
                IssuedTo = this.userData.DatabaseKey.Personnel.PersonnelId.ToString(),
                PartStoreroomId = objServiceOrderPart.PartStoreroomId,
                TransactionDate = System.DateTime.UtcNow,
                ChargeToClientLookupId = objServiceOrderPart.ClientLookupId,
                TransactionQuantity = objServiceOrderPart.TransactionQuantity,
                PartClientLookupId = objServiceOrderPart.PartClientLookupId,
                Description = objServiceOrderPart.Description,
                TransactionType = PartHistoryTranTypes.PartIssue,
                IsPartIssue = true,
                ErrorMessagerow = null,
                PartUPCCode = objServiceOrderPart.UPCCode ?? "",
                PerformedById = this.userData.DatabaseKey.Personnel.PersonnelId,
                RequestorId = this.userData.DatabaseKey.Personnel.PersonnelId,
                IsPerformAdjustment = true
            };

            partHistory.CreateByForeignKeys_V2(this.userData.DatabaseKey);

            return partHistory;
        }
        internal long GetStoreroomId(long _partId)
        {
            List<PartStoreroom> psList = new List<PartStoreroom>();
            PartStoreroom ps = new PartStoreroom();
            ps.ClientId = userData.DatabaseKey.Client.ClientId;
            ps.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            ps.PartId = _partId;
            psList = PartStoreroom.RetrieveByPartId(userData.DatabaseKey, ps);
            dynamic PartStoreroomId = psList.First().PartStoreroomId;
            return PartStoreroomId;
        }
        #endregion

        #region Others
        public OtherCosts AddOthers(ServiceOrderOthers objServiceOrderOthers)
        {
            OtherCosts othercosts = new OtherCosts()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ObjectType = "ServiceOrder",
                ObjectId = objServiceOrderOthers.ServiceOrderId,
                Category = "Others",
                CategoryId = 0,
                Description = objServiceOrderOthers.Description,
                UnitCost = objServiceOrderOthers.UnitCost ?? 0,
                Quantity = objServiceOrderOthers.Quantity ?? 0,
                Source = objServiceOrderOthers.Source,
                VendorId = objServiceOrderOthers.VendorId ?? 0,
                ObjectType_Secondary = "SOLineItem",
                ObjectId_Secondary = objServiceOrderOthers.ServiceOrderLineItemId
            };
            othercosts.Create(this.userData.DatabaseKey);

            return othercosts;
        }


        public ServiceOrderOthers RetrieveByOthercostid(long OtherCostsId, long ServiceOrderId, long ServiceOrderLineItemId)
        {
            ServiceOrderOthers objServiceOrderOthers = new ServiceOrderOthers();
            OtherCosts othercosts = new OtherCosts()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ObjectId = ServiceOrderId,
                ObjectType = "ServiceOrder",
                ObjectType_Secondary = "SOLineItem",
                ObjectId_Secondary = ServiceOrderLineItemId,
                OtherCostsId = OtherCostsId
            };
            var OtherCostData = othercosts.RetrieveByObjectIdandOtherCostId(this.userData.DatabaseKey);
            foreach (var item in OtherCostData)
            {
                objServiceOrderOthers.Source = item.Source;
                objServiceOrderOthers.VendorId = item.VendorId;
                objServiceOrderOthers.VendorClientLookupId = item.VendorClientLookupId;
                objServiceOrderOthers.Description = item.Description;
                objServiceOrderOthers.ServiceOrderId = item.ObjectId;
                objServiceOrderOthers.ServiceOrderLineItemId = item.ObjectId_Secondary;
                objServiceOrderOthers.UnitCost = item.UnitCost;
                objServiceOrderOthers.Quantity = item.Quantity;

            }
            return objServiceOrderOthers;
        }

        public OtherCosts EditOtherscost(ServiceOrderOthers objServiceOrderOther)
        {
            OtherCosts OtherCosts = new OtherCosts()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                OtherCostsId = objServiceOrderOther.OtherCostsId
            };

            OtherCosts.Retrieve(this.userData.DatabaseKey);
            OtherCosts.Description = !string.IsNullOrEmpty(objServiceOrderOther.Description) ? objServiceOrderOther.Description : "";
            OtherCosts.UnitCost = objServiceOrderOther.UnitCost ?? 0;
            OtherCosts.Quantity = objServiceOrderOther.Quantity ?? 0;
            OtherCosts.Source = objServiceOrderOther.Source;
            OtherCosts.VendorId = objServiceOrderOther.VendorId ?? 0;

            OtherCosts.Update(this.userData.DatabaseKey);
            return OtherCosts;
        }

        public bool DeleteOther(long OtherCostsId)
        {
            OtherCosts OtherCosts = new OtherCosts()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                OtherCostsId = OtherCostsId
            };
            try
            {
                OtherCosts.Delete(this.userData.DatabaseKey);
                return true;
            }
            catch
            {
                return false;
            }

        }
        #endregion
        #endregion

        #region Add Or Edit 
        public ServiceOrder AddOrEditFleetServiceOrder(string Equip_ClientLookupId, FleetServiceVM objFSVM)
        {
            string newClientlookupId = "";
            Equipment equipment = new Equipment { ClientId = _dbKey.Client.ClientId, SiteId = _dbKey.User.DefaultSiteId, ClientLookupId = Equip_ClientLookupId };
            equipment.RetrieveByClientLookupId(_dbKey);

            string emptyValue = string.Empty;

            List<string> errList = new List<string>();
            ServiceOrder fleetServiceOrder = new ServiceOrder();
            string shift = String.Empty;
            string type = String.Empty;
            if (objFSVM.FleetServiceModel.ServiceOrderId == 0)
            {
                //Add in ServiceOrder Table
                fleetServiceOrder.ClientId = this.userData.DatabaseKey.User.ClientId;
                fleetServiceOrder.SiteId = this.userData.DatabaseKey.User.DefaultSiteId;
                fleetServiceOrder.AreaId = 0;
                fleetServiceOrder.DepartmentId = 0;
                fleetServiceOrder.StoreroomId = 0;
                if (objFSVM.FleetServiceModel.ServiceOrderClientLookupId == null && ServiceOrderStatusConstant.So_AutoGenerateEnabled)
                {
                    newClientlookupId = CustomSequentialId.GetNextId(userData.DatabaseKey, AutoGenerateKey.SO_Annual, userData.DatabaseKey.User.DefaultSiteId, "");
                }
                fleetServiceOrder.ClientLookupId = newClientlookupId;
                fleetServiceOrder.Assign_PersonnelId = 0;
                fleetServiceOrder.CancelBy_PersonnelId = 0;
                fleetServiceOrder.CancelReason = "";
                fleetServiceOrder.Description = !string.IsNullOrEmpty(objFSVM.FleetServiceModel.Description) ? objFSVM.FleetServiceModel.Description.Trim() : emptyValue;
                fleetServiceOrder.EquipmentId = equipment.EquipmentId;
                fleetServiceOrder.Shift = objFSVM.FleetServiceModel.Shift;
                fleetServiceOrder.Status = ServiceOrderStatusConstant.Open;
                fleetServiceOrder.Type = objFSVM.FleetServiceModel.Type;
                fleetServiceOrder.VendorId = 0;
                fleetServiceOrder.Create(this.userData.DatabaseKey);
                fleetServiceOrder.Retrieve(this.userData.DatabaseKey);

                //Add in ServiceOrderEventLog table
                CreateEventLog(fleetServiceOrder.ServiceOrderId, ServiceOrderStatusConstant.Open);


            }

            else
            {
                fleetServiceOrder.ServiceOrderId = objFSVM.FleetServiceModel.ServiceOrderId;
                fleetServiceOrder.Retrieve(this.userData.DatabaseKey);
                fleetServiceOrder.EquipmentId = equipment.EquipmentId;
                fleetServiceOrder.Shift = objFSVM.FleetServiceModel.Shift ?? "";
                fleetServiceOrder.Type = objFSVM.FleetServiceModel.Type ?? "";
                fleetServiceOrder.Description = !string.IsNullOrEmpty(objFSVM.FleetServiceModel.Description) ? objFSVM.FleetServiceModel.Description.Trim() : emptyValue;

                fleetServiceOrder.Update(this.userData.DatabaseKey);

            }

            return fleetServiceOrder;
        }

        #endregion
        #region Search View
        public List<List<ServiceOrderSchedule>> SOSchedulePersonnelList(string ServiceOrderId = "")
        {
            ServiceOrderSchedule SO = new ServiceOrderSchedule();
            SO.ClientId = userData.DatabaseKey.Client.ClientId;
            SO.SiteId = userData.Site.SiteId;
            SO.ServiceOrderId = string.IsNullOrEmpty(ServiceOrderId) ? 0 : Convert.ToInt64(ServiceOrderId);
            SO.RetrievePersonnel(userData.DatabaseKey);
            return SO.TotalRecords;
        }
        #endregion

        #region Fleet Only
        public int GetCount()
        {
            int count = 0;
            ServiceOrder sjob = new ServiceOrder()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.Personnel.SiteId
            };
            var serviceOrderJobs = sjob.RetrieveDashboardChart(userData.DatabaseKey, sjob);
            if (serviceOrderJobs != null && serviceOrderJobs.Count > 0)
            {
                count = serviceOrderJobs[0].ServiceOrderCount;
            }
            return count;
        }
        #endregion

        #region Cancel SO
        public ServiceOrder CancelJob(long ServiceorderId, string CancelReason)
        {
            ServiceOrder serviceOrder = new ServiceOrder()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                ServiceOrderId = ServiceorderId,
                SiteId = userData.DatabaseKey.Personnel.SiteId
            };
            serviceOrder.Retrieve(userData.DatabaseKey);
            serviceOrder.Status = ServiceOrderStatusConstant.Canceled;
            serviceOrder.CancelReason = CancelReason.Trim();
            serviceOrder.CancelDate = DateTime.UtcNow;
            serviceOrder.CancelBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            serviceOrder.Update(this.userData.DatabaseKey);

            if (serviceOrder.ErrorMessages == null)
            {
                CreateEventLog(serviceOrder.ServiceOrderId, ServiceOrderStatusConstant.Canceled);
                Int64 CallerUserInfoId = this.userData.DatabaseKey.User.UserInfoId;
                List<object> listSO = new List<object>();
                listSO.Add(serviceOrder.ServiceOrderId);
                ProcessAlert objAlert = new ProcessAlert(this.userData);
                objAlert.CreateAlert<ServiceOrder>(this.userData, serviceOrder, AlertTypeEnum.ServiceOrderCancel, CallerUserInfoId, listSO);
            }
            return serviceOrder;
        }
        private void CreateEventLog(Int64 SOId, string Status)
        {
            ServiceOrderEventLog log = new ServiceOrderEventLog();
            log.ClientId = userData.DatabaseKey.Client.ClientId;
            log.SiteId = userData.DatabaseKey.Personnel.SiteId;
            log.ServiceOrderId = SOId;
            log.Event = Status;
            log.TransactionDate = DateTime.UtcNow;
            log.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            log.Comments = "";
            log.SourceTable = "";
            log.SourceId = 0;
            log.Create(userData.DatabaseKey);
        }
        #endregion

        #region Reopen SO
        public ServiceOrder ReopenJob(long ServiceorderId)
        {
            ServiceOrder serviceOrder = new ServiceOrder()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                ServiceOrderId = ServiceorderId,
                SiteId = userData.DatabaseKey.Personnel.SiteId
            };
            serviceOrder.Retrieve(userData.DatabaseKey);
            serviceOrder.Status = ServiceOrderStatusConstant.Open;
            serviceOrder.CancelReason = "";
            serviceOrder.CancelDate = null;
            serviceOrder.CancelBy_PersonnelId = 0;
            serviceOrder.CompleteDate = null;
            serviceOrder.CompleteBy_PersonnelId = 0;
            serviceOrder.Update(this.userData.DatabaseKey);

            if (serviceOrder.ErrorMessages == null)
            {
                CreateEventLog(serviceOrder.ServiceOrderId, ServiceOrderStatusConstant.ReOpen);
                Int64 CallerUserInfoId = this.userData.DatabaseKey.User.UserInfoId;
                List<object> listSO = new List<object>();
                listSO.Add(serviceOrder.ServiceOrderId);
                ProcessAlert objAlert = new ProcessAlert(this.userData);
                objAlert.CreateAlert<ServiceOrder>(this.userData, serviceOrder, AlertTypeEnum.WorkOrderCancel, CallerUserInfoId, listSO);
            }
            return serviceOrder;
        }
        #endregion

        #region Complete SO
        public List<string> CompleteFleetService(CompleteServiceOrderModel CompleteServiceOrderModel)
        {
            Equipment equipment = new DataContracts.Equipment()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                EquipmentId = CompleteServiceOrderModel.EquipmentId,
                ObjectName = AttachmentTableConstant.FleetMeterReading
            };
            equipment.RetrieveByPKForeignKeys_V2(_dbKey);
            var fmdate = CompleteServiceOrderModel.CurrentReadingDate;
            DateTime FMDateTime = DateTime.ParseExact(fmdate + " " + CompleteServiceOrderModel.CurrentReadingTime, "MM/dd/yyyy h:mm tt", CultureInfo.InvariantCulture);

            List<string> errList = new List<string>();

            //--for meter1
            if (CompleteServiceOrderModel.MetersAssociated == "single")
            {
                //--validation for single meter
                equipment.Reading = CompleteServiceOrderModel.SOMeter1CurrentReading;
                if (!CompleteServiceOrderModel.Meter1Void)  //--for non-void record add
                {
                    equipment.CheckMeter1CurrentReading(_dbKey);
                    if (equipment.ErrorMessages != null && equipment.ErrorMessages.Count > 0)
                    {
                        return equipment.ErrorMessages;
                    }
                    else
                    {
                        #region Service Order Update
                        ServiceOrder serviceOrder = new ServiceOrder()
                        {
                            ClientId = userData.DatabaseKey.Client.ClientId,
                            ServiceOrderId = CompleteServiceOrderModel.ServiceOrderId,
                            SiteId = userData.DatabaseKey.Personnel.SiteId
                        };
                        serviceOrder.Retrieve(userData.DatabaseKey);
                        serviceOrder.CompleteDate = DateTime.UtcNow;
                        serviceOrder.CompleteBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
                        serviceOrder.Status = ServiceOrderStatusConstant.Complete;
                        serviceOrder.Update(this.userData.DatabaseKey);
                        if (serviceOrder.ErrorMessages != null && serviceOrder.ErrorMessages.Count > 0)
                        {
                            errList.AddRange(serviceOrder.ErrorMessages);
                        }
                        #endregion

                        if (errList.Count == 0)
                        {
                            Task[] tasks = new Task[2];

                            #region Service Order Event Log Insert
                            tasks[0] = Task.Factory.StartNew(() => CreateEventLog(serviceOrder.ServiceOrderId, ServiceOrderStatusConstant.Complete));
                            #endregion

                            #region FleetMeterReading Insert
                            FleetMeterReading fleetMeter = new FleetMeterReading();
                            fleetMeter.ClientId = this.userData.DatabaseKey.User.ClientId;
                            fleetMeter.EquipmentId = equipment.EquipmentId;
                            fleetMeter.Meter2Indicator = false;
                            fleetMeter.Reading = CompleteServiceOrderModel.SOMeter1CurrentReading;
                            fleetMeter.ReadingDate = FMDateTime;
                            fleetMeter.SourceId = serviceOrder.ServiceOrderId;    // RKL - The source id for a fleet meter reading is the service order id
                            fleetMeter.SourceType = SourceTypeConstant.ServiceOrder;
                            fleetMeter.Void = CompleteServiceOrderModel.Meter1Void;
                            //tasks[1] = Task.Factory.StartNew(() => fleetMeter.Create(_dbKey));
                            fleetMeter.Create(_dbKey);
                            if (fleetMeter.ErrorMessages != null && fleetMeter.ErrorMessages.Count > 0)
                            {
                                errList.AddRange(fleetMeter.ErrorMessages);
                            }
                            #endregion

                            #region  update equipment
                            equipment.Meter1CurrentReading = CompleteServiceOrderModel.SOMeter1CurrentReading;
                            equipment.Meter1CurrentReadingDate = FMDateTime;
                            equipment.FirstMeterVoid = CompleteServiceOrderModel.Meter1Void;
                            //tasks[2] = Task.Factory.StartNew(() => equipment.UpdateForFleetMeter(_dbKey));
                            equipment.UpdateForFleetMeter(_dbKey);
                            if (equipment.ErrorMessages != null && equipment.ErrorMessages.Count > 0)
                            {
                                errList.AddRange(equipment.ErrorMessages);
                            }
                            #endregion
                            #region FleetIssue Update
                            List<DataContracts.FleetIssues> FleetIssuesList = new List<FleetIssues>();
                            FleetIssues objFI = new FleetIssues()
                            {
                                ClientId = this.userData.DatabaseKey.Client.ClientId,
                                ServiceOrderId = CompleteServiceOrderModel.ServiceOrderId,
                            };

                            FleetIssuesList = objFI.RetrieveByServiceOrderId_V2(this.userData.DatabaseKey);
                            if (FleetIssuesList.Count > 0)
                            {
                                foreach (var item in FleetIssuesList)
                                {
                                    objFI.FleetIssuesId = item.FleetIssuesId;
                                    objFI.ClientId = item.ClientId;
                                    objFI.SiteId = item.SiteId;
                                    objFI.AreaId = item.AreaId;
                                    objFI.DepartmentId = item.DepartmentId;
                                    objFI.StoreroomId = item.StoreroomId;
                                    objFI.Defects = item.Defects;
                                    objFI.Description = item.Description;
                                    objFI.RecordDate = item.RecordDate;
                                    objFI.EquipmentId = item.EquipmentId;
                                    objFI.DriverName = item.DriverName;
                                    objFI.CompleteDate = DateTime.UtcNow.Date;
                                    objFI.Status = ServiceOrderStatusConstant.Complete;
                                    objFI.Update(this.userData.DatabaseKey);
                                    if (objFI.ErrorMessages != null && objFI.ErrorMessages.Count > 0)
                                    {
                                        errList.AddRange(objFI.ErrorMessages);
                                    }
                                }

                            }
                            #endregion

                            //V2-421 Scheduled service update
                            errList.AddRange(ScheduledServiceUpdate(CompleteServiceOrderModel.ServiceOrderId, CompleteServiceOrderModel.SOMeter1CurrentReading, CompleteServiceOrderModel.SOMeter2CurrentReading, CompleteServiceOrderModel.Meter1Type, CompleteServiceOrderModel.Meter2Type));

                            List<long> sos = new List<long>() { serviceOrder.ServiceOrderId };
                            tasks[1] = Task.Factory.StartNew(() => this.SendAlert(sos));
                        }

                        return errList;
                    }
                }
                else   //-- for void record add
                {
                    #region Service Order Update
                    ServiceOrder serviceOrder = new ServiceOrder()
                    {
                        ClientId = userData.DatabaseKey.Client.ClientId,
                        ServiceOrderId = CompleteServiceOrderModel.ServiceOrderId,
                        SiteId = userData.DatabaseKey.Personnel.SiteId
                    };
                    serviceOrder.Retrieve(userData.DatabaseKey);
                    serviceOrder.CompleteDate = DateTime.UtcNow;
                    serviceOrder.CompleteBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
                    serviceOrder.Status = ServiceOrderStatusConstant.Complete;
                    serviceOrder.Update(this.userData.DatabaseKey);
                    if (serviceOrder.ErrorMessages != null && serviceOrder.ErrorMessages.Count > 0)
                    {
                        errList.AddRange(serviceOrder.ErrorMessages);
                    }
                    #endregion

                    if (errList.Count == 0)
                    {
                        Task[] tasks = new Task[2];

                        #region Service Order Event Log Insert
                        tasks[0] = Task.Factory.StartNew(() => CreateEventLog(serviceOrder.ServiceOrderId, ServiceOrderStatusConstant.Complete));
                        #endregion

                        #region FleetMeterReading Insert
                        FleetMeterReading fleetMeter = new FleetMeterReading();
                        fleetMeter.ClientId = this.userData.DatabaseKey.User.ClientId;
                        fleetMeter.EquipmentId = equipment.EquipmentId;
                        fleetMeter.Meter2Indicator = false;
                        fleetMeter.Reading = CompleteServiceOrderModel.SOMeter1CurrentReading;
                        fleetMeter.ReadingDate = FMDateTime;
                        fleetMeter.SourceId = serviceOrder.ServiceOrderId;  // RKL 2021-Feb-13
                        fleetMeter.SourceType = SourceTypeConstant.ServiceOrder;
                        fleetMeter.Void = CompleteServiceOrderModel.Meter1Void;
                        //tasks[1] = Task.Factory.StartNew(() => fleetMeter.Create(_dbKey));
                        fleetMeter.Create(_dbKey);
                        if (fleetMeter.ErrorMessages != null && fleetMeter.ErrorMessages.Count > 0)
                        {
                            errList.AddRange(fleetMeter.ErrorMessages);
                        }
                        #endregion

                        #region FleetIssue Update
                        List<DataContracts.FleetIssues> FleetIssuesList = new List<FleetIssues>();
                        FleetIssues objFI = new FleetIssues()
                        {
                            ClientId = this.userData.DatabaseKey.Client.ClientId,
                            ServiceOrderId = CompleteServiceOrderModel.ServiceOrderId,
                        };

                        FleetIssuesList = objFI.RetrieveByServiceOrderId_V2(this.userData.DatabaseKey);
                        if (FleetIssuesList.Count > 0)
                        {
                            foreach (var item in FleetIssuesList)
                            {
                                objFI.FleetIssuesId = item.FleetIssuesId;
                                objFI.ClientId = item.ClientId;
                                objFI.SiteId = item.SiteId;
                                objFI.AreaId = item.AreaId;
                                objFI.DepartmentId = item.DepartmentId;
                                objFI.StoreroomId = item.StoreroomId;
                                objFI.Defects = item.Defects;
                                objFI.Description = item.Description;
                                objFI.RecordDate = item.RecordDate;
                                objFI.EquipmentId = item.EquipmentId;
                                objFI.DriverName = item.DriverName;
                                objFI.CompleteDate = DateTime.UtcNow.Date;
                                objFI.Status = ServiceOrderStatusConstant.Complete;
                                objFI.Update(this.userData.DatabaseKey);
                                if (objFI.ErrorMessages != null && objFI.ErrorMessages.Count > 0)
                                {
                                    errList.AddRange(objFI.ErrorMessages);
                                }
                            }

                        }
                        #endregion

                        //V2-421 Scheduled service update
                        errList.AddRange(ScheduledServiceUpdate(CompleteServiceOrderModel.ServiceOrderId, CompleteServiceOrderModel.SOMeter1CurrentReading, CompleteServiceOrderModel.SOMeter2CurrentReading, CompleteServiceOrderModel.Meter1Type, CompleteServiceOrderModel.Meter2Type));

                        List<long> sos = new List<long>() { serviceOrder.ServiceOrderId };
                        tasks[1] = Task.Factory.StartNew(() => this.SendAlert(sos));
                    }

                    return errList;
                }
            }
            //--for meter1 and 2
            else
            {
                equipment.Meter1CurrentReading = CompleteServiceOrderModel.SOMeter1CurrentReading;
                equipment.Meter2CurrentReading = CompleteServiceOrderModel.SOMeter2CurrentReading;
                equipment.FirstMeterVoid = CompleteServiceOrderModel.Meter1Void;
                equipment.SecondMeterVoid = CompleteServiceOrderModel.Meter2Void;

                if (CompleteServiceOrderModel.Meter1Void && CompleteServiceOrderModel.Meter2Void)  //--for both void record add
                {
                    //--add to fleetmeter only


                    #region Service Order Update
                    ServiceOrder serviceOrder = new ServiceOrder()
                    {
                        ClientId = userData.DatabaseKey.Client.ClientId,
                        ServiceOrderId = CompleteServiceOrderModel.ServiceOrderId,
                        SiteId = userData.DatabaseKey.Personnel.SiteId
                    };
                    serviceOrder.Retrieve(userData.DatabaseKey);
                    serviceOrder.CompleteDate = DateTime.UtcNow;
                    serviceOrder.CompleteBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
                    serviceOrder.Status = ServiceOrderStatusConstant.Complete;
                    serviceOrder.Update(this.userData.DatabaseKey);
                    if (serviceOrder.ErrorMessages != null && serviceOrder.ErrorMessages.Count > 0)
                    {
                        errList.AddRange(serviceOrder.ErrorMessages);
                    }
                    #endregion

                    if (errList.Count > 0)
                    {
                        Task[] tasks = new Task[2];

                        #region Service Order Event Log Insert
                        tasks[0] = Task.Factory.StartNew(() => CreateEventLog(serviceOrder.ServiceOrderId, ServiceOrderStatusConstant.Complete));
                        #endregion

                        #region FleetMeterReading Insert
                        //--Primary Meter
                        FleetMeterReading fleetMeter = new FleetMeterReading();
                        fleetMeter.ClientId = this.userData.DatabaseKey.User.ClientId;
                        fleetMeter.EquipmentId = equipment.EquipmentId;
                        fleetMeter.ReadingDate = FMDateTime;
                        //fleetMeter.SourceId = 0;
                        fleetMeter.SourceId = serviceOrder.ServiceOrderId;  // RKL 2021-Feb-13
                        fleetMeter.SourceType = SourceTypeConstant.ServiceOrder;
                        fleetMeter.Meter2Indicator = false;
                        fleetMeter.Reading = CompleteServiceOrderModel.SOMeter1CurrentReading;
                        fleetMeter.Void = CompleteServiceOrderModel.Meter1Void;
                        //tasks[1] = Task.Factory.StartNew(() => fleetMeter.Create(_dbKey));
                        fleetMeter.Create(_dbKey);
                        if (fleetMeter.ErrorMessages != null && fleetMeter.ErrorMessages.Count > 0)
                        {
                            errList.AddRange(fleetMeter.ErrorMessages);
                        }

                        //--Secondary Meter
                        FleetMeterReading fleetMeter2 = new FleetMeterReading();
                        fleetMeter2.ClientId = this.userData.DatabaseKey.User.ClientId;
                        fleetMeter2.EquipmentId = equipment.EquipmentId;
                        fleetMeter2.ReadingDate = FMDateTime;
                        //fleetMeter2.SourceId = 0;
                        fleetMeter2.SourceId = serviceOrder.ServiceOrderId;  // RKL 2021-Feb-13
                        fleetMeter2.SourceType = SourceTypeConstant.ServiceOrder;
                        fleetMeter2.Meter2Indicator = true;
                        fleetMeter2.Reading = CompleteServiceOrderModel.SOMeter2CurrentReading;
                        fleetMeter2.Void = CompleteServiceOrderModel.Meter2Void;
                        //tasks[2] = Task.Factory.StartNew(() => fleetMeter2.Create(_dbKey));
                        fleetMeter2.Create(_dbKey);
                        if (fleetMeter2.ErrorMessages != null && fleetMeter2.ErrorMessages.Count > 0)
                        {
                            errList.AddRange(fleetMeter2.ErrorMessages);
                        }
                        #endregion

                        #region FleetIssue Update
                        List<DataContracts.FleetIssues> FleetIssuesList = new List<FleetIssues>();
                        FleetIssues objFI = new FleetIssues()
                        {
                            ClientId = this.userData.DatabaseKey.Client.ClientId,
                            ServiceOrderId = CompleteServiceOrderModel.ServiceOrderId,
                        };

                        FleetIssuesList = objFI.RetrieveByServiceOrderId_V2(this.userData.DatabaseKey);
                        if (FleetIssuesList.Count > 0)
                        {
                            foreach (var item in FleetIssuesList)
                            {
                                objFI.FleetIssuesId = item.FleetIssuesId;
                                objFI.ClientId = item.ClientId;
                                objFI.SiteId = item.SiteId;
                                objFI.AreaId = item.AreaId;
                                objFI.DepartmentId = item.DepartmentId;
                                objFI.StoreroomId = item.StoreroomId;
                                objFI.Defects = item.Defects;
                                objFI.Description = item.Description;
                                objFI.RecordDate = item.RecordDate;
                                objFI.EquipmentId = item.EquipmentId;
                                objFI.DriverName = item.DriverName;
                                objFI.CompleteDate = DateTime.UtcNow.Date;
                                objFI.Status = ServiceOrderStatusConstant.Complete;
                                objFI.Update(this.userData.DatabaseKey);
                                if (objFI.ErrorMessages != null && objFI.ErrorMessages.Count > 0)
                                {
                                    errList.AddRange(objFI.ErrorMessages);
                                }
                            }

                        }
                        #endregion

                        //V2-421 Scheduled service update
                        errList.AddRange(ScheduledServiceUpdate(CompleteServiceOrderModel.ServiceOrderId, CompleteServiceOrderModel.Meter1CurrentReading, CompleteServiceOrderModel.Meter2CurrentReading, CompleteServiceOrderModel.Meter1Type, CompleteServiceOrderModel.Meter2Type));

                        List<long> sos = new List<long>() { serviceOrder.ServiceOrderId };
                        tasks[1] = Task.Factory.StartNew(() => this.SendAlert(sos));
                    }

                    return errList;
                }

                else  //--for either non-void record add
                {
                    //--validation for both meters

                    equipment.CheckBothMeterReading(_dbKey);
                    if (equipment.ErrorMessages != null && equipment.ErrorMessages.Count > 0)
                    {
                        return equipment.ErrorMessages;
                    }
                    else
                    {
                        #region Service Order Update
                        ServiceOrder serviceOrder = new ServiceOrder()
                        {
                            ClientId = userData.DatabaseKey.Client.ClientId,
                            ServiceOrderId = CompleteServiceOrderModel.ServiceOrderId,
                            SiteId = userData.DatabaseKey.Personnel.SiteId
                        };
                        serviceOrder.Retrieve(userData.DatabaseKey);
                        serviceOrder.CompleteDate = DateTime.UtcNow;
                        serviceOrder.CompleteBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
                        serviceOrder.Status = ServiceOrderStatusConstant.Complete;
                        serviceOrder.Update(this.userData.DatabaseKey);
                        if (serviceOrder.ErrorMessages != null && serviceOrder.ErrorMessages.Count > 0)
                        {
                            errList.AddRange(serviceOrder.ErrorMessages);
                        }
                        #endregion

                        if (errList.Count == 0)
                        {
                            Task[] tasks = new Task[2];

                            #region Service Order Event Log Insert
                            tasks[0] = Task.Factory.StartNew(() => CreateEventLog(serviceOrder.ServiceOrderId, ServiceOrderStatusConstant.Complete));
                            #endregion

                            #region  FleetMeterReading Insert

                            //--Primary Meter
                            FleetMeterReading fleetMeter = new FleetMeterReading();
                            fleetMeter.ClientId = this.userData.DatabaseKey.User.ClientId;
                            fleetMeter.EquipmentId = equipment.EquipmentId;
                            fleetMeter.ReadingDate = FMDateTime;
                            //fleetMeter.SourceId = 0;
                            fleetMeter.SourceId = serviceOrder.ServiceOrderId;  // RKL 2021-Feb-13
                            fleetMeter.SourceType = SourceTypeConstant.ServiceOrder;
                            fleetMeter.Meter2Indicator = false;
                            fleetMeter.Reading = CompleteServiceOrderModel.SOMeter1CurrentReading;
                            fleetMeter.Void = CompleteServiceOrderModel.Meter1Void;
                            //tasks[1] = Task.Factory.StartNew(() => fleetMeter.Create(_dbKey));
                            fleetMeter.Create(_dbKey);
                            if (fleetMeter.ErrorMessages != null && fleetMeter.ErrorMessages.Count > 0)
                            {
                                errList.AddRange(fleetMeter.ErrorMessages);
                            }

                            //--Secondary Meter
                            FleetMeterReading fleetMeter2 = new FleetMeterReading();
                            fleetMeter2.ClientId = this.userData.DatabaseKey.User.ClientId;
                            fleetMeter2.EquipmentId = equipment.EquipmentId;
                            fleetMeter2.ReadingDate = FMDateTime;
                            //fleetMeter2.SourceId = 0;
                            fleetMeter2.SourceId = serviceOrder.ServiceOrderId;  // RKL 2021-Feb-13
                            fleetMeter2.SourceType = SourceTypeConstant.ServiceOrder;
                            fleetMeter2.Meter2Indicator = true;
                            fleetMeter2.Reading = CompleteServiceOrderModel.SOMeter2CurrentReading;
                            fleetMeter2.Void = CompleteServiceOrderModel.Meter2Void;
                            //tasks[2] = Task.Factory.StartNew(() => fleetMeter2.Create(_dbKey));
                            fleetMeter2.Create(_dbKey);
                            if (fleetMeter2.ErrorMessages != null && fleetMeter2.ErrorMessages.Count > 0)
                            {
                                errList.AddRange(fleetMeter2.ErrorMessages);
                            }
                            #endregion

                            #region  update equipment
                            equipment.Meter1CurrentReading = CompleteServiceOrderModel.SOMeter1CurrentReading;
                            equipment.Meter1CurrentReadingDate = FMDateTime;
                            equipment.Meter2CurrentReading = CompleteServiceOrderModel.SOMeter2CurrentReading;
                            equipment.Meter2CurrentReadingDate = FMDateTime;
                            equipment.FirstMeterVoid = CompleteServiceOrderModel.Meter1Void;
                            equipment.SecondMeterVoid = CompleteServiceOrderModel.Meter2Void;
                            //tasks[3] = Task.Factory.StartNew(() => equipment.UpdateForFleetMeter(_dbKey));
                            equipment.UpdateForFleetMeter(_dbKey);
                            if (equipment.ErrorMessages != null && equipment.ErrorMessages.Count > 0)
                            {
                                errList.AddRange(equipment.ErrorMessages);
                            }
                            #endregion

                            #region FleetIssue Update
                            List<DataContracts.FleetIssues> FleetIssuesList = new List<FleetIssues>();
                            FleetIssues objFI = new FleetIssues()
                            {
                                ClientId = this.userData.DatabaseKey.Client.ClientId,
                                ServiceOrderId = CompleteServiceOrderModel.ServiceOrderId,
                            };

                            FleetIssuesList = objFI.RetrieveByServiceOrderId_V2(this.userData.DatabaseKey);
                            if (FleetIssuesList.Count > 0)
                            {
                                foreach (var item in FleetIssuesList)
                                {
                                    objFI.FleetIssuesId = item.FleetIssuesId;
                                    objFI.ClientId = item.ClientId;
                                    objFI.SiteId = item.SiteId;
                                    objFI.AreaId = item.AreaId;
                                    objFI.DepartmentId = item.DepartmentId;
                                    objFI.StoreroomId = item.StoreroomId;
                                    objFI.Defects = item.Defects;
                                    objFI.Description = item.Description;
                                    objFI.RecordDate = item.RecordDate;
                                    objFI.EquipmentId = item.EquipmentId;
                                    objFI.DriverName = item.DriverName;
                                    objFI.CompleteDate = DateTime.UtcNow.Date;
                                    objFI.Status = ServiceOrderStatusConstant.Complete;
                                    objFI.Update(this.userData.DatabaseKey);
                                    if (objFI.ErrorMessages != null && objFI.ErrorMessages.Count > 0)
                                    {
                                        errList.AddRange(objFI.ErrorMessages);
                                    }
                                }

                            }
                            #endregion

                            //V2-421 Scheduled service update
                            errList.AddRange(ScheduledServiceUpdate(CompleteServiceOrderModel.ServiceOrderId, CompleteServiceOrderModel.SOMeter1CurrentReading, CompleteServiceOrderModel.SOMeter2CurrentReading, CompleteServiceOrderModel.Meter1Type, CompleteServiceOrderModel.Meter2Type));


                            List<long> sos = new List<long>() { serviceOrder.ServiceOrderId };
                            tasks[1] = Task.Factory.StartNew(() => this.SendAlert(sos));
                        }
                        return errList;
                    }
                }
            }

        }

        private void SendAlert(List<long> Sos)
        {
            ProcessAlert objAlert = new ProcessAlert(userData);
            objAlert.CreateAlert<ServiceOrder>(AlertTypeEnum.ServiceOrderComplete, Sos);
        }

        #endregion

        #region Service order history
        public List<FleetServiceModel> ServiceOrderHistory(string orderbycol = "", string orderDir = "", int skip = 0, int length = 0, long AssetID = 0, long ServiceOrderId = 0,
              string ClientLookupId = "", string AssetId = "", string AssetName = "", string Status = "", string Type = "", string CreatedDate = "",
              string CompletedDate = "")
        {
            ServiceOrder ServiceOrder = new ServiceOrder();
            FleetServiceModel SModel;
            List<FleetServiceModel> fleetServiceModelList = new List<FleetServiceModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<string> StatusList = new List<string>();
            List<string> AssetIdList = new List<string>();
            ServiceOrder.ClientId = userData.DatabaseKey.Client.ClientId;
            ServiceOrder.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            ServiceOrder.orderbyColumn = orderbycol;
            ServiceOrder.orderBy = orderDir;
            ServiceOrder.offset1 = Convert.ToString(skip);
            ServiceOrder.nextrow = Convert.ToString(length);

            ServiceOrder.EquipmentId = AssetID;
            ServiceOrder.ServiceOrderId = ServiceOrderId;

            ServiceOrder.ClientLookupId = ClientLookupId;
            ServiceOrder.EquipmentClientLookupId = AssetId;
            ServiceOrder.AssetName = AssetName;
            ServiceOrder.Status = Status;
            ServiceOrder.Type = Type;
            DateTime dateTime = new DateTime();
            bool correctConvert;
            if (!string.IsNullOrEmpty(CreatedDate))
            {                
                correctConvert = DateTime.TryParseExact(CreatedDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
                if (correctConvert)
                {
                    ServiceOrder.CreateDate = dateTime;
                }
                else
                {
                    ServiceOrder.CreateDate = DateTime.MinValue;
                }
            }

            if (!string.IsNullOrEmpty(CompletedDate))
            {
                correctConvert = DateTime.TryParseExact(CompletedDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
                if (correctConvert)
                {
                    ServiceOrder.CompleteDate = dateTime;
                }
                else
                {
                    ServiceOrder.CompleteDate = null;
                }
            }
            List<ServiceOrder> ServiceOrderList = ServiceOrder.RetrieveServiceOrderHistory(this.userData.DatabaseKey);

            if (ServiceOrderList != null)
            {
                foreach (var item in ServiceOrderList)
                {
                    SModel = new FleetServiceModel();
                    SModel.ServiceOrderId = item.ServiceOrderId;
                    SModel.ClientLookupId = item.ClientLookupId;
                    SModel.EquipmentClientLookupId = item.EquipmentClientLookupId;
                    SModel.AssetName = item.AssetName;
                    SModel.Status = item.Status;
                    SModel.Type = item.Type;
                    if (item.CreateDate != null && item.CreateDate == default(DateTime))
                    {
                        SModel.CreateDate = null;
                    }
                    else
                    {
                        SModel.CreateDate = item.CreateDate;
                    }

                    if (item.CompleteDate != null && item.CompleteDate == default(DateTime))
                    {
                        SModel.CompleteDate = null;
                    }
                    else
                    {
                        SModel.CompleteDate = item.CompleteDate;
                    }

                    SModel.ChildCount = item.ChildCount;
                    SModel.TotalCount = item.TotalCount;
                    fleetServiceModelList.Add(SModel);
                }
            }



            return fleetServiceModelList;
        }
        #endregion

        #region Scheduled service for service order
        public List<FleetScheduledServiceSearchModel> RetrieveScheduledServiceByAssetId(string orderbycol = "0", string orderDir = "asc", int skip = 0, int length = 0, long AssetId = 0, string ServiceTaskDesc = "")
        {
            FleetScheduledServiceSearchModel scServSearchModel;
            List<FleetScheduledServiceSearchModel> scServSearchModelList = new List<FleetScheduledServiceSearchModel>();
            List<DataContracts.ScheduledService> scheduledServiceList = new List<DataContracts.ScheduledService>();
            DataContracts.ScheduledService scheduledService = new DataContracts.ScheduledService();
            scheduledService.ClientId = this.userData.DatabaseKey.Client.ClientId;
            scheduledService.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            scheduledService.OrderbyColumn = orderbycol;
            scheduledService.OrderBy = orderDir;
            scheduledService.OffSetVal = skip;
            scheduledService.NextRow = length;
            scheduledService.EquipmentId = AssetId;
            scheduledService.ServiceTaskDesc = ServiceTaskDesc;
            scheduledServiceList = scheduledService.RetrieveScheduledServiceForServiceOrder(userData.DatabaseKey, userData.Site.TimeZone);
            foreach (var item in scheduledServiceList)
            {
                scServSearchModel = new FleetScheduledServiceSearchModel();
                scServSearchModel.ScheduledServiceId = item.ScheduledServiceId;
                scServSearchModel.ServiceTaskId = item.ServiceTaskId;
                scServSearchModel.ServiceTask = item.ServiceTasksDescription;
                scServSearchModel.Schedule = item.Schedule;
                scServSearchModel.NextDue = item.NextDue;
                scServSearchModel.LastCompletedstr = item.LastCompletedstr;
                scServSearchModel.TotalCount = item.TotalCount;
                scServSearchModel.RepairReason = item.RepairReason;
                scServSearchModel.System = item.VMRSSystem;
                scServSearchModel.Assembly = item.VMRSAssembly;
                scServSearchModelList.Add(scServSearchModel);
            }
            return scServSearchModelList;
        }
        public List<string> BulkLineItemAdd(string[] ScheduledServiceIds, long ServiceOrderId)
        {
            List<string> errorMessage = new List<string>();
            if (ScheduledServiceIds.Length > 0)
            {
                foreach (var item in ScheduledServiceIds)
                {
                    DataContracts.ScheduledService scheduledService = new DataContracts.ScheduledService()
                    {
                        ScheduledServiceId = Convert.ToInt64(item)
                    };
                    scheduledService.Retrieve(_dbKey);

                    ServiceOrderLineItem serviceOrderLineItem = new ServiceOrderLineItem()
                    {
                        ClientId = this.userData.DatabaseKey.Client.ClientId,
                        SchedServiceId = scheduledService.ScheduledServiceId,
                        ServiceOrderId = ServiceOrderId,
                        ServiceTaskId = scheduledService.ServiceTaskId,
                        RepairReason = scheduledService.RepairReason,
                        VMRSSystem = scheduledService.VMRSSystem,
                        VMRSAssembly = scheduledService.VMRSAssembly,
                        Comment = "",
                    };
                    serviceOrderLineItem.Create(this.userData.DatabaseKey);
                    if (serviceOrderLineItem.ErrorMessages != null && serviceOrderLineItem.ErrorMessages.Count > 0)
                    {
                        errorMessage.AddRange(serviceOrderLineItem.ErrorMessages);
                    }

                }
            }
            return errorMessage;
        }
        public List<string> ScheduledServiceUpdate(long ServiceOrderId, decimal LastPerformedMeter1, decimal LastPerformedMeter2, string Meter1Type, string Meter2Type)
        {
            List<string> errList = new List<string>();
            ServiceOrderLineItem serviceOrderLineItem = new ServiceOrderLineItem()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ServiceOrderId = ServiceOrderId
            };
            List<ServiceOrderLineItem> ServiceOrderLineItemList = ServiceOrderLineItem.ServiceOrderLineItemRetrieveByServiceOrderId(this.userData.DatabaseKey, serviceOrderLineItem);

            ServiceOrderLineItemList.Where(x => x.SchedServiceId > 0).ToList().ForEach(item =>
            {

                DataContracts.ScheduledService scheduledService = new DataContracts.ScheduledService()
                {
                    ClientId = this.userData.DatabaseKey.Client.ClientId,
                    SiteId = userData.DatabaseKey.User.DefaultSiteId,
                    ScheduledServiceId = item.SchedServiceId
                };
                scheduledService.Retrieve(_dbKey);
                scheduledService.Last_ServiceOrderId = ServiceOrderId;
                scheduledService.LastPerformedDate = DateTime.UtcNow;
                if (LastPerformedMeter1 > 0 && !string.IsNullOrEmpty(Meter1Type))
                {
                    scheduledService.LastPerformedMeter1 = LastPerformedMeter1;
                    scheduledService.NextDueMeter1 = LastPerformedMeter1 + scheduledService.Meter1Interval;
                }
                if (LastPerformedMeter2 > 0 && !string.IsNullOrEmpty(Meter2Type))
                {
                    scheduledService.LastPerformedMeter2 = LastPerformedMeter2;
                    scheduledService.NextDueMeter2 = LastPerformedMeter2 + scheduledService.Meter2Interval;
                }
                if (scheduledService.TimeIntervalType == TimeTypeConstants.Days)
                {
                    scheduledService.NextDueDate = DateTime.UtcNow.AddDays(scheduledService.TimeInterval);
                }
                else if (scheduledService.TimeIntervalType == TimeTypeConstants.Weeks)
                {
                    scheduledService.NextDueDate = DateTime.UtcNow.AddDays(scheduledService.TimeInterval * 7);
                }
                else if (scheduledService.TimeIntervalType == TimeTypeConstants.Months)
                {
                    scheduledService.NextDueDate = DateTime.UtcNow.AddMonths(scheduledService.TimeInterval);
                }
                else if (scheduledService.TimeIntervalType == TimeTypeConstants.Years)
                {
                    scheduledService.NextDueDate = DateTime.UtcNow.AddYears(scheduledService.TimeInterval);
                }
                scheduledService.Update(_dbKey);
                if (scheduledService.ErrorMessages != null && scheduledService.ErrorMessages.Count > 0)
                {
                    errList.AddRange(scheduledService.ErrorMessages);
                }

            });
            return errList;
        }
        #endregion
    }
}


