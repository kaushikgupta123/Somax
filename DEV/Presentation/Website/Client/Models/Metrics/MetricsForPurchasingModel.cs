using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models
{
    public class MetricsForPurchasingModel
    {
        public string SiteName { get; set; }
        public decimal PurchaseOrdersCreated { get; set; }
        public decimal PurchaseOrdersCompleted { get; set; }
        public decimal ReceivedAmount { get; set; }
    }
}