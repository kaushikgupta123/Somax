using Common.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.IoTEvent
{
    public class IoTEventAcknowledgeModel
    {
        public long IoTEventId { get; set; }
        public IEnumerable<SelectListItem> FaultCodeList { get; set; }
        [Required(ErrorMessage = "ValidationFaultCode|" + LocalizeResourceSetConstants.IoTEvent)]
        public string FaultCode { get; set; }

        public string Comments { get; set; }
    }
}