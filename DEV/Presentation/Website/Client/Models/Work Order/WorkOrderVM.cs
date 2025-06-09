using Client.Models.Common;
using Client.Models.Dashboard;
using Client.Models.WorkOrder;
using Common.Constants;
using DataContracts;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using static Client.Models.Common.UserMentionDataModel;
using Client.Models.Work_Order.UIConfiguration;

namespace Client.Models.Work_Order
{
    public class WorkOrderVM : LocalisationBaseVM
    {
        public WorkOrderVM()
        {
            WoTaskList = new List<WorkOrderTask>();
            WoActualLabor = new List<WoActualLabor>();
            WoPart = new List<PartHistoryModel>();
            workOrderModelList = new List<WorkOrderModel>();
            eventLogList = new List<EventLogModel>();
            NotesList = new List<Notes>();
            userMentionDatas = new List<UserMentionData>();
            #region v2-611 Add Work Order
            UIConfigurationDetails = new List<Client.Common.UIConfigurationDetailsForModelValidation>();
            AllRequiredLookUplist = new List<WOAddUILookupList>();
            AddWorkorder = new UIConfiguration.AddWorkOrderModelDynamic();
            SourceTypeList = new List<SelectListItem>();
            StatusList = new List<SelectListItem>();
            EditWorkOrder = new UIConfiguration.EditWorkOrderModelDynamic();
            workOrderSummaryModel = new WorkOrderSummaryModel();
            #endregion
        }
        public WorkOrderSummaryModel workOrderSummaryModel { get; set; }
        public WorkOrderModel workOrderModel { get; set; }
        public WOInfoModel WOInfoModel { get; set; }
        public EstimateLabor estimateLabor { get; set; }
        public EstimatePart estimatePart { get; set; }
        public EstimateOther estimateOtherModel { get; set; }
        public WoNotesModel notesModel { get; set; }
        public AttachmentModel attachmentModel { get; set; }
        public WoTaskModel woTaskModel { get; set; }
        public WoAssignmentModel woAssignmentModel { get; set; }

        public ActualOther actualOther { get; set; }
        public WoActualLabor woLaborModel { get; set; }
        public WorkOrderPrintModel workOrderPrintModel { get; set; }
        public WoRequestModel woRequestModel { get; set; }
        public AddSanitationRequestModel addSanitationRequestModel { get; set; }
        public bool IsAddRequest { get; set; }
        public bool IsAddWorkOrderFromDashBoard { get; set; }
        public bool IsDetailWorkOrderFromDashBoard { get; set; }
        public bool IsAddWorkOrderFromEquipment { get; set; }

        public IEnumerable<SelectListItem> CancelReasonListWo { get; set; }
        public IEnumerable<SelectListItem> UnitOfmesureListWo { get; set; } //V2-1068
        public IEnumerable<SelectListItem> AccountListWo { get; set; } //V2-1068
        public string CancelReasonWo { get; set; }
        public WoEquipmentTreeModel woEquipmentTreeModel { get; set; }
        public UserData _userdata { get; set; }
        public Security security { get; set; }
        public DowntimeModel downtimeModel { get; set; }
        public WoEmergencyDescribeModel woEmergencyDescribeModel { get; set; }
        public WoEmergencyOnDamandModel woEmergencyOnDemandModel { get; set; }
        public SanitationDescribeWoModel sanitationDescribeWoModel { get; set; }
        public SanitationOnDemandWOModel sanitationOnDemandWOModel { get; set; }
        public bool IsSanitationDescribeAdd { get; set; }
        public List<WorkOrderTask> WoTaskList { get; set; }
        public List<WoActualLabor> WoActualLabor { get; set; }
        public List<PartHistoryModel> WoPart { get; set; }
        public List<ActualOther> WOOthers { get; set; }
        public List<ActualSummery> WOSummary { get; set; }
        public string treeHeader { get; set; }
        public string treeHeaderVal { get; set; }
        public bool IsOnDemandAdd { get; set; }
        public bool IsDescribeAdd { get; set; }
        public WoOnDemandModel woOnDemandModel { get; set; }
        public WoDescriptionModel woDescriptionModel { get; set; }
        public bool IsMaintOnDemand { get; set; }
        public bool IsAddWoRequestDynamic { get; set; }
        public List<AttachmentModel> AttachmentList { get; set; }

        public List<WorkOrderModel> workOrderModelList { get; set; }

        public int attachmentCount { get; set; }
        public List<EventLogModel> eventLogList { get; set; }
        public List<Notes> NotesList { get; set; }
        public RequestOrderModel requestOrderModel { get; set; }


        public long PersonnelId { get; set; }

        public WOPlannerModel wOPlannerModel { get; set; }

        public WoScheduleModel woScheduleModel { get; set; }

