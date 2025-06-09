using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.Equipment
{
    public class EquipmentBulkUpdateModel
    {
        public string EquipmentID { get; set; }
        
        public string Type { get; set; }
       
        public long? DeptID { get; set; }
        
        public long? LineID { get; set; }
        
        public long? SystemInfoId { get; set; }
       
        public long? Account { get; set; }
        public string LaborAccountClientLookupId { get; set; }
        public IEnumerable<SelectListItem> AccountList { get; set; }
        public IEnumerable<SelectListItem> LookupTypeList { get; set; }
        public IEnumerable<SelectListItem> LineList { get; set; }
        public IEnumerable<SelectListItem> DeptList { get; set; }
        public IEnumerable<SelectListItem> SystemList { get; set; }
        public string EquipmentIdList { get; set; }
       
        public long? AssetGroup1Id { get; set; }
        public long? AssetGroup2Id { get; set; }
        public long? AssetGroup3Id { get; set; }        
        public string Location { get; set; }
        #region V2-1158
        public string Model { get; set; }
        public string Make { get; set; }
        #endregion
    }
}