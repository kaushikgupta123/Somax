using Client.CustomValidation;
using Common.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace Client.Models.Configuration.LookupLists
{
    public class LookUpListsModel
    {
        public LookUpListsModel()
        {
            DescriptionLookUpList = new List<SelectListItem>();
            InactiveFlagList = new List<SelectListItem>();
        }
        public long LookupListId { get; set; }
        
        [RequiredIf("LookupListId", "0", ErrorMessage = "validationenterValue|" + LocalizeResourceSetConstants.LookUpListDetails)]
        public string ListValue { get; set; }
        [Required (ErrorMessage = "validationenterDescription|" + LocalizeResourceSetConstants.Global)]
        public string Description { get; set; }      
        public bool InactiveFlag { get; set; }
        public bool IsReadOnly { get; set; }
        public string DescriptionLookUp { get; set; }
        public string DescriptionLookUpText { get; set; }
        public int UpdateIndex { get; set; }
        public IEnumerable<SelectListItem> DescriptionLookUpList { get; set; }
        public IEnumerable<SelectListItem> InactiveFlagList { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class LookUpListDescription
    {
        public string Value { get; set; }
        public string Name { get; set; } 
    }
}