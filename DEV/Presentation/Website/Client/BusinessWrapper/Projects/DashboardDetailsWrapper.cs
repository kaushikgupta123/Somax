using Client.Common;
using Client.Localization;
using Client.Models;
using Client.Models.Dashboard;
using Client.Models.Work_Order;
using Client.Models.Work_Order.UIConfiguration;

using Common.Constants;
using Common.Enumerations;

using Database.Transactions;

using DataContracts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace Client.BusinessWrapper.Projects
{
    public class DashboardDetailsWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        public string newClientlookupId { get; set; }
        public DashboardDetailsWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        public List<KeyValuePair<string, long>> ProjectTaskDashboardStatuses(long ProjectId)
        {
            ProjectTask projectTask = new ProjectTask()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                ProjectId = ProjectId
            };

            List<KeyValuePair<string, long>> keyValuePairs = projectTask.ProjectTaskDashboardStatuses(this.userData.DatabaseKey);

            return keyValuePairs;
        }

        public List<KeyValuePair<string, long>> ProjectTaskDashboardScheduleComplianceStatuses(long ProjectId)
        {
            ProjectTask projectTask = new ProjectTask()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                ProjectId = ProjectId
            };

            List<KeyValuePair<string, long>> keyValuePairs = projectTask.ProjectTaskDashboardScheduleComplianceStatuses(this.userData.DatabaseKey);

            return keyValuePairs;
        }

        #region  Task
        public List<DashboardWoTaskModel> PopulateTaskV2(long workOrderId, string orderbycol = "", string orderDir = "", int skip = 0, int length = 0)
        {
            DashboardWoTaskModel DashboardWoTaskModel;
            List<DashboardWoTaskModel> DashboardWoTaskModelList = new List<DashboardWoTaskModel>();
            WorkOrderTask woTask = new WorkOrderTask()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ParentSiteId = this.userData.DatabaseKey.Personnel.SiteId,
                WorkOrderId = workOrderId,
                OrderbyColumn = orderbycol,
                OrderBy = orderDir,
                OffSetVal = skip,
                NextRow = length
            };
            List<WorkOrderTask> DashboardWoTaskList = new List<WorkOrderTask>();
            DashboardWoTaskList = WorkOrderTask.RetriveByWorkOrderIdV2(this.userData.DatabaseKey, woTask);

            if (DashboardWoTaskList != null)
            {
                foreach (var item in DashboardWoTaskList)
                {
                    DashboardWoTaskModel = new DashboardWoTaskModel();
                    DashboardWoTaskModel.WorkOrderTaskId = item.WorkOrderTaskId;
                    DashboardWoTaskModel.TaskNumber = item.TaskNumber;
                    DashboardWoTaskModel.Description = item.Description;
                    DashboardWoTaskModel.Status = item.Status;
                    DashboardWoTaskModel.ChargeToClientLookupId = item.ChargeToClientLookupId;
                    DashboardWoTaskModel.TotalCount = item.TotalCount;
                    DashboardWoTaskModelList.Add(DashboardWoTaskModel);
                }
            }

            return DashboardWoTaskModelList;
        }
        public List<DashboardWoTaskModel> PopulateTask(long workOrderId)
        {
            DashboardWoTaskModel DashboardWoTaskModel;
            List<DashboardWoTaskModel> DashboardWoTaskModelList = new List<DashboardWoTaskModel>();
            WorkOrderTask woTask = new WorkOrderTask()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ParentSiteId = this.userData.DatabaseKey.Personnel.SiteId,
                WorkOrderId = workOrderId
            };
            List<WorkOrderTask> DashboardWoTaskList = new List<WorkOrderTask>();
            DashboardWoTaskList = WorkOrderTask.RetriveByWorkOrderId(this.userData.DatabaseKey, woTask);

            if (DashboardWoTaskList != null)
            {
                foreach (var item in DashboardWoTaskList)
                {
                    DashboardWoTaskModel = new DashboardWoTaskModel();
                    DashboardWoTaskModel.WorkOrderTaskId = item.WorkOrderTaskId;
                    DashboardWoTaskModelList.Add(DashboardWoTaskModel);
                }
            }

            return DashboardWoTaskModelList;
        }

        #region Cancel Task
        public string CancelTask(string taskList, string cancelReason, ref int successCount)
        {
            string[] cancelArray = null;
            if (taskList != null)
            {
                cancelArray = taskList.Split(',');
            }
            string result = string.Empty;

            foreach (var item in cancelArray)
            {
                WorkOrderTask workordertask = new WorkOrderTask()
                {
                    ClientId = this.userData.DatabaseKey.Client.ClientId,
                    ParentSiteId = this.userData.DatabaseKey.Personnel.SiteId,
                    WorkOrderTaskId = Convert.ToInt64(item)
                };
                workordertask.RetriveByPKForeignKeys(userData.DatabaseKey);
                if (workordertask.Status != WorkOrderStatusConstants.Canceled)
                {
                    // workordertask.Retrieve(userData.DatabaseKey);
                    workordertask.CancelReason = cancelReason;
                    workordertask.CompleteBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
                    workordertask.CompleteDate = System.DateTime.UtcNow;
                    workordertask.Status = WorkOrderStatusConstants.Canceled;
                    workordertask.UpdateByPKForeignKeys(this.userData.DatabaseKey);
                    successCount++;
                }

            }
            if (successCount > 0)
            {
                return "success";
            }
            else
            {
                return "failed";
            }
        }
        #endregion

        #region Complete Task
        public string CompleteTask(string taskList, ref int successCount)
        {
            string[] array = taskList.Split(',');
            string result = string.Empty;
            foreach (var item in array)
            {
                WorkOrderTask workordertask = new WorkOrderTask()
                {
                    ClientId = this.userData.DatabaseKey.Client.ClientId,
                    ParentSiteId = this.userData.DatabaseKey.User.DefaultSiteId,
                    WorkOrderTaskId = Convert.ToInt64(item)
                };
                workordertask.RetriveByPKForeignKeys(userData.DatabaseKey);
                if (workordertask.Status != WorkOrderStatusConstants.Complete)
                {
                    workordertask.CompleteBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
                    workordertask.CompleteBy_PersonnelClientLookupId = userData.DatabaseKey.Personnel.ClientLookupId;
                    workordertask.CompleteDate = System.DateTime.UtcNow;
                    workordertask.Status = WorkOrderStatusConstants.Complete;
                    workordertask.UpdateByPKForeignKeys(this.userData.DatabaseKey);
                    successCount++;
                }
            }
            if (successCount > 0)
            {
                return "success";
            }
            else
            {
                return "failed";
            }
        }

        #endregion
        #endregion

        #region "Labor"
        public List<LaborModel> RetrieveLaborByWorkOrderId(long workOrderId, string orderbycol = "", string orderDir = "", int skip = 0, int length = 0)
        {
            List<LaborModel> laborList = new List<LaborModel>();
            LaborModel objLabor;

            Timecard timecard = new Timecard()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ChargeType_Primary = "WorkOrder",
                ChargeToId_Primary = workOrderId,
                OrderbyColumn = orderbycol,
                OrderBy = orderDir,
                OffSetVal = skip,
                NextRow = length

            };

            var laborData = Timecard.RetriveByWorkOrderIdForMaintananceWorkbenchDetails(this.userData.DatabaseKey, timecard);

            if (laborData != null)
            {
                foreach (var item in laborData)
                {
                    objLabor = new LaborModel();
                    objLabor.PersonnelID = item.PersonnelId;
                    objLabor.Name = item.Name;
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
                    objLabor.TotalCount = item.TotalCount;
                    laborList.Add(objLabor);
                }
            }
            return laborList;
        }
        public LaborModel RetrieveByTimecardid(long TimecardId)
        {
            LaborModel objLabor = new LaborModel();
            Timecard timecard = new Timecard()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                //ChargeType_Primary = "WorkOrder",
                //ChargeToId_Primary = WorkOrderId,
                //ChargeType_Secondary = "SOLineItem",
                //ChargeToId_Secondary = ServiceOrderLineItemId,
                TimecardId = TimecardId
            };

            timecard.Retrieve(this.userData.DatabaseKey);
            objLabor = PopulateLaborModel(timecard);
            return objLabor;
        }
        internal LaborModel PopulateLaborModel(Timecard aobj)
        {
            LaborModel oModel = new LaborModel();
            oModel.TimecardId = aobj.TimecardId;
            oModel.PersonnelID = aobj.PersonnelId;
            if (aobj.StartDate != null && aobj.StartDate != default(DateTime))
            {
                oModel.StartDate = aobj.StartDate;
            }
            else
            {
                oModel.StartDate = null;
            }
            oModel.Hours = aobj.Hours;
            return oModel;
        }
        public Timecard AddLabor(LaborModel objLaborModel)
        {
            Timecard timecard = new Timecard()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                ChargeType_Primary = "WorkOrder",
                ChargeToId_Primary = objLaborModel.WorkOrderId
            };
            timecard.PersonnelId = objLaborModel.PersonnelID ?? 0;
            timecard.Hours = objLaborModel.Hours;
            timecard.StartDate = objLaborModel.StartDate;

            timecard.CreateByPKForeignKeys(this.userData.DatabaseKey);

            return timecard;
        }
        public Timecard UpdateLabor(LaborModel objLaborModel)
        {
            Timecard timecard = new Timecard()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                ChargeType_Primary = "WorkOrder",
                ChargeToId_Primary = objLaborModel.WorkOrderId,
                TimecardId = objLaborModel.TimecardId
            };

            timecard.Retrieve(this.userData.DatabaseKey);
            timecard.Hours = objLaborModel.Hours;
            timecard.StartDate = objLaborModel.StartDate;

            timecard.Update(this.userData.DatabaseKey);
            return timecard;
        }
        public bool DeleteLabor(long TimecardId)
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
        #endregion

        #region parts
        internal List<PartHistory> PartIssueAddData(PartIssueAddModel Obj, string ClientLookupId, string Description, string UPCCode)
        {
            List<PartHistory> tmpList = new List<PartHistory>();
            List<PartHistory> PartHistoryListTemp = new List<PartHistory>();
            if (Obj != null)
            {
                tmpList.Add(new PartHistory
                {
                    IssueToClientLookupId = userData.DatabaseKey.Personnel.ClientLookupId,
                    IssuedTo = Convert.ToString(userData.DatabaseKey.Personnel.PersonnelId),
                    PartStoreroomId = 0,
                    TransactionDate = System.DateTime.UtcNow,
                    ChargeType_Primary = "WorkOrder",
                    ChargeToClientLookupId = Obj.WorkOrderClientLookupId,
                    ChargeToId_Primary = Convert.ToInt64(Obj.WorkOrderId),
                    TransactionQuantity = Obj.Quantity,
                    PartClientLookupId = ClientLookupId,
                    PartId = Obj.PartId,
                    Description = Description ?? string.Empty,
                    SiteId = userData.DatabaseKey.Personnel.SiteId,
                    TransactionType = PartHistoryTranTypes.PartIssue,
                    IsPartIssue = true,
                    ErrorMessagerow = null,
                    PartUPCCode = UPCCode ?? string.Empty,
                    PerformedById = 0,
                    RequestorId = 0,
                    IsPerformAdjustment = true,
                    MultiStoreroom = userData.DatabaseKey.Client.UseMultiStoreroom, //V2-687
                    StoreroomId = Obj.StoreroomId ?? 0  //V2-687
                });
            }

            PartHistory parthistory = new PartHistory() { PartHistoryList = tmpList };
            PartHistoryListTemp = parthistory.CreateByForeignKeysnew(userData.DatabaseKey);
            return PartHistoryListTemp;
        }
        #endregion

        #region On demand Work Order
        public List<DataContracts.MaintOnDemandMaster> GetOndemandList()
        {
            DataContracts.MaintOnDemandMaster maintOnDemandMaster = new DataContracts.MaintOnDemandMaster();
            maintOnDemandMaster.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            List<DataContracts.MaintOnDemandMaster> maintOnDemandMasterList = maintOnDemandMaster.RetrieveAllBySiteId(this.userData.DatabaseKey, this.userData.Site.TimeZone).Where(a => a.InactiveFlag == false).ToList();
            return maintOnDemandMasterList;
        }
        public WorkOrder AddOndemandWorkOrder(OnDamandWOModel WoEmergencyModel, ref List<string> ErrorMsgList)
        {
            WorkOrder workOrder = new WorkOrder
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId
            };
            if (WorkOrderStatusConstants.Wo_AutoGenerateEnabled)
            {
                newClientlookupId = CustomSequentialId.GetNextId(userData.DatabaseKey, AutoGenerateKey.WO_Annual, userData.DatabaseKey.User.DefaultSiteId, "");
            }

            if (WoEmergencyModel.IsTypeShow)
            {
                workOrder.Type = WoEmergencyModel.Type;
            }
            workOrder.ChargeType = ChargeType.Equipment;

            if (WoEmergencyModel.ChargeToClientLookupId != null)
            {
                var index = WoEmergencyModel.ChargeToClientLookupId.IndexOf('|');
                if (index != -1)
                {
                    workOrder.ChargeToClientLookupId = WoEmergencyModel.ChargeToClientLookupId.Substring(0, index).Trim();
                    long ChrgTo = 0;
                    long.TryParse(workOrder.ChargeToClientLookupId, out ChrgTo);
                    workOrder.ChargeToId = ChrgTo;
                }
                else
                {
                    workOrder.ChargeToClientLookupId = WoEmergencyModel.ChargeToClientLookupId;
                    long ChrgTo = 0;
                    long.TryParse(workOrder.ChargeToClientLookupId, out ChrgTo);
                    workOrder.ChargeToId = ChrgTo;
                }
            }


            workOrder.MaintOnDemandClientLookUpId = WoEmergencyModel.OnDemandID;
            workOrder.ClientLookupId = newClientlookupId;
            workOrder.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            workOrder.Creator_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
            workOrder.Creator_PersonnelClientLookupId = userData.DatabaseKey.Personnel.ClientLookupId;
            workOrder.ApproveBy_PersonnelClientLookupId = userData.DatabaseKey.Personnel.ClientLookupId;
            workOrder.ApproveBy_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
            workOrder.ApproveDate = DateTime.UtcNow;
            workOrder.Status = WorkOrderStatusConstants.Scheduled;
            workOrder.SourceType = WorkOrderSourceTypes.OnDemand;
            workOrder.ScheduledStartDate = DateTime.UtcNow;
            workOrder.Priority = "";
            workOrder.RequiredDate = null;
            workOrder.CreateFromOnDemandMaster_V2(this.userData.DatabaseKey, userData.Site.TimeZone);

            if (workOrder.ErrorMessages.Count == 0)
            {
                //-- V2-606
                CreateEventLog(workOrder.WorkOrderId, WorkOrderEvents.Create);//-----------SOM-1632-------------//
                CreateEventLog(workOrder.WorkOrderId, WorkOrderEvents.Approved);
                CreateEventLog(workOrder.WorkOrderId, WorkOrderEvents.Scheduled);//-----------SOM-1632-------------//                 
                //-- V2-606

                WorkOrderSchedule workorderschedule = new WorkOrderSchedule()
                {
                    ClientId = this.userData.DatabaseKey.Client.ClientId,
                    WorkOrderId = workOrder.WorkOrderId
                };
                workorderschedule.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
                workorderschedule.ScheduledStartDate = DateTime.UtcNow;
                workorderschedule.ScheduledHours = 1;
                // Create record - sp updates the work order 
                workorderschedule.CreateForWorkOrder(this.userData.DatabaseKey);
            }
            else
            {
                ErrorMsgList = workOrder.ErrorMessages;
            }
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
        #endregion
        #region Describe Work Order
        public WorkOrder AddDescribeWorkOrder(DescribeWOModel WoEmergencyModel, ref List<string> ErrorMsgList)
        {
            WorkOrder workOrder = new WorkOrder
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId
            };

            if (WorkOrderStatusConstants.Wo_AutoGenerateEnabled)
            {
                newClientlookupId = CustomSequentialId.GetNextId(userData.DatabaseKey, AutoGenerateKey.WO_Annual, userData.DatabaseKey.User.DefaultSiteId, "");
            }

            if (WoEmergencyModel.IsTypeShow)
            {
                workOrder.Type = WoEmergencyModel.Type;
            }
            // Commented for V2-608
            workOrder.ChargeType = ChargeType.Equipment;

            if (WoEmergencyModel.ChargeToClientLookupId != null)
            {
                var index = WoEmergencyModel.ChargeToClientLookupId.IndexOf('|');
                if (index != -1)
                {
                    workOrder.ChargeToClientLookupId = WoEmergencyModel.ChargeToClientLookupId.Substring(0, index).Trim();
                    long ChrgTo = 0;
                    long.TryParse(workOrder.ChargeToClientLookupId, out ChrgTo);
                    workOrder.ChargeToId = ChrgTo;
                }
                else
                {
                    workOrder.ChargeToClientLookupId = WoEmergencyModel.ChargeToClientLookupId;
                    long ChrgTo = 0;
                    long.TryParse(workOrder.ChargeToClientLookupId, out ChrgTo);
                    workOrder.ChargeToId = ChrgTo;
                }
            }
            #region V2-948
            if (userData.Site.SourceAssetAccount == true && workOrder.Labor_AccountId == 0)
            {
                Equipment equipment = new Equipment
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    SiteId = userData.Site.SiteId,
                    ClientLookupId = workOrder.ChargeToClientLookupId
                };
                equipment.RetrieveByClientLookupId(this.userData.DatabaseKey);
                workOrder.Labor_AccountId = equipment.Labor_AccountId;
            }
            #endregion
            if (WoEmergencyModel.IsDescriptionShow)
            {
                workOrder.Description = WoEmergencyModel.Description.Trim();
            }
            workOrder.ClientLookupId = newClientlookupId;
            workOrder.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            workOrder.ClientId = userData.DatabaseKey.Client.ClientId;
            workOrder.CompleteBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            workOrder.SourceType = WorkOrderSourceTypes.Emergency;
            workOrder.SourceId = WoEmergencyModel.WorkOrderId;

            workOrder.Status = WorkOrderStatusConstants.Scheduled;
            workOrder.ScheduledStartDate = DateTime.UtcNow;
            //---Add Scheduler_PersonnelId 
            workOrder.Scheduler_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
            workOrder.WorkAssigned_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            workOrder.ApproveDate = DateTime.UtcNow;
            workOrder.EmergencyWorkOrder = true;//----------added on 23-12-2014------------
            workOrder.ApproveBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            // SOM-628 
            workOrder.Creator_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            workOrder.CreateByPKForeignKeys(this.userData.DatabaseKey, userData.Site.TimeZone);
            if (workOrder.ErrorMessages.Count == 0)
            {
                //CreateEventLog(workOrder.WorkOrderId, WorkOrderEvents.Create);//-----------SOM-1632-------------// //--added on 7/7/20
                //CreateEventLog(workOrder.WorkOrderId, WorkOrderEvents.Scheduled);//-----------SOM-1632-------------// //--added on 7/7/20

                //-- V2-606                
                CreateEventLog(workOrder.WorkOrderId, WorkOrderEvents.Create);
                CreateEventLog(workOrder.WorkOrderId, WorkOrderEvents.Approved);
                CreateEventLog(workOrder.WorkOrderId, WorkOrderEvents.Scheduled);
                //-- V2-606

                //------------------Adding Assignment Record for Somax 487---------added on 23-12-2014------------
                WorkOrderSchedule workorderschedule = new WorkOrderSchedule()
                {
                    ClientId = this.userData.DatabaseKey.Client.ClientId,
                    WorkOrderId = workOrder.WorkOrderId
                };
                workorderschedule.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
                workorderschedule.ScheduledStartDate = DateTime.UtcNow;
                workorderschedule.ScheduledHours = 1;
                workorderschedule.CreateForWorkOrder(this.userData.DatabaseKey);
            }
            else
            {
                ErrorMsgList = workOrder.ErrorMessages;
            }
            return workOrder;
        }
        #endregion
        #region Follow Up Work Order
        public DashboardWorkOrderModel GetDashboardWorkOderDetailsById(long workOrderId)
        {
            DashboardWorkOrderModel workOrderModel = new DashboardWorkOrderModel();
            WorkOrder workorder = new WorkOrder()
            {
                ClientId = _dbKey.Client.ClientId,
                WorkOrderId = workOrderId
            };
            workorder.RetrieveByPKForeignKeys(this.userData.DatabaseKey, userData.Site.TimeZone);
            workOrderModel = initializeControls(workorder);
            return workOrderModel;
        }
        public DashboardWorkOrderModel initializeControls(WorkOrder obj)
        {
            DashboardWorkOrderModel objworkorder = new DashboardWorkOrderModel();
            objworkorder.ClientLookupId = obj.ClientLookupId;
            objworkorder.WorkOrderId = obj.WorkOrderId;
            objworkorder.Status = obj?.Status ?? string.Empty;
            objworkorder.Shift = obj?.Shift ?? string.Empty;
            objworkorder.Type = obj?.Type ?? string.Empty;
            objworkorder.DownRequired = obj.DownRequired;
            objworkorder.Description = obj?.Description ?? string.Empty;
            objworkorder.Priority = obj.Priority;
            objworkorder.Labor_AccountId = obj.Labor_AccountId;
            objworkorder.CreateBy_PersonnelName = obj?.CreateBy_PersonnelName ?? string.Empty;
            objworkorder.CreateDate = obj?.CreateDate ?? DateTime.MinValue;
            objworkorder.ChargeType = obj?.ChargeType ?? string.Empty;
            objworkorder.ChargeToId = obj.ChargeToId;
            objworkorder.ScheduledDuration = obj.ScheduledDuration;
            objworkorder.ActualDuration = obj.ActualDuration;
            objworkorder.FailureCode = obj?.FailureCode ?? string.Empty;
            objworkorder.RequiredDate = obj?.RequiredDate ?? DateTime.MinValue;

            objworkorder.Creator = obj?.Creator ?? string.Empty;

            objworkorder.Assigned = obj?.Assigned ?? string.Empty;//---V2-293--//
            if (obj.AssignedFullName != null && obj.AssignedFullName.Length > 0)
            {
                objworkorder.AssignedFullName = obj.AssignedFullName.Trim().TrimEnd(',');
            }
            else
            {
                objworkorder.AssignedFullName = "";
            }
            //objworkorder.AssignedFullName = obj?.AssignedFullName.TrimEnd(',') ?? string.Empty;//--V2-293--//
            objworkorder.ScheduledStartDate = obj?.ScheduledStartDate ?? DateTime.MinValue;

            objworkorder.ActualFinishDate = obj?.ActualFinishDate ?? DateTime.MinValue;
            objworkorder.CompleteBy = obj?.CompleteBy_PersonnelName ?? string.Empty;
            objworkorder.CompleteBy_PersonnelId = obj.CompleteBy_PersonnelId;
            objworkorder.CompleteComments = obj?.CompleteComments ?? string.Empty;
            objworkorder.CompleteDate = obj?.CompleteDate ?? DateTime.MinValue;
            objworkorder.ChargeTo_Name = obj?.ChargeTo_Name ?? string.Empty;
            objworkorder.AssetLocation = obj?.AssetLocation ?? string.Empty;
            objworkorder.ChargeToClientLookupId = obj?.ChargeToClientLookupId ?? string.Empty;

            objworkorder.Createby = obj?.Createby ?? string.Empty;
            objworkorder.ModifyBy = obj?.ModifyBy ?? string.Empty;
            objworkorder.ModifyDate = obj?.ModifyDate ?? DateTime.MinValue;
            objworkorder.SourceType = obj?.SourceType ?? string.Empty;
            objworkorder.RequestorName = obj?.RequestorName ?? string.Empty;
            objworkorder.RequestorPhone = obj?.RequestorPhoneNumber ?? string.Empty;
            objworkorder.RequestorEmail = obj?.RequestorEmail ?? string.Empty;
            // objworkorder.WorkImage = obj.WorkImage;
            objworkorder.SiteInformation = userData.DatabaseKey.Client.WOPrintMessage.ToString();
            objworkorder.WorkAssigned_PersonnelClientLookupId = obj?.WorkAssigned_PersonnelClientLookupId ?? string.Empty;
            objworkorder.CompleteBy_PersonnelClientLookupId = obj?.CompleteBy_PersonnelClientLookupId ?? string.Empty;
            objworkorder.SignoffBy_PersonnelClientLookupId = obj?.SignoffBy_PersonnelClientLookupId ?? string.Empty;

            objworkorder.Personnels = obj?.Personnels ?? string.Empty;//---V2-293---//
            //objworkorder.AssignedFullName = obj?.AssignedFullName ?? string.Empty;//---V2-293---//

            objworkorder.WorkAssigned_PersonnelId = obj.WorkAssigned_PersonnelId;
            objworkorder.PartsOnOrder = obj.PartsOnOrder;
            #region FoodSafety
            if (userData.DatabaseKey.Client.BusinessType.ToUpper() == BusinessTypeConstants.FoodServices
            && obj.Status == WorkOrderStatusConstants.Complete
            && obj.SignoffBy_PersonnelId > 0)
            {
                Personnel personnel = new Personnel();
                personnel.ClientId = userData.DatabaseKey.Client.ClientId;
                personnel.SiteId = userData.DatabaseKey.User.DefaultSiteId;
                personnel.PersonnelId = obj.SignoffBy_PersonnelId;
                personnel.Retrieve(userData.DatabaseKey);
                string name = personnel.NameFirst + " " + personnel.NameLast;
                objworkorder.SignoffBy_PersonnelClientLookupIdSecond = name;

                objworkorder.IsFoodSafetyShow = true;
            }
            //V2-463
            if (obj.EquipDownDate != null && obj.EquipDownDate != default(DateTime))
            {
                objworkorder.EquipDownDate = obj.EquipDownDate;
            }
            else
            {
                objworkorder.EquipDownDate = null;
            }

            objworkorder.EquipDownHours = obj?.EquipDownHours ?? 0;
            #endregion

            objworkorder.AssetLocation = obj.AssetLocation;
            objworkorder.ProjectClientLookupId = obj.ProjectClientLookupId;
            return objworkorder;
        }


        public WorkOrder FollowUpWorkOrder(DashboardWoRequestModel WoRequestModel)
        {
            WorkOrder workOrder = new WorkOrder
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId
            };
            if (WorkOrderStatusConstants.Wo_AutoGenerateEnabled)
            {
                newClientlookupId = CustomSequentialId.GetNextId(userData.DatabaseKey, AutoGenerateKey.WO_Annual, userData.DatabaseKey.User.DefaultSiteId, "");
            }
            if (WoRequestModel.IsTypeShow)
            {
                workOrder.Type = WoRequestModel.Type;
            }
            // Commented for V2-608
            workOrder.ChargeType = ChargeType.Equipment;
            if (WoRequestModel.ChargeToClientLookupId != null)
            {
                var index = WoRequestModel.ChargeToClientLookupId.IndexOf('|');
                if (index != -1)
                {

                    workOrder.ChargeToClientLookupId = WoRequestModel.ChargeToClientLookupId.Substring(0, index).Trim();

                    long ChrgTo = 0;
                    long.TryParse(workOrder.ChargeToClientLookupId, out ChrgTo);
                    workOrder.ChargeToId = ChrgTo;

                }
                else
                {
                    workOrder.ChargeToClientLookupId = WoRequestModel.ChargeToClientLookupId;
                    long ChrgTo = 0;
                    long.TryParse(workOrder.ChargeToClientLookupId, out ChrgTo);
                    workOrder.ChargeToId = ChrgTo;
                }
            }
            #region V2-948
            if (userData.Site.SourceAssetAccount == true && workOrder.Labor_AccountId == 0)
            {
                Equipment equipment = new Equipment
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    SiteId = userData.Site.SiteId,
                    ClientLookupId = WoRequestModel.ChargeToClientLookupId
                };
                equipment.RetrieveByClientLookupId(this.userData.DatabaseKey);
                workOrder.Labor_AccountId = equipment.Labor_AccountId;
            }
            #endregion
            if (WoRequestModel.IsDescriptionShow)
            {
                workOrder.Description = WoRequestModel.Description.Trim();
            }
            workOrder.ClientLookupId = newClientlookupId;
            workOrder.SiteId = userData.DatabaseKey.Personnel.SiteId;

            workOrder.ClientId = userData.DatabaseKey.Client.ClientId;
            workOrder.Creator_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            workOrder.SourceType = WorkOrderSourceTypes.FollowUp;
            workOrder.SourceId = WoRequestModel.WorkOrderId;
            workOrder.Status = WorkOrderStatusConstants.Approved;

            workOrder.WorkAssigned_PersonnelId = 0;// UserData.DatabaseKey.Personnel.PersonnelId;
            workOrder.ApproveDate = DateTime.UtcNow;
            workOrder.ApproveBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            workOrder.CreateByPKForeignKeys(this.userData.DatabaseKey, userData.Site.TimeZone);
            if (workOrder.ErrorMessages.Count == 0)
            {
                #region V2-1077
                ProcessAlert objAlert = new ProcessAlert(this.userData);
                List<long> listWO = new List<long>();
                listWO.Add(workOrder.WorkOrderId);
                objAlert.CreateAlert<WorkOrder>(AlertTypeEnum.WorkOrderPlanner, listWO);
                #endregion
                CreateEventLog(workOrder.WorkOrderId, WorkOrderEvents.Create, workOrder.SourceId, "WorkOrder");//-----------SOM-1632-------------//
                CreateEventLog(workOrder.WorkOrderId, WorkOrderEvents.Approved, 0, "");//-----------SOM-1632-------------//
            }
            return workOrder;
        }
        private void CreateEventLog(Int64 WOId, string Status, Int64 sourceId, string sourceTable)
        {
            WorkOrderEventLog log = new WorkOrderEventLog();
            log.ClientId = userData.DatabaseKey.Client.ClientId;
            log.SiteId = userData.DatabaseKey.Personnel.SiteId;
            log.WorkOrderId = WOId;
            log.Event = Status;
            log.TransactionDate = DateTime.UtcNow;
            log.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            log.Comments = "";
            log.SourceId = sourceId;
            log.SourceTable = sourceTable;
            log.Create(userData.DatabaseKey);
        }
        #endregion
        #region Save Work Request
        public WorkOrder AddWorkRequestDynamic(DashboardVM objDashboardVM, ref List<string> errorMsg)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            WorkOrder obj = new WorkOrder();
            WorkOrder workorder = new WorkOrder
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId
            };
            if (WorkOrderStatusConstants.Wo_AutoGenerateEnabled)
            {
                newClientlookupId = CustomSequentialId.GetNextId(userData.DatabaseKey, AutoGenerateKey.WO_Annual, userData.DatabaseKey.User.DefaultSiteId, "");
            }

            workorder.SiteId = userData.DatabaseKey.Personnel.SiteId;
            List<UIConfigurationDetailsForModelValidation> configDetails = new RetrieveDataForUIConfiguration().Retrieve(DataDictionaryViewNameConstant.AddWorkRequest, userData);
            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == false && x.Section == false);
            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, objDashboardVM.AddWorkRequest);
                getpropertyInfo = objDashboardVM.AddWorkRequest.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(objDashboardVM.AddWorkRequest);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }

                setpropertyInfo = workorder.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(workorder, val);
            }
            workorder.DepartmentId = userData.DatabaseKey.Personnel.DepartmentId;
            #region V2-948
            if (userData.Site.SourceAssetAccount == true && workorder.Labor_AccountId == 0)
            {
                Equipment equipment = new Equipment
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    SiteId = userData.Site.SiteId,
                    EquipmentId = workorder.ChargeToId
                };
                equipment.Retrieve(this.userData.DatabaseKey);
                workorder.Labor_AccountId = equipment.Labor_AccountId;
            }
            #endregion
            workorder.ChargeToId = 0;
            workorder.ChargeType = objDashboardVM.ChargeType;
            workorder.ChargeToClientLookupId = objDashboardVM.AddWorkRequest.ChargeToClientLookupId ?? string.Empty;// workOrderModel?.ChargeToClientLookupId ?? string.Empty;
            workorder.ClientLookupId = newClientlookupId;
            workorder.SiteId = userData.DatabaseKey.Personnel.SiteId;
            workorder.Creator_PersonnelClientLookupId = userData.DatabaseKey.Personnel.ClientLookupId.ToString();
            workorder.Creator_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            workorder.CreateMode = true;
            workorder.Status = WorkOrderStatusConstants.WorkRequest;
            workorder.SourceType = WorkOrderSourceTypes.WorkRequest;//V2 607
            workorder.CreateByPKForeignKeys(this.userData.DatabaseKey, userData.Site.TimeZone);

            if (workorder.ErrorMessages != null && workorder.ErrorMessages.Count == 0)
            {
                ProcessAlert objAlert = new ProcessAlert(this.userData);
                List<long> wos = new List<long>() { workorder.WorkOrderId };
                Task CreateAlertTask = Task.Factory.StartNew(() => objAlert.CreateAlert<DataContracts.WorkOrder>(AlertTypeEnum.WorkRequestApprovalNeeded, wos));
                Task CreateEventTask1 = Task.Factory.StartNew(() => CreateEventLog(workorder.WorkOrderId, "Create"));
                Task CreateEventTask2 = Task.Factory.StartNew(() => CreateEventLog(workorder.WorkOrderId, "WorkRequest"));
                #region V2-1077
                Task CreateAlertTask3 = Task.Factory.StartNew(() => objAlert.CreateAlert<DataContracts.WorkOrder>(AlertTypeEnum.WorkOrderPlanner, wos));
                #endregion 
                if (configDetails.Any(x => x.Display == true && x.UDF == true && x.Section == false))
                {
                    IEnumerable<string> errors = AddWorkRequestUDFDynamic(objDashboardVM.AddWorkRequest, workorder.WorkOrderId, configDetails);
                    if (errors != null && errors.Count() > 0)
                    {
                        workorder.ErrorMessages.AddRange(errors);
                    }

                }

            }
            else
            {
                errorMsg = workorder.ErrorMessages;
            }
            return workorder;
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
        public List<string> AddWorkRequestUDFDynamic(Models.Work_Order.UIConfiguration.AddWorkRequestModelDynamic woRequest, long WorkOrderId,
   List<UIConfigurationDetailsForModelValidation> configDetails)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            WorkOrderUDF woUDF = new WorkOrderUDF();
            woUDF.ClientId = userData.DatabaseKey.Client.ClientId;
            woUDF.WorkOrderId = WorkOrderId;

            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == true && x.Section == false);
            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, woRequest);
                getpropertyInfo = woRequest.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(woRequest);

                //DyanamicDataBind(ref equipment, ref equipmentUDF);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                setpropertyInfo = woUDF.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(woUDF, val);
            }

            woUDF.Create(_dbKey);
            return woUDF.ErrorMessages;
        }
        #endregion

        #region V2-695 WorkOrder Downtime V2
        public List<WoDowntimeModel> GetWorkOrderDowntime(long WorkOrderId, int skip = 0, int length = 10, string orderbycol = "1", string orderDir = "desc")
        {
            List<WoDowntimeModel> DownTimeModelList = new List<WoDowntimeModel>();
            WoDowntimeModel objDownTimeModel;
            Downtime downtime = new Downtime()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                WorkOrderId = WorkOrderId,
                OrderbyColumn = orderbycol,
                OrderBy = orderDir,
                offset1 = skip,
                nextrow = length
            };
            List<Downtime> workOrderIdDowntimeList = Downtime.RetriveByWorkOrderId_V2(this.userData.DatabaseKey, downtime);
            if (workOrderIdDowntimeList != null)
            {
                //var workOrderList = workOrderIdDowntimeList.Select(x => new { x.WorkOrderId, x.WorkOrderClientLookupId, x.DateDown, x.MinutesDown, x.DowntimeId, x.PersonnelClientLookupId , x.TotalCount }).ToList();
                foreach (var v in workOrderIdDowntimeList)
                {
                    objDownTimeModel = new WoDowntimeModel();
                    objDownTimeModel.Downdate = Convert.ToDateTime(v.DateDown);
                    objDownTimeModel.WorkOrderId = v.WorkOrderId;
                    objDownTimeModel.MinutesDown = v.MinutesDown;
                    objDownTimeModel.WorkOrderClientLookupId = v.WorkOrderClientLookupId;
                    objDownTimeModel.DowntimeId = v.DowntimeId;
                    objDownTimeModel.DowntimeCreateSecurity = userData.Security.MaintenanceCompletionWorkbenchWidget_Downtime.Create;
                    objDownTimeModel.DowntimeEditSecurity = userData.Security.MaintenanceCompletionWorkbenchWidget_Downtime.Edit;
                    objDownTimeModel.DowntimeDeleteSecurity = userData.Security.MaintenanceCompletionWorkbenchWidget_Downtime.Delete;
                    objDownTimeModel.TotalCount = v.TotalCount;
                    objDownTimeModel.ReasonForDownDescription = v.ReasonForDownDescription;
                    objDownTimeModel.TotalMinutesDown = v.TotalMinutesDown;
                    DownTimeModelList.Add(objDownTimeModel);
                }
            }
            return DownTimeModelList;
        }
        public WoDowntimeModel RetrieveByDowntimeId(long DowntimeId)
        {
            WoDowntimeModel objWoDowntimeModel = new WoDowntimeModel();
            Downtime downtime = new Downtime()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                DowntimeId = DowntimeId
            };

            downtime.Retrieve(this.userData.DatabaseKey);
            objWoDowntimeModel = PopulateDowntimeModel(downtime);
            return objWoDowntimeModel;
        }
        internal WoDowntimeModel PopulateDowntimeModel(Downtime aobj)
        {
            WoDowntimeModel oModel = new WoDowntimeModel();
            oModel.DowntimeId = aobj.DowntimeId;
            if (aobj.DateDown != null && aobj.DateDown != default(DateTime))
            {
                oModel.Downdate = aobj.DateDown;
            }
            else
            {
                oModel.Downdate = null;
            }
            oModel.MinutesDown =aobj.MinutesDown;
            oModel.ReasonForDown = aobj.ReasonForDown;
            return oModel;
        }
        public bool DeleteDowntime(long DowntimeId)
        {
            Downtime downtime = new Downtime()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                DowntimeId = DowntimeId
            };
            try
            {
                downtime.Delete(this.userData.DatabaseKey);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public Downtime AddDownTime(WoDowntimeModel wodowntime)
        {
            Downtime downtime = new Downtime()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                EquipmentId = wodowntime.ChargeToId ?? 0,
                ParentSiteId = this.userData.DatabaseKey.User.DefaultSiteId,
                Operator_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId,
                WorkOrderId = wodowntime.WorkOrderId,
                ReasonForDown= wodowntime.ReasonForDown,
                MinutesDown = wodowntime.MinutesDown ?? 0,
                DateDown = wodowntime.Downdate.Value

            };
            var validateMinutesDown = wodowntime.MinutesDown ?? 0;
            if (validateMinutesDown > 0)
            {
              downtime.Create(this.userData.DatabaseKey);
            }
            return downtime;
        }
        public Downtime EditDownTime(WoDowntimeModel wodowntime)
        {
            Downtime downtime = new Downtime()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                EquipmentId = wodowntime.ChargeToId??0,
                DowntimeId = wodowntime.DowntimeId
            };

            downtime.Retrieve(userData.DatabaseKey);

            downtime.ParentSiteId = this.userData.DatabaseKey.User.DefaultSiteId;

            downtime.DateDown = wodowntime.Downdate;
            downtime.MinutesDown = wodowntime.MinutesDown ?? 0;
            downtime.ReasonForDown = wodowntime.ReasonForDown;
            downtime.Update(this.userData.DatabaseKey);
            return downtime;
        }
        #endregion

        #region V2-1056 CMMS – Add Sanitation Request

        public SanitationRequest AddSanitationRequestWorkOrder(DashboardAddSanitationRequestModel obj, ref List<string> ErrorMsgList)
        {
            SanitationRequest SanitationRequest = new SanitationRequest
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId
            };
            string newClientlookupId = "";

            if (obj.ClientLookupId == null && SanitationJobConstant.SanitaionJob_AutoGenerateEnabled)
            {
                newClientlookupId = CustomSequentialId.GetNextId(userData.DatabaseKey, AutoGenerateKey.SANIT_ANNUAL, userData.DatabaseKey.User.DefaultSiteId, "");
            }

            SanitationRequest.SiteId = userData.DatabaseKey.Personnel.SiteId;
            SanitationRequest.ChargeType = obj?.ChargeType ?? string.Empty;
            SanitationRequest.ChargeToClientLookupId = obj?.ChargeToClientLookupId ?? string.Empty;
            SanitationRequest.Description = obj?.Description ?? string.Empty;
            SanitationRequest.ClientLookupId = newClientlookupId;
            SanitationRequest.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            SanitationRequest.Creator_PersonnelClientLookupId = userData.DatabaseKey.Personnel.ClientLookupId.ToString();
            SanitationRequest.Creator_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            SanitationRequest.Status = SanitationJobConstant.JobRequest;
            SanitationRequest.SourceType = SanitationJobSourceType.WorkOrder;
            SanitationRequest.SourceId = obj.WorkOrderId;


            SanitationRequest.Add_SanitationRequest(this.userData.DatabaseKey);

            CreateSanitationEventLog(SanitationRequest.SanitationJobId, SanitationEvents.Create, obj.WorkOrderId);
            CreateSanitationEventLog(SanitationRequest.SanitationJobId, SanitationEvents.JobRequest, obj.WorkOrderId);

            return SanitationRequest;
        }

        private void CreateSanitationEventLog(Int64 sanId, string Status, Int64 sourceId)
        {
            SanitationEventLog log = new SanitationEventLog();
            log.ClientId = userData.DatabaseKey.Client.ClientId;
            log.SiteId = userData.DatabaseKey.Personnel.SiteId;
            log.ObjectId = sanId;
            log.Event = Status;
            log.TransactionDate = DateTime.UtcNow;
            log.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            log.Comments = "";
            log.SourceId = sourceId;
            log.Create(userData.DatabaseKey);
        }
        #endregion

        #region V2-1067 Unplanned WO Dynamic
        public WorkOrder WO_DescribeDynamic(DashboardVM objVM, ref List<string> ErrorMsgList)
        {
            WoDescriptionModelDynamic WoEmergencyModel = objVM.woDescriptionModelDynamic;
            PropertyInfo getpropertyInfo, setpropertyInfo;
            WorkOrder workOrder = new WorkOrder
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId
            };

            if (WorkOrderStatusConstants.Wo_AutoGenerateEnabled)
            {
                newClientlookupId = CustomSequentialId.GetNextId(userData.DatabaseKey, AutoGenerateKey.WO_Annual, userData.DatabaseKey.User.DefaultSiteId, "");
            }

            if (WoEmergencyModel.IsTypeShow)
            {
                workOrder.Type = WoEmergencyModel.Type;
            }
            workOrder.ChargeType = ChargeType.Equipment;

            if (WoEmergencyModel.ChargeToClientLookupId != null)
            {
                var index = WoEmergencyModel.ChargeToClientLookupId.IndexOf('|');
                if (index != -1)
                {
                    workOrder.ChargeToClientLookupId = WoEmergencyModel.ChargeToClientLookupId.Substring(0, index).Trim();
                    long ChrgTo = 0;
                    long.TryParse(workOrder.ChargeToClientLookupId, out ChrgTo);
                    workOrder.ChargeToId = ChrgTo;
                }
                else
                {
                    workOrder.ChargeToClientLookupId = WoEmergencyModel.ChargeToClientLookupId;
                    long ChrgTo = 0;
                    long.TryParse(workOrder.ChargeToClientLookupId, out ChrgTo);
                    workOrder.ChargeToId = ChrgTo;
                }
            }
            if (userData.Site.SourceAssetAccount == true && workOrder.Labor_AccountId == 0)
            {
                Equipment equipment = new Equipment
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    SiteId = userData.Site.SiteId,
                    ClientLookupId = workOrder.ChargeToClientLookupId
                };
                equipment.RetrieveByClientLookupId(this.userData.DatabaseKey);
                workOrder.Labor_AccountId = equipment.Labor_AccountId;
            }
            if (WoEmergencyModel.IsDescriptionShow)
            {
                workOrder.Description = WoEmergencyModel.Description.Trim();
            }
            workOrder.ClientLookupId = newClientlookupId;
            workOrder.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            workOrder.ClientId = userData.DatabaseKey.Client.ClientId;
            workOrder.CompleteBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            workOrder.SourceType = WorkOrderSourceTypes.Emergency;
           
            workOrder.Status = WorkOrderStatusConstants.Scheduled;
            workOrder.ScheduledStartDate = DateTime.UtcNow;
            workOrder.Scheduler_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
            workOrder.WorkAssigned_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            workOrder.ApproveDate = DateTime.UtcNow;
            workOrder.EmergencyWorkOrder = true;
            workOrder.ApproveBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            workOrder.Creator_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;

            List<UIConfigurationDetailsForModelValidation> configDetails = new RetrieveDataForUIConfiguration().Retrieve(DataDictionaryViewNameConstant.WorkOrderDescribeAdd, userData);
            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == false && x.Section == false);

            foreach (var item in ColumnDetails)
            {
                getpropertyInfo = objVM.woDescriptionModelDynamic.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(objVM.woDescriptionModelDynamic);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }

                setpropertyInfo = workOrder.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(workOrder, val);
            }

            workOrder.CreateByPKForeignKeys(this.userData.DatabaseKey, userData.Site.TimeZone);
            if (workOrder.ErrorMessages.Count == 0)
            {
                #region V2-1077
                ProcessAlert objAlert = new ProcessAlert(this.userData);
                List<long> listWO = new List<long>();
                listWO.Add(workOrder.WorkOrderId);
                objAlert.CreateAlert<WorkOrder>(AlertTypeEnum.WorkOrderPlanner, listWO);
                #endregion
                // V2-1123
                // RKL - 2024-Nov-11 - begin
                // Not Writing Schedule Records if there are no udfs displayed
                // Not Writing Event Log Entries if there are no udf displayed
                if (configDetails.Any(x => x.Display == true && x.UDF == true && x.Section == false))
                {                  
                    IEnumerable<string> errors = AddWorkorderDescribeUDFDynamic(objVM.woDescriptionModelDynamic, workOrder.WorkOrderId, configDetails);
                    if (errors != null && errors.Count() > 0)
                    {
                        //workOrder.ErrorMessages.AddRange(errors);
                        ErrorMsgList = workOrder.ErrorMessages;
                    }
                }
                CreateEventLog(workOrder.WorkOrderId, WorkOrderEvents.Create);
                CreateEventLog(workOrder.WorkOrderId, WorkOrderEvents.Approved);
                CreateEventLog(workOrder.WorkOrderId, WorkOrderEvents.Scheduled);
                WorkOrderSchedule workorderschedule = new WorkOrderSchedule()
                {
                    ClientId = this.userData.DatabaseKey.Client.ClientId,
                    WorkOrderId = workOrder.WorkOrderId
                };
                workorderschedule.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
                workorderschedule.ScheduledStartDate = DateTime.UtcNow;
                workorderschedule.ScheduledHours = 1;
                workorderschedule.CreateForWorkOrder(this.userData.DatabaseKey);
                // RKL - 2024-Nov-11 - end
            }
            return workOrder;

        }

        public List<string> AddWorkorderDescribeUDFDynamic(WoDescriptionModelDynamic workorder, long WorkOrderId,
            List<UIConfigurationDetailsForModelValidation> configDetails)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            WorkOrderUDF workOrderUDF = new WorkOrderUDF();
            workOrderUDF.ClientId = userData.DatabaseKey.Client.ClientId;
            workOrderUDF.WorkOrderId = WorkOrderId;

            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == true && x.Section == false);
            foreach (var item in ColumnDetails)
            {
                getpropertyInfo = workorder.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(workorder);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                setpropertyInfo = workOrderUDF.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(workOrderUDF, val);
            }

            workOrderUDF.Create(_dbKey);
            return workOrderUDF.ErrorMessages;
        }

        #endregion
    }
}


