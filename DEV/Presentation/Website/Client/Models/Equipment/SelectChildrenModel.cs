namespace Client.Models
{
    public class SelectChildrenModel
    {
        public long EquipmentId { get; set; }
        public string ClientLookupId { get; set; }
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public string Type { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
    }
}