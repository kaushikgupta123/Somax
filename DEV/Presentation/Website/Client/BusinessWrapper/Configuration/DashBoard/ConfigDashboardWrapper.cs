using Client.Common;
using Client.Models;
using Client.Models.Common.Charts;
using Client.Models.Common.Charts.Fusions;
using Client.Models.Configuration.Dashboard;

using Common.Constants;

using DataContracts;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Client.BusinessWrapper.Configuration.DashBoard
{
    public class ConfigDashboardWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;

        public ConfigDashboardWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        #region Availability
        public UserSeatsModel GetSeatAvailabilityChartDataNonEnterprise()
        {
            UserSeatsModel dModel = new UserSeatsModel();
            List<doughnut2dChartData> data = new List<doughnut2dChartData>();
            doughnut2dChartData dChartData;

            //dModel.info.caption = UtilityFunction.GetMessageFromResource("spnScheduleCompliance", LocalizeResourceSetConstants.DashboardConfig);
            dModel.info.showpercentvalues = "1";
            dModel.info.showLegend = true;

            DataContracts.Client client = new DataContracts.Client();
            client.ClientId = userData.DatabaseKey.Client.ClientId;
            client.Retrieve(this.userData.DatabaseKey);

            dModel.TotalSeats = client.MaxAppUsers;
            dModel.SeatsAvailable = client.MaxAppUsers - client.AppUsers;
            dModel.ActiveUser = client.AppUsers;
            dModel.WorkRequestUser = client.WorkRequestUsers;

            dChartData = new doughnut2dChartData();
            dChartData.label = UtilityFunction.GetMessageFromResource("spnActiveUsers", LocalizeResourceSetConstants.DashboardConfig);
            dChartData.value = client.AppUsers.ToString();
            dModel.data.Add(dChartData);

            dChartData = new doughnut2dChartData();
            dChartData.label = UtilityFunction.GetMessageFromResource("spnSeatsAvailability", LocalizeResourceSetConstants.DashboardConfig);
            dChartData.value = dModel.SeatsAvailable.ToString();
            dModel.data.Add(dChartData);

            string percentage = GetAvailabilityPercentage(client.AppUsers, client.MaxAppUsers);
            dModel.info.defaultCenterLabel = percentage;

            return dModel;
        }
        public UserSeatsModel GetSeatAvailabilityChartDataEnterprise(long SiteId)
        {
            UserSeatsModel dModel = new UserSeatsModel();
            List<doughnut2dChartData> data = new List<doughnut2dChartData>();
            doughnut2dChartData dChartData;
            //dModel.info.caption = UtilityFunction.GetMessageFromResource("spnScheduleCompliance", LocalizeResourceSetConstants.DashboardConfig);
            //dModel.info.subCaption = "Schedule Compliance";
            dModel.info.showpercentvalues = "1";
            dModel.info.showLegend = true;

            Site site = new Site();
            site.ClientId = userData.DatabaseKey.Client.ClientId;
            site.SiteId = SiteId;
            site.Retrieve(userData.DatabaseKey);

            dModel.TotalSeats = site.MaxAppUsers;
            dModel.SeatsAvailable = site.MaxAppUsers - site.AppUsers;
            dModel.ActiveUser = site.AppUsers;
            dModel.WorkRequestUser = site.WorkRequestUsers;

            dChartData = new doughnut2dChartData();
            dChartData.label = "Active Users";
            dChartData.value = site.AppUsers.ToString();
            dModel.data.Add(dChartData);

            dChartData = new doughnut2dChartData();
            dChartData.label = "Seats Available";
            dChartData.value = dModel.SeatsAvailable.ToString();
            dModel.data.Add(dChartData);

            string percentage = GetAvailabilityPercentage(site.AppUsers, site.MaxAppUsers);
            dModel.info.defaultCenterLabel = percentage;

            return dModel;
        }
        #endregion

        #region Activity
        public Tuple<multiSeriesLine2dModel, ActivitydoughnutChartModel> GetActivityChartData(long SiteId, int CaseNo)
        {
            multiSeriesLine2dModel seriesChartModel = new multiSeriesLine2dModel();
            ActivitydoughnutChartModel doughnutChartModel = new ActivitydoughnutChartModel();

            GetActivityChartData(SiteId, CaseNo, seriesChartModel, doughnutChartModel);
            return Tuple.Create(seriesChartModel, doughnutChartModel);
        }
        public void GetActivityChartData(long SiteId, int CaseNo, multiSeriesLine2dModel model, ActivitydoughnutChartModel dModel)
        {
            List<KeyValuePair<DateTime, decimal>> loginAuditingSeriesData = new List<KeyValuePair<DateTime, decimal>>();
            List<KeyValuePair<DateTime, decimal>> workOrdersCreatedSeriesData = new List<KeyValuePair<DateTime, decimal>>();
            List<KeyValuePair<DateTime, decimal>> purchaseOrdersCreatedSeriesData = new List<KeyValuePair<DateTime, decimal>>();
            Task[] tasks;

            if (!userData.DatabaseKey.Client.SiteControl && userData.DatabaseKey.User.IsSuperUser)
            {
                tasks = new Task[3];
                tasks[0] = Task.Factory.StartNew(() => loginAuditingSeriesData = GetLoginAuditingSeriesData(SiteId, CaseNo));
                tasks[1] = Task.Factory.StartNew(() => workOrdersCreatedSeriesData = GetMetricsSeriesData(SiteId, CaseNo, "WO_Created"));
                tasks[2] = Task.Factory.StartNew(() => purchaseOrdersCreatedSeriesData = GetMetricsSeriesData(SiteId, CaseNo, "PO_Created"));
            }
            else
            {
                tasks = new Task[2];                
                tasks[0] = Task.Factory.StartNew(() => workOrdersCreatedSeriesData = GetMetricsSeriesData(SiteId, CaseNo, "WO_Created"));
                tasks[1] = Task.Factory.StartNew(() => purchaseOrdersCreatedSeriesData = GetMetricsSeriesData(SiteId, CaseNo, "PO_Created"));
            }
            
            Task.WaitAll(tasks);

            var loginAuditingSeriesDate = loginAuditingSeriesData.Select(x => x.Key).ToList();
            var workOrdersCreatedSeriesDate = workOrdersCreatedSeriesData.Select(x => x.Key).ToList();
            var purchaseOrdersCreatedSeriesDate = purchaseOrdersCreatedSeriesData.Select(x => x.Key).ToList();

            #region MultiSeries
            var finalDates = loginAuditingSeriesDate;
            finalDates.AddRange(workOrdersCreatedSeriesDate);
            finalDates.AddRange(purchaseOrdersCreatedSeriesDate);
            finalDates = finalDates.Distinct().ToList();

            multiSeriesLineCategoryItems Categoryitem;
            multiSeriesLineCategory category = new multiSeriesLineCategory();
            foreach (var item in finalDates)
            {
                Categoryitem = new multiSeriesLineCategoryItems();
                Categoryitem.label = item.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                category.category.Add(Categoryitem);
            }
            model.categories.Add(category);

            var auditingSeriesValues = GetSeriesValues(finalDates, loginAuditingSeriesData);
            var workOrdersCreatedSeriesValues = GetSeriesValues(finalDates, workOrdersCreatedSeriesData);
            var purchaseOrdersCreatedSeriesValues = GetSeriesValues(finalDates, purchaseOrdersCreatedSeriesData);

            multiSeriesLineDataset dataset;

            if (!userData.DatabaseKey.Client.SiteControl && userData.DatabaseKey.User.IsSuperUser)
            {
                dataset = new multiSeriesLineDataset();
                dataset.seriesname = UtilityFunction.GetMessageFromResource("spnLogins", LocalizeResourceSetConstants.DashboardConfig);
                dataset.data = auditingSeriesValues.Select(x => new multiSeriesLineDatum { value = Convert.ToString(x) }).ToList();
                model.dataset.Add(dataset);
            }

            dataset = new multiSeriesLineDataset();
            dataset.seriesname = UtilityFunction.GetMessageFromResource("globalWorkOrdersCreated", LocalizeResourceSetConstants.Global);
            dataset.data = workOrdersCreatedSeriesValues.Select(x => new multiSeriesLineDatum { value = Convert.ToString(x) }).ToList();
            model.dataset.Add(dataset);

            dataset = new multiSeriesLineDataset();
            dataset.seriesname = UtilityFunction.GetMessageFromResource("globalPurchaseOrdersCreated", LocalizeResourceSetConstants.Global);
            dataset.data = purchaseOrdersCreatedSeriesValues.Select(x => new multiSeriesLineDatum { value = Convert.ToString(x) }).ToList();
            model.dataset.Add(dataset);

            model.chart.xAxisName = UtilityFunction.GetMessageFromResource("GlobalDate", LocalizeResourceSetConstants.Global);
            #endregion

            #region doughnut2dChart
            List<doughnut2dChartData> data = new List<doughnut2dChartData>();
            doughnut2dChartData dChartData;

            dModel.info.showLegend = true;

            if (!userData.DatabaseKey.Client.SiteControl && userData.DatabaseKey.User.IsSuperUser)
            {
                dChartData = new doughnut2dChartData();
                dChartData.label = UtilityFunction.GetMessageFromResource("spnLogins", LocalizeResourceSetConstants.DashboardConfig);
                dChartData.value = auditingSeriesValues.Sum().ToString();
                dModel.data.Add(dChartData);
            }

            dChartData = new doughnut2dChartData();
            dChartData.label = UtilityFunction.GetMessageFromResource("globalWorkOrdersCreated", LocalizeResourceSetConstants.Global);
            dChartData.value = workOrdersCreatedSeriesValues.Sum().ToString();
            dModel.data.Add(dChartData);

            dChartData = new doughnut2dChartData();
            dChartData.label = UtilityFunction.GetMessageFromResource("globalPurchaseOrdersCreated", LocalizeResourceSetConstants.Global);
            dChartData.value = purchaseOrdersCreatedSeriesValues.Sum().ToString();
            dModel.data.Add(dChartData);
            #endregion

        }
        private List<decimal> GetSeriesValues(List<DateTime> dates, List<KeyValuePair<DateTime, decimal>> data)
        {
            List<decimal> val = new List<decimal>();
            foreach (var date in dates)
            {
                if (data.Any(x => x.Key == date))
                {
                    val.Add(data.Where(y => y.Key == date).Select(x => x.Value).FirstOrDefault());
                }
                else
                {
                    val.Add(0);
                }
            }
            return val;
        }
        private List<KeyValuePair<DateTime, decimal>> GetLoginAuditingSeriesData(long SiteId, int CaseNo)
        {
            LoginAuditing la = new LoginAuditing();
            la.ClientId = this.userData.DatabaseKey.Client.ClientId;
            la.UserInfoId = this.userData.DatabaseKey.User.UserInfoId;
            la.SiteId = SiteId;
            //la.IsEnterprise = IsEnterPrise;
            la.CaseNo = CaseNo;

            var result = la.RetrieveLoginRecordsCountByCreateDate(userData.DatabaseKey);
            if (result != null)
            {
                return result.Select(x => new KeyValuePair<DateTime, decimal>(x.CreateDate, x.TotalRecords)).ToList();
            }
            else
                return new List<KeyValuePair<DateTime, decimal>>();

        }
        private List<KeyValuePair<DateTime, decimal>> GetMetricsSeriesData(long SiteId, int CaseNo, string MetricName)
        {
            DataContracts.Metrics me = new DataContracts.Metrics()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = SiteId,
                CaseNo = CaseNo,
                //IsEnterprise = IsEnterPrise,
                MetricName = MetricName
            };

            var result = me.RetrieveMetricsValueSumByDataDate(userData.DatabaseKey);
            if (result != null)
            {
                return result.Select(x => new KeyValuePair<DateTime, decimal>(x.DataDate, x.TotalValue)).ToList();
            }
            else
                return new List<KeyValuePair<DateTime, decimal>>();

        }
        #endregion

        #region Engagement
        public scrollbar2dModel GetEngagementChartData(int CaseNo)
        {
            scrollbar2dModel model = new scrollbar2dModel();

            model.chart.XAxisname = UtilityFunction.GetMessageFromResource("spnLogins", LocalizeResourceSetConstants.DashboardConfig);

            LoginAuditing la = new LoginAuditing();
            la.ClientId = this.userData.DatabaseKey.Client.ClientId;
            la.UserInfoId = this.userData.DatabaseKey.User.UserInfoId;
            //la.SiteId = SiteId;
            //la.IsEnterprise = IsEnterPrise;
            la.CaseNo = CaseNo;

            var result = la.RetrieveLoginRecordsCountByUser(userData.DatabaseKey);

            scrollbar2dCategoryItem categoryItem;
            scrollbar2dCategory category = new scrollbar2dCategory();
            foreach (var item in result)
            {
                categoryItem = new scrollbar2dCategoryItem();
                categoryItem.label = item.Name;
                category.category.Add(categoryItem);
            }
            model.categories.Add(category);

            scrollbar2dDataset dataset = new scrollbar2dDataset();
            dataset.data = result.Select(x => new scrollbar2dDatum { value = Convert.ToString(x.LoginCount) }).ToList();
            model.dataset.Add(dataset);

            return model;
        }
        #endregion

        #region Common
        private string GetAvailabilityPercentage(int AppUsers, int MaxAppUsers)
        {
            double percentage = 0;
            if (MaxAppUsers != 0)
            {
                percentage = ((double)AppUsers / (double)MaxAppUsers) * 100;
            }
            return string.Concat(percentage.ToString(), "%");
        }
        public List<SelectListItem> GetSites()
        {
            Site site = new Site();
            site.ClientId = userData.DatabaseKey.Personnel.ClientId;
            site.AuthorizedUser = userData.DatabaseKey.User.UserInfoId;

            var result = site.RetrieveAuthorizedForUser(userData.DatabaseKey);
            if (result != null)
            {
                return result.Select(x => new SelectListItem { Text = x.Name, Value = x.SiteId.ToString() }).ToList();
            }
            return new List<SelectListItem>();
        }
        #endregion
    }
}