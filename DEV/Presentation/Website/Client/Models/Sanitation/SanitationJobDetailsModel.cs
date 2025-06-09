using Common.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace Client.Models.Sanitation
{
    public class SanitationJobDetailsModel
    {
        public long? SanitationJobId { get; set; }
        public string ClientLookupId { get; set; }
        public string Status { get; set; }
        public bool IsCompleteButtonShow { get; set; }
      
        public bool ExternalSanitation { get; set; }
        public string Shift { get; set; }
        public string ShiftDesc { get; set; }
        public bool DownRequired { get; set; }
       
        [Required(ErrorMessage = "spnReqDescription|" + LocalizeResourceSetConstants.SanitationDetails)]
        public string Description { get; set; }
        public long? AssignedTo_PersonnelId { get; set; }  //Assigned = ""  //Assigned_PersonnelClientLookupId = null
        public string Assigned { get; set; }
        public string CreateBy_Name { get; set; } //No Match Property 
        public string CreateBy{ get; set; }
        public long? ChargeToId { get; set; }
        public string ChargeToId_string { get; set; }

       
        [Required(ErrorMessage = "spnReqPlantLocation|" + LocalizeResourceSetConstants.SanitationDetails)]
        public string PlantLocationDescription { get; set; }
        public string ChargeTo_ClientLookupId { get; set; }
        public string ChargeType { get; set; }
        public long? PlantLocationId { get; set; }
        public string ChargeTo_Name { get; set; }
        public string AssetLocation { get; set; }

        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 999999.99, ErrorMessage = "globalTwoDecimalAfterTotalEightRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? ScheduledDuration { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public DateTime? CompleteDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CompleteBy { get; set; } //No Match Property 

        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 999999.99, ErrorMessage = "globalTwoDecimalAfterTotalEightRegErrMsg|" + LocalizeResourceSetConstants.Global)]
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

        public bool PlantLocation { get; set; }

        public string SourceType { get; set; }

        public string SourceIDClientLookUpId { get; set; }

        public long? SanitationMasterId { get; set; }

        public string PassBy { get; set; }

        public string FailBy { get; set; }

        public DateTime? PassDate { get; set; }

        public DateTime? FailDate { get; set; }

        public string FailReason { get; set; }

        public bool ClientOnPremise { get; set; }
        public string FailComment { get; set; } //V2-827
    }
}