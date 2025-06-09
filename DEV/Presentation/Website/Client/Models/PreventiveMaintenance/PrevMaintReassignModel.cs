using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.PreventiveMaintenance
{
    public class PrevMaintReassignModel
    {     
        [Required()]
        public long PersonnelId { get; set; }
        public IEnumerable<SelectListItem> PersonnelIdList { get; set; }
        public long LocationId { get; set; }
        public IEnumerable<SelectListItem> LocationIdList { get; set; }
        public long EquipmentId { get; set; }
        public IEnumerable<SelectListItem> EquipmentIdList { get; set; }
        public string PrevMainIdsList { get; set; }
        #region V2-977
        public string PMSchedAssignIdsList { get; set; }
        #endregion
    }
}