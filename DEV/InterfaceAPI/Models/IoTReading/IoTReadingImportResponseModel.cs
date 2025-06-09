using System.Collections.Generic;

namespace InterfaceAPI.Models.IoTReading
{
    public class IoTReadingImportResponseModel
    {
        public IoTReadingImportResponseModel()
        {
            errMessageList = new List<IoTReadingErrorResponseModel>();
        }
        public string fileName { get; set; }
        public string status { get; set; }
        public string errMessage { get; set; }
        public List<IoTReadingErrorResponseModel> errMessageList { get; set; }
        public List<IoTReadingImportResponseModel> IoTReadingImportResponseModelList { get; set; }
    }
}