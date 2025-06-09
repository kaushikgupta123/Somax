using DataContracts;
using System.Collections.Generic;
using System.Web.Mvc;
namespace Client.Models.FleetFuel
{
    public class FleetFuelVM : LocalisationBaseVM
    {
        public FleetFuelVM()
        {
        }
        public Security security { get; set; }
        public UserData _userdata { get; set; }
        public IEnumerable<SelectListItem> LookupFuelTypeList { get; set; }
        public FleetFuelModel FleetFuelModel { get; set; }
        public List<FleetFuelPDFPrintModel> fleetFuelPDFPrintModel { get; set; }
       
    }

   
}