using Client.BusinessWrapper;
using Client.BusinessWrapper.ActiveSiteWrapper;
using Client.BusinessWrapper.Configuration.DashBoard;
using Client.Common;
using Client.Controllers.Common;
using Client.Models.Common.Charts;
using Client.Models.Common.Charts.Fusions;
using Client.Models.Configuration;
using Client.Models.Configuration.Dashboard;

using Common.Constants;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using ViewModels;

namespace Client.Controllers.Configuration.Home
{
    public class ConfigurationController : ConfigBaseController
    {
        public ActionResult Dashboard()
        {
            //LoginAuditingRetrieveUserInfoVM objLoginAuditingRetrieveUser = new LoginAuditingRetrieveUserInfoVM();
            ConfigDashboardWrapper wrapper = new ConfigDashboardWrapper(userData);
            ConfigDashboardVM configDashboardVM = new ConfigDashboardVM();

            var client = userData.DatabaseKey.Client;
            var userinfo = userData.DatabaseKey.User;
            //bool IsEnterPrise = userData.DatabaseKey.User.UserType.ToLower() == UserTypeConstants.Enterprise.ToLower();

            //configDashboardVM.IsEnterPrise = IsEnterPrise;
            configDashboardVM.IsSiteControl = client.SiteControl;
            configDashboardVM.IsSiteAdmin = userinfo.IsSiteAdmin;
            configDashboardVM.IsSuperUser = userinfo.IsSuperUser;

            // checking if enterprise
            if (client.SiteControl && userinfo.IsSuperUser)
            {
                var sites = wrapper.GetSites();
                configDashboardVM.SiteList = sites;
            }
            configDashboardVM.DateRangeDropListForActivity = UtilityFunction.GetTimeRangeDropForConfigDashboard().Select(x => new SelectListItem { Text = x.text, Value = x.value });
            configDashboardVM.LoggedInUserSiteId = userData.DatabaseKey.User.DefaultSiteId;
            LocalizeControls(configDashboardVM, LocalizeResourceSetConstants.DashboardConfig);

            return View("~/Views/Configuration/Home/Index.cshtml", configDashboardVM);

        }

        #region LoginAuditing
        [HttpPost]
        public string PopulateLoginAuditing(int? draw, int? start, int? length)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            LoginAuditingRetrieveWrapper loginAuditingRetrieveWrapper = new LoginAuditingRetrieveWrapper(userData);
            var retLoginAuditingValue = loginAuditingRetrieveWrapper.GetLoginAuditingRetrieveByUserInfoId();
            retLoginAuditingValue = this.GetAllLoginAuditingRetrieveSortByColumnWithOrder(order, orderDir, retLoginAuditingValue);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = retLoginAuditingValue.Count();
            totalRecords = retLoginAuditingValue.Count();
            int initialPage = start.Value;
            var filteredResult = retLoginAuditingValue
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();


            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeSettings);

        }
        private List<LoginAuditingRetrieveUserInfoVM> GetAllLoginAuditingRetrieveSortByColumnWithOrder(string order, string orderDir, List<LoginAuditingRetrieveUserInfoVM> data)
        {
            List<LoginAuditingRetrieveUserInfoVM> lst = new List<LoginAuditingRetrieveUserInfoVM>();
            if (data != null)
            {
                switch (order)
                {
                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.UserName).ToList() : data.OrderBy(p => p.UserName).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.LogIn).ToList() : data.OrderBy(p => p.LogIn).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.IPAddress).ToList() : data.OrderBy(p => p.IPAddress).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.UserName).ToList() : data.OrderBy(p => p.UserName).ToList();
                        break;
                }
            }
            return lst;
        }
        #endregion

        #region Availability
        public JsonResult GetAvailabilityChartData(long SiteId = 0)
        {
            ConfigDashboardWrapper wrapper = new ConfigDashboardWrapper(userData);
            UserSeatsModel model = new UserSeatsModel();
            var client = userData.DatabaseKey.Client;
            var userinfo = userData.DatabaseKey.User;
            //bool IsEnterPrise = userData.DatabaseKey.User.UserType.ToLower() == UserTypeConstants.Enterprise.ToLower();

            if(!client.SiteControl && userinfo.IsSuperUser)
            {
                model = wrapper.GetSeatAvailabilityChartDataNonEnterprise();
            }
            else if(userinfo.IsSuperUser || (client.SiteControl && userinfo.IsSiteAdmin))
            {
                if (userinfo.IsSiteAdmin)
                {
                    SiteId = userData.DatabaseKey.User.DefaultSiteId;
                }
                model = wrapper.GetSeatAvailabilityChartDataEnterprise(SiteId);
            }            
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Activity
        public JsonResult GetActivityMultiSeriesData(long SiteId = 0, int CaseNo = 0)
        {
            ConfigDashboardWrapper wrapper = new ConfigDashboardWrapper(userData);
            var client = userData.DatabaseKey.Client;
            var userinfo = userData.DatabaseKey.User;
            multiSeriesLine2dModel seriesDetails = new multiSeriesLine2dModel();
            ActivitydoughnutChartModel doughnutDetails = new ActivitydoughnutChartModel();
            //bool IsEnterPrise = userData.DatabaseKey.User.UserType.ToLower() == UserTypeConstants.Enterprise.ToLower();

            if (userinfo.IsSuperUser || (client.SiteControl && userinfo.IsSiteAdmin))
            {
                if (userinfo.IsSiteAdmin)
                {
                    SiteId = userData.DatabaseKey.User.DefaultSiteId;
                }
                var result = wrapper.GetActivityChartData(SiteId, CaseNo);
                seriesDetails = result.Item1;
                doughnutDetails = result.Item2;
            }
            return Json(new { seriesdata = seriesDetails, doughnutdata = doughnutDetails }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Engagement
        public JsonResult GetActivityEngagementData(long SiteId = 0, int CaseNo = 0)
        {
            ConfigDashboardWrapper wrapper = new ConfigDashboardWrapper(userData);
            var client = userData.DatabaseKey.Client;
            var userinfo = userData.DatabaseKey.User;
            scrollbar2dModel engagementDetails = new scrollbar2dModel();
            //bool IsEnterPrise = userData.DatabaseKey.User.UserType.ToLower() == UserTypeConstants.Enterprise.ToLower();

            if (!client.SiteControl && userinfo.IsSuperUser)
            {
                var result = wrapper.GetEngagementChartData(CaseNo);
                if (result != null)
                {
                    engagementDetails = result;
                }
            }
            return Json(engagementDetails, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}