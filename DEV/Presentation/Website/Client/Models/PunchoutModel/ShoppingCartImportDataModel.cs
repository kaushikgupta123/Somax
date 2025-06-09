using Client.Models.Common;
using System;
using System.Collections.Generic;

namespace Client.Models.PunchoutModel
{
    [Serializable]
    public class ShoppingCartImportDataModel
    {
        public Int64 ClientId { get; set; }
        public Int64 SiteId { get; set; }
        public Int64 PunchoutLineItemId { get; set; }
        public Int64 PurchaseRequestId { get; set; }
        public Int64 AccountId { get; set; }
        public Int64 ChargeToID { get; set; }
        public string ChargeType { get; set; }
        public string Description { get; set; }
        public Int64 PartId { get; set; }
        public Int64 PartStoreroomId { get; set; }
        public decimal OrderQuantity { get; set; }
        public string UnitofMeasure { get; set; }
        public decimal UnitCost { get; set; }
        public string SupplierPartId { get; set; }
        public string SupplierPartAuxiliaryId { get; set; }
        public string Classification { get; set; }
        public string ManufacturerPartID { get; set; }
        public string ManufacturerName { get; set; }
        public int IsValid { get; set; }
        public string ClientLookupId { get; set; }
        public string Status { get; set; }
        public List<DataTableDropdownModel> ChargeTypeListdropDown { get; set; }
        #region V2-1119
        public string OrderUnit { get; set; }
        public string Part_ClientLookupId { get; set; }
        public List<DataTableDropdownModel> OrderUnitListdropDown { get; set; }
        #endregion
    }
}