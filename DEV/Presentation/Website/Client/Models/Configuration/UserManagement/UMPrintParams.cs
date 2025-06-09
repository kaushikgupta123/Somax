using Client.Models.Common;
using System;
using System.Collections.Generic;

namespace Client.Models.Configuration.UserManagement
{
    [Serializable]
    public class UMPrintParams
    {
        public List<TableHaederProp> tableHaederProps { get; set; }
        public string UserName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public long CraftId { get; set; }
        public string order { get; set; }
        public string orderDir { get; set; }
        public int CaseNo { get; set; }
        public string SelectedSites { get; set; }
        public string SearchText { get; set; }
        public string SecurityProfileIds { get; set; }

        public string UserTypes { get; set; }
        public string Shifts { get; set; }
        public bool? IsActive { get; set; }
        public string EmployeeId { get; set; } //V2-1160
    }
}