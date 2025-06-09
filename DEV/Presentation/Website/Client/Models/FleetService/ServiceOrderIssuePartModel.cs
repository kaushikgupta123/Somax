using Common.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace Client.Models.FleetService
{
    public class ServiceOrderIssuePartModel
    {
        
        [Required(ErrorMessage = "spnPleaseSelectPartId|" + LocalizeResourceSetConstants.Global)]
        [Display(Name = "spnPartID|" + LocalizeResourceSetConstants.Global)]
        public string PartClientLookupId { get; set; }
        public string Description { get; set; }
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(double.Epsilon, 99999999.99, ErrorMessage = "globalGreaterThan0TwoDecimalAfterTotalTenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Display(Name = "spnQuantity|" + LocalizeResourceSetConstants.Global)]
        public decimal TransactionQuantity { get; set; }
        public decimal Cost { get; set; }
        public decimal TotalCost { get; set; }
        public string UnitofMeasure { get; set; }
        public DateTime? TransactionDate { get; set; }
        public long ServiceOrderId { get; set; }
        public long ServiceOrderLineItemId { get; set; }
        public long PartId { get; set; }
        public long PartStoreroomId { get; set; }
        public string ClientLookupId { get; set; }
        public string UPCCode { get; set; }
        [Display(Name = "FailureCode|" + LocalizeResourceSetConstants.FleetServiceOrder)]
        [Required(ErrorMessage = "FailureCodeRequired|" + LocalizeResourceSetConstants.FleetServiceOrder)]
        public string VMRSFailure { get; set; }
        public string VMRSFailureCode { get; set; }
    }
}