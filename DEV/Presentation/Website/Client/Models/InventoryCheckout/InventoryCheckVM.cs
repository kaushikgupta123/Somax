using DataContracts;

namespace Client.Models.InventoryCheckout
{
    public class InventoryCheckVM : LocalisationBaseVM
    {
        public InventoryCheckoutModel inventoryCheckoutModel { get; set; }
        public InventoryCheckoutValidationModel inventoryCheckoutValidationModel { get; set; }       
        public EquipmentTreeModel equipmentTreeModel { get; set; }
        public UserData userData { get; set; }
        //V2-624
        public InvenroryCheckoutReturnModel inventoryCheckoutReturnModel { get; set; }

    }
}