namespace Client.Models.Common
{
    public class UIConfigurationDetailsModel
    {
        public long UIConfigurationId { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string ColumnLabel { get; set; }
        public string ColumnType { get; set; }
        public bool Required { get; set; }
        public string LookupType { get; set; }
        public string LookupName { get; set; }
        public bool UDF { get; set; }
        public bool Enabled { get; set; }
        public bool SystemRequired { get; set; }
        public int Order { get; set; }
        public bool Display { get; set; }
        public bool ViewOnly { get; set; }
        public bool Section { get; set; }
        public string SectionName { get; set; }
    }
}