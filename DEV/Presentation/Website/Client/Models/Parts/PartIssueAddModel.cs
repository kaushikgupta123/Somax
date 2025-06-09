using Client.CustomValidation;

using Common.Constants;

using System.ComponentModel.DataAnnotations;

namespace Client.Models.Dashboard
{
    public class PartIssueAddModel
    {
        [Required(ErrorMessage = "validationPartId|" + LocalizeResourceSetConstants.Global)]
        public string Part_ClientLookupId { get; set; }
        public long PartId { get; set; }
        [Required(ErrorMessage = "validationQuantity|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0.01, 99999999.99, ErrorMessage = "globalTwoDecimalAfterEightDecimalBeforeErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal Quantity { get; set; }
        public long WorkOrderId { get; set; }
        public long PartStoreroomId { get; set; }
        public string WorkOrderClientLookupId { get; set; }
        public bool UseMultiStoreroom { get; set; } // V2-687
        [RequiredIf("UseMultiStoreroom", true, ErrorMessage = "GlobalStoreroomSelect|" + LocalizeResourceSetConstants.Global)]
        public long? StoreroomId { get; set; } // V2-687
    }
}