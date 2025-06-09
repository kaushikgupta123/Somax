using Client.CustomValidation;
using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.MasterSanitationSchedule
{
    public class MasterSanitationScheduleModel
    {
        public long ClientId { get; set; }
        public long SanitationMasterId { get; set; }
        public long SiteId { get; set; }
        public long AreaId { get; set; }
        public long DepartmentId { get; set; }
        public long StoreroomId { get; set; }
        public long? Assignto_PersonnelId { get; set; }
        public string ChargeType { get; set; }
        public long ChargeToId { get; set; }
        [Required(ErrorMessage = "validationChargeTo|" + LocalizeResourceSetConstants.Global)]
        public string ChargeToDescription { get; set; }
        public string ChargeTo_ClientLookupId { get; set; }
        [Required(ErrorMessage = "spnReqDescription|" + LocalizeResourceSetConstants.SanitationDetails)]
        public string Description { get; set; }
        public string ExclusionDays { get; set; }
        public string[] ExclusionDaysString { get; set; }
        [Range(0, Int32.MaxValue, ErrorMessage = "validationIntegerValue|" + LocalizeResourceSetConstants.SanitationDetails)]
        public int? Frequency { get; set; }
        public DateTime? LastScheduled { get; set; }
        [Required(ErrorMessage = "validationNextDue|" + LocalizeResourceSetConstants.SanitationDetails)]
        public DateTime? NextDue { get; set; }

        [Required(ErrorMessage = "validationScheduleType|" + LocalizeResourceSetConstants.SanitationDetails)]
        public string ScheduledType { get; set; }

        [RequiredIf("ScheduledType", "OnDemand", ErrorMessage = "validationOnDemandGroup|" + LocalizeResourceSetConstants.SanitationDetails)]
        public string OnDemandGroup { get; set; }
        public decimal? ScheduledDuration { get; set; }     
       
        public string Shift { get; set; }
        public bool InactiveFlag { get; set; }
        public int UpdateIndex { get; set; }
        private bool m_validateClientLookupId;
        public string ChargeToClientLookupId { get; set; }       
        public string ChargeToName { get; set; }
        public string Assigned { get; set; }
        public long PlantLocationId { get; set; }

        public bool PlantLocation { get; set; }
        public IEnumerable<SelectListItem> ShiftList { get; set; }
        public IEnumerable<SelectListItem> ScheduleTypeList { get; set; }
        public IEnumerable<SelectListItem> PersonnelList { get; set; }
        public IEnumerable<SelectListItem> OnDemandGroupList { get; set; }
        public IEnumerable<SelectListItem> DaysOfWeekList { get; set; }

        public bool Sunday { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
    }
}
