using System;

namespace Client.Models.BBUKPISite
{
    public class BBUKPISitePrintModel
    {
        //Custom Column View
        public string Year { get; set; }
        public string Week { get; set; }
        public string siteName { get; set; }
        public DateTime Created { get; set; }
        public decimal PMWOCompleted { get; set; }
        public int pMFollowUpComp { get; set; }
        public int activeMechUsers { get; set; }
        public int WOBacklogCount { get; set; }
        public int rCACount { get; set; }
        public int tTRCount { get; set; }
        public decimal invValueOverMax { get; set; }
        public decimal phyInvAccuracy { get; set; }
        public decimal cycleCountProgress { get; set; }
        public decimal eVTrainingHrs { get; set; }
        public string weekStart { get; set; }
        public string weekEnd { get; set; }
    }
}