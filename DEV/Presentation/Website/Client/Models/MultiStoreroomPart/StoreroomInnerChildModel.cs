using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.MultiStoreroomPart
{
    public class StoreroomInnerChildModel
    {
       
        public long PartStoreroomId { get; set; }
        public string Location1_1 { get; set; }
        public string Location1_2 { get; set; }
        public string Location1_3 { get; set; }
        public string Location1_4 { get; set; }
        public string Location1_5 { get; set; }
        public string Location2_1 { get; set; }
        public string Location2_2 { get; set; }
        public string Location2_3 { get; set; }
        public string Location2_4 { get; set; }
        public string Location2_5 { get; set; }
        public string StoreroomName { get; set; }
        public long StoreroomId { get; set; }
        public int CountFrequency { get; set; }
        public DateTime? LastCounted { get; set; }

        public bool Critical { get; set; }
        public bool AutoPurchase { get; set; }
        public string VendorName { get; set; }
        public decimal? QuantityOnHand { get; set; }
        public decimal? MaximumQuantity { get; set; }
        public decimal? MinimumQuantity { get; set; }
        public long PartVendorId { get; set; }
        public string VendorClientLookupId { get; set; }
    }
}