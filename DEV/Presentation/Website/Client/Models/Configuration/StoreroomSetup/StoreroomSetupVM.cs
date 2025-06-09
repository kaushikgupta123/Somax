using DataContracts;

using System.Collections.Generic;
using System.Web.Mvc;

namespace Client.Models.Configuration.StoreroomSetup
{
    public class StoreroomSetupVM : LocalisationBaseVM
    {
        public StoreroomModel storeroomModel { get; set; }
        public IEnumerable<SelectListItem> SiteList { get; set; }
        public string SiteName { get; set; }
        public IEnumerable<SelectListItem> InactiveFlagList { get; set; }
    }
}
