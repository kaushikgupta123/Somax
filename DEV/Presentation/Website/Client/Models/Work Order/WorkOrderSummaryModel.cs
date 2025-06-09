using DataContracts;
using System;

namespace Client.Models.Work_Order
{
    public class WorkOrderSummaryModel
    {
        public long WorkOrderId { get; set; }
        public string WorkOrder_ClientLookupId { get; set; }
        public string woName { get; set; }
        public string ImageUrl { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Priority { get; set; }
        public string ChargeToClientLookupId { get; set; }
        public string ChargeTo_Name { get; set; }
        public DateTime? ScheduledStartDate { get; set; }
        public string AssignedFullName { get; set; }
        public DateTime? CompleteDate { get; set; }
        public decimal? ScheduledDuration { get; set; }
        public Security security { get; set; }
        public string WorkAssigned_PersonnelClientLookupId { get; set; }
        public long WorkAssigned_PersonnelId { get; set; }
        public string Assigned { get; set; }
        public bool IsDetail { get; set; }
        //v2-463//
        public DateTime? EquipDownDate { get; set; }
        public decimal EquipDownHours { get; set; }

        public int PartsonOrder { get; set; }
        public string  AssetLocation { get; set; }
        public string ProjectClientLookupId { get; set; }//V2-626

        public bool ClientOnPremise { get; set; } //V2-635
        #region V2-847
        public string AssetGroup1Name { get; set; }
        public string AssetGroup2Name { get; set; }
        public string AssetGroup3Name { get; set; }
        public string AssetGroup1ClientlookupId { get; set; }
        public string AssetGroup2ClientlookupId { get; set; }
        #endregion
    }
}