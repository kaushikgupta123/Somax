using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Client.Models.PartTransfer
{
    public class PartTransferIssueModel
    {
        public long? PartTransferId { get; set; }
        [RegularExpression(@"^\d*\.?\d{0,6}$", ErrorMessage = "globalSixDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(1.0, 999999999.999999, ErrorMessage = "globalSixDecimalAfterNineDecimalBeforeTotalFifteenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "ValidQuantity|" + LocalizeResourceSetConstants.PartTransferDetail)]
        public decimal? IssueQuantity { get; set; }
        public string Comment { get; set; }
    }
}