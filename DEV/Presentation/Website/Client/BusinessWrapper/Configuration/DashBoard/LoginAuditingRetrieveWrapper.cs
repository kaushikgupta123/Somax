using Client.Models.Configuration;
using Database.Business;
using DataContracts;
using System.Collections.Generic;
using System.Linq;

namespace Client.BusinessWrapper
{
    public class LoginAuditingRetrieveWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        List<string> errorMessage = new List<string>();
        public LoginAuditingRetrieveWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }
        public List<LoginAuditingRetrieveUserInfoVM> GetLoginAuditingRetrieveByUserInfoId()
        {
            List<LoginAuditingRetrieveUserInfoVM> userloginlistVM = new List<LoginAuditingRetrieveUserInfoVM>();
            //RetrieveAllLoginAuditInfo lgList = new RetrieveAllLoginAuditInfo();
            LoginAuditing la = new LoginAuditing();
            la.ClientId = this.userData.DatabaseKey.Client.ClientId;
            la.UserInfoId = this.userData.DatabaseKey.User.UserInfoId;
            List<LoginAuditing> loginAuditingList = new List<LoginAuditing>();
            loginAuditingList = la.RetrieveByUserInfoId(this.userData.DatabaseKey);
            userloginlistVM = loginAuditingList.Select(s => new LoginAuditingRetrieveUserInfoVM
            {
                UserName = s.UserName,
                LogIn = s.CreateDate,
                IPAddress = s.IPAddress
            }).ToList();
            return userloginlistVM;
        }
    }
}