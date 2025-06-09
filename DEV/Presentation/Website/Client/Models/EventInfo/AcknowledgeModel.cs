using Common.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.EventInfo
{
    public class AcknowledgeModel
    {
        public long EventInfoId { get; set; }
        public IEnumerable<SelectListItem> FaultCodeList { get; set; }
        [Required(ErrorMessage = "ValidationFaultCode|" + LocalizeResourceSetConstants.EventInfo)]
        public string FaultCode { get; set; }

        [Required(ErrorMessage = "ValidationComment|" + LocalizeResourceSetConstants.EventInfo)]
        public string Comments { get; set; }


    }
}