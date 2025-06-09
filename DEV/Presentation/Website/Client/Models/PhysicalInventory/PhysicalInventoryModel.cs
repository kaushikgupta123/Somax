using Common.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Client.CustomValidation;
namespace Client.Models.PhysicalInventory
{
    public class PhysicalInventoryModel
    {

        public long PartId { get; set; }

        [Required(ErrorMessage = "validationpartidupc|" + LocalizeResourceSetConstants.PartDetails)]
        public string PartClientLookupId { get; set; }

        [Required(ErrorMessage = "validationquantitycount|" + LocalizeResourceSetConstants.PartDetails)]
        [RegularExpression(@"^\d*\.?\d{0,6}$", ErrorMessage = "globalSixDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 999999999.999999, ErrorMessage = "globalSixDecimalAfterNineDecimalBeforeTotalFifteenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? ReceiptQuantity { get; set; }
        public IEnumerable<SelectListItem> PartList { get; set; }
        public bool MultiStoreroom { get; set; }

        [RequiredIf("MultiStoreroom", true, ErrorMessage = "GlobalStoreroomSelect|" + LocalizeResourceSetConstants.Global)]
        public long? StoreroomId { get; set; }
        public IEnumerable<SelectListItem> StoreroomList { get; set; }
    }
}