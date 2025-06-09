using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.PartTransfer
{
    public class PartTransferCancelModel
    {
        public long? PartTransferId { get; set; }
        [Required(ErrorMessage = "ValidComment|" + LocalizeResourceSetConstants.PartTransferDetail)]
        public string CancelComment { get; set; }
        [Required(ErrorMessage = "ValidCancelReason|" + LocalizeResourceSetConstants.PartTransferDetail)]
        public string CancelReason { get; set; }
        public IEnumerable<SelectListItem> CancelReasonList { get; set; }
    }
}