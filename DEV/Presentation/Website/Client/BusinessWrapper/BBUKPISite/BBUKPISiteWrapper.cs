using Client.Models;
using Client.Models.BBUKPISite;

using Common.Constants;
using Common.Enumerations;
using Common.Extensions;

using DataContracts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Client.BusinessWrapper
{
    public class BBUKPISiteWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        internal List<string> errorMessage = new List<string>();
        public BBUKPISiteWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        #region Search and Print
        public List<BBUKPISiteSearchGridModel> GetBBUKPISiteGridData(int customQueryDisplayId, string Order, string orderDir, int skip, int length, string Week, string Year, string Status, decimal PMWOCompleted, int WOBacklogCount, string SubmitStartDateVw, string SubmitEndDateVw, string CreateStartDateVw, string CreateEndDateVw, string searchText, string YearWeeks = "")
        {
            List<BBUKPISiteSearchGridModel> Resultlist = new List<BBUKPISiteSearchGridModel>();
            BBUKPISiteSearchGridModel bBUKPISiteSearchGridModel;
            BBUKPI bBUKPI = new BBUKPI()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.Site.SiteId,
                OffSetVal = skip,
                NextRow = length,
                CustomQueryDisplayId = customQueryDisplayId,
                OrderBy = orderDir,
                OrderbyColumn = Order,
                Week = Week,
                Year = Year,
                Status = Status,
                PMWOCompleted = PMWOCompleted,
                WOBacklogCount = WOBacklogCount,
                SubmitStartDateVw = SubmitStartDateVw,
                SubmitEndDateVw = SubmitEndDateVw,
                CreateStartDateVw = CreateStartDateVw,
                CreateEndDateVw = CreateEndDateVw,
                //searchText = searchText
                YearWeekLists = YearWeeks
            };
            bBUKPI = bBUKPI.RetrieveChunkSiteSearch(_dbKey, userData.Site.TimeZone);
            foreach (var item in bBUKPI.listOfBBUKPI)
            {
                bBUKPISiteSearchGridModel = new BBUKPISiteSearchGridModel();

                bBUKPISiteSearchGridModel.ClientId = item.ClientId;
                bBUKPISiteSearchGridModel.SiteId = item.SiteId;
                bBUKPISiteSearchGridModel.BBUKPIId = item.BBUKPIId;
                bBUKPISiteSearchGridModel.Year = item.Year;
                bBUKPISiteSearchGridModel.Week = item.Week;
                
                //bBUKPISiteSearchGridModel.Status = item.Status;
                bBUKPISiteSearchGridModel.Created = item.CreateDate;
                bBUKPISiteSearchGridModel.PMWOCompleted = item.PMWOCompleted;
                bBUKPISiteSearchGridModel.WOBacklogCount = item.WOBacklogCount;
                //if (item.SubmitDate == null || item.SubmitDate == DateTime.MinValue)
                //{
                //    bBUKPISiteSearchGridModel.SubmitDate = null;
                //}
                //else
                //{
                //    bBUKPISiteSearchGridModel.SubmitDate = item.SubmitDate;
                //}
                #region V2-909***
                if (item.WeekStart == null || item.WeekStart == DateTime.MinValue)
                {
                    bBUKPISiteSearchGridModel.weekStart = "";
                }
                else
                {
                    //bBUKPISiteSearchGridModel.weekStart = Convert.ToString(item.WeekStart);
                    bBUKPISiteSearchGridModel.weekStart = item.WeekStart?.ToString("MM/dd/yyyy");
                }
                if (item.WeekEnd == null || item.WeekEnd == DateTime.MinValue)
                {
                    bBUKPISiteSearchGridModel.weekEnd = "";
                }
                else
                {
                    //bBUKPISiteSearchGridModel.weekEnd = Convert.ToString(item.WeekEnd);
                    bBUKPISiteSearchGridModel.weekEnd = item.WeekEnd?.ToString("MM/dd/yyyy");
                }
                bBUKPISiteSearchGridModel.pMFollowUpComp = item.PMFollowUpComp;
                bBUKPISiteSearchGridModel.phyInvAccuracy = item.PhyInvAccuracy;                
                bBUKPISiteSearchGridModel.activeMechUsers = item.ActiveMechUsers;
                bBUKPISiteSearchGridModel.rCACount = item.RCACount;
                bBUKPISiteSearchGridModel.tTRCount = item.TTRCount;
                bBUKPISiteSearchGridModel.invValueOverMax = item.InvValueOverMax;
                bBUKPISiteSearchGridModel.cycleCountProgress = item.CycleCountProgress;
                bBUKPISiteSearchGridModel.eVTrainingHrs = item.EVTrainingHrs;
                bBUKPISiteSearchGridModel.siteName = item.SiteName;


                //********
                #endregion
                bBUKPISiteSearchGridModel.TotalCount = item.TotalCount;

                Resultlist.Add(bBUKPISiteSearchGridModel);
            }
            return Resultlist;
        }

        public List<BBUKPISitePrintModel> GetBBUKPISitePrintData(int customQueryDisplayId, string Order, string orderDir, int skip, int length, string Week, string Year, string Status, decimal PMWOCompleted, int WOBacklogCount, string SubmitStartDateVw, string SubmitEndDateVw, string CreateStartDateVw, string CreateEndDateVw, string searchText = "", List<string> YearWeeks = null)
        {
            List<BBUKPISitePrintModel> Resultlist = new List<BBUKPISitePrintModel>();
            BBUKPISitePrintModel bBUKPISiteSearchGridModel;
            BBUKPI bBUKPI = new BBUKPI()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.Site.SiteId,
                OffSetVal = skip,
                NextRow = length,
                CustomQueryDisplayId = customQueryDisplayId,
                OrderBy = orderDir,
                OrderbyColumn = Order,
                Week = Week,
                Year = Year,
                Status = Status,
                PMWOCompleted = PMWOCompleted,
                WOBacklogCount = WOBacklogCount,
                SubmitStartDateVw = SubmitStartDateVw,
                SubmitEndDateVw = SubmitEndDateVw,
                CreateStartDateVw = CreateStartDateVw,
                CreateEndDateVw = CreateEndDateVw,
                //searchText = searchText
                YearWeekLists = string.Join(",", YearWeeks ?? new List<string>())
        };
            bBUKPI = bBUKPI.RetrieveChunkSiteSearch(_dbKey, userData.Site.TimeZone);
            foreach (var item in bBUKPI.listOfBBUKPI)
            {
                bBUKPISiteSearchGridModel = new BBUKPISitePrintModel();

                bBUKPISiteSearchGridModel.Year = item.Year;
                bBUKPISiteSearchGridModel.Week = item.Week;
                //bBUKPISiteSearchGridModel.Status = item.Status;
                bBUKPISiteSearchGridModel.Created = item.CreateDate;
                bBUKPISiteSearchGridModel.PMWOCompleted = item.PMWOCompleted;
                bBUKPISiteSearchGridModel.WOBacklogCount = item.WOBacklogCount;
                //if (item.SubmitDate == null || item.SubmitDate == DateTime.MinValue)
                //{
                //    bBUKPISiteSearchGridModel.SubmitDate = null;
                //}
                //else
                //{
                //    bBUKPISiteSearchGridModel.SubmitDate = item.SubmitDate;
                //}
                #region//**V2-909***
                if (item.WeekStart == null || item.WeekStart == DateTime.MinValue)
                {
                    bBUKPISiteSearchGridModel.weekStart = "";
                }
                else
                {
                    bBUKPISiteSearchGridModel.weekStart = Convert.ToString(item.WeekStart);
                }
                if (item.WeekEnd == null || item.WeekEnd == DateTime.MinValue)
                {
                    bBUKPISiteSearchGridModel.weekEnd = "";
                }
                else
                {
                    bBUKPISiteSearchGridModel.weekEnd = Convert.ToString(item.WeekEnd);
                }
                bBUKPISiteSearchGridModel.phyInvAccuracy = item.PhyInvAccuracy;



                bBUKPISiteSearchGridModel.pMFollowUpComp = item.PMFollowUpComp;
                bBUKPISiteSearchGridModel.activeMechUsers = item.ActiveMechUsers;
                bBUKPISiteSearchGridModel.rCACount = item.RCACount;
                bBUKPISiteSearchGridModel.tTRCount = item.TTRCount;
                bBUKPISiteSearchGridModel.invValueOverMax = item.InvValueOverMax;
                bBUKPISiteSearchGridModel.cycleCountProgress = item.CycleCountProgress;
                bBUKPISiteSearchGridModel.eVTrainingHrs = item.EVTrainingHrs;
                bBUKPISiteSearchGridModel.siteName = item.SiteName;
                //********
                #endregion
                Resultlist.Add(bBUKPISiteSearchGridModel);
            }
            return Resultlist;
        }
        public List<SelectListItem> GetYearWeekForFilter()
        {
            BBUKPI bBUKPI = new BBUKPI();
            bBUKPI.ClientId = userData.DatabaseKey.Personnel.ClientId;
            bBUKPI.SiteId = userData.DatabaseKey.Personnel.SiteId;
            List<BBUKPI> obj_BBUKPI = bBUKPI.RetrieveYearWeekForFilter(userData.DatabaseKey);

            var Sites = obj_BBUKPI.Select(x => new SelectListItem { Text = x.YearWeek, Value = x.Id.ToString() }).ToList();
            return Sites;
        }
        #endregion

        #region Details
        public BBUKPISiteModel PopulateBBUKPIDetails(long BBUKPIId)
        {
            BBUKPISiteModel objBBUKPI = new BBUKPISiteModel();
            BBUKPI obj = new BBUKPI()
            {
                ClientId = _dbKey.Client.ClientId,
                SiteId = userData.Site.SiteId,
                BBUKPIId = BBUKPIId
            };
            obj.RetriveSiteDetailsByBBUKPIId(userData.DatabaseKey);
            objBBUKPI = initializeControls(obj);

            return objBBUKPI;
        }
        public BBUKPISiteModel initializeControls(BBUKPI obj)
        {
            BBUKPISiteModel objBBUKPI = new BBUKPISiteModel();
            objBBUKPI.ClientId = obj.ClientId;
            objBBUKPI.SiteId = obj.ClientId;

            objBBUKPI.BBUKPIId = obj.BBUKPIId;
            objBBUKPI.SiteName = obj?.SiteName ?? string.Empty;
            objBBUKPI.PMWOCompleted = obj.PMWOCompleted;
            objBBUKPI.WOBacklogCount = obj.WOBacklogCount;
            objBBUKPI.RCACount = obj.RCACount;
            objBBUKPI.TTRCount = obj.TTRCount;
            objBBUKPI.InvValueOverMax = obj.InvValueOverMax;
            objBBUKPI.PhyInvAccuracy = obj.PhyInvAccuracy;
            objBBUKPI.EVTrainingHrs = obj.EVTrainingHrs;
            objBBUKPI.DownDaySched = obj.DownDaySched;
            objBBUKPI.OptPMPlansCompleted = obj.OptPMPlansCompleted;
            objBBUKPI.OptPMPlansAdopted = obj.OptPMPlansAdopted;
            objBBUKPI.MLT = obj.MLT;
            objBBUKPI.TrainingPlanImp = obj.TrainingPlanImp;
            objBBUKPI.SubmitBy_Name = obj.SubmitBy_Name;
            //if (obj.SubmitDate != null && obj.SubmitDate == default(DateTime))
            //{
            //    objBBUKPI.SubmitDate = null;
            //}
            //else
            //{
            //    objBBUKPI.SubmitDate = obj.SubmitDate;
            //}
            objBBUKPI.Status = obj?.Status ?? string.Empty;
            objBBUKPI.Week = obj?.Week ?? string.Empty;
            objBBUKPI.Year = obj?.Year ?? string.Empty;
            objBBUKPI.CreateBy = obj.CreateBy;
            objBBUKPI.ModifyBy = obj.ModifyBy;
            //if (obj.CreateDate != null && obj.CreateDate == default(DateTime))
            //{
            //    objBBUKPI.CreateDate = null;
            //}
            //else
            //{
            //    objBBUKPI.CreateDate = obj.CreateDate;
            //}
            //if (obj.ModifyDate != null && obj.ModifyDate == default(DateTime))
            //{
            //    objBBUKPI.ModifyDate = null;
            //}
            //else
            //{
            //    objBBUKPI.ModifyDate = obj.ModifyDate;
            //}
            objBBUKPI.SubmitDate = obj?.SubmitDate ?? DateTime.MinValue;
            objBBUKPI.CreateDate = obj?.CreateDate ?? DateTime.MinValue;
            objBBUKPI.ModifyDate = obj?.ModifyDate ?? DateTime.MinValue;

            #region V2-909
            objBBUKPI.PMFollowUpComp = obj.PMFollowUpComp;
            objBBUKPI.ActiveMechUsers = obj.ActiveMechUsers;
            objBBUKPI.CycleCountProgress = obj.CycleCountProgress;
            objBBUKPI.WeekStart = obj.WeekStart;
            objBBUKPI.WeekEnd = obj.WeekEnd;
            #endregion

            return objBBUKPI;
        }
        #endregion

        #region Event Log
        public List<EventLogModel> PopulateEventLog(long ObjectId)
        {
            EventLogModel objEventLogModel;
            List<EventLogModel> EventLogModelList = new List<EventLogModel>();

            BBUKPIEventLog log = new BBUKPIEventLog();
            List<BBUKPIEventLog> data = new List<BBUKPIEventLog>();
            log.ClientId = userData.DatabaseKey.Client.ClientId;
            log.SiteId = userData.DatabaseKey.Personnel.SiteId;
            log.ObjectId = ObjectId;
            data = log.RetriveByBBUKPIId(userData.DatabaseKey);

            if (data != null)
            {
                foreach (var item in data)
                {
                    objEventLogModel = new EventLogModel();
                    objEventLogModel.ClientId = item.ClientId;
                    objEventLogModel.SiteId = item.SiteId;
                    objEventLogModel.EventLogId = item.EventLogId;
                    objEventLogModel.ObjectId = item.ObjectId;
                    if (item.TransactionDate != null && item.TransactionDate == default(DateTime))
                    {
                        objEventLogModel.TransactionDate = null;
                    }
                    else
                    {
                        objEventLogModel.TransactionDate = item.TransactionDate.ToUserTimeZone(userData.Site.TimeZone);
                    }
                    objEventLogModel.Event = item.Event;
                    objEventLogModel.Comments = item.Comments;
                    objEventLogModel.SourceId = item.SourceId;
                    objEventLogModel.Personnel = item.Personnel;
                    objEventLogModel.Events = item.Events;
                    objEventLogModel.PersonnelInitial = item.PersonnelInitial;
                    EventLogModelList.Add(objEventLogModel);
                }
            }
            return EventLogModelList;
        }
        #endregion

        #region Edit
        public BBUKPISiteEditModel PopulateBBUKPIDetailsForEdit(long BBUKPIId)
        {
            BBUKPISiteEditModel objBBUKPI = new BBUKPISiteEditModel();
            BBUKPI obj = new BBUKPI()
            {
                ClientId = _dbKey.Client.ClientId,
                SiteId = userData.Site.SiteId,
                BBUKPIId = BBUKPIId
            };
            obj.RetriveSiteDetailsByBBUKPIId(userData.DatabaseKey);
            objBBUKPI = initializeControlsForEdit(obj);

            return objBBUKPI;
        }
        public BBUKPISiteEditModel initializeControlsForEdit(BBUKPI obj)
        {
            BBUKPISiteEditModel objBBUKPI = new BBUKPISiteEditModel();
            objBBUKPI.ClientId = obj.ClientId;
            objBBUKPI.SiteId = obj.ClientId;

            objBBUKPI.BBUKPIId = obj.BBUKPIId;
            //objBBUKPI.SiteName = obj?.SiteName ?? string.Empty;
            //objBBUKPI.PMWOCompleted = obj.PMWOCompleted;
            //objBBUKPI.WOBacklogCount = obj.WOBacklogCount;
            //objBBUKPI.RCACount = obj.RCACount;
            //objBBUKPI.TTRCount = obj.TTRCount;
            //objBBUKPI.InvValueOverMax = obj.InvValueOverMax;
            //objBBUKPI.PhyInvAccuracy = obj.PhyInvAccuracy;
            //objBBUKPI.EVTrainingHrs = obj.EVTrainingHrs;
            objBBUKPI.DownDaySched = obj.DownDaySched;
            objBBUKPI.OptPMPlansCompleted = obj.OptPMPlansCompleted;
            objBBUKPI.OptPMPlansAdopted = obj.OptPMPlansAdopted;
            objBBUKPI.MLT = obj.MLT;
            objBBUKPI.TrainingPlanImp = obj.TrainingPlanImp;
            //objBBUKPI.SubmitBy_Name = obj.SubmitBy_Name;
            //if (obj.SubmitDate != null && obj.SubmitDate == default(DateTime))
            //{
            //    objBBUKPI.SubmitDate = null;
            //}
            //else
            //{
            //    objBBUKPI.SubmitDate = obj.SubmitDate;
            //}
            objBBUKPI.Status = obj?.Status ?? string.Empty;
            objBBUKPI.Week = obj?.Week ?? string.Empty;
            objBBUKPI.Year = obj?.Year ?? string.Empty;
            //objBBUKPI.CreateBy = obj.CreateBy;
            //objBBUKPI.ModifyBy = obj.ModifyBy;
            //if (obj.CreateDate != null && obj.CreateDate == default(DateTime))
            //{
            //    objBBUKPI.CreateDate = null;
            //}
            //else
            //{
            //    objBBUKPI.CreateDate = obj.CreateDate;
            //}
            //if (obj.ModifyDate != null && obj.ModifyDate == default(DateTime))
            //{
            //    objBBUKPI.ModifyDate = null;
            //}
            //else
            //{
            //    objBBUKPI.ModifyDate = obj.ModifyDate;
            //}
            //objBBUKPI.SubmitDate = obj?.SubmitDate ?? DateTime.MinValue;
            //objBBUKPI.CreateDate = obj?.CreateDate ?? DateTime.MinValue;
            //objBBUKPI.ModifyDate = obj?.ModifyDate ?? DateTime.MinValue;

            return objBBUKPI;
        }
        public BBUKPI BBUKPISiteEdit(BBUKPISiteVM siteVM)
        {
            BBUKPI bBUKPI = new BBUKPI
            {
                BBUKPIId = siteVM.BBUKPISiteEditModel.BBUKPIId,
                ClientId = siteVM.BBUKPISiteEditModel.ClientId
            };
            bBUKPI.Retrieve(_dbKey);

            bBUKPI.DownDaySched = siteVM.BBUKPISiteEditModel.DownDaySched;
            bBUKPI.OptPMPlansCompleted = siteVM.BBUKPISiteEditModel.OptPMPlansCompleted ?? 0;
            bBUKPI.OptPMPlansAdopted = siteVM.BBUKPISiteEditModel.OptPMPlansAdopted ?? 0;
            bBUKPI.MLT = siteVM.BBUKPISiteEditModel.MLT ?? 0;
            bBUKPI.TrainingPlanImp = siteVM.BBUKPISiteEditModel.TrainingPlanImp;
            if (bBUKPI.SubmitDate != null && bBUKPI.SubmitDate == DateTime.MinValue)
            {
                bBUKPI.SubmitDate = null;
            }
            if (bBUKPI.WeekStart != null && bBUKPI.WeekStart == DateTime.MinValue)
            {
                bBUKPI.WeekStart = null;
            }
            if (bBUKPI.WeekEnd != null && bBUKPI.WeekEnd == DateTime.MinValue)
            {
                bBUKPI.WeekEnd = null;
            }

            bBUKPI.Update(_dbKey);

            return bBUKPI;
        }
        #endregion

        #region Submit
        public BBUKPI BBUKPISiteSubmit(long BBUKPIId, long ClientId)
        {
            BBUKPI bBUKPI = new BBUKPI
            {
                BBUKPIId = BBUKPIId,
                ClientId = ClientId
            };
            bBUKPI.Retrieve(_dbKey);

            bBUKPI.Status = BBUKPIStatus.Submitted;
            bBUKPI.SubmitDate = DateTime.UtcNow;
            bBUKPI.SubmitBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;

            bBUKPI.Update(_dbKey);

            CreateEventLog(BBUKPIId);

            List<long> Ids = new List<long>() { BBUKPIId };
            ProcessAlert objAlert = new ProcessAlert(this.userData);
            objAlert.CreateAlert<DataContracts.BBUKPI>(AlertTypeEnum.KPISubmitted, Ids);

            return bBUKPI;
        }
        private void CreateEventLog(long BBUKPIId)
        {
            BBUKPIEventLog bBUKPIEventLog = new BBUKPIEventLog();
            bBUKPIEventLog.ClientId = userData.DatabaseKey.Client.ClientId;
            bBUKPIEventLog.SiteId = userData.Site.SiteId;
            bBUKPIEventLog.ObjectId = BBUKPIId;
            bBUKPIEventLog.TransactionDate = DateTime.UtcNow;
            bBUKPIEventLog.Event = BBUKPIStatus.Submitted;
            bBUKPIEventLog.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            bBUKPIEventLog.Create(_dbKey);
        }
        #endregion
    }
}