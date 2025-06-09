namespace Client.Models.Configuration.PreventiveMaintenanceLibrary
{
    public class TaskModel
    {
        public long PrevMaintLibraryId { get; set; }
        public string ClientLookUpId { get; set; }
        public long? PrevMaintLibraryTaskId { get; set; }
        public string TaskId { get; set; }
        public string Description { get; set; }
    }
}