using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.AutoPRGeneration
{
    public class AutoTRGenerationPrintModel
    {
        public string PartIdClientLookupId { get; set; }
        public string RequestStr { get; set; }
        public string IssueStr { get; set; }
        public string PartDescription { get; set; }
        public decimal? TransferQuantity { get; set; }
        public decimal? Max { get; set; }
        public decimal? Min { get; set; }
        public decimal? OnHand { get; set; }
    }
}