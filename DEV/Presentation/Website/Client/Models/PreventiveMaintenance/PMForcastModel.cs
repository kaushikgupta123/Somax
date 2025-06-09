using Client.CustomValidation;
using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.PreventiveMaintenance
{
    public class PMForcastModel
    {
        public long SiteId { get; set; }
        public string ClientLookupId { get; set; }
        public string Description { get; set; }
        public decimal EstimatedTotalCosts { get; set; }
        public decimal jobDuration { get; set; }
        public decimal Duration { get; set; }
        public decimal EstLaborHours { get; set; }
        public decimal EstLaborCost { get; set; }
        public decimal EstMaterialCost { get; set; }
        public decimal EstOtherCost { get; set; }
        public string AssignedTo_PersonnelClientLookupId { get; set; }
        public string ChargeToName { get; set; }
        public string ChargeToClientLookupId { get; set; }
        public DateTime? SchedueledDate { get; set; }
        public DateTime CurrentDate { get; set; }
        [Display(Name = "spnGenerateThrough|" + LocalizeResourceSetConstants.Global)]
        public DateTime ForecastDate { get; set; }
        public int PMForeCastId { get; set; }
        public string ScheduleType { get; set; }
        public IEnumerable<SelectListItem> ScheduleTypeList { get; set; }      
        [RequiredIf("ScheduleType", "OnDemand", ErrorMessage = "spnOnDemandGrouprequired|" + LocalizeResourceSetConstants.PrevMaintDetails)]
        public string OnDemandGroup { get; set; }
        public IEnumerable<SelectListItem> OnDemandGroupList { get; set; }
        public long PrevMaintSchedId { get; set; }
        public int ChildCount { get; set; }
        public IEnumerable<SelectListItem> PersonnelIdList { get; set; }
        public string Assigned { get; set; }
        public DateTime? RequiredDate { get; set; }
        public string AssignedMultiple { get; set; }
        #region V2-1082
        public bool? DownRequired { get; set; }
        public string Shift { get; set; }
        public IEnumerable<SelectListItem> ShiftList { get; set; }
        public IEnumerable<SelectListItem> DownRequiredInactiveFlagList { get; set; }
        #endregion
        public string Type { get; set; }//V2-1207
    }
}