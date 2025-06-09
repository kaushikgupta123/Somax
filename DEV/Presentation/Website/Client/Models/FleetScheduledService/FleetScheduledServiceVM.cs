using DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.FleetScheduledService
{
    public class FleetScheduledServiceVM : LocalisationBaseVM
    {
        public Security security { get; set; }
        public UserData _userdata { get; set; }
        public FleetScheduledServiceModel ScheduledServiceModel { get; set; }
        public IEnumerable<SelectListItem> InactiveFlagList { get; set; }
        public List<FleetScheduledPDFPrintModel> fleetScheduledPDFPrintModel { get; set; }
        public IEnumerable<SelectListItem> LookUpRepairReasonList { get; set; }
        public List<HierarchicalList> VMRSSystemList { get; set; }
    }
}