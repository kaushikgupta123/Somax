using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Client.Models.Equipment
{
    public class AssetAvailabilityRemoveModel
    {
        public long EquipmentId { get; set; }
        public DateTime? ExpectedReturnToService { get; set; }
        [Required(ErrorMessage = "ReasonErrMsg|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string RemoveFromServiceReasonCode { get; set; }
        public string RemoveFromServiceReason { get; set; }
        public bool RemoveFromService { get; set; }
    }
}