using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.PurchaseRequest
{
    public class PurchaseRequestListPrintModel
    {
        public long PurchaseRequestId { get; set; }
        public string ClientLookupId { get; set; }
        public string Status { get; set; }
        public int ChildCount { get; set; }
        public long VendorId { get; set; }
        public bool VendorIsExternal { get; set; }
    }

    public class PurchaseRequestPrntModel
    {
        public PurchaseRequestPrntModel()
        {
            list = new List<PurchaseRequestListPrintModel>();
        }
        public List<PurchaseRequestListPrintModel> list { get; set; }
        private string _comments;
        public string comments
        {
            get
            {
                return string.IsNullOrEmpty(_comments) ? "" : _comments;
            }
            set
            {
                _comments = value;
            }

        }

    }
}