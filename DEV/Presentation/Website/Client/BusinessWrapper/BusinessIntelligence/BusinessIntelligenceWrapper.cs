using System.Collections.Generic;
using Client.Models;
using Common.Constants;
using DataContracts;
using Client.Common;

namespace Client.BusinessWrapper
{
    public class BusinessIntelligenceWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        List<string> errorMessage = new List<string>();
        public BusinessIntelligenceWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }
        public YurbiReportModel RetrieveYurbiReportDetails()
        {
            YurbiReportModel ojYurbiReportModel = new YurbiReportModel();
            AccessControl ac = new AccessControl();
            ac.ClientId = userData.DatabaseKey.Client.ClientId;
            ac.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            ac.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            ac.UserInfoId = userData.DatabaseKey.User.UserInfoId;
            ac.ControlType = AccessControlTypes.BusinessIntelligence;
            ac.IsUserAuthorized(userData.DatabaseKey);
            if (ac.IsAuthorized == true)
            {
                string adminUserName = System.Configuration.ConfigurationManager.AppSettings["ReportAdminUserName"].ToString();
                string adminPwd = System.Configuration.ConfigurationManager.AppSettings["ReportAdminPassword"].ToString();
                string userName = userData.DatabaseKey.UserName;
                string userPwd = System.Configuration.ConfigurationManager.AppSettings["ReportUserPassword"].ToString();
                string userEmail = userData.DatabaseKey.User.Email;
                long userInfoId = userData.DatabaseKey.User.UserInfoId;
                ojYurbiReportModel.ReportApiUrl = (System.Configuration.ConfigurationManager.AppSettings["ReportApiUrl"].ToString());
                ojYurbiReportModel.ReportServerUrl = (System.Configuration.ConfigurationManager.AppSettings["ReportServerUrl"].ToString());
                ojYurbiReportModel.ReportServerLibUrl = (System.Configuration.ConfigurationManager.AppSettings["ReportServerLibUrl"].ToString());
                ojYurbiReportModel.ReportServerDashBoard = (System.Configuration.ConfigurationManager.AppSettings["ReportServerDashBoard"].ToString());
                ojYurbiReportModel.AdminUserNameval = adminUserName;
                ojYurbiReportModel.UserNameVal = userName;
                ojYurbiReportModel.AdminPassword = adminPwd;
                ojYurbiReportModel.UserPassword = userPwd;
                ojYurbiReportModel.UserEmail = (userEmail == "" ? "" : userEmail);
                ojYurbiReportModel.FirstName = userData.DatabaseKey.User.FirstName;
                ojYurbiReportModel.LastName = userData.DatabaseKey.User.LastName;
                ojYurbiReportModel.Company = (userData.DatabaseKey.User.LocalizationCompany == "" ? userData.DatabaseKey.Client.CompanyName : userData.DatabaseKey.User.LocalizationCompany);
                ojYurbiReportModel.Description = "Report User Created";
                ojYurbiReportModel.UserInfoId = userInfoId.ToString();
                AzureUtil.AzureBlob az = new AzureUtil.AzureBlob();
                string group = az.GetFileNumberbyClientSite(userData.DatabaseKey.Client.ClientId, userData.DatabaseKey.User.DefaultSiteId);
                ojYurbiReportModel.Group = group;

                /* ******** Set the following cookies ********* */
                ojYurbiReportModel.Standard = "0";
                ojYurbiReportModel.FoodServices = "0";
                ojYurbiReportModel.Facilities = "0";

                // Get the Product Level (Client table | PackageLevel column) 
                // Set one (1) if the Client Product level is "Basic" (Standard)
                string prodLevel = userData.DatabaseKey.Client.PackageLevel;
                if (prodLevel == "Standard")
                {
                    ojYurbiReportModel.Standard = "1";
                }
                // Get the Business Type (Client table | BusinessType column) 
                // Set with a value of one (1) if the Client Business type is "FOOD SERVICES"
                string businessType = userData.DatabaseKey.Client.BusinessType;
                if (businessType.ToUpper() == BusinessTypeConstants.FoodServices)
                {
                    ojYurbiReportModel.FoodServices = "1";
                }

                // Set with a value of one (1) if the Client Business type is “FACILITIES”. 
                if (businessType.ToUpper() == BusinessTypeConstants.Facilities)
                {
                    ojYurbiReportModel.FoodServices = "1";
                }
                // User settings
                ojYurbiReportModel.UserType = userData.DatabaseKey.User.UserType;
                //**** Uncomment the following line after the UserInfo.ContactId column is added ****
                //hdnVar.Set("ContactId", UserData.DatabaseKey.User.ContactId);
                /* ************************************************ */
                string timezone = userData.DatabaseKey.Client.DefaultTimeZone;
                ojYurbiReportModel.TimeZone = timezone;
                ojYurbiReportModel.Access = "true";

            }
            else
            {
                ojYurbiReportModel.Access = "false";
            }
            ojYurbiReportModel.ReportApiUrlText = "ReportApiUrl";
            ojYurbiReportModel.ReportServerUrlText = "ReportServerUrl";
            ojYurbiReportModel.ReportServerLibUrlText = "ReportServerLibUrl";
            ojYurbiReportModel.ReportServerDashBoardText = "ReportServerDashBoard";
            ojYurbiReportModel.AdminUserNamevalText = "AdminUserNameval";
            ojYurbiReportModel.UserNameValText = "UserNameVal";
            ojYurbiReportModel.AdminPasswordText = "AdminPassword";
            ojYurbiReportModel.UserPasswordText = "UserPassword";
            ojYurbiReportModel.UserEmailText = "UserEmail";
            ojYurbiReportModel.FirstNameText = "FirstName";
            ojYurbiReportModel.LastNameText = "LastName";
            ojYurbiReportModel.CompanyText = "Company";
            ojYurbiReportModel.DescriptionText = "Description";
            ojYurbiReportModel.GroupText = "Group";
            ojYurbiReportModel.AccessText = "Access";
            ojYurbiReportModel.UserInfoIdText = "UserInfoId";
            ojYurbiReportModel.StandardText = "Standard";
            ojYurbiReportModel.FoodServicesText = "FoodServices";
            ojYurbiReportModel.FacilitiesText = "Facilities";
            ojYurbiReportModel.UserTypeText = "UserType";
            ojYurbiReportModel.TimeZoneText = "TimeZone";
            return ojYurbiReportModel;
        }
    }
}