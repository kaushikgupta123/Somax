using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Configuration.VendorMaster
{
  public class VMAddGridModel
  {
    public long VendorMasterId { get; set; }
    public string ClientLookupId { get; set; }
    public string Name { get; set; }
    public string AddressCity { get; set; }
    public string AddressState { get; set; }
    public string Type { get; set; }
    public bool Inactive { get; set; }
  }
}