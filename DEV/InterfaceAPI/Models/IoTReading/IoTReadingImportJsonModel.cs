using System;
using System.Collections.Generic;

namespace InterfaceAPI.Models.IoTReading
{
    public class IoTReadingImportJsonModel
    {
        public IoTReadingImportJsonModel()
        {
            IoTDevice = "";
            Reading = "";
            ReadingDate = DateTime.MinValue;
            ReadingUnit = "";
        }
        public string IoTDevice { get; set; }
        public string Reading { get; set; }
        public DateTime? ReadingDate { get; set; }
        public string ReadingUnit { get; set; }
    }
}