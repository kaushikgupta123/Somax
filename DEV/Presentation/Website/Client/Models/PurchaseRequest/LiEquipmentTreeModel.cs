using System.Collections.Generic;
using DataContracts;

namespace Client.Models.PurchaseRequest
{
    public class LiEquipmentTreeModel
    {
        public long EquipmentId { get; set; }
        public long ParentId { get; set; }

        public string ClientLookupId { get; set; }
        public string Name { get; set; }

        public List<DataContracts.Equipment> Children { get; set; }
    }
}

