using Common.Constants;
using DataContracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Client.CustomValidation;
namespace Client.Models.Configuration.ClientSetUp
{
    public class PasswordSettingsModel
    {
        //ErrorMessage = "ValidationPasswordAgeLimitBetweenThirtyToThreeHundredSixtyFive|" + LocalizeResourceSetConstants.Global
        [Required(ErrorMessage = "ValidationMaxFailLoginAttempts|" + LocalizeResourceSetConstants.Global)]
        [Range(5, 20, ErrorMessage = "ValidationMaxFailLoginAttemptsBetweenFiveToTwenty|" + LocalizeResourceSetConstants.Global)]
        public int MaxAttempts { get; set; }
        public bool PWReqMinLength { get; set; }

        [RequiredIf("PWReqMinLength", true, ErrorMessage = "ValidationMinLengthPassword|" + LocalizeResourceSetConstants.Global)]
        [Range(5, 30, ErrorMessage = "ValidationMinLengthPasswordBetweenFiveToThirty|" + LocalizeResourceSetConstants.Global)]
        public int? PWMinLength { get; set; }
        public bool PWReqExpiration { get; set; }
        [RequiredIf("PWReqExpiration", true, ErrorMessage = "ValidationPasswordAgeLimit|" + LocalizeResourceSetConstants.Global)]
        [Range(30, 365, ErrorMessage = "ValidationPasswordAgeLimitBetweenThirtyToThreeHundredSixtyFive|" + LocalizeResourceSetConstants.Global)]
        public int? PWExpiresDays { get; set; }
        public bool PWRequireNumber { get; set; }
        public bool PWRequireAlpha { get; set; }
        public bool PWRequireMixedCase { get; set; }
        public bool PWRequireSpecialChar { get; set; }
        public bool PWNoRepeatChar { get; set; }
        public bool PWNotEqualUserName { get; set; }
        public bool AllowAdminReset { get; set; }

    }
}