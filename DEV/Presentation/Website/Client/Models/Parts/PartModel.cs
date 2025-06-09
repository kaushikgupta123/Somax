using Client.CustomValidation;

using Common.Constants;

using DataContracts;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.Parts
{
    public class PartModel
    {

        #region Identification

        public long PartId { get; set; }
        [Required(ErrorMessage = "validationPartId|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression("^[A-Z0-9\\%\\-\\:\\/\\$\\*\\+\\.]+$", ErrorMessage = "PartIdRegErrMsg|" + LocalizeResourceSetConstants.PartDetails)]
        [Display(Name = "spnPartID|" + LocalizeResourceSetConstants.Global)]
        public string ClientLookupId { get; set; }
        [Display(Name = "spnDescription|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "validationDescription|" + LocalizeResourceSetConstants.Global)]
        public string Description { get; set; }
        [Display(Name = "spnUPCCode|" + LocalizeResourceSetConstants.Global)]
        public string UPCCode { get; set; }
        [Display(Name = "spnAccount|" + LocalizeResourceSetConstants.Global)]
        public long? AccountId { get; set; }
        public string AccountClientLookupId { get; set; }
        [Display(Name = "spnStockType|" + LocalizeResourceSetConstants.Global)]
        public string StockType { get; set; }
        [Required(ErrorMessage = "validationIssueUnit|" + LocalizeResourceSetConstants.PartDetails)]
        [Display(Name = "spnIssueUnit|" + LocalizeResourceSetConstants.Global)]
        public string IssueUnit { get; set; }
        [Display(Name = "spnManufacturer|" + LocalizeResourceSetConstants.Global)]
        public string Manufacturer { get; set; }
        [Display(Name = "spnManufacturerID|" + LocalizeResourceSetConstants.Global)]
        public string ManufacturerID { get; set; }
        [Display(Name = "globalInActive|" + LocalizeResourceSetConstants.Global)]
        public bool InactiveFlag { get; set; }
        [Display(Name = "spnCritical|" + LocalizeResourceSetConstants.Global)]
        public bool CriticalFlag { get; set; }
        //public string Location1_5 { get; set; }
        //public string Location1_1 { get; set; }
        //public string Location1_2 { get; set; }
        //public string Location1_3 { get; set; }
        //public string Location1_4 { get; set; }
        /// <summary>

        /// </summary>
        ////---------for Type and Stock Type dropdown-------------
        public string ChargeType_Primary { get; set; }
        public string TransactionType { get; set; }
        #endregion

        #region Location
        [Display(Name = "spnPlaceArea|" + LocalizeResourceSetConstants.PartDetails)]
        public string PlaceArea { get; set; }
        [Display(Name = "spnSection|" + LocalizeResourceSetConstants.PartDetails)]
        public string Section { get; set; }
        [Display(Name = "spnRow|" + LocalizeResourceSetConstants.PartDetails)]
        public string Row { get; set; }
        [Display(Name = "spnShelf|" + LocalizeResourceSetConstants.PartDetails)]
        public string Shelf { get; set; }
        [Display(Name = "spnBin|" + LocalizeResourceSetConstants.PartDetails)]
        public string Bin { get; set; }
        #endregion

        #region Quantities / Cost
        [Display(Name = "spnAverageCost|" + LocalizeResourceSetConstants.PartDetails)]
        [RegularExpression(@"^\d*\.?\d{0,5}$", ErrorMessage = "globalFiveDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999999999.99999, ErrorMessage = "PartDecRegErrMsg|" + LocalizeResourceSetConstants.PartDetails)]
        public decimal? AverageCost { get; set; }
        [Display(Name = "spnAppliedCost|" + LocalizeResourceSetConstants.PartDetails)]
        [RegularExpression(@"^\d*\.?\d{0,5}$", ErrorMessage = "globalFiveDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999999999.99999, ErrorMessage = "PartDecRegErrMsg|" + LocalizeResourceSetConstants.PartDetails)]        
        public decimal? AppliedCost { get; set; }
        [Display(Name = "spnLastPurchaseCost|" + LocalizeResourceSetConstants.PartDetails)]
        public decimal? LastPurchaseCost { get; set; }
        [Display(Name = "spnOnHandQuantity|" + LocalizeResourceSetConstants.PartDetails)]
        [RegularExpression(@"^\d*\.?\d{0,6}$", ErrorMessage = "globalSixDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999999999.999999, ErrorMessage = "globalSixDecimalAfterNineDecimalBeforeTotalFifteenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? OnHandQuantity { get; set; }
        [Display(Name = "spnOnOrderQuantity|" + LocalizeResourceSetConstants.PartDetails)]
        public decimal? OnOrderQuantity { get; set; }
        [Display(Name = "spnOnRequestQuantity|" + LocalizeResourceSetConstants.PartDetails)]
        public decimal? OnRequestQuantity { get; set; }
        [Display(Name = "spnAutoPurchase|" + LocalizeResourceSetConstants.PartDetails)]
        public bool AutoPurchaseFlag { get; set; }
        [Display(Name = "spnMaximum|" + LocalizeResourceSetConstants.PartDetails)]
        [RegularExpression(@"^\d*\.?\d{0,6}$", ErrorMessage = "globalSixDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999999999.999999, ErrorMessage = "globalSixDecimalAfterNineDecimalBeforeTotalFifteenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [GreaterThanOrEqualTo("Minimum", ErrorMessage = "globalGreaterThanOrEqualToErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? Maximum { get; set; }
        [Display(Name = "spnMinimum|" + LocalizeResourceSetConstants.PartDetails)]
        [RegularExpression(@"^\d*\.?\d{0,6}$", ErrorMessage = "globalSixDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999999999.999999, ErrorMessage = "globalSixDecimalAfterNineDecimalBeforeTotalFifteenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? Minimum { get; set; }
        [Display(Name = "spnCountFrequency|" + LocalizeResourceSetConstants.PartDetails)]
        public int? CountFrequency { get; set; }
        [Display(Name = "spnLastCounted|" + LocalizeResourceSetConstants.PartDetails)]
        public DateTime? LastCounted { get; set; }
        #endregion
        public string PartImageUrl { get; set; }
        public decimal Quantity { get; set; }
        public decimal MinimumQuantity { get; set; }
        public bool AutoPurch { get; set; }
        public byte[] PartImage { get; set; }
        public IEnumerable<SelectListItem> AccountList { get; set; }
        public IEnumerable<SelectListItem> LookupStokeTypeList { get; set; }
        public IEnumerable<SelectListItem> LookupIssueUnitList { get; set; }
        public bool IsPartAdd { get; set; }
        [Display(Name = "spnConsignment|" + LocalizeResourceSetConstants.PartDetails)]
        public bool Consignment { get; set; }
        public bool RepairablePart { get; set; }

        public string VendorClientLookupId { get; set; }
        public IEnumerable<SelectListItem> VendorsList { get; set; }
        public IEnumerable<SelectListItem> ChargeTypeList { get; set; }
        [Display(Name = "spnPreviousId|" + LocalizeResourceSetConstants.PartDetails)]
        public string PreviousId { get; set; }
        public IEnumerable<SelectListItem> PartConsignmentList { get; set; }

        public Guid hdLoginId { get; set; }

        public string localurl { get; set; }
        public string PackageLevel { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<SelectListItem> PartViewSearchList { get; set; }
        public string partStatusForRedirection { get; set; }
        public long? UserId { get; set; }

        public bool ClientOnPremise { get; set; }

        #region For-641

        public string MSDSContainerCode { get; set; }
        public string MSDSPressureCode { get; set; }
        public string MSDSReference { get; set; }
        public bool MSDSRequired { get; set; }
        public string MSDSTemperatureCode { get; set; }
        public bool NoEquipXref { get; set; }
        #endregion

        #region V2-668
        public string PartMaster_ClientLookupId { get; set; }
        public string LongDescription { get; set; }
         public string ShortDescription { get; set; }
        public string Category { get; set; }
        public string CategoryDesc { get; set; }
        #endregion
        #region V2-687
        public long PartStoreroomId { get; set; }
        #endregion

        #region V2-836
        public string AzureImageURL { get; set; }
        #endregion
        #region V2-840
        public Security security { get; set; }
        public decimal QtyReorderLevel { get; set; }
        #endregion
        #region V2-1196
        public string VendorName { get; set; }
        public bool AutoPurchase { get; set; }
        #endregion
    }
}