using System;
using System.Collections.Generic;

namespace Client.Models.PartIssue
{
    public class PartHistoryModel
    {
        #region Properties
        public int PartIssueId { get; set; }
        public string IssueToClientLookupId { get; set; }
        public long PartStoreroomId { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string ChargeType_Primary { get; set; }
        public string ChargeToClientLookupId { get; set; }
        public long ChargeToId_Primary { get; set; }
        public decimal TransactionQuantity { get; set; }
        public string PartClientLookupId { get; set; }
        public long PartId { get; set; }
        public string Description { get; set; }
        public long SiteId { get; set; }
        public string TransactionType { get; set; }
        public bool IsPartIssue { get; set; }
        public string ErrorMessagerow { get; set; }
        public string PartUPCCode { get; set; }
        public List<string> ErrorMessages { get; set; }
        public string Comments { get; set; }

        #endregion
    }
}