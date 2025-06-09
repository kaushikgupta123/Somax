using Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace Client.Models.Configuration.PreventiveMaintenanceLibrary
{
    public class ChangePreventiveLibraryIDModel
    {
        public long PrevMaintLibraryId { get; set; }
        [Required(ErrorMessage = "validPrevMaintLibId|" + LocalizeResourceSetConstants.PrevMaintDetails)]
        [RegularExpression("^[A-Z0-9\\%\\-\\:\\/\\$\\*\\+\\.]+$", ErrorMessage = "PrevMaintLibIdRegErrMsg|" + LocalizeResourceSetConstants.PrevMaintDetails)]
        [Display(Name = "spnPrevLibId|" + LocalizeResourceSetConstants.PrevMaintDetails)]
        public string ClientLookupId { get; set; }
        public int updateindex { get; set; }

    }
}