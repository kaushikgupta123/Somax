using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Text.RegularExpressions;

namespace Client.CustomValidation
{
    public class RequireMixedcaseCharacterAttribute : ValidationAttribute, IClientValidatable
    {
        private string RequireMixedcaseCharacterProperty;
        public RequireMixedcaseCharacterAttribute(string RequireMixedcaseCharacterProperty)
        {
            this.RequireMixedcaseCharacterProperty = RequireMixedcaseCharacterProperty;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null && !string.IsNullOrEmpty(RequireMixedcaseCharacterProperty))
            {
                var RequireMixedcaseCharacterDet = validationContext.ObjectInstance.GetType().GetProperty(RequireMixedcaseCharacterProperty);
                var RequireMixedcaseCharacter = RequireMixedcaseCharacterDet.GetValue(validationContext.ObjectInstance, null);

                if (RequireMixedcaseCharacter != null && Convert.ToBoolean(RequireMixedcaseCharacter))
                {
                    Regex regexLower = new Regex(@"[a-z]");// to check if any lower case character is present
                    Regex regexUpper = new Regex(@"[A-Z]");// to check if any upper case character is present
                    if (!regexLower.IsMatch(value.ToString()) || !regexUpper.IsMatch(value.ToString()))
                    {
                        return new ValidationResult(ErrorMessage);
                    }
                }
            }
            return ValidationResult.Success;
        }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule()
            {
                ValidationType = "requiremixedcasecharacter",
                ErrorMessage = ErrorMessage
            };

            rule.ValidationParameters.Add("requiremixedcasecharacterproperty", RequireMixedcaseCharacterProperty);

            yield return rule;
        }
    }
}