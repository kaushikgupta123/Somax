using Client.CustomValidation;
using SomaxMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models
{
    public class CreatePassword : ViewModelBase
    {
        [Required]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please enter new password")]
        [MinLengthPassword("PWReqMinLength", "PWMinLength", ErrorMessage = "Password should have a minimum of ({0}) characters")]
        [RequireNumber("PWRequireNumber", ErrorMessage = "Password should contain a number")]
        [RequireAlphaCharacter("PWRequireAlpha", ErrorMessage = "Password should contain an alpha character")]
        [RequireMixedcaseCharacter("PWRequireMixedCase", ErrorMessage = "Password should contain both upper and lower case alpha characters")]
        [RequireSpecialcaseCharacter("PWRequireSpecialChar", ErrorMessage = "Password should contain a special character from ~ ` ! @ # $ % ^ & * ( ) + { } pipesymbol [ ] \\ : ; < > ? , . /")]
        [NoRepeatedCharacters("PWNoRepeatChar", ErrorMessage = "Password should not contain 3 or more sequential repeated characters")]
        [PasswordNotEqualUsername("PWNotEqualUserName", "UserName", ErrorMessage = "Password should not same as username")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Please enter confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "Password and confirm password does not match")]
        public string ConfirmPassword { get; set; }
        [RequiredIfValueExist("UserMail", ErrorMessage = "Please select security question")]
        public string SecurityQuestion { get; set; }
        [RequiredIfValueExist("UserMail", ErrorMessage = "Please enter security answer")]
        public string SecurityAnswer { get; set; }
        public string TempPassword { get; set; }
        public string PasswordCode { get; set; }
        public DateTime? ResetPasswordRequestDate { get; set; }
        public string UserMail { get; set; }
        public bool IsResetPassword { get; set; }
        public IEnumerable<SelectListItem> SecurityQuestList { get; set; }

        #region V2-491
        public bool IsPasswordExpired { get; set; }
        public long UserInfoId { get; set; }
        public long LoginInfoId { get; set; }
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