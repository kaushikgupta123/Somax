namespace Client.Models.Work_Order
{
    public class TasksDevExpressPrintModel
    {
        public long WorkOrderTaskId { get; set; }
        public string TaskNumber { get; set; }
        public string Description { get; set; }
        public decimal? ScheduledDuration { get; set; }
        public string Completed { get; set; }
        #region Localization
        public string spnTasks { get; set; }
        public string spnOrder { get; set; }
        public string spnSchedDuration { get; set; }
        public string spnGlobalCompleted { get; set; }
        public string spnDescription { get; set; }
        #endregion Localization
    }
}