using Client.Models.Work_Order;
using Client.Models.WorkOrder;
using Common.Constants;
using Common.Enumerations;
using DataContracts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Client.BusinessWrapper.Work_Order
{
    public class ApprovalWorkBenchWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        List<string> errorMessage = new List<string>();

        internal ApprovalWorkBenchWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        internal List<WorkOrderModel> GetWOApprovalWorkBenchDetails(string status, string createdates)
        {
            WorkOrder workorder = new WorkOrder();
            List<WorkOrder> woList = new List<WorkOrder>();
            WorkOrderModel workOrderModel;
            List<WorkOrderModel> workOrderModelList = new List<WorkOrderModel>();

            workorder.ClientId = this.userData.DatabaseKey.User.ClientId;
            workorder.SiteId = this.userData.DatabaseKey.Personnel.SiteId;
            workorder.StatusDrop = status;
            workorder.Created = createdates;
            workorder.UserInfoId = this.userData.DatabaseKey.User.CallerUserInfoId;
            // SOM-706 - Added Timezone parameter  
            //workorder.RetrieveWorkBenchForSearchNew method is commented for V2 Implementation V2-630
            // List<WorkOrder> workorderList = workorder.RetrieveWorkBenchForSearchNew(this.userData.DatabaseKey, this.userData.Site.TimeZone);
            List<WorkOrder> workorderList = workorder.RetrieveWorkBenchForSearchV2(this.userData.DatabaseKey, this.userData.Site.TimeZone);
            if (workorderList.Count == 0)
            {
                woList = workorderList;
            }
            else
            {
                woList = workorderList.Where(x => x.Status != "Canceled" && x.Status != "Complete").ToList();
            }
            foreach (var wo in woList)
            {
                workOrderModel = new WorkOrderModel();
                workOrderModel.WorkOrderId = wo.WorkOrderId;
                workOrderModel.ClientLookupId = wo.ClientLookupId;
                workOrderModel.Description = wo.Description;
                workOrderModel.ChargeTo_Name = wo.ChargeTo_Name;
                workOrderModel.ChargeToClientLookupId = wo.ChargeToClientLookupId;
                workOrderModel.WorkAssigned_PersonnelId = wo.WorkAssigned_PersonnelId;
                if (wo.WorkAssigned_PersonnelId != 0)
                {
                    //Personnel p = new Personnel()
                    //{
                    //    ClientId = this.userData.DatabaseKey.User.ClientId,
                    //    PersonnelId = wo.WorkAssigned_PersonnelId
                    //};
                    //p.Retrieve(userData.DatabaseKey);
                    //workOrderModel.WorkAssigned = p.ClientLookupId;
                    workOrderModel.WorkAssigned = wo.WorkAssigned_PersonnelClientLookupId; //Code optimization based on Error log (using data retrieved from sp instead of calling the Personnel.Retrieve())
                }
                else
                {
                    workOrderModel.WorkAssigned = "--Select--";
                }
                workOrderModel.Shift = wo.Shift;
                if (wo.CreateDate != null && wo.CreateDate == default(DateTime))
                {
                    workOrderModel.CreateDate = null;
                }
                else
                {
                    workOrderModel.CreateDate = wo.CreateDate;
                }
                if (wo.ScheduledStartDate != null && wo.ScheduledStartDate == default(DateTime))
                {
                    workOrderModel.ScheduledStartDate = null;
                }
                else
                {
                    workOrderModel.ScheduledStartDate = wo.ScheduledStartDate;
                }

                workOrderModel.Createby = wo.Createby;
                workOrderModel.ScheduledDuration = wo.ScheduledDuration;
                workOrderModelList.Add(workOrderModel);
            }
            return workOrderModelList;
        }
        internal bool DenyWorkorder(string[] workOrderId, string DeniedReason, string denyComments)
        {
            try
            {
                foreach (var wid in workOrderId)
                {
                    // SOM-831
                    // We need to use WorkOrder.Creator_PersonnelId NOT WorkOrder.CreatedBy
                    // We need to retrieve the work order information   
                    // We can get the userinfoid from the personnel record 
                    WorkOrder WorkOrderJob = new WorkOrder()
                    {
                        ClientId = this.userData.DatabaseKey.User.ClientId,
                        SiteId = this.userData.DatabaseKey.User.DefaultSiteId,
                        WorkOrderId = Convert.ToInt64(wid)
                    };
                    // Retrieve the work order from the database 
                    WorkOrderJob.RetrieveByPKForeignKeys(this.userData.DatabaseKey, userData.Site.TimeZone);
                    // Now fill in so it will be approved when done
                    WorkOrderJob.Status = WorkOrderStatusConstants.Denied;
                    WorkOrderJob.DeniedReason = DeniedReason;
                    WorkOrderJob.DeniedDate = DateTime.UtcNow;
                    // SOM-1037
                    WorkOrderJob.ApproveBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
                    WorkOrderJob.ModifyBy = userData.DatabaseKey.Personnel.CallerUserName;
                    WorkOrderJob.ModifyDate = DateTime.UtcNow;
                    WorkOrderJob.DeniedComment = denyComments;
                    WorkOrderJob.DeniedFlag = "Yes";
                    WorkOrderJob.ScheduledStartDate = null;

                    if (!string.IsNullOrEmpty(DeniedReason))
                    {
                        // SOM-1037
                        WorkOrderJob.UpdateByWorkbench(userData.DatabaseKey);
                        CreateEventLog(WorkOrderJob.WorkOrderId, WorkOrderEvents.Denied, denyComments);
                        AlertCreate(WorkOrderJob, AlertTypeEnum.WorkRequestDenied);
                    }

                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }


        }
        #region Event Log
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
        private void CreateEventLog(Int64 WOId, string Status, string Comment)
        {
            WorkOrderEventLog log = new WorkOrderEventLog();
            log.ClientId = userData.DatabaseKey.Client.ClientId;
            log.SiteId = userData.DatabaseKey.Personnel.SiteId;
            log.WorkOrderId = WOId;
            log.Event = Status;
            log.TransactionDate = DateTime.UtcNow;
            log.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            log.Comments = Comment;
            log.SourceId = 0;
            log.Create(userData.DatabaseKey);
        }
        #endregion

        internal List<string> ApproveWODetails(List<ApproveWorkOrderModel> wOData)
        {
            WorkOrder WorkOrderJob = new WorkOrder();
            CultureInfo provider = CultureInfo.InvariantCulture;
            var todaysDate = DateTime.UtcNow;
            List<string> errorMessage = new List<string>();

            Parallel.ForEach(wOData,wo=> {
                WorkOrderJob = new WorkOrder();
                WorkOrderJob.ClientId = this.userData.DatabaseKey.User.ClientId;
                WorkOrderJob.SiteId = userData.DatabaseKey.User.DefaultSiteId;
                WorkOrderJob.WorkOrderId = Convert.ToInt64(wo.woid);
                WorkOrderJob.ApproveBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
                WorkOrderJob.Createby = userData.DatabaseKey.UserName;
                WorkOrderJob.ModifyBy = userData.DatabaseKey.UserName;
                WorkOrderJob.ApproveFlag = "Yes";
                WorkOrderJob.DeniedFlag = "No";
                WorkOrderJob.ScheduledDuration = Convert.ToDecimal(wo.duration);
                WorkOrderJob.WorkAssigned_PersonnelId = Convert.ToInt64(wo.workassignedval);
                WorkOrderJob.Shift = wo.shiftval;
                if (!string.IsNullOrEmpty(wo.scheduledate))
                {
                    WorkOrderJob.ScheduledStartDate = DateTime.ParseExact(wo.scheduledate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    if (WorkOrderJob.ScheduledStartDate < todaysDate)
                    {
                        WorkOrderJob.ScheduledStartDate = todaysDate;
                    }
                }
                else
                {
                    WorkOrderJob.ScheduledStartDate = DateTime.UtcNow;
                }
                if (WorkOrderJob.ScheduledDuration > 0 && WorkOrderJob.WorkAssigned_PersonnelId > 0 &&
                    WorkOrderJob.ScheduledStartDate != null && (WorkOrderJob.ScheduledStartDate.Value.Year > DateTime.Now.Year - 1))
                {
                    WorkOrderJob.ScheduleFlag = "Yes";
                }
                else
                    WorkOrderJob.ScheduleFlag = "No";

                WorkOrderJob.UpdateByWorkbench(userData.DatabaseKey);

                if (WorkOrderJob.ErrorMessages != null && WorkOrderJob.ErrorMessages.Count > 0)
                {
                    errorMessage.AddRange(WorkOrderJob.ErrorMessages);
                    return;// WorkOrderJob;
                }
                else
                {
                    if (WorkOrderJob.ScheduleFlag == "Yes")
                    {
                        Task task1 = Task.Factory.StartNew(() => AlertCreate(WorkOrderJob, AlertTypeEnum.WorkRequestApproved));
                        Task task2 = Task.Factory.StartNew(() => AlertCreate(WorkOrderJob, AlertTypeEnum.WorkOrderScheduleAssignedUser));

                        Task task3 = Task.Factory.StartNew(() => CreateEventLog(WorkOrderJob.WorkOrderId, WorkOrderEvents.Approved));
                        Task task4 = Task.Factory.StartNew(() => CreateEventLog(WorkOrderJob.WorkOrderId, WorkOrderEvents.Scheduled));
                    }
                    if (WorkOrderJob.ScheduleFlag == "No")
                    {
                        Task task1 = Task.Factory.StartNew(() => AlertCreate(WorkOrderJob, AlertTypeEnum.WorkRequestApproved));
                        Task task2 = Task.Factory.StartNew(() => AlertCreate(WorkOrderJob, AlertTypeEnum.WorkOrderScheduleAssignedUser));
                        Task task3 = Task.Factory.StartNew(() => ApproveWorkDetails(WorkOrderJob.WorkOrderId));
                        task3.Wait();
                    }
                }


            });
            //foreach (var wo in wOData)
            //{
            //    WorkOrderJob = new WorkOrder();
            //    WorkOrderJob.ClientId = this.userData.DatabaseKey.User.ClientId;
            //    WorkOrderJob.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            //    WorkOrderJob.WorkOrderId = Convert.ToInt64(wo.woid);
            //    WorkOrderJob.ApproveBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            //    WorkOrderJob.Createby = userData.DatabaseKey.UserName;
            //    WorkOrderJob.ModifyBy = userData.DatabaseKey.UserName;
            //    WorkOrderJob.ApproveFlag = "Yes";
            //    WorkOrderJob.DeniedFlag = "No";
            //    WorkOrderJob.ScheduledDuration = Convert.ToDecimal(wo.duration);
            //    WorkOrderJob.WorkAssigned_PersonnelId = Convert.ToInt64(wo.workassignedval);
            //    WorkOrderJob.Shift = wo.shiftval;
            //    if (!string.IsNullOrEmpty(wo.scheduledate))
            //    {
            //        WorkOrderJob.ScheduledStartDate = DateTime.ParseExact(wo.scheduledate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            //        if (WorkOrderJob.ScheduledStartDate < todaysDate)
            //        {
            //            WorkOrderJob.ScheduledStartDate = todaysDate;
            //        }
            //    }
            //    else
            //    {
            //        WorkOrderJob.ScheduledStartDate = DateTime.UtcNow;
            //    }
            //    if (WorkOrderJob.ScheduledDuration > 0 && WorkOrderJob.WorkAssigned_PersonnelId > 0 &&
            //        WorkOrderJob.ScheduledStartDate != null && (WorkOrderJob.ScheduledStartDate.Value.Year > DateTime.Now.Year - 1))
            //    {
            //        WorkOrderJob.ScheduleFlag = "Yes";
            //    }
            //    else
            //        WorkOrderJob.ScheduleFlag = "No";

            //    WorkOrderJob.UpdateByWorkbench(userData.DatabaseKey);

            //    if (WorkOrderJob.ErrorMessages != null && WorkOrderJob.ErrorMessages.Count > 0)
            //    {
            //        return WorkOrderJob;
            //    }
               
            //    if (WorkOrderJob.ScheduleFlag == "Yes")
            //    {
            //        Task task1 = Task.Factory.StartNew(() =>AlertCreate(WorkOrderJob, AlertTypeEnum.WorkRequestApproved));
            //        Task task2 = Task.Factory.StartNew(() =>AlertCreate(WorkOrderJob, AlertTypeEnum.WorkOrderScheduleAssignedUser));
                                
            //        Task task3 = Task.Factory.StartNew(() =>CreateEventLog(WorkOrderJob.WorkOrderId, WorkOrderEvents.Approved));
            //        Task task4 = Task.Factory.StartNew(() => CreateEventLog(WorkOrderJob.WorkOrderId, WorkOrderEvents.Scheduled));
            //    }
            //    if (WorkOrderJob.ScheduleFlag == "No")
            //    {
            //        Task task1 = Task.Factory.StartNew(() => AlertCreate(WorkOrderJob, AlertTypeEnum.WorkRequestApproved));
            //        Task task2 = Task.Factory.StartNew(() => AlertCreate(WorkOrderJob, AlertTypeEnum.WorkOrderScheduleAssignedUser));
            //        Task task3 = Task.Factory.StartNew(() => ApproveWorkDetails(WorkOrderJob.WorkOrderId));
            //        task3.Wait();
            //    }
                
            //}
            
            return errorMessage;

        }
        internal WOInfoModel getWorkOderInfoDetailsById(long workOrderId)
        {
            WOInfoModel workOrderModel = new WOInfoModel();
            WorkOrder workorder = new WorkOrder()
            {
                ClientId = _dbKey.Client.ClientId,
                WorkOrderId = workOrderId
            };
            workorder.RetrieveByPKForeignKeys(this.userData.DatabaseKey, userData.Site.TimeZone);
            workOrderModel = initializeWOInfoControls(workorder);
            return workOrderModel;
        }
        private void ApproveWorkDetails(long woid)
        {
            WorkOrder Job = new WorkOrder()
            {
                ClientId = this.userData.DatabaseKey.User.ClientId,
                SiteId = this.userData.DatabaseKey.User.DefaultSiteId,
                WorkOrderId = woid
            };
            // Retrieve the work order from the database 
            Job.RetrieveByPKForeignKeys(this.userData.DatabaseKey, userData.Site.TimeZone);
            // Now fill in so it will be approved when done
            Job.Status = WorkOrderStatusConstants.Approved;
            Job.ApproveBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            Job.ModifyBy = userData.DatabaseKey.UserName;
            Job.DeniedFlag = "No";
            Job.ApproveFlag = "Yes";
            Job.ScheduleFlag = "No";
            Job.ScheduledStartDate = null;
            Job.UpdateByWorkbench(userData.DatabaseKey);
            CreateEventLog(Job.WorkOrderId, WorkOrderEvents.Approved);
            AlertCreate(Job, AlertTypeEnum.WorkRequestApproved);
        }
        private void AlertCreate(WorkOrder workOrder, AlertTypeEnum alertTypeEnum) //Process Alert
        {
            ProcessAlert objAlert = new ProcessAlert(this.userData);
            if (alertTypeEnum == AlertTypeEnum.WorkRequestApproved || alertTypeEnum == AlertTypeEnum.WorkRequestDenied)
            {
                List<long> wolist = new List<long>();
                wolist.Add(workOrder.WorkOrderId);
                objAlert.CreateAlert<WorkOrder>(alertTypeEnum, wolist);
            }
        }
        internal WorkOrder AddWorkOrderInfoDetails(WorkOrderVM workOrderVM)
        {
            WorkOrder WorkOrderJob = new WorkOrder()
            {
                ClientId = this.userData.DatabaseKey.User.ClientId,
                SiteId = this.userData.DatabaseKey.User.DefaultSiteId,
                WorkOrderId = Convert.ToInt64(workOrderVM.WOInfoModel.WorkOrderId)
            };
            // Retrieve the work order from the database 
            WorkOrderJob.RetrieveByPKForeignKeys(this.userData.DatabaseKey, userData.Site.TimeZone);

            WorkOrderJob.ChargeToClientLookupId = workOrderVM.WOInfoModel.ChargeToClientLookupId ?? string.Empty;
            WorkOrderJob.ChargeType = workOrderVM.WOInfoModel.ChargeType ?? string.Empty;
            WorkOrderJob.Shift = workOrderVM.WOInfoModel.Shift ?? string.Empty;
            WorkOrderJob.Type = workOrderVM.WOInfoModel.Type ?? string.Empty;
            WorkOrderJob.Priority = workOrderVM.WOInfoModel.Priority ?? string.Empty;
            WorkOrderJob.ApproveDate = DateTime.UtcNow;
            WorkOrderJob.ApproveBy_PersonnelClientLookupId = this.userData.DatabaseKey.Personnel.ClientLookupId;
            WorkOrderJob.ModifyBy = userData.DatabaseKey.UserName;
            WorkOrderJob.ApproveBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;

            if ((workOrderVM.WOInfoModel.WorkAssigned_PersonnelId == null || workOrderVM.WOInfoModel.WorkAssigned_PersonnelId == 0) && (workOrderVM.WOInfoModel.ScheduledStartDate == null))
            {
                WorkOrderJob.Status = WorkOrderStatusConstants.Approved;
                WorkOrderJob.WorkAssigned_PersonnelId = 0;
                WorkOrderJob.ScheduledStartDate = null;
                WorkOrderJob.UpdateByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);
                // SOM-1227 - RKL
                AlertCreate(WorkOrderJob, AlertTypeEnum.WorkRequestApproved);

            }
            else if ((workOrderVM.WOInfoModel.WorkAssigned_PersonnelId != null || workOrderVM.WOInfoModel.WorkAssigned_PersonnelId != 0) && (workOrderVM.WOInfoModel.ScheduledStartDate != null))
            {
                WorkOrderJob.Status = WorkOrderStatusConstants.Scheduled;
                WorkOrderJob.WorkAssigned_PersonnelId = workOrderVM.WOInfoModel.WorkAssigned_PersonnelId ?? 0;
                WorkOrderJob.ScheduledStartDate = workOrderVM.WOInfoModel.ScheduledStartDate;
                WorkOrderJob.ApproveFlag = "Yes";
                WorkOrderJob.ScheduleFlag = "Yes";
                WorkOrderJob.ScheduledDuration = workOrderVM.WOInfoModel.ScheduledDuration ?? 0;
                WorkOrderJob.UpdateByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);
                WorkOrderJob.ModifyBy = userData.DatabaseKey.UserName;
                WorkOrderJob.UpdateByWorkbench(userData.DatabaseKey);
                AlertCreate(WorkOrderJob, AlertTypeEnum.WorkRequestApproved);

            }
            return WorkOrderJob;
        }
        private WOInfoModel initializeWOInfoControls(WorkOrder obj)
        {
            WOInfoModel objworkorder = new WOInfoModel();

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
            objworkorder.Assigned = obj?.Assigned ?? string.Empty;
            if (obj.ScheduledStartDate != null && obj.ScheduledStartDate == default(DateTime))
            {
                objworkorder.ScheduledStartDate = null;

            }
            else
            {
                objworkorder.ScheduledStartDate = obj.ScheduledStartDate;
            }

            objworkorder.ActualFinishDate = obj?.ActualFinishDate ?? DateTime.MinValue;
            objworkorder.CompleteBy = obj?.CompleteBy_PersonnelName ?? string.Empty;
            objworkorder.CompleteBy_PersonnelId = obj.CompleteBy_PersonnelId;
            objworkorder.CompleteComments = obj?.CompleteComments ?? string.Empty;
            objworkorder.ChargeTo_Name = obj?.ChargeTo_Name ?? string.Empty;
            objworkorder.ChargeToClientLookupId = obj?.ChargeToClientLookupId ?? string.Empty;
            return objworkorder;
        }
    }
}