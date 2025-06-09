namespace Client.Models
{
    public class PartsEquipmentGridModel
    {
        public string Equipment_ClientLookupId { get; set; }
        public string Equipment_Name { get; set; }
        public string Part_ClientLookupId { get; set; }       
        public long Equipment_Parts_XrefId { get; set; }
        public long EquipmentId { get; set; }
        public long PartId { get; set; }
        public string Comment { get; set; }
        public decimal QuantityNeeded { get; set; }
        public decimal QuantityUsed { get; set; }
        public int UpdateIndex { get; set; }
        public string PartEquipmentSecurity { get; set; }
    }
}