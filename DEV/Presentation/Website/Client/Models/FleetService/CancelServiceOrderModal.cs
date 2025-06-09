using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.FleetService
{
    public class CancelServiceOrderModal
    {
        [Required(ErrorMessage = "CancelReasonErrormsg|" + LocalizeResourceSetConstants.Global)]
        public string CancelReasonSo { get; set; }
        public IEnumerable<SelectListItem> CancelReasonListSo { get; set; }
        public long ServiceOrderId { get; set; }
    }
}