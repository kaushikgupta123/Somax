using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models.Common
{
    public class UserMentionDataModel
    {
        public class UserMentionData
        {
            public string id { get; set; }
            public string name { get; set; }
            public string type { get; set; }
            public long userId { get; set; }
            public string userName { get; set; }
            public string emailId { get; set; }
        }
    }
}