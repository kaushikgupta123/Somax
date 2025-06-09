using System;

namespace Client.Models.MasterSanitationSchedule
{
    public class SanitationJobDetailsModel
    {
        public long? SanitationMasterId { get; set; }
        public long? SanitationJobId { get; set; }
        public string ClientLookupId { get; set; }
        public string Status { get; set; }
        public bool IsCompleteButtonShow { get; set; }
        public bool ExternalSanitation { get; set; }
        public string Shift { get; set; }
        public string ShiftDesc { get; set; }
        public bool DownRequired { get; set; }
        public string Description { get; set; }
        public long? AssignedTo_PersonnelId { get; set; }  //Assigned = ""  //Assigned_PersonnelClientLookupId = null
        public string Assigned { get; set; }
        public string CreateBy_Name { get; set; } //No Match Property 
        public string CreateBy { get; set; }
        public long? ChargeToId { get; set; }
        public string ChargeToId_string { get; set; }
        public string PlantLocationDescription { get; set; }
        public string ChargeTo_ClientLookupId { get; set; }
        public string ChargeType { get; set; }
        public long? PlantLocationId { get; set; }
        public string ChargeTo_Name { get; set; }
        public decimal? ScheduledDuration { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public DateTime? CompleteDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CompleteBy { get; set; } //No Match Property 

      
        public decimal? ActualDuration { get; set; }

        public string CompleteComments { get; set; }
        public string AzureImageURL { get; set; }

        public string VerificationStatus { get; set; }
        public DateTime? VerificationDate { get; set; }
        public string VerificationBy { get; set; }
        public string VerificationComments { get; set; }
        public string VerificationReason { get; set; }
        public bool VerificationCommentsVisible { get; set; }
        public bool VerificationReasonVisible { get; set; }
        public string Status_Display { get; set; }
    }
}