using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.Sanitation
{
    public class AddODemandModel
    {
        [Required(ErrorMessage = "spnReqOnDemandProcedure|" + LocalizeResourceSetConstants.Global)]        
        public long? OnDemandId { get; set; }
        [Required(ErrorMessage = "spnSanitationChargeTo|" + LocalizeResourceSetConstants.SanitationDetails)]
        public string PlantLocationDescription { get; set; }     
        [Display(Name = "spnRequired|" + LocalizeResourceSetConstants.Global)]
        public DateTime? RequiredDate { get; set; }
        public string ChargeType { get; set; }
        public long? PlantLocationId { get; set; }
        public string ChargeToClientLookupId { get; set; }
        public string Status { get; set; }
        public string SourceType { get; set; }
        public int FlagSourceType { get; set; }
        public string ClientLookupId { get; set; }
        public string Description { get; set; }
        public bool IsRequest { get; set; }  
        
        public bool PlantLocation { get; set; }
        public IEnumerable<SelectListItem> OnDemandList { get; set; }
    }
}

