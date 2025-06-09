using Common.Constants;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.Configuration.SiteSetUp
{
    public class ApprovalGroupSettingsModel
    {
        public long ApprovalGroupSettingsId { get; set; }
        public bool MaterialRequests { get; set; }
        public bool PurchaseRequests { get; set; }
        public bool SanitationRequests { get; set; }
        public bool WorkRequests { get; set; }
    }
}