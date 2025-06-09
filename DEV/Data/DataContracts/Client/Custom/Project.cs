using Database;
using Database.Business;
using Database.Transactions;
using System;
using System.Collections.Generic;

namespace DataContracts
{
    public partial class Project : DataContractBase, IStoredProcedureValidation
    {
        #region Property
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int OffSetVal { get; set; }
        public int NextRow { get; set; }
        public int CustomQueryDisplayId { get; set; }
        public string SearchText { get; set; }
        public string WorkOrderClientLookupId { get; set; }
        public string CreateStartDateVw { get; set; }
        public string CreateEndDateVw { get; set; }
        public string CompleteStartDateVw { get; set; }
        public string CompleteEndDateVw { get; set; }
        public string CloseStartDateVw { get; set; }
        public string CloseEndDateVw { get; set; }
        public DateTime CreateDate { get; set; }
        public string Coordinator { get; set; }
        public string Priority { get; set; }
        public long ChargeToId { get; set; }
        public string ChargeTo { get; set; }
        public string ChargeTo_Name { get; set; }
        public long WorkOrderId { get; set; }
        public long PurchasOrderId { get; set; }
        
        public DateTime? RequiredDate { get; set; }
        public int TotalCount { get; set; }
        public int ChildCount { get; set; }

        public string AG1ClientLookupId { get; set; }
        public string AG2ClientLookupId { get; set; }
        public string AG3ClientLookupId { get; set; }
        # region V2-1087
        public string Planner { get; set; }
        public string Buyer { get; set; }
        public string PurchasOrderClientLookupId { get; set; }
        public int Line { get; set; }
        public string PartID { get; set; }
        public string PC_WO_CompleteDate { get; set; }

        
        public decimal? MaterialCost { get; set; }
        public decimal? LaborCost { get; set; }
        public decimal? TotalCost { get; set; }
        public decimal? UnitCost { get; set; }
        public decimal? Quantity { get; set; }
        public string PC_PO_CompleteDate { get; set; }
        #endregion

        public string ValidateFor = string.Empty;
        public List<Project> listOfProject { get; set; }
        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public Int32 offset1 { get; set; }
        public Int32 nextrow { get; set; }
        public int totalCount { get; set; }
        public string CoordinatorFullName { get; set; }
        public string OwnerFullName { get; set; }
        public decimal PurchasingCost { get; set; }
        public decimal Spent { get; set; }
        public decimal Remaining { get; set; }
        public decimal SpentPercentage { get; set; }
        public decimal RemainingPercentage { get; set; }

        #endregion
        #region Project Chunk Search
        public List<Project> RetrieveChunkSearch(DatabaseKey dbKey)
        {
            Project_RetrieveChunkSearch trans = new Project_RetrieveChunkSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Project = this.ToDateBaseObjectForRetrieveChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Project> Projectlist = new List<Project>();

            foreach (b_Project proj in trans.ProjectList)
            {
                Project tmpProj = new Project();
                tmpProj.UpdateFromDatabaseObjectForRetriveAllForSearch(proj);
                Projectlist.Add(tmpProj);
            }
            return Projectlist;
        }
        public b_Project ToDateBaseObjectForRetrieveChunkSearch()
        {
            b_Project dbObj = this.ToDatabaseObject();
            dbObj.CustomQueryDisplayId = this.CustomQueryDisplayId;
            dbObj.SearchText = this.SearchText;
            dbObj.WorkOrderClientLookupId = this.WorkOrderClientLookupId;
            dbObj.CreateStartDateVw = this.CreateStartDateVw;
            dbObj.CreateEndDateVw = this.CreateEndDateVw;
            dbObj.CompleteStartDateVw = this.CompleteStartDateVw;
            dbObj.CompleteEndDateVw = this.CompleteEndDateVw;
            dbObj.CloseStartDateVw = this.CloseStartDateVw;
            dbObj.CloseEndDateVw = this.CloseEndDateVw;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;

            return dbObj;
        }
        public void UpdateFromDatabaseObjectForRetriveAllForSearch(b_Project dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.CustomQueryDisplayId = dbObj.CustomQueryDisplayId;
            this.SearchText = dbObj.SearchText;
            this.WorkOrderClientLookupId = dbObj.WorkOrderClientLookupId;
            this.CreateStartDateVw = dbObj.CreateStartDateVw;
            this.CreateEndDateVw = dbObj.CreateEndDateVw;
            this.CompleteStartDateVw = dbObj.CompleteStartDateVw;
            this.CompleteEndDateVw = dbObj.CompleteEndDateVw;
            this.CloseStartDateVw = dbObj.CloseStartDateVw;
            this.CloseEndDateVw = dbObj.CloseEndDateVw;
            this.CreateDate = dbObj.CreateDate;
            this.OrderbyColumn = dbObj.OrderbyColumn;
            this.OrderBy = dbObj.OrderBy;
            this.OffSetVal = dbObj.OffSetVal;
            this.NextRow = dbObj.NextRow;
            this.ChildCount = dbObj.ChildCount;
            this.Budget = dbObj.Budget;
            this.TotalCount = dbObj.TotalCount;
        }

