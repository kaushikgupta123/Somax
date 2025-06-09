using Admin.Models.Common;
using System;
using System.Collections.Generic;

namespace Admin.Models.UserManagement
{
    [Serializable]
    public class UMPrintParams
    {
        public List<TableHaederProp> tableHaederProps { get; set; }
        public string UserName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public long SiteId { get; set; }
        public long ClientId { get; set; }
        public string order { get; set; }
        public string orderDir { get; set; }
        public int CaseNo { get; set; }
        public string SearchText { get; set; }
        public bool IsActive { get; set; }

    }
}