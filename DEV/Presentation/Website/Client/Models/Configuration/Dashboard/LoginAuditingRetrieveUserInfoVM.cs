using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Configuration
{
    public class LoginAuditingRetrieveUserInfoVM: LocalisationBaseVM
    {
        public string UserName { get; set; }
        public DateTime LogIn { get; set; }
        public string IPAddress { get; set; }
    }
}