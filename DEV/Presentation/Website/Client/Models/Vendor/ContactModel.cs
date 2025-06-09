using Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace Client.Models
{
    public class ContactModel
    {
        public long ContactId { get; set; }
        [Display(Name = "spnName|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "VendorContactErrMsg|"+ LocalizeResourceSetConstants.VendorDetails)]
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string AddressCity { get; set; }
        public string AddressCountry { get; set; }
        public string AddressPostCode { get; set; }
        public string AddressState { get; set; }
        [Display(Name = "VendorContactOffice|" + LocalizeResourceSetConstants.VendorDetails)]
        public string Phone1 { get; set; }
        [Display(Name = "VendorContactMobile|" + LocalizeResourceSetConstants.VendorDetails)]
        public string Phone2 { get; set; }
        [Display(Name = "VendorFax|" + LocalizeResourceSetConstants.VendorDetails)]
        public string Phone3 { get; set; }
        [Display(Name = "VendorContactEmail1|" + LocalizeResourceSetConstants.VendorDetails)]
        [EmailAddress(ErrorMessage = "VendorEmailErrMsg|" + LocalizeResourceSetConstants.VendorDetails)]
        public string Email1 { get; set; }
        [Display(Name = "VendorContactEmail2|" + LocalizeResourceSetConstants.VendorDetails)]
        [EmailAddress(ErrorMessage = "VendorEmailErrMsg|" + LocalizeResourceSetConstants.VendorDetails)]
        public string Email2 { get; set; }
        public string ClientLookupId { get; set; }
        public long VendorId { get; set; }
        public int updatedindex { get; set; }
        public string OwnerName { get; set; }
    }
}