using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Linq;

namespace Client.CustomValidation
{
    public class NoRepeatedCharactersAttribute : ValidationAttribute, IClientValidatable
    {
        private string RepeatedCharactersProperty;
        public NoRepeatedCharactersAttribute(string RepeatedCharactersProperty)
        {
            this.RepeatedCharactersProperty = RepeatedCharactersProperty;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null && !string.IsNullOrEmpty(RepeatedCharactersProperty))
            {
                var RepeatedCharactersDet = validationContext.ObjectInstance.GetType().GetProperty(RepeatedCharactersProperty);
                var RepeatedCharacters = RepeatedCharactersDet.GetValue(validationContext.ObjectInstance, null);

                if (RepeatedCharacters != null && Convert.ToBoolean(RepeatedCharacters))
                {
                    //int valLength = value.ToString().Length;
                    //int distinctValueLength = value.ToString().Distinct().ToString().Length;

                    //if (valLength != distinctValueLength)
                    //{
                    //    return new ValidationResult(ErrorMessage);
                    //}
                    //Regex regex = new Regex(@"([\w\W]).*?\1");// duplicate character
                    Regex regex = new Regex(@"([\w\W])\1\1");// any repeat character 3 more then 3 times
                    if (regex.IsMatch(value.ToString().ToLower()))
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
                ValidationType = "norepeatedcharacters",
                ErrorMessage = ErrorMessage
            };

            rule.ValidationParameters.Add("repeatedcharactersproperty", RepeatedCharactersProperty);

            yield return rule;
        }
    }
}