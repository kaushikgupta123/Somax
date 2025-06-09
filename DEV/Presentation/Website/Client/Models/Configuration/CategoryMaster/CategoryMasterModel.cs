using Common.Constants;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Client.Models.Configuration.CategoryMaster
{
    public class CategoryMasterModel
    {
        [Required(ErrorMessage = "globalValidPartCategoryMasterId|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression("^[A-Z0-9\\%\\-\\:\\/\\$\\*\\+\\.]+$", ErrorMessage = "spnOnPartCategoryIDcontainsinvalidcharacters|" + LocalizeResourceSetConstants.ConfigMasterDetail)]
        public string ClientLookupId { get; set; }
        public long PartCategoryMasterId { get; set; }
        [Required(ErrorMessage = "validationDescription|" + LocalizeResourceSetConstants.Global)]
        public string Description { get; set; }
        public bool InactiveFlag { get; set; }
        public int TotalCount { get; set; }
    }
}