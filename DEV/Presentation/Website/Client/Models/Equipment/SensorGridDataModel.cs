using Client.CustomValidation;
using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models
{
    public class SensorGridDataModel
    {       
        public long EquipmentId { get; set; }
        public string SensorName { get; set; }
        public string Sensor { get; set; }
        public string AssignedTo_Name { get; set; }
        public string SensorAlertProcedureClientLookUpId { get; set; }
        public decimal LastReading { get; set; }
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 99999999999999999.99, ErrorMessage = "GlobalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [NotRequiredIfValueExists("TriggerHigh", ErrorMessage = "modelSensorTriggerCheck|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public decimal TriggerLow { get; set; }
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 99999999999999999.99, ErrorMessage = "GlobalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [NotRequiredIfValueExists("TriggerLow", ErrorMessage = "modelSensorTriggerCheck|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public decimal TriggerHigh { get; set; }
        public long SensorId { get; set; }
        public long Equipment_Sensor_XrefId { get; set; }
        public string SensorSecurityAdd { get; set; }
        public string SensorSecurityEdit { get; set; }
        public string SensorSecurityDelete { get; set; }
        public string SensorSecurityEquipment { get; set; }
        public string EquipmentClientLookupId { get; set; }
        [Required(ErrorMessage = "spnSensorTrigger|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public long SensorAlertProcedureId { get; set; }
        public long? AssignedTo_PersonnelId { get; set; }       
        public IEnumerable<SelectListItem> SensorPrecedureList { get; set; }
        public IEnumerable<SelectListItem> AssignedToPersonnelList { get; set; }
        #region V2-537
        public long IoTDeviceId { get; set; }
        public string ClientLookupId { get; set; }
        public string Name { get; set; }
        public string SensorType { get; set; }
        public string SensorUnit { get; set; }
        public DateTime? LastReadingDate { get; set; }
        public bool InactiveFlag { get; set; }
        public int TotalCount { get; set; }
        #endregion
    }
}