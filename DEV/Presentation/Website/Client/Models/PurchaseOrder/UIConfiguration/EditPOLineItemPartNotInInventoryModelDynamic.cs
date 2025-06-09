using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.PurchaseOrder.UIConfiguration
{
    public class EditPOLineItemPartNotInInventoryModelDynamic
    {
        #region UDF columns
        public long POLineUDFId { get; set; }
        public string Text1 { get; set; }
        public string Text2 { get; set; }
        public string Text3 { get; set; }
        public string Text4 { get; set; }
        public DateTime? Date1 { get; set; }
        public DateTime? Date2 { get; set; }
        public DateTime? Date3 { get; set; }
        public DateTime? Date4 { get; set; }
        public bool Bit1 { get; set; }
        public bool Bit2 { get; set; }
        public bool Bit3 { get; set; }
        public bool Bit4 { get; set; }
        public decimal? Numeric1 { get; set; }
        public decimal? Numeric2 { get; set; }
        public decimal? Numeric3 { get; set; }
        public decimal? Numeric4 { get; set; }
        public string Select1 { get; set; }
        public string Select2 { get; set; }
        public string Select3 { get; set; }
        public string Select4 { get; set; }
        #endregion
        #region Purchase Order Line Item Columns
        public long? PurchaseOrderLineItemId { get; set; }
        public long? PurchaseOrderId { get; set; }
        public long? DepartmentId { get; set; }
        public long? StoreroomId { get; set; }
        public long? AccountId { get; set; }
        public long? ChargeToId { get; set; }
        public string ChargeType { get; set; }
        public long? CompleteBy_PersonnelId { get; set; }
        public DateTime? CompleteDate { get; set; }
        public long? Creator_PersonnelId { get; set; }
        public string Description { get; set; }
        public DateTime? EstimatedDelivery { get; set; }
        public int LineNumber { get; set; }
        public long? PartId { get; set; }
        public long? PartStoreroomId { get; set; }
        public long? PRCreator_PersonnelId { get; set; }
        public long? PurchaseRequestId { get; set; }
        public decimal? OrderQuantity { get; set; }
        public decimal? OrderQuantityOriginal { get; set; }
        public string Status { get; set; }
        public bool Taxable { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal? UnitCost { get; set; }
        public long ExPurchaseOrderLineId { get; set; }
        public string PurchaseUOM { get; set; }
        public decimal? UOMConversion { get; set; }
        public decimal? PurchaseQuantity { get; set; }
        public decimal? PurchaseCost { get; set; }
        public string SupplierPartId { get; set; }
        public string SupplierPartAuxiliaryId { get; set; }
        public string ManufacturerPartId { get; set; }
        public string Manufacturer { get; set; }
        public string Classification { get; set; }
        public long? VendorCatalogItemId { get; set; }
        public int? UpdateIndex { get; set; }
        public long ? UNSPSC { get; set; }
        #endregion
        public string PurchaseOrder_ClientLookupId { get; set; }
        public string PartClientLookupId { get; set; }
        public decimal? TotalCost { get; set; }
        public string AccountClientLookupId { get; set; }
        public string ChargeTo_Name { get; set; }
        public string ChargeToClientLookupId { get; set; }
        public bool IsPunchout { get; set; }
        public bool IsShopingCart { get; set; }
        public string PartCategoryMasterClientLookupId { get; set; }
    }
}