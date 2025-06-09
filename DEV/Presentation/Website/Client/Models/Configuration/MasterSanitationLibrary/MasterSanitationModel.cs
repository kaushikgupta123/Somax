using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace Client.Models.Configuration.MasterSanitationLibrary
{
    public class MasterSanitationModel
    {
        public long MasterSanLibraryId { get; set; }
        [Required(ErrorMessage = "spnSanitationMasterIDErrorMsg|" + LocalizeResourceSetConstants.LibraryDetails)]      
        public string ClientLookUpId { get; set; }
        public string Description { get; set; }
        [Range(0, 999999999, ErrorMessage = "spnFrequencyLengthExceedNine|" + LocalizeResourceSetConstants.LibraryDetails)]
        public int? Frequency { get; set; }
        public string FrequencyType { get; set; }
        public decimal? JobDuration { get; set; }
        public string ScheduleMethod { get; set; }
        public string ScheduleType { get; set; }
        public bool InactiveFlag { get; set; }
        public DateTime? CreateDate { get; set; }
        public long MasterIdForCancel { get; set; }
        public IEnumerable<SelectListItem> ScheduleTypeList { get; set; }
        public IEnumerable<SelectListItem> ScheduleMethodList { get; set; }
        public IEnumerable<SelectListItem> FrequencyTypeList { get; set; }
    }
}