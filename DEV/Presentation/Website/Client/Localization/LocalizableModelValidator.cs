using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Client.Localization
{
    public class LocalizableModelValidator : ModelValidator
    {
        private readonly ModelValidator innerValidator;

        public LocalizableModelValidator(ModelValidator innerValidator, ModelMetadata metadata, ControllerContext controllerContext)
            : base(metadata, controllerContext)
        {
            this.innerValidator = innerValidator;
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            var rules = innerValidator.GetClientValidationRules();
            var modelClientValidationRules = rules as ModelClientValidationRule[] ?? rules.ToArray();
            foreach (var rule in modelClientValidationRules)
            {
                var localisationDetails = rule.ErrorMessage.Split('|');
                if (localisationDetails != null && localisationDetails.Count() > 1)
                {
                    rule.ErrorMessage = Common.UtilityFunction.GetMessageFromResource(localisationDetails[0], localisationDetails[1]);
                }

            }
            return modelClientValidationRules;
        }

        public override IEnumerable<ModelValidationResult> Validate(object container)
        {
            // execute the inner validation which doesn't have localization
            var results = innerValidator.Validate(container);
            // convert the error message (text id) to the localized value
            return results.Select(result =>
            {
                int textId;
                if (Int32.TryParse(result.Message, out textId))
                {
                    var localisationDetails = result.Message.Split('|');
                    if (localisationDetails != null && localisationDetails.Count() > 1)
                    {
                        result.Message = Common.UtilityFunction.GetMessageFromResource(localisationDetails[0], localisationDetails[1]);
                    }

                }
                return new ModelValidationResult() { Message = result.Message };
            });
        }
    }
}