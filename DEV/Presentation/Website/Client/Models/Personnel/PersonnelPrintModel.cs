using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Personnel
{
    public class PersonnelPrintModel
    {
        public string ClientLookupId { get; set; }
        public string Name { get; set; }
        public string ShiftDescription { get; set; }
        public string CraftClientLookupId { get; set; }
        #region 1108
        public string AssetGroup1Names { get; set; }
        public string AssetGroup2Names { get; set; }
        public string AssetGroup3Names { get; set; }
        #endregion

    }
}