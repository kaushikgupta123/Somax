namespace Client.Models.PartsManagement.PartsManagementRequest
{
    public class PartsManagementRequestPrintModel
    {
        public long PartMasterRequestId { get; set; }
        public string Requester { get; set; }
        public string Justification { get; set; }
        public string RequestType { get; set; }
        public string Status_Display { get; set; }
        public string Manufacturer { get; set; }
        public string ManufacturerId { get; set; }

    }
} 