using System;

namespace Client.Models.BusinessIntelligence
{
    [Serializable]
    public class GridColumnsProp
    {
        public string data { get; set; }
        public string title { get; set; }
        public bool orderable { get; set; }
        public bool bSortable { get; set; }
        public bool bVisible { get; set; }
        public string className { get; set; }
        public bool IsGroupTotaled { get; set; }
        public bool IsGrandTotal { get; set; }
        public int NumofDecPlaces { get; set; }
        public string NumericFormat { get; set; }
        public string SiteLocalization { get; set; }
        public string CurrencyCode { get; set; }
        public bool AvailableOnFilter { get; set; }
        public string FilterMethod { get; set; }
        public int Sequence { get; set; }
        public string Filter { get; set; }
        public bool bRequired { get; set; }
        public bool DateDisplay { get; set; }
        #region V2-975
        public bool LocalizeDate { get; set; }
        #endregion
    }
}