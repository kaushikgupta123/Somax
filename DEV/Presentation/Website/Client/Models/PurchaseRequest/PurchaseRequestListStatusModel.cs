using System.Collections.Generic;

namespace Client.Models.PurchaseRequest
{
    public class PurchaseRequestListStatusModel
    {
        public long PurchaseRequestId { get; set; }
        public string ClientLookupId { get; set; }
        public string Status { get; set; }
        public int ChildCount { get; set; }
        public long VendorId { get; set; }
        public bool VendorIsExternal { get; set; }
    }

    public class PurchaseRequestStatusModel
    {
        public PurchaseRequestStatusModel()
        {
            list = new List<PurchaseRequestListStatusModel>();
        }
        public List<PurchaseRequestListStatusModel> list { get; set; }
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