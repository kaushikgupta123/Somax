using Client.Common;
using Client.CustomValidation;
using Common.Constants;
using DataContracts;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models
{
    public class VendorsModel
    {
        public VendorsModel()
        {
            _VendorSummaryModel = new VendorSummaryModel();
            VendorData = new DataContracts.Vendor();
        }
        public VendorSummaryModel _VendorSummaryModel { get; set; }
        public DataContracts.Vendor VendorData { get; set; }
        public long ClientId { get; set; }
        [Display(Name = "VendorId|" + LocalizeResourceSetConstants.VendorDetails)]
        public long VendorId { get; set; }
        [Required(ErrorMessage = "VendorIdErrMsg|" + LocalizeResourceSetConstants.VendorDetails)]
        [RegularExpression("^[A-Z0-9\\%\\-\\:\\/\\$\\*\\+\\.]+$", ErrorMessage = "VendorIdRegErrMsg|" + LocalizeResourceSetConstants.VendorDetails)]
        public string ClientLookupId { get; set; }
        [Display(Name = "spnName|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "VendorNameErrMsg|" + LocalizeResourceSetConstants.VendorDetails)]
        [RegularExpression("^[A-Za-z0-9 _]*[A-Za-z0-9][A-Za-z0-9 &amp()-_]*$", ErrorMessage = "VendorNameRegErrMsg|" + LocalizeResourceSetConstants.VendorDetails)]
        public string Name { get; set; }
        [Display(Name = "VendorAddress|" + LocalizeResourceSetConstants.VendorDetails)]
        [RequiredAsPerUI(UiConfigConstants.VendorAdd, UiConfigConstants.VendorEdit, "VendorId", "0", "IsExternal", ErrorMessage = "FirstLineAddressErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string Address1 { get; set; }
        [RequiredAsPerUI(UiConfigConstants.VendorAdd, UiConfigConstants.VendorEdit, "VendorId", "0", "IsExternal", ErrorMessage = "SecondLineAddressErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string Address2 { get; set; }
        [RequiredAsPerUI(UiConfigConstants.VendorAdd, UiConfigConstants.VendorEdit, "VendorId", "0", "IsExternal", ErrorMessage = "ThirdLineAddressErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string Address3 { get; set; }
        [Display(Name = "VendorCity|" + LocalizeResourceSetConstants.VendorDetails)]
        [RequiredAsPerUI(UiConfigConstants.VendorAdd, UiConfigConstants.VendorEdit, "VendorId", "0", "IsExternal", ErrorMessage = "CityErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string AddressCity { get; set; }
        [Display(Name = "VendorState|" + LocalizeResourceSetConstants.VendorDetails)]
        [RequiredAsPerUI(UiConfigConstants.VendorAdd, UiConfigConstants.VendorEdit, "VendorId", "0", "IsExternal", ErrorMessage = "StateErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string AddressState { get; set; }
        [Display(Name = "VendorPostalCode|" + LocalizeResourceSetConstants.VendorDetails)]
        [RequiredAsPerUI(UiConfigConstants.VendorAdd, UiConfigConstants.VendorEdit, "VendorId", "0", "IsExternal", ErrorMessage = "PostalCodeErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string PostalCode { get; set; }
        [Display(Name = "VendorAddress|" + LocalizeResourceSetConstants.VendorDetails)]
        [RequiredAsPerUI(UiConfigConstants.VendorAdd, UiConfigConstants.VendorEdit, "VendorId", "0", "IsExternal", ErrorMessage = "FirstLineAddressErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string RemitAddress1 { get; set; }
        [RequiredAsPerUI(UiConfigConstants.VendorAdd, UiConfigConstants.VendorEdit, "VendorId", "0", "IsExternal", ErrorMessage = "SecondLineAddressErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string RemitAddress2 { get; set; }
        [RequiredAsPerUI(UiConfigConstants.VendorAdd, UiConfigConstants.VendorEdit, "VendorId", "0", "IsExternal", ErrorMessage = "ThirdLineAddressErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string RemitAddress3 { get; set; }
        [Display(Name = "VendorCity|" + LocalizeResourceSetConstants.VendorDetails)]
        [RequiredAsPerUI(UiConfigConstants.VendorAdd, UiConfigConstants.VendorEdit, "VendorId", "0", "IsExternal", ErrorMessage = "CityErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string RemitAddressCity { get; set; }
        [Display(Name = "VendorState|" + LocalizeResourceSetConstants.VendorDetails)]
        [RequiredAsPerUI(UiConfigConstants.VendorAdd, UiConfigConstants.VendorEdit, "VendorId", "0", "IsExternal", ErrorMessage = "StateErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string RemitAddressState { get; set; }
        [Display(Name = "VendorPostalCode|" + LocalizeResourceSetConstants.VendorDetails)]
        [RequiredAsPerUI(UiConfigConstants.VendorAdd, UiConfigConstants.VendorEdit, "VendorId", "0", "IsExternal", ErrorMessage = "PostalCodeErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string RemitPostalCode { get; set; }
        [Display(Name = "VendorCountry|" + LocalizeResourceSetConstants.VendorDetails)]
        [RequiredAsPerUI(UiConfigConstants.VendorAdd, UiConfigConstants.VendorEdit, "VendorId", "0", "IsExternal", ErrorMessage = "CountryErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string RemitCountry { get; set; }
        [Display(Name = "GlobalType|" + LocalizeResourceSetConstants.Global)]
        [RequiredAsPerUI(UiConfigConstants.VendorAdd, UiConfigConstants.VendorEdit, "VendorId", "0", "IsExternal", ErrorMessage = "TypeErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string Type { get; set; }
        [Display(Name = "VendorTerms|" + LocalizeResourceSetConstants.VendorDetails)]
        [RequiredAsPerUI(UiConfigConstants.VendorAdd, UiConfigConstants.VendorEdit, "VendorId", "0", "IsExternal", ErrorMessage = "TermsErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string Terms { get; set; }
        [Display(Name = "VendorFOBCode|" + LocalizeResourceSetConstants.VendorDetails)]
        [RequiredAsPerUI(UiConfigConstants.VendorAdd, UiConfigConstants.VendorEdit, "VendorId", "0", "IsExternal", ErrorMessage = "FOBErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string FOBCode { get; set; }
        public bool RemitUseBusiness { get; set; }

        public IEnumerable<SelectListItem> LookupTypeList { get; set; }
        public IEnumerable<SelectListItem> LookupFOBList { get; set; }
        public IEnumerable<SelectListItem> LookupTermList { get; set; }

        [Display(Name = "VendorCusAcc|" + LocalizeResourceSetConstants.VendorDetails)]
        [RequiredAsPerUI(UiConfigConstants.VendorAdd, UiConfigConstants.VendorEdit, "VendorId", "0", "IsExternal", ErrorMessage = "CustomerAccountErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string CustomerAccount { get; set; }
        [Display(Name = "VendorEmail|" + LocalizeResourceSetConstants.VendorDetails)]
        //[Required(ErrorMessage = "VendorEmailErrMsg|" + LocalizeResourceSetConstants.VendorDetails)]
        [EmailAddress(ErrorMessage = "VendorEmailErrMsg|" + LocalizeResourceSetConstants.VendorDetails)]
        [RequiredAsPerUI(UiConfigConstants.VendorAdd, UiConfigConstants.VendorEdit, "VendorId", "0", "IsExternal", ErrorMessage = "EmailErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string Email { get; set; }
        [Display(Name = "VendorWebsite|" + LocalizeResourceSetConstants.VendorDetails)]
        [RegularExpression(@"(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)?[a-zA-Z0-9]+([\-\.]{1}[a-zA-Z0-9]+)*\.[a-zA-Z]{2,5}(:[0-9]{1,5})?(\/.*)?$", ErrorMessage = "VendorWebErrMsg|" + LocalizeResourceSetConstants.VendorDetails)]
        [RequiredAsPerUI(UiConfigConstants.VendorAdd, UiConfigConstants.VendorEdit, "VendorId", "0", "IsExternal", ErrorMessage = "WebErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string Website { get; set; }
        [Display(Name = "VendorPhNo|" + LocalizeResourceSetConstants.VendorDetails)]
        [RegularExpression(@"^[\+0-9()-]*$", ErrorMessage = "VendorPhoneNoErrMsg|" + LocalizeResourceSetConstants.VendorDetails)]
        [RequiredAsPerUI(UiConfigConstants.VendorAdd, UiConfigConstants.VendorEdit, "VendorId", "0", "IsExternal", ErrorMessage = "PhNoErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string PhoneNumber { get; set; }
        [Display(Name = "VendorFax|" + LocalizeResourceSetConstants.VendorDetails)]
        [RequiredAsPerUI(UiConfigConstants.VendorAdd, UiConfigConstants.VendorEdit, "VendorId", "0", "IsExternal", ErrorMessage = "FaxErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string Fax { get; set; }
        public string AssetNumber { get; set; }
        [Display(Name = "VendorCountry|" + LocalizeResourceSetConstants.VendorDetails)]
        [RequiredAsPerUI(UiConfigConstants.VendorAdd, UiConfigConstants.VendorEdit, "VendorId", "0", "IsExternal", ErrorMessage = "CountryErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string Country { get; set; }
        [Display(Name = "globalInActive|" + LocalizeResourceSetConstants.Global)]
        public bool InactiveFlag { get; set; }
        public bool IsVendorAddFromUpperMenu { get; set; }
        [Display(Name = "AutoEmailPO|" + LocalizeResourceSetConstants.VendorDetails)]
        public bool AutoEmailPO { get; set; }
        public bool ExVendorStat { get; set; }
        public int TotalCount { get; set; }
        //-----V2-375-----
        public bool Hidden { get; set; }
        public bool Required { get; set; }
        [Display(Name = "External")]
        public bool IsExternal { get; set; }
        public bool Disabled { get; set; }
        public string ViewName { get; set; }
        public IEnumerable<SelectListItem> ExternalTypeList { get; set; }
        public long VendorMasterId { get; set; }

        public bool PunchoutIndicator { get; set; }

        [RequiredAsPerUI(UiConfigConstants.VendorAdd, UiConfigConstants.VendorEdit, "VendorId", "0", "IsExternal", ErrorMessage = "spnValidationselectVendorToDomain|" + LocalizeResourceSetConstants.Global)]
        public string VendorDomain { get; set; }

        [RequiredAsPerUI(UiConfigConstants.VendorAdd, UiConfigConstants.VendorEdit, "VendorId", "0", "IsExternal", ErrorMessage = "spnValidationVendorToIdentity|" + LocalizeResourceSetConstants.Global)]   
        public string VendorIdentity { get; set; }

        [RequiredAsPerUI(UiConfigConstants.VendorAdd, UiConfigConstants.VendorEdit, "VendorId", "0", "IsExternal", ErrorMessage = "spnValidSharedSecret|" + LocalizeResourceSetConstants.Global)]
        public string SharedSecret { get; set; }

        [RequiredAsPerUI(UiConfigConstants.VendorAdd, UiConfigConstants.VendorEdit, "VendorId", "0", "IsExternal", ErrorMessage = "spValidSenderFromDomain|" + LocalizeResourceSetConstants.Global)] 
        public string SenderDomain { get; set; }

        [RequiredAsPerUI(UiConfigConstants.VendorAdd, UiConfigConstants.VendorEdit, "VendorId", "0", "IsExternal", ErrorMessage = "spnValidSenderFromIdentity|" + LocalizeResourceSetConstants.Global)] 
        public string SenderIdentity { get; set; }

        [Url(ErrorMessage = "spnValidUrl|" + LocalizeResourceSetConstants.Global)]        
        [RequiredAsPerUI(UiConfigConstants.VendorAdd, UiConfigConstants.VendorEdit, "VendorId", "0", "IsExternal", ErrorMessage = "spnVaildPunchOutURL|" + LocalizeResourceSetConstants.Global)] 
        public string PunchoutURL { get; set; }
        public bool AutoSendPunchOutPO { get; set; }
        //V2-582
        [Url(ErrorMessage = "spnValidUrl|" + LocalizeResourceSetConstants.Global)]
        [RequiredAsPerUI(UiConfigConstants.VendorAdd, UiConfigConstants.VendorEdit, "VendorId", "0", "IsExternal", ErrorMessage = "SendPunchoutPOURLErrMsg|" + LocalizeResourceSetConstants.VendorDetails)]

        public string SendPunchoutPOURL { get; set; }
        //V2-587
        [EmailAddress(ErrorMessage = "ValidSendPunchoutPOEmailErrMsg|" + LocalizeResourceSetConstants.VendorDetails)]
        [RequiredAsPerUI(UiConfigConstants.VendorAdd, UiConfigConstants.VendorEdit, "VendorId", "0", "IsExternal", ErrorMessage = "SendPunchoutPOEmailErrMsg|" + LocalizeResourceSetConstants.VendorDetails)]

        public string SendPunchoutPOEmail { get; set; }
        public IEnumerable<SelectListItem> VendorDomainList { get; set; }
        public IEnumerable<SelectListItem> SenderDomainList { get; set; }

        public bool VendorConfigurePunchOutSecurity { get; set; }

        public bool IsSitePunchOut { get; set; }
        public bool EmailConfigurationCriteriaOnInterfaceProp { get; set; }
    }

}