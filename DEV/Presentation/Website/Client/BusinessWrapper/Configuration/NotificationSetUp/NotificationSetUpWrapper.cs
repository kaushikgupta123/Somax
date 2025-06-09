using Client.BusinessWrapper.Common;
using Client.Common;
using Client.Models.Configuration.NotificationSetUp;
using Client.Models.Configuration.SiteSetUp;
using DataContracts;
using INTDataLayer.BAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Client.BusinessWrapper.Configuration.NotificationSetUp
{
    public class NotificationSetUpWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        List<string> errorMessage = new List<string>();

        public NotificationSetUpWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }
        #region Comment NotificationDetails Old
        //internal AlertDefine NotificationDetails(long DefaultAlertSetupId,ref long AlertSetupId, ref int UpdateIndex,ref bool IsActive, ref bool IsEmailSend,ref bool IsIncludeEmailAttachedment)
        //{
        //    AlertSetup alertSetup = new AlertSetup()
        //    {
        //        ClientId = this.userData.DatabaseKey.Client.ClientId,
        //        AlertSetupId = DefaultAlertSetupId
        //    };
        //    alertSetup.Retrieve(this.userData.DatabaseKey);
        //    UpdateIndex= alertSetup.UpdateIndex;
        //    AlertSetupId = alertSetup.AlertSetupId;
        //    IsActive = alertSetup.IsActive;
        //    IsEmailSend = alertSetup.EmailSend;
        //    IsIncludeEmailAttachedment = alertSetup.EmailAttachment;
        //    AlertDefine alertSetupRetrieve = new AlertDefine()
        //    {
        //        AlertDefineId = alertSetup.AlertDefineId
        //    };

        //    alertSetupRetrieve.Retrieve(this.userData.DatabaseKey);
        //    AlertLocal alertLocal = new AlertLocal()
        //    {
        //        AlertLocalId = alertSetup.AlertLocalId

        //    };
        //    alertLocal.Retrieve(this.userData.DatabaseKey);
        //    alertSetupRetrieve.Description = alertLocal.Description;

        //    return alertSetupRetrieve;
        //}
        #endregion 
        #region Add new NotificationDetails Method
        internal AlertSetup NotificationDetails(long DefaultAlertSetupId, ref long AlertSetupId, ref int UpdateIndex, ref bool IsActive, ref bool IsEmailSend, ref bool IsIncludeEmailAttachedment)
        {
            AlertSetup alertSetup = new AlertSetup()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                AlertSetupId = DefaultAlertSetupId
            };
            alertSetup.RetrieveNotificationDetails(this.userData.DatabaseKey);
            UpdateIndex = alertSetup.UpdateIndex;
            AlertSetupId = alertSetup.AlertSetupId;
            IsActive = alertSetup.IsActive;
            IsEmailSend = alertSetup.EmailSend;
            IsIncludeEmailAttachedment = alertSetup.EmailAttachment;
            return alertSetup;
        }
        #endregion

        internal DataTable NotificationList()
        {
            long strSite = this.userData.DatabaseKey.User.DefaultSiteId;
            DataTable AlertNotification = new DataTable();
            AlertNotification = AlertRepositoryBL.GeAlertNotification(this.userData.DatabaseKey.Client.ClientId, strSite, this.userData.DatabaseKey.Client.CallerUserInfoId,
                 this.userData.DatabaseKey.User.FullName,
                 userData.DatabaseKey.Client.ConnectionString);
            return AlertNotification;
        }
        public DataTable AlertSetUpList(long strSite)
        {
            DataTable AlertNotification = new DataTable();
            AlertNotification = AlertRepositoryBL.GeAlertNotification(this.userData.DatabaseKey.Client.ClientId, strSite, this.userData.DatabaseKey.Client.CallerUserInfoId, this.userData.DatabaseKey.User.FullName, this.userData.DatabaseKey.Client.ConnectionString);
            return AlertNotification;
        }
        public List<AlertTargetModel> populateTargetGrid(long AlertSetupId)
        {          
            AlertTarget alertTarget1 = new AlertTarget()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                include_inactive = true
            };
            alertTarget1.AlertSetupId = AlertSetupId;
            List<AlertTarget> alertlist = alertTarget1.RetreiveTargetList(this.userData.DatabaseKey);
            List<AlertTargetModel> returnObj = new List<AlertTargetModel>();
            AlertTargetModel obj;
            foreach (var item in alertlist)
            {
                obj = new AlertTargetModel();
                obj.Personnel_ClientLookupId = item.Personnel_ClientLookupId.LRTrim();
                obj.FirstName = item.FirstName.LRTrim();
                obj.LastName = item.LastName.LRTrim();
                obj.IsActive = item.IsActive;
                obj.AlertTargetId = item.AlertTargetId;
                obj.AlertSetupId = item.AlertSetupId;
                obj.UpdateIndex = item.UpdateIndex;            
                returnObj.Add(obj);
            }
            return returnObj;
        }
        internal List<DataContracts.Personnel> PersonalList()
        {         
            Personnel personnel = new Personnel()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.User.DefaultSiteId
            };

            List<DataContracts.Personnel> PersonnelList = personnel.RetrieveForLookupList(this.userData.DatabaseKey);
            return PersonnelList;
        }

        internal  List<string> UpadteTarget(AlertTargetModel obj)
        {
               
                AlertTarget AlertTargetrv = new AlertTarget()
                {
                    ClientId = this.userData.DatabaseKey.Client.ClientId,
                    AlertTargetId = obj.AlertTargetId
                };
                AlertTargetrv.Retrieve(this.userData.DatabaseKey);
                AlertTarget alertTarget = new AlertTarget()
                {
                    ClientId = this.userData.DatabaseKey.Client.ClientId,
                    AlertTargetId = obj.AlertTargetId
                };
                alertTarget.AlertSetupId = AlertTargetrv.AlertSetupId;
                alertTarget.IsActive = Convert.ToBoolean(obj.IsActive);
                alertTarget.UserInfoId = AlertTargetrv.UserInfoId;
                alertTarget.UpdateIndex = AlertTargetrv.UpdateIndex;
                alertTarget.Update(this.userData.DatabaseKey);
                return alertTarget.ErrorMessages;
        }
        internal List<string> AddTarget(AlertTargetModel obj)
        {

            #region Row Add Validation
            long UserInfoId = obj.ClientLookupID;
            AlertTarget alertTargetrv = new AlertTarget()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
            };
            List<AlertTarget> alertTargetList = alertTargetrv.RetrieveAll(this.userData.DatabaseKey);
            IEnumerable<AlertTarget> alertTargetFilter = from TargetList in alertTargetList
                                                         where TargetList.AlertSetupId == obj.AlertSetupId && TargetList.UserInfoId == UserInfoId
                                                         select new AlertTarget
                                                         {
                                                             AlertTargetId = TargetList.AlertTargetId
                                                         };
            if (alertTargetFilter.Count() > 0)
            {
                List<string> ErrorList = new List<string>();
                ErrorList.Add("Name  must be unique for every alert Setup");
                return ErrorList;
            }
            #endregion

            AlertTarget alertTarget = new AlertTarget()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
            };
            alertTarget.AlertSetupId = obj.AlertSetupId;
            alertTarget.UserInfoId = obj.ClientLookupID;
            alertTarget.IsActive = obj.IsActive;
            alertTarget.Create(this.userData.DatabaseKey);

            return alertTarget.ErrorMessages;
        }        
        public bool DeleteTarget(long AlertTargetId)
        {
            try
            {
                AlertTarget alertTarget = new AlertTarget()
                {
                    ClientId = this.userData.DatabaseKey.Client.ClientId,
                    AlertTargetId = AlertTargetId
                };
                alertTarget.Delete(this.userData.DatabaseKey);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal List<string> UpdateNotificationAlertSetUp(NotificationSetUpModel objMo)
        {
            AlertSetup alertSetupRetrieve = new AlertSetup()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                AlertSetupId = objMo.AlertSetupId
            };
            alertSetupRetrieve.Retrieve(this.userData.DatabaseKey); 
            alertSetupRetrieve.IsActive = objMo.IsActive;
            alertSetupRetrieve.EmailSend = objMo.IsEmailSend;
            alertSetupRetrieve.EmailAttachment = objMo.IsIncludeEmailAttachedment;
            alertSetupRetrieve.Update(this.userData.DatabaseKey);
            return alertSetupRetrieve.ErrorMessages;
        }
    }
}