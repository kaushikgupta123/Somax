using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Models
{
    public class KnowledgebaseModel
    {
        [Display(Name = "spnKBTopicsId|" + LocalizeResourceSetConstants.KnowledgebaseDetails)]
        public long KBTopicsId { get; set; }
        [Display(Name = "spnTitle|" + LocalizeResourceSetConstants.KnowledgebaseDetails)]
        [Required(ErrorMessage = "KBTopicsTitleErrorMessage|" + LocalizeResourceSetConstants.KnowledgebaseDetails)]
        public string Title { get; set; }
        [Display(Name = "spnCategory|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "KBTopicsCategoryErrorMessage|" + LocalizeResourceSetConstants.KnowledgebaseDetails)]
        public string Category { get; set; }
        [Display(Name = "spnFolder|" + LocalizeResourceSetConstants.KnowledgebaseDetails)]
        [Required(ErrorMessage = "KBTopicsFolderErrorMessage|" + LocalizeResourceSetConstants.KnowledgebaseDetails)]
        public string Folder { get; set; }
        [Display(Name = "spnDescription|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "KBTopicsDescriptionErrorMessage|" + LocalizeResourceSetConstants.KnowledgebaseDetails)]
        [AllowHtml]
        public string Description { get; set; }
        [Display(Name = "spnTags|" + LocalizeResourceSetConstants.KnowledgebaseDetails)]
        public string Tags { get; set; }

        public string CategoryName { get; set; }
        public IEnumerable<SelectListItem> KBTopicsCategoryList { get; set; }
        public List<string> PersonnelIds { get; set; }
        public IEnumerable<SelectListItem> Personnellist { get; set; }
        public IEnumerable<SelectListItem> SearchPersonnellist { get; set; }
        public string KbTopicsTags { get; set; }
    }
}