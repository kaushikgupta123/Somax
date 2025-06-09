using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterfaceAPI.Models.BBU.POReceiptImport
{
    public class POReceiptImportModel
    {
        public POReceiptImportModel()
        {
            ClientId = 0;
            POReceiptImport2Id = 0;
            SiteId = 0;
            ExReceipt = "";
            ExReceiptId = 0;
            ExReceiptTxn = 0;
            ExVendorId = 0;
            ExVendor = "";
            ReceiptDate = DateTime.MinValue;
            TransactionDate = DateTime.MinValue;
            ExPurchaseOrderId = 0;
            ExPurchaseOrder = "";
            ExPurchaseOrderLineId = 0;
            POLineNumber = 0;
            ExPartId = 0;
            ExPart = "";
            Description = "";
            ReceiptQuantity = 0;
            PurchaseUOM = "";
            UnitOfMeasure = "";
            UOMConversion = 0;
            Reason = "";
            ErrorMessage = "";
            LastProcessed = DateTime.MinValue;
            CreateBy = "";
            CreateDate = DateTime.MinValue;
            UpdateIndex = 0;

        }
        public long ClientId { get; set; }
        public long POReceiptImport2Id { get; set; }
        public long SiteId { get; set; }
        public string ExReceipt { get; set; }
        public long ExReceiptId { get; set; }
        public long ExReceiptTxn { get; set; }
        public long ExVendorId { get; set; }
        public string ExVendor { get; set; }
        public DateTime? ReceiptDate { get; set; }
        public DateTime? TransactionDate { get; set; }
        public long ExPurchaseOrderId { get; set; }
        public string ExPurchaseOrder { get; set; }
        public long ExPurchaseOrderLineId { get; set; }
        public int POLineNumber { get; set; }
        public long ExPartId { get; set; }
        public string ExPart { get; set; }
        public string Description { get; set; }
        public Decimal ReceiptQuantity { get; set; }
        public string PurchaseUOM { get; set; }
        public string UnitOfMeasure { get; set; }
        public Decimal UOMConversion { get; set; }
        public string Reason { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime? LastProcessed { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public int UpdateIndex { get; set; }

    }
}