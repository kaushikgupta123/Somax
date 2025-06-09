using Client.Models.Common;
using Client.Models.Configuration.LookupLists;
using Client.Models.Equipment;
using DataContracts;
using PagedList;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
//Add on 23/06/2020 Create Contact Chips for mention user in comment widget
using static Client.Models.Common.UserMentionDataModel;
//Add on 23/06/2020 Create Contact Chips for mention user in comment widget
using Client.Models.Equipment.UIConfiguration;
using Client.Models.Parts;

namespace Client.Models
{
    public class EquipmentCombined : LocalisationBaseVM
    {
        public EquipmentCombined()
        {
            _EquipmentSummaryModel = new EquipmentSummaryModel();
            EquipData = new DataContracts.Equipment();
            UIConfigurationDetails = new List<Client.Common.UIConfigurationDetailsForModelValidation>();
            AssetGroup1List = new List<SelectListItem>();
            AssetGroup2List = new List<SelectListItem>();
            AssetGroup3List = new List<SelectListItem>();
            AllRequiredLookUplist = new List<UILookupList>();
            AddEquipment = new Equipment.UIConfiguration.AddEquipmentModelDynamic();
            AddAssetOperation = false;
        }
        public EquipmentSummaryModel _EquipmentSummaryModel { get; set; }
        //public List<DataContracts.Equipment> EquipDataList { get; set; }
        public DataContracts.Equipment EquipData { get; set; }
        //public LookupList LookupData { get; set; }
        //public EquipmentModel EquipModelData { get; set; }
        //public long HiddenAccountID { get; set; }
        //public long HiddenVendorMainId { get; set; }
        public Security security { get; set; }
        public UserData _userdata { get; set; }
        public string Mode { get; set; }
        public string HiddenType { get; set; }
        public IEnumerable<SelectListItem> AccountList { get; set; }
        public IEnumerable<SelectListItem> LookupTypeList { get; set; }
        public IEnumerable<SelectListItem> VendorList { get; set; }
        public IEnumerable<SelectListItem> AssetCategoryList { get; set; }
        public EquipmentModel EquipModel { get; set; }
        public TechSpecsModel techSpecsModel { get; set; }
        public PartsSessionData partsSessionData { get; set; }
        public DownTimeModel downTimeModel { get; set; }
        public AttachmentModel attachmentModel { get; set; }

        // public notesModel 
        public NotesModel notesModel { get; set; }
        //public EquipmentPrintModel equipmentPrintModel { get; set; }
        public IEnumerable<SelectListItem> equipmentDownTimeDateList { get; set; }
        public string equipDropdown { get; set; }
        public IEnumerable<SelectListItem> workOrderTypeDateList { get; set; }
        public string wotypeDropdown { get; set; }
        public CreatedLastUpdatedModel _CreatedLastUpdatedModel { get; set; }
        public ChangeEquipmentIDModel _ChangeEquipmentIDModel { get; set; }
        public EqEquipmentTreeModel eqEquipmentTreeModel { get; set; }
        public SensorGridDataModel sensorGridDataModel { get; set; }
        public IPagedList<NotesModel> ListNotesModel { get; set; }
        public int PageNumber { get; set; }
        public QRCodeModel qRCodeModel { get; set; }
        public List<ProcessSystemTreeModel> processSystemTreeModel { get; set; }

        public IEnumerable<SelectListItem> LineList { get; set; }
        public IEnumerable<SelectListItem> DeptList { get; set; }
        public IEnumerable<SelectListItem> SystemList { get; set; }
        public EquipmentBulkUpdateModel equipmentBulkUpdateModel { get; set; }
        public int attachmentCount { get; set; }
        public List<Notes> NotesList { get; set; }
        public IEnumerable<SelectListItem> InactiveFlagList { get; set; }
        public IEnumerable<SelectListItem> AssetCategory1List { get; set; }
        public IEnumerable<SelectListItem> AssetCategory2List { get; set; }
        public IEnumerable<SelectListItem> AssetCategory3List { get; set; }

        //Add on 23/06/2020 Create Contact Chips for mention user in comment widget
        public List<UserMentionData> userMentionDatas { get; set; }

        //Add on 23/06/2020 Create Contact Chips for mention user in comment widget

        // V2-611
        public List<Client.Common.UIConfigurationDetailsForModelValidation> UIConfigurationDetails { get; set; }
        public IEnumerable<UILookupList> AllRequiredLookUplist { get; set; }
        public AddEquipmentModelDynamic AddEquipment { get; set; }
        public EditEquipmentModelDynamic EditEquipment { get; set; }
        public ViewEquipmentModelDynamic ViewEquipment { get; set; }
        public IEnumerable<SelectListItem> AssetGroup1List { get; set; }
        public IEnumerable<SelectListItem> AssetGroup2List { get; set; }
        public IEnumerable<SelectListItem> AssetGroup3List { get; set; }
        public bool PlantLocation { get; set; }
        public string BusinessType { get; set; }
        public string AssetGroup1Label { get; set; }
        public string AssetGroup2Label { get; set; }
        public string AssetGroup3Label { get; set; }
        public bool AddAssetOperation { get; set; }
        // V2-611
        #region Asset Availability
        public bool IsAssetAvailability { get; set; }
        public DateTime RemoveServiceDate { get; set; }
        public IEnumerable<SelectListItem> LookupRemoveFromServiceReasonCode { get; set; }
        public AssetAvailabilityRemoveModel _AssetAvailabilityRemoveModel { get; set; }
        public IEnumerable<SelectListItem> LookupAssetAvailability { get; set; }
        public string ServiceReasonCode { get; set; }
        public AssetAvailabilityUpdateModel _AssetAvailabilityUpdateModel { get; set; }
        #endregion

        //V2-637
        public RepairableSpareModel RepairableSpareModel { get; set; }
        public List<SelectListItem> RepairableSpareStatusList { get; set; }
        public bool IsAssigned { get; set; }
        public AssignmentModel assignmentModel { get; set; }
        //

        //#endregion
        //V2-695
        public List<SelectListItem> ReasonForDownList { get; set; }

        #region V2-716
        public List<ImageAttachmentModel> imageAttachmentModels { get; set; }
        #endregion
        public PartModel PartModel { get; set; }
        public List<EquipmentSearchModel> EquipmentCardList { get; set; }
        public EquipmentSearchModel EquipmentDetailsCard { get; set; }
        public bool EPMInvoiceImportInUse { get; set; } //V2-1115
        #region V2-1202 
        public bool Copy_Part_Xref { get; set; }
        public bool Copy_TechSpecs { get; set; }
        public bool Copy_Notes { get; set; }
        #endregion
    }
    public class UILookupList
    {
        public string text { get; set; }
        public string value { get; set; }
        public string lookupname { get; set; }
    }
}
