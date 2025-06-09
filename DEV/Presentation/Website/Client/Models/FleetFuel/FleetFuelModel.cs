using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.FleetFuel
{
    public class FleetFuelModel
    {
        [Display(Name = "spnGlobalEquipmentId|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "SelectEquipmentIDErrorMessage|" + LocalizeResourceSetConstants.Global)]
        [StringLength(31, ErrorMessage = "EquipmentStrLenErrorMessage|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression("^[A-Z0-9\\%\\-\\:\\/\\$\\*\\+\\.]+$", ErrorMessage = "EquipmentIDRegErrMsg|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string ClientLookupId { get; set; }
        [Display(Name = "spnDate|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "DateDownErrorMessage|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string MtrCurrentReadingDate { get; set; }
        [Required(ErrorMessage = "TimeDownErrorMessage|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression(@"^([0-9]|0[0-9]|1[0-9]|2[0-3]):([0-9]|[0-5][0-9]) (am|pm|AM|PM)$", ErrorMessage = "globalTimeErrorMessage|" + LocalizeResourceSetConstants.Global)]
        public string StartTimeValue { get; set; }
        [Display(Name = "spnMeter1CurrentReading|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression(@"^\d*\.?\d{0,1}$", ErrorMessage = "globalOneDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0.1, 99999999.9, ErrorMessage = "GT0.0Min1OneDecimalAfterEightDecimalBeforeTotalNineRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal Reading { get; set; }
        [Display(Name = "spnVoidMeter|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public bool Void { get; set; }
        [Display(Name = "spnFuelAmount|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [Required(ErrorMessage = "fieldErrorMessage|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression(@"^\d*\.?\d{0,3}$", ErrorMessage = "globalThreeDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0.001, 99999.999, ErrorMessage = "GT0.0ThreeDecimalAfterFiveDecimalBeforeTotalEightRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal FuelAmount { get; set; }
        [Display(Name = "spnUnitCost|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression(@"^\d*\.?\d{0,3}$", ErrorMessage = "globalThreeDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0.001, 99999.999, ErrorMessage = "GT0.0ThreeDecimalAfterFiveDecimalBeforeTotalEightRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal UnitCost { get; set; }
        [Display(Name = "spnFuelType|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string FuelType { get; set; }

        public long EquipId { get; set; }
        public int FuelTrackingId { get; set; }
        [Display(Name = "Fuel Unit")]
        public string FuelUnit { get; set; }
        public DateTime? ReadingDate { get; set; }
        public decimal PrevMeter1Reading { get; set; }
        [Display(Name = "Odometer Reading")]
        public decimal Meter1CurrentReading { get; set; }
        [Display(Name = "Date")]
        public DateTime? Meter1CurrentReadingDate { get; set; }
        public string EquipmentID { get; set; }
        public string Mtr1CurrentReadingDate { get; set; }
        public string Meter1Units { get; set; }
        #region from FleetMeter
      
        public long FleetMeterReadingId { get; set; }
        #endregion
        public IEnumerable<SelectListItem> DateRangeDropListForFFReadingdate { get; set; }
        [DefaultValue("Add")]
        public string Pagetype { get; set; }
        public decimal FltMrtReading { get; set; }
    }
}