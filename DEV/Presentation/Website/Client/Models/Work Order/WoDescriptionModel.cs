using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.Work_Order
{
    public class WoDescriptionModel
    {
        public string ClientLookupId { get; set; }
        public string MaintOnDemandClientLookUpId { get; set; }
        public string Type { get; set; }
        [Display(Name = "spnChargeType|" + LocalizeResourceSetConstants.Global)]
        public string ChargeType { get; set; }
        public string ChargeToClientLookupId { get; set; }
        //[Required(ErrorMessage = "Please enter Description")]
        [Required(ErrorMessage = "validationDescription|" + LocalizeResourceSetConstants.Global)]
        public string Description { get; set; }
        public IEnumerable<SelectListItem> MaintOnDemandList { get; set; }
        public IEnumerable<SelectListItem> TypeList { get; set; }
        public IEnumerable<SelectListItem> ChargeTypeList { get; set; }
        public IEnumerable<SelectListItem> ChargeTypelookUpList { get; set; }
        [Display(Name = "spnDownRequired|" + LocalizeResourceSetConstants.Global)]
        public bool DownRequired { get; set; }
        [Display(Name = "spnPriority|" + LocalizeResourceSetConstants.Global)]
        public string Priority { get; set; }
        public IEnumerable<SelectListItem> PriorityList { get; set; }
        [Display(Name = "spnRequired|" + LocalizeResourceSetConstants.Global)]
        public DateTime? RequiredDate { get; set; }
    }
}