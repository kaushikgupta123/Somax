using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.Meters
{
    public class MetersModel
    {
        [Required(ErrorMessage = "ValidationMeteriD|" + LocalizeResourceSetConstants.MeterDetails)]
        public string MeterClientLookUpId { get; set; }
        public long MeterId { get; set; }
        [Required(ErrorMessage = "ValidationName|" + LocalizeResourceSetConstants.MeterDetails)]
        public string MeterName { get; set; }
        public decimal ReadingCurrent { get; set; }
        public string StringReadingCurrent { get; set; }
        public DateTime? ReadingDate { get; set; }
        public long ReadingBy { get; set; }
        public string PersonnelClientLookUpId { get; set; }
        public decimal ReadingLife { get; set; }

        public string StringReadingLife { get; set; }
        [Required(ErrorMessage = "ValidationMaximumReading|" + LocalizeResourceSetConstants.MeterDetails)]
       [RegularExpression(@"^\d*\.?\d{0,3}$", ErrorMessage = "globalThreeDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        //[Range(1, 99999999999999.999, ErrorMessage = "ValidationTotalSeventeenAfterThreeBeforeFourteen|" + LocalizeResourceSetConstants.MeterDetails)]
        public decimal MaxReading { get; set; }
        public string StringMaxReading { get; set; }
        public bool InActive { get; set; }
        public string ReadingUnits { get; set; }
        public IEnumerable<SelectListItem> ReadingUnitsList { get; set; }
        public int TotalCount { get; set; } // V2-950
    }
}