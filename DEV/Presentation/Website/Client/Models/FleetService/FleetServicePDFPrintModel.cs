using System.Collections.Generic;

namespace Client.Models.FleetService
{
    public class FleetServicePDFPrintModel: FleetServicePrintModel
    {
        public FleetServicePDFPrintModel()
        {
            LineItemModelList = new List<FleetServiceLineItemModel>();
        }
        public List<FleetServiceLineItemModel> LineItemModelList { get; set; }
        public decimal Total { get; set; }
        public string CreateDateString { get; set; }
        public string ScheduleDateString { get; set; }
        public string CompleteDateString { get; set; }
    }
}