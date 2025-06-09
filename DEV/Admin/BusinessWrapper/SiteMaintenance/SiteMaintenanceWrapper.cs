using Admin.Models;

using Business.Authentication;

using DataContracts;

using System;
using System.Collections.Generic;
using System.Globalization;

namespace Admin.BusinessWrapper
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
            string timeZone = "US Eastern Standard Time";
            DataContracts.SiteMaintenance siteMaintenanance = new DataContracts.SiteMaintenance();
            siteMaintenanance.TimeZone = timeZone;
            siteMaintenanance.SameDay = sameday;
            siteMaintenanance.RetrieveNextSch(dbKey, timeZone);
            return siteMaintenanance;
        }
        #endregion

        #region WebSiteMaintainenceMessage Details V2-994

        public SiteMaintenance AddSiteMaintenance(SiteMaintenanceModel model)
        {           
            var dnStartTimedate = model.DowntimeStartDate;
            DateTime DowntimeStart = DateTime.ParseExact(dnStartTimedate + " " + model.DowntimeStartTime, "MM/dd/yyyy h:mm tt", CultureInfo.InvariantCulture);
            var dnEndTimedate = model.DowntimeEndDate;
            DateTime DowntimeEnd = DateTime.ParseExact(dnEndTimedate + " " + model.DowntimeEndTime, "MM/dd/yyyy h:mm tt", CultureInfo.InvariantCulture);
            SiteMaintenance siteMaintenance = new SiteMaintenance()
            {
                SiteMaintenanceId = model.SiteMaintenanceId,
                LoginPageMessage = model.LoginPageMessage,
                DashboardMessage = model.DashboardMessage,
                DowntimeStart = DowntimeStart,
                DowntimeEnd = DowntimeEnd,
            };
            siteMaintenance.Create(this.userData.DatabaseKey);
            return siteMaintenance;
        }
        public SiteMaintenance EditSiteMaintenance(SiteMaintenanceModel model)
        {
            SiteMaintenance siteMaintenance = new SiteMaintenance()
            {
                SiteMaintenanceId = model.SiteMaintenanceId,
            };
            siteMaintenance.Retrieve(userData.DatabaseKey);
            var dnStartTimedate = model.DowntimeStartDate;
            DateTime DowntimeStart = DateTime.ParseExact(dnStartTimedate + " " + model.DowntimeStartTime, "MM/dd/yyyy h:mm tt", CultureInfo.InvariantCulture);
            var dnEndTimedate = model.DowntimeEndDate;
            DateTime DowntimeEnd = DateTime.ParseExact(dnEndTimedate + " " + model.DowntimeEndTime, "MM/dd/yyyy h:mm tt", CultureInfo.InvariantCulture);
            siteMaintenance.SiteMaintenanceId = model.SiteMaintenanceId;
            siteMaintenance.DowntimeStart = DowntimeStart;
            siteMaintenance.DowntimeEnd = DowntimeEnd;
            siteMaintenance.LoginPageMessage = model.LoginPageMessage;
            siteMaintenance.DashboardMessage = model.DashboardMessage;
            siteMaintenance.Update(userData.DatabaseKey);
            return siteMaintenance;
        }
        public List<SiteMaintenanceModel> SiteMaintenanceDetailsChunkSearch(string orderbycol = "", int length = 0, string orderDir = "", int skip = 0)
        {
            List<SiteMaintenanceModel> siteMaintenanceModelList = new List<SiteMaintenanceModel>();
            SiteMaintenanceModel siteMaintenanceModel;
            SiteMaintenance siteMaintenance = new SiteMaintenance();
            siteMaintenance.OrderbyColumn = orderbycol;
            siteMaintenance.OrderBy = orderDir;
            siteMaintenance.OffSetVal = skip;
            siteMaintenance.NextRow = length;
            var siteMaintenanceList =
                siteMaintenance.SiteMaintenanceDetailsChunkSearch
                (userData.DatabaseKey);
            foreach (var item in siteMaintenanceList)
            {
                siteMaintenanceModel = new SiteMaintenanceModel();
                siteMaintenanceModel.SiteMaintenanceId = item.SiteMaintenanceId;
                siteMaintenanceModel.LoginPageMessage = item.LoginPageMessage;
                siteMaintenanceModel.DashboardMessage = item.DashboardMessage;
                siteMaintenanceModel.DowntimeStart = item.DowntimeStart;
                siteMaintenanceModel.DowntimeEnd = item.DowntimeEnd;
                siteMaintenanceModel.CreateDate = item.CreateDate;
                siteMaintenanceModel.TotalCount = item.TotalCount;
                siteMaintenanceModelList.Add(siteMaintenanceModel);
            }
            return siteMaintenanceModelList;
        }

        public SiteMaintenanceModel RetrieveSiteMaintenance(long SiteMaintenanceId)
        {
            var siteMaintenanceModel = new SiteMaintenanceModel();
            SiteMaintenance siteMaintenance = new SiteMaintenance()
            {               
                SiteMaintenanceId = SiteMaintenanceId,
            };
            siteMaintenance.Retrieve(userData.DatabaseKey);
            if (siteMaintenance != null)
            {
                siteMaintenanceModel.SiteMaintenanceId = siteMaintenance.SiteMaintenanceId;
                siteMaintenanceModel.LoginPageMessage = siteMaintenance.LoginPageMessage;
                siteMaintenanceModel.DashboardMessage = siteMaintenance.DashboardMessage;
                siteMaintenanceModel.DowntimeStartDate = siteMaintenance.DowntimeStart.ToString("MM/dd/yyyy");
                siteMaintenanceModel.DowntimeStartTime = siteMaintenance.DowntimeStart.ToString("hh:mm tt");
                siteMaintenanceModel.DowntimeEndDate = siteMaintenance.DowntimeEnd.ToString("MM/dd/yyyy");
                siteMaintenanceModel.DowntimeEndTime = siteMaintenance.DowntimeEnd.ToString("hh:mm tt");
            }
            return siteMaintenanceModel;
        }

        #endregion

    }
}