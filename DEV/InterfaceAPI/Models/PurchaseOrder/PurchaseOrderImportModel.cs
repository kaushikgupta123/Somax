using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.ComponentModel.DataAnnotations;

namespace InterfaceAPI.Models
{
    public class PurchaseOrderImportModel
    {
        public PurchaseOrderImportModel()
        {
            LineItems = new List<PurchaseOrderLineImportModel>();
        }
        public int ClientId { get; set; }
        public int SiteId { get; set; }
        public string PONumber { get; set; }
        public int Revision { get; set; }
        public int ExPOId { get; set; }
        public int ExPRId { get; set; }
        public string SOMAXReqNo { get; set; }
        public int SOMAXReqId { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; } = (DateTime)SqlDateTime.MinValue;
        public string Currency { get; set; }
        public string Vendor { get; set; }
        public int ExVendorId { get; set; }
        public DateTime RequiredDate { get; set; } = (DateTime)SqlDateTime.MinValue;
        public string PaymentTerms { get; set; }
        public List<PurchaseOrderLineImportModel> LineItems { get; set; }
        public string ErrorCodes { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime LastProcess { get; set; } = (DateTime)SqlDateTime.MinValue;
  }
}