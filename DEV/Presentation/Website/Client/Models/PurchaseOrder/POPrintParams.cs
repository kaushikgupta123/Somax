using Client.Models.Common;
using System;
using System.Collections.Generic;

namespace Client.Models.PurchaseOrder
{
    [Serializable]
    public class POPrintParams
    {
        public POPrintParams()
        {
            tableHaederProps = new List<TableHaederProp>();
        }
        public List<TableHaederProp> tableHaederProps { get; set; }
        public string colname { get; set; }
        public string coldir { get; set; }
        public int CustomQueryDisplayId { get; set; }
        public DateTime? CompleteStartDateVw { get; set; }
        public DateTime? CompleteEndDateVw { get; set; }
        public DateTime? CreateStartDateVw { get; set; }
        public DateTime? CreateEndDateVw { get; set; }
        public string PurchaseOrder { get; set; }
        public string Status { get; set; }
        public string VendorClientLookupId { get; set; }
        public string VendorName { get; set; }
        public DateTime? StartCreateDate { get; set; }
        public DateTime? EndCreateDate { get; set; }
        public string Attention { get; set; }
        public string VendorPhoneNumber { get; set; }
        public DateTime? StartCompleteDate { get; set; }
        public DateTime? EndCompleteDate { get; set; }
        public string Reason { get; set; }
        public string Buyer_PersonnelName { get; set; }
        public string TotalCost { get; set; }
        public string FilterValue { get; set; }
        public string txtSearchval { get; set; }
        public DateTime? Required { get; set; } //V2-1171
    }
}