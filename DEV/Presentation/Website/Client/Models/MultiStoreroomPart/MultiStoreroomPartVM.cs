using Client.Models.Common;
using Client.Models.MultiStoreroomPart.UIConfiguration;
using Client.Models.Parts;

using DataContracts;

using System.Collections.Generic;
using System.Web.Mvc;
//Add on 23/06/2020 Create Contact Chips for mention user in comment widget
using static Client.Models.Common.UserMentionDataModel;
//Add on 23/06/2020 Create Contact Chips for mention user in comment widget


namespace Client.Models.MultiStoreroomPart
{
    public class MultiStoreroomPartVM : LocalisationBaseVM
    {
        public MultiStoreroomPartVM()
        {
            tableHaederProps = new List<TableHaederProp>();
            multiStoreroomPartPDFPrintModel = new List<MultiStoreroomPartPDFPrintModel>();
        }
        public Security security { get; set; }
        public MultiStoreroomPartModel MultiStoreroomPartModel { get; set; }
        public MultiStoreroomPartChildModel MultiStoreroomPartChildModel { get; set; }
        public List<MultiStoreroomPartChildModel> MultiStoreroomPartChildModelList { get; set; }
        public MultiStoreroomPartSummaryModel multiStoreroomSummaryModel { get; set; }
        public UserData _userdata { get; set; }
        public QRCodeModel QRCodeModel { get; set; }
        //public CreatedLastUpdatedPartForMultiStoreroomPartModel CreatedLastUpdatedPartModel { get; set; }
        public ChangePartIdForMultiPartStoreroomPartModel ChangePartIdModel { get; set; }
        public List<TableHaederProp> tableHaederProps { get; set; }
        public List<MultiStoreroomPartPDFPrintModel> multiStoreroomPartPDFPrintModel { get; set; }
        public int AttachmentCount { get; set; }
        public string PackageLevel { get; set; }
        public bool UsePartMaster { get; set; }
        public StoreroomModel StoreroomModel { get; set; }
        public IEnumerable<SelectListItem> StoreroomList { get; set; }
        public List<UserMentionData> userMentionDatas { get; set; }
        public List<Notes> NotesList { get; set; }
        public AttachmentModel AttachmentModel { get; set; }
        public MultiStoreroomPartVendorModel MSPVendorModel { get; set; }
        public MultiStoreroomPartEquipmentXrefModel MSPEquipmentXrefModel { get; set; }
        public MultiStoreroomPartHistoryModel MSPHistoryModel { get; set; }

        public MultiStoreroomReceiptModel MSPReceiptModel { get; set; }
        public AddPartTransferRequest addPartTransferRequest { get; set; }
        public AddToAutoTransfer addToAutoTransfer { get; set; }
        public bool IsMaintain { get; set; }
        #region V2-716
        public List<ImageAttachmentModel> imageAttachmentModels { get; set; }
        #endregion

        #region V2-755
        public ParInvCheckoutModel inventoryCheckoutModel { get; set; } //V2-755
        public MultiStoreroomPartPhysicalInvModel inventoryModel { get; set; } //V2-755
        #endregion

        #region V2-1007
        public bool IsAddPartFromEquipment { get; set; }
        public string Equipment_ClientLookupId { get; set; }
        public long EquipmentId { get; set; }
        #endregion
        #region V2-1025
        public long DefaultStoreroom { get; set; }
        public StoreroomInnerChildModel StoreroomInnerChild{ get; set; }
        #endregion

        #region V2-1045
        public MultiStoreroomPartClientLookupIdModel multiStoreroomPartClientLookupIdModel { get; set; }
        #endregion
        public bool EPMInvoiceImportInUse { get; set; } //V2-1115

        #region V2-1187 Part Dynamic Ui Configuration
        public List<Client.Common.UIConfigurationDetailsForModelValidation> UIConfigurationDetails { get; set; }
        public IEnumerable<UILookupList> AllRequiredLookUplist { get; set; }
        public AddPartModelDynamic AddPart { get; set; }
        public IEnumerable<SelectListItem> AccountList { get; set; }
        public EditPartModelDynamic EditPart { get; set; }
        public ViewPartModelDynamic ViewPart { get; set; }
        #endregion

        public long CurrentPartId { get; set; } //V2-1203
        #region V2-1196
        public PartsConfigureAutoPurchasingModel partsConfigureAutoPurchasingModel { get; set; }
        #endregion

    }
}