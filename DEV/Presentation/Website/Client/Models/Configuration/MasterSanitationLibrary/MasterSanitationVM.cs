using DataContracts;
namespace Client.Models.Configuration.MasterSanitationLibrary
{
    public class MasterSanitationVM: LocalisationBaseVM
    {
        public MasterSanitationModel masterSanitationModel { get; set; }
        public TaskModel taskModel { get; set; }
        public Security security { get; set; }
    }
}