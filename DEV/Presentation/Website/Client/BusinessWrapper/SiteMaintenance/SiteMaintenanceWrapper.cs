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

namespace Client.BusinessWrapper
{
    public class SiteMaintenanceWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        List<string> errorMessage = new List<string>();
        public SiteMaintenanceWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        public SiteMaintenanceWrapper()
        {

        }
        #region GetAllSitemaintenance
        public DataContracts.SiteMaintenance GetNextSitemaintenance(string sameday)
        {
            string timeZone = userData.Site.TimeZone;
            DataContracts.SiteMaintenance siteMaintenanance = new DataContracts.SiteMaintenance();
            siteMaintenanance.TimeZone = timeZone;
            siteMaintenanance.SameDay = sameday;
            siteMaintenanance.RetrieveNextSch(userData.DatabaseKey, timeZone);
            return siteMaintenanance;
        }

        public DataContracts.SiteMaintenance GetNextSitemaintenancewithoutLogin(string sameday)
        {
            DatabaseKey dbKey = Authentication.GetAdminOnlyKey();
            // V2-857 - Time is intended to be Eastern Standard Time
            //string timeZone = "US Eastern Standard Time";
            string timeZone = "Eastern Standard Time";
            DataContracts.SiteMaintenance siteMaintenanance = new DataContracts.SiteMaintenance();
            siteMaintenanance.TimeZone = timeZone;
            siteMaintenanance.SameDay = sameday;
            siteMaintenanance.RetrieveNextSch(dbKey, timeZone);
            return siteMaintenanance;
        }
        #endregion

    }
}