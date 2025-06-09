using Client.Models.Common;
using System;
using System.Collections.Generic;

namespace Client.Models.PurchaseRequest
{
    [Serializable]
    public class PRPrintParams
    {
        public PRPrintParams()
        {
            tableHaederProps = new List<TableHaederProp>();
        }
        public List<TableHaederProp> tableHaederProps { get; set; }
        public string colname { get; set; }
        public string coldir { get; set; }
        public int CustomQueryDisplayId { get; set; }
        public string PurchaseRequest { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public string Vendor { get; set; }
        public string VendorName { get; set; }
        public DateTime? CreateStartDate { get; set; }
        public DateTime? CreateEndDate { get; set; }
        public string PONumber { get; set; }
        public string ProcessedBy { get; set; }
        public DateTime? ProcessedStartDate { get; set; }
        public DateTime? ProcessedEndDate { get; set; }
        public string txtSearchval { get; set; }
        public string Creator_PersonnelName { get; set; }
        public string Processed_PersonnelName { get; set; }

        public DateTime? ProcessedStartDateVw { get; set; }
        public DateTime? ProcessedEndDateVw { get; set; }

        public DateTime? CreateStartDateVw { get; set; }
        public DateTime? CreateEndDateVw { get; set; }

        public DateTime? CancelandDeniedStartDateVw { get; set; }
        public DateTime? CancelandDeniedEndDateVw { get; set; }
    }
}