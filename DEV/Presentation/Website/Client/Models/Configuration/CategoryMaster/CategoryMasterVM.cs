using DataContracts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Configuration.CategoryMaster
{
    public class CategoryMasterVM : LocalisationBaseVM
    {
        public CategoryMasterModel categoryMasterModel { get; set; }
        public CategoryMasterPrintModel categoryMasterPrintModel { get; set; }
        public Security security { get; set; }
    }
}