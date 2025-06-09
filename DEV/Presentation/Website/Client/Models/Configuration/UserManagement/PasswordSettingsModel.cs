using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Configuration.UserManagement
{
    public class PasswordSettingsModel
    {
        public int MaxAttempts { get; set; }
        public bool PWReqMinLength { get; set; }
        public int PWMinLength { get; set; }
        public bool PWReqExpiration { get; set; }
        public int PWExpiresDays { get; set; }
        public bool PWRequireNumber { get; set; }
        public bool PWRequireAlpha { get; set; }
        public bool PWRequireMixedCase { get; set; }
        public bool PWRequireSpecialChar { get; set; }
        public bool PWNoRepeatChar { get; set; }
        public bool PWNotEqualUserName { get; set; }
        public bool AllowAdminReset { get; set; }
    }
}