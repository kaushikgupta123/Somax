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
    public class DeviceAPMOnlyModel
    {
        public long IoTDeviceId { get; set; }
        public long SiteId { get; set; }
        [RequiredIf("IoTDeviceId", "0", ErrorMessage = "DeviceIdErrorMessage|" + LocalizeResourceSetConstants.DeviceDetails)]
        public string ClientLookupID { get; set; }
        [Required(ErrorMessage = "DeviceNameErrorMessage|" + LocalizeResourceSetConstants.DeviceDetails)]
        public string Name { get; set; }
        [RequiredIf("IoTDeviceId", "0", ErrorMessage = "DeviceLastReadingErrorMessage|" + LocalizeResourceSetConstants.DeviceDetails)]
        [Range(0, 999999999999999.99, ErrorMessage = "globalTwoDecimalAfterTotalSeventeenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? LastReading { get; set; }
        [RequiredIf("IoTDeviceId", "0", ErrorMessage = "DeviceLastReadingDateErrorMessage|" + LocalizeResourceSetConstants.DeviceDetails)]
        public DateTime? LastReadingDate { get; set; }
        [Required(ErrorMessage = "DeviceIntervalErrorMessage|" + LocalizeResourceSetConstants.DeviceDetails)]
        [RegularExpression(@"^\d*\.?\d{0,3}$", ErrorMessage = "globalThreeDecimalAfterTotalSeventeenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 99999999999999.999, ErrorMessage = "globalThreeDecimalAfterTotalSeventeenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? MeterInterval { get; set; }
        [Required(ErrorMessage = "DeviceLifetimeReadingErrorMessage|" + LocalizeResourceSetConstants.DeviceDetails)]
        [RegularExpression(@"^\d*\.?\d{0,3}$", ErrorMessage = "globalThreeDecimalAfterTotalSeventeenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 99999999999999.999, ErrorMessage = "globalThreeDecimalAfterTotalSeventeenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? MeterReadingLife { get; set; }
        [Required(ErrorMessage = "DeviceMaximumReadingErrorMessage|" + LocalizeResourceSetConstants.DeviceDetails)]
        [RegularExpression(@"^\d*\.?\d{0,3}$", ErrorMessage = "globalThreeDecimalAfterTotalSeventeenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 99999999999999.999, ErrorMessage = "globalThreeDecimalAfterTotalSeventeenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? MeterReadingMax { get; set; }
        [Required(ErrorMessage = "GlobalSelectEquipment|" + LocalizeResourceSetConstants.Global)]
        public long EquipmentId { get; set; }
        [Required(ErrorMessage = "DeviceWarningProcedureErrorMessage|" + LocalizeResourceSetConstants.DeviceDetails)]
        public long SensorAlertProcedureId { get; set; }
        [Required(ErrorMessage = "DeviceUnitErrorMessage|" + LocalizeResourceSetConstants.DeviceDetails)]
        public string SensorUnit { get; set; }
        public IEnumerable<SelectListItem> SensorUnitList { get; set; }
        public string Mode { get; set; }
        [Required(ErrorMessage = "GlobalSelectEquipment|" + LocalizeResourceSetConstants.Global)]
        public string Equipment_ClientLookupId { get; set; }
        [Required(ErrorMessage = "DeviceWarningProcedureErrorMessage|" + LocalizeResourceSetConstants.DeviceDetails)]
        public string SensorAlertProcedure_ClientLookupId { get; set; }
        public int SensorId { get; set; }
    }
}