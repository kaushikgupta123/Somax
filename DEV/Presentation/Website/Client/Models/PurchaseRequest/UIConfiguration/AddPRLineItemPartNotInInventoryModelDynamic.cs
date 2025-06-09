using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.PurchaseRequest.UIConfiguration
{
    public class AddPRLineItemPartNotInInventoryModelDynamic
    {
        #region UDF columns
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

        #region Purchase Request Line Item table coulmn
        public string Description { get; set; }
        public long? AccountId { get; set; }
        public string ChargeType { get; set; }
        public long? ChargeToId { get; set; }
        public long? PurchaseRequestId { get; set; }
        public long? PurchaseRequestLineItemId { get; set; }
        public int UpdateIndex { get; set; }
        public string ClientLookupId { get; set; }
        public decimal? OrderQuantity { get; set; }
        public decimal? UnitCost { get; set; }
        public string ChargeTo_Name { get; set; }
        public string ViewName { get; set; }
        public string PurchaseUOM { get; set; }
        public DateTime? RequiredDate { get; set; }
        public bool IsShopingCart { get; set; }
        public string Status { get; set; }
        public int LineNumber { get; set; }
        public long? PartId { get; set; }
        public string UnitofMeasure { get; set; }
        public long? PartStoreroomId { get; set; }
        public long? VendorCatalogItemId { get; set; }
        public string SupplierPartId { get; set; }
        public string SupplierPartAuxiliaryId { get; set; }
        public string ManufacturerPartId { get; set; }
        public string Manufacturer { get; set; }
        public string Classification { get; set; }
        public decimal? PurchaseQuantity { get; set; }
        public decimal? UOMConversion { get; set; }
        public long? UNSPSC { get; set; }
        public string AccountClientLookupId { get; set; } //V2-788
        #endregion

    }
}