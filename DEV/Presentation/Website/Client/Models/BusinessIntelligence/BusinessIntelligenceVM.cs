using Client.Models.BusinessIntelligence;
using Client.Models.Common;
using DataContracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace Client.Models
{
    public class BusinessIntelligenceVM : LocalisationBaseVM
    {
        public BusinessIntelligenceVM()
        {
            ReportGroups = new List<string>();
            ReportLists = new List<ReportListingModel>();
            RecentReports = new List<ReportListingModel>();
            multiSelectPrompt1 = new List<DropDownModel>();
            multiSelectPrompt2 = new List<DropDownModel>();
        }
        public YurbiReportModel _YurbiReportModel { get; set; }
        public ReportListingModel reportListingModel { get; set; }
        public List<ReportListingModel> ReportLists { get; set; }
        public List<ReportListingModel> RecentReports { get; set; }
        public List<string> ReportGroups { get; set; }
        public IEnumerable<SelectListItem> DateRangeDropListForReport { get; set; }
        public bool IncludePrompt { get; set; }
        public Security security { get; set; }
        public bool IsGrouped { get; set; }
        //public List<TableHaederProp> tableHaederProps { get; set; }
        public ReportPrintModel reportPrintModel { get; set; }
        public List<DropDownModel> multiSelectPrompt1 { get; set; }
        public List<DropDownModel> multiSelectPrompt2 { get; set; }
        public UserReportsModel userReportsModel { get; set; }
        public UserReportGridDefintionModel userReportGridDefintionModel { get; set; }
        public bool IsUserReport { get; set; }
    }

}