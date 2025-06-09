using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Client.Models.Sanitation
{
    public class SanitationJobSearchModel
    {
        #region Public Variables
        public string TextSearchItem { get; set; }
        public int CustomQueryDisplayId { get; set; }
        public long ClientId { get; set; }
        public long SanitationJobId { get; set; }
        public string ClientLookupId { get; set; }
        public string Description { get; set; }
        public string ChargeTo_ClientLookupId { get; set; }
        public string ChargeTo_Name { get; set; }
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
        public string ValidateType { get; set; }
        public string OnDemandGroup { get; set; }
        public string CompleteBy { get; set; }
        public string ShiftDescription { get; set; }
        public long PlantLocationId { get; set; }
        public string DisplayTextChecked { get; set; }
        public string Status_Display { get; set; }
        public string Prefix { get; set; }
        public long PersonnelId { get; set; }
        public int SanitationMasterCount { get; set; }
        public int SanitationJobCount { get; set; }
        public string SanitationJobList { get; set; }
        public string PersonnelIdList { get; set; }
        public DateTime? ScheduledDateFrom { get; set; }
        public DateTime? ScheduledDateTo { get; set; }
        public string Creator_PersonnelClientLookupId { get; set; }
        public string VerificationCompleteDate { get; set; }
        public long SanitationJobTaskId { get; set; } //
        public string CheckStatus { get; set; }
        public string CreateByName { get; set; }
        public string PassBy { get; set; }

        public string FailBy { get; set; }
        public bool Extracted { get; set; }
        public DateTime? ScheduledDate { get; set; }
        //[DataMember]
        public string ImageURI { get; set; }
        public bool ExternalSanitation { get; set; }
        public int TotalCount { get; set; }
        public string AssetLocation { get; set; }
        public IEnumerable<SelectListItem> TextSearchList { get; set; }
        public IEnumerable<SelectListItem> StatusList { get; set; }
        public IEnumerable<SelectListItem> ShiftList { get; set; }
        public IEnumerable<SelectListItem> CreateByList { get; set; }
        public IEnumerable<SelectListItem> AssignedList { get; set; }
        public IEnumerable<SelectListItem> VerifiedByList { get; set; }
        public IEnumerable<SelectListItem> SanitExtractedList { get; set; }
        public IEnumerable<SelectListItem> SanitationJobViewSearchList { get; set; }
        #endregion
        #region V2-910
        public bool ClientOnPremise { get; set; }
        public string AzureImageURL { get; set; }
        public DateTime? PassDate { get; set; }
        public DateTime? FailDate { get; set; }
        #endregion

    }
}