using System.ComponentModel.DataAnnotations;

namespace Admin.Models.UserProfile
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
        public string UserProfileCurrentPassword { get; set; }
        public string UserProfileNewPassword { get; set; }
        [Compare("UserProfileNewPassword")]
        public string UserProfileConfirmPassword { get; set; }
        public long UserInfoUpdateIndex { get; set; }
        public long LoginInfoUpdateIndex { get; set; }

    }
}