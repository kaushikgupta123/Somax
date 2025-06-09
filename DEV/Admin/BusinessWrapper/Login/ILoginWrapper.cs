using Admin.Models;
using Business.Authentication;
using DataContracts;
using System;

namespace Admin.BusinessWrapper
{
    public interface ILoginWrapper
    {
        VMLogin GetLogIn(UserInfoDetails objUser);
        bool SendForgetUserIdByMail(string mailId);
        bool SendForgetPasswordResend(string userId);
        LoginInfo ResetPasswordData(Guid code);
        bool ResetPassword(ResetPassword resetPassword);
        AccountPassword GetUserDataForPasswordCreate(string userId);
        string CreateNewPassword(CreatePassword createPassword);
    }
}
