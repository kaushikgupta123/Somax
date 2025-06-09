using Client.Models.ProjectCosting;

using DataContracts;
using System.Collections.Generic;
namespace Client.BusinessWrapper.ProjectsCosting
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
        public List<KeyValuePair<string, long>> WorkOrderStatusesCountForDashboard_V2(long ProjectId)
        {
            WorkOrder workorder = new WorkOrder()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                ProjectId = ProjectId
            };

            List<KeyValuePair<string, long>> keyValuePairs = workorder.WorkOrderStatusesCountForDashboard_V2(this.userData.DatabaseKey);

            return keyValuePairs;
        }

        public DashboardGridModel RetrieveProjectByProjectIdForWorkOrderCostDetails(long ProjectId)
        {
            DashboardGridModel dashboardGridModel = new DashboardGridModel();
            DashboardSpendingModel dashboardSpendingModel = new DashboardSpendingModel();
            Project objProject = new Project()
            {
                ClientId = _dbKey.Client.ClientId,
                SiteId = _dbKey.User.DefaultSiteId,
                ProjectId = ProjectId
            };
            objProject.RetrieveByProjectIdForPCDashboard_V2(this.userData.DatabaseKey);
            dashboardGridModel.Budget = objProject.Budget;
            dashboardGridModel.MaterialCost = objProject.MaterialCost;
            dashboardGridModel.LaborCost = objProject.LaborCost;
            dashboardGridModel.PurchasingCost = objProject.PurchasingCost;
            dashboardGridModel.Remaining = objProject.Remaining;
            dashboardGridModel.Spent = objProject.Spent;
            dashboardGridModel.SpentPercentage = objProject.SpentPercentage;
            dashboardGridModel.RemainingPercentage = objProject.RemainingPercentage;
            return dashboardGridModel;
        }

        public Dictionary<string, decimal?> GetProjectCostingDashboardGridAsDictionary(DashboardGridModel model)
        {
            var detailsDictionary = new Dictionary<string, decimal?>
    {
        { "Budget", model.Budget },
        { "Material Cost", model.MaterialCost },
        { "Labor Cost", model.LaborCost },
        { "Purchasing", model.PurchasingCost },
        //{ "Spent", model.Spent },
        { "Remaining", model.Remaining }
    };

            return detailsDictionary;
        }

        public Dictionary<string, decimal> GetProjectCostingSpendingAsDictionary(DashboardSpendingModel model)
        {
            var detailsDictionary = new Dictionary<string, decimal>
    {
        { "Spent", model.Spent },
        { "Remaining", model.Remaining }
    };
            return detailsDictionary;
        }
    }
}