        #endregion
        #region Project tab header
        public Project RetrieveProjectByProjectIdForHeader(DatabaseKey dbKey)
        {
            Project_RetrieveByProjectIdForHeader trans = new Project_RetrieveByProjectIdForHeader()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Project = this.ToDateBaseObjectForRetrieveProjectByProjectIdForHeader();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            Project tmpProj = new Project();
            tmpProj.UpdateFromDatabaseObjectForRetrieveProjectByProjectIdForHeader(trans.projectObj);
            return tmpProj;
        }
        public b_Project ToDateBaseObjectForRetrieveProjectByProjectIdForHeader()
        {
            b_Project dbObj = this.ToDatabaseObject();
            dbObj.Coordinator = this.Coordinator;
            return dbObj;
        }
        public void UpdateFromDatabaseObjectForRetrieveProjectByProjectIdForHeader(b_Project dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.Coordinator = dbObj.Coordinator;
        }
        #endregion
        #region WorkOrder Project details LookupList By Search Criteria
        public List<Project> RetrieveWorkOrder_ProjectDetailsLookupListBySearchCriteria(DatabaseKey dbKey)
        {
            WorkOrder_ProjectDetailsLookupListBySearchCriteria trans = new WorkOrder_ProjectDetailsLookupListBySearchCriteria()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Project = this.ToDateBaseObjectForRetrieveWorkOrder_ProjectDetailsLookupListBySearchCriteria();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Project> workOrderlist = new List<Project>();

            foreach (b_Project wo in trans.workOrderList)
            {
                Project tmpWO = new Project();
                tmpWO.UpdateFromDatabaseObjectForRetrieveWorkOrder_ProjectDetailsLookupListBySearchCriteria(wo);
                workOrderlist.Add(tmpWO);
            }
            return workOrderlist;
        }
        public b_Project ToDateBaseObjectForRetrieveWorkOrder_ProjectDetailsLookupListBySearchCriteria()
        {
            b_Project dbObj = this.ToDatabaseObject();
            dbObj.WorkOrderId = this.WorkOrderId;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;
            dbObj.WorkOrderClientLookupId = this.WorkOrderClientLookupId;
            dbObj.ChargeToId = this.ChargeToId;
            dbObj.ChargeTo_Name = this.ChargeTo_Name;
            dbObj.Description = this.Description;
            dbObj.Status = this.Status;
            dbObj.Priority = this.Priority;
            dbObj.Type = this.Type;
            dbObj.RequiredDate = this.RequiredDate;
            dbObj.ChargeTo = this.ChargeTo;
            dbObj.TotalCount = this.TotalCount;
            return dbObj;
        }
        public void UpdateFromDatabaseObjectForRetrieveWorkOrder_ProjectDetailsLookupListBySearchCriteria(b_Project dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.WorkOrderId = dbObj.WorkOrderId;
            this.OrderbyColumn = dbObj.OrderbyColumn;
            this.OrderBy = dbObj.OrderBy;
            this.OffSetVal = dbObj.OffSetVal;
            this.NextRow = dbObj.NextRow;
            this.WorkOrderClientLookupId = dbObj.ClientLookupId;
            this.ChargeToId = dbObj.ChargeToId;
            this.ChargeTo_Name = dbObj.ChargeTo_Name;
            this.Description = dbObj.Description;
            this.Status = dbObj.Status;
            this.Priority = dbObj.Priority;
            this.Type = dbObj.Type;
            this.RequiredDate = dbObj.RequiredDate;
            this.ChargeTo = dbObj.ChargeTo;
            this.TotalCount = dbObj.TotalCount;
        }


        #endregion
        #region Check storedprocedure validation
        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();
            if (ValidateFor == "CheckDuplicate")
            {
                Project_ValidateClientLookupId trans = new Project_ValidateClientLookupId()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.Project = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                errors = StoredProcValidationError.UpdateFromDatabaseObjects(trans.StoredProcValidationErrorList);


            }
            return errors;
        }

