using Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace Client.Models.MultiStoreroomPart
{
    public class ChangePartIdForMultiPartStoreroomPartModel
    {
        [Required(ErrorMessage = "validationPartId|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression("^[A-Z0-9\\%\\-\\:\\/\\$\\*\\+\\.]+$", ErrorMessage = "PartIdRegErrMsg|" + LocalizeResourceSetConstants.PartDetails)]
        [Display(Name = "spnPartID|" + LocalizeResourceSetConstants.Global)]
        public string ClientLookupId { get; set; }
    }
}