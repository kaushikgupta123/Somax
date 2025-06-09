using Common.Constants;

using System;
using System.ComponentModel.DataAnnotations;

namespace Client.Models.BBUKPISite
{
    public class BBUKPISiteEditModel
    {
        public long ClientId { get; set; }
        public long SiteId { get; set; }
        //public string SiteName { get; set; }
        public long BBUKPIId { get; set; }
        public string Week{ get; set; }
        public string Year { get; set; }
        public string Status { get; set; }
        //public decimal PMWOCompleted { get; set; }
        //public int WOBacklogCount { get; set; }
        //public int RCACount { get; set; }
        //public int TTRCount { get; set; }
        //public decimal InvValueOverMax { get; set; }
        //public decimal PhyInvAccuracy { get; set; }
        //public decimal EVTrainingHrs { get; set; }
        public bool DownDaySched { get; set; }
        public int? OptPMPlansCompleted { get; set; }
        public int? OptPMPlansAdopted { get; set; }
        [RegularExpression(@"^\d*\.?\d{0,3}$", ErrorMessage = "globalThreeDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0.000, 99.999, ErrorMessage = "GlobalValidDecimalNoWithMaximum3DecimalPlacesAnd5DigitsinTotal|" + LocalizeResourceSetConstants.Global)]
        public decimal? MLT { get; set; }
        public bool TrainingPlanImp { get; set; }
        //public DateTime? SubmitDate { get; set; }
        //public string SubmitBy_Name { get; set; }
        //public string CreateBy { get; set; }
        //public DateTime? CreateDate { get; set; }
        //public string ModifyBy { get; set; }
        //public DateTime? ModifyDate { get; set; }

    }
}