using Client.BusinessWrapper.Common;
using Client.Common;
using Client.Localization;
using Client.Models;
using Client.Models.Common;
using Client.Models.Work_Order;
using Client.Models.WorkOrder;
using Common.Constants;
using Common.Enumerations;
using Common.Extensions;
using DataContracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Client.Models.Work_Order.UIConfiguration;
using Client.Models.GuestWorkRequest;
using Client.Models.WorkOrderStatus;
namespace Client.BusinessWrapper.Work_Order
{
    public class WorkOrderWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        List<string> errorMessage = new List<string>();
        public string newClientlookupId { get; set; }

        public WorkOrderWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        public List<Personnel> MentionList(string searchtext)
        {
            Personnel p = new Personnel();
            List<Personnel> Mlist = new List<Personnel>();
            p.ClientId = userData.DatabaseKey.Client.ClientId;
            p.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            p.Searchtext = searchtext;
            Mlist = p.PersonnelRetrieveForMention(userData.DatabaseKey);
            return Mlist;
        }

        public List<List<WorkOrderSchedule>> WOSchedulePersonnelList(string WorkOrderId = "")//--V2-293--//
        {
            WorkOrderSchedule WS = new WorkOrderSchedule();
            WS.ClientId = userData.DatabaseKey.Client.ClientId;
            WS.SiteId = userData.Site.SiteId;                         // 2020-May-06
            WS.WorkOrderId = string.IsNullOrEmpty(WorkOrderId) ? 0 : Convert.ToInt64(WorkOrderId);
            WS.RetrievePersonnelByAssetGroupMasterQuery(userData.DatabaseKey);
            return WS.TotalRecords;
        }



        public string PopulateHoverList(long workOrderId)
        {
            WorkOrder wo = new WorkOrder();
            WorkOrder PersonData = new WorkOrder();
            wo.ClientId = userData.DatabaseKey.Client.ClientId;
            wo.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            wo.WorkOrderId = workOrderId;
            PersonData = wo.RetrievePersonnelInitial(userData.DatabaseKey);
            string userList = PersonData.PersonnelFull;
            return userList;
        }
        public WorkOrder AddScheduleRecord(WoScheduleModel wosm)//--V2-293--//
        {
            string PersonnelList = String.Empty;
            ProcessAlert objAlert = new ProcessAlert(this.userData);
            WorkOrder w = new WorkOrder();
            w.ClientId = userData.DatabaseKey.Client.ClientId;
            w.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            w.WorkOrderId = wosm.WorkOrderId;
            w.ScheduledDuration = wosm.ScheduledDuration;
            w.ScheduledStartDate = wosm.Schedulestartdate;


            CommonWrapper coWrapper = new CommonWrapper(userData);
            var namelist = coWrapper.MentionList("");
            List<UserMentionDataModel.UserMentionData> userMentionDataList = new List<UserMentionDataModel.UserMentionData>();
            UserMentionDataModel.UserMentionData objUserMentionData;
            List<long> nos = new List<long>() { wosm.WorkOrderId };
            if (wosm.PersonnelIds != null && wosm.PersonnelIds.Count > 0)
            {
                foreach (var item in wosm.PersonnelIds)
                {
                    PersonnelList += item + ",";
                    objUserMentionData = new UserMentionDataModel.UserMentionData();//new UserMentionData();
                    objUserMentionData.userId = Convert.ToInt64(item);
                    objUserMentionData.userName = namelist.Where(x => x.PersonnelId == Convert.ToInt64(item)).Select(y => y.UserName).FirstOrDefault();
                    objUserMentionData.emailId = namelist.Where(x => x.PersonnelId == Convert.ToInt64(item)).Select(y => y.Email).FirstOrDefault();
                    userMentionDataList.Add(objUserMentionData);
                }
            }
            List<long> userIds = new List<long>();
            var UserList = new List<Tuple<long, string, string>>();
            if (userMentionDataList != null && userMentionDataList.Count > 0)
            {
                foreach (var item in userMentionDataList)
                {
                    UserList.Add
                   (
                        Tuple.Create(Convert.ToInt64(item.userId), item.userName, item.emailId)
                  );
                }

            }

            w.PersonnelList = (!String.IsNullOrEmpty(PersonnelList)) ? PersonnelList.TrimEnd(',') : string.Empty;
            if ((wosm.PersonnelIds != null && wosm.PersonnelIds.Count > 0) && (wosm.Schedulestartdate != null && wosm.Schedulestartdate != default(DateTime)))
            {
                w.IsDeleteFlag = false;
            }
            else
            {
                w.IsDeleteFlag = true;
            }
            w.AddScheduleRecord(userData.DatabaseKey);
            // V2-1168 - Add an Event Log entry
            if (w.ErrorMessages == null || w.ErrorMessages.Count == 0)
            {
              CreateEventLog(w.WorkOrderId, WorkOrderEventLogFunction.Scheduled, "Schedule Record(s) Added");
            }
            objAlert.CreateAlert<WorkOrder>(AlertTypeEnum.WorkOrderAssign, nos, UserList);
            return w;
        }


        public WorkOrder RemoveScheduleRecord(long workorderId, ref string Statusmsg)//--V2-349--//
        {
            string PersonnelList = String.Empty;
            ProcessAlert objAlert = new ProcessAlert(this.userData);
            WorkOrder w = new WorkOrder();
            w.ClientId = userData.DatabaseKey.Client.ClientId;
            w.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            w.WorkOrderId = workorderId;
            w.WorkOrderUpdateOnRemovingSchedule(this.userData.DatabaseKey, userData.Site.TimeZone);
            if (w.ErrorMessages.Count == 0)
            {
                Statusmsg = "success";
                CreateEventLog(workorderId,WorkOrderEvents.RemoveSchedule);//V2-1183
            }
            else
            {
                Statusmsg = "error";
            }

            return w;
        }
        public bool DeleteScheduleRecord(WorkOrder wo)//--V2-293--//
        {
            bool isSuccess = true;
            WorkOrder w = new WorkOrder();
            w.ClientId = userData.DatabaseKey.Client.ClientId;
            w.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            w.WorkOrderId = wo.WorkOrderId;
            w.IsDeleteFlag = true;
            w.AddScheduleRecord(userData.DatabaseKey);
            if (w.ErrorMessages != null && w.ErrorMessages.Count > 0)
            {
                isSuccess = false;
            }
            return isSuccess;
        }

        #region New code implementation for grid load and advance search
        public List<WorkOrderModel> populateWODetails(int CustomQueryDisplayId, int skip = 0,
           int length = 0, string orderbycol = "", string orderDir = "", string _CompleteStartDateVw = "", string _CompleteEndDateVw = "",
           string _CreateStartDateVw = "", string _CreateEndDateVw = "", string workOrderLookUpId = "", string description = "", string ChargeTo = "", string ChargeToName = "", string AssetLocation = "",
          List<string> type = null, List<string> status = null, List<string> shift = null, string AssetGroup1ClientLookUpId = "", string AssetGroup2ClientLookUpId = "",
           string AssetGroup3ClientLookUpId = "", List<string> priority = null, string _startcreated = "", string _endcreated = "", string creator = "",
           string assigned = "", string _startscheduled = "", string _endscheduled = "", List<string> failcode = null, string _startactualFinish = "", string _endactualFinish = "",
           string txtSearchval = "", string personnelList = "", decimal? ActualDuration = null, List<string> sourcetype = null, List<string> AssetGroup1Id = null,
           List<string> AssetGroup2Id = null, List<string> AssetGroup3Id = null, bool? downRequired = null, List<string> assignedWo = null, string _RequiredDate = "", List<string> planner = null, List<string> projectIds = null)
        {
            if (!string.IsNullOrEmpty(txtSearchval))
            {
                txtSearchval = txtSearchval.Trim();
            }
            WorkOrder workorder = new WorkOrder();
            WorkOrderModel workOrderModel;
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);//<!--(Added on 25/06/2020)-->
            List<WorkOrderModel> workOrderModelList = new List<WorkOrderModel>();
            List<DataContracts.LookupList> Type = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> PMType = new List<DataContracts.LookupList>();   // RKL - 2019-07-12
            List<DataContracts.LookupList> Shift = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> Failure = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> Priority = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> SourceType = new List<DataContracts.LookupList>();

            workorder.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            workorder.CustomQueryDisplayId = CustomQueryDisplayId;
            workorder.OrderbyColumn = orderbycol;
            workorder.OrderBy = orderDir;
            workorder.OffSetVal = skip;
            workorder.NextRow = length;
            workorder.ClientLookupId = workOrderLookUpId;
            workorder.Description = description;
            workorder.ChargeToClientLookupId = ChargeTo;
            workorder.ChargeTo_Name = ChargeToName;
            workorder.AssetLocation = AssetLocation;
            workorder.Type = string.Join(",", type ?? new List<string>());
            workorder.Status = string.Join(",", status ?? new List<string>());
            workorder.Shift = string.Join(",", shift ?? new List<string>());
            workorder.Priority = string.Join(",", priority ?? new List<string>());
            workorder.StartCreateDate = _startcreated;
            workorder.EndCreateDate = _endcreated;
            workorder.Creator = creator;
            workorder.Assigned = assigned;
            workorder.StartScheduledDate = _startscheduled;
            workorder.EndScheduledDate = _endscheduled;
            workorder.FailureCode = string.Join(",", failcode ?? new List<string>());
            workorder.StartActualFinishDate = _startactualFinish;
            workorder.EndActualFinishDate = _endactualFinish;
            workorder.CompleteComments = string.Empty;
            workorder.SearchText = txtSearchval;
            workorder.AssetGroup1ClientlookupId = AssetGroup1ClientLookUpId;
            workorder.AssetGroup2ClientlookupId = AssetGroup2ClientLookUpId;
            workorder.AssetGroup3ClientlookupId = AssetGroup3ClientLookUpId;
            workorder.PersonnelList = personnelList;
            workorder.LoggedInUserPEID = userData.DatabaseKey.Personnel.PersonnelId;
            workorder.StartActualFinishDateVw = _CompleteStartDateVw;
            workorder.EndActualFinishDateVw = _CompleteEndDateVw;
            workorder.CreateStartDateVw = _CreateStartDateVw;
            workorder.CreateEndDateVw = _CreateEndDateVw;
            workorder.SourceType = string.Join(",", sourcetype ?? new List<string>());
            //<!--Added on 23/06/2020-->
            //<!--(Added on 25/06/2020)-->
           
            workorder.AssetGroup1AdvSearchId = string.Join(",", AssetGroup1Id ?? new List<string>());
            workorder.AssetGroup2AdvSearchId = string.Join(",", AssetGroup2Id ?? new List<string>());
            workorder.AssetGroup3AdvSearchId = string.Join(",", AssetGroup3Id ?? new List<string>());
            workorder.downRequired = downRequired;//V2-892
            //V2-984
            workorder.Assigned = string.Join(",", assignedWo ?? new List<string>());
            workorder.RequireDate = _RequiredDate;
            workorder.PlannerFullName = string.Join(",", planner ?? new List<string>()); //V2-1078
            workorder.ProjectIds = string.Join(",", projectIds ?? new List<string>()); //V2-1135
            //<!--(Added on 25/06/2020)-->
            if (ActualDuration != null)
                workorder.ActualDuration = ActualDuration.Value;

            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();

            Task<WorkOrder> task1 = Task.Factory.StartNew<WorkOrder>(
                           () => workorder.RetrieveV2(this.userData.DatabaseKey, userData.Site.TimeZone));


            Task task2 = Task.Factory.StartNew(() => AllLookUps = commonWrapper.GetAllLookUpList());

            Task.WaitAll(task1, task2);

            if (AllLookUps != null)
            {
                Type = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_TYPE).ToList();
                PMType = AllLookUps.Where(x => x.ListName == LookupListConstants.Preventive_Maint_WO_Type).ToList();  // RKL - 2019-07-12
                Shift = AllLookUps.Where(x => x.ListName == LookupListConstants.Shift).ToList();
                Failure = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_FAILURE).ToList();
                Priority = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_Priority).ToList();
            }
            List<WorkOrder> woList = workorder.listOfWO;
            foreach (var wo in woList)
            {

                workOrderModel = new WorkOrderModel();
                workOrderModel.WorkOrderId = wo.WorkOrderId;
                workOrderModel.ClientLookupId = wo.ClientLookupId;
                workOrderModel.Description = wo.Description;
                workOrderModel.ChargeTo_Name = wo.ChargeTo_Name;
                workOrderModel.AssetLocation = wo.AssetLocation;
                workOrderModel.ChargeToId = wo.ChargeToId;
                workOrderModel.ChargeToClientLookupId = wo.ChargeToClientLookupId;
                workOrderModel.WorkAssigned_PersonnelId = wo.WorkAssigned_PersonnelId;
                workOrderModel.PartsOnOrder = wo.PartsOnOrder;

                workOrderModel.Assigned = wo.Personnels;
                if (!string.IsNullOrEmpty(wo.AssignedFullName))
                {
                    workOrderModel.AssignedFullName = wo.AssignedFullName;
                }
                else
                {
                    workOrderModel.AssignedFullName = "";
                }
                if (wo.SourceType == WorkOrderSourceTypes.Emergency)
                {
                    Type = AllLookUps.Where(x => x.ListName == LookupListConstants.UP_WO_TYPE).ToList();
                }
                else if (wo.SourceType == WorkOrderSourceTypes.WorkRequest)
                {
                    Type = AllLookUps.Where(x => x.ListName == LookupListConstants.WR_WO_TYPE).ToList();
                }
                else if (wo.SourceType == WorkOrderSourceTypes.OnDemand)
                {
                    Type = AllLookUps.Where(x => x.ListName == LookupListConstants.UP_WO_TYPE).ToList();
                }
                if (Type != null && Type.Any(cus => cus.ListValue == wo.Type))
                {
                    workOrderModel.Type = Type.Where(x => x.ListValue == wo.Type).Select(x => x.Description).First();
                }
                // RKL - 2019-07-11
                else if (PMType != null && PMType.Any(cus => cus.ListValue == wo.Type))
                {
                    workOrderModel.Type = PMType.Where(x => x.ListValue == wo.Type).Select(x => x.Description).First();
                }
                else
                {
                    workOrderModel.Type = string.Empty;
                }
                workOrderModel.Status = wo.Status;

                if (Shift != null && Shift.Any(cus => cus.ListValue == wo.Shift))
                {
                    workOrderModel.Shift = Shift.Where(x => x.ListValue == wo.Shift).Select(x => x.Description).First();
                }
                else
                {
                    workOrderModel.Shift = string.Empty;
                }
                workOrderModel.Priority = wo.Priority;
                if (Priority != null && Priority.Any(cus => cus.ListValue == wo.Priority))
                {
                    workOrderModel.Priority = Priority.Where(x => x.ListValue == wo.Priority).Select(x => x.Description).First();
                }

                if (wo.CreateDate != null && wo.CreateDate == default(DateTime))
                {
                    workOrderModel.CreateDate = null;
                }
                else
                {
                    workOrderModel.CreateDate = wo.CreateDate;
                }

                if (wo.CompleteDate != null && wo.CompleteDate == default(DateTime))
                {
                    workOrderModel.CompleteDate = null;
                }
                else
                {
                    workOrderModel.CompleteDate = wo.CompleteDate;
                }
                workOrderModel.Creator = wo.Creator;
                

                if (wo.ScheduledStartDate == null || wo.ScheduledStartDate == default(DateTime))
                {
                    workOrderModel.ScheduledStartDate = null;
                }
                else
                {
                    workOrderModel.ScheduledStartDate = wo.ScheduledStartDate;
                }

                workOrderModel.FailureCode = wo.FailureCode;
                if (Failure != null && Failure.Any(cus => cus.ListValue == wo.FailureCode))
                {
                    workOrderModel.FailureCode = Failure.Where(x => x.ListValue == wo.FailureCode).Select(x => x.Description).First();
                }

                if (wo.ActualFinishDate != null && wo.ActualFinishDate == default(DateTime))
                {
                    workOrderModel.ActualFinishDate = null;
                }
                else
                {
                    workOrderModel.ActualFinishDate = wo.ActualFinishDate;
                }
                workOrderModel.CompleteComments = wo.CompleteComments;
                workOrderModel.Personnels = wo.Personnels;
                workOrderModel.AssetGroup1ClientlookupId = wo.AssetGroup1ClientlookupId;
                workOrderModel.AssetGroup2ClientlookupId = wo.AssetGroup2ClientlookupId;
                workOrderModel.AssetGroup3ClientlookupId = wo.AssetGroup3ClientlookupId;
                workOrderModel.TotalCount = wo.TotalCount;
                workOrderModel.ScheduledDuration = wo.ScheduledDuration;
                workOrderModel.ActualDuration = wo.ActualDuration;
                workOrderModel.SourceType = wo.SourceType;
                workOrderModel.AssetGroup1AdvSearchId = string.Join(",", AssetGroup1Id ?? new List<string>());
                workOrderModel.AssetGroup2AdvSearchId = string.Join(",", AssetGroup2Id ?? new List<string>());
                workOrderModel.AssetGroup3AdvSearchId = string.Join(",", AssetGroup3Id ?? new List<string>());
                if (wo.RequiredDate != null && wo.RequiredDate == default(DateTime))
                {
                    workOrderModel.RequiredDate = null;
                }
                else
                {
                    workOrderModel.RequiredDate = wo.RequiredDate;
                }
                workOrderModel.ProjectClientLookupId = wo.ProjectClientLookupId;//V2-850
                workOrderModel.DownRequired = wo.DownRequired;//V2-892
                workOrderModel.PlannerFullName = wo.PlannerFullName;//V2-1078
                workOrderModelList.Add(workOrderModel);
            }

            return workOrderModelList;
        }
        public List<WorkOrderModel> populateWODetailsForDashboard(int CustomQueryDisplayId, out Dictionary<string, Dictionary<string, string>> lookupLists, int skip = 0,
           int length = 0, string orderbycol = "", string orderDir = "", string workOrderLookUpId = "", string description = "", string ChargeTo = "", string ChargeToName = "",
            string status = "", string _created = "", string assigned = "",
           string _scheduled = "", string _actualFinish = "", string complete = "")
        {
            WorkOrder workorder = new WorkOrder();
            WorkOrderModel workOrderModel;
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<WorkOrderModel> workOrderModelList = new List<WorkOrderModel>();

            workorder.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            workorder.CustomQueryDisplayId = CustomQueryDisplayId;
            workorder.OrderbyColumn = orderbycol;
            workorder.OrderBy = orderDir;
            workorder.OffSetVal = skip;
            workorder.NextRow = length;
            workorder.ClientLookupId = workOrderLookUpId;
            workorder.Description = description;
            workorder.ChargeToClientLookupId = ChargeTo;
            workorder.ChargeTo_Name = ChargeToName;
            workorder.Status = status;
            workorder.Created = _created;
            workorder.Assigned = assigned;
            workorder.Scheduled = _scheduled;
            workorder.ActualFinish = _actualFinish;
            workorder.Completed = complete;

            workorder.WRDashboardRetrieveAllForSearch(this.userData.DatabaseKey, userData.Site.TimeZone);
            lookupLists = new Dictionary<string, Dictionary<string, string>>();
            lookupLists.Add("status", workorder.utilityAdd.list1.ToDictionary(key => key, value => value));
            List<WorkOrder> woList = workorder.listOfWO;
            foreach (var wo in woList)
            {
                workOrderModel = new WorkOrderModel();
                workOrderModel.WorkOrderId = wo.WorkOrderId;
                workOrderModel.ClientLookupId = wo.ClientLookupId;
                workOrderModel.Description = wo.Description;
                workOrderModel.ChargeTo_Name = wo.ChargeTo_Name;
                workOrderModel.ChargeToId = wo.ChargeToId;
                workOrderModel.ChargeToClientLookupId = wo.ChargeToClientLookupId;
                workOrderModel.Status = wo.Status;

                if (wo.CreateDate != null && wo.CreateDate == default(DateTime))
                {
                    workOrderModel.CreateDate = null;
                }
                else
                {
                    workOrderModel.CreateDate = wo.CreateDate;
                }
                workOrderModel.Assigned = wo.Assigned;
                if (wo.ScheduledFinishDate != null && wo.ScheduledFinishDate == default(DateTime))
                {
                    workOrderModel.CompleteDate = null;
                }
                else
                {
                    workOrderModel.CompleteDate = null;
                }

                if (wo.CompleteDate != null && wo.CompleteDate == default(DateTime))
                {
                    workOrderModel.CompleteDate = null;
                }
                else
                {
                    workOrderModel.CompleteDate = wo.CompleteDate;
                }
                workOrderModel.TotalCount = wo.TotalCount;
                workOrderModelList.Add(workOrderModel);
            }
            return workOrderModelList;
        }
        #endregion
        #region Old Pattern
        public List<WorkOrderModel> populateWODetails(WorkOrderPrintModel workOrderPrintModel)
        {
            WorkOrder workorder = new WorkOrder();
            WorkOrderModel workOrderModel;
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<WorkOrderModel> workOrderModelList = new List<WorkOrderModel>();
            List<DataContracts.LookupList> Type = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> Shift = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> Failure = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> Priority = new List<DataContracts.LookupList>();

            workorder.SiteId = userData.DatabaseKey.User.DefaultSiteId;

            workorder.ClientId = userData.DatabaseKey.Client.ClientId;
            workorder.DateRange = userData.Site.TimeZone;
            workorder.ChargeTo = workOrderPrintModel.ChargeToClientLookupId;
            workorder.CustomQueryDisplayId = workOrderPrintModel.CustomQueryDisplayId;
            workorder.OrderbyColumn = workOrderPrintModel.OrderbyColumn;
            workorder.OrderBy = workOrderPrintModel.OrderBy;
            workorder.OffSetVal = workOrderPrintModel.OffSetVal ?? 0;
            workorder.NextRow = workOrderPrintModel.NextRow;
            workorder.ClientLookupId = workOrderPrintModel.ClientLookupId;
            workorder.Description = workOrderPrintModel.Description;
            workorder.ChargeToClientLookupId = workOrderPrintModel.ChargeToClientLookupId;
            workorder.ChargeTo_Name = workOrderPrintModel.ChargeTo_Name;
            workorder.Type = workOrderPrintModel.Type;
            workorder.Status = workOrderPrintModel.Status;
            workorder.Shift = workOrderPrintModel.Shift;
            workorder.AssetGroup1ClientlookupId = workOrderPrintModel.AssetGroup1ClientlookupId;
            workorder.AssetGroup2ClientlookupId = workOrderPrintModel.AssetGroup2ClientlookupId;
            workorder.AssetGroup3ClientlookupId = workOrderPrintModel.AssetGroup3ClientlookupId;
            //<!--(Added on 25/06/2020)-->
            //workorder.AssetGroup1Id = workOrderPrintModel.AssetGroup1Id;
            //workorder.AssetGroup2Id = workOrderPrintModel.AssetGroup2Id;
            //workorder.AssetGroup3Id = workOrderPrintModel.AssetGroup3Id;

            workorder.AssetGroup1AdvSearchId = workOrderPrintModel.AssetGroup1AdvSearchId;
            workorder.AssetGroup2AdvSearchId = workOrderPrintModel.AssetGroup2AdvSearchId;
            workorder.AssetGroup3AdvSearchId = workOrderPrintModel.AssetGroup3AdvSearchId;
            //<!--(Added on 25/06/2020)-->
            workorder.Priority = workOrderPrintModel.Priority;
            workorder.Creator = workOrderPrintModel.Creator;
            workorder.StartCreateDate = workOrderPrintModel.StartCreateDate;
            workorder.EndCreateDate = workOrderPrintModel.EndCreateDate;
            workorder.Assigned = workOrderPrintModel.Assigned;
            workorder.StartScheduledDate = workOrderPrintModel.StartScheduled;
            workorder.EndScheduledDate = workOrderPrintModel.EndScheduled;
            workorder.FailureCode = workOrderPrintModel.FailureCode;
            workorder.ActualDuration = workOrderPrintModel.ActualDuration;           
            workorder.StartActualFinishDateVw = workOrderPrintModel.CompleteStartDateVw;
            workorder.EndActualFinishDateVw = workOrderPrintModel.CompleteEndDateVw;
            workorder.StartActualFinishDate = workOrderPrintModel.StartActualFinish;
            workorder.EndActualFinishDate = workOrderPrintModel.EndActualFinish;
            //V2-364
            workorder.CreateStartDateVw = workOrderPrintModel.CreateStartDateVw;
            workorder.CreateEndDateVw = workOrderPrintModel.CreateEndDateVw;
            workorder.CompleteDate = null;
            workorder.SearchText = workOrderPrintModel.TxtSearchval;
            workorder.LoggedInUserPEID = this.userData.DatabaseKey.Personnel.PersonnelId;
            workorder.SourceType = workOrderPrintModel.SourceType;//<!--Added on 23/06/2020-->


            workorder.RetrieveV2(this.userData.DatabaseKey, userData.Site.TimeZone);
            List<WorkOrder> woList = workorder.listOfWO;
            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                Type = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_TYPE).ToList();
                Shift = AllLookUps.Where(x => x.ListName == LookupListConstants.Shift).ToList();
                Failure = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_FAILURE).ToList();
                Priority = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_Priority).ToList();
            }

            foreach (var wo in woList)
            {
                workOrderModel = new WorkOrderModel();
                workOrderModel.WorkOrderId = wo.WorkOrderId;
                workOrderModel.ClientLookupId = wo.ClientLookupId;
                workOrderModel.Description = wo.Description;
                workOrderModel.ChargeTo_Name = wo.ChargeTo_Name;
                workOrderModel.AssetLocation = wo.AssetLocation;
                workOrderModel.ChargeToId = wo.ChargeToId;
                workOrderModel.ChargeToClientLookupId = wo.ChargeToClientLookupId;
                workOrderModel.SourceType = wo.SourceType;

                if (Type != null && Type.Any(cus => cus.ListValue == wo.Type))
                {
                    workOrderModel.Type = Type.Where(x => x.ListValue == wo.Type).Select(x => x.Description).First();
                }

                workOrderModel.Status = wo.Status;

                if (Shift != null && Shift.Any(cus => cus.ListValue == wo.Shift))
                {
                    workOrderModel.Shift = Shift.Where(x => x.ListValue == wo.Shift).Select(x => x.Description).First();
                }
                workOrderModel.AssetGroup1ClientlookupId = wo.AssetGroup1ClientlookupId;
                workOrderModel.AssetGroup2ClientlookupId = wo.AssetGroup2ClientlookupId;
                workOrderModel.AssetGroup3ClientlookupId = wo.AssetGroup3ClientlookupId;
                //<!--(Added on 25/06/2020)-->
                workOrderModel.AssetGroup1Id = wo.AssetGroup1Id;
                workOrderModel.AssetGroup2Id = wo.AssetGroup2Id;
                workOrderModel.AssetGroup3Id = wo.AssetGroup3Id;
                //<!--(Added on 25/06/2020)-->
                workOrderModel.Priority = wo.Priority;
                if (Priority != null && Priority.Any(cus => cus.ListValue == wo.Priority))
                {
                    workOrderModel.Priority = Priority.Where(x => x.ListValue == wo.Priority).Select(x => x.Description).First();
                }
                if (wo.CreateDate != null && wo.CreateDate == default(DateTime))
                {
                    workOrderModel.CreateDate = null;
                }
                else
                {
                    workOrderModel.CreateDate = wo.CreateDate;
                }

                workOrderModel.Creator = wo.Creator;
                workOrderModel.Assigned = wo.Assigned;
                if (wo.ScheduledStartDate != null && wo.ScheduledStartDate == default(DateTime))
                {
                    workOrderModel.ScheduledStartDate = null;
                }
                else
                {
                    workOrderModel.ScheduledStartDate = wo.ScheduledStartDate;
                }

                workOrderModel.FailureCode = wo.FailureCode;
                if (Failure != null && Failure.Any(cus => cus.ListValue == wo.FailureCode))
                {
                    workOrderModel.FailureCode = Failure.Where(x => x.ListValue == wo.FailureCode).Select(x => x.Description).First();
                }
                if (wo.ActualFinishDate != null && wo.ActualFinishDate == default(DateTime))
                {
                    workOrderModel.ActualFinishDate = null;
                }
                else
                {
                    workOrderModel.ActualFinishDate = wo.ActualFinishDate;
                }
                workOrderModel.ActualDuration = wo.ActualDuration;
                workOrderModel.CompleteComments = wo.CompleteComments;
                workOrderModel.AssignedFullName = wo.AssignedFullName;
                if (wo.RequiredDate != null && wo.RequiredDate == default(DateTime))
                {
                    workOrderModel.RequiredDate = null;
                }
                else
                {
                    workOrderModel.RequiredDate = wo.RequiredDate;
                }
                workOrderModelList.Add(workOrderModel);
            }

            return workOrderModelList;
        }
        public List<WorkOrderModel> populateWODetailsForDashboardPrint(int CustomQueryDisplayId)
        {
            WorkOrder workorder = new WorkOrder();
            WorkOrderModel workOrderModel;
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<WorkOrderModel> workOrderModelList = new List<WorkOrderModel>();
            List<DataContracts.LookupList> Type = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> Shift = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> Failure = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> Priority = new List<DataContracts.LookupList>();

            workorder.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            workorder.CustomQueryDisplayId = CustomQueryDisplayId;
            List<WorkOrder> woList = workorder.WorkOrder_WRDashboardRetrieveAllForPrint_V2(this.userData.DatabaseKey, userData.Site.TimeZone);

            foreach (var wo in woList)
            {
                workOrderModel = new WorkOrderModel();
                workOrderModel.WorkOrderId = wo.WorkOrderId;
                workOrderModel.ClientLookupId = wo.ClientLookupId;
                workOrderModel.Description = wo.Description;
                workOrderModel.ChargeTo_Name = wo.ChargeTo_Name;
                workOrderModel.ChargeToId = wo.ChargeToId;
                workOrderModel.ChargeToClientLookupId = wo.ChargeToClientLookupId;

                workOrderModel.Status = wo.Status;
                workOrderModel.CreateDate = wo.CreateDate;

                if (workOrderModel.CreateDate != null && workOrderModel.CreateDate == default(DateTime))
                {
                    workOrderModel.CreateDate = null;
                }
                else
                {
                    workOrderModel.CreateDate = workOrderModel.CreateDate;
                }

                workOrderModel.Assigned = wo.Assigned;
                workOrderModel.ScheduledFinishDate = wo.ScheduledFinishDate;
                if (workOrderModel.ScheduledFinishDate != null && workOrderModel.ScheduledFinishDate == default(DateTime))
                {
                    workOrderModel.ScheduledFinishDate = null;
                }
                workOrderModel.CompleteDate = wo.CompleteDate;
                if (workOrderModel.CompleteDate != null && workOrderModel.CompleteDate == default(DateTime))
                {
                    workOrderModel.CompleteDate = null;
                }

                workOrderModelList.Add(workOrderModel);
            }
            return workOrderModelList;
        }
        #endregion
        public WorkOrderModel getWorkOderDetailsById(long workOrderId)
        {
            WorkOrderModel workOrderModel = new WorkOrderModel();
            WorkOrder workorder = new WorkOrder()
            {
                ClientId = _dbKey.Client.ClientId,
                WorkOrderId = workOrderId
            };
            workorder.RetrieveByPKForeignKeys(this.userData.DatabaseKey, userData.Site.TimeZone);
            workOrderModel = initializeControls(workorder);
            return workOrderModel;
        }
        public List<DataModel> GetLookupList_Account()
        {
            LookUpListModel model = new LookUpListModel();
            DataModel dModel;
            Account Account = new Account()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId
            };
            List<Account> acc = Account.RetrieveClientLookupIdBySearchCriteria(userData.DatabaseKey);

            foreach (var ac in acc)
            {
                dModel = new DataModel();
                dModel.AccountId = ac.AccountId;
                dModel.Account = ac.ClientLookupId;
                dModel.Name = ac.Name;
                model.data.Add(dModel);
            }
            return model.data;
        }

        public WorkOrderModel initializeControlsWOPrint(WorkOrder obj)
        {
            WorkOrderModel objworkorder = new WorkOrderModel();
            objworkorder.ClientLookupId = obj.ClientLookupId;
            objworkorder.WorkOrderId = obj.WorkOrderId;
            objworkorder.Status = obj?.Status ?? string.Empty;
            objworkorder.Type = obj?.Type ?? string.Empty;
            objworkorder.DownRequired = obj.DownRequired;
            objworkorder.Description = obj?.Description ?? string.Empty;
            objworkorder.CreateBy_PersonnelName = obj?.CreateBy_PersonnelName ?? string.Empty;
            objworkorder.ScheduledDuration = obj.ScheduledDuration;
            objworkorder.RequiredDate = obj?.RequiredDate ?? DateTime.MinValue;
            objworkorder.Assigned = obj?.Assigned ?? string.Empty;
            if (obj.AssignedFullName != null && obj.AssignedFullName.Length > 0)
            {
                objworkorder.AssignedFullName = obj.AssignedFullName.Trim().TrimEnd(',');
            }
            else
            {
                objworkorder.AssignedFullName = "";
            }
            objworkorder.ScheduledStartDate = obj?.ScheduledStartDate ?? DateTime.MinValue;
            objworkorder.CompleteBy = obj?.CompleteBy_PersonnelName ?? string.Empty;
            objworkorder.CompleteBy_PersonnelId = obj.CompleteBy_PersonnelId;
            objworkorder.CompleteComments = obj?.CompleteComments ?? string.Empty;
            objworkorder.CompleteDate = obj?.CompleteDate ?? DateTime.MinValue;
            objworkorder.ChargeTo_Name = obj?.ChargeTo_Name ?? string.Empty;
            objworkorder.AssetLocation = obj?.AssetLocation ?? string.Empty;
            objworkorder.ChargeToClientLookupId = obj?.ChargeToClientLookupId ?? string.Empty;
            objworkorder.Createby = obj?.Createby ?? string.Empty;
            objworkorder.SiteInformation = userData.DatabaseKey.Client.WOPrintMessage.ToString();
            objworkorder.CompleteBy_PersonnelClientLookupId = obj?.CompleteBy_PersonnelClientLookupId ?? string.Empty;
            objworkorder.SignoffBy_PersonnelClientLookupId = obj?.SignoffBy_PersonnelClientLookupId ?? string.Empty;
            #region FoodSafety
            if (userData.DatabaseKey.Client.BusinessType.ToUpper() == BusinessTypeConstants.FoodServices
            && obj.Status == WorkOrderStatusConstants.Complete
            && obj.SignoffBy_PersonnelId > 0)
            {
                objworkorder.IsFoodSafetyShow = true;
            }
            #endregion
            return objworkorder;
        }
        public WorkOrderModel initializeControls(WorkOrder obj)
        {
            WorkOrderModel objworkorder = new WorkOrderModel();
            objworkorder.ClientLookupId = obj.ClientLookupId;
            objworkorder.WorkOrderId = obj.WorkOrderId;
            objworkorder.Status = obj?.Status ?? string.Empty;
            objworkorder.Shift = obj?.Shift ?? string.Empty;
            objworkorder.Type = obj?.Type ?? string.Empty;
            objworkorder.DownRequired = obj.DownRequired;
            objworkorder.Description = obj?.Description ?? string.Empty;
            objworkorder.Priority = obj.Priority;
            objworkorder.Labor_AccountId = obj.Labor_AccountId;
            objworkorder.CreateBy_PersonnelName = obj?.CreateBy_PersonnelName ?? string.Empty;
            objworkorder.CreateDate = obj?.CreateDate ?? DateTime.MinValue;
            objworkorder.ChargeType = obj?.ChargeType ?? string.Empty;
            objworkorder.ChargeToId = obj.ChargeToId;
            objworkorder.ScheduledDuration = obj.ScheduledDuration;
            objworkorder.ActualDuration = obj.ActualDuration;
            objworkorder.FailureCode = obj?.FailureCode ?? string.Empty;
            objworkorder.RequiredDate = obj?.RequiredDate ?? DateTime.MinValue;

            objworkorder.Creator = obj?.Creator ?? string.Empty;

            objworkorder.Assigned = obj?.Assigned ?? string.Empty;//---V2-293--//
            if (obj.AssignedFullName != null && obj.AssignedFullName.Length > 0)
            {
                objworkorder.AssignedFullName = obj.AssignedFullName.Trim().TrimEnd(',');
            }
            else
            {
                objworkorder.AssignedFullName = "";
            }
            //objworkorder.AssignedFullName = obj?.AssignedFullName.TrimEnd(',') ?? string.Empty;//--V2-293--//
            objworkorder.ScheduledStartDate = obj?.ScheduledStartDate ?? DateTime.MinValue;

            objworkorder.ActualFinishDate = obj?.ActualFinishDate ?? DateTime.MinValue;
            objworkorder.CompleteBy = obj?.CompleteBy_PersonnelName ?? string.Empty;
            objworkorder.CompleteBy_PersonnelId = obj.CompleteBy_PersonnelId;
            objworkorder.CompleteComments = obj?.CompleteComments ?? string.Empty;
            objworkorder.CompleteDate = obj?.CompleteDate ?? DateTime.MinValue;
            objworkorder.ChargeTo_Name = obj?.ChargeTo_Name ?? string.Empty;
            objworkorder.AssetLocation = obj?.AssetLocation ?? string.Empty;
            objworkorder.ChargeToClientLookupId = obj?.ChargeToClientLookupId ?? string.Empty;

            objworkorder.Createby = obj?.Createby ?? string.Empty;
            objworkorder.ModifyBy = obj?.ModifyBy ?? string.Empty;
            objworkorder.ModifyDate = obj?.ModifyDate ?? DateTime.MinValue;
            objworkorder.SourceType = obj?.SourceType ?? string.Empty;
            objworkorder.RequestorName = obj?.RequestorName ?? string.Empty;
            objworkorder.RequestorPhone = obj?.RequestorPhoneNumber ?? string.Empty;
            objworkorder.RequestorEmail = obj?.RequestorEmail ?? string.Empty;
            objworkorder.SiteInformation = userData.DatabaseKey.Client.WOPrintMessage.ToString();
            objworkorder.WorkAssigned_PersonnelClientLookupId = obj?.WorkAssigned_PersonnelClientLookupId ?? string.Empty;
            objworkorder.CompleteBy_PersonnelClientLookupId = obj?.CompleteBy_PersonnelClientLookupId ?? string.Empty;
            objworkorder.SignoffBy_PersonnelClientLookupId = obj?.SignoffBy_PersonnelClientLookupId ?? string.Empty;

            objworkorder.Personnels = obj?.Personnels ?? string.Empty;//---V2-293---//
            //objworkorder.AssignedFullName = obj?.AssignedFullName ?? string.Empty;//---V2-293---//

            objworkorder.WorkAssigned_PersonnelId = obj.WorkAssigned_PersonnelId;
            objworkorder.PartsOnOrder = obj.PartsOnOrder;
            #region FoodSafety
            if (userData.DatabaseKey.Client.BusinessType.ToUpper() == BusinessTypeConstants.FoodServices
            && obj.Status == WorkOrderStatusConstants.Complete
            && obj.SignoffBy_PersonnelId > 0)
            {
                objworkorder.SignoffBy_PersonnelClientLookupIdSecond = obj.SignoffBy_PersonnelClientLookupIdName;
                objworkorder.IsFoodSafetyShow = true;
            }
            //V2-463
            if (obj.EquipDownDate != null && obj.EquipDownDate != default(DateTime))
            {
                objworkorder.EquipDownDate = obj.EquipDownDate;
            }
            else
            {
                objworkorder.EquipDownDate = null;
            }

            objworkorder.EquipDownHours = obj?.EquipDownHours ?? 0;
            #endregion
            objworkorder.ProjectClientLookupId = obj?.ProjectClientLookupId ?? string.Empty; //V2-626
            objworkorder.ProjectClientLookupId = obj?.ProjectClientLookupId ?? string.Empty; //V2-626
            objworkorder.ClientOnPremise = userData.DatabaseKey.Client.OnPremise;
            return objworkorder;
        }
        public WorkOrder editWorkOrder(WorkOrderModel workOrderModel)
        {
            WorkOrder wo = new WorkOrder()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                WorkOrderId = workOrderModel.WorkOrderId,
                SiteId = this.userData.DatabaseKey.Personnel.SiteId
            };
            wo.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);

            wo.ClientLookupId = workOrderModel?.ClientLookupId ?? string.Empty;
            if (workOrderModel.SourceType != WorkOrderSourceTypes.PreventiveMaint)
            {
                wo.Type = workOrderModel?.Type ?? string.Empty;
            }
            wo.Shift = workOrderModel?.Shift ?? string.Empty;
            wo.DownRequired = workOrderModel?.DownRequired ?? false;
            wo.ActionCode = workOrderModel?.ActionCode ?? string.Empty;
            wo.ActualDuration = workOrderModel?.ActualDuration ?? 0;
            wo.ActualFinishDate = workOrderModel.ActualFinishDate;
            wo.ActualStartDate = workOrderModel.ActualStartDate;
            wo.ChargeToClientLookupId = workOrderModel?.ChargeToClientLookupId ?? string.Empty;
            // Commented for V2-608
            //wo.ChargeType = workOrderModel?.ChargeType ?? string.Empty;
            wo.ChargeType = ChargeType.Equipment;
            wo.ChargeToId = workOrderModel.ChargeToId ?? 0;
            wo.Labor_AccountId = workOrderModel?.Labor_AccountId ?? 0;
            wo.Material_AccountId = workOrderModel?.Material_AccountId ?? 0;
            wo.CompleteBy_PersonnelId = workOrderModel?.CompleteBy_PersonnelId ?? 0;
            wo.Description = workOrderModel?.Description ?? string.Empty;
            wo.FailureCode = workOrderModel?.FailureCode ?? string.Empty;
            wo.Location = workOrderModel?.Location ?? string.Empty;
            wo.Priority = workOrderModel?.Priority ?? string.Empty;
            wo.ReasonNotDone = workOrderModel?.ReasonNotDone ?? string.Empty;
            wo.RequiredDate = workOrderModel.RequiredDate ?? default(DateTime);
            wo.Section = workOrderModel?.Section ?? string.Empty;
            wo.SignOffDate = workOrderModel.SignOffDate;
            wo.SignoffBy_PersonnelId = workOrderModel.SignoffBy_PersonnelId;
            wo.SourceType = workOrderModel?.SourceType ?? string.Empty;
            wo.Status = workOrderModel?.Status ?? string.Empty;
            wo.ScheduledStartDate = workOrderModel.ScheduledStartDate;
            wo.ScheduledFinishDate = workOrderModel.ScheduledFinishDate;
            wo.ScheduledDuration = workOrderModel?.ScheduledDuration ?? 0;
            wo.CompleteComments = workOrderModel?.CompleteComments ?? string.Empty;
            wo.CreateMode = false;

            wo.UpdateByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);

            return wo;
        }
        public WorkOrder addWorkOrder(WorkOrderModel workOrderModel)
        {
            WorkOrder wo = new WorkOrder()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.Personnel.SiteId
            };

            if (workOrderModel.ClientLookupId == null && WorkOrderStatusConstants.Wo_AutoGenerateEnabled)
            {
                newClientlookupId = CustomSequentialId.GetNextId(userData.DatabaseKey, AutoGenerateKey.WO_Annual, userData.DatabaseKey.User.DefaultSiteId, "");
            }

            wo.ClientLookupId = newClientlookupId;
            wo.Creator_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
            wo.Creator_PersonnelClientLookupId = userData.DatabaseKey.Personnel.ClientLookupId;
            wo.ApproveBy_PersonnelClientLookupId = userData.DatabaseKey.Personnel.ClientLookupId;
            wo.ApproveBy_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
            wo.ApproveDate = DateTime.UtcNow;
            wo.CreateMode = true;
            wo.Type = workOrderModel?.Type ?? string.Empty;
            wo.Shift = workOrderModel?.Shift ?? string.Empty;
            wo.DownRequired = workOrderModel?.DownRequired ?? false;
            wo.ActionCode = workOrderModel?.ActionCode ?? string.Empty;
            wo.ActualDuration = workOrderModel?.ActualDuration ?? 0;
            wo.ActualFinishDate = workOrderModel.ActualFinishDate;
            wo.ChargeToId = workOrderModel?.ChargeToId ?? 0;
            wo.ChargeType = ChargeType.Equipment;
            wo.Labor_AccountId = workOrderModel?.Labor_AccountId ?? 0;
            wo.Material_AccountId = workOrderModel?.Material_AccountId ?? 0;
            wo.CompleteBy_PersonnelId = workOrderModel?.CompleteBy_PersonnelId ?? 0;
            wo.Description = workOrderModel?.Description ?? string.Empty;
            wo.FailureCode = workOrderModel?.FailureCode ?? string.Empty;
            wo.Location = workOrderModel?.Location ?? string.Empty;
            wo.Priority = workOrderModel?.Priority ?? string.Empty;
            wo.ReasonNotDone = workOrderModel?.ReasonNotDone ?? string.Empty;
            wo.RequiredDate = workOrderModel.RequiredDate ?? default(DateTime);
            wo.Section = workOrderModel?.Section ?? string.Empty;
            wo.SourceType = workOrderModel?.SourceType ?? string.Empty;
            wo.Status = WorkOrderStatusConstants.Approved;
            wo.ScheduledStartDate = workOrderModel.ScheduledStartDate;
            wo.ScheduledFinishDate = workOrderModel.ScheduledFinishDate;
            wo.ScheduledDuration = workOrderModel?.ScheduledDuration ?? 0;
            wo.CompleteComments = workOrderModel?.CompleteComments ?? string.Empty;
            wo.ChargeToClientLookupId = workOrderModel?.ChargeToClientLookupId ?? string.Empty;
            wo.CreateByPKForeignKeys(this.userData.DatabaseKey, userData.Site.TimeZone);
            if (wo.ErrorMessages.Count == 0)
            {
                CreateEventLog(wo.WorkOrderId, WorkOrderEvents.Create);
                CreateEventLog(wo.WorkOrderId, WorkOrderEvents.Approved);
            }
            return wo;
        }
        private void CreateEventLog(Int64 WOId, string Status)
        {
            WorkOrderEventLog log = new WorkOrderEventLog();
            log.ClientId = userData.DatabaseKey.Client.ClientId;
            log.SiteId = userData.DatabaseKey.Personnel.SiteId;
            log.WorkOrderId = WOId;
            log.Event = Status;
            log.TransactionDate = DateTime.UtcNow;
            log.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            log.Comments = "";
            log.SourceId = 0;
            log.Create(userData.DatabaseKey);
        }
        private void CreateEventLog(Int64 WOId, string Status, string comment)
        {
            WorkOrderEventLog log = new WorkOrderEventLog();
            log.ClientId = userData.DatabaseKey.Client.ClientId;
            log.SiteId = userData.DatabaseKey.Personnel.SiteId;
            log.WorkOrderId = WOId;
            log.Event = Status;
            log.TransactionDate = DateTime.UtcNow;
            log.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            log.Comments = comment;
            log.SourceId = 0;
            log.Create(userData.DatabaseKey);
        }

        public WorkOrder CompleteWO(WorkOrderModel workOrderModel)
        {
            WorkOrder wo = new WorkOrder()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                WorkOrderId = workOrderModel.WorkOrderId,
                SiteId = this.userData.DatabaseKey.Personnel.SiteId
            };
            wo.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);
            wo.CompleteComments = workOrderModel?.CompleteComments ?? string.Empty;

            // Update with completion information
            wo.Status = WorkOrderStatusConstants.Complete;
            wo.CompleteBy_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;

            if (this.userData.DatabaseKey.Client.BusinessType.ToUpper() == BusinessTypeConstants.FoodServices)
            {
                wo.SignOffDate = DateTime.UtcNow;
                wo.SignoffBy_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
                wo.SignoffBy_PersonnelClientLookupId = userData.DatabaseKey.Personnel.ClientLookupId;
            }
            wo.CompleteDate = DateTime.UtcNow;
            wo.CompleteBy_PersonnelClientLookupId = userData.DatabaseKey.Personnel.ClientLookupId; // Converts to personnel id in the sp
            if (wo.ActualFinishDate == null || wo.ActualFinishDate == DateTime.MinValue)
            {
                wo.ActualFinishDate = DateTime.UtcNow.ConvertFromUTCToUser(userData.Site.TimeZone);
            }
            wo.CompleteWorkOrder(this.userData.DatabaseKey, userData.Site.TimeZone);
            CreateEventLog(wo.WorkOrderId, WorkOrderEvents.Complete, wo.CompleteComments);
            List<long> wos = new List<long>() { wo.WorkOrderId };

            if (wo.ErrorMessages.Count == 0)
            {
                Task task1 = Task.Factory.StartNew(() => this.SendAlert(wos));
                return wo;

            }
            return wo;
        }

        private void SendAlert(List<long> wos)
        {
            ProcessAlert objAlert = new ProcessAlert(userData);
            objAlert.CreateAlert<WorkOrder>(AlertTypeEnum.WorkOrderComplete, wos);
        }



        #region Task

        public List<WorkOrderTask> PopulateTask(long workOrderId, ref bool ActionSecurity)
        {
            WorkOrderTask woTask = new WorkOrderTask()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ParentSiteId = this.userData.DatabaseKey.Personnel.SiteId,
                WorkOrderId = workOrderId
            };
            List<WorkOrderTask> obj = new List<WorkOrderTask>();
            obj = WorkOrderTask.RetriveByWorkOrderId(this.userData.DatabaseKey, woTask);
            ActionSecurity = userData.Security.WorkOrders.Edit;
            return obj;
        }
        #region Cancel Task
        public string CancelTask(string taskList, string cancelReason, ref int successCount)
        {

            string[] cancelArray = null;
            if (taskList != null)
            {
                cancelArray = taskList.Split(',');
            }
            string result = string.Empty;

            foreach (var item in cancelArray)
            {
                WorkOrderTask workordertask = new WorkOrderTask()
                {
                    ClientId = this.userData.DatabaseKey.Client.ClientId,
                    ParentSiteId = this.userData.DatabaseKey.Personnel.SiteId,
                    WorkOrderTaskId = Convert.ToInt64(item)
                };
                workordertask.RetriveByPKForeignKeys(userData.DatabaseKey);
                if (workordertask.Status != WorkOrderStatusConstants.Canceled)
                {
                    workordertask.CancelReason = cancelReason;
                    workordertask.CompleteBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
                    workordertask.CompleteDate = System.DateTime.UtcNow;
                    workordertask.Status = WorkOrderStatusConstants.Canceled; //"Canceled";
                    workordertask.UpdateByPKForeignKeys(this.userData.DatabaseKey);
                    successCount++;
                }

            }
            if (successCount > 0)
            {
                return "success";
            }
            else
            {
                return "failed";
            }
        }
        #endregion

        #region Complete Task
        public string CompleteTask(string taskList, ref int successCount)
        {
            string[] array = taskList.Split(',');
            string result = string.Empty;
            foreach (var item in array)
            {
                WorkOrderTask workordertask = new WorkOrderTask()
                {
                    ClientId = this.userData.DatabaseKey.Client.ClientId,
                    ParentSiteId = this.userData.DatabaseKey.User.DefaultSiteId,
                    WorkOrderTaskId = Convert.ToInt64(item)
                };
                workordertask.RetriveByPKForeignKeys(userData.DatabaseKey);
                if (workordertask.Status != WorkOrderStatusConstants.Complete)
                {
                    workordertask.CompleteBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
                    workordertask.CompleteBy_PersonnelClientLookupId = userData.DatabaseKey.Personnel.ClientLookupId;
                    workordertask.CompleteDate = System.DateTime.UtcNow;
                    workordertask.Status = WorkOrderStatusConstants.Complete;
                    workordertask.UpdateByPKForeignKeys(this.userData.DatabaseKey);
                    successCount++;
                }
            }

            if (successCount > 0)
            {
                return "success";
            }
            else
            {
                return "failed";
            }
        }

        #endregion

        #region Reopen Task
        public string ReopenTask(string taskList, ref int successCount)
        {
            string[] array = taskList.Split(',');
            string result = string.Empty;
            for (int i = 0; i < array.Length; i++)
            {
                var Statusvalue = "";
                var TaskId = "";
                if (i == 0 || i % 2 == 0)
                {
                    TaskId = array[i];
                    Statusvalue = array[i + 1];
                }
                if (Statusvalue == WorkOrderStatusConstants.Complete || Statusvalue == WorkOrderStatusConstants.Canceled)
                {
                    WorkOrderTask workordertask = new WorkOrderTask()
                    {
                        ClientId = this.userData.DatabaseKey.Client.ClientId,
                        ParentSiteId = this.userData.DatabaseKey.User.DefaultSiteId,
                        WorkOrderTaskId = Convert.ToInt64(TaskId)
                    };
                    workordertask.RetriveByPKForeignKeys(userData.DatabaseKey);
                    workordertask.CompleteBy_PersonnelId = 0;
                    workordertask.CompleteBy_PersonnelClientLookupId = string.Empty;
                    workordertask.CompleteDate = null;
                    workordertask.CancelReason = string.Empty;
                    workordertask.Status = WorkOrderStatusConstants.Approved;
                    workordertask.UpdateByPKForeignKeys(this.userData.DatabaseKey);
                    successCount++;
                }
            }
            if (successCount > 0)
            {
                return "success";
            }
            else
            {
                return "failed";
            }
        }

        #endregion

        #region Retrive Task
        public WorkOrderTask RetriveTaskDetails(long workOrderId, long _taskId)
        {
            WorkOrderTask woTask = new WorkOrderTask()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                WorkOrderId = workOrderId,
                WorkOrderTaskId = _taskId,
                ParentSiteId = this.userData.DatabaseKey.User.DefaultSiteId
            };
            woTask.RetriveByPKForeignKeys(userData.DatabaseKey);
            return woTask;
        }
        #endregion

        #region AddOrUpdate Task
        public List<String> AddOrUpdateTask(WorkOrderVM woVM, ref string Mode)
        {
            WorkOrderTask woTask = new WorkOrderTask();
            if (woVM.woTaskModel.WorkOrderTaskId != 0)
            {
                Mode = "update";
                woTask.ClientId = this.userData.DatabaseKey.Client.ClientId;
                woTask.WorkOrderId = woVM.woTaskModel.WorkOrderId;
                woTask.WorkOrderTaskId = woVM.woTaskModel.WorkOrderTaskId;
                woTask.ParentSiteId = this.userData.DatabaseKey.User.DefaultSiteId;
                woTask.RetriveByPKForeignKeys(userData.DatabaseKey);
                woTask.TaskNumber = woVM.woTaskModel.TaskNumber;
                woTask.Description = woVM.woTaskModel.Description;
                woTask.ChargeType = woVM.woTaskModel.ChargeType ?? string.Empty;
                woTask.ChargeToClientLookupId = woVM.woTaskModel.ChargeToClientLookupId;
                woTask.UpdateIndex = woVM.woTaskModel.updatedindex;
                woTask.ChargeToId = woVM.woTaskModel.ChargeToId;
                woTask.UpdateByPKForeignKeys(userData.DatabaseKey);
            }
            else
            {
                Mode = "add";
                woTask.ClientId = this.userData.DatabaseKey.Client.ClientId;
                woTask.WorkOrderId = woVM.woTaskModel.WorkOrderId;
                woTask.ParentSiteId = this.userData.DatabaseKey.User.DefaultSiteId;
                woTask.TaskNumber = woVM.woTaskModel.TaskNumber;
                woTask.Description = woVM.woTaskModel.Description;
                woTask.ChargeType = woVM.woTaskModel.ChargeType;
                woTask.ChargeToClientLookupId = woVM.woTaskModel.ChargeToClientLookupId;
                woTask.ChargeToId = woVM.woTaskModel.ChargeToId;
                woTask.CreateByPKForeignKeys(userData.DatabaseKey);
            }
            return woTask.ErrorMessages;
        }
        #endregion

        #region Delete Task
        public bool DeleteTask(long taskNumber)
        {
            try
            {
                WorkOrderTask wotask = new WorkOrderTask()
                {
                    WorkOrderTaskId = Convert.ToInt64(taskNumber)
                };
                wotask.Delete(userData.DatabaseKey);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #endregion

        #region Assignment
        public List<WorkOrderSchedule> PopulateAssignment(long workOrderId)
        {
            WorkOrderSchedule workorderschdule = new WorkOrderSchedule()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                WorkOrderId = workOrderId
            };
            List<WorkOrderSchedule> woAssgnList = workorderschdule.RetriveByWorkOrderId(userData.DatabaseKey);
            return woAssgnList;
        }
        #region AddOrUpdate Task
        public List<String> AddOrUpdateAssignment(WorkOrderVM woVM, ref string Mode)
        {
            WorkOrderSchedule wos = new WorkOrderSchedule();
            if (woVM.woAssignmentModel.WorkOrderSchedId != 0)
            {
                Mode = "update";
                wos.ClientId = userData.DatabaseKey.Client.ClientId;
                wos.WorkOrderId = woVM.woAssignmentModel.WorkOrderId;
                wos.WorkOrderSchedId = woVM.woAssignmentModel.WorkOrderSchedId;
                wos.Retrieve(userData.DatabaseKey);
                wos.ScheduledHours = woVM.woAssignmentModel.ScheduledHours ?? 0;
                wos.UpdateForWorkOrder(userData.DatabaseKey);
            }
            else
            {
                Mode = "add";
                wos.ClientId = userData.DatabaseKey.Client.ClientId;
                wos.WorkOrderId = woVM.woAssignmentModel.WorkOrderId;
                wos.PersonnelId = woVM.woAssignmentModel.PersonnelId;
                wos.ScheduledHours = woVM.woAssignmentModel.ScheduledHours ?? 0;
                wos.ScheduledStartDate = woVM.woAssignmentModel.ScheduledStartDate ?? default(DateTime);
                wos.CreateForWorkOrder(userData.DatabaseKey);
                CreateEventLog(wos.WorkOrderId, WorkOrderEvents.Scheduled, "");
            }
            return wos.ErrorMessages;
        }
        #endregion

        #region Retrive Task
        public WorkOrderSchedule RetriveAssignmentDetails(long WorkOrderID, long _assignmentId)
        {
            WorkOrderSchedule workorderschdule = new WorkOrderSchedule()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                WorkOrderId = WorkOrderID,
                WorkOrderSchedId = _assignmentId
            };
            workorderschdule.Retrieve(userData.DatabaseKey);
            return workorderschdule;
        }
        #endregion

        #region Delete Assignment
        public bool DeleteAssignment(long WorkOrderID, long _assignmentId)
        {
            try
            {
                WorkOrderSchedule workorderschdule = new WorkOrderSchedule()
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    WorkOrderId = WorkOrderID,
                    WorkOrderSchedId = _assignmentId
                };
                workorderschdule.RemoveRecord(userData.DatabaseKey);
                if (workorderschdule.RetriveByWorkOrderId(userData.DatabaseKey).Count == 0)
                {
                    CreateEventLog(workorderschdule.WorkOrderId, WorkOrderEvents.Approved, "");
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #endregion

        #region Event Log
        public List<EventLogModel> PopulateEventLog(long WorkOrderId)
        {
            EventLogModel objEventLogModel;
            List<EventLogModel> EventLogModelList = new List<EventLogModel>();

            WorkOrderEventLog log = new WorkOrderEventLog();
            List<WorkOrderEventLog> data = new List<WorkOrderEventLog>();
            log.ClientId = userData.DatabaseKey.Client.ClientId;
            log.SiteId = userData.DatabaseKey.Personnel.SiteId;
            log.WorkOrderId = WorkOrderId;
            data = log.RetriveByWorkOrderId(userData.DatabaseKey);

            if (data != null)
            {
                foreach (var item in data)
                {
                    objEventLogModel = new EventLogModel();
                    objEventLogModel.ClientId = item.ClientId;
                    objEventLogModel.SiteId = item.SiteId;
                    objEventLogModel.EventLogId = item.EventLogId;
                    objEventLogModel.ObjectId = item.WorkOrderId;
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

        #region Notes

        public WoNotesModel GetNoteById(long NotesId, long WorkOrderId, string ClientLookupId)
        {
            WoNotesModel objNotesModel = new WoNotesModel();
            Notes note = new Notes()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                NotesId = NotesId,
            };
            note.Retrieve(userData.DatabaseKey);
            objNotesModel.NotesId = note.NotesId;
            objNotesModel.updatedindex = note.UpdateIndex;
            objNotesModel.Subject = note.Subject;
            objNotesModel.Content = note.Content;
            objNotesModel.WorkOrderId = WorkOrderId;
            objNotesModel.ClientLookupId = ClientLookupId;
            return objNotesModel;
        }



        #endregion

        #region Estimate-Part
        public List<EstimatePart> populateEstimatedParts(long workOrderId)
        {
            EstimatePart estimatePart;
            List<EstimatePart> estimatedlList = new List<EstimatePart>();

            EstimatedCosts estimatecost = new EstimatedCosts()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ObjectId = workOrderId,
                ObjectType = SearchCategoryConstants.TBL_WORKORDER
            };
            List<EstimatedCosts> list = EstimatedCosts.EstimatedCostsRetrieveByObjectId_ForChild(this.userData.DatabaseKey, estimatecost);
            foreach (var ec in list)
            {
                estimatePart = new EstimatePart();
                estimatePart.EstimatedCostsId = ec.EstimatedCostsId;
                estimatePart.ClientLookupId = ec.PartClientLookupId;
                estimatePart.Description = ec.Description;
                estimatePart.UnitCost = ec.UnitCost;
                estimatePart.Quantity = ec.Quantity;
                estimatePart.Unit = ec.Unit;
                estimatePart.TotalCost = Math.Round(ec.TotalCost, 2);
                estimatePart.AccountClientLookupId = ec.AccountClientLookupId;
                estimatePart.CategoryId = ec.CategoryId;
                estimatePart.VendorClientLookupId = ec.VendorClientLookupId;
                estimatePart.AccountId = ec.AccountId;
                estimatePart.VendorId = ec.VendorId;
                estimatePart.PartCategoryMasterId = ec.UNSPSC;
                estimatePart.PartCategoryClientLookupId = ec.PartCategoryClientLookupId;
                estimatePart.Status = ec.Status;
                estimatePart.PurchaseRequestId = ec.PurchaseRequestId;
                estimatedlList.Add(estimatePart);
            }

            return estimatedlList;
        }
        public EstimatedCosts AddEstimatePart(WorkOrderVM WoVM)
        {
            string part_ClientLookupId = WoVM.estimatePart.ClientLookupId;
            Part pt = new Part { ClientId = _dbKey.Client.ClientId, SiteId = _dbKey.User.DefaultSiteId, ClientLookupId = part_ClientLookupId };
            pt.RetrieveByClientLookUpIdNUPCCode(_dbKey);

            EstimatedCosts estimatecost = new EstimatedCosts()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ObjectId = WoVM.estimatePart.WorkOrderId,
                ObjectType = SearchCategoryConstants.TBL_WORKORDER,
                Category = "Parts",
                CategoryId = pt.PartId,
                Description = pt.Description,
                UnitCost = pt.AppliedCost,
                Quantity = WoVM.estimatePart.Quantity ?? 0,
                Source = "Internal",
            };
            estimatecost.CheckDuplicateCraftForAdd(userData.DatabaseKey);
            if (estimatecost != null && estimatecost.ErrorMessages.Count > 0)
            {
                return estimatecost;
            }
            estimatecost.Create(this.userData.DatabaseKey);
            return estimatecost;
        }
        public EstimatedCosts EditEstimatePart(WorkOrderVM WoVM)
        {
            EstimatedCosts estimatecost = new EstimatedCosts()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ObjectId = WoVM.estimatePart.WorkOrderId,
                EstimatedCostsId = WoVM.estimatePart.EstimatedCostsId,
            };
            estimatecost.Retrieve(this.userData.DatabaseKey);
            estimatecost.Description = WoVM.estimatePart.Description;
            estimatecost.Quantity = WoVM.estimatePart.Quantity ?? 0;
            estimatecost.UnitCost = WoVM.estimatePart.UnitCost ?? 0;
            estimatecost.UnitOfMeasure = WoVM.estimatePart.Unit;
            estimatecost.AccountId = WoVM.estimatePart?.AccountId ?? 0;
            estimatecost.AccountClientLookupId = WoVM.estimatePart.AccountClientLookupId;
            estimatecost.VendorId = WoVM.estimatePart?.VendorId ?? 0 ;
            estimatecost.VendorClientLookupId = WoVM.estimatePart.VendorClientLookupId;
            if (userData.Site.ShoppingCart)
            {
                estimatecost.UNSPSC = WoVM.estimatePart?.PartCategoryMasterId ?? 0;
                estimatecost.PartClientLookupId = WoVM.estimatePart.PartCategoryClientLookupId;
            }
            else
            {
                estimatecost.UNSPSC =  0;
                estimatecost.PartClientLookupId = null;
            }
            estimatecost.Update(this.userData.DatabaseKey);
            return estimatecost;
        }
        public bool DeleteEstimatePart(long EstimatedCostsId)
        {
            try
            {
                EstimatedCosts ems = new EstimatedCosts()
                {
                    EstimatedCostsId = EstimatedCostsId
                };
                ems.Delete(userData.DatabaseKey);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Estimate-Purchased
        public List<POLineItemModel> populatePurchasedList(long _woId)
        {
            POLineItemModel pOLineItemModel;
            List<POLineItemModel> pOLineItemModelList = new List<POLineItemModel>();
            PurchaseOrderLineItem purchaseOrderLineItem = new PurchaseOrderLineItem()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.Site.SiteId,
                ChargeToId = _woId
            };
            List<PurchaseOrderLineItem> list = purchaseOrderLineItem.RetriveByWorkOrderId(this.userData.DatabaseKey);

            foreach (var item in list)
            {
                pOLineItemModel = new POLineItemModel();
                pOLineItemModel.ClientLookupId = item.ClientLookupId;
                pOLineItemModel.LineNumber = item.LineNumber;
                pOLineItemModel.Description = item.Description;
                if (item.EstimatedDelivery != null && item.EstimatedDelivery == default(DateTime))
                {
                    pOLineItemModel.EstimatedDelivery = null;
                }
                else
                {
                    pOLineItemModel.EstimatedDelivery = item.EstimatedDelivery;
                }
                pOLineItemModel.Status = item.Status;
                pOLineItemModel.OrderQuantity = item.OrderQuantity;
                pOLineItemModel.UnitOfMeasure = item.UnitOfMeasure;
                pOLineItemModel.ReceivedQuantity = item.ReceivedQuantity;
                pOLineItemModelList.Add(pOLineItemModel);
            }
            return pOLineItemModelList;
        }
        #endregion

        #region Estimate-Labors
        public List<EstimateLabor> populateEstimatedLabors(long workOrderId)
        {
            EstimateLabor estimateLabor;
            List<EstimateLabor> estimatedLaborList = new List<EstimateLabor>();

            EstimatedCosts estimatecost = new EstimatedCosts()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ObjectId = workOrderId,
                ObjectType = SearchCategoryConstants.TBL_WORKORDER,
                Category = SearchCategoryConstants.TBL_CRAFT
            };
            List<EstimatedCosts> list = estimatecost.RetriveByObjectId(this.userData.DatabaseKey);
            foreach (var ec in list)
            {
                estimateLabor = new EstimateLabor();
                estimateLabor.EstimatedCostsId = ec.EstimatedCostsId;
                estimateLabor.ClientLookupId = ec.ClientLookupId;
                estimateLabor.Description = ec.Description;
                estimateLabor.UnitCost = ec.UnitCost;
                estimateLabor.Quantity = ec.Quantity;
                estimateLabor.Duration = ec.Duration;
                estimateLabor.TotalCost = ec.TotalCost;
                estimateLabor.CategoryId = ec.CategoryId;
                estimatedLaborList.Add(estimateLabor);
            }

            return estimatedLaborList;
        }

        public EstimatedCosts AddEstimateLabor(WorkOrderVM WoVM)
        {
            EstimatedCosts estimatecost = new EstimatedCosts()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ObjectId = WoVM.estimateLabor.workOrderId,
                ObjectType = SearchCategoryConstants.TBL_WORKORDER,
                Category = SearchCategoryConstants.TBL_CRAFT
            };

            Craft craft = new Craft();
            craft.ClientId = userData.DatabaseKey.Personnel.ClientId;
            craft.CraftId = WoVM.estimateLabor.CraftId;
            craft.Retrieve(userData.DatabaseKey);

            estimatecost.CategoryId = craft.CraftId;
            estimatecost.Description = craft.Description;
            estimatecost.UnitCost = craft.ChargeRate;
            estimatecost.Quantity = WoVM.estimateLabor?.Quantity ?? 0;
            estimatecost.Duration = WoVM.estimateLabor.Duration ?? 0;
            estimatecost.Source = "Internal";
            estimatecost.CheckDuplicateCraftForAdd(userData.DatabaseKey);
            if (estimatecost.ErrorMessages != null && estimatecost.ErrorMessages.Count > 0)
            {
                return estimatecost;
            }
            estimatecost.Create(this.userData.DatabaseKey);

            return estimatecost;
        }
        public EstimatedCosts EditEstimateLabor(WorkOrderVM WoVM)
        {
            EstimatedCosts estimatecost = new EstimatedCosts()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ObjectId = WoVM.estimateLabor.workOrderId,
                EstimatedCostsId = WoVM.estimateLabor.EstimatedCostsId,
            };
            estimatecost.Retrieve(this.userData.DatabaseKey);

            Craft craft = new Craft();
            craft.ClientId = userData.DatabaseKey.Personnel.ClientId;
            craft.CraftId = WoVM.estimateLabor.CraftId;
            craft.Retrieve(userData.DatabaseKey);

            estimatecost.ObjectType = SearchCategoryConstants.TBL_WORKORDER;
            estimatecost.Category = SearchCategoryConstants.TBL_CRAFT;
            estimatecost.CategoryId = craft.CraftId;
            estimatecost.Description = craft.Description;
            estimatecost.UnitCost = craft.ChargeRate;
            estimatecost.Quantity = WoVM.estimateLabor?.Quantity ?? 0;
            estimatecost.Duration = WoVM.estimateLabor.Duration ?? 0;
            estimatecost.Source = "Internal";
            estimatecost.CheckDuplicateCraftForUpdate(userData.DatabaseKey);
            if (estimatecost != null && estimatecost.ErrorMessages.Count > 0)
            {
                return estimatecost;
            }
            estimatecost.Update(this.userData.DatabaseKey);

            return estimatecost;
        }
        public bool DeleteEstimateLabour(long EstimatedCostsId)
        {
            try
            {
                EstimatedCosts ems = new EstimatedCosts()
                {
                    EstimatedCostsId = EstimatedCostsId
                };
                ems.Delete(userData.DatabaseKey);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Estimate-Others
        public List<EstimateOther> populateEstimatedOthers(long workOrderId)
        {
            EstimateOther estimatedOther;
            List<EstimateOther> estimatedCostModelList = new List<EstimateOther>();

            EstimatedCosts estimatecost = new EstimatedCosts()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ObjectId = workOrderId,
                ObjectType = SearchCategoryConstants.TBL_WORKORDER,
                Category = "Other"
            };
            List<EstimatedCosts> list = estimatecost.RetriveByObjectId(this.userData.DatabaseKey);
            foreach (var ec in list)
            {
                estimatedOther = new EstimateOther();
                estimatedOther.EstimatedCostsId = ec.EstimatedCostsId;
                estimatedOther.Source = ec.Source;
                estimatedOther.VendorClientLookupId = ec.VendorClientLookupId; //If ---(UserData.Site.UseVendorMaster)- true then this column visibility false
                estimatedOther.Description = ec.Description;
                estimatedOther.UnitCost = ec.UnitCost;
                estimatedOther.Quantity = ec.Quantity;
                estimatedOther.TotalCost = ec.TotalCost;
                estimatedOther.VendorId = ec.VendorId;
                estimatedOther.UpdateIndex = ec.UpdateIndex;
                estimatedCostModelList.Add(estimatedOther);
            }

            return estimatedCostModelList;
        }

        public List<String> addUpdateOther(EstimatedCostModel estimatedCostModel)
        {
            EstimatedCosts estimatecost = new EstimatedCosts()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                EstimatedCostsId = estimatedCostModel.EstimatedCostsId,
                ObjectId = estimatedCostModel.WorkOrderId,
                ObjectType = SearchCategoryConstants.TBL_WORKORDER,
                Category = "Other"
            };
            if (userData.Site.UseVendorMaster)
            {
                estimatecost.VendorId = 0;
            }
            else
            {
                estimatecost.VendorId = estimatedCostModel.VendorId;
            }

            estimatecost.CategoryId = 0;

            estimatecost.Description = estimatedCostModel.Description ?? " ";
            estimatecost.UnitCost = estimatedCostModel.UnitCost ?? 0;
            estimatecost.Quantity = estimatedCostModel.Quantity ?? 0;
            estimatecost.Source = estimatedCostModel.Source;
            estimatecost.UpdateIndex = estimatedCostModel.UpdateIndex;

            if (estimatedCostModel.EstimatedCostsId == 0)
            {
                estimatecost.Create(userData.DatabaseKey);
            }
            else
            {
                estimatecost.Update(userData.DatabaseKey);
            }

            return estimatecost.ErrorMessages; ;
        }
        public bool DeleteEstimateOther(long EstimatedCostsId)
        {
            try
            {
                EstimatedCosts estimatecost = new EstimatedCosts()
                {
                    ClientId = this.userData.DatabaseKey.Client.ClientId,
                    EstimatedCostsId = EstimatedCostsId
                };

                estimatecost.Delete(userData.DatabaseKey);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Estimate-Summery
        public List<EstimatedCostModel> populateEstimatedSummery(long workOrderId)
        {
            EstimatedCostModel estimatedCostModel;
            List<EstimatedCostModel> estimatedCostModelList = new List<EstimatedCostModel>();

            EstimatedCosts estimatecost = new EstimatedCosts()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ObjectId = workOrderId,
                ObjectType = SearchCategoryConstants.TBL_WORKORDER,
            };
            List<EstimatedCosts> list = estimatecost.SummeryRetriveByObjectId(this.userData.DatabaseKey);
            foreach (var ec in list)
            {
                estimatedCostModel = new EstimatedCostModel();
                estimatedCostModel.EstimatedCostsId = ec.EstimatedCostsId;
                estimatedCostModel.TotalPartCost = ec.TotalPartCost;
                estimatedCostModel.TotalPurchaseCost = ec.TotalPurchaseCost;
                estimatedCostModel.TotalCraftCost = ec.TotalCraftCost;
                estimatedCostModel.TotalLaborHours = ec.TotalLaborHours;
                estimatedCostModel.TotalExternalCost = ec.TotalExternalCost;
                estimatedCostModel.TotalInternalCost = ec.TotalInternalCost;
                estimatedCostModel.TotalSummeryCost = ec.TotalSummeryCost;
                estimatedCostModelList.Add(estimatedCostModel);
            }

            return estimatedCostModelList;
        }
        #endregion

        #region Actuals-Parts
        public List<PartHistoryModel> populateActualPart(long workOrderId)
        {
            PartHistoryModel partHistoryModel;
            List<PartHistoryModel> plist = new List<PartHistoryModel>();
            PartHistory parthistory = new PartHistory()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ChargeType_Primary = SearchCategoryConstants.TBL_WORKORDER,
                ChargeToId_Primary = workOrderId
            };
            List<PartHistory> phList = parthistory.ReturnRetriveByWorkOrderId(this.userData.DatabaseKey);

            foreach (var item in phList)
            {
                partHistoryModel = new PartHistoryModel();
                partHistoryModel.PartClientLookupId = item.PartClientLookupId;
                partHistoryModel.Description = item.Description;
                partHistoryModel.TransactionQuantity = item.TransactionQuantity;
                partHistoryModel.Cost = item.Cost;
                partHistoryModel.TotalCost = item.TotalCost;
                partHistoryModel.UnitofMeasure = item.UnitofMeasure;
                //V2-624
                partHistoryModel.PartId = item.PartId;
                partHistoryModel.UPCCode = item.UPCCode;
                partHistoryModel.PartHistoryId = item.PartHistoryId;
                if (item.TransactionDate != null && item.TransactionDate == default(DateTime))
                {
                    partHistoryModel.TransactionDate = null;
                }
                else
                {
                    partHistoryModel.TransactionDate = item.TransactionDate.ToUserTimeZone(this.userData.Site.TimeZone);
                }
                partHistoryModel.StoreroomId = item.StoreroomId;
                partHistoryModel.PerformBy = item.PerformBy;

                plist.Add(partHistoryModel);
            }
            return plist;
        }

        #endregion

        #region Actuals-Labor
        public List<WoActualLabor> PopulateActualLabor(long workOrderId)
        {
            List<WoActualLabor> ActualLaborList = new List<WoActualLabor>();
            WoActualLabor objActualLabor;

            Timecard timecard = new Timecard()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ChargeType_Primary = SearchCategoryConstants.TBL_WORKORDER,
                ChargeToId_Primary = workOrderId

            };

            var ActuallaborData = Timecard.RetriveByWorkOrderId(this.userData.DatabaseKey, timecard);

            if (ActuallaborData != null)
            {
                foreach (var item in ActuallaborData)
                {
                    objActualLabor = new WoActualLabor();
                    objActualLabor.PersonnelClientLookupId = item.PersonnelClientLookupId;
                    objActualLabor.PersonnelID = item.PersonnelId;
                    objActualLabor.NameFull = item.NameFull.Trim(',');
                    if (item.StartDate != null && item.StartDate == default(DateTime))
                    {
                        objActualLabor.StartDate = null;
                    }
                    else
                    {
                        objActualLabor.StartDate = item.StartDate;
                    }
                    objActualLabor.Hours = item.Hours;
                    objActualLabor.TCValue = item.TCValue;
                    objActualLabor.TimecardId = item.TimecardId;
                    ActualLaborList.Add(objActualLabor);
                }
            }
            return ActualLaborList;
        }
        public Timecard AddActualLabor(WoActualLabor objActualLabor)
        {
            Timecard timecard = new Timecard()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                ChargeType_Primary = SearchCategoryConstants.TBL_WORKORDER,
                ChargeToId_Primary = objActualLabor.workOrderId
            };
            timecard.PersonnelId = objActualLabor.PersonnelID ?? 0;
            timecard.Hours = objActualLabor.Hours;
            timecard.StartDate = objActualLabor.StartDate;

            timecard.CreateByPKForeignKeys(this.userData.DatabaseKey);

            return timecard;
        }
        public Timecard EditActualLabor(WoActualLabor objActualLabor)
        {
            Timecard timecard = new Timecard()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                ChargeType_Primary = SearchCategoryConstants.TBL_WORKORDER,
                ChargeToId_Primary = objActualLabor.workOrderId,
                TimecardId = objActualLabor.TimecardId
            };

            timecard.Retrieve(this.userData.DatabaseKey);
            timecard.PersonnelId = objActualLabor.PersonnelID ?? 0;
            timecard.Hours = objActualLabor.Hours;
            timecard.StartDate = objActualLabor.StartDate;

            timecard.UpdateByPKForeignKeys(this.userData.DatabaseKey);
            return timecard;
        }
        public bool DeleteActualLabor(long TimecardId)
        {
            Timecard timecard = new Timecard()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                TimecardId = TimecardId
            };
            try
            {
                timecard.Delete(this.userData.DatabaseKey);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public List<DataModel> GetList_Personnel()
        {
            LookUpListModel model = new LookUpListModel();
            DataModel dModel;
            Personnel personnel = new Personnel()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.User.DefaultSiteId
            };

            List<Personnel> PersonnelList = personnel.RetrieveForLookupList(this.userData.DatabaseKey);
            switch (UserTypeConstants.Buyer)
            {
                case "Buyer":
                    PersonnelList = PersonnelList.Where(x => x.Buyer == true).ToList();
                    break;
                default:
                    break;
            }
            foreach (var p in PersonnelList)
            {
                dModel = new DataModel();

                dModel.AssignedTo_PersonnelId = p.PersonnelId;
                dModel.AssignedTo_PersonnelClientLookupId = p.ClientLookupId;
                dModel.NameFirst = p.NameFirst;
                dModel.NameLast = p.NameLast;
                model.data.Add(dModel);
            }
            return model.data;
        }

        public Timecard getTimecardOfLaborById(long WorkOrderID, long TimecardId)
        {
            Timecard timecard = new Timecard()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.User.DefaultSiteId,
                ChargeType_Primary = SearchCategoryConstants.TBL_WORKORDER,
                ChargeToId_Primary = WorkOrderID,
                TimecardId = TimecardId
            };

            timecard.Retrieve(this.userData.DatabaseKey);
            return timecard;
        }

        #endregion

        #region Actuals-Other
        public List<ActualOther> PopulateActualOther(long workOrderId)
        {
            List<ActualOther> ActualOtherList = new List<ActualOther>();
            ActualOther objActualOther;
            OtherCosts othercost = new OtherCosts()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ObjectId = workOrderId,
                ObjectType = SearchCategoryConstants.TBL_WORKORDER
            };

            var ActualOthersData = othercost.RetriveByObjectId(this.userData.DatabaseKey);
            if (ActualOthersData != null)
            {
                foreach (var item in ActualOthersData)
                {
                    objActualOther = new ActualOther();
                    objActualOther.OtherCostsId = item.OtherCostsId;
                    objActualOther.Source = item.Source;
                    objActualOther.VendorClientLookupId = item.VendorClientLookupId;
                    objActualOther.VendorId = item.VendorId;
                    objActualOther.Description = item.Description;
                    objActualOther.UnitCost = item.UnitCost;
                    objActualOther.Quantity = item.Quantity;
                    objActualOther.TotalCost = item.TotalCost;

                    ActualOtherList.Add(objActualOther);
                }
            }
            return ActualOtherList;
        }
        public List<string> AddActualOther(ActualOther objActualOther)
        {
            OtherCosts othercost = new OtherCosts()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ObjectType = SearchCategoryConstants.TBL_WORKORDER,
                ObjectId = objActualOther.workOrderId
            };
            othercost.Category = "Other";
            othercost.CategoryId = 0;
            othercost.Description = objActualOther.Description;
            othercost.Source = objActualOther.Source;
            othercost.VendorId = objActualOther.VendorId ?? 0;
            othercost.Quantity = objActualOther.Quantity ?? 0;
            othercost.UnitCost = objActualOther.UnitCost ?? 0;

            othercost.Create(this.userData.DatabaseKey);

            return othercost.ErrorMessages;
        }
        public List<string> EditActualOther(ActualOther objActualOther)
        {
            OtherCosts othercost = new OtherCosts()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                OtherCostsId = objActualOther.OtherCostsId,
                ObjectType = SearchCategoryConstants.TBL_WORKORDER,
                ObjectId = objActualOther.workOrderId,

            };
            othercost.Retrieve(this.userData.DatabaseKey);

            othercost.Category = "Other";
            othercost.CategoryId = 0;

            othercost.Description = objActualOther.Description ?? string.Empty;
            othercost.Source = objActualOther.Source;
            othercost.VendorId = objActualOther.VendorId ?? 0;
            othercost.Quantity = objActualOther.Quantity ?? 0;
            othercost.UnitCost = objActualOther.UnitCost ?? 0;

            othercost.Update(this.userData.DatabaseKey);

            return othercost.ErrorMessages;
        }
        public bool DeleteActualOther(long OtherCostsId)
        {
            OtherCosts othercost = new OtherCosts()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                OtherCostsId = OtherCostsId
            };
            try
            {
                othercost.Delete(this.userData.DatabaseKey);
                return true;
            }
            catch
            {
                return false;
            }

        }
        #endregion

        #region Actuals-Summery
        public List<ActualSummery> populateActualSummery(long workOrderId)
        {
            List<ActualSummery> ActualSummeryList = new List<ActualSummery>();
            ActualSummery objActualSummery;

            OtherCosts othercost = new OtherCosts()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ObjectType = SearchCategoryConstants.TBL_WORKORDER,
                ObjectId = workOrderId
            };
            List<OtherCosts> othercosts = othercost.SummeryRetriveByObjectId(this.userData.DatabaseKey);
            if (othercosts != null)
            {
                foreach (var item in othercosts)
                {
                    objActualSummery = new ActualSummery();
                    objActualSummery.TotalPartCost = item.TotalPartCost;
                    objActualSummery.TotalCraftCost = item.TotalCraftCost;
                    objActualSummery.TotalExternalCost = item.TotalExternalCost;
                    objActualSummery.TotalInternalCost = item.TotalInternalCost;
                    objActualSummery.TotalSummeryCost = item.TotalSummeryCost;
                    ActualSummeryList.Add(objActualSummery);
                }
            }
            return ActualSummeryList;
        }
        #endregion

        #region Work Request Add
        public List<Department> GetDepartMentList()
        {
            List<Department> department = new DataContracts.Department().RetrieveAllTemplatesWithClient(userData.DatabaseKey);
            return department;
        }
        public WorkOrder AddWorkRequestWrap(WoRequestModel WoRequestModel, ref List<string> errorMsg)
        {
            WorkOrder obj = new WorkOrder();
            WorkOrder workorder = new WorkOrder
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId
            };
            if (WorkOrderStatusConstants.Wo_AutoGenerateEnabled)
            {
                newClientlookupId = CustomSequentialId.GetNextId(userData.DatabaseKey, AutoGenerateKey.WO_Annual, userData.DatabaseKey.User.DefaultSiteId, "");
            }


            workorder.SiteId = userData.DatabaseKey.Personnel.SiteId;
            if (WoRequestModel.IsDepartmentShow)
            {
                workorder.DepartmentId = WoRequestModel?.DepartmentId ?? userData.DatabaseKey.Personnel.DepartmentId;
            }
            else
            {
                workorder.DepartmentId = userData.DatabaseKey.Personnel.DepartmentId;
            }
            if (WoRequestModel.IsTypeShow)
            {
                workorder.Type = WoRequestModel.Type;
            }

            workorder.ChargeToId = 0;
            workorder.ChargeType = WoRequestModel.ChargeType;


            if (WoRequestModel.ChargeToClientLookupId != null)
            {
                var index = WoRequestModel.ChargeToClientLookupId.IndexOf('|');
                if (index != -1)
                {
                    workorder.ChargeToClientLookupId = WoRequestModel.ChargeToClientLookupId.Substring(0, index).Trim();
                }
                else
                {
                    workorder.ChargeToClientLookupId = WoRequestModel.ChargeToClientLookupId;
                }
            }
            if (WoRequestModel.IsDescriptionShow)
            {
                workorder.Description = WoRequestModel.Description.Trim();
            }
            workorder.ClientLookupId = newClientlookupId;
            workorder.SiteId = userData.DatabaseKey.Personnel.SiteId;
            workorder.Creator_PersonnelClientLookupId = userData.DatabaseKey.Personnel.ClientLookupId.ToString();
            workorder.Creator_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            workorder.CreateMode = true;
            workorder.Status = WorkOrderStatusConstants.WorkRequest;
            workorder.SourceType = WorkOrderSourceTypes.WorkRequest;//V2 607
            workorder.CreateByPKForeignKeys(this.userData.DatabaseKey, userData.Site.TimeZone);
            if (workorder.ErrorMessages.Count == 0)
            {
                ProcessAlert objAlert = new ProcessAlert(this.userData);
                List<long> wos = new List<long>() { workorder.WorkOrderId };
                Task CreateAlertTask = Task.Factory.StartNew(() => objAlert.CreateAlert<DataContracts.WorkOrder>(AlertTypeEnum.WorkRequestApprovalNeeded, wos));
                Task CreateEventTask1 = Task.Factory.StartNew(() => CreateEventLog(workorder.WorkOrderId, WorkOrderEventLogFunction.Create));
                Task CreateEventTask2 = Task.Factory.StartNew(() => CreateEventLog(workorder.WorkOrderId, WorkOrderEventLogFunction.WorkRequest));
            }
            else
            {
                errorMsg = workorder.ErrorMessages;
            }
            return workorder;

        }
        #region Work Request Add  611
        public WorkOrder AddWorkRequestDynamic(WorkOrderVM objWorkOrderVM, ref List<string> errorMsg)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            WorkOrder obj = new WorkOrder();
            WorkOrder workorder = new WorkOrder
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId
            };
            if (WorkOrderStatusConstants.Wo_AutoGenerateEnabled)
            {
                newClientlookupId = CustomSequentialId.GetNextId(userData.DatabaseKey, AutoGenerateKey.WO_Annual, userData.DatabaseKey.User.DefaultSiteId, "");
            }

            workorder.SiteId = userData.DatabaseKey.Personnel.SiteId;
            List<UIConfigurationDetailsForModelValidation> configDetails = new RetrieveDataForUIConfiguration().Retrieve(DataDictionaryViewNameConstant.AddWorkRequest, userData);
            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == false && x.Section == false);
            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, objWorkOrderVM.AddWorkRequest);
                getpropertyInfo = objWorkOrderVM.AddWorkRequest.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(objWorkOrderVM.AddWorkRequest);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }

                setpropertyInfo = workorder.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(workorder, val);
            }
            workorder.DepartmentId = userData.DatabaseKey.Personnel.DepartmentId;
            #region V2-948
            if (userData.Site.SourceAssetAccount == true && workorder.Labor_AccountId == 0)
            {
                Equipment equipment = new Equipment
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    SiteId = userData.Site.SiteId,
                    EquipmentId = workorder.ChargeToId
                };
                equipment.Retrieve(this.userData.DatabaseKey);
                workorder.Labor_AccountId = equipment.Labor_AccountId;
            }
            #endregion
            workorder.ChargeToId = 0;
            workorder.ChargeType = objWorkOrderVM.ChargeType;
            workorder.ChargeToClientLookupId = objWorkOrderVM.AddWorkRequest.ChargeToClientLookupId ?? string.Empty;
            workorder.ClientLookupId = newClientlookupId;
            workorder.SiteId = userData.DatabaseKey.Personnel.SiteId;
            workorder.Creator_PersonnelClientLookupId = userData.DatabaseKey.Personnel.ClientLookupId.ToString();
            workorder.Creator_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            workorder.CreateMode = true;
            workorder.Status = WorkOrderStatusConstants.WorkRequest;
            workorder.SourceType = WorkOrderSourceTypes.WorkRequest;//V2 607
            workorder.CreateByPKForeignKeys(this.userData.DatabaseKey, userData.Site.TimeZone);

            if (workorder.ErrorMessages != null && workorder.ErrorMessages.Count == 0)
            {
                ProcessAlert objAlert = new ProcessAlert(this.userData);
                List<long> wos = new List<long>() { workorder.WorkOrderId };
                #region V2-1077
                Task CreateAlertTask3 = Task.Factory.StartNew(() => objAlert.CreateAlert<WorkOrder>(AlertTypeEnum.WorkOrderPlanner, wos)); 
                #endregion
                Task CreateAlertTask = Task.Factory.StartNew(() => objAlert.CreateAlert<WorkOrder>(AlertTypeEnum.WorkRequestApprovalNeeded, wos));
                Task CreateEventTask1 = Task.Factory.StartNew(() => CreateEventLog(workorder.WorkOrderId, WorkOrderEventLogFunction.Create));
                Task CreateEventTask2 = Task.Factory.StartNew(() => CreateEventLog(workorder.WorkOrderId, WorkOrderEventLogFunction.WorkRequest));
                if (configDetails.Any(x => x.Display == true && x.UDF == true && x.Section == false))
                {
                    IEnumerable<string> errors = AddWorkRequestUDFDynamic(objWorkOrderVM.AddWorkRequest, workorder.WorkOrderId, configDetails);
                    if (errors != null && errors.Count() > 0)
                    {
                        workorder.ErrorMessages.AddRange(errors);
                    }

                }

            }
            else
            {
                errorMsg = workorder.ErrorMessages;
            }
            return workorder;

        }
        public List<string> AddWorkRequestUDFDynamic(AddWorkRequestModelDynamic woRequest, long WorkOrderId,
           List<UIConfigurationDetailsForModelValidation> configDetails)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            WorkOrderUDF woUDF = new WorkOrderUDF();
            woUDF.ClientId = userData.DatabaseKey.Client.ClientId;
            woUDF.WorkOrderId = WorkOrderId;

            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == true && x.Section == false);
            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, woRequest);
                getpropertyInfo = woRequest.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(woRequest);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                setpropertyInfo = woUDF.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(woUDF, val);
            }

            woUDF.Create(_dbKey);
            return woUDF.ErrorMessages;
        }
        private void AssignDefaultOrNullValue(ref object val, Type t)
        {
            if (t.Equals(typeof(long?)))
            {
                val = val ?? 0;
            }
            else if (t.Equals(typeof(DateTime?)))
            {
                //val = val ?? null;
            }
            else if (t.Equals(typeof(decimal?)))
            {
                val = val ?? 0M;
            }
            else if (t.Name == "String")
            {
                val = val ?? string.Empty;
            }
        }
        #endregion
        #endregion

        #region Cancel WO
        public WorkOrder CancelJob(long WorkorderId, string CancelReason, string Comments)
        {
            WorkOrder workOrder = new WorkOrder()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                WorkOrderId = WorkorderId,
                SiteId = userData.DatabaseKey.Personnel.SiteId
            };
            workOrder.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);
            workOrder.Status = WorkOrderStatusConstants.Canceled;
            workOrder.CompleteComments = Comments.Trim();
            workOrder.CancelReason = CancelReason.Trim();
            workOrder.CompleteDate = DateTime.UtcNow;
            if (workOrder.ActualFinishDate == null || workOrder.ActualFinishDate == DateTime.MinValue)
                workOrder.ActualFinishDate = DateTime.UtcNow;
            workOrder.CompleteBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            workOrder.CreateMode = false;
            workOrder.CompleteBy_PersonnelClientLookupId = userData.DatabaseKey.Personnel.ClientLookupId;
            workOrder.CompleteWorkOrder(this.userData.DatabaseKey, userData.Site.TimeZone);

            if (workOrder.ErrorMessages.Count == 0)
            {
                CreateEventLog(workOrder.WorkOrderId, WorkOrderStatusConstants.Canceled, workOrder.CompleteComments);
                // SOM - 797
                Int64 CallerUserInfoId = this.userData.DatabaseKey.User.UserInfoId;
                List<object> listWO = new List<object>();
                listWO.Add(workOrder.WorkOrderId);
                ProcessAlert objAlert = new ProcessAlert(this.userData);
                objAlert.CreateAlert<WorkOrder>(this.userData, workOrder, AlertTypeEnum.WorkOrderCancel, CallerUserInfoId, listWO);
            }
            return workOrder;
        }
        #endregion

        #region DownTime WO
        public Downtime DowntimeWO(long WorkorderId, DateTime? Downdate, decimal? Minutes, ref string IsAddOrUpdate)
        {
            WorkOrder workOrder = new WorkOrder()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                WorkOrderId = WorkorderId,
                SiteId = userData.DatabaseKey.Personnel.SiteId
            };

            workOrder.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);

            Downtime downtime = new Downtime()
            {
                ClientId = workOrder.ClientId,
                WorkOrderId = workOrder.WorkOrderId

            };
            var validateMinutesDown = Minutes ?? 0;

            if (validateMinutesDown > 0)
            {
                List<Downtime> downtimelst = new List<Downtime>();
                downtimelst = Downtime.RetriveByWorkOrderId(this.userData.DatabaseKey, downtime);

                if (downtimelst.Count == 0)
                {
                    IsAddOrUpdate = "spnInsertSuccessDowntime";
                    downtime.ClientId = workOrder.ClientId;
                    downtime.EquipmentId = workOrder.ChargeToId;
                    downtime.WorkOrderId = workOrder.WorkOrderId;
                    downtime.DateDown = Downdate;
                    downtime.MinutesDown = Minutes ?? 0;
                    downtime.Operator_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
                    downtime.Create(this.userData.DatabaseKey);
                }
                else
                {
                    IsAddOrUpdate = "spnUpdateSuccessDowntime";
                    downtime.ClientId = workOrder.ClientId;
                    downtime.EquipmentId = workOrder.ChargeToId;
                    downtime.WorkOrderId = workOrder.WorkOrderId;
                    downtime.DowntimeId = Convert.ToInt32(downtimelst[0].DowntimeId.ToString());
                    downtime.DateDown = Downdate;
                    downtime.MinutesDown = Minutes ?? 0;
                    downtime.UpdateIndex = Convert.ToInt32(downtimelst[0].UpdateIndex.ToString());
                    downtime.ParentSiteId = userData.DatabaseKey.Personnel.SiteId;
                    downtime.Operator_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
                    downtime.WorkOrderClientLookupId = workOrder.ClientLookupId;
                    downtime.PersonnelClientLookupId = userData.DatabaseKey.Personnel.ClientLookupId;
                    downtime.UpdatePKForeignKeys(userData.DatabaseKey);
                }
                workOrder.EquipDownDate = Downdate;
                workOrder.EquipDownHours = Minutes ?? 0;
                workOrder.UpdateByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);
            }
            return downtime;
        }
        #endregion

        #region Follow Up Work Order
        public WorkOrder FollowUpWorkOrder(WoRequestModel WoRequestModel)
        {
            WorkOrder workOrder = new WorkOrder
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId
            };
            if (WorkOrderStatusConstants.Wo_AutoGenerateEnabled)
            {
                newClientlookupId = CustomSequentialId.GetNextId(userData.DatabaseKey, AutoGenerateKey.WO_Annual, userData.DatabaseKey.User.DefaultSiteId, "");
            }
            if (WoRequestModel.IsTypeShow)
            {
                workOrder.Type = WoRequestModel.Type;
            }
            // Commented for V2-608
            workOrder.ChargeType = ChargeType.Equipment;
            if (WoRequestModel.ChargeToClientLookupId != null)
            {
                var index = WoRequestModel.ChargeToClientLookupId.IndexOf('|');
                if (index != -1)
                {

                    workOrder.ChargeToClientLookupId = WoRequestModel.ChargeToClientLookupId.Substring(0, index).Trim();

                    long ChrgTo = 0;
                    long.TryParse(workOrder.ChargeToClientLookupId, out ChrgTo);
                    workOrder.ChargeToId = ChrgTo;

                }
                else
                {
                    workOrder.ChargeToClientLookupId = WoRequestModel.ChargeToClientLookupId;
                    long ChrgTo = 0;
                    long.TryParse(workOrder.ChargeToClientLookupId, out ChrgTo);
                    workOrder.ChargeToId = ChrgTo;
                }
            }
            #region V2-948
            if (userData.Site.SourceAssetAccount == true && workOrder.Labor_AccountId == 0)
            {
                Equipment equipment = new Equipment
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    SiteId = userData.Site.SiteId,
                    ClientLookupId = workOrder.ChargeToClientLookupId
                };
                equipment.RetrieveByClientLookupId(this.userData.DatabaseKey);
                workOrder.Labor_AccountId = equipment.Labor_AccountId;
            }
            #endregion
            if (WoRequestModel.IsDescriptionShow)
            {
                workOrder.Description = WoRequestModel.Description.Trim();
            }
            workOrder.ClientLookupId = newClientlookupId;
            workOrder.SiteId = userData.DatabaseKey.Personnel.SiteId;

            workOrder.ClientId = userData.DatabaseKey.Client.ClientId;
            workOrder.Creator_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            workOrder.SourceType = WorkOrderSourceTypes.FollowUp;
            workOrder.SourceId = WoRequestModel.WorkOrderId;
            workOrder.Status = WorkOrderStatusConstants.Approved;

            workOrder.WorkAssigned_PersonnelId = 0;
            workOrder.ApproveDate = DateTime.UtcNow;
            workOrder.ApproveBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            workOrder.CreateByPKForeignKeys(this.userData.DatabaseKey, userData.Site.TimeZone);
            if (workOrder.ErrorMessages.Count == 0)
            {
                CreateEventLog(workOrder.WorkOrderId, WorkOrderEvents.Create, workOrder.SourceId, AttachmentTableConstant.WorkOrder);//-----------SOM-1632-------------//
                CreateEventLog(workOrder.WorkOrderId, WorkOrderEvents.Approved, 0, "");//-----------SOM-1632-------------//
            }
            return workOrder;
        }
        private void CreateEventLog(Int64 WOId, string Status, Int64 sourceId, string sourceTable)
        {
            WorkOrderEventLog log = new WorkOrderEventLog();
            log.ClientId = userData.DatabaseKey.Client.ClientId;
            log.SiteId = userData.DatabaseKey.Personnel.SiteId;
            log.WorkOrderId = WOId;
            log.Event = Status;
            log.TransactionDate = DateTime.UtcNow;
            log.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            log.Comments = "";
            log.SourceId = sourceId;
            log.SourceTable = sourceTable;
            log.Create(userData.DatabaseKey);
        }
        #endregion

        #region Reopen
        public WorkOrder ReOpenWorkOrder(long workorderId, ref string Statusmsg)
        {
            WorkOrder workOrder = new WorkOrder()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                WorkOrderId = workorderId,
                SiteId = this.userData.DatabaseKey.Personnel.SiteId
            };
            workOrder.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);
            if (workOrder.Status == WorkOrderStatusConstants.Complete || workOrder.Status == WorkOrderStatusConstants.Denied || workOrder.Status == WorkOrderStatusConstants.Canceled)
            {
                if (workOrder.WorkAssigned_PersonnelId > 0)
                {
                    workOrder.Status = WorkOrderStatusConstants.Scheduled;
                }
                else
                {
                    workOrder.Status = WorkOrderStatusConstants.Approved;
                }
                workOrder.CompleteBy_PersonnelClientLookupId = string.Empty;
                workOrder.CompleteBy_PersonnelId = 0;
                workOrder.CompleteBy_PersonnelName = string.Empty;
                workOrder.CompleteDate = null;
                workOrder.CompleteComments = string.Empty;
                workOrder.ActualFinishDate = null;
                workOrder.DeniedBy_PersonnelId = 0;
                workOrder.DeniedDate = null;
                workOrder.DeniedReason = string.Empty;
                workOrder.CancelDate = null;
                workOrder.CancelReason = string.Empty;
                workOrder.CreateMode = false;
                workOrder.UpdateByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);

                if (workOrder.ErrorMessages.Count > 0)
                {
                    Statusmsg = "error";
                }
                else
                {
                    CreateEventLog(workOrder.WorkOrderId, WorkOrderEventLogFunction.Reopen); //V2-953
                    Statusmsg = "success";
                }

            }
            else
            {
                Statusmsg = "unsuccess";//"Cannot re-open work order unless it is canceled, completed or denied.";
            }
            return workOrder;
        }
        #endregion

        #region Approve Wo
        public WorkOrder ApproveWO(long workorderId, ref string Statusmsg)
        {
            WorkOrder workOrder = new WorkOrder()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                WorkOrderId = workorderId,
                SiteId = this.userData.DatabaseKey.Personnel.SiteId
            };
            workOrder.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);
            if (workOrder.Status == WorkOrderStatusConstants.WorkRequest || workOrder.Status == WorkOrderStatusConstants.AwaitingApproval || workOrder.Status == WorkOrderStatusConstants.Planning)
            {
                // Approve the work order
                workOrder.Status = WorkOrderStatusConstants.Approved;
                workOrder.ApproveDate = DateTime.UtcNow;
                workOrder.ApproveBy_PersonnelClientLookupId = this.userData.DatabaseKey.Personnel.ClientLookupId;
                workOrder.CreateMode = false;
                workOrder.UpdateByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);

                if (workOrder.ErrorMessages.Count == 0)
                {
                    Task t1 = Task.Factory.StartNew(() => CreateEventLog(workOrder.WorkOrderId, WorkOrderEvents.Approved, "Work Order Approved"));

                    List<long> listWO = new List<long>();
                    listWO.Add(workOrder.WorkOrderId);
                    ProcessAlert objAlert = new ProcessAlert(this.userData);
                    Task t2 = Task.Factory.StartNew(() => objAlert.CreateAlert<WorkOrder>(AlertTypeEnum.WorkRequestApproved, listWO));
                    Statusmsg = "success";
                }
                else
                {
                    Statusmsg = "error";
                }
            }
            return workOrder;
        }
        #endregion


        #region Planning Wo

        public WorkOrder PlanningWO(long workorderId, long PersonnelId, ref string Statusmsg)
        {
            WorkOrder workOrder = new WorkOrder()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                WorkOrderId = workorderId,
                SiteId = this.userData.DatabaseKey.Personnel.SiteId
            };
            workOrder.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);
            string PrevStatus= workOrder.Status;
            if (workOrder.Status == WorkOrderStatusConstants.WorkRequest || workOrder.Status == WorkOrderStatusConstants.Scheduled || workOrder.Status == WorkOrderStatusConstants.Approved)
            {
                // set the planning the work order
                workOrder.Status = WorkOrderStatusConstants.Planning;
                workOrder.Planner_PersonnelId = PersonnelId;
                workOrder.UpdateByWorkOrderPlanner(userData.DatabaseKey, userData.Site.TimeZone);
                if (workOrder.ErrorMessages.Count == 0)
                {
                    #region V2-1221    
                    if (PrevStatus == WorkOrderStatusConstants.Scheduled)
                    { 
                        workOrder.WorkOrderUpdateOnRemovingSchedule(this.userData.DatabaseKey, userData.Site.TimeZone); 
                    }                    
                    if (workOrder.ErrorMessages.Count == 0)
                    {
                        #region V2-1077
                        ProcessAlert objAlert = new ProcessAlert(this.userData);
                        List<long> listWO = new List<long>();
                        listWO.Add(workOrder.WorkOrderId);
                        objAlert.CreateAlert<WorkOrder>(AlertTypeEnum.WorkOrderPlanner, listWO);
                        #endregion
                        Task t1 = Task.Factory.StartNew(() => CreateEventLog(workOrder.WorkOrderId, WorkOrderEvents.Planning, string.Empty));
                        Statusmsg = "success";
                    }
                    else
                    {
                        Statusmsg = "error";
                    }
                    #endregion
                }
                else
                {
                    Statusmsg = "error";
                }
            }
            return workOrder;
        }


        #endregion

        #region Deny Wo
        public WorkOrder DenyWO(long workorderId, string Denycomments, ref string Statusmsg)
        {
            WorkOrder workOrder = new WorkOrder()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                WorkOrderId = workorderId,
                SiteId = this.userData.DatabaseKey.Personnel.SiteId
            };
            workOrder.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);
            string strWOStatus = workOrder.Status;
            workOrder.Status = WorkOrderStatusConstants.Denied;
            workOrder.DeniedComment = Denycomments.Trim();
            workOrder.DeniedBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            workOrder.DeniedDate = DateTime.UtcNow;
            workOrder.CompleteComments = Denycomments.Trim();
            workOrder.CompleteDate = DateTime.UtcNow;
            if (workOrder.ActualFinishDate == null || workOrder.ActualFinishDate == DateTime.MinValue)
                workOrder.ActualFinishDate = DateTime.UtcNow.ConvertFromUTCToUser(userData.Site.TimeZone);
            workOrder.CompleteBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            workOrder.ChargeType = workOrder.ChargeType;
            workOrder.ChargeToClientLookupId = workOrder.ChargeToClientLookupId;
            workOrder.CreateMode = false;
            workOrder.CompleteBy_PersonnelClientLookupId = userData.DatabaseKey.Personnel.ClientLookupId;
            workOrder.CompleteWorkOrder(this.userData.DatabaseKey, userData.Site.TimeZone);
            if (workOrder.ErrorMessages.Count == 0)
            {
                CreateEventLog(workOrder.WorkOrderId, WorkOrderEvents.Denied, workOrder.DeniedComment);
                ProcessAlert objAlert = new ProcessAlert(this.userData);
                List<long> listWO = new List<long>();
                listWO.Add(workOrder.WorkOrderId);
                objAlert.CreateAlert<WorkOrder>(AlertTypeEnum.WorkRequestDenied, listWO);
                Statusmsg = "success";
            }
            else
            {
                Statusmsg = "error";
            }
            return workOrder;
        }
        #endregion

        #region Emergency Wo Describe

        public WorkOrder Emergency_Describe(WoEmergencyDescribeModel WoEmergencyModel, ref List<string> ErrorMsgList)
        {
            WorkOrder workOrder = new WorkOrder
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId
            };

            if (WorkOrderStatusConstants.Wo_AutoGenerateEnabled)
            {
                newClientlookupId = CustomSequentialId.GetNextId(userData.DatabaseKey, AutoGenerateKey.WO_Annual, userData.DatabaseKey.User.DefaultSiteId, "");
            }

            if (WoEmergencyModel.IsTypeShow)
            {
                workOrder.Type = WoEmergencyModel.Type;
            }
            // Commented for V2-608
            workOrder.ChargeType = ChargeType.Equipment;

            if (WoEmergencyModel.ChargeToClientLookupId != null)
            {
                var index = WoEmergencyModel.ChargeToClientLookupId.IndexOf('|');
                if (index != -1)
                {
                    workOrder.ChargeToClientLookupId = WoEmergencyModel.ChargeToClientLookupId.Substring(0, index).Trim();
                    long ChrgTo = 0;
                    long.TryParse(workOrder.ChargeToClientLookupId, out ChrgTo);
                    workOrder.ChargeToId = ChrgTo;
                }
                else
                {
                    workOrder.ChargeToClientLookupId = WoEmergencyModel.ChargeToClientLookupId;
                    long ChrgTo = 0;
                    long.TryParse(workOrder.ChargeToClientLookupId, out ChrgTo);
                    workOrder.ChargeToId = ChrgTo;
                }
            }
            #region V2-948
            if (userData.Site.SourceAssetAccount == true && workOrder.Labor_AccountId == 0)
            {
                Equipment equipment = new Equipment
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    SiteId = userData.Site.SiteId,
                    ClientLookupId = workOrder.ChargeToClientLookupId
                };
                equipment.RetrieveByClientLookupId(this.userData.DatabaseKey);
                workOrder.Labor_AccountId = equipment.Labor_AccountId;
            }
            #endregion
            if (WoEmergencyModel.IsDescriptionShow)
            {
                workOrder.Description = WoEmergencyModel.Description.Trim();
            }
            workOrder.ClientLookupId = newClientlookupId;
            workOrder.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            workOrder.ClientId = userData.DatabaseKey.Client.ClientId;
            workOrder.CompleteBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            workOrder.SourceType = WorkOrderSourceTypes.Emergency;
            workOrder.SourceId = WoEmergencyModel.WorkOrderId;

            workOrder.Status = WorkOrderStatusConstants.Scheduled;
            workOrder.ScheduledStartDate = DateTime.UtcNow;
            //---Add Scheduler_PersonnelId 
            workOrder.Scheduler_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
            workOrder.WorkAssigned_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            workOrder.ApproveDate = DateTime.UtcNow;
            workOrder.EmergencyWorkOrder = true;//----------added on 23-12-2014------------
            workOrder.ApproveBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            // SOM-628 
            workOrder.Creator_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            workOrder.CreateByPKForeignKeys(this.userData.DatabaseKey, userData.Site.TimeZone);
            if (workOrder.ErrorMessages.Count == 0)
            {
                //CreateEventLog(workOrder.WorkOrderId, WorkOrderEvents.Create);//-----------SOM-1632-------------// //--added on 7/7/20
                //CreateEventLog(workOrder.WorkOrderId, WorkOrderEvents.Scheduled);//-----------SOM-1632-------------// //--added on 7/7/20

                //-- V2-606                
                CreateEventLog(workOrder.WorkOrderId, WorkOrderEvents.Create);
                CreateEventLog(workOrder.WorkOrderId, WorkOrderEvents.Approved);
                CreateEventLog(workOrder.WorkOrderId, WorkOrderEvents.Scheduled);
                //-- V2-606

                //------------------Adding Assignment Record for Somax 487---------added on 23-12-2014------------
                WorkOrderSchedule workorderschedule = new WorkOrderSchedule()
                {
                    ClientId = this.userData.DatabaseKey.Client.ClientId,
                    WorkOrderId = workOrder.WorkOrderId
                };
                workorderschedule.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
                workorderschedule.ScheduledStartDate = DateTime.UtcNow;
                workorderschedule.ScheduledHours = 1;
                workorderschedule.CreateForWorkOrder(this.userData.DatabaseKey);
            }
            else
            {
                ErrorMsgList = workOrder.ErrorMessages;
            }
            return workOrder;
        }
        #endregion

        #region Emergency Wo OnDemand

        public WorkOrder Emergency_Ondemand(WoEmergencyOnDamandModel WoEmergencyModel, ref List<string> ErrorMsgList)
        {
            WorkOrder workOrder = new WorkOrder
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId
            };
            if (WorkOrderStatusConstants.Wo_AutoGenerateEnabled)
            {
                newClientlookupId = CustomSequentialId.GetNextId(userData.DatabaseKey, AutoGenerateKey.WO_Annual, userData.DatabaseKey.User.DefaultSiteId, "");
            }

            if (WoEmergencyModel.IsTypeShow)
            {
                workOrder.Type = WoEmergencyModel.Type;
            }
            workOrder.ChargeType = ChargeType.Equipment;

            if (WoEmergencyModel.ChargeToClientLookupId != null)
            {
                var index = WoEmergencyModel.ChargeToClientLookupId.IndexOf('|');
                if (index != -1)
                {
                    workOrder.ChargeToClientLookupId = WoEmergencyModel.ChargeToClientLookupId.Substring(0, index).Trim();
                    long ChrgTo = 0;
                    long.TryParse(workOrder.ChargeToClientLookupId, out ChrgTo);
                    workOrder.ChargeToId = ChrgTo;
                }
                else
                {
                    workOrder.ChargeToClientLookupId = WoEmergencyModel.ChargeToClientLookupId;
                    long ChrgTo = 0;
                    long.TryParse(workOrder.ChargeToClientLookupId, out ChrgTo);
                    workOrder.ChargeToId = ChrgTo;
                }
            }


            workOrder.MaintOnDemandClientLookUpId = WoEmergencyModel.OnDemandID;
            workOrder.ClientLookupId = newClientlookupId;
            workOrder.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            workOrder.Creator_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
            workOrder.Creator_PersonnelClientLookupId = userData.DatabaseKey.Personnel.ClientLookupId;
            workOrder.ApproveBy_PersonnelClientLookupId = userData.DatabaseKey.Personnel.ClientLookupId;
            workOrder.ApproveBy_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
            workOrder.ApproveDate = DateTime.UtcNow;
            workOrder.Status = WorkOrderStatusConstants.Scheduled;
            workOrder.SourceType = WorkOrderSourceTypes.OnDemand;
            workOrder.ScheduledStartDate = DateTime.UtcNow;
            workOrder.Priority = "";
            workOrder.RequiredDate = null;
            workOrder.CreateFromOnDemandMaster_V2(this.userData.DatabaseKey, userData.Site.TimeZone);

            if (workOrder.ErrorMessages.Count == 0)
            {
                //-- V2-606
                CreateEventLog(workOrder.WorkOrderId, WorkOrderEvents.Create);//-----------SOM-1632-------------//
                CreateEventLog(workOrder.WorkOrderId, WorkOrderEvents.Approved);
                CreateEventLog(workOrder.WorkOrderId, WorkOrderEvents.Scheduled);//-----------SOM-1632-------------//                 
                //-- V2-606

                WorkOrderSchedule workorderschedule = new WorkOrderSchedule()
                {
                    ClientId = this.userData.DatabaseKey.Client.ClientId,
                    WorkOrderId = workOrder.WorkOrderId
                };
                workorderschedule.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
                workorderschedule.ScheduledStartDate = DateTime.UtcNow;
                workorderschedule.ScheduledHours = 1;
                // Create record - sp updates the work order 
                workorderschedule.CreateForWorkOrder(this.userData.DatabaseKey);
            }
            else
            {
                ErrorMsgList = workOrder.ErrorMessages;
            }
            return workOrder;
        }

        public List<MaintOnDemandMaster> GetOndemandList()
        {
            MaintOnDemandMaster maintOnDemandMaster = new MaintOnDemandMaster();
            maintOnDemandMaster.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            List<MaintOnDemandMaster> maintOnDemandMasterList = maintOnDemandMaster.RetrieveAllBySiteId(this.userData.DatabaseKey, this.userData.Site.TimeZone).Where(a => a.InactiveFlag == false).ToList();
            return maintOnDemandMasterList;
        }
        #endregion

        #region Sanitation Wo OnDemand
        public List<DataContracts.SanOnDemandMaster> SanitationOnDemandMaster()
        {
            SanOnDemandMaster SanOnDemandMaster = new SanOnDemandMaster();
            SanOnDemandMaster.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            SanOnDemandMaster.InactiveFlag = false;
            List<SanOnDemandMaster> obj_Lookuplist = SanOnDemandMaster.Retrieve_SanOnDemandMaster_ByInActiveFlag(this.userData.DatabaseKey, this.userData.Site.TimeZone);
            return obj_Lookuplist;
        }
        public SanitationRequest santitation_Ondemand(SanitationOnDemandWOModel WoOndemand, ref List<string> ErrorMsg)
        {
            SanitationRequest SanitationRequest = new SanitationRequest
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId
            };
            if (SanitationJobConstant.SanitaionJob_AutoGenerateEnabled)
            {
                newClientlookupId = CustomSequentialId.GetNextId(userData.DatabaseKey, AutoGenerateKey.SANIT_ANNUAL, userData.DatabaseKey.User.DefaultSiteId, "");
            }
            SanitationRequest.SiteId = userData.DatabaseKey.Personnel.SiteId;

            SanitationRequest.ChargeType = WoOndemand.ChargeType;
            if (WoOndemand.PlantLocationDescription != null)
            {
                var index = WoOndemand.PlantLocationDescription.IndexOf('(');
                if (index != -1) SanitationRequest.ChargeToClientLookupId = WoOndemand.PlantLocationDescription.Substring(0, index);
                else SanitationRequest.ChargeToClientLookupId = WoOndemand.PlantLocationDescription.Trim();
            }
            SanitationRequest.PlantLocationId = WoOndemand.ChargeType == ChargeType.PlantLocation ? WoOndemand.PlantLocationId : 0;
            SanitationRequest.SanOnDemandMasterId = WoOndemand.OnDemandId ?? 0;

            SanitationRequest.ClientLookupId = newClientlookupId;
            SanitationRequest.SiteId = userData.DatabaseKey.User.DefaultSiteId;

            SanitationRequest.Creator_PersonnelClientLookupId = userData.DatabaseKey.Personnel.ClientLookupId.ToString();
            SanitationRequest.Creator_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;

            SanitationRequest.Status = SanitationJobConstant.JobRequest;
            SanitationRequest.SourceType = SanitationJobConstant.SourceType_NewJob;

            SanitationRequest.FlagSourceType = 0;
            SanitationRequest.Add_SanitationJobOnDemandjobsAndRequests(this.userData.DatabaseKey);

            if (SanitationRequest.ErrorMessages.Count != 0)
            {
                ErrorMsg = SanitationRequest.ErrorMessages;
            }
            else
            {
                CreateEventLogSanitaion(SanitationRequest.SanitationJobId, SanitationEvents.Create, 0);//-----------SOM-1633-------------// 
                CreateEventLogSanitaion(SanitationRequest.SanitationJobId, SanitationEvents.JobRequest, 0);//-----------SOM-1633-------------// 

            }
            return SanitationRequest;
        }
        #endregion

        #region Sanitation Wo Describe
        public SanitationRequest santitation_Describe(SanitationDescribeWoModel WoDescribe, ref List<string> ErrorMsgList)
        {
            SanitationRequest SanitationRequest = new SanitationRequest
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId
            };

            if (SanitationJobConstant.SanitaionJob_AutoGenerateEnabled)
            {
                newClientlookupId = CustomSequentialId.GetNextId(userData.DatabaseKey, AutoGenerateKey.SANIT_ANNUAL, userData.DatabaseKey.User.DefaultSiteId, "");
            }

            SanitationRequest.ClientId = userData.DatabaseKey.Client.ClientId;
            SanitationRequest.SanOnDemandMasterId = WoDescribe.OnDemandId ?? 0;
            SanitationRequest.RequiredDate = WoDescribe.Required;
            SanitationRequest.FlagSourceType = 1;
            SanitationRequest.ChargeToId = WoDescribe.WorkOrderId;

            SanitationRequest.ClientLookupId = newClientlookupId;
            SanitationRequest.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            SanitationRequest.Creator_PersonnelClientLookupId = userData.DatabaseKey.Personnel.ClientLookupId.ToString();
            SanitationRequest.Creator_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            SanitationRequest.Status = SanitationJobConstant.JobRequest;
            SanitationRequest.AddEXSanitationRequest(this.userData.DatabaseKey);
            CreateEventLogSanitaion(SanitationRequest.SanitationJobId, SanitationEvents.Create, WoDescribe.WorkOrderId);
            CreateEventLogSanitaion(SanitationRequest.SanitationJobId, SanitationEvents.JobRequest, WoDescribe.WorkOrderId);

            if (SanitationRequest.ErrorMessages.Count != 0)
            {
                ErrorMsgList = SanitationRequest.ErrorMessages;
            }
            return SanitationRequest;
        }
        private void CreateEventLogSanitaion(Int64 objId, string Status, Int64 SourceId)
        {
            SanitationEventLog log = new SanitationEventLog();
            log.ClientId = userData.DatabaseKey.Client.ClientId;
            log.SiteId = userData.DatabaseKey.Personnel.SiteId;
            log.ObjectId = objId;
            log.Event = Status;
            log.TransactionDate = DateTime.UtcNow;
            log.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            log.Comments = "";
            log.SourceId = SourceId;
            log.Create(userData.DatabaseKey);
        }
        #endregion

        #region Wo OnDemand
        public WorkOrder AddWoOnDemand(WoOnDemandModel woOnDemandModel)
        {
            // Generate the new number
            string newClientlookupId = "";
            WorkOrder workOrder = new WorkOrder
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId
            };

            if (woOnDemandModel.ClientLookupId == null && WorkOrderStatusConstants.Wo_AutoGenerateEnabled)
            {
                newClientlookupId = CustomSequentialId.GetNextId(userData.DatabaseKey, AutoGenerateKey.WO_Annual, userData.DatabaseKey.User.DefaultSiteId, "");
            }

            workOrder.MaintOnDemandClientLookUpId = woOnDemandModel.MaintOnDemandClientLookUpId;
            workOrder.Type = woOnDemandModel.Type;
            workOrder.ChargeType = ChargeType.Equipment;
            workOrder.ChargeToClientLookupId = woOnDemandModel.ChargeToClientLookupId;
            workOrder.ClientLookupId = newClientlookupId;
            workOrder.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            workOrder.Creator_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
            workOrder.Creator_PersonnelClientLookupId = userData.DatabaseKey.Personnel.ClientLookupId;
            workOrder.ApproveBy_PersonnelClientLookupId = userData.DatabaseKey.Personnel.ClientLookupId;
            workOrder.ApproveBy_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
            workOrder.ApproveDate = DateTime.UtcNow;
            workOrder.Status = WorkOrderEvents.Approved;
            workOrder.SourceType = WorkOrderSourceTypes.OnDemand;
            workOrder.Priority = string.IsNullOrEmpty(woOnDemandModel.Priority) ? "" : woOnDemandModel.Priority;
            workOrder.RequiredDate = woOnDemandModel.RequiredDate;
            //#region V2-948
            //if (userData.Site.SourceAssetAccount == true)
            //{
            //    Equipment equipment = new Equipment
            //    {
            //        ClientId = userData.DatabaseKey.Client.ClientId,
            //        SiteId = userData.Site.SiteId,
            //        ClientLookupId = workOrder.ChargeToClientLookupId
            //    };
            //    equipment.RetrieveByClientLookupId(this.userData.DatabaseKey);
            //    workOrder.Labor_AccountId = equipment.Labor_AccountId;
            //}
            //#endregion
            workOrder.CreateFromOnDemandMaster_V2(this.userData.DatabaseKey, userData.Site.TimeZone);

            if (workOrder.ErrorMessages.Count == 0)
            {
                CreateEventLog(workOrder.WorkOrderId, WorkOrderEvents.Create);
                CreateEventLog(workOrder.WorkOrderId, WorkOrderEvents.Approved);
            }

            return workOrder;
        }
        #endregion Wo OnDemand

        #region Describe Need
        public WorkOrder AddWoDesc(WoDescriptionModel woDescriptionModel)
        {
            // Generate the new number
            string newClientlookupId = "";
            WorkOrder workOrder = new WorkOrder
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId
            };

            if (woDescriptionModel.ClientLookupId == null && WorkOrderStatusConstants.Wo_AutoGenerateEnabled)
            {
                newClientlookupId = CustomSequentialId.GetNextId(userData.DatabaseKey, AutoGenerateKey.WO_Annual, userData.DatabaseKey.User.DefaultSiteId, "");
            }

            workOrder.Type = woDescriptionModel.Type;
            workOrder.ChargeType = ChargeType.Equipment;
            workOrder.ChargeToClientLookupId = woDescriptionModel.ChargeToClientLookupId;
            workOrder.Description = woDescriptionModel.Description;

            workOrder.DownRequired = woDescriptionModel.DownRequired;
            workOrder.Priority = woDescriptionModel.Priority;
            workOrder.RequiredDate = woDescriptionModel.RequiredDate;
            // Fill 
            workOrder.ClientLookupId = newClientlookupId;
            workOrder.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            workOrder.Creator_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
            workOrder.Creator_PersonnelClientLookupId = userData.DatabaseKey.Personnel.ClientLookupId;
            //SOM-628
            workOrder.ApproveBy_PersonnelClientLookupId = userData.DatabaseKey.Personnel.ClientLookupId;
            workOrder.ApproveBy_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
            workOrder.ApproveDate = DateTime.UtcNow;
            workOrder.CreateMode = true;
            workOrder.Status = WorkOrderEvents.Approved;
            workOrder.CreateByPKForeignKeys(this.userData.DatabaseKey, userData.Site.TimeZone);

            if (workOrder.ErrorMessages.Count == 0)
            {
                CreateEventLog(workOrder.WorkOrderId, WorkOrderEvents.Create);
                CreateEventLog(workOrder.WorkOrderId, WorkOrderEvents.Approved);
            }

            return workOrder;
        }
        #endregion Describe Need 
        #region wo Describe Need Dynamic
        public WorkOrder AddWoDescDynamic(Models.Work_Order.UIConfiguration.WoDescriptionModelDynamic woDescriptionModelDynamic)
        {
            // Generate the new number
            string newClientlookupId = "";


            if (woDescriptionModelDynamic.ClientLookupId == null && WorkOrderStatusConstants.Wo_AutoGenerateEnabled)
            {
                newClientlookupId = CustomSequentialId.GetNextId(userData.DatabaseKey, AutoGenerateKey.WO_Annual, userData.DatabaseKey.User.DefaultSiteId, "");
            }

            PropertyInfo getpropertyInfo, setpropertyInfo;

            WorkOrder workOrder = new WorkOrder();
            workOrder.ClientId = userData.DatabaseKey.Client.ClientId;
            workOrder.ChargeType = ChargeType.Equipment;
            workOrder.ChargeToClientLookupId = woDescriptionModelDynamic.ChargeToClientLookupId ?? string.Empty;

            // Fill 
            workOrder.ClientLookupId = newClientlookupId;
            workOrder.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            workOrder.Creator_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
            workOrder.Creator_PersonnelClientLookupId = userData.DatabaseKey.Personnel.ClientLookupId;
            //SOM-628
            workOrder.ApproveBy_PersonnelClientLookupId = userData.DatabaseKey.Personnel.ClientLookupId;
            workOrder.ApproveBy_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
            workOrder.ApproveDate = DateTime.UtcNow;
            workOrder.CreateMode = true;
            workOrder.Status = WorkOrderEvents.Approved;
            List<UIConfigurationDetailsForModelValidation> configDetails = new RetrieveDataForUIConfiguration().Retrieve(DataDictionaryViewNameConstant.WorkOrderDescribeAdd, userData);
            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == false && x.Section == false);
            foreach (var item in ColumnDetails)
            {
                getpropertyInfo = woDescriptionModelDynamic.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(woDescriptionModelDynamic);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }

                setpropertyInfo = workOrder.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(workOrder, val);
            }
            #region V2-948
            if (userData.Site.SourceAssetAccount == true && workOrder.Labor_AccountId == 0)
            {
                Equipment equipment = new Equipment
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    SiteId = userData.Site.SiteId,
                    EquipmentId = workOrder.ChargeToId
                };
                equipment.Retrieve(this.userData.DatabaseKey);
                workOrder.Labor_AccountId = equipment.Labor_AccountId;
            }
            #endregion
            workOrder.CreateByPKForeignKeys(this.userData.DatabaseKey, userData.Site.TimeZone);

            if (workOrder.ErrorMessages.Count == 0)
            {
                #region V2-1077
                ProcessAlert objAlert = new ProcessAlert(this.userData);
                List<long> listWO = new List<long>();
                listWO.Add(workOrder.WorkOrderId);
                objAlert.CreateAlert<WorkOrder>(AlertTypeEnum.WorkOrderPlanner, listWO);
                #endregion
                CreateEventLog(workOrder.WorkOrderId, WorkOrderEvents.Create);
                CreateEventLog(workOrder.WorkOrderId, WorkOrderEvents.Approved);
            }


            if (workOrder.ErrorMessages != null && workOrder.ErrorMessages.Count == 0 && configDetails.Any(x => x.Display == true && x.UDF == true && x.Section == false))
            {
                IEnumerable<string> errors = AddWorkorderDescUDFDynamic(woDescriptionModelDynamic, workOrder.WorkOrderId, configDetails);
                if (errors != null && errors.Count() > 0)
                {
                    workOrder.ErrorMessages.AddRange(errors);
                }
            }

            return workOrder;
        }
        public List<string> AddWorkorderDescUDFDynamic(Models.Work_Order.UIConfiguration.WoDescriptionModelDynamic woDescriptionModelDynamic, long WorkOrderId,
            List<UIConfigurationDetailsForModelValidation> configDetails)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            WorkOrderUDF woUDF = new WorkOrderUDF();
            woUDF.ClientId = userData.DatabaseKey.Client.ClientId;
            woUDF.WorkOrderId = WorkOrderId;

            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == true && x.Section == false);
            foreach (var item in ColumnDetails)
            {
                getpropertyInfo = woDescriptionModelDynamic.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(woDescriptionModelDynamic);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                setpropertyInfo = woUDF.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(woUDF, val);
            }

            woUDF.Create(_dbKey);
            return woUDF.ErrorMessages;
        }

        #endregion Describe Need 
        #region V2-276
        public ActivityLogCostModel GetAllCosts(long WorkOrderId, string costtype)
        {
            ActivityLogCostModel activityLogCountModel = new ActivityLogCostModel();
            WorkOrder wo = new WorkOrder()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ObjectId = WorkOrderId,
                IsActualOrEstimated = costtype,
                SiteId = this.userData.DatabaseKey.Personnel.SiteId,
                ChargeType = AttachmentTableConstant.WorkOrder,
            };
            var data = wo.WorkOrderRetrieveForWorkOrderCostWidget(userData.DatabaseKey);
            if (data != null && data.Count > 0)
            {
                activityLogCountModel.TotalCost = data[0].TotalCost;
                activityLogCountModel.PartCost = data[0].PartCost;
                activityLogCountModel.LaborCost = data[0].LaborCost;
                activityLogCountModel.OtherCost = data[0].OtherCost;
                if (data[0].TotalCost != 0)
                {
                    activityLogCountModel.PartCostPercentage = (activityLogCountModel.PartCost / activityLogCountModel.TotalCost) * 100;
                    activityLogCountModel.LaborCostPercentage = (activityLogCountModel.LaborCost / activityLogCountModel.TotalCost) * 100;
                    activityLogCountModel.OtherCostPercentage = (activityLogCountModel.OtherCost / activityLogCountModel.TotalCost) * 100;
                }
            }
            return activityLogCountModel;
        }
        #endregion

        #region V2-288: Request/Order Grid
        public List<RequestOrderModel> GetRequestOrder(long workOrderId)
        {
            RequestOrderModel objRequestOrderModel;
            List<RequestOrderModel> RequestOrderModelList = new List<RequestOrderModel>();
            WorkOrder workOrder = new WorkOrder()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                WorkOrderId = workOrderId
            };
            var reqData = workOrder.RetrievePOandPR(userData.DatabaseKey, userData.Site.TimeZone);
            foreach (var item in reqData)
            {
                objRequestOrderModel = new RequestOrderModel();
                objRequestOrderModel.ClientLookupId = item.PONumber;
                objRequestOrderModel.Status = item.POStatus;
                objRequestOrderModel.Vendor = item.VendorClientlookupId;
                objRequestOrderModel.VendorName = item.VendorName;
                objRequestOrderModel.Created = item.CreateDate;
                RequestOrderModelList.Add(objRequestOrderModel);
            }
            return RequestOrderModelList;
        }
        #endregion
        #region Send For Approval
        public WorkOrder WoSendForApproval(WoSendForApprovalModel wosm)
        {
            WorkOrder wo = new WorkOrder()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                WorkOrderId = wosm.WorkOrderId,
                SiteId = this.userData.DatabaseKey.Personnel.SiteId
            };
            wo.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);

            wo.Status = WorkOrderStatusConstants.AwaitingApproval;
            wo.UpdateByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);
            if (wo.ErrorMessages.Count == 0)
            {
                List<long> workorderid = new List<long>() { wosm.WorkOrderId };
                var UserList = new List<Tuple<long, string, string>>();
                CommonWrapper coWrapper = new CommonWrapper(userData);
                var PersonnelInfo = coWrapper.GetPersonnelDetailsByPersonnelId(wosm.PersonnelId);
                if (PersonnelInfo != null)
                {
                    UserList.Add
                     (
                               Tuple.Create(Convert.ToInt64(PersonnelInfo.PersonnelId), PersonnelInfo.UserName, PersonnelInfo.Email)
                    );
                    ProcessAlert objAlert = new ProcessAlert(this.userData);
                    Task t1 = Task.Factory.StartNew(() => objAlert.CreateAlert<WorkOrder>(AlertTypeEnum.WorkOrderApprovalNeeded, workorderid, UserList));
                }

                Task t2 = Task.Factory.StartNew(() => CreateEventLog(wo.WorkOrderId, WorkOrderEvents.AwaitApproval));

            }
            return wo;
        }
        #endregion

        #region V2-611 Add workorder
        public WorkOrder addWorkOrderDynamic(WorkOrderVM objVM) //V2-611
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            WorkOrder wo = new WorkOrder()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.Personnel.SiteId
            };

            if (objVM.AddWorkorder.ClientLookupId == null && WorkOrderStatusConstants.Wo_AutoGenerateEnabled)
            {
                newClientlookupId = CustomSequentialId.GetNextId(userData.DatabaseKey, AutoGenerateKey.WO_Annual, userData.DatabaseKey.User.DefaultSiteId, "");
            }
            wo.ClientLookupId = newClientlookupId;
            wo.Creator_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
            wo.Creator_PersonnelClientLookupId = userData.DatabaseKey.Personnel.ClientLookupId;
            wo.ApproveBy_PersonnelClientLookupId = userData.DatabaseKey.Personnel.ClientLookupId;
            wo.ApproveBy_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
            wo.ApproveDate = DateTime.UtcNow;
            wo.CreateMode = true;
            wo.ChargeToClientLookupId = objVM.AddWorkorder.ChargeToClientLookupId ?? string.Empty;
            wo.ChargeType = ChargeType.Equipment;
            wo.Status = WorkOrderStatusConstants.Approved;

            List<UIConfigurationDetailsForModelValidation> configDetails = new RetrieveDataForUIConfiguration().Retrieve(DataDictionaryViewNameConstant.AddWorkOrder, userData);
            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == false && x.Section == false);

            foreach (var item in ColumnDetails)
            {
                getpropertyInfo = objVM.AddWorkorder.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(objVM.AddWorkorder);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }

                setpropertyInfo = wo.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(wo, val);
            }

            wo.CreateByPKForeignKeys(this.userData.DatabaseKey, userData.Site.TimeZone);
            if (wo.ErrorMessages.Count == 0)
            {
                #region V2-1077
                ProcessAlert objAlert = new ProcessAlert(this.userData);
                List<long> listWO = new List<long>();
                listWO.Add(wo.WorkOrderId);
                objAlert.CreateAlert<WorkOrder>(AlertTypeEnum.WorkOrderPlanner, listWO);
                #endregion
                // V2-1123

                // RKL - 2024-Nov-11 - begin
                // Not Writing Event Log Entries if there are no udf displayed
                if (configDetails.Any(x => x.Display == true && x.UDF == true && x.Section == false))
                {
                    IEnumerable<string> errors = AddWorkorderUDFDynamic(objVM.AddWorkorder, wo.WorkOrderId, configDetails);
                    if (errors != null && errors.Count() > 0)
                    {
                        wo.ErrorMessages.AddRange(errors);
                        return wo;
                    }
                }
                // Create Event Logs - Create then Approved
                CreateEventLog(wo.WorkOrderId, WorkOrderEvents.Create);
                CreateEventLog(wo.WorkOrderId, WorkOrderEvents.Approved);
                // RKL - 2024-Nov-11 - End
            }
            return wo;
        }

        public List<string> AddWorkorderUDFDynamic(Models.Work_Order.UIConfiguration.AddWorkOrderModelDynamic workorder, long WorkOrderId,
            List<UIConfigurationDetailsForModelValidation> configDetails)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            WorkOrderUDF workOrderUDF = new WorkOrderUDF();
            workOrderUDF.ClientId = userData.DatabaseKey.Client.ClientId;
            workOrderUDF.WorkOrderId = WorkOrderId;

            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == true && x.Section == false);
            foreach (var item in ColumnDetails)
            {
                getpropertyInfo = workorder.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(workorder);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                setpropertyInfo = workOrderUDF.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(workOrderUDF, val);
            }

            workOrderUDF.Create(_dbKey);
            return workOrderUDF.ErrorMessages;
        }
        #endregion

        #region Edit Work Order V2-611
        public EditWorkOrderModelDynamic getEditWorkorderDetailsDynamic(long workOrderId)
        {
            EditWorkOrderModelDynamic editWoDynamic = new EditWorkOrderModelDynamic();
            WorkOrder WO = getWorkOderDetailsByWorkOrderId(workOrderId);
            WorkOrderUDF WOUDF = getWorkOrderUDFByWorkOrderId(workOrderId);

            editWoDynamic = MapWorkOrderData(editWoDynamic, WO);
            editWoDynamic = MapWorkOrderUDFData(editWoDynamic, WOUDF);

            return editWoDynamic;
        }

        public WorkOrder getWorkOderDetailsByWorkOrderId(long workOrderId)
        {
            WorkOrder workorder = new WorkOrder()
            {
                ClientId = _dbKey.Client.ClientId,
                WorkOrderId = workOrderId
            };
            workorder.RetrieveByPKForeignKeys(this.userData.DatabaseKey, userData.Site.TimeZone);
            return workorder;
        }
        public WorkOrderUDF getWorkOrderUDFByWorkOrderId(long workOrderId)
        {
            WorkOrderUDF workOrderUDF = new WorkOrderUDF()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                WorkOrderId = workOrderId
            };

            workOrderUDF = workOrderUDF.RetriveWorkOrderUDFByWorkOrderId(this.userData.DatabaseKey);
            return workOrderUDF;
        }

        public EditWorkOrderModelDynamic MapWorkOrderData(EditWorkOrderModelDynamic editWorkOrderModelDynamic, WorkOrder workorder)
        {
            editWorkOrderModelDynamic.WorkOrderId = workorder.WorkOrderId;
            editWorkOrderModelDynamic.ClientLookupId = workorder.ClientLookupId;
            editWorkOrderModelDynamic.ActionCode = workorder.ActionCode;
            editWorkOrderModelDynamic.ActualDuration = workorder.ActualDuration;
            if (workorder.ActualFinishDate != null && workorder.ActualFinishDate != default(DateTime))
            {
                editWorkOrderModelDynamic.ActualFinishDate = workorder.ActualFinishDate;
            }
            else
            {
                editWorkOrderModelDynamic.ActualFinishDate = null;
            }
            editWorkOrderModelDynamic.ChargeToId = workorder.ChargeToId;
            editWorkOrderModelDynamic.ChargeTo_Name = workorder.ChargeTo_Name;
            editWorkOrderModelDynamic.CompleteAllTasks = workorder.CompleteAllTasks;
            editWorkOrderModelDynamic.Description = workorder.Description;
            editWorkOrderModelDynamic.DownRequired = workorder.DownRequired;
            editWorkOrderModelDynamic.FailureCode = workorder.FailureCode;
            editWorkOrderModelDynamic.Labor_AccountId = workorder.Labor_AccountId;
            editWorkOrderModelDynamic.Location = workorder.Location;
            editWorkOrderModelDynamic.Priority = workorder.Priority;
            editWorkOrderModelDynamic.ProjectId = workorder.ProjectId;
            editWorkOrderModelDynamic.ReasonNotDone = workorder.ReasonNotDone;
            if (workorder.RequiredDate != null && workorder.RequiredDate != default(DateTime))
            {
                editWorkOrderModelDynamic.RequiredDate = workorder.RequiredDate;
            }
            else
            {
                editWorkOrderModelDynamic.RequiredDate = null;
            }
            editWorkOrderModelDynamic.ScheduledDuration = workorder.ScheduledDuration;
            if (workorder.ScheduledStartDate != null && workorder.ScheduledStartDate != default(DateTime))
            {
                editWorkOrderModelDynamic.ScheduledStartDate = workorder.ScheduledStartDate;
            }
            else
            {
                editWorkOrderModelDynamic.ScheduledStartDate = null;
            }
            editWorkOrderModelDynamic.Shift = workorder.Shift;
            editWorkOrderModelDynamic.SourceId = workorder.SourceId;
            editWorkOrderModelDynamic.SourceType = workorder.SourceType;
            editWorkOrderModelDynamic.Status = workorder.Status;
            editWorkOrderModelDynamic.Type = workorder.Type;
            editWorkOrderModelDynamic.ChargeToClientLookupId = workorder.ChargeToClientLookupId;
            editWorkOrderModelDynamic.AccountClientLookupId = workorder.AccountClientLookupId;
            editWorkOrderModelDynamic.SourceIdClientLookupId = workorder.SourceIdClientLookupId;
            editWorkOrderModelDynamic.RequestorName = workorder.RequestorName;
            editWorkOrderModelDynamic.RequestorPhoneNumber = workorder.RequestorPhoneNumber;
            editWorkOrderModelDynamic.RequestorEmail = workorder.RequestorEmail;
            editWorkOrderModelDynamic.CreateDate = workorder.CreateDate;
            editWorkOrderModelDynamic.CreateBy_PersonnelName = workorder.CreateBy_PersonnelName;
            editWorkOrderModelDynamic.CompleteBy_PersonnelName = workorder.CompleteBy_PersonnelName;
            editWorkOrderModelDynamic.RootCauseCode = workorder.RootCauseCode;
            editWorkOrderModelDynamic.ReasonforDown = workorder.ReasonforDown;
            editWorkOrderModelDynamic.ProjectClientLookupId = workorder.ProjectClientLookupId; //V2-782
            if (workorder.Planner_PersonnelId == 0)
            {
                editWorkOrderModelDynamic.Planner_PersonnelId = null; //V2-1076;
            }
            else
            {
                editWorkOrderModelDynamic.Planner_PersonnelId = workorder.Planner_PersonnelId; //V2-1076

            }
            // V2-1157
            // RKL - 2025-02-12 Support Ticket - 10852 
            //  For editing - we must load the planner clientlookupid - not the full name
            editWorkOrderModelDynamic.PlannerClientLookupId = workorder.Planner_PersonnelClientLookupId;  // V2-1076
            //editWorkOrderModelDynamic.PlannerClientLookupId = workorder.PlannerFullName;                // V2-1157
            return editWorkOrderModelDynamic;
        }
        private EditWorkOrderModelDynamic MapWorkOrderUDFData(EditWorkOrderModelDynamic editWorkorderModelDynamic, WorkOrderUDF workOrderUDF)
        {
            if (workOrderUDF != null)
            {
                editWorkorderModelDynamic.WorkOrderUDFId = workOrderUDF.WorkOrderUDFId;

                editWorkorderModelDynamic.Text1 = workOrderUDF.Text1;
                editWorkorderModelDynamic.Text2 = workOrderUDF.Text2;
                editWorkorderModelDynamic.Text3 = workOrderUDF.Text3;
                editWorkorderModelDynamic.Text4 = workOrderUDF.Text4;

                if (workOrderUDF.Date1 != null && workOrderUDF.Date1 == DateTime.MinValue)
                {
                    editWorkorderModelDynamic.Date1 = null;
                }
                else
                {
                    editWorkorderModelDynamic.Date1 = workOrderUDF.Date1;
                }
                if (workOrderUDF.Date2 != null && workOrderUDF.Date2 == DateTime.MinValue)
                {
                    editWorkorderModelDynamic.Date2 = null;
                }
                else
                {
                    editWorkorderModelDynamic.Date2 = workOrderUDF.Date2;
                }
                if (workOrderUDF.Date3 != null && workOrderUDF.Date3 == DateTime.MinValue)
                {
                    editWorkorderModelDynamic.Date3 = null;
                }
                else
                {
                    editWorkorderModelDynamic.Date3 = workOrderUDF.Date3;
                }
                if (workOrderUDF.Date4 != null && workOrderUDF.Date4 == DateTime.MinValue)
                {
                    editWorkorderModelDynamic.Date4 = null;
                }
                else
                {
                    editWorkorderModelDynamic.Date4 = workOrderUDF.Date4;
                }

                editWorkorderModelDynamic.Bit1 = workOrderUDF.Bit1;
                editWorkorderModelDynamic.Bit2 = workOrderUDF.Bit2;
                editWorkorderModelDynamic.Bit3 = workOrderUDF.Bit3;
                editWorkorderModelDynamic.Bit4 = workOrderUDF.Bit4;

                editWorkorderModelDynamic.Numeric1 = workOrderUDF.Numeric1;
                editWorkorderModelDynamic.Numeric2 = workOrderUDF.Numeric2;
                editWorkorderModelDynamic.Numeric3 = workOrderUDF.Numeric3;
                editWorkorderModelDynamic.Numeric4 = workOrderUDF.Numeric4;

                editWorkorderModelDynamic.Select1 = workOrderUDF.Select1;
                editWorkorderModelDynamic.Select2 = workOrderUDF.Select2;
                editWorkorderModelDynamic.Select3 = workOrderUDF.Select3;
                editWorkorderModelDynamic.Select4 = workOrderUDF.Select4;
            }
            return editWorkorderModelDynamic;
        }

        public WorkOrder editWorkOrderDynamic(WorkOrderVM objVM)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            WorkOrder wo = new WorkOrder()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                WorkOrderId = objVM.EditWorkOrder.WorkOrderId,
                SiteId = this.userData.DatabaseKey.Personnel.SiteId
            };
            wo.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);

            wo.ClientLookupId = objVM.EditWorkOrder?.ClientLookupId ?? string.Empty;
            wo.Planner_PersonnelId = objVM.EditWorkOrder?.Planner_PersonnelId ?? 0;
            wo.Planner_PersonnelClientLookupId = objVM.EditWorkOrder?.PlannerClientLookupId ?? string.Empty;
            List<UIConfigurationDetailsForModelValidation> configDetails = new RetrieveDataForUIConfiguration()
                                       .Retrieve(DataDictionaryViewNameConstant.EditWorkOrder, userData);
            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == false && x.Section == false && x.ViewOnly == false);
            foreach (var item in ColumnDetails)
            {
                getpropertyInfo = objVM.EditWorkOrder.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(objVM.EditWorkOrder);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }

                setpropertyInfo = wo.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(wo, val);
            }
            wo.ChargeType = ChargeType.Equipment;
            wo.CreateMode = false;
            wo.ChargeToClientLookupId = objVM.EditWorkOrder?.ChargeToClientLookupId ?? string.Empty;
            wo.UpdateByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);
            #region V2-1077
            if (wo.ErrorMessages == null || wo.ErrorMessages.Count == 0)
            {
                ProcessAlert objAlert = new ProcessAlert(this.userData);
                List<long> listWO = new List<long>();
                listWO.Add(wo.WorkOrderId);
                objAlert.CreateAlert<WorkOrder>(AlertTypeEnum.WorkOrderPlanner, listWO);
            }
            #endregion
            if (wo.ErrorMessages != null && wo.ErrorMessages.Count == 0 &&
                configDetails.Any(x => x.Display == true && x.UDF == true && x.Section == false && x.ViewOnly == false))
            {
                IEnumerable<string> errors = EditWorkorderUDFDynamic(objVM.EditWorkOrder, configDetails);
                if (errors != null && errors.Count() > 0)
                {
                    wo.ErrorMessages.AddRange(errors);
                }
            }
            return wo;
        }
        public List<string> EditWorkorderUDFDynamic(Models.Work_Order.UIConfiguration.EditWorkOrderModelDynamic workorderDyn, List<UIConfigurationDetailsForModelValidation> configDetails)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            WorkOrderUDF workorderUDF = new WorkOrderUDF()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                WorkOrderId = workorderDyn.WorkOrderId
            };
            workorderUDF = workorderUDF.RetriveWorkOrderUDFByWorkOrderId(_dbKey);

            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == true && x.Section == false && x.ViewOnly == false);
            foreach (var item in ColumnDetails)
            {
                getpropertyInfo = workorderDyn.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(workorderDyn);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                setpropertyInfo = workorderUDF.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(workorderUDF, val);
            }
            if (workorderUDF.WorkOrderId == 0)
            {
                workorderUDF.WorkOrderId = workorderDyn.WorkOrderId;
                workorderUDF.Create(_dbKey);
            }
            else
            {
                workorderUDF.Update(_dbKey);
            }

            return workorderUDF.ErrorMessages;
        }
        #endregion

        #region View Work Order V2-611
        public ViewWorkOrderModelDynamic MapWorkOrderDataForView(ViewWorkOrderModelDynamic viewWorkOrderModelDynamic, WorkOrder workorder)
        {
            viewWorkOrderModelDynamic.WorkOrderId = workorder.WorkOrderId;
            viewWorkOrderModelDynamic.ClientLookupId = workorder.ClientLookupId;
            viewWorkOrderModelDynamic.ActionCode = workorder.ActionCode;
            viewWorkOrderModelDynamic.ActualDuration = workorder.ActualDuration;
            if (workorder.ActualFinishDate != null && workorder.ActualFinishDate != default(DateTime))
            {
                viewWorkOrderModelDynamic.ActualFinishDate = workorder.ActualFinishDate;
            }
            else
            {
                viewWorkOrderModelDynamic.ActualFinishDate = null;
            }

            viewWorkOrderModelDynamic.Labor_AccountClientLookupId = workorder.AccountClientLookupId;
            viewWorkOrderModelDynamic.ChargeToId = workorder.ChargeToId;

            viewWorkOrderModelDynamic.ChargeTo_Name = workorder.ChargeTo_Name;


            viewWorkOrderModelDynamic.CompleteAllTasks = workorder.CompleteAllTasks;

            viewWorkOrderModelDynamic.Description = workorder.Description;
            viewWorkOrderModelDynamic.DownRequired = workorder.DownRequired;

            viewWorkOrderModelDynamic.FailureCode = workorder.FailureCode;

            viewWorkOrderModelDynamic.Labor_AccountId = workorder.Labor_AccountId;
            viewWorkOrderModelDynamic.Location = workorder.Location;

            viewWorkOrderModelDynamic.Priority = workorder.Priority;
            viewWorkOrderModelDynamic.ProjectId = workorder.ProjectId;
            viewWorkOrderModelDynamic.ProjectClientLookupId = workorder.ProjectClientLookupId;
            viewWorkOrderModelDynamic.ReasonNotDone = workorder.ReasonNotDone;


            if (workorder.RequiredDate != null && workorder.RequiredDate != default(DateTime))
            {
                viewWorkOrderModelDynamic.RequiredDate = workorder.RequiredDate;
            }
            else
            {
                viewWorkOrderModelDynamic.RequiredDate = null;
            }

            viewWorkOrderModelDynamic.ScheduledDuration = workorder.ScheduledDuration;

            if (workorder.ScheduledStartDate != null && workorder.ScheduledStartDate != default(DateTime))
            {
                viewWorkOrderModelDynamic.ScheduledStartDate = workorder.ScheduledStartDate;
            }
            else
            {
                viewWorkOrderModelDynamic.ScheduledStartDate = null;
            }

            viewWorkOrderModelDynamic.Shift = workorder.Shift;
            viewWorkOrderModelDynamic.SourceType = workorder.SourceType;
            viewWorkOrderModelDynamic.Status = workorder.Status;
            viewWorkOrderModelDynamic.Type = workorder.Type;
            viewWorkOrderModelDynamic.ChargeToClientLookupId = workorder.ChargeToClientLookupId;

            viewWorkOrderModelDynamic.CreateBy = workorder.Createby;
            viewWorkOrderModelDynamic.CreateDate = workorder.CreateDate;
            viewWorkOrderModelDynamic.CreateBy_PersonnelName = workorder.CreateBy_PersonnelName;
            viewWorkOrderModelDynamic.CompleteBy_PersonnelId = workorder.CompleteBy_PersonnelId;
            viewWorkOrderModelDynamic.CompleteBy_PersonnelName = workorder.CompleteBy_PersonnelName;
            viewWorkOrderModelDynamic.SourceIdClientLookupId = workorder.SourceIdClientLookupId;

            viewWorkOrderModelDynamic.RequestorName = workorder.RequestorName;
            viewWorkOrderModelDynamic.RequestorPhoneNumber = workorder.RequestorPhoneNumber;
            viewWorkOrderModelDynamic.RequestorEmail = workorder.RequestorEmail;
            viewWorkOrderModelDynamic.RootCauseCode = workorder.RootCauseCode;
            viewWorkOrderModelDynamic.ReasonforDown = workorder.ReasonforDown;
            if (workorder.SchedInitDate != null && workorder.SchedInitDate != default(DateTime))
            {
                viewWorkOrderModelDynamic.SchedInitDate = workorder.SchedInitDate;
            }
            else
            {
                viewWorkOrderModelDynamic.SchedInitDate = null;
            }
            #region V2-1076
            //viewWorkOrderModelDynamic.PlannerClientLookupId = workorder.Planner_PersonnelClientLookupId;
            viewWorkOrderModelDynamic.PlannerClientLookupId = workorder.PlannerFullName;  // V2-1157
            viewWorkOrderModelDynamic.Planner_PersonnelId = workorder.Planner_PersonnelId;
            #endregion
            return viewWorkOrderModelDynamic;
        }
        public ViewWorkOrderModelDynamic MapWorkOrderUDFDataForView(ViewWorkOrderModelDynamic viewWorkorderModelDynamic, WorkOrderUDF workOrderUDF)
        {
            if (workOrderUDF != null)
            {
                viewWorkorderModelDynamic.WorkOrderUDFId = workOrderUDF.WorkOrderUDFId;

                viewWorkorderModelDynamic.Text1 = workOrderUDF.Text1;
                viewWorkorderModelDynamic.Text2 = workOrderUDF.Text2;
                viewWorkorderModelDynamic.Text3 = workOrderUDF.Text3;
                viewWorkorderModelDynamic.Text4 = workOrderUDF.Text4;

                if (workOrderUDF.Date1 != null && workOrderUDF.Date1 == DateTime.MinValue)
                {
                    viewWorkorderModelDynamic.Date1 = null;
                }
                else
                {
                    viewWorkorderModelDynamic.Date1 = workOrderUDF.Date1;
                }
                if (workOrderUDF.Date2 != null && workOrderUDF.Date2 == DateTime.MinValue)
                {
                    viewWorkorderModelDynamic.Date2 = null;
                }
                else
                {
                    viewWorkorderModelDynamic.Date2 = workOrderUDF.Date2;
                }
                if (workOrderUDF.Date3 != null && workOrderUDF.Date3 == DateTime.MinValue)
                {
                    viewWorkorderModelDynamic.Date3 = null;
                }
                else
                {
                    viewWorkorderModelDynamic.Date3 = workOrderUDF.Date3;
                }
                if (workOrderUDF.Date4 != null && workOrderUDF.Date4 == DateTime.MinValue)
                {
                    viewWorkorderModelDynamic.Date4 = null;
                }
                else
                {
                    viewWorkorderModelDynamic.Date4 = workOrderUDF.Date4;
                }

                viewWorkorderModelDynamic.Bit1 = workOrderUDF.Bit1;
                viewWorkorderModelDynamic.Bit2 = workOrderUDF.Bit2;
                viewWorkorderModelDynamic.Bit3 = workOrderUDF.Bit3;
                viewWorkorderModelDynamic.Bit4 = workOrderUDF.Bit4;

                viewWorkorderModelDynamic.Numeric1 = workOrderUDF.Numeric1;
                viewWorkorderModelDynamic.Numeric2 = workOrderUDF.Numeric2;
                viewWorkorderModelDynamic.Numeric3 = workOrderUDF.Numeric3;
                viewWorkorderModelDynamic.Numeric4 = workOrderUDF.Numeric4;

                viewWorkorderModelDynamic.Select1 = workOrderUDF.Select1;
                viewWorkorderModelDynamic.Select2 = workOrderUDF.Select2;
                viewWorkorderModelDynamic.Select3 = workOrderUDF.Select3;
                viewWorkorderModelDynamic.Select4 = workOrderUDF.Select4;

                viewWorkorderModelDynamic.Select1ClientLookupId = workOrderUDF.Select1ClientLookupId;
                viewWorkorderModelDynamic.Select2ClientLookupId = workOrderUDF.Select2ClientLookupId;
                viewWorkorderModelDynamic.Select3ClientLookupId = workOrderUDF.Select3ClientLookupId;
                viewWorkorderModelDynamic.Select4ClientLookupId = workOrderUDF.Select4ClientLookupId;
            }
            return viewWorkorderModelDynamic;
        }
        public WorkOrder getWorkOderDetailsById_V2(long workOrderId)
        {
            WorkOrder workorder = new WorkOrder()
            {
                ClientId = _dbKey.Client.ClientId,
                WorkOrderId = workOrderId
            };
            workorder.RetrieveByPKForeignKeys(this.userData.DatabaseKey, userData.Site.TimeZone);
            return workorder;
        }
        #endregion
        #region Add actual parts
        internal List<PartHistory> PartIssueAddData(ActualPart Obj, string ClientLookupId, string Description, string UPCCode)
        {
            List<PartHistory> tmpList = new List<PartHistory>();
            List<PartHistory> PartHistoryListTemp = new List<PartHistory>();
            if (Obj != null)
            {
                tmpList.Add(new PartHistory
                {
                    IssueToClientLookupId = userData.DatabaseKey.Personnel.ClientLookupId,
                    IssuedTo = Convert.ToString(userData.DatabaseKey.Personnel.PersonnelId),
                    PartStoreroomId = 0,
                    TransactionDate = DateTime.UtcNow,
                    ChargeType_Primary = SearchCategoryConstants.TBL_WORKORDER,
                    ChargeToClientLookupId = Obj.WorkOrderClientLookupId,
                    ChargeToId_Primary = Convert.ToInt64(Obj.WorkOrderId),
                    TransactionQuantity = Obj.Quantity,
                    PartClientLookupId = ClientLookupId,
                    PartId = Obj.PartId,
                    Description = Description ?? string.Empty,
                    SiteId = userData.DatabaseKey.Personnel.SiteId,
                    TransactionType = PartHistoryTranTypes.PartIssue,
                    IsPartIssue = true,
                    ErrorMessagerow = null,
                    PartUPCCode = UPCCode ?? string.Empty,
                    PerformedById = 0,
                    RequestorId = 0,
                    IsPerformAdjustment = true,
                    MultiStoreroom = userData.DatabaseKey.Client.UseMultiStoreroom, //V2-687
                    StoreroomId = Obj.StoreroomId ?? 0 // V2-687
                });
            }

            PartHistory parthistory = new PartHistory() { PartHistoryList = tmpList };
            PartHistoryListTemp = parthistory.CreateByForeignKeysnew(userData.DatabaseKey);
            return PartHistoryListTemp;
        }
        #endregion
        #region Equipment QR Scanner V2-625
        public Equipment GetEquipmentIdByClientLookUpId(string clientLookUpId)
        {
            Equipment equipment = new Equipment { ClientId = _dbKey.Client.ClientId, SiteId = _dbKey.User.DefaultSiteId, ClientLookupId = clientLookUpId };
            var result = equipment.RetrieveEquipmentIdByClientLookupIdV2(_dbKey);
            return result;
        }
        #endregion

        #region V2-624 Return parts
        internal List<PartHistory> ReturnPartData(ActualPart Obj)
        {
            List<PartHistory> tmpList = new List<PartHistory>();
            List<PartHistory> PartHistoryListTemp = new List<PartHistory>();
            if (Obj != null)
            {
                tmpList.Add(new PartHistory
                {
                    IssueToClientLookupId = userData.DatabaseKey.Personnel.ClientLookupId,
                    IssuedTo = Convert.ToString(userData.DatabaseKey.Personnel.PersonnelId),
                    PartStoreroomId = 0,
                    TransactionDate = DateTime.UtcNow,
                    ChargeType_Primary = AttachmentTableConstant.WorkOrder,
                    ChargeToClientLookupId = Obj.WorkOrderClientLookupId,
                    ChargeToId_Primary = Convert.ToInt64(Obj.WorkOrderId),
                    TransactionQuantity = Obj.Quantity,
                    PartClientLookupId = Obj.Part_ClientLookupId,
                    PartId = Obj.PartId,
                    Description = Obj.Description ?? string.Empty,
                    SiteId = userData.DatabaseKey.Personnel.SiteId,
                    TransactionType = PartHistoryTranTypes.PartIssue,
                    IsPartIssue = true,
                    ErrorMessagerow = null,
                    PartUPCCode = Obj.UPCCode ?? string.Empty,
                    PerformedById = 0,
                    RequestorId = 0,
                    IsPerformAdjustment = true,
                    MultiStoreroom = userData.DatabaseKey.Client.UseMultiStoreroom, // V2-687
                    StoreroomId = Obj.StoreroomId ?? 0 // V2-687
                });
            }

            PartHistory parthistory = new PartHistory() { PartHistoryList = tmpList };
            PartHistoryListTemp = parthistory.CreateReturnPartByForeignKeysnew(userData.DatabaseKey);
            return PartHistoryListTemp;
        }
        #endregion

        #region Add Project from Workorder Deials V2-626
        public WorkOrder AddProjectToWorkorder(long workOrderId, long projectId = 0)
        {
            WorkOrder wo = new WorkOrder()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                WorkOrderId = workOrderId,
                SiteId = this.userData.DatabaseKey.Personnel.SiteId
            };
            wo.Retrieve(userData.DatabaseKey);

            wo.ProjectId = projectId;
            wo.Update(userData.DatabaseKey);
            return wo;
        }
        #endregion

        #region WorkOrder Completion Wizard
        public WorkOrder CompleteWOFromWizard(WorkOrderCompletionInformationModelDynamic objCompletion, long WorkOrderId, WorkOrderCompletionWizard objWizard,
            bool TimecardTab, bool AutoAddTimecard, bool WOCompletionCriteriaTab = false)
        {
            WorkOrder wo = new WorkOrder()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                WorkOrderId = WorkOrderId,
                SiteId = userData.DatabaseKey.Personnel.SiteId
            };
            wo.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);

            //-- Completion information dynamic tab

            PropertyInfo getpropertyInfo, setpropertyInfo;

            List<UIConfigurationDetailsForModelValidation> configDetails = new RetrieveDataForUIConfiguration()
                                       .Retrieve(DataDictionaryViewNameConstant.WorkOrderCompletion, userData);
            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == false && x.Section == false && x.ViewOnly == false);
            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, objCompletion);
                getpropertyInfo = objCompletion.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(objCompletion);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }

                setpropertyInfo = wo.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(wo, val);
            }

            WorkOrderUDF workOrderUDF = WorkorderUDFDynamicCompletion(objCompletion, configDetails, WorkOrderId);
            wo.WorkOrderUDF = workOrderUDF;
            //-- Completion information dynamic tab

            wo.CompleteComments = objWizard.CompletionComments ?? string.Empty;

            // Update with completion information
            wo.Status = WorkOrderStatusConstants.Complete;
            wo.CompleteBy_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;

            //V2-728
            if (WOCompletionCriteriaTab == true && objWizard.CompletionCriteriaConfirm == true)
            {
                wo.SignOffDate = DateTime.UtcNow;
                wo.SignoffBy_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
                wo.SignoffBy_PersonnelClientLookupId = userData.DatabaseKey.Personnel.ClientLookupId;
            }
            wo.CompleteDate = DateTime.UtcNow;
            wo.CompleteBy_PersonnelClientLookupId = userData.DatabaseKey.Personnel.ClientLookupId; // Converts to personnel id in the sp
            if (wo.ActualFinishDate == null || wo.ActualFinishDate == DateTime.MinValue)
            {
                wo.ActualFinishDate = DateTime.UtcNow.ConvertFromUTCToUser(userData.Site.TimeZone);
            }

            wo.TimecardTab = TimecardTab;
            wo.AutoAddTimecard = AutoAddTimecard;

            List<Timecard> TimecardList = new List<Timecard>();
            Timecard timeCard;
            for (int i = 0; i < objWizard.WOLabors.Count; i++)
            {
                timeCard = new Timecard();
                timeCard.ClientId = userData.DatabaseKey.Client.ClientId;
                timeCard.SiteId = userData.DatabaseKey.User.DefaultSiteId;
                timeCard.ChargeType_Primary = ChargeType.WorkOrder;
                timeCard.ChargeToId_Primary = WorkOrderId;
                timeCard.PersonnelId = objWizard.WOLabors[i].PersonnelID ?? 0;
                timeCard.StartDate = objWizard.WOLabors[i].StartDate ?? DateTime.UtcNow;
                timeCard.Hours = objWizard.WOLabors[i].Hours;
                TimecardList.Add(timeCard);
            }
            wo.TimecardList = TimecardList;

            wo.CompleteWorkOrderWizard(this.userData.DatabaseKey, userData.Site.TimeZone);
            CreateEventLog(wo.WorkOrderId, WorkOrderEvents.Complete, "");
            List<long> wos = new List<long>() { wo.WorkOrderId };

            if (wo.ErrorMessages.Count == 0)
            {
                //comments Alert
                if (!string.IsNullOrEmpty(objWizard.CommentUserIds))
                {
                    var CommentUserIdList = objWizard.CommentUserIds.Split(',').ToList();
                    if (CommentUserIdList.Count > 0)
                    {
                        CommentAlertWOCompletionWizard(WorkOrderId, wo.CompleteComments, wo.ClientLookupId, CommentUserIdList, 0, 0);
                    }
                }


                Task task1 = Task.Factory.StartNew(() => SendAlert(wos));
                return wo;

            }
            return wo;
        }
        public WorkOrderUDF WorkorderUDFDynamicCompletion
            (WorkOrderCompletionInformationModelDynamic modelDynamic, List<UIConfigurationDetailsForModelValidation> configDetails,
            long WorkOrderId)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            WorkOrderUDF workorderUDF = new WorkOrderUDF()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                WorkOrderId = WorkOrderId
            };
            workorderUDF = workorderUDF.RetriveWorkOrderUDFByWorkOrderId(_dbKey);

            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == true && x.Section == false && x.ViewOnly == false);
            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, modelDynamic);
                getpropertyInfo = modelDynamic.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(modelDynamic);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                setpropertyInfo = workorderUDF.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(workorderUDF, val);
            }
            if (workorderUDF.WorkOrderId == 0)
            {
                workorderUDF.ClientId = userData.DatabaseKey.Client.ClientId;
                workorderUDF.WorkOrderId = WorkOrderId;
            }

            return workorderUDF;
        }

        #endregion
        #region Comment alert for WorkOrder Completion Wizard V2-634
        public void CommentAlertWOCompletionWizard(long workOrderId, string content, string woClientLookupId, List<string> userList, long noteId = 0, long updatedindex = 0)
        {
            CommonWrapper coWrapper = new CommonWrapper(userData);
            var namelist = coWrapper.MentionList("");
            var UserList = new List<Tuple<long, string, string>>();
            if (userList != null && userList.Count > 0)
            {
                foreach (var item in userList)
                {

                    long userId = namelist.Where(x => x.UserName == item).Select(y => y.PersonnelId).FirstOrDefault();
                    string userName = item;
                    string emailId = namelist.Where(x => x.UserName == item).Select(y => y.Email).FirstOrDefault();
                    UserList.Add
                    (
                      Tuple.Create(userId, userName, emailId)
                    );
                }
                Notes notes = new Notes()
                {
                    OwnerId = userData.DatabaseKey.User.UserInfoId,
                    OwnerName = userData.DatabaseKey.User.FullName,
                    Content = content,
                    ObjectId = workOrderId,
                    UpdateIndex = updatedindex,
                    NotesId = noteId
                };
                List<long> nos = new List<long>() { workOrderId };
                ProcessAlert objAlert = new ProcessAlert(this.userData);
                objAlert.CreateAlert<WorkOrder>(AlertTypeEnum.WorkOrderCommentMention, nos, notes, UserList);

            }

        }
        #endregion

        #region V2-663
        public void SendClientProgressBarCurrentStatus(int TotalCount, int CurrentPrintingcount, string PrintingCountConnectionID)
        {
            ProcessAlert objAlert = new ProcessAlert(this.userData);
            objAlert.SendClientProgressBarPrintingCurrentStatus(TotalCount, CurrentPrintingcount, PrintingCountConnectionID);
        }

        public WorkOrder RetrieveAllByWorkOrdeV2Print(List<long> WorkOrderIDList = null)
        {
            WorkOrder w = new WorkOrder();
            w.ClientId = userData.DatabaseKey.Client.ClientId;
            w.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            w.WorkOrderIDList = WorkOrderIDList != null && WorkOrderIDList.Count > 0 ? string.Join(",", WorkOrderIDList) : string.Empty;
            w.RetrieveAllByWorkOrdeV2Print(userData.DatabaseKey, userData.Site.TimeZone);

            return w;
        }
        #endregion

        #region V2-690 Add/Edit Estimate Part Not In Inventory
        public EstimatedCosts AddEstimatePartNotInInventory(WorkOrderVM WoVM)
        {
            string part_ClientLookupId = WoVM.estimatePart.ClientLookupId;
            Part pt = new Part { ClientId = _dbKey.Client.ClientId, SiteId = _dbKey.User.DefaultSiteId, ClientLookupId = part_ClientLookupId };
            pt.RetrieveByClientLookUpIdNUPCCode(_dbKey);
            //V2-726 Start
            ApprovalGroupSettings approvalGroupSettings = new ApprovalGroupSettings
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.Site.SiteId
            };
            var approvalGroupSettingsList = approvalGroupSettings.RetrieveApprovalGroupSettings_V2(userData.DatabaseKey).FirstOrDefault();
            //V2-726 End

            EstimatedCosts estimatecost = new EstimatedCosts()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ObjectId = WoVM.estimatePart.WorkOrderId,
                ObjectType = AttachmentTableConstant.WorkOrder,
                Category = "Parts",
                CategoryId = 0,
                Description = WoVM.estimatePart.Description,
                Duration = 0,
                Quantity = WoVM.estimatePart.Quantity ?? 0,
                UnitCost = WoVM.estimatePart.UnitCost ?? 0,
                UnitOfMeasure = WoVM.estimatePart.Unit,
                AccountId = WoVM.estimatePart?.AccountId ?? 0,
                UNSPSC = WoVM.estimatePart?.PartCategoryMasterId ?? 0,
                Source = "Internal",
                VendorId = WoVM.estimatePart?.VendorId ?? 0,
                UpdateIndex = 0,
            };

            //V2-726 Start
            if (approvalGroupSettingsList != null)
            {
                if (approvalGroupSettingsList.MaterialRequests == true)
                {
                    estimatecost.Status = MaterialRequestLineStatus.Initiated;
                }
                else
                {
                    estimatecost.Status = MaterialRequestLineStatus.Approved;
                }
            }
            else
            {
                estimatecost.Status = "";
            }
            //V2-726 End
            estimatecost.Create(this.userData.DatabaseKey);
            return estimatecost;
        }
        #endregion
        #region V2-690 Edit Estimate Part In Inventory
        public PartNotInInventoryModel GetLineItem(long EstimatedCostsId, long WorkOrderId)
        {
            PartNotInInventoryModel objPartNotInInventoryModel = new PartNotInInventoryModel();

            EstimatedCosts estimatedCosts = new EstimatedCosts()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ObjectId = WorkOrderId,
                ObjectType = AttachmentTableConstant.WorkOrder
            };
            List<EstimatedCosts> EstmatedCostsItems = EstimatedCosts.EstimatedCostsRetrieveByObjectId_ForChild(this.userData.DatabaseKey, estimatedCosts);

            var selectedEstmatedCostsItems = EstmatedCostsItems != null ? EstmatedCostsItems.Where(x => x.EstimatedCostsId == EstimatedCostsId).SingleOrDefault() : null;
            objPartNotInInventoryModel.EstimatedCostsId = selectedEstmatedCostsItems.EstimatedCostsId;
            objPartNotInInventoryModel.ClientId = selectedEstmatedCostsItems.ClientId;
            objPartNotInInventoryModel.Quantity = selectedEstmatedCostsItems.Quantity;
            objPartNotInInventoryModel.ObjectId = selectedEstmatedCostsItems.ObjectId;
            objPartNotInInventoryModel.Description = selectedEstmatedCostsItems.Description;
            objPartNotInInventoryModel.CategoryId = selectedEstmatedCostsItems.CategoryId;
            objPartNotInInventoryModel.PartClientLookupId = selectedEstmatedCostsItems.PartClientLookupId;
            // V2- 1148 Check if the ShoppingCart feature is enabled for the site
            if (userData.Site.ShoppingCart)
            {
                // If ShoppingCart is enabled, set the UnitCost from the selected estimated costs items
                objPartNotInInventoryModel.UnitCost = selectedEstmatedCostsItems?.UnitCost ?? 00;
            }
            else
            {
                // If ShoppingCart is not enabled, set the UnitCostStockPart from the selected estimated costs items
                objPartNotInInventoryModel.UnitCostStockPart = selectedEstmatedCostsItems?.UnitCost ?? 00;
            }
            objPartNotInInventoryModel.Unit = selectedEstmatedCostsItems.Unit;
            objPartNotInInventoryModel.AccountId = selectedEstmatedCostsItems.AccountId;
            objPartNotInInventoryModel.AccountClientLookupId = selectedEstmatedCostsItems.AccountClientLookupId;
            objPartNotInInventoryModel.VendorId = selectedEstmatedCostsItems.VendorId;
            objPartNotInInventoryModel.VendorClientLookupId = selectedEstmatedCostsItems.VendorClientLookupId;
            objPartNotInInventoryModel.PartCategoryMasterId = selectedEstmatedCostsItems.UNSPSC;
            objPartNotInInventoryModel.PartCategoryClientLookupId = selectedEstmatedCostsItems.PartCategoryClientLookupId;
            return objPartNotInInventoryModel;
        }

        public EstimatedCosts EditPartInInventory(PartNotInInventoryModel mspModel)
        {
            EstimatedCosts estimatedCosts = new EstimatedCosts()
            {
                EstimatedCostsId = mspModel.EstimatedCostsId,
            };
            estimatedCosts.Retrieve(this.userData.DatabaseKey);
            estimatedCosts.Quantity =mspModel.Quantity ?? 0;
            estimatedCosts.UnitOfMeasure = !string.IsNullOrEmpty(mspModel.Unit) ? mspModel.Unit : "";
            estimatedCosts.AccountId =mspModel?.AccountId ?? 0;
            estimatedCosts.AccountClientLookupId =mspModel.AccountClientLookupId;
            estimatedCosts.VendorId =mspModel?.VendorId ?? 0;
            estimatedCosts.VendorClientLookupId =mspModel.VendorClientLookupId;

            if(userData.Site.ShoppingCart==false)
            {
                estimatedCosts.UNSPSC = 0;
                estimatedCosts.PartClientLookupId = null;
                estimatedCosts.UnitCost = mspModel.UnitCostStockPart ?? 0;
            }
            else {
                estimatedCosts.UnitCost = mspModel.UnitCost ?? 0;
            }
            estimatedCosts.Update(this.userData.DatabaseKey);
            if (estimatedCosts.ErrorMessages != null && estimatedCosts.ErrorMessages.Count > 0)
            {
                return estimatedCosts;

            }
            return estimatedCosts;
        }
        #endregion
        #region V2-695 WorkOrder Downtime v2
        public List<WorkOrderDowntimeModel> GetWorkOrderDowntime(long WorkOrderId, int skip = 0, int length = 10, string orderbycol = "1", string orderDir = "desc")
        {
            List<WorkOrderDowntimeModel> DownTimeModelList = new List<WorkOrderDowntimeModel>();
            WorkOrderDowntimeModel objDownTimeModel;
            Downtime downtime = new Downtime()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                WorkOrderId = WorkOrderId
            };
            //**
            downtime.OrderbyColumn = orderbycol;
            downtime.OrderBy = orderDir;
            downtime.offset1 = skip;
            downtime.nextrow = length;
            //**
            List<Downtime> workOrderIdDowntimeList = Downtime.RetriveByWorkOrderId_V2(this.userData.DatabaseKey, downtime);
            if (workOrderIdDowntimeList != null)
            {
                var workOrderList = workOrderIdDowntimeList.Select(x => new { x.WorkOrderId, x.WorkOrderClientLookupId, x.DateDown, x.MinutesDown, x.DowntimeId, x.ReasonForDownDescription, x.TotalCount, x.TotalMinutesDown }).ToList();
                foreach (var v in workOrderList)
                {
                    objDownTimeModel = new WorkOrderDowntimeModel();
                    objDownTimeModel.Downdate = Convert.ToDateTime(v.DateDown);
                    objDownTimeModel.MinutesDown = v.MinutesDown;
                    objDownTimeModel.WorkOrderClientLookupId = v.WorkOrderClientLookupId;
                    objDownTimeModel.DowntimeId = v.DowntimeId;
                    objDownTimeModel.DowntimeCreateSecurity = userData.Security.WorkOrder_Downtime.Create;
                    objDownTimeModel.DowntimeEditSecurity = userData.Security.WorkOrder_Downtime.Edit;
                    objDownTimeModel.DowntimeDeleteSecurity = userData.Security.WorkOrder_Downtime.Delete;
                    objDownTimeModel.ReasonForDownDescription = v.ReasonForDownDescription;
                    objDownTimeModel.TotalCount = v.TotalCount;
                    objDownTimeModel.TotalMinutesDown = v.TotalMinutesDown;
                    DownTimeModelList.Add(objDownTimeModel);
                }
            }
            return DownTimeModelList;
        }

        public Downtime AddDownTime(WorkOrderDowntimeModel wodowntime)

        {
            Downtime downtime = new Downtime()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                EquipmentId = wodowntime.ChargeToId ?? 0,
                ParentSiteId = this.userData.DatabaseKey.User.DefaultSiteId,
                Operator_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId,
                WorkOrderId = wodowntime.WorkOrderId,
                ReasonForDown = wodowntime.ReasonForDown,
                MinutesDown = wodowntime.MinutesDown ?? 0,
                DateDown = wodowntime.Downdate.Value

            };
            var validateMinutesDown = wodowntime.MinutesDown ?? 0;
            if (validateMinutesDown > 0)
            {
                downtime.Create(this.userData.DatabaseKey);
            }
            return downtime;
        }

        public WorkOrderDowntimeModel RetrieveByDowntimeId(long DowntimeId)
        {
            WorkOrderDowntimeModel objWoDowntimeModel = new WorkOrderDowntimeModel();
            Downtime downtime = new Downtime()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                DowntimeId = DowntimeId
            };

            downtime.Retrieve(this.userData.DatabaseKey);
            objWoDowntimeModel = PopulateDowntimeModel(downtime);
            return objWoDowntimeModel;
        }
        internal WorkOrderDowntimeModel PopulateDowntimeModel(Downtime aobj)
        {
            WorkOrderDowntimeModel oModel = new WorkOrderDowntimeModel();
            oModel.DowntimeId = aobj.DowntimeId;
            if (aobj.DateDown != null && aobj.DateDown != default(DateTime))
            {
                oModel.Downdate = aobj.DateDown;
            }
            else
            {
                oModel.Downdate = null;
            }
            oModel.MinutesDown = aobj.MinutesDown;
            oModel.ReasonForDown = aobj.ReasonForDown;
            return oModel;
        }

        public Downtime EditDownTime(WorkOrderDowntimeModel wodowntime)
        {
            Downtime downtime = new Downtime()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                EquipmentId = wodowntime.ChargeToId ?? 0,
                DowntimeId = wodowntime.DowntimeId
            };

            downtime.Retrieve(userData.DatabaseKey);

            downtime.ParentSiteId = this.userData.DatabaseKey.User.DefaultSiteId;

            downtime.DateDown = wodowntime.Downdate;
            downtime.MinutesDown = wodowntime.MinutesDown ?? 0;
            downtime.ReasonForDown = wodowntime.ReasonForDown;
            downtime.Update(this.userData.DatabaseKey);
            return downtime;
        }

        public bool DeleteDowntime(long DowntimeId)
        {
            Downtime downtime = new Downtime()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                DowntimeId = DowntimeId
            };
            try
            {
                downtime.Delete(this.userData.DatabaseKey);
                return true;
            }
            catch
            {
                return false;
            }

        }

        #endregion

        #region V2-726

        public EstimatedCosts SendForApproval(ApprovalRouteModel arModel)
        {
            EstimatedCosts ec = new EstimatedCosts()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ObjectId = arModel.ObjectId,
                ObjectType = AttachmentTableConstant.WorkOrder
            };

            List<EstimatedCosts> estimatedCostsList = EstimatedCosts.EstimatedCostsRetrieveByObjectId_ForChild(this.userData.DatabaseKey, ec);

            estimatedCostsList = estimatedCostsList.Where(x => x.Status == MaterialRequestLineStatus.Initiated).ToList();
            if (estimatedCostsList != null && estimatedCostsList.Count > 0)
            {
                foreach (var item in estimatedCostsList)
                {
                    item.Retrieve(_dbKey);
                    item.Status = MaterialRequestLineStatus.Route;
                    item.Update(userData.DatabaseKey);
                    if (item.ErrorMessages != null && item.ErrorMessages.Count > 0)
                    {
                        ec.ErrorMessages = item.ErrorMessages;
                        break;
                    }

                    Task t1 = Task.Factory.StartNew(() => CreateEventLog(arModel.ApproverId, item.EstimatedCostsId, arModel.Comments, arModel.ApprovalGroupId, arModel.RequestType));
                }
                if (ec.ErrorMessages == null || ec.ErrorMessages.Count == 0)
                {
                    List<long> objectId = new List<long>() { arModel.ObjectId };
                    var UserList = new List<Tuple<long, string, string>>();
                    CommonWrapper coWrapper = new CommonWrapper(userData);
                    var PersonnelInfo = coWrapper.GetPersonnelDetailsByPersonnelId(arModel.ApproverId);
                    if (PersonnelInfo != null)
                    {
                        UserList.Add
                        (
                            Tuple.Create(Convert.ToInt64(PersonnelInfo.PersonnelId), PersonnelInfo.UserName, PersonnelInfo.Email)
                        );
                        ProcessAlert objAlert = new ProcessAlert(this.userData);
                        Task t2 = Task.Factory.StartNew(() => objAlert.CreateAlert<WorkOrder>(AlertTypeEnum.MaterialRequestApprovalNeeded, objectId, UserList));
                    }
                }
            }
            return ec;
        }

        private void CreateEventLog(Int64 ApproverId, Int64 ObjectId, string comment, long ApprovalGroupId, string RequestType)
        {
            ApprovalRoute log = new ApprovalRoute();
            log.ClientId = userData.DatabaseKey.Client.ClientId;
            log.ApproverId = ApproverId;
            log.ObjectId = ObjectId;
            log.ApprovalGroupId = ApprovalGroupId;
            log.RequestType = RequestType;
            log.Comments = comment;
            log.IsProcessed = false;
            log.ProcessResponse = String.Empty;
            log.Create(userData.DatabaseKey);
        }
        public bool RetrieveApprovalGroupMaterialRequestStatus(string RequestType)
        {
            bool IsRequestType = false;
            ApprovalGroupSettings approvalGroupSettings = new ApprovalGroupSettings
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.Site.SiteId
            };
            var approvalGroupSettingsList = approvalGroupSettings.RetrieveApprovalGroupSettings_V2(userData.DatabaseKey).FirstOrDefault();
            if (approvalGroupSettingsList != null)
            {
                if (RequestType == "WorkRequests")
                {
                    if (approvalGroupSettingsList.WorkRequests == true)
                    {
                        IsRequestType = true;
                    }
                }
                else if (RequestType == "MaterialRequests")
                {
                    if (approvalGroupSettingsList.MaterialRequests == true)
                    {
                        IsRequestType = true;
                    }
                }
            }
            return IsRequestType;
        }

        public WorkOrder ApproveWR(ApprovalRouteModel arModel)
        {
            WorkOrder workOrder = new WorkOrder()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                WorkOrderId = arModel.ObjectId,
                SiteId = this.userData.DatabaseKey.Personnel.SiteId
            };
            workOrder.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);
            if (workOrder.Status == WorkOrderStatusConstants.WorkRequest)
            {
                workOrder.Status = WorkOrderStatusConstants.AwaitingApproval;
                workOrder.Update(userData.DatabaseKey);

                if (workOrder.ErrorMessages == null || workOrder.ErrorMessages.Count == 0)
                {
                    Task t1 = Task.Factory.StartNew(() => CreateEventLog(arModel.ApproverId, arModel.ObjectId, arModel.Comments, arModel.ApprovalGroupId, arModel.RequestType));
                    List<long> workorderid = new List<long>() { workOrder.WorkOrderId };
                    var UserList = new List<Tuple<long, string, string>>();
                    CommonWrapper coWrapper = new CommonWrapper(userData);
                    var PersonnelInfo = coWrapper.GetPersonnelDetailsByPersonnelId(arModel.ApproverId);
                    if (PersonnelInfo != null)
                    {
                        UserList.Add
                         (
                                   Tuple.Create(Convert.ToInt64(PersonnelInfo.PersonnelId), PersonnelInfo.UserName, PersonnelInfo.Email)
                        );
                        ProcessAlert objAlert = new ProcessAlert(this.userData);
                        Task t2 = Task.Factory.StartNew(() => objAlert.CreateAlert<DataContracts.WorkOrder>(AlertTypeEnum.WRApprovalRouting, workorderid, UserList));
                    }
                }
            }
            return workOrder;
        }
        #endregion

        #region V2-753
        public WorkOrderCompletionInformationModelDynamic RetrieveWorkOrderCompletionInformationByWorkOrderId(long workOrderId)
        {
            WorkOrderCompletionInformationModelDynamic woCompletionInformation = new WorkOrderCompletionInformationModelDynamic();
            WorkOrder workorder = new WorkOrder()
            {
                ClientId = _dbKey.Client.ClientId,
                WorkOrderId = workOrderId
            };
            workorder.RetrieveByPKForeignKeys(this.userData.DatabaseKey, userData.Site.TimeZone);

            woCompletionInformation = MapWorkOrderData(woCompletionInformation, workorder);
            return woCompletionInformation;
        }

        public WorkOrderCompletionInformationModelDynamic MapWorkOrderData(WorkOrderCompletionInformationModelDynamic woCompletionInformation, WorkOrder workorder)
        {
            woCompletionInformation.WorkOrderId = workorder.WorkOrderId;
            woCompletionInformation.ClientLookupId = workorder.ClientLookupId;
            if (workorder.CompleteDate != null && workorder.CompleteDate != default(DateTime))
            {
                woCompletionInformation.CompleteDate = workorder.CompleteDate;
            }
            else
            {
                woCompletionInformation.CompleteDate = null;
            }
            woCompletionInformation.ActualDuration = workorder.ActualDuration;
            woCompletionInformation.ActionCode = workorder.ActionCode;
            if (workorder.ChargeToId == 0)
            {
                woCompletionInformation.ChargeToId = null;
                woCompletionInformation.ChargeTo_Name = "";
            }
            else
            {
                woCompletionInformation.ChargeToId = workorder.ChargeToId;
                woCompletionInformation.ChargeTo_Name = workorder.ChargeTo_Name;
            }
            woCompletionInformation.CompleteAllTasks = workorder.CompleteAllTasks;
            if (workorder.ActualFinishDate != null && workorder.ActualFinishDate != default(DateTime))
            {
                woCompletionInformation.ActualFinishDate = workorder.ActualFinishDate;
            }
            else
            {
                woCompletionInformation.ActualFinishDate = null;
            }
            woCompletionInformation.Description = workorder.Description;
            woCompletionInformation.DownRequired = workorder.DownRequired;
            woCompletionInformation.FailureCode = workorder.FailureCode;
            woCompletionInformation.Location = workorder.Location;
            if (workorder.Labor_AccountId == 0)
            {
                woCompletionInformation.Labor_AccountId = null;
            }
            else
            {
                woCompletionInformation.Labor_AccountId = workorder.Labor_AccountId;
            }
            woCompletionInformation.Priority = workorder.Priority;
            woCompletionInformation.ScheduledDuration = workorder.ScheduledDuration;
            if (workorder.ProjectId == 0)
            {
                woCompletionInformation.ProjectId = null;
            }
            else
            {
                woCompletionInformation.ProjectId = workorder.ProjectId;
            }
            if (workorder.RequiredDate != null && workorder.RequiredDate != default(DateTime))
            {
                woCompletionInformation.RequiredDate = workorder.RequiredDate;
            }
            else
            {
                woCompletionInformation.RequiredDate = null;
            }
            woCompletionInformation.ReasonNotDone = workorder.ReasonNotDone;
            if (workorder.ScheduledStartDate != null && workorder.ScheduledStartDate != default(DateTime))
            {
                woCompletionInformation.ScheduledStartDate = workorder.ScheduledStartDate;
            }
            else
            {
                woCompletionInformation.ScheduledStartDate = null;
            }
            if (workorder.SourceId == 0)
            {
                woCompletionInformation.SourceId = null;
            }
            else
            {
                woCompletionInformation.SourceId = workorder.SourceId;
            }
            woCompletionInformation.SourceType = workorder.SourceType;
            woCompletionInformation.Status = workorder.Status;
            woCompletionInformation.Type = workorder.Type;
            if (workorder.PartsOnOrder == 0)
            {
                woCompletionInformation.PartsOnOrder = null;
            }
            else
            {
                woCompletionInformation.PartsOnOrder = workorder.PartsOnOrder;
            }
            woCompletionInformation.RequestorName = workorder.RequestorName;
            woCompletionInformation.RequestorPhoneNumber = workorder.RequestorPhoneNumber;
            woCompletionInformation.RequestorEmail = workorder.RequestorEmail;
            if (workorder.CreateDate != null && workorder.CreateDate != default(DateTime))
            {
                woCompletionInformation.CreateDate = workorder.CreateDate;
            }
            else
            {
                woCompletionInformation.CreateDate = null;
            }
            woCompletionInformation.ReasonforDown = workorder.ReasonforDown;

            woCompletionInformation.RootCauseCode = workorder.RootCauseCode;

            return woCompletionInformation;
        }
        #endregion

        #region V2-730
        public List<ApprovalRouteModelByObjectId> RetrieveApprovalGroupIdbyObjectId(long ApproverId, long ObjectId, string RequestType)
        {
            ApprovalRouteModelByObjectId approvalRouteModel;
            List<ApprovalRouteModelByObjectId> approvalRouteModelList = new List<ApprovalRouteModelByObjectId>();

            ApprovalRoute approvalRoute = new ApprovalRoute()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ObjectId = ObjectId,
                RequestType = RequestType,
                ApproverId = ApproverId
            };
            List<ApprovalRoute> list = ApprovalRoute.ApprovalRoute_RetrievebyObjectId_V2(this.userData.DatabaseKey, approvalRoute);
            foreach (var ec in list)
            {
                approvalRouteModel = new ApprovalRouteModelByObjectId();
                approvalRouteModel.ApprovalGroupId = ec.ApprovalGroupId;
                approvalRouteModel.ApprovalRouteId = ec.ApprovalRouteId;

                approvalRouteModelList.Add(approvalRouteModel);
            }

            return approvalRouteModelList;
        }
        public WorkOrder MultiLevelApproveWR(ApprovalRouteModel arModel)
        {
            WorkOrder workOrder = new WorkOrder()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                WorkOrderId = arModel.ObjectId,
                SiteId = this.userData.DatabaseKey.Personnel.SiteId
            };
            workOrder.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);

            workOrder.Status = WorkOrderStatusConstants.AwaitingApproval;
            workOrder.Update(userData.DatabaseKey);

            ApprovalRoute AR = new ApprovalRoute();
            AR.ClientId = this.userData.DatabaseKey.Client.ClientId;
            AR.ApprovalGroupId = arModel.ApprovalGroupId;
            AR.ObjectId = arModel.ObjectId;
            AR.ProcessResponse = WorkOrderStatusConstants.Approved;
            AR.ApproverId = this.userData.DatabaseKey.Personnel.PersonnelId;
            AR.UpdateByObjectId_V2(userData.DatabaseKey);

            if (AR.ErrorMessages == null || AR.ErrorMessages.Count == 0)
            {
                Task t1 = Task.Factory.StartNew(() => CreateEventLog(arModel.ApproverId, arModel.ObjectId, arModel.Comments, arModel.ApprovalGroupId, arModel.RequestType));
                List<long> workorderid = new List<long>() { workOrder.WorkOrderId };
                var UserList = new List<Tuple<long, string, string>>();
                CommonWrapper coWrapper = new CommonWrapper(userData);
                var PersonnelInfo = coWrapper.GetPersonnelDetailsByPersonnelId(arModel.ApproverId);
                if (PersonnelInfo != null)
                {
                    UserList.Add
                     (
                               Tuple.Create(Convert.ToInt64(PersonnelInfo.PersonnelId), PersonnelInfo.UserName, PersonnelInfo.Email)
                    );
                    ProcessAlert objAlert = new ProcessAlert(this.userData);
                    Task t2 = Task.Factory.StartNew(() => objAlert.CreateAlert<DataContracts.WorkOrder>(AlertTypeEnum.WRApprovalRouting, workorderid, UserList));
                }
            }
            return workOrder;
        }
        public WorkOrder MultiLevelFinalApproveWR(long workorderId, long ApprovalGroupId)
        {
            WorkOrder workOrder = new WorkOrder()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                WorkOrderId = workorderId,
                SiteId = this.userData.DatabaseKey.Personnel.SiteId
            };
            workOrder.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);
            if (workOrder.Status == WorkOrderStatusConstants.AwaitingApproval)
            {
                // Approve the work order
                workOrder.Status = WorkOrderStatusConstants.Approved;
                workOrder.ApproveDate = DateTime.UtcNow;
                workOrder.ApproveBy_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
                workOrder.Update(userData.DatabaseKey);

                ApprovalRoute AR = new ApprovalRoute();
                AR.ClientId = this.userData.DatabaseKey.Client.ClientId;
                AR.ApprovalGroupId = ApprovalGroupId;
                AR.ObjectId = workorderId;
                AR.ProcessResponse = WorkOrderStatusConstants.Approved;
                AR.ApproverId = this.userData.DatabaseKey.Personnel.PersonnelId;
                AR.UpdateByObjectId_V2(userData.DatabaseKey);

                if (AR.ErrorMessages == null || AR.ErrorMessages.Count == 0)
                {
                    List<long> listWO = new List<long>();
                    listWO.Add(workOrder.WorkOrderId);
                    ProcessAlert objAlert = new ProcessAlert(this.userData);
                    Task t2 = Task.Factory.StartNew(() => objAlert.CreateAlert<WorkOrder>(AlertTypeEnum.WorkRequestApproved, listWO));
                }
            }
            return workOrder;

        }

        public WorkOrder MultiLevelDenyWO(long workorderId, long ApprovalGroupId, ref string Statusmsg)
        {
            WorkOrder workOrder = new WorkOrder()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                WorkOrderId = workorderId,
                SiteId = this.userData.DatabaseKey.Personnel.SiteId
            };
            workOrder.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);
            string strWOStatus = workOrder.Status;
            workOrder.Status = WorkOrderStatusConstants.Denied;
            workOrder.Update(userData.DatabaseKey);
            ApprovalRoute AR = new ApprovalRoute();
            AR.ClientId = this.userData.DatabaseKey.Client.ClientId;
            AR.ApprovalGroupId = ApprovalGroupId;
            AR.ObjectId = workorderId;
            AR.ProcessResponse = WorkOrderStatusConstants.Denied;
            AR.ApproverId = this.userData.DatabaseKey.Personnel.PersonnelId;
            AR.UpdateByObjectId_V2(userData.DatabaseKey);
            if (workOrder.ErrorMessages == null || workOrder.ErrorMessages.Count == 0)
            {
                ProcessAlert objAlert = new ProcessAlert(this.userData);
                List<long> listWO = new List<long>();
                listWO.Add(workOrder.WorkOrderId);
                objAlert.CreateAlert<WorkOrder>(AlertTypeEnum.WorkRequestDenied, listWO);
                Statusmsg = "success";
            }
            else
            {
                Statusmsg = "error";
            }
            return workOrder;
        }
        #endregion

        #region V2-949
        public AttachmentModel EditWoAttachment(long WorkOrderId, long FileAttachmentId)
        {
            AttachmentModel objAttachmentModel = new AttachmentModel();
            Attachment attachment = new Attachment()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                AttachmentId = FileAttachmentId,
            };
            attachment.Retrieve(userData.DatabaseKey);
            objAttachmentModel.AttachmentId = attachment.AttachmentId;
            objAttachmentModel.Subject = attachment.Description;
            objAttachmentModel.PrintwithForm = attachment.PrintwithForm;
            objAttachmentModel.WorkOrderId = WorkOrderId;
            #region V2-1006
            objAttachmentModel.FileType = attachment.FileType;
            #endregion
            return objAttachmentModel;
        }

        #endregion

        #region V2-1056 CMMS – Add Sanitation Request
        public SanitationRequest AddSanitationRequestWorkOrder(AddSanitationRequestModel obj, ref List<string> ErrorMsgList)
        {
            SanitationRequest SanitationRequest = new SanitationRequest
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId
            };
            string newClientlookupId = "";

            if (obj.ClientLookupId == null && SanitationJobConstant.SanitaionJob_AutoGenerateEnabled)
            {
                newClientlookupId = CustomSequentialId.GetNextId(userData.DatabaseKey, AutoGenerateKey.SANIT_ANNUAL, userData.DatabaseKey.User.DefaultSiteId, "");
            }

            SanitationRequest.SiteId = userData.DatabaseKey.Personnel.SiteId;
            SanitationRequest.ChargeType = obj?.ChargeType ?? string.Empty;
            SanitationRequest.ChargeToClientLookupId = obj?.ChargeToClientLookupId ?? string.Empty;
            SanitationRequest.Description = obj?.Description ?? string.Empty;
            SanitationRequest.ClientLookupId = newClientlookupId;
            SanitationRequest.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            SanitationRequest.Creator_PersonnelClientLookupId = userData.DatabaseKey.Personnel.ClientLookupId.ToString();
            SanitationRequest.Creator_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            SanitationRequest.Status = SanitationJobConstant.JobRequest;
            SanitationRequest.SourceType = SanitationJobSourceType.WorkOrder;
            SanitationRequest.SourceId = obj.WorkOrderId;


            SanitationRequest.Add_SanitationRequest(this.userData.DatabaseKey);

            CreateSanitationEventLog(SanitationRequest.SanitationJobId, SanitationEvents.Create, obj.WorkOrderId);
            CreateSanitationEventLog(SanitationRequest.SanitationJobId, SanitationEvents.JobRequest, obj.WorkOrderId);

            return SanitationRequest;
        }

        private void CreateSanitationEventLog(Int64 sanId, string Status, Int64 sourceId)
        {
            SanitationEventLog log = new SanitationEventLog();
            log.ClientId = userData.DatabaseKey.Client.ClientId;
            log.SiteId = userData.DatabaseKey.Personnel.SiteId;
            log.ObjectId = sanId;
            log.Event = Status;
            log.TransactionDate = DateTime.UtcNow;
            log.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            log.Comments = "";
            log.SourceId = sourceId;
            log.Create(userData.DatabaseKey);
        }
        #endregion

        #region V2-1051
        public WorkOrder CreateWorkOrderModel(long workOrderId)
        {
            WorkOrder wo = new WorkOrder()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.Personnel.SiteId,
                WorkOrderId = workOrderId
            };

            if (WorkOrderStatusConstants.Wo_AutoGenerateEnabled)
            {
                newClientlookupId = CustomSequentialId.GetNextId(userData.DatabaseKey, AutoGenerateKey.WO_Annual, userData.DatabaseKey.User.DefaultSiteId, "");
            }

            wo.ClientLookupId = newClientlookupId;
            wo.Creator_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
            wo.CreateWOModel(this.userData.DatabaseKey);
            return wo;
        }
        #endregion

        #region V2-1067 Unplanned WO Dynamic
        public WorkOrder WO_DescribeDynamic(WorkOrderVM objVM, ref List<string> ErrorMsgList)
        {
            WoDescriptionModelDynamic WoEmergencyModel = objVM.woDescriptionModelDynamic;
            PropertyInfo getpropertyInfo, setpropertyInfo;
            WorkOrder workOrder = new WorkOrder
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId
            };

            if (WorkOrderStatusConstants.Wo_AutoGenerateEnabled)
            {
                newClientlookupId = CustomSequentialId.GetNextId(userData.DatabaseKey, AutoGenerateKey.WO_Annual, userData.DatabaseKey.User.DefaultSiteId, "");
            }

            if (WoEmergencyModel.IsTypeShow)
            {
                workOrder.Type = WoEmergencyModel.Type;
            }
            workOrder.ChargeType = ChargeType.Equipment;

            if (WoEmergencyModel.ChargeToClientLookupId != null)
            {
                var index = WoEmergencyModel.ChargeToClientLookupId.IndexOf('|');
                if (index != -1)
                {
                    workOrder.ChargeToClientLookupId = WoEmergencyModel.ChargeToClientLookupId.Substring(0, index).Trim();
                    long ChrgTo = 0;
                    long.TryParse(workOrder.ChargeToClientLookupId, out ChrgTo);
                    workOrder.ChargeToId = ChrgTo;
                }
                else
                {
                    workOrder.ChargeToClientLookupId = WoEmergencyModel.ChargeToClientLookupId;
                    long ChrgTo = 0;
                    long.TryParse(workOrder.ChargeToClientLookupId, out ChrgTo);
                    workOrder.ChargeToId = ChrgTo;
                }
            }
            if (userData.Site.SourceAssetAccount == true && workOrder.Labor_AccountId == 0)
            {
                Equipment equipment = new Equipment
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    SiteId = userData.Site.SiteId,
                    ClientLookupId = workOrder.ChargeToClientLookupId
                };
                equipment.RetrieveByClientLookupId(this.userData.DatabaseKey);
                workOrder.Labor_AccountId = equipment.Labor_AccountId;
            }
            if (WoEmergencyModel.IsDescriptionShow)
            {
                workOrder.Description = WoEmergencyModel.Description.Trim();
            }
            workOrder.ClientLookupId = newClientlookupId;
            workOrder.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            workOrder.ClientId = userData.DatabaseKey.Client.ClientId;
            workOrder.CompleteBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            workOrder.SourceType = WorkOrderSourceTypes.Emergency;
           
            workOrder.Status = WorkOrderStatusConstants.Scheduled;
            workOrder.ScheduledStartDate = DateTime.UtcNow;
            workOrder.Scheduler_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
            workOrder.WorkAssigned_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            workOrder.ApproveDate = DateTime.UtcNow;
            workOrder.EmergencyWorkOrder = true;
            workOrder.ApproveBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            workOrder.Creator_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
        
            List<UIConfigurationDetailsForModelValidation> configDetails = new RetrieveDataForUIConfiguration().Retrieve(DataDictionaryViewNameConstant.WorkOrderDescribeAdd, userData);
            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == false && x.Section == false);

            foreach (var item in ColumnDetails)
            {
                getpropertyInfo = objVM.woDescriptionModelDynamic.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(objVM.woDescriptionModelDynamic);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }

                setpropertyInfo = workOrder.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(workOrder, val);
            }

            workOrder.CreateByPKForeignKeys(this.userData.DatabaseKey, userData.Site.TimeZone);
            if (workOrder.ErrorMessages.Count == 0)
            {
                #region V2-1077
                ProcessAlert objAlert = new ProcessAlert(this.userData);
                List<long> listWO = new List<long>();
                listWO.Add(workOrder.WorkOrderId);
                objAlert.CreateAlert<WorkOrder>(AlertTypeEnum.WorkOrderPlanner, listWO);
                #endregion
                // V2-1123

                // RKL - 2024-Nov-11 - begin
                // Not Writing Schedule Records if there are no udfs displayed
                // Not Writing Event Log Entries if there are no udf displayed
                if (configDetails.Any(x => x.Display == true && x.UDF == true && x.Section == false))
                {
                    IEnumerable<string> errors = AddWorkorderDescribeUDFDynamic(objVM.woDescriptionModelDynamic, workOrder.WorkOrderId, configDetails);
                    if (errors != null && errors.Count() > 0)
                    {
                        ErrorMsgList = workOrder.ErrorMessages;
                        return workOrder;
                    }
                }
                // Create Event Log entries - Create, Approved, Scheduled
                CreateEventLog(workOrder.WorkOrderId, WorkOrderEvents.Create);
                CreateEventLog(workOrder.WorkOrderId, WorkOrderEvents.Approved);
                CreateEventLog(workOrder.WorkOrderId, WorkOrderEvents.Scheduled);
                // Create the WorkOrderSchedule Record
                WorkOrderSchedule workorderschedule = new WorkOrderSchedule()
                {
                    ClientId = this.userData.DatabaseKey.Client.ClientId,
                    WorkOrderId = workOrder.WorkOrderId
                };
                workorderschedule.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
                workorderschedule.ScheduledStartDate = DateTime.UtcNow;
                workorderschedule.ScheduledHours = 1;
                workorderschedule.CreateForWorkOrder(this.userData.DatabaseKey);
                // V2-1123

                // RKL - 2024-Nov-11 - end
            }
            return workOrder;
            
        }

        public List<string> AddWorkorderDescribeUDFDynamic(WoDescriptionModelDynamic workorder, long WorkOrderId,
            List<UIConfigurationDetailsForModelValidation> configDetails)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            WorkOrderUDF workOrderUDF = new WorkOrderUDF();
            workOrderUDF.ClientId = userData.DatabaseKey.Client.ClientId;
            workOrderUDF.WorkOrderId = WorkOrderId;

            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == true && x.Section == false);
            foreach (var item in ColumnDetails)
            {
                getpropertyInfo = workorder.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(workorder);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                setpropertyInfo = workOrderUDF.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(workOrderUDF, val);
            }

            workOrderUDF.Create(_dbKey);
            return workOrderUDF.ErrorMessages;
        }

        #endregion
        #region V2-1135
        public List<Project> RetrieveWorkoderProjectLookupList()
        {
            Project project = new Project()
            {
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                ClientId = _dbKey.Client.ClientId,
            };
            return project.RetrieveWorkoderProjectLookupList_V2(this.userData.DatabaseKey); ;
        }
        #endregion

        #region V2-1176 GueastWorkRequest

        public WorkOrder AddGuestWorkRequest(GuestWorkRequestModel WoRequestModel, ref List<string> errorMsg)
        {
            WorkOrder obj = new WorkOrder();
            WorkOrder workorder = new WorkOrder
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId
            };
            if (WorkOrderStatusConstants.Wo_AutoGenerateEnabled)
            {
                newClientlookupId = CustomSequentialId.GetNextId(userData.DatabaseKey, AutoGenerateKey.WO_Annual, userData.DatabaseKey.User.DefaultSiteId, "");
            }
            workorder.ClientLookupId = newClientlookupId;
            workorder.SiteId = userData.DatabaseKey.Personnel.SiteId;
            workorder.Creator_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            workorder.Creator_PersonnelClientLookupId = userData.DatabaseKey.Personnel.ClientLookupId.ToString();
            workorder.Description = WoRequestModel.Description;
            workorder.Status = WorkOrderStatusConstants.WorkRequest;
            workorder.Type = WoRequestModel.Type;
            workorder.RequestorName = WoRequestModel.RequestorName;
            workorder.RequestorPhoneNumber = WoRequestModel.RequestorPhoneNumber;
            workorder.RequestorEmail = WoRequestModel.RequestorEmail;
            workorder.CreateByPKForeignKeys(this.userData.DatabaseKey, userData.Site.TimeZone);
            if (workorder.ErrorMessages.Count == 0)
            {
                ProcessAlert objAlert = new ProcessAlert(this.userData);
                List<long> wos = new List<long>() { workorder.WorkOrderId };
                Task CreateAlertTask = Task.Factory.StartNew(() => objAlert.CreateAlert<DataContracts.WorkOrder>(AlertTypeEnum.WorkRequestApprovalNeeded, wos));
                Task CreateEventTask1 = Task.Factory.StartNew(() => CreateEventLog(workorder.WorkOrderId, WorkOrderEventLogFunction.Create));
                Task CreateEventTask2 = Task.Factory.StartNew(() => CreateEventLog(workorder.WorkOrderId, WorkOrderEventLogFunction.WorkRequest));
            }
            else
            {
                errorMsg = workorder.ErrorMessages;
            }
            return workorder;

        }
        #endregion
        #region V2-1177
        public AnalyticsWorkOrderStatusVM AnalyticsWOStatus(int CustomQueryDisplayId)
        {
            WorkOrder workorder = new WorkOrder();
            AnalyticsWorkOrderStatusVM workOrderVM;
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> Type = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> PMType = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> Priority = new List<DataContracts.LookupList>();
            workorder.ClientId = userData.DatabaseKey.Client.ClientId;
            workorder.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            workorder.CustomQueryDisplayId = CustomQueryDisplayId;

            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();

            Task<WorkOrder> task1 = Task.Factory.StartNew<WorkOrder>(
                           () => workorder.AnalyticsWOStatusDashboardV2(this.userData.DatabaseKey, userData.Site.TimeZone));
            Task task2 = Task.Factory.StartNew(() => AllLookUps = commonWrapper.GetAllLookUpList());
            Task.WaitAll(task1, task2);

            if (AllLookUps != null)
            {
                Type = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_TYPE).ToList();
                PMType = AllLookUps.Where(x => x.ListName == LookupListConstants.Preventive_Maint_WO_Type).ToList();
                Priority = AllLookUps.Where(x => x.ListName == LookupListConstants.WO_Priority).ToList();
            }


            List<WorkOrder> woList = workorder.listOfWO;
            var CreatedCount = woList.Count();
            var ApprovedCount = woList.Where(m => m.Status == WorkOrderStatusConstants.Approved).Count();
            var ScheduledCount = woList.Where(m => m.Status == WorkOrderStatusConstants.Scheduled).Count();
            var CompleteCount = woList.Where(m => m.Status == WorkOrderStatusConstants.Complete).Count();
            var CanceledCount = woList.Where(m => m.Status == WorkOrderStatusConstants.Canceled).Count();
            var PlanningCount = woList.Where(m => m.Status == WorkOrderStatusConstants.Planning).Count();
            workOrderVM = new AnalyticsWorkOrderStatusVM();            
            List<WOStatusModel> wOStatusModelList = new List<WOStatusModel>
            {
                new WOStatusModel
                {
                    Status = "Created",
                    StatusCount = CreatedCount
                },
                new WOStatusModel
                {
                    Status = WorkOrderStatusConstants.Approved,
                    StatusCount = ApprovedCount
                },
                new WOStatusModel
                {
                    Status = WorkOrderStatusConstants.Scheduled,
                    StatusCount = ScheduledCount
                },
                new WOStatusModel
                {
                    Status = WorkOrderStatusConstants.Complete,
                    StatusCount = CompleteCount
                },
                new WOStatusModel
                {
                    Status = WorkOrderStatusConstants.Canceled,
                    StatusCount = CanceledCount
                },
                new WOStatusModel
                {
                    Status = WorkOrderStatusConstants.Planning,
                    StatusCount = PlanningCount
                }
            };
            workOrderVM.WOStatusModel = wOStatusModelList;
            List<WOModel> workOrderModelList = new List<WOModel>();
            WOModel workOrderModel;
            foreach (var wo in woList)
            {
                workOrderModel = new WOModel();
                workOrderModel.WorkOrderId = wo.WorkOrderId;
                workOrderModel.ClientLookupId = wo.ClientLookupId;
                workOrderModel.Status = wo.Status;
                workOrderModel.Priority = wo.Priority != "" ? wo.Priority : "No Value";
                if (Priority != null && Priority.Any(cus => cus.ListValue == wo.Priority))
                {
                    workOrderModel.Priority = Priority.Where(x => x.ListValue == wo.Priority).Select(x => x.Description).First();
                }
                else
                {
                    workOrderModel.Priority = "No Value";
                }
                if (wo.CreateDate != null && wo.CreateDate == default(DateTime))
                {
                    workOrderModel.CreateDate = null;
                }
                else
                {
                    workOrderModel.CreateDate = wo.CreateDate;
                }
                if (wo.CompleteDate != null && wo.CompleteDate == default(DateTime))
                {
                    workOrderModel.CompleteDate = null;
                }
                else
                {
                    workOrderModel.CompleteDate = wo.CompleteDate;
                }
                workOrderModel.SourceType = wo.SourceType != "" ? wo.SourceType : "No Value";
                if (Type != null && Type.Any(cus => cus.ListValue == wo.Type))
                {
                    workOrderModel.Type = Type.Where(x => x.ListValue == wo.Type).Select(x => x.Description).First();
                }
                else if (PMType != null && PMType.Any(cus => cus.ListValue == wo.Type))
                {
                    workOrderModel.Type = PMType.Where(x => x.ListValue == wo.Type).Select(x => x.Description).First();
                }
                else
                {
                    workOrderModel.Type = "No Value";
                }
                if (wo.SourceType == WorkOrderSourceTypes.Emergency || wo.SourceType == WorkOrderSourceTypes.OnDemand)
                {
                    Type = AllLookUps.Where(x => x.ListName == LookupListConstants.UP_WO_TYPE).ToList();
                }
                else if (wo.SourceType == WorkOrderSourceTypes.WorkRequest)
                {
                    Type = AllLookUps.Where(x => x.ListName == LookupListConstants.WR_WO_TYPE).ToList();
                }
                if (wo.Status == WorkOrderStatusConstants.Complete)
                {
                    workOrderModel.CompleteStatus = "Complete";
                }
                else if (wo.Status == WorkOrderStatusConstants.Canceled || wo.Status == WorkOrderStatusConstants.Approved || wo.Status == WorkOrderStatusConstants.Planning || wo.Status == WorkOrderStatusConstants.Scheduled)
                {
                    workOrderModel.CompleteStatus = "Incomplete";
                }
                else
                {
                    workOrderModel.CompleteStatus = "No Value";
                }
                if(wo.SourceType == WorkOrderSourceTypes.PreventiveMaint)
                {
                    if (wo.Status == WorkOrderStatusConstants.Complete)
                    {
                        workOrderModel.PrevMaintCompleteStatus = "Complete";
                    }
                    else if (wo.Status == WorkOrderStatusConstants.Canceled || wo.Status == WorkOrderStatusConstants.Approved || wo.Status == WorkOrderStatusConstants.Planning || wo.Status == WorkOrderStatusConstants.Scheduled)
                    {
                        workOrderModel.PrevMaintCompleteStatus = "Incomplete";
                    }
                    else
                    {
                        workOrderModel.PrevMaintCompleteStatus = "No Value";
                    }
                }
                else
                {
                    workOrderModel.PrevMaintCompleteStatus = "No Value";
                }
                workOrderModel.MaterialCosts = wo.ActualMaterialCosts;
                workOrderModel.LaborHours = wo.ActualLaborHours;
                workOrderModel.LaborCosts = wo.LaborCost;
                workOrderModel.TotalCosts = wo.TotalCost;
                workOrderModelList.Add(workOrderModel);
            }
            workOrderVM.WOModel = workOrderModelList;
            return workOrderVM;
        }
        #endregion
    }
}