        public ApproveWorkOrderModel approveWorkOrderModel { get; set; }
        public WoDenymodel woDenymodel { get; set; }
        public IEnumerable<SelectListItem> AssignedList { get; set; }
        public IEnumerable<SelectListItem> PersonnelList { get; set; }
        public List<UserMentionData> userMentionDatas { get; set; }
        public InstructionModel InstructionModel { get; set; }
        public WoSendForApprovalModel woSendForApprovalModel { get; set; }
        public ActualPart PartIssueAddModel { get; set; }
        public List<string> ErrorMessage { get; set; }
        public IEnumerable<SelectListItem> DownRequiredInactiveFlagList { get; set; }//V2-892
        public UIConfiguration.AddWorkRequestModelDynamic AddWorkRequest { get; set; }

        #region Add WO Description
        public Work_Order.UIConfiguration.WoDescriptionModelDynamic woDescriptionModelDynamic { get; set; }
        public bool IsWoDescDynamic { get; set; }
        public IEnumerable<SelectListItem> ChargeTypelookUpList { get; set; }
        #endregion

        #region V2-611 Add Workorder
        public List<Client.Common.UIConfigurationDetailsForModelValidation> UIConfigurationDetails { get; set; }
        public List<WOAddUILookupList> AllRequiredLookUplist { get; set; }
        public UIConfiguration.AddWorkOrderModelDynamic AddWorkorder { get; set; }
        public bool IsAddWorkOrderDynamic { get; set; }
        public bool PlantLocation { get; set; }
        public IEnumerable<SelectListItem> SourceTypeList { get; set; }
        public IEnumerable<SelectListItem> StatusList { get; set; }
        #endregion
        #region V2-611
        public string BusinessType { get; set; }
        public IEnumerable<SelectListItem> DateRangeDropList { get; set; }
        public IEnumerable<SelectListItem> DateRangeDropListForWO { get; set; }

        public IEnumerable<SelectListItem> DateRangeDropListForWOCreatedate { get; set; }

        public bool IsDepartmentShow { get; set; }
        public bool IsTypeShow { get; set; }
        public bool IsDescriptionShow { get; set; }
        public string ChargeType { get; set; }
        // public string ChargeToClientLookupId { get; set; }
        #endregion

        #region V2-611 Edit Work Order Dynamic
        public UIConfiguration.EditWorkOrderModelDynamic EditWorkOrder { get; set; }
        public bool IsPreventiveMaint { get; set; }
        #endregion

        #region V2-1052
        public bool IsUnplannedWorOrder { get; set; }
        public bool IsWorkOrderRequest { get; set; }
        #endregion


        #region V2-611 View Work Order dynamic
        public ViewWorkOrderModelDynamic ViewWorkOrderModel { get; set; }
        #endregion

        #region V2-634 
        public bool UseWOCompletionWizard { get; set; }
        public bool WOCommentTab { get; set; }
        public bool TimecardTab { get; set; }
        public bool AutoAddTimecard { get; set; }
        public WorkOrderCompletionInformationModelDynamic CompletionModelDynamic { get; set; }
        public WorkOrderCompletionWizard WorkOrderCompletionWizard { get; set; }
        public CompletionLaborWizard CompletionLaborWizard { get; set; }
        #endregion
        #region V2-728
        public bool WOCompletionCriteriaTab { get; set; }
        public string WOCompletionCriteriaTitle { get; set; }
        public string WOCompletionCriteria { get; set; }
        #endregion
        #region V2-676
        public string WOBarcode { get; set; }
        #endregion

        public IEnumerable<SelectListItem> StoreroomList { get; set; } //V2-687
        public PartNotInInventoryModel PartNotInInventoryModel { get; set; } //V2-690
        #region V2-695
        //public DowntimeModel DowntimeModel { get; set; }                                                        
        public List<SelectListItem> ReasonForDownList { get; set; }
       public WorkOrderDowntimeModel wodowntimeModel { get; set; }
        #endregion

        #region V2-716
        public List<ImageAttachmentModel> imageAttachmentModels { get; set; }
        #endregion

        #region V2-726
        public ApprovalRouteModel ApprovalRouteModel { get; set; }
        #endregion
        public MaterialRequestMultiStoreroomModel MaterialRequestMultiStoreroomModel { get; set; }//V2-732

        #region V2-730
        public bool IsWorkRequestApproval { get; set; } //V2-730
        public bool IsWorkRequestApprovalAccessCheck { get; set; } //V2-730
        public ApprovalRouteModelByObjectId ApprovalRouteModelByObjectId { get; set; }
        #endregion

        public IEnumerable<SelectListItem> PersonnelIdList { get; set; }//V2-984
        public IEnumerable<SelectListItem> PlannerList { get; set; }//V2-1078
        public IEnumerable<SelectListItem> ProjectList { get; set; }//V2-1135
        #region V2-1136
        public string WorkOrderAlertName { get; set; } 
        public bool IsDetailFromNotification { get; set; }
        #endregion
    }
    public class WOAddUILookupList
    {
        public string text { get; set; }
        public string value { get; set; }
        public string lookupname { get; set; }
        public string BusinessType { get; set; }

        public IEnumerable<SelectListItem> DateRangeDropList { get; set; }
        public IEnumerable<SelectListItem> DateRangeDropListForWO { get; set; }

        public IEnumerable<SelectListItem> DateRangeDropListForWOCreatedate { get; set; }
       
    }
}