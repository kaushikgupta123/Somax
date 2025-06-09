using Common.Constants;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.Configuration.FleetServiceTask
{
    public class ServiceTaskModel
    {
        [Display(Name = "spnServiceTaskId|" + LocalizeResourceSetConstants.FleetServiceTask)]
        [Required(ErrorMessage = "ServiceTaskIDErrorMessage|" + LocalizeResourceSetConstants.FleetServiceTask)]
        [StringLength(31, ErrorMessage = "ServiceTaskIdStrLenErrorMessage|" + LocalizeResourceSetConstants.FleetServiceTask)]
        [RegularExpression("^[A-Z0-9\\%\\-\\:\\/\\$\\*\\+\\.]+$", ErrorMessage = "ServiceTaskIdRegErrMsg|" + LocalizeResourceSetConstants.FleetServiceTask)]
        [Remote("CheckIfServiceTaskExist", "FleetServiceTask", HttpMethod = "POST", ErrorMessage = "184|" + LocalizeResourceSetConstants.StoredProcValidation)]
        public string ClientLookupId { get; set; }
        [Display(Name = "spnDescription|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "validationDescription|" + LocalizeResourceSetConstants.Global)]
        public string Description { get; set; }
    }
}