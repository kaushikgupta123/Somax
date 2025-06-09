using Common.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace Client.Models.FleetService
{
    public class CompleteServiceOrderModel
    {
        public long ServiceOrderId { get; set; }
        public long EquipmentId { get; set; }
        [Display(Name = "spnGlobalEquipmentId|" + LocalizeResourceSetConstants.Global)]
        public string EquipmentClientLookupId { get; set; }
        public string Meter1Type { get; set; }
        public decimal Meter1CurrentReading { get; set; }
        public string Meter2Type { get; set; }
        public decimal Meter2CurrentReading { get; set; }
        //Complete Service Order
        public string SOMeter1Type { get; set; }
        [Display(Name = "spnMeter1CurrentReading|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression(@"^\d*\.?\d{0,1}$", ErrorMessage = "globalOneDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0.1, 99999999.9, ErrorMessage = "GT0.0Min1OneDecimalAfterEightDecimalBeforeTotalNineRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal SOMeter1CurrentReading { get; set; }
        public string SOMeter2Type { get; set; }
        [Display(Name = "spnMeter2CurrentReading|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression(@"^\d*\.?\d{0,1}$", ErrorMessage = "globalOneDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0.1, 99999999.9, ErrorMessage = "GT0.0Min1OneDecimalAfterEightDecimalBeforeTotalNineRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal SOMeter2CurrentReading { get; set; }
        [Display(Name = "spnVoid|" + LocalizeResourceSetConstants.Global)]
        public bool Meter1Void { get; set; }
        [Display(Name = "spnVoid|" + LocalizeResourceSetConstants.Global)]
        public bool Meter2Void { get; set; }
        [Display(Name = "spnDate|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "vaildDate|" + LocalizeResourceSetConstants.Global)]
        public string CurrentReadingDate { get; set; }
        [Required(ErrorMessage = "vaildTime|" + LocalizeResourceSetConstants.FleetServiceOrder)]
        [RegularExpression(@"^([0-9]|0[0-9]|1[0-9]|2[0-3]):([0-9]|[0-5][0-9]) (am|pm|AM|PM)$", ErrorMessage = "globalTimeErrorMessage|" + LocalizeResourceSetConstants.Global)]
        public string CurrentReadingTime { get; set; }
        public string MetersAssociated { get; set; }
        public double Meter1DayDiff { get; set; }
        public double Meter2DayDiff { get; set; }
        public DateTime? Meter1CurrentReadingDate { get; set; }
        public DateTime? Meter2CurrentReadingDate { get; set; }
        public string Meter1Units { get; set; }
        public string Meter2Units { get; set; }
        //Complete Service Order
       
    }
}