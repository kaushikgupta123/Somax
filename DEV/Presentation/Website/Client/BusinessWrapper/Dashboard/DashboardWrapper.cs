using Client.BusinessWrapper.Common;
using Client.Common;
using Client.Models;
using Client.Models.Dashboard;
using Client.Models.Work_Order;
using Client.Models.WorkOrder;

using Common.Constants;
using Common.Enumerations;
using Common.Extensions;

using DataContracts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Client.BusinessWrapper
{
    public class DashboardWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        List<string> errorMessage = new List<string>();

        public DashboardWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        #region Dashboard operation
        public List<DashboardContentModel> GetDashboardContentlist(long dashboardId)
        {
            List<DashboardContentModel> DashboardContentList = new List<DashboardContentModel>();

            DashboardContent dc = new DashboardContent()
            {
                DashboardListingId = dashboardId
            };
            var dashboardContent = dc.RetriveDashboardContentV2(this.userData.DatabaseKey);

            if (dashboardContent != null)
            {
                DashboardContentModel objdashboard;
                foreach (var v in dashboardContent)
                {
                    objdashboard = new DashboardContentModel();
                    objdashboard.DashboardContentId = v.DashboardContentId;
                    objdashboard.DashboardListingId = v.DashboardListingId;
                    objdashboard.WidgetListingId = v.WidgetListingId;
                    objdashboard.Display = v.Display;
                    objdashboard.Required = v.Required;
                    objdashboard.GridColWidth = v.GridColWidth;
                    objdashboard.ViewName = v.ViewName;
                    objdashboard.Name = v.Name;
                    DashboardContentList.Add(objdashboard);
                }
            }

            return DashboardContentList;
        }
        public List<SelectListItem> GetAllDashboard()
        {
            DashboardListing dashboard = new DashboardListing();

            List<DashboardListing> dashboardlisting = dashboard.RetrieveAllCustom(userData.DatabaseKey);
            var dashboardlist = dashboardlisting.Select(x => new SelectListItem { Text = x.Name, Value = x.DashboardListingId.ToString() }).ToList();
            return dashboardlist;
        }
        public DashboardUserSettings AddorUpdateDashboardsettings(long DashboardListingId, bool IsDefault = false, string SettingsInfo = "")
        {
            DashboardUserSettings settings = new DashboardUserSettings();
            settings.ClientId = this.userData.DatabaseKey.Client.ClientId;
            settings.UserInfoId = this.userData.DatabaseKey.User.UserInfoId;
            settings.DashboardListingId = DashboardListingId;
            settings.SettingInfo = SettingsInfo;
            settings.RetrieveByDashboardlistingIdandUserinfoId(this.userData.DatabaseKey);
            settings.IsDefault = IsDefault;
            if (settings.DashboardUserSettingsId == 0)
            {
                settings.CreateFromDatabase_V2(this.userData.DatabaseKey);
            }
            else
            {
                settings.SettingInfo = SettingsInfo;
                settings.UpdateFromDatabase_V2(this.userData.DatabaseKey);
            }
            // 
            return settings;

        }
        public DashboardUserSettings RetrieveDashboardUserSettings(long DashboardListingId)
        {
            DashboardUserSettings settings = new DashboardUserSettings();
            settings.ClientId = this.userData.DatabaseKey.Client.ClientId;
            settings.UserInfoId = this.userData.DatabaseKey.User.UserInfoId;
            settings.DashboardListingId = DashboardListingId;
            settings.RetrieveByDashboardlistingIdandUserinfoId(this.userData.DatabaseKey);
            return settings;
        }
        public long RetrieveDefaultDashboardListingId()
        {
            DashboardUserSettings settings = new DashboardUserSettings();
            settings.ClientId = this.userData.DatabaseKey.Client.ClientId;
            settings.UserInfoId = this.userData.DatabaseKey.User.UserInfoId;
            settings.RetrieveDefaultDashboardListingId(this.userData.DatabaseKey);
            return settings.DashboardListingId;
        }
        #endregion

        #region Maintenance Technician

        #region Maintenance Completion Workbench Search grid
        // if there is any change in any column order then please look into mobile
        // card view also as in both the places we are using SP
        public List<WoCompletionWorkbenchSearchModel> populateWOCompletionWorkbench(int CustomQueryDisplayId, int skip = 0,
        int length = 0, string orderbycol = "", string orderDir = "", string txtSearchval = "")
        {
            if (!string.IsNullOrEmpty(txtSearchval))
            {
                txtSearchval = txtSearchval.Trim();
            }
            WorkOrder workorder = new WorkOrder();
            WoCompletionWorkbenchSearchModel WoCompletion;
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<WoCompletionWorkbenchSearchModel> workOrderCompletionModelList = new List<WoCompletionWorkbenchSearchModel>();
            List<DataContracts.LookupList> Type = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> Priority = new List<DataContracts.LookupList>();

            workorder.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            workorder.CustomQueryDisplayId = CustomQueryDisplayId;
            workorder.OrderbyColumn = orderbycol;
            workorder.OrderBy = orderDir;
            workorder.OffSetVal = skip;
            workorder.NextRow = length;
            workorder.SearchText = txtSearchval;

            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();

            Task<WorkOrder> task1 = Task.Factory.StartNew<DataContracts.WorkOrder>(
                           () => workorder.RetrieveV2ForCompletionWorkbench(this.userData.DatabaseKey, userData.Site.TimeZone));

            Task task2 = Task.Factory.StartNew(() => AllLookUps = commonWrapper.GetAllLookUpList());

            Task.WaitAll(task1, task2);

            if (AllLookUps != null)
            {
                Type = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_TYPE).ToList();
                Priority = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_Priority).ToList();
            }

            List<WorkOrder> woList = workorder.listOfWO;
            foreach (var wo in woList)
            {
                WoCompletion = new WoCompletionWorkbenchSearchModel();
                WoCompletion.WorkOrderId = wo.WorkOrderId;
                WoCompletion.ClientLookupId = wo.ClientLookupId;
                WoCompletion.Description = wo.Description;
                WoCompletion.EquipmentClientLookupId = wo.EquipmentClientLookupId;
                WoCompletion.AssetName = wo.AssetName;
                WoCompletion.Status = wo.Status;


                if (wo.SourceType == "Unplanned")
                {
                    Type = AllLookUps.Where(x => x.ListName == LookupListConstants.UP_WO_TYPE).ToList();
                }
                else if (wo.SourceType == "WorkRequest")
                {
                    Type = AllLookUps.Where(x => x.ListName == LookupListConstants.WR_WO_TYPE).ToList();
                }
                else if (wo.SourceType == "OnDemand")
                {
                    Type = AllLookUps.Where(x => x.ListName == LookupListConstants.UP_WO_TYPE).ToList();
                }

                if (Type != null && Type.Any(cus => cus.ListValue == wo.Type))
                {
                    WoCompletion.Type = Type.Where(x => x.ListValue == wo.Type).Select(x => x.Description).First();
                }
                if (wo.CreateDate != null && wo.CreateDate == default(DateTime))
                {
                    WoCompletion.CreateDate = null;
                }
                else
                {
                    WoCompletion.CreateDate = wo.CreateDate;
                }
                if (Priority != null && Priority.Any(cus => cus.ListValue == wo.Priority))
                {
                    WoCompletion.Priority = Priority.Where(x => x.ListValue == wo.Priority).Select(x => x.Description).First();
                }

                if (wo.ScheduledStartDate == null || wo.ScheduledStartDate == default(DateTime))
                {
                    WoCompletion.ScheduledStartDate = null;
                }
                else
                {
                    WoCompletion.ScheduledStartDate = wo.ScheduledStartDate;
                }

                if (wo.CompleteDate != null && wo.CompleteDate == default(DateTime))
                {
                    WoCompletion.CompleteDate = null;
                }
                else
                {
                    WoCompletion.CompleteDate = wo.CompleteDate;
                }
                if (wo.RequiredDate != null && wo.RequiredDate == default(DateTime))
                {
                    WoCompletion.RequiredDate = null;
                }
                else
                {
                    WoCompletion.RequiredDate = wo.RequiredDate;
                }
                WoCompletion.TotalCount = wo.TotalCount;

                workOrderCompletionModelList.Add(WoCompletion);
            }

            return workOrderCompletionModelList;
        }
        public List<WorkOrderModel> populateWOCompletionWorkbenchForGridAndCardView(int CustomQueryDisplayId, int skip = 0,
            int length = 0, string orderbycol = "", string orderDir = "", string txtSearchval = "")
        {
            if (!string.IsNullOrEmpty(txtSearchval))
            {
                txtSearchval = txtSearchval.Trim();
            }
            WorkOrder workorder = new WorkOrder();
            WorkOrderModel workOrderModel;
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<WorkOrderModel> workOrderCompletionModelList = new List<WorkOrderModel>();
            List<DataContracts.LookupList> Type = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> Priority = new List<DataContracts.LookupList>();

            workorder.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            workorder.CustomQueryDisplayId = CustomQueryDisplayId;
            workorder.OrderbyColumn = orderbycol;
            workorder.OrderBy = orderDir;
            workorder.OffSetVal = skip;
            workorder.NextRow = length;
            workorder.SearchText = txtSearchval;

            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();

            Task<WorkOrder> task1 = Task.Factory.StartNew<DataContracts.WorkOrder>(
                           () => workorder.RetrieveV2ForCompletionWorkbench(this.userData.DatabaseKey, userData.Site.TimeZone));

            Task task2 = Task.Factory.StartNew(() => AllLookUps = commonWrapper.GetAllLookUpList());

            Task.WaitAll(task1, task2);

            if (AllLookUps != null)
            {
                Type = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_TYPE).ToList();
                Priority = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_Priority).ToList();
            }

            List<WorkOrder> woList = workorder.listOfWO;
            foreach (var wo in woList)
            {
                workOrderModel = new WorkOrderModel();
                workOrderModel.WorkOrderId = wo.WorkOrderId;
                workOrderModel.ClientLookupId = wo.ClientLookupId;
                workOrderModel.Description = wo.Description;
                workOrderModel.ChargeToClientLookupId = wo.EquipmentClientLookupId ?? "";
                workOrderModel.ChargeTo_Name = wo.AssetName ?? "";
                workOrderModel.Status = wo.Status;

                if (wo.SourceType == "Unplanned")
                {
                    Type = AllLookUps.Where(x => x.ListName == LookupListConstants.UP_WO_TYPE).ToList();
                }
                else if (wo.SourceType == "WorkRequest")
                {
                    Type = AllLookUps.Where(x => x.ListName == LookupListConstants.WR_WO_TYPE).ToList();
                }
                else if (wo.SourceType == "OnDemand")
                {
                    Type = AllLookUps.Where(x => x.ListName == LookupListConstants.UP_WO_TYPE).ToList();
                }

                if (Type != null && Type.Any(cus => cus.ListValue == wo.Type))
                {
                    workOrderModel.Type = Type.Where(x => x.ListValue == wo.Type).Select(x => x.Description).First();
                }
                if (wo.CreateDate != null && wo.CreateDate == default(DateTime))
                {
                    workOrderModel.CreateDate = null;
                }
                else
                {
                    workOrderModel.CreateDate = wo.CreateDate;
                }
                if (Priority != null && Priority.Any(cus => cus.ListValue == wo.Priority))
                {
                    workOrderModel.Priority = Priority.Where(x => x.ListValue == wo.Priority).Select(x => x.Description).First();
                }

                if (wo.ScheduledStartDate == null || wo.ScheduledStartDate == default(DateTime))
                {
                    workOrderModel.ScheduledStartDate = null;
                }
                else
                {
                    workOrderModel.ScheduledStartDate = wo.ScheduledStartDate;
                }

                if (wo.CompleteDate != null && wo.CompleteDate == default(DateTime))
                {
                    workOrderModel.CompleteDate = null;
                }
                else
                {
                    workOrderModel.CompleteDate = wo.CompleteDate;
                }
                if (wo.RequiredDate != null && wo.RequiredDate == default(DateTime))
                {
                    workOrderModel.RequiredDate = null;
                }
                else
                {
                    workOrderModel.RequiredDate = wo.RequiredDate;
                }
                workOrderModel.AssignedFullName = wo.AssignedFullName;
                workOrderModel.WorkAssigned_PersonnelId = wo.WorkAssigned_PersonnelId;
                workOrderModel.AssetLocation = wo.AssetLocation;
                workOrderModel.ProjectClientLookupId = wo.ProjectClientLookupId;
                workOrderModel.TotalCount = wo.TotalCount;

                workOrderCompletionModelList.Add(workOrderModel);
            }

            return workOrderCompletionModelList;
        }
        #endregion

        #region Maintenance Completion Workbench Details
        public WoCompletionDetailsHeader getWorkOderDetailsByIdForCompletionHeader(long workOrderId)
        {
            WoCompletionDetailsHeader WoCompletionDetailsHeaderModel = new WoCompletionDetailsHeader();
            WorkOrder workorder = new WorkOrder()
            {
                ClientId = _dbKey.Client.ClientId,
                WorkOrderId = workOrderId
            };
            workorder.RetrieveByPKForeignKeys(this.userData.DatabaseKey, userData.Site.TimeZone);
            WoCompletionDetailsHeaderModel = initializeControlsforCompletionHeader(workorder);
            return WoCompletionDetailsHeaderModel;
        }
        public WoCompletionDetailsHeader initializeControlsforCompletionHeader(WorkOrder obj)
        {
            WoCompletionDetailsHeader objwoCompletionHeader = new WoCompletionDetailsHeader();
            objwoCompletionHeader.ClientLookupId = obj.ClientLookupId;
            objwoCompletionHeader.WorkOrderId = obj.WorkOrderId;
            objwoCompletionHeader.Status = obj?.Status ?? string.Empty;
            objwoCompletionHeader.Type = obj?.Type ?? string.Empty;
            objwoCompletionHeader.Description = obj?.Description ?? string.Empty;
            objwoCompletionHeader.Priority = obj.Priority;
            objwoCompletionHeader.Assigned = obj?.Assigned ?? string.Empty;
            objwoCompletionHeader.ScheduledStartDate = obj?.ScheduledStartDate ?? DateTime.MinValue;
            objwoCompletionHeader.CompleteDate = obj?.CompleteDate ?? DateTime.MinValue;
            objwoCompletionHeader.ChargeTo_Name = obj?.ChargeTo_Name ?? string.Empty;
            objwoCompletionHeader.ChargeToClientLookupId = obj?.ChargeToClientLookupId ?? string.Empty;
            objwoCompletionHeader.WorkAssigned_PersonnelId = obj.WorkAssigned_PersonnelId;
            objwoCompletionHeader.RequiredDate=obj?.RequiredDate ?? DateTime.MinValue;
            objwoCompletionHeader.ChargeToId = obj.ChargeToId;
            objwoCompletionHeader.AssetGroup1ClientlookupId=obj.AssetGroup1ClientlookupId;
            objwoCompletionHeader.AssetGroup2ClientlookupId = obj.AssetGroup2ClientlookupId;

            objwoCompletionHeader.Assetlocation = obj.AssetLocation;
            objwoCompletionHeader.ProjectClientLookupId = obj.ProjectClientLookupId;

            return objwoCompletionHeader;
        }

        public string PopulateHoverList(long workOrderId)
        {
            WorkOrder wo = new WorkOrder();
            WorkOrder PersonData = new WorkOrder();
            wo.ClientId = userData.DatabaseKey.Client.ClientId;
            wo.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            wo.WorkOrderId = workOrderId;
            PersonData = wo.RetrievePersonnelInitial(userData.DatabaseKey);
            string userList = PersonData.PersonnelFull;
            return userList;
        }
        #region  Work Order Completion Workbench Status Update

        public List<string> updateWorkOrderCompletionStatusforWorkbench(long WorkOrderId)
        {
            WorkOrder wo = new WorkOrder()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                WorkOrderId = WorkOrderId,
                SiteId = this.userData.DatabaseKey.Personnel.SiteId
            };
            wo.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);
            wo.CompleteComments =string.Empty;

            // Update with completion information
            wo.Status = WorkOrderStatusConstants.Complete;
            wo.CompleteBy_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;

            if (this.userData.DatabaseKey.Client.BusinessType.ToUpper() == BusinessTypeConstants.FoodServices)
            {
                wo.SignOffDate = DateTime.UtcNow;
                wo.SignoffBy_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
                wo.SignoffBy_PersonnelClientLookupId = userData.DatabaseKey.Personnel.ClientLookupId;
            }
            wo.CompleteDate = DateTime.UtcNow;
            wo.CompleteBy_PersonnelClientLookupId = userData.DatabaseKey.Personnel.ClientLookupId; // Converts to personnel id in the sp
            if (wo.ActualFinishDate == null || wo.ActualFinishDate == DateTime.MinValue)
            {
                wo.ActualFinishDate = DateTime.UtcNow.ConvertFromUTCToUser(userData.Site.TimeZone);
            }
            wo.CompleteWorkOrder(this.userData.DatabaseKey, userData.Site.TimeZone);
            CreateEventLog(wo.WorkOrderId, WorkOrderEvents.Complete, wo.CompleteComments);
            List<long> wos = new List<long>() { wo.WorkOrderId };

            if (wo.ErrorMessages.Count == 0)
            {
                Task task1 = Task.Factory.StartNew(() => this.SendAlert(wos));
                return wo.ErrorMessages;
            }
            return wo.ErrorMessages;
        }
        private void SendAlert(List<long> wos)
        {
            ProcessAlert objAlert = new ProcessAlert(userData);
            objAlert.CreateAlert<WorkOrder>(AlertTypeEnum.WorkOrderComplete, wos);
        }
        #endregion
        #region  Work Order Cancellation Workbench Status Update
        public List<string> updateWorkOrderCancelationStatusforWorkbench(long WorkorderId, string CancelReason, string Comments)
        {
            WorkOrder workOrder = new WorkOrder()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                WorkOrderId = WorkorderId,
                SiteId = userData.DatabaseKey.Personnel.SiteId
            };
            workOrder.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);
            workOrder.Status = WorkOrderStatusConstants.Canceled;
            workOrder.CompleteComments = Comments.Trim();
            workOrder.CancelReason = CancelReason.Trim();
            workOrder.CompleteDate = DateTime.UtcNow;
            if (workOrder.ActualFinishDate == null || workOrder.ActualFinishDate == DateTime.MinValue)
                workOrder.ActualFinishDate = DateTime.UtcNow;
            workOrder.CompleteBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            workOrder.CreateMode = false;
            workOrder.CompleteBy_PersonnelClientLookupId = userData.DatabaseKey.Personnel.ClientLookupId;
            workOrder.CompleteWorkOrder(this.userData.DatabaseKey, userData.Site.TimeZone);

            if (workOrder.ErrorMessages.Count == 0)
            {
                CreateEventLog(workOrder.WorkOrderId, WorkOrderStatusConstants.Canceled, workOrder.CompleteComments);
                // SOM - 797
                Int64 CallerUserInfoId = this.userData.DatabaseKey.User.UserInfoId;
                List<object> listWO = new List<object>();
                listWO.Add(workOrder.WorkOrderId);
                ProcessAlert objAlert = new ProcessAlert(this.userData);
                objAlert.CreateAlert<WorkOrder>(this.userData, workOrder, AlertTypeEnum.WorkOrderCancel, CallerUserInfoId, listWO);
            }
            return workOrder.ErrorMessages;
        }

        #endregion
        #region Create EventLog for WorkOrder
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
        #endregion

        #region Maintenance Technician Schedule Compliance
        #endregion

        #region Maintenance Technician Workbench Part grid

        public List<PartHistoryModel> GetPartListGriddata(long workOrderId, string orderbycol = "", string orderDir = "", int skip = 0, int length = 0)
        {
            PartHistoryModel partHistoryModel;
            List<PartHistoryModel> plist = new List<PartHistoryModel>();
            PartHistory parthistory = new PartHistory()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ChargeType_Primary = "WorkOrder",
                ChargeToId_Primary = workOrderId,
                OrderbyColumn = orderbycol,
                OrderBy = orderDir,
                OffSetVal = skip,
                NextRow = length
            };
            List<PartHistory> phList = parthistory.RetriveForMaintenanceTechnician_V2(this.userData.DatabaseKey);

            foreach (var item in phList)
            {
                partHistoryModel = new PartHistoryModel();
                partHistoryModel.PartClientLookupId = item.PartClientLookupId;
                partHistoryModel.Description = item.Description;
                partHistoryModel.TransactionQuantity = item.TransactionQuantity;
                partHistoryModel.Cost = item.Cost;
                partHistoryModel.TotalCost = item.TotalCost;
                partHistoryModel.UnitofMeasure = item.UnitofMeasure;
                partHistoryModel.UPCCode = item.UPCCode;
                partHistoryModel.TotalCount = item.TotalCount;
                //V2-624
                partHistoryModel.PartId = item.PartId;
                partHistoryModel.PartHistoryId = item.PartHistoryId;
                if (item.TransactionDate != null && item.TransactionDate == default(DateTime))
                {
                    partHistoryModel.TransactionDate = null;
                }
                else
                {
                    partHistoryModel.TransactionDate = item.TransactionDate.ToUserTimeZone(this.userData.Site.TimeZone);
                }
                partHistoryModel.StoreroomId = item.StoreroomId;
                plist.Add(partHistoryModel);
            }
            return plist;
        }
        #endregion

        #endregion


    }
}