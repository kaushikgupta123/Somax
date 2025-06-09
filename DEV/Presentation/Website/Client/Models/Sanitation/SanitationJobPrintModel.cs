using System;

namespace Client.Models.Sanitation
{
    public class SanitationJobPrintModel
    {
        public string ClientLookupId { get; set; }
        public string Description { get; set; }
        public string ChargeTo_ClientLookupId { get; set; }
        public string ChargeTo_Name { get; set; }
        public string AssetLocation { get; set; }
        public string Status { get; set; }
        public string Shift { get; set; }
        public string AssetGroup1_ClientLookUpId { get; set; }
        public string AssetGroup2_ClientLookUpId { get; set; }
        public string AssetGroup3_ClientLookUpId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string Assigned { get; set; }
        public DateTime? CompleteDate { get; set; }
        public string VerifiedBy { get; set; }
        public DateTime? VerifiedDate { get; set; }
        public bool Extracted { get; set; }
        public DateTime? ScheduledDate { get; set; }
       
    }
}