using Client.BusinessWrapper;
using Client.BusinessWrapper.Common;
using Client.BusinessWrapper.Configuration.ClientSetUp;
using Client.BusinessWrapper.EventInfo;
using Client.BusinessWrapper.FleetIssue;
using Client.BusinessWrapper.FleetService;
using Client.BusinessWrapper.InventoryCheckout;
using Client.BusinessWrapper.Metrics;
using Client.BusinessWrapper.PartLookup;
using Client.BusinessWrapper.Projects;
using Client.BusinessWrapper.ScheduledService;
using Client.BusinessWrapper.TimeCard;
using Client.BusinessWrapper.Work_Order;
using Client.BusinessWrapper.WorkOrder_Schedule;
using Client.Common;
using Client.Controllers.Common;
using Client.Localization;
using Client.Models;
using Client.Models.Common;
using Client.Models.Common.Charts.Fusions;
using Client.Models.Configuration.CategoryMaster;
using Client.Models.Configuration.ClientSetUp;
using Client.Models.Dashboard;
using Client.Models.PartLookup;
using Client.Models.Sanitation;
using Client.Models.Work_Order;
using Client.Models.Work_Order.UIConfiguration;
using Client.Models.WorkOrder;
using Client.Models.WorkOrderSchedule;
using Client.ViewModels;

using Common.Constants;

using Database.Business;

using DataContracts;

using INTDataLayer.BAL;

using Newtonsoft.Json;


using QRCoder;


using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

using static Client.Models.Common.UserMentionDataModel;

namespace Client.Controllers
{
    public class DashboardController : SomaxBaseController
    {
        #region Task-Initialization
        Task t1;
        Task t2;
        Task t3;
        Task t4;
        Task t5;
        Task t6;
        Task t7;
        Task t8;
        Task t9;
        Task t10;
        Task t11;
        Task t12;
        Task t13;
        #endregion

        #region Page-Loading
        public ActionResult Dashboard()
        {
            DashboardVM objDashboardVM = new DashboardVM();
            objDashboardVM.WoSecurity = userData.Security.WorkOrders.Access;
            objDashboardVM.PartSecurity = userData.Security.Parts.Access;
            //---V2-375------
            objDashboardVM.UseVendorMaster = userData.Site.UseVendorMaster;
            objDashboardVM.VendorMaster_AllowLocal = userData.Site.VendorMaster_AllowLocal;  /*V2-375*/
            //---------------


            //LocalizeControls(objDashboardVM, LocalizeResourceSetConstants.DashboardDetails);

            #region Maintenance Technician
            objDashboardVM.DateRangeDropListForActivity = UtilityFunction.GetTimeRangeDropForConfigDashboard().Take(2).Select(x => new SelectListItem { Text = x.text, Value = x.value });
            #endregion

            bool isRequestforDashboardCustomize = true; //V2-552
            if (userData.DatabaseKey.User.UserType.ToLower() == UserTypeConstants.WorkRequest.ToLower())
            {
                isRequestforDashboardCustomize = false;
            }
            if (isRequestforDashboardCustomize) //V2-552
            {
                #region WorkRequestOnly

                //if (userData.DatabaseKey.User.UserType.ToLower() == UserTypeConstants.WorkRequest.ToLower())
                //{
                //    DashboardWROnlyVM objDashboardWROnlyVM = new DashboardWROnlyVM();
                //    CommonWrapper commonWrapper = new CommonWrapper(userData);
                //    if (userData.Site.CMMS == true)
                //    {
                //        objDashboardWROnlyVM.ScheduleWorkList = commonWrapper.PopulateCustomQueryDisplay(AttachmentTableConstant.WorkRequest, true);
                //    }
                //    if (userData.Site.Sanitation == true)
                //    {
                //        objDashboardWROnlyVM.SanitationList = commonWrapper.PopulateCustomQueryDisplay(AttachmentTableConstant.WRSanitationJob, true);
                //    }
                //    LocalizeControls(objDashboardWROnlyVM, LocalizeResourceSetConstants.DashboardDetails);
                //    return View("~/Views/Dashboard/WorkRequestOnly/DashboardWorkRequestOnly.cshtml", objDashboardWROnlyVM);
                //}
                #endregion
                //-- For new multiple deashboard system
                string mode = Convert.ToString(TempData["Mode"]);
                long DLId = 0;
                if (mode == "RedirectFromDashboardChange")
                {
                    DLId = Convert.ToInt64(TempData["DashboardListingId"]);
                }
                objDashboardVM = GetWidgetlist(objDashboardVM, DLId);
                return View("~/Views/Dashboard/GetDashboardlist.cshtml", objDashboardVM);
                //-- For new multiple deashboard system
            }
            else
            {

                #region WorkRequestOnly
                // Work Request User - Security Profile is not == 9 in V2
                //if (userData.DatabaseKey.User.UserType.ToLower() == UserTypeConstants.WorkRequest.ToLower() && userData.DatabaseKey.User.SecurityProfileId == 9)
                if (userData.DatabaseKey.User.UserType.ToLower() == UserTypeConstants.WorkRequest.ToLower())
                {
                    DashboardWROnlyVM objDashboardWROnlyVM = new DashboardWROnlyVM();
                    CommonWrapper commonWrapper = new CommonWrapper(userData);
                    WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
                    SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);

                    if (userData.Site.CMMS == true)
                    {
                        //var schduleWorkList = commonWrapper.populateWRListDetails("WorkRequest");

                        //if (schduleWorkList != null)
                        //{
                        //    objDashboardWROnlyVM.ScheduleWorkList = schduleWorkList.Select(x => new SelectListItem { Text = x.Key, Value = x.Value });
                        //}
                        objDashboardWROnlyVM.ScheduleWorkList = commonWrapper.PopulateCustomQueryDisplay(AttachmentTableConstant.WorkRequest, true);
                    }
                    if (userData.Site.Sanitation == true)
                    {
                        //var schduleJobList = commonWrapper.populateWRListDetails1(AttachmentTableConstant.WRSanitationJob, true);
                        //if (schduleJobList != null)
                        //{
                        //    objDashboardWROnlyVM.SanitationList = schduleJobList.Select(x => new SelectListItem { Text = x.Key, Value = x.Value }).OrderBy(x => x.Text);
                        //}
                        objDashboardWROnlyVM.SanitationList = commonWrapper.PopulateCustomQueryDisplay(AttachmentTableConstant.WRSanitationJob, true);
                    }
                    LocalizeControls(objDashboardWROnlyVM, LocalizeResourceSetConstants.DashboardDetails);
                    return View("~/Views/Dashboard/WorkRequestOnly/DashboardWorkRequestOnly.cshtml", objDashboardWROnlyVM);
                }
                #endregion
                #region Enterprise Only

                else if (userData.DatabaseKey.User.UserType.ToLower() == UserTypeConstants.Enterprise.ToLower())
                {
                    var EnterpriseUserHoursList = UtilityFunction.EnterpriseUserDatesList();
                    if (EnterpriseUserHoursList != null)
                    {
                        objDashboardVM.EnterpriseUserHoursList = EnterpriseUserHoursList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                    }
                    return View("~/Views/Dashboard/EnterpriseOnly/EnterpriseOnly.cshtml", objDashboardVM);
                }
                #endregion
                #region SanitationOnly
                else if (userData.Site.Sanitation == true && userData.Site.CMMS == false && userData.Site.APM == false)
                {

                    var SanitationOnlyDropList = UtilityFunction.SanitationOnlyChartDatesList();
                    if (SanitationOnlyDropList != null)
                    {
                        objDashboardVM.SanitationDropList = SanitationOnlyDropList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                    }
                    return View("~/Views/Dashboard/SanitationOnly/SanitationOnly.cshtml", objDashboardVM);
                }
                #endregion
                #region DashboardAPMOnly
                else if (userData.Site.APM == true && userData.Site.CMMS == false && userData.Site.Sanitation == false)
                {
                    var APMOnlyDropList = UtilityFunction.APMChartDatesList();
                    if (APMOnlyDropList != null)
                    {
                        objDashboardVM.APMDropList = APMOnlyDropList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                    }
                    return View("DashboardAPMOnly", objDashboardVM);
                }
                #endregion
                #region Fleet Only

                else if (userData.Site.Fleet == true && userData.Site.CMMS == false && userData.Site.APM == false && userData.Site.Sanitation == false)
                {
                    var soLaborHoursList = UtilityFunction.SoLaborHoursDatesList();
                    if (soLaborHoursList != null)
                    {
                        objDashboardVM.SoLaborHoursList = soLaborHoursList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                    }
                    return View("~/Views/Dashboard/FleetOnly/FleetOnly.cshtml", objDashboardVM);
                }
                #endregion

                else
                {
                    //var equipmentDownTimeList = UtilityFunction.DBEquipmentDownTimeDatesList();
                    //if (equipmentDownTimeList != null)
                    //{
                    //    objDashboardVM.EquipmentDownTimeDateList = equipmentDownTimeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                    //}

                    var woSourceTimeList = UtilityFunction.WOSourceTypeTimeDatesList();
                    if (woSourceTimeList != null)
                    {
                        objDashboardVM.WoSourceTypeDateList = woSourceTimeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                    }

                    var woByTypeTimeList = UtilityFunction.WOByTypeDatesList();
                    if (woByTypeTimeList != null)
                    {
                        objDashboardVM.WoByTypeDateList = woByTypeTimeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                    }

                    var woByPriorityTimeList = UtilityFunction.WoByPriorityDatesList();
                    if (woByPriorityTimeList != null)
                    {
                        objDashboardVM.WoByPriorityDateList = woByPriorityTimeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                    }

                    var woSourceDurationList = UtilityFunction.WoSourceDatesList();
                    if (woSourceDurationList != null)
                    {
                        objDashboardVM.WoSourceList = woSourceDurationList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                    }

                    var woLaborHoursList = UtilityFunction.WoLaborHoursDatesList();
                    if (woLaborHoursList != null)
                    {
                        objDashboardVM.WoLaborHoursList = woLaborHoursList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                    }
                    return View(objDashboardVM);
                }

            }




        }
        #endregion

