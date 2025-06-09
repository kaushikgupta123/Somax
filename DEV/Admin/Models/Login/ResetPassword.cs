using System.ComponentModel.DataAnnotations;

namespace Admin.Models
{
    public class ResetPassword 
    {
        [Required]
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
    }
}