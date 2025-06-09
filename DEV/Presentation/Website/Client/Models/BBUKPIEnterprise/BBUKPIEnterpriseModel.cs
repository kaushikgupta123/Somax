using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Client.Models.BBUKPIEnterprise
{
    public class BBUKPIEnterpriseModel
    {
        public long ClientId { get; set; }
        public long SiteId { get; set; }
        public string SiteName { get; set; }
        public long BBUKPIId { get; set; }
        public string Week{ get; set; }
        public string Year { get; set; }
        public string Status { get; set; }
        public decimal PMWOCompleted { get; set; }
        public int WOBacklogCount { get; set; }
        public int RCACount { get; set; }
        public int TTRCount { get; set; }
        public decimal InvValueOverMax { get; set; }
        public decimal PhyInvAccuracy { get; set; }
        public decimal EVTrainingHrs { get; set; }
        public bool DownDaySched { get; set; }
        public int OptPMPlansCompleted { get; set; }
        public int OptPMPlansAdopted { get; set; }
        public decimal MLT { get; set; }
        public bool TrainingPlanImp { get; set; }
        public DateTime? SubmitDate { get; set; }
        public string SubmitBy_Name { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        #region V2-909
        public int PMFollowUpComp { get; set; }
        public int ActiveMechUsers { get; set; }
        public decimal CycleCountProgress { get; set; }
        public DateTime? WeekStart { get; set; }
        public DateTime? WeekEnd { get; set; }
        #endregion

    }
}