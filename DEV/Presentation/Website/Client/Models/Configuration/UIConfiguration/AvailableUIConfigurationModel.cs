using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Configuration.UIConfiguration
{
    public class AvailableUIConfigurationModel
    {
        public long UIConfigurationId { get; set; }
        public string ColumnName { get; set; }
        public string ColumnLabel { get; set; }
        public int Order { get; set; }
        public bool Required { get; set; }
        public bool SystemRequired { get; set; }
        public long DataDictionaryId { get; set; }
        public bool Section { get; set; }
        public string SectionName { get; set; }
        public bool UDF { get; set; }
    }
}