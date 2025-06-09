using Common.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.Work_Order
{
    public class SanitationOnDemandWOModel
    {
        [Required(ErrorMessage = "spnReqOnDemandProcedure|" + LocalizeResourceSetConstants.Global)]
        public long? OnDemandId { get; set; }
        [Required(ErrorMessage = "spnReqPlantLocation|" + LocalizeResourceSetConstants.SanitationDetails)]
        public string PlantLocationDescription { get; set; }
        public string ChargeType { get; set; }
        public long PlantLocationId { get; set; }


        public long WorkOrderId { get; set; }
        public string ClientLookupId { get; set; }
        public IEnumerable<SelectListItem> OnDemandList { get; set; }
       
    }
}