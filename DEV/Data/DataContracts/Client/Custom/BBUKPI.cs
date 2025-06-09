using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Reflection;
using System.Text;
using System.Data;

using Database;
using Database.Business;
using Common.Extensions;
using Newtonsoft.Json;

namespace DataContracts
{
    public partial class BBUKPI : DataContractBase
    {
        #region Properties
        public List<BBUKPI> listOfBBUKPI { get; set; }
        public string Sites { get; set; }
        public int CustomQueryDisplayId { get; set; }
        public string CreateStartDateVw { get; set; }
        public string CreateEndDateVw { get; set; }
        public string SubmitStartDateVw { get; set; }
        public string SubmitEndDateVw { get; set; }
        public string SiteName { get; set; }
        //public string SearchText { get; set; }
        public int TotalCount { get; set; }
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int OffSetVal { get; set; }
        public int NextRow { get; set; }
        public DateTime CreateDate { get; set; }
        public string SubmitBy_Name { get; set; }
        public string CreateBy { get; set; }
        public string ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
        #region V2-909
        public string YearWeek { get; set; }
        public long Id { get; set; }
        public string YearWeekLists { get; set; }
        public string week_Start { get; set; }
        public string week_End { get; set; }
        #endregion
        #endregion

        #region BBU KPI Enterprise search
        public BBUKPI RetrieveChunkEnterpriseSearch(DatabaseKey dbKey, string TimeZone)
        {
            BBUKPI_RetrieveChunkEnterpriseSearch trans = new BBUKPI_RetrieveChunkEnterpriseSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.BBUKPI = this.ToDateBaseObjectForRetriveChunkEnterpriseSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            this.listOfBBUKPI = new List<BBUKPI>();

            List<BBUKPI> BBUKPIlist = new List<BBUKPI>();
            foreach (b_BBUKPI BBUKPI in trans.BBUKPI.listOfBBUKPI)
            {
                BBUKPI tmpBBUKPI = new BBUKPI();

                tmpBBUKPI.UpdateFromDatabaseObjectForRetrieveChunkEnterpriseSearch(BBUKPI, TimeZone);
                BBUKPIlist.Add(tmpBBUKPI);
            }
            this.listOfBBUKPI.AddRange(BBUKPIlist);
            return this;
        }
        public b_BBUKPI ToDateBaseObjectForRetriveChunkEnterpriseSearch()
        {
            b_BBUKPI dbObj = this.ToDatabaseObject();

            dbObj.CustomQueryDisplayId = this.CustomQueryDisplayId;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.NextRow = this.NextRow;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.Sites = this.Sites;
            dbObj.CreateStartDateVw = this.CreateStartDateVw;
            dbObj.CreateEndDateVw = this.CreateEndDateVw;
            dbObj.SubmitStartDateVw = this.SubmitStartDateVw;
            dbObj.SubmitEndDateVw = this.SubmitEndDateVw;
            dbObj.YearWeekLists = this.YearWeekLists;
            return dbObj;
        }
        public void UpdateFromDatabaseObjectForRetrieveChunkEnterpriseSearch(b_BBUKPI dbObj, string TimeZone)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.BBUKPIId = dbObj.BBUKPIId;
            this.Week = dbObj.Week;
            this.Year = dbObj.Year;
            this.SiteName = dbObj.SiteName;
            this.Status = dbObj.Status;
            if (dbObj.CreateDate != null && dbObj.CreateDate != DateTime.MinValue)
            {
                this.CreateDate = dbObj.CreateDate.ToUserTimeZone(TimeZone);
            }
            else
            {
                this.CreateDate = dbObj.CreateDate;
            }
            if (dbObj.SubmitDate != null && dbObj.SubmitDate != DateTime.MinValue)
            {
                this.SubmitDate = dbObj.SubmitDate.ToUserTimeZone(TimeZone);
            }
            else
            {
                this.SubmitDate = dbObj.SubmitDate;
            }
            this.PMWOCompleted = dbObj.PMWOCompleted;
            this.WOBacklogCount = dbObj.WOBacklogCount;

            #region//**V2-909***
            this.WeekStart = dbObj.WeekStart;
            this.WeekEnd = dbObj.WeekEnd;
            this.PhyInvAccuracy = dbObj.PhyInvAccuracy;
            this.PMFollowUpComp = dbObj.PMFollowUpComp;
            this.ActiveMechUsers = dbObj.ActiveMechUsers;
            this.RCACount = dbObj.RCACount;
            this.TTRCount = dbObj.TTRCount;
            this.InvValueOverMax = dbObj.InvValueOverMax;
            this.CycleCountProgress = dbObj.CycleCountProgress;
            this.EVTrainingHrs = dbObj.EVTrainingHrs;
            //********
            #endregion

            this.TotalCount = dbObj.TotalCount;
        }
        public List<BBUKPI> RetrieveForEnterpriseSiteFilter(DatabaseKey dbKey)
        {
            BBUKPI_RetrieveForEnterpriseSiteFilter trans = new BBUKPI_RetrieveForEnterpriseSiteFilter()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.dbKey = dbKey.ToTransDbKey();
            trans.UseTransaction = false;
            trans.BBUKPI = this.ToDatabaseObject();
            trans.Execute();
            return UpdateFromDatabaseObjectForSiteFilterList(trans.SiteList);
        }
        public static List<BBUKPI> UpdateFromDatabaseObjectForSiteFilterList(List<b_BBUKPI> dbObjs)
        {
            List<BBUKPI> result = new List<BBUKPI>();

            foreach (b_BBUKPI dbObj in dbObjs)
            {
                BBUKPI tmp = new BBUKPI();
                tmp.UpdateFromDatabaseObjectForSiteFilterList(dbObj);
                result.Add(tmp);
            }
            return result;
        }
        public void UpdateFromDatabaseObjectForSiteFilterList(b_BBUKPI dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.SiteId = dbObj.SiteId;
            this.SiteName = dbObj.SiteName;
        }
        public void RetriveEnterpriseDetailsByBBUKPIId(DatabaseKey dbKey, string TimeZone)
        {
            BBUKPI_RetrieveEnterpriseDetailsByBBUKPIId trans = new BBUKPI_RetrieveEnterpriseDetailsByBBUKPIId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.BBUKPI = this.ToDatabaseObjectRetriveEnterpriseDetailsByBBUKPIId();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObjectExtendedEnterpriseDetailsByBBUKPIId(trans.BBUKPI,TimeZone);
        }

        public b_BBUKPI ToDatabaseObjectRetriveEnterpriseDetailsByBBUKPIId()
        {
            b_BBUKPI dbObj = new b_BBUKPI();
            dbObj.ClientId = this.ClientId;
            dbObj.BBUKPIId = this.BBUKPIId;
            return dbObj;
        }
        public void UpdateFromDatabaseObjectExtendedEnterpriseDetailsByBBUKPIId(b_BBUKPI dbObj,string TimeZone)
        {
            this.ClientId = dbObj.ClientId;
            this.BBUKPIId = dbObj.BBUKPIId;
            this.SiteId = dbObj.SiteId;
            this.SiteName = dbObj.SiteName;
            this.PMWOCompleted = dbObj.PMWOCompleted;
            this.WOBacklogCount = dbObj.WOBacklogCount;
            this.RCACount = dbObj.RCACount;
            this.TTRCount = dbObj.TTRCount;
            this.InvValueOverMax = dbObj.InvValueOverMax;
            this.PhyInvAccuracy = dbObj.PhyInvAccuracy;
            this.EVTrainingHrs = dbObj.EVTrainingHrs;
            this.DownDaySched = dbObj.DownDaySched;
            this.OptPMPlansCompleted = dbObj.OptPMPlansCompleted;
            this.OptPMPlansAdopted = dbObj.OptPMPlansAdopted;
            this.MLT = dbObj.MLT;
            this.TrainingPlanImp = dbObj.TrainingPlanImp;
            this.SubmitBy_PersonnelId = dbObj.SubmitBy_PersonnelId;
            this.SubmitBy_Name = dbObj.SubmitBy_Name;
            this.Status = dbObj.Status;
            this.Week = dbObj.Week;
            this.Year = dbObj.Year;
            this.CreateBy = dbObj.CreateBy;
            this.ModifyBy = dbObj.ModifyBy;
            if (dbObj.CreateDate != null && dbObj.CreateDate != DateTime.MinValue)
            {
                this.CreateDate = dbObj.CreateDate.ToUserTimeZone(TimeZone);
            }
            else
            {
                this.CreateDate = dbObj.CreateDate;
            }
            if (dbObj.SubmitDate != null && dbObj.SubmitDate != DateTime.MinValue)
            {
                this.SubmitDate = dbObj.SubmitDate.ToUserTimeZone(TimeZone);
            }
            else
            {
                this.SubmitDate = dbObj.SubmitDate;
            }
            if (dbObj.ModifyDate != null && dbObj.ModifyDate != DateTime.MinValue)
            {
                this.ModifyDate = dbObj.ModifyDate.ToUserTimeZone(TimeZone);
            }
            else
            {
                this.ModifyDate = dbObj.ModifyDate;
            }
            //**V2-909
            this.PMFollowUpComp = dbObj.PMFollowUpComp;
            this.ActiveMechUsers = dbObj.ActiveMechUsers;
            this.CycleCountProgress = dbObj.CycleCountProgress;
            if (dbObj.WeekStart == null || dbObj.WeekStart == DateTime.MinValue)
            {
                this.week_Start = "";
            }
            else
            {
                this.week_Start = Convert.ToString(dbObj.WeekStart);
            }
            if (dbObj.WeekEnd == null || dbObj.WeekEnd == DateTime.MinValue)
            {
                this.week_End = "";
            }
            else
            {
                this.week_End = Convert.ToString(dbObj.WeekEnd);
            }
            //**
        }
        #endregion

        #region BBU KPI Site search
        public BBUKPI RetrieveChunkSiteSearch(DatabaseKey dbKey, string TimeZone)
        {
            BBUKPI_RetrieveChunkSiteSearch trans = new BBUKPI_RetrieveChunkSiteSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.BBUKPI = this.ToDateBaseObjectForRetriveChunkSiteSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            this.listOfBBUKPI = new List<BBUKPI>();

            List<BBUKPI> BBUKPIlist = new List<BBUKPI>();
            foreach (b_BBUKPI BBUKPI in trans.BBUKPI.listOfBBUKPI)
            {
                BBUKPI tmpBBUKPI = new BBUKPI();

                tmpBBUKPI.UpdateFromDatabaseObjectForRetrieveChunkSiteSearch(BBUKPI, TimeZone);
                BBUKPIlist.Add(tmpBBUKPI);
            }
            this.listOfBBUKPI.AddRange(BBUKPIlist);
            return this;
        }
        public b_BBUKPI ToDateBaseObjectForRetriveChunkSiteSearch()
        {
            b_BBUKPI dbObj = this.ToDatabaseObject();

            dbObj.CustomQueryDisplayId = this.CustomQueryDisplayId;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.NextRow = this.NextRow;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.Sites = this.Sites;
            dbObj.CreateStartDateVw = this.CreateStartDateVw;
            dbObj.CreateEndDateVw = this.CreateEndDateVw;
            dbObj.SubmitStartDateVw = this.SubmitStartDateVw;
            dbObj.SubmitEndDateVw = this.SubmitEndDateVw;
            //dbObj.SearchText = this.SearchText;
            dbObj.YearWeekLists = this.YearWeekLists;
            return dbObj;
        }
        public void UpdateFromDatabaseObjectForRetrieveChunkSiteSearch(b_BBUKPI dbObj, string TimeZone)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.BBUKPIId = dbObj.BBUKPIId;
            this.Week = dbObj.Week;
            this.Year = dbObj.Year;
            this.SiteName = dbObj.SiteName;
            this.Status = dbObj.Status;
            if (dbObj.CreateDate != null && dbObj.CreateDate != DateTime.MinValue)
            {
                this.CreateDate = dbObj.CreateDate.ToUserTimeZone(TimeZone);
            }
            else
            {
                this.CreateDate = dbObj.CreateDate;
            }
            if (dbObj.SubmitDate != null && dbObj.SubmitDate != DateTime.MinValue)
            {
                this.SubmitDate = dbObj.SubmitDate.ToUserTimeZone(TimeZone);
            }
            else
            {
                this.SubmitDate = dbObj.SubmitDate;
            }
            this.PMWOCompleted = dbObj.PMWOCompleted;
            this.WOBacklogCount = dbObj.WOBacklogCount;
            #region V2-909
            this.WeekStart = dbObj.WeekStart;
            this.WeekEnd = dbObj.WeekEnd;
            this.PhyInvAccuracy = dbObj.PhyInvAccuracy;

            this.PMFollowUpComp = dbObj.PMFollowUpComp;
            this.ActiveMechUsers = dbObj.ActiveMechUsers;
            this.RCACount = dbObj.RCACount;
            this.TTRCount = dbObj.TTRCount;
            this.InvValueOverMax = dbObj.InvValueOverMax;
            this.CycleCountProgress = dbObj.CycleCountProgress;
            this.EVTrainingHrs = dbObj.EVTrainingHrs;
            #endregion
            this.TotalCount = dbObj.TotalCount;
        }
        public List<BBUKPI> RetrieveYearWeekForFilter(DatabaseKey dbKey)
        {
            BBUKPI_RetrieveYearWeekForFilter trans = new BBUKPI_RetrieveYearWeekForFilter()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.dbKey = dbKey.ToTransDbKey();
            trans.UseTransaction = false;
            trans.BBUKPI = this.ToDatabaseObject();
            trans.Execute();
            return UpdateFromDatabaseObjectForYearWeekFilterList(trans.YearWeekList);
        }
        public static List<BBUKPI> UpdateFromDatabaseObjectForYearWeekFilterList(List<b_BBUKPI> dbObjs)
        {
            List<BBUKPI> result = new List<BBUKPI>();

            foreach (b_BBUKPI dbObj in dbObjs)
            {
                BBUKPI tmp = new BBUKPI();
                tmp.UpdateFromDatabaseObjectForYearWeekFilterList(dbObj);
                result.Add(tmp);
            }
            return result;
        }
        public void UpdateFromDatabaseObjectForYearWeekFilterList(b_BBUKPI dbObj)
        {
            //this.ClientId = dbObj.ClientId;
            //this.SiteId = dbObj.SiteId;
            //this.SiteName = dbObj.SiteName;
            this.Id = dbObj.Id;
            this.YearWeek = dbObj.YearWeek;
        }
        #endregion

        #region BBU KPI Site Deails
        public void RetriveSiteDetailsByBBUKPIId(DatabaseKey dbKey)
        {
            BBUKPI_RetrieveSiteDetailsByBBUKPIId trans = new BBUKPI_RetrieveSiteDetailsByBBUKPIId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.BBUKPI = this.ToDatabaseObjectRetriveSiteByBBUKPIId();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObjectExtendedSiteDetailsByBBUKPIId(trans.BBUKPI);
        }

        public b_BBUKPI ToDatabaseObjectRetriveSiteByBBUKPIId()
        {
            b_BBUKPI dbObj = new b_BBUKPI();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.BBUKPIId = this.BBUKPIId;
            return dbObj;
        }
        public void UpdateFromDatabaseObjectExtendedSiteDetailsByBBUKPIId(b_BBUKPI dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.BBUKPIId = dbObj.BBUKPIId;
            this.SiteId = dbObj.SiteId;
            this.SiteName = dbObj.SiteName;
            this.PMWOCompleted = dbObj.PMWOCompleted;
            this.WOBacklogCount = dbObj.WOBacklogCount;
            this.RCACount = dbObj.RCACount;
            this.TTRCount = dbObj.TTRCount;
            this.InvValueOverMax = dbObj.InvValueOverMax;
            this.PhyInvAccuracy = dbObj.PhyInvAccuracy;
            this.EVTrainingHrs = dbObj.EVTrainingHrs;
            this.DownDaySched = dbObj.DownDaySched;
            this.OptPMPlansCompleted = dbObj.OptPMPlansCompleted;
            this.OptPMPlansAdopted = dbObj.OptPMPlansAdopted;
            this.MLT = dbObj.MLT;
            this.TrainingPlanImp = dbObj.TrainingPlanImp;
            this.SubmitDate = dbObj.SubmitDate;
            this.SubmitBy_PersonnelId = dbObj.SubmitBy_PersonnelId;
            this.SubmitBy_Name = dbObj.SubmitBy_Name;
            this.Status = dbObj.Status;
            this.Week = dbObj.Week;
            this.Year = dbObj.Year;
            this.CreateBy = dbObj.CreateBy;
            this.CreateDate = dbObj.CreateDate;
            this.ModifyBy = dbObj.ModifyBy;
            this.ModifyDate = dbObj.ModifyDate;
            //**V2-909
            this.PMFollowUpComp = dbObj.PMFollowUpComp;
            this.ActiveMechUsers = dbObj.ActiveMechUsers;
            this.CycleCountProgress = dbObj.CycleCountProgress;
            //this.week_Start = dbObj.WeekStart;
            //this.week_End = dbObj.WeekEnd;
            // V2-976 - Load this.WeekStart and this.WeekEnd 
            this.WeekStart = dbObj.WeekStart;
            this.WeekEnd = dbObj.WeekEnd;
            if (dbObj.WeekStart == null || dbObj.WeekStart == DateTime.MinValue)
            {
                this.week_Start = "";
            }
            else
            {
                this.week_Start = Convert.ToString(dbObj.WeekStart);
            }
            if (dbObj.WeekEnd == null || dbObj.WeekEnd == DateTime.MinValue)
            {
                this.week_End = "";
            }
            else
            {
                this.week_End = Convert.ToString(dbObj.WeekEnd);
            }
            //**
        }
        #endregion
    }
}
