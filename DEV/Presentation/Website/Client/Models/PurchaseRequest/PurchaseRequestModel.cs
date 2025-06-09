using Client.Common;
using Client.CustomValidation;
using Client.Models.Common;
using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.PurchaseRequest
{
    public class PurchaseRequestModel
    {
        public string Created { get; set; }
        public string StatusDrop { get; set; }
        public Int64 UserInfoId { get; set; }
        
        public string VendorName { get; set; }
        public string VendorPhoneNumber { get; set; }
        //[RequiredAsPerUI(UiConfigConstants.PurchaseRequestAdd, UiConfigConstants.PurchaseRequestEdit, "PurchaseRequestId", "0",null, null, ErrorMessage = "PoSelectVendor|" + LocalizeResourceSetConstants.PurchaseOrder)]
        //[Required(ErrorMessage = "PoSelectVendor|" + LocalizeResourceSetConstants.PurchaseOrder)]
        public string VendorClientLookupId { get; set; }
        public bool VendorIsExternal { get; set; }
        public string PRClientLookupId { get; set; }
        public string VendorEmailAddress { get; set; }
        public string Creator_PersonnelName { get; set; }
        public string PersonnelName { get; set; }
        public string Approved_PersonnelName { get; set; }
        
        public string Processed_PersonnelName { get; set; }
        
        public int? CountLineItem { get; set; }
        public Int64 PurchaseRequestLineItemId { get; set; }
        public int LineNumber { get; set; }
        
        public string Description { get; set; }
        
        public decimal? TotalCost { get; set; }
        public int CustomQueryDisplayId { get; set; }
        public string DateRange { get; set; }
        public string DateColumn { get; set; }
       
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public long PartId { get; set; }
        public decimal OrderQuantity { get; set; }
        public string UnitofMeasure { get; set; }
        public decimal UnitCost { get; set; }
       
        public DateTime? ProcessedDate { get; set; }
        public long AccountId { get; set; }
        public string Prefix { get; set; }
        public long PersonnelId { get; set; }
        public long ProcessLogId { get; set; }
       
        public string PurchaseOrderClientLookupId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public long ChargeToID { get; set; }
        public string ChargeToClientLookupId { get; set; }
        public string ChargeTo_Name { get; set; }
        public string ChargeType { get; set; }
        public string VendorAddress1 { get; set; }
        public string VendorAddress2 { get; set; }
        public string VendorAddress3 { get; set; }
        public string VendorAddressCity { get; set; }
        public string VendorAddressCityPrint { get; set; }
        public string VendorAddressCountry { get; set; }
        public string VendorAddressPostCode { get; set; }
        public string VendorAddressState { get; set; }
        public string SiteName { get; set; }
        public string SiteAddress1 { get; set; }
        public string SiteAddress2 { get; set; }
        public string SiteAddress3 { get; set; }
        public string SiteAddressCity { get; set; }
        public string SiteAddressCityPrint { get; set; }
        public string SiteAddressCountry { get; set; }
        public string SiteAddressPostCode { get; set; }
        public string SiteAddressState { get; set; }
        public string SiteBillToAddress1 { get; set; }
        public string SiteBillToAddress2 { get; set; }
        public string SiteBillToAddress3 { get; set; }
        public string SiteBillToAddressCity { get; set; }
        public string SiteBillToAddressCityPrint { get; set; }
        public string SiteBillToAddressCountry { get; set; }
        public string SiteBillToAddressPostCode { get; set; }
        public string SiteBillToAddressState { get; set; }
        public long PurchaseRequestId { get; set; }
        public long SiteId { get; set; }
        public long ClientId { get; set; }
        public long AreaId { get; set; }
        public long DepartmentId { get; set; }
        public long StoreroomId { get; set; }
        public string ClientLookupId { get; set; }
        [RequiredAsPerUI(UiConfigConstants.PurchaseRequestAdd, UiConfigConstants.PurchaseRequestEdit, "PurchaseRequestId", "0",null,null,  ErrorMessage = "ReasonErrMsg|" + LocalizeResourceSetConstants.PurchaseRequest)]
        public string Reason { get; set; }
        public long ApprovedBy_PersonnelId { get; set; }
        public DateTime? Approved_Date { get; set; }
        public long CreatedBy_PersonnelId { get; set; } 
        
        public string Process_Comments { get; set; }
        public DateTime? Processed_Date { get; set; }
        public long ProcessBy_PersonnelId { get; set; }
       
        public string Status { get; set; }
        public string LocalizedStatus { get; set; }
        public long? VendorId { get; set; }
        public long PurchaseOrderId { get; set; }
       
        public bool AutoGenerated { get; set; }
       
        public string Return_Comments { get; set; }
        public long ExtractLogId { get; set; }
        public int UpdateIndex { get; set; }
        public Int32 ChildCount { get; set; }
        public int TotalCount { get; set; }

        public IEnumerable<SelectListItem> ScheduleWorkList { get; set; }
        public IEnumerable<SelectListItem> VendorsList { get; set; }
        public IEnumerable<SelectListItem> PersonnelList { get; set; }
        public IEnumerable<SelectListItem> StatusList { get; set; }
        public string PersonalName { get; set; }
        public bool IsPurchaseRequestAdd { get; set; }
        public bool IsPurchaseRequestAddDynamic { get; set; }
        public bool IsPurchaseRequestFromApproval { get; set; }
        public bool ApproveSecurity { get; set; }
        public bool EditSecurity { get; set; }
        public bool EditAwaitApproveSecurity { get; set; }
        public bool CreateSecurity { get; set; }
        public bool _menuAdd { get; set; }
        public IEnumerable<SelectListItem> AccountList { get; set; }
        public string Account_ClientLookupId { get; set; }
        public IEnumerable<SelectListItem> ChargeTypeList { get; set; }
        public IEnumerable<SelectListItem> UOMList { get; set; }
        public string Uoms { get; set; }
        public IEnumerable<SelectListItem> ChargeTypelookUpList { get; set; }
        public DateTime? CurrentDate { get; set; }
        public List<EventLogModel> EventLogList { get; set; }
        public string AzureImageURL { get; set; }
        public string MailBodyComments { get; set; }
        public string ToEmailId { get; set; }
        public string CcEmailId { get; set; }
        public IEnumerable<SelectListItem> DateRangeDropList { get; set; }
        //public bool IsExternal { get; set; }
        public IEnumerable<SelectListItem> DateRangeDropListForPR { get; set; }
        public IEnumerable<SelectListItem> DateRangeDropListForPRCancel { get; set; }
        public IEnumerable<SelectListItem> DateRangeDropListAllStatus { get; set; }
        public bool PrApproveSecurity { get; set; }
        public string ViewName { get; set; }
        public bool DisabledVal { get; set; }
        public bool EditApprovedSecurity { get; set; }

        public bool PRUsePunchOutSecurity { get; set; }

        public bool IsSitePunchOut { get; set; }

        public bool IsPunchOut { get; set; }

        public IEnumerable<SelectListItem> PartlookUpList { get; set; }
        public IEnumerable<SelectListItem> ChargeTolookUpList { get; set; }

        public IEnumerable<SelectListItem> AccountlookUpList { get; set; }

        public bool IsRedirectFromPart { get; set; }
        public bool IsRedirectFromPlusMenu { get; set; }
        //V2-563
        [DefaultValue(false)]
        public bool IsAdditionalCatalogItem { get; set; }
        public string PurchaseUOM { get; set; }
        public long PartStoreroomId { get; set; }
        public long VendorCatalogItemId { get; set; }
        public bool IsRedirectFromDetailPurchaseOrder { get; set; }
        public bool SingleStockLineItem { get; set; } //V2-1032
        public bool IsRedirectFromNotification { get; set; } //V2-1147
        public string AlertNameRedirectFromNotification { get; set; }  //V2-1147
    }
    
}