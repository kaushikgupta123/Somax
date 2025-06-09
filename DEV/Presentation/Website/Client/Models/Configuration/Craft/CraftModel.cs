using Common.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.Configuration.ConfigCraft
{
    public class CraftModel
    {
        public long ClientId { get; set; }
        public long CraftId { get; set; }
        [Required(ErrorMessage = "1100|" + LocalizeResourceSetConstants.StoredProcValidation)]
        [StringLength(15, MinimumLength = 2, ErrorMessage = "1100|" + LocalizeResourceSetConstants.StoredProcValidation)]
        public string ClientLookupId { get; set; }
        [RegularExpression(@"^\d*\.?\d{0,3}$", ErrorMessage = "globalThreeDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 999999.999, ErrorMessage = "globalThreeDecimalAfterTotalNineRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? ChargeRate { get; set; }
        [Required(ErrorMessage = "validationDescription|" + LocalizeResourceSetConstants.Global)]      
        public string Description { get; set; }
        public bool InactiveFlag { get; set; }
        public int UpdateIndex { get; set; }
        public IEnumerable<SelectListItem> InactiveFlagList { get; set; }
    }
}