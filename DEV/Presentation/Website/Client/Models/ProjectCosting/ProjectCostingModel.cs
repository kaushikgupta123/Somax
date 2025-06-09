using System.Collections.Generic;
using System.Web.Mvc;

namespace Client.Models.ProjectCosting
{
    public class ProjectCostingModel
    {
        public long ProjectId { get; set; }
        public string Status { get; set; }
        public int MyProperty { get; set; }
        public string ClientLookupId { get; set; }
        public IEnumerable<SelectListItem> StatusList { get; set; }
        public IEnumerable<SelectListItem> DateRangeDropListForAllStatus { get; set; }
        public IEnumerable<SelectListItem> DateRangeDropListForCompletedProject { get; set; }
        public IEnumerable<SelectListItem> DateRangeDropListForClosedProject { get; set; }
    }
}