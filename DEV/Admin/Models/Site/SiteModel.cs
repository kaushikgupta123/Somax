using Admin.CustomValidation;
using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Models.Site
{
    public class SiteModel
    {
        public long SiteId { get; set; }

        public long ClientId { get; set; }

        public string ClientName { get; set; }
        [Required(ErrorMessage = "globalValidName|" + LocalizeResourceSetConstants.Global)]
        public string Name { get; set; }
        public string AddressCity { get; set; }
        public string AddressState { get; set; }
        [Required(ErrorMessage = "validationDescription|" + LocalizeResourceSetConstants.Global)]
        public string Description { get; set; }
        [Required(ErrorMessage = "validationLocalizationLanguage|" + LocalizeResourceSetConstants.Global)]
        public string Localization { get; set; }
        public string LocalizationName { get; set; }

        public string UIConfigurationLocation { get; set; }

        public string UIConfigurationCompany { get; set; }
        [Required(ErrorMessage = "validationStatus|" + LocalizeResourceSetConstants.Global)]
        public string Status { get; set; }
        [Required(ErrorMessage = "validationTimeZone|" + LocalizeResourceSetConstants.Global)]
        public string TimeZone { get; set; }

        public string TimeZoneName { get; set; }

        public bool APM { get; set; }

        public bool CMMS { get; set; }

        public bool Sanitation { get; set; }

        public bool PMLibrary { get; set; }

        public bool ClientSiteControl { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Address3 { get; set; }

        public string AddressCountry { get; set; }

        public string AddressPostCode { get; set; }

        public int AppUsers { get; set; }

        public int WorkRequestUsers { get; set; }

        public int SuperUsers { get; set; }

        public int LimitedUsers { get; set; }

        [RequiredIf("ClientSiteControl", true, ErrorMessage = "validationMaxAppUsers|" + LocalizeResourceSetConstants.Global)]
        public int? MaxAppUsers { get; set; }

        public int? MaxWorkRequestUsers { get; set; }

        public int? MaxSuperUsers { get; set; }

        [RequiredIf("ClientSiteControl", true, ErrorMessage = "validationMaxReferenceUsers|" + LocalizeResourceSetConstants.Global)]
        public int? MaxLimitedUsers { get; set; }
        public int UpdateIndex { get; set; }

        public bool UsePunchOut { get; set; }
        public IEnumerable<SelectListItem> ClientList { get; set; }

        public IEnumerable<SelectListItem> InactiveFlagList { get; set; }

        public IEnumerable<SelectListItem> LocalizationList { get; set; }

        public IEnumerable<SelectListItem> TimeZoneList { get; set; }

    }
}