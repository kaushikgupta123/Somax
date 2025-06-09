using Common.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.Sanitation
{
    public class ChemicalSuppliesModel
    {
        public long ClientId { get; set; }
        public int UpdateIndex { get; set; }
        public long SanitationPlanningId { get; set; }
        public long SanitationMasterId { get; set; }
        public long SanitationJobId { get; set; }
        public string Category { get; set; }     
        [Required(ErrorMessage = "spnReqChemical|" + LocalizeResourceSetConstants.SanitationDetails)]
        public string CategoryValue { get; set; }
        public long CategoryId { get; set; }

        public IEnumerable<SelectListItem> CategoryIdList { get; set; }
        public string Description { get; set; }
        public string Dilution { get; set; }
        public string Instructions { get; set; }
      
        [RegularExpression(@"^\d*\.?\d{0,6}$", ErrorMessage = "globalSixDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 999999999.999999, ErrorMessage = "globalSixDecimalAfterNineDecimalBeforeTotalFifteenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "spnReqQuantity|" + LocalizeResourceSetConstants.SanitationDetails)]      
        public decimal? Quantity { get; set; }
        public decimal? UnitCost { get; set; }
        public string ClientLookupId { get; set; }
        public string hdnDropdownDescription { get; set; }
    }
}