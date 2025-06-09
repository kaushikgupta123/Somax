using DataContracts;

namespace Client.Models.Configuration.ShipToAddress
{
    public class ShipToVM : LocalisationBaseVM
    {
        public ShipToModel shipToModel { get; set; }
        public ShipToPrintModel shipToPrintModel { get; set; }
        public Security security { get; set; }
    }
}