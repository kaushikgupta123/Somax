using Client.Common;
using Client.Models;
using Client.Models.BusinessIntelligence;
using System.Globalization;
using DataContracts;
using Common.Constants;
using INTDataLayer.BAL;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Linq;      // RKL 2021-Mar-19

namespace Client.BusinessWrapper.Reports
{
    public class ReportsWrapper
    {
        private readonly DatabaseKey _dbKey;
        private readonly UserData _userData;
        private readonly ReportBAL report;
        private readonly long callerUserInfoId;
        private readonly string callerUserName;
        private readonly long clientId;
        private readonly long siteId;
        private readonly string clientConnectionString;


        public ReportsWrapper(UserData userData)
        {
            _userData = userData;
            _dbKey = userData.DatabaseKey;
            report = new ReportBAL();
            callerUserInfoId = _dbKey.User.UserInfoId;
            callerUserName = _dbKey.UserName;
            clientId = _dbKey.Client.ClientId;
            siteId = _userData.Site.SiteId;
            clientConnectionString = _userData.DatabaseKey.ClientConnectionString;
        }

        #region Recent Reports
        public List<ReportListingModel> GetRecentReports()
        {
            List<ReportListingModel> reportListingList = new List<ReportListingModel>();
            ReportListingModel reportListingModel;
            ReportListing reportListing = new ReportListing();
            reportListing.SiteId = _userData.Site.SiteId;
            reportListing.EventLogResultCount = 5;
            reportListing.PersonnelId = _userData.DatabaseKey.Personnel.PersonnelId;

            var reports = reportListing.RetrieveRecentReports(_dbKey);
            foreach (var item in reports)
            {
                if (GetReportmenuSecurity(item.ReportGroup))
                {
                    reportListingModel = new ReportListingModel();
                    reportListingModel = ToReportListingModel(item);
                    reportListingList.Add(reportListingModel);
                }
            }
            return reportListingList;
        }
        #endregion

        #region SideMenu
        public List<string> GetReportMenu()
        {
            List<string> reportsList = new List<string>();
            ReportListing reportListing = new ReportListing();

            var reports = reportListing.RetrieveReportGroup(_dbKey);
            foreach (var item in reports)
            {
                if (GetReportmenuSecurity(item.ReportGroup))
                {
                    reportsList.Add(item.ReportGroup);
                }
            }
            return reportsList;
        }
        #endregion

        #region ReportList
        public List<ReportListingModel> GetReportList(string ReportGroup)
        {
            ReportListing reportListing = new ReportListing();

            UserReports userReportListing = new UserReports();
            List<ReportListingModel> reportListingList = new List<ReportListingModel>();
            ReportListingModel reportListingModel;
            reportListing.ReportGroup = ReportGroup;
            reportListing.PersonnelId = _dbKey.Personnel.PersonnelId;
            var reports = reportListing.RetrieveByGroup(_dbKey);
            foreach (var item in reports)
            {
                reportListingModel = new ReportListingModel();
                reportListingModel = ToReportListingModel(item);
                reportListingModel.IsFavorite = item.IsFavorite == 1 ? true : false;
                reportListingModel.ReportFavoritesId = item.ReportFavoritesId;
                reportListingModel.IsUserReport = false;
                reportListingModel.SaveType = string.Empty;
                reportListingList.Add(reportListingModel);
            }
            userReportListing.ClientId = _dbKey.Client.ClientId;
            userReportListing.SiteId = _userData.Site.SiteId; ;

            userReportListing.ReportGroup = ReportGroup;
            userReportListing.PersonnelId = _dbKey.Personnel.PersonnelId;
            var userReports = userReportListing.RetrieveByGroup(_dbKey);
            foreach (var item in userReports)
            {
                reportListingModel = new ReportListingModel();
                reportListingModel = ToUserReportListingModel(item);
                reportListingModel.IsFavorite = item.IsFavorite == 1 ? true : false;
                reportListingModel.ReportFavoritesId = item.ReportFavoritesId;
                reportListingModel.IsUserReport = true;
                reportListingModel.SaveType = item.SaveType;
                reportListingList.Add(reportListingModel);
            }
            return reportListingList;
        }
        #endregion

        #region Favorites
        public List<ReportListingModel> GetFavorites()
        {
            ReportFavorites reportFavorites = new ReportFavorites();
            List<ReportListingModel> reportListingList = new List<ReportListingModel>();
            ReportListingModel reportListingModel;

            reportFavorites.PersonnelId = _dbKey.Personnel.PersonnelId;
            var reports = reportFavorites.RetrieveMyFavorites(_dbKey);
            foreach (var item in reports)
            {
                if (GetReportmenuSecurity(item.ReportGroup))
                {
                    reportListingModel = new ReportListingModel();
                    reportListingModel.ReportListingId = item.ReportListingId;
                    reportListingModel.ReportName = GetReportName(item.ReportName);
                    reportListingModel.Description = GetReportDescription(item.Description);
                    reportListingModel.ReportGroup = item.ReportGroup;
                    reportListingModel.SourceName = item.SourceName;
                    reportListingModel.UseSP = item.UseSP;
                    reportListingModel.PrimarySortColumn = item.PrimarySortColumn;
                    reportListingModel.SecondarySortColumn = item.SecondarySortColumn;
                    reportListingModel.IsGrouped = item.IsGrouped;
                    reportListingModel.GroupColumn = item.GroupColumn;
                    reportListingModel.IncludePrompt = item.IncludePrompt;
                    reportListingModel.Prompt1Source = item.Prompt1Source;
                    reportListingModel.Prompt1Type = item.Prompt1Type;
                    reportListingModel.Prompt1List = item.Prompt1List;
                    reportListingModel.Prompt1ListSource = item.Prompt1ListSource;
                    reportListingModel.Prompt2Source = item.Prompt2Source;
                    reportListingModel.Prompt2Type = item.Prompt2Type;
                    reportListingModel.Prompt2List = item.Prompt2List;
                    reportListingModel.Prompt2ListSource = item.Prompt2ListSource;
                    reportListingModel.IsFavorite = true;
                    reportListingModel.ReportFavoritesId = item.ReportFavoritesId;
                    reportListingModel.IsUserReport = item.IsUserReports;
                    reportListingModel.SaveType = item.SaveType;
                    reportListingList.Add(reportListingModel);
                }
            }
            return reportListingList;
        }
        public List<string> CreateFavorite(long ReportListingId, long ReportFavoritesId, bool IsUserReport = false)
        {
            ReportFavorites reportFavorites = new ReportFavorites();
            if (ReportFavoritesId != 0)
            {
                reportFavorites.ReportFavoritesId = ReportFavoritesId;
                reportFavorites.IsUserReports = IsUserReport;
                reportFavorites.Del = false;
                reportFavorites.UpdateMyFavorites(_dbKey);
            }
            else
            {
                reportFavorites.ReportListingId = ReportListingId;
                reportFavorites.PersonnelId = _dbKey.Personnel.PersonnelId;
                reportFavorites.IsUserReports = IsUserReport;
                reportFavorites.Del = false;
                reportFavorites.Create(_dbKey);
            }
            return reportFavorites.ErrorMessages;
        }
        public List<string> DeleteFavorite(long ReportFavoritesId)
        {
            ReportFavorites reportFavorites = new ReportFavorites();
            reportFavorites.ReportFavoritesId = ReportFavoritesId;
            reportFavorites.Del = true;
            reportFavorites.UpdateMyFavorites(_dbKey);
            return reportFavorites.ErrorMessages;
        }
        #endregion

        #region ReportListingDetails
        public ReportListingModel GetReportListDetail(long ReportListingId)
        {
            ReportListingModel reportListingModel = new ReportListingModel();
            ReportListing reportListing = new ReportListing();
            reportListing.ReportListingId = ReportListingId;
            reportListing.Retrieve(_dbKey);
            if (reportListing != null)
            {
                reportListingModel = ToReportListingModel(reportListing);
            }
            return reportListingModel;
        }
        #endregion

