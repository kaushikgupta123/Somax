using Client.Common;
using Client.Models.Common.Charts;
using Client.Models.Common.Charts.Fusions;
using Client.Models.WorkOrderPlanning;
using Client.Models.WorkOrderSchedule;

using Common.Constants;

using Database.Transactions;

using DataContracts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Client.BusinessWrapper.WorkOrder_Schedule
{
    public class WorkOrderScheduleDashboardWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        public WorkOrderScheduleDashboardWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        #region WorkOrder Schedule ComplianceChartData
        public WorkOrderScheduleCompliancedoughnutChartModel GetWorkOrderScheduleComplianceChartData(long PersonnelId, long CaseNo)
        {
            WorkOrderScheduleCompliancedoughnutChartModel dModel = new WorkOrderScheduleCompliancedoughnutChartModel();
            DataContracts.WorkOrderSchedule workOrderSchedule = new DataContracts.WorkOrderSchedule()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                PersonnelId = PersonnelId,
                CaseNo = CaseNo
            };

            WorkOrderPlan woPlan = new WorkOrderPlan();
            Task[] tasks = new Task[2];
            //var completeCount = "";
            //var inCompleteCount = 0;

            List<doughnut2dChartData> data = new List<doughnut2dChartData>();
            doughnut2dChartData dChartData;
            var completeCount = workOrderSchedule.RetrieveCountWorkorderScheduleByComplete(userData.DatabaseKey);
            var inCompleteCount = workOrderSchedule.RetrieveCountWorkorderScheduleByInComplete(userData.DatabaseKey);
            tasks[0] = Task.Factory.StartNew(() => completeCount); //= workOrderSchedule.RetrieveCountWorkorderScheduleByComplete(userData.DatabaseKey)); ;
            tasks[1] = Task.Factory.StartNew(() => inCompleteCount);
            Task.WaitAll(tasks);

            dModel.info.showpercentvalues = "0";
            dModel.info.showLegend = true;
            dModel.info.numberSuffix = "%";

            dChartData = new doughnut2dChartData();
            dChartData.label = UtilityFunction.GetMessageFromResource("GlobalComplete", LocalizeResourceSetConstants.Global);
            dChartData.value = GetPercentage(Convert.ToInt64(completeCount.Count), Convert.ToInt64(completeCount.Count) + Convert.ToInt64(inCompleteCount.Count));
            dModel.data.Add(dChartData);

            dChartData = new doughnut2dChartData();
            dChartData.label = UtilityFunction.GetMessageFromResource("GlobalIncomplete", LocalizeResourceSetConstants.Global);
            dChartData.value = GetPercentage(Convert.ToInt64(inCompleteCount.Count), Convert.ToInt64(completeCount.Count) + Convert.ToInt64(inCompleteCount.Count));
            dModel.data.Add(dChartData);

            dModel.info.centerLabel = "$value";

            return dModel;
        }
        private string GetPercentage(long number, long total)
        {
            double percentage = 0;
            if (total != 0)
            {
                percentage = ((double)number / (double)total) * 100;
            }
            return percentage.ToString();
        }
        #endregion

    }
}