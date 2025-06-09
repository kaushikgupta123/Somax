using DataContracts;

namespace Client.Models.SensorAlert
{
    public class SensorAlertVM : LocalisationBaseVM
    {
        public SensorAlertModel sensorAlertModel { get; set; }
        public SensorAlertPrintModel sensorAlertPrintModel { get; set; }
        public SensorAlertTaskModel sensorAlertTaskModel { get; set; }
        public Security security { get; set; }
    }
}