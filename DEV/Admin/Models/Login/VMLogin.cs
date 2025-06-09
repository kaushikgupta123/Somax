//using SomaxMVC.Models;

using SomaxMVC.ViewModels;

namespace Admin.Models
{
    public class VMLogin : ViewModelBase
    {
        public UserInfoDetails UserInfoDetails { get; set; }
        public bool IsAuthenticated { get; set; }
        public bool IsMatchTempPassword { get; set; }
        public bool UIVersionRedirectToV1 { get; set; }
        public bool IsResetPassword { get; set; }
    }
}