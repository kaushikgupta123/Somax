using System.Collections.Generic;

namespace Client.Models.PurchaseRequest
{
    public class PurchaseRequestPDFPrintModel: PurchaseRequestPrintModel
    {
        public PurchaseRequestPDFPrintModel()
        {
            LineItemModelList = new List<LineItemModel>();
        }
        public List<LineItemModel> LineItemModelList { get; set; }
        public string CreateDateString { get; set; }
        public string ProcessedDateString { get; set; }
        public decimal Total { get; set; }
    }
}