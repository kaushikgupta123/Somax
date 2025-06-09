using Common.Constants;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.Configuration.ApprovalGroups
{
    public class ApprovalGroupMasterModel
    {
        public int ApprovalGroupId { get; set; }
        [Required(ErrorMessage = "spnValidationRequstType|" + LocalizeResourceSetConstants.ApprovalGroups)]
        public string RequestType { get; set; }
        [Required(ErrorMessage = "validationDescription|" + LocalizeResourceSetConstants.Global)]
        public string Description { get; set; }
        public long? AssetGroup1 { get; set; }
        public long? AssetGroup2 { get; set; }
        public long? AssetGroup3 { get; set; }
        public IEnumerable<SelectListItem> RequestTypeList { get; set; }
        public IEnumerable<SelectListItem> AssetGroup1List { get; set; }
        public IEnumerable<SelectListItem> AssetGroup2List { get; set; }
        public IEnumerable<SelectListItem> AssetGroup3List { get; set; }
        public string AssetGroup1ClientLookupId { get; set; }
        public string AssetGroup2ClientLookupId { get; set; }
        public string AssetGroup3ClientLookupId { get; set; }

    }
}