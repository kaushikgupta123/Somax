using Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace Client.Models.Configuration.UserManagement
{
    public class ResetPasswordModel
    {
        public long UserInfoId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        //[Required(ErrorMessage = "UserPasswordErrorMessage|" + LocalizeResourceSetConstants.UserDetails)]        
        public string Password { get; set; }
        [Required(ErrorMessage = "UserReEnterPasswordErrorMessage|" + LocalizeResourceSetConstants.UserDetails)]
        [Compare("Password", ErrorMessage = "UserPasswordMismatchErrorMessage|" + LocalizeResourceSetConstants.UserDetails)]
        public string ConfirmPassword { get; set; }
        public string EmailAddress { get; set; }
    }
}