using Client.CustomValidation;
using Client.Models.InventoryCheckout;
using Common.Constants;
using DataContracts;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.Parts
{
    public class ParInvCheckoutModel
    {
        public long? PersonnelId { get; set; }
        public long? selectedPersonnelId { get; set; }
        public string IssueToClentLookupId { get; set; }
        [Required(ErrorMessage = "spnPleaseSelectChargeTo|" + LocalizeResourceSetConstants.PartDetails)]
        [Display(Name = "Charge To")]
        public long? ChargeToId { get; set; }
        [Required(ErrorMessage = "spnPleaseSelectChargeTo|" + LocalizeResourceSetConstants.PartDetails)]
        [Display(Name = "Charge To")]
        public string ChargeToClientLookupId { get; set; }
        [Required(ErrorMessage = "spnPleaseSelectChargeType|" + LocalizeResourceSetConstants.PartDetails)]
        [Display(Name = "Charge Type")]
        public string ChargeType { get; set; }
        [Display(Name = "Part ID")]
        public string ClientLookupId { get; set; }
        [Required(ErrorMessage = "spnQuantityVal|" + LocalizeResourceSetConstants.PartDetails)]
        [RegularExpression(@"^\d*\.?\d{0,6}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(1, 999999999.999999, ErrorMessage = "GlobalMaxRangeOneToNieDigitWithSixDecimalPlace|" + LocalizeResourceSetConstants.PartDetails)]
        [Display(Name = "Quantity")]
        public decimal? Quantity { get; set; }
        public long? PartId { get; set; }
        public string PartDescription { get; set; }
        public string UPCCode { get; set; }
        public string ErrorMessagerow { get; set; }
        public string PartClientLookupId { get; set; }
        public int? IssueToListIndex { get; set; }
        public long? PartStoreroomId { get; set; }
        public bool IsPerformAdjustment { get; set; }
        public IEnumerable<SelectListItem> IssueToList { get; set; }
        public IEnumerable<SelectListItem> ChargeTypeList { get; set; }
        public IEnumerable<SelectListItem> ChargeToList { get; set; }
        public IEnumerable<SelectListItem> PartIdList { get; set; }
    }

    //public class ParInvCheckVM : LocalisationBaseVM
    //{
    //    public ParInvCheckoutModel inventoryCheckoutModel { get; set; }
    //    public InventoryCheckoutValidationModel inventoryCheckoutValidationModel { get; set; }
    //    public EquipmentTreeModel equipmentTreeModel { get; set; }
    //    public UserData userData { get; set; }
    //}
}