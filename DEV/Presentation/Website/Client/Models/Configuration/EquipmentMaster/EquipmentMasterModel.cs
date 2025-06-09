using Common.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.Configuration.EquipmentMaster
{
    public class EquipmentMasterModel
    {
        public long ClientId { get; set; }
        public long EquipmentMasterId { get; set; }
        public string ClientLookupId { get; set; }
        public bool InactiveFlag { get; set; }
        [Required(ErrorMessage = "ValidName|" + LocalizeResourceSetConstants.ConfigMasterDetail)]
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Type { get; set; }
        public int UpdateIndex { get; set; }
        public IEnumerable<SelectListItem> TypeList { get; set; }
    }
}