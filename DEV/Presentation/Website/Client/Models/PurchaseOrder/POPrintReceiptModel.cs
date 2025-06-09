using Common.Constants;
using DataContracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace Client.Models.PurchaseOrder
{
    public class POPrintReceiptModel
    {
        public Int64 POReceiptHeaderId { get; set; }
        public Int64 PurchaseOrderId { get; set; }
        public string VendorName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string AddressCity { get; set; }
        public string AddressState { get; set; }
        public string AddressPostCode { get; set; }
        public string AddressCountry { get; set; }
        public string ClientLookupId { get; set; }
        public int ReceiptNumber { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? ReceiveDate { get; set; }
        public string Comments { get; set; }
        public string PersonnelName { get; set; }
        public int LineNumber { get; set; }
        public string PartClientLookUpId { get; set; }
        public string Description { get; set; }
        public string ManufacturerId { get; set; }
        public string Location { get; set; }
        public decimal QuantityReceived { get; set; }
        public string UnitofMeasure { get; set; }
        public decimal UnitCost { get; set; }
        public decimal Total { get; set; } 
        public UserData userData { get; set; }
        public string AzureImageURL { get; set; }
        public List<POPrintReceiptModel> poLineItemList { get; set; }

       //V2-560
        public string AccountClientLookupId { get; set; }


    }
}