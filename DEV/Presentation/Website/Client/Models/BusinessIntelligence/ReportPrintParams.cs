using System;

namespace Client.Models.BusinessIntelligence
{
    [Serializable]
    public class ReportPrintParams
    {
        public long ReportListingId { get; set; }
        public string MultiSelectData1 { get; set; }
        public int CaseNo1 { get; set; }
        public DateTime? StartDate1 { get; set; }
        public DateTime? EndDate1 { get; set; }
        public string MultiSelectData2 { get; set; }
        public int CaseNo2 { get; set; }
        public DateTime? StartDate2 { get; set; }
        public DateTime? EndDate2 { get; set; }
        public bool HasChildGrid { get; set; }
        public bool IsGrouped { get; set; }
        public bool IsUserReport { get; set; }
    }
}