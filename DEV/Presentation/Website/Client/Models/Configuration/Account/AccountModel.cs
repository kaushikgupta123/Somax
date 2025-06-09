using Client.Common;
using Common.Constants;
using Client.CustomValidation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.Configuration.Account
{
    public class AccountModel
    {
        public long AccountId { get; set; }
        public long DepartmentId { get; set; }       
        [Required(ErrorMessage = "spnAccountErrorMsg|" + LocalizeResourceSetConstants.AccountDetails)]
        public string ClientLookupId { get; set; }
        [Required(ErrorMessage = "spnNameErrorMsg|" + LocalizeResourceSetConstants.Global)]
        public string Name { get; set; }
        public bool InactiveFlag { get; set; }              
        public bool IsAddFromIndex { get; set; }
        public bool IsAddFromDetails { get; set; }
        public List<NoteModel> noteList { get; set; }
        public List<AttachmentModel> attachmentList { get; set; }
        public IEnumerable<SelectListItem> AccountTypeList { get; set; }
        public IEnumerable<SelectListItem> ExternalTypeList { get; set; }

        public long ClientId { get; set; }
        public long SiteId { get; set; }
        public long AreaId { get; set; }
        public long StoreroomId { get; set; }
        public long ParentId { get; set; }
        public int UpdateIndex { get; set; }
        //V2-375
        public string ViewName { get; set; }
        public bool IsExternal { get; set; }
        //V2-402
        public string SiteName { get; set; }
        public string PackageLevel { get; set; }
        public bool IsSuperUser { get; set; }
    }
    public class ChangeAccountId
    {
        [Required(ErrorMessage = "spnAccountErrorMsg|" + LocalizeResourceSetConstants.AccountDetails)]
        public string ClientLookupId { get; set; }
        public long AccountId { get; set; }
        public int UpdateIndex { get; set; }

    }
  
}