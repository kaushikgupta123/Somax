namespace Client.Models
{
    public class Sensor
    {

        public string ClientId { get; set; }  //found
        public string Equipment_Sensor_XrefId { get; set; } //found
        public string EquipmentId { get; set; } //found
        public string SensorId { get; set; }  //found
        public string SensorName { get; set; }  //found
        public string SensorAppId { get; set; }  //
        public string SensorAlertProcedureId { get; set; }  //
        public string AssignedTo_PersonnelId { get; set; }  //
        public string WorkOrderId { get; set; }  //
        public string TriggerHigh { get; set; }  //
        public string TriggerLow { get; set; }  //
        public string LastReading { get; set; } //found
        public string EquipmentClientLookupId { get; set; }
        public string EquipmentName { get; set; }

    }
   
}