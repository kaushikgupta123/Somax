using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Client.CustomValidation
{
    public class RequireNumberAttribute : ValidationAttribute, IClientValidatable
    {
        public string RequireNumberProperty;
        public RequireNumberAttribute(string RequireNumberProperty)
        {
            this.RequireNumberProperty = RequireNumberProperty;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null && !string.IsNullOrEmpty(RequireNumberProperty))
            {
                var RequireNumberDet = validationContext.ObjectInstance.GetType().GetProperty(RequireNumberProperty);
                var RequireNumber = RequireNumberDet.GetValue(validationContext.ObjectInstance, null);                

                if (RequireNumber != null && Convert.ToBoolean(RequireNumber))
                {
                    Regex regex = new Regex(@"\d");// to check if any interger is present
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
                ValidationType = "requirenumber",
                ErrorMessage = ErrorMessage
            };

            rule.ValidationParameters.Add("requirenumberproperty", RequireNumberProperty);

            yield return rule;
        }
    }
}