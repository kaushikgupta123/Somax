using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Client.Models.BBUKPIEnterprise
{
    public class BBUKPIEnterprisePrintModel
    {
        //Custom Column View
        public string Year { get; set; }
        public string WeekNumber { get; set; }
        public string SiteName { get; set; }
        public DateTime Created { get; set; }
        public decimal PMPercentCompleted { get; set; }
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