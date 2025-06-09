using Common.Constants;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.Personnel
{
    public class PersonnelModel
    {
        public PersonnelModel()
        {
            DeptList = new List<SelectListItem>();
            CraftList = new List<SelectListItem>();
            CrewList = new List<SelectListItem>();
            ShiftList = new List<SelectListItem>();
            ScheduleGroupList = new List<SelectListItem>();
            StoreroomList = new List<SelectListItem>();
        }
        public long ClientId { get; set; }
        public long SiteId { get; set; }
        public long PersonnelId { get; set; }
        public string ClientLookupId { get; set; }
        [Required(ErrorMessage = "GlobalFirstNameErrorMessage|" + LocalizeResourceSetConstants.Global)]
        public string FirstName { get; set; }
        public string LastName { get; set; }      
        public string MiddleName { get; set; }

        private string _Name;
        public string Name { get { return FirstName + " " + LastName; } set { _Name = Name; } }
        public string Shift { get; set; }
        public string ShiftDescription { get; set; }
        public string Crew { get; set; }
        public string CrewDescription { get; set; }
        public long? CraftId { get; set; }
        public string CraftClientLookupId { get; set; }
        public string ScheduleGroup { get; set; }
        public string ScheduleGroupDescription { get; set; }

        public int TotalCount { get; set; }
        public IEnumerable<SelectListItem> DeptList { get; set; }
        public IEnumerable<SelectListItem> CraftList { get; set; }
        public IEnumerable<SelectListItem> CrewList { get; set; }
        public IEnumerable<SelectListItem> ShiftList { get; set; }
        public IEnumerable<SelectListItem> ScheduleGroupList { get; set; }
        public IEnumerable<SelectListItem> StoreroomList { get; set; } //V2-1178
        public string DepartmentDescription { get; set; }
        public long? Deptid { get; set; }
        public string CraftDescription { get; set; }
        public bool Planner { get; set; }
        public int UpdateIndex { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string AssetGroup1Names { get; set; }
        public string AssetGroup2Names { get; set; }
        public string AssetGroup3Names { get; set; }
        public bool ScheduleEmployee { get; set; }
        public string ExternalId { get; set; } //V2-831
        public bool InactiveFlag { get; set; } //V2-1098
        #region V2-1108
        public long? AssignedAssetGroup1 { get; set; }
        public long? AssignedAssetGroup2 { get; set; }
        public long? AssignedAssetGroup3 { get; set; }
        public string AssignedAssetGroup1Names { get; set; }
        public string AssignedAssetGroup2Names { get; set; }
        public string AssignedAssetGroup3Names { get; set; }
        #endregion
        public string DefaultStoreroom { get; set; } //V2-1178
        public long? Default_StoreroomId { get; set; } //V2-1178
    }
}