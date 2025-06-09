using Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Client.CustomValidation;

namespace Client.Models.Parts
{
    public class PartPhysicalInvModel
    {
        public long PartId { get; set; }
        public string PartClientLookupId { get; set; }
        [Required(ErrorMessage = "validationquantitycount|" + LocalizeResourceSetConstants.PartDetails)]
        [RegularExpression(@"^\d*\.?\d{0,6}$", ErrorMessage = "globalSixDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 999999999.999999, ErrorMessage = "GlobalMaxRangeOneToNieDigitWithSixDecimalPlace|" + LocalizeResourceSetConstants.PartDetails)]
        public decimal? ReceiptQuantity { get; set; }
        public IEnumerable<SelectListItem> PartList { get; set; }
        public string PartUPCCode { get; set; }
        public string Description { get; set; }
    }
}