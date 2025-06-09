using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models.UserManagement
{
    public class UserManagementPersonnelModel
    {
        public long ClientId { get; set; }
        public long PersonnelId { get; set; }
        public long UserInfoId { get; set; }
        public long SiteId { get; set; }
        public string UserSiteName { get; set; }
        public string CraftDescription { get; set; }
        public List<string> ErrorMessages { get; set; }


    }
}