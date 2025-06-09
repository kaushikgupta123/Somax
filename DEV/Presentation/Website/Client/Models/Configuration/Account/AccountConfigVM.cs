
using DataContracts;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Client.Models.Configuration.Account
{
    public class AccountConfigVM : LocalisationBaseVM
    {
        public AccountModel accountDetails { get; set; }
        public List<AccountModel> accountList { get; set; }
        public NoteModel noteModel { get; set; }
        public AttachmentModel attachmentModel { get; set; }
        public ChangeAccountId changeAccountIdModel { get; set; }
        public Security security { get; set; }
        //V2-375
        public List<string> hiddenColumnList { get; set; }
        public bool Switch1 { get; set; }
        public bool InUse { get; set; }
        public List<string> requiredColumnList { get; set; }
        public List<string> disabledColumnList { get; set; }
        public UserData _userdata { get; set; }
        public bool AllowSiteNameColumn { get; set; }
        public IEnumerable<SelectListItem> SiteList { get; set; }
        public long? SiteId { get; set; }
    }
}