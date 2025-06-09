using Client.CustomValidation;
using Common.Constants;
using System;

namespace Client.Models.MasterSanitationSchedule
{
    public class SanitationJobGenerationModel
    {
        [RequiredIf("RadioButton", "OnDemand", ErrorMessage = "validationOnDemandGroup|" + LocalizeResourceSetConstants.SanitationDetails)]
        public string OnDemandGroup { get; set; }
        [RequiredIf("RadioButton", "OnDemand", ErrorMessage = "validationOnDemandDate|" + LocalizeResourceSetConstants.SanitationDetails)]
        public DateTime? ScheduledDate { get; set; }

      
        public string RadioButton { get; set; }
        public bool IsPrint { get; set; }
    }
}