using System.Collections.Generic;

namespace Client.Models.PurchaseOrder
{
    public class PurchaseOrderPDFPrintModel : POPrintModel
    {
        public PurchaseOrderPDFPrintModel()
        {
            LineItemModelList = new List<POLineItemModel>();
        }
        public List<POLineItemModel> LineItemModelList { get; set; }
        public string CreateDateString { get; set; }
        public string CompleteDateString { get; set; }
        public decimal Total { get; set; }
        public string RequiredDateString { get; set; } //V2-1171
    }
}