using System.Collections.Generic;

namespace Client.Models.InventoryReceipt
{
    public class ReceiptVM: LocalisationBaseVM
    {
       public ReceiptModel receiptModel { get; set; }
        public ErrorModel errorModel { get; set; }
    }
    public class ErrorModel
    {
        public int position { get; set; }
        public string Errormsg { get; set; }
        public long PartId { get; set; }
        public List<successListId>  SuccListId{ get; set; }
    }
    public class successListId
    {
        public int PostionToRemove { get; set; }
        public long? SuccpartId { get; set; }
    }
}