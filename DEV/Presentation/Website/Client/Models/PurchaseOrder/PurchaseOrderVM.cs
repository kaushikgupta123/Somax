using Client.Models.Common;
using Client.Models.PurchaseOrder.UIConfiguration;

using DataContracts;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Client.Models.PurchaseOrder
{
    public class PurchaseOrderVM : LocalisationBaseVM
    {
        public PurchaseOrderVM()
        {
            purchaseOrderPDFPrintModels = new List<PurchaseOrderPDFPrintModel>();
            tableHaederProps = new List<TableHaederProp>();
        }
        public POSummaryModel POSummaryModel { get; set; }
        public PurchaseOrderModel PurchaseOrderModel { get; set; }
        public NotesModel notesModel { get; set; }
        public AttachmentModel attachmentModel { get; set; }
        public POLineItemModel POLineItemModel { get; set; }

        public List<POLineItemModel> POLineItemList { get; set; }
        public LineItem lineItem { get; set; }
        public PartInInventoryModel partInInventoryModel { get; set; }
        public InnerGridLineItemModel LineItem;
        public List<InnerGridLineItemModel> LineItemList { get; set; }
        public PurchaseOrderReceiptModel purchaseOrderReceiptModel { get; set; }
        public PurchaseOrderReceiptLineItemModel purchaseOrderReceiptLineItemModel { get; set; }
        public QRCodeModel qRCodeModel { get; set; }
        public PurchaseOrderEmailModel POEmailModel { get; set; }
        public POPrintReceiptModel poPrintReceiptmodel { get; set; }
        public Security security { get; set; }
        public int attachmentCount { get; set; }
        public List<PurchaseOrderPDFPrintModel> purchaseOrderPDFPrintModels { get; set; }
        public List<TableHaederProp> tableHaederProps { get; set; }      
        public bool Switch1 { get; set; }
        public bool InUse { get; set; }
        #region V2-653 Dynamic  UIConfiguration
        public List<Client.Common.UIConfigurationDetailsForModelValidation> UIConfigurationDetails { get; set; }
        public IEnumerable<UILookupList> AllRequiredLookUplist { get; set; }
        public AddPurchaseOrderModelDynamic addPurchaseOrder { get; set; }
        public bool IsPurchaseOrderDynamic { get; set; }
        public IEnumerable<SelectListItem> BuyerList { get; set; }
        public ViewPurchaseOrderModelDynamic ViewPurchaseOrder { get; set; }
        public EditPurchaseOrderModelDynamic EditPurchaseOrder { get; set; }
        public AddPOLineItemPartNotInInventoryDynamic AddPOLineItemPartNotInInventory { get; set; }
        public IEnumerable<SelectListItem> ChargeTypeList { get; set; }
        //public IEnumerable<SelectListItem> ChargeToList { get; set; }
        public EditPOLineItemPartNotInInventoryModelDynamic EditPOLineItemPartNotInInventory { get; set; }
        public EditPOLineItemPartInInventoryModelDynamic EditPOLineItemPartInInventory { get; set; }
        #endregion
        #region  V2-738
        public IEnumerable<SelectListItem> StoreroomList { get; set; }
        public UserData udata { get; set; }
        #endregion
        #region V2-796
        public bool OraclePOImportInUse { get; set; }
        public PurchaseOrderUpdateModel PurchaseOrderUpdateModel { get; set; }
        #endregion

        #region V2-810
        public POLiEquipmentTreeModel pOLiEquipmentTreeModel { get; set; }
        #endregion
        #region V2-1032
        public AddPOLineItemPartInInventoryModelDynamic AddPOLineItemPartInInventory { get; set; }
        public EditPOLineItemPartInInventorySingleStockModelDynamic EditPOLineItemPartInInventorySingleStock { get; set; }
        #endregion

        public IEnumerable<SelectListItem> ShipToList { get; set; } //V2-1086
        public bool EPMInvoiceImportInUse { get; set; } //V2-1115
        public AddCustomPurchaseOrderModel addCustomPurchaseOrder { get; set; } //V2-1112
        public IEnumerable<SelectListItem> ShipToClientLookupList { get; set; } //V2-1112
    }
}