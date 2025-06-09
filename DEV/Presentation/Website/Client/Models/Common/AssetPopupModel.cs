using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Common
{
    public class AssetPopupModel
    {
        public long EquipmentId;
        public string ClientLookUpId;
        public string Name;
        public string AssetType;
        public long DepartmentId;
        public string DeptDescription;
        public string DeptClientLookupId;
        public long LineId;
        public string LineDescription;
        public string LineClientLookupId;
        public long SystemInfoId;
        public string SystemDescription;
        public string SystemClientLookupId;
    }
}