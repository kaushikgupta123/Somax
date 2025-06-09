using Client.CustomValidation;
using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.Project
{
    public class ProjectAddOrEditModel
    {
        public long ProjectId { get; set; }

        [StringLength(15, ErrorMessage = "ProjectIdMustHaveMaxLenOf15ErrMsg|" + LocalizeResourceSetConstants.Project)]
        [RegularExpression("^[A-Z0-9\\%\\-\\:\\/\\$\\*\\+\\.]+$", ErrorMessage = "ProjectIdContainsInvalidCharsErrMsg|" + LocalizeResourceSetConstants.Project)]
        [Required(ErrorMessage = "ValidationProjectIdReqErrMsg|" + LocalizeResourceSetConstants.Project)]
        public string ClientLookupId { get; set; }
        [Required(ErrorMessage = "validationDescription|" + LocalizeResourceSetConstants.Global)]
        public string Description { get; set; }
        [RequiredIfValueExist("ScheduleFinish", ErrorMessage = "SelectScheduledStartforScheduledFinishErrMsg|" + LocalizeResourceSetConstants.Project)]
        public DateTime? ScheduleStart { get; set; }
        [DateGreaterThanAttribute(otherPropertyName = "ScheduleStart", ErrorMessage = "ScheduledFinishMustBeGreaterThanScheduledStartErrMsg|" + LocalizeResourceSetConstants.Project)]
        public DateTime? ScheduleFinish { get; set; }
        public long? Owner_PersonnelId { get; set; }
        public long? Coordinator_PersonnelId { get; set; }
        public int? FiscalYear { get; set; }
        [RegularExpression(@"^\d*\.?\d{0,3}$", ErrorMessage = "globalThreeDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 999999999999.999, ErrorMessage = "globalThreeDecimalAfterTwelveDecimalBeforeTotalFifteenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? Budget { get; set; }
        public string Type { get; set; }
        public string PageType { get; set; }
        public IEnumerable<SelectListItem> OwnerPersonnelList { get; set; }
        public IEnumerable<SelectListItem> CoorPersonnelList { get; set; }
        public IEnumerable<SelectListItem> TypeList { get; set; }
        [DateGreaterThanAttribute(otherPropertyName = "ActualStart", ErrorMessage = "ActualFinishMustBeGreaterThanActualStartErrMsg|" + LocalizeResourceSetConstants.Project)]
        public DateTime? ActualFinish { get; set; }
        [RequiredIfValueExist("ActualFinish", ErrorMessage = "SelectActualStartforActualFinishErrMsg|" + LocalizeResourceSetConstants.Project)]
        public DateTime? ActualStart { get; set; }
    }
}