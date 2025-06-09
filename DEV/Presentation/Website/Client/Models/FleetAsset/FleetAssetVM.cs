using Client.Models.Configuration.LookupLists;
using DataContracts;
using PagedList;
using System.Collections.Generic;
using System.Web.Mvc;
using static Client.Models.Common.UserMentionDataModel;

namespace Client.Models.FleetAsset
{
    public class FleetAssetVM : LocalisationBaseVM
    {
        public FleetAssetVM()
        {
            _FleetAssetSummaryModel = new FleetAssetSummaryModel();
            FleetAssetData = new DataContracts.Equipment();
        }
        public FleetAssetSummaryModel _FleetAssetSummaryModel { get; set; }      
        public DataContracts.Equipment FleetAssetData { get; set; }       
        public Security security { get; set; }
        public UserData _userdata { get; set; }
        public string Mode { get; set; }
        public string HiddenType { get; set; }
        public IEnumerable<SelectListItem> LookupVehicleTypeList { get; set; }
        public IEnumerable<SelectListItem> LookupFuelTypeList { get; set; }
        public FleetAssetModel FleetAssetModel { get; set; }       
        public AttachmentModel attachmentModel { get; set; }
     
        public IEnumerable<SelectListItem> workOrderTypeDateList { get; set; }
        public CreatedLastUpdatedFleetAssetModel _CreatedLastUpdatedFleetAssetModel { get; set; }
        public ChangeFleetAssetIDModel _ChangeFleetAssetIDModel { get; set; }      
        public int PageNumber { get; set; }      
        public int attachmentCount { get; set; }
        public PartsSessionData partsSessionData { get; set; }
        public List<Notes> NotesList { get; set; }
        public IEnumerable<SelectListItem> InactiveFlagList { get; set; }    
        public List<UserMentionData> userMentionDatas { get; set; }
        public FleetAssetQRCodeModel qRCodeModel { get; set; }

        public IEnumerable<SelectListItem> LookupFleetFuelUnits { get; set; }
        public IEnumerable<SelectListItem> LookupFleetMeter1Types { get; set; }
        public IEnumerable<SelectListItem> LookupFleetMeter1Units { get; set; }
        public IEnumerable<SelectListItem> LookupFleetMeter2Types { get; set; }
        public IEnumerable<SelectListItem> LookupFleetMeter2Units { get; set; }
        public IEnumerable<SelectListItem> LookupFleetReadingSourceType { get; set; }

        public AssetAvailabilityModel _AssetAvailabilityModel { get; set; }
        public IEnumerable<SelectListItem> LookupAssetAvailability { get; set; }


    }
}