using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Common
{
    public class AccountLookUpModel
    {
        public string ClientLookupId { get; set; }
        public string Name { get; set; }
        public long TotalCount { get; set; }

        public long? AccountId { get; set; }
    }
}