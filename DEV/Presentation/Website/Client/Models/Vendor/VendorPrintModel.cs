namespace Client.Models.VendorPrint
{
    public class VendorPrintModel
    {
        public string ClientLookupId { get; set; }
        public string Name { get; set; }
        public string AddressCity { get; set; }
        public string AddressState { get; set; }
        public string Type { get; set; }
        public string Terms { get; set; }
        public string FOBCode { get; set; }
        public bool IsExternal { get; set; }
    }
}