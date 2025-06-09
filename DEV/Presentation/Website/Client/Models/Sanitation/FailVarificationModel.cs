using Common.Constants;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.Sanitation
{
   
    public class FailVarificationModel
    {

      
        [Required(ErrorMessage = "SpanSanitationReasonFailedMustSelected|" + LocalizeResourceSetConstants.SanitationDetails)]
        public string FailReason { get; set; }
        public string Comments { get; set; }
        public long SanitationJobId { get; set; }
        public IEnumerable<SelectListItem> FailReasonList { get; set; }
    }
}