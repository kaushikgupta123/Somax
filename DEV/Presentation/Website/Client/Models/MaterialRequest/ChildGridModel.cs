using Common.Constants;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;

namespace Client.Models.MaterialRequest
{
    public class ChildGridModel
    {
        public long ClientId { get; set; }
        public long EstimatedCostsId { get; set; }
        public string PartClientLookupId { get; set; }
        public long CategoryId { get; set; }
        public long ObjectId { get; set; }
        public string Description { get; set; }
        public decimal UnitCost { get; set; }
        public decimal Quantity { get; set; }
        public decimal TotalCost { get; set; } 
        public string Status { get; set; }
    }
}