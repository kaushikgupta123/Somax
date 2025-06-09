using DataContracts;

namespace Client.Models.Configuration.ManufacturerMaster
{
  public class ManufacturerMasterVM : LocalisationBaseVM
  {
    public ManufacturerModel manufacturerModel { get; set; }
    public ManufacturerMasterPrintModel manufacturerPrintModel { get; set; }
    public Security security { get; set; }
  }
}