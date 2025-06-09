using DataContracts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.VendorRequest
{
    public class VendorRequestVM : LocalisationBaseVM
    {
        public List<VendorRequestSearchModel> vendorRequestSearchModel { get; set; }
        public IEnumerable<SelectListItem> VendorRequestViewList { get; set; }
        public Security security { get; set; }
        public VendorRequestModel vendorRequestModel { get; set; }
        public IEnumerable<SelectListItem> LookupTypeList { get; set; }
        public string Type { get; set; }
    }
}