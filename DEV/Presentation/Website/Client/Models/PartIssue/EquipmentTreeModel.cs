using DataContracts;
using System.Collections.Generic;

namespace Client.Models.PartIssue
{
    public class EquipmentTreeModel
    {
        public long EquipmentId { get; set; }
        public long ParentId { get; set; }

        public string ClientLookupId { get; set; }
        public string Name { get; set; }

        public List<DataContracts.Equipment> Children { get; set; }
    }
}