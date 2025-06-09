using System;

namespace Client.Models.BBUKPISite
{
    public class BBUKPISiteSearchGridModel
    {
        public long ClientId { get; set; }
        public long SiteId { get; set; }
        public long BBUKPIId { get; set; }
        public string Week{ get; set; }
        public string Year { get; set; }
        public string Status { get; set; }
        public DateTime Created { get; set; }
        public decimal PMWOCompleted { get; set; }
        public int WOBacklogCount { get; set; }
        public DateTime? SubmitDate { get; set; }
        #region//**V2-909
        public string weekEnd { get; set; }

        public string weekStart { get; set; }

        public decimal phyInvAccuracy { get; set; }

        public int pMFollowUpComp { get; set; }
        public int activeMechUsers { get; set; }
        public int rCACount { get; set; }
        public int tTRCount { get; set; }
        public decimal invValueOverMax { get; set; }
        public decimal cycleCountProgress { get; set; }
        public decimal eVTrainingHrs { get; set; }
        public string siteName { get; set; }

        #endregion
        public int TotalCount { get; set; }
    }
}