using Common.Constants;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Client.Models.Approval
{
    public class MaterialRequestModel
    {
        public long SiteId { get; set; }
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int Offset { get; set; }
        public int Nextrow { get; set; }
        public string FilterTypeCase { get; set; }
        public string ClientLookupId { get; set; }
        public DateTime? Date { get; set; }
        public string Description { get; set; }
        public decimal UnitCost { get; set; }
        public decimal Quantity { get; set; }
        public decimal TotalCost { get; set; }
        public string Comments { get; set; }
        public int TotalCount { get; set; }

        public long ApprovalRouteId { get; set; }
        public long EstimatedCostsId { get; set; }
        public long MaterialRequestId { get; set; }
        public long ApprovalGroupId { get; set; }
    }
}