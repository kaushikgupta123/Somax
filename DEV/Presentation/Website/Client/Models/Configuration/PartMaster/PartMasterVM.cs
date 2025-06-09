using DataContracts;

namespace Client.Models.Configuration.PartMaster
{
    public class PartMasterVM: LocalisationBaseVM
    {
        public PartMasterModel PartMasterModel { get; set; }
        public Security security { get; set; }
        public bool delf { get; set; }
        public bool ShowDeleteBtnAfterUpload { get; set; }
    }
}