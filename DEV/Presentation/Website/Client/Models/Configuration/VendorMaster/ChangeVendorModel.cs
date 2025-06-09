using Client.CustomValidation;
using Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace Client.Models.Configuration.VendorMaster
{
    public class ChangeVendorModel
    {
        [Required(ErrorMessage = "VendorIdErrMsg|" + LocalizeResourceSetConstants.VendorDetails)]
        [RegularExpression("^[A-Z0-9\\%\\-\\:\\/\\$\\*\\+\\.]+$", ErrorMessage = "VendorIdRegErrMsg|" + LocalizeResourceSetConstants.VendorDetails)]
        [Unlike("oldClientLookupId", ErrorMessage = "globalUnlikeChangeClientLookupId|" + LocalizeResourceSetConstants.Global)]
        public string ClientLookupId { get; set; }
        public long VendorMasterId { get; set; }
        public long UpdateIndex { get; set; }
        public string oldClientLookupId { get; set; }
    }
}