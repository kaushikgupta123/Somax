using System;

namespace Client.Models.PurchaseOrder
{
    public class InnerGridLineItemModel
    {
        public long ReceiptNumber { get; set; }
        public string ReceiveBy_PersonnelName { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public string strReceiveDate { get; set; }
        public decimal QuantityReceived { get; set; }
        public string Comments { get; set; }
        public bool Reversed { get; set; }
        public string ReversedComments { get; set; }
        public string ExReceiptNo { get; set; }
        public long ExReceiptTxnId { get; set; }

        public long POReceiptItemId { get; set; }
        public long POReceiptHeaderId { get; set; }
        public long PurchaseOrderLineItemId { get; set; }
        public long ReceiveBy_PersonnelID { get; set; }

        public bool IsPurchasingEdit { get; set; }
        public decimal UOMConversion { get; set; }

        public long ChargeToId { get; set; }
        public string ChargeType { get; set; }

        #region V2-878
        public decimal FreightAmount { get; set; }
        public string FreightBill { get; set; }
        public string PackingSlip { get; set; }
        #endregion
    }
}