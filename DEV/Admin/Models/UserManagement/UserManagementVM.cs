

using Admin.Models.Common;

using DataContracts;

using System.Collections.Generic;
using System.Web.Mvc;

namespace Admin.Models.UserManagement
{
    public class UserManagementVM : LocalisationBaseVM
    {
        public UserModel userModels { get; set; }
        public UserManagementModel UserManagementModel { get; set; }
        public UserDetails UserDetail { get; set; }
        public string PackageLevel { get; set; }
        public bool IsSuperUser { get; set; }
        public IEnumerable<SelectListItem> SiteList { get; set; }
        public List<InnerGridUserManagement> InnerGridDataList { get; set; }
        public IEnumerable<SelectListItem> ClientList { get; set; }
        public List<UMPdfPrintModel> PrintModelList { get; set; }
        public List<TableHaederProp> tableHaederProps { get; set; }
        public UserSiteModel userSiteModel { get; set; }
    }
}