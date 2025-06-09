using Client.CustomValidation;
using Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace Client.Models
{
    public class ChangeVendorIDModel
    {
        public long VendorId { get; set; }
        public string OldClientLookupId { get; set; }
        [Required(ErrorMessage = "VendorIdErrMsg|" + LocalizeResourceSetConstants.VendorDetails)]
        [RegularExpression("^[A-Z0-9\\%\\-\\:\\/\\$\\*\\+\\.]+$", ErrorMessage = "VendorIdRegErrMsg|" + LocalizeResourceSetConstants.VendorDetails)]
        [Unlike("OldClientLookupId", ErrorMessage = "globalUnlikeChangeClientLookupId|" + LocalizeResourceSetConstants.Global)]
        public string NewClientLookupId { get; set; }
    }
}