        public void CheckDuplicateProject(DatabaseKey dbKey)
        {
            ValidateFor = "CheckDuplicate";
            Validate<Project>(dbKey);
        }
        #endregion

        #region Project Chunk Search Lookuplist
        public List<Project> GetAllProjectLookupListV2(DatabaseKey dbKey, string TimeZone)
        {
            Project_RetrieveChunkSearchLookupListV2 trans = new Project_RetrieveChunkSearchLookupListV2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Project = this.ToDateBaseObjectForProjectLookuplistChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            //this.listOfProject = new List<Project>();

            List<Project> Projectlist = new List<Project>();
            // Moved this to UpdateFromDatabaseObjectForRetriveV2
            foreach (b_Project proj in trans.ProjectList)
            {
                Project tmpProject = new Project();

                tmpProject.UpdateFromDatabaseObjectForProjectLookupListChunkSearch(proj, TimeZone);
                Projectlist.Add(tmpProject);
            }
            return Projectlist;
        }
        public b_Project ToDateBaseObjectForProjectLookuplistChunkSearch()
        {
            b_Project dbObj = this.ToDatabaseObject();


            dbObj.orderbyColumn = this.orderbyColumn;
            dbObj.orderBy = this.orderBy;
            dbObj.offset1 = this.offset1;
            dbObj.nextrow = this.nextrow;
            dbObj.ClientLookupId = this.ClientLookupId;
            dbObj.Description = this.Description;
            return dbObj;
        }
        public void UpdateFromDatabaseObjectForProjectLookupListChunkSearch(b_Project dbObj, string TimeZone)
        {
            this.UpdateFromDatabaseObject(dbObj);

            this.totalCount = dbObj.totalCount;
        }
        #endregion

        #region V2-1135
        public List<Project> RetrieveWorkoderProjectLookupList_V2(DatabaseKey dbKey)
        {
            Project_RetrieveWorkoderProjectLookupList_V2 trans = new Project_RetrieveWorkoderProjectLookupList_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Project = this.ToDateBaseObjectForRetrieveWorkoderProjectLookupList_V2();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Project> Projectlist = new List<Project>();
            foreach (b_Project proj in trans.ProjectList)
            {
                Project tmpProject = new Project();

                tmpProject.UpdateFromDatabaseObjectForRetrieveWorkoderProjectLookupList_V2(proj);
                Projectlist.Add(tmpProject);
            }
            return Projectlist;
        }
        public b_Project ToDateBaseObjectForRetrieveWorkoderProjectLookupList_V2()
        {
            b_Project dbObj = new b_Project();
            dbObj.SiteId = this.SiteId;
            dbObj.ClientLookupId = this.ClientLookupId;
            return dbObj;
        }
        public void UpdateFromDatabaseObjectForRetrieveWorkoderProjectLookupList_V2(b_Project dbObj)
        {
         
            this.ProjectId = dbObj.ProjectId;
            this.ClientLookupId = dbObj.ClientLookupId;
        }
        #endregion

