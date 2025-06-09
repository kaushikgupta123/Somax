using DataContracts;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Client.Models.EventInfo
{
    public class EventInfoVM : LocalisationBaseVM
    {
        public EventInfoModel eventInfoModel { get; set; }
        public Security security { get; set; }
        public DismissModel dismissModel { get; set; }
        public AcknowledgeModel acknowledgeModel { get; set; }

        public EventDescribeModel eventDescribeModel { get; set; }
        
        public IEnumerable<SelectListItem> scheduleList { get; set; }
        public EventOnDemandModel eventOnDemandModel { get; set; }
    }
}