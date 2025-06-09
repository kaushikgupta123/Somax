using System;

namespace Client.Models
{
    public class PartIssues
    {
        public string PartClientLookupId { get; set; }
        public string Description { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string ChargeToClientLookupId { get; set; }
        public decimal TransactionQuantity { get; set; }
        public string UnitofMeasure { get; set; }
        public decimal Cost { get; set; }
        public string IssuedTo { get; set; }

    }
}