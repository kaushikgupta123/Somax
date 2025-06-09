using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.PartTransfer
{
    public class PartTransferForceCompleteModel
    {
        public long? PartTransferId { get; set; }
        public IEnumerable<SelectListItem> ForceCompleteReasonList { get; set; }
        [Required(ErrorMessage = "ValidForceCompleteReason|" + LocalizeResourceSetConstants.PartTransferDetail)]
        public string ForceCompleteReasonId { get; set; }
        [Required(ErrorMessage = "ValidComment|" + LocalizeResourceSetConstants.PartTransferDetail)]
        public string Comment { get; set; }
    }
}