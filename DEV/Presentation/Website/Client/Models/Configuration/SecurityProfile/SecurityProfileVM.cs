using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Configuration.SecurityProfile
{
    public class SecurityProfileVM : LocalisationBaseVM
    {
        public SecurityProfileVM()
        {
            SecurityItemModelList = new List<SecurityItemModel>();
        }
        public SecurityProfileModel securityProfileModel { get; set; }
        public SecurityItemModel securityItemModel { get; set; }

        public List<SecurityItemModel> SecurityItemModelList { get; set; }
        public ProcessToupdate processToupdate { get; set; }
        public ModulesToupdate modulesToupdate { get; set; }

    }
}