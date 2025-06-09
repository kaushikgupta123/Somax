using Common.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.IoTEvent
{
    public class IoTEventDismissModel
    {
        public long IoTEventId { get; set; }
        public IEnumerable<SelectListItem> DismissReasonList { get; set; }
        [Required(ErrorMessage = "ValidationDismissReason|" + LocalizeResourceSetConstants.IoTEvent)]
        public string DismissReason { get; set; }
        public string Comments { get; set; }
    }
}