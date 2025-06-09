namespace Client.Models
{
    public class PartsModel
    {
        public long EquipmentId { get; set; }
        public string Part_ClientLookupId { get; set; }
        public string Part_Description { get; set; }
        public decimal QuantityNeeded { get; set; }
        public decimal QuantityUsed { get; set; }
        public string Comment { get; set; }
        public long Equipment_Parts_XrefId { get; set; }
        public string PartsSecurity { get; set; }
        public int UpdatedIndex { get; set; }
        public long PartId { get; set; }
        #region V2-1007
        public string Equipment_ClientLookupId { get; set; }
        #endregion
    }
}