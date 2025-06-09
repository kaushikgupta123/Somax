using DataContracts;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Client.Models.IoTEvent
{ 
    public class IoTEventVM : LocalisationBaseVM
    {
        public IoTEventModel ioTEventModel { get; set; }
        public Security security { get; set; }
        public IoTEventDismissModel dismissModel { get; set; }
        public IoTEventAcknowledgeModel acknowledgeModel { get; set; }
        public IEnumerable<SelectListItem> OpenFlagList { get; set; }
    }
}