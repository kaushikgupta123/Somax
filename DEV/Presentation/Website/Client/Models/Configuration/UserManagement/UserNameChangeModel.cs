using Client.CustomValidation;
using Common.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;


namespace Client.Models.Configuration.UserManagement
{
    public class UserNameChangeModel
    {
        public long UserInfoId { get; set; }

        //public long SiteId { get; set; }

        //public long Personnelid { get; set; }
        public long? DefaultSiteId { get; set; }
        public long? DefaultPersonnelId { get; set; }

        [Required(ErrorMessage = "UserNameErrorMessage|" + LocalizeResourceSetConstants.UserDetails)]
        [StringLength(63, ErrorMessage = "ValidationMaxLength63UserName|" + LocalizeResourceSetConstants.Global)]
        [Display(Name = "UserName|" + LocalizeResourceSetConstants.Global)]
        [Remote("CheckIfUserExistForUserNameChange", "UserManagement", HttpMethod = "POST", ErrorMessage = "504|" + LocalizeResourceSetConstants.StoredProcValidation)]
        public string UserName { get; set; }

       // public string OldUserName { get; set; }

        //public string Password { get; set; }

        public List<string> ErrorMessages { get; set; }

        [Required(ErrorMessage = "UserPasswordErrorMessage|" + LocalizeResourceSetConstants.UserDetails)]
        [MinLengthPassword("PWReqMinLength", "PWMinLength", ErrorMessage = "ValidationPasswordMinimumCharacters|" + LocalizeResourceSetConstants.Global)] 
        [RequireNumber("PWRequireNumber", ErrorMessage = "ValidationPasswordContainNumber|" + LocalizeResourceSetConstants.Global)]
        [RequireAlphaCharacter("PWRequireAlpha", ErrorMessage = "ValidationPasswordContainAlphaCharacter|" + LocalizeResourceSetConstants.Global)]
        [RequireMixedcaseCharacter("PWRequireMixedCase", ErrorMessage = "ValidationPasswordContainUpperLowerAlphaCharacter|" + LocalizeResourceSetConstants.Global)]
        [RequireSpecialcaseCharacter("PWRequireSpecialChar", ErrorMessage = "ValidationPasswordContainSpecialChFrom|" + LocalizeResourceSetConstants.Global)]
        [NoRepeatedCharacters("PWNoRepeatChar", ErrorMessage = "ValidationPasswordNotContainThreeOrMoreSequentialRepeatCh|" + LocalizeResourceSetConstants.Global)]
        [PasswordNotEqualUsername("PWNotEqualUserName", "UserName", ErrorMessage = "ValidationPasswordNotSameAsUserName|" + LocalizeResourceSetConstants.Global)]
        public string Password { get; set; }
        [Required(ErrorMessage = "UserReEnterPasswordErrorMessage|" + LocalizeResourceSetConstants.UserDetails)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "UserPasswordMismatchErrorMessage|" + LocalizeResourceSetConstants.UserDetails)]
        public string ConfirmPassword { get; set; }

        #region V2-491
        //---- password setting Fields
        public bool PWReqMinLength { get; set; }
        public int PWMinLength { get; set; }
        public bool PWRequireNumber { get; set; }
        public bool PWRequireAlpha { get; set; }
        public bool PWRequireMixedCase { get; set; }
        public bool PWRequireSpecialChar { get; set; }
        public bool PWNoRepeatChar { get; set; }
        public bool PWNotEqualUserName { get; set; }
        //---- password setting Fields
        #endregion
    }
}