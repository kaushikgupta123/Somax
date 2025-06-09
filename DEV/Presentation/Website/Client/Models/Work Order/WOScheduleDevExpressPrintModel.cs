using System;

namespace Client.Models.Work_Order
{
    public class WOScheduleDevExpressPrintModel
    {
        public string Assigned{ get; set; }
        public string ScheduledStartDate { get; set; }
        public decimal? ScheduledHours { get; set; }
        #region Localization
        public string spnGlobalAssigned { get; set; }
        public string spnDate { get; set; }
        public string spnDuration { get; set; }
        #endregion Localization
    }
}