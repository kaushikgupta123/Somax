using Client.Models.Personnel;

using DataContracts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models
{
    public class PersonnelVM : LocalisationBaseVM
    {
        public PersonnelVM()
        {
            AssetGroup1List = new List<SelectListItem>();
            AssetGroup2List = new List<SelectListItem>();
            AssetGroup3List = new List<SelectListItem>();
        }
        public PersonnelModel personnelModel { get; set; }
        public AttachmentModel attachmentModel { get; set; }
        public EventsModel eventmodel { get; set; }
        public IEnumerable<SelectListItem> EventTypeList { get; set; }

        public PersonnelAvailabilityModel personnelAvailabilityModel { get; set; }
        public PersonnelAttendanceModel personnelAttendanceModel { get; set; }
        public IEnumerable<SelectListItem> ShiftList { get; set; }
        public AuxiliaryInformationModel auxiliaryInformation { get; set; }
        
        public Security security { get; set; }
        public string AssetGroup1Label { get; set; } // V2-630
        public string AssetGroup2Label { get; set; } // V2-630
        public string AssetGroup3Label { get; set; } // V2-630
        public bool UseAssetGroupMasterQuery { get; set; } // V2-630
        public AssetGroupMasterQuery assetGroupMasterQuery { get; set; } // V2-630
        public IEnumerable<SelectListItem> AssetGroup1List { get; set; } // V2-630
        public IEnumerable<SelectListItem> AssetGroup2List { get; set; } // V2-630
        public IEnumerable<SelectListItem> AssetGroup3List { get; set; } // V2-630
        public IEnumerable<SelectListItem> InactiveFlagList { get; set; }//V2-1098

        public UserData udata { get; set; }
    }
}