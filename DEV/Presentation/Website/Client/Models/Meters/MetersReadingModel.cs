using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Client.Models.Meters
{
    public class MetersReadingModel
    {
        public long MeterId { get; set; }
        public long ReadingId { get; set; }
        [Required(ErrorMessage = "ValidationReading|" + LocalizeResourceSetConstants.MeterDetails)]
          [RegularExpression(@"^\d*\.?\d{0,3}$", ErrorMessage = "globalThreeDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(1, 99999999999999.999, ErrorMessage = "ValidationTotalSeventeenAfterThreeBeforeFourteen|" + LocalizeResourceSetConstants.MeterDetails)]
        public decimal Reading { get; set; }
        public string StringReading { get; set; }
        public string ReadByClientLookupId { get; set; }
        public string MeterClientLookUpId { get; set; }
        public bool Reset { get; set; }
        [Required(ErrorMessage = "ValidationReadingDate|" + LocalizeResourceSetConstants.MeterDetails)]
        public DateTime ? DateRead { get; set; }

    }
}