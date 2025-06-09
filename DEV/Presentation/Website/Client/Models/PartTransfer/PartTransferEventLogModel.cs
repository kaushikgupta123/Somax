using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.PartTransfer
{
    public class PartTransferEventLogModel
    {
        public long PartTransferEventLogId { get; set; }
        public string Event { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Created { get; set; }
        public decimal Quantity { get; set; }
        public string Comments { get; set; }
    }
    public class PrintPartTransferModel
    {
        public string RequestPart_ClientLookupId { get; set; }
        public string RequestSite_Name { get; set; }
        public string RequestPart_Description { get; set; }
        public string RequesterName { get; set; }
        public string IssueSite_Name { get; set; }
        public string IssuePart_ClientLookupId { get; set; }
        public decimal? QtyOnHand { get; set; }
        public string IssuePart_Description { get; set; }
        public string ShipperName { get; set; }
        public long PartTransferID { get; set; }
        public string IssueSite_Address1 { get; set; }
        public string IssueSite_Address2 { get; set; }
        public string IssueSite_CityStateZip { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string RequestSite_Address1 { get; set; }
        public string RequestSite_Address2 { get; set; }
        public string RequestSite_CityStateZip { get; set; }

        public string Shipping_Account { get; set; }
        public string AzureImageURL { get; set; }
    }
}