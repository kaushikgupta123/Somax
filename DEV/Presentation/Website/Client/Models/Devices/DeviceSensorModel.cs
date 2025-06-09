using Client.CustomValidation;
using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.Devices
{
    public class DeviceSensorModel
    {
        public long IoTDeviceId { get; set; }
        public long SiteId { get; set; }
        [RequiredIf("IoTDeviceId", "0", ErrorMessage = "DeviceIdErrorMessage|" + LocalizeResourceSetConstants.DeviceDetails)]
        public string ClientLookupID { get; set; }
        [Required(ErrorMessage = "DeviceNameErrorMessage|" + LocalizeResourceSetConstants.DeviceDetails)]
        public string Name { get; set; }
        [RequiredIf("IoTDeviceId", "0", ErrorMessage = "TypeErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string SensorType { get; set; }
        public long? SensorAlertProcedureId { get; set; }
        public long? CriticalProcedureId { get; set; }
        [Required(ErrorMessage = "DeviceUnitErrorMessage|" + LocalizeResourceSetConstants.DeviceDetails)]
        public string SensorUnit { get; set; }
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalAfterTotalSeventeenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 999999999999999.99, ErrorMessage = "globalTwoDecimalAfterTotalSeventeenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? TriggerHigh { get; set; }
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalAfterTotalSeventeenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 999999999999999.99, ErrorMessage = "globalTwoDecimalAfterTotalSeventeenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? TriggerLow { get; set; }
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalAfterTotalSeventeenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 999999999999999.99, ErrorMessage = "globalTwoDecimalAfterTotalSeventeenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? TriggerHighCrit { get; set; }
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalAfterTotalSeventeenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 999999999999999.99, ErrorMessage = "globalTwoDecimalAfterTotalSeventeenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? TriggerLowCrit { get; set; }
        [RequiredIf("Mode", "Edit", ErrorMessage = "GlobalSelectEquipment|" + LocalizeResourceSetConstants.Global)]
        public long? EquipmentId { get; set; }
        public IEnumerable<SelectListItem> SensorUnitList { get; set; }
        public string Mode { get; set; }
        [RequiredIf("Mode", "Edit", ErrorMessage = "GlobalSelectEquipment|" + LocalizeResourceSetConstants.Global)]
        public string Equipment_ClientLookupId { get; set; }
        public string SensorAlertProcedure_ClientLookupId { get; set; }
        public string CriticalProcedure_ClientLookupId { get; set; }
        public IEnumerable<SelectListItem> SensorTypeList { get; set; }
        public int SensorId { get; set; }
    }
}