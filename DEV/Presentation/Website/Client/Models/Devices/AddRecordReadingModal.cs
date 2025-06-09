using Client.CustomValidation;

using Common.Constants;

using System.ComponentModel.DataAnnotations;

namespace Client.Models
{
    public class AddRecordReadingModal
    {
        public long IoTDeviceId { get; set; }

        [Display(Name = "GlobalDate|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "vaildDate|" + LocalizeResourceSetConstants.Global)]
        public string ReadingDate { get; set; }

        [Required(ErrorMessage = "vaildTime|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression(@"^([0-9]|0[0-9]|1[0-9]|2[0-3]):([0-9]|[0-5][0-9]) (am|pm|AM|PM)$", ErrorMessage = "globalTimeErrorMessage|" + LocalizeResourceSetConstants.Global)]
        public string ReadingTime { get; set; }

        [Required(ErrorMessage = "ReadingErrorMessage|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression(@"^\d*\.?\d{0,3}$", ErrorMessage = "globalThreeDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(1, 99999999999999.999, ErrorMessage = "ValidationTotalSeventeenAfterThreeBeforeFourteen|" + LocalizeResourceSetConstants.MeterDetails)]
        public decimal Reading { get; set; }
    }
}