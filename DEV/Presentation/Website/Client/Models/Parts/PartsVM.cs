using Client.Models.Common;
using Client.Models.InventoryCheckout;
using Client.Models.Parts.UIConfiguration;
using Client.Models.PhysicalInventory;
using DataContracts;
using System.Collections.Generic;
using System.Web.Mvc;
//Add on 23/06/2020 Create Contact Chips for mention user in comment widget
using static Client.Models.Common.UserMentionDataModel;
//Add on 23/06/2020 Create Contact Chips for mention user in comment widget

namespace Client.Models.Parts
{
    public class PartsVM : LocalisationBaseVM
    {
        public PartsVM()
        {
            partSummaryModel = new PartSummaryModel();
        }   
        public PartModel PartModel { get; set; }
        public NotesModel notesModel { get; set; }
        public PartsVendorModel partsVendorModel { get; set; }
        public AttachmentModel attachmentModel { get; set; }
        public EquipmentPartXrefModel equipmentPartXrefModel { get; set; }
        public PartsReceiptModel partsReceiptModel { get; set; }
        public PartsHistoryModel partsHistoryModel { get; set; }
        public UserData _userdata { get; set; }
        public Security security { get; set; }
        public CreatedLastUpdatedPartModel createdLastUpdatedPartModel { get; set; }
        public ChangePartIdModel changePartIdModel { get; set; }
        public QRCodeModel qRCodeModel { get; set; }
        public PartSummaryModel partSummaryModel { get; set; }

        public PartTransferModel partTransferModel { get; set; }

        public PartMasterModel partMasterModel { get; set; }
        public ReviewSiteModel reviewSiteModel { get; set; }
        public PartBulkUpdateModel partBulkUpdateModel { get; set; }

        public string PackageLevel { get; set; }
        public bool UsePartMaster { get; set; }
        public bool IsPartDetailsFromEquipment { get; set; }
        public int attachmentCount { get; set; }
        public List<Notes> NotesList { get; set; }
        public List<PartsHistoryModel> partsHistoryModelList { get; set; }
        public List<UserMentionData> userMentionDatas { get; set; }
        public PartPhysicalInvModel inventoryModel { get; set; }
        public ParInvCheckoutModel inventoryCheckoutModel { get; set; }
        public InventoryCheckoutValidationModel inventoryCheckoutValidationModel { get; set; }
        public EquipmentTreeModel equipmentTreeModel { get; set; }
        public UserData userData { get; set; }
        public string PartStatusVal { get; set; }

        #region V2-641 Part Dynamic Ui Configuration
        public List<Client.Common.UIConfigurationDetailsForModelValidation> UIConfigurationDetails { get; set; }
        public IEnumerable<UILookupList> AllRequiredLookUplist { get; set; }
        public AddPartModelDynamic AddPart { get; set; }
        public bool PlantLocation { get; set; }
        public IEnumerable<SelectListItem> AccountList { get; set; }
        public EditPartModelDynamic EditPart { get; set; }
        #endregion

        #region V2-642 Dynamic PartDetail
        public ViewPartModelDynamic ViewPart { get; set; }
        #endregion

        #region V2-716
        public List<ImageAttachmentModel> imageAttachmentModels { get; set; }
        #endregion

        #region V2-836
        public List<PartModel> PartModelList { get; set; }
        #endregion
        #region V2-906
        public UpdatePartCostsModel UpdatePartCostsModel { get; set; }
        #endregion

        #region V2-1007
        public bool IsAddPartFromEquipment { get; set; }
        public string Equipment_ClientLookupId { get; set; }
        public long EquipmentId { get; set; }
        #endregion
        public long CurrentPartId { get; set; } //V2-1203
        public PartsConfigureAutoPurchasingModel partsConfigureAutoPurchasingModel { get; set; }

    }
}