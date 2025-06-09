using Common.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.Configuration.EquipmentMaster
{
    public class EquipmentMasterPmModel
    {
        public string Description { get; set; }
        public int Frequency { get; set; }
        public string FrequencyType { get; set; }
        [Required(ErrorMessage = "ValidPMMasterID|" + LocalizeResourceSetConstants.ConfigMasterDetail)]
        public string ClientLookupId { get; set; }
        public long EQMaster_PMLibraryId { get; set; }
        public long EQMasterId { get; set; }
        public long PMLibraryId { get; set; }
        public int UpdateIndex { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
      
        public IEnumerable<SelectListItem> PmList { get; set; }
    }
}