using Admin.BusinessWrapper.Metrics;
using Admin.Common;
using Admin.Models.Dashboard;

using Common.Constants;

using Newtonsoft.Json;

using System.Linq;
using System.Web.Mvc;

namespace Admin.Controllers
{
    public class DashboardController : BaseController
    {
        public ActionResult Dashboard()
        {
            DashBoardVM objDashboardVM = new DashBoardVM();
            var DashboardHoursList = UtilityFunction.DashboardGridDatesList();
            if (DashboardHoursList != null)
            {
                objDashboardVM.DashboardGridHoursList = DashboardHoursList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            LocalizeControls(objDashboardVM, LocalizeResourceSetConstants.DashboardDetails);
            return View("~/Views/Dashboard/Dashboard.cshtml", objDashboardVM);
        }

        #region Retrieve Maintenance Details
        [HttpPost]
        public string GetMetrics_Maintenance(int? draw, int? start, int? length, int duration)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            MetricsWrapper metWrapper = new MetricsWrapper(userData);
            var Maintenance = metWrapper.GetMetricsMaintenanceForAdmin(duration);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = Maintenance.Count();
            totalRecords = Maintenance.Count();
            int initialPage = start.Value;
            var filteredResult = Maintenance
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
        #endregion

        #region Retrieve Inventory Details
        [HttpPost]
        public string GetMetrics_Inventory(int? draw, int? start, int? length)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            MetricsWrapper metWrapper = new MetricsWrapper(userData);
            var Inventory = metWrapper.GetMetricsForInventoryForAdmin();
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = Inventory.Count();
            totalRecords = Inventory.Count();
            int initialPage = start.Value;
            var filteredResult = Inventory
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
        #endregion

        #region Retrieve Purchasing Details
        [HttpPost]
        public string GetMetrics_Purchasing(int? draw, int? start, int? length, int duration)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            MetricsWrapper metWrapper = new MetricsWrapper(userData);
            var Purchasing = metWrapper.GetMetricsForPurchasingForAdmin(duration);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = Purchasing.Count();
            totalRecords = Purchasing.Count();
            int initialPage = start.Value;
            var filteredResult = Purchasing
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
        #endregion
    }
}