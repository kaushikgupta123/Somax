using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Client.Models
{
    public class PrintPartVendorXrefModel
    {
        public string Part { get; set; }
        public string PartDescription { get; set; }
        public string CatalogNumber { get; set; }
        public string Manufacturer { get; set; }
        public string ManufacturerID { get; set; }
        public int? OrderQuantity { get; set; }
        public string OrderUnit { get; set; }
        public decimal? Price { get; set; }
    }
}