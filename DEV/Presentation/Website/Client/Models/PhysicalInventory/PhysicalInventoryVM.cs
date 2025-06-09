using DataContracts;
namespace Client.Models.PhysicalInventory
{
    public class PhysicalInventoryVM:LocalisationBaseVM
    {
        public PhysicalInventoryModel inventoryModel { get; set; }
        public UserData udata { get; set; } //V2-790
    }
}