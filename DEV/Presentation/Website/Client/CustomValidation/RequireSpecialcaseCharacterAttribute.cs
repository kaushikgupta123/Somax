using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Client.CustomValidation
{
    public class RequireSpecialcaseCharacterAttribute : ValidationAttribute, IClientValidatable
    {
        public string RequireSpecialcaseCharacterProperty;
        public RequireSpecialcaseCharacterAttribute(string RequireSpecialcaseCharacterProperty)
        {
            this.RequireSpecialcaseCharacterProperty = RequireSpecialcaseCharacterProperty;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null && !string.IsNullOrEmpty(RequireSpecialcaseCharacterProperty))
            {
                var RequireSpecialcaseCharacterDet = validationContext.ObjectInstance.GetType().GetProperty(RequireSpecialcaseCharacterProperty);
                var RequireSpecialcaseCharacter = RequireSpecialcaseCharacterDet.GetValue(validationContext.ObjectInstance, null);

                if (RequireSpecialcaseCharacter != null && Convert.ToBoolean(RequireSpecialcaseCharacter))
                {
                    //char[] specialChar = { '~', '`', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '+', '{', '}', '|', '[', ']', '\\', ':', ';', '<', '>', '?', ',', '.', '/' };
                    //int idx = value.ToString().IndexOfAny(specialChar);
                    //if (idx < 0)
                    //{
                    //    return new ValidationResult(ErrorMessage);
                    //}

                    Regex regex = new Regex(@"[!@#$%^&*()`~+{}|:;<>?,./\[\]\\]");// Special character
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
                ValidationType = "requirespecialcasecharacter",
                ErrorMessage = ErrorMessage
            };

            rule.ValidationParameters.Add("requireppecialcasepharacterproperty", RequireSpecialcaseCharacterProperty);

            yield return rule;
        }
    }
}