        #region UserReportListingDetails
        public ReportListingModel GetUserReportListDetail(long ReportListingId)
        {
            ReportListingModel reportListingModel = new ReportListingModel();
            UserReports UserreportListing = new UserReports();
            UserreportListing.UserReportsId = ReportListingId;
            UserreportListing.Retrieve(_dbKey);
            if (UserreportListing != null)
            {
                reportListingModel = ToUserReportListingModel(UserreportListing);
            }
            return reportListingModel;
        }
        #endregion

        #region CreateReport
        public DataTable GetMasterGrid(long ReportListingId, bool IsUserReport, ref List<GridColumnsProp> gridColumnsProps, ref ReportListingModel reportListingModel,
           string MultiSelectData1 = "", string MultiSelectData2 = "", int caseNo1 = 0, int caseNo2 = 0, DateTime? StartDate1 = null, DateTime? EndDate1 = null,
           DateTime? StartDate2 = null, DateTime? EndDate2 = null)
        {
            DataTable finalDt = new DataTable();
            DataTable dtRpt = new DataTable();
            if (IsUserReport)
            {
                reportListingModel = GetUserReportListDetail(ReportListingId);
            }
            else
            {
                reportListingModel = GetReportListDetail(ReportListingId);
            }

            reportListingModel.IsUserReport = IsUserReport;

            #region Site Group column name //V2-474
            reportListingModel.ReportName = GetReportName(reportListingModel.ReportName);
            reportListingModel.Description = GetReportDescription(reportListingModel.Description);
            #endregion

            if (reportListingModel != null)
            {
                dtRpt = report.GetDataFromSource(ReportListingId, reportListingModel.UseSP, callerUserInfoId, callerUserName, clientId, siteId, clientConnectionString, reportListingModel.SourceName,
                    reportListingModel.IncludePrompt, reportListingModel.Prompt1Source, reportListingModel.Prompt2Source, reportListingModel.Prompt1Type, reportListingModel.Prompt2Type,
                    MultiSelectData1, MultiSelectData2, caseNo1, caseNo2, StartDate1, EndDate1, StartDate2, EndDate2, reportListingModel.IncludeChild, reportListingModel.ChildSourceName,
                    reportListingModel.MasterLinkColumn, reportListingModel.ChildLinkColumn, reportListingModel.IsEnterprise, IsUserReport, reportListingModel.BaseQuery);
                gridColumnsProps = GetReportGridColumnsForMaster(ReportListingId, reportListingModel);
                if (gridColumnsProps != null)
                {
                    finalDt = dtRpt.Copy();
                    if ((reportListingModel.IsGrouped && !string.IsNullOrEmpty(reportListingModel.GroupColumn)) || !string.IsNullOrEmpty(reportListingModel.PrimarySortColumn))
                    {
                        finalDt = SortAndGroup(finalDt, reportListingModel, gridColumnsProps);
                    }
                    #region V2-975
                    finalDt = LocalizeDate(finalDt, gridColumnsProps);
                    #endregion
                    finalDt = DateDisplay(finalDt, gridColumnsProps);
                }
            }
            return finalDt;
        }
        #region RKL-Mail Report Timeout
        public Tuple< DataTable,string> GetMasterGridReport(long ReportListingId, bool IsUserReport, ref List<GridColumnsProp> gridColumnsProps, ref ReportListingModel reportListingModel,
            string MultiSelectData1 = "", string MultiSelectData2 = "", int caseNo1 = 0, int caseNo2 = 0, DateTime? StartDate1 = null, DateTime? EndDate1 = null,
            DateTime? StartDate2 = null, DateTime? EndDate2 = null)
        {
            DataTable finalDt = new DataTable();
            DataTable dtRpt = new DataTable();
            if (IsUserReport)
            {
                reportListingModel = GetUserReportListDetail(ReportListingId);
            }
            else
            {
                reportListingModel = GetReportListDetail(ReportListingId);
            }

            reportListingModel.IsUserReport = IsUserReport;

            #region Site Group column name //V2-474
            reportListingModel.ReportName = GetReportName(reportListingModel.ReportName);
            reportListingModel.Description = GetReportDescription(reportListingModel.Description);
            #endregion
            string timeoutError = string.Empty;
            if (reportListingModel != null)
            {
                dtRpt = report.GetDataFromSourceReport(ReportListingId, reportListingModel.UseSP, callerUserInfoId, callerUserName, clientId, siteId, clientConnectionString, reportListingModel.SourceName,
                    reportListingModel.IncludePrompt, reportListingModel.Prompt1Source, reportListingModel.Prompt2Source, reportListingModel.Prompt1Type, reportListingModel.Prompt2Type,
                    MultiSelectData1, MultiSelectData2, caseNo1, caseNo2, StartDate1, EndDate1, StartDate2, EndDate2, reportListingModel.IncludeChild, reportListingModel.ChildSourceName,
                    reportListingModel.MasterLinkColumn, reportListingModel.ChildLinkColumn, reportListingModel.IsEnterprise, IsUserReport, reportListingModel.BaseQuery,ref timeoutError);
                gridColumnsProps = GetReportGridColumnsForMaster(ReportListingId, reportListingModel);
                if (gridColumnsProps != null && string.IsNullOrEmpty(timeoutError))
                {
                    finalDt = dtRpt.Copy();
                    if ((reportListingModel.IsGrouped && !string.IsNullOrEmpty(reportListingModel.GroupColumn)) || !string.IsNullOrEmpty(reportListingModel.PrimarySortColumn))
                    {
                        finalDt = SortAndGroup(finalDt, reportListingModel, gridColumnsProps);
                    }
                    #region V2-975
                    finalDt = LocalizeDate(finalDt, gridColumnsProps);
                    #endregion
                    finalDt = DateDisplay(finalDt, gridColumnsProps);
                }
            }
            return Tuple.Create(finalDt,timeoutError);
        }
        #endregion
        public DataTable GetChildGrid(long ReportListingId, ref List<GridColumnsProp> gridColumnsProps, ref ReportListingModel reportListingModel, long MasterLinkValue, bool IsUserReport = false)
        {
            DataTable dtRpt = new DataTable();
            DataTable finalDt = new DataTable();

            if (reportListingModel != null)
            {
                dtRpt = report.GetChildGridData(callerUserInfoId, callerUserName, clientId, siteId, clientConnectionString, reportListingModel.ChildSourceName,
                                MasterLinkValue, reportListingModel.ChildLinkColumn);

                if (gridColumnsProps.Count == 0)
                {
                    gridColumnsProps = GetReportGridColumnsForChild(ReportListingId, reportListingModel, IsUserReport);
                }

                if (gridColumnsProps != null)
                {
                    finalDt = GetNewTable(dtRpt, gridColumnsProps);
                    #region V2-975
                    finalDt = LocalizeDate(finalDt, gridColumnsProps);
                    #endregion
                    finalDt = DateDisplay(finalDt, gridColumnsProps);
                }
            }

            return finalDt;
        }
        public string DataTableToJSONWithJSONNet(DataTable table)
        {
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(table);
            return JSONString;
        }
        public List<GridColumnsProp> GetReportGridColumnsForMaster(long ReportListingId, ReportListingModel reportListingModel)
        {
            List<GridColumnsProp> gridColumnsProps = new List<GridColumnsProp>();

            UserReportGridDefintion userreportGridDefintion = new UserReportGridDefintion();
            userreportGridDefintion.ReportId = ReportListingId;
            userreportGridDefintion.IsChildColumn = false;
            // var definition = reportGridDefintion.RetrieveByReportListingId(_dbKey);
            // var definition1 = userreportGridDefintion.RetrieveByReportId(_dbKey);
            // var definition = "";

            if (reportListingModel.IncludeChild && !string.IsNullOrEmpty(reportListingModel.ChildSourceName))
            {
                IncludeCountColumnForChild(reportListingModel, gridColumnsProps);
                IncludeIconColumnForChild(reportListingModel, gridColumnsProps);
            }
            if (reportListingModel.IsUserReport)
            {
                var definitionUReport = userreportGridDefintion.RetrieveByReportId(_dbKey);
                IncludeURCommonColumn(reportListingModel, gridColumnsProps, definitionUReport);
            }
            else
            {
                ReportGridDefintion reportGridDefintion = new ReportGridDefintion();
                reportGridDefintion.ReportListingId = ReportListingId;
                reportGridDefintion.IsChildColumn = false;
                var definitionReport = reportGridDefintion.RetrieveByReportListingId(_dbKey);
                IncludeCommonColumn(reportListingModel, gridColumnsProps, definitionReport);

            }
            if (reportListingModel.IsGrouped && !string.IsNullOrEmpty(reportListingModel.GroupColumn))
            {
                IncludeGroupedColumn(reportListingModel, gridColumnsProps);
            }
            return gridColumnsProps;
        }

