using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.Work_Order
{
    public class WoSendForApprovalModel
    {
        [Required(ErrorMessage = "globalRequireSendTo|" + LocalizeResourceSetConstants.Global)]
        public long PersonnelId { get; set; }
        public IEnumerable<SelectListItem> Personnellist { get; set; }
        public long WorkOrderId { get; set; }
    }
}