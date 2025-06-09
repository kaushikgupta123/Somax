using System;

namespace Client.Models.BusinessIntelligence
{
    [Serializable]
    public class ReportFilterProp
    {
        public string ColumnName { get; set; }
        public string Type { get; set; }
        public string Searchval { get; set; }
    }
}