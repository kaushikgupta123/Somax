using Admin.Common;
using Admin.CustomValidation;
using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace Admin.Models.Client
{
    public class ClientModel
    {
        [Display(Name = "spnClientId|" + LocalizeResourceSetConstants.ClientDetails)]
        public long ClientId { get; set; }

        [Display(Name = "spnName|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "ClientNameErrorMessage|" + LocalizeResourceSetConstants.ClientDetails)]
        [Remote("CheckIfClientNameExist", "Client", HttpMethod = "POST", AdditionalFields = "ClientId", ErrorMessage = "3205|" + LocalizeResourceSetConstants.StoredProcValidation)]
        public string Name { get; set; }
        [Display(Name = "spnLegalName|" + LocalizeResourceSetConstants.ClientDetails)]
        [Required(ErrorMessage = "ClientLegalNameErrorMessage|" + LocalizeResourceSetConstants.ClientDetails)]
        public string LegalName { get; set; }
        [Display(Name = "spnContact|" + LocalizeResourceSetConstants.ClientDetails)]
        [Required(ErrorMessage = "ClientContactErrorMessage|" + LocalizeResourceSetConstants.ClientDetails)]
        public string Contact { get; set; }
        [Display(Name = "spnEmail|" + LocalizeResourceSetConstants.Global)]
        [EmailAddress(ErrorMessage = "ClientEmailErrorMessage|" + LocalizeResourceSetConstants.ClientDetails)]
        [Required(ErrorMessage = "EmailErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string Email { get; set; }        
        [Display(Name = "spnBusinessType|" + LocalizeResourceSetConstants.ClientDetails)]
        public string BusinessType { get; set; }
        [Display(Name = "spnPackageLevel|" + LocalizeResourceSetConstants.ClientDetails)]
        [Required(ErrorMessage = "PackageLevelErrorMessage|" + LocalizeResourceSetConstants.ClientDetails)]
        public string PackageLevel { get; set; }
        public DateTime? CreateDate { get; set; }
        [Display(Name = "spnStatus|" + LocalizeResourceSetConstants.ClientDetails)]
        [Required(ErrorMessage = "StatusErrorMessage|" + LocalizeResourceSetConstants.ClientDetails)]
        public string Status { get; set; }
        public int NumberOfEmployees { get; set; }
        [Display(Name = "spnAnnualSales|" + LocalizeResourceSetConstants.ClientDetails)]
        public Int64? AnnualSales { get; set; }
        public string TaxIDNumber { get; set; }
        public string VATNumber { get; set; }
        [Display(Name = "spnWebsite|" + LocalizeResourceSetConstants.ClientDetails)]
        public string Website { get; set; }
        public DateTime? DateEstablished { get; set; }
        public Int32 NumberOfLocations { get; set; }
        public string OfficerName { get; set; }
        public string OfficerPhone { get; set; }
        public string DunnsNumber { get; set; }
        [Display(Name = "spnAppUsers|" + LocalizeResourceSetConstants.ClientDetails)]
        public Int32? AppUsers { get; set; }
        [Display(Name = "spnMaxAppUsers|" + LocalizeResourceSetConstants.ClientDetails)]     
        [RequiredIf("SiteControl", false, ErrorMessage = "MaxAppUsersErrorMessage|" + LocalizeResourceSetConstants.ClientDetails)]
        [Range(1, int.MaxValue, ErrorMessage = "MaxAppUsersErrorMessage|" + LocalizeResourceSetConstants.ClientDetails)]
        public Int32? MaxAppUsers { get; set; }
        [Display(Name = "spnLimitedUsers|" + LocalizeResourceSetConstants.ClientDetails)]
        public Int32? LimitedUsers { get; set; }
        [Display(Name = "spnMaxLimitedUsers|" + LocalizeResourceSetConstants.ClientDetails)]
        [RequiredIf("SiteControl", false, ErrorMessage = "MaxLimitedUsersErrorMessage|" + LocalizeResourceSetConstants.ClientDetails)]
        public Int32? MaxLimitedUsers { get; set; }
        public Int32? PhoneUsers { get; set; }
        public Int32? MaxPhoneUsers { get; set; }
        [Display(Name = "spnWorkRequestUsers|" + LocalizeResourceSetConstants.ClientDetails)]
        public Int32? WorkRequestUsers { get; set; }
        [Display(Name = "spnMaxWorkRequestUsers|" + LocalizeResourceSetConstants.ClientDetails)]
        public Int32? MaxWorkRequestUsers { get; set; }
        [Display(Name = "spnSiteControl|" + LocalizeResourceSetConstants.ClientDetails)]
        public bool SiteControl { get; set; }
        public bool Purchasing { get; set; }
        public bool Sanitation { get; set; }
        public Int32? SanitationUsers { get; set; }
        public Int32? MaxSanitationUsers { get; set; }
        [Display(Name = "spnSuperUsers|" + LocalizeResourceSetConstants.ClientDetails)]
        public Int32? SuperUsers { get; set; }
        [Display(Name = "spnMaxSuperUsers|" + LocalizeResourceSetConstants.ClientDetails)]
        public Int32? MaxSuperUsers { get; set; }
        public string PrimarySICCode { get; set; }
        public string NAICSCode { get; set; }
        [Display(Name = "spnSiteGlobal|" + LocalizeResourceSetConstants.Global)]
        public Int32? Sites { get; set; }
        [Display(Name = "spnMaxSites|" + LocalizeResourceSetConstants.ClientDetails)]
        [Required(ErrorMessage = "MaxSitesErrorMessage|" + LocalizeResourceSetConstants.ClientDetails)]
        [Range(1, int.MaxValue, ErrorMessage = "MaxSitesErrorMessage|" + LocalizeResourceSetConstants.ClientDetails)]
        public Int32? MaxSites { get; set; }
        public string MinorityStatus { get; set; }
        [Display(Name = "spnLocalization|" + LocalizeResourceSetConstants.ClientDetails)]
        [Required(ErrorMessage = "LocalizationErrorMessage|" + LocalizeResourceSetConstants.ClientDetails)]
        public string Localization { get; set; }
        [Display(Name = "spnLocalizationLocation|" + LocalizeResourceSetConstants.ClientDetails)]
        public string LocalizationLocation { get; set; }
        [Display(Name = "spnLocalizationCompany|" + LocalizeResourceSetConstants.ClientDetails)]
        public string LocalizationCompany { get; set; }
        [Display(Name = "spnLocalizationHierarchicalLevel1|" + LocalizeResourceSetConstants.ClientDetails)]
        public string LocalizationHierarchicalLevel1 { get; set; }
        [Display(Name = "spnLocalizationHierarchicalLevel2|" + LocalizeResourceSetConstants.ClientDetails)]
        public string LocalizationHierarchicalLevel2 { get; set; }
        [Display(Name = "spnDefaultTimeZone|" + LocalizeResourceSetConstants.ClientDetails)]
        [Required(ErrorMessage = "DefaultTimeZoneErrorMessage|" + LocalizeResourceSetConstants.ClientDetails)]
        public string DefaultTimeZone { get; set; }
        [Display(Name = "spnDefaultCustomerManager|" + LocalizeResourceSetConstants.ClientDetails)]
        [Required(ErrorMessage = "DefaultCustomerManagerErrorMessage|" + LocalizeResourceSetConstants.ClientDetails)]
        public string DefaultCustomerManager { get; set; }
        [Display(Name = "spnMaxAttempts|" + LocalizeResourceSetConstants.ClientDetails)]
        public Int32? MaxAttempts { get; set; }
        [Display(Name = "spnMaxTimeOut|" + LocalizeResourceSetConstants.ClientDetails)]
        public Int32? MaxTimeOut { get; set; }
        [Display(Name = "spnConnectionString|" + LocalizeResourceSetConstants.ClientDetails)]
        [Required(ErrorMessage = "ConnectionStringErrorMessage|" + LocalizeResourceSetConstants.ClientDetails)]
        public string ConnectionString { get; set; }
        public Int32 TabletUsers { get; set; }
        public Int32 MaxTabletUsers { get; set; }
        [Display(Name = "spnUIConfigurationCompany|" + LocalizeResourceSetConstants.ClientDetails)]
        [Required(ErrorMessage = "ClientUIConfigurationErrorMessage|" + LocalizeResourceSetConstants.ClientDetails)]
        public string UIConfiguration { get; set; }
        [Display(Name = "spnUIConfigurationLocation|" + LocalizeResourceSetConstants.ClientDetails)]
        public string UIConfigurationLocation { get; set; }
        [Display(Name = "spnUIConfigurationHierarchicalLevel1|" + LocalizeResourceSetConstants.ClientDetails)]
        public string UIConfigurationHierarchicalLevel1 { get; set; }
        [Display(Name = "spnUIConfigurationHierarchicalLevel2|" + LocalizeResourceSetConstants.ClientDetails)]
        public string UIConfigurationHierarchicalLevel2 { get; set; }
        public string WOPrintMessage { get; set; }
        public bool PMLibCopy { get; set; }
        public bool AssetTree { get; set; }
        public int ProdAppUsers { get; set; }
        public int MaxProdAppUsers { get; set; }
        public Int64 UpdateIndex { get; set; }
        public bool InactiveFlag { get; set; }
        public IEnumerable<SelectListItem> BusinessTypeList { get; set; }
        public IEnumerable<SelectListItem> PackagelevelList { get; set; }
        public string LocalizationName { get; set; }
        public string TimeZoneName { get; set; }
        #region V2-964
        public string OfficerTitle { get; set; }
        public string PurchaseTermsandConds { get; set; }
        //public string CreateBy { get; set; }
        #endregion
    }
}