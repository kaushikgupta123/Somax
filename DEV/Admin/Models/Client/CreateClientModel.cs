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
    public class CreateClientModel
    {
        public long ClientId { get; set; }

        [Required(ErrorMessage = "CompanyNameErrorMessage|" + LocalizeResourceSetConstants.ClientDetails)]
        [Remote("CheckIfClientNameExist", "Clients", HttpMethod = "POST", AdditionalFields = "ClientId", ErrorMessage = "3205|" + LocalizeResourceSetConstants.StoredProcValidation)]
        public string Name { get; set; }
        [Required(ErrorMessage = "ClientLegalNameErrorMessage|" + LocalizeResourceSetConstants.ClientDetails)]
        public string LegalName { get; set; }
        [Required(ErrorMessage = "ClientContactErrorMessage|" + LocalizeResourceSetConstants.ClientDetails)]
        public string PrimaryContact { get; set; }
        [EmailAddress(ErrorMessage = "ClientEmailErrorMessage|" + LocalizeResourceSetConstants.ClientDetails)]
        [Required(ErrorMessage = "EmailErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string Email { get; set; }        
        [Required(ErrorMessage = "ClientBusinessTypeErrorMessage|" + LocalizeResourceSetConstants.ClientDetails)]
        public string BusinessType { get; set; }
        [Required(ErrorMessage = "PackageLevelErrorMessage|" + LocalizeResourceSetConstants.ClientDetails)]
        public string PackageLevel { get; set; }     
        [Range(1, int.MaxValue, ErrorMessage = "MaxAppUsersErrorMessage|" + LocalizeResourceSetConstants.ClientDetails)]
        public Int32? MaxAppUsers { get; set; }
        public bool SiteControl { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "MaxWRUsersErrorMessage|" + LocalizeResourceSetConstants.ClientDetails)]
        public Int32? MaxWorkRequestUsers { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "MaxSanitUsersErrorMessage|" + LocalizeResourceSetConstants.ClientDetails)]
        public Int32? MaxSanitationUsers { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "MaxProdUsersErrorMessage|" + LocalizeResourceSetConstants.ClientDetails)]
        public Int32? MaxProdAppUsers { get; set; }
        [RequiredIf("IsAdd", false, ErrorMessage = "ClientMaxAttemptsErrorMessage|" + LocalizeResourceSetConstants.ClientDetails)]
        [Range(1, int.MaxValue, ErrorMessage = "MaxAttemptErrorMessage|" + LocalizeResourceSetConstants.ClientDetails)]
        public Int32? MaxAttempts { get; set; }
        [RequiredIf("IsAdd", false, ErrorMessage = "ClientMaxTimeOutErrorMessage|" + LocalizeResourceSetConstants.ClientDetails)]
        [Range(1, int.MaxValue, ErrorMessage = "MaxTimeoutErrorMessage|" + LocalizeResourceSetConstants.ClientDetails)]
        public Int32? MaxTimeOut { get; set; }
        [RequiredIf("IsAdd", false, ErrorMessage = "ConnectionStringErrorMessage|" + LocalizeResourceSetConstants.ClientDetails)]
        public string ConnectionString { get; set; }
        public bool APM { get; set; }
        public bool CMMS { get; set; }
        public bool Sanitation { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "MaxAppUsersErrorMessage|" + LocalizeResourceSetConstants.ClientDetails)]
        public Int32? Site_MaxAppUsers { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "MaxSanitUsersErrorMessage|" + LocalizeResourceSetConstants.ClientDetails)]
        public Int32? Site_MaxSanitationUsers { get; set; }
        [Required(ErrorMessage = "MaxSitesErrorMessage|" + LocalizeResourceSetConstants.ClientDetails)]
        [Range(1, int.MaxValue, ErrorMessage = "MaxSitesErrorMessage|" + LocalizeResourceSetConstants.ClientDetails)]
        public Int32? MaxSites { get; set; }
        [RequiredIf("IsAdd", true, ErrorMessage = "OnlyLocalizationErrorMessage|" + LocalizeResourceSetConstants.ClientDetails)]
        public string Localization { get; set; }
        [RequiredIf("IsAdd", true, ErrorMessage = "spnTimeZoneErrorMsg|" + LocalizeResourceSetConstants.ClientDetails)]
        public string TimeZone { get; set; }
        public Int64 UpdateIndex { get; set; }
        [RequiredIf("IsAdd", true, ErrorMessage = "SiteNameErrorMessage|" + LocalizeResourceSetConstants.ClientDetails)]
        public string SiteName { get; set; }
        [RequiredIf("IsAdd", true, ErrorMessage = "SiteDescriptionErrorMessage|" + LocalizeResourceSetConstants.ClientDetails)]
        public string Site_Description { get; set; }
        public IEnumerable<SelectListItem> BusinessTypeList { get; set; }
        public IEnumerable<SelectListItem> PackagelevelList { get; set; }
        public string LocalizationName { get; set; }

        public string TimeZoneName { get; set; }
        public bool IsAdd { get; set; }
    }
}