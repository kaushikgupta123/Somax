using DataContracts;
namespace Client.Models.Configuration.MaintenanceOnDemandLibrary
{
    public class MaintenanceOnDemandVM : LocalisationBaseVM
    {
        public MaintenanceOnDemandModel maintenanceOnDemanModel { get; set; }
        public TaskModel taskModel { get; set; }
        public Security security { get; set; }
    }
}