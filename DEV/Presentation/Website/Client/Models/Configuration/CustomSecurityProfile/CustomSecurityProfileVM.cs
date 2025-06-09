using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Configuration.CustomSecurityProfile
{
    public class CustomSecurityProfileVM : LocalisationBaseVM
    {
        public CustomSecurityProfileVM()
        {
            
        }
        public CustomSecurityProfileModel customSecurityProfileModel { get; set; }
        public CustomSecurityItemModel securityItemModel { get; set; }
        public List<CustomSecurityItemModel> customsecurityItemList { get; set; }
    }
}