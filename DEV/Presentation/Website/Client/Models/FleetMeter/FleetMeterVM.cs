using DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.FleetMeter
{
    public class FleetMeterVM : LocalisationBaseVM
    {
        public FleetMeterModel fleetMeterModel { get; set; }
        public Security security { get; set; }
        public UserData _userdata { get; set; }
        public IEnumerable<SelectListItem> DateRangeDropListForFMreadingDate { get; set; }
        public List<FleetMeterPDFPrintModel> fleetMeterPDFPrintModel { get; set; }
    }
}