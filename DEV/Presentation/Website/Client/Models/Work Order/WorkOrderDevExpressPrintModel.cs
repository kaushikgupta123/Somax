using System;
using System.Collections.Generic;

namespace Client.Models.Work_Order
{
    public class WorkOrderDevExpressPrintModel
    {
        public WorkOrderDevExpressPrintModel()
        {
            TasksDevExpressPrintModelList = new List<TasksDevExpressPrintModel>();
            LaborDevExpressPrintModelList = new List<LaborDevExpressPrintModel>();
            PartDevExpressPrintModelList = new List<PartDevExpressPrintModel>();
            OthersDevExpressPrintModelList = new List<OthersDevExpressPrintModel>();
            SummaryDevExpressPrintModelList = new List<SummaryDevExpressPrintModel>();
            PhotosDevExpressPrintModelList = new List<PhotosDevExpressPrintModel>();
            AttachmentDevExpressPrintModelList = new List<AttachmentDevExpressPrintModel>();
            NotesDevExpressPrintModelList=new List<NotesDevExpressPrintModel>();
            WOScheduleDevExpressPrintModelList = new List<WOScheduleDevExpressPrintModel>();
        }
        public long WorkOrderId { get; set; }
        public string ClientlookupId { get; set; }
        public string Description { get; set; }
        public string ChargeType { get; set; }
        public string ChargeToClientLookupId { get; set; }
        public string ChargeTo_Name { get; set; }
        public string AssetLocation { get; set; }
        public string DownRequired { get; set; }
        public string Type { get; set; }
        public string SourceType { get; set; }
        public string RequiredDate { get; set; }
        public string ScheduledStartDate { get; set; }
        public decimal ScheduledDuration { get; set; }
        public string Assigned { get; set; }
        public string AssignedFullName { get; set; }
        public string CreateBy { get; set; }
        public string CreateByPersonnelName { get; set; }
        public string CreateDate { get; set; }
        public string Instructions { get; set; }
        public string SiteInformation { get; set; }
        public string AzureImageUrl { get; set; }
        public long CompleteBy_PersonnelId { get; set; }
        public string CompleteBy_PersonnelClientLookupId { get; set; }
        public string CompleteDate { get; set; }
        public string CompleteComments { get; set; }
        public string Status { get; set; }
        public bool IsFoodSafetyShow { get; set; }
        public string SignoffBy { get; set; }
        public string EquipmentType { get; set; }
        public string SerialNumber { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string AssetGroup1ClientlookupId { get; set; }
        public string AssetGroup2ClientlookupId { get; set; }
        public string AssetGroup3ClientlookupId { get; set; }
        public bool RemoveFromService { get; set; }
        public string AssetUrl { get; set; }
        public string WOCompCriteriaTitle { get; set; }
        public string WOCompCriteria { get; set; }
        public bool WOCompCriteriaTab { get; set; }
        #region V2-944
        public string Priority { get; set; }
        public string AccountClientLookupId { get; set; }
        public long WorkOrderUDF_WOId { get; set; }
        public string Text1 { get; set; }
        public string Text2 { get; set; }
        public string Text3 { get; set; }
        public string Text4 { get; set; }
        public string Date1 { get; set; }
        public string Date2 { get; set; }
        public string Date3 { get; set; }
        public string Date4 { get; set; }
        public string Bit1 { get; set; }
        public string Bit2 { get; set; }
        public string Bit3 { get; set; }
        public string Bit4 { get; set; }
        public decimal Numeric1 { get; set; }
        public decimal Numeric2 { get; set; }
        public decimal Numeric3 { get; set; }
        public decimal Numeric4 { get; set; }
        public string Select1 { get; set; }
        public string Select2 { get; set; }
        public string Select3 { get; set; }
        public string Select4 { get; set; }
        public string Text1Label { get; set; }
        public string Text2Label { get; set; }
        public string Text3Label { get; set; }
        public string Text4Label { get; set; }
        public string Date1Label { get; set; }
        public string Date2Label { get; set; }
        public string Date3Label { get; set; }
        public string Date4Label { get; set; }
        public string Bit1Label { get; set; }
        public string Bit2Label { get; set; }
        public string Bit3Label { get; set; }
        public string Bit4Label { get; set; }
        public string Numeric1Label { get; set; }
        public string Numeric2Label { get; set; }
        public string Numeric3Label { get; set; }
        public string Numeric4Label { get; set; }
        public string Select1Label { get; set; }
        public string Select2Label { get; set; }
        public string Select3Label { get; set; }
        public string Select4Label { get; set; }
        public bool WOUIC { get; set; }
        public bool WOPhotos { get; set; }
        public bool WOSummary { get; set; }
        public bool WOComments { get; set; }
        public bool WOLaborRecording { get; set; }
        public bool WOScheduling { get; set; }
        #endregion

        #region Localization

        public string spnDetails { get; set; }
        //public string GlobalChargeTo { get; set; }
        public string SpnAsset { get; set; }
        public string SpnMake { get; set; }
        public string SpnModel { get; set; }
        public string GlobalSerialNumber { get; set; }
        public string SpnLocation { get; set; }
        public string AssetGroup1Label { get; set; }
        public string AssetGroup2Label { get; set; }
        public string AssetGroup3Label { get; set; }
        public string GlobalType { get; set; }
        public string spnRequired { get; set; }
        public string spnDownRequired { get; set; }
        public string spnScheduledDate { get; set; }
        public string spnScheduledDuration { get; set; }
        public string spnCreateBy { get; set; }
        public string spnWorkAssigned { get; set; }
        public string spnDescription { get; set; }
        public string spnAddInstructions { get; set; }
        public string spnDate { get; set; }
        public string spnHours { get; set; }
        public string spnWoEmployeeName { get; set; }
        public string spnCompleteComments { get; set; }
        public string spnFoodSafety_1 { get; set; }
        public string spnFoodSafety_2 { get; set; }
        public string spnFoodSafety_3 { get; set; }
        public string spnFoodSafety_4 { get; set; }
        public string spnFoodSafety_5 { get; set; }
        public string spnSignoffBy { get; set; }
        public string spnOn { get; set; }
        public string spnSignature { get; set; }
        public string GlobalWorkOrder { get; set; }
        public string globalCreateDate { get; set; }
        public string spnCopyRights { get; set; }
        public string spnGlobalCompleted { get; set; }
        public string spnAccount { get; set; }
        #region V2-944
        public string spnAssetGroups { get; set; }
        public string spnWorkOrderUIC { get; set; }
        public string GlobalStatus { get; set; }
        public string spnPriority { get; set; }
        public string spnScheduling { get; set; }
        public string spnLaborRecording { get; set; }
        #endregion
        #endregion
        public List<TasksDevExpressPrintModel> TasksDevExpressPrintModelList { get; set; }
        public List<LaborDevExpressPrintModel> LaborDevExpressPrintModelList { get; set; }
        public List<PartDevExpressPrintModel> PartDevExpressPrintModelList { get; set; }
        public List<OthersDevExpressPrintModel> OthersDevExpressPrintModelList { get; set; }
        public List<SummaryDevExpressPrintModel> SummaryDevExpressPrintModelList { get; set; }
        public List<PhotosDevExpressPrintModel> PhotosDevExpressPrintModelList { get; set; }
        public List<AttachmentDevExpressPrintModel> AttachmentDevExpressPrintModelList { get; set; }
        public List<NotesDevExpressPrintModel> NotesDevExpressPrintModelList { get; set; }
        #region V2-944
        public List<WOScheduleDevExpressPrintModel> WOScheduleDevExpressPrintModelList { get; set; }
        #endregion
        public bool OnPremise { get; set; }
    }
}