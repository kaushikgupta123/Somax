using Client.CustomValidation;
using Common.Constants;
using DataContracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace Client.Models.PurchaseOrder
{
    public class PurchaseOrderModel
    {
        public long PurchaseOrderId { get; set; }
        public long? SiteId { get; set; }
        public long? DepartmentId { get; set; }
        public long? AreaId { get; set; }
        public long? StoreroomId { get; set; }
        public string ClientLookupId { get; set; }

        //[Required(ErrorMessage = "PoAttention|" + LocalizeResourceSetConstants.PurchaseOrder)]    //--V2-178
        [RequiredAsPerUI(Client.Common.UiConfigConstants.PurchaseOrderAdd, Client.Common.UiConfigConstants.PurchaseOrderEdit, "PurchaseOrderId", "0", "IsExternal", ErrorMessage = "PoAttention|" + LocalizeResourceSetConstants.PurchaseOrder)]
        public string Attention { get; set; }
        [RequiredAsPerUI(Client.Common.UiConfigConstants.PurchaseOrderAdd, Client.Common.UiConfigConstants.PurchaseOrderEdit, "PurchaseOrderId", "0", "IsExternal", ErrorMessage = "BuyerErrMsg|" + LocalizeResourceSetConstants.PurchaseOrder)]
        public long? Buyer_PersonnelId { get; set; }
        [RequiredAsPerUI(Client.Common.UiConfigConstants.PurchaseOrderAdd, Client.Common.UiConfigConstants.PurchaseOrderEdit, "PurchaseOrderId", "0", "IsExternal", ErrorMessage = "CarrierErrMsg|" + LocalizeResourceSetConstants.PurchaseOrder)]
        public string Carrier { get; set; }
        public long? CompleteBy_PersonnelId { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]

        [Display(Name = "spnPoRequired|" + LocalizeResourceSetConstants.PurchaseOrder)]
        [RequiredAsPerUI(Client.Common.UiConfigConstants.PurchaseOrderAdd, Client.Common.UiConfigConstants.PurchaseOrderEdit, "PurchaseOrderId", "0", "IsExternal", ErrorMessage = "RequiredErrMsg|" + LocalizeResourceSetConstants.PurchaseOrder)]
        public DateTime? Required { get; set; }
        public DateTime? CompleteDate { get; set; }
        public long? Creator_PersonnelId { get; set; }
        [RequiredAsPerUI(Client.Common.UiConfigConstants.PurchaseOrderAdd, Client.Common.UiConfigConstants.PurchaseOrderEdit, "PurchaseOrderId", "0", "IsExternal", ErrorMessage = "FOBErrMsg|" + LocalizeResourceSetConstants.PurchaseOrder)]
        public string FOB { get; set; }
        public string Status { get; set; }
        [RequiredAsPerUI(Client.Common.UiConfigConstants.PurchaseOrderAdd, Client.Common.UiConfigConstants.PurchaseOrderEdit, "PurchaseOrderId", "0", "IsExternal", ErrorMessage = "TermsErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string Terms { get; set; }

        public long VendorId { get; set; }
        public long? VoidBy_PersonnelId { get; set; }
        public DateTime? VoidDate { get; set; }
        public string VoidReason { get; set; }
        [RequiredAsPerUI(Client.Common.UiConfigConstants.PurchaseOrderAdd, Client.Common.UiConfigConstants.PurchaseOrderEdit, "PurchaseOrderId", "0", "IsExternal", ErrorMessage = "ReasonErrMsg|" + LocalizeResourceSetConstants.PurchaseOrder)]
        public string Reason { get; set; }
        public string CustomFilter { get; set; }
        [RequiredAsPerUI(Client.Common.UiConfigConstants.PurchaseOrderAdd, Client.Common.UiConfigConstants.PurchaseOrderEdit, "PurchaseOrderId", "0", "IsExternal", ErrorMessage = "MessageToVendorErrMsg|" + LocalizeResourceSetConstants.PurchaseOrder)]
        public string MessageToVendor { get; set; }
        public long ExPurchaseOrderId { get; set; }
        public string ExPurchaseRequest { get; set; }
        public string Currency { get; set; }
        public int Revision { get; set; }
        public string PaymentTerms { get; set; }
        public int UpdateIndex { get; set; }
        public DateTime? CreateDate { get; set; }
        public string VendorName { get; set; }
        public string VendorPhoneNumber { get; set; }
        public string VendorCustomerAccount { get; set; }
        public string VendorEmailAddress { get; set; }

        [Required(ErrorMessage = "PoSelectVendor|" + LocalizeResourceSetConstants.PurchaseOrder)]
        public string VendorClientLookupId { get; set; }

        public Int64 PurchaseOrderLineItemId { get; set; }
        public string Creator_PersonnelName { get; set; }
        public string Completed_PersonnelName { get; set; }
        public string Buyer_PersonnelName { get; set; }
        public int CountLineItem { get; set; }
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [DataType(DataType.Currency)]
        public decimal? TotalCost { get; set; }
        public int CustomQueryDisplayId { get; set; }
        public string DateRange { get; set; }
        public string DateColumn { get; set; }
        public string VendorAddress1 { get; set; }
        public string VendorAddress2 { get; set; }
        public string VendorAddress3 { get; set; }
        public string VendorAddressCity { get; set; }
        public string VendorAddressCityForPrint { get; set; }
        public string VendorAddressCountry { get; set; }
        public string VendorAddressPostCode { get; set; }
        public string VendorAddressState { get; set; }
        public string SiteName { get; set; }
        public string SiteAddress1 { get; set; }
        public string SiteAddress2 { get; set; }
        public string SiteAddress3 { get; set; }
        public string SiteAddressCity { get; set; }
        public string SiteAddressCityForPrint { get; set; }
        public string SiteAddressCountry { get; set; }
        public string SiteAddressPostCode { get; set; }
        public string SiteAddressState { get; set; }
        public string SiteBillToName { get; set; }
        public string SiteBillToAddress1 { get; set; }
        public string SiteBillToAddress2 { get; set; }
        public string SiteBillToAddress3 { get; set; }
        public string SiteBillToAddressCity { get; set; }
        public string SiteBillToAddressCityForPrint { get; set; }
        public string SiteBillToAddressCountry { get; set; }
        public string SiteBillToAddressPostCode { get; set; }
        public string SiteBillToAddressState { get; set; }
        public string SiteBillToComment { get; set; }
        public string WorkFlowMessageForceComplete { get; set; }
        public Int64 FilterValue { get; set; }
        public long? POImportId { get; set; }
        public string UnitOfMeasure { get; set; }
        public string ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string CreateBy { get; set; }
        public string AzureImageURL { get; set; }
        public string PersonnelEmail { get; set; }
        public bool IsExternal { get; set; }
        public IEnumerable<SelectListItem> StatusList { get; set; }

        public IEnumerable<SelectListItem> ScheduleWorkList { get; set; }
        public IEnumerable<SelectListItem> VendorsList { get; set; }
        public IEnumerable<SelectListItem> BuyerList { get; set; }
        public IEnumerable<SelectListItem> FOBList { get; set; }
        public IEnumerable<SelectListItem> TermsList { get; set; }
        public IEnumerable<SelectListItem> VoidReasonList { get; set; }
        public bool Sec_Void { get; set; }
        public bool Sec_ForceComplete { get; set; }
        public bool Sec_PurchaseEdit { get; set; }

        public bool _menuAdd { get; set; }
        public bool _POVendorCountE2V { get; set; }
        public bool _PRCountRD { get; set; }
        public string PackageLevel { get; set; }
        public bool UseVendorMaster { get; set; }
        public UserData userData { get; set; }
        public PurchaseRequestModel purchaseRequestModel { get; set; }

        public bool IsPurchaseOrderAdd { get; set; }

        public bool IsPurchasingReceive { get; set; }
        public IEnumerable<SelectListItem> TextSearchList { get; set; }
        public List<EventLogModel> EventLogList { get; set; }
        public string ToEmailId { get; set; }
        public string CcEmailId { get; set; }
        public Int32 ChildCount { get; set; }
        public int TotalCount { get; set; }

        //V2-347
        public DateTime? CompleteStartDateVw { get; set; }
        public DateTime? CompleteEndDateVw { get; set; }
        public DateTime? StartCompleteDate { get; set; }
        public DateTime? EndCompleteDate { get; set; }
        public DateTime? StartCreateDate { get; set; }
        public DateTime? EndCreateDate { get; set; }
        public IEnumerable<SelectListItem> DateRangeDropList { get; set; }
        public IEnumerable<SelectListItem> DateRangeDropListForCreateDate { get; set; }
        //V2-347

        public List<string> hiddenColumnList { get; set; }
        public List<string> disabledColumnList { get; set; }
        public List<string> requiredColumnList { get; set; }
        public string ViewName { get; set; }

        public IEnumerable<SelectListItem> SchedulePurchaseReceiptList { get; set; }//V2-331

        public bool IsPunchout { get; set; }
        public bool SentOrderRequest { get; set; }
        public long ClientId { get; set; }

        public bool PurchaseOrderSendPunchOutPOSecurity { get; set; }
        public bool IsRedirectFromPart { get; set; }


        public IEnumerable<SelectListItem> AccountList { get; set; }

        public string AccountClientLookupId { get; set; }
        public string Logo { get; set; }
        public string PrintDate { get; set; }
        public bool SingleStockLineItem { get; set; } //V2-1032
    }
}