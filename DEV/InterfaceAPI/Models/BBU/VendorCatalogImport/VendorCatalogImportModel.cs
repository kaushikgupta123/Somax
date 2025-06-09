using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterfaceAPI.Models.BBU.VendorCatalogImport
{
    public class VendorCatalogImportModel
    {
        public VendorCatalogImportModel()
        {
            ClientId = 0;
            VendorCatalogImportId = 0;
            ExVendorId = 0;
            ExVendorNumber = "";
            ExVendorSiteId = 0;
            ExVendorSiteCode = "";
            ExSourceId = 0;
            ExSourceDocument = "";
            StartDate = DateTime.MaxValue;
            EndDate = DateTime.MinValue;
            Canceled = "";
            LineNumber = 0;
            ExLineId = 0;
            ExPartId = 0;
            ExPartNumber = "";
            Category = "";
            Description = "";
            PurchaseUOM = "";
            UnitCost = 0;
            UnitOfMeasure = "";
            UOMConversion = 0;
            VendorPartNumber = "";
            LeadTime = 0;
            MinimumQuantity = 0;
            ErrorMessage = "";
            LastProcessed = DateTime.MinValue;
            ExpirationDate = DateTime.MinValue;
            UpdateIndex = 0;

        }
        public long ClientId { get; set; }
        public long VendorCatalogImportId { get; set; }
        public long ExVendorId { get; set; }
        public string ExVendorNumber { get; set; }
        public long ExVendorSiteId { get; set; }
        public string ExVendorSiteCode { get; set; }
        public long ExSourceId { get; set; }
        public string ExSourceDocument { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Canceled { get; set; }
        public int LineNumber { get; set; }
        public long ExLineId { get; set; }
        public long ExPartId { get; set; }
        public string ExPartNumber { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string PurchaseUOM { get; set; }
        public decimal UnitCost { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal UOMConversion { get; set; }
        public string VendorPartNumber { get; set; }
        public int LeadTime { get; set; }
        public int MinimumQuantity { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime? LastProcessed { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int UpdateIndex { get; set; }

    }
}