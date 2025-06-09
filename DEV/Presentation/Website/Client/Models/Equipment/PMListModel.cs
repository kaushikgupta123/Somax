using System;

namespace Client.Models
{
    public class PMListModel
    {
        public string ClientLookupId { get; set; }
        public string Description { get; set; }
        public DateTime? LastScheduled { get; set; }
        public DateTime? LastPerformed { get; set; }
        public string AssignedTo_PersonnelName { get; set; }
        public long PrevMaintMasterId { get; set; }
    }

}