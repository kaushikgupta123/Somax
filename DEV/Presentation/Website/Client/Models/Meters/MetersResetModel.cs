using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Client.Models.Meters
{
    public class MetersResetModel
    {
        
        public string MeterClientLookUpId { get; set; }
        public long MeterId { get; set; }      
        [Required(ErrorMessage = "ValidationReading|" + LocalizeResourceSetConstants.MeterDetails)]
        [RegularExpression(@"^\d*\.?\d{0,3}$", ErrorMessage = "globalThreeDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(1, 99999999999999.999, ErrorMessage = "ValidationTotalSeventeenAfterThreeBeforeFourteen|" + LocalizeResourceSetConstants.MeterDetails)]
        public decimal Reading { get; set; }
        [Required(ErrorMessage = "ValidationReadingDate|" + LocalizeResourceSetConstants.MeterDetails)]
        public DateTime? ReadingDate { get; set; }
    }
}