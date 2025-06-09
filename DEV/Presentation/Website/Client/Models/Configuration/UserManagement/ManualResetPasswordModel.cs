using Client.CustomValidation;
using Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace Client.Models.Configuration.UserManagement
{
    public class ManualResetPasswordModel
    {
        public long UserInfoId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
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
        [Compare("Password", ErrorMessage = "UserPasswordMismatchErrorMessage|" + LocalizeResourceSetConstants.UserDetails)]
        public string ConfirmPassword { get; set; }
        public string EmailAddress { get; set; }
        public long ClientId { get; set; }
        public long SiteId { get; set; }
        public long PersonnelId { get; set; }
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