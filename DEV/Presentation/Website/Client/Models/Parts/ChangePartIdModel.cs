using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Client.Models.Parts
{
    public class ChangePartIdModel
    {
        [Required(ErrorMessage = "validationPartId|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression("^[A-Z0-9\\%\\-\\:\\/\\$\\*\\+\\.]+$", ErrorMessage = "PartIdRegErrMsg|" + LocalizeResourceSetConstants.PartDetails)]
        [Display(Name = "spnPartID|" + LocalizeResourceSetConstants.Global)]
        public string ClientLookupId { get; set; }

    }
}