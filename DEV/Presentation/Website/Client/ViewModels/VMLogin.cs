using SomaxMVC.Models;

namespace SomaxMVC.ViewModels
{
    public class VMLogin : ViewModelBase
    {
        public UserInfoDetails UserInfoDetails { get; set; }
        public bool IsAuthenticated { get; set; }
        public bool IsMatchTempPassword { get; set; }
        public bool UIVersionRedirectToV1 { get; set; }
        public bool IsResetPassword { get; set; }
        public int PWExpiresDays { get; set; }
        public bool PWReqExpiration { get; set; }
        //V2-491
        public bool IsPasswordExpired { get; set; }
        public int ExpiresDaysCount { get; set; }
        public string UserNameEncript { get; set; }
        public string EmailEncript { get; set; }
    }
}