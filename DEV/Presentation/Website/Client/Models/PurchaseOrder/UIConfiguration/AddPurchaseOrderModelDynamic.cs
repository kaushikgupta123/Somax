using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.PurchaseOrder.UIConfiguration
{
    public class AddPurchaseOrderModelDynamic
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

        #region PurchaseOrder Columns
        public long ? PurchaseOrderId { get; set; }
        public string ClientLookupId { get; set; }
        public string Attention { get; set; }
        public long ? Buyer_PersonnelId { get; set; }
        public string Carrier { get; set; }
        public long ? CompleteBy_PersonnelId { get; set; }
        public DateTime? Required { get; set; }
        public DateTime? CompleteDate { get; set; }
        public long ? Creator_PersonnelId { get; set; }
        public string FOB { get; set; }
        public string Status { get; set; }
        public string Terms { get; set; }
        public long ? VendorId { get; set; }
        public long ? VoidBy_PersonnelId { get; set; }
        public DateTime? VoidDate { get; set; }
        public string VoidReason { get; set; }
        public string Reason { get; set; }
        public string MessageToVendor { get; set; }
        public long ? ExPurchaseOrderId { get; set; }
        public string ExPurchaseRequest { get; set; }
        public string Currency { get; set; }
        public int Revision { get; set; }
        public string PaymentTerms { get; set; }
        public bool IsExternal { get; set; }
        public bool IsPunchOut { get; set; }
        public bool SentOrderRequest { get; set; }
        public long? StoreroomId { get; set; }
        public long? Shipto { get; set; }
        #endregion
        public string VendorClientLookupId { get; set; }

    }
}