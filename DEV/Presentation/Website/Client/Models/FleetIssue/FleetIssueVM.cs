using DataContracts;
using System.Collections.Generic;
using System.Web.Mvc;
namespace Client.Models.FleetIssue
{
    public class FleetIssueVM : LocalisationBaseVM
    {
        public FleetIssueVM()
        {
        }

        public Security security { get; set; }
        public UserData _userdata { get; set; }
        public IEnumerable<SelectListItem> LookupDefectsList { get; set; }
        public FleetIssueModel FleetIssueModel { get; set; }
        public List<FleetIssuePDFPrintModel> fleetIssuePDFPrintModel { get; set; }
    }
}