using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Client.Models.MultiStoreroomPart
{
    public class MultiStoreroomPartChildModel
    {
        public string PartClientLookupId { get; set; }
        public long PartStoreroomId { get; set; }
        public decimal? QtyOnHand { get; set; }
        public decimal? QtyMaximum { get; set; }
        public decimal? QtyReorderLevel { get; set; }
        public string Location1_1 { get; set; }
        public string Location1_2 { get; set; }
        public string Location1_3 { get; set; }
        public string Location1_4 { get; set; }
        public string Location1_5 { get; set; }
        public string StoreroomName { get; set; }
        public long StoreroomId { get; set; }
        public int CountFrequency { get; set; }
        public DateTime? LastCounted { get; set; }
        //V2-1059
        public long AutoTransfer { get; set; }
        public bool Maintain { get; set; }
        #region V2-755
        public bool Issue { get; set; }
        public bool PhysicalInventory { get; set; }
        #endregion
        #region 1025
        public string StoreroomNameWithDescription { get; set; }
        public decimal TotalOnRequest { get; set; }
        public decimal TotalOnOrder { get; set; }
        #endregion
    }
}