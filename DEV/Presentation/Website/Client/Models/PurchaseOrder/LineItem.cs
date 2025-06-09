using Client.CustomValidation;
using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.PurchaseOrder
{
    public class LineItem
    {
        public long PurchaseOrderLineItemId { get; set; }
        public long PurchaseOrderId { get; set; }
        public long DepartmentId { get; set; }
        public long StoreroomId { get; set; }

        [RequiredAsPerUI(Client.Common.UiConfigConstants.PurchaseOrderLineItemAdd, Client.Common.UiConfigConstants.PurchaseOrderLineItemEdit, "PurchaseOrderLineItemId", "0", null,null, ErrorMessage = "validationAccountId|" + LocalizeResourceSetConstants.Global)]
        public long? AccountId { get; set; }

        [RequiredIf("PartId", "0", ErrorMessage = "PrLiChargeTo|" + LocalizeResourceSetConstants.PurchaseOrder)]
        public long? ChargeToId { get; set; }
        [RequiredIf("PartId", "0", ErrorMessage = "PrLiChargeType|" + LocalizeResourceSetConstants.PurchaseOrder)]
        public string ChargeType { get; set; }       
        public long CompleteBy_PersonnelId { get; set; }
        public DateTime? CompleteDate { get; set; }       
        public long Creator_PersonnelId { get; set; }

        [RequiredAsPerUI(Client.Common.UiConfigConstants.PurchaseOrderLineItemAdd, Client.Common.UiConfigConstants.PurchaseOrderLineItemEdit, "PurchaseOrderLineItemId", "0", null,null, ErrorMessage = "validationDescription|" + LocalizeResourceSetConstants.Global)]
        public string Description { get; set; }

        [RequiredAsPerUI(Client.Common.UiConfigConstants.PurchaseOrderLineItemAdd, Client.Common.UiConfigConstants.PurchaseOrderLineItemEdit, "PurchaseOrderLineItemId", "0", null,null, ErrorMessage = "validationEstimatedDelivery|" + LocalizeResourceSetConstants.Global)]
        [Display(Name = "spnPoEstimatedDelivery|" + LocalizeResourceSetConstants.PurchaseOrder)]
        public DateTime? EstimatedDelivery { get; set; }
        public int LineNumber { get; set; }
        public long PartId { get; set; }
        public long PartStoreroomId { get; set; }      
        public long PRCreator_PersonnelId { get; set; }
        public long PurchaseRequestId { get; set; }

        [RegularExpression(@"^\d*\.?\d{0,6}$", ErrorMessage = "globalSixDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 999999999.999999, ErrorMessage = "globalSixDecimalAfterNineDecimalBeforeTotalFifteenRegErrMsg|" + LocalizeResourceSetConstants.Global)]

        [Required(ErrorMessage = "GlobalValidateOrderQty|" + LocalizeResourceSetConstants.Global)]
        public decimal OrderQuantity { get; set; }    
        public decimal OrderQuantityOriginal { get; set; }
        public string Status { get; set; }      
        public bool Taxable { get; set; }

       // [RequiredIf("PartId", "0", ErrorMessage = "PrLiUOM|" + LocalizeResourceSetConstants.PurchaseOrder)]
        public string UnitOfMeasure { get; set; }

        [RegularExpression(@"^\d*\.?\d{0,5}$", ErrorMessage = "globalFiveDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999999999.99999, ErrorMessage = "globalFiveDecimalAfterTotalFifteenRegErrMsg|" + LocalizeResourceSetConstants.Global)]

        [Required(ErrorMessage = "GlobalValidateUnitCost|" + LocalizeResourceSetConstants.Global)]
        [DisplayFormat(DataFormatString = "{0:C5}")]
        public decimal UnitCost { get; set; }
        public int ExPurchaseOrderLineId { get; set; }
        [RequiredIf("PartId", "0", ErrorMessage = "Please select Purchase UOM.")]
        public string PurchaseUOM { get; set; }
        public decimal? UOMConversion { get; set; }
        public decimal PurchaseQuantity { get; set; }
        public decimal PurchaseCost { get; set; }
        public int UpdateIndex { get; set; }
        public string PartClientLookupId { get; set; }
        public decimal TotalCost { get; set; }
        public string ChargeToClientLookupId { get; set; }
        [RequiredIf("PartId", "0", ErrorMessage = "PrLiChargeTo|" + LocalizeResourceSetConstants.PurchaseOrder)]
        public string ChargeToClientLookupIdToShow { get; set; }
        public string ChargeTo_Name { get; set; }
        public string AccountClientLookupId { get; set; }
        public string ErrorMessageRow { get; set; }
        public decimal QuantityReceived { get; set; }
        public decimal QuantityToDate { get; set; }
        public decimal CurrentAverageCost { get; set; }
        public decimal CurrentAppliedCost { get; set; }
        public decimal CurrentOnHandQuantity { get; set; }
        public string StockType { get; set; }
        public decimal QuantityBackOrdered { get; set; }
        public string Status_Display { get; set; }
        public Int64 SiteId { get; set; } //--SOM-892--//
        public string Part_Manufacturer { get; set; }
        public string Part_ManufacturerID { get; set; }
        public string PurchaseOrder_ClientLookupId { get; set; }
        public string PurchaseRequest_ClientLookupId { get; set; }
        public long PurchaseRequest_Creator_PersonnelId { get; set; }
        public bool PurchaseRequest_AutoGenerated { get; set; }
        public decimal LineTotal { get; set; }
        public string PartNumber { get; set; }
        public IEnumerable<SelectListItem> AccountList { get; set; }
        public IEnumerable<SelectListItem> ChargeTypeList { get; set; }

        public IEnumerable<SelectListItem> ChargeToList { get; set; }
        public IEnumerable<SelectListItem> UOMList { get; set; }
        public List<string> hiddenColumnList { get; set; }
        public List<string> disabledColumnList { get; set; }
        public List<string> requiredColumnList { get; set; }

        public string ViewName { get; set; }
    }
}