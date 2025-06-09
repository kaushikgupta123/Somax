using System.ComponentModel.DataAnnotations;
namespace Client.Models.Configuration.MasterSanitationLibrary
{
    public class TaskModel
    {      
        public string TaskId { get; set; }     
        public string Description { get; set; }
        public long MasterSanLibraryId { get; set; }
        public string ClientLookUpId { get; set; }
        public long? MasterSanLibraryTaskId { get; set; }
    }
}