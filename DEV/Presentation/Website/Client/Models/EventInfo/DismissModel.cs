using Common.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.EventInfo
{
    public class DismissModel
    {
        public long EventInfoId { get; set; }
        public IEnumerable<SelectListItem> DismissReasonList { get; set; }
        [Required(ErrorMessage = "ValidationDismissReason|" + LocalizeResourceSetConstants.EventInfo)]
        public string DismissReason { get; set; }
        [Required(ErrorMessage = "ValidationComments|" + LocalizeResourceSetConstants.EventInfo)]
        public string Comments { get; set; }
    }
}