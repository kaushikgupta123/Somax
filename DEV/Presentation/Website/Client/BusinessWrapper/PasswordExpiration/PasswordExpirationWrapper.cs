using Client.Models;
using Common.Constants;
using DataContracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Client.ViewModels;
using Client.Common;
using Client.Models.Equipment;
using System.Threading.Tasks;
using Client.Controllers;
using Common.Enumerations;
using Client.BusinessWrapper.Common;
using Business.Authentication;
using SomaxMVC.ViewModels;

namespace Client.BusinessWrapper
{
    public class PasswordExpirationWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        List<string> errorMessage = new List<string>();

        public PasswordExpirationWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        public int GetPasswordExpirationDays()
        {
            VMLogin objVMLogin = new VMLogin();
            int daysCount = 0;
            DataContracts.PasswordSettings PS = new DataContracts.PasswordSettings();
            PS.ClientId = userData.DatabaseKey.Client.ClientId;
            PS.RetrieveByClientId(userData.DatabaseKey);

            objVMLogin.PWExpiresDays = PS.PWExpiresDays;
            objVMLogin.PWReqExpiration = PS.PWReqExpiration;
            if (objVMLogin.PWReqExpiration)
            {
                LoginInfo loginInfo = new LoginInfo()
                {
                    LoginInfoId = userData.LoginAuditing.LoginInfoId
                };
                loginInfo.RetrieveByLoginInfoId(userData.DatabaseKey);
                //code to add days
                
                DateTime lastpassChangeDate = DateTime.Parse(loginInfo.LastPWChangeDate.ToString());
                DateTime chkExpired = lastpassChangeDate.AddDays(objVMLogin.PWExpiresDays);
               // DateTime chkExpired = lastpassChangeDate;
                DateTime currentDate = DateTime.UtcNow;
                if (objVMLogin.PWExpiresDays > 0)
                {
                    if (chkExpired.Date <= currentDate.Date)
                    {
                        //code for notofication and redirect to reset password page
                        daysCount = (chkExpired - currentDate).Days;
                        objVMLogin.ExpiresDaysCount = daysCount;
                        //redirect code to reset password
                        objVMLogin.IsPasswordExpired = true;  
                    }
                    else
                    {
                        objVMLogin.IsPasswordExpired = false;
                        daysCount = (chkExpired - currentDate).Days;
                        objVMLogin.ExpiresDaysCount = daysCount;
                    }
                }
            }

            return daysCount;
        }

    }
}