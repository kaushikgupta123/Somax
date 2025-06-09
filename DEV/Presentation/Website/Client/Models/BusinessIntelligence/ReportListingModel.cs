namespace Client.Models.BusinessIntelligence
{
    public class ReportListingModel
    {
        public long ReportListingId { get; set; }
        public string ReportName { get; set; }
        public string Description { get; set; }
        public string ReportGroup { get; set; }
        public string SourceName { get; set; }
        public bool UseSP { get; set; }
        public string PrimarySortColumn { get; set; }
        public string SecondarySortColumn { get; set; }
        public bool IsGrouped { get; set; }
        public string GroupColumn { get; set; }
        public bool IncludePrompt { get; set; }
        public string Prompt1Source { get; set; }
        public string Prompt1Type { get; set; }
        public string Prompt1ListSource { get; set; }
        public string Prompt1List { get; set; }
        public string Prompt2Source { get; set; }
        public string Prompt2Type { get; set; }
        public string Prompt2ListSource { get; set; }
        public string Prompt2List { get; set; }
        public bool IsFavorite { get; set; }
        public long ReportFavoritesId { get; set; }
        public long ReportEventLogId { get; set; }
        public string ChildSourceName { get; set; }
        public string MasterLinkColumn { get; set; }
        public string ChildLinkColumn { get; set; }
        public bool IncludeChild { get; set; }
        public bool IsUserReport { get; set; }
        public string SaveType { get; set; }
        public bool IsEnterprise { get; set; }
        public string BaseQuery { get; set; }
        #region V2-879
        public bool NoExcel { get; set; }
        public bool NoCSV { get; set; }
        #endregion
        #region V2-879
        public bool DevExpressRpt { get; set; }
        public string DevExpressRptName { get; set; }
        #endregion
    }
}