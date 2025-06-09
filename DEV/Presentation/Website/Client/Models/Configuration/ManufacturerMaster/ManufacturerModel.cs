using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Client.Models.Configuration.ManufacturerMaster
{
    public class ManufacturerModel
    {
        [Required(ErrorMessage = "globalValidManufacturerID|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression("^[A-Z0-9\\%\\-\\:\\/\\$\\*\\+\\.]+$", ErrorMessage = "spnOnManufacturerIDcontainsinvalidcharacters|" + LocalizeResourceSetConstants.ConfigMasterDetail)]
        public string ClientLookupId { get; set; }
        public long ManufacturerID { get; set; }
        [Required(ErrorMessage = "globalValidName|" + LocalizeResourceSetConstants.Global)]
        public string Name { get; set; }
        public bool Inactive { get; set; }

    }
}