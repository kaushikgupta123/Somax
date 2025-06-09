using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.ProjectCosting
{
    public class ProjetCostingWorkOrderSearchModel
    {
        public long ProjectId { get; set; }
        public string ClientLookupId { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Planner { get; set; }
        public string CompleteDate { get; set; }
        public decimal? MaterialCost { get; set; }
        public decimal? LaborCost { get; set; }
        public decimal? TotalCost { get; set; }
        public int TotalCount { get; set; }
    }
}