using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Client.Models.LaborScheduling
{
    public class AvailableSchedulingModel
    {
        public long  WorkOrderId { get; set; }
        public string ClientLookupId { get; set; }
        public string ChargeTo { get; set; }
        public string ChargeToName { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public string DownRequired { get; set; }
        public string Assigned { get; set; }
       
        public string Type { get; set; }
        public DateTime? StartDate { get; set; }
        public Decimal? Duration { get; set; }
        public DateTime? RequiredDate { get; set; }
        public int PartsOnOrder { get; set; } //V2-838
        #region V2-984
        public IEnumerable<SelectListItem> StatusList { get; set; }
        public IEnumerable<SelectListItem> PriorityList { get; set; }
        public IEnumerable<SelectListItem> DownRequiredInactiveFlagList { get; set; }
        public IEnumerable<SelectListItem> PersonnelIdList { get; set; }
        public IEnumerable<SelectListItem> TypeList { get; set; }
        public long WorkAssigned_PersonnelId { get; set; }
        public int TotalCount { get; set; }
        #endregion

    }
}