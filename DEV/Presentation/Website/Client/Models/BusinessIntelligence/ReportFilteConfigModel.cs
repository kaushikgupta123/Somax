namespace Client.Models.BusinessIntelligence
{
    public class ReportFilteConfigModel
    {
        public string ColumnName { get; set; }
        private string filter;
        public string Filter { get { return filter; } set { filter = string.IsNullOrEmpty(value) ? "" : value; } }
    }
}