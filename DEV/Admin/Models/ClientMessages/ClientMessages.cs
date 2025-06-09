using Common.Constants;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Admin.Models.ClientMessages
{
    public class ClientMessages : LocalisationBaseVM
    {
        public ClientMessageModel ClientMessageModel { get; set; }

    }
    public class ClientMessageModel
    {
        public long ClientId { get; set; }
        public long ClientCustomId { get; set; }
        public long ClientMessageId { get; set; }


        [Display(Name = "spnMessage|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "validationMessage|" + LocalizeResourceSetConstants.Global)]
        public string Message { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]

        [Display(Name = "globalStartDate|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "vaildDate|" + LocalizeResourceSetConstants.Global)]
        public string CMStartDate { get; set; }

        [Required(ErrorMessage = "vaildTime|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression(@"^([0-9]|0[0-9]|1[0-9]|2[0-3]):([0-9]|[0-5][0-9]) (am|pm|AM|PM)$", ErrorMessage = "globalTimeErrorMessage|" + LocalizeResourceSetConstants.Global)]
        public string CMStartTime { get; set; }

        [Display(Name = "globalEndDate|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "vaildDate|" + LocalizeResourceSetConstants.Global)]
        public string CMEndDate { get; set; }

        [Required(ErrorMessage = "vaildTime|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression(@"^([0-9]|0[0-9]|1[0-9]|2[0-3]):([0-9]|[0-5][0-9]) (am|pm|AM|PM)$", ErrorMessage = "globalTimeErrorMessage|" + LocalizeResourceSetConstants.Global)]
        public string CMEndTime { get; set; }
        public DateTime CreateDate { get; set; }
        public int TotalCount { get; set; }

    }
}