using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models
{
    public class AutoPRGenerationVM : LocalisationBaseVM
    {
        public string VendorIds { get; set; }
        public bool IsMultistoreroomAutoPRGeneration { get; set; }
        public IEnumerable<SelectListItem> StoreroomList { get; set; } //1196
        public List<AutoPRGenerationSearchModel> autoPRGenerationSearchModel { get; set; }
        public AutoPRGenerationStoreroomModel autoPRGenerationStoreroomModel { get; set; }
    }
}