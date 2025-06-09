using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Client.Models.FleetAsset
{
    public class AssetAvailabilityModel
    {
        public long EquipmentId { get; set; }
        [Display(Name = "spnExpectedReturn|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public DateTime? ExpectedReturnToService { get; set; }
        [Required(ErrorMessage = "RemoveFromServiceReasonErrorMessage|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [Display(Name = "GlobalRemove|" + LocalizeResourceSetConstants.Global)]
        public string RemoveFromServiceReason { get; set; }
        public bool RemoveFromService { get; set; }
    }
}