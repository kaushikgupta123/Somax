namespace Client.Models
{
    public class ChildrenGridDataModel
    {
        public string ClientLookupId { get; set; }
        public long EquipmentId { get; set; }
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public string Type { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }      
        public int TotalCount { get; set; }
    }
}