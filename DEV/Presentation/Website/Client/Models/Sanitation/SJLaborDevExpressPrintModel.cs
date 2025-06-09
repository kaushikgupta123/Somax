namespace Client.Models.Sanitation
{
    public class SJLaborDevExpressPrintModel
    {
        public long SanitationJobId { get; set; }
        public long TimecardId { get; set; }
        public long PersonnelID { get; set; }
        public string PersonnelClientLookupId { get; set; }
        public string StartDate { get; set; }
        public string PersonnelName { get; set; }
        public decimal? Hours { get; set; }

        #region Localization
        public string spnLabor { get; set; }
        public string spnPersonnel { get; set; }
        public string spnStartDate { get; set; }
        public string spnName { get; set; }
        public string spnHours { get; set; }
        #endregion Localization
    }
}
