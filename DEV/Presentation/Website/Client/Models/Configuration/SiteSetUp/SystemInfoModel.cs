using Common.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.Configuration.SiteSetUp
{
    public class SystemInfoModel
    {
        public long SystemId { get; set; }

        [Required(ErrorMessage = "ValidationSystemInfoID|" + LocalizeResourceSetConstants.SetUpDetails)]
        [RegularExpression("^[A-Z0-9\\%\\-\\:\\/\\$\\*\\+\\.]+$", ErrorMessage = "SystemInfoIDRegErrMsg|" + LocalizeResourceSetConstants.SetUpDetails)]
        public string ClientLookupId { get; set; }
        [Required(ErrorMessage = "Please enter Description")]
        public string Description { get; set; }
        public bool InactiveFlag { get; set; }
        public int UpdateIndex { get; set; }
        public IEnumerable<SelectListItem> InactiveFlagList { get; set; }
    }
}