        #region V2-1087 Project tab View
        public void RetrieveByPK_V2(DatabaseKey dbKey)
        {
            Project_RetrieveByPk_V2 trans = new Project_RetrieveByPk_V2();
            trans.Project = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseByPK_V2(trans.Project);
        }
        public void UpdateFromDatabaseByPK_V2(b_Project dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.CoordinatorFullName = dbObj.CoordinatorFullName;
            this.OwnerFullName = dbObj.OwnerFullName;
            this.AG1ClientLookupId = dbObj.AG1ClientLookupId;
            this.AG2ClientLookupId = dbObj.AG2ClientLookupId;
            this.AG3ClientLookupId = dbObj.AG3ClientLookupId;
        }
        #region WorkOrder ProjectCostingWO LookupList By Search Criteria
        public List<Project> RetrieveProject_ProjectCostingWorkOrderSearchCriteria(DatabaseKey dbKey)
        {
            Project_RetrieveProjectCostingWorkOrderLookupListBySearchCriteria_V2 trans = new Project_RetrieveProjectCostingWorkOrderLookupListBySearchCriteria_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Project = this.ToDateBaseObjectForRetrieveWorkOrder_ProjectCostingLookupListBySearchCriteria();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Project> projectlist = new List<Project>();

            foreach (b_Project WOP in trans.ProjectList)
            {
                Project tmpWOP = new Project();
                tmpWOP.UpdateFromDatabaseObjectForRetrieveWorkOrder_ProjectCostingLookupListBySearchCriteria(WOP);
                projectlist.Add(tmpWOP);
            }
            return projectlist;
        }
        public b_Project ToDateBaseObjectForRetrieveWorkOrder_ProjectCostingLookupListBySearchCriteria()
        {
            b_Project dbObj = this.ToDatabaseObject();
            dbObj.WorkOrderId = this.WorkOrderId;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;
            dbObj.WorkOrderClientLookupId = this.WorkOrderClientLookupId;
            dbObj.ProjectId = this.ProjectId;
            dbObj.Description = this.Description;
            dbObj.Status = this.Status;
            dbObj.Planner = this.Planner;
            dbObj.PC_WO_CompleteDate = this.PC_WO_CompleteDate;
            dbObj.MaterialCost = this.MaterialCost;
            dbObj.LaborCost = this.LaborCost;
            dbObj.TotalCost = this.TotalCost;
            dbObj.TotalCount = this.TotalCount;
            return dbObj;
        }
        public void UpdateFromDatabaseObjectForRetrieveWorkOrder_ProjectCostingLookupListBySearchCriteria(b_Project dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.WorkOrderId = dbObj.WorkOrderId;
            this.OrderbyColumn = dbObj.OrderbyColumn;
            this.OrderBy = dbObj.OrderBy;
            this.OffSetVal = dbObj.OffSetVal;
            this.NextRow = dbObj.NextRow;
            this.WorkOrderClientLookupId = dbObj.ClientLookupId;
            this.Description = dbObj.Description;
            this.Status = dbObj.Status;
            this.Planner = dbObj.Planner;
            this.CompleteDate = dbObj.CompleteDate;
            this.MaterialCost = dbObj.MaterialCost;
            this.LaborCost = dbObj.LaborCost;
            this.TotalCost = dbObj.TotalCost;
            this.TotalCount = dbObj.TotalCount;
        }
        #endregion
       

        public void RetrieveByProjectIdForPCDashboard_V2(DatabaseKey dbKey)
        {
            Project_RetrieveByProjectIdForPCDashboard_V2 trans = new Project_RetrieveByProjectIdForPCDashboard_V2();
            trans.Project = this.ToDateBaseObjectForRetrieveDashboardForPC_V2();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseForPCDashboard_V2(trans.projectObj);
        }

        public b_Project ToDateBaseObjectForRetrieveDashboardForPC_V2()
        {
            b_Project dbObj = new b_Project();
            dbObj.ProjectId = this.ProjectId;
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            return dbObj;
        }
        public void UpdateFromDatabaseForPCDashboard_V2(b_Project dbObj)
        {
            this.Budget = dbObj.Budget;
            this.MaterialCost = dbObj.MaterialCost;
            this.LaborCost = dbObj.LaborCost;
            this.PurchasingCost = dbObj.PurchasingCost;
            this.Spent = dbObj.Spent;
            this.Remaining = dbObj.Remaining;
            this.SpentPercentage = dbObj.SpentPercentage;
            this.RemainingPercentage = dbObj.RemainingPercentage;
        }


        #region WorkOrder ProjectCosting PurchaseOrder Search Criteria
        public List<Project> RetrieveProject_ProjectCostingPurchaseOrderSearchCriteria(DatabaseKey dbKey)
        {
            Project_RetrieveProjectCostingPurchaseOrderSearchCriteria_V2 trans = new Project_RetrieveProjectCostingPurchaseOrderSearchCriteria_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Project = this.ToDateBaseObjectForRetrievePurchaseOrder_ProjectCostingSearchCriteria();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Project> projectlist = new List<Project>();

            foreach (b_Project WOP in trans.ProjectList)
            {
                Project tmpWOP = new Project();
                tmpWOP.UpdateFromDatabaseObjectForRetrievePurchaseOrder_ProjectCostingSearchCriteria(WOP);
                projectlist.Add(tmpWOP);
            }
            return projectlist;
        }
        public b_Project ToDateBaseObjectForRetrievePurchaseOrder_ProjectCostingSearchCriteria()
        {
            b_Project dbObj = this.ToDatabaseObject();
            dbObj.PurchasOrderId = this.PurchasOrderId;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;
            dbObj.PurchasOrderClientLookupId = this.PurchasOrderClientLookupId;
            dbObj.ProjectId = this.ProjectId;
            dbObj.Line = this.Line;
            dbObj.PartID = this.PartID;
            dbObj.Description = this.Description;
            dbObj.Status = this.Status;
            dbObj.Buyer = this.Buyer;
            dbObj.PC_PO_CompleteDate = this.PC_PO_CompleteDate;
            dbObj.Quantity = this.Quantity;
            dbObj.UnitCost = this.UnitCost;
            dbObj.TotalCost = this.TotalCost;
            dbObj.TotalCount = this.TotalCount;
            return dbObj;
        }
        public void UpdateFromDatabaseObjectForRetrievePurchaseOrder_ProjectCostingSearchCriteria(b_Project dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.WorkOrderId = dbObj.WorkOrderId;
            this.OrderbyColumn = dbObj.OrderbyColumn;
            this.OrderBy = dbObj.OrderBy;
            this.OffSetVal = dbObj.OffSetVal;
            this.NextRow = dbObj.NextRow;
            this.PurchasOrderClientLookupId = dbObj.ClientLookupId;
            this.Line = dbObj.Line;
            this.PartID = dbObj.PartID;
            this.Description = dbObj.Description;
            this.Quantity = dbObj.Quantity;
            this.UnitCost = dbObj.UnitCost;
            this.TotalCost = dbObj.TotalCost;
            this.Status = dbObj.Status;
            this.Buyer = dbObj.Buyer;
            this.PC_PO_CompleteDate = dbObj.PC_PO_CompleteDate;
          
           
            this.TotalCount = dbObj.TotalCount;
        }
        #endregion


