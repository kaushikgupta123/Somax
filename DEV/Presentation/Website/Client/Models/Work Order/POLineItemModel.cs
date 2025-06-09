using System;

namespace Client.Models.Work_Order
{
    public class POLineItemModel
    {
        public long PurchaseOrderLineItemId { get; set; }
        public long PurchaseOrderId { get; set; }
        public long DepartmentId { get; set; }
        public long StoreroomId { get; set; }
        public long AccountId { get; set; }
        public long ChargeToId { get; set; }
        public string ChargeType { get; set; }
        public long CompleteBy_PersonnelId { get; set; }
        public DateTime? CompleteDate { get; set; }
        public long Creator_PersonnelId { get; set; }
        public string Description { get; set; }
        public DateTime? EstimatedDelivery { get; set; }
        public int LineNumber { get; set; }
        public long PartId { get; set; }
        public long PartStoreroomId { get; set; }
        public long PRCreator_PersonnelId { get; set; }
        public long PurchaseRequestId { get; set; }
        public decimal OrderQuantity { get; set; }
        public decimal OrderQuantityOriginal { get; set; }
        public string Status { get; set; }
        public bool Taxable { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal UnitCost { get; set; }
        public int ExPurchaseOrderLineId { get; set; }
        public string PurchaseUOM { get; set; }
        public decimal UOMConversion { get; set; }
        public decimal PurchaseQuantity { get; set; }
        public decimal PurchaseCost { get; set; }
        public decimal ReceivedQuantity { get; set; }
        public DateTime Required { get; set; }
        public string ClientLookupId { get; set; }
        public int UpdateIndex { get; set; }
    }
}