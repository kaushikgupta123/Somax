using DataContracts;

namespace Client.Models.Configuration.EquipmentMaster
{
    public class EquipmentMasterVM: LocalisationBaseVM
    {
        public EquipmentMasterModel EquipmentMasterModel { get; set; }
        public EquipmentMasterPrintModel EquipmentMasterPrintModel { get; set; }
        public EquipmentMasterPmModel EquipmentMasterPmModel { get; set; }
        public EqMastPMGridDataModel EqMastPMGridDataModel { get; set; }
        public Security security { get; set; }
    }
}