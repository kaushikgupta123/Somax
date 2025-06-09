using Client.ActionFilters;
using Client.BusinessWrapper.Reports;
using Client.Common;
using Client.Controllers.Common;
using Client.Models;
using Client.Models.BusinessIntelligence;
using Client.Models.PurchaseOrder;
using Client.Models.Reports;
using Common.Constants;

using DataContracts;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

using Newtonsoft.Json;

using Rotativa;
using Rotativa.Options;

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Client.Controllers.BusinessIntelligence
{
    public class ReportsController : SomaxBaseController
    {
        [CheckUserSecurity(securityType = SecurityConstants.Reports)]
        public ActionResult Index()
        {
            ReportsWrapper reportsWrapper = new ReportsWrapper(userData);
            BusinessIntelligenceVM objVM = new BusinessIntelligenceVM();
            List<ReportListingModel> recentReports = new List<ReportListingModel>();
            List<string> reportListing = new List<string>();
            Task[] tasks = new Task[2];
            tasks[0] = Task.Factory.StartNew(() => recentReports = reportsWrapper.GetRecentReports());
            tasks[1] = Task.Factory.StartNew(() => reportListing = reportsWrapper.GetReportMenu());
            Task.WaitAll(tasks);
            if (!tasks[0].IsFaulted && tasks[0].IsCompleted)
            {
                objVM.RecentReports = recentReports;
            }
            if (!tasks[1].IsFaulted && tasks[1].IsCompleted)
            {
                objVM.ReportGroups = reportListing;
            }
            objVM.security = this.userData.Security;
            LocalizeControls(objVM, LocalizeResourceSetConstants.Report);
            return View(objVM);
        }
        #region ReportList
        public ActionResult GetReportList(string GroupName)
        {
            ReportsWrapper reportsWrapper = new ReportsWrapper(userData);
            BusinessIntelligenceVM objVM = new BusinessIntelligenceVM();
            objVM.DateRangeDropListForReport = UtilityFunction.GetTimeRangeDropForReport().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            if (!string.IsNullOrEmpty(GroupName))
            {
                objVM.ReportLists = reportsWrapper.GetReportList(GroupName);
            }
            else
            {
                objVM.ReportLists = reportsWrapper.GetFavorites();
            }


            objVM.security = this.userData.Security;
            LocalizeControls(objVM, LocalizeResourceSetConstants.Report);

            return PartialView("_ReportList", objVM);
        }
        #endregion

        #region Favorites
        [HttpPost]
        public ActionResult CreateFavorite(long ReportListingId, long ReportFavoritesId, bool IsUserReport = false)
        {
            ReportsWrapper reportsWrapper = new ReportsWrapper(userData);
            var result = reportsWrapper.CreateFavorite(ReportListingId, ReportFavoritesId, IsUserReport);
            if (result == null)
            {
                return Json(new { result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult DeleteFavorite(long ReportFavoritesId)
        {
            ReportsWrapper reportsWrapper = new ReportsWrapper(userData);
            var result = reportsWrapper.DeleteFavorite(ReportFavoritesId);
            if (result == null)
            {
                return Json(new { result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region MasterGrid
        [HttpPost]
        public JsonResult GetTableData(long ReportListingId, Boolean IsUserReport, string MultiSelectData1, string MultiSelectData2, int CaseNo1 = 0, int CaseNo2 = 0,
            DateTime? StartDate1 = null, DateTime? EndDate1 = null, DateTime? StartDate2 = null, DateTime? EndDate2 = null)
        {
            ReportListingModel reportListingModel = new ReportListingModel();
            ReportsWrapper reportsWrapper = new ReportsWrapper(userData);
            List<GridColumnsProp> gridColumnsProps = new List<GridColumnsProp>();
            if (CaseNo1 != 0 && CaseNo1 != 6)
            {
                EndDate1 = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 0, 0, 0);
                StartDate1 = GetStartDate(CaseNo1);
            }
            if (CaseNo2 != 0 && CaseNo2 != 6)
            {
                EndDate2 = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 0, 0, 0);
                StartDate2 = GetStartDate(CaseNo2);
            }
            string timeoutError = string.Empty;
            var results = reportsWrapper.GetMasterGridReport(ReportListingId, IsUserReport, ref gridColumnsProps, ref reportListingModel, MultiSelectData1, MultiSelectData2,
                CaseNo1, CaseNo2, StartDate1, EndDate1, StartDate2, EndDate2);
            var  result = results.Item1;
            timeoutError = results.Item2;
            var jsonData = reportsWrapper.DataTableToJSONWithJSONNet(result);
            var jsonResult = Json(new { data = jsonData, columns = gridColumnsProps, repdetail = reportListingModel , timeoutError = timeoutError }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public PartialViewResult GetPromptModal(long ReportListingId, bool IsUserReport = false)
        {
            BusinessIntelligenceVM objVM = new BusinessIntelligenceVM();
            ReportsWrapper reportsWrapper = new ReportsWrapper(userData);
            ReportListingModel reportListingModel = new ReportListingModel();
            List<DropDownModel> multiSelectModelsP1 = new List<DropDownModel>();
            List<DropDownModel> multiSelectModelsP2 = new List<DropDownModel>();

            if (IsUserReport)
            {
                reportListingModel = reportsWrapper.GetUserReportListDetail(ReportListingId);
            }
            else
            {
                reportListingModel = reportsWrapper.GetReportListDetail(ReportListingId);
            }
            if (reportListingModel != null)
            {
                objVM.reportListingModel = reportListingModel;
                objVM.DateRangeDropListForReport = UtilityFunction.GetTimeRangeDropForReport().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });

                if (!string.IsNullOrWhiteSpace(reportListingModel.Prompt1Source) && !string.IsNullOrWhiteSpace(reportListingModel.Prompt1Type) && !string.IsNullOrWhiteSpace(reportListingModel.Prompt1ListSource) && !string.IsNullOrWhiteSpace(reportListingModel.Prompt1List))
                {
                    multiSelectModelsP1 = GetMultiSectect1ControlsData(reportListingModel);
                }
                if (!string.IsNullOrWhiteSpace(reportListingModel.Prompt2Source) && !string.IsNullOrWhiteSpace(reportListingModel.Prompt2Type) && !string.IsNullOrWhiteSpace(reportListingModel.Prompt2ListSource) && !string.IsNullOrWhiteSpace(reportListingModel.Prompt2List))
                {
                    multiSelectModelsP2 = GetMultiSectect2ControlsData(reportListingModel);
                }
            }
            objVM.multiSelectPrompt1 = multiSelectModelsP1;
            objVM.multiSelectPrompt2 = multiSelectModelsP2;
            LocalizeControls(objVM, LocalizeResourceSetConstants.Report);
            return PartialView("_PromptModal", objVM);
        }

        [HttpPost]
        public ActionResult GetTable(ReportListingModel ReportDetail)
        {
            BusinessIntelligenceVM objVM = new BusinessIntelligenceVM();
            List<DropDownModel> multiSelectModelsP1 = new List<DropDownModel>();
            List<DropDownModel> multiSelectModelsP2 = new List<DropDownModel>();

            objVM.IncludePrompt = ReportDetail.IncludePrompt;
            objVM.IsGrouped = ReportDetail.IsGrouped;
            objVM.IsUserReport = ReportDetail.IsUserReport;
            if (ReportDetail.IncludePrompt)
            {
                if (ReportDetail != null)
                {
                    objVM.reportListingModel = ReportDetail;
                }
                objVM.DateRangeDropListForReport = UtilityFunction.GetTimeRangeDropForReport().Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
                if (!string.IsNullOrWhiteSpace(ReportDetail.Prompt1Source) && !string.IsNullOrWhiteSpace(ReportDetail.Prompt1Type) && !string.IsNullOrWhiteSpace(ReportDetail.Prompt1ListSource) && !string.IsNullOrWhiteSpace(ReportDetail.Prompt1List))
                {
                    multiSelectModelsP1 = GetMultiSectect1ControlsData(ReportDetail);
                }
                if (!string.IsNullOrWhiteSpace(ReportDetail.Prompt2Source) && !string.IsNullOrWhiteSpace(ReportDetail.Prompt2Type) && !string.IsNullOrWhiteSpace(ReportDetail.Prompt2ListSource) && !string.IsNullOrWhiteSpace(ReportDetail.Prompt2List))
                {
                    multiSelectModelsP2 = GetMultiSectect2ControlsData(ReportDetail);
                }
                objVM.multiSelectPrompt1 = multiSelectModelsP1;
                objVM.multiSelectPrompt2 = multiSelectModelsP2;
            }
            objVM.security = this.userData.Security;
            LocalizeControls(objVM, LocalizeResourceSetConstants.Report);
            return PartialView("_Report", objVM);
        }
        private DateTime? GetStartDate(int CaseNo)
        {
            DateTime? startDate = null;
            DateTime toDay = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 0, 0, 0);

            switch (CaseNo)
            {
                case 0:
                    startDate = null;
                    break;
                case 1:
                    startDate = toDay.AddDays(0);
                    break;
                case 2:
                    startDate = toDay.AddDays(-7);
                    break;
                case 3:
                    startDate = toDay.AddDays(-30);
                    break;
                case 4:
                    startDate = toDay.AddDays(-60);
                    break;
                case 5:
                    startDate = toDay.AddDays(-90);
                    break;
            }
            return startDate;
        }
        private List<DropDownModel> GetMultiSectect1ControlsData(ReportListingModel ReportDetail)
        {
            ReportsWrapper reportsWrapper = new ReportsWrapper(userData);
            List<DropDownModel> multiSelectModel = new List<DropDownModel>();
            if (!string.IsNullOrWhiteSpace(ReportDetail.Prompt1Source) && !string.IsNullOrWhiteSpace(ReportDetail.Prompt1Type) && !string.IsNullOrWhiteSpace(ReportDetail.Prompt1ListSource) && !string.IsNullOrWhiteSpace(ReportDetail.Prompt1List))
            {
                if (ReportDetail.Prompt1Type.ToUpper() == ReportConstants.MULTISELECT && ReportDetail.Prompt1ListSource.ToUpper() == ReportConstants.CONSTANTSOURCE)
                {
                    multiSelectModel = reportsWrapper.GetMultiSelectValuesForConstant(ReportDetail.ReportListingId, ReportDetail.Prompt1List);
                }
                // This is Prompt1 not prompt 2
                else if (ReportDetail.Prompt1Type.ToUpper() == ReportConstants.MULTISELECT && ReportDetail.Prompt1ListSource.ToUpper() == ReportConstants.LOOKUPSOURCE)
                //else if (ReportDetail.Prompt2Type.ToUpper() == ReportConstants.MULTISELECT && ReportDetail.Prompt2ListSource.ToUpper() == ReportConstants.LOOKUPSOURCE)
                {
                    multiSelectModel = reportsWrapper.GetMultiSelectValuesForLookUp(ReportDetail.ReportListingId, ReportDetail.Prompt1List);
                }
            }
            return multiSelectModel;
        }
        private List<DropDownModel> GetMultiSectect2ControlsData(ReportListingModel ReportDetail)
        {
            ReportsWrapper reportsWrapper = new ReportsWrapper(userData);
            List<DropDownModel> multiSelectModel = new List<DropDownModel>();
            if (!string.IsNullOrWhiteSpace(ReportDetail.Prompt2Source) && !string.IsNullOrWhiteSpace(ReportDetail.Prompt2Type) && !string.IsNullOrWhiteSpace(ReportDetail.Prompt2ListSource) && !string.IsNullOrWhiteSpace(ReportDetail.Prompt2List))
            {
                if (ReportDetail.Prompt2Type.ToUpper() == ReportConstants.MULTISELECT && ReportDetail.Prompt2ListSource.ToUpper() == ReportConstants.CONSTANTSOURCE)
                {
                    multiSelectModel = reportsWrapper.GetMultiSelectValuesForConstant(ReportDetail.ReportListingId, ReportDetail.Prompt2List);
                }
                else if (ReportDetail.Prompt2Type.ToUpper() == ReportConstants.MULTISELECT && ReportDetail.Prompt2ListSource.ToUpper() == ReportConstants.LOOKUPSOURCE)
                {
                    multiSelectModel = reportsWrapper.GetMultiSelectValuesForLookUp(ReportDetail.ReportListingId, ReportDetail.Prompt2List);
                }
            }
            return multiSelectModel;
        }
        #endregion

        #region ChildGrid
        public JsonResult GetChildTableData(ReportListingModel ReportDetail, long Data, bool IsUserReport = false)
        {
            ReportsWrapper reportsWrapper = new ReportsWrapper(userData);
            List<GridColumnsProp> gridColumnsProps = new List<GridColumnsProp>();
            var result = reportsWrapper.GetChildGrid(ReportDetail.ReportListingId, ref gridColumnsProps, ref ReportDetail, Data, IsUserReport);
            var json = reportsWrapper.DataTableToJSONWithJSONNet(result);
            return Json(new { data = json, columns = gridColumnsProps }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetChildTable()
        {
            BusinessIntelligenceVM objVM = new BusinessIntelligenceVM();
            LocalizeControls(objVM, LocalizeResourceSetConstants.Report);
            return PartialView("_InnerGrid", objVM);
        }
        #endregion

        #region Export
        [HttpPost]
        public JsonResult SetPrintData(ReportPrintParams ReportPrintParams, List<GridColumnsProp> ColumnsProps, List<ReportFilterProp> FilterProp)
        {
            Session["RPPARAMS"] = ReportPrintParams;
            Session["COLPARAMS"] = ColumnsProps;
            Session["FILTERPARAMS"] = FilterProp;
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [NoDirectAccess]
        public ActionResult ExportASPDF(string d = "")
        {
            BusinessIntelligenceVM objVM = new BusinessIntelligenceVM();
            ReportListingModel reportListingModel = new ReportListingModel();
            ReportsWrapper reportsWrapper = new ReportsWrapper(userData);
            List<GridColumnsProp> gridColumnsProps = new List<GridColumnsProp>();
            List<GridColumnsProp> childgridColumnsProps = new List<GridColumnsProp>();
            List<ChildGridtPrintModel> childGridtPrintModels = new List<ChildGridtPrintModel>();
            ChildGridtPrintModel childGridtPrintModel;
            ReportPrintModel reportPrintModel = new ReportPrintModel();
            ReportPrintParams ReportPrintParams = (ReportPrintParams)Session["RPPARAMS"];
            List<GridColumnsProp> ColumnsProps = (List<GridColumnsProp>)Session["COLPARAMS"];
            List<ReportFilterProp> FilterProp = (List<ReportFilterProp>)Session["FILTERPARAMS"];

            if (ReportPrintParams.CaseNo1 != 0 && ReportPrintParams.CaseNo1 != 6)
            {
                ReportPrintParams.EndDate1 = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 0, 0, 0);
                ReportPrintParams.StartDate1 = GetStartDate(ReportPrintParams.CaseNo1);
            }
            if (ReportPrintParams.CaseNo2 != 0 && ReportPrintParams.CaseNo2 != 6)
            {
                ReportPrintParams.EndDate2 = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 0, 0, 0);
                ReportPrintParams.StartDate2 = GetStartDate(ReportPrintParams.CaseNo2);
            }
            var result = reportsWrapper.GetMasterGrid(ReportPrintParams.ReportListingId, ReportPrintParams.IsUserReport, ref gridColumnsProps, ref reportListingModel, ReportPrintParams.MultiSelectData1, ReportPrintParams.MultiSelectData2,
                ReportPrintParams.CaseNo1, ReportPrintParams.CaseNo2, ReportPrintParams.StartDate1, ReportPrintParams.EndDate1, ReportPrintParams.StartDate2, ReportPrintParams.EndDate2);

            //-- For column show / hide and position changing
            int ind = 0;
            //if (!ReportPrintParams.IsUserReport)
            //{
            ColumnsProps.ForEach(item =>
            {
                gridColumnsProps.Where(x => x.data == item.data).FirstOrDefault().bVisible = item.bVisible;
                gridColumnsProps.Where(x => x.data == item.data).FirstOrDefault().Sequence = ind;// item.Sequence;
                ind++;
            });
            //}

            var finalColumnList = gridColumnsProps.Where(x => x.bVisible == true).OrderBy(x => x.Sequence).ToList();
            if (!string.IsNullOrEmpty(reportListingModel.GroupColumn))
            {
                if (!finalColumnList.Any(f => f.data.ToLower() == reportListingModel.GroupColumn.ToLower()))
                {
                    finalColumnList.Add(gridColumnsProps.Where(x => x.data == reportListingModel.GroupColumn).FirstOrDefault());
                }
            }
            if (ReportPrintParams.HasChildGrid)
            {
                finalColumnList.Insert(0, gridColumnsProps.FirstOrDefault());
            }

            ind = 0;
            List<string> colnams = result.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();
            foreach (var colname in finalColumnList.Select(x => x.data))
            {
                result.Columns[colname].SetOrdinal(ind);
                colnams.Remove(colname);
                ind++;
            }
            if (colnams.Count > 0)
            {
                colnams.ForEach(item =>
                {
                    result.Columns.Remove(item);
                });
            }
            //-- 

            //-- For applying filter
            if (FilterProp != null)
            {
                DataView dv = result.DefaultView;
                StringBuilder sb = new StringBuilder("");
                FilterProp.ForEach(item =>
                {
                    if (item.Type == "string")
                    {
                        if (sb.ToString() != "")
                        {
                            sb.Append(" AND ");
                        }
                        sb.Append(item.ColumnName + " LIKE '%" + item.Searchval + "%'");

                    }
                    else if (item.Type == "multiselect")
                    {
                        if (sb.ToString() != "")
                        {
                            sb.Append(" AND ");
                        }
                        string[] val = JsonConvert.DeserializeObject<string[]>(item.Searchval);
                        string search = "";
                        for (int i = 0; i < val.Length; i++)
                        {
                            if (search == "")
                            {
                                search = val[i];
                            }
                            else
                            {
                                search = search + "','" + val[i];
                            }
                        }
                        sb.Append(item.ColumnName + " in ('" + search + "')");
                    }
                });
                dv.RowFilter = sb.ToString();
                result = dv.ToTable();
            }
            //-- 

            reportPrintModel.gridColumnsProps = finalColumnList;
            reportPrintModel.ReportData = result;

            if (ReportPrintParams.HasChildGrid)
            {
                reportPrintModel.hasChild = true;
                for (int i = 0; i < result.Rows.Count; i++)
                {
                    DataRow thisRow = result.Rows[i];
                    int childCount = Convert.ToInt32(thisRow.ItemArray[0]);
                    if (childCount > 0)
                    {
                        childGridtPrintModel = new ChildGridtPrintModel();
                        childGridtPrintModel.ReportData = new DataTable();
                        var childData = reportsWrapper.GetChildGrid(ReportPrintParams.ReportListingId, ref childgridColumnsProps, ref reportListingModel, Convert.ToInt32(thisRow.ItemArray[1]), reportListingModel.IsUserReport);
                        //-- removing hidden column
                        childgridColumnsProps.Where(x => x.bVisible == false).ToList().Select(x => x.data).ToList().ForEach(colName =>
                        {
                            childData.Columns.Remove(colName);
                        });
                        //-- removing hidden column
                        childGridtPrintModel.ReportData = childData;
                        childGridtPrintModel.gridColumnsProps = childgridColumnsProps.Where(x => x.bVisible == true).ToList();
                        childGridtPrintModels.Add(childGridtPrintModel);
                    }

                }
                reportPrintModel.ChildGrids = childGridtPrintModels;
            }
            reportPrintModel.IsGrouped = ReportPrintParams.IsGrouped;
            reportPrintModel.GroupColumn = reportListingModel.GroupColumn;

            objVM.reportPrintModel = reportPrintModel;
            LocalizeControls(objVM, LocalizeResourceSetConstants.PurchaseOrder);

            if (d == "excel")
            {
                return GenerateExcelReport(reportPrintModel, reportListingModel.ReportName);
            }

            if (d == "d")
            {
                return new PartialViewAsPdf("_PrintTemplate", objVM)
                {
                    PageSize = Size.A3,
                    FileName = reportListingModel.ReportName + ".pdf",
                    PageMargins = { Left = 3, Right = 3, Top = 3, Bottom = 3 },
                };
            }
            else
            {
                return new ViewAsPdf("_PrintTemplate", objVM)
                {
                    PageSize = Size.A3,
                    PageMargins = { Left = 3, Right = 3, Top = 3, Bottom = 3 },
                };
            }

        }
        #endregion

        #region CreateEvent
        public JsonResult CreateReportEventLog(long ReportListingId, bool IsUserReports, string Event)
        {
            ReportsWrapper reportsWrapper = new ReportsWrapper(userData);
            reportsWrapper.CreateReportEventLog(ReportListingId, IsUserReports, Event);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Add - Edit Report

        #region Private Report
        public PartialViewResult PrivateReportAddOrEdit(long UserReportsId, long SourceId = 0, bool IsUserReport = true)
        {
            BusinessIntelligenceVM objVM = new BusinessIntelligenceVM();
            objVM.userReportsModel = new UserReportsModel();
            ReportsWrapper reportsWrapper = new ReportsWrapper(userData);
            objVM.userReportsModel.UserReportsId = UserReportsId;
            objVM.userReportsModel.SourceId = SourceId;
            objVM.userReportsModel.IsUserReport = IsUserReport;
            if (UserReportsId != 0)
            {
                objVM.userReportsModel = reportsWrapper.GetUserReportDetailsById(UserReportsId);

            }
            else
            {
                objVM.userReportsModel.SaveType = ReportTypeConstants.Private;
            }

            LocalizeControls(objVM, LocalizeResourceSetConstants.Report);
            return PartialView("~/Views/Reports/_PrivateReportAddOrEdit.cshtml", objVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PrivateReportAddOrEdit(BusinessIntelligenceVM objVM)
        {
            List<string> ErrorList = new List<string>();
            ReportsWrapper reportsWrapper = new ReportsWrapper(userData);
            string ModelValidationFailedMessage = string.Empty;
            string Mode = string.Empty;
            if (ModelState.IsValid)
            {
                objVM.userReportsModel.SaveType = ReportTypeConstants.Private;
                UserReports UserReports = new UserReports();
                if (objVM.userReportsModel.UserReportsId > 0)
                {
                    Mode = "Edit";
                    UserReports = reportsWrapper.EditReport(objVM.userReportsModel);
                }
                else
                {
                    Mode = "Add";
                    UserReports = reportsWrapper.AddReport(objVM.userReportsModel);
                }
                if (UserReports.ErrorMessages != null && UserReports.ErrorMessages.Count > 0)
                {
                    return Json(UserReports.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Site Report
        public PartialViewResult SiteReportAddOrEdit(long UserReportsId, long SourceId = 0, bool IsUserReport = true)
        {
            BusinessIntelligenceVM objVM = new BusinessIntelligenceVM();
            objVM.userReportsModel = new UserReportsModel();
            ReportsWrapper reportsWrapper = new ReportsWrapper(userData);
            objVM.userReportsModel.UserReportsId = UserReportsId;
            objVM.userReportsModel.SourceId = SourceId;
            objVM.userReportsModel.IsUserReport = IsUserReport;
            if (UserReportsId != 0)
            {
                objVM.userReportsModel = reportsWrapper.GetUserReportDetailsById(UserReportsId);

            }
            else
            {
                objVM.userReportsModel.SaveType = ReportTypeConstants.Site;
            }

            LocalizeControls(objVM, LocalizeResourceSetConstants.Report);
            return PartialView("~/Views/Reports/_SiteReportAddOrEdit.cshtml", objVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SiteReportAddOrEdit(BusinessIntelligenceVM objVM)
        {
            List<string> ErrorList = new List<string>();
            ReportsWrapper reportsWrapper = new ReportsWrapper(userData);
            string ModelValidationFailedMessage = string.Empty;
            string Mode = string.Empty;
            if (ModelState.IsValid)
            {
                UserReports UserReports = new UserReports();
                if (objVM.userReportsModel.UserReportsId > 0)
                {
                    Mode = "Edit";
                    UserReports = reportsWrapper.EditReport(objVM.userReportsModel);
                }
                else
                {
                    Mode = "Add";
                    UserReports = reportsWrapper.AddReport(objVM.userReportsModel);
                }

                if (UserReports.ErrorMessages != null && UserReports.ErrorMessages.Count > 0)
                {
                    return Json(UserReports.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion


        #region Public Report
        public PartialViewResult PublicReportAddOrEdit(long UserReportsId, long SourceId = 0, bool IsUserReport = true)
        {
            BusinessIntelligenceVM objVM = new BusinessIntelligenceVM();
            objVM.userReportsModel = new UserReportsModel();
            ReportsWrapper reportsWrapper = new ReportsWrapper(userData);
            objVM.userReportsModel.UserReportsId = UserReportsId;
            objVM.userReportsModel.SourceId = SourceId;
            objVM.userReportsModel.IsUserReport = IsUserReport;
            if (UserReportsId != 0)
            {
                objVM.userReportsModel = reportsWrapper.GetUserReportDetailsById(UserReportsId);

            }
            else
            {
                objVM.userReportsModel.SaveType = ReportTypeConstants.Public;
            }
            LocalizeControls(objVM, LocalizeResourceSetConstants.Report);
            return PartialView("~/Views/Reports/_PublicReportAddOrEdit.cshtml", objVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PublicReportAddOrEdit(BusinessIntelligenceVM objVM)
        {
            List<string> ErrorList = new List<string>();
            ReportsWrapper reportsWrapper = new ReportsWrapper(userData);
            string ModelValidationFailedMessage = string.Empty;
            string Mode = string.Empty;
            UserReports UserReports = new UserReports();
            if (ModelState.IsValid)
            {
                if (objVM.userReportsModel.UserReportsId > 0)
                {
                    Mode = "Edit";
                    UserReports = reportsWrapper.EditReport(objVM.userReportsModel);
                }
                else
                {
                    Mode = "Add";
                    UserReports = reportsWrapper.AddReport(objVM.userReportsModel);
                }


                if (UserReports.ErrorMessages != null && UserReports.ErrorMessages.Count > 0)
                {
                    return Json(UserReports.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        [HttpPost]
        public ActionResult CheckIfUserReportNameExist(BusinessIntelligenceVM objVM)
        {
            bool IfUserReportNameExist = false;
            int count = 0;
            ReportsWrapper reportsWrapper = new ReportsWrapper(userData);
            count = reportsWrapper.CountIfUserReportNameExist(objVM.userReportsModel.ReportName);
            if (count > 0)
            {
                IfUserReportNameExist = true;
            }
            else
            {
                IfUserReportNameExist = false;
            }
            return Json(!IfUserReportNameExist, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Delete Report
        public ActionResult DeleteUserReport(long UserReportsId)
        {
            ReportsWrapper reportsWrapper = new ReportsWrapper(userData);
            var deleteResult = reportsWrapper.DeleteUserReport(UserReportsId);
            if (deleteResult)
            {
                return Json(JsonReturnEnum.success.ToString(), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(JsonReturnEnum.failed.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Save Report Setting
        public JsonResult SaveUsersReportSettings(long ReportId, List<ReportConfigModel> config, List<ReportFilteConfigModel> filterConfig)
        {
            ReportsWrapper reportsWrapper = new ReportsWrapper(userData);
            var updateResult = new List<string>();
            if (config != null && config.Count > 0)
            {
                updateResult = reportsWrapper.SaveUsersReportSettings(ReportId, config);
            }
            if (filterConfig != null && filterConfig.Count > 0)
            {
                updateResult = reportsWrapper.SaveUsersReportFilterSettings(ReportId, filterConfig);
            }
            if (updateResult != null && updateResult.Count > 0)
            {
                return Json(updateResult, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(JsonReturnEnum.success.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Export Report to excel functionality
        public ActionResult GenerateExcelReport(ReportPrintModel reportPrintModel, string reportName)
        {
            //This method is used to create excel sheet in memory and export the sheet.
            MemoryStream memoryStream = new MemoryStream();
            using (SpreadsheetDocument package = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
            {

                CreatePartsForExcel(package, reportPrintModel);
                package.Close();
                memoryStream.Seek(0, SeekOrigin.Begin);
                return File(memoryStream.ToArray(),
                  "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
               string.Concat(reportName, ".xlsx"));

            }
        }
        private void CreatePartsForExcel(SpreadsheetDocument document, ReportPrintModel data)
        {
            //This method is creating sheetdata and its related style
            int length = 0;
            int TotalHeadercolumns = 0;
            List<int> groupRowIndexes = new List<int>();
            SheetData partSheetData = GenerateSheetdataForDetails(data, ref groupRowIndexes, ref length);
            WorkbookPart workbookPart1 = document.AddWorkbookPart();
            GenerateWorkbookPartContent(workbookPart1);

            //based on maximum cell size contain we indetify 
            //export data has big size data ane be make 
            // hasBigRow flag as true
            bool hasBigRows = false;
            if (length > 50)
            {
                hasBigRows = true;
            }
            TotalHeadercolumns = CountOfMaximumColumn(data);

            WorkbookStylesPart workbookStylesPart1 = workbookPart1.AddNewPart<WorkbookStylesPart>("rId3");
            GenerateWorkbookStylesPartContent(workbookStylesPart1, hasBigRows);

            WorksheetPart worksheetPart1 = workbookPart1.AddNewPart<WorksheetPart>("rId1");
            GenerateWorksheetPartContent(worksheetPart1, partSheetData, groupRowIndexes, TotalHeadercolumns, hasBigRows);

        }
        private SheetData GenerateSheetdataForDetails(ReportPrintModel reportPrintModel, ref List<int> groupRowIndexes, ref int length)
        {
            //for creating excel logic used same as per pdf export
            //based on report model data different rows created and attached to sheetdata 
            //and return sheetdata to caller function
            SheetData sheetData1 = new SheetData();
            int loopStartCount = 0;
            int loopEndCount = 0;
            int tblColCount = 0;
            int lastheader = 0;
            int tditemarraydeduction = 0;
            var dataTable = reportPrintModel.ReportData;
            tblColCount = reportPrintModel.gridColumnsProps.Count;
            int dtcolumnCount = reportPrintModel.ReportData.Columns.Count;
            int colspan = dtcolumnCount - 1;
            string prevGroup = "";
            string groupColumn = reportPrintModel.GroupColumn;
            int groupColumnIdx = reportPrintModel.gridColumnsProps.Select(x => x.data.ToLower()).ToList().IndexOf(groupColumn.ToLower());
            if (groupColumnIdx > -1)
            {
                prevGroup = dataTable.Rows[0][groupColumnIdx].ToString();
            }
            string currentGroup = prevGroup;
            int thisRow = 0;
            bool hasGrouped = reportPrintModel.IsGrouped;
            bool hasGroupTotal = false;
            if (hasGrouped)
            {
                hasGroupTotal = reportPrintModel.gridColumnsProps.Any(x => x.IsGroupTotaled == true);
                //-- group column could have any position in the list
                if (reportPrintModel.gridColumnsProps.Any(gp => gp.data.ToLower() == groupColumn.ToLower() && gp.bVisible == false))
                {
                    loopEndCount = dtcolumnCount - 1;
                    tditemarraydeduction = 1;
                    lastheader = tblColCount - 2;
                }
                else
                {
                    loopEndCount = dtcolumnCount;
                    lastheader = tblColCount - 1;
                    colspan++;
                }
            }
            else
            {
                loopEndCount = dtcolumnCount;
                lastheader = tblColCount - 1;
            }
            bool hasGrandTotal = reportPrintModel.gridColumnsProps.Any(x => x.IsGrandTotal == true);
            decimal[] grantTotalCounts = new decimal[dtcolumnCount];

            decimal[] groupTotalCounts = new decimal[dtcolumnCount];
            bool hasChild = reportPrintModel.hasChild;
            List<string> groupTotalCell = new List<string>();
            List<string> grandTotalCell = new List<string>();
            List<string> Allcells = new List<string>();
            if (hasChild)
            {
                loopStartCount = 2;
            }

            int childgriditemcount = 0;


            List<string> GroupTotal = new List<string>();
            // for ui perspect and display blank cell at the 
            //end of row we are using this logic 
            int MaxTotalgridColCount = CountOfMaximumColumn(reportPrintModel);

            //create header column of the excel sheet
            sheetData1.Append(CreateHeaderRow(reportPrintModel,MaxTotalgridColCount));


                //(from d in reportPrintModel.gridColumnsProps.ToList().Where(x => x.title != null && x.title.Length > 0 && x.bVisible).ToList()
                //                     select d.title).ToList().Count;


            if (hasGrouped)
            {
                //creating group header of the sheet
                Row headerowtitle = CreateGroupHeader(prevGroup, MaxTotalgridColCount);
                sheetData1.Append(headerowtitle);
                groupRowIndexes.Add(sheetData1.ChildElements.Count);
                //above line used in logic to merge the cells groups header row
            }

            foreach (DataRow row in reportPrintModel.ReportData.Rows)
            {
                if (hasGrouped)
                {
                    currentGroup = row[groupColumnIdx].ToString();
                    if (currentGroup != prevGroup)
                    {
                        if (hasGroupTotal)
                        {
                            int count = 0;
                            for (int item = loopStartCount; item < dtcolumnCount - 1; item++)
                            {
                                if (reportPrintModel.gridColumnsProps[item].IsGroupTotaled)
                                {
                                    groupTotalCell.Add(UtilityFunction.GetFormattedData(groupTotalCounts[count], reportPrintModel.gridColumnsProps[item]).ToString());
                                }
                                else
                                {
                                    groupTotalCell.Add("");
                                }
                                count++;
                            }

                        }
                        prevGroup = currentGroup;
                        //create row for group
                        Row groupTotalrow = CreateRowData(groupTotalCell, MaxTotalgridColCount, 2U);
                        sheetData1.Append(groupTotalrow);
                        groupTotalCell.Clear();

                        //create group header row
                        Row headerowtitle = CreateGroupHeader(prevGroup, MaxTotalgridColCount);
                        sheetData1.Append(headerowtitle);
                        groupRowIndexes.Add(sheetData1.ChildElements.Count);
                        //above line index is used to merge the cells of group header row
                        groupTotalCounts = new decimal[dtcolumnCount];
                    }
                }

                List<string> Parentcells = new List<string>();
                List<string> Childcells = new List<string>();

                if (thisRow % 2 == 0)
                {
                    for (int i = loopStartCount; i < row.ItemArray.Count() - tditemarraydeduction; i++)
                    {
                        if (reportPrintModel.gridColumnsProps[i].IsGrandTotal)
                        {
                            grantTotalCounts[i] = grantTotalCounts[i] + Convert.ToDecimal(row.ItemArray[i]);
                        }
                        if (reportPrintModel.gridColumnsProps[i].IsGroupTotaled)
                        {
                            groupTotalCounts[i] = groupTotalCounts[i] + Convert.ToDecimal(row.ItemArray[i]);
                        }
                        if (i == row.ItemArray.Count() - tditemarraydeduction)
                        {
                            Parentcells.Add(UtilityFunction.GetFormattedData(row.ItemArray[i], reportPrintModel.gridColumnsProps[i]).ToString());
                        }
                        else
                        {
                            Parentcells.Add(UtilityFunction.GetFormattedData(row.ItemArray[i], reportPrintModel.gridColumnsProps[i]).ToString());
                        }

                    }
                    Row datarow = CreateRowData(Parentcells, MaxTotalgridColCount, 1U);
                    sheetData1.Append(datarow);
                    Allcells.AddRange(Parentcells);
                    Parentcells.Clear();
                }
                else
                {

                    for (int i = loopStartCount; i < row.ItemArray.Count() - tditemarraydeduction; i++)
                    {
                        if (reportPrintModel.gridColumnsProps[i].IsGrandTotal)
                        {
                            grantTotalCounts[i] = grantTotalCounts[i] + Convert.ToDecimal(row.ItemArray[i]);
                        }
                        if (reportPrintModel.gridColumnsProps[i].IsGroupTotaled)
                        {
                            groupTotalCounts[i] = groupTotalCounts[i] + Convert.ToDecimal(row.ItemArray[i]);

                        }
                        if (i == row.ItemArray.Count() - tditemarraydeduction)
                        {
                            Parentcells.Add(UtilityFunction.GetFormattedData(row.ItemArray[i], reportPrintModel.gridColumnsProps[i]).ToString());
                        }
                        else
                        {
                            Parentcells.Add(UtilityFunction.GetFormattedData(row.ItemArray[i], reportPrintModel.gridColumnsProps[i]).ToString());
                        }
                    }
                    Allcells.AddRange(Parentcells);
                    //crating data row for particular group
                    Row datarow = CreateRowData(Parentcells, MaxTotalgridColCount);
                    sheetData1.Append(datarow);
                    Parentcells.Clear();
                }

                if (hasChild)
                {
                    int childCount = Convert.ToInt32(row.ItemArray[0]);
                    if (childCount > 0)
                    {
                        var childmodel = reportPrintModel.ChildGrids[childgriditemcount];
                        Row Childheaderowtitle = CreateHeaderRowForChildGrid(childmodel,MaxTotalgridColCount);
                        sheetData1.Append(Childheaderowtitle);
                        //This logic different than print logic because of this total 
                        //group total will be affected
                        decimal[] grantTotalCountsChild = new decimal[dtcolumnCount];

                        foreach (DataRow row1 in reportPrintModel.ChildGrids[childgriditemcount].ReportData.Rows)
                        {
                            for (int i = 0; i < row1.ItemArray.Count(); i++)
                            {
                                if (reportPrintModel.ChildGrids[childgriditemcount].gridColumnsProps[i].IsGrandTotal)
                                {
                                    grantTotalCountsChild[i] = grantTotalCountsChild[i] + Convert.ToDecimal(row1.ItemArray[i]);

                                }
                                if (i == row1.ItemArray.Count())
                                {
                                    Childcells.Add(UtilityFunction.GetFormattedData(row1.ItemArray[i], reportPrintModel.ChildGrids[childgriditemcount].gridColumnsProps[i]).ToString());
                                }
                                else
                                {
                                    Childcells.Add(UtilityFunction.GetFormattedData(row1.ItemArray[i], reportPrintModel.ChildGrids[childgriditemcount].gridColumnsProps[i]).ToString());
                                }
                            }
                            Allcells.AddRange(Childcells);
                            //creating rows for child grid data
                            Row datarow = CreateRowData(Childcells, MaxTotalgridColCount);
                            sheetData1.Append(datarow);
                            Childcells.Clear();
                        }
                        List<string> childTotal = new List<string>();
                        hasGrandTotal = reportPrintModel.ChildGrids[childgriditemcount].gridColumnsProps.Any(x => x.IsGrandTotal == true);
                        if (hasGrandTotal)
                        {
                            int count = 0;
                            // RKL 2023-Feb-08 - I think this should be the child column count - not the header
                            //for (int item = 0; item < dtcolumnCount; item++)
                            int clcolumncount = reportPrintModel.ChildGrids[childgriditemcount].gridColumnsProps.Count;
                            for (int item = 0; item < clcolumncount; item++)
                            {
                                if (reportPrintModel.ChildGrids[childgriditemcount].gridColumnsProps[item].IsGrandTotal)
                                {
                                    //adding child total in total array
                                    childTotal.Add(UtilityFunction.GetFormattedData(grantTotalCountsChild[count], reportPrintModel.ChildGrids[childgriditemcount].gridColumnsProps[item]).ToString());
                                }
                                else
                                {
                                    //creating blank cells
                                    childTotal.Add("");
                                }
                                count++;
                            }
                            //create row for child Grid Total
                            Row datarow = CreateRowData(childTotal, MaxTotalgridColCount, 2U);
                            sheetData1.Append(datarow);
                            childTotal.Clear();
                        }
                        childgriditemcount++;
                    }
                }
                thisRow++;
            }

            if (hasGroupTotal)
            {
                int count = 0;
                for (int item = loopStartCount; item < dtcolumnCount - 1; item++)
                {
                    if (reportPrintModel.gridColumnsProps[item].IsGroupTotaled)
                    {
                        //adding group total cell in total array
                        groupTotalCell.Add(UtilityFunction.GetFormattedData(groupTotalCounts[count], reportPrintModel.gridColumnsProps[item]).ToString());
                    }
                    else
                    {
                        //creating blank cell for group total
                        groupTotalCell.Add("");
                    }
                    count++;
                }
                //create row for group total
                Row groupTotal = CreateRowData(groupTotalCell, MaxTotalgridColCount, 2U);
                sheetData1.Append(groupTotal);
                groupTotalCell.Clear();

            }

            if (hasGrandTotal)
            {
                int count = 0;
                for (int item = loopStartCount; item < loopEndCount; item++)
                {
                    if (reportPrintModel.gridColumnsProps[item].IsGrandTotal)
                    {
                        //adding data for grand total
                        grandTotalCell.Add(UtilityFunction.GetFormattedData(grantTotalCounts[count], reportPrintModel.gridColumnsProps[item]).ToString());
                    }
                    else
                    {
                        //creating blank cells for grand total
                        grandTotalCell.Add("");
                    }
                    count++;
                }
                //creating row for grand total
                Row grandTotal = CreateRowData(grandTotalCell, MaxTotalgridColCount, 2U);
                sheetData1.Append(grandTotal);
                grandTotalCell.Clear();
            }

            //below code is used to identity if cotent has 
            //bigger size data like description we are sending make size 
            //cell length
            // var result = Allcells.OrderByDescending(s => s.Length).First();
            if (Allcells.Count > 0)
            {
                length = Allcells.OrderByDescending(s => s.Length).First().Length;
            }

            return sheetData1;
        }
        private Row CreateHeaderRow(ReportPrintModel data,int MaxTotalgridColCount)
        {
             //creating parent grid header row
            var headrs = (from d in data.gridColumnsProps.ToList().Where(x => x.title != null && x.title.Length > 0 && x.bVisible).ToList()
                          select d.title).ToList();

            return CreateRowData(headrs, MaxTotalgridColCount, 2U);
        }
        private Row CreateHeaderRowForChildGrid(ChildGridtPrintModel data,int MaxTotalgridColCount)
        {
            //creating child grid header row
            var headrs = (from d in data.gridColumnsProps.ToList().Where(x => x.title != null && x.title.Length > 0 && x.bVisible).ToList()
                          select d.title).ToList();
            return CreateRowData(headrs,0, 2U);

        }
        //this function will find maximum columns in report 
        //based on child or parent grid whichever has highest column will be 
        // return and based on blank cells will be created for the excel report
        private int CountOfMaximumColumn(ReportPrintModel data)
        {
            List<int> columnCounts = new List<int>();
            var headercolumnscount = (from d in data.gridColumnsProps.ToList().Where(x => x.title != null && x.title.Length > 0 && x.bVisible).ToList()
                          select d.title).ToList().Count;
            columnCounts.Add(headercolumnscount);
            if (data.hasChild)
                {
                var childcolumnscount = (from d in data.ChildGrids[0].gridColumnsProps.ToList().Where(x => x.title != null && x.title.Length > 0 && x.bVisible).ToList()
                                         select d.title).ToList().Count;
                columnCounts.Add(childcolumnscount);
            }
            return columnCounts.Max();

        }
        #endregion

        #region V2-1073
        [HttpPost]
        public ActionResult PrintPOByAccount(POByAccoutntReportDevExpressPrintModel model)
        {
            POByAccoutntReportDevExpressPrintModel pOByAccoutntReportDevExpressPrintModel = new POByAccoutntReportDevExpressPrintModel();
            var POItemsDevExpressPrintModelList=new List<POItemsDevExpressPrintModel>();
            var POItemsDevExpressPrintModel=new POItemsDevExpressPrintModel();
            ReportsWrapper reportsWrapper = new ReportsWrapper(userData);
            var starDate = model.StartDate ?? DateTime.UtcNow.AddDays(-10);
            var endDate = model.EndDate ?? DateTime.UtcNow;
            pOByAccoutntReportDevExpressPrintModel.StartDate = starDate;
            pOByAccoutntReportDevExpressPrintModel.EndDate = endDate;
            var purchaseOrders = reportsWrapper.POByAccountReport(starDate, endDate);
            // RKL - 2024-Nov-26
            // Fill a dictionary with the localizations
            Dictionary<string,string> rptmsg = new System.Collections.Generic.Dictionary<string,string>();
            rptmsg.Add("spnCreated", UtilityFunction.GetMessageFromResource("spnCreated", LocalizeResourceSetConstants.Global));
            rptmsg.Add("globalthrough", UtilityFunction.GetMessageFromResource("globalthrough", LocalizeResourceSetConstants.Global));
            rptmsg.Add("spnPurchaseOrderByAccount", UtilityFunction.GetMessageFromResource("spnPurchaseOrderByAccount", LocalizeResourceSetConstants.Global));
            rptmsg.Add("GlobalAccount", UtilityFunction.GetMessageFromResource("GlobalAccount", LocalizeResourceSetConstants.Global));
            rptmsg.Add("spnPurchaseOrder", UtilityFunction.GetMessageFromResource("spnPurchaseOrder", LocalizeResourceSetConstants.Global));
            rptmsg.Add("GlobalReason", UtilityFunction.GetMessageFromResource("GlobalReason", LocalizeResourceSetConstants.Global));
            rptmsg.Add("GlobalVendor", UtilityFunction.GetMessageFromResource("GlobalVendor", LocalizeResourceSetConstants.Global));
            rptmsg.Add("GlobalVendorName", UtilityFunction.GetMessageFromResource("GlobalVendorName", LocalizeResourceSetConstants.Global));
            rptmsg.Add("GlobalStatus", UtilityFunction.GetMessageFromResource("GlobalStatus", LocalizeResourceSetConstants.Global));
            rptmsg.Add("globalOrderDate", UtilityFunction.GetMessageFromResource("globalOrderDate", LocalizeResourceSetConstants.Global));
            rptmsg.Add("spnPoLineNo", UtilityFunction.GetMessageFromResource("spnPoLineNo", LocalizeResourceSetConstants.PurchaseOrder));
            rptmsg.Add("spnPartID", UtilityFunction.GetMessageFromResource("spnPartID", LocalizeResourceSetConstants.Global));
            rptmsg.Add("spnDescription", UtilityFunction.GetMessageFromResource("spnDescription", LocalizeResourceSetConstants.Global));
            rptmsg.Add("spnPoOrderQty", UtilityFunction.GetMessageFromResource("spnPoOrderQty", LocalizeResourceSetConstants.PurchaseOrder));
            rptmsg.Add("spnPdUOM", UtilityFunction.GetMessageFromResource("spnPdUOM", LocalizeResourceSetConstants.Global));
            rptmsg.Add("globalLineCost", UtilityFunction.GetMessageFromResource("globalLineCost", LocalizeResourceSetConstants.Global));
            rptmsg.Add("spnQtyRec", UtilityFunction.GetMessageFromResource("spnQtyRec", LocalizeResourceSetConstants.PurchaseOrder));
            rptmsg.Add("spnReceivedCost", UtilityFunction.GetMessageFromResource("spnReceivedCost", LocalizeResourceSetConstants.Global));
            rptmsg.Add("globalQtyRemaining", UtilityFunction.GetMessageFromResource("globalQtyRemaining", LocalizeResourceSetConstants.Global));
            rptmsg.Add("globalRemainingCost", UtilityFunction.GetMessageFromResource("globalRemainingCost", LocalizeResourceSetConstants.Global));
            rptmsg.Add("globalTotalCostForAccount", UtilityFunction.GetMessageFromResource("globalTotalCostForAccount", LocalizeResourceSetConstants.Global));
            rptmsg.Add("globalTotalRemaining", UtilityFunction.GetMessageFromResource("globalTotalRemaining", LocalizeResourceSetConstants.Global));
            rptmsg.Add("spnTotalReceived", UtilityFunction.GetMessageFromResource("spnTotalReceived", LocalizeResourceSetConstants.StoreroomTransfers));
            rptmsg.Add("globalGrandTotalofLineReceived", UtilityFunction.GetMessageFromResource("globalGrandTotalofLineReceived", LocalizeResourceSetConstants.Global));
            rptmsg.Add("globalGrandTotalofLineRemaining", UtilityFunction.GetMessageFromResource("globalGrandTotalofLineRemaining", LocalizeResourceSetConstants.Global));
            rptmsg.Add("globalGrandTotalofLineTotal", UtilityFunction.GetMessageFromResource("globalGrandTotalofLineTotal", LocalizeResourceSetConstants.Global));
            List<PurchaseOrder> listOfPurchaseOrder = purchaseOrders.listOfPO;
            foreach(var item in listOfPurchaseOrder)
            {
                POItemsDevExpressPrintModel = new POItemsDevExpressPrintModel();
                POItemsDevExpressPrintModel.AccountClientLookupId = item.AccountClientLookupId;
                POItemsDevExpressPrintModel.AccountName = item.AccountName;
                POItemsDevExpressPrintModel.VendorClientLookupId = item.VendorClientLookupId;
                POItemsDevExpressPrintModel.VendorName = item.VendorName;
                POItemsDevExpressPrintModel.ClientLookupId = item.ClientLookupId;
                POItemsDevExpressPrintModel.Reason = item.Reason;
                POItemsDevExpressPrintModel.Status = item.Status;
                if (item.CreateDate == null || item.CreateDate == default(DateTime))
                {
                    POItemsDevExpressPrintModel.CreateDate = "";
                }
                else
                {
                    POItemsDevExpressPrintModel.CreateDate = Convert.ToDateTime(item.CreateDate).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                POItemsDevExpressPrintModel.LineNumber = item.LineNumber;
                POItemsDevExpressPrintModel.PartClientLookupId = item.PartClientLookupId;
                POItemsDevExpressPrintModel.POLDescription = item.POLDescription;
                POItemsDevExpressPrintModel.OrderQuantity = item.OrderQuantity;
                POItemsDevExpressPrintModel.UnitOfMeasure = item.UnitOfMeasure;
                POItemsDevExpressPrintModel.UnitCost = item.UnitCost;
                POItemsDevExpressPrintModel.LineTotal = item.LineTotal;
                POItemsDevExpressPrintModel.QuantityReceived = item.QuantityReceived;
                POItemsDevExpressPrintModel.ReceivedTotalCost = item.ReceivedTotalCost;
                POItemsDevExpressPrintModel.RemainingQuantity = item.OrderQuantity - item.QuantityReceived;
                POItemsDevExpressPrintModel.RemainingCost = item.LineTotal - item.ReceivedTotalCost;
                POItemsDevExpressPrintModel.LineItemStatus = item.LineItemStatus;
                POItemsDevExpressPrintModel.FromDate = Convert.ToDateTime(starDate).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                POItemsDevExpressPrintModel.ToDate = Convert.ToDateTime(endDate).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                POItemsDevExpressPrintModel.ReportDate = Convert.ToDateTime(DateTime.Now).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                #region Localization
                POItemsDevExpressPrintModel.spnCreated = rptmsg["spnCreated"];
                POItemsDevExpressPrintModel.globalthrough = rptmsg["globalthrough"];
                POItemsDevExpressPrintModel.spnPurchaseOrderByAccount = rptmsg["spnPurchaseOrderByAccount"];
                POItemsDevExpressPrintModel.GlobalAccount = rptmsg["GlobalAccount"];
                POItemsDevExpressPrintModel.spnPurchaseOrder = rptmsg["spnPurchaseOrder"];
                POItemsDevExpressPrintModel.GlobalReason = rptmsg["GlobalReason"];
                POItemsDevExpressPrintModel.GlobalVendor = rptmsg["GlobalVendor"];
                POItemsDevExpressPrintModel.GlobalVendorName = rptmsg["GlobalVendorName"];
                POItemsDevExpressPrintModel.GlobalStatus = rptmsg["GlobalStatus"];
                POItemsDevExpressPrintModel.globalOrderDate = rptmsg["globalOrderDate"];
                POItemsDevExpressPrintModel.spnPoLineNo = rptmsg["spnPoLineNo"];
                POItemsDevExpressPrintModel.spnPartID = rptmsg["spnPartID"];
                POItemsDevExpressPrintModel.spnDescription = rptmsg["spnDescription"];
                POItemsDevExpressPrintModel.spnPoOrderQty = rptmsg["spnPoOrderQty"];
                POItemsDevExpressPrintModel.spnPdUOM = rptmsg["spnPdUOM"];
                POItemsDevExpressPrintModel.globalLineCost = rptmsg["globalLineCost"];
                POItemsDevExpressPrintModel.spnQtyRec = rptmsg["spnQtyRec"];
                POItemsDevExpressPrintModel.spnReceivedCost = rptmsg["spnReceivedCost"];
                POItemsDevExpressPrintModel.globalQtyRemaining = rptmsg["globalQtyRemaining"];
                POItemsDevExpressPrintModel.globalRemainingCost = rptmsg["globalRemainingCost"];
                POItemsDevExpressPrintModel.globalTotalCostForAccount = rptmsg["globalTotalCostForAccount"];
                POItemsDevExpressPrintModel.globalTotalRemaining = rptmsg["globalTotalRemaining"];
                POItemsDevExpressPrintModel.spnTotalReceived = rptmsg["spnTotalReceived"];
                POItemsDevExpressPrintModel.globalGrandTotalofLineReceived = rptmsg["globalGrandTotalofLineReceived"];
                POItemsDevExpressPrintModel.globalGrandTotalofLineRemaining = rptmsg["globalGrandTotalofLineRemaining"];
                POItemsDevExpressPrintModel.globalGrandTotalofLineTotal = rptmsg["globalGrandTotalofLineTotal"];
                /*
                POItemsDevExpressPrintModel.spnCreated = UtilityFunction.GetMessageFromResource("spnCreated", LocalizeResourceSetConstants.Global);
                POItemsDevExpressPrintModel.globalthrough = UtilityFunction.GetMessageFromResource("globalthrough", LocalizeResourceSetConstants.Global);
                POItemsDevExpressPrintModel.spnPurchaseOrderByAccount = UtilityFunction.GetMessageFromResource("spnPurchaseOrderByAccount", LocalizeResourceSetConstants.Global);
                POItemsDevExpressPrintModel.GlobalAccount = UtilityFunction.GetMessageFromResource("GlobalAccount", LocalizeResourceSetConstants.Global);
                POItemsDevExpressPrintModel.spnPurchaseOrder = UtilityFunction.GetMessageFromResource("spnPurchaseOrder", LocalizeResourceSetConstants.Global);
                POItemsDevExpressPrintModel.GlobalReason = UtilityFunction.GetMessageFromResource("GlobalReason", LocalizeResourceSetConstants.Global);
                POItemsDevExpressPrintModel.GlobalVendor = UtilityFunction.GetMessageFromResource("GlobalVendor", LocalizeResourceSetConstants.Global);
                POItemsDevExpressPrintModel.GlobalVendorName = UtilityFunction.GetMessageFromResource("GlobalVendorName", LocalizeResourceSetConstants.Global);
                POItemsDevExpressPrintModel.GlobalStatus = UtilityFunction.GetMessageFromResource("GlobalStatus", LocalizeResourceSetConstants.Global);
                POItemsDevExpressPrintModel.globalOrderDate = UtilityFunction.GetMessageFromResource("globalOrderDate", LocalizeResourceSetConstants.Global);
                POItemsDevExpressPrintModel.spnPoLineNo = UtilityFunction.GetMessageFromResource("spnPoLineNo", LocalizeResourceSetConstants.PurchaseOrder);
                POItemsDevExpressPrintModel.spnPartID = UtilityFunction.GetMessageFromResource("spnPartID", LocalizeResourceSetConstants.Global);
                POItemsDevExpressPrintModel.spnDescription = UtilityFunction.GetMessageFromResource("spnDescription", LocalizeResourceSetConstants.Global);
                POItemsDevExpressPrintModel.spnPoOrderQty = UtilityFunction.GetMessageFromResource("spnPoOrderQty", LocalizeResourceSetConstants.PurchaseOrder);
                POItemsDevExpressPrintModel.spnPdUOM = UtilityFunction.GetMessageFromResource("spnPdUOM", LocalizeResourceSetConstants.Global);
                POItemsDevExpressPrintModel.globalLineCost = UtilityFunction.GetMessageFromResource("globalLineCost", LocalizeResourceSetConstants.Global);
                POItemsDevExpressPrintModel.spnQtyRec = UtilityFunction.GetMessageFromResource("spnQtyRec", LocalizeResourceSetConstants.PurchaseOrder);
                POItemsDevExpressPrintModel.spnReceivedCost = UtilityFunction.GetMessageFromResource("spnReceivedCost", LocalizeResourceSetConstants.Global);
                POItemsDevExpressPrintModel.globalQtyRemaining = UtilityFunction.GetMessageFromResource("globalQtyRemaining", LocalizeResourceSetConstants.Global);
                POItemsDevExpressPrintModel.globalRemainingCost = UtilityFunction.GetMessageFromResource("globalRemainingCost", LocalizeResourceSetConstants.Global);
                POItemsDevExpressPrintModel.globalTotalCostForAccount = UtilityFunction.GetMessageFromResource("globalTotalCostForAccount", LocalizeResourceSetConstants.Global);
                POItemsDevExpressPrintModel.globalTotalRemaining = UtilityFunction.GetMessageFromResource("globalTotalRemaining", LocalizeResourceSetConstants.Global);
                POItemsDevExpressPrintModel.spnTotalReceived = UtilityFunction.GetMessageFromResource("spnTotalReceived", LocalizeResourceSetConstants.StoreroomTransfers);
                POItemsDevExpressPrintModel.globalGrandTotalofLineReceived = UtilityFunction.GetMessageFromResource("globalGrandTotalofLineReceived", LocalizeResourceSetConstants.Global);
                POItemsDevExpressPrintModel.globalGrandTotalofLineRemaining = UtilityFunction.GetMessageFromResource("globalGrandTotalofLineRemaining", LocalizeResourceSetConstants.Global);
                POItemsDevExpressPrintModel.globalGrandTotalofLineTotal = UtilityFunction.GetMessageFromResource("globalGrandTotalofLineTotal", LocalizeResourceSetConstants.Global);
                */
                #endregion
                POItemsDevExpressPrintModelList.Add(POItemsDevExpressPrintModel);
            }
            pOByAccoutntReportDevExpressPrintModel.POItemsDevExpressPrintModel = POItemsDevExpressPrintModelList;
            LocalizeControls(pOByAccoutntReportDevExpressPrintModel, LocalizeResourceSetConstants.PurchaseOrder);
            return View("POByAccountDevExpressPrint", pOByAccoutntReportDevExpressPrintModel);
        }
        #endregion
    }

}


