using Common.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models
{
    public class PartVendorXrefModel
    {

        public long PartVendorXrefId { get; set; }
        [Display(Name = "VendorPartId|" + LocalizeResourceSetConstants.VendorDetails)]
        [Required(ErrorMessage = "spnPleaseSelectPartId|" + LocalizeResourceSetConstants.Global)]
        public string Part { get; set; }
        [Display(Name = "VendorPartCatNum|" + LocalizeResourceSetConstants.VendorDetails)]
        public string CatalogNumber { get; set; }
        [Display(Name = "spnManufacturer|" + LocalizeResourceSetConstants.Global)]
        public string Manufacturer { get; set; }
        [Display(Name = "spnManufacturerID|" + LocalizeResourceSetConstants.Global)]
        public string ManufacturerID { get; set; }
        [Display(Name = "spnOrderQnt|" + LocalizeResourceSetConstants.Global)]
        [Range(0, int.MaxValue, ErrorMessage = "globalIntMaxValueRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public int? OrderQuantity { get; set; }
        [Display(Name = "VendorPartUnit|" + LocalizeResourceSetConstants.VendorDetails)]
        public string OrderUnit { get; set; }
        [Display(Name = "VendorPartPrice|" + LocalizeResourceSetConstants.VendorDetails)]
        [RegularExpression(@"^\d*\.?\d{0,5}$", ErrorMessage = "globalFiveDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999999999.99999, ErrorMessage = "globalFiveDecimalAfterTotalFifteenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? Price { get; set; }
        [Display(Name = "VendorPartChoice|" + LocalizeResourceSetConstants.VendorDetails)]
        public bool PreferredVendor { get; set; }
        public IEnumerable<SelectListItem> PartsList { get; set; }
        public IEnumerable<SelectListItem> OrderUnitList { get; set; }
        public string VendorClientLookupId { get; set; }
        public long VendorId { get; set; }
        public int updatedindex { get; set; }
        public string PartDescription { get; set; }
        public string VendorName { get; set; }
        public long PartID { get; set; }
    }
}