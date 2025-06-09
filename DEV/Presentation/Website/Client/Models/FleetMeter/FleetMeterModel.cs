using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.FleetMeter
{
    public class FleetMeterModel
    {
        public long EquipmentId { get; set; }

        [Display(Name = "spnGlobalEquipmentId|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "EquipmentIDErrorMessage|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [StringLength(31, ErrorMessage = "EquipmentStrLenErrorMessage|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression("^[A-Z0-9\\%\\-\\:\\/\\$\\*\\+\\.]+$", ErrorMessage = "EquipmentIDRegErrMsg|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string ClientLookupId { get; set; }
        public string Name { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string VIN { get; set; }
        public long NoOfDays { get; set; }
        public bool Meter2Indicator { get; set; }
        public DateTime? ReadingDate { get; set; }
        public int TotalCount { get; set; }
        public long FleetMeterReadingId { get; set; }
        public decimal Reading { get; set; }
        public string ReadingLine1 { get; set; }
        public string ReadingLine2 { get; set; }
        public string SourceType { get; set; }
        public long SourceId { get; set; }
        public bool Void { get; set; }
        public string EquipmentImage { get; set; }
        [Display(Name = "spnMeter1CurrentReading|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression(@"^\d*\.?\d{0,1}$", ErrorMessage = "globalOneDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0.1, 99999999.9, ErrorMessage = "GT0.0Min1OneDecimalAfterEightDecimalBeforeTotalNineRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal Meter1CurrentReading { get; set; }
        [Display(Name = "spnMeter2CurrentReading|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression(@"^\d*\.?\d{0,1}$", ErrorMessage = "globalOneDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0.1, 99999999.9, ErrorMessage = "GT0.0Min1OneDecimalAfterEightDecimalBeforeTotalNineRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal Meter2CurrentReading { get; set; }
        [Display(Name = "spnDate|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "DateDownErrorMessage|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string CurrentReadingDate { get; set; }
        [Required(ErrorMessage = "TimeDownErrorMessage|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression(@"^([0-9]|0[0-9]|1[0-9]|2[0-3]):([0-9]|[0-5][0-9]) (am|pm|AM|PM)$", ErrorMessage = "globalTimeErrorMessage|" + LocalizeResourceSetConstants.Global)]
        public string CurrentReadingTime { get; set; }
        [Display(Name = "spnVoid|" + LocalizeResourceSetConstants.Global)]
        public bool Meter1Void { get; set; }
        [Display(Name = "spnVoid|" + LocalizeResourceSetConstants.Global)]
        public bool Meter2Void { get; set; }
        public string Meter1Type { get; set; }
        public string Meter2Type { get; set; }
        public string MetersAssociated { get; set; }
    }
}