using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Client.Models.Configuration.UIConfiguration
{
    public class ColumnSettingConfigModel
    {
        public long DataDictionaryId { get; set; }
        public string ColumnName { get; set; }
        [Required(ErrorMessage = "Please enter Description")]
        public string ColumnLabel { get; set; }
        public bool Required { get; set; }
        public bool UDF { get; set; }
        public string ColumnType { get; set; }
        public string ListName { get; set; }

        public long UIConfigurationId { get; set; }
        public bool DisplayonForm { get; set; } //V2-944
    }
}