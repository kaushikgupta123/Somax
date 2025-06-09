using Client.Models.Common;
using Client.Models.PunchoutModel;
using Client.Models.PurchaseOrder;
using Client.Models.PurchaseRequest.UIConfiguration;
using Client.Models.UIConfig;
using DataContracts;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Client.Models.PurchaseRequest
{
    public class PurchaseRequestVM : LocalisationBaseVM
    {
        public PurchaseRequestVM()
        {
            tableHaederProps = new List<TableHaederProp>();
            purchaseRequestPDFPrintModel = new List<PurchaseRequestPDFPrintModel>();
            shoppingList = new List<ShoppingCartImportDataModel>();
        }
        public PurchaseRequestModel purchaseRequestModel { get; set; }
        public PurchaseRequestPunchOutModel purchaseRequestPunchOutModel { get; set; }
        public PurchaseRequestEmailModel prEmailModel { get; set; }
        public AttachmentModel attachmentModel { get; set; }
        public NotesModel notesModel { get; set; }
        public LineItemModel lineItem { get; set; }
        public PartInInventoryModel partInInventoryModel { get; set; }
        public PartNotInInventoryModel PartNotInInventoryModel { get; set; }
        public LiEquipmentTreeModel liEquipmentTreeModel { get; set; }
        public List<LineItemModel> listLineItem { get; set; }
        public Security security { get; set; }
        public UserData udata { get; set; }
        public int attachmentCount { get; set; }
        public List<string> hiddenColumnList { get; set; }
        public List<string> requiredColumnList { get; set; }
        public List<string> disabledColumnList { get; set; }
        public List<UIConfigModel> UIConfigModelList { get; set; }
        public List<PurchaseRequestPDFPrintModel> purchaseRequestPDFPrintModel { get; set; }
        public List<TableHaederProp> tableHaederProps { get; set; }
        public bool isActiveInterface { get; set; }
        public bool ipropInUse { get; set; }
        public List<ShoppingCartImportDataModel> shoppingList { get; set; }
        public bool IsPunchOutCheckOut { get; set; }
        public List<Client.Common.UIConfigurationDetailsForModelValidation> UIConfigurationDetails { get; set; }
        public AddPurchaseRequestModelDynamic AddPurchaseRequest { get; set; }
        public EditPurchaseRequestModelDynamic EditPurchaseRequest { get; set; }
        public ViewPurchaseRequestModelDynamic ViewPurchaseRequest { get; set; }
        public AddPRLineItemPartNotInInventoryModelDynamic AddPRLineItemPartNotInInventory { get; set; }
        public IEnumerable<SelectListItem> ChargeTypeList { get; set; }
        public IEnumerable<UILookupList> AllRequiredLookUplist { get; set; }
        public EditPRLineItemPartInInventoryModelDynamic EditPRLineItemPartInInventory { get; set; }
        public EditPRLineItemPartNotInInventoryModelDynamic EditPRLineItemPartNotInInventory { get; set; }
        #region V2-726
        public ApprovalRouteModel ApprovalRouteModel { get; set; }
        #endregion
        public bool IsSendToSAP { get; set; } //V2-693
        public IEnumerable<SelectListItem> StoreroomList { get; set; } //738
        #region V2-730
        public bool IsPurchaseRequestApproval { get; set; } //V2-730
        public bool IsPurchaseRequestApprovalAccessCheck { get; set; } //V2-730
        public ApprovalRouteModelByObjectId ApprovalRouteModelByObjectId { get; set; }
        #endregion
        #region V2-820
        public ReviewPRSendForApprovalModel reviewPRSendForApprovalModel { get; set; }
        #endregion
        #region V2-1032
        public AddPRLineItemPartInInventoryModelDynamic AddPRLineItemPartInInventory { get; set; }
        public EditPRLineItemPartInInventorySingleStockModelDynamic EditPRLineItemPartInInventorySingleStock { get; set; }
        #endregion
        public bool IsOraclePurchaseRequestExportInUse { get; set; } //V2-1119
        #region V2-1112
        public IEnumerable<SelectListItem> ShipToList { get; set; } 
        public AddCustomPurchaseOrderModel addCustomPurchaseOrder { get; set; }
        #endregion

        public bool IsPunchOutCheckOutTab { get; set; } // RKL Mail for close the vendor punchout website and send user back to the original tab
    }

}
