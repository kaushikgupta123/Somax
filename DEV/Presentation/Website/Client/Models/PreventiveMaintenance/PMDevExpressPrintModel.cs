using Client.Models.Common;

using System;
using System.Collections.Generic;

namespace Client.Models.PreventiveMaintenance
{
    public class PMDevExpressPrintModel
    {
        public PMDevExpressPrintModel()
        {
            SchdeduledRecordsList = new List<PMScheduledRecordsDevExpressPrintModel>();
            TasksList = new List<PMTaskDevExpressPrintModel>();
            PartsList = new List<PMPartDevExpressPrintModel>();
            LaborsList = new List<PMLaborDevExpressPrintModel>();
            OthersList = new List<PMOtherDevExpressPrintModel>();
            SummaryList = new List<PMSummaryDevExpressPrintModel>();
            InstructionsList = new List<InstructionModel>();
        }
        public List<PMScheduledRecordsDevExpressPrintModel> SchdeduledRecordsList { get; set; }
        public List<PMTaskDevExpressPrintModel> TasksList { get; set; }
        public List<PMPartDevExpressPrintModel> PartsList { get; set; }
        public List<PMLaborDevExpressPrintModel> LaborsList { get; set; }
        public List<PMOtherDevExpressPrintModel> OthersList { get; set; }
        public List<PMSummaryDevExpressPrintModel> SummaryList { get; set; }
        public List<InstructionModel> InstructionsList { get; set; }
        public string AzureImageUrl { get; set; }
        public string MasterJobId { get; set; }
        public string Type { get; set; }
        public bool Inactive { get; set; }
        public string Description { get; set; }
        public string ScheduleType { get; set; }
        public decimal JobDuration { get; set; }
        public string CurrentDateTime { get; set; }
        public bool OnPremise { get; set; }
        #region Localizations
        public string spnPMMasterJob { get; set; }
        public string spnMasterJobID { get; set; }
        public string spnType { get; set; }
        public string spnInactive { get; set; }
        public string spnDescription { get; set; }
        public string spnScheduleType { get; set; }
        public string spnJobDuration { get; set; }
        public string spnAddInstructions { get; set; }
        #endregion
    }
    public class PMScheduledRecordsDevExpressPrintModel
    {
        public string ChargeType { get; set; }
        public string ChargeToClientLookupId { get; set; }
        //public string ChargeToName { get; set; }
        public string AssignedTo_PersonnelClientLookupId { get; set; }
        //public long PrevMaintMasterId { get; set; }
        //public long PrevMaintLibraryID { get; set; }
        //public long PrevMaintScheId { get; set; }
        //public string OnDemandGroup { get; set; }
        public string NextDueDateString { get; set; }
        public bool Scheduled { get; set; }
        public int? Frequency { get; set; }
        public string FrequencyType { get; set; }


        //public string AssociationGroup { get; set; }        
        //public DateTime? LastPerformed { get; set; }
        //public DateTime? LastScheduled { get; set; }
        //public DateTime? NextDueDate { get; set; }       
        //public string BusinessType { get; set; }
        //public bool ChargeToIdStatus { get; set; }

        #region Localizations
        public string spnScheduleRecords { get; set; }
        public string spnChargeType { get; set; }
        public string spnChargeTo { get; set; }
        public string spnAssignedTo { get; set; }
        public string spnOnDemandGroup { get; set; }
        public string spnPerformEvery { get; set; }
        public string spnNextDue { get; set; }
        public string spnScheduled { get; set; }
        #endregion
    }
    public class PMTaskDevExpressPrintModel
    {
        //public long PrevMaintTaskId { get; set; }
        //public long PrevMaintMasterId { get; set; }
        public string TaskNumber { get; set; }
        public string Description { get; set; }
        public string ChargeType { get; set; }
        public string ChargeToClientLookupId { get; set; }

        #region Localizations
        public string spnTasks { get; set; }
        public string spnOrder { get; set; }
        public string spnTaskDescription { get; set; }
        public string spnTaskChargeType { get; set; }
        public string spnTaskChargeTo { get; set; }
        #endregion
    }
    public class PMPartDevExpressPrintModel
    {
        //public long PrevMaintTaskId { get; set; }
        //public long PrevMaintMasterId { get; set; }
        public string PartClientlookupId { get; set; }
        public string Description { get; set; }
        public decimal? UnitCost { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? TotalCost { get; set; }

        #region Localizations
        public string spnParts { get; set; }
        public string spnPart { get; set; }
        public string spnDescription { get; set; }
        public string spnUnitCost { get; set; }
        public string spnQuantity { get; set; }
        public string spnTotalCost { get; set; }
        #endregion
    }
    public class PMLaborDevExpressPrintModel
    {
        //public long PrevMaintTaskId { get; set; }
        //public long PrevMaintMasterId { get; set; }
        public string Craft { get; set; }
        public string Description { get; set; }
        public decimal? UnitCost { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Duration { get; set; }
        public decimal? TotalCost { get; set; }
        #region Localizations
        public string spnLabor { get; set; }
        public string spnCraft { get; set; }
        public string spnDescription { get; set; }
        public string spnUnitCost { get; set; }
        public string spnQuantity { get; set; }
        public string spnLaborDuration { get; set; }
        public string spnTotalCost { get; set; }
        #endregion
    }
    public class PMOtherDevExpressPrintModel
    {
        public long PrevMaintTaskId { get; set; }
        public long PrevMaintMasterId { get; set; }
        public string Source { get; set; }
        public string Description { get; set; }
        public string VendorClientLookupId { get; set; }
        public decimal? UnitCost { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? TotalCost { get; set; }
        #region Localizations
        public string spnOther { get; set; }
        public string spnSource { get; set; }
        public string spnOtherDescription { get; set; }
        public string spnVendor { get; set; }
        public string spnOtherUnitCost { get; set; }
        public string spnOtherQuantity { get; set; }
        public string spnOtherTotalCost { get; set; }
        #endregion
    }
    public class PMSummaryDevExpressPrintModel
    {
        public decimal TotalPartCost { get; set; }
        public decimal TotalCraftCost { get; set; }
        public decimal TotalLaborHours { get; set; }
        public decimal TotalExternalCost { get; set; }
        public decimal TotalInternalCost { get; set; }
        public decimal TotalSummeryCost { get; set; }
        #region Localizations
        public string spnSummary { get; set; }
        public string spnPartsCosts { get; set; }
        public string spnLaborHours { get; set; }
        public string spnCraftCosts { get; set; }
        public string spnOtherExternalCosts { get; set; }
        public string spnOtherInternalCosts { get; set; }
        public string spnSummaryTotalCosts { get; set; }
        #endregion
    }
    [Serializable]
    public class PMPrintInputModel
    {
        public long PrevMaintMasterId { get; set; }
        public long PrevMaintLibraryID { get; set; }
    }
}