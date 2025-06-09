using Common.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.MultiStoreroomPart
{
    public class MultiStoreroomPartEquipmentXrefModel
    {
        [Display(Name = "spnGlobalEquipmentId|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "SelectEquipmentIDErrorMessage|" + LocalizeResourceSetConstants.Global)]
        public string Equipment_ClientLookupId { get; set; }
        public long Equipment_Parts_XrefId { get; set; }
        public string Part_ClientLookupId { get; set; }
        [Display(Name = "spnQuantityNeeded|" + LocalizeResourceSetConstants.PartDetails)]
        [RegularExpression(@"^\d*\.?\d{0,6}$", ErrorMessage = "globalSixDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999999999.999999, ErrorMessage = "globalSixDecimalAfterNineDecimalBeforeTotalFifteenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? QuantityNeeded { get; set; }
        [Display(Name = "spnQuantityUsed|" + LocalizeResourceSetConstants.PartDetails)]
        [RegularExpression(@"^\d*\.?\d{0,6}$", ErrorMessage = "globalSixDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999999999.999999, ErrorMessage = "globalSixDecimalAfterNineDecimalBeforeTotalFifteenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? QuantityUsed { get; set; }
        [Display(Name = "spnLogComment|" + LocalizeResourceSetConstants.Global)]
        public string Comment { get; set; }
        public long EquipmentId { get; set; }
        public long PartId { get; set; }
        public bool EquipmentPartXrefSecurity { get; set; }
        public IEnumerable<SelectListItem> EquipmentList { get; set; }
    }
}