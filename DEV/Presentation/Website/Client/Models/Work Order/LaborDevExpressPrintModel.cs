using DevExpress.CodeParser;
using System;

namespace Client.Models.Work_Order
{
    public class LaborDevExpressPrintModel
    {
        public long TimecardId { get; set; }
        public long PersonnelID { get; set; }
        public string PersonnelClientLookupId { get; set; }
        public string PersonnelName{ get; set; }
        public string StartDate { get; set; }
        public decimal? Hours { get; set; }
        public decimal? UnitCost { get; set; }
        #region Localization
        public string spnLabor { get; set; }
        public string spnPersonnelID { get; set; }
        public string spnName { get; set; }
        public string spnDate { get; set; }
        public string spnHours { get; set; }
        public string spnUnitCost { get; set; }
        #region V2-944
        public string spnTotalCost { get; set; }
        #endregion
        #endregion Localization
    }
}