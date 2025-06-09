using System.Collections.Generic;

namespace InterfaceAPI.Models.Vendor
{
    public class VendorMasterErrorModel
    {

        public VendorMasterErrorModel()
        {
            VendorMasterErrMsgList = new List<string>();
            VendorMasterProcessErrMsgList = new List<string>();
        }

        public int ExVendorId { get; set; }
        public List<string> VendorMasterErrMsgList { get; set; }
        public List<string> VendorMasterProcessErrMsgList { get; set; }
    }

}