        #region Project Costing Chunk Search
        public List<Project> RetrieveProjectCostingChunkSearch(DatabaseKey dbKey)
        {
            Project_ProjectCostingRetrieveChunkSearch trans = new Project_ProjectCostingRetrieveChunkSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Project = this.ToDateBaseObjectForRetrieveProjectCostingChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Project> Projectlist = new List<Project>();

            foreach (b_Project proj in trans.ProjectList)
            {
                Project tmpProj = new Project();
                tmpProj.UpdateFromDatabaseObjectForRetriveProjectCostingAllForSearch(proj);
                Projectlist.Add(tmpProj);
            }
            return Projectlist;
        }
        public b_Project ToDateBaseObjectForRetrieveProjectCostingChunkSearch()
        {
            b_Project dbObj = this.ToDatabaseObject();
            dbObj.CustomQueryDisplayId = this.CustomQueryDisplayId;
            dbObj.SearchText = this.SearchText;
            #region V2-1087
            dbObj.AssignedAssetGroup1 = this.AssignedAssetGroup1;
            dbObj.AssignedAssetGroup2 = this.AssignedAssetGroup2;
            dbObj.AssignedAssetGroup3 = this.AssignedAssetGroup3;
            #endregion
            dbObj.CreateStartDateVw = this.CreateStartDateVw;
            dbObj.CreateEndDateVw = this.CreateEndDateVw;
            dbObj.CompleteStartDateVw = this.CompleteStartDateVw;
            dbObj.CompleteEndDateVw = this.CompleteEndDateVw;
            dbObj.CloseStartDateVw = this.CloseStartDateVw;
            dbObj.CloseEndDateVw = this.CloseEndDateVw;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;

            return dbObj;
        }
        public void UpdateFromDatabaseObjectForRetriveProjectCostingAllForSearch(b_Project dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.CustomQueryDisplayId = dbObj.CustomQueryDisplayId;
            this.SearchText = dbObj.SearchText;
            this.CreateStartDateVw = dbObj.CreateStartDateVw;
            this.CreateEndDateVw = dbObj.CreateEndDateVw;
            this.CompleteStartDateVw = dbObj.CompleteStartDateVw;
            this.CompleteEndDateVw = dbObj.CompleteEndDateVw;
            this.CloseStartDateVw = dbObj.CloseStartDateVw;
            this.CloseEndDateVw = dbObj.CloseEndDateVw;
            this.CreateDate = dbObj.CreateDate;
            this.OrderbyColumn = dbObj.OrderbyColumn;
            this.OrderBy = dbObj.OrderBy;
            this.OffSetVal = dbObj.OffSetVal;
            this.NextRow = dbObj.NextRow;
            this.Budget = dbObj.Budget;
            this.AG1ClientLookupId = dbObj.AG1ClientLookupId;
            this.AG2ClientLookupId = dbObj.AG2ClientLookupId;
            this.AG3ClientLookupId = dbObj.AG3ClientLookupId;
            this.Coordinator = dbObj.Coordinator;
            this.TotalCount = dbObj.TotalCount;
        }

        #endregion
        #endregion

    }
}
