using Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace Client.Models
{
    public class ChangePreventiveIDModel
    {
        public long PrevMaintMasterId { get; set; }
        [Required(ErrorMessage = "validPrevMaintMasterId|" + LocalizeResourceSetConstants.PrevMaintDetails)]
        [RegularExpression("^[A-Z0-9\\%\\-\\:\\/\\$\\*\\+\\.]+$", ErrorMessage = "PrevMaintMasterIdRegErrMsg|" + LocalizeResourceSetConstants.PrevMaintDetails)]
        [Display(Name = "spnPrevMstrId|" + LocalizeResourceSetConstants.PrevMaintDetails)]
        public string ClientLookupId { get; set; }
        public int updateindex { get; set; }

    }
}