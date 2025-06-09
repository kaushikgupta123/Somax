using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.ChangeActiveSite
{
    public class ChangeActiveSiteModel
    {
        [Required]
        public long ChangeSiteSiteId { get; set; }
        public long ActiveSiteUpdateIndex { get; set; }
        public IEnumerable<SelectListItem> SiteList { get; set; }
    }
}