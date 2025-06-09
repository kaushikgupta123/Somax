

using System;
using Common.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Client.Models.Configuration.StoreroomSetup
{
    public class StoreroomModel
    {
        public long ClientId { get; set; }
        public long StoreroomId { get; set; }
        [Required(ErrorMessage = "GlobalSiteSelect|" + LocalizeResourceSetConstants.Global)]
        public long SiteId { get; set; }
        public string SiteName { get; set; }
        [Required(ErrorMessage = "spnNameErrorMsg|" + LocalizeResourceSetConstants.Global)]
        [MaxLength(15,ErrorMessage = "ValidationMaxLength15Name|" + LocalizeResourceSetConstants.Global)]
        public string Name { get; set; }
        [Required(ErrorMessage = "validationDescription|" + LocalizeResourceSetConstants.Global)]
        [MaxLength(255, ErrorMessage = "ValidationMaxLength255Description|" + LocalizeResourceSetConstants.Global)]
        public string Description { get; set; }
        public bool InactiveFlag { get; set; }
        public int UpdateIndex { get; set; }
        public bool IsAdd { get; set; }
        public IEnumerable<SelectListItem> SiteList { get; set; }
        public int totalCount { get; set; }
    }
}