using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Common.Constants;
using Client.CustomValidation;

namespace Client.Models.MaterialRequest
{
    public class MaterialRequestModel
    {
        public long ClientId { get; set; }
        public long SiteId { get; set; }
        [Display(Name = "spnMaterialRequestId|" + LocalizeResourceSetConstants.MaterialRequest)]
        public long MaterialRequestId { get; set; }
        [Display(Name = "spnDescription|" + LocalizeResourceSetConstants.Global)]
        [MaxLength(200, ErrorMessage = "ValidationMaxLength200Description|" + LocalizeResourceSetConstants.MaterialRequest)]
        public string Description { get; set; }
        public long? AccountId { get; set; }
        [Display(Name = "spnCreated|" + LocalizeResourceSetConstants.Global)]
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        [Display(Name = "spnRequiredDate|" + LocalizeResourceSetConstants.Global)]
        [RequiredIf("Mode","Edit", ErrorMessage = "ValidateRequiredDate|" + LocalizeResourceSetConstants.Global)]
        public DateTime? RequiredDate { get; set; }
        [Display(Name = "spnGlobalCompleted|" + LocalizeResourceSetConstants.Global)]
        public DateTime? CompleteDate { get; set; }
        [Display(Name = "spnStatus|" + LocalizeResourceSetConstants.Global)]
        public string Status { get; internal set; }
        public Int32 ChildCount { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<SelectListItem> StatusList { get; set; }
        public IEnumerable<SelectListItem> AccountList { get; set; }
        [Display(Name = "spnAccount|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "validationAccountId|"+ LocalizeResourceSetConstants.Global)]
        public string Account_ClientLookupId { get; set; }
        public string Personnel_NameFirst { get; set; }
        public string Personnel_NameLast { get; set; }
        public IEnumerable<SelectListItem> ScheduleWorkList { get; set; }
        public string Mode { get; set; }
        //V2-732
        public bool IsUseMultiStoreroom { get; set; }
    }
}