namespace InterfaceAPI.Models.Vendor
{
    public class VendorMasterImportModel
  {
    public VendorMasterImportModel()
    {
      ClientId = 0;
      SiteId = 0;
      VendorNumber = "";
      ExVendorId = 0;
      Name = "";
      Status = "";
      Address1 = "";
      Address2 = "";
      Address3 = "";
      AddressCity = "";
      AddressState = "";
      AddressPostCode = "";
      AddressCountry = "";
      AddressRemitUseBus = "";
      RemitAddress1 = "";
      RemitAddress2 = "";
      RemitAddress3 = "";
      RemitAddressCity = "";
      RemitAddressState = "";
      RemitAddressPostCode = "";
      RemitAddressCountry = "";
      PhoneNumber = "";
      FaxNumber = "";
      EmailAddress = "";
      TermCode = "";
      TermDescription = "";
    }
    public int ClientId { get; set; }
    public int SiteId { get; set; }
    public string VendorNumber { get; set; }
    public int ExVendorId { get; set; }
    public string Name { get; set; }
    public string Status { get; set; }
    public string Address1 { get; set; }
    public string Address2 { get; set; }
    public string Address3 { get; set; }
    public string AddressCity { get; set; }
    public string AddressState { get; set; }
    public string AddressPostCode { get; set; }
    public string AddressCountry { get; set; }
    public string AddressRemitUseBus { get; set; }
    public string RemitAddress1 { get; set; }
    public string RemitAddress2 { get; set; }
    public string RemitAddress3 { get; set; }
    public string RemitAddressCity { get; set; }
    public string RemitAddressState { get; set; }
    public string RemitAddressPostCode { get; set; }
    public string RemitAddressCountry { get; set; }
    public string PhoneNumber { get; set; }
    public string FaxNumber { get; set; }
    public string EmailAddress { get; set; }
    public string TermCode { get; set; }
    public string TermDescription { get; set; }

  }
}