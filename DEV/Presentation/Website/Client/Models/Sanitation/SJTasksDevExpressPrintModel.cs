namespace Client.Models.Sanitation
{
    public class SJTasksDevExpressPrintModel
    {
        public long SanitationJobId { get; set; }
        public string TaskId { get; set; }
        public string Description { get; set; }
        public string Completed { get; set; }
        public string Value { get; set; }

        #region Localization
        public string spnTasks { get; set; }
        public string spnOrder { get; set; }
        public string spnDuration { get; set; }
        public string spnGlobalCompleted { get; set; }
        public string spnValue { get; set; }
        public string spnDescription { get; set; }
        #endregion Localization
    }
}
