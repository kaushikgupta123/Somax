using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.EventInfo
{
    public class EventOnDemandModel
    {
        [Required(ErrorMessage = "ValidationPersonnelID|" + LocalizeResourceSetConstants.EventInfo)]
        public long? PersonnelId { get; set; }
        public IEnumerable<SelectListItem> PersonnelIdList { get; set; }
        [Required(ErrorMessage = "ValidationType|" + LocalizeResourceSetConstants.EventInfo)]
        public string Type { get; set; }
        public IEnumerable<SelectListItem> TypeList { get; set; }
        public string ChargeType { get; set; }
        public IEnumerable<SelectListItem> ChargeTypeList { get; set; }
        public long? ChargeTo { get; set; }
        public IEnumerable<SelectListItem> ChargeToList { get; set; }
        [Required(ErrorMessage = "ValidationChargeToID|" + LocalizeResourceSetConstants.EventInfo)]
        public string ChargeToClientLookupId { get; set; }
        [Required(ErrorMessage = "ValidationDate|" + LocalizeResourceSetConstants.EventInfo)]
        public DateTime? Date { get; set; }
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(1, 99999999.99, ErrorMessage = "globalMin1TwoDecimalAfterTotalTenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "ValidationHours|" + LocalizeResourceSetConstants.EventInfo)]
        public decimal? Hours { get; set; }
        public long EventInfoId { get; set; }
        [Required(ErrorMessage = "ValidationOnDemandID|" + LocalizeResourceSetConstants.EventInfo)]
        public string OnDemandID { get; set; }
        public IEnumerable<SelectListItem> OnDemandIDList { get; set; }
    }
}