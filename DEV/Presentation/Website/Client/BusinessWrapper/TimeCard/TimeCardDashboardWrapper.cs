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

namespace Client.BusinessWrapper.TimeCard
{
    public class TimeCardDashboardWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;

        public TimeCardDashboardWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        public scrollbar2dModel GetTimeCardLabourHoursChartData(long PersonnelId)
        {
            scrollbar2dModel model = new scrollbar2dModel();

            model.chart.XAxisname = UtilityFunction.GetMessageFromResource("Labour Hours", LocalizeResourceSetConstants.DashboardConfig);
            Timecard timecard = new Timecard()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                PersonnelId = PersonnelId                
            };            

            var result = timecard.RetrieveSumOfLabourHours(userData.DatabaseKey);

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
            dataset.data = result.Select(x => new scrollbar2dDatum { value = Convert.ToString(x.TotalCount) }).ToList();
            model.dataset.Add(dataset);

            return model;
        }
    }
}