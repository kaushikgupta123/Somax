using Client.CustomValidation;

using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.PartsManagement.PartsManagementRequest
{
    public class PartsManagementRequestModel
    {
        public long PartMasterRequestId { get; set; }
        [Required(ErrorMessage = "ValidJustification|" + LocalizeResourceSetConstants.PartsManagementDetail)]
        public string Justification { get; set; }
        public string RequestType { get; set; }
        public string RequestTypeForPopUp { get; set; }
        public string LocalizedRequestType { get; set; }
        [Required(ErrorMessage = "ValidManufacturer|" + LocalizeResourceSetConstants.PartsManagementDetail)]
        public string Manufacturer { get; set; }
        [Required(ErrorMessage = "ValidManufacturerPartNumber|" + LocalizeResourceSetConstants.PartsManagementDetail)]
        [RegularExpression("^[A-Z0-9\\%\\-\\:\\/\\$\\*\\+\\.]+$", ErrorMessage = "Validinputalphabets|" + LocalizeResourceSetConstants.PartsManagementDetail)]
        public string ManufacturerId { get; set; }
        public long EXPartId { get; set; }
        public long EXPartId_Replace { get; set; }
        public string PartMaster_ClientLookupId { get; set; }
        public string Part_ClientLookupId { get; set; }
        public string Part_PrevClientLookupId { get; set; }
        public string PartMasterClientLookUpId_Replace { get; set; }
        public string Part_Description { get; set; }
        public string CompleteBy { get; set; }
        public string PartMaster_LongDescription { get; set; }
        public long CustomQueryDisplayId { get; set; }
        public string Requester { get; set; }
        public string Requester_Email { get; set; }
        public string Status_Display { get; set; }
        public string ApproveDenyBy { get; set; }
        public string ApproveDenyBy2 { get; set; }
        public string LastReviewedBy { get; set; }
        public string PartMasterCreateBy { get; set; }
        public string Site_Name { get; set; }

        public string ValidateFor = string.Empty;
        public string ExOrganizationId { get; set; }
        public IEnumerable<SelectListItem> PMRStatusList { get; set; }
        [Required(ErrorMessage = "ValidUnitofMeasure|" + LocalizeResourceSetConstants.PartsManagementDetail)]
        public string UnitOfMeasure { get; set; }
        public DateTime? CreatedDate { get; set; }
        [Required(ErrorMessage = "ValidPurchaseFrequency|" + LocalizeResourceSetConstants.PartsManagementDetail)]
        public string PurchaseFreq { get; set; }
        public string Status { get; set; }
        [Required(ErrorMessage = "ValidDescription|" + LocalizeResourceSetConstants.PartsManagementDetail)]
        public string Description { get; set; }
        [Required(ErrorMessage = "ValidLeadTime|" + LocalizeResourceSetConstants.PartsManagementDetail)]
        public string PurchaseLeadTime { get; set; }
        public bool Critical { get; set; }
        [Required(ErrorMessage = "ValidCostLevel|" + LocalizeResourceSetConstants.PartsManagementDetail)]
        public string PurchaseCost { get; set; }
        public long? PartId { get; set; }
        public DateTime? LastReviewed_Date { get; set; }
        public DateTime? ApproveDeny_Date { get; set; }
        public DateTime? ApproveDeny2_Date { get; set; }
        public long? PartMasterId { get; set; }
        public DateTime? CompleteDate { get; set; }
        public string ImageURL { get; set; }
        public long ClientId { get; set; }
        public long SiteId { get; set; }

        public bool ShowDeleteBtnAfterUpload { get; set; }
        public IEnumerable<SelectListItem> PurchaseCostList { get; set; }
        public IEnumerable<SelectListItem> PurchaseLeadTimeList { get; set; }
        public IEnumerable<SelectListItem> PurchaseFreqList { get; set; }
        public IEnumerable<SelectListItem> ManufacturerList { get; set; }
        public IEnumerable<SelectListItem> UnitOfMeasureList { get; set; }

        //-----for buttons' visibility
        public bool btnSave { get; set; }
        public bool btnSendToApproval { get; set; }
        public bool btnReturn2Requester { get; set; }
        public bool btnApprv { get; set; }
        public bool btndenied { get; set; }
        public bool btnCancel { get; set; }
        public bool btnEnterpriseApprv { get; set; }
        public bool DocumentsUploadControl { get; set; }
        #region V2-798
        [Required(ErrorMessage = "GlobalPleaseEnterLocation|" + LocalizeResourceSetConstants.Global)]
        public string Location { get; set; }
        [Required(ErrorMessage = "validationUnitCost|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression(@"^\d*\.?\d{0,5}$", ErrorMessage = "globalFiveDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0.01, 9999999999.99999, ErrorMessage = "globalFiveDecimalAfterTotalFifteenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? UnitCost { get; set; }
        [RegularExpression(@"^\d*\.?\d{0,6}$", ErrorMessage = "globalSixDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0.00, 999999999.999999, ErrorMessage = "globalSixDecimalAfterNineDecimalBeforeTotalFifteenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? QtyMinimum { get; set; }
        [Required(ErrorMessage = "globalSixDecimalAfterNineDecimalBeforeTotalFifteenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression(@"^\d*\.?\d{0,6}$", ErrorMessage = "globalSixDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [GreaterThan("QtyMinimum", ErrorMessage = "globalGreaterThanToMinimumValueErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0.01, 999999999.999999, ErrorMessage = "globalSixDecimalAfterNineDecimalBeforeTotalFifteenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? QtyMaximum { get; set; }
        public IEnumerable<SelectListItem> StatusList { get; set; }
        public IEnumerable<SelectListItem> RequestTypeList { get; set; }
        public long CreatedByPersonnelId { get; set; }
        public IEnumerable<SelectListItem> CreatedByPersonnelList { get; set; }
        public int TotalCount { get; set; }
        #endregion
    }
}