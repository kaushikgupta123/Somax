using System.Collections.Generic;

namespace Client.Models.Configuration.VendorMaster
{
  public class VendorAddFromVMGridErrorModel
  {
    public VendorAddFromVMGridErrorModel()
    {
      ErrorMessage = new List<string>();
    }
    public string ClinetLookUpId { get; set; }
    public List<string> ErrorMessage { get; set; }
  }
}