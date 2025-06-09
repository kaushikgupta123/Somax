using Common.Constants;

using System;
using System.ComponentModel.DataAnnotations;

namespace Client.Models.Personnel
{
    public class AuxiliaryInformationModel
    {
        [Required]
        public long PersonnelId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? LastSalaryReview { get; set; }
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 99999999.99, ErrorMessage = "globalTwoDecimalAfterTotalTenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? InitialPay { get; set; }
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 99999999.99, ErrorMessage = "globalTwoDecimalAfterTotalTenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? BasePay { get; set; }
    }
}