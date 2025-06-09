using Client.CustomValidation;

using Common.Constants;

using System.ComponentModel.DataAnnotations;

namespace Client.Models.Equipment
{
    public class AssignmentModel
    {
        //[Required(ErrorMessage = "Please select Assigned to Id")]
        //[Display(Name = "Assigne to Asset Id")]
        [Required(ErrorMessage = "spnPleaseSelectAssignedtoAssetId|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [Display(Name = "spnAssignedtoAssetId|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string AssignedAssetClientlookupId { get; set; }
        public long AssignedAssetId { get; set; }
        //[RequiredIf("RepairableSpareStatus", "Unassigned", ErrorMessage = "Please enter location")]
        //[Display(Name = "Location")]
        [RequiredIf("RepairableSpareStatusAssign", AssetStatusConstant.Unassigned, ErrorMessage = "GlobalPleaseEnterLocation|" + LocalizeResourceSetConstants.Global)]
        [Display(Name = "spnLocation|" + LocalizeResourceSetConstants.Global)]
        public string Location { get; set; }
        public long EquipmentId { get; set; }
        public string RepairableSpareStatusAssign { get; set; }
    }
}