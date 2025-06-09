using System;
using System.Collections.Generic;

namespace ViewModels
{
    [Serializable()]
    public class MenuVM
    {  
        public MenuVM ()
        {
            Childlist = new List<MenuVM>();
        }
        public Int64 ClientId { get; set; }
        public Int64 MenuId { get; set; }
        public Int64 SecurityItemId { get; set; }
        public Int64 UserInfoId { get; set; }
        public string MenuName { get; set; }
        public Int64 ParentMenuId { get; set; }
        public Int64 MenuLevel { get; set; }
        public string MenuUrl { get; set; }
        public Int32 MenuPosition { get; set; }
        public string ToolTip { get; set; }
        public string CssClass { get; set; }
        public bool itemAccess { get; set; }
        public string LocalizedName { get; set; }
        public string LocaleId { get; set; }
        public string MenuType { get; set; }
        public List<MenuVM> Childlist { get; set; }
    }
    
}