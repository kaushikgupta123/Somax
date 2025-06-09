using Client.Models.Common;
using DataContracts;
using System.Collections.Generic;
using System.Web.Mvc;
using static Client.Models.Common.UserMentionDataModel;

namespace Client.Models.FleetService
{
    public class FleetServiceVM : LocalisationBaseVM
    {
        public FleetServiceVM()
        {
            _FleetServiceSummaryModel = new FleetServiceSummaryModel();
            ServiceOrderData = new DataContracts.ServiceOrder();
            EventLogList = new List<EventLogModel>();
            NotesList = new List<Notes>();
            userMentionData = new List<UserMentionData>();
        }
        public Security security { get; set; }
        public UserData _userdata { get; set; }
        public IEnumerable<SelectListItem> LookupFuelTypeList { get; set; }
        public FleetServiceSummaryModel _FleetServiceSummaryModel { get; set; }
        public DataContracts.ServiceOrder ServiceOrderData { get; set; }
        public FleetServiceModel FleetServiceModel { get; set; }
        public FleetServiceLineItemModel FleetServiceLineItemModel { get; set; }
        public List<FleetServiceLineItemModel> FleetServiceLineItemModelList { get; set; }
        public List<FleetServicePDFPrintModel> FleetServicePDFPrintModel { get; set; }
        public List<TableHaederProp> tableHaederProps { get; set; }
        public IEnumerable<SelectListItem> LookupShiftList { get; set; }
        public IEnumerable<SelectListItem> PersonnelList { get; set; }
        public long PersonnelId { get; set; }
        public IEnumerable<SelectListItem> LookupRepairReasonList { get; set; }
        public IEnumerable<SelectListItem> LookupTypeList { get; set; }
        public AttachmentModel attachmentModel { get; set; }
        public int attachmentCount { get; set; }
        public List<EventLogModel> EventLogList { get; set; }
        public List<Notes> NotesList { get; set; }
        public List<UserMentionData> userMentionData { get; set; }
        public List<SelectListItem> ServiceTaskList { get; set; }
        public ServiceOrderScheduleModel soScheduleModel { get; set; }
        public IEnumerable<SelectListItem> AssignedList { get; set; }
        public string CancelReasonSo { get; set; }
        public IEnumerable<SelectListItem> CancelReasonListSo { get; set; }
        public ServiceOrderLabourModel ServiceOrderLabour { get; set; }
        public ServiceOrderLabourTimerModel ServiceOrderLabourTimer { get; set; }
        public IEnumerable<SelectListItem> PersonnelLabourList { get; set; }
        public CompleteServiceOrderModel CompleteServiceOrderModel { get; set; }
        public ServiceOrderIssuePartModel IssuePartModel { get; set; }
        public ServiceOrderOthers ServiceOrderOthers { get; set; }
        public CancelServiceOrderModal CancelServiceOrderModal { get; set; }
        public bool IsRedirectFromAsset { get; set; }
        public long SOId { get; set; }
        //V2-466
        public IEnumerable<SelectListItem> VMRSWorkAccomplishedList { get; set; }
        public IEnumerable<SelectListItem> VMRSFailureList { get; set; }
        public List<HierarchicalList> VMRSSystemList { get; set; }        
    }
}