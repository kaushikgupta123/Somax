using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.PurchaseRequest
{
    public class ConvertToPOModel
    {
        public long PurchaseRequestId { get; set; }
        public string ClientLookupId { get; set; }
        public int CountLineItem { get; set; }
        public string Reason { get; set; }
        public string Creator_PersonnelName { get; set; }
        public string Approved_PersonnelName { get; set; }
        public string Message { get; set; }
        public long VendorId { get; set; }
        public bool VendorIsExternal { get; set; }
        public string Status { get; set; }
    }
}