using Client.CustomValidation;
using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.Configuration.UserManagement
{
    public class UserSiteModel
    {       
        public long ClientId { get; set; }
        public long PersonnelId { get; set; }
        public long UserInfoId { get; set; }

        [Required(ErrorMessage = "GlobalNameSelect|" + LocalizeResourceSetConstants.Global)]
       
        public long SiteId { get; set; }
     
        public string ClientLookupId { get; set; }
        [Display(Name = "Craft")]

        [RequiredIf("IsUserTypeWorkRequest", true,ErrorMessage = "validationSelectCraft|" + LocalizeResourceSetConstants.Global)]
        public long CraftId { get; set; }

        public bool Buyer { get; set; }

        public IEnumerable<SelectListItem> SiteList { get; set; }
        public IEnumerable<SelectListItem> LookupCraftList { get; set; }
        public List<string> ErrorMessages { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string UserType { get; set; }
        public bool IsSuperUser { get; set; }
        public bool SiteControlled { get; set; }
        public bool IsUserTypeWorkRequest { get; set; }
    }
}