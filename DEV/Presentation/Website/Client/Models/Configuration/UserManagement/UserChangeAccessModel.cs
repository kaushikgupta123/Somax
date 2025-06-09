using Client.CustomValidation;
using Common.Constants;
using DataContracts;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace Client.Models.Configuration.UserManagement
{
    public class UserChangeAccessModel
    {
        public UserChangeAccessModel()
        {
            UserDetail = new UserDetails();
        }
        public UserDetails UserDetail { get; set; }
        [Display(Name = "UserInfoId|" + LocalizeResourceSetConstants.UserDetails)]
        public long UserInfoId { get; set; }
        [Display(Name = "UserType|" + LocalizeResourceSetConstants.UserDetails)]
        public string UserType { get; set; }
        //[RequiredSecurityProfileForUser("UserType", ErrorMessage = "UserSecurityProfileerrorMessage|" + LocalizeResourceSetConstants.UserDetails)]
        [Display(Name = "SecurityProfile|" + LocalizeResourceSetConstants.UserDetails)]
        public long? SecurityProfileId { get; set; }
        public UserData _userdata { get; set; }
        public int ProductGrouping { get; set; }
        public string PackageLevel { get; set; }

        [Required(ErrorMessage = "GlobalUserAccessSelect|" + LocalizeResourceSetConstants.Global)]
        public string SecurityProfileName { get; set; }      
        public long? DefaultSiteId { get; set; }

        public bool IsSuperUser { get; set; }

        public List<string> ErrorMessages { get; set; }

        public long UserUpdateIndex { get; set; }

        public bool CMMS { get; set; }
        public bool APM { get; set; }
        public bool Sanitation { get; set; }
        public bool Fleet { get; set; }
        public bool Production { get; set; }
        public string OldUserType { get; set; }
        //public bool CMMSUser { get; set; }
        //public bool SanitationUser { get; set; }
    }
}