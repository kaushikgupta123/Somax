using System;
namespace Client.Models.PurchaseRequest
{
    public class PRShoppingcartModel
    {
        public long AccountId { get; set; }
        public long ChargeToID { get; set; }
        public string ChargeType { get; set; }
        public long ClientId { get; set; }
        public long SiteId { get; set; }
        public string Description { get; set; }
        public string ManufacturerName { get; set; }
        public string ManufacturerPartID { get; set; }
        public long OrderQuantity { get; set; }
        public long PartId { get; set; }
        public long PunchoutLineItemId { get; set; }
        public string SupplierPartAuxiliaryId { get; set; }
        public string SupplierPartId { get; set; }
        public decimal UnitCost { get; set; }
        public string UnitofMeasure { get; set; }
        public string Classification { get; set; }
        #region V2-1119
        public DateTime? RequiredDate { get; set; }
        public Int64 UNSPSC { get; set; }
        #endregion
    }
}