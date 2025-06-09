using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models
{
    public class AutoTRGenerationSearchModel
    {
        public long RowId { get; set; }
        public string PartIdClientLookupId { get; set; }
        public string RequestStr { get; set; }
        public string IssueStr { get; set; }
        public string PartDescription { get; set; }
        public decimal? TransferQuantity { get; set; }
        public decimal? Max { get; set; }
        public decimal? Min { get; set; }
        public decimal? OnHand { get; set; }
        public long RequestPTStoreroomId { get; set; }
        public long RequestStoreroomId { get; set; }
        public long RequestPartId { get; set; }
        public long IssuePTStoreroomId { get; set; }
        public long IssueStoreroomId { get; set; }
        public long IssuePartId { get; set; }
        public long Creator_PersonnelId { get; set; }
        public int TotalCount { get; set; }
    }
}