using Client.CustomValidation;

using Common.Constants;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.Parts
{
    public class PartsConfigureAutoPurchasingModel
    {
        public long PartId { get; set; }
        public long PartStoreroomId { get; set; }
        public bool IsAutoPurchase { get; set; }
        [RequiredIf("IsAutoPurchase", true, ErrorMessage = "spnIdSelectVendor|" + LocalizeResourceSetConstants.InvoiceDetails)]
        public long? PartVendorId { get; set; }
        [Display(Name = "spnMaximum|" + LocalizeResourceSetConstants.PartDetails)]
        [RegularExpression(@"^\d*\.?\d{0,5}$", ErrorMessage = "globalFiveDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999999999.99999, ErrorMessage = "globalFiveDecimalAfterTotalFifteenRegErrMsg|" + LocalizeResourceSetConstants.PartDetails)]
        [GreaterThanForPartAutoPurchasingConfiguration("QtyReorderLevel", ErrorMessage = "globalGreaterThanToMinimumValueErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? QtyMaximum { get; set; }

        [Display(Name = "spnMinimum|" + LocalizeResourceSetConstants.PartDetails)]
        [RegularExpression(@"^\d*\.?\d{0,5}$", ErrorMessage = "globalFiveDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999999999.99999, ErrorMessage = "globalFiveDecimalAfterTotalFifteenRegErrMsg|" + LocalizeResourceSetConstants.PartDetails)]
        [LessThanForPartAutoPurchasingConfiguration("QtyMaximum", ErrorMessage = "globalGreaterThanToMinimumValueErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? QtyReorderLevel { get; set; }
        public IEnumerable<SelectListItem> VendorList { get; set; }
    }
}