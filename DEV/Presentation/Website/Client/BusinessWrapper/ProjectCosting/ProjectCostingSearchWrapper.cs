using Client.Models.Project;
using Client.Models.ProjectCosting;

using Common.Constants;
using DataContracts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Client.BusinessWrapper.ProjectsCosting
{
    public class ProjectCostingSearchWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;

        public ProjectCostingSearchWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        public List<ProjectCostingSearchModel> GetProjectCostingGridData(int CustomQueryDisplayId, string ClientlookupId = "", string Description = "", string CreateStartDateVw = "", string CreateEndDateVw = "",
            string CompleteStartDateVw = "", string CompleteEndDateVw = "", string CloseStartDateVw = "", string CloseEndDateVw = "",
             string Status = "", string orderbycol = "", string orderDir = "", int skip = 0, int length = 0, string searchText = "", string assignedAssetGroup1 = "", string assignedAssetGroup2 = "", string assignedAssetGroup3 = "")
        {
            Project Proj = new Project();
            ProjectCostingSearchModel ProjSerModel;
            List<ProjectCostingSearchModel> ProjModelList = new List<ProjectCostingSearchModel>();

            Proj.ClientId = userData.DatabaseKey.Client.ClientId;
            Proj.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            Proj.CustomQueryDisplayId = CustomQueryDisplayId;
            Proj.ClientLookupId = ClientlookupId;
            Proj.Description = Description;
            Proj.Status = Status;
            Proj.CreateStartDateVw = CreateStartDateVw;
            Proj.CreateEndDateVw = CreateEndDateVw;
            Proj.CompleteStartDateVw = CompleteStartDateVw;
            Proj.CompleteEndDateVw = CompleteEndDateVw;
            Proj.CloseStartDateVw = CloseStartDateVw;
            Proj.CloseEndDateVw = CloseEndDateVw;
            Proj.OrderbyColumn = orderbycol;
            Proj.OrderBy = orderDir;
            Proj.OffSetVal = skip;
            Proj.NextRow = length;
            Proj.SearchText = searchText;
            Proj.AssignedAssetGroup1 = Convert.ToInt64(string.IsNullOrEmpty(assignedAssetGroup1) ? "0" : assignedAssetGroup1);
            Proj.AssignedAssetGroup2 = Convert.ToInt64(string.IsNullOrEmpty(assignedAssetGroup2) ? "0" : assignedAssetGroup2);
            Proj.AssignedAssetGroup3 = Convert.ToInt64(string.IsNullOrEmpty(assignedAssetGroup3) ? "0" : assignedAssetGroup3);
            List<Project> projectList = Proj.RetrieveProjectCostingChunkSearch(this.userData.DatabaseKey);

            if (projectList != null)
            {
                foreach (var item in projectList)
                {
                    ProjSerModel = new ProjectCostingSearchModel();
                    ProjSerModel.ProjectId = item.ProjectId;
                    ProjSerModel.ClientlookupId = item.ClientLookupId;
                    ProjSerModel.Description = item.Description;
                    if (item.ActualStart != null && item.ActualStart == default(DateTime))
                    {
                        ProjSerModel.ActualStart = null;
                    }
                    else
                    {
                        ProjSerModel.ActualStart = item.ActualStart;
                    }

                    if (item.ActualFinish != null && item.ActualFinish == default(DateTime))
                    {
                        ProjSerModel.ActualFinish = null;
                    }
                    else
                    {
                        ProjSerModel.ActualFinish = item.ActualFinish;
                    }
                    ProjSerModel.Status = item.Status;
                    if (item.CreateDate != null && item.CreateDate == default(DateTime))
                    {
                        ProjSerModel.Created = null;
                    }
                    else
                    {
                        ProjSerModel.Created = item.CreateDate;
                    }
                    if (item.CompleteDate != null && item.CompleteDate == default(DateTime))
                    {
                        ProjSerModel.CompleteDate = null;
                    }
                    else
                    {
                        ProjSerModel.CompleteDate = item.CompleteDate;
                    }
                    ProjSerModel.Budget = item.Budget;
                    ProjSerModel.AG1ClientLookupId = item.AG1ClientLookupId;
                    ProjSerModel.AG2ClientLookupId = item.AG2ClientLookupId;
                    ProjSerModel.AG3ClientLookupId = item.AG3ClientLookupId;
                    ProjSerModel.Coordinator = item.Coordinator;
                    ProjSerModel.TotalCount = item.TotalCount;
                    ProjModelList.Add(ProjSerModel);
                }
            }
            return ProjModelList;
        }

        internal List<ProjectCostingTaskModel> PopulateLineitems(long ProjectId)
        {
            ProjectCostingTaskModel objLineItem;
            List<ProjectCostingTaskModel> LineItemList = new List<ProjectCostingTaskModel>();

            ProjectTask woPlanLineItem = new ProjectTask()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ProjectId = ProjectId
            };
            List<ProjectTask> woplanLintItemList = woPlanLineItem.RetrieveProjectTask_ByProjectId(this.userData.DatabaseKey);

            if (woplanLintItemList != null)
            {
                foreach (var item in woplanLintItemList)
                {
                    objLineItem = new ProjectCostingTaskModel();
                    objLineItem.ProjectTaskId = item.ProjectTaskId;
                    objLineItem.WorkOrderClientLookupId = item.WorkOrderClientlookupId;
                    objLineItem.WorkOrderDescription = item.WorkOrderDescription;
                    if (item.StartDate == null || item.StartDate == default(DateTime))
                    {
                        objLineItem.StartDate = null;
                    }
                    else
                    {
                        objLineItem.StartDate = item.StartDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    }
                    if (item.EndDate == null || item.EndDate == default(DateTime))
                    {
                        objLineItem.Enddate = null;
                    }
                    else
                    {
                        objLineItem.Enddate = item.EndDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    }
                    objLineItem.Progress = item.Progress;
                    objLineItem.ProgressPercentage = Convert.ToInt32(item.Progress * 100);

                    LineItemList.Add(objLineItem);
                }
            }
            return LineItemList;
        }
        public List<AssetGroup1Model> GetAssetGroup1Dropdowndata()
        {
            AssetGroup1 assetGroup1 = new AssetGroup1()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.User.DefaultSiteId,
                InactiveFlag = false
            };
            var retData = assetGroup1.RetrieveAssetGroup1ByByInActiveFlag_V2(this.userData.DatabaseKey);
            AssetGroup1Model assetGroup1Model;
            List<AssetGroup1Model> AssetGroup1ModelList = new List<AssetGroup1Model>();
            foreach (var item in retData)
            {
                assetGroup1Model = new AssetGroup1Model();
                assetGroup1Model.AssetGroup1Id = item.AssetGroup1Id;
                assetGroup1Model.AssetGroup1DescWithClientLookupId = item.ClientLookup_Desc;
                AssetGroup1ModelList.Add(assetGroup1Model);
            }
            return AssetGroup1ModelList;
        }
        public List<AssetGroup2Model> GetAssetGroup2Dropdowndata()
        {
            AssetGroup2 assetGroup2 = new AssetGroup2()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.User.DefaultSiteId,
                InactiveFlag = false
            };
            var retData = assetGroup2.RetrieveAssetGroup2ByByInActiveFlag_V2(this.userData.DatabaseKey);
            AssetGroup2Model assetGroup2Model;
            List<AssetGroup2Model> AssetGroup2ModelList = new List<AssetGroup2Model>();
            foreach (var item in retData)
            {
                assetGroup2Model = new AssetGroup2Model();
                assetGroup2Model.AssetGroup2Id = item.AssetGroup2Id;
                assetGroup2Model.AssetGroup2DescWithClientLookupId = item.ClientLookup_Desc;
                AssetGroup2ModelList.Add(assetGroup2Model);
            }
            return AssetGroup2ModelList;
        }
        public List<AssetGroup3Model> GetAssetGroup3Dropdowndata()
        {
            AssetGroup3 assetGroup3 = new AssetGroup3()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.User.DefaultSiteId,
                InactiveFlag = false
            };
            var retData = assetGroup3.RetrieveAssetGroup3ByByInActiveFlag_V2(this.userData.DatabaseKey);
            AssetGroup3Model assetGroup3Model;
            List<AssetGroup3Model> AssetGroup3ModelList = new List<AssetGroup3Model>();
            foreach (var item in retData)
            {
                assetGroup3Model = new AssetGroup3Model();
                assetGroup3Model.AssetGroup3Id = item.AssetGroup3Id;
                assetGroup3Model.AssetGroup3DescWithClientLookupId = item.ClientLookup_Desc;
                AssetGroup3ModelList.Add(assetGroup3Model);
            }
            return AssetGroup3ModelList;
        }
       
    }
}