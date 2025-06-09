using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.PartTransfer
{
    public class PartTransferDenyModel
    {
        public long? PartTransferId { get; set; }   
        public IEnumerable<SelectListItem> DenyReasonIdList { get; set; }
        [Required(ErrorMessage = "ValidDenyReason|" + LocalizeResourceSetConstants.PartTransferDetail)]
        public string DenyReasonId { get; set; }
        [Required(ErrorMessage = "ValidComment|" + LocalizeResourceSetConstants.PartTransferDetail)]
        public string Comment { get; set; }
    }
}