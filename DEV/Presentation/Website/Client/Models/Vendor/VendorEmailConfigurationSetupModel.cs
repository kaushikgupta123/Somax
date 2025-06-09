using Common.Constants;

using System.ComponentModel.DataAnnotations;

namespace Client.Models
{
    public class VendorEmailConfigurationSetupModel
    {
        public long VendorId { get; set; }
        [Display(Name = "VendorEmail|" + LocalizeResourceSetConstants.VendorDetails)]
        [Required(ErrorMessage = "VendorEmailErrMsg|" + LocalizeResourceSetConstants.VendorDetails)]
        [EmailAddress(ErrorMessage = "VendorEmailErrMsg|" + LocalizeResourceSetConstants.VendorDetails)]
        public string Email { get; set; }
        public bool AutoEmailPO { get; set; }
    }
}