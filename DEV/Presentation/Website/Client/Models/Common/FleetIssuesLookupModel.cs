using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Common
{
    public class FleetIssuesLookupModel
    {
        public long FleetIssuesId { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public long SiteId { get; set; }
        public int PageNumber { get; set; }
        public int ResultsPerPage { get; set; }
        public string OrderColumn { get; set; }
        public string OrderDirection { get; set; }
        public string Defects { get; set; }
        public DateTime? Readingdate { get; set; }
        public int TotalCount { get; set; }
    }
}