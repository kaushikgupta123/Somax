using Client.Models.WorkOrderPlanning;
using Common.Constants;
using DataContracts;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Client.BusinessWrapper.WorkOrderPlanning
{
    public class WorkOrderPlanningSearchWrapper 
    {
        private DatabaseKey _dbKey;
        private UserData userData;

        public WorkOrderPlanningSearchWrapper(UserData userData) 
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        public List<WorkOrderPlanningModel> GetWorkOrderPlanGridData(int CustomQueryDisplayId, string orderbycol = "", string orderDir = "", int skip = 0, int length = 0)
        {
            WorkOrderPlan workOrderPlan = new WorkOrderPlan();
            WorkOrderPlanningModel WOPModel;
            List<WorkOrderPlanningModel> WOPModelList = new List<WorkOrderPlanningModel>();

            workOrderPlan.ClientId = userData.DatabaseKey.Client.ClientId;
            workOrderPlan.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            workOrderPlan.CustomQueryDisplayId = CustomQueryDisplayId;
            workOrderPlan.OrderbyColumn = orderbycol;
            workOrderPlan.OrderBy = orderDir;
            workOrderPlan.OffSetVal = skip;
            workOrderPlan.NextRow = length;


            List<WorkOrderPlan> WorkOrderPlanList = workOrderPlan.RetrieveChunkSearch(this.userData.DatabaseKey);

            if (WorkOrderPlanList != null)
            {
                foreach (var item in WorkOrderPlanList)
                {
                    WOPModel = new WorkOrderPlanningModel();
                    WOPModel.WorkOrderPlanId = item.WorkOrderPlanId;
                    WOPModel.PlanID = item.WorkOrderPlanId;
                    WOPModel.Description = item.Description;
                    if (item.StartDate != null && item.StartDate == default(DateTime))
                    {
                        WOPModel.StartDate = null;
                    }
                    else
                    {
                        WOPModel.StartDate = item.StartDate;
                    }
                    if (item.EndDate != null && item.EndDate == default(DateTime))
                    {
                        WOPModel.EndDate = null;
                    }
                    else
                    {
                        WOPModel.EndDate = item.EndDate;
                    }
                    WOPModel.Status = item.Status;
                    if (item.CreateDate != null && item.CreateDate == default(DateTime))
                    {
                        WOPModel.Created = null;
                    }
                    else
                    {
                        WOPModel.Created = item.CreateDate;
                    }
                    if (item.CompleteDate != null && item.CompleteDate == default(DateTime))
                    {
                        WOPModel.Completed = null;
                    }
                    else
                    {
                        WOPModel.Completed = item.CompleteDate;
                    }
                    WOPModel.ChildCount = item.ChildCount;
                    WOPModel.TotalCount = item.TotalCount;
                    WOPModelList.Add(WOPModel);
                }
            }



            return WOPModelList;
        }
        internal List<WOPlanLineItemModel> PopulateLineitems(long WorkOrderPlanId)
        {
            WOPlanLineItemModel objLineItem;
            List<WOPlanLineItemModel> LineItemList = new List<WOPlanLineItemModel>();

            WOPlanLineItem woPlanLineItem = new WOPlanLineItem()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                WorkOrderPlanId = WorkOrderPlanId
            };
            List<WOPlanLineItem> woplanLintItemList = woPlanLineItem.RetrieveWOPlanLineItem_ByWorkOrderPlanId(this.userData.DatabaseKey);

            if (woplanLintItemList != null)
            {
                foreach (var item in woplanLintItemList)
                {
                    objLineItem = new WOPlanLineItemModel();
                    objLineItem.ClientLookupId = item.WorkOrderClientLookupId;
                    objLineItem.Description = item.Description;
                    if (item.RequiredDate == null || item.RequiredDate == default(DateTime))
                    {
                        objLineItem.RequiredDate = null;
                    }
                    else
                    {
                        objLineItem.RequiredDate = item.RequiredDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    }
                    objLineItem.AssetId = item.EquipmentClientLookupId;
                    objLineItem.ChargeTo_Name = item.ChargeTo_Name;
                    objLineItem.Status = item.Status;
                    if (item.CompleteDate == null || item.CompleteDate == default(DateTime))
                    {
                        objLineItem.CompleteDate = null;
                    }
                    else
                    {
                        objLineItem.CompleteDate = item.CompleteDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    }
                    //objLineItem.PurchaseUOM = item.PurchaseUOM;
                    objLineItem.Type = item.Type;
                   

                    LineItemList.Add(objLineItem);
                }
            }

            return LineItemList;
        }

        #region Add Wop
        public WorkOrderPlan AddWorkOrderPlan(WorkOrderPlanningVM objWOPVM)
        {
           
            string emptyValue = string.Empty;

            List<string> errList = new List<string>();
            WorkOrderPlan workorderPlan = new WorkOrderPlan();

            if (objWOPVM.workorderPlanningModel.WorkOrderPlanId == 0)
            {
                //Add in WorkOrderPlan Table
                workorderPlan.ClientId = this.userData.DatabaseKey.User.ClientId;
                workorderPlan.SiteId = this.userData.DatabaseKey.User.DefaultSiteId;
                workorderPlan.AreaId = 0;
                workorderPlan.DepartmentId = 0;
                workorderPlan.StoreroomId = 0;               
                workorderPlan.Description = !string.IsNullOrEmpty(objWOPVM.workorderPlanningModel.Description) ? objWOPVM.workorderPlanningModel.Description.Trim() : emptyValue;
                workorderPlan.StartDate = objWOPVM.workorderPlanningModel.StartDate;
                workorderPlan.EndDate = objWOPVM.workorderPlanningModel.EndDate;
                workorderPlan.Assign_PersonnelId = objWOPVM.workorderPlanningModel.PersonnelId ?? 0;
                workorderPlan.LockPlan = false;
                workorderPlan.CompleteDate = null;
                workorderPlan.CompleteBy_PersonnelId = 0;
                workorderPlan.Status = WorkOrderPlanStatusConstants.Open;
                workorderPlan.Create(this.userData.DatabaseKey);


                //Add in WorkOrderPlanEventLog table
                CreateWOPEventLog(workorderPlan.WorkOrderPlanId, WorkOrderPlanStatusConstants.Open);


            }


            return workorderPlan;
        }
        #endregion

        #region Common method
        private void CreateWOPEventLog(Int64 WOPId, string Status)
        {
            WorkOrderPlanEventLog log = new WorkOrderPlanEventLog();
            log.ClientId = userData.DatabaseKey.Client.ClientId;
            log.SiteId = userData.DatabaseKey.Personnel.SiteId;
            log.WorkOrderPlanId = WOPId;
            log.TransactionDate = DateTime.UtcNow;
            log.Event = Status;
            log.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            log.Comments = "";
            log.SourceTable = "";
            log.SourceId = 0;
            log.Create(userData.DatabaseKey);
        }
        #endregion
    }
}