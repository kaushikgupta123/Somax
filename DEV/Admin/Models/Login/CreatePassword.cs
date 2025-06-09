using Admin.CustomValidation;
using SomaxMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Admin.Models
{
    public class CreatePassword : ViewModelBase
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
        [RequiredIfValueExist("UserMail")]
        public string SecurityQuestion { get; set; }
        [RequiredIfValueExist("UserMail")]
        public string SecurityAnswer { get; set; }
        public string TempPassword { get; set; }
        public string PasswordCode { get; set; }
        public DateTime? ResetPasswordRequestDate { get; set; }
        public string UserMail { get; set; }
        public bool IsResetPassword { get; set; }
        public IEnumerable<SelectListItem> SecurityQuestList { get; set; }
    }
}