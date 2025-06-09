using Client.CustomValidation;

using Common.Constants;
using DevExpress.Pdf.Native.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.MultiStoreroomPart
{
    public class MultiStoreroomPartModel
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
        [Display(Name = "spnStockType|" + LocalizeResourceSetConstants.Global)]
        public string StockType { get; set; }
        public string StockTypeDescription { get; set; }

        [MaxLength(31, ErrorMessage = "spnMaxLength31|" + LocalizeResourceSetConstants.PartDetails)]
        [Display(Name = "spnManufacturer|" + LocalizeResourceSetConstants.Global)]
        public string Manufacturer { get; set; }
        [MaxLength(63, ErrorMessage = "spnMaxLength63|" + LocalizeResourceSetConstants.PartDetails)]
        [Display(Name = "spnManufacturerID|" + LocalizeResourceSetConstants.Global)]
        public string ManufacturerID { get; set; }
        
        #endregion
        
        public IEnumerable<SelectListItem> LookupStokeTypeList { get; set; }
        public IEnumerable<SelectListItem> LookupIssueUnitList { get; set; }
        public IEnumerable<SelectListItem> AccountList { get; set; }
        public int ChildCount { get; set; }
        public int TotalCount { get; set; }
        [Display(Name = "globalInActive|" + LocalizeResourceSetConstants.Global)]
        public bool InactiveFlag { get; set; }
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
        [Required(ErrorMessage = "validationIssueUnit|" + LocalizeResourceSetConstants.PartDetails)]
        [Display(Name = "spnIssueUnit|" + LocalizeResourceSetConstants.Global)]
        public string IssueUnit { get; set; }
        public string IssueUnitDescription { get; set; }
        [Display(Name = "spnAccount|" + LocalizeResourceSetConstants.Global)]
        public long? AccountId { get; set; }
        public string AccountClientLookupId { get; set; }
        #region Quantities / Cost
        [Display(Name = "spnAverageCost|" + LocalizeResourceSetConstants.PartDetails)]
        [RegularExpression(@"^\d*\.?\d{0,5}$", ErrorMessage = "globalFiveDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999999999.99999, ErrorMessage = "globalFiveDecimalAfterTotalFifteenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? AverageCost { get; set; }
        [Display(Name = "spnAppliedCost|" + LocalizeResourceSetConstants.PartDetails)]
        [RegularExpression(@"^\d*\.?\d{0,5}$", ErrorMessage = "globalFiveDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999999999.99999, ErrorMessage = "globalFiveDecimalAfterTotalFifteenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
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

        [Display(Name = "spnCritical|" + LocalizeResourceSetConstants.Global)]
        public bool CriticalFlag { get; set; }
        [MaxLength(31, ErrorMessage = "spnMaxLength31|" + LocalizeResourceSetConstants.PartDetails)]
        [Display(Name = "spnUPCCode|" + LocalizeResourceSetConstants.Global)]
        public string UPCCode { get; set; }
        [Display(Name = "spnConsignment|" + LocalizeResourceSetConstants.PartDetails)]
        public bool Consignment { get; set; }
        public bool ClientOnPremise { get; set; }

        [MaxLength(7, ErrorMessage = "spnMaxLength7|" + LocalizeResourceSetConstants.PartDetails)]
        [Display(Name = "spnABCCode|" + LocalizeResourceSetConstants.PartDetails)]
        public string ABCCode { get; set; }
        [MaxLength(7, ErrorMessage = "spnMaxLength7|" + LocalizeResourceSetConstants.PartDetails)]
        [Display(Name = "spnABCStoreCost|" + LocalizeResourceSetConstants.PartDetails)]
        public string ABCStoreCost { get; set; }
        [MaxLength(7, ErrorMessage = "spnMaxLength7|" + LocalizeResourceSetConstants.PartDetails)]
        [Display(Name = "spnMSDSContainerCode|" + LocalizeResourceSetConstants.PartDetails)]
        public string MSDSContainerCode { get; set; }
        [MaxLength(7, ErrorMessage = "spnMaxLength7|" + LocalizeResourceSetConstants.PartDetails)]
        [Display(Name = "spnMSDSPressureCode|" + LocalizeResourceSetConstants.PartDetails)]
        public string MSDSPressureCode { get; set; }
        [MaxLength(31, ErrorMessage = "spnMaxLength31|" + LocalizeResourceSetConstants.PartDetails)]
        [Display(Name = "spnMSDSReference|" + LocalizeResourceSetConstants.PartDetails)]
        public string MSDSReference { get; set; }
        [Display(Name = "spnMSDSRequired|" + LocalizeResourceSetConstants.PartDetails)]
        public bool MSDSRequired { get; set; }
        [MaxLength(7, ErrorMessage = "spnMaxLength7|" + LocalizeResourceSetConstants.PartDetails)]
        [Display(Name = "spnMSDSTemperatureCode|" + LocalizeResourceSetConstants.PartDetails)]
        public string MSDSTemperatureCode { get; set; }
        [Display(Name = "spnNoEquipXref|" + LocalizeResourceSetConstants.PartDetails)]
        public bool NoEquipXref { get; set; }
        public IEnumerable<SelectListItem> PartViewSearchList { get; set; }
        public string PartImageUrl { get; set; }
        #region V2-1025
        public string DefStoreroom { get; set; }
        public decimal TotalOnHand { get; set; }
        public decimal TotalOnRequest { get; set; }
        public decimal TotalOnOrder { get; set; }
        public string DefaultStoreroom { get; set; }
        #endregion
    }
}