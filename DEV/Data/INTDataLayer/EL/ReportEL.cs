using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INTDataLayer.EL
{
    public class ReportEL
    {
        public Int64 ClientId
        { set; get; }
        public Int64 UserInfoId
        { set; get; }
        public string ReportName
        { set; get; }
        public string ReportTitle
        { set; get; }       
        public string ReportSortBy1
        { set; get; }
        public string ReportSortBy2
        { set; get; }
        public string ReportSortBy3
        { set; get; }
        public string ReportOrderBy1
        { set; get; }
        public string ReportOrderBy2
        { set; get; }
        public string ReportOrderBy3
        { set; get; }
        public string CompanyName
        { set; get; }
        public string Language
        { set; get; }
        public string Culture
        { set; get; }
        public string Description
        { set; get; }
        public string ReportURL
        { set; get; }
        public Int64 AccessNo
        { set; get; }
        public Int64 ReportId
        { set; get; }
        public Int64 RuntimeId
        { set; get; }
        public String SearchName
        { set; get; }
        public String SortCols
        { set; get; }
        public string SortOrder
        { set; get; }
        public String GroupCols
        { set; get; }
        public String QryVal
        { set; get; }
        public String HeaderText
        { set; get; }
        public String FooterText
        { set; get; }
        public bool IncludeHeader
        { set; get; }
        public bool SomaxOwned
        { set; get; }
        public bool ShowCompanyName
        { set; get; }
        public bool ShowDateRange
        { set; get; }
        public bool ShowReportName
        { set; get; }
        public bool ShowReportTitle
        { set; get; }
        public bool ShowSites
        { set; get; }
        public bool ShowRunDate
        { set; get; }
        public bool ShowRunTime
        { set; get; }
        public DateTime Startdate
        { set; get; }
        public DateTime EndDate
        { set; get; }
        public String ViewCols
        { set; get; }
        public String TotCols
        { set; get; }    
        public String CreateBy
        { set; get; }
        public String ModifyBy
        { set; get; }
        public DateTime CreateDate
        { set; get; }
        public DateTime ModifyDate
        { set; get; }
        public String Query
        { set; get; }
        public String Sites
        { set; get; }
        public Int64 SiteId
        { set; get; }
        public Int64 AreaId
        { set; get; }
        public Int64 DepartmentId
        { set; get; }
        public Char Flag
        { set; get; }
        public String DateCol
        { set; get; }
        public String DateRange
        { set; get; }
        public bool AllowDateFilter
        { set; get; }
        public bool AllowOtherFilter
        { set; get; }
        public bool IsGrouped
        { set; get; }
        public bool Public // Som-1193
        { set; get; }
        public int UpdateIndex
        { set; get; }
        public string DtlsReportUrl
        { set; get; }
        public string ReportGroup
        { get; set; }
        public string Button
        { get; set; }
        public string ColName
        { get; set; }       
        public string EventName
        { get; set; }
    }
}