        #region Charts
        [HttpGet]
        public ActionResult EquipMentChartData(int timeframe = 1)
        {
            DataTable equipment_chartdata = new DataTable();
            var dataModel = new VMDashboardEquipChart();
            string color = "#34bfa3";
            List<string> ColorList = new List<string>();
            Chart _chart = new Chart();

            t1 = Task.Factory.StartNew(() => equipment_chartdata = DashboardReports.Equipment_RetrieveDownTimeforChart(userData.DatabaseKey, timeframe));
            t1.Wait();
            IList<VMDashboardEquipChart> itemsEquipChart = equipment_chartdata.AsEnumerable().Select(row =>
                    new VMDashboardEquipChart
                    {
                        EQNAME = row.Field<string>("EQNAME"),
                        MINS = row.Field<long>("MINS")
                    }).ToList();

            if (itemsEquipChart != null && itemsEquipChart.Count > 0)
            {
                dataModel.VMDashboardEquipCharts = itemsEquipChart.ToList();
                _chart.labels = dataModel.VMDashboardEquipCharts.Select(x => x.EQNAME).ToArray();
                _chart.datasets = new List<Datasets>();
                List<Datasets> _dataSet = new List<Datasets>();
                _dataSet.Add(new Datasets()
                {
                    data = dataModel.VMDashboardEquipCharts.Select(x => x.MINS).ToArray(),
                });
                if (_dataSet != null)
                {
                    for (int dataCount = 0; dataCount < _dataSet[0].data.Count(); dataCount++)
                    {
                        ColorList.Add(color);
                    }
                }

                _chart.datasets = _dataSet;
            }
            return Json(_chart, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult EquipMentChartDataNew(int timeframe = 1)
        {
            DataTable equipment_chartdata = new DataTable();
            var dataModel = new VMDashboardEquipChart();
            List<ChartFusion> _chart = new List<ChartFusion>();

            t1 = Task.Factory.StartNew(() => equipment_chartdata = DashboardReports.Equipment_RetrieveDownTimeforChart(userData.DatabaseKey, timeframe));
            t1.Wait();
            _chart = equipment_chartdata.AsEnumerable().Select(row =>
                    new ChartFusion
                    {
                        label = row.Field<string>("EQNAME"),
                        value = row.Field<long>("MINS")
                    }).ToList();

            return Json(_chart, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult WOSourceTypeData(int timeframe = 2)
        {
            List<KeyValuePair<string, List<b_WorkOrder>>> entries = new List<KeyValuePair<string, List<b_WorkOrder>>>();
            List<long> Series1Data = new List<long>();
            List<long> Series2Data = new List<long>();
            List<string> SourceType = new List<string>();
            long thisCountAsStatus = 0;
            long thisCountAsType = 0;
            t2 = Task.Factory.StartNew(() => entries = DashboardReports.WorkOrder_RetrieveByWOCountBForChart(userData.DatabaseKey, timeframe));
            t2.Wait();
            var series1 = entries.Where(x => x.Key.Equals("Series1")).ToList();
            var series2 = entries.Where(x => x.Key.Equals("Series2")).ToList();
            foreach (var s in series1)
            {
                foreach (var v in s.Value)
                {
                    SourceType.Add(v.SourceType);
                }
            }
            foreach (var s in series2)
            {
                foreach (var v in s.Value)
                {
                    if (!SourceType.Any(x => x.Equals(v.SourceType)))
                    {
                        SourceType.Add(v.SourceType);
                    }
                }
            }
            foreach (var sType in SourceType)
            {
                // for series1
                if (series1.Select(x => x.Value).FirstOrDefault() != null)
                {
                    thisCountAsType = series1.Select(x => x.Value).FirstOrDefault().Where(x => x.SourceType.Equals(sType)).Select(x => x.WOCountAsType).FirstOrDefault();
                }
                Series1Data.Add(thisCountAsType);

                // for series2
                if (series2.Select(x => x.Value).FirstOrDefault() != null)
                {
                    thisCountAsStatus = series2.Select(x => x.Value).FirstOrDefault().Where(x => x.SourceType.Equals(sType)).Select(x => x.WOCountAsStatus).FirstOrDefault();
                }
                Series2Data.Add(thisCountAsStatus);
            }
            return Json(new { labels = SourceType.ToArray(), dataPack1 = Series1Data.ToArray(), dataPack2 = Series2Data.ToArray() }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult WObyTypeData(int wotimeframe = 2)
        {
            List<KeyValuePair<string, long>> entries = new List<KeyValuePair<string, long>>();
            List<int> dataList = new List<int>();
            List<string> LabelList = new List<string>();
            List<string> bColorList = new List<string>();
            List<string> hColorList = new List<string>();
            Random random = new Random();
            t3 = Task.Factory.StartNew(() => entries = DashboardReports.WorkOrder_ByWorkOrderType(userData.DatabaseKey, wotimeframe, 0));
            t3.Wait();
            foreach (var ent in entries)
            {
                dataList.Add((int)ent.Value);
                LabelList.Add(ent.Key);
            }

            return Json(new { series = dataList.ToArray(), labels = LabelList.ToArray(), backgroundColor = bColorList.ToArray(), hoverBackgroundColor = hColorList.ToArray() }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult WObyPriorityData(int wotimeframe1 = 3)
        {
            List<KeyValuePair<string, long>> entries = new List<KeyValuePair<string, long>>();
            List<int> dataList = new List<int>();
            List<string> LabelList = new List<string>();
            List<string> bColorList = new List<string>();
            List<string> hColorList = new List<string>();
            Random random = new Random();
            t4 = Task.Factory.StartNew(() => entries = DashboardReports.WorkOrder_ByWorkOrderPriority(userData.DatabaseKey, wotimeframe1));
            t4.Wait();
            foreach (var ent in entries)
            {
                dataList.Add((int)ent.Value);
                LabelList.Add(ent.Key);
            }

            return Json(new { series = dataList.ToArray(), labels = LabelList.ToArray(), backgroundColor = bColorList.ToArray(), hoverBackgroundColor = hColorList.ToArray() }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult WoLaborHr(int duration = 7)
        {
            List<KeyValuePair<string, decimal>> entries = new List<KeyValuePair<string, decimal>>();
            WoLaborHr woLaborHr;
            List<WoLaborHr> WoLaborHrList = new List<WoLaborHr>();

            t5 = Task.Factory.StartNew(() => entries = DashboardReports.WorkOrderLaborHrs(userData.DatabaseKey, duration));
            t5.Wait();
            foreach (var ent in entries)
            {
                woLaborHr = new WoLaborHr();
                woLaborHr.PID = ent.Key;
                woLaborHr.Hrs = ent.Value;
                WoLaborHrList.Add(woLaborHr);
            }

            return Json(WoLaborHrList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult WoLaborHrNew(int duration = 7)
        {
            List<ChartFusionLabour> _chart = new List<ChartFusionLabour>();

            List<KeyValuePair<string, decimal>> entries = new List<KeyValuePair<string, decimal>>();
            ChartFusionLabour woLaborHr;

            t5 = Task.Factory.StartNew(() => entries = DashboardReports.WorkOrderLaborHrs(userData.DatabaseKey, duration));
            t5.Wait();
            foreach (var ent in entries)
            {
                woLaborHr = new ChartFusionLabour();
                woLaborHr.label = ent.Key;
                woLaborHr.value = ent.Value;
                _chart.Add(woLaborHr);
            }

            return Json(_chart, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult InventoryValuation()
        {
            DataTable dtList = new DataTable();
            Random random = new Random();
            string bColor = string.Empty;
            List<string> dateList = new List<string>();
            long minValue = 0;
            long maxValue = 0;
            List<InventoryValuationChartData> iventoryList = new List<InventoryValuationChartData>();
            DashboardReportBAL objReports = new DashboardReportBAL();
            string conString = userData.DatabaseKey.Client.ConnectionString;// System.Configuration.ConfigurationManager.AppSettings["ClientConnectionString"].ToString();
            t6 = Task.Factory.StartNew(() => dtList = objReports.GetInventoryValuationData(userData.DatabaseKey.Client.ClientId, userData.DatabaseKey.User.DefaultSiteId, 2, conString));
            t6.Wait();
            var list = ConvertDataTable<Models.Metrics>(dtList);
            if (list != null && list.Count > 0)
            {
                list.Sort((x, y) => DateTime.Compare(x.DataDate, y.DataDate));    // RKL - 2020-Aug-17 Sort so dates go left to right
                minValue = list.Min(x => x.MetricValue);
                maxValue = list.Max(x => x.MetricValue);                          // RKL - 2020-Aug-17 - Was "Min" - gave erroneous results
                dateList = list.Select(x => x.DataDate.ToString("MM/dd/yyyy")).ToList();
            }
            foreach (var item in list)
            {
                InventoryValuationChartData model = new InventoryValuationChartData();
                bColor = string.Format("#{0:X6}", random.Next(0x1000000));
                model.backgroundColor = bColor;
                model.borderColor = bColor;
                model.borderWidth = 1;
                model.pointBorderColor = bColor;
                model.pointRadius = 1;
                model.pointHoverRadius = 5;
                model.pointHitRadius = 30;
                model.pointBorderWidth = 2;
                model.data.AddRange(list.Select(s => s.MetricValue).ToList());
                iventoryList.Add(model);
            }
            return Json(new { metricsValueList = iventoryList, dataDateList = dateList, minValue = minValue, maxValue = maxValue }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult WOSource(int duration = 3)
        {
            List<WorkOrderSourceChartData> WOSourceDbList = new List<WorkOrderSourceChartData>();
            List<b_WorkOrder> entries = new List<b_WorkOrder>();
            Random random = new Random();
            string bColor = string.Empty;
            int MaxMonths = 0;
            int MinMonths = 0;
            List<int> AllMonths = new List<int>();
            List<string> SourceType = new List<string>();
            List<string> Months = new List<string>();
            t7 = Task.Factory.StartNew(() => entries = DashboardReports.WorkOrderSource(userData.DatabaseKey, duration));
            t7.Wait();
            var AllSourcetypes = entries.GroupBy(x => x.SourceType).ToList();
            if ((entries != null && entries.Count > 0))
            {
                MaxMonths = entries.Select(x => x.Month).Max();
                MinMonths = entries.Select(x => x.Month).Min();
                AllMonths = entries.Select(x => x.Month).Distinct().ToList();
            }

            foreach (var m in AllMonths)
            {
                Months.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m));
            }
            if (AllSourcetypes != null)
            {
                foreach (var source in AllSourcetypes)
                {
                    WorkOrderSourceChartData model = new WorkOrderSourceChartData();
                    model.label = source.Key;
                    bColor = string.Format("#{0:X6}", random.Next(0x1000000));
                    model.backgroundColor = bColor;
                    model.borderColor = bColor;
                    model.borderWidth = 1;
                    model.pointBorderColor = bColor;
                    model.pointRadius = 1;
                    model.pointHoverRadius = 5;
                    model.pointHitRadius = 30;
                    model.pointBorderWidth = 2;
                    for (int i = MinMonths; i <= MaxMonths; i++)
                    {
                        var thisData = entries.Where(x => x.SourceType.Equals(source.Key) && x.Month == i).Select(x => x.WOCount).FirstOrDefault();
                        model.data.Add(thisData);
                    }
                    WOSourceDbList.Add(model);
                }
            }
            return Json(new { WOSourceDbList = WOSourceDbList, SourceType = Months }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult WorkOrderBacklogWidget()
        {
            List<KeyValuePair<int, string>> entries = new List<KeyValuePair<int, string>>();
            List<WorkOrderBacklogDb> WorkOrderBacklogDbList = new List<WorkOrderBacklogDb>();
            List<WorkOrderBacklogDb> WorkOrderBacklogDbListFinal = new List<WorkOrderBacklogDb>();
            WorkOrderBacklogDb workOrderBacklogDb;
            t8 = Task.Factory.StartNew(() => entries = DashboardReports.WorkOrderBacklog(userData.DatabaseKey));
            t8.Wait();
            foreach (var ent in entries)
            {
                workOrderBacklogDb = new WorkOrderBacklogDb();
                workOrderBacklogDb.WoCount = Convert.ToString(ent.Key);
                workOrderBacklogDb.DateRange = Convert.ToString(ent.Value);
                WorkOrderBacklogDbList.Add(workOrderBacklogDb);
            }
            if (!WorkOrderBacklogDbList.Any(x => x.DateRange.Equals("LessThan30Days")))
            {
                workOrderBacklogDb = new WorkOrderBacklogDb();
                workOrderBacklogDb.WoCount = Convert.ToString(0);
                workOrderBacklogDb.DateRange = UtilityFunction.GetMessageFromResource("LessThan30Days", LocalizeResourceSetConstants.DashboardDetails);
                WorkOrderBacklogDbListFinal.Add(workOrderBacklogDb);
            }
            else
            {
                workOrderBacklogDb = new WorkOrderBacklogDb();
                workOrderBacklogDb.WoCount = WorkOrderBacklogDbList.Where(x => x.DateRange.Equals("LessThan30Days")).Select(x => x.WoCount).FirstOrDefault();
                workOrderBacklogDb.DateRange = UtilityFunction.GetMessageFromResource("LessThan30Days", LocalizeResourceSetConstants.DashboardDetails);
                WorkOrderBacklogDbListFinal.Add(workOrderBacklogDb);

            }
            if (!WorkOrderBacklogDbList.Any(x => x.DateRange.Equals("30To60Days")))
            {
                workOrderBacklogDb = new WorkOrderBacklogDb();
                workOrderBacklogDb.WoCount = Convert.ToString(0);
                workOrderBacklogDb.DateRange = UtilityFunction.GetMessageFromResource("30To60Days", LocalizeResourceSetConstants.DashboardDetails);
                WorkOrderBacklogDbListFinal.Add(workOrderBacklogDb);
            }
            else
            {
                workOrderBacklogDb = new WorkOrderBacklogDb();
                workOrderBacklogDb.WoCount = WorkOrderBacklogDbList.Where(x => x.DateRange.Equals("30To60Days")).Select(x => x.WoCount).FirstOrDefault();
                workOrderBacklogDb.DateRange = UtilityFunction.GetMessageFromResource("30To60Days", LocalizeResourceSetConstants.DashboardDetails);
                WorkOrderBacklogDbListFinal.Add(workOrderBacklogDb);
            }
            if (!WorkOrderBacklogDbList.Any(x => x.DateRange.Equals("GreaterThan60Days")))
            {
                workOrderBacklogDb = new WorkOrderBacklogDb();
                workOrderBacklogDb.WoCount = Convert.ToString(0);
                workOrderBacklogDb.DateRange = UtilityFunction.GetMessageFromResource("GreaterThan60Days", LocalizeResourceSetConstants.DashboardDetails);
                WorkOrderBacklogDbListFinal.Add(workOrderBacklogDb);
            }
            else
            {
                workOrderBacklogDb = new WorkOrderBacklogDb();
                workOrderBacklogDb.WoCount = WorkOrderBacklogDbList.Where(x => x.DateRange.Equals("GreaterThan60Days")).Select(x => x.WoCount).FirstOrDefault();
                workOrderBacklogDb.DateRange = UtilityFunction.GetMessageFromResource("GreaterThan60Days", LocalizeResourceSetConstants.DashboardDetails);
                WorkOrderBacklogDbListFinal.Add(workOrderBacklogDb);
            }
            return Json(WorkOrderBacklogDbListFinal, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region DashBoard SparkLine Charts
        [HttpGet]
        public ActionResult LoadingOpenWoSpChart()
        {
            List<string> dateList = new List<string>();
            DataTable dtList = new DataTable();
            List<KeyValuePair<string, long>> entries = new List<KeyValuePair<string, long>>();
            DashboardReportBAL objReports = new DashboardReportBAL();
            long OpenWoCount = 0;
            long wrCount = 0;
            List<long> data = new List<long>();
            string conString = userData.DatabaseKey.Client.ConnectionString; ;// System.Configuration.ConfigurationManager.AppSettings["ClientConnectionString"].ToString();

            t9 = Task.Factory.StartNew(() => entries = DashboardReports.WorkOrder_RetrieveByWOStatusForChart_1(userData.DatabaseKey, 0));
            t9.Wait();
            if (entries != null && entries.Count > 0)
            {
                // RKL - 2021-12-20 - Open should include "AwaitApproval" and "Planning"
                OpenWoCount = entries.Where(x => x.Key.ToLower().Equals("workrequest")
                                              || x.Key.ToLower().Equals("scheduled")
                                              || x.Key.ToLower().Equals("approved")
                                              || x.Key.ToLower().Equals("awaitapproval")
                                              || x.Key.ToLower().Equals("planning")).Sum(x => x.Value);
                wrCount = entries.Where(x => x.Key.ToLower().Equals("workrequest")).Sum(x => x.Value);
            }

            t10 = Task.Factory.StartNew(() => dtList = objReports.GetInventoryValuationData(userData.DatabaseKey.Client.ClientId, userData.DatabaseKey.User.DefaultSiteId, 1, conString));
            t10.Wait();
            var list = ConvertDataTable<Models.Metrics>(dtList);
            if (list != null)
            {
                dateList = list.Select(x => x.DataDate.ToString("MM/dd/yyyy")).ToList();
                data = list.Select(s => s.MetricValue).ToList();
            }

            return Json(new { metricsValueList = data, dataDateList = dateList, OpenWoCount = OpenWoCount, wrCount = wrCount }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult LoadingWorkRequestSpChart()
        {
            List<string> dateList = new List<string>();
            DataTable dtList = new DataTable();
            DashboardReportBAL objReports = new DashboardReportBAL();
            List<long> data = new List<long>();
            string conString = userData.DatabaseKey.Client.ConnectionString; ;// System.Configuration.ConfigurationManager.AppSettings["ClientConnectionString"].ToString();
            t11 = Task.Factory.StartNew(() => dtList = objReports.GetInventoryValuationData(userData.DatabaseKey.Client.ClientId, userData.DatabaseKey.User.DefaultSiteId, 3, conString));
            t11.Wait();  // RKL - 2020-APR-20
            var list = ConvertDataTable<Models.Metrics>(dtList);
            dateList = list.Select(x => x.DataDate.ToString("MM/dd/yyyy")).ToList();
            if (dateList != null)
            {
                data = list.Select(s => s.MetricValue).ToList();
            }
            return Json(new { metricsValueList = data, dataDateList = dateList }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult LoadingOverDuePmSpChart()
        {
            List<KeyValuePair<long, long>> entries = new List<KeyValuePair<long, long>>();
            List<string> dateList = new List<string>();
            DashboardReportBAL objReports = new DashboardReportBAL();
            long thisCount = 0;
            List<long> data = new List<long>();
            t12 = Task.Factory.StartNew(() => entries = DashboardReports.WorkOrder_RetrieveByWOStatusForChart_3(userData.DatabaseKey, 0));
            t12.Wait();  // RKL - 2020-APR-20
            if (entries != null && entries.Count > 0)
            {
                long totalCount = entries[0].Key;
                thisCount = entries[0].Value;
            }
            string conString = userData.DatabaseKey.Client.ConnectionString; ;// System.Configuration.ConfigurationManager.AppSettings["ClientConnectionString"].ToString();
            DataTable dtList = objReports.GetInventoryValuationData(userData.DatabaseKey.Client.ClientId, userData.DatabaseKey.User.DefaultSiteId, 4, conString);
            var list = ConvertDataTable<Models.Metrics>(dtList);
            if (list != null)
            {
                dateList = list.Select(x => x.DataDate.ToString("MM/dd/yyyy")).ToList();
                data = list.Select(s => s.MetricValue).ToList();
            }
            return Json(new { metricsValueList = data, dataDateList = dateList, overDuePmCount = thisCount }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult LoadingLowPartsSpChart()
        {
            List<KeyValuePair<string, long>> entries = new List<KeyValuePair<string, long>>();
            DataTable dtlist = new DataTable();
            List<string> dateList = new List<string>();
            DashboardReportBAL objReports = new DashboardReportBAL();
            string conString = userData.DatabaseKey.Client.ConnectionString; //;System.Configuration.ConfigurationManager.AppSettings["ClientConnectionString"].ToString();
            long thisCount = 0;
            List<long> data = new List<long>();
            Task[] tasks = new Task[2];
            tasks[0] = Task.Factory.StartNew(() => entries = DashboardReports.PartStoreRoom_LowParts(userData.DatabaseKey));
            tasks[1] = Task.Factory.StartNew(() => dtlist = objReports.GetInventoryValuationData(userData.DatabaseKey.Client.ClientId, userData.DatabaseKey.User.DefaultSiteId, 5, conString));
            Task.WaitAll(tasks);
            if (!tasks[0].IsFaulted && tasks[0].IsCompleted)
            {
                if (entries != null && entries.Count > 0)
                {
                    thisCount = entries[0].Value;
                }
            }
            if (!tasks[1].IsFaulted && tasks[1].IsCompleted)
            {
                DataTable dtList = objReports.GetInventoryValuationData(userData.DatabaseKey.Client.ClientId, userData.DatabaseKey.User.DefaultSiteId, 5, conString);
                var list = ConvertDataTable<Models.Metrics>(dtList);
                if (list != null)
                {
                    dateList = list.Select(x => x.DataDate.ToString("MM/dd/yyyy")).ToList();
                    data = list.Select(s => s.MetricValue).ToList();
                }
            }
            /*
            t13 = Task.Factory.StartNew(() => entries = DashboardReports.PartStoreRoom_LowParts(userData.DatabaseKey));
            if (entries != null && entries.Count > 0)
            {
                string totalCount = entries[0].Key;
                thisCount = entries[0].Value;
            }
            string conString = System.Configuration.ConfigurationManager.AppSettings["ClientConnectionString"].ToString();

            DataTable dtList = objReports.GetInventoryValuationData(userData.DatabaseKey.Client.ClientId, userData.DatabaseKey.User.DefaultSiteId, 5, conString);
            var list = ConvertDataTable<Metrics>(dtList);
            if (list != null)
            {
                dateList = list.Select(x => x.DataDate.ToString("MM/dd/yyyy")).ToList();
                data = list.Select(s => s.MetricValue).ToList();
            }
            */
            return Json(new { metricsValueList = data, dataDateList = dateList, lowPartsCount = thisCount }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region APMOnly
        public JsonResult GetHozBarCount()
        {
            EventInfoWrapper evWrapper = new EventInfoWrapper(userData);
            var counts = evWrapper.GetAPMCountHozBar();
            return Json(counts, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FaultCodeChartData(int timeframe = 1)
        {
            EventInfoWrapper evWrapper = new EventInfoWrapper(userData);
            var chartData = evWrapper.GetAPMBarChartData(timeframe);
            return Json(chartData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DispositionChartData(int timeframe = 1)
        {
            List<int> dataList = new List<int>();
            List<string> LabelList = new List<string>();
            List<string> bColorList = new List<string>();
            List<string> hColorList = new List<string>();

            EventInfoWrapper evWrapper = new EventInfoWrapper(userData);
            var chartData = evWrapper.GetAPMDoughChart(timeframe);

            foreach (var ent in chartData)
            {
                dataList.Add((int)ent.EventCount);
                LabelList.Add(UtilityFunction.GetMessageFromResource("spn" + ent.Disposition, LocalizeResourceSetConstants.EventInfo));
            }
            return Json(new { series = dataList.ToArray(), labels = LabelList.ToArray(), backgroundColor = bColorList.ToArray(), hoverBackgroundColor = hColorList.ToArray() }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Work Request Only
        public string GetWorkOrderMaintGrid(int? draw, int? start, int? length, int CustomQueryDisplayId = 0,
         string workorder = "", string description = "", string Chargeto = "", string Chargetoname = "", string status = "",
         DateTime? Created = null, string assigned = "", DateTime? Scheduled = null,
         DateTime? Complete = null)
        {
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            List<string> statusList = new List<string>();
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var colname = Request.Form.GetValues("columns[" + order + "][name]");
            string _created = string.Empty;
            string _scheduled = string.Empty;
            string _complete = string.Empty;
            _created = Created.HasValue ? Created.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _scheduled = Scheduled.HasValue ? Scheduled.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            _complete = Complete.HasValue ? Complete.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;

            start = start.HasValue
                ? start / length
                : 0;
            int skip = start * length ?? 0;

            List<WorkOrderModel> woMaintMasterList = woWrapper.populateWODetailsForDashboard(CustomQueryDisplayId, out Dictionary<string, Dictionary<string, string>> lookupLists,
                skip, length ?? 0, colname[0], orderDir, workorder, description, Chargeto, Chargetoname, status, _created,
                assigned, _scheduled, _complete);
            var totalRecords = 0;
            var recordsFiltered = 0;

            recordsFiltered = woMaintMasterList.Select(o => o.TotalCount).FirstOrDefault();
            totalRecords = woMaintMasterList.Select(o => o.TotalCount).FirstOrDefault();
            var filteredResult = woMaintMasterList
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, lookupLists = lookupLists }, JsonSerializerDateSettings);
        }

        [HttpGet]
        public string GetWorkOrderPrintData(string _colname, string _coldir, int _CustomQueryDisplayId = 0, string _ClientLookupId = "", string _Description = "", string _ChargeToClientLookupId = "", string _ChargeToName = "",
                               string _Status = "", DateTime? _Created = null, string _Assigned = "", DateTime? _Scheduled = null, DateTime? _Complete = null)
        {
            List<DashBoardWorkOrderPrintModel> WorkOrderPrintModelList = new List<DashBoardWorkOrderPrintModel>();
            DashBoardWorkOrderPrintModel objWorkOrderPrintModel;

            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            var woList = woWrapper.populateWODetailsForDashboardPrint(_CustomQueryDisplayId);
            woList = this.GetAllWorkOrderSortByColumnWithOrder(_colname, _coldir, woList);
            if (woList != null)
            {
                if (!string.IsNullOrEmpty(_ClientLookupId))
                {
                    _ClientLookupId = _ClientLookupId.ToUpper();
                    woList = woList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookupId) && x.ClientLookupId.ToUpper().Contains(_ClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(_Description))
                {
                    _Description = _Description.ToUpper();
                    woList = woList.Where(x => (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(_Description))).ToList();
                }
                if (!string.IsNullOrEmpty(_ChargeToClientLookupId))
                {
                    _ChargeToClientLookupId = _ChargeToClientLookupId.ToUpper();
                    woList = woList.Where(x => (!string.IsNullOrWhiteSpace(x.ChargeToClientLookupId) && x.ChargeToClientLookupId.ToUpper().Contains(_ChargeToClientLookupId))).ToList();
                }
                if (!string.IsNullOrEmpty(_ChargeToName))
                {
                    _ChargeToName = _ChargeToName.ToUpper();
                    woList = woList.Where(x => (!string.IsNullOrWhiteSpace(x.ChargeTo_Name) && x.ChargeTo_Name.ToUpper().Contains(_ChargeToName))).ToList();
                }

                if (!string.IsNullOrEmpty(_Status))
                {
                    _Status = _Status.ToUpper();
                    woList = woList.Where(x => (!string.IsNullOrWhiteSpace(x.Status) && x.Status.ToUpper().Contains(_Status))).ToList();
                }

                if (_Created != null)
                {
                    woList = woList.Where(x => (x.CreateDate != null && x.CreateDate.Value.Date.Equals(_Created.Value.Date))).ToList();
                }
                if (!string.IsNullOrEmpty(_Assigned))
                {
                    _Assigned = _Assigned.ToUpper();
                    woList = woList.Where(x => (!string.IsNullOrWhiteSpace(x.Assigned) && x.Assigned.ToUpper().Contains(_Assigned))).ToList();
                }
                if (_Scheduled != null)
                {
                    woList = woList.Where(x => (x.ScheduledStartDate != null && x.ScheduledStartDate.Value.Date.Equals(_Scheduled.Value.Date))).ToList();
                }
                if (_Complete != null)
                {
                    woList = woList.Where(x => (x.CompleteDate != null && x.CompleteDate.Value.Date.Equals(_Complete.Value.Date))).ToList();
                }
                foreach (var v in woList)
                {
                    objWorkOrderPrintModel = new DashBoardWorkOrderPrintModel();
                    objWorkOrderPrintModel.ClientLookupId = v.ClientLookupId;
                    objWorkOrderPrintModel.Description = v.Description;
                    objWorkOrderPrintModel.ChargeToClientLookupId = v.ChargeToClientLookupId;
                    objWorkOrderPrintModel.ChargeTo_Name = v.ChargeTo_Name;
                    objWorkOrderPrintModel.Status = v.Status;
                    objWorkOrderPrintModel.CreateDate = v.CreateDate;
                    objWorkOrderPrintModel.Assigned = v.Assigned;
                    objWorkOrderPrintModel.ScheduledFinishDate = v.ScheduledFinishDate;
                    objWorkOrderPrintModel.CompleteDate = v.CompleteDate;
                    WorkOrderPrintModelList.Add(objWorkOrderPrintModel);
                }
            }
            return JsonConvert.SerializeObject(new { data = WorkOrderPrintModelList }, JsonSerializerDateSettings);
        }
        private List<WorkOrderModel> GetAllWorkOrderSortByColumnWithOrder(string order, string orderDir, List<WorkOrderModel> data)
        {
            List<WorkOrderModel> lst = new List<WorkOrderModel>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ChargeToClientLookupId).ToList() : data.OrderBy(p => p.ChargeToClientLookupId).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ChargeTo_Name).ToList() : data.OrderBy(p => p.ChargeTo_Name).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Status).ToList() : data.OrderBy(p => p.Status).ToList();
                    break;
                case "5":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CreateDate).ToList() : data.OrderBy(p => p.CreateDate).ToList();
                    break;
                case "6":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Assigned).ToList() : data.OrderBy(p => p.Assigned).ToList();
                    break;
                case "7":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ScheduledFinishDate).ToList() : data.OrderBy(p => p.ScheduledFinishDate).ToList();
                    break;
                case "8":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CompleteDate).ToList() : data.OrderBy(p => p.CompleteDate).ToList();
                    break;
                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
            }


            return lst;
        }
        public ActionResult AddWoRequest(string mode = null)
        {
            WorkOrderVM objWorkOrderVM = new WorkOrderVM();
            WorkOrderModel woModel = new WorkOrderModel();
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> Type = new List<DataContracts.LookupList>();
            AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                Type = AllLookUps.Where(x => x.ListName == LookupListConstants.WR_WO_TYPE).ToList();
            }
            WoRequestModel RequestModel = new WoRequestModel();
            if (Type != null)
            {
                RequestModel.TypeList = Type.Select(x => new SelectListItem { Text = x.ListValue + "   |   " + x.Description, Value = x.ListValue.ToString() });
            }
            // V2-608
            //var ChargeTypeList = UtilityFunction.populateChargeType();
            //if (ChargeTypeList != null)
            //{
            //    RequestModel.ChargeTypeList = ChargeTypeList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            //}
            // V2-608
            List<SelectListItem> defaultChargeToList = new List<SelectListItem>();
            RequestModel.ChargeToList = defaultChargeToList;
            woModel.PlantLocation = userData.Site.PlantLocation;
            objWorkOrderVM._userdata = this.userData;     // RKL - Missing - Caused error when hitting the add work request button
            objWorkOrderVM.woRequestModel = RequestModel;
            objWorkOrderVM.IsAddRequest = true;
            objWorkOrderVM.workOrderModel = woModel;
            objWorkOrderVM._userdata = this.userData;
            LocalizeControls(objWorkOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return View("~/Views/Dashboard/WorkRequestOnly/_AddWoRequest.cshtml", objWorkOrderVM);
        }
        public ActionResult AddWoRequestDynamic(string mode = null)
        {
            WorkOrderVM objWorkOrderVM = new WorkOrderVM();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            AllLookUps = commonWrapper.GetAllLookUpList();
            objWorkOrderVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
                                                .Retrieve(DataDictionaryViewNameConstant.AddWorkRequest, userData);
            IList<string> LookupNames = objWorkOrderVM.UIConfigurationDetails.ToList()
                                     .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
                                     .Select(s => s.LookupName)
                                     .ToList();
            objWorkOrderVM.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                  .GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault())
                                                  .Select(s => new WOAddUILookupList
                                                  { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                  .ToList();

            objWorkOrderVM.IsWorkOrderRequest = false; //V2-1052
            if (string.IsNullOrEmpty(mode))
            {
                objWorkOrderVM.IsWorkOrderRequest = true;
                objWorkOrderVM.AllRequiredLookUplist.AddRange(AllLookUps.Where(x => x.ListName == LookupListConstants.WR_WO_TYPE)
                                               .GroupBy(x => new { x.ListName, x.ListValue }).Select(x => x.FirstOrDefault())
                                               .Select(s => new WOAddUILookupList
                                               { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                               .ToList());
            }

            objWorkOrderVM._userdata = this.userData;     // RKL - Missing - Caused error when hitting the add work request button
            objWorkOrderVM.IsAddRequest = true;
            objWorkOrderVM.AddWorkRequest = new Models.Work_Order.UIConfiguration.AddWorkRequestModelDynamic();
            objWorkOrderVM._userdata = this.userData;
            LocalizeControls(objWorkOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return View("~/Views/Dashboard/WorkRequestOnly/AddWoRequestDynamic.cshtml", objWorkOrderVM);
            
        }
        public JsonResult GetOpenJobsCounts()
        {
            SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
            var openJobsCount = sWrapper.GetCount("OpenJobs");
            return Json(openJobsCount, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetRequestsCounts()
        {
            SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
            var requestCount = sWrapper.GetCount("Requests");
            return Json(requestCount, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetOverdueJobsCounts()
        {
            SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
            var overdueJobsCount = sWrapper.GetCount("OverDueJobs");
            return Json(overdueJobsCount, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBarChartOfJobsByStatusData(int timeFrame = 1)
        {
            SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
            var chartData = sWrapper.GetBarChartOfJobsByStatusData(timeFrame);
            return Json(chartData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDoughnutbyPassFailChart(int timeFrame = 1)
        {
            List<int> dataList = new List<int>();
            List<string> LabelList = new List<string>();
            List<string> bColorList = new List<string>();
            List<string> hColorList = new List<string>();

            SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
            var chartData = sWrapper.GetDoughnutbyPassFailChart(timeFrame);

            foreach (var ent in chartData)
            {
                dataList.Add((int)ent.SanitationJobCount);
                LabelList.Add(ent.Status);
            }

            return Json(new { series = dataList.ToArray(), labels = LabelList.ToArray(), backgroundColor = bColorList.ToArray(), hoverBackgroundColor = hColorList.ToArray() }, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Sanitation Job Only
        [HttpPost]
        public string GetSantGridData(int? draw, int? start, int? length, int CustomQueryDisplayId = 0, string ClientLookupId = "", string Description = "", string ChargeTo_ClientLookupId = "", string ChargeTo_Name = "",
                              string Status = "", DateTime? CreateDate = null, string Assigned = "", DateTime? CompleteDate = null)
        {
            List<string> statusList = new List<string>();
            List<string> createdByList = new List<string>();
            List<string> assignedList = new List<string>();
            List<string> verifiedByList = new List<string>();
            var filter = CustomQueryDisplayId;
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var colname = Request.Form.GetValues("columns[" + order + "][name]");
            SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
            var sJlist = sWrapper.SanitationWRSearch(CustomQueryDisplayId);
            if (sJlist != null)
            {
                statusList = sJlist.Select(r => r.Status).GroupBy(x => x.ToString()).Select(x => x.First()).ToList();
                assignedList = sJlist.Select(r => r.Assigned).GroupBy(x => x.ToString()).Select(x => x.First()).ToList();
            }
            sJlist = this.GetAllSanitationSortByColumnWithOrder(colname[0], orderDir, sJlist);
            if (sJlist != null)
            {
                sJlist = this.GetSanitSearchResult(sJlist, ClientLookupId, Description, ChargeTo_ClientLookupId, ChargeTo_Name, Status, CreateDate, Assigned, CompleteDate);
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = sJlist.Count();
            totalRecords = sJlist.Count();

            int initialPage = start.Value;
            var filteredResult = sJlist
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            var ss = sJlist.Select(x => x.Extracted).ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, statusList = statusList, assignedList = assignedList }, JsonSerializerDateSettings);
        }

        private List<SanitationJobSearchModel> GetAllSanitationSortByColumnWithOrder(string order, string orderDir, List<SanitationJobSearchModel> data)
        {
            List<SanitationJobSearchModel> lst = new List<SanitationJobSearchModel>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ChargeTo_ClientLookupId).ToList() : data.OrderBy(p => p.ChargeTo_ClientLookupId).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ChargeTo_Name).ToList() : data.OrderBy(p => p.ChargeTo_Name).ToList();
                    break;
                case "4":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Status).ToList() : data.OrderBy(p => p.Status).ToList();
                    break;
                case "5":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CreateDate).ToList() : data.OrderBy(p => p.CreateDate).ToList();
                    break;
                case "6":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Assigned).ToList() : data.OrderBy(p => p.Assigned).ToList();
                    break;
                case "7":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CompleteDate).ToList() : data.OrderBy(p => p.CompleteDate).ToList();
                    break;

                default:
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                    break;
            }
            return lst;
        }


        private List<SanitationJobSearchModel> GetSanitSearchResult(List<SanitationJobSearchModel> sJlist, string ClientLookupId = "", string Description = "", string ChargeTo_ClientLookupId = "", string ChargeTo_Name = "",
                              string Status = "", DateTime? CreateDate = null, string Assigned = "", DateTime? CompleteDate = null)
        {
            if (!string.IsNullOrEmpty(ClientLookupId))
            {
                ClientLookupId = ClientLookupId.ToUpper();
                sJlist = sJlist.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookupId) && x.ClientLookupId.ToUpper().Contains(ClientLookupId))).ToList();
            }
            if (!string.IsNullOrEmpty(Description))
            {
                Description = Description.ToUpper();
                sJlist = sJlist.Where(x => (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToUpper().Contains(Description))).ToList();
            }
            if (!string.IsNullOrEmpty(ChargeTo_ClientLookupId))
            {
                ChargeTo_ClientLookupId = ChargeTo_ClientLookupId.ToUpper();
                sJlist = sJlist.Where(x => (!string.IsNullOrWhiteSpace(x.ChargeTo_ClientLookupId) && x.ChargeTo_ClientLookupId.ToUpper().Contains(ChargeTo_ClientLookupId))).ToList();
            }
            if (!string.IsNullOrEmpty(ChargeTo_Name))
            {
                ChargeTo_Name = ChargeTo_Name.ToUpper();
                sJlist = sJlist.Where(x => (!string.IsNullOrWhiteSpace(x.ChargeTo_Name) && x.ChargeTo_Name.ToUpper().Contains(ChargeTo_Name))).ToList();
            }
            if (!string.IsNullOrEmpty(Status))
            {
                sJlist = sJlist.Where(x => (!string.IsNullOrWhiteSpace(x.Status) && x.Status.Equals(Status))).ToList();
            }
            if (CreateDate != null)
            {
                sJlist = sJlist.Where(x => (x.CreateDate != null && x.CreateDate.Value.Date.Equals(CreateDate.Value.Date))).ToList();
            }

            if (!string.IsNullOrEmpty(Assigned))
            {
                Assigned = Assigned.ToUpper();
                sJlist = sJlist.Where(x => (!string.IsNullOrWhiteSpace(x.Assigned) && x.Assigned.ToUpper().Contains(Assigned))).ToList();
            }
            if (CompleteDate != null)
            {
                sJlist = sJlist.Where(x => (x.CompleteDate != null && x.CompleteDate.Value.Date.Equals(CompleteDate.Value.Date))).ToList();
            }
            return sJlist;
        }



        public ActionResult AddSanitationRequest()
        {
            SanitationVM objVM = new SanitationVM();
            //V2-609
            objVM.AssetTree = this.userData.DatabaseKey.Client.AssetTree;
            objVM._userdata = this.userData;//
            LocalizeControls(objVM, LocalizeResourceSetConstants.SanitationDetails);
            return View("~/Views/Dashboard/WorkRequestOnly/_AddSanitationRequest.cshtml", objVM);
        }
        [HttpGet]
        public string GetSantPrintData(string colname, string coldir, int CustomQueryDisplayId = 0, string ClientLookupId = "", string Description = "", string ChargeTo_ClientLookupId = "", string ChargeTo_Name = "",
                            string Status = "", DateTime? CreateDate = null, string Assigned = "", DateTime? CompleteDate = null)
        {
            List<DashboardSanitationJobPrintModel> sanitationJobPrintModelList = new List<DashboardSanitationJobPrintModel>();
            DashboardSanitationJobPrintModel objSanitationJobPrintModel;
            SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);

            var sJlist = sWrapper.SanitationWRSearch(CustomQueryDisplayId);
            sJlist = this.GetAllSanitationSortByColumnWithOrder(colname, coldir, sJlist);
            if (sJlist != null)
            {
                sJlist = this.GetSanitSearchResult(sJlist, ClientLookupId, Description, ChargeTo_ClientLookupId, ChargeTo_Name, Status, CreateDate, Assigned, CompleteDate);
                foreach (var p in sJlist)
                {
                    objSanitationJobPrintModel = new DashboardSanitationJobPrintModel();
                    objSanitationJobPrintModel.ClientLookupId = p.ClientLookupId;
                    objSanitationJobPrintModel.Description = p.Description;
                    objSanitationJobPrintModel.ChargeTo_ClientLookupId = p.ChargeTo_ClientLookupId;
                    objSanitationJobPrintModel.ChargeTo_Name = p.ChargeTo_Name;
                    objSanitationJobPrintModel.Status = p.Status;
                    objSanitationJobPrintModel.CreateDate = p.CreateDate;
                    objSanitationJobPrintModel.Assigned = p.Assigned;
                    objSanitationJobPrintModel.CompleteDate = p.CompleteDate;
                    sanitationJobPrintModelList.Add(objSanitationJobPrintModel);
                }
            }
            return JsonConvert.SerializeObject(new { data = sanitationJobPrintModelList }, JsonSerializerDateSettings);
        }
        #endregion

        #region V2-375
        [HttpGet]
        public ActionResult AddVendor3FromUpperMenu()
        {
            DashboardVM objDashboardVM = new DashboardVM();
            LocalizeControls(objDashboardVM, LocalizeResourceSetConstants.DashboardDetails);
            return Json(objDashboardVM, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Fleet Only
        public JsonResult GetOpenServiceOrdersCounts()
        {
            FleetServiceWrapper sWrapper = new FleetServiceWrapper(userData);
            var openJobsCount = sWrapper.GetCount();
            return Json(openJobsCount, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOpenFleetIssuesCounts()
        {
            FleetIssueWrapper sWrapper = new FleetIssueWrapper(userData);
            var openJobsCount = sWrapper.GetCount();
            return Json(openJobsCount, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetPastDueServiceCounts()
        {
            ScheduledServiceWrapper sWrapper = new ScheduledServiceWrapper(userData);
            var openJobsCount = sWrapper.GetCount();
            return Json(openJobsCount, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ServiceorderLaborHr(int duration = 7)
        {
            List<KeyValuePair<string, decimal>> entries = new List<KeyValuePair<string, decimal>>();
            SoLaborHr soLaborHr;
            List<SoLaborHr> SoLaborHrList = new List<SoLaborHr>();

            t5 = Task.Factory.StartNew(() => entries = DashboardReports.ServiceOrderLaborHrs(userData.DatabaseKey, duration));
            t5.Wait();
            foreach (var ent in entries)
            {
                soLaborHr = new SoLaborHr();
                soLaborHr.PID = ent.Key;
                soLaborHr.Hrs = ent.Value;
                SoLaborHrList.Add(soLaborHr);
            }

            return Json(SoLaborHrList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ServiceOrderBacklogWidget()
        {
            List<KeyValuePair<int, string>> entries = new List<KeyValuePair<int, string>>();
            List<ServiceOrderBacklogDb> ServiceOrderBacklogDbList = new List<ServiceOrderBacklogDb>();
            List<ServiceOrderBacklogDb> ServiceOrderBacklogDbListFinal = new List<ServiceOrderBacklogDb>();
            ServiceOrderBacklogDb serviceOrderBacklogDb;
            t8 = Task.Factory.StartNew(() => entries = DashboardReports.ServiceOrderBacklog(userData.DatabaseKey));
            t8.Wait();
            foreach (var ent in entries)
            {
                serviceOrderBacklogDb = new ServiceOrderBacklogDb();
                serviceOrderBacklogDb.SoCount = Convert.ToString(ent.Key);
                serviceOrderBacklogDb.DateRange = Convert.ToString(ent.Value);
                ServiceOrderBacklogDbList.Add(serviceOrderBacklogDb);
            }
            if (!ServiceOrderBacklogDbList.Any(x => x.DateRange.Equals("LessThan30Days")))
            {
                serviceOrderBacklogDb = new ServiceOrderBacklogDb();
                serviceOrderBacklogDb.SoCount = Convert.ToString(0);
                serviceOrderBacklogDb.DateRange = UtilityFunction.GetMessageFromResource("LessThan30Days", LocalizeResourceSetConstants.DashboardDetails);
                ServiceOrderBacklogDbListFinal.Add(serviceOrderBacklogDb);
            }
            else
            {
                serviceOrderBacklogDb = new ServiceOrderBacklogDb();
                serviceOrderBacklogDb.SoCount = ServiceOrderBacklogDbList.Where(x => x.DateRange.Equals("LessThan30Days")).Select(x => x.SoCount).FirstOrDefault();
                serviceOrderBacklogDb.DateRange = UtilityFunction.GetMessageFromResource("LessThan30Days", LocalizeResourceSetConstants.DashboardDetails);
                ServiceOrderBacklogDbListFinal.Add(serviceOrderBacklogDb);

            }
            if (!ServiceOrderBacklogDbList.Any(x => x.DateRange.Equals("30To60Days")))
            {
                serviceOrderBacklogDb = new ServiceOrderBacklogDb();
                serviceOrderBacklogDb.SoCount = Convert.ToString(0);
                serviceOrderBacklogDb.DateRange = UtilityFunction.GetMessageFromResource("30To60Days", LocalizeResourceSetConstants.DashboardDetails);
                ServiceOrderBacklogDbListFinal.Add(serviceOrderBacklogDb);
            }
            else
            {
                serviceOrderBacklogDb = new ServiceOrderBacklogDb();
                serviceOrderBacklogDb.SoCount = ServiceOrderBacklogDbList.Where(x => x.DateRange.Equals("30To60Days")).Select(x => x.SoCount).FirstOrDefault();
                serviceOrderBacklogDb.DateRange = UtilityFunction.GetMessageFromResource("30To60Days", LocalizeResourceSetConstants.DashboardDetails);
                ServiceOrderBacklogDbListFinal.Add(serviceOrderBacklogDb);
            }
            if (!ServiceOrderBacklogDbList.Any(x => x.DateRange.Equals("GreaterThan60Days")))
            {
                serviceOrderBacklogDb = new ServiceOrderBacklogDb();
                serviceOrderBacklogDb.SoCount = Convert.ToString(0);
                serviceOrderBacklogDb.DateRange = UtilityFunction.GetMessageFromResource("GreaterThan60Days", LocalizeResourceSetConstants.DashboardDetails);
                ServiceOrderBacklogDbListFinal.Add(serviceOrderBacklogDb);
            }
            else
            {
                serviceOrderBacklogDb = new ServiceOrderBacklogDb();
                serviceOrderBacklogDb.SoCount = ServiceOrderBacklogDbList.Where(x => x.DateRange.Equals("GreaterThan60Days")).Select(x => x.SoCount).FirstOrDefault();
                serviceOrderBacklogDb.DateRange = UtilityFunction.GetMessageFromResource("GreaterThan60Days", LocalizeResourceSetConstants.DashboardDetails);
                ServiceOrderBacklogDbListFinal.Add(serviceOrderBacklogDb);
            }
            return Json(ServiceOrderBacklogDbListFinal, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Enterprise User
        [HttpGet]
        public ActionResult EnterpriseUserBarChart(string flag)
        {
            List<KeyValuePair<string, decimal>> entries = new List<KeyValuePair<string, decimal>>();
            EnterpriseUserBarChartBySite EnterpriseUserBarChartBySite;
            List<EnterpriseUserBarChartBySite> EnterpriseUserBarChartBySiteList = new List<EnterpriseUserBarChartBySite>();

            t5 = Task.Factory.StartNew(() => entries = DashboardReports.EnterpriseUserBarChart(userData.DatabaseKey, flag));
            t5.Wait();
            foreach (var ent in entries)
            {
                EnterpriseUserBarChartBySite = new EnterpriseUserBarChartBySite();
                EnterpriseUserBarChartBySite.Site = ent.Key;
                EnterpriseUserBarChartBySite.Value = ent.Value;
                EnterpriseUserBarChartBySiteList.Add(EnterpriseUserBarChartBySite);
            }

            return Json(EnterpriseUserBarChartBySiteList, JsonRequestBehavior.AllowGet);
        }

        #region Retrieve Maintenance Details
        [HttpPost]
        public string GetMetrics_Maintenance(int? draw, int? start, int? length, int duration)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            MetricsWrapper metWrapper = new MetricsWrapper(userData);
            var Maintenance = metWrapper.GetMetricsForMaintenance(duration);
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
            var Inventory = metWrapper.GetMetricsForInventory();
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
            var Purchasing = metWrapper.GetMetricsForPurchasing(duration);
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

        #endregion


        #region V2-552
        public RedirectResult RedirectfromDashboardChange(long DashboardId)
        {
            TempData["Mode"] = "RedirectFromDashboardChange";
            string DLString = Convert.ToString(DashboardId);
            TempData["DashboardListingId"] = DLString;
            return Redirect("/Dashboard/Dashboard");
        }
        [NonAction]
        public DashboardVM GetWidgetlist(DashboardVM objDashboardVM, long dashboardId = 0)
        {
            DashboardWrapper dashboardW = new DashboardWrapper(userData);

            objDashboardVM = AssignSecurityFlags(objDashboardVM);

            objDashboardVM.DashboardList = dashboardW.GetAllDashboard();
            if (objDashboardVM.IsAccessMaintenanceDashboard == false)
            {
                objDashboardVM.DashboardList = objDashboardVM.DashboardList.Where(x => x.Text != "Maintenance").ToList();
            }
            if (objDashboardVM.IsAccessSanitationDashboard == false)
            {
                objDashboardVM.DashboardList = objDashboardVM.DashboardList.Where(x => x.Text != "Sanitation").ToList();
            }
            if (objDashboardVM.IsAccessAPMDashboard == false)
            {
                objDashboardVM.DashboardList = objDashboardVM.DashboardList.Where(x => x.Text != "APM").ToList();
            }
            if (objDashboardVM.IsAccessFleetDashboard == false)
            {
                objDashboardVM.DashboardList = objDashboardVM.DashboardList.Where(x => x.Text != "Fleet").ToList();
            }
            if (objDashboardVM.IsAccessEnterprise_MaintenanceDashboard == false)
            {
                objDashboardVM.DashboardList = objDashboardVM.DashboardList.Where(x => x.Text != "Enterprise – Maintenance").ToList();
            }
            if (objDashboardVM.IsMaintenanceTechnicianDashboard == false)
            {
                objDashboardVM.DashboardList = objDashboardVM.DashboardList.Where(x => x.Text != "Maintenance Technician").ToList();
            }
            //For checking multiple Dashboard Access
            if (objDashboardVM.DashboardList.Count() > 1)
                objDashboardVM.IsMultipleDashboardAccess = true;
            else
                objDashboardVM.IsMultipleDashboardAccess = false;

            if (dashboardId == 0 && objDashboardVM.DashboardList != null && objDashboardVM.DashboardList.Count() > 0)
            {
                dashboardId = Convert.ToInt64(dashboardW.RetrieveDefaultDashboardListingId());
                if (dashboardId > 0 && !objDashboardVM.DashboardList.Any(x => Convert.ToInt64(x.Value) == dashboardId))
                {
                    dashboardId = 0;
                }
                if (dashboardId == 0)
                {
                    dashboardId = Convert.ToInt64(objDashboardVM.DashboardList.FirstOrDefault().Value);
                }
            }

            if (dashboardId != 0)
            {
                List<DashboardContentModel> dashboardContentModelList = new List<DashboardContentModel>();
                DashboardUserSettings dashboardUserSettings = new DashboardUserSettings();

                Task[] tasks = new Task[2];
                tasks[0] = Task.Factory.StartNew(() => dashboardContentModelList = dashboardW.GetDashboardContentlist(dashboardId));
                tasks[1] = Task.Factory.StartNew(() => dashboardUserSettings = dashboardW.RetrieveDashboardUserSettings(dashboardId));
                Task.WaitAll(tasks);
                if (!tasks[0].IsFaulted && tasks[0].IsCompleted)
                {
                    if (dashboardContentModelList.Count > 0)
                    {
                        objDashboardVM.DashboardContentModelList = BindDashboardContent(dashboardContentModelList);
                    }
                }
                if (!tasks[1].IsFaulted && tasks[1].IsCompleted)
                {
                    if (dashboardUserSettings != null)
                    {
                        objDashboardVM = LoadExistingUserDashboardSettings(objDashboardVM, dashboardUserSettings.SettingInfo, dashboardContentModelList,
                            dashboardUserSettings);
                    }
                }
            }
            objDashboardVM.DashboardlistingId = dashboardId;
            LocalizeControls(objDashboardVM, LocalizeResourceSetConstants.DashboardDetails);
            objDashboardVM.security = userData.Security;
            objDashboardVM._userdata = userData;
            return objDashboardVM;
        }
        private List<DashboardContentModel> BindDashboardContent(List<DashboardContentModel> dashboardContentModelList)
        {
            int Position = 0;
            bool isMobile = userData.IsLoggedInFromMobile;
            foreach (var item in dashboardContentModelList)
            {
                item.Position = Position++;
                item.ClassList = "col-xl-" + item.GridColWidth;
                // only specific widgets are created for mobile
                // else the same widget will be used for mobile and desktop
                // if it is a mobile then js and view will be used from mobile folder
                if (isMobile && item.ViewName == "MaintenanceCompletionWorkbench")
                {
                    item.ViewPath = "~/Views/Dashboard/Widgets/" + item.ViewName + "/Mobile/" + item.ViewName + ".cshtml";
                    item.JSPath = "~/bundles/widgets/mobile/" + item.ViewName;
                }
                else
                {
                    item.ViewPath = "~/Views/Dashboard/Widgets/" + item.ViewName + "/" + item.ViewName + ".cshtml";
                    item.JSPath = "~/bundles/widgets/" + item.ViewName;
                }
            }
            return dashboardContentModelList;
        }
        private DashboardVM LoadExistingUserDashboardSettings(DashboardVM objDashboardVM, string SettingInfo,
            List<DashboardContentModel> dashboardContentModelList, DashboardUserSettings dashboardUserSettings)
        {
            if (!string.IsNullOrEmpty(SettingInfo))
            {
                List<DashboardUserSettingModel> dashboardUserSettingModels = JsonConvert.DeserializeObject<List<DashboardUserSettingModel>>(SettingInfo);
                dashboardUserSettingModels.ForEach(item =>
                {
                    if (dashboardContentModelList.Any(x => x.WidgetListingId == item.WidgetListingId))
                    {
                        var model = dashboardContentModelList.Where(x => x.WidgetListingId == item.WidgetListingId).FirstOrDefault();
                        model.Display = item.Display;
                        model.Position = item.Position;
                    }
                });
                objDashboardVM.DashboardContentModelList = dashboardContentModelList;
                objDashboardVM.IsDefault = dashboardUserSettings.IsDefault;
            }
            return objDashboardVM;
        }
        private DashboardVM AssignSecurityFlags(DashboardVM objDashboardVM)
        {
            objDashboardVM.IsAccessMaintenanceDashboard = userData.Security.AccessMaintenanceDashboard.Access;
            objDashboardVM.IsAccessSanitationDashboard = userData.Security.AccessSanitationDashboard.Access;
            objDashboardVM.IsAccessAPMDashboard = userData.Security.AccessAPMDashboard.Access;
            objDashboardVM.IsAccessFleetDashboard = userData.Security.AccessFleetDashboard.Access;
            objDashboardVM.IsAccessEnterprise_MaintenanceDashboard = userData.Security.Access_Enterprise_Maintenance_Dashboard.Access;
            objDashboardVM.IsMaintenanceTechnicianDashboard = userData.Security.AccessMaintenanceTechnicianDashboard.Access;
            return objDashboardVM;
        }
        [HttpPost]
        public JsonResult AddorUpdateDashboardsettings(long DashboardListingId, bool IsDefault = false, string SettingsInfo = "")
        {
            DashboardWrapper dashboardW = new DashboardWrapper(userData);
            DashboardUserSettings settings = new DashboardUserSettings();
            List<DashboardUserSettingModel> dashboardUserSettingModels = new List<DashboardUserSettingModel>();
            DashboardUserSettingModel dashboardUserSettingModel;
            List<DashboardContentModel> dashboardContentModelList = JsonConvert.DeserializeObject<List<DashboardContentModel>>(SettingsInfo);
            dashboardContentModelList.ForEach(item =>
            {
                dashboardUserSettingModel = new DashboardUserSettingModel();
                dashboardUserSettingModel.WidgetListingId = item.WidgetListingId;
                dashboardUserSettingModel.Position = item.Position;
                dashboardUserSettingModel.Display = item.Display;
                dashboardUserSettingModels.Add(dashboardUserSettingModel);
            });
            SettingsInfo = JsonConvert.SerializeObject(dashboardUserSettingModels);
            settings = dashboardW.AddorUpdateDashboardsettings(DashboardListingId, IsDefault, SettingsInfo);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Maintenance Technician

        #region Maintenance Completion Workbench Search grid
        public string GetWorkOrderCompletionWorkbenchGrid(int? draw, int? start, int? length, int CustomQueryDisplayId = 0, string txtSearchval = "", string Order = "1")
        {
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            DashboardWrapper dashWrapper = new DashboardWrapper(userData);
            List<string> statusList = new List<string>();

            start = start.HasValue
                ? start / length
                : 0;
            int skip = start * length ?? 0;

            List<WoCompletionWorkbenchSearchModel> woCompletionWorkbench = dashWrapper.populateWOCompletionWorkbench(CustomQueryDisplayId, skip, length ?? 0, Order, orderDir, txtSearchval);

            var totalRecords = 0;
            var recordsFiltered = 0;

            recordsFiltered = woCompletionWorkbench.Select(o => o.TotalCount).FirstOrDefault();
            totalRecords = woCompletionWorkbench.Select(o => o.TotalCount).FirstOrDefault();

            var filteredResult = woCompletionWorkbench
                .ToList();


            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, /*lookupLists = lookupLists*/ }, JsonSerializerDateSettings);
        }
        #endregion

        #region Maintenance Completion Workbench Details
        //Use MaintenanceCompletionWorkbenchDetails js        

        public PartialViewResult CompletionWorkbench_Details(long WorkOrderId, string ClientLookupId)
        {

            //ViewBag.WorkOrderId = WorkOrderId;
            //ViewBag.ClientlookupId = ClientLookupId;
            DashboardVM dashboardVM = new DashboardVM();
            dashboardVM.security = this.userData.Security;
            dashboardVM._userdata = this.userData;
            WOCompletionWorkbenchSummary WOCompletionWorkbenchSummaryModel = new WOCompletionWorkbenchSummary();
            CommonWrapper comWrapper = new CommonWrapper(userData);
            DashboardWrapper dashboardWrap = new DashboardWrapper(userData);

            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            AllLookUps = comWrapper.GetAllLookUpList();
            dashboardVM.woCompletionDetailsHeader = dashboardWrap.getWorkOderDetailsByIdForCompletionHeader(WorkOrderId);
            if (AllLookUps != null)
            {
                var Priority = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_Priority).ToList();
                if (Priority != null && Priority.Any(cus => cus.ListValue == dashboardVM.woCompletionDetailsHeader.Priority))
                {
                    dashboardVM.woCompletionDetailsHeader.Priority = Priority.Where(x => x.ListValue == dashboardVM.woCompletionDetailsHeader.Priority).Select(x => x.Description).First();
                }
            }

            if (AllLookUps != null)
            {
                var CancelReason = AllLookUps.Where(x => x.ListName == LookupListConstants.Wo_CancelReason).ToList();
                if (CancelReason != null)
                {
                    dashboardVM.CancelReasonListWo = CancelReason.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
                }

            }
            dashboardVM._userdata = this.userData;
            WOCompletionWorkbenchSummaryModel.WorkOrderId = dashboardVM.woCompletionDetailsHeader.WorkOrderId;
            WOCompletionWorkbenchSummaryModel.WorkOrder_ClientLookupId = dashboardVM.woCompletionDetailsHeader.ClientLookupId;
            WOCompletionWorkbenchSummaryModel.Status = dashboardVM.woCompletionDetailsHeader.Status;
            WOCompletionWorkbenchSummaryModel.Description = dashboardVM.woCompletionDetailsHeader.Description;
            WOCompletionWorkbenchSummaryModel.ImageUrl = comWrapper.GetAzureImageUrl(WorkOrderId, AttachmentTableConstant.WorkOrder);
            WOCompletionWorkbenchSummaryModel.Type = dashboardVM.woCompletionDetailsHeader.Type;
            WOCompletionWorkbenchSummaryModel.Priority = dashboardVM.woCompletionDetailsHeader.Priority;
            WOCompletionWorkbenchSummaryModel.ChargeToClientLookupId = dashboardVM.woCompletionDetailsHeader.ChargeToClientLookupId;
            WOCompletionWorkbenchSummaryModel.ChargeTo_Name = dashboardVM.woCompletionDetailsHeader.ChargeTo_Name;
            WOCompletionWorkbenchSummaryModel.Assigned = dashboardVM.woCompletionDetailsHeader.Assigned;
            WOCompletionWorkbenchSummaryModel.WorkAssigned_PersonnelId = dashboardVM.woCompletionDetailsHeader.WorkAssigned_PersonnelId;
            WOCompletionWorkbenchSummaryModel.ScheduledStartDate = dashboardVM.woCompletionDetailsHeader.ScheduledStartDate;
            WOCompletionWorkbenchSummaryModel.RequiredDate = dashboardVM.woCompletionDetailsHeader.RequiredDate;
            WOCompletionWorkbenchSummaryModel.CompleteDate = dashboardVM.woCompletionDetailsHeader.CompleteDate;
            WOCompletionWorkbenchSummaryModel.ChargeToId = dashboardVM.woCompletionDetailsHeader.ChargeToId;
            #region//*****V2-847

            WOCompletionWorkbenchSummaryModel.AssetGroup1Name = String.IsNullOrEmpty(this.userData.Site.AssetGroup1Name) ? "Asset Group 1" : this.userData.Site.AssetGroup1Name;
            WOCompletionWorkbenchSummaryModel.AssetGroup2Name = String.IsNullOrEmpty(this.userData.Site.AssetGroup2Name) ? "Asset Group 2" : this.userData.Site.AssetGroup2Name;
            WOCompletionWorkbenchSummaryModel.AssetGroup1ClientlookupId = dashboardVM.woCompletionDetailsHeader.AssetGroup1ClientlookupId;
            WOCompletionWorkbenchSummaryModel.AssetGroup2ClientlookupId = dashboardVM.woCompletionDetailsHeader.AssetGroup2ClientlookupId;
            #endregion//*****
            WOCompletionWorkbenchSummaryModel.AssetLocation = dashboardVM.woCompletionDetailsHeader.Assetlocation;//V2-1012
            WOCompletionWorkbenchSummaryModel.ProjectClientlookupId = dashboardVM.woCompletionDetailsHeader.ProjectClientLookupId; //V2-1012
            dashboardVM.woCompletionWorkbenchSummary = WOCompletionWorkbenchSummaryModel;
            //-- V2-634 for completion wizard
            WoCompletionSettingsModel completionSettingsModel = new WoCompletionSettingsModel();
            ClientSetUpWrapper clientSetUpWrapper = new ClientSetUpWrapper(userData);
            completionSettingsModel = clientSetUpWrapper.CompletionSettingsDetails();
            dashboardVM.UseWOCompletionWizard = completionSettingsModel.UseWOCompletionWizard;

            //--V2-676
            dashboardVM.WOBarcode = this.userData.Site.WOBarcode;
            //objWorkOrderVM.WOCommentTab = completionSettingsModel.WOCommentTab;
            //objWorkOrderVM.TimecardTab = completionSettingsModel.TimecardTab;
            //objWorkOrderVM.AutoAddTimecard = completionSettingsModel.AutoAddTimecard;

            //if (objWorkOrderVM.UseWOCompletionWizard == true)
            //{
            //    objWorkOrderVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration().Retrieve(DataDictionaryViewNameConstant.WorkOrderCompletion, userData);
            //    objWorkOrderVM.CompletionModelDynamic = new WorkOrderCompletionInformationModelDynamic();
            //    objWorkOrderVM.WorkOrderCompletionWizard = new WorkOrderCompletionWizard();

            //    objWorkOrderVM.WorkOrderCompletionWizard.UseWOCompletionWizard = completionSettingsModel.UseWOCompletionWizard;
            //    objWorkOrderVM.WorkOrderCompletionWizard.WOCommentTab = completionSettingsModel.WOCommentTab;
            //    objWorkOrderVM.WorkOrderCompletionWizard.TimecardTab = completionSettingsModel.TimecardTab;
            //    objWorkOrderVM.WorkOrderCompletionWizard.AutoAddTimecard = completionSettingsModel.AutoAddTimecard;

            //    IList<string> LookupNames = objWorkOrderVM.UIConfigurationDetails.ToList()
            //                           .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
            //                           .Select(s => s.LookupName)
            //                           .ToList();

            //    objWorkOrderVM.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
            //                                          .GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault())
            //                                          .Select(s => new WOAddUILookupList
            //                                          { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
            //                                          .ToList();

            //}
            //-- V2-634 

            LocalizeControls(dashboardVM, LocalizeResourceSetConstants.DashboardDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/_WOCompletionWorkbenchDetails.cshtml", dashboardVM);
        }

        #region Hover for Assigned user
        [HttpPost]
        public JsonResult PopulateHover(long workOrderId = 0)
        {
            DashboardWrapper dashWrapper = new DashboardWrapper(userData);
            string personnelList = dashWrapper.PopulateHoverList(workOrderId);
            if (!string.IsNullOrEmpty(personnelList))
            {
                personnelList = personnelList.Trim().TrimEnd(',');
            }
            return Json(new { personnelList = personnelList }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Instructions
        [HttpPost]
        public PartialViewResult LoadIntructions(long WorkOrderId)
        {
            DashboardVM dashboardVM = new DashboardVM();
            CommonWrapper coWrapper = new CommonWrapper(userData);
            var instructions = coWrapper.PopulateInstructions(WorkOrderId, "WorkOrder");
            dashboardVM.InstructionModel = new InstructionModel();
            if (instructions != null && instructions.Count() > 0)
            {
                dashboardVM.InstructionModel = instructions.FirstOrDefault();
            }
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/_InstructionDetails.cshtml", dashboardVM);
        }
        #endregion

        #region Attachment
        [HttpPost]
        public PartialViewResult LoadAttachment()
        {
            DashboardVM dashboardVM = new DashboardVM();
            LocalizeControls(dashboardVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/_AttachmentDetails.cshtml", dashboardVM);
        }
        [HttpPost]
        public string PopulateAttachment(int? draw, int? start, int? length, long workOrderId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            var AttachmentList = objCommonWrapper.PopulateAttachments(workOrderId, "WorkOrder", userData.Security.WorkOrders.Edit);
            if (AttachmentList != null)
            {
                AttachmentList = GetAllAttachmentsSortByColumnWithOrder(order, orderDir, AttachmentList);
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = AttachmentList.Count();
            totalRecords = AttachmentList.Count();
            int initialPage = start.Value;
            var filteredResult = AttachmentList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeSettings);
        }
        public ActionResult DownloadAttachment(long _fileinfoId)
        {
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            DataContracts.Attachment fileInfo = new DataContracts.Attachment();
            bool OnPremise = userData.DatabaseKey.Client.OnPremise;
            if (OnPremise)
            {
                fileInfo = objCommonWrapper.DownloadAttachmentOnPremise(_fileinfoId);
                string contentType = MimeMapping.GetMimeMapping(fileInfo.AttachmentURL);
                return File(fileInfo.AttachmentURL, contentType, fileInfo.FileName + '.' + fileInfo.FileType);
            }
            else
            {
                fileInfo = objCommonWrapper.DownloadAttachment(_fileinfoId);
                return Redirect(fileInfo.AttachmentURL);
            }
        }

        #endregion

        #region Labor
        [HttpGet]
        public PartialViewResult AddEditLabor(long WorkOrderId, long TimeCardId = 0)
        {
            DashboardVM objDashboardVM = new DashboardVM();
            DashboardDetailsWrapper objDashboardDetailsWrapper = new DashboardDetailsWrapper(userData);
            Models.Dashboard.LaborModel objLabor = new Models.Dashboard.LaborModel();
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);

            //var PersonnelLookUplist = GetList_Personnel(); //V2-911
            var PersonnelLookUplist = GetList_PersonnelV2();
            objDashboardVM.PersonnelLaborList = PersonnelLookUplist != null ? PersonnelLookUplist.Select(x => new SelectListItem { Text = x.AssignedTo_PersonnelClientLookupId + " - " + x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() }) : null; //PersonnelLookUplist != null ? PersonnelLookUplist.Select(x => new SelectListItem { Text = x.NameFirst + " " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() }) : null;
            if (TimeCardId > 0)
            {
                ViewBag.Mode = "Edit";
                objLabor = objDashboardDetailsWrapper.RetrieveByTimecardid(TimeCardId);
                objLabor.TimecardId = TimeCardId;
            }
            else
            {
                ViewBag.Mode = "Add";
                objLabor.StartDate = DateTime.Now;
            }
            objLabor.WorkOrderId = WorkOrderId;
            //objLabor.ServiceOrderLineItemId = ServiceOrderLineItemId;
            objDashboardVM.LaborModel = objLabor;

            LocalizeControls(objDashboardVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/_AddEditLabor.cshtml", objDashboardVM);
        }

        public ActionResult SaveLabor(DashboardVM dashboardVM)
        {
            if (ModelState.IsValid)
            {
                DashboardDetailsWrapper dashboardDetailsWrapper = new DashboardDetailsWrapper(userData);
                if (dashboardVM.LaborModel.StartDate == null)
                    dashboardVM.LaborModel.StartDate = DateTime.UtcNow;
                Timecard result = new Timecard();
                string Mode = string.Empty;
                if (dashboardVM.LaborModel.TimecardId == 0)
                {
                    Mode = "add";
                    result = dashboardDetailsWrapper.AddLabor(dashboardVM.LaborModel);
                }
                else
                {
                    Mode = "update";
                    result = dashboardDetailsWrapper.UpdateLabor(dashboardVM.LaborModel);
                }
                if (result.ErrorMessages != null && result.ErrorMessages.Count > 0)
                {
                    return Json(result.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), workorderid = dashboardVM.LaborModel.WorkOrderId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult DeleteLabor(long TimeCardId)
        {
            DashboardDetailsWrapper dashboardDetailsWrapper = new DashboardDetailsWrapper(userData);
            var deleteResult = dashboardDetailsWrapper.DeleteLabor(TimeCardId);
            if (deleteResult)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public PartialViewResult LoadLabor()
        {
            DashboardVM objDashboardVM = new DashboardVM();
            objDashboardVM.security = this.userData.Security;
            LocalizeControls(objDashboardVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/_Labor.cshtml", objDashboardVM);
        }

        [HttpPost]
        public string PopulateLabor(int? draw, int? start, int? length, long workOrderId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            start = start.HasValue
          ? start / length
          : 0;
            int skip = start * length ?? 0;

            DashboardDetailsWrapper dashboardDetailsWrapper = new DashboardDetailsWrapper(userData);
            List<Models.Dashboard.LaborModel> laborList = dashboardDetailsWrapper.RetrieveLaborByWorkOrderId(workOrderId, order, orderDir, skip, length ?? 0);
            var totalRecords = 0;
            var recordsFiltered = 0;
            if (laborList != null && laborList.Count > 0)
            {
                recordsFiltered = laborList[0].TotalCount;
                totalRecords = laborList[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }
            int initialPage = start.Value;
            var filteredResult = laborList
              .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, security = userData.Security.MaintenanceCompletionWorkbenchWidget_Complete }, JsonSerializerDateSettings);
        }


        #endregion
        #region Cancel Work order
        public JsonResult CancelJob(long WorkorderId, string CancelReason = null, string Comments = null)
        {
            string result = string.Empty;
            DashboardWrapper dashboardWrapper = new DashboardWrapper(userData);
            List<string> ErrorMessages = dashboardWrapper.updateWorkOrderCancelationStatusforWorkbench(WorkorderId, CancelReason, Comments);
            if (ErrorMessages != null && ErrorMessages.Count > 0)
            {
                return Json(new { data = ErrorMessages }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                result = JsonReturnEnum.success.ToString();
            }
            return Json(new { data = result, WorkorderId = WorkorderId }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Complete WorkOrder 
        public JsonResult CompleteWorkOrder(long WorkOrderId)
        {
            DashboardWrapper dashboardWrapper = new DashboardWrapper(userData);
            List<string> ErrorMessages = dashboardWrapper.updateWorkOrderCompletionStatusforWorkbench(WorkOrderId);
            if (ErrorMessages != null && ErrorMessages.Count > 0)
            {
                return Json(ErrorMessages, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(JsonReturnEnum.success.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region On-Demand WO
        public PartialViewResult AddOnDemandWO(long WorkoderId, string ClientLookupId)
        {
            DashboardVM objDashboardVM = new DashboardVM();
            objDashboardVM.WoEmergencyOnDemandModel = new OnDamandWOModel();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            DashboardDetailsWrapper dashboardDetailsWrapper = new DashboardDetailsWrapper(userData);
            List<DataContracts.LookupList> Type = new List<DataContracts.LookupList>();

            objDashboardVM.WoEmergencyOnDemandModel.WorkOrderId = WorkoderId;
            objDashboardVM._userdata = this.userData;
            objDashboardVM.WorkOrderModel = dashboardDetailsWrapper.GetDashboardWorkOderDetailsById(WorkoderId);
            objDashboardVM.WoEmergencyOnDemandModel.ClientLookupId = objDashboardVM.WorkOrderModel.ClientLookupId;
            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                Type = AllLookUps.Where(x => x.ListName == LookupListConstants.UP_WO_TYPE).ToList();
                if (Type != null)
                {
                    var tmpTypeList = Type.GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).ToList();
                    objDashboardVM.WoEmergencyOnDemandModel.TypeList = tmpTypeList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue });
                }
            }

            List<SelectListItem> defaultChargeToList = new List<SelectListItem>();
            objDashboardVM.WoEmergencyOnDemandModel.ChargeToList = defaultChargeToList;
            var OnDemand = dashboardDetailsWrapper.GetOndemandList();
            objDashboardVM.WoEmergencyOnDemandModel.OnDemandIDList = OnDemand.Select(x => new SelectListItem { Text = x.ClientLookUpId + "   |   " + x.Description, Value = x.ClientLookUpId.ToString() }); ;

            LocalizeControls(objDashboardVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/_OnDemandWOPopUp.cshtml", objDashboardVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddOndemandWorkOrder(DashboardVM dashboardVM)
        {
            if (ModelState.IsValid)
            {
                List<string> ErrorMsgList = new List<string>();
                DashboardVM objVM = new DashboardVM();
                DashboardDetailsWrapper dashboardDetailsWrapper = new DashboardDetailsWrapper(userData);
                objVM.WoEmergencyOnDemandModel = new OnDamandWOModel();

                dashboardVM.WoEmergencyOnDemandModel.IsDepartmentShow = true;
                dashboardVM.WoEmergencyOnDemandModel.IsTypeShow = true;
                dashboardVM.WoEmergencyOnDemandModel.IsDescriptionShow = true;

                var returnObj = dashboardDetailsWrapper.AddOndemandWorkOrder(dashboardVM.WoEmergencyOnDemandModel, ref ErrorMsgList);
                if (ErrorMsgList != null && ErrorMsgList.Count > 0)
                {
                    return Json(ErrorMsgList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    bool IsredirectToSaniPage = (userData.Site.ExternalSanitation == true && userData.Security.SanitationJob.CreateRequest == true) ? true : false;
                    return Json(new { data = JsonReturnEnum.success.ToString(), IsToSanitationPage = IsredirectToSaniPage, workOrderID = returnObj.WorkOrderId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region Describe WO
        public PartialViewResult AddDescribeWO(long WorkoderId, string ClientLookupId)
        {
            DashboardVM objdashboardVM = new DashboardVM();
            objdashboardVM.WoEmergencyDescribeModel = new DescribeWOModel();
            objdashboardVM.WoEmergencyDescribeModel.WorkOrderId = WorkoderId;
            DashboardDetailsWrapper dashboardDetailsWrapper = new DashboardDetailsWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> Type = new List<DataContracts.LookupList>();
            objdashboardVM._userdata = this.userData;
            objdashboardVM.WorkOrderModel = dashboardDetailsWrapper.GetDashboardWorkOderDetailsById(WorkoderId);
            objdashboardVM.WoEmergencyDescribeModel.ClientLookupId = objdashboardVM.WorkOrderModel.ClientLookupId;
            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                Type = AllLookUps.Where(x => x.ListName == LookupListConstants.UP_WO_TYPE).ToList();
                if (Type != null)
                {
                    var tmpTypeList = Type.GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).ToList();
                    objdashboardVM.WoEmergencyDescribeModel.TypeList = tmpTypeList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue });
                }
            }

            List<SelectListItem> defaultChargeToList = new List<SelectListItem>();
            objdashboardVM.WoEmergencyDescribeModel.ChargeToList = defaultChargeToList;

            LocalizeControls(objdashboardVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/_DescribeWOPopUp.cshtml", objdashboardVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddDescribeWorkOrder(DashboardVM dashboardVM)
        {
            if (ModelState.IsValid)
            {
                List<string> ErrorMsgList = new List<string>();
                DashboardVM objVM = new DashboardVM();
                DashboardDetailsWrapper dashboardDetailsWrapper = new DashboardDetailsWrapper(userData);
                objVM.WoEmergencyDescribeModel = new DescribeWOModel();

                dashboardVM.WoEmergencyDescribeModel.IsDepartmentShow = true;
                dashboardVM.WoEmergencyDescribeModel.IsTypeShow = true;
                dashboardVM.WoEmergencyDescribeModel.IsDescriptionShow = true;

                var returnObj = dashboardDetailsWrapper.AddDescribeWorkOrder(dashboardVM.WoEmergencyDescribeModel, ref ErrorMsgList);
                if (ErrorMsgList != null && ErrorMsgList.Count > 0)
                {
                    return Json(ErrorMsgList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    bool IsredirectToSaniPage = (userData.Site.ExternalSanitation == true && userData.Security.SanitationJob.CreateRequest == true) ? true : false;
                    return Json(new { data = JsonReturnEnum.success.ToString(), IsToSanitationPage = IsredirectToSaniPage, workOrderID = returnObj.WorkOrderId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
        #region Add Follow up
        public PartialViewResult AddFollowUpWO(long WorkoderId, string ClientLookupId)
        {
            DashboardVM objDashboardVM = new DashboardVM();
            WorkOrderModel woModel = new WorkOrderModel();
            DashboardDetailsWrapper dashboardDetailsWrapper = new DashboardDetailsWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> Type = new List<DataContracts.LookupList>();
            AllLookUps = commonWrapper.GetAllLookUpList();
            objDashboardVM.WoRequestModel = new DashboardWoRequestModel();
            if (AllLookUps != null)
            {
                Type = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_TYPE).ToList();
                if (Type != null)
                {
                    var tmpTypeList = Type.GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).ToList();
                    woModel.TypeList = tmpTypeList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue });
                }
            }
            objDashboardVM.WorkOrderModel = dashboardDetailsWrapper.GetDashboardWorkOderDetailsById(WorkoderId);
            if (Type != null)
            {
                objDashboardVM.WoRequestModel.TypeList = Type.Select(x => new SelectListItem { Text = x.ListValue + "   |   " + x.Description, Value = x.ListValue.ToString() });
            }
            List<SelectListItem> defaultChargeToList = new List<SelectListItem>();
            objDashboardVM._userdata = this.userData;
            objDashboardVM.WoRequestModel.ChargeToList = defaultChargeToList;
            objDashboardVM.WoRequestModel.WorkOrderId = objDashboardVM.WorkOrderModel.WorkOrderId;
            objDashboardVM.WoRequestModel.ClientLookupId = ClientLookupId;
            objDashboardVM.WoRequestModel.ChargeType = objDashboardVM.WorkOrderModel.ChargeType;
            objDashboardVM.WoRequestModel.Description = objDashboardVM.WorkOrderModel.Description;
            objDashboardVM.WoRequestModel.ChargeToClientLookupId = objDashboardVM.WorkOrderModel.ChargeToClientLookupId;
            objDashboardVM.WoRequestModel.ChargeTo = objDashboardVM.WorkOrderModel.ChargeToId;
            objDashboardVM.WoRequestModel.Type = objDashboardVM.WorkOrderModel.Type;
            LocalizeControls(objDashboardVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/_AddFollowupWO.cshtml", objDashboardVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddFollowUpWorkOrder(DashboardVM objVM)
        {
            string result = string.Empty;
            //WorkOrder obj = new WorkOrder();
            string IsAddOrUpdate = string.Empty;
            if (ModelState.IsValid)
            {
                DashboardDetailsWrapper dashboardDetailsWrapper = new DashboardDetailsWrapper(userData);
                objVM.WoRequestModel.IsDepartmentShow = true;
                objVM.WoRequestModel.IsTypeShow = true;
                objVM.WoRequestModel.IsDescriptionShow = true;

                var Wojob = dashboardDetailsWrapper.FollowUpWorkOrder(objVM.WoRequestModel);
                if (Wojob.ErrorMessages != null && Wojob.ErrorMessages.Count > 0)
                {
                    return Json(Wojob.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Issuccess = true, msg = IsAddOrUpdate, WorkOrderId = Wojob.WorkOrderId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Task

        public PartialViewResult TaskList()
        {
            CommonWrapper comWrapper = new CommonWrapper(userData);
            DashboardVM dashboardVM = new DashboardVM();
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            AllLookUps = comWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                var CancelTaskLookUpList = AllLookUps.Where(x => x.ListName == LookupListConstants.Wo_Task_Cancel).ToList();
                if (CancelTaskLookUpList != null)
                {
                    dashboardVM.dashboardWoTaskModel = new DashboardWoTaskModel();
                    dashboardVM.dashboardWoTaskModel.CancelReasonList = CancelTaskLookUpList.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue.ToString() });
                }
            }
            LocalizeControls(dashboardVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/_WOCompletionWorkBenchTasks.cshtml", dashboardVM);
        }
        [HttpPost]
        public string PopulateTask(int? draw, int? start, int? length, long workOrderId)
        {
            start = start.HasValue
          ? start / length
          : 0;
            int skip = start * length ?? 0;
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            DashboardDetailsWrapper DDWrapper = new DashboardDetailsWrapper(userData);
            List<DashboardWoTaskModel> TaskList = DDWrapper.PopulateTaskV2(workOrderId, order, orderDir, skip, length ?? 0);
            var totalRecords = 0;
            var recordsFiltered = 0;
            if (TaskList != null && TaskList.Count > 0)
            {
                recordsFiltered = TaskList[0].TotalCount;
                totalRecords = TaskList[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }
            int initialPage = start.Value;
            var filteredResult = TaskList
              .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);


        }
        [HttpGet]
        public JsonResult PopulateTaskIds(long workOrderId)
        {

            DashboardDetailsWrapper DDWrapper = new DashboardDetailsWrapper(userData);
            List<DashboardWoTaskModel> TaskList = DDWrapper.PopulateTask(workOrderId);
            return Json(TaskList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CancelTask(string taskList, string cancelReason)
        {
            DashboardDetailsWrapper DDWrapper = new DashboardDetailsWrapper(userData);
            string result = string.Empty;
            int successCount = 0;
            result = DDWrapper.CancelTask(taskList, cancelReason, ref successCount);
            return Json(new { data = result, successcount = successCount }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CompleteTask(string taskList)
        {
            DashboardDetailsWrapper DDWrapper = new DashboardDetailsWrapper(userData);
            string result = string.Empty;
            int successCount = 0;
            result = DDWrapper.CompleteTask(taskList, ref successCount);
            return Json(new { data = result, successcount = successCount }, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region Comments
        [HttpPost]
        public PartialViewResult GetCommentsDetails()
        {
            DashboardVM objDashboardVM = new DashboardVM();
            objDashboardVM._userdata = this.userData;
            LocalizeControls(objDashboardVM, LocalizeResourceSetConstants.DashboardDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/_CommentDetails.cshtml", objDashboardVM);
        }
        [HttpPost]
        public PartialViewResult LoadComments(long WorkOrderId)
        {
            DashboardVM objDashboardVM = new DashboardVM();
            CommonWrapper coWrapper = new CommonWrapper(userData);
            List<Personnel> personnelsList = new List<Personnel>();
            List<Notes> NotesList = new List<Notes>();
            UserMentionData userMentionData;
            List<UserMentionData> userMentionDatas = new List<UserMentionData>();

            Task[] tasks = new Task[2];
            tasks[0] = Task.Factory.StartNew(() => personnelsList = coWrapper.MentionList(""));
            tasks[1] = Task.Factory.StartNew(() => NotesList = coWrapper.PopulateComment(WorkOrderId, "WorkOrder"));
            Task.WaitAll(tasks);

            if (!tasks[0].IsFaulted && tasks[0].IsCompleted)
            {
                foreach (var myuseritem in personnelsList)
                {
                    userMentionData = new UserMentionData();
                    userMentionData.id = myuseritem.UserName;
                    userMentionData.name = myuseritem.FullName;
                    userMentionData.type = myuseritem.PersonnelInitial;
                    userMentionDatas.Add(userMentionData);
                }
                objDashboardVM.userMentionData = userMentionDatas;
            }
            if (!tasks[1].IsFaulted && tasks[1].IsCompleted)
            {
                objDashboardVM.NotesList = NotesList;
            }
            objDashboardVM._userdata = this.userData;
            LocalizeControls(objDashboardVM, LocalizeResourceSetConstants.DashboardDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/_CommentsList.cshtml", objDashboardVM);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddComments(long workOrderId, string content, string woClientLookupId, List<string> userList, long noteId = 0, long updatedindex = 0)
        {
            CommonWrapper coWrapper = new CommonWrapper(userData);
            var namelist = coWrapper.MentionList("");
            List<UserMentionData> userMentionDataList = new List<UserMentionData>();
            UserMentionData objUserMentionData;
            if (userList != null && userList.Count > 0)
            {
                foreach (var item in userList)
                {
                    objUserMentionData = new UserMentionData();//new UserMentionData();
                    objUserMentionData.userId = namelist.Where(x => x.UserName == item).Select(y => y.PersonnelId).FirstOrDefault();
                    objUserMentionData.userName = item;
                    objUserMentionData.emailId = namelist.Where(x => x.UserName == item).Select(y => y.Email).FirstOrDefault();
                    userMentionDataList.Add(objUserMentionData);
                }
            }
            //else
            //{
            //    userMentionDataList.Add(null);
            //}

            Models.NotesModel notesModel = new Models.NotesModel();
            notesModel.ObjectId = workOrderId;
            notesModel.Content = content;
            notesModel.NotesId = noteId;
            notesModel.ClientLookupId = woClientLookupId;
            notesModel.updatedindex = updatedindex;
            if (ModelState.IsValid)
            {
                string Mode = string.Empty;
                List<String> errorList = new List<string>();
                errorList = coWrapper.AddOrUpdateComment(notesModel, ref Mode, "WorkOrder", userMentionDataList);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), workOrderId = workOrderId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Maintenance Completion Workbench Parts
        #region list
        public PartialViewResult PartsList()
        {
            DashboardVM dashboardVM = new DashboardVM();
            dashboardVM.security = userData.Security;
            LocalizeControls(dashboardVM, LocalizeResourceSetConstants.DashboardDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/_WOCompletionWorkbenchPartsListSearch.cshtml", dashboardVM);
        }

        [HttpPost]
        public string GetPartListGrid(int? draw, int? start, int? length, long workOrderId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            DashboardWrapper dashboardWrapper = new DashboardWrapper(userData);
            start = start.HasValue
                ? start / length
                : 0;
            int skip = start * length ?? 0;
            List<PartHistoryModel> PartsList = dashboardWrapper.GetPartListGriddata(workOrderId, order, orderDir, skip, length ?? 0);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = PartsList.Select(o => o.TotalCount).FirstOrDefault();
            totalRecords = PartsList.Select(o => o.TotalCount).FirstOrDefault();

            var filteredResult = PartsList.ToList();


            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeSettings);

        }

        #endregion
        #region add
        [HttpPost]
        public PartialViewResult AddPartIssue()
        {
            var dashboardVM = new DashboardVM();
            var commonWrapper = new CommonWrapper(userData);
            var PartIssueAddModel = new PartIssueAddModel();

            if (userData.DatabaseKey.Client.UseMultiStoreroom)
            {
                PartIssueAddModel.UseMultiStoreroom = userData.DatabaseKey.Client.UseMultiStoreroom;
                dashboardVM.StoreroomList = commonWrapper.GetStoreroomList(StoreroomAuthType.Issue);
            }
            dashboardVM.PartIssueAddModel = PartIssueAddModel;

            LocalizeControls(dashboardVM, LocalizeResourceSetConstants.DashboardDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/_AddPartIssue.cshtml", dashboardVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SavePartIssue(DashboardVM dashboardVM)
        {
            InventoryCheckoutWrapper invWrapper = new InventoryCheckoutWrapper(userData);
            var partList = invWrapper.populatePartDetails(dashboardVM.PartIssueAddModel.PartId, dashboardVM.PartIssueAddModel.StoreroomId ?? 0);
            //long PartStoreroomId = partList.PartStoreroomId; // invWrapper.GetStoreroomId(dashboardVM.PartIssueAddModel.PartId);
            //dashboardVM.PartIssueAddModel.PartStoreroomId = PartStoreroomId;
            if (ModelState.IsValid)
            {
                DashboardDetailsWrapper dashboardDetailsWrapper = new DashboardDetailsWrapper(userData);

                var result = dashboardDetailsWrapper.PartIssueAddData(dashboardVM.PartIssueAddModel, partList.ClientLookupId, partList.Description, partList.UPCCode);
                if (result.Count > 0)
                {
                    return Json(dashboardVM.ErrorMessage, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        [HttpGet]
        public JsonResult GetPartIdByClientLookUpId(string clientLookUpId)
        {
            var commonWrapper = new CommonWrapper(userData);
            var part = commonWrapper.RetrievePartIdByClientLookUp(clientLookUpId);
            return Json(new { PartId = part.PartId, MultiStoreroomError = false }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Add Work Request to Maintenance Completion Workbench
        public PartialViewResult AddWoRequestDynamicInMaintenanceCompletionWorkbench(string mode = null)
        {
            DashboardVM objDashboardVM = new DashboardVM();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            AllLookUps = commonWrapper.GetAllLookUpList();
            objDashboardVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
                                                .Retrieve(DataDictionaryViewNameConstant.AddWorkRequest, userData);
            IList<string> LookupNames = objDashboardVM.UIConfigurationDetails.ToList()
                                     .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
                                     .Select(s => s.LookupName)
                                     .ToList();
            objDashboardVM.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                  .GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault())
                                                  .Select(s => new WOAddUILookupList
                                                  { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                  .ToList();


            objDashboardVM.IsWorkOrderRequest = false; //V2-1052
            if (string.IsNullOrEmpty(mode))
            {
                objDashboardVM.IsWorkOrderRequest = true;
                objDashboardVM.AllRequiredLookUplist.AddRange(AllLookUps.Where(x => x.ListName == LookupListConstants.WR_WO_TYPE)
                                               .GroupBy(x => new { x.ListName, x.ListValue }).Select(x => x.FirstOrDefault())
                                               .Select(s => new WOAddUILookupList
                                               { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                               .ToList());
            }

            objDashboardVM._userdata = this.userData;     // RKL - Missing - Caused error when hitting the add work request button
            objDashboardVM.IsAddWoRequestDynamic = true;
            objDashboardVM.AddWorkRequest = new Models.Work_Order.UIConfiguration.AddWorkRequestModelDynamic();
            objDashboardVM._userdata = this.userData;
            objDashboardVM.BusinessType = this.userData.DatabaseKey.Client.BusinessType.ToUpper();
            objDashboardVM.DateRangeDropList = UtilityFunction.GetTimeRangeDrop().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }); //V2-347
            objDashboardVM.DateRangeDropListForWO = UtilityFunction.GetTimeRangeDropForWO().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }); //V2-347
            objDashboardVM.DateRangeDropListForWOCreatedate = UtilityFunction.GetTimeRangeDropForWOCreateDate().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }); //V2-364

            LocalizeControls(objDashboardVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/_AddWorkRequestDynamicPopUp.cshtml", objDashboardVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveWorkRequestDynamic(DashboardVM dashboardVM, string Command)
        {
            if (ModelState.IsValid)
            {
                List<string> ErrorMsg = new List<string>();
                WorkOrderVM objVM = new WorkOrderVM();
                DashboardDetailsWrapper dashboardDetailsWrapper = new DashboardDetailsWrapper(userData);
                dashboardVM.IsDepartmentShow = true;
                dashboardVM.IsTypeShow = true;
                dashboardVM.IsDescriptionShow = true;
                dashboardVM.ChargeType = ChargeType.Equipment;
                var returnObj = dashboardDetailsWrapper.AddWorkRequestDynamic(dashboardVM, ref ErrorMsg);
                if (ErrorMsg != null && ErrorMsg.Count > 0)
                {
                    return Json(ErrorMsg, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString(), Command = Command, workOrderID = returnObj.WorkOrderId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #endregion

        #region Maintenance Technician Schedule Compliance
        [HttpPost]
        public JsonResult GetWorkOrderScheduleComplianceChartData(int CaseNo = 0)
        {
            WorkOrderScheduleDashboardWrapper woWrapper = new WorkOrderScheduleDashboardWrapper(userData);
            WorkOrderScheduleCompliancedoughnutChartModel dModel = woWrapper.GetWorkOrderScheduleComplianceChartData(userData.DatabaseKey.Personnel.PersonnelId, CaseNo);
            return Json(dModel, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetTimeCardLabourHoursChartData()
        {
            TimeCardDashboardWrapper wrapper = new TimeCardDashboardWrapper(userData);
            scrollbar2dModel engagementDetails = new scrollbar2dModel();
            var result = wrapper.GetTimeCardLabourHoursChartData(userData.DatabaseKey.Personnel.PersonnelId);
            if (result != null)
            {
                engagementDetails = result;
            }
            return Json(engagementDetails, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion
        #region ReturnActualPart V2-624
        public JsonResult ReturnPartSelectedList(WorkOrderVM model)
        {
            InventoryCheckoutWrapper invWrapper = new InventoryCheckoutWrapper(userData);
            WorkOrderWrapper workorderWrapper = new WorkOrderWrapper(userData);
            List<string> errorMessage = new List<string>();

            System.Text.StringBuilder failedWoList = new System.Text.StringBuilder();
            foreach (var item in model.WoPart)
            {
                //var partList = invWrapper.populatePartDetails(item.PartId);
                //dynamic PartStoreroomId = invWrapper.GetStoreroomId(item.PartId);
                //model.PartIssueAddModel.PartStoreroomId = PartStoreroomId;
                model.PartIssueAddModel.PartId = item.PartId;
                model.PartIssueAddModel.Quantity = item.TransactionQuantity;
                model.PartIssueAddModel.Part_ClientLookupId = item.PartClientLookupId;
                model.PartIssueAddModel.Description = item.Description;
                model.PartIssueAddModel.UPCCode = item.UPCCode;
                model.PartIssueAddModel.StoreroomId = item.StoreroomId;
                var result = workorderWrapper.ReturnPartData(model.PartIssueAddModel);
                if (result.Count > 0)
                {
                    string errormessage = "Failed to return part ";
                    errorMessage.Add(errormessage);
                    return Json(model.ErrorMessage, JsonRequestBehavior.AllowGet);
                }
            }
            if (errorMessage.Count > 0 || !string.IsNullOrEmpty(failedWoList.ToString()))
            {
                if (!string.IsNullOrEmpty(failedWoList.ToString()))
                {
                    errorMessage.Add("Failed to return part ");
                }
                return Json(new { data = errorMessage }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Maintenance Completion Workbench Mobile Card View and details
        [HttpPost]
        public PartialViewResult GetWorkOrderCompletionWorkbenchCardViewMobile(int currentpage = 0, int? start = 0, int? length = 0, int CustomQueryDisplayId = 0,
            string txtSearchval = "", string Order = "1", string orderDir = "asc")
        {
            WorkOrderVM objWorkOrderVM = new WorkOrderVM();
            DashboardWrapper dashWrapper = new DashboardWrapper(userData);
            List<string> statusList = new List<string>();

            objWorkOrderVM._userdata = this.userData;

            start = start.HasValue
                ? start / length
                : 0;
            int skip = start * length ?? 0;

            List<WorkOrderModel> woCompletionWorkbench = dashWrapper.populateWOCompletionWorkbenchForGridAndCardView(CustomQueryDisplayId, skip,
                length ?? 0, Order, orderDir, txtSearchval);

            var totalRecords = 0;
            var recordsFiltered = 0;

            recordsFiltered = woCompletionWorkbench.Select(o => o.TotalCount).FirstOrDefault();
            totalRecords = woCompletionWorkbench.Select(o => o.TotalCount).FirstOrDefault();

            var filteredResult = woCompletionWorkbench
                .ToList();

            ViewBag.Start = skip;
            ViewBag.TotalRecords = totalRecords;
            ViewBag.Length = length;
            ViewBag.CurrentPage = currentpage;//start / length + 1;//
            //var filteredResult = cardData.ToList();

            Parallel.ForEach(woCompletionWorkbench, item =>
            {
                item.ClientOnPremise = userData.DatabaseKey.Client.OnPremise;
                item.AzureImageURL = WorkOrderImageUrl(item.WorkOrderId);
                item.security = this.userData.Security;
            });
            objWorkOrderVM.workOrderModelList = woCompletionWorkbench;
            LocalizeControls(objWorkOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/_WorkOrderGridCardView.cshtml", objWorkOrderVM);
        }
        public string WorkOrderImageUrl(long WorkOrderId)
        {
            string ImageUrl = string.Empty;
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            bool ClientOnPremise = userData.DatabaseKey.Client.OnPremise;
            if (ClientOnPremise)
            {
                ImageUrl = objCommonWrapper.GetOnPremiseImageUrl(WorkOrderId, AttachmentTableConstant.WorkOrder);
            }
            else
            {
                ImageUrl = objCommonWrapper.GetAzureImageUrl(WorkOrderId, AttachmentTableConstant.WorkOrder);
            }
            return ImageUrl;

        }
        public PartialViewResult CompletionWorkbench_Details_Mobile(long WorkOrderId, string ClientLookupId)
        {
            DashboardVM dashboardVM = new DashboardVM();
            dashboardVM.security = this.userData.Security;
            WOCompletionWorkbenchSummary WOCompletionWorkbenchSummaryModel = new WOCompletionWorkbenchSummary();
            CommonWrapper comWrapper = new CommonWrapper(userData);
            DashboardWrapper dashboardWrap = new DashboardWrapper(userData);

            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            AllLookUps = comWrapper.GetAllLookUpList();
            dashboardVM.woCompletionDetailsHeader = dashboardWrap.getWorkOderDetailsByIdForCompletionHeader(WorkOrderId);
            if (AllLookUps != null)
            {
                var Priority = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_Priority).ToList();
                if (Priority != null && Priority.Any(cus => cus.ListValue == dashboardVM.woCompletionDetailsHeader.Priority))
                {
                    dashboardVM.woCompletionDetailsHeader.Priority = Priority.Where(x => x.ListValue == dashboardVM.woCompletionDetailsHeader.Priority).Select(x => x.Description).First();
                }
            }

            if (AllLookUps != null)
            {
                var CancelReason = AllLookUps.Where(x => x.ListName == LookupListConstants.Wo_CancelReason).ToList();
                if (CancelReason != null)
                {
                    dashboardVM.CancelReasonListWo = CancelReason.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
                }

            }
            dashboardVM._userdata = this.userData;
            WOCompletionWorkbenchSummaryModel.WorkOrderId = dashboardVM.woCompletionDetailsHeader.WorkOrderId;
            WOCompletionWorkbenchSummaryModel.WorkOrder_ClientLookupId = dashboardVM.woCompletionDetailsHeader.ClientLookupId;
            WOCompletionWorkbenchSummaryModel.Status = dashboardVM.woCompletionDetailsHeader.Status;
            WOCompletionWorkbenchSummaryModel.Description = dashboardVM.woCompletionDetailsHeader.Description;
            WOCompletionWorkbenchSummaryModel.ImageUrl = comWrapper.GetAzureImageUrl(WorkOrderId, AttachmentTableConstant.WorkOrder);
            WOCompletionWorkbenchSummaryModel.Type = dashboardVM.woCompletionDetailsHeader.Type;
            WOCompletionWorkbenchSummaryModel.Priority = dashboardVM.woCompletionDetailsHeader.Priority;
            WOCompletionWorkbenchSummaryModel.ChargeToClientLookupId = dashboardVM.woCompletionDetailsHeader.ChargeToClientLookupId;
            WOCompletionWorkbenchSummaryModel.ChargeTo_Name = dashboardVM.woCompletionDetailsHeader.ChargeTo_Name;
            WOCompletionWorkbenchSummaryModel.Assigned = dashboardVM.woCompletionDetailsHeader.Assigned;
            WOCompletionWorkbenchSummaryModel.WorkAssigned_PersonnelId = dashboardVM.woCompletionDetailsHeader.WorkAssigned_PersonnelId;
            WOCompletionWorkbenchSummaryModel.ScheduledStartDate = dashboardVM.woCompletionDetailsHeader.ScheduledStartDate;
            WOCompletionWorkbenchSummaryModel.RequiredDate = dashboardVM.woCompletionDetailsHeader.RequiredDate;
            WOCompletionWorkbenchSummaryModel.CompleteDate = dashboardVM.woCompletionDetailsHeader.CompleteDate;
            WOCompletionWorkbenchSummaryModel.ChargeToId = dashboardVM.woCompletionDetailsHeader.ChargeToId;
            WOCompletionWorkbenchSummaryModel.AssetLocation = dashboardVM.woCompletionDetailsHeader.Assetlocation;//V2-1012
            WOCompletionWorkbenchSummaryModel.ProjectClientlookupId = dashboardVM.woCompletionDetailsHeader.ProjectClientLookupId; //V2-1012
            #region//*****V2-847

            WOCompletionWorkbenchSummaryModel.AssetGroup1Name = String.IsNullOrEmpty(this.userData.Site.AssetGroup1Name) ? "Asset Group 1" : this.userData.Site.AssetGroup1Name;
            WOCompletionWorkbenchSummaryModel.AssetGroup2Name = String.IsNullOrEmpty(this.userData.Site.AssetGroup2Name) ? "Asset Group 2" : this.userData.Site.AssetGroup2Name;
            WOCompletionWorkbenchSummaryModel.AssetGroup1ClientlookupId = dashboardVM.woCompletionDetailsHeader.AssetGroup1ClientlookupId;
            WOCompletionWorkbenchSummaryModel.AssetGroup2ClientlookupId = dashboardVM.woCompletionDetailsHeader.AssetGroup2ClientlookupId;
            #endregion//*****
            dashboardVM.woCompletionWorkbenchSummary = WOCompletionWorkbenchSummaryModel;

            //-- V2-634 for completion wizard
            WoCompletionSettingsModel completionSettingsModel = new WoCompletionSettingsModel();
            ClientSetUpWrapper clientSetUpWrapper = new ClientSetUpWrapper(userData);
            completionSettingsModel = clientSetUpWrapper.CompletionSettingsDetails();
            dashboardVM.UseWOCompletionWizard = completionSettingsModel.UseWOCompletionWizard;
            //--V2-676
            dashboardVM.WOBarcode = this.userData.Site.WOBarcode;
            //objWorkOrderVM.WOCommentTab = completionSettingsModel.WOCommentTab;
            //objWorkOrderVM.TimecardTab = completionSettingsModel.TimecardTab;
            //objWorkOrderVM.AutoAddTimecard = completionSettingsModel.AutoAddTimecard;

            //if (objWorkOrderVM.UseWOCompletionWizard == true)
            //{
            //    objWorkOrderVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration().Retrieve(DataDictionaryViewNameConstant.WorkOrderCompletion, userData);
            //    objWorkOrderVM.CompletionModelDynamic = new WorkOrderCompletionInformationModelDynamic();
            //    objWorkOrderVM.WorkOrderCompletionWizard = new WorkOrderCompletionWizard();

            //    objWorkOrderVM.WorkOrderCompletionWizard.UseWOCompletionWizard = completionSettingsModel.UseWOCompletionWizard;
            //    objWorkOrderVM.WorkOrderCompletionWizard.WOCommentTab = completionSettingsModel.WOCommentTab;
            //    objWorkOrderVM.WorkOrderCompletionWizard.TimecardTab = completionSettingsModel.TimecardTab;
            //    objWorkOrderVM.WorkOrderCompletionWizard.AutoAddTimecard = completionSettingsModel.AutoAddTimecard;

            //    IList<string> LookupNames = objWorkOrderVM.UIConfigurationDetails.ToList()
            //                           .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
            //                           .Select(s => s.LookupName)
            //                           .ToList();

            //    objWorkOrderVM.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
            //                                          .GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault())
            //                                          .Select(s => new WOAddUILookupList
            //                                          { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
            //                                          .ToList();

            //}
            //-- V2-634 

            LocalizeControls(dashboardVM, LocalizeResourceSetConstants.DashboardDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/_WOCompletionWorkbenchDetails.cshtml", dashboardVM);
        }

        #region Labor
        [HttpGet]
        public PartialViewResult AddEditLabor_Mobile(long WorkOrderId, long TimeCardId = 0)
        {
            DashboardVM objDashboardVM = new DashboardVM();
            DashboardDetailsWrapper objDashboardDetailsWrapper = new DashboardDetailsWrapper(userData);
            Models.Dashboard.LaborModel objLabor = new Models.Dashboard.LaborModel();
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);

            var PersonnelLookUplist = GetList_Personnel();
            objDashboardVM.PersonnelLaborList = PersonnelLookUplist != null ? PersonnelLookUplist.Select(x => new SelectListItem { Text = x.AssignedTo_PersonnelClientLookupId + " - " + x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() }) : null; //PersonnelLookUplist != null ? PersonnelLookUplist.Select(x => new SelectListItem { Text = x.NameFirst + " " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() }) : null;
            if (TimeCardId > 0)
            {
                ViewBag.Mode = "Edit";
                objLabor = objDashboardDetailsWrapper.RetrieveByTimecardid(TimeCardId);
                objLabor.TimecardId = TimeCardId;
            }
            else
            {
                ViewBag.Mode = "Add";
                objLabor.StartDate = DateTime.Now;
            }
            objLabor.WorkOrderId = WorkOrderId;
            //objLabor.ServiceOrderLineItemId = ServiceOrderLineItemId;
            objDashboardVM.LaborModel = objLabor;

            LocalizeControls(objDashboardVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/_AddEditLabor.cshtml", objDashboardVM);
        }

        public ActionResult SaveLabor_Mobile(DashboardVM dashboardVM)
        {
            if (ModelState.IsValid)
            {
                DashboardDetailsWrapper dashboardDetailsWrapper = new DashboardDetailsWrapper(userData);
                if (dashboardVM.LaborModel.StartDate == null)
                    dashboardVM.LaborModel.StartDate = DateTime.UtcNow;
                Timecard result = new Timecard();
                string Mode = string.Empty;
                if (dashboardVM.LaborModel.TimecardId == 0)
                {
                    Mode = "add";
                    result = dashboardDetailsWrapper.AddLabor(dashboardVM.LaborModel);
                }
                else
                {
                    Mode = "update";
                    result = dashboardDetailsWrapper.UpdateLabor(dashboardVM.LaborModel);
                }
                if (result.ErrorMessages != null && result.ErrorMessages.Count > 0)
                {
                    return Json(result.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), workorderid = dashboardVM.LaborModel.WorkOrderId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult DeleteLabor_Mobile(long TimeCardId)
        {
            DashboardDetailsWrapper dashboardDetailsWrapper = new DashboardDetailsWrapper(userData);
            var deleteResult = dashboardDetailsWrapper.DeleteLabor(TimeCardId);
            if (deleteResult)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public PartialViewResult LoadLabor_Mobile()
        {
            DashboardVM objDashboardVM = new DashboardVM();
            objDashboardVM.security = this.userData.Security;
            LocalizeControls(objDashboardVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/_Labor.cshtml", objDashboardVM);
        }

        [HttpPost]
        public string PopulateLabor_Mobile(int? draw, int? start, int? length, long workOrderId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            start = start.HasValue
          ? start / length
          : 0;
            int skip = start * length ?? 0;

            DashboardDetailsWrapper dashboardDetailsWrapper = new DashboardDetailsWrapper(userData);
            List<Models.Dashboard.LaborModel> laborList = dashboardDetailsWrapper.RetrieveLaborByWorkOrderId(workOrderId, order, orderDir, skip, length ?? 0);
            var totalRecords = 0;
            var recordsFiltered = 0;
            if (laborList != null && laborList.Count > 0)
            {
                recordsFiltered = laborList[0].TotalCount;
                totalRecords = laborList[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }
            int initialPage = start.Value;
            var filteredResult = laborList
              .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, security = userData.Security.MaintenanceCompletionWorkbenchWidget_Complete }, JsonSerializerDateSettings);
        }
        #endregion

        #region Parts
        public PartialViewResult PartLookupListView_Mobile()
        {
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/_AddPartIdPopupSearchGrid.cshtml");
        }
        public JsonResult GetPartLookupListchunksearch_Mobile(int Start, int Length, string Search = "", string Storeroomid = "")//ClientLookupId = "", string Description = "")
        {
            var modelList = new List<PartXRefGridDataModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            //ScrollViewModel<PartScrollViewModel> scrollViewModel = new ScrollViewModel<PartScrollViewModel>();
            //List<PartScrollViewModel> partScrollViewModels = new List<PartScrollViewModel>();
            //var Pagenos = new List<int>();
            //var partScrollViewModel = new PartScrollViewModel();

            string order = "0"; //Request.Form.GetValues("order[0][column]")[0];
            string orderDir = "asc"; //Request.Form.GetValues("order[0][dir]")[0];

            modelList = commonWrapper.GetPartLookupListGridData_Mobile(order, orderDir, Start, Length, Search, Search, Storeroomid);

            return Json(new { recordsTotal = modelList.Count() > 0 ? modelList[0].TotalCount : 0, data = modelList });
        }
        public PartialViewResult PartsList_Mobile()
        {
            DashboardVM dashboardVM = new DashboardVM();
            dashboardVM.security = userData.Security;
            LocalizeControls(dashboardVM, LocalizeResourceSetConstants.DashboardDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/_WOCompletionWorkbenchPartsListSearch.cshtml", dashboardVM);
        }

        [HttpPost]
        public string GetPartListGrid_Mobile(int? draw, int? start, int? length, long workOrderId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            DashboardWrapper dashboardWrapper = new DashboardWrapper(userData);
            start = start.HasValue
                ? start / length
                : 0;
            int skip = start * length ?? 0;
            List<PartHistoryModel> PartsList = dashboardWrapper.GetPartListGriddata(workOrderId, order, orderDir, skip, length ?? 0);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = PartsList.Select(o => o.TotalCount).FirstOrDefault();
            totalRecords = PartsList.Select(o => o.TotalCount).FirstOrDefault();

            var filteredResult = PartsList.ToList();


            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeSettings);

        }
        [HttpPost]
        public PartialViewResult AddPartIssue_Mobile()
        {
            var dashboardVM = new DashboardVM();
            var commonWrapper = new CommonWrapper(userData);
            var PartIssueAddModel = new PartIssueAddModel();

            if (userData.DatabaseKey.Client.UseMultiStoreroom)
            {
                PartIssueAddModel.UseMultiStoreroom = userData.DatabaseKey.Client.UseMultiStoreroom;
                dashboardVM.StoreroomList = commonWrapper.GetStoreroomList(StoreroomAuthType.Issue);
            }
            dashboardVM.PartIssueAddModel = PartIssueAddModel;

            LocalizeControls(dashboardVM, LocalizeResourceSetConstants.DashboardDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/_AddPartIssue.cshtml", dashboardVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SavePartIssue_Mobile(DashboardVM dashboardVM)
        {
            InventoryCheckoutWrapper invWrapper = new InventoryCheckoutWrapper(userData);
            var partList = invWrapper.populatePartDetails(dashboardVM.PartIssueAddModel.PartId, dashboardVM.PartIssueAddModel.StoreroomId ?? 0);
            //dynamic PartStoreroomId = invWrapper.GetStoreroomId(dashboardVM.PartIssueAddModel.PartId);
            //long PartStoreroomId = partList.PartStoreroomId;
            //dashboardVM.PartIssueAddModel.PartStoreroomId = PartStoreroomId;
            if (ModelState.IsValid)
            {
                DashboardDetailsWrapper dashboardDetailsWrapper = new DashboardDetailsWrapper(userData);

                var result = dashboardDetailsWrapper.PartIssueAddData(dashboardVM.PartIssueAddModel, partList.ClientLookupId, partList.Description, partList.UPCCode);
                if (result.Count > 0)
                {
                    return Json(dashboardVM.ErrorMessage, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult GetPartIdByClientLookUpId_Mobile(string clientLookUpId)
        {
            var commonWrapper = new CommonWrapper(userData);
            var part = commonWrapper.RetrievePartIdByClientLookUp(clientLookUpId);
            return Json(new { PartId = part.PartId, MultiStoreroomError = false }, JsonRequestBehavior.AllowGet);
        }

        #region Return Actual Part V2-624
        public JsonResult ReturnPartSelectedList_Mobile(WorkOrderVM model)
        {
            InventoryCheckoutWrapper invWrapper = new InventoryCheckoutWrapper(userData);
            WorkOrderWrapper workorderWrapper = new WorkOrderWrapper(userData);
            List<string> errorMessage = new List<string>();

            System.Text.StringBuilder failedWoList = new System.Text.StringBuilder();
            foreach (var item in model.WoPart)
            {
                //var partList = invWrapper.populatePartDetails(item.PartId);
                //dynamic PartStoreroomId = invWrapper.GetStoreroomId(item.PartId);
                // model.PartIssueAddModel.PartStoreroomId = PartStoreroomId;
                model.PartIssueAddModel.PartId = item.PartId;
                model.PartIssueAddModel.Quantity = item.TransactionQuantity;
                model.PartIssueAddModel.Part_ClientLookupId = item.PartClientLookupId;
                model.PartIssueAddModel.Description = item.Description;
                model.PartIssueAddModel.UPCCode = item.UPCCode;
                model.PartIssueAddModel.StoreroomId = item.StoreroomId;
                var result = workorderWrapper.ReturnPartData(model.PartIssueAddModel);
                if (result.Count > 0)
                {
                    string errormessage = "Failed to return part ";
                    errorMessage.Add(errormessage);
                    return Json(model.ErrorMessage, JsonRequestBehavior.AllowGet);
                }
            }
            if (errorMessage.Count > 0 || !string.IsNullOrEmpty(failedWoList.ToString()))
            {
                if (!string.IsNullOrEmpty(failedWoList.ToString()))
                {
                    errorMessage.Add("Failed to return part ");
                }
                return Json(new { data = errorMessage }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #endregion

        #region Task
        public PartialViewResult TaskList_Mobile()
        {
            CommonWrapper comWrapper = new CommonWrapper(userData);
            DashboardVM dashboardVM = new DashboardVM();
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            AllLookUps = comWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                var CancelTaskLookUpList = AllLookUps.Where(x => x.ListName == LookupListConstants.Wo_Task_Cancel).ToList();
                if (CancelTaskLookUpList != null)
                {
                    dashboardVM.dashboardWoTaskModel = new DashboardWoTaskModel();
                    dashboardVM.dashboardWoTaskModel.CancelReasonList = CancelTaskLookUpList.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue.ToString() });
                }
            }
            LocalizeControls(dashboardVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/_WOCompletionWorkBenchTasks.cshtml", dashboardVM);
        }
        [HttpPost]
        public string PopulateTask_Mobile(int? draw, int? start, int? length, long workOrderId)
        {
            start = start.HasValue
          ? start / length
          : 0;
            int skip = start * length ?? 0;
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            DashboardDetailsWrapper DDWrapper = new DashboardDetailsWrapper(userData);
            List<DashboardWoTaskModel> TaskList = DDWrapper.PopulateTaskV2(workOrderId, order, orderDir, skip, length ?? 0);
            var totalRecords = 0;
            var recordsFiltered = 0;
            if (TaskList != null && TaskList.Count > 0)
            {
                recordsFiltered = TaskList[0].TotalCount;
                totalRecords = TaskList[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }
            int initialPage = start.Value;
            var filteredResult = TaskList
              .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);


        }
        [HttpGet]
        public JsonResult PopulateTaskIds_Mobile(long workOrderId)
        {

            DashboardDetailsWrapper DDWrapper = new DashboardDetailsWrapper(userData);
            List<DashboardWoTaskModel> TaskList = DDWrapper.PopulateTask(workOrderId);
            return Json(TaskList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CancelTask_Mobile(string taskList, string cancelReason)
        {
            DashboardDetailsWrapper DDWrapper = new DashboardDetailsWrapper(userData);
            string result = string.Empty;
            int successCount = 0;
            result = DDWrapper.CancelTask(taskList, cancelReason, ref successCount);
            return Json(new { data = result, successcount = successCount }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CompleteTask_Mobile(string taskList)
        {
            DashboardDetailsWrapper DDWrapper = new DashboardDetailsWrapper(userData);
            string result = string.Empty;
            int successCount = 0;
            result = DDWrapper.CompleteTask(taskList, ref successCount);
            return Json(new { data = result, successcount = successCount }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Instructions
        [HttpPost]
        public PartialViewResult LoadIntructions_Mobile(long WorkOrderId)
        {
            DashboardVM dashboardVM = new DashboardVM();
            CommonWrapper coWrapper = new CommonWrapper(userData);
            var instructions = coWrapper.PopulateInstructions(WorkOrderId, "WorkOrder");
            dashboardVM.InstructionModel = new InstructionModel();
            if (instructions != null && instructions.Count() > 0)
            {
                dashboardVM.InstructionModel = instructions.FirstOrDefault();
            }
            LocalizeControls(dashboardVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/_InstructionDetails.cshtml", dashboardVM);
        }
        #endregion

        #region Comments
        [HttpPost]
        public PartialViewResult GetCommentsDetails_Mobile()
        {
            DashboardVM objDashboardVM = new DashboardVM();
            objDashboardVM._userdata = this.userData;
            LocalizeControls(objDashboardVM, LocalizeResourceSetConstants.DashboardDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/_CommentDetails.cshtml", objDashboardVM);
        }
        [HttpPost]
        public PartialViewResult LoadComments_Mobile(long WorkOrderId)
        {
            DashboardVM objDashboardVM = new DashboardVM();
            CommonWrapper coWrapper = new CommonWrapper(userData);
            List<Personnel> personnelsList = new List<Personnel>();
            List<Notes> NotesList = new List<Notes>();
            UserMentionData userMentionData;
            List<UserMentionData> userMentionDatas = new List<UserMentionData>();

            Task[] tasks = new Task[2];
            tasks[0] = Task.Factory.StartNew(() => personnelsList = coWrapper.MentionList(""));
            tasks[1] = Task.Factory.StartNew(() => NotesList = coWrapper.PopulateComment(WorkOrderId, "WorkOrder"));
            Task.WaitAll(tasks);

            if (!tasks[0].IsFaulted && tasks[0].IsCompleted)
            {
                foreach (var myuseritem in personnelsList)
                {
                    userMentionData = new UserMentionData();
                    userMentionData.id = myuseritem.UserName;
                    userMentionData.name = myuseritem.FullName;
                    userMentionData.type = myuseritem.PersonnelInitial;
                    userMentionDatas.Add(userMentionData);
                }
                objDashboardVM.userMentionData = userMentionDatas;
            }
            if (!tasks[1].IsFaulted && tasks[1].IsCompleted)
            {
                objDashboardVM.NotesList = NotesList;
            }
            objDashboardVM._userdata = this.userData;
            LocalizeControls(objDashboardVM, LocalizeResourceSetConstants.DashboardDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/_CommentsList.cshtml", objDashboardVM);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddComments_Mobile(long workOrderId, string content, string woClientLookupId, List<string> userList, long noteId = 0, long updatedindex = 0)
        {
            CommonWrapper coWrapper = new CommonWrapper(userData);
            var namelist = coWrapper.MentionList("");
            List<UserMentionData> userMentionDataList = new List<UserMentionData>();
            UserMentionData objUserMentionData;
            if (userList != null && userList.Count > 0)
            {
                foreach (var item in userList)
                {
                    objUserMentionData = new UserMentionData();//new UserMentionData();
                    objUserMentionData.userId = namelist.Where(x => x.UserName == item).Select(y => y.PersonnelId).FirstOrDefault();
                    objUserMentionData.userName = item;
                    objUserMentionData.emailId = namelist.Where(x => x.UserName == item).Select(y => y.Email).FirstOrDefault();
                    userMentionDataList.Add(objUserMentionData);
                }
            }
            //else
            //{
            //    userMentionDataList.Add(null);
            //}

            Models.NotesModel notesModel = new Models.NotesModel();
            notesModel.ObjectId = workOrderId;
            notesModel.Content = content;
            notesModel.NotesId = noteId;
            notesModel.ClientLookupId = woClientLookupId;
            notesModel.updatedindex = updatedindex;
            if (ModelState.IsValid)
            {
                string Mode = string.Empty;
                List<String> errorList = new List<string>();
                errorList = coWrapper.AddOrUpdateComment(notesModel, ref Mode, "WorkOrder", userMentionDataList);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), workOrderId = workOrderId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Attachment
        [HttpPost]
        public PartialViewResult LoadAttachment_Mobile()
        {
            DashboardVM dashboardVM = new DashboardVM();
            LocalizeControls(dashboardVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/_AttachmentDetails.cshtml", dashboardVM);
        }
        [HttpPost]
        public string PopulateAttachment_Mobile(int? draw, int? start, int? length, long workOrderId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            var AttachmentList = objCommonWrapper.PopulateAttachments(workOrderId, "WorkOrder", userData.Security.WorkOrders.Edit);
            if (AttachmentList != null)
            {
                AttachmentList = GetAllAttachmentsSortByColumnWithOrder(order, orderDir, AttachmentList);
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = AttachmentList.Count();
            totalRecords = AttachmentList.Count();
            int initialPage = start.Value;
            var filteredResult = AttachmentList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeSettings);
        }
        public ActionResult DownloadAttachment_Mobile(long _fileinfoId)
        {
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            DataContracts.Attachment fileInfo = new DataContracts.Attachment();
            bool OnPremise = userData.DatabaseKey.Client.OnPremise;
            if (OnPremise)
            {
                fileInfo = objCommonWrapper.DownloadAttachmentOnPremise(_fileinfoId);
                string contentType = MimeMapping.GetMimeMapping(fileInfo.AttachmentURL);
                return File(fileInfo.AttachmentURL, contentType, fileInfo.FileName + '.' + fileInfo.FileType);
            }
            else
            {
                fileInfo = objCommonWrapper.DownloadAttachment(_fileinfoId);
                return Redirect(fileInfo.AttachmentURL);
            }
        }

        #endregion        

        #region Cancel Work order
        public JsonResult CancelJob_Mobile(long WorkorderId, string CancelReason = null, string Comments = null)
        {
            string result = string.Empty;
            DashboardWrapper dashboardWrapper = new DashboardWrapper(userData);
            List<string> ErrorMessages = dashboardWrapper.updateWorkOrderCancelationStatusforWorkbench(WorkorderId, CancelReason, Comments);
            if (ErrorMessages != null && ErrorMessages.Count > 0)
            {
                return Json(new { data = ErrorMessages }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                result = JsonReturnEnum.success.ToString();
            }
            return Json(new { data = result, WorkorderId = WorkorderId }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Complete WorkOrder 
        public JsonResult CompleteWorkOrder_Mobile(long WorkOrderId)
        {
            DashboardWrapper dashboardWrapper = new DashboardWrapper(userData);
            List<string> ErrorMessages = dashboardWrapper.updateWorkOrderCompletionStatusforWorkbench(WorkOrderId);
            if (ErrorMessages != null && ErrorMessages.Count > 0)
            {
                return Json(ErrorMessages, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(JsonReturnEnum.success.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Add Work request
        public PartialViewResult AddWoRequestDynamicInMaintenanceCompletionWorkbench_Mobile(string mode = null)
        {
            DashboardVM objDashboardVM = new DashboardVM();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            AllLookUps = commonWrapper.GetAllLookUpList();
            objDashboardVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
                                                .Retrieve(DataDictionaryViewNameConstant.AddWorkRequest, userData);
            IList<string> LookupNames = objDashboardVM.UIConfigurationDetails.ToList()
                                     .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
                                     .Select(s => s.LookupName)
                                     .ToList();
            objDashboardVM.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                  .GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault())
                                                  .Select(s => new WOAddUILookupList
                                                  { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                  .ToList();

            objDashboardVM.IsWorkOrderRequest = false; //V2-1052
            if (string.IsNullOrEmpty(mode))
            {
                objDashboardVM.IsWorkOrderRequest = true;
                objDashboardVM.AllRequiredLookUplist.AddRange(AllLookUps.Where(x => x.ListName == LookupListConstants.WR_WO_TYPE)
                                               .GroupBy(x => new { x.ListName, x.ListValue }).Select(x => x.FirstOrDefault())
                                               .Select(s => new WOAddUILookupList
                                               { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                               .ToList());
            }


            objDashboardVM._userdata = this.userData;     // RKL - Missing - Caused error when hitting the add work request button
            objDashboardVM.IsAddWoRequestDynamic = true;
            objDashboardVM.AddWorkRequest = new Models.Work_Order.UIConfiguration.AddWorkRequestModelDynamic();
            objDashboardVM._userdata = this.userData;
            objDashboardVM.BusinessType = this.userData.DatabaseKey.Client.BusinessType.ToUpper();
            objDashboardVM.DateRangeDropList = UtilityFunction.GetTimeRangeDrop().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }); //V2-347
            objDashboardVM.DateRangeDropListForWO = UtilityFunction.GetTimeRangeDropForWO().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }); //V2-347
            objDashboardVM.DateRangeDropListForWOCreatedate = UtilityFunction.GetTimeRangeDropForWOCreateDate().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }); //V2-364

            LocalizeControls(objDashboardVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/_AddWorkRequestDynamicPopUp.cshtml", objDashboardVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveWorkRequestDynamic_Mobile(DashboardVM dashboardVM, string Command)
        {
            if (ModelState.IsValid)
            {
                List<string> ErrorMsg = new List<string>();
                WorkOrderVM objVM = new WorkOrderVM();
                DashboardDetailsWrapper dashboardDetailsWrapper = new DashboardDetailsWrapper(userData);
                dashboardVM.IsDepartmentShow = true;
                dashboardVM.IsTypeShow = true;
                dashboardVM.IsDescriptionShow = true;
                dashboardVM.ChargeType = ChargeType.Equipment;
                var returnObj = dashboardDetailsWrapper.AddWorkRequestDynamic(dashboardVM, ref ErrorMsg);
                if (ErrorMsg != null && ErrorMsg.Count > 0)
                {
                    return Json(ErrorMsg, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString(), Command = Command, workOrderID = returnObj.WorkOrderId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Add unplanned On-Demand WO
        //public PartialViewResult AddOnDemandWO_Mobile(long WorkoderId, string ClientLookupId) sweta
        public PartialViewResult AddOnDemandWO_Mobile()
        {
            DashboardVM objDashboardVM = new DashboardVM();
            objDashboardVM.WoEmergencyOnDemandModel = new OnDamandWOModel();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            DashboardDetailsWrapper dashboardDetailsWrapper = new DashboardDetailsWrapper(userData);
            List<DataContracts.LookupList> Type = new List<DataContracts.LookupList>();

            //objDashboardVM.WoEmergencyOnDemandModel.WorkOrderId = WorkoderId;
            objDashboardVM._userdata = this.userData;
            //objDashboardVM.WorkOrderModel = dashboardDetailsWrapper.GetDashboardWorkOderDetailsById(WorkoderId);
            //objDashboardVM.WoEmergencyOnDemandModel.ClientLookupId = objDashboardVM.WorkOrderModel.ClientLookupId;
            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                Type = AllLookUps.Where(x => x.ListName == LookupListConstants.UP_WO_TYPE).ToList();
                if (Type != null)
                {
                    var tmpTypeList = Type.GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).ToList();
                    objDashboardVM.WoEmergencyOnDemandModel.TypeList = tmpTypeList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue });
                }
            }

            List<SelectListItem> defaultChargeToList = new List<SelectListItem>();
            objDashboardVM.WoEmergencyOnDemandModel.ChargeToList = defaultChargeToList;
            var OnDemand = dashboardDetailsWrapper.GetOndemandList();
            objDashboardVM.WoEmergencyOnDemandModel.OnDemandIDList = OnDemand.Select(x => new SelectListItem { Text = x.ClientLookUpId + "   |   " + x.Description, Value = x.ClientLookUpId.ToString() }); ;

            LocalizeControls(objDashboardVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/_OnDemandWOPopUp.cshtml", objDashboardVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddOndemandWorkOrder_Mobile(DashboardVM dashboardVM)
        {
            if (ModelState.IsValid)
            {
                List<string> ErrorMsgList = new List<string>();
                DashboardVM objVM = new DashboardVM();
                DashboardDetailsWrapper dashboardDetailsWrapper = new DashboardDetailsWrapper(userData);
                objVM.WoEmergencyOnDemandModel = new OnDamandWOModel();

                dashboardVM.WoEmergencyOnDemandModel.IsDepartmentShow = true;
                dashboardVM.WoEmergencyOnDemandModel.IsTypeShow = true;
                dashboardVM.WoEmergencyOnDemandModel.IsDescriptionShow = true;

                var returnObj = dashboardDetailsWrapper.AddOndemandWorkOrder(dashboardVM.WoEmergencyOnDemandModel, ref ErrorMsgList);
                if (ErrorMsgList != null && ErrorMsgList.Count > 0)
                {
                    return Json(ErrorMsgList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    bool IsredirectToSaniPage = (userData.Site.ExternalSanitation == true && userData.Security.SanitationJob.CreateRequest == true) ? true : false;
                    return Json(new { data = JsonReturnEnum.success.ToString(), IsToSanitationPage = IsredirectToSaniPage, workOrderID = returnObj.WorkOrderId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Add unplanned Describe WO
        public PartialViewResult AddDescribeWO_Mobile()
        {
            DashboardVM objdashboardVM = new DashboardVM();
            objdashboardVM.WoEmergencyDescribeModel = new DescribeWOModel();
            //objdashboardVM.WoEmergencyDescribeModel.WorkOrderId = WorkoderId;
            DashboardDetailsWrapper dashboardDetailsWrapper = new DashboardDetailsWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> Type = new List<DataContracts.LookupList>();
            objdashboardVM._userdata = this.userData;
            //objdashboardVM.WorkOrderModel = dashboardDetailsWrapper.GetDashboardWorkOderDetailsById(WorkoderId);
            //objdashboardVM.WoEmergencyDescribeModel.ClientLookupId = objdashboardVM.WorkOrderModel.ClientLookupId;
            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                Type = AllLookUps.Where(x => x.ListName == LookupListConstants.UP_WO_TYPE).ToList();
                if (Type != null)
                {
                    var tmpTypeList = Type.GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).ToList();
                    objdashboardVM.WoEmergencyDescribeModel.TypeList = tmpTypeList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue });
                }
            }

            List<SelectListItem> defaultChargeToList = new List<SelectListItem>();
            objdashboardVM.WoEmergencyDescribeModel.ChargeToList = defaultChargeToList;

            LocalizeControls(objdashboardVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/_DescribeWOPopUp.cshtml", objdashboardVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddDescribeWorkOrder_Mobile(DashboardVM dashboardVM)
        {
            if (ModelState.IsValid)
            {
                List<string> ErrorMsgList = new List<string>();
                DashboardVM objVM = new DashboardVM();
                DashboardDetailsWrapper dashboardDetailsWrapper = new DashboardDetailsWrapper(userData);
                objVM.WoEmergencyDescribeModel = new DescribeWOModel();

                dashboardVM.WoEmergencyDescribeModel.IsDepartmentShow = true;
                dashboardVM.WoEmergencyDescribeModel.IsTypeShow = true;
                dashboardVM.WoEmergencyDescribeModel.IsDescriptionShow = true;

                var returnObj = dashboardDetailsWrapper.AddDescribeWorkOrder(dashboardVM.WoEmergencyDescribeModel, ref ErrorMsgList);
                if (ErrorMsgList != null && ErrorMsgList.Count > 0)
                {
                    return Json(ErrorMsgList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    bool IsredirectToSaniPage = (userData.Site.ExternalSanitation == true && userData.Security.SanitationJob.CreateRequest == true) ? true : false;
                    return Json(new { data = JsonReturnEnum.success.ToString(), IsToSanitationPage = IsredirectToSaniPage, workOrderID = returnObj.WorkOrderId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Add Follow up WO
        public PartialViewResult AddFollowUpWO_Mobile(long WorkoderId, string ClientLookupId)
        {
            DashboardVM objDashboardVM = new DashboardVM();
            WorkOrderModel woModel = new WorkOrderModel();
            DashboardDetailsWrapper dashboardDetailsWrapper = new DashboardDetailsWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> Type = new List<DataContracts.LookupList>();
            AllLookUps = commonWrapper.GetAllLookUpList();
            objDashboardVM.WoRequestModel = new DashboardWoRequestModel();
            if (AllLookUps != null)
            {
                Type = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_TYPE).ToList();
                if (Type != null)
                {
                    var tmpTypeList = Type.GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault()).ToList();
                    woModel.TypeList = tmpTypeList.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue });
                }
            }
            objDashboardVM.WorkOrderModel = dashboardDetailsWrapper.GetDashboardWorkOderDetailsById(WorkoderId);
            if (Type != null)
            {
                objDashboardVM.WoRequestModel.TypeList = Type.Select(x => new SelectListItem { Text = x.ListValue + "   |   " + x.Description, Value = x.ListValue.ToString() });
            }
            List<SelectListItem> defaultChargeToList = new List<SelectListItem>();
            objDashboardVM._userdata = this.userData;
            objDashboardVM.WoRequestModel.ChargeToList = defaultChargeToList;
            objDashboardVM.WoRequestModel.WorkOrderId = objDashboardVM.WorkOrderModel.WorkOrderId;
            objDashboardVM.WoRequestModel.ClientLookupId = ClientLookupId;
            objDashboardVM.WoRequestModel.ChargeType = objDashboardVM.WorkOrderModel.ChargeType;
            objDashboardVM.WoRequestModel.Description = objDashboardVM.WorkOrderModel.Description;
            objDashboardVM.WoRequestModel.ChargeToClientLookupId = objDashboardVM.WorkOrderModel.ChargeToClientLookupId;
            objDashboardVM.WoRequestModel.Type = objDashboardVM.WorkOrderModel.Type;
            LocalizeControls(objDashboardVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/_AddFollowupWO.cshtml", objDashboardVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddFollowUpWorkOrder_Mobile(DashboardVM objVM)
        {
            string result = string.Empty;
            //WorkOrder obj = new WorkOrder();
            string IsAddOrUpdate = string.Empty;
            if (ModelState.IsValid)
            {
                DashboardDetailsWrapper dashboardDetailsWrapper = new DashboardDetailsWrapper(userData);
                objVM.WoRequestModel.IsDepartmentShow = true;
                objVM.WoRequestModel.IsTypeShow = true;
                objVM.WoRequestModel.IsDescriptionShow = true;

                var Wojob = dashboardDetailsWrapper.FollowUpWorkOrder(objVM.WoRequestModel);
                if (Wojob.ErrorMessages != null && Wojob.ErrorMessages.Count > 0)
                {
                    return Json(Wojob.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Issuccess = true, msg = IsAddOrUpdate, WorkOrderId = Wojob.WorkOrderId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        public PartialViewResult EquipmentLookupListView_Mobile()
        {
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/_EquipmentGridPopUp.cshtml");
        }
        [HttpPost]
        public JsonResult GetEquipmentLookupListchunksearch_Mobile(int Start, int Length, string Search = "") //string clientLookupId = "", string name = "")
        {
            var modelList = new List<EquipmentLookupModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);

            string order = "0"; //Request.Form.GetValues("order[0][column]")[0];
            string orderDir = "asc"; //Request.Form.GetValues("order[0][dir]")[0];

            modelList = commonWrapper.GetEquipmentLookupListGridDataMobile(order, orderDir, Start, Length, Search, Search);
            // commnet for new implementation
            //modelList = modelList.Skip(Start).Take(Length).ToList();
            return Json(new { recordsTotal = modelList.Count() > 0 ? modelList[0].TotalCount : 0, data = modelList });
        }

        public PartialViewResult GetAccountLookupList_Mobile()
        {
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/_AccountGridPopUp.cshtml");
        }

        [HttpPost]
        public JsonResult GetAccountLookupListchunksearch_Mobile(int Start, int Length, string Search = "")
        {
            var modelList = new List<AccountLookUpModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);

            string order = "0";
            string orderDir = "asc";

            modelList = commonWrapper.GetAccountLookupListGridDataMobile(order, orderDir, Start, Length, Search, Search);
            return Json(new { recordsTotal = modelList.Count() > 0 ? modelList[0].TotalCount : 0, data = modelList });
        }

        #region Work order completion wizard
        [HttpPost]

        public PartialViewResult CompleteWorkOrderFromWizard_Mobile(List<WoCancelAndPrintListModel> WorkOrderIds)
        {
            WorkOrderVM objWorkOrderVM = new WorkOrderVM();
            WoCompletionSettingsModel completionSettingsModel = new WoCompletionSettingsModel();
            ClientSetUpWrapper clientSetUpWrapper = new ClientSetUpWrapper(userData);
            RetrieveDataForUIConfiguration UIConfiguration = new RetrieveDataForUIConfiguration();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            Task[] tasks = new Task[2];

            completionSettingsModel = clientSetUpWrapper.CompletionSettingsDetails();

            objWorkOrderVM.UseWOCompletionWizard = completionSettingsModel.UseWOCompletionWizard;
            objWorkOrderVM.WOCommentTab = completionSettingsModel.WOCommentTab;
            objWorkOrderVM.TimecardTab = completionSettingsModel.TimecardTab;
            objWorkOrderVM.AutoAddTimecard = completionSettingsModel.AutoAddTimecard;

            if (objWorkOrderVM.UseWOCompletionWizard == true)
            {
                #region V2-728
                objWorkOrderVM.WOCompletionCriteriaTab = completionSettingsModel.WOCompCriteriaTab;
                objWorkOrderVM.WOCompletionCriteriaTitle = completionSettingsModel.WOCompCriteriaTitle;
                objWorkOrderVM.WOCompletionCriteria = completionSettingsModel.WOCompCriteria;
                #endregion
                //tasks[0] = Task.Factory.StartNew(() => objWorkOrderVM.UIConfigurationDetails = UIConfiguration.Retrieve
                //                                        (DataDictionaryViewNameConstant.WorkOrderCompletion, userData));
                //tasks[1] = Task.Factory.StartNew(() => AllLookUps = commonWrapper.GetAllLookUpList());
                //Task.WaitAll(tasks);

                objWorkOrderVM.UIConfigurationDetails = UIConfiguration.Retrieve
                                                        (DataDictionaryViewNameConstant.WorkOrderCompletion, userData);
                AllLookUps = commonWrapper.GetAllLookUpList();

                objWorkOrderVM.CompletionModelDynamic = new WorkOrderCompletionInformationModelDynamic();
                objWorkOrderVM.WorkOrderCompletionWizard = new WorkOrderCompletionWizard();

                #region V2-753
                if (WorkOrderIds.Count == 1)
                {
                    Task taskWODetails;
                    WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
                    var WorkOrderId = WorkOrderIds[0].WorkOrderId;
                    taskWODetails = Task.Factory.StartNew(() => objWorkOrderVM.CompletionModelDynamic = woWrapper.RetrieveWorkOrderCompletionInformationByWorkOrderId(WorkOrderId));
                    Task.WaitAll(taskWODetails);
                }
                #endregion

                //objWorkOrderVM.WorkOrderCompletionWizard.UseWOCompletionWizard = completionSettingsModel.UseWOCompletionWizard;
                //objWorkOrderVM.WorkOrderCompletionWizard.WOCommentTab = completionSettingsModel.WOCommentTab;
                //objWorkOrderVM.WorkOrderCompletionWizard.TimecardTab = completionSettingsModel.TimecardTab;
                //objWorkOrderVM.WorkOrderCompletionWizard.AutoAddTimecard = completionSettingsModel.AutoAddTimecard;
                objWorkOrderVM.WorkOrderCompletionWizard.WorkOrderIds = WorkOrderIds;

                IList<string> LookupNames = objWorkOrderVM.UIConfigurationDetails.ToList()
                                       .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
                                       .Select(s => s.LookupName)
                                       .ToList();

                objWorkOrderVM.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                      .GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault())
                                                      .Select(s => new WOAddUILookupList
                                                      { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                      .ToList();

            }
            objWorkOrderVM._userdata = userData;
            LocalizeControls(objWorkOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/CompletionWizard/_WorkOrderCompletionWizard.cshtml", objWorkOrderVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CompleteWorkOrderBatchFromWizard_Mobile(WorkOrderVM model)
        {
            if (model.WorkOrderCompletionWizard != null && !string.IsNullOrEmpty(model.WorkOrderCompletionWizard.WOLaborsString))
            {
                model.WorkOrderCompletionWizard.WOLabors = JsonConvert.DeserializeObject<List<CompletionLaborWizard>>
                                                            (model.WorkOrderCompletionWizard.WOLaborsString);
            }
            if (!ModelState.IsValid)
            {
                return Json(UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global), JsonRequestBehavior.AllowGet);
            }
            List<string> errMsgList = new List<string>();
            //List<BatchCompleteResultModel> WoBatchList = new List<BatchCompleteResultModel>();
            WorkOrder objWorkOrder = new WorkOrder();
            WorkOrderWrapper objWrapper = new WorkOrderWrapper(userData);

            StringBuilder failedWoList = new StringBuilder();
            foreach (var item in model.WorkOrderCompletionWizard.WorkOrderIds)
            {
                if (item.Status == WorkOrderStatusConstants.Approved || item.Status == WorkOrderStatusConstants.Scheduled)
                {
                    objWorkOrder = objWrapper.CompleteWOFromWizard(model.CompletionModelDynamic, item.WorkOrderId, model.WorkOrderCompletionWizard,
                        model.TimecardTab, model.AutoAddTimecard, model.WOCompletionCriteriaTab);
                    //objWorkOrder = this.WorkOrderComplete(item.WorkOrderId, model.WorkOrderCompletionWizard.CompletionComments);
                    if (objWorkOrder.ErrorMessages != null && objWorkOrder.ErrorMessages.Count > 0)
                    {
                        string errormessage = "Failed to complete " + objWorkOrder.ClientLookupId + ": " + objWorkOrder.ErrorMessages[0];
                        errMsgList.Add(errormessage);
                    }
                }
                else
                {
                    failedWoList.Append(item.ClientLookupId + ",");

                }
            }
            if (errMsgList.Count > 0 || !string.IsNullOrEmpty(failedWoList.ToString()))
            {
                if (!string.IsNullOrEmpty(failedWoList.ToString()))
                {
                    errMsgList.Add("Work Order(s) " + failedWoList + " can't be completed. Please check the status.");
                }
                return Json(new { data = errMsgList }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        #region Work order completion Labor tab
        [HttpPost]
        public PartialViewResult AddLaborFromCompletionWizard_Mobile()
        {
            var PersonnelLookUplist = GetList_PersonnelV2();
            WorkOrderVM objWorksOrderVM = new WorkOrderVM()
            {
                CompletionLaborWizard = new CompletionLaborWizard
                {
                    WorkAssignedLookUpList = PersonnelLookUplist != null ? PersonnelLookUplist.Select(x => new SelectListItem { Text = x.AssignedTo_PersonnelClientLookupId + " - " + x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() }) : null,
                }
            };
            LocalizeControls(objWorksOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/CompletionWizard/_AddEditLaborWizard.cshtml", objWorksOrderVM);
        }
        [HttpPost]
        public PartialViewResult EditLaborFromCompletionWizard_Mobile(long PersonnelID, decimal Hours, DateTime? StartDate)
        {
            var PersonnelLookUplist = GetList_PersonnelV2();
            WorkOrderVM objWorksOrderVM = new WorkOrderVM()
            {
                CompletionLaborWizard = new CompletionLaborWizard
                {
                    PersonnelID = PersonnelID,
                    Hours = Hours,
                    StartDate = StartDate,
                    WorkAssignedLookUpList = PersonnelLookUplist != null ? PersonnelLookUplist.Select(x => new SelectListItem { Text = x.AssignedTo_PersonnelClientLookupId + " - " + x.NameFirst + " - " + x.NameLast, Value = x.AssignedTo_PersonnelId.ToString() }) : null,
                }
            };
            LocalizeControls(objWorksOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/CompletionWizard/_AddEditLaborWizard.cshtml", objWorksOrderVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RetrieveCompletionLaborDetails_Mobile(WorkOrderVM woVM)
        {
            if (ModelState.IsValid)
            {
                PersonnelWrapper personnelWrapper = new PersonnelWrapper(userData);
                Personnel personnel = personnelWrapper.RetrieveForWorkOrderCompletionWizard
                                (woVM.CompletionLaborWizard.PersonnelID ?? 0, woVM.CompletionLaborWizard.Hours);
                return Json(personnel, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #endregion

        #region V2-690 Material Request Mobile
        [HttpPost]
        public PartialViewResult LoadMaterialRequest_Mobile()
        {
            DashboardVM dashboardVM = new DashboardVM();
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);

            //V2-726 Start
            ApprovalRouteModel approvalRouteModel = new ApprovalRouteModel();
            var ismaterialrequest = woWrapper.RetrieveApprovalGroupMaterialRequestStatus("MaterialRequests");
            approvalRouteModel.IsMaterialRequest = ismaterialrequest;
            dashboardVM.ApprovalRouteModel = approvalRouteModel;
            //V2 726 End
            dashboardVM.IsUseMultiStoreroom = userData.DatabaseKey.Client.UseMultiStoreroom;

            LocalizeControls(dashboardVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/_WOCompletionWorkbenchMaterialRequestsListSearch.cshtml", dashboardVM);
        }
        [HttpPost]
        public string PopulateMaterialRequest_Mobile(int? draw, int? start, int? length, long workOrderId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            List<EstimatePart> EstimatePartList = woWrapper.populateEstimatedParts(workOrderId);
            var IsInitiated = EstimatePartList.Where(x => x.Status == "Initiated").Count() > 0 ? true : false;
            if (EstimatePartList != null)
            {
                EstimatePartList = this.GetAllEstimatePartSortByColumnWithOrder(order, orderDir, EstimatePartList);
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = EstimatePartList.Count();
            totalRecords = EstimatePartList.Count();
            int initialPage = start.Value;
            var filteredResult = EstimatePartList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, IsInitiated = IsInitiated }, JsonSerializer12HoursDateAndTimeSettings);

        }

        public PartialViewResult AddPartInInventory_Mobile(long WorkOrderID, string ClientLookupId, long vendorId = 0, long StoreroomId = 0)
        {
            PartLookupVM partLookupVM = new PartLookupVM();
            partLookupVM.WorkOrderID = WorkOrderID;
            partLookupVM.ClientLookupId = ClientLookupId;
            partLookupVM.VendorId = vendorId;
            partLookupVM.StoreroomId = StoreroomId;
            partLookupVM.ShoppingCart = userData.Site.ShoppingCart;
            ViewBag.IsFromDashboard = true;
            LocalizeControls(partLookupVM, LocalizeResourceSetConstants.PartLookUpDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/partlookup/indexWO.cshtml", partLookupVM);
        }

        [HttpGet]
        public PartialViewResult AddEstimatesPartNotInInventory_Mobile(long WorkOrderId, string ClientLookupId)
        {
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            DashboardVM dashboardVM = new DashboardVM()
            {
                estimatePart = new EstimatePart
                {
                    WorkOrderId = WorkOrderId,
                    MainClientLookupId = ClientLookupId,
                    ClientLookupId = ClientLookupId,
                    ShoppingCart = userData.Site.ShoppingCart//V2-1068  
                }
            };
            var AllLookUps = commonWrapper.GetAllLookUpList();
            var UNIT_OF_MEASURE = AllLookUps.Where(x => x.ListName == LookupListConstants.Unit_Of_Measure).ToList();
            if (UNIT_OF_MEASURE != null)
            {
                dashboardVM.UnitOfmesureListWo = UNIT_OF_MEASURE.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
            }
            // Check if the user has a shopping cart and the estimate part category is not selected
            if (userData.Site.ShoppingCart && dashboardVM.estimatePart.CategoryId == 0)
            {
                dashboardVM.estimatePart.IsAccountClientLookupIdReq = true;
                dashboardVM.estimatePart.IsPartCategoryClientLookupIdReq = true;
            }
            LocalizeControls(dashboardVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/_AddEstimatesPartNotInInventory.cshtml", dashboardVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEstimatesPartNotInInventory_Mobile(WorkOrderVM workOrderVM)
        {
            if (ModelState.IsValid)
            {
                string Mode = string.Empty;
                WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
                EstimatedCosts objEstimatedCost = new EstimatedCosts();
                if (workOrderVM.estimatePart.EstimatedCostsId != 0)
                {
                    objEstimatedCost = woWrapper.EditEstimatePart(workOrderVM);
                }
                else
                {
                    Mode = "add";
                    objEstimatedCost = woWrapper.AddEstimatePartNotInInventory(workOrderVM);
                }
                if (objEstimatedCost.ErrorMessages != null && objEstimatedCost.ErrorMessages.Count > 0)
                {
                    return Json(objEstimatedCost.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), workorderid = workOrderVM.estimatePart.WorkOrderId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public PartialViewResult EditEstimatesPart_Mobile(long WorkOrderId, string MainClientLookupId, string PartclientLookupId, long EstimatedCostsId, string Description, decimal UnitCost, string Unit, string AccountClientLookupId, long AccountId, string VendorClientLookupId, long VendorId, string PartCategoryClientLookupId, long PartCategoryMasterId, decimal Quantity, decimal TotalCost)
        {
            DashboardVM dashboardVM = new DashboardVM()
            {
                estimatePart = new EstimatePart
                {
                    WorkOrderId = WorkOrderId,
                    MainClientLookupId = MainClientLookupId,
                    ClientLookupId = PartclientLookupId,
                    EstimatedCostsId = EstimatedCostsId,
                    UnitCost = UnitCost,
                    Unit = Unit,
                    AccountId = AccountId,
                    AccountClientLookupId = AccountClientLookupId,
                    VendorId = VendorId,
                    VendorClientLookupId = VendorClientLookupId,
                    PartCategoryMasterId = PartCategoryMasterId,
                    PartCategoryClientLookupId = PartCategoryClientLookupId,
                    Quantity = Quantity,
                    TotalCost = TotalCost,
                    Description = Description,
                    ShoppingCart = userData.Site.ShoppingCart  //V2-1068  
                }
            };
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            var AllLookUps = commonWrapper.GetAllLookUpList();
            var UNIT_OF_MEASURE = AllLookUps.Where(x => x.ListName == LookupListConstants.Unit_Of_Measure).ToList();
            if (UNIT_OF_MEASURE != null)
            {
                dashboardVM.UnitOfmesureListWo = UNIT_OF_MEASURE.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
            }
            // Check if the user has Shopping Cart enabled and the estimate part category is not specified
            if (userData.Site.ShoppingCart && dashboardVM.estimatePart.CategoryId == 0)
            {
                dashboardVM.estimatePart.IsAccountClientLookupIdReq = true;
                dashboardVM.estimatePart.IsPartCategoryClientLookupIdReq = true;
            }
            LocalizeControls(dashboardVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/_AddEstimatesPartNotInInventory.cshtml", dashboardVM);
        }
        [HttpPost]
        public JsonResult DeleteEstimatesPart_Mobile(long EstimatedCostsId)
        {
            WorkOrderWrapper EWrapper = new WorkOrderWrapper(userData);
            var deleteResult = EWrapper.DeleteEstimatePart(EstimatedCostsId);
            if (deleteResult)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public string GetPartLookUpGridWO_Mobile(int? draw, int? start, int? length, string orderbycol = "", string orderDir = "", string searchString = "", long StoreroomId = 0)
        {
            PartLookupWrapper partLookupWrapper = new PartLookupWrapper(userData);
            List<PartLookupModel> cardData = new List<PartLookupModel>();
            start = start.HasValue
              ? start / length
              : 0;
            int skip = start * length ?? 0;
            if (userData.DatabaseKey.Client.UseMultiStoreroom)
            {
                if (userData.Site.ShoppingCart)
                {
                    cardData = partLookupWrapper.SearchForCart_VendorCatalogWOMultiStoreroom(skip, length ?? 0, orderbycol, orderDir, searchString, StoreroomId);
                }
                else
                {
                    cardData = partLookupWrapper.SearchForCartWOMultiStoreroom(skip, length ?? 0, orderbycol, orderDir, searchString, StoreroomId);
                }
            }
            else
            {
                if (userData.Site.ShoppingCart)
                {
                    cardData = partLookupWrapper.SearchForCart_VendorCatalogWO(skip, length ?? 0, orderbycol, orderDir, searchString);
                }
                else
                {
                    cardData = partLookupWrapper.SearchForCartWO(skip, length ?? 0, orderbycol, orderDir, searchString);
                }
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            recordsFiltered = cardData.Select(o => o.TotalCount).FirstOrDefault();
            totalRecords = cardData.Select(o => o.TotalCount).FirstOrDefault();
            int initialPage = start.Value;
            var filteredResult = cardData.ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }

        public PartialViewResult GetCardViewDataWO_Mobile(int currentpage, int? start, int? length, string currentorderedcolumn, string currentorder, string searchString = "", long WorkOrderID = 0, string ModeForRedirect = "", long StoreroomId = 0)
        {
            PartLookupVM partLookupVM = new PartLookupVM();
            List<PartLookupModel> cardData = new List<PartLookupModel>();
            PartLookupWrapper partLookupWrapper = new PartLookupWrapper(userData);
            start = start.HasValue
            ? start / length
            : 0;
            int skip = start * length ?? 0;
            ViewBag.Start = skip;
            if (userData.DatabaseKey.Client.UseMultiStoreroom)
            {
                if (userData.Site.ShoppingCart)
                {
                    cardData = partLookupWrapper.SearchForCart_VendorCatalogWOMultiStoreroom(skip, length ?? 0, currentorderedcolumn, currentorder, searchString, StoreroomId);
                }
                else
                {
                    cardData = partLookupWrapper.SearchForCartWOMultiStoreroom(skip, length ?? 0, currentorderedcolumn, currentorder, searchString, StoreroomId);
                }
            }
            else
            {
                if (userData.Site.ShoppingCart)
                {
                    cardData = partLookupWrapper.SearchForCart_VendorCatalogWO(skip, length ?? 0, currentorderedcolumn, currentorder, searchString);
                }
                else
                {
                    cardData = partLookupWrapper.SearchForCartWO(skip, length ?? 0, currentorderedcolumn, currentorder, searchString);
                }
            }
            var recordsFiltered = 0;
            recordsFiltered = cardData.Select(o => o.TotalCount).FirstOrDefault();
            ViewBag.TotalRecords = cardData.Select(o => o.TotalCount).FirstOrDefault();
            ViewBag.Length = length;
            ViewBag.CurrentPage = currentpage;
            partLookupVM.WorkOrderID = WorkOrderID;
            partLookupVM.partLookupModels = cardData;
            partLookupVM.ModeForRedirect = ModeForRedirect;
            LocalizeControls(partLookupVM, LocalizeResourceSetConstants.PartLookUpDetails);

            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/PartLookup/_CardViewWO.cshtml", partLookupVM);
        }

        public PartialViewResult GetAddToCartDataWO_Mobile(PartAddItemToCartModel model)
        {
            LocalizeControls(model, LocalizeResourceSetConstants.PartLookUpDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/PartLookup/_AddToCartSideBarWO.cshtml", model);
        }

        public PartialViewResult GetAllAddToCartDataWO_Mobile(PartAddToCartModel model)
        {
            LocalizeControls(model, LocalizeResourceSetConstants.PartLookUpDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/PartLookup/_AddAllToCartSideBarWO.cshtml", model);
        }

        [HttpPost]
        public ActionResult ProcesssPartCartDataWO_Mobile(List<PartAddToCartProcessModel> modelData, long WorkOrderID, long MaterialRequestId)
        {
            PartLookupWrapper partLookupWrapper = new PartLookupWrapper(userData);
            #region ServerSide Validation Quantity & Price
            bool isValidData = true;
            string message = string.Empty;
            string clientlookUpId = string.Empty;

            // V2- 1148 Check if the site is using a shopping cart
            if (userData.Site.ShoppingCart)
            {
                // Validate order quantity and unit cost
                if (modelData.Any(x => x.OrderQuantity <= 0 || x.UnitCost <= 0))
                {
                    isValidData = false;
                    foreach (var model in modelData.Where(x => x.OrderQuantity <= 0 || x.UnitCost <= 0))
                    {
                        clientlookUpId = clientlookUpId + model.ClientLookUpId + ",";
                    }
                }
                else
                {
                    foreach (var model in modelData)
                    {
                        model.WorkOrderID = WorkOrderID;
                        model.MaterialRequestId = MaterialRequestId;
                    }
                }
            }
            else
            {
                foreach (var model in modelData)
                {
                    model.WorkOrderID = WorkOrderID;
                    model.MaterialRequestId = MaterialRequestId;
                }
            }


            // If data is invalid, return error message
            if (isValidData == false)
            {
                clientlookUpId = clientlookUpId.Remove(clientlookUpId.LastIndexOf(','));
                message = UtilityFunction.GetMessageFromResource("QtyPartNotZeroErrorMsg", LocalizeResourceSetConstants.PartLookUpDetails) + clientlookUpId;
                return Json(new { errormessge = message }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                // Process valid data
                if (WorkOrderID > 0)
                {
                    if (userData.DatabaseKey.Client.UseMultiStoreroom)
                    {
                        var result = partLookupWrapper.InsertPartLookUpWorkOrderMultiStoreroom(modelData);
                        if (result != null && result.Count == 0)
                        {
                            message = UtilityFunction.GetMessageFromResource("MaterialRequestAddedAlert", LocalizeResourceSetConstants.JsAlerts);
                        }
                        else
                        {
                            message = "fail";
                        }
                    }
                    else
                    {
                        var result = partLookupWrapper.InsertPartLookUpWorkOrder(modelData);
                        if (result != null && result.Count == 0)
                        {
                            message = UtilityFunction.GetMessageFromResource("MaterialRequestAddedAlert", LocalizeResourceSetConstants.JsAlerts);
                        }
                        else
                        {
                            message = "fail";
                        }
                    }
                }
                else if (MaterialRequestId > 0)
                {
                    if (userData.DatabaseKey.Client.UseMultiStoreroom)
                    {
                        var result = partLookupWrapper.InsertPartLookUpMaterialRequestMultiStoreroom(modelData);
                        if (result != null && result.Count == 0)
                        {
                            message = UtilityFunction.GetMessageFromResource("MaterialRequestItemAddedAlert", LocalizeResourceSetConstants.JsAlerts);
                        }
                        else
                        {
                            message = "fail";
                        }
                    }
                    else
                    {
                        var result = partLookupWrapper.InsertPartLookUpMaterialRequest(modelData);
                        if (result != null && result.Count == 0)
                        {
                            message = UtilityFunction.GetMessageFromResource("MaterialRequestItemAddedAlert", LocalizeResourceSetConstants.JsAlerts);
                        }
                        else
                        {
                            message = "fail";
                        }
                    }
                }
                else
                {
                    message = "fail";
                }

                return Json(new { errormessge = "", status = message, data = modelData }, JsonRequestBehavior.AllowGet);
            }
            #endregion

        }

        public PartialViewResult AddWOAdditionalCatalogGrid_Mobile(string clientlookupid)
        {
            PartLookupVM partLookupVM = new PartLookupVM();
            PartLookupModel partlookupmodel = new PartLookupModel();
            partlookupmodel.ClientLookupId = clientlookupid;
            partLookupVM.partLookup = partlookupmodel;
            LocalizeControls(partLookupVM, LocalizeResourceSetConstants.PartLookUpDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/PartLookup/_AdditionalCatalogItems.cshtml", partLookupVM);
        }
        [HttpPost]
        public string GetWOAdditionalCatalogGrid_Mobile(int? draw, int? start, int? length, long PartId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            PartLookupWrapper partLookupWrapper = new PartLookupWrapper(userData);
            PartLookupVM partLookupVM = new PartLookupVM();

            start = start.HasValue
          ? start / length
          : 0;
            int skip = start * length ?? 0;

            partLookupVM.additionalCatalogItemlist = partLookupWrapper.PopulateAdditionalCatalogitems(PartId, order, orderDir, skip, length ?? 0);

            var totalRecords = 0;
            var recordsFiltered = 0;

            recordsFiltered = partLookupVM.additionalCatalogItemlist.Count();
            totalRecords = partLookupVM.additionalCatalogItemlist.Count();
            int initialPage = start.Value;
            var filteredResult = partLookupVM.additionalCatalogItemlist
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
        #endregion

        #region V2-690 EditPartInInventory_Mobile
        public PartialViewResult EditPartInInventory_Mobile(long EstimatedCostsId, long WorkOrderId)
        {
            WorkOrderVM objMaterialRequestVM = new WorkOrderVM();
            WorkOrderWrapper mrWrapper = new WorkOrderWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            objMaterialRequestVM.security = this.userData.Security;

            var AllLookUps = commonWrapper.GetAllLookUpList();
            var UNIT_OF_MEASURE = AllLookUps.Where(x => x.ListName == LookupListConstants.Unit_Of_Measure).ToList();
            if (UNIT_OF_MEASURE != null)
            {
                objMaterialRequestVM.UnitOfmesureListWo = UNIT_OF_MEASURE.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
            }
            var childItem = mrWrapper.GetLineItem(EstimatedCostsId, WorkOrderId);

            objMaterialRequestVM.PartNotInInventoryModel = childItem;


            // Check if the user has a shopping cart and the category ID of the part is not specified
            if (userData.Site.ShoppingCart && objMaterialRequestVM.PartNotInInventoryModel.CategoryId == 0)
            {
                objMaterialRequestVM.PartNotInInventoryModel.IsAccountClientLookupIdReq = true;
                objMaterialRequestVM.PartNotInInventoryModel.IsPartCategoryClientLookupIdReq = true;
            }
            if (userData.Site.ShoppingCart)
            {
                objMaterialRequestVM.PartNotInInventoryModel.ShoppingCart = true;
            }
            LocalizeControls(objMaterialRequestVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/_EditPartInInventory.cshtml", objMaterialRequestVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPartInInventory_Mobile(WorkOrderVM PurchaseRequestVM)
        {
            string ModelValidationFailedMessage = string.Empty;
            WorkOrderWrapper pWrapper = new WorkOrderWrapper(userData);
            if (ModelState.IsValid)
            {
                var lineItem = pWrapper.EditPartInInventory(PurchaseRequestVM.PartNotInInventoryModel);
                if (lineItem.ErrorMessages != null && lineItem.ErrorMessages.Count > 0)
                {
                    return Json(lineItem.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), workOrderId = PurchaseRequestVM.PartNotInInventoryModel.ObjectId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion
        #region V2-695 Downtime Mobile

        [HttpGet]
        public PartialViewResult AddEditDowntime_Mobile(long WorkOrderId, long DowntimeId = 0)
        {
            DashboardVM objDashboardVM = new DashboardVM();
            DashboardDetailsWrapper objDashboardDetailsWrapper = new DashboardDetailsWrapper(userData);
            Models.Dashboard.WoDowntimeModel objDowntime = new Models.Dashboard.WoDowntimeModel();
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            AllLookUps = objCommonWrapper.GetAllLookUpList();
            var ReasonforDownlist = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_DOWN_REASON).ToList();


            if (DowntimeId > 0)
            {
                ViewBag.Mode = "Edit";
                objDowntime = objDashboardDetailsWrapper.RetrieveByDowntimeId(DowntimeId);
                objDowntime.DowntimeId = DowntimeId;
            }
            else
            {
                ViewBag.Mode = "Add";
                objDowntime.Downdate = DateTime.Now;
            }
            if (ReasonforDownlist != null)
            {
                var objReasonforDownlist = ReasonforDownlist.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
                objDowntime.ReasonForDownList = objReasonforDownlist;
            }
            objDowntime.WorkOrderId = WorkOrderId;

            objDashboardVM.woDowntimeModel = objDowntime;

            LocalizeControls(objDashboardVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/_AddEditDowntime.cshtml", objDashboardVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveDowntime_Mobile(DashboardVM dashboardVM)
        {
            if (ModelState.IsValid)
            {
                DashboardDetailsWrapper dashboardDetailsWrapper = new DashboardDetailsWrapper(userData);
                if (dashboardVM.woDowntimeModel.Downdate == null)
                    dashboardVM.woDowntimeModel.Downdate = DateTime.UtcNow;
                Downtime result = new Downtime();
                string Mode = string.Empty;
                if (dashboardVM.woDowntimeModel.DowntimeId == 0)
                {
                    Mode = "add";
                    result = dashboardDetailsWrapper.AddDownTime(dashboardVM.woDowntimeModel);
                }
                else
                {
                    Mode = "update";
                    result = dashboardDetailsWrapper.EditDownTime(dashboardVM.woDowntimeModel);
                }
                if (result.ErrorMessages != null && result.ErrorMessages.Count > 0)
                {
                    return Json(result.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), workorderid = dashboardVM.woDowntimeModel.WorkOrderId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult DeleteDowntime_Mobile(long DowntimeId)
        {
            DashboardDetailsWrapper dashboardDetailsWrapper = new DashboardDetailsWrapper(userData);
            var deleteResult = dashboardDetailsWrapper.DeleteDowntime(DowntimeId);
            if (deleteResult)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public PartialViewResult LoadDowntime_Mobile()
        {
            DashboardVM objDashboardVM = new DashboardVM();
            objDashboardVM.security = this.userData.Security;
            LocalizeControls(objDashboardVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/_Downtime.cshtml", objDashboardVM);
        }

        [HttpPost]
        public string PopulateDowntime_Mobile(int? draw, int? start, int? length, long workOrderId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            start = start.HasValue
          ? start / length
          : 0;
            int skip = start * length ?? 0;

            DashboardDetailsWrapper dashboardDetailsWrapper = new DashboardDetailsWrapper(userData);
            List<Models.Dashboard.WoDowntimeModel> downtimeList = dashboardDetailsWrapper.GetWorkOrderDowntime(workOrderId, skip, length ?? 0, order, orderDir); /*dashboardDetailsWrapper.RetrieveLaborByWorkOrderId(workOrderId, order, orderDir, skip, length ?? 0);*/
            var totalRecords = 0;
            var recordsFiltered = 0;
            if (downtimeList != null && downtimeList.Count > 0)
            {
                recordsFiltered = downtimeList[0].TotalCount;
                totalRecords = downtimeList[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }
            int initialPage = start.Value;
            var filteredResult = downtimeList
              .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, security = userData.Security.MaintenanceCompletionWorkbenchWidget_Downtime }, JsonSerializerDateSettings);
        }




        #endregion

        #region V2-716 Photos_Mobile
        [HttpPost]
        public PartialViewResult LoadPhotos_Mobile()
        {
            DashboardVM dashboardVM = new DashboardVM();
            dashboardVM._userdata = this.userData;
            dashboardVM.security = this.userData.Security;
            LocalizeControls(dashboardVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/_ImageDetails.cshtml", dashboardVM);
        }
        public PartialViewResult GetImages_Mobile(int currentpage, int? start, int? length, long WorkOrderId)
        {
            DashboardVM dashboard = new DashboardVM();
            List<ImageAttachmentModel> imgDatalist = new List<ImageAttachmentModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<Attachment> Attachments = new List<Attachment>();
            dashboard.security = this.userData.Security;
            dashboard._userdata = this.userData;
            start = start.HasValue
            ? start / length
            : 0;
            int skip = start * length ?? 0;
            ViewBag.Start = skip;

            if (userData.DatabaseKey.Client.OnPremise)
            {
                Attachments = commonWrapper.GetOnPremiseMultipleImageUrl(skip, length ?? 0, WorkOrderId, AttachmentTableConstant.WorkOrder);
            }
            else
            {
                Attachments = commonWrapper.GetAzureMultipleImageUrl(skip, length ?? 0, WorkOrderId, AttachmentTableConstant.WorkOrder);
            }
            foreach (var attachment in Attachments)
            {
                ImageAttachmentModel imgdata = new ImageAttachmentModel();
                imgdata.AttachmentId = attachment.AttachmentId;
                imgdata.ObjectId = attachment.ObjectId;
                imgdata.Profile = attachment.Profile;
                imgdata.FileName = attachment.FileName;
                string SASImageURL = string.Empty;
                if (!string.IsNullOrEmpty(attachment.AttachmentURL))
                {
                    SASImageURL = attachment.AttachmentURL;
                }
                else
                {
                    commonWrapper.GetNoImageUrl();
                }
                imgdata.AttachmentURL = SASImageURL;
                imgdata.ObjectName = attachment.ObjectName;
                imgdata.TotalCount = attachment.TotalCount;
                imgDatalist.Add(imgdata);
            }
            var recordsFiltered = 0;
            recordsFiltered = imgDatalist.Select(o => o.TotalCount).FirstOrDefault();
            ViewBag.TotalRecords = imgDatalist.Select(o => o.TotalCount).FirstOrDefault();
            ViewBag.Length = length;
            ViewBag.CurrentPage = currentpage;
            dashboard.imageAttachmentModels = imgDatalist;
            LocalizeControls(dashboard, LocalizeResourceSetConstants.Global);

            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/_AllImages.cshtml", dashboard);
        }
        #endregion
        #region V2-726 Mobile
        public PartialViewResult SendMRForApproval_Mobile(long WorkOrderId)
        {
            DashboardVM objWOVM = new DashboardVM();

            ApprovalRouteModel approvalRouteModel = new ApprovalRouteModel();
            var dataModels = Get_ApproverList(this.userData.DatabaseKey.Personnel.PersonnelId, ApprovalGroupRequestTypes.MaterialRequest, 1);
            var approvers = new List<SelectListItem>();
            if (dataModels.Count > 0)
            {
                approvers = dataModels.Select(x => new SelectListItem
                {
                    Text = x.ApproverName,
                    Value = x.ApproverId.ToString()
                }).ToList();
            }
            else
            {
                var securityName = SecurityConstants.MaterialRequest_Approve;
                var ItemAccess = "ItemAccess";
                dataModels = Get_PersonnelList(securityName, ItemAccess);
                approvers = dataModels.Select(x => new SelectListItem
                {
                    Text = x.NameFirst + " " + x.NameLast,
                    Value = x.AssignedTo_PersonnelId.ToString()
                }).ToList();
            }
            approvalRouteModel.ApproverList = approvers;
            approvalRouteModel.ApproverCount = approvers.Count;
            approvalRouteModel.ObjectId = WorkOrderId;

            objWOVM.ApprovalRouteModel = approvalRouteModel;
            ViewBag.IsMaterialRequestDetails = false;
            LocalizeControls(objWOVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/_SendItemsForApproval.cshtml", objWOVM);
        }
        [HttpPost]
        public JsonResult SendMRForApproval_Mobile(DashboardVM woVM)
        {
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            if (ModelState.IsValid)
            {
                string MaterialRequest = ApprovalGroupRequestTypes.MaterialRequest;
                long ApprovalGroupId = RetrieveApprovalGroupIdByRequestorIdAndRequestType(MaterialRequest);
                woVM.ApprovalRouteModel.ApprovalGroupId = ApprovalGroupId;
                woVM.ApprovalRouteModel.RequestType = MaterialRequest;
                if (ApprovalGroupId >= 0)
                {
                    var objWorkOrder = woWrapper.SendForApproval(woVM.ApprovalRouteModel);
                    if (objWorkOrder.ErrorMessages != null && objWorkOrder.ErrorMessages.Count > 0)
                    {
                        return Json(objWorkOrder.ErrorMessages, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { data = JsonReturnEnum.success.ToString(), WorkOrderId = woVM.ApprovalRouteModel.ObjectId, ApprovalGroupId = ApprovalGroupId }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString(), WorkOrderId = woVM.ApprovalRouteModel.ObjectId, ApprovalGroupId = ApprovalGroupId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region V2-732 Mobile
        public PartialViewResult PopulateStorerooms_Mobile()
        {
            DashboardVM wrVM = new DashboardVM();
            var commonWrapper = new CommonWrapper(userData);
            if (userData.DatabaseKey.Client.UseMultiStoreroom)
            {
                wrVM.StoreroomList = commonWrapper.GetStoreroomList(StoreroomAuthType.Issue);
            }
            LocalizeControls(wrVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/_StoreroomList.cshtml", wrVM);
        }
        [HttpPost]
        public JsonResult SelectStoreroom_Mobile(DashboardVM wrVM)
        {
            if (ModelState.IsValid)
            {
                return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #endregion

        #region V2-690 Material Request
        [HttpPost]
        public PartialViewResult LoadMaterialRequest()
        {
            DashboardVM dashboardVM = new DashboardVM();
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);

            //V2-726 Start
            ApprovalRouteModel approvalRouteModel = new ApprovalRouteModel();
            var ismaterialrequest = woWrapper.RetrieveApprovalGroupMaterialRequestStatus("MaterialRequests");
            approvalRouteModel.IsMaterialRequest = ismaterialrequest;
            dashboardVM.ApprovalRouteModel = approvalRouteModel;
            //V2 726 End
            dashboardVM.IsUseMultiStoreroom = userData.DatabaseKey.Client.UseMultiStoreroom;//V2-732

            LocalizeControls(dashboardVM, LocalizeResourceSetConstants.WorkOrderDetails);
            dashboardVM._userdata = userData;
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/_WOCompletionWorkbenchMaterialRequestsListSearch.cshtml", dashboardVM);
        }
        [HttpPost]
        public string PopulateMaterialRequest(int? draw, int? start, int? length, long workOrderId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            List<EstimatePart> EstimatePartList = woWrapper.populateEstimatedParts(workOrderId);
            var IsInitiated = EstimatePartList.Where(x => x.Status == "Initiated").Count() > 0 ? true : false;
            if (EstimatePartList != null)
            {
                EstimatePartList = this.GetAllEstimatePartSortByColumnWithOrder(order, orderDir, EstimatePartList);
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = EstimatePartList.Count();
            totalRecords = EstimatePartList.Count();
            int initialPage = start.Value;
            var filteredResult = EstimatePartList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, IsInitiated = IsInitiated }, JsonSerializer12HoursDateAndTimeSettings);

        }
        private List<EstimatePart> GetAllEstimatePartSortByColumnWithOrder(string order, string orderDir, List<EstimatePart> data)
        {
            List<EstimatePart> lst = new List<EstimatePart>();
            if (data != null)
            {
                switch (order)
                {
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                        break;
                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.UnitCost).ToList() : data.OrderBy(p => p.UnitCost).ToList();
                        break;
                    case "4":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Quantity).ToList() : data.OrderBy(p => p.Quantity).ToList();
                        break;
                    case "5":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Unit).ToList() : data.OrderBy(p => p.Unit).ToList();
                        break;
                    case "6":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TotalCost).ToList() : data.OrderBy(p => p.TotalCost).ToList();
                        break;
                    case "7":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.AccountClientLookupId).ToList() : data.OrderBy(p => p.AccountClientLookupId).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                        break;
                }
            }
            return lst;
        }


        public PartialViewResult AddPartInInventory(long WorkOrderID, string ClientLookupId, long vendorId = 0, long StoreroomId = 0)
        {
            PartLookupVM partLookupVM = new PartLookupVM();
            partLookupVM.WorkOrderID = WorkOrderID;
            partLookupVM.ClientLookupId = ClientLookupId;
            partLookupVM.VendorId = vendorId;
            partLookupVM.StoreroomId = StoreroomId;
            partLookupVM.ShoppingCart = userData.Site.ShoppingCart;
            ViewBag.IsFromDashboard = true;
            LocalizeControls(partLookupVM, LocalizeResourceSetConstants.PartLookUpDetails);
            return PartialView("~/views/partlookup/indexWO.cshtml", partLookupVM);
        }


        [HttpGet]
        public PartialViewResult AddEstimatesPartNotInInventory(long WorkOrderId, string ClientLookupId)
        {
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            DashboardVM dashboardVM = new DashboardVM()
            {
                estimatePart = new EstimatePart
                {
                    WorkOrderId = WorkOrderId,
                    MainClientLookupId = ClientLookupId,
                    ClientLookupId = ClientLookupId,
                    ShoppingCart = userData.Site.ShoppingCart //V2-1068  

                }
            };

            var AllLookUps = commonWrapper.GetAllLookUpList();
            var UNIT_OF_MEASURE = AllLookUps.Where(x => x.ListName == LookupListConstants.Unit_Of_Measure).ToList();
            if (UNIT_OF_MEASURE != null)
            {
                dashboardVM.UnitOfmesureListWo = UNIT_OF_MEASURE.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
            }
            // Check if the user has a shopping cart and the estimate part category is not selected
            if (userData.Site.ShoppingCart && dashboardVM.estimatePart.CategoryId == 0)
            {
                dashboardVM.estimatePart.IsAccountClientLookupIdReq = true;
                dashboardVM.estimatePart.IsPartCategoryClientLookupIdReq = true;
            }
            LocalizeControls(dashboardVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/_AddEstimatesPartNotInInventory.cshtml", dashboardVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEstimatesPartNotInInventory(WorkOrderVM workOrderVM)
        {
            if (ModelState.IsValid)
            {
                string Mode = string.Empty;
                WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
                EstimatedCosts objEstimatedCost = new EstimatedCosts();
                if (workOrderVM.estimatePart.EstimatedCostsId != 0)
                {
                    objEstimatedCost = woWrapper.EditEstimatePart(workOrderVM);
                }
                else
                {
                    Mode = "add";
                    objEstimatedCost = woWrapper.AddEstimatePartNotInInventory(workOrderVM);
                }
                if (objEstimatedCost.ErrorMessages != null && objEstimatedCost.ErrorMessages.Count > 0)
                {
                    return Json(objEstimatedCost.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), workorderid = workOrderVM.estimatePart.WorkOrderId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public PartialViewResult EditEstimatesPart(long WorkOrderId, string MainClientLookupId, string PartclientLookupId, long EstimatedCostsId, string Description, decimal UnitCost, string Unit, string AccountClientLookupId, long AccountId, string VendorClientLookupId, long VendorId, string PartCategoryClientLookupId, long PartCategoryMasterId, decimal Quantity, decimal TotalCost)
        {
            DashboardVM dashboardVM = new DashboardVM()
            {
                estimatePart = new EstimatePart
                {
                    WorkOrderId = WorkOrderId,
                    MainClientLookupId = MainClientLookupId,
                    ClientLookupId = PartclientLookupId,
                    EstimatedCostsId = EstimatedCostsId,
                    UnitCost = UnitCost,
                    Unit = Unit,
                    AccountId = AccountId,
                    AccountClientLookupId = AccountClientLookupId,
                    VendorId = VendorId,
                    VendorClientLookupId = VendorClientLookupId,
                    PartCategoryMasterId = PartCategoryMasterId,
                    PartCategoryClientLookupId = PartCategoryClientLookupId,
                    Quantity = Quantity,
                    TotalCost = TotalCost,
                    Description = Description,
                    ShoppingCart = userData.Site.ShoppingCart //V2-1068  
                }
            };
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            var AllLookUps = commonWrapper.GetAllLookUpList();
            var UNIT_OF_MEASURE = AllLookUps.Where(x => x.ListName == LookupListConstants.Unit_Of_Measure).ToList();
            if (UNIT_OF_MEASURE != null)
            {
                dashboardVM.UnitOfmesureListWo = UNIT_OF_MEASURE.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
            }
            // Check if the user has a shopping cart and the estimate part category is not selected
            if (userData.Site.ShoppingCart && dashboardVM.estimatePart.CategoryId == 0)
            {
                // Add your code comments here to explain the purpose of this condition
                dashboardVM.estimatePart.IsAccountClientLookupIdReq = true;
                dashboardVM.estimatePart.IsPartCategoryClientLookupIdReq = true;
            }
            LocalizeControls(dashboardVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/_AddEstimatesPartNotInInventory.cshtml", dashboardVM);
        }
        [HttpPost]
        public JsonResult DeleteEstimatesPart(long EstimatedCostsId)
        {
            WorkOrderWrapper EWrapper = new WorkOrderWrapper(userData);
            var deleteResult = EWrapper.DeleteEstimatePart(EstimatedCostsId);
            if (deleteResult)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        # region V2-690 EditPartInInventory
        public PartialViewResult EditPartInInventory(long EstimatedCostsId, long WorkOrderId)
        {
            WorkOrderVM objMaterialRequestVM = new WorkOrderVM();
            WorkOrderWrapper mrWrapper = new WorkOrderWrapper(userData);
            objMaterialRequestVM.security = this.userData.Security;
            var childItem = mrWrapper.GetLineItem(EstimatedCostsId, WorkOrderId);

            objMaterialRequestVM.PartNotInInventoryModel = childItem;


            CommonWrapper commonWrapper = new CommonWrapper(userData);
            objMaterialRequestVM.PartNotInInventoryModel.ShoppingCart = userData.Site.ShoppingCart;
            var AllLookUps = commonWrapper.GetAllLookUpList();
            var UNIT_OF_MEASURE = AllLookUps.Where(x => x.ListName == LookupListConstants.Unit_Of_Measure).ToList();
            if (UNIT_OF_MEASURE != null)
            {
                objMaterialRequestVM.UnitOfmesureListWo = UNIT_OF_MEASURE.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
            }
            // Check if the user has a shopping cart and the category ID of the part is not specified
            if (userData.Site.ShoppingCart && objMaterialRequestVM.PartNotInInventoryModel.CategoryId == 0)
            {
                objMaterialRequestVM.PartNotInInventoryModel.IsAccountClientLookupIdReq = true;
                objMaterialRequestVM.PartNotInInventoryModel.IsPartCategoryClientLookupIdReq = true;
            }
            LocalizeControls(objMaterialRequestVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/_EditPartInInventory.cshtml", objMaterialRequestVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPartInInventory(WorkOrderVM PurchaseRequestVM)
        {
            string ModelValidationFailedMessage = string.Empty;
            WorkOrderWrapper pWrapper = new WorkOrderWrapper(userData);
            if (ModelState.IsValid)
            {
                var lineItem = pWrapper.EditPartInInventory(PurchaseRequestVM.PartNotInInventoryModel);
                if (lineItem.ErrorMessages != null && lineItem.ErrorMessages.Count > 0)
                {
                    return Json(lineItem.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), workOrderId = PurchaseRequestVM.PartNotInInventoryModel.ObjectId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region V2-695 Downtime

        [HttpGet]
        public PartialViewResult AddEditDowntime(long WorkOrderId, long DowntimeId = 0)
        {
            DashboardVM objDashboardVM = new DashboardVM();
            DashboardDetailsWrapper objDashboardDetailsWrapper = new DashboardDetailsWrapper(userData);
            Models.Dashboard.WoDowntimeModel objDowntime = new Models.Dashboard.WoDowntimeModel();
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            AllLookUps = objCommonWrapper.GetAllLookUpList();
            var ReasonforDownlist = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_DOWN_REASON).ToList();


            if (DowntimeId > 0)
            {
                ViewBag.Mode = "Edit";
                objDowntime = objDashboardDetailsWrapper.RetrieveByDowntimeId(DowntimeId);
                objDowntime.DowntimeId = DowntimeId;
            }
            else
            {
                ViewBag.Mode = "Add";
                objDowntime.Downdate = DateTime.Now;
            }
            if (ReasonforDownlist != null)
            {
                var objReasonforDownlist = ReasonforDownlist.Select(x => new SelectListItem { Text = x.ListValue + ' ' + '|' + ' ' + x.Description, Value = x.ListValue.ToString() });
                objDowntime.ReasonForDownList = objReasonforDownlist;
            }
            objDowntime.WorkOrderId = WorkOrderId;

            objDashboardVM.woDowntimeModel = objDowntime;

            LocalizeControls(objDashboardVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/_AddEditDowntime.cshtml", objDashboardVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveDowntime(DashboardVM dashboardVM)
        {
            if (ModelState.IsValid)
            {
                DashboardDetailsWrapper dashboardDetailsWrapper = new DashboardDetailsWrapper(userData);
                if (dashboardVM.woDowntimeModel.Downdate == null)
                    dashboardVM.woDowntimeModel.Downdate = DateTime.UtcNow;
                Downtime result = new Downtime();
                string Mode = string.Empty;
                if (dashboardVM.woDowntimeModel.DowntimeId == 0)
                {
                    Mode = "add";
                    result = dashboardDetailsWrapper.AddDownTime(dashboardVM.woDowntimeModel);
                }
                else
                {
                    Mode = "update";
                    result = dashboardDetailsWrapper.EditDownTime(dashboardVM.woDowntimeModel);
                }
                if (result.ErrorMessages != null && result.ErrorMessages.Count > 0)
                {
                    return Json(result.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), workorderid = dashboardVM.woDowntimeModel.WorkOrderId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult DeleteDowntime(long DowntimeId)
        {
            DashboardDetailsWrapper dashboardDetailsWrapper = new DashboardDetailsWrapper(userData);
            var deleteResult = dashboardDetailsWrapper.DeleteDowntime(DowntimeId);
            if (deleteResult)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public PartialViewResult LoadDowntime()
        {
            DashboardVM objDashboardVM = new DashboardVM();
            objDashboardVM.security = this.userData.Security;
            LocalizeControls(objDashboardVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/_Downtime.cshtml", objDashboardVM);
        }

        [HttpPost]
        public string PopulateDowntime(int? draw, int? start, int? length, long workOrderId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            start = start.HasValue
          ? start / length
          : 0;
            int skip = start * length ?? 0;

            DashboardDetailsWrapper dashboardDetailsWrapper = new DashboardDetailsWrapper(userData);
            List<Models.Dashboard.WoDowntimeModel> downtimeList = dashboardDetailsWrapper.GetWorkOrderDowntime(workOrderId, skip, length ?? 0, order, orderDir); /*dashboardDetailsWrapper.RetrieveLaborByWorkOrderId(workOrderId, order, orderDir, skip, length ?? 0);*/
            var totalRecords = 0;
            var recordsFiltered = 0;
            if (downtimeList != null && downtimeList.Count > 0)
            {
                recordsFiltered = downtimeList[0].TotalCount;
                totalRecords = downtimeList[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }
            int initialPage = start.Value;
            var filteredResult = downtimeList
              .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, security = userData.Security.MaintenanceCompletionWorkbenchWidget_Downtime }, JsonSerializerDateSettings);
        }




        #endregion

        #region V2-716
        [HttpPost]
        public PartialViewResult LoadPhotos()
        {
            DashboardVM dashboardVM = new DashboardVM();
            dashboardVM._userdata = this.userData;
            dashboardVM.security = this.userData.Security;
            LocalizeControls(dashboardVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/_ImageDetails.cshtml", dashboardVM);
        }
        public PartialViewResult GetImages(int currentpage, int? start, int? length, long WorkOrderId)
        {
            DashboardVM dashboard = new DashboardVM();
            List<ImageAttachmentModel> imgDatalist = new List<ImageAttachmentModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<Attachment> Attachments = new List<Attachment>();
            dashboard.security = this.userData.Security;
            dashboard._userdata = this.userData;
            start = start.HasValue
            ? start / length
            : 0;
            int skip = start * length ?? 0;
            ViewBag.Start = skip;

            if (userData.DatabaseKey.Client.OnPremise)
            {
                Attachments = commonWrapper.GetOnPremiseMultipleImageUrl(skip, length ?? 0, WorkOrderId, AttachmentTableConstant.WorkOrder);
            }
            else
            {
                Attachments = commonWrapper.GetAzureMultipleImageUrl(skip, length ?? 0, WorkOrderId, AttachmentTableConstant.WorkOrder);
            }
            foreach (var attachment in Attachments)
            {
                ImageAttachmentModel imgdata = new ImageAttachmentModel();
                imgdata.AttachmentId = attachment.AttachmentId;
                imgdata.ObjectId = attachment.ObjectId;
                imgdata.Profile = attachment.Profile;
                imgdata.FileName = attachment.FileName;
                string SASImageURL = string.Empty;
                if (!string.IsNullOrEmpty(attachment.AttachmentURL))
                {
                    SASImageURL = attachment.AttachmentURL;
                }
                else
                {
                    commonWrapper.GetNoImageUrl();
                }
                imgdata.AttachmentURL = SASImageURL;
                imgdata.ObjectName = attachment.ObjectName;
                imgdata.TotalCount = attachment.TotalCount;
                imgDatalist.Add(imgdata);
            }
            var recordsFiltered = 0;
            recordsFiltered = imgDatalist.Select(o => o.TotalCount).FirstOrDefault();
            ViewBag.TotalRecords = imgDatalist.Select(o => o.TotalCount).FirstOrDefault();
            ViewBag.Length = length;
            ViewBag.CurrentPage = currentpage;
            dashboard.imageAttachmentModels = imgDatalist;
            LocalizeControls(dashboard, LocalizeResourceSetConstants.Global);

            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/_AllImages.cshtml", dashboard);
        }
        #endregion

        #region V2-726
        public PartialViewResult SendMRForApproval(long WorkOrderId)
        {
            DashboardVM objWOVM = new DashboardVM();

            ApprovalRouteModel approvalRouteModel = new ApprovalRouteModel();
            var dataModels = Get_ApproverList(this.userData.DatabaseKey.Personnel.PersonnelId, ApprovalGroupRequestTypes.MaterialRequest, 1);
            var approvers = new List<SelectListItem>();
            if (dataModels.Count > 0)
            {
                approvers = dataModels.Select(x => new SelectListItem
                {
                    Text = x.ApproverName,
                    Value = x.ApproverId.ToString()
                }).ToList();
            }
            else
            {
                var securityName = SecurityConstants.MaterialRequest_Approve;
                var ItemAccess = "ItemAccess";
                dataModels = Get_PersonnelList(securityName, ItemAccess);
                approvers = dataModels.Select(x => new SelectListItem
                {
                    Text = x.NameFirst + " " + x.NameLast,
                    Value = x.AssignedTo_PersonnelId.ToString()
                }).ToList();
            }
            approvalRouteModel.ApproverList = approvers;
            approvalRouteModel.ApproverCount = approvers.Count;
            approvalRouteModel.ObjectId = WorkOrderId;

            objWOVM.ApprovalRouteModel = approvalRouteModel;
            ViewBag.IsMaterialRequestDetails = false;
            LocalizeControls(objWOVM, LocalizeResourceSetConstants.DashboardDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/_SendItemsForApproval.cshtml", objWOVM);
        }
        [HttpPost]
        public JsonResult SendMRForApproval(DashboardVM woVM)
        {
            WorkOrderWrapper woWrapper = new WorkOrderWrapper(userData);
            if (ModelState.IsValid)
            {
                string MaterialRequest = ApprovalGroupRequestTypes.MaterialRequest;
                long ApprovalGroupId = RetrieveApprovalGroupIdByRequestorIdAndRequestType(MaterialRequest);
                woVM.ApprovalRouteModel.ApprovalGroupId = ApprovalGroupId;
                woVM.ApprovalRouteModel.RequestType = MaterialRequest;
                if (ApprovalGroupId >= 0)
                {
                    var objWorkOrder = woWrapper.SendForApproval(woVM.ApprovalRouteModel);
                    if (objWorkOrder.ErrorMessages != null && objWorkOrder.ErrorMessages.Count > 0)
                    {
                        return Json(objWorkOrder.ErrorMessages, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { data = JsonReturnEnum.success.ToString(), WorkOrderId = woVM.ApprovalRouteModel.ObjectId, ApprovalGroupId = ApprovalGroupId }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { data = JsonReturnEnum.success.ToString(), WorkOrderId = woVM.ApprovalRouteModel.ObjectId, ApprovalGroupId = ApprovalGroupId }, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region V2-732
        public PartialViewResult PopulateStorerooms()
        {
            DashboardVM wrVM = new DashboardVM();
            var commonWrapper = new CommonWrapper(userData);
            if (userData.DatabaseKey.Client.UseMultiStoreroom)
            {
                wrVM.StoreroomList = commonWrapper.GetStoreroomList(StoreroomAuthType.Issue);
            }
            LocalizeControls(wrVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/_StoreroomList.cshtml", wrVM);
        }
        [HttpPost]
        public JsonResult SelectStoreroom(DashboardVM wrVM)
        {
            if (ModelState.IsValid)
            {
                return Json(new { data = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region V2-1056 Add Sanitation Request
        public PartialViewResult AddSanitationRequestWO(long WorkoderId, string ClientLookupId)
        {
            DashboardVM objDashboardVM = new DashboardVM();
            DashboardDetailsWrapper dashboardDetailsWrapper = new DashboardDetailsWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);

            objDashboardVM.dashboardAddSanitationRequestModel = new DashboardAddSanitationRequestModel();
            objDashboardVM.WorkOrderModel = dashboardDetailsWrapper.GetDashboardWorkOderDetailsById(WorkoderId);
            objDashboardVM._userdata = this.userData;
            objDashboardVM.dashboardAddSanitationRequestModel.WorkOrderId = objDashboardVM.WorkOrderModel.WorkOrderId;
            objDashboardVM.dashboardAddSanitationRequestModel.ClientLookupId = ClientLookupId;
            objDashboardVM.dashboardAddSanitationRequestModel.ChargeType = objDashboardVM.WorkOrderModel.ChargeType;
            objDashboardVM.dashboardAddSanitationRequestModel.ChargeToClientLookupId = objDashboardVM.WorkOrderModel.ChargeToClientLookupId;
            objDashboardVM.dashboardAddSanitationRequestModel.ChargeTo = objDashboardVM.WorkOrderModel.ChargeToId;
            LocalizeControls(objDashboardVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/_AddSanitationRequestWO.cshtml", objDashboardVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddSanitationRequestWODashboard(DashboardVM objVM)
        {
            string IsAddOrUpdate = string.Empty;
            if (ModelState.IsValid)
            {
                List<string> ErrorMsg = new List<string>();
                DashboardDetailsWrapper dashboardDetailsWrapper = new DashboardDetailsWrapper(userData);

                var returnObj = dashboardDetailsWrapper.AddSanitationRequestWorkOrder(objVM.dashboardAddSanitationRequestModel, ref ErrorMsg);

                if (returnObj.ErrorMessages != null && returnObj.ErrorMessages.Count > 0)
                {
                    return Json(returnObj.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Issuccess = true }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }

        }

        //For Add SanitationRequest WO Mobile
        public PartialViewResult AddSanitationRequestWO_Mobile(long WorkoderId, string ClientLookupId)
        {
            DashboardVM objDashboardVM = new DashboardVM();
            DashboardDetailsWrapper dashboardDetailsWrapper = new DashboardDetailsWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);

            objDashboardVM.dashboardAddSanitationRequestModel = new DashboardAddSanitationRequestModel();
            objDashboardVM.WorkOrderModel = dashboardDetailsWrapper.GetDashboardWorkOderDetailsById(WorkoderId);
            objDashboardVM._userdata = this.userData;
            objDashboardVM.dashboardAddSanitationRequestModel.WorkOrderId = objDashboardVM.WorkOrderModel.WorkOrderId;
            objDashboardVM.dashboardAddSanitationRequestModel.ClientLookupId = ClientLookupId;
            objDashboardVM.dashboardAddSanitationRequestModel.ChargeType = objDashboardVM.WorkOrderModel.ChargeType;
            objDashboardVM.dashboardAddSanitationRequestModel.ChargeToClientLookupId = objDashboardVM.WorkOrderModel.ChargeToClientLookupId;
            objDashboardVM.dashboardAddSanitationRequestModel.ChargeTo = objDashboardVM.WorkOrderModel.ChargeToId;
            LocalizeControls(objDashboardVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/_AddSanitationRequestWO.cshtml", objDashboardVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddSanitationRequestWODashboard_Mobile(DashboardVM objVM)
        {
            string IsAddOrUpdate = string.Empty;
            if (ModelState.IsValid)
            {
                List<string> ErrorMsg = new List<string>();
                DashboardDetailsWrapper dashboardDetailsWrapper = new DashboardDetailsWrapper(userData);

                var returnObj = dashboardDetailsWrapper.AddSanitationRequestWorkOrder(objVM.dashboardAddSanitationRequestModel, ref ErrorMsg);
                if (returnObj.ErrorMessages != null && returnObj.ErrorMessages.Count > 0)
                {
                    return Json(returnObj.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Issuccess = true }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region GetVendorLookupList_Mobile-- v2-1068
        public PartialViewResult GetVendorLookupList_Mobile()
        {
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/_VendorGridPopUp.cshtml");
        }
        [HttpPost]
        public JsonResult GetVendorLookupListchunksearch_Mobile(int Start, int Length, string Search = "")
        {
            var modelList = new List<VendorLookupModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);

            string order = "0";
            string orderDir = "asc";

            modelList = commonWrapper.GetVendorLookupListGridData_ForMobile(order, orderDir, Start, Length, Search);
            return Json(new { recordsTotal = modelList.Count() > 0 ? modelList[0].TotalCount : 0, data = modelList });
        }
        #endregion

        #region GetPartCategoryMasterLookupList_Mobile-- v2-1068
        public PartialViewResult GetPartCategoryMasterLookupList_Mobile()
        {
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/_PartCategoryMasterGridPopUp.cshtml");
        }
        [HttpPost]
        public JsonResult GetPartCategoryMasterLookupListchunksearch_Mobile(int Start, int Length, string Search = "")
        {
            var modelList = new List<CategoryMasterModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);

            string order = "0";
            string orderDir = "asc";

            modelList = commonWrapper.GetPartCategoryMasterLookupListGridData_ForMobile(order, orderDir, Start, Length, Search);
            return Json(new { recordsTotal = modelList.Count() > 0 ? modelList[0].TotalCount : 0, data = modelList });
        }
        #endregion
        #region V2-1076  lookuplist for personnel planner 
        public PartialViewResult GetPersonnelPlannerLookupList_Mobile()
        {
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/_PersonnelPlannerGridPopUp.cshtml");
        }
        [HttpPost]
        public JsonResult GetPersonnelPlannerLookupListchunksearch_Mobile(int Start, int Length, string Search = "")
        {
            var modelList = new List<PersonnelLookUpModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);

            string order = "0";
            string orderDir = "asc";

            modelList = commonWrapper.GetActiveAdminOrFullPlannerPersonnelLookupListGridData_Mobile(order, orderDir, Start, Length, Search, Search, Search);
            return Json(new { recordsTotal = modelList.Count() > 0 ? modelList[0].TotalCount : 0, data = modelList });
        }

        #endregion

        #region V2-1067 Add unplanned Describe WO Dynamic

        public PartialViewResult AddDescribeWODynamic(long WorkoderId, string ClientLookupId)
        {
            DashboardVM objdashboardVM = new DashboardVM();
            objdashboardVM.WoEmergencyDescribeModel = new DescribeWOModel();
            objdashboardVM.WoEmergencyDescribeModel.WorkOrderId = WorkoderId;
            DashboardDetailsWrapper dashboardDetailsWrapper = new DashboardDetailsWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> Type = new List<DataContracts.LookupList>();
            objdashboardVM._userdata = this.userData;
            objdashboardVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
                                    .Retrieve(DataDictionaryViewNameConstant.WorkOrderDescribeAdd, userData);
            objdashboardVM.AllRequiredLookUplist = new List<WOAddUILookupList>();
            var AllLookUps = commonWrapper.GetAllLookUpList();
            IList<string> LookupNames = objdashboardVM.UIConfigurationDetails.ToList()
                           .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
                           .Select(s => s.LookupName)
                           .ToList();
            if (AllLookUps != null && AllLookUps.Any())
            {
                if (LookupNames.Contains("WO_PRIORITY"))
                {
                    var woPriorityLookups = AllLookUps.Where(x => x.ListName == "WO_PRIORITY").ToList();
                    if (woPriorityLookups != null && woPriorityLookups.Any())
                    {
                        objdashboardVM.AllRequiredLookUplist.AddRange(woPriorityLookups
                            .GroupBy(x => new { x.ListName, x.ListValue })
                            .Select(x => x.FirstOrDefault())
                            .Select(s => new WOAddUILookupList
                            {
                                text = s.Description,
                                value = s.ListValue,
                                lookupname = s.ListName
                            })
                            .ToList());
                        LookupNames.Remove("WO_PRIORITY");
                    }
                }
            }

            objdashboardVM.AllRequiredLookUplist.AddRange(AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                 .GroupBy(x => new { x.ListName, x.ListValue }).Select(x => x.FirstOrDefault())
                                                      .Select(s => new WOAddUILookupList
                                                      { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                      .ToList());
            objdashboardVM.AllRequiredLookUplist.AddRange(AllLookUps.Where(x => x.ListName == LookupListConstants.UP_WO_TYPE)
                                               .GroupBy(x => new { x.ListName, x.ListValue }).Select(x => x.FirstOrDefault())
                                               .Select(s => new WOAddUILookupList
                                               { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                               .ToList());

            objdashboardVM.PlantLocation = userData.Site.PlantLocation;
            objdashboardVM.IsAddWorkOrderDynamic = true;
            objdashboardVM._userdata = this.userData;
            objdashboardVM.security = this.userData.Security;
            objdashboardVM.woDescriptionModelDynamic = new WoDescriptionModelDynamic();

            LocalizeControls(objdashboardVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/_DescribeWOPopUpDynamic.cshtml", objdashboardVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddDescribeWorkOrderDynamic(DashboardVM dashboardVM)
        {
            if (ModelState.IsValid)
            {
                List<string> ErrorMsgList = new List<string>();
                DashboardVM objVM = new DashboardVM();
                DashboardDetailsWrapper dashboardDetailsWrapper = new DashboardDetailsWrapper(userData);
                objVM.WoEmergencyDescribeModel = new DescribeWOModel();

                dashboardVM.woDescriptionModelDynamic.IsDepartmentShow = true;
                dashboardVM.woDescriptionModelDynamic.IsTypeShow = true;
                dashboardVM.woDescriptionModelDynamic.IsDescriptionShow = true;
                WorkOrder returnObj = dashboardDetailsWrapper.WO_DescribeDynamic(dashboardVM, ref ErrorMsgList);
                if (ErrorMsgList != null && ErrorMsgList.Count > 0)
                {
                    return Json(ErrorMsgList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    bool IsredirectToSaniPage = (userData.Site.ExternalSanitation == true && userData.Security.SanitationJob.CreateRequest == true) ? true : false;
                    return Json(new { data = JsonReturnEnum.success.ToString(), IsToSanitationPage = IsredirectToSaniPage, workOrderID = returnObj.WorkOrderId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        public PartialViewResult AddDescribeWO_MobileDynamic()
        {
            DashboardVM objdashboardVM = new DashboardVM();
            objdashboardVM.WoEmergencyDescribeModel = new DescribeWOModel();
            DashboardDetailsWrapper dashboardDetailsWrapper = new DashboardDetailsWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> Type = new List<DataContracts.LookupList>();
            objdashboardVM._userdata = this.userData;
            var AllLookUps = commonWrapper.GetAllLookUpList();
            objdashboardVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
                                     .Retrieve(DataDictionaryViewNameConstant.WorkOrderDescribeAdd, userData);
            objdashboardVM.AllRequiredLookUplist = new List<WOAddUILookupList>();

            objdashboardVM.woDescriptionModelDynamic = new WoDescriptionModelDynamic();

            IList<string> LookupNames = objdashboardVM.UIConfigurationDetails.ToList()
                                     .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
                                     .Select(s => s.LookupName)
                                     .ToList();
            objdashboardVM.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                  .GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault())
                                                  .Select(s => new WOAddUILookupList
                                                  { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                  .ToList();

            objdashboardVM.AllRequiredLookUplist.AddRange(AllLookUps.Where(x => x.ListName == LookupListConstants.UP_WO_TYPE)
                                               .GroupBy(x => new { x.ListName, x.ListValue }).Select(x => x.FirstOrDefault())
                                               .Select(s => new WOAddUILookupList
                                               { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                               .ToList());


            objdashboardVM._userdata = this.userData;
            objdashboardVM.DateRangeDropList = UtilityFunction.GetTimeRangeDrop().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            objdashboardVM.DateRangeDropListForWO = UtilityFunction.GetTimeRangeDropForWO().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            objdashboardVM.DateRangeDropListForWOCreatedate = UtilityFunction.GetTimeRangeDropForWOCreateDate().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });


            LocalizeControls(objdashboardVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/Dashboard/Widgets/MaintenanceCompletionWorkbench/Mobile/_DescribeWOPopUpDynamic.cshtml", objdashboardVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddDescribeWorkOrder_MobileDynamic(DashboardVM dashboardVM)
        {
            if (ModelState.IsValid)
            {
                List<string> ErrorMsgList = new List<string>();
                DashboardVM objVM = new DashboardVM();
                DashboardDetailsWrapper dashboardDetailsWrapper = new DashboardDetailsWrapper(userData);
                objVM.WoEmergencyDescribeModel = new DescribeWOModel();

                dashboardVM.woDescriptionModelDynamic.IsDepartmentShow = true;
                dashboardVM.woDescriptionModelDynamic.IsTypeShow = true;
                dashboardVM.woDescriptionModelDynamic.IsDescriptionShow = true;
                WorkOrder returnObj = dashboardDetailsWrapper.WO_DescribeDynamic(dashboardVM, ref ErrorMsgList);
                if (ErrorMsgList != null && ErrorMsgList.Count > 0)
                {
                    return Json(ErrorMsgList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    bool IsredirectToSaniPage = (userData.Site.ExternalSanitation == true && userData.Security.SanitationJob.CreateRequest == true) ? true : false;
                    return Json(new { data = JsonReturnEnum.success.ToString(), IsToSanitationPage = IsredirectToSaniPage, workOrderID = returnObj.WorkOrderId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region V2-1100 Add Work request Only Dashboard Mobile
        //public PartialViewResult AddWoRequestDynamicOnlyDashboard_Mobile(string mode = null)
        //{
        //    DashboardVM objDashboardVM = new DashboardVM();
        //    CommonWrapper commonWrapper = new CommonWrapper(userData);
        //    List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
        //    AllLookUps = commonWrapper.GetAllLookUpList();
        //    objDashboardVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
        //                                        .Retrieve(DataDictionaryViewNameConstant.AddWorkRequest, userData);
        //    IList<string> LookupNames = objDashboardVM.UIConfigurationDetails.ToList()
        //                             .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
        //                             .Select(s => s.LookupName)
        //                             .ToList();
        //    objDashboardVM.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
        //                                          .GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault())
        //                                          .Select(s => new WOAddUILookupList
        //                                          { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
        //                                          .ToList();

        //    objDashboardVM.IsWorkOrderRequest = false; //V2-1052
        //    if (string.IsNullOrEmpty(mode))
        //    {
        //        objDashboardVM.IsWorkOrderRequest = true;
        //        objDashboardVM.AllRequiredLookUplist.AddRange(AllLookUps.Where(x => x.ListName == LookupListConstants.WR_WO_TYPE)
        //                                       .GroupBy(x => new { x.ListName, x.ListValue }).Select(x => x.FirstOrDefault())
        //                                       .Select(s => new WOAddUILookupList
        //                                       { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
        //                                       .ToList());
        //    }


        //    objDashboardVM._userdata = this.userData;     // RKL - Missing - Caused error when hitting the add work request button
        //    objDashboardVM.IsAddWoRequestDynamic = true;
        //    objDashboardVM.AddWorkRequest = new Models.Work_Order.UIConfiguration.AddWorkRequestModelDynamic();
        //    objDashboardVM._userdata = this.userData;
        //    objDashboardVM.BusinessType = this.userData.DatabaseKey.Client.BusinessType.ToUpper();
        //    objDashboardVM.DateRangeDropList = UtilityFunction.GetTimeRangeDrop().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }); //V2-347
        //    objDashboardVM.DateRangeDropListForWO = UtilityFunction.GetTimeRangeDropForWO().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }); //V2-347
        //    objDashboardVM.DateRangeDropListForWOCreatedate = UtilityFunction.GetTimeRangeDropForWOCreateDate().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() }); //V2-364

        //    LocalizeControls(objDashboardVM, LocalizeResourceSetConstants.WorkOrderDetails);
        //    return PartialView("~/Views/Dashboard/WorkRequestOnly/Mobile/_AddWorkRequestDynamicPopUp.cshtml", objDashboardVM);
        //}

        public PartialViewResult AddWoRequestDynamicOnlyDashboard_Mobile(string mode = null)
        {
            WorkOrderVM objWorkOrderVM = new WorkOrderVM();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            AllLookUps = commonWrapper.GetAllLookUpList();
            objWorkOrderVM.UIConfigurationDetails = new RetrieveDataForUIConfiguration()
                                                .Retrieve(DataDictionaryViewNameConstant.AddWorkRequest, userData);
            IList<string> LookupNames = objWorkOrderVM.UIConfigurationDetails.ToList()
                                     .Where(x => x.LookupType == "LookupList" && !string.IsNullOrEmpty(x.LookupName))
                                     .Select(s => s.LookupName)
                                     .ToList();
            objWorkOrderVM.AllRequiredLookUplist = AllLookUps.Where(x => LookupNames.Contains(x.ListName))
                                                  .GroupBy(x => x.ListValue).Select(x => x.FirstOrDefault())
                                                  .Select(s => new WOAddUILookupList
                                                  { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                                  .ToList();

            objWorkOrderVM.IsWorkOrderRequest = false; //V2-1052
            if (string.IsNullOrEmpty(mode))
            {
                objWorkOrderVM.IsWorkOrderRequest = true;
                objWorkOrderVM.AllRequiredLookUplist.AddRange(AllLookUps.Where(x => x.ListName == LookupListConstants.WR_WO_TYPE)
                                               .GroupBy(x => new { x.ListName, x.ListValue }).Select(x => x.FirstOrDefault())
                                               .Select(s => new WOAddUILookupList
                                               { text = s.ListValue + " - " + s.Description, value = s.ListValue, lookupname = s.ListName })
                                               .ToList());
            }

            objWorkOrderVM._userdata = this.userData;     // RKL - Missing - Caused error when hitting the add work request button
            objWorkOrderVM.IsAddRequest = true;
            objWorkOrderVM.AddWorkRequest = new Models.Work_Order.UIConfiguration.AddWorkRequestModelDynamic();
            objWorkOrderVM._userdata = this.userData;
            LocalizeControls(objWorkOrderVM, LocalizeResourceSetConstants.WorkOrderDetails);
            return PartialView("~/Views/Dashboard/WorkRequestOnly/Mobile/_AddWorkRequestDynamicPopUp.cshtml", objWorkOrderVM);

        }
        public PartialViewResult GetAccountWOLookupList_Mobile()
        {
            return PartialView("~/Views/Dashboard/WorkRequestOnly/Mobile/_AccountGridPopUp.cshtml");
        }

        #region For Add SanitationRequestWR Mobile
        public PartialViewResult AddSanitationRequestWR_Mobile()
        {

            SanitationJobWrapper sWrapper = new SanitationJobWrapper(userData);
            SanitationVM objVM = new SanitationVM();
            objVM.AssetTree = this.userData.DatabaseKey.Client.AssetTree;
            objVM._userdata = this.userData;//
            LocalizeControls(objVM, LocalizeResourceSetConstants.SanitationDetails);
            return PartialView("~/Views/Dashboard/WorkRequestOnly/Mobile/_AddSanitationRequestWO.cshtml", objVM);
        }
        #endregion
        #endregion
    }
}

