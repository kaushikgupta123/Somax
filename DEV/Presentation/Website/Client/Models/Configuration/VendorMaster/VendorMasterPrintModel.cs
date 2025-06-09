namespace Client.Models.Configuration.VendorMaster
{
    public class VendorMasterPrintModel
    {
        public string ClientLookupId { get; set; }
        public string Name { get; set; }
        public string ExVendorSiteCode { get; set; }
        public string AddressCity { get; set; }
        public string AddressState { get; set; }
        public string Type { get; set; }
        public bool IsExternal { get; set; }
    }
}