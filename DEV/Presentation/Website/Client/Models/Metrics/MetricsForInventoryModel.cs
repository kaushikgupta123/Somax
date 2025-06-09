using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models
{
    public class MetricsForInventoryModel
    {
        public string SiteName { get; set; }
        public decimal Valuation { get; set; }
        public decimal LowParts { get; set; }
    }
}