using System;
using Common.Constants;
using System.ComponentModel.DataAnnotations;
using Client.CustomValidation;

namespace Client.Models.Equipment
{
    public class RepairableSpareModel
    {
        public long EquipmentId { get; set; } // For Repairable spare asset add and edit
        //[Display(Name = "Asset Id")]
        [Display(Name = "spnGlobalEquipmentId|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "EquipmentIDErrorMessage|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [StringLength(31, ErrorMessage = "EquipmentStrLenErrorMessage|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression("^[A-Z0-9\\%\\-\\:\\/\\$\\*\\+\\.]+$", ErrorMessage = "EquipmentIDRegErrMsg|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string ClientLookupId { get; set; }
        [Display(Name = "spnName|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "EquipmentNameErrorMessage|" + LocalizeResourceSetConstants.EquipmentDetails)]
        //[StringLength(63, ErrorMessage = "EquipmentStrLenErrorMessage|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string Name { get; set; }
        [Display(Name = "spnSerialNo|" + LocalizeResourceSetConstants.EquipmentDetails)]
        //[StringLength(63, ErrorMessage = "EquipmentStrLenErrorMessage|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string SerialNumber { get; set; }
        [Required(ErrorMessage = "EquipmenttypeErrorMessage|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [Display(Name = "GlobalType|" + LocalizeResourceSetConstants.Global)]
        public string Type { get; set; }
        [Display(Name = "spnMake|" + LocalizeResourceSetConstants.Global)]
        //[StringLength(31, ErrorMessage = "EquipmentStrLenErrorMessage|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string Make { get; set; }
        [Display(Name = "spnModelNo|" + LocalizeResourceSetConstants.Global)]
        //[StringLength(63, ErrorMessage = "EquipmentStrLenErrorMessage|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string Model { get; set; }
        [Display(Name = "GlobalAccount|" + LocalizeResourceSetConstants.Global)]
        public long? Material_AccountId { get; set; }
        public string MaterialAccountClientLookupId { get; set; }
        [Display(Name = "spnAssetNo|" + LocalizeResourceSetConstants.EquipmentDetails)]
        //[StringLength(31, ErrorMessage = "EquipmentStrLenErrorMessage|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string AssetNumber { get; set; }
        [Display(Name = "spnMaintenanceWarrantyExpirationDate|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public DateTime? Maint_WarrantyExpire { get; set; }
        //[Display(Name = "Maintenance Vendor")]
        [Display(Name = "spnMaintenanceVendor|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public long? Maint_VendorId { get; set; }
        public string MaintVendorIdClientLookupId { get; set; }
        //[Display(Name = "Maintenance Warranty Description")]
        [Display(Name = "spnMaintenanceWarrantyDescription|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string Maint_WarrantyDesc { get; set; }
        //[Display(Name = "Acquired Cost")]
        [Display(Name = "spnAcquiredCost|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        //[Range(0, 9999999999999.99, ErrorMessage = "Valid Decimal number with maximum 2 decimal places and 15 digits in total")]
        [Range(0, 9999999999999.99, ErrorMessage = "GlobalValidDecimalNoWithMaximum2DecimalPlacesAnd15DigitsinTotal|" + LocalizeResourceSetConstants.Global)]
        public decimal? AcquiredCost { get; set; }
        //[Display(Name = "Acquired Date")]
        [Display(Name = "spnAcquiredDate|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public DateTime? AcquiredDate { get; set; }
        [Display(Name = "Catalog Number")]
        //[Display(Name = "spnCatalogNumber|" + LocalizeResourceSetConstants.PartDetails)]
        public string CatalogNumber { get; set; }
        //[Display(Name = "Cost Center")]
        [Display(Name = "spnCostCenter|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string CostCenter { get; set; }
        //[Display(Name = "Critical")]
        [Display(Name = "spnCritical|" + LocalizeResourceSetConstants.Global)]
        public bool CriticalFlag { get; set; }
        //[Display(Name = "Install Date")]
        [Display(Name = "spnInstallDate|" + LocalizeResourceSetConstants.Global)]
        public DateTime? InstallDate { get; set; }
        [RequiredIf("ValidationMode", "Add",ErrorMessage = "validationStatus|" + LocalizeResourceSetConstants.Global)]
        [Display(Name = "GlobalStatus|" + LocalizeResourceSetConstants.Global)]
        public string RepairableSpareStatus { get; set; }
        //[Required(ErrorMessage = "Please select Assigned to Asset Id")]
        [RequiredIf("ValidationMode", "Add",ErrorMessage = "spnPleaseSelectAssignedtoAssetId|" + LocalizeResourceSetConstants.EquipmentDetails)]
        /*[Display(Name = "Assigned to Asset Id")]*/
        [Display(Name = "spnAssignedtoAssetId|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string AssignedAssetClientlookupId { get; set; }
        public long AssignedAssetId { get; set; }
        //[RequiredIf("RepairableSpareStatus", AssetStatusConstant.Unassigned, ErrorMessage = "Please enter location")]
        [RequiredIf("RepairableSpareStatus", AssetStatusConstant.Unassigned, ErrorMessage = "GlobalPleaseEnterLocation|" + LocalizeResourceSetConstants.Global)]
        //[Display(Name = "Location")]
        [Display(Name = "spnLocation|" + LocalizeResourceSetConstants.Global)]
        public string Location { get; set; }
        public long DetailsEquipmentId { get; set; } // used for update asset
        public string ValidationMode { get; set; }
        public string MaintVendorIdClientLookupIdWithName { get; set; } //V2-1211
    }
}