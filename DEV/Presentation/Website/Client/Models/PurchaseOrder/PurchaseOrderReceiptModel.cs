using Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace Client.Models.PurchaseOrder
{
    public class PurchaseOrderReceiptModel
    {
        public long PurchaseOrderId { get; set; }      
        public string ClientLookupId { get; set; }
        public string Description { get; set; }
        public string Location1_5 { get; set; }
        public string Location1_1 { get; set; }
        public string Location1_2 { get; set; }
        public string Location1_3 { get; set; }
        public string Location1_4 { get; set; }
        public string Minimum { get; set; }
        public string Maximum { get; set; }
        public string Manufacturer { get; set; }
        public string Carrier { get; set; }
        public string Comments { get; set; }       
        [RegularExpression(@"^-?(?=.*[1-9])\d+(\.\d+)?$", ErrorMessage = "spnPositiveNum|" + LocalizeResourceSetConstants.PurchaseOrder)]      
        public string FreightAmount { get; set; }
        public string FreightBill { get; set; }
        public string PackingSlip { get; set; }
       public int NoOfItems { get; set; }
        public int ItemsIssued { get; set; }
        public bool PrintReceiptCheck { get; set; }
        public int ItemsReceived { get; set; }
        public long POReceiptHeaderId { get; set; }
        public string UOM { get; set; } //V2-965
        public string ManufacturerId { get; set; } //V2-998
    }
}