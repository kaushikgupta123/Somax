using DataContracts;
using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Client.Models.Common;
using Client.Models.PreventiveMaintenance.UIConfiguration;
using Client.Models.Work_Order;

namespace Client.Models.PreventiveMaintenance
{
    public class PrevMaintVM : LocalisationBaseVM
    {
        public PrevMaintVM()
        {
            scheduleRecordsList = new List<ScheduleRecords>();
            tableHaederProps = new List<TableHaederProp>();
            #region V2-950
            AddPMSRecordsModelDynamic_Calendar = new AddPMSRecordsModelDynamic_Calendar();
            EditPMSRecordsModelDynamic_Calendar = new EditPMSRecordsModelDynamic_Calendar();
            AddPMSRecordsModelDynamic_Meter = new AddPMSRecordsModelDynamic_Meter();
            EditPMSRecordsModelDynamic_Meter = new EditPMSRecordsModelDynamic_Meter();
            AddPMSRecordsModelDynamic_OnDemand = new AddPMSRecordsModelDynamic_OnDemand();
            EditPMSRecordsModelDynamic_OnDemand = new EditPMSRecordsModelDynamic_OnDemand();
            #endregion
        }
        public PreventiveMaintenanceModel preventiveMaintenanceModel { get; set; }
        public NotesModel notesModel { get; set; }
        public AttachmentModel attachmentModel { get; set; }
        public PrevMaintTaskModel prevMaintTaskModel { get; set; }
        public ScheduleRecords scheduleRecords { get; set; }
        public EstimatedCostModel estimatedCostModel { get; set; }
        public EstimateLabor estimateLabor { get; set; }
        public EstimatePart estimatePart { get; set; }
        public EstimateOtherModel estimateOtherModel { get; set; }

        public UserData userData { get; set; }

        public PrevMaintReassignModel prevMaintReassignModel { get; set; }

        public CreatedLastUpdatedPMModel createdLastUpdatedPMModel { get; set; }
        public PreventiveMaintenanceOptionModel prevMaintOptionModel { get; set; }
        public bool IsFromEquipment { get; set; }

        public PMForcastModel pMForcastModel { get; set; }
        public PreventiveMaitenanceWOModel preventiveMaitenanceWOModel { get; set; }
        public ProcesLogModel procesLogModel { get; set; }
        public bool IsPMLibrary { get; set; }

        public PreventiveMaintenanceLibraryModel preventiveMaintenanceLibraryModel { get; set; }
        public int attachmentCount { get; set; }
        public ChangePreventiveIDModel _ChangePreventiveIDModel { get; set; }

        public List<ScheduleRecords> scheduleRecordsList { get; set; }
        public List<PMPDFPrintModel> pMPDFPrintModel { get; set; }
        public List<TableHaederProp> tableHaederProps { get; set; }

        public InstructionModel InstructionPMModel { get; set; }

        public PMGenerateWorkOrdersModel pMGenerateWorkOrdersModel { get; set; }
        public Security security { get; set; }
        #region V2-919
        public string EquipmentClientLookupId { get; set; }
        public long EquipmentId { get; set; }
        #endregion
        #region V2-950
        public UIConfiguration.AddPMSRecordsModelDynamic_Calendar AddPMSRecordsModelDynamic_Calendar { get; set; }
        public UIConfiguration.EditPMSRecordsModelDynamic_Calendar EditPMSRecordsModelDynamic_Calendar { get; set; }
        public UIConfiguration.AddPMSRecordsModelDynamic_Meter AddPMSRecordsModelDynamic_Meter { get; set; }
        public UIConfiguration.EditPMSRecordsModelDynamic_Meter EditPMSRecordsModelDynamic_Meter { get; set; }
        public UIConfiguration.AddPMSRecordsModelDynamic_OnDemand AddPMSRecordsModelDynamic_OnDemand { get; set; }
        public UIConfiguration.EditPMSRecordsModelDynamic_OnDemand EditPMSRecordsModelDynamic_OnDemand { get; set; }
        public List<Client.Common.UIConfigurationDetailsForModelValidation> UIConfigurationDetails { get; set; }
        public List<PMSAddUILookupList> AllRequiredLookUplist { get; set; }
        public IEnumerable<SelectListItem> ScheduleMethodList { get; set; }
        public IEnumerable<SelectListItem> FrequencyTypeList { get; set; }
        //public IEnumerable<SelectListItem> PlannerPersonnelList { get; set; }
        //public IEnumerable<SelectListItem> FailureCodeList { get; set; }
        //public IEnumerable<SelectListItem> ActionCodeList { get; set; }
        //public IEnumerable<SelectListItem> RootCauseCodeList { get; set; }
        public IEnumerable<SelectListItem> DaysOfWeekList { get; set; }
        //public IEnumerable<SelectListItem> ScheduledPriorityList { get; set; }
        //public IEnumerable<SelectListItem> ScheduledShiftList { get; set; }
        public IEnumerable<SelectListItem> OnDemandList { get; set; }
        public string PMSScheduleType { get; set; }
        #endregion
        #region V2-712
        public List<InnerGridPMSchedAssignModel> innerGridPMSchedAssignModel { get; set; }
        public PMSchedAssignModel pMSchedAssignModel { get; set; }
        #endregion
        #region V2-1151
        public bool IsUseMultiStoreroom { get; set; }
        public IEnumerable<SelectListItem> StoreroomList { get; set; }
        public MaterialRequestMultiStoreroomModel MaterialRequestMultiStoreroomModel { get; set; }
        public IEnumerable<SelectListItem> UnitOfmesureListWo { get; set; }
        public EditMaterialRequestModel PartNotInInventoryModel { get; set; }
        #endregion
        public IEnumerable<SelectListItem> TypeList { get; set; } //V2-1207
    }
    public class PMSAddUILookupList
    {
        public string text { get; set; }
        public string value { get; set; }
        public string lookupname { get; set; }
        public string BusinessType { get; set; }

        public IEnumerable<SelectListItem> DateRangeDropList { get; set; }
        public IEnumerable<SelectListItem> DateRangeDropListForPMS { get; set; }

        public IEnumerable<SelectListItem> DateRangeDropListForPMSCreatedate { get; set; }
    }
}