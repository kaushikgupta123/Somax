using Common.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models
{
    public class PartsSessionData
    {
        public PartsSessionData()
        {
            _EquipmentSummaryModel = new EquipmentSummaryModel();
        }
        public EquipmentSummaryModel _EquipmentSummaryModel { get; set; }
        public long ClientId { get; set; }

        public long Equipment_Parts_XrefId { get; set; }
        public long EquipmentId { get; set; }
        public string PartId { get; set; }
        [Display(Name = "spnLogComment|" + LocalizeResourceSetConstants.Global)]
        public string Comment { get; set; }
        [Display(Name = "spnQtyNeeded|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression(@"^\d*\.?\d{0,6}$", ErrorMessage = "GlobalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 999999999.999999, ErrorMessage = "globalSixDecimalAfterNineDecimalBeforeTotalFifteenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? QuantityNeeded { get; set; }
        [Display(Name = "spnQtyUsed|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression(@"^\d*\.?\d{0,6}$", ErrorMessage = "GlobalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 999999999.999999, ErrorMessage = "globalSixDecimalAfterNineDecimalBeforeTotalFifteenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? QuantityUsed { get; set; }
        public long UpdateIndex { get; set; }
        [Display(Name = "spnPartID|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "spnPleaseSelectPartId|" + LocalizeResourceSetConstants.Global)]
        public string Part { get; set; }
        public IEnumerable<SelectListItem> PartsList { get; set; }
        public List<string> ErrorMessage { get; set; }
    }
}