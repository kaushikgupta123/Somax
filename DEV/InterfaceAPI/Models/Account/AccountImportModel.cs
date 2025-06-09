namespace InterfaceAPI.Models.Account
{
    public class AccountImportModel
    {
      public AccountImportModel()
      {
        ClientId = 0;
        SiteId = 0;
        AccountNumber = "";
        ExAccountId = 0;
        Name = "";
        Status = "";
      }
      public int ClientId { get; set; }
      public int SiteId { get; set; }
      public string AccountNumber { get; set; }
      public int ExAccountId { get; set; }
      public string Name { get; set; }
      public string Status { get; set; }
    }
}