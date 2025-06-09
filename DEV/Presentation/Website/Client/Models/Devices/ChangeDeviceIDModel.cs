using Client.CustomValidation;
using Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace Client.Models
{
    public class ChangeDeviceIDModel
    {
        public long IoTDeviceId { get; set; }
        [StringLength(31, ErrorMessage = "EquipmentStrLenErrorMessage|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [Unlike("OldClientLookupId", ErrorMessage = "globalUnlikeChangeClientLookupId|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "EquipmentIDErrorMessage|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string ClientLookupId { get; set; }       
        public int UpdateIndex { get; set; }
        public string OldClientLookupId { get; set; }
    }
}