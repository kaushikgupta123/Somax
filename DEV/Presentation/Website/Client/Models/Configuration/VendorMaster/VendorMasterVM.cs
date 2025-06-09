using DataContracts;
using System.Collections.Generic;

namespace Client.Models.Configuration.VendorMaster
{
    public class VendorMasterVM: LocalisationBaseVM
    {
        public VendorMasterModel vendorMasterModel { get; set; }
        public ChangeVendorModel changeVendorModel { get; set; }
        public Security security { get; set; }
        public List<string> hiddenColumnList { get; set; }
        public List<string> requiredColumnList { get; set; }
        public List<string> disabledColumnList { get; set; }
        public bool Switch1 { get; set; }
        public bool InUse { get; set; }
    }
}