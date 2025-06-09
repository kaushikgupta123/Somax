using Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace Client.Models.MultiStoreroomPart
{
    public class AddToAutoTransfer
    {
        public long PartId { get; set; }
        public long PartStoreroomId { get; set; }
        public long StoreroomId { get; set; }
        public long AutoTransfer { get; set; }
        [Required(ErrorMessage = "spnStoreroomErrorMsg|" + LocalizeResourceSetConstants.Global)]
        public long AutoTransferIssueStoreroom { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Required(ErrorMessage = "Please Enter Request Quantity")]
        [RegularExpression(@"^\d*\.?\d{0,6}$", ErrorMessage = "globalSixDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(1, 999999999.999999, ErrorMessage = "GlobalMaxRangeOneToNieDigitWithSixDecimalPlace|" + LocalizeResourceSetConstants.PartDetails)]
        public decimal? AutoTransferMaxQty { get; set; }
        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Required(ErrorMessage = "Please Enter Request Quantity")]
        [RegularExpression(@"^\d*\.?\d{0,6}$", ErrorMessage = "globalSixDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(1, 999999999.999999, ErrorMessage = "GlobalMaxRangeOneToNieDigitWithSixDecimalPlace|" + LocalizeResourceSetConstants.PartDetails)]
        public decimal? AutoTransferMinQty { get; set; }
    }
}