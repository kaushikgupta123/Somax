namespace Client.Models.FleetService
{
    public class FleetServicePrintModel
    {
        public string ClientLookupId { get; set; }
        public string EquipmentClientLookupId { get; set; }
        public string AssetName { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string CreateDate { get; set; }
        public string Assigned { get; set; }
        public string ScheduleDate { get; set; }
        public string CompleteDate { get; set; }      

    }
}