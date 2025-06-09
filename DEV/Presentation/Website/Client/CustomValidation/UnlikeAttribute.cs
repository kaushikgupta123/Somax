using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Text.RegularExpressions;

namespace Client.CustomValidation
{

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class UnlikeAttribute : ValidationAttribute, IClientValidatable
    {
        private const string DefaultErrorMessage = "The value of {0} cannot be the same as the value of the {1}.";

        public string property1 { get; private set; }
        public string property2 { get; private set; }

        public UnlikeAttribute(
            string otherProperty,
            string otherPropertyName = "")
            : base(DefaultErrorMessage)
        {
            property1 = otherProperty;
            property2 = otherPropertyName;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null && !string.IsNullOrEmpty(property1))
            {
                var property1det = validationContext.ObjectInstance.GetType().GetProperty(property1);
                var property1Value = property1det.GetValue(validationContext.ObjectInstance, null);

                if (!string.IsNullOrEmpty(property2))
                {
                    var property2det = validationContext.ObjectInstance.GetType().GetProperty(property2);
                    var property2Value = property2det.GetValue(validationContext.ObjectInstance, null);

                    if (value.Equals(property1Value) || value.Equals(property2Value))
                    {
                        return new ValidationResult(ErrorMessage);
                    }
                }
                else
                {
                    if (value.Equals(property1Value))
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
                ValidationType = "unlike",
                ErrorMessage = ErrorMessage
            };

            rule.ValidationParameters.Add("property1", property1);
            rule.ValidationParameters.Add("property2", property2);

            yield return rule;
        }
    }
}



