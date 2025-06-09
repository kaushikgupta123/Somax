using Client.CustomValidation;
using System.ComponentModel.DataAnnotations;

namespace Client.Models
{
    public class ResetPassword 
    {
        [Required]
        [MinLengthPassword("PWReqMinLength", "PWMinLength", ErrorMessage = "Password should have a minimum of ({0}) characters")]
        [RequireNumber("PWRequireNumber", ErrorMessage = "Password should contain a number")]
        [RequireAlphaCharacter("PWRequireAlpha", ErrorMessage = "Password should contain an alpha character")]
        [RequireMixedcaseCharacter("PWRequireMixedCase", ErrorMessage = "Password should contain both upper and lower case alpha characters")]
        [RequireSpecialcaseCharacter("PWRequireSpecialChar", ErrorMessage = "Password should contain a special character from ~ ` ! @ # $ % ^ & * ( ) + { } pipesymbol [ ] \\ : ; < > ? , . /")]
        [NoRepeatedCharacters("PWNoRepeatChar", ErrorMessage = "Password should not contain 3 or more sequential repeated characters")]
        [PasswordNotEqualUsername("PWNotEqualUserName", "UserName", ErrorMessage = "Password should not same as username")]
        public string NewPassword { get; set; }
        [Required]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string TempPassword { get; set; }
        [Required]
        public string SecurityAnswer { get; set; }
        public string PasswordCode { get; set; }
        public string UserName { get; set; }
        [Required]
        public string SecurityQuestion { get; set; }
        //public IEnumerable<SelectListItem> SecurityQuestList { get; set; }
        //----V2-491 password setting Fields
        public bool PWReqMinLength { get; set; }
        public int PWMinLength { get; set; }
        public bool PWRequireNumber { get; set; }
        public bool PWRequireAlpha { get; set; }
        public bool PWRequireMixedCase { get; set; }
        public bool PWRequireSpecialChar { get; set; }
        public bool PWNoRepeatChar { get; set; }
        public bool PWNotEqualUserName { get; set; }
        //---- password setting Fields
    }
}