using Client.CustomValidation;
using System.ComponentModel.DataAnnotations;

namespace Client.Models.UserProfile
{
    public class UserProfileModel
    {
        [Required]
        public string UserProfileFirstName { get; set; }
        [Required]
        public string UserProfileLastName { get; set; }
        public string UserProfileMiddleName { get; set; }
        public string UserProfileEmailAddress { get; set; }
        public string UserProfileSiteName { get; set; }
        [Required]
        public string UserProfileSecurityQuestion { get; set; }
        [Required]
        public string UserProfileSecurityAnswer { get; set; }        
        [RequiredIfValueExist("UserProfileNewPassword")]
        public string UserProfileCurrentPassword { get; set; }
        [MinLengthPassword("PasswordReqMinLength", "PasswordMinLength", ErrorMessage = "Password should have a minimum of ({0}) characters")]
        [RequireNumber("PasswordRequireNumber", ErrorMessage = "Password should contain a number")]
        [RequireAlphaCharacter("PasswordRequireAlpha", ErrorMessage = "Password should contain an alpha character")]
        [RequireMixedcaseCharacter("PasswordRequireMixedCase", ErrorMessage = "Password should contain both upper and lower case alpha characters")]
        [RequireSpecialcaseCharacter("PasswordRequireSpecialChar", ErrorMessage = "Password should contain a special character from ~ ` ! @ # $ % ^ & * ( ) + { } pipesymbol [ ] \\ : ; < > ? , . /")]
        [NoRepeatedCharacters("PasswordNoRepeatChar", ErrorMessage = "Password should not contain 3 or more sequential repeated characters")]
        [PasswordNotEqualUsername("PasswordNotEqualUserName", "UserProfileUserName", ErrorMessage = "Password should not same as username")]
        public string UserProfileNewPassword { get; set; }
        [Compare("UserProfileNewPassword")]
        public string UserProfileConfirmPassword { get; set; }
        public long UserInfoUpdateIndex { get; set; }
        public long LoginInfoUpdateIndex { get; set; }

        //----V2-491 password setting Fields
        public bool PasswordReqMinLength { get; set; }
        public int PasswordMinLength { get; set; }
        public bool PasswordRequireNumber { get; set; }
        public bool PasswordRequireAlpha { get; set; }
        public bool PasswordRequireMixedCase { get; set; }
        public bool PasswordRequireSpecialChar { get; set; }
        public bool PasswordNoRepeatChar { get; set; }
        public bool PasswordNotEqualUserName { get; set; }
        //---- password setting Fields
        public string UserProfileUserName { get; set; }
    }
}