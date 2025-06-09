using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Client.Models.Configuration.CustomSecurityProfile
{
    public class CustomSecurityProfileModel
    {
        public long ClientId { get; set; }
        public long SecurityProfileId { get; set; }
        [Display(Name = "globalSubject|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "NoteSubjectErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string Name { get; set; }
        [Display(Name = "spnDescription|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "validationDescription|" + LocalizeResourceSetConstants.Global)]
        public string Description { get; set; }
        public int SortOrder { get; set; }
        public bool Protected { get; set; }
        public int UpdateIndex { get; set; }
        [DefaultValue("Add")]
        public string Pagetype { get; set; }
        public bool SingleItem { get; set; }
        public long SecurityItemId { get; set; }
        public string SecurityLocalizedName { get; set; }
        public string ItemName { get; set; }
        public bool ItemAccess { get; set; }
        public bool ItemCreate { get; set; }
        public bool ItemEdit { get; set; }
        public bool ItemDelete { get; set; }

       
    }
}