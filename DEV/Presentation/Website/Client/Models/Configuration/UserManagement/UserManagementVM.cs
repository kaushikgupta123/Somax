using Client.Models.Common;

using DataContracts;

using System.Collections.Generic;
using System.Web.Mvc;

namespace Client.Models.Configuration.UserManagement
{
    public class UserManagementVM : LocalisationBaseVM
    {
        public UserModel userModels { get; set; }
        public UserManagementModel UserManagementModel { get; set; }
        public UserDetails UserDetail { get; set; }
        public NotesModel notesModel { get; set; }
        public AttachmentModel attachmentModel { get; set; }
        public ResetPasswordModel resetPasswordModel { get; set; }
        public UserManagementContactModel userManagementContactModel { get; set; }
        public string PackageLevel { get; set; }
        public bool IsSuperUser { get; set; }
        public List<InnerGridCraft> CraftList { get; set; }
        public List<SelectListItem> SiteList { get; set; }
        public List<UMPdfPrintModel> PrintModelList { get; set; }
        public List<TableHaederProp> tableHaederProps { get; set; }
        public UserSiteModel userSiteModel { get; set; }

        public UserChangeAccessModel userChangeAccessModel { get; set; }

        public PasswordSettingsModel passwordSettingsModel { get; set; }
        public ManualResetPasswordModel manualResetPasswordModel { get; set; }

        public UserNameChangeModel userNameChangeModel { get; set; }
        public UserStoreroomModel userStoreroomModel { get; set; }
        public List<SelectListItem> StoreroomList { get; set; }
        public ReferenceUserModel referenceUserModel { get; set; }
    }
}