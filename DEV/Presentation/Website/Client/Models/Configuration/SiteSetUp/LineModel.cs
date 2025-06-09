using Common.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.Configuration.SiteSetUp
{
    public class LineModel
    {
        public long LineId { get; set; }
        [Required(ErrorMessage = "ValidationLineID|" + LocalizeResourceSetConstants.SetUpDetails)]
        [RegularExpression("^[A-Z0-9\\%\\-\\:\\/\\$\\*\\+\\.]+$", ErrorMessage = "LineIDRegErrMsg|" + LocalizeResourceSetConstants.SetUpDetails)]
        public string ClientLookupId { get; set; }
        [Required(ErrorMessage = "validationenterDescription|" + LocalizeResourceSetConstants.Global)]
        public string Description { get; set; }
        public bool InactiveFlag { get; set; }
        public int UpdateIndex { get; set; }
        public IEnumerable<SelectListItem> InactiveFlagList { get; set; }
    }
}