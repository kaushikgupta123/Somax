using Admin.CustomValidation;
using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Models.Client
{
    public class SiteBillingModelView
    {
        public long SiteId { get; set; }
        public long ClientId { get; set; }
        public long SiteBillingId { get; set; }
        public DateTime? AnniversaryDate { get; set; }
        public string InvoiceFreq { get; set; }
        public string Terms { get; set; }
        public string CurrentInvoice { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime? NextInvoiceDate { get; set; }
        public bool QuoteRequired { get; set; }
    }
}