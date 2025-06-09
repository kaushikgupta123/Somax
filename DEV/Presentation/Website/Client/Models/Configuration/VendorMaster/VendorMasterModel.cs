using Client.Common;
using Client.CustomValidation;
using Common.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.Configuration.VendorMaster
{
    public class VendorMasterModel
    {
        public long ClientId {get ;set;}     
        public long VendorMasterId { get; set; }
        [Required(ErrorMessage = "ValidVendor|" + LocalizeResourceSetConstants.ConfigMasterDetail)]
        [RegularExpression("^[A-Z0-9\\%\\-\\:\\/\\$\\*\\+\\.]+$", ErrorMessage = "VendorIdRegErrMsg|" + LocalizeResourceSetConstants.VendorDetails)]
        public string ClientLookupId { get; set; }
        public long ExVendorId { get; set; }
        public long ExVendorSiteId { get; set; }
        public string ExVendorSiteCode { get; set; }
        [Required(ErrorMessage = "ValidName|" + LocalizeResourceSetConstants.ConfigMasterDetail)]
        public string Name { get; set; }
        [RequiredAsPerUI(UiConfigConstants.VendorMasterAdd, UiConfigConstants.VendorMasterEdit, "VendorMasterId", "0", "IsExternal", ErrorMessage = "TypeErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string Type { get; set; }
        [RequiredAsPerUI(UiConfigConstants.VendorMasterAdd, UiConfigConstants.VendorMasterEdit, "VendorMasterId", "0", "IsExternal", ErrorMessage = "TermsErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string Terms { get; set; }
        [RequiredAsPerUI(UiConfigConstants.VendorMasterAdd, UiConfigConstants.VendorMasterEdit, "VendorMasterId", "0", "IsExternal", ErrorMessage = "FOBErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string FOBCode { get; set; }

        [RequiredAsPerUI(UiConfigConstants.VendorMasterAdd, UiConfigConstants.VendorMasterEdit, "VendorMasterId", "0", "IsExternal", ErrorMessage = "FirstLineAddressErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string Address1 { get; set; }

        [RequiredAsPerUI(UiConfigConstants.VendorMasterAdd, UiConfigConstants.VendorMasterEdit, "VendorMasterId", "0", "IsExternal", ErrorMessage = "SecondLineAddressErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string Address2 { get; set; }
        [RequiredAsPerUI(UiConfigConstants.VendorMasterAdd, UiConfigConstants.VendorMasterEdit, "VendorMasterId", "0", "IsExternal", ErrorMessage = "ThirdLineAddressErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string Address3 { get; set; }
        [RequiredAsPerUI(UiConfigConstants.VendorMasterAdd, UiConfigConstants.VendorMasterEdit, "VendorMasterId", "0", "IsExternal", ErrorMessage = "CityErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string AddressCity { get; set; }
        [RequiredAsPerUI(UiConfigConstants.VendorMasterAdd, UiConfigConstants.VendorMasterEdit, "VendorMasterId", "0", "IsExternal", ErrorMessage = "StateErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string AddressState { get; set; }
        [RequiredAsPerUI(UiConfigConstants.VendorMasterAdd, UiConfigConstants.VendorMasterEdit, "VendorMasterId", "0", "IsExternal", ErrorMessage = "PostalCodeErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string AddressPostCode { get; set; }
        [RequiredAsPerUI(UiConfigConstants.VendorMasterAdd, UiConfigConstants.VendorMasterEdit, "VendorMasterId", "0", "IsExternal", ErrorMessage = "CountryErrMsg|"+ LocalizeResourceSetConstants.Global)]
        public string AddressCountry { get; set; }
        [RequiredAsPerUI(UiConfigConstants.VendorMasterAdd, UiConfigConstants.VendorMasterEdit, "VendorMasterId", "0", "IsExternal", ErrorMessage = "FirstLineAddressErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string RemitAddress1 { get; set; }
        [RequiredAsPerUI(UiConfigConstants.VendorMasterAdd, UiConfigConstants.VendorMasterEdit, "VendorMasterId", "0", "IsExternal", ErrorMessage = "SecondLineAddressErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string RemitAddress2 { get; set; }
        [RequiredAsPerUI(UiConfigConstants.VendorMasterAdd, UiConfigConstants.VendorMasterEdit, "VendorMasterId", "0", "IsExternal", ErrorMessage = "SecondLineAddressErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string RemitAddress3 { get; set; }
        [RequiredAsPerUI(UiConfigConstants.VendorMasterAdd, UiConfigConstants.VendorMasterEdit, "VendorMasterId", "0", "IsExternal", ErrorMessage = "CityErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string RemitAddressCity { get; set; }
        [RequiredAsPerUI(UiConfigConstants.VendorMasterAdd, UiConfigConstants.VendorMasterEdit, "VendorMasterId", "0", "IsExternal", ErrorMessage = "StateErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string RemitAddressState { get; set; }
        [RequiredAsPerUI(UiConfigConstants.VendorMasterAdd, UiConfigConstants.VendorMasterEdit, "VendorMasterId", "0", "IsExternal", ErrorMessage = "PostalCodeErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string RemitAddressPostCode { get; set; }
        [RequiredAsPerUI(UiConfigConstants.VendorMasterAdd, UiConfigConstants.VendorMasterEdit, "VendorMasterId", "0", "IsExternal", ErrorMessage = "CountryErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string RemitAddressCountry { get; set; }
        public bool RemitUseBusiness { get; set; }
        public bool InactiveFlag { get; set; }
        [RequiredAsPerUI(UiConfigConstants.VendorMasterAdd, UiConfigConstants.VendorMasterEdit, "VendorMasterId", "0", "IsExternal", ErrorMessage = "FaxErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string FaxNumber { get; set; }
        [RequiredAsPerUI(UiConfigConstants.VendorMasterAdd, UiConfigConstants.VendorMasterEdit, "VendorMasterId", "0", "IsExternal", ErrorMessage = "PhNoErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string PhoneNumber { get; set; }

        [RequiredAsPerUI(UiConfigConstants.VendorMasterAdd, UiConfigConstants.VendorMasterEdit, "VendorMasterId", "0", "IsExternal", ErrorMessage = "EmailErrMsg|" + LocalizeResourceSetConstants.Global)]
        [EmailAddress(ErrorMessage = "ValidEmail|" + LocalizeResourceSetConstants.ConfigMasterDetail)]
        public string EmailAddress { get; set; }
        public long UpdateIndex { get; set; }

        public IEnumerable<SelectListItem> TermsList { get; set; }
        public IEnumerable<SelectListItem> FobCodeList { get; set; }
        public IEnumerable<SelectListItem> TypeList { get; set; }
        //-----V2-375-----
        public bool Hidden { get; set; }
        public bool Required { get; set; }
        [Display(Name = "External")]
        public bool IsExternal { get; set; }
        public bool Disabled { get; set; }
        public string ViewName { get; set; }
        public IEnumerable<SelectListItem> ExternalTypeList { get; set; }
    }
}