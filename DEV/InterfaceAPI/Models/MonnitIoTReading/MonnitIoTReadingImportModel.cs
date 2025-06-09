using System;

namespace InterfaceAPI.Models.MonnitIoTReading
{
    public class MonnitIoTReadingImportModel
    {
        public MonnitIoTReadingImportModel()
        {
            ClientId = 0;
            SiteId = 0;
            sensorID = "";
            sensorName = "";
            applicationID = "";
            networkID = "";
            dataMessageGUID = "";
            state = 0;
            messageDate = DateTime.MinValue;
            rawData = 0;
            datatype = "";
            dataValue = 0;
            plotValues = 0;
            plotLabels = "";
            batteryLevel = 0;
            signalStrength = 0;
            pendingChange = "";
            voltage = 0;

        }
        public long ClientId { get; set; }
        public long SiteId { get; set; }
        public string sensorID { get; set; }
        public string sensorName { get; set; }
        public string applicationID { get; set; }
        public string networkID { get; set; }
        public string dataMessageGUID { get; set; }
        public int state { get; set; }
        public DateTime? messageDate { get; set; }
        public decimal rawData { get; set; }
        public string datatype { get; set; }
        public decimal dataValue { get; set; }
        public decimal plotValues { get; set; }
        public string plotLabels { get; set; }
        public int batteryLevel { get; set; }
        public int signalStrength { get; set; }
        public string pendingChange { get; set; }
        public decimal voltage { get; set; }
    }
}
