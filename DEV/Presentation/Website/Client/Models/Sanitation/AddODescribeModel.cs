using Common.Constants;
using System.ComponentModel.DataAnnotations;
namespace Client.Models.Sanitation
{
    public class AddODescribeModel
    {
        [Required(ErrorMessage = "spnSanitationChargeTo|" + LocalizeResourceSetConstants.SanitationDetails)]        
        public string PlantLocationDescription { get; set; }

        [Required(ErrorMessage = "spnReqDescription|" + LocalizeResourceSetConstants.SanitationDetails)]
        public string Description { get; set; }

        public string ChargeType { get; set; }
        public long? PlantLocationId { get; set; }
        public string Status { get; set; }
        public int FlagSourceType { get; set; }
    }
}