using DataContracts;
namespace Client.Models.Configuration.PreventiveMaintenanceLibrary
{
    public class PrevMaintLibraryVM : LocalisationBaseVM
    {
        public PreventiveMaintenanceLibraryModel preventiveMaintenanceLibraryModel { get; set; }
        public TaskModel taskModel { get; set; }
        public Security security { get; set; }
        public ChangePreventiveLibraryIDModel ChangePreventiveLibraryIDModel { get; set; }
    }
}