using Common.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Client.CustomValidation;
namespace Client.Models.InventoryReceipt
{
    public class ReceiptModel
    {
        public long PartId { get; set; }

        [Required(ErrorMessage = "validationpartidupc|" + LocalizeResourceSetConstants.PartDetails)]
        public string PartClientLookupId { get; set; }

        [Required(ErrorMessage = "validationunitcost|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression(@"^\d*\.?\d{0,5}$", ErrorMessage = "globalFiveDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0.00001, 9999999999.99999, ErrorMessage = "globalFiveDecimalAfterTotalFifteenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? UnitCost { get; set; }


        [Required(ErrorMessage = "validationreceiptquantity|" + LocalizeResourceSetConstants.PartDetails)]
        [RegularExpression(@"^\d*\.?\d{0,6}$", ErrorMessage = "globalSixDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(1.0, 999999999.999999, ErrorMessage = "globalSixDecimalAfterNineDecimalBeforeTotalFifteenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? ReceiptQuantity { get; set; }
        public IEnumerable<SelectListItem> PartList { get; set; }
        public bool MultiStoreroom { get; set; }

        [RequiredIf("MultiStoreroom", true, ErrorMessage = "GlobalStoreroomSelect|" + LocalizeResourceSetConstants.Global)]
        public long? StoreroomId { get; set; }
        public IEnumerable<SelectListItem> StoreroomList { get; set; }
    }
}