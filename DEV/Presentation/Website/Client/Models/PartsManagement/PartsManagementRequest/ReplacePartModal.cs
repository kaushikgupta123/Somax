using Common.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.PartsManagement.PartsManagementRequest
{
    public class ReplacePartModal
    {
        [Required(ErrorMessage = "ValidPartToReplace|" + LocalizeResourceSetConstants.PartsManagementDetail)]
        public string PartToReplace { get; set; }
        [Required(ErrorMessage = "ValidReplaceWith|" + LocalizeResourceSetConstants.PartsManagementDetail)]
        public string ReplaceWith { get; set; }
        [Required(ErrorMessage = "ValidJustification|" + LocalizeResourceSetConstants.PartsManagementDetail)]
        public string Justification { get; set; }
        public IEnumerable<SelectListItem> PartToReplaceList { get; set; }
        public IEnumerable<SelectListItem> ReplaceWithList { get; set; }
        public string RequestType { get; set; }
    }
}