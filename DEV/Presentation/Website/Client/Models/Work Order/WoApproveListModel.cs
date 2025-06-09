using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Work_Order
{
    public class WoApproveListModel
    {
        public long WorkOrderId { get; set; }
        public string ClientLookupId { get; set; }
        public string Status { get; set; }
    }
    public class WoApproveModel
    {
        public List<WoApproveListModel> list { get; set; }

      
    }
}