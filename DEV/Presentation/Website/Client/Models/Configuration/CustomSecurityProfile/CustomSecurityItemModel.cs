using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Configuration.CustomSecurityProfile
{
    public class CustomSecurityItemModel
    {
        public long ClientId { get; set; }
        public long SecurityItemId { get; set; }
        public long SecurityProfileId { get; set; }
        public string ItemName { get; set; }
        public string SecurityLocalizedName { get; set; }
        public int SortOrder { get; set; }
        public bool Protected { get; set; }
       
        public bool ItemAccess { get; set; }
        public bool ItemCreate { get; set; }
        public bool ItemEdit { get; set; }
        public bool ItemDelete { get; set; }
        public bool SingleItem { get; set; }
        public bool ReportItem { get; set; }
        public int UpdateIndex { get; set; }
       

    }
}