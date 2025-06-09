using System;

namespace Client.Models.FleetService
{
    public class FleetServiceSummaryModel
    {
        public string ServiceOrder_ClientLookupId { get; set; }
        public string Equipment_ClientLookupId { get; set; }
        public string EquipmentName { get; set; }
        public string ImageUrl { get; set; }
        public string Meter1Type { get; set; }
        public decimal Meter1CurrentReading { get; set; }
        public string Meter2Type { get; set; }
        public decimal Meter2CurrentReading { get; set; }
        public DateTime? ScheduleDate { get; set; }
        public string Assigned { get; set; }
        public DateTime? CompleteDate { get; set; }
        public long ServiceOrderId { get; set; }
        public long SOAssigned_PersonnelId { get; set; }
        public string Meter1Units { get; set; }
        public string Meter2Units { get; set; }
        public double Meter1DayDiff { get; set; }
        public double Meter2DayDiff { get; set; }
        public string Status { get; set; }
        public bool IsDetail { get; set; }
        public bool ClientOnPremise { get; set; }
    }
}