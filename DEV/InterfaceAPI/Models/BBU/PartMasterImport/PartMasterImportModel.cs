using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterfaceAPI.Models.BBU.PartMasterImport
{
    public class PartMasterImportModel
    {
        public PartMasterImportModel()
        {
            ClientId = 0;
            SiteId = 0;
            PartMasterImportId = 0;
            ClientLookupId = "";
            OEMPart = false;
            EXPartId = 0;
            ExAltPartId1 = "";
            ExAltPartId2 = "";
            ExAltPartId3 = "";
            ExUniqueId = Guid.Empty;
            Enabled = "";
            LongDescription = "";
            Manufacturer = "";
            ManufacturerId = "";
            ShortDescription = "";
            UnitCost = 0;
            UnitOfMeasure = "";
            UnitOfMeasureDesc = "";
            Category = "";
            CategoryDesc = "";
            UPCCode = "";
            ImageURL = "";
            ErrorMessage = "";
            LastProcessed = null;
            UpdateIndex = 0;


        }
        public Int64 ClientId { get; set; }
        public Int64 SiteId { get; set; }
        public Int64 PartMasterImportId { get; set; }
        public string ClientLookupId { get; set; }
        public bool OEMPart { get; set; }
        public Int64 EXPartId { get; set; }
        public string ExAltPartId1 { get; set; }
        public string ExAltPartId2 { get; set; }
        public string ExAltPartId3 { get; set; }
        public System.Guid ExUniqueId { get; set; }
        public string Enabled { get; set; }
        public string LongDescription { get; set; }
        public string Manufacturer { get; set; }
        public string ManufacturerId { get; set; }
        public string ShortDescription { get; set; }
        public decimal UnitCost { get; set; }
        public string UnitOfMeasure { get; set; }
        public string UnitOfMeasureDesc { get; set; }
        public string Category { get; set; }
        public string CategoryDesc { get; set; }
        public string UPCCode { get; set; }
        public string ImageURL { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime? LastProcessed { get; set; }
        public int UpdateIndex { get; set; }

    }
}