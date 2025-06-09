using Common.Constants;
using System.ComponentModel.DataAnnotations;
namespace Client.Models.Configuration.MaintenanceOnDemandLibrary
{
    public class TaskModel
    {
        [Required(ErrorMessage = "spnOrderErrorMsg|" + LocalizeResourceSetConstants.LibraryDetails)]
        public string TaskId { get; set; }
        [Required(ErrorMessage = "spnDescriptionErrorMsg|" + LocalizeResourceSetConstants.LibraryDetails)]      
        public string Description { get; set; }
        public long MaintOnDemandMasterId { get; set; }
        public string ClientLookUpId { get; set; }
        public long? MaintOnDemandMasterTaskId { get; set; }
    }
}