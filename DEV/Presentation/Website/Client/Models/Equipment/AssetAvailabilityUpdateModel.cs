using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Client.Models.Equipment
{
    public class AssetAvailabilityUpdateModel
    {
        public long EquipmentId { get; set; }
        [Required(ErrorMessage = "ExpectedReturnErrMsg|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public DateTime? ExpectedReturnToService { get; set; }
        [Required(ErrorMessage = "CommentErrMsg|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string RemoveFromServiceReason { get; set; }
    }
}