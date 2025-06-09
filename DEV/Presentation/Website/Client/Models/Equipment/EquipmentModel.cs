using Client.Common;
using Client.CustomValidation;
using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace Client.Models
{
    public class EquipmentModel
    {
        [Display(Name = "spnGlobalEquipmentId|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "EquipmentIDErrorMessage|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [StringLength(31, ErrorMessage = "EquipmentStrLenErrorMessage|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression("^[A-Z0-9\\%\\-\\:\\/\\$\\*\\+\\.]+$", ErrorMessage = "EquipmentIDRegErrMsg|" + LocalizeResourceSetConstants.EquipmentDetails)]
        //[Remote("CheckExistingId", "Equipment", HttpMethod = "POST", ErrorMessage = "{0}")] //For Checking existing email with suggessation
        public string EquipmentID { get; set; }
        [Required(ErrorMessage = "EquipmentNameErrorMessage|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [Display(Name = "spnName|" + LocalizeResourceSetConstants.Global)]
        public string Name { get; set; }
        [Display(Name = "spnLocation|" + LocalizeResourceSetConstants.Global)]
        public string Location { get; set; }
        [Display(Name = "spnSerialNo|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string SerialNumber { get; set; }
        [Required(ErrorMessage = "EquipmenttypeErrorMessage|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [Display(Name = "GlobalType|" + LocalizeResourceSetConstants.Global)]
        public string Type { get; set; }
        [Display(Name = "spnMake|" + LocalizeResourceSetConstants.Global)]
        public string Make { get; set; }
        [Display(Name = "spnModelNo|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string ModelNumber { get; set; }
        [Display(Name = "spnAccount|" + LocalizeResourceSetConstants.Global)]
        public string Account { get; set; }
        [Display(Name = "spnAssetNo|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string AssetNumber { get; set; }
        public string Area { get; set; }
        public string Line { get; set; }
        public string Photos { get; set; }
        [Display(Name = "spnParentId|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string ParentIdClientLookupId { get; set; }
        public string PlantLocationDescription { get; set; }
        [Display(Name = "globalInActive|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public bool InactiveFlag { get; set; }
        public bool HiddenInactiveFlag { get; set; }

        public bool CriticalFlag { get; set; }
        [Display(Name = "spnExpires|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public DateTime? Maint_WarrantyExpire { get; set; }
        [Display(Name = "spnVendor|" + LocalizeResourceSetConstants.Global)]
        public string MaintVendorIdClientLookupId { get; set; }
        [Display(Name = "spnDescription|" + LocalizeResourceSetConstants.Global)]
        public string Maint_WarrantyDesc { get; set; }

        public string HiddenType { get; set; }
        public HttpPostedFileBase TypeImageFile { get; set; }

        public IEnumerable<SelectListItem> AccountList { get; set; }
        public IEnumerable<SelectListItem> LookupTypeList { get; set; }
        public IEnumerable<SelectListItem> VendorList { get; set; }
        public IEnumerable<SelectListItem> AssetCategoryList { get; set; }
        public bool AlertFollowedEquipment { get; set; }

        public string ErrorMessages { get; set; }
        public string BusinessType { get; set; }
        public string ClientLookupId { get; set; }
        public Guid hdLoginId { get; set; }

        public string localurl { get; set; }
        public long? LineID { get; set; }
     
      //[RequiredIf("PlantLocation", true, ErrorMessage = "ValidationDepartment|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public long? DeptID { get; set; }

        public long? SystemInfoId { get; set; }
        public byte[] EquipmentImage { get; set; }
        [Required(ErrorMessage = "CategoryErrorMessage|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [Display(Name = "spnCategory|" + LocalizeResourceSetConstants.Global)]
        public string AssetCategory { get; set; }

        public bool PlantLocation { get; set; }
        public IEnumerable<SelectListItem> LineList { get; set; }
        public IEnumerable<SelectListItem> DeptList { get; set; }
        public IEnumerable<SelectListItem> SystemList { get; set; }
        public IEnumerable<SelectListItem> ActiveDeptList { get; set; }
        public string AssetGroup1Name { get; set; }
        public string AssetGroup2Name { get; set; }
        public string AssetGroup3Name { get; set; }
        public string AssetGroup1ClientLookupId { get; set; }
        public string AssetGroup2ClientLookupId { get; set; }
        public string AssetGroup3ClientLookupId { get; set; }
        public string AssetGroup1Desc { get; set; }
        public string AssetGroup2Desc { get; set; }
        public string AssetGroup3Desc { get; set; }

        public long? AssetGroup1Id { get; set; }
        public string AssetGroup1IdErrorMessage { get; set; }
        public long? AssetGroup2Id { get; set; }
        public long? AssetGroup3Id { get; set; }
        public IEnumerable<SelectListItem> AssetGroup1List { get; set; }
        public IEnumerable<SelectListItem> AssetGroup2List { get; set; }
        public IEnumerable<SelectListItem> AssetGroup3List { get; set; }
        public long? UserId { get; set; }
        #region Asset Availability V2-636
        public string AssetAvailability { get; set; }

        #endregion
       
    }
}