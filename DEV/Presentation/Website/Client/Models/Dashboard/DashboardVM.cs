using Client.Models.Common;
using DataContracts;
using System.Collections.Generic;
using System.Web.Mvc;
using static Client.Models.Common.UserMentionDataModel;
using Client.Models.Work_Order.UIConfiguration;
using Client.Models.Work_Order;

namespace Client.Models.Dashboard
{
    public class DashboardVM: LocalisationBaseVM
    {
        public DashboardVM()
        {
            DashboardContentModelList = new List<DashboardContentModel>();
        }        
        public string EquipmentDropdown { get; set; }
        public IEnumerable<SelectListItem> EquipmentDownTimeDateList { get; set; }

        public string WoSourceType { get; set; }
        public IEnumerable<SelectListItem> WoSourceTypeDateList { get; set; }

        public string WoByType { get; set; }
        public IEnumerable<SelectListItem> WoByTypeDateList { get; set; }

        public string WoByPriority { get; set; }
        public IEnumerable<SelectListItem> WoByPriorityDateList { get; set; }

        public string WoSource { get; set; }
        public IEnumerable<SelectListItem> WoSourceList { get; set; }

        public string WoLaborHours { get; set; }
        public IEnumerable<SelectListItem> WoLaborHoursList { get; set; }

        public string InvValuationDates { get; set; }
        public IEnumerable<SelectListItem> InvValuationDatesList { get; set; }

        public bool WoSecurity { get; set; }
        public bool PartSecurity { get; set; }

        #region APM
        public string APMDrop { get; set; }
        public IEnumerable<SelectListItem> APMDropList { get; set; }
        #endregion

        #region SanitationOnly
        public string SanitationDrop { get; set; }
        public IEnumerable<SelectListItem> SanitationDropList { get; set; }
        #endregion

        #region V2-375
        public bool UseVendorMaster { get; set; }
        public bool VendorMaster_AllowLocal { get; set; }
        #endregion
        #region Fleet Only
        public string SoLaborHours { get; set; }

        public IEnumerable<SelectListItem> SoLaborHoursList { get; set; }

        #endregion

        #region Enterprise User
        public string EnterpriseUserHours { get; set; }
        public IEnumerable<SelectListItem> EnterpriseUserHoursList { get; set; }
        #endregion

        #region Properties for V2-552
        public string DashboardDrop { get; set; }
        public IEnumerable<SelectListItem> DashboardList { get; set; }
        public List<DashboardContentModel> DashboardContentModelList { get; set; }
        public long DashboardlistingId { get; set; }
        public bool IsAccessMaintenanceDashboard { get; set; }
        public bool IsAccessSanitationDashboard { get; set; }
        public bool IsAccessAPMDashboard { get; set; }
        public bool IsAccessFleetDashboard { get; set; }
        public bool IsAccessEnterprise_MaintenanceDashboard { get; set; }
        public bool IsMultipleDashboardAccess { get; set; }
        public bool IsDefault { get; set; }
        public bool IsMaintenanceTechnicianDashboard { get; set; }
        #endregion

        #region Maintenanace Technician Dashboard

        private string DefaultdateRangeValue = "0";
        public string selectedDateRangeVal
        {
            get => DefaultdateRangeValue;
        }
        public IEnumerable<SelectListItem> DateRangeDropListForActivity { get; set; }

        #endregion

        #region Work Order Maintenance Completion Workbench
        public string CompletionUserHours { get; set; }
        public IEnumerable<SelectListItem> CompletionHoursList { get; set; }
        public WOCompletionWorkbenchSummary woCompletionWorkbenchSummary { get; set; }
        public WoCompletionDetailsHeader woCompletionDetailsHeader { get; set; }
        public InstructionModel InstructionModel { get; set; }
        public LaborModel LaborModel { get; set; }
        public PartIssueAddModel PartIssueAddModel { get; set; }
        public List<string> ErrorMessage { get; set; }
        public IEnumerable<SelectListItem> PersonnelLaborList { get; set; }
        public DashboardWoTaskModel dashboardWoTaskModel { get; set; }
        #endregion

        #region Action function
        public IEnumerable<SelectListItem> CancelReasonListWo { get; set; }
        public string CancelReasonWo { get; set; }
        public Security security { get; set; }
        public UserData _userdata { get; set; }
        public List<Notes> NotesList { get; set; }
        public List<UserMentionData> userMentionData { get; set; }
        public DashboardWoRequestModel WoRequestModel { get; set; }
        public DashboardWorkOrderModel WorkOrderModel { get; set; }
        public DescribeWOModel WoEmergencyDescribeModel { get; set; }
        public OnDamandWOModel WoEmergencyOnDemandModel { get; set; }
        public DashboardAddSanitationRequestModel dashboardAddSanitationRequestModel { get; set; }
        #region V2-652
        public AddWorkRequestModelDynamic AddWorkRequest { get; set; }
        public List<Client.Common.UIConfigurationDetailsForModelValidation> UIConfigurationDetails { get; set; }
        public List<WOAddUILookupList> AllRequiredLookUplist { get; set; }
        public bool IsAddWoRequestDynamic { get; set; }
        public bool IsDepartmentShow { get; set; }
        public bool IsTypeShow { get; set; }
        public bool IsDescriptionShow { get; set; }
        public string ChargeType { get; set; }
        public string BusinessType { get; set; }
        public IEnumerable<SelectListItem> DateRangeDropList { get; set; }
        public IEnumerable<SelectListItem> DateRangeDropListForWO { get; set; }

        public IEnumerable<SelectListItem> DateRangeDropListForWOCreatedate { get; set; }
        public bool IsWorkOrderRequest { get; set; }//V2-1052
        public IEnumerable<SelectListItem> UnitOfmesureListWo { get; set; } //V2-1068
        #region V2-1067
        public WoDescriptionModelDynamic woDescriptionModelDynamic { get; set; }
        public bool IsAddWorkOrderDynamic { get; set; }
        public bool PlantLocation { get; set; }
        #endregion
        #endregion

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

        #region V2-676
        public string WOBarcode { get; set; }
        #endregion

        public IEnumerable<SelectListItem> StoreroomList { get; set; }
        #region V2-690
        public EstimatePart estimatePart { get; set; }
        #endregion
        #region V2-695
        public WoDowntimeModel woDowntimeModel { get; set; }
        #endregion


        #region V2-716
        public List<ImageAttachmentModel> imageAttachmentModels { get; set; }
        #endregion

        #region V2-726
        public ApprovalRouteModel ApprovalRouteModel { get; set; }
        #endregion

        #region V2-732
        public bool IsUseMultiStoreroom { get; set; }//V2-732
        public MaterialRequestMultiStoreroomModel MaterialRequestMultiStoreroomModel { get; set; }//V2-732
        #endregion
    }

}