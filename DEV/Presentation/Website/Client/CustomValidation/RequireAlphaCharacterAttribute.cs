using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Client.CustomValidation
{
    public class RequireAlphaCharacterAttribute : ValidationAttribute, IClientValidatable
    {
        public string RequireAlphacharacterProperty;
        public RequireAlphaCharacterAttribute(string RequireAlphacharacterProperty)
        {
            this.RequireAlphacharacterProperty = RequireAlphacharacterProperty;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null && !string.IsNullOrEmpty(RequireAlphacharacterProperty))
            {
                var RequireAlphacharacterDet = validationContext.ObjectInstance.GetType().GetProperty(RequireAlphacharacterProperty);
                var RequireAlphacharacter = RequireAlphacharacterDet.GetValue(validationContext.ObjectInstance, null);

                if (RequireAlphacharacter != null && Convert.ToBoolean(RequireAlphacharacter))
                {
                    Regex regex = new Regex(@"[a-zA-Z]");// to check if any character is present
                    if (!regex.IsMatch(value.ToString()))
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
                ValidationType = "requirealphacharacter",
                ErrorMessage = ErrorMessage
            };

            rule.ValidationParameters.Add("requirealphacharacterproperty", RequireAlphacharacterProperty);

            yield return rule;
        }
    }
}