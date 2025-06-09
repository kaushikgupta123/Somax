using DataContracts;

namespace Client.Models.Configuration.VendorCatalog
{
    public class VendorCatalogVM: LocalisationBaseVM
    {
        public VendorCatalogModel VendorCatalogModel { get; set; }
        public Security security { get; set; }
    }
}