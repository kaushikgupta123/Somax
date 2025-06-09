using Client.CustomValidation;
using Common.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.MultiStoreroomPart
{
    public class MultiStoreroomPartVendorModel
    {
        public long PartVendorXrefId { get; set; }
        [Display(Name = "Vendor|" + LocalizeResourceSetConstants.VendorDetails)]
        [Required(ErrorMessage = "VendorIdErrMessage|" + LocalizeResourceSetConstants.Global)]
        public string VendorClientLookupId { get; set; }
        [Display(Name = "VendorPartCatNum|" + LocalizeResourceSetConstants.VendorDetails)]
        public string CatalogNumber { get; set; }
        [Display(Name = "spnManufacturer|" + LocalizeResourceSetConstants.Global)]
        public string Manufacturer { get; set; }
        [Display(Name = "spnManufacturerID|" + LocalizeResourceSetConstants.Global)]
        public string ManufacturerID { get; set; }
        [Display(Name = "spnOrderQnt|" + LocalizeResourceSetConstants.Global)]
        [Range(0, int.MaxValue)]
        public int? OrderQuantity { get; set; }
        [Display(Name = "VendorPartUnit|" + LocalizeResourceSetConstants.VendorDetails)]
        [RequiredIf("UOMConvRequired", "true", ErrorMessage = "OrderUnitErrMessage|" + LocalizeResourceSetConstants.VendorDetails)]
        public string OrderUnit { get; set; }
        [RequiredIf("UOMConvRequired", "true", ErrorMessage = "PriceErrMessage|" + LocalizeResourceSetConstants.VendorDetails)]
        [Display(Name = "VendorPartPrice|" + LocalizeResourceSetConstants.VendorDetails)]
        [RegularExpression(@"^\d*\.?\d{0,5}$", ErrorMessage = "globalFiveDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999999999.99999, ErrorMessage = "GlobalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [GreaterThanOrEqualTo("DefaultPricevalue", ErrorMessage = "PriceGreaterThanZeroErrMessage|" + LocalizeResourceSetConstants.VendorDetails)]
        public decimal? Price { get; set; }
        [Display(Name = "VendorPartChoice|" + LocalizeResourceSetConstants.VendorDetails)]
        public bool PreferredVendor { get; set; }
        public IEnumerable<SelectListItem> OrderUnitList { get; set; }
        [Display(Name = "Vendor|" + LocalizeResourceSetConstants.VendorDetails)]
        [Required(ErrorMessage = "Please select vendor.")]
        public long VendorId { get; set; }
        public int updatedindex { get; set; }
        public long PartId { get; set; }
        public string PartClientLookupId { get; set; }
        public IEnumerable<SelectListItem> VendorsList { get; set; }
        [Display(Name = "IssueOrder|" + LocalizeResourceSetConstants.Global)]
        [RequiredIf("UOMConvRequired", "true", ErrorMessage = "IssueOrderErrMessage|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression(@"^\d*\.?\d{0,6}$", ErrorMessage = "globalSixDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(1, 999999999.999999, ErrorMessage = "GlobalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [GreaterThanOrEqualTo("DefaultissueOrdervalue", ErrorMessage = "IssueOrderGreaterThanErrMessage|" + LocalizeResourceSetConstants.VendorDetails)]
        public decimal? IssueOrder { get; set; }

        [Display(Name = "UOMConvRequired|" + LocalizeResourceSetConstants.Global)]
        public bool UOMConvRequired { get; set; }
        public decimal DefaultissueOrdervalue { get; set; }
        public decimal DefaultPricevalue { get; set; }
    }
}