        public List<GridColumnsProp> GetReportGridColumnsForChild(long ReportListingId, ReportListingModel reportListingModel, bool IsUserReport = false)
        {
            List<GridColumnsProp> gridColumnsProps = new List<GridColumnsProp>();

            if (IsUserReport)
            {
                UserReportGridDefintion userreportGridDefintion = new UserReportGridDefintion();
                userreportGridDefintion.ReportId = ReportListingId;
                userreportGridDefintion.IsChildColumn = true;
                var definitionUReport = userreportGridDefintion.RetrieveByReportId(_dbKey);
                IncludeURCommonColumn(reportListingModel, gridColumnsProps, definitionUReport);
            }
            else
            {
                ReportGridDefintion reportGridDefintion = new ReportGridDefintion();
                reportGridDefintion.ReportListingId = ReportListingId;
                reportGridDefintion.IsChildColumn = true;
                var definition = reportGridDefintion.RetrieveByReportListingId(_dbKey);
                IncludeCommonColumn(reportListingModel, gridColumnsProps, definition);
            }

            return gridColumnsProps;
        }
        public List<DropDownModel> GetMultiSelectValuesForConstant(long ReportListingId, string ConstantType)
        {
            List<DropDownModel> values = new List<DropDownModel>();
            DropDownModel multiSelectModel;

            DataConstant dataConstant = new DataConstant();
            dataConstant.ConstantType = ConstantType;
            // The localization value should only be the language and the culture
            dataConstant.LocaleId = _dbKey.Client.LocalizationLanguageAndCulture;
            //dataConstant.LocaleId = _dbKey.Localization;

            var dtValues = dataConstant.RetrieveLocaleForConstantType_V2(_dbKey);
            foreach (var item in dtValues)
            {
                multiSelectModel = new DropDownModel();
                multiSelectModel.value = item.Value;
                multiSelectModel.text = item.LocalName;
                values.Add(multiSelectModel);
            }
            return values;
        }
        public List<DropDownModel> GetMultiSelectValuesForLookUp(long ReportListingId, string ListName)
        {
            List<DropDownModel> values = new List<DropDownModel>();
            DropDownModel multiSelectModel;

            DataContracts.LookupList lookupList = new DataContracts.LookupList();
            lookupList.ListName = ListName;
            var dtValues = lookupList.GetLookUpListByListName(_dbKey);

            foreach (var item in dtValues)
            {
                multiSelectModel = new DropDownModel();
                multiSelectModel.value = item.ListValue;
                multiSelectModel.text = item.Description;
                values.Add(multiSelectModel);
            }
            return values;
        }
        #endregion

        #region EventLog
        public void CreateReportEventLog(long ReportListingId, bool IsUserReports, string Event = "")
        {
            ReportEventLog reportEventLog = new ReportEventLog();
            reportEventLog.ClientId = _dbKey.Client.ClientId;
            reportEventLog.SiteId = _userData.Site.SiteId;
            reportEventLog.ReportListingId = ReportListingId;
            reportEventLog.TransactionDate = DateTime.UtcNow;
            reportEventLog.Event = Event;
            reportEventLog.PersonnelId = _dbKey.Personnel.PersonnelId;
            reportEventLog.IsUserReports = IsUserReports;

            reportEventLog.Create(_dbKey);
        }
        #endregion

        #region Private Method
        private DataTable GetNewTable(DataTable OldTable, List<GridColumnsProp> gridColumnsProps)
        {
            DataTable dt = new DataTable();
            dt.Clear();
            foreach (var item in gridColumnsProps)
            {
                dt.Columns.Add(item.data);
            }
            foreach (DataRow row in OldTable.Rows)
            {
                DataRow dr = dt.NewRow();
                foreach (var item in gridColumnsProps)
                {
                    dr[item.data] = row[item.data].ToString();
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
        private ReportListingModel ToReportListingModel(ReportListing reportListing)
        {
            ReportListingModel reportListingModel = new ReportListingModel();
            if (reportListing != null)
            {
                reportListingModel.ReportListingId = reportListing.ReportListingId;
                #region Site Group column name //V2-474
                reportListingModel.ReportName = GetReportName(reportListing.ReportName);
                reportListingModel.Description = GetReportDescription(reportListing.Description);
                #endregion


                reportListingModel.ReportGroup = reportListing.ReportGroup;
                reportListingModel.SourceName = reportListing.SourceName;
                reportListingModel.UseSP = reportListing.UseSP;
                reportListingModel.PrimarySortColumn = reportListing.PrimarySortColumn;
                reportListingModel.SecondarySortColumn = reportListing.SecondarySortColumn;
                reportListingModel.IsGrouped = reportListing.IsGrouped;
                reportListingModel.GroupColumn = reportListing.GroupColumn;
                reportListingModel.IncludePrompt = reportListing.IncludePrompt;

                reportListingModel.Prompt1Source = reportListing.Prompt1Source;
                reportListingModel.Prompt1Type = reportListing.Prompt1Type;
                reportListingModel.Prompt1ListSource = reportListing.Prompt1ListSource;
                reportListingModel.Prompt1List = reportListing.Prompt1List;

                reportListingModel.Prompt2Source = reportListing.Prompt2Source;
                reportListingModel.Prompt2Type = reportListing.Prompt2Type;
                reportListingModel.Prompt2ListSource = reportListing.Prompt2ListSource;
                reportListingModel.Prompt2List = reportListing.Prompt2List;

                reportListingModel.ChildSourceName = reportListing.ChildSourceName;
                reportListingModel.MasterLinkColumn = reportListing.MasterLinkColumn;
                reportListingModel.ChildLinkColumn = reportListing.ChildLinkColumn;
                reportListingModel.IncludeChild = reportListing.IncludeChild;
                reportListingModel.IsUserReport = reportListing.IsUserReport;
                reportListingModel.IsEnterprise = reportListing.IsEnterprise;
                reportListingModel.BaseQuery = reportListing.BaseQuery;
                reportListingModel.NoCSV = reportListing.NoCSV;
                reportListingModel.NoExcel = reportListing.NoExcel;
                reportListingModel.DevExpressRpt = reportListing.DevExpressRpt;
                reportListingModel.DevExpressRptName = reportListing.DevExpressRptName;
            }
            return reportListingModel;
        }
        private ReportListingModel ToUserReportListingModel(UserReports userReportsListing)
        {
            ReportListingModel reportListingModel = new ReportListingModel();
            if (userReportsListing != null)
            {
                reportListingModel.ReportListingId = userReportsListing.UserReportsId;
                reportListingModel.ReportName = GetReportName(userReportsListing.ReportName);
                reportListingModel.Description = GetReportDescription(userReportsListing.Description);
                reportListingModel.ReportGroup = userReportsListing.ReportGroup;
                reportListingModel.SourceName = userReportsListing.SourceName;
                reportListingModel.UseSP = userReportsListing.UseSP;
                reportListingModel.PrimarySortColumn = userReportsListing.PrimarySortColumn;
                reportListingModel.SecondarySortColumn = userReportsListing.SecondarySortColumn;
                reportListingModel.IsGrouped = userReportsListing.IsGrouped;
                reportListingModel.GroupColumn = userReportsListing.GroupColumn;
                reportListingModel.IncludePrompt = userReportsListing.IncludePrompt;

                reportListingModel.Prompt1Source = userReportsListing.Prompt1Source;
                reportListingModel.Prompt1Type = userReportsListing.Prompt1Type;
                reportListingModel.Prompt1ListSource = userReportsListing.Prompt1ListSource;
                reportListingModel.Prompt1List = userReportsListing.Prompt1List;

                reportListingModel.Prompt2Source = userReportsListing.Prompt2Source;
                reportListingModel.Prompt2Type = userReportsListing.Prompt2Type;
                reportListingModel.Prompt2ListSource = userReportsListing.Prompt2ListSource;
                reportListingModel.Prompt2List = userReportsListing.Prompt2List;

                reportListingModel.ChildSourceName = userReportsListing.ChildSourceName;
                reportListingModel.MasterLinkColumn = userReportsListing.MasterLinkColumn;
                reportListingModel.ChildLinkColumn = userReportsListing.ChildLinkColumn;
                reportListingModel.IncludeChild = userReportsListing.IncludeChild;
                reportListingModel.IsEnterprise = userReportsListing.IsEnterprise;
                reportListingModel.BaseQuery = userReportsListing.BaseQuery;
            }
            return reportListingModel;
        }
        private bool GetReportmenuSecurity(string MenuName)
        {
            bool status = false;
            ReportListing reportListing = new ReportListing();
            // RKL - Use Access NOT ReportItem
            if (MenuName == _userData.Security.Asset_Reports.ItemName)
            {
                status = _userData.Security.Asset_Reports.Access;
            }
            else if (MenuName == _userData.Security.Work_Order_Reports.ItemName)
            {
                status = _userData.Security.Work_Order_Reports.Access;
            }
            else if (MenuName == _userData.Security.Preventive_Maintenance_Reports.ItemName)
            {
                status = _userData.Security.Preventive_Maintenance_Reports.Access;
            }
            else if (MenuName == _userData.Security.Inventory_Reports.ItemName)
            {
                status = _userData.Security.Inventory_Reports.Access;
            }
            else if (MenuName == _userData.Security.Purchasing_Reports.ItemName)
            {
                status = _userData.Security.Purchasing_Reports.Access;
            }
            else if (MenuName == _userData.Security.Sanitation_Reports.ItemName)
            {
                status = _userData.Security.Sanitation_Reports.Access;
            }
            else if (MenuName == _userData.Security.APM_Reports.ItemName)
            {
                status = _userData.Security.APM_Reports.Access;    
            }
            else if (MenuName == _userData.Security.Configuration_Reports.ItemName)
            {
                status = _userData.Security.Configuration_Reports.Access;
            }
            else if (MenuName == _userData.Security.Enterprise_CMMS.ItemName)
            {
                status = _userData.Security.Enterprise_CMMS.Access;
            }
            else if (MenuName == _userData.Security.Reports_Fleet.ItemName)
            {
              status = _userData.Security.Reports_Fleet.Access;
            }
            else if (MenuName == _userData.Security.Enterprise_Sanitation.ItemName)
            {
              status = _userData.Security.Enterprise_Sanitation.Access;
            }
            else if (MenuName == _userData.Security.Enterprise_APM.ItemName)
            {
              status = _userData.Security.Enterprise_APM.Access;
            }
            else if (MenuName == _userData.Security.Enterprise_Fleet.ItemName)
            {
              status = _userData.Security.Enterprise_Fleet.Access;
            }
            /*
            if (MenuName == _userData.Security.Asset_Reports.ItemName)
            {
                status = _userData.Security.Asset_Reports.ReportItem;
            }
            else if (MenuName == _userData.Security.Work_Order_Reports.ItemName)
            {
                status = _userData.Security.Work_Order_Reports.ReportItem;
            }
            else if (MenuName == _userData.Security.Preventive_Maintenance_Reports.ItemName)
            {
                status = _userData.Security.Preventive_Maintenance_Reports.ReportItem;
            }
            else if (MenuName == _userData.Security.Inventory_Reports.ItemName)
            {
                status = _userData.Security.Inventory_Reports.ReportItem;
            }
            else if (MenuName == _userData.Security.Purchasing_Reports.ItemName)
            {
                status = _userData.Security.Purchasing_Reports.ReportItem;
            }
            // Anadi
            //else if (MenuName == _userData.Security.PurchaseRequest_Reports.ItemName)
            //{
            //    status = _userData.Security.PurchaseRequest_Reports.ReportItem;
            //}
            else if (MenuName == _userData.Security.Sanitation_Reports.ItemName)
            {
                status = _userData.Security.Sanitation_Reports.ReportItem;
            }
            else if (MenuName == _userData.Security.APM_Reports.ItemName)
            {
                status = _userData.Security.APM_Reports.ReportItem;    
            }
            else if (MenuName == _userData.Security.Configuration_Reports.ItemName)
            {
                status = _userData.Security.Configuration_Reports.ReportItem;
            }
            else if (MenuName == _userData.Security.Enterprise_CMMS.ItemName)
            {
                status = _userData.Security.Enterprise_CMMS.ReportItem;
            }
            else if (MenuName == _userData.Security.Reports_Fleet.ItemName)
            {
              status = _userData.Security.Reports_Fleet.ReportItem;
            }
            else if (MenuName == _userData.Security.Enterprise_Sanitation.ItemName)
            {
              status = _userData.Security.Enterprise_Sanitation.ReportItem;
            }
            else if (MenuName == _userData.Security.Enterprise_APM.ItemName)
            {
              status = _userData.Security.Enterprise_APM.ReportItem;
            }
            else if (MenuName == _userData.Security.Enterprise_Fleet.ItemName)
            {
              status = _userData.Security.Enterprise_Fleet.ReportItem;
            }
            */
            return status;

        }
        private DataTable DateDisplay(DataTable dtRpt, List<GridColumnsProp> gridColumnsProps)
        {
            if (dtRpt != null && gridColumnsProps != null)
            {
                List<string> dateColumns = gridColumnsProps.Where(x => x.DateDisplay == true).Select(x => x.data).ToList();
                if (dateColumns.Count > 0)
                {
                    for (int rows = 0; rows < dtRpt.Rows.Count; rows++)
                    {
                        for (int cols = 0; cols < dateColumns.Count; cols++)
                        {
                            var colName = dateColumns[cols];
                            var val = dtRpt.Rows[rows][colName].ToString();
                            if (!string.IsNullOrEmpty(val.ToString()))
                            {
                                try
                                {
                                    DateTime dt = Convert.ToDateTime(val);
                                    dtRpt.Rows[rows].SetField(colName, dt.Date.ToString().Split(' ')[0]);
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }
                    }
                }
            }
            return dtRpt;
        }
        private DataTable SortAndGroup(DataTable dtRpt, ReportListingModel reportListingModel, List<GridColumnsProp> gridColumnsProps)
        {
            DataTable finalDt = new DataTable();
            string sortColumn = string.Empty;

            if (reportListingModel.IsGrouped && !string.IsNullOrWhiteSpace(reportListingModel.GroupColumn))
            {
                sortColumn = reportListingModel.GroupColumn.Trim();
            }
            if (!string.IsNullOrWhiteSpace(reportListingModel.PrimarySortColumn))
            {
                if (reportListingModel.PrimarySortColumn != reportListingModel.GroupColumn)
                {
                    sortColumn = !string.IsNullOrWhiteSpace(sortColumn) ? sortColumn + "," + reportListingModel.PrimarySortColumn.Trim() : reportListingModel.PrimarySortColumn.Trim();
                }
                if (!string.IsNullOrWhiteSpace(reportListingModel.SecondarySortColumn) && reportListingModel.PrimarySortColumn != reportListingModel.SecondarySortColumn
                    && reportListingModel.SecondarySortColumn != reportListingModel.GroupColumn)
                {
                    sortColumn = sortColumn + "," + reportListingModel.SecondarySortColumn.Trim();
                }
            }
            if (dtRpt != null && gridColumnsProps != null)
            {
                if (!string.IsNullOrEmpty(sortColumn))
                {
                    dtRpt.DefaultView.Sort = sortColumn;
                    finalDt = dtRpt.DefaultView.ToTable();
                }
                else
                {
                    finalDt = dtRpt;
                }
                finalDt = GetNewTable(finalDt, gridColumnsProps);
            }
            return finalDt;
        }
        private void IncludeCountColumnForChild(ReportListingModel reportListingModel, List<GridColumnsProp> gridColumnsProps)
        {
            var gridColumnsProp = new GridColumnsProp();
            gridColumnsProp.title = string.Empty;
            gridColumnsProp.data = ReportConstants.CHILDCOUNT;
            gridColumnsProp.className = ReportConstants.TEXTLEFT + " noVis";
            gridColumnsProp.IsGroupTotaled = false;
            gridColumnsProp.IsGrandTotal = false;
            gridColumnsProp.bSortable = false;
            gridColumnsProp.orderable = false;
            gridColumnsProp.bVisible = false;
            gridColumnsProps.Add(gridColumnsProp);
        }
        private void IncludeIconColumnForChild(ReportListingModel reportListingModel, List<GridColumnsProp> gridColumnsProps)
        {
            var gridColumnsProp = new GridColumnsProp();
            gridColumnsProp.title = "";
            gridColumnsProp.data = reportListingModel.MasterLinkColumn;
            gridColumnsProp.IsGroupTotaled = false;
            gridColumnsProp.IsGrandTotal = false;
            gridColumnsProp.bSortable = false;
            gridColumnsProp.orderable = false;
            gridColumnsProp.bVisible = true;
            gridColumnsProp.className = "noVis";
            gridColumnsProps.Add(gridColumnsProp);
        }
        private void IncludeCommonColumn(ReportListingModel reportListingModel, List<GridColumnsProp> gridColumnsProps, List<ReportGridDefintion> definition)
        {
            foreach (var item in definition)
            {
                var gridColumnsProp = new GridColumnsProp();
                gridColumnsProp.title = GetReportHeader(item.Heading);
                gridColumnsProp.data = item.Columns;
                if (!string.IsNullOrEmpty(item.Alignment))
                {
                    if (item.Alignment.ToUpper() == ReportConstants.LEFTALIGNMENT)
                    {
                        gridColumnsProp.className = ReportConstants.TEXTLEFT;
                    }
                    else if (item.Alignment.ToUpper() == ReportConstants.RIGHTLIGNMENT)
                    {
                        gridColumnsProp.className = ReportConstants.TEXTRIGHT;
                    }
                    else if (item.Alignment.ToUpper() == ReportConstants.CENTERALIGNMENT)
                    {
                        gridColumnsProp.className = ReportConstants.TEXTCENTER;
                    }
                    else
                    {
                        gridColumnsProp.className = ReportConstants.TEXTLEFT;
                    }

                    if (item.Required)
                    {
                        gridColumnsProp.bRequired = true;
                        if (item.Columns == reportListingModel.GroupColumn)
                        {
                            gridColumnsProp.className = gridColumnsProp.className + " noVis";
                            gridColumnsProp.className = gridColumnsProp.className + " grp-column";
                        }

                    }
                    else
                    {
                        gridColumnsProp.className = gridColumnsProp.className + " grp-column";
                    }
                }
                else
                {
                    if (item.Required)
                    {
                        gridColumnsProp.bRequired = true;
                        if (item.Columns == reportListingModel.GroupColumn)
                        {
                            gridColumnsProp.className = "noVis";
                            gridColumnsProp.className = gridColumnsProp.className + " grp-column";
                        }
                    }
                    else
                    {
                        gridColumnsProp.className = "grp-column";
                    }
                }
                gridColumnsProp.IsGroupTotaled = item.IsGroupTotaled;
                gridColumnsProp.IsGrandTotal = item.IsGrandTotal;
                gridColumnsProp.bSortable = false;
                gridColumnsProp.orderable = false;
                if (item.Display)
                {
                    gridColumnsProp.bVisible = true;
                }
                gridColumnsProp.NumofDecPlaces = item.NumofDecPlaces;
                gridColumnsProp.NumericFormat = item.NumericFormat;
                //gridColumnsProp.SiteLocalization = _userData.Site.Localization;
                gridColumnsProp.SiteLocalization = _userData.DatabaseKey.Client.LocalizationLanguageAndCulture;
                if (!string.IsNullOrEmpty(gridColumnsProp.SiteLocalization))
                {
                    if (gridColumnsProp.SiteLocalization.ToUpper() == ReportConstants.ENUS)
                    {
                        gridColumnsProp.CurrencyCode = ReportConstants.USD;
                    }
                    if (gridColumnsProp.SiteLocalization.ToUpper() == ReportConstants.FRFR)
                    {
                        gridColumnsProp.CurrencyCode = ReportConstants.EUR;
                    }
                }
                gridColumnsProp.AvailableOnFilter = item.AvailableonFilter;
                gridColumnsProp.FilterMethod = !string.IsNullOrEmpty(item.FilterMethod) ? item.FilterMethod : "string";
                gridColumnsProp.Sequence = item.Sequence;
                gridColumnsProp.DateDisplay = item.DateDisplay;
                #region V2-975
                gridColumnsProp.LocalizeDate = item.LocalizeDate;
                #endregion
                gridColumnsProps.Add(gridColumnsProp);
            }
        }
        private void IncludeURCommonColumn(ReportListingModel reportListingModel, List<GridColumnsProp> gridColumnsProps, List<UserReportGridDefintion> definition)
        {
            foreach (var item in definition)
            {
                var gridColumnsProp = new GridColumnsProp();
                gridColumnsProp.title = GetReportHeader(item.Heading);
                gridColumnsProp.data = item.Columns;
                if (!string.IsNullOrEmpty(item.Alignment))
                {
                    if (item.Alignment.ToUpper() == ReportConstants.LEFTALIGNMENT)
                    {
                        gridColumnsProp.className = ReportConstants.TEXTLEFT;
                    }
                    else if (item.Alignment.ToUpper() == ReportConstants.RIGHTLIGNMENT)
                    {
                        gridColumnsProp.className = ReportConstants.TEXTRIGHT;
                    }
                    else if (item.Alignment.ToUpper() == ReportConstants.CENTERALIGNMENT)
                    {
                        gridColumnsProp.className = ReportConstants.TEXTCENTER;
                    }
                    else
                    {
                        gridColumnsProp.className = ReportConstants.TEXTLEFT;
                    }

                    if (item.Required)
                    {
                        gridColumnsProp.bRequired = true;
                        gridColumnsProp.className = gridColumnsProp.className + " noVis1";
                    }
                }
                else
                {
                    if (item.Required)
                    {
                        gridColumnsProp.bRequired = true;
                        gridColumnsProp.className = "noVis1";
                    }
                }
                gridColumnsProp.IsGroupTotaled = item.IsGroupTotaled;
                gridColumnsProp.IsGrandTotal = item.IsGrandTotal;
                gridColumnsProp.bSortable = false;
                gridColumnsProp.orderable = false;
                if (item.Display)
                {
                    gridColumnsProp.bVisible = true;
                }
                gridColumnsProp.NumofDecPlaces = 0;
                gridColumnsProp.NumericFormat = item.NumericFormat ?? "";
                gridColumnsProp.SiteLocalization = _userData.Site.Localization;
                if (!string.IsNullOrEmpty(gridColumnsProp.SiteLocalization))
                {
                    if (gridColumnsProp.SiteLocalization.ToUpper() == ReportConstants.ENUS)
                    {
                        gridColumnsProp.CurrencyCode = ReportConstants.USD;
                    }
                    if (gridColumnsProp.SiteLocalization.ToUpper() == ReportConstants.FRFR)
                    {
                        gridColumnsProp.CurrencyCode = ReportConstants.EUR;
                    }
                }
                gridColumnsProp.AvailableOnFilter = item.AvailableonFilter;
                gridColumnsProp.FilterMethod = !string.IsNullOrEmpty(item.FilterMethod) ? item.FilterMethod : ReportConstants.FMSTRING.ToLower();
                gridColumnsProp.Sequence = item.Sequence;
                gridColumnsProp.Filter = item.Filter;
                gridColumnsProp.NumofDecPlaces = item.NumofDecPlaces;
                gridColumnsProp.NumericFormat = item.NumericFormat;
                gridColumnsProp.FilterMethod = item.FilterMethod;
                gridColumnsProp.DateDisplay = item.DateDisplay;
                #region V2-975
                gridColumnsProp.LocalizeDate = item.LocalizeDate;
                #endregion
                gridColumnsProps.Add(gridColumnsProp);
            }
        }
        private void IncludeGroupedColumn(ReportListingModel reportListingModel, List<GridColumnsProp> gridColumnsProps)
        {
            // RKL - 2021-Mar-19 - Check with Indus Net
            // Do not add the column if it already exists
            // 
            if (!gridColumnsProps.Any(gp => gp.data.ToLower() == reportListingModel.GroupColumn.ToLower()))
            {
                var gridColumnsProp = new GridColumnsProp();
                gridColumnsProp.title = reportListingModel.GroupColumn;
                gridColumnsProp.data = reportListingModel.GroupColumn;
                gridColumnsProp.className = "noVis";
                gridColumnsProps.Add(gridColumnsProp);
            }
        }
        private string GetSiteGroupColumn(string text)
        {
            string siteData = string.Empty;
            string textInLower = !string.IsNullOrEmpty(text) ? text.ToLower() : string.Empty;
            if (textInLower.Contains(ReportConstants.GROUP1NAME))
            {
                var regex = new Regex(ReportConstants.GROUP1NAME, RegexOptions.IgnoreCase);
                return regex.Replace(text, _userData.Site.AssetGroup1Name);
            }
            else if (textInLower.Contains(ReportConstants.GROUP2NAME))
            {
                var regex = new Regex(ReportConstants.GROUP2NAME, RegexOptions.IgnoreCase);
                return regex.Replace(text, _userData.Site.AssetGroup2Name);
            }
            else if (textInLower.Contains(ReportConstants.GROUP3NAME))
            {
                var regex = new Regex(ReportConstants.GROUP3NAME, RegexOptions.IgnoreCase);
                return regex.Replace(text, _userData.Site.AssetGroup3Name);
            }
            return text;
        }
        private string GetReportName(string name)
        {
            string rptName = string.Empty;
            string nameInLower = !string.IsNullOrEmpty(name) ? name.ToLower() : string.Empty;
            if (!string.IsNullOrEmpty(name) && (nameInLower.Contains(ReportConstants.GROUP1NAME) || nameInLower.Contains(ReportConstants.GROUP2NAME) || nameInLower.Contains(ReportConstants.GROUP3NAME)))
            {
                rptName = GetSiteGroupColumn(name);
            }
            else
            {
                rptName = name;
            }
            return rptName;
        }
        private string GetReportDescription(string description)
        {
            string rptDescription = string.Empty;
            string descriptionInLower = !string.IsNullOrEmpty(description) ? description.ToLower() : string.Empty;
            if (!string.IsNullOrEmpty(description) && (descriptionInLower.Contains(ReportConstants.GROUP1NAME) || descriptionInLower.Contains(ReportConstants.GROUP2NAME) || descriptionInLower.Contains(ReportConstants.GROUP3NAME)))
            {
                rptDescription = GetSiteGroupColumn(description);
            }
            else
            {
                rptDescription = description;
            }
            return rptDescription;
        }
        private string GetReportHeader(string header)
        {
            string rptHeader = string.Empty;
            string headerInLower = !string.IsNullOrEmpty(header) ? header.ToLower() : string.Empty;
            if (!string.IsNullOrEmpty(header) && (headerInLower.Contains(ReportConstants.GROUP1NAME) || headerInLower.Contains(ReportConstants.GROUP2NAME) || headerInLower.Contains(ReportConstants.GROUP3NAME)))
            {
                rptHeader = GetSiteGroupColumn(header);
            }
            else
            {
                rptHeader = header;
            }
            return rptHeader;
        }
        #endregion

        #region Add-Edit-Delete Report
        public UserReportsModel initializeAddUserReportFromUserReportControls(UserReports obj)
        {
            UserReportsModel objUserReportsModel = new UserReportsModel();
            objUserReportsModel.ReportGroup = obj.ReportGroup;
            objUserReportsModel.SourceName = obj.SourceName;
            objUserReportsModel.UseSP = obj.UseSP;
            objUserReportsModel.PrimarySortColumn = obj.PrimarySortColumn;
            objUserReportsModel.SecondarySortColumn = obj.SecondarySortColumn;
            objUserReportsModel.IsGrouped = obj.IsGrouped;
            objUserReportsModel.GroupColumn = obj.GroupColumn;
            objUserReportsModel.IncludePrompt = obj.IncludePrompt;
            objUserReportsModel.Prompt1Source = obj.Prompt1Source;
            objUserReportsModel.Prompt1Type = obj.Prompt1Type;
            objUserReportsModel.Prompt1ListSource = obj.Prompt1ListSource;
            objUserReportsModel.Prompt1List = obj.Prompt1List;
            objUserReportsModel.Prompt2Source = obj.Prompt2Source;
            objUserReportsModel.Prompt2Type = obj.Prompt2Type;
            objUserReportsModel.Prompt2ListSource = obj.Prompt2ListSource;
            objUserReportsModel.Prompt2List = obj.Prompt2List;
            objUserReportsModel.ChildSourceName = obj.ChildSourceName;
            objUserReportsModel.MasterLinkColumn = obj.MasterLinkColumn;
            objUserReportsModel.ChildLinkColumn = obj.ChildLinkColumn;
            objUserReportsModel.IncludeChild = obj.IncludeChild;
            objUserReportsModel.IsEnterprise = obj.IsEnterprise;
            objUserReportsModel.BaseQuery = obj.BaseQuery;
            return objUserReportsModel;
        }
        public UserReportsModel initializeAddUserReportFromReportListingControls(ReportListing obj)
        {
            UserReportsModel objUserReportsModel = new UserReportsModel();
            objUserReportsModel.ReportGroup = obj.ReportGroup;
            objUserReportsModel.SourceName = obj.SourceName;
            objUserReportsModel.UseSP = obj.UseSP;
            objUserReportsModel.PrimarySortColumn = obj.PrimarySortColumn;
            objUserReportsModel.SecondarySortColumn = obj.SecondarySortColumn;
            objUserReportsModel.IsGrouped = obj.IsGrouped;
            objUserReportsModel.GroupColumn = obj.GroupColumn;
            objUserReportsModel.IncludePrompt = obj.IncludePrompt;
            objUserReportsModel.Prompt1Source = obj.Prompt1Source;
            objUserReportsModel.Prompt1Type = obj.Prompt1Type;
            objUserReportsModel.Prompt1ListSource = obj.Prompt1ListSource;
            objUserReportsModel.Prompt1List = obj.Prompt1List;
            objUserReportsModel.Prompt2Source = obj.Prompt2Source;
            objUserReportsModel.Prompt2Type = obj.Prompt2Type;
            objUserReportsModel.Prompt2ListSource = obj.Prompt2ListSource;
            objUserReportsModel.Prompt2List = obj.Prompt2List;
            objUserReportsModel.ChildSourceName = obj.ChildSourceName;
            objUserReportsModel.MasterLinkColumn = obj.MasterLinkColumn;
            objUserReportsModel.ChildLinkColumn = obj.ChildLinkColumn;
            objUserReportsModel.IncludeChild = obj.IncludeChild;
            objUserReportsModel.IsEnterprise = obj.IsEnterprise;
            objUserReportsModel.BaseQuery = obj.BaseQuery;
            return objUserReportsModel;
        }
        public List<UserReportGridDefintionModel> initializeAddUserReportGridDefinitionFromReportGridDefinitionControls(List<ReportGridDefintion> AllReportGridDefintion)
        {
            List<UserReportGridDefintionModel> userReportGridDefintionModelList = new List<UserReportGridDefintionModel>();
            UserReportGridDefintionModel UserReportGridDefintionModel;
            foreach (var item in AllReportGridDefintion)
            {
                UserReportGridDefintionModel = new UserReportGridDefintionModel();
                UserReportGridDefintionModel.Sequence = item.Sequence;
                UserReportGridDefintionModel.Columns = item.Columns;
                UserReportGridDefintionModel.Heading = item.Heading;
                UserReportGridDefintionModel.Alignment = item.Alignment;
                UserReportGridDefintionModel.Display = item.Display;
                UserReportGridDefintionModel.Required = item.Required;
                UserReportGridDefintionModel.AvailableonFilter = item.AvailableonFilter;
                UserReportGridDefintionModel.IsGroupTotaled = item.IsGroupTotaled;
                UserReportGridDefintionModel.IsGrandTotal = item.IsGrandTotal;
                UserReportGridDefintionModel.LocalizeDate = item.LocalizeDate;
                UserReportGridDefintionModel.IsChildColumn = item.IsChildColumn;
                UserReportGridDefintionModel.NumofDecPlaces = item.NumofDecPlaces;
                UserReportGridDefintionModel.NumericFormat = item.NumericFormat;
                UserReportGridDefintionModel.FilterMethod = item.FilterMethod;
                UserReportGridDefintionModel.DateDisplay = item.DateDisplay;
                userReportGridDefintionModelList.Add(UserReportGridDefintionModel);

            }
            return userReportGridDefintionModelList;
        }
        public List<UserReportGridDefintionModel> initializeAddUserReportGridDefinitionFromUserReportGridDefinitionControls(List<UserReportGridDefintion> AllUserReportGridDefintion)
        {
            List<UserReportGridDefintionModel> userReportGridDefintionModelList = new List<UserReportGridDefintionModel>();
            UserReportGridDefintionModel UserReportGridDefintionModel;
            foreach (var item in AllUserReportGridDefintion)
            {
                UserReportGridDefintionModel = new UserReportGridDefintionModel();
                UserReportGridDefintionModel.Sequence = item.Sequence;
                UserReportGridDefintionModel.Columns = item.Columns;
                UserReportGridDefintionModel.Heading = item.Heading;
                UserReportGridDefintionModel.Alignment = item.Alignment;
                UserReportGridDefintionModel.Display = item.Display;
                UserReportGridDefintionModel.Required = item.Required;
                UserReportGridDefintionModel.AvailableonFilter = item.AvailableonFilter;
                UserReportGridDefintionModel.IsGroupTotaled = item.IsGroupTotaled;
                UserReportGridDefintionModel.IsGrandTotal = item.IsGrandTotal;
                UserReportGridDefintionModel.LocalizeDate = item.LocalizeDate;
                UserReportGridDefintionModel.IsChildColumn = item.IsChildColumn;
                UserReportGridDefintionModel.Filter = item.Filter;
                UserReportGridDefintionModel.NumofDecPlaces = item.NumofDecPlaces;
                UserReportGridDefintionModel.NumericFormat = item.NumericFormat;
                UserReportGridDefintionModel.FilterMethod = item.FilterMethod;
                UserReportGridDefintionModel.DateDisplay = item.DateDisplay;
                userReportGridDefintionModelList.Add(UserReportGridDefintionModel);

            }
            return userReportGridDefintionModelList;

        }
        public UserReports AddReport(UserReportsModel objUserRpt)
        {
            #region UserReports
            UserReportsModel userReportsModel = new UserReportsModel();
            List<UserReportGridDefintionModel> listUserReportGridDefintionModel = new List<UserReportGridDefintionModel>();
            if (objUserRpt.IsUserReport)
            {
                DataContracts.UserReports UR = new DataContracts.UserReports()
                {
                    ClientId = this._userData.DatabaseKey.Client.ClientId,
                    UserReportsId = objUserRpt.SourceId
                };
                UR.Retrieve(_dbKey);
                userReportsModel = initializeAddUserReportFromUserReportControls(UR);

            }
            else
            {
                DataContracts.ReportListing RL = new DataContracts.ReportListing()
                {
                    ReportListingId = objUserRpt.SourceId
                };
                RL.Retrieve(_dbKey);
                userReportsModel = initializeAddUserReportFromReportListingControls(RL);

            }
            UserReports userReports = new UserReports()
            {
                ClientId = this._userData.DatabaseKey.Client.ClientId,
                SiteId = this._userData.Site.SiteId,
                ReportName = objUserRpt.ReportName,
                Description = objUserRpt.Description ?? "",
                SaveType = objUserRpt.SaveType,
                OwnerId = _dbKey.Personnel.PersonnelId

            };
            userReports.ReportGroup = userReportsModel.ReportGroup;
            userReports.SourceName = userReportsModel.SourceName;
            userReports.UseSP = userReportsModel.UseSP;
            userReports.PrimarySortColumn = userReportsModel.PrimarySortColumn;
            userReports.SecondarySortColumn = userReportsModel.SecondarySortColumn;
            userReports.IsGrouped = userReportsModel.IsGrouped;
            userReports.GroupColumn = userReportsModel.GroupColumn;
            userReports.IncludePrompt = userReportsModel.IncludePrompt;
            userReports.Prompt1Source = userReportsModel.Prompt1Source;
            userReports.Prompt1Type = userReportsModel.Prompt1Type;
            userReports.Prompt1ListSource = userReportsModel.Prompt1ListSource;
            userReports.Prompt1List = userReportsModel.Prompt1List;
            userReports.Prompt2Source = userReportsModel.Prompt2Source;
            userReports.Prompt2Type = userReportsModel.Prompt2Type;
            userReports.Prompt2ListSource = userReportsModel.Prompt2ListSource;
            userReports.Prompt2List = userReportsModel.Prompt2List;
            userReports.ChildSourceName = userReportsModel.ChildSourceName;
            userReports.MasterLinkColumn = userReportsModel.MasterLinkColumn;
            userReports.ChildLinkColumn = userReportsModel.ChildLinkColumn;
            userReports.IncludeChild = userReportsModel.IncludeChild;
            userReports.IsEnterprise = userReportsModel.IsEnterprise;
            userReports.BaseQuery = userReportsModel.BaseQuery;
            userReports.Create(this._userData.DatabaseKey);
            #region UserReportGridDefintion          
            if (objUserRpt.IsUserReport)
            {
                UserReportGridDefintion userReportGridDefintionAll = new UserReportGridDefintion();
                userReportGridDefintionAll.ReportId = objUserRpt.SourceId;
                var AllUserReportGridDefintion = userReportGridDefintionAll.RetrieveAllByReportId_V2(_dbKey);
                listUserReportGridDefintionModel = initializeAddUserReportGridDefinitionFromUserReportGridDefinitionControls(AllUserReportGridDefintion);

            }
            else
            {
                ReportGridDefintion reportGridDefintionAll = new ReportGridDefintion();
                reportGridDefintionAll.ReportListingId = objUserRpt.SourceId;
                var AllReportGridDefintion = reportGridDefintionAll.RetrieveAllByReportListingId_V2(_dbKey);
                listUserReportGridDefintionModel = initializeAddUserReportGridDefinitionFromReportGridDefinitionControls(AllReportGridDefintion);
            }

            foreach (var item in listUserReportGridDefintionModel)
            {
                UserReportGridDefintion userReportGridDefintion = new UserReportGridDefintion();
                userReportGridDefintion.ReportId = userReports.UserReportsId;
                userReportGridDefintion.Sequence = item.Sequence;
                userReportGridDefintion.Columns = item.Columns;
                userReportGridDefintion.Heading = item.Heading;
                userReportGridDefintion.Alignment = item.Alignment;
                userReportGridDefintion.Display = item.Display;
                userReportGridDefintion.Required = item.Required;
                userReportGridDefintion.AvailableonFilter = item.AvailableonFilter;
                userReportGridDefintion.IsGroupTotaled = item.IsGroupTotaled;
                userReportGridDefintion.IsGrandTotal = item.IsGrandTotal;
                userReportGridDefintion.LocalizeDate = item.LocalizeDate;
                userReportGridDefintion.IsChildColumn = item.IsChildColumn;
                userReportGridDefintion.Filter = item.Filter;
                userReportGridDefintion.NumericFormat = item.NumericFormat;
                userReportGridDefintion.NumofDecPlaces = item.NumofDecPlaces;
                userReportGridDefintion.FilterMethod = item.FilterMethod;
                userReportGridDefintion.DateDisplay = item.DateDisplay;
                userReportGridDefintion.Create(this._userData.DatabaseKey);
            }

            #endregion
            #region ReportEventLog          
            CreateReportEventLog(userReports.UserReportsId, true, ReportsEventLogConstants.Create);
            #endregion
            #endregion
            return userReports;
        }
        public UserReports EditReport(UserReportsModel objUserRpt)
        {
            #region UserReports

            UserReports userReports = new UserReports()
            {
                ClientId = this._userData.DatabaseKey.Client.ClientId,
                SiteId = this._userData.Site.SiteId,
                UserReportsId = objUserRpt.UserReportsId,
                ReportName = objUserRpt.ReportName
            };
            userReports.Retrieve(this._userData.DatabaseKey);
            userReports.Description = objUserRpt.Description ?? "";
            userReports.SaveType = objUserRpt.SaveType;
            userReports.OwnerId = _dbKey.Personnel.PersonnelId;
            userReports.Update(this._userData.DatabaseKey);
            #region ReportEventLog          
            CreateReportEventLog(userReports.UserReportsId, true, ReportsEventLogConstants.Save);
            #endregion
            #endregion
            return userReports;
        }
        public bool DeleteUserReport(long UserReportsId)
        {
            #region UserReports
            bool DelReport = true;
            UserReports userReports = new UserReports()
            {
                ClientId = this._userData.DatabaseKey.Client.ClientId,
                SiteId = this._userData.Site.SiteId,
                UserReportsId = UserReportsId

            };
            userReports.Retrieve(this._userData.DatabaseKey);
            userReports.Del = true;
            userReports.Update(this._userData.DatabaseKey);
            #region UserReportGridDefintion          
            UserReportGridDefintion userReportGridDefintion = new UserReportGridDefintion()
            {
                ReportId = userReports.UserReportsId
            };

            userReportGridDefintion.DeleteByReportId(this._userData.DatabaseKey);
            #endregion
            #region ReportEventLog          
            CreateReportEventLog(userReports.UserReportsId, true, ReportsEventLogConstants.Delete);
            #endregion
            #endregion
            if (userReports.ErrorMessages != null && userReports.ErrorMessages.Count > 0)
            {
                DelReport = false;

            }
            return DelReport;
        }
        public UserReportsModel GetUserReportDetailsById(long UserReportsId)
        {
            UserReportsModel userReportsModel = new UserReportsModel();
            DataContracts.UserReports userReports = new DataContracts.UserReports()
            {
                ClientId = this._userData.DatabaseKey.Client.ClientId,
                UserReportsId = UserReportsId
            };
            userReports.Retrieve(_dbKey);
            userReportsModel = initializeDetailsControls(userReports);
            return userReportsModel;
        }
        public UserReportsModel initializeDetailsControls(UserReports obj)
        {
            UserReportsModel objUserReportsModel = new UserReportsModel();
            objUserReportsModel.UserReportsId = obj.UserReportsId;
            objUserReportsModel.ReportName = obj.ReportName;
            objUserReportsModel.Description = obj.Description;
            objUserReportsModel.SaveType = obj.SaveType;
            return objUserReportsModel;
        }
        public void RunReport(UserReportsModel objUserRpt)
        {
            CreateReportEventLog(objUserRpt.UserReportsId, true, ReportsEventLogConstants.Run);
        }
        public int CountIfUserReportNameExist(string ReportName)
        {
            UserReports _userReports = new UserReports();
            _userReports.ClientId = _userData.DatabaseKey.Client.ClientId;
            _userReports.ReportName = ReportName;
            var cnt = _userReports.RetrieveCountForReportNameExist(_userData.DatabaseKey);
            int count = cnt.Count;

            return count;
        }
        #endregion

        #region update user Report
        public List<string> SaveUsersReportSettings(long ReportId, List<ReportConfigModel> config)
        {
            UserReportGridDefintion objUR = new UserReportGridDefintion();
            objUR.ReportId = ReportId;
            objUR.UserReportList = ToDataTable<ReportConfigModel>(config);
            objUR.UpdateByReportId(this._userData.DatabaseKey);
            return objUR.ErrorMessages;
        }
        public List<string> SaveUsersReportFilterSettings(long ReportId, List<ReportFilteConfigModel> config)
        {
            UserReportGridDefintion objUR = new UserReportGridDefintion();
            objUR.ReportId = ReportId;
            objUR.UserReportList = ToDataTable<ReportFilteConfigModel>(config);
            objUR.UpdateFilterByReportId(this._userData.DatabaseKey);
            return objUR.ErrorMessages;
        }
        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
        #endregion
        #region V2-975
        private DataTable LocalizeDate(DataTable dtRpt, List<GridColumnsProp> gridColumnsProps)
        {
            if (dtRpt != null && gridColumnsProps != null)
            {
                List<string> dateColumns = gridColumnsProps.Where(x => x.LocalizeDate == true).Select(x => x.data).ToList();
                if (dateColumns.Count > 0)
                {
                    for (int rows = 0; rows < dtRpt.Rows.Count; rows++)
                    {
                        for (int cols = 0; cols < dateColumns.Count; cols++)
                        {
                            var colName = dateColumns[cols];
                            var val = dtRpt.Rows[rows][colName].ToString();
                            if (!string.IsNullOrEmpty(val.ToString()))
                            {
                                try
                                {
                                    DateTime dt = DateTime.SpecifyKind(Convert.ToDateTime(val), DateTimeKind.Utc);
                                    TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(_userData.Site.TimeZone);
                                    DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(dt, timeZone);
                                    dtRpt.Rows[rows].SetField(colName, localTime.ToString());
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }
                    }
                }
            }
            return dtRpt;
        }
        #endregion

        #region POByAccountReport V2-1073
        public PurchaseOrder POByAccountReport(DateTime StartDate, DateTime EndDate)
        {
            PurchaseOrder po = new PurchaseOrder();
            po.ClientId = this._userData.DatabaseKey.Client.ClientId;
            po.SiteId = this._userData.DatabaseKey.User.DefaultSiteId;
            po.StartDate = StartDate;
            po.EndDate = EndDate;
            po.RetrievePOByAccountReport(this._userData.DatabaseKey);
            return po;
        }
        #endregion
    }

}
