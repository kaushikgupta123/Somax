using Common.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models
{

    public class TechSpecsModel
    {
        public TechSpecsModel()
        {
            _EquipmentSummaryModel = new EquipmentSummaryModel();
        }
        public EquipmentSummaryModel _EquipmentSummaryModel { get; set; }
        public long EquipmentId { get; set; }
        public string ClientLookupId { get; set; }
        [Display(Name = "spnValue|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage= "SpecValueErrorMessage|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string SpecValue { get; set; }
        public string Description { get; set; }
        [Display(Name = "spnLogComment|" + LocalizeResourceSetConstants.Global)]
        public string Comments { get; set; }
        public string UnitOfMeasure { get; set; }
        [Required(ErrorMessage = "SpecSpecificationErrorMessage|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [Display(Name = "spnSpecification|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public long TechSpecId { get; set; }
        public long Equipment_TechSpecsId { get; set; }
        public string TechSpecsSecurity { get; set; }
        public IEnumerable<SelectListItem> TechSpecsList { get; set; }
        public string Mode { get; set; }
        public int updatedindex { get; set; }
        public List<string> ErrorMessage { get; set; }
    }
}