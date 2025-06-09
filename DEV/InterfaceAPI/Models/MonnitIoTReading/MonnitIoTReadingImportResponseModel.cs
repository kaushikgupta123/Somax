using System.Collections.Generic;

namespace InterfaceAPI.Models.MonnitIoTReading
{
    public class MonnitIoTReadingImportResponseModel
    {
        public MonnitIoTReadingImportResponseModel()
        {
            errMessageList = new List<MonnitIoTReadingErrorResponseModel>();
        }
        public string fileName { get; set; }
        public string status { get; set; }
        public string errMessage { get; set; }
        public List<MonnitIoTReadingErrorResponseModel> errMessageList { get; set; }
        public List<MonnitIoTReadingImportResponseModel> MonnitIoTReadingImportResponseModelList { get; set; }
    }
}