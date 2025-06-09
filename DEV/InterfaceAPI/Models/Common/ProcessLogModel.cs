namespace InterfaceAPI.Models.Common
{
    public class ProcessLogModel
    {
        public int TotalProcess { get; set; }
        public int SuccessfulProcess { get; set; }
        public int FailedProcess { get; set; }
        public string logMessage { get; set; }
        public int NewProcess { get; set; }
    }
}