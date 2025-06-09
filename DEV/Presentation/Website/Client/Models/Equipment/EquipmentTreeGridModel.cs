using System.Collections.Generic;
using DataContracts;
namespace Client.Models
{
    public class EquipmentTreeGridModel
    {
        public long EquipmentId { get; set; }
        public long ParentId { get; set; }
        public string ClientLookupId { get; set; }
        public string Name { get; set; }
        public long ChildCount { get; set; }
    }
}