using Client.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataContracts;
using static Client.Models.Common.UserMentionDataModel;
namespace Client.Models.WorkOrderPlanning
{
    public class WorkOrderPlanningVM : LocalisationBaseVM
    {
        public WorkOrderPlanningVM()
        {
            PersonnelList = new List<SelectListItem>();
            WorkOrderList = new List<SelectListItem>();
        }

        public WorkOrderPlanningModel workorderPlanningModel { get; set; }
        public List<WOPlanLineItemModel> woPlanLineitemModel { get; set; }
        public IEnumerable<SelectListItem> PersonnelList { get; set; }
        public IEnumerable<SelectListItem> WorkOrderList { get; set; }        
        public IEnumerable<SelectListItem> WorkOrderPlanViewList { get; set; }
        public List<WorkOrderPlanPDFPrintModel> workOrderPlanPDFPrintModel { get; set; }
        

        public List<TableHaederProp> tableHaederProps { get; set; }
        #region ResourceList
        public List<ResourceListPdfPrintModel> resourceListPdfPrintModel { get; set; }
        
        public IEnumerable<SelectListItem> ScheduledGroupingList { get; set; }
        public WoRescheduleModel woRescheduleModel { get; set; }
        public AvailableWorkAssignModel availableWorkAssignRLModel { get; set; }

        public ResourceListSearchModel resourceListSearchModel { get; set; }
        #endregion
        #region Plan
        public WorkOrderPlanSummaryModel workorderPlanSummaryModel { get; set; }
        public List<WorkOrderWOPlanLookupListModel> workorderWOPlanLookupListModelList { get; set; }
        public List<WorkOrderForWorkOrderPlanModel> workOrderForWorkOrderPlanModel { get; set; }
        public List<EventLogModel> EventLogList { get; set; }
        public List<Notes> NotesList { get; set; }
        public List<UserMentionData> userMentionData { get; set; }
        public WOPScheduleModel WOPScheduleModel { get; set; }
        public UserData _userdata { get; set; }
        #endregion



        public ResourceCalendarAddScheduleModel ResourceCalendarAddScheduleModel { get; set; }
        public ResourceCalendarEditScheduleModel ResourceCalendarEditScheduleModel { get; set; }
    }
}