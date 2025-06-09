using System;
using System.Collections.Generic;

namespace InterfaceAPI.Models.MonnitIoTReading
{
    public class MonnitIoTReadingImportJsonModel
    {
        public MonnitIoTReadingImportJsonModel()
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
            rawData = "";
            datatype = "";
            dataValue = "";
            plotValues = "";
            plotLabels = "";
            batteryLevel = 0;
            signalStrength = 0;
            pendingChange = "";
            voltage = "";

        }
        public long ClientId { get; set; }
        public long SiteId { get; set; }
        public string sensorID { get; set; }
        public string sensorName { get; set; }
        public string applicationID { get; set; }
        public string networkID { get; set; }
        public string dataMessageGUID { get; set; }
        public int? state { get; set; }
        public DateTime messageDate { get; set; }
        public string rawData { get; set; }
        public string datatype { get; set; }
        public string dataValue { get; set; }
        public string plotValues { get; set; }
        public string plotLabels { get; set; }
        public int? batteryLevel { get; set; }
        public int? signalStrength { get; set; }
        public string pendingChange { get; set; }
        public string voltage { get; set; }
    }
}