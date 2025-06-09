using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.UIConfig
{
    public class UIConfigModel
    {       
        public long ClientId { get; set; }
        public long UIConfigId { get; set; }
        public long SiteId { get; set; }
        public string ViewName { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public bool Required { get; set; }
        public bool Hide { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string ModifyBy { get; set; }
        public string IsHide { get; set; }
        public string IsRequired { get; set; }
        public bool IsExternal { get; set; }
        public bool Disable { get; set; }

    }
}