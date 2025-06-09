using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace Client.Models.Configuration.PreventiveMaintenanceLibrary
{
    public class PreventiveMaintenanceLibraryModel
    {
        public PreventiveMaintenanceLibraryModel()
        {
            FrequencyTypeList = new List<SelectListItem>();
            TypeList = new List<SelectListItem>();
            ScheduleTypeList = new List<SelectListItem>();
            ScheduleMethodList = new List<SelectListItem>();
        } 
        public long PrevMaintLibraryId { get; set; }
        [Required(ErrorMessage = "spnEnterPMMasterID|" + LocalizeResourceSetConstants.PrevMaintDetails)]        
        public string ClientLookupId { get; set; }
        [Required(ErrorMessage = "validationDescription|" + LocalizeResourceSetConstants.Global)]
        public string Description { get; set; }
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 999999.99, ErrorMessage = "globalTwoDecimalAfterTotalEightRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? JobDuration { get; set; }
        public string FrequencyType { get; set; }
        public int? Frequency { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Type { get; set; }
        [Required(ErrorMessage = "validscheduletype|" + LocalizeResourceSetConstants.PrevMaintDetails)]
        public string ScheduleType { get; set; }
        public string ScheduleMethod { get; set; }
        public bool InactiveFlag { get; set; }
        public bool DownRequired { get; set; }
        public IEnumerable<SelectListItem> FrequencyTypeList { get; set; }
        public IEnumerable<SelectListItem> TypeList { get; set; }
        public IEnumerable<SelectListItem> ScheduleTypeList { get; set; }
        public IEnumerable<SelectListItem> ScheduleMethodList { get; set; }
    }
}