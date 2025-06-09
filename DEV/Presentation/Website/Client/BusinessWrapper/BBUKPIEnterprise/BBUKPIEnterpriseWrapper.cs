using DataContracts;

using Client.Models.BBUKPIEnterprise;
using System.Collections.Generic;
using System;
using System.Web.Mvc;
using System.Linq;
using Common.Extensions;
using Client.Models;
using Common.Constants;
using Client.BusinessWrapper.Common;
using System.Threading.Tasks;
using Common.Enumerations;

namespace Client.BusinessWrapper
{
    public class BBUKPIEnterpriseWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        internal List<string> errorMessage = new List<string>();
        public BBUKPIEnterpriseWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        #region Search
        public List<BBUKPIEnterpriseSearchGridModel> GetBBUKPIEnterpriseGriddata(int CustomQueryDisplayId, string _SubmitStartDateVw = "", string _SubmitEndDateVw = "",
           string _CreateStartDateVw = "", string _CreateEndDateVw = "", List<string> Sites = null, string orderbycol = "", string orderDir = "", int skip = 0, int length = 0, string txtSearchval = "", string YearWeeks = "")
        {
            List<BBUKPIEnterpriseSearchGridModel> alllist = new List<BBUKPIEnterpriseSearchGridModel>();
            BBUKPI bBUKPI = new BBUKPI();
            bBUKPI.ClientId = this.userData.DatabaseKey.Client.ClientId;
            bBUKPI.CustomQueryDisplayId = CustomQueryDisplayId;
            bBUKPI.OrderbyColumn = orderbycol;
            bBUKPI.OrderBy = orderDir;
            bBUKPI.OffSetVal = skip;
            bBUKPI.NextRow = length;
            bBUKPI.SubmitStartDateVw = _SubmitStartDateVw;
            bBUKPI.SubmitEndDateVw = _SubmitEndDateVw;
            bBUKPI.CreateStartDateVw = _CreateStartDateVw;
            bBUKPI.CreateEndDateVw = _CreateEndDateVw;
            bBUKPI.Sites = string.Join(",", Sites ?? new List<string>());
            bBUKPI.YearWeekLists = YearWeeks;

            bBUKPI.RetrieveChunkEnterpriseSearch(this.userData.DatabaseKey, userData.Site.TimeZone);
            List<BBUKPI> bBUKPIlist = bBUKPI.listOfBBUKPI;

            foreach (var item in bBUKPIlist)
            {
                BBUKPIEnterpriseSearchGridModel data = new BBUKPIEnterpriseSearchGridModel();
                data.BBUKPIId = item.BBUKPIId;
                data.WeekNumber = item.Week;
                data.Year = item.Year;
                data.Status = item.Status;
                data.SiteName = item.SiteName;
                data.PMPercentCompleted = item.PMWOCompleted;
                data.WOBacklogCount = item.WOBacklogCount;
                if (item.SubmitDate != null && item.SubmitDate == default(DateTime))
                {
                    data.SubmitDate = null;
                }
                else
                {
                    data.SubmitDate = item.SubmitDate;
                }
                if (item.CreateDate != null && item.CreateDate == default(DateTime))
                {
                    data.Created = null;
                }
                else
                {
                    data.Created = item.CreateDate;
                }

                #region V2-909
                if (item.WeekStart == null || item.WeekStart == DateTime.MinValue)
                {
                    data.weekStart = "";
                }
                else
                {
                    data.weekStart = Convert.ToString(item.WeekStart);
                }
                if (item.WeekEnd == null || item.WeekEnd == DateTime.MinValue)
                {
                    data.weekEnd = "";
                }
                else
                {
                    data.weekEnd = Convert.ToString(item.WeekEnd);
                }
                data.phyInvAccuracy = item.PhyInvAccuracy;



                data.pMFollowUpComp = item.PMFollowUpComp;
                data.activeMechUsers = item.ActiveMechUsers;
                data.rCACount = item.RCACount;
                data.tTRCount = item.TTRCount;
                data.invValueOverMax = item.InvValueOverMax;
                data.cycleCountProgress = item.CycleCountProgress;
                data.eVTrainingHrs = item.EVTrainingHrs;
                #endregion

                data.TotalCount = item.TotalCount;
                alllist.Add(data);
            }
            return alllist;
        }
        public List<SelectListItem> GetSitesForSiteFilter()
        {
            BBUKPI bBUKPI = new BBUKPI();
            bBUKPI.ClientId = userData.DatabaseKey.Personnel.ClientId;
            List<BBUKPI> obj_BBUKPI = bBUKPI.RetrieveForEnterpriseSiteFilter(userData.DatabaseKey);

            var Sites = obj_BBUKPI.Select(x => new SelectListItem { Text = x.SiteName, Value = x.SiteId.ToString() }).ToList();
            return Sites;
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
        public BBUKPIEnterpriseModel PopulateBBUKPIDetails(long BBUKPIId)
        {
            BBUKPIEnterpriseModel objBBUKPI = new BBUKPIEnterpriseModel();
            BBUKPI obj = new BBUKPI()
            {
                ClientId = _dbKey.Client.ClientId,
                BBUKPIId = BBUKPIId
            };
            obj.RetriveEnterpriseDetailsByBBUKPIId(userData.DatabaseKey, userData.Site.TimeZone);
            objBBUKPI = initializeControls(obj);

            return objBBUKPI;
        }
        public BBUKPIEnterpriseModel initializeControls(BBUKPI obj)
        {
            BBUKPIEnterpriseModel objBBUKPI = new BBUKPIEnterpriseModel();
            objBBUKPI.ClientId = obj.ClientId;
            objBBUKPI.SiteId = obj.SiteId;

            objBBUKPI.BBUKPIId = obj.BBUKPIId;
            objBBUKPI.SiteName = obj?.SiteName ?? string.Empty;
            objBBUKPI.PMWOCompleted = obj.PMWOCompleted;
            objBBUKPI.WOBacklogCount = obj.WOBacklogCount;
            objBBUKPI.RCACount = obj.RCACount;
            objBBUKPI.TTRCount = obj.TTRCount;
            objBBUKPI.InvValueOverMax = obj.InvValueOverMax;
            objBBUKPI.PhyInvAccuracy = obj.PhyInvAccuracy;
            objBBUKPI.EVTrainingHrs = obj.EVTrainingHrs;
            objBBUKPI.DownDaySched= obj.DownDaySched;
            objBBUKPI.OptPMPlansCompleted = obj.OptPMPlansCompleted;
            objBBUKPI.OptPMPlansAdopted= obj.OptPMPlansAdopted;
            objBBUKPI.MLT= obj.MLT;
            objBBUKPI.TrainingPlanImp= obj.TrainingPlanImp;
            objBBUKPI.SubmitBy_Name= obj.SubmitBy_Name;
            if (obj.SubmitDate != null && obj.SubmitDate == default(DateTime))
            {
                objBBUKPI.SubmitDate = null;
            }
            else
            {
                objBBUKPI.SubmitDate = obj.SubmitDate;
            }
            objBBUKPI.Status = obj?.Status ?? string.Empty;
            objBBUKPI.Week = obj?.Week ?? string.Empty;
            objBBUKPI.Year = obj?.Year ?? string.Empty;
            objBBUKPI.CreateBy = obj.CreateBy; 
            objBBUKPI.ModifyBy = obj.ModifyBy;
            if (obj.CreateDate != null && obj.CreateDate == default(DateTime))
            {
                objBBUKPI.CreateDate = null;
            }
            else
            {
                objBBUKPI.CreateDate = obj.CreateDate;
            }
            if (obj.ModifyDate != null && obj.ModifyDate == default(DateTime))
            {
                objBBUKPI.ModifyDate = null;
            }
            else
            {
                objBBUKPI.ModifyDate = obj.ModifyDate;
            }
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

        #region ReOpen
        public BBUKPI ChangeStatus(long BBUKPIId, string Status)
        {
            BBUKPI bBUKPI = new BBUKPI()
            {
                ClientId = _dbKey.Client.ClientId,
                BBUKPIId = BBUKPIId
            };
            bBUKPI.Retrieve(userData.DatabaseKey);
            var submitBy_PersonnelId = bBUKPI.SubmitBy_PersonnelId;
            if (Status == "Reopen")
            {
                bBUKPI.Status = BBUKPIStatus.Open;
                bBUKPI.SubmitDate = null;
                bBUKPI.SubmitBy_PersonnelId = 0;
            }

            bBUKPI.Update(userData.DatabaseKey);

            Task t1 = Task.Factory.StartNew(() => CreateEventLog(BBUKPIId));

            if (bBUKPI.ErrorMessages == null || bBUKPI.ErrorMessages.Count == 0)
            {
                List<long> objectId = new List<long>() { BBUKPIId };
                var UserList = new List<Tuple<long, string, string>>();
                CommonWrapper coWrapper = new CommonWrapper(userData);
                var PersonnelInfo = coWrapper.GetPersonnelDetailsByPersonnelId(submitBy_PersonnelId);
                if (PersonnelInfo != null)
                {
                    UserList.Add
                    (
                        Tuple.Create(Convert.ToInt64(PersonnelInfo.PersonnelId), PersonnelInfo.UserName, PersonnelInfo.Email)
                    );
                    ProcessAlert objAlert = new ProcessAlert(this.userData);
                    Task t2 = Task.Factory.StartNew(() => objAlert.CreateAlert<BBUKPI>(AlertTypeEnum.KPIReOpened, objectId, UserList));
                }
            }
            return bBUKPI;
        }
        private void CreateEventLog(long BBUKPIId)
        {
            BBUKPIEventLog bBUKPIEventLog = new BBUKPIEventLog();
            bBUKPIEventLog.ClientId = userData.DatabaseKey.Client.ClientId;
            bBUKPIEventLog.SiteId = userData.Site.SiteId;
            bBUKPIEventLog.ObjectId = BBUKPIId;
            bBUKPIEventLog.TransactionDate = DateTime.UtcNow;
            bBUKPIEventLog.Event = EventStatusConstants.ReOpened;
            bBUKPIEventLog.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            bBUKPIEventLog.Create(_dbKey);
        }
        #endregion
    }
}