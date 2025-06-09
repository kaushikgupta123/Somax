using DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Client.Models.WorkOrderPlanning;
using System.Globalization;
using Client.BusinessWrapper.Common;
using DataContracts;
using Client.Models.Common;
using Common.Enumerations;
using Client.Models;
using Common.Extensions;
using Client.Common.Constants;

namespace Client.BusinessWrapper.WorkOrderPlanning
{
    public class WorkOrderPlanningDetailsPlanWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        public WorkOrderPlanningDetailsPlanWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }
        #region get WorkOderPlanDetails By WorkOrderPlanId
        public WorkOrderPlanSummaryModel getWorkOderPlanDetailsByWorkOrderPlanId(long workOrderPlanId)
        {
            WorkOrderPlanSummaryModel WorkOrderPlanDetails = new WorkOrderPlanSummaryModel();
            WorkOrderPlan workorderplan = new WorkOrderPlan()
            {
                ClientId = _dbKey.Client.ClientId,
                WorkOrderPlanId = workOrderPlanId
            };
            var records = workorderplan.RetrieveListForRetrieveByWorkOrderPlanId(this.userData.DatabaseKey, userData.Site.TimeZone);
            WorkOrderPlanDetails.WorkOrderPlanId = records.WorkOrderPlanId;
            WorkOrderPlanDetails.Description = records.Description;
            WorkOrderPlanDetails.EndDate = records.EndDate != null && records.EndDate != default(DateTime) ? Convert.ToDateTime(records.EndDate).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : "";
            WorkOrderPlanDetails.StartDate = records.StartDate != null && records.StartDate != default(DateTime) ? Convert.ToDateTime(records.StartDate).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : "";
            WorkOrderPlanDetails.CompleteDate = records.CompleteDate != null && records.CompleteDate != default(DateTime) ? Convert.ToDateTime(records.CompleteDate).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : "";
            WorkOrderPlanDetails.LockPlan = records.LockPlan;
            WorkOrderPlanDetails.Status = records.Status;
            WorkOrderPlanDetails.PlannerName = records.PersonneNameFirst + " " + records.PersonneNameLast;
            return WorkOrderPlanDetails;
        }
        #endregion
        #region Get WorkOrder WOPlanLookupList Grid Data
        public List<WorkOrderWOPlanLookupListModel> GetWorkOrderWOPlanLookupListGridData( string orderbycol = "", string orderDir = "", int skip = 0, int length = 0, string clientLookupId = "", string ChargeTo = "", string ChargeTo_Name = "", string Description = "", string Status = "", string Priority = "",string Type="", string RequiredDate = "")
        {

            WorkOrderWOPlanLookupListModel workorderWOPlanLookupListModel;
            List<WorkOrderWOPlanLookupListModel> workorderWOPlanLookupListModelList = new List<WorkOrderWOPlanLookupListModel>();
            List<WorkOrderPlan> WorkOrderWOPlanLookupList = new List<WorkOrderPlan>();
            WorkOrderPlan WOPlanLookupList = new WorkOrderPlan();
            WOPlanLookupList.ClientId = this.userData.DatabaseKey.Client.ClientId;
            WOPlanLookupList.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            WOPlanLookupList.OrderbyColumn = orderbycol;
            WOPlanLookupList.OrderBy = orderDir;
            WOPlanLookupList.OffSetVal = skip;
            WOPlanLookupList.NextRow = length;
            WOPlanLookupList.WorkOrderClientLookupId = clientLookupId;
            WOPlanLookupList.ChargeTo = ChargeTo;
            WOPlanLookupList.ChargeTo_Name = ChargeTo_Name;
            WOPlanLookupList.Description = Description;
            WOPlanLookupList.Status = Status;
            WOPlanLookupList.Priority = Priority;
            WOPlanLookupList.RequireDate = RequiredDate;
            WOPlanLookupList.Type = Type;
            WorkOrderWOPlanLookupList = WOPlanLookupList.RetrieveWorkOrder_WorkOrderPlanLookupListBySearchCriteria(userData.DatabaseKey);
            foreach (var item in WorkOrderWOPlanLookupList)
            {
                workorderWOPlanLookupListModel = new WorkOrderWOPlanLookupListModel();
                workorderWOPlanLookupListModel.WorkOrderId = item.WorkOrderId;
                workorderWOPlanLookupListModel.ChargeTo= item.ChargeTo;
                workorderWOPlanLookupListModel.ChargeTo_Name = item.ChargeTo_Name;
                workorderWOPlanLookupListModel.Description = item.Description;
                workorderWOPlanLookupListModel.ClientLookupId = item.WorkOrderClientLookupId;
                workorderWOPlanLookupListModel.Status = item.Status;
                workorderWOPlanLookupListModel.Priority = item.Priority;
                workorderWOPlanLookupListModel.Type = item.Type;
                workorderWOPlanLookupListModel.TotalCount = item.TotalCount;
                if (item.RequiredDate != null && item.RequiredDate == default(DateTime))
                {
                    workorderWOPlanLookupListModel.RequiredDate = null;
                }
                else
                {
                    workorderWOPlanLookupListModel.RequiredDate = item.RequiredDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                workorderWOPlanLookupListModelList.Add(workorderWOPlanLookupListModel);
            }

            return workorderWOPlanLookupListModelList;
        }
        #endregion
        #region Work order grid data for work order plan
        public List<WorkOrderForWorkOrderPlanModel> GetWorkOrderGridDataForWorkOrderPlan(string orderbycol = "", string orderDir = "", int skip = 0, int length = 0, long workOrderPlanId = 0, string equipmentClientLookupId = "", string chargeToName = "", string description = "", string requireDate = "", string type = "", string searchText = "")
        {
            List<WorkOrderForWorkOrderPlanModel> workOrderForWorkOrderPlanList = new List<WorkOrderForWorkOrderPlanModel>();
            WorkOrderPlan workOrderPlan = new WorkOrderPlan();
            workOrderPlan.ClientId = this.userData.DatabaseKey.Client.ClientId;
            workOrderPlan.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            workOrderPlan.OrderbyColumn = orderbycol;
            workOrderPlan.OrderBy = orderDir;
            workOrderPlan.OffSetVal = skip;
            workOrderPlan.NextRow = length;
            workOrderPlan.WorkOrderPlanId = workOrderPlanId;
            workOrderPlan.EquipmentClientLookupId = equipmentClientLookupId;
            workOrderPlan.ChargeTo_Name = chargeToName;
            workOrderPlan.Description = description;
            workOrderPlan.RequireDate = requireDate;
            workOrderPlan.Type = type;
            workOrderPlan.SearchText = searchText;
            var workOrderGridData = workOrderPlan.RetrieveWorkOrderForWorkOrderPlanChunkSearch(userData.DatabaseKey);

            foreach (var item in workOrderGridData)
            {
                WorkOrderForWorkOrderPlanModel workOrderForWorkOrderPlanModel = new WorkOrderForWorkOrderPlanModel();
                workOrderForWorkOrderPlanModel.WOPlanLineItemId = item.WOPlanLineItemId;
                workOrderForWorkOrderPlanModel.WOPlanLineItemType = item.WOPlanLineItemType;
                workOrderForWorkOrderPlanModel.WorkOrderId = item.WorkOrderId;
                workOrderForWorkOrderPlanModel.WorkOrderPlanId = item.WorkOrderPlanId;
                workOrderForWorkOrderPlanModel.WorkOrderClientLookupId = item.WorkOrderClientLookupId;
                workOrderForWorkOrderPlanModel.Description = item.Description;
                workOrderForWorkOrderPlanModel.Type = item.Type;
                if (item.RequiredDate != null && item.RequiredDate == default(DateTime))
                {
                    workOrderForWorkOrderPlanModel.RequiredDate = null;
                }
                else
                {
                    workOrderForWorkOrderPlanModel.RequiredDate = item.RequiredDate;
                }
                workOrderForWorkOrderPlanModel.EquipmentClientLookupId = item.EquipmentClientLookupId;
                workOrderForWorkOrderPlanModel.ChargeTo_Name = item.ChargeTo_Name;
                workOrderForWorkOrderPlanModel.TotalCount = item.TotalCount;
                workOrderForWorkOrderPlanList.Add(workOrderForWorkOrderPlanModel);
            }

            return workOrderForWorkOrderPlanList;
        }
        #endregion

        #region Add WOPlan LineItem
        public List<string> AddWOPlanLineItem(string [] WOPlanLineItems,long WorkOrderPlanID,string Type)
        {
            var errorList = new List<string>();
            if (WOPlanLineItems.Length>0)
            {
                for(int i=0;i< WOPlanLineItems.Length;i++)
                {
                    if(!string.IsNullOrEmpty(WOPlanLineItems[i]))
                    {
                        WOPlanLineItem woPlanLineItems = new WOPlanLineItem()
                        {
                            ClientId = _dbKey.Client.ClientId,
                            WorkOrderPlanId = WorkOrderPlanID,
                            WorkOrderId = Convert.ToInt64(WOPlanLineItems[i].ToString()),
                            Type = Type
                        };

                        woPlanLineItems.Create(this.userData.DatabaseKey);
                        if(woPlanLineItems.ErrorMessages != null && woPlanLineItems.ErrorMessages.Count > 0)
                        {
                            errorList = woPlanLineItems.ErrorMessages;
                            break;
                        }
                    
                    }
                   
                }
            }
            return errorList;


        }
        #endregion
        #region Remove WOPlan LineItem
        public List<string> RemoveWOPlanLineItem(string[] WOPlanLineItems)
        {
            var errorList = new List<string>();
            if (WOPlanLineItems.Length > 0)
            {
                for (int i = 0; i < WOPlanLineItems.Length; i++)
                {
                    if (!string.IsNullOrEmpty(WOPlanLineItems[i]))
                    {
                        WOPlanLineItem woPlanLineItems = new WOPlanLineItem()
                        {
                            ClientId = _dbKey.Client.ClientId,
                            WOPlanLineItemId = Convert.ToInt64(WOPlanLineItems[i].ToString()),
                        };

                        woPlanLineItems.Delete(this.userData.DatabaseKey);
                        if (woPlanLineItems.ErrorMessages != null && woPlanLineItems.ErrorMessages.Count > 0)
                        {
                            errorList = woPlanLineItems.ErrorMessages;
                            break;
                        }

                    }

                }
            }
            return errorList;


        }
        #endregion
        public WorkOrderPlanningModel getWorkOrderPlan_RetrieveByWorkOrderPlanId(long workOrderPlanId)
        {
            WorkOrderPlanningModel WOPModel = new WorkOrderPlanningModel();
            WorkOrderPlan workorderplan = new WorkOrderPlan()
            {
                ClientId = _dbKey.Client.ClientId,
                WorkOrderPlanId = workOrderPlanId
            };
            workorderplan.Retrieve(this.userData.DatabaseKey);
            WOPModel.WorkOrderPlanId = workorderplan.WorkOrderPlanId;
            WOPModel.Description = workorderplan.Description;
            WOPModel.PersonnelId = workorderplan.Assign_PersonnelId;
            if (workorderplan.StartDate != null && workorderplan.StartDate == default(DateTime))
            {
                WOPModel.StartDate = null;
            }
            else
            {
                WOPModel.StartDate = workorderplan.StartDate;
            }
            if (workorderplan.EndDate != null && workorderplan.EndDate == default(DateTime))
            {
                WOPModel.EndDate = null;
            }
            else
            {
                WOPModel.EndDate = workorderplan.EndDate;
            }
            return WOPModel;
        }
        #region Edit Wop Save
        public Tuple<List<string>,long> EditWorkOrderPlan(WorkOrderPlanningVM objWOPVM)
        {
            string emptyValue = string.Empty;
            List<string> errList = new List<string>();
            WorkOrderPlan workorderplan = new WorkOrderPlan()
            {
                ClientId = _dbKey.Client.ClientId,
                WorkOrderPlanId = objWOPVM.workorderPlanningModel.WorkOrderPlanId
            };
            workorderplan.Retrieve(this.userData.DatabaseKey);
            // ------Updating ------
            workorderplan.Description = !string.IsNullOrEmpty(objWOPVM.workorderPlanningModel.Description) ? objWOPVM.workorderPlanningModel.Description.Trim() : emptyValue;
            workorderplan.StartDate = objWOPVM.workorderPlanningModel.StartDate;
            workorderplan.EndDate = objWOPVM.workorderPlanningModel.EndDate;
            workorderplan.Assign_PersonnelId = objWOPVM.workorderPlanningModel.PersonnelId ?? 0;
            workorderplan.Update(this.userData.DatabaseKey);
            // ------Updating ------
            var RetunObj = Tuple.Create(workorderplan.ErrorMessages, workorderplan.WorkOrderPlanId);
            return RetunObj;
        }

        #endregion
        public List<string> WorkOrderPlanStatusUpdating(long WorkOrderPlanId, string Status)
        {
            string emptyValue = string.Empty;
            List<string> errList = new List<string>();
            WorkOrderPlan workorderplan = new WorkOrderPlan()
            {
                ClientId = _dbKey.Client.ClientId,
                WorkOrderPlanId = WorkOrderPlanId
            };
            workorderplan.Retrieve(this.userData.DatabaseKey);
            // ------Updating ------
            if(Status== WorkOrderPlanningConstant.Complete)
            {
                workorderplan.CompleteDate = DateTime.UtcNow;
                workorderplan.CompleteBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            }
            else if (Status == WorkOrderPlanningConstant.Reopen)
            {
                CreateWOPEventLog(WorkOrderPlanId, Status);
                Status = WorkOrderPlanningConstant.Locked;
                workorderplan.CompleteDate = null;
                workorderplan.CompleteBy_PersonnelId = 0;
            }
            else
            {
                bool lockplan = Status == WorkOrderPlanningConstant.Locked ? true : false;
                workorderplan.LockPlan = lockplan;
            }
           
            workorderplan.Status = Status;
            workorderplan.Update(this.userData.DatabaseKey);
            // ------creating new event logs ------
            CreateWOPEventLog(WorkOrderPlanId, Status);
            return errList;
        }

        public List<string> AddScheduleRecord(WOPScheduleModel addScheduleModel,long[] WorkorderItemIds)//--V2-524--//
        {
            var errorList = new List<string>();
            for (int i = 0; i < WorkorderItemIds.Length; i++)
            {
                long WorkOrderId = getWorkOrderIdByWorkorderItemId(WorkorderItemIds[i]);
                errorList = saveScheduledRecord(addScheduleModel, WorkOrderId);
                if (errorList != null && errorList.Count > 0)
                {
                    break;
                }
            }
            return errorList;
        }

        private List<string>  saveScheduledRecord(WOPScheduleModel addScheduleModel, long WorkOrderIds)
        {
            string PersonnelList = String.Empty;
            ProcessAlert objAlert = new ProcessAlert(this.userData);
            WorkOrderPlan w = new WorkOrderPlan()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                WorkOrderId = WorkOrderIds,
                ScheduledHours = 0,
                ScheduledStartDate = addScheduleModel.Schedulestartdate,
            };
            CommonWrapper coWrapper = new CommonWrapper(userData);
            var namelist = coWrapper.MentionList("");
            List<UserMentionDataModel.UserMentionData> userMentionDataList = new List<UserMentionDataModel.UserMentionData>();
            UserMentionDataModel.UserMentionData objUserMentionData;
            List<long> nos = new List<long>() { WorkOrderIds };
            if (addScheduleModel.PersonnelIds != null && addScheduleModel.PersonnelIds.Count > 0)
            {
                foreach (var item in addScheduleModel.PersonnelIds)
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
            if ((addScheduleModel.PersonnelIds != null && addScheduleModel.PersonnelIds.Count > 0) &&
                (addScheduleModel.Schedulestartdate != null && addScheduleModel.Schedulestartdate != default(DateTime)))
            {
                w.IsDeleteFlag = false;
            }
            else
            {
                w.IsDeleteFlag = true;
            }

            w.AddScheduleRecord(userData.DatabaseKey);
            objAlert.CreateAlert<DataContracts.WorkOrder>(AlertTypeEnum.WorkOrderAssign, nos, UserList);
            return w.ErrorMessages;
        }

        public WorkOrderPlan RemoveWorkOrderSchedule(long WorkOrderID, long WorkOrderScheduledID)
        {
            WorkOrderPlan workOrderPlan = new WorkOrderPlan();
            workOrderPlan.ClientId = userData.DatabaseKey.Client.ClientId;
            workOrderPlan.WorkOrderId = WorkOrderID;
            workOrderPlan.WorkOrderScheduleId = WorkOrderScheduledID;
            workOrderPlan.RemoveScheduleRecord(userData.DatabaseKey);
            return workOrderPlan;
        }
        #region V2-646
        public long getWorkOrderIdByWorkorderItemId(long WorkOrderItemId)
        {
            WOPlanLineItem woPlanLineItems = new WOPlanLineItem()
            {
                ClientId = _dbKey.Client.ClientId,
                WOPlanLineItemId = WorkOrderItemId,
            };
            woPlanLineItems.Retrieve(this.userData.DatabaseKey);
            return woPlanLineItems.WorkOrderId;
        }
        #endregion
        public List<WorkOrderSchedule> GetScheduleIDsByworkOrderId(long workOrderId)
        {
            WorkOrderSchedule workorderschdule = new WorkOrderSchedule()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                WorkOrderId = workOrderId
            };
            List<WorkOrderSchedule> woAssgnList = workorderschdule.RetriveByWorkOrderId(userData.DatabaseKey);
            return woAssgnList;
        }

        #region Event Log
        public List<EventLogModel> PopulateEventLog(long WorkOrderPlanId)
        {
            EventLogModel objEventLogModel;
            List<EventLogModel> EventLogModelList = new List<EventLogModel>();

            WorkOrderPlanEventLog log = new WorkOrderPlanEventLog();
            List<WorkOrderPlanEventLog> data = new List<WorkOrderPlanEventLog>();
            log.ClientId = userData.DatabaseKey.Client.ClientId;
            log.SiteId = userData.DatabaseKey.Personnel.SiteId;
            log.WorkOrderPlanId = WorkOrderPlanId;
            data = log.RetriveByWorkOrderPlanId(userData.DatabaseKey);

            if (data != null)
            {
                foreach (var item in data)
                {
                    objEventLogModel = new EventLogModel();
                    objEventLogModel.ClientId = item.ClientId;
                    objEventLogModel.SiteId = item.SiteId;
                    objEventLogModel.EventLogId = item.WorkOrderPlanEventLogId;
                    objEventLogModel.ObjectId = item.WorkOrderPlanId;
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

        #region Common method
        private void CreateWOPEventLog(Int64 WOPId, string Status)
        {
            WorkOrderPlanEventLog log = new WorkOrderPlanEventLog();
            log.ClientId = userData.DatabaseKey.Client.ClientId;
            log.SiteId = userData.DatabaseKey.Personnel.SiteId;
            log.WorkOrderPlanId = WOPId;
            log.TransactionDate = DateTime.UtcNow;
            log.Event = Status;
            log.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            log.Comments = "";
            log.SourceTable = "";
            log.SourceId = 0;
            log.Create(userData.DatabaseKey);
        }
        #endregion
    }
}