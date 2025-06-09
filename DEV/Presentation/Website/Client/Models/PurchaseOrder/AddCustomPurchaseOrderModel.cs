
using Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace Client.Models.PurchaseOrder
{
    public class AddCustomPurchaseOrderModel
    {
        [Required(ErrorMessage = "validationInitials|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression("^[A-Z]+$", ErrorMessage = "initialsRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string Initials { get; set; }
        [Required(ErrorMessage = "globalValidShipToId|" + LocalizeResourceSetConstants.Global)]
        public string ShiptoSuffix { get; set; }
    }
}