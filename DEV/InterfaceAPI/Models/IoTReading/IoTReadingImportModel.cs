using System;

namespace InterfaceAPI.Models.IoTReading
{
    public class IoTReadingImportModel
    {
        public IoTReadingImportModel()
        {
            ClientId = 0;
            SiteId = 0;
            IoTDevice = "";
            Reading = 0;
            ReadingDate = DateTime.MinValue;
            ReadingUnit = "";
        }
        public long ClientId { get; set; }
        public long SiteId { get; set; }
        public string IoTDevice { get; set; }
        public decimal Reading { get; set; }
        public DateTime? ReadingDate { get; set; }
        public string ReadingUnit { get; set; }
    }
}