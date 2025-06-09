using System;

namespace Client.Models.Work_Order
{
    public class ApproveWorkOrderModel
    {
        public string woid { get; set; }
        public string workassignedval { get; set; }
        public string workassignedtext { get; set; }
        public string shiftval { get; set; }
        public string shifttext { get; set; }
        public string scheduledate { get; set; }
        public String duration { get; set; }
        public string WorkOrderIds { get; set; }
        public string ClientLookupIds { get; set; }
    }
}