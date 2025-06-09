namespace Client.Models.Configuration.PartMaster
{
    public class ManufacturerMasterModel
    {
        public string Name { get; set; }
        public string ClientLookupId { get; set; }
        public long ManufacturerMasterId { get; set; }
        public long PartMasterId { get; set; }
        public long TotalCount { get; set; }
    }

}