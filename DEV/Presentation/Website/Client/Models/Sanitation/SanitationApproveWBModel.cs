using System;

namespace Client.Models.Sanitation
{
    public class SanitationApproveWBModel
    {
        public string SanitationJobId { get; set; }
        public string workassignedval { get; set; }
        public string workassignedtext { get; set; }
        public string shiftval { get; set; }
        public string shifttext { get; set; }
        public string scheduledate { get; set; }
        public String duration { get; set; }
    }
}