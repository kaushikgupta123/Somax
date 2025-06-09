using Client.Common;
using Client.Models.Common.Charts;
using Client.Models.Common.Charts.Fusions;
using Client.Models.WorkOrderPlanning;

using Common.Constants;

using Database.Transactions;

using DataContracts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Client.BusinessWrapper.WorkOrderPlanning
{
    public class WorkOrderPlanningDashboardWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        public WorkOrderPlanningDashboardWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }
        public List<KeyValuePair<string, long>> WorkOrderPlanningLineItemStatuses(long workOrderPlanID)
        {
            WorkOrderPlan woPlan = new WorkOrderPlan()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                WorkOrderPlanId = workOrderPlanID
            };       
            List<KeyValuePair<string, long>> keyValuePairs = woPlan.WorkOrderPlanningLineItemStatuses(userData.DatabaseKey);

            return keyValuePairs;
        }    

        #region Schedule ComplianceChartData
        public ScheduleCompliancedoughnutChartModel GetScheduleComplianceChartData(long WorkOrderPlanId)
        {
            ScheduleCompliancedoughnutChartModel dModel = new ScheduleCompliancedoughnutChartModel();
            WorkOrderPlan woPlan = new WorkOrderPlan();
            Task[] tasks = new Task[2];
            long completeCount = 0;
            long inCompleteCount = 0;

            List<doughnut2dChartData> data = new List<doughnut2dChartData>();
            doughnut2dChartData dChartData;

            tasks[0] = Task.Factory.StartNew(() => completeCount = woPlan.RetrieveCountPlannedWorkorderByComplete(userData.DatabaseKey, WorkOrderPlanId));
            tasks[1] = Task.Factory.StartNew(() => inCompleteCount = woPlan.RetrieveCountPlannedWorkorderByInComplete(userData.DatabaseKey, WorkOrderPlanId));
            Task.WaitAll(tasks);

            dModel.info.showpercentvalues = "0";
            dModel.info.showLegend = true;
            dModel.info.numberSuffix = "%";

            dChartData = new doughnut2dChartData();
            dChartData.label = UtilityFunction.GetMessageFromResource("GlobalComplete", LocalizeResourceSetConstants.Global);
            dChartData.value = GetPercentage(completeCount, completeCount + inCompleteCount);
            dModel.data.Add(dChartData);

            dChartData = new doughnut2dChartData();
            dChartData.label = UtilityFunction.GetMessageFromResource("GlobalIncomplete", LocalizeResourceSetConstants.Global);
            dChartData.value = GetPercentage(inCompleteCount, completeCount + inCompleteCount);
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

        #region Work Orders by Type
        public multiSeriesLine2dModel GetWorkOrdersByTypeChartData(long workOrderPlanID)
        {
            multiSeriesLine2dModel model = new multiSeriesLine2dModel();
            List<WorkOrderPlan> completeData = new List<WorkOrderPlan>();
            List<WorkOrderPlan> inCompleteData = new List<WorkOrderPlan>();

            WorkOrderPlan woPlan = new WorkOrderPlan()
            {
                WorkOrderPlanId = workOrderPlanID,
                SiteId= userData.DatabaseKey.User.DefaultSiteId
            };

            Task[] tasks = new Task[2];
            tasks[0] = Task.Factory.StartNew(() => completeData = woPlan.RetrieveCompleteWorkorderByType(userData.DatabaseKey));
            tasks[1] = Task.Factory.StartNew(() => inCompleteData = woPlan.RetrieveIncompleteWorkorderByType(userData.DatabaseKey));
            Task.WaitAll(tasks);

            var allTypes = completeData.Select(x=>x.Type).ToList();
            allTypes.AddRange(inCompleteData.Select(x => x.Type).ToList());
            allTypes = allTypes.Distinct().ToList();

            multiSeriesLineCategoryItems Categoryitem;
            multiSeriesLineCategory category = new multiSeriesLineCategory();
            foreach (var item in allTypes)
            {
                Categoryitem = new multiSeriesLineCategoryItems();
                Categoryitem.label = item;
                category.category.Add(Categoryitem);
            }
            model.categories.Add(category);

            var completeSeriesValues = GetSeriesValuesForWorkOrdersByType(allTypes, completeData);
            var inCompleteSeriesValues = GetSeriesValuesForWorkOrdersByType(allTypes, inCompleteData);

            multiSeriesLineDataset dataset;

            dataset = new multiSeriesLineDataset();
            dataset.seriesname = UtilityFunction.GetMessageFromResource("GlobalComplete", LocalizeResourceSetConstants.Global);
            dataset.data = completeSeriesValues.Select(x => new multiSeriesLineDatum { value = Convert.ToString(x.value) }).ToList();
            model.dataset.Add(dataset);

            dataset = new multiSeriesLineDataset();
            dataset.seriesname = UtilityFunction.GetMessageFromResource("GlobalIncomplete", LocalizeResourceSetConstants.Global);
            dataset.data = inCompleteSeriesValues.Select(x => new multiSeriesLineDatum { value = Convert.ToString(x.value) }).ToList();
            model.dataset.Add(dataset);

            return model;
        }
        private List<overlappingChartDatum> GetSeriesValuesForWorkOrdersByType(List<string> types, List<WorkOrderPlan> s)
        {
            List<overlappingChartDatum> data = new List<overlappingChartDatum>();
            overlappingChartDatum chartDatum;

            foreach (var item in types)
            {
                chartDatum = new overlappingChartDatum();
                if (s.Any(x => x.Type == item))
                {
                    chartDatum.value = s.Where(p => p.Type == item).Select(x => x.TotalCount).FirstOrDefault().ToString();
                }
                else
                {
                    chartDatum.value = "0";
                }
                data.Add(chartDatum);
            }

            return data;
        }

        #endregion

        #region Assigned
        public overlapping2dModel GetWorkOrdersByAssignedChart(long workOrderPlanID)
        {
            WorkOrderPlan woPlan = new WorkOrderPlan();
            List<WorkOrderPlan> completeData = new List<WorkOrderPlan>();
            List<WorkOrderPlan> inCompleteData = new List<WorkOrderPlan>();
            List<long> finalCategories = new List<long>();

            overlapping2dModel model = new overlapping2dModel();
            overlappingChartCategory chartCategory = new overlappingChartCategory();

            woPlan.WorkOrderPlanId = workOrderPlanID;
            woPlan.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            Task[] tasks = new Task[2];
            tasks[0] = Task.Factory.StartNew(() => completeData = woPlan.RetrieveChartCompleteWorkorderByAssigned(userData.DatabaseKey));
            tasks[1] = Task.Factory.StartNew(() => inCompleteData = woPlan.RetrieveChartIncompleteWorkorderByAssigned(userData.DatabaseKey));
            Task.WaitAll(tasks);

            if (completeData != null)
            {
                completeData = completeData.OrderBy(x => x.PersonnelId).ToList();
            }
            if (inCompleteData != null)
            {
                inCompleteData = inCompleteData.OrderBy(x => x.PersonnelId).ToList();
            }

            finalCategories = completeData.Select(x => x.PersonnelId).ToList();
            finalCategories.AddRange(inCompleteData.Select(x => x.PersonnelId).ToList());
            finalCategories = finalCategories.Distinct().ToList();

            var categories = GetCategories(finalCategories, completeData, inCompleteData);
            chartCategory.category = categories;

            model.categories.Add(chartCategory);

            overlappingChartDatum chartDatum = new overlappingChartDatum();
            overlappingChartDataset chartDataset;

            chartDataset = new overlappingChartDataset();
            chartDataset.seriesname = UtilityFunction.GetMessageFromResource("GlobalComplete", LocalizeResourceSetConstants.Global);
            chartDataset.data = GetSeriesValuesForWorkOrdersbyAssigned(finalCategories, completeData);
            model.dataset.Add(chartDataset);

            chartDataset = new overlappingChartDataset();
            chartDataset.seriesname = UtilityFunction.GetMessageFromResource("GlobalIncomplete", LocalizeResourceSetConstants.Global);
            chartDataset.data = GetSeriesValuesForWorkOrdersbyAssigned(finalCategories, inCompleteData);
            model.dataset.Add(chartDataset);

            return model;
        }
        public overlapping2dModel GetHoursByAssignedChart(long workOrderPlanID)
        {
            WorkOrderPlan woPlan = new WorkOrderPlan();
            List<WorkOrderPlan> estimatedData = new List<WorkOrderPlan>();
            List<WorkOrderPlan> actualData = new List<WorkOrderPlan>();
            List<long> finalCategories = new List<long>();

            overlapping2dModel model = new overlapping2dModel();
            overlappingChartCategory chartCategory = new overlappingChartCategory();

            woPlan.WorkOrderPlanId = workOrderPlanID;
            woPlan.SiteId = userData.DatabaseKey.User.DefaultSiteId;

            Task[] tasks = new Task[2];
            tasks[0] = Task.Factory.StartNew(() => estimatedData = woPlan.RetrieveWorkOrderPlanningChartEstimatedHoursByAssigned(userData.DatabaseKey));
            tasks[1] = Task.Factory.StartNew(() => actualData = woPlan.RetrieveWorkOrderPlanningChartActualHoursByAssigned(userData.DatabaseKey));
            Task.WaitAll(tasks);

            if (estimatedData != null)
            {
                estimatedData = estimatedData.OrderBy(x => x.PersonnelId).ToList();
            }
            if (actualData != null)
            {
                actualData = actualData.OrderBy(x => x.PersonnelId).ToList();
            }

            finalCategories = estimatedData.Select(x => x.PersonnelId).ToList();
            finalCategories.AddRange(actualData.Select(x => x.PersonnelId).ToList());
            finalCategories = finalCategories.Distinct().ToList();

            var categories = GetCategories(finalCategories, estimatedData, actualData);
            chartCategory.category = categories;

            model.categories.Add(chartCategory);

            overlappingChartDatum chartDatum = new overlappingChartDatum();
            overlappingChartDataset chartDataset;

            chartDataset = new overlappingChartDataset();
            chartDataset.seriesname = UtilityFunction.GetMessageFromResource("WorkorderEstimatedChartSeriesName", LocalizeResourceSetConstants.WorkOrderPlanning);
            chartDataset.data = GetSeriesValuesForHoursbyAssigned(finalCategories, estimatedData);
            model.dataset.Add(chartDataset);

            chartDataset = new overlappingChartDataset();
            chartDataset.seriesname = UtilityFunction.GetMessageFromResource("WorkorderActualChartSeriesName", LocalizeResourceSetConstants.WorkOrderPlanning);
            chartDataset.data = GetSeriesValuesForHoursbyAssigned(finalCategories, actualData);
            model.dataset.Add(chartDataset);

            return model;
        }

        private List<overlappingChartCategoryItem> GetCategories(List<long> personnels, List<WorkOrderPlan> s1, List<WorkOrderPlan> s2)
        {
            overlappingChartCategoryItem chartCategoryItem;
            List<overlappingChartCategoryItem> items = new List<overlappingChartCategoryItem>();

            foreach (var item in personnels)
            {
                chartCategoryItem = new overlappingChartCategoryItem();

                if (s1.Any(x => x.PersonnelId == item))
                {
                    chartCategoryItem.label = s1.Where(p => p.PersonnelId == item).Select(x => x.PersonnelName).FirstOrDefault();
                }
                else
                {
                    chartCategoryItem.label = s2.Where(p => p.PersonnelId == item).Select(x => x.PersonnelName).FirstOrDefault();
                }
                items.Add(chartCategoryItem);
            }

            return items;
        }
        private List<overlappingChartDatum> GetSeriesValuesForWorkOrdersbyAssigned(List<long> personnels, List<WorkOrderPlan> s)
        {
            List<overlappingChartDatum> data = new List<overlappingChartDatum>();
            overlappingChartDatum chartDatum;

            foreach (var item in personnels)
            {
                chartDatum = new overlappingChartDatum();
                if (s.Any(x => x.PersonnelId == item))
                {
                    chartDatum.value = s.Where(p => p.PersonnelId == item).Select(x => x.TotalCount).FirstOrDefault().ToString();
                }
                else
                {
                    chartDatum.value = "0";
                }
                data.Add(chartDatum);
            }

            return data;
        }
        private List<overlappingChartDatum> GetSeriesValuesForHoursbyAssigned(List<long> personnels, List<WorkOrderPlan> s)
        {
            List<overlappingChartDatum> data = new List<overlappingChartDatum>();
            overlappingChartDatum chartDatum;

            foreach (var item in personnels)
            {
                chartDatum = new overlappingChartDatum();
                if (s.Any(x => x.PersonnelId == item))
                {
                    chartDatum.value = s.Where(p => p.PersonnelId == item).Select(x => x.Total).FirstOrDefault().ToString();
                }
                else
                {
                    chartDatum.value = "0";
                }
                data.Add(chartDatum);
            }

            return data;
        }
        #endregion


        public List<KeyValuePair<string, decimal>> WorkOrderPlanningEstimateHours(long workOrderPlanID)
        {
            WorkOrderPlan woPlan = new WorkOrderPlan()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                WorkOrderPlanId = workOrderPlanID
            };
            List<KeyValuePair<string, decimal>> keyValuePairs =  woPlan.WorkOrderPlanningEstimateHours(userData.DatabaseKey);
            return keyValuePairs;
        }

      
    }
}