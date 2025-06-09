using Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Client.CustomValidation;

namespace Client.Models.MultiStoreroomPart
{
    public class MultiStoreroomPartClientLookupIdModel
    {
        [Required(ErrorMessage = "validationPartId|" + LocalizeResourceSetConstants.Global)]
        public string PartClientLookupId { get; set; }
    }
}