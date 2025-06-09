using Common.Constants;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.VendorRequest
{
    public class VendorRequestModel
    {   
        public long VendorRequestId { get; set; }
        public string MyProperty { get; set; }
        [Display(Name = "VendorAddress|" + LocalizeResourceSetConstants.VendorDetails)]
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        [Display(Name = "VendorCity|" + LocalizeResourceSetConstants.VendorDetails)]
        public string AddressCity { get; set; }
        [Display(Name = "VendorCountry|" + LocalizeResourceSetConstants.VendorDetails)]
        public string AddressCountry { get; set; }
        [Display(Name = "VendorPostalCode|" + LocalizeResourceSetConstants.VendorDetails)]
        public string AddressPostCode { get; set; }
        [Display(Name = "VendorState|" + LocalizeResourceSetConstants.VendorDetails)]
        public string AddressState { get; set; }
        [Display(Name = "VendorCusAcc|" + LocalizeResourceSetConstants.VendorDetails)]
        public string CustomerAccount { get; set; }
        [Display(Name = "VendorEmail|" + LocalizeResourceSetConstants.VendorDetails)]
        //[Required(ErrorMessage = "VendorEmailErrMsg|" + LocalizeResourceSetConstants.VendorDetails)]
        [EmailAddress(ErrorMessage = "VendorEmailErrMsg|" + LocalizeResourceSetConstants.VendorDetails)]

        public string EmailAddress { get; set; }
        [Display(Name = "VendorFax|" + LocalizeResourceSetConstants.VendorDetails)]
        public string FaxNumber { get; set; }
        [Display(Name = "VendorFOBCode|" + LocalizeResourceSetConstants.VendorDetails)]
        public string FOBCode { get; set; }
        [Display(Name = "spnName|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "spnNameErrorMsg|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression("^[A-Za-z0-9 _]*[A-Za-z0-9][A-Za-z0-9 &amp()-_]*$", ErrorMessage = "VendorNameRegErrMsg|" + LocalizeResourceSetConstants.VendorDetails)]

        public string Name { get; set; }
        [Display(Name = "VendorPhNo|" + LocalizeResourceSetConstants.VendorDetails)]
        [RegularExpression(@"^[\+0-9()-]*$", ErrorMessage = "VendorPhoneNoErrMsg|" + LocalizeResourceSetConstants.VendorDetails)]
        public string PhoneNumber { get; set; }
        [Display(Name = "VendorAddress|" + LocalizeResourceSetConstants.VendorDetails)]
        public string RemitAddress1 { get; set; }
        public string RemitAddress2 { get; set; }
        public string RemitAddress3 { get; set; }
        [Display(Name = "VendorCity|" + LocalizeResourceSetConstants.VendorDetails)]
        public string RemitCity { get; set; }
        [Display(Name = "VendorCountry|" + LocalizeResourceSetConstants.VendorDetails)]
        public string RemitCountry { get; set; }
        [Display(Name = "VendorPostalCode|" + LocalizeResourceSetConstants.VendorDetails)]
        public string RemitPostCode { get; set; }
        [Display(Name = "VendorState|" + LocalizeResourceSetConstants.VendorDetails)]
        public string RemitState { get; set; }
        public bool RemitUseBusiness { get; set; }
        public string Status { get; set; }
        [Display(Name = "VendorTerms|" + LocalizeResourceSetConstants.VendorDetails)]
        public string Terms { get; set; }
        [Display(Name = "GlobalType|" + LocalizeResourceSetConstants.Global)]
        public string Type { get; set; }
         [Display(Name = "VendorWebsite|" + LocalizeResourceSetConstants.VendorDetails)]
        [RegularExpression(@"(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)?[a-zA-Z0-9]+([\-\.]{1}[a-zA-Z0-9]+)*\.[a-zA-Z]{2,5}(:[0-9]{1,5})?(\/.*)?$", ErrorMessage = "VendorWebErrMsg|" + LocalizeResourceSetConstants.VendorDetails)]
        public string Website { get; set; }
        [Range(typeof(bool), "true", "true",ErrorMessage = "globalContractorErrMsg|" + LocalizeResourceSetConstants.Global)]
        //public bool Contractor { get; set; }
        public IEnumerable<SelectListItem> LookupTypeList { get; set; }
        public IEnumerable<SelectListItem> LookupFOBList { get; set; }
        public IEnumerable<SelectListItem> LookupTermList { get; set; }
    }
}