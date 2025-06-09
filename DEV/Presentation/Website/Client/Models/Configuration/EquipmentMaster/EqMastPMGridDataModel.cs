namespace Client.Models.Configuration.EquipmentMaster
{
    public class EqMastPMGridDataModel
    {
        public string Description { get; set; }
        public int Frequency { get; set; }
        public string FrequencyType { get; set; }
        public string ClientLookupId { get; set; }
        public long EQMaster_PMLibraryId { get; set; }
        public long EQMasterId { get; set; }
        public long PMLibraryId { get; set; }
        public long TotalCount { get; set; }
    }
}