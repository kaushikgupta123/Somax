

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Web.Mvc;

namespace Client.CustomValidation
{
    public class GreaterThanForPartAutoPurchasingConfigurationAttribute : ValidationAttribute, IClientValidatable
    {
        private string PropertyName { get; set; }
        public GreaterThanForPartAutoPurchasingConfigurationAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            PropertyInfo autoPurchasePropertyInfo = context.ObjectType.GetProperty("IsAutoPurchase");
            if (autoPurchasePropertyInfo == null)
            {
                return new ValidationResult("Unknown property: IsAutoPurchase");
            }
            bool autoPurchaseValue = (bool)(autoPurchasePropertyInfo.GetValue(context.ObjectInstance) ?? false);

            // Only perform the comparison if AutoPurchase is true
            if (autoPurchaseValue)
            {
                var dependentValue = context.ObjectInstance.GetType().GetProperty(PropertyName).GetValue(context.ObjectInstance, null);
                value = (value ?? 0);
                dependentValue = (dependentValue ?? 0);
                if (Convert.ToDecimal(value) < Convert.ToDecimal(dependentValue))
                {
                    return new ValidationResult(FormatErrorMessage(context.DisplayName), new[] { context.MemberName });
                }
            }
            return ValidationResult.Success;
        }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ErrorMessageString,
                ValidationType = "greaterthanforpartautopurchasingconfiguration",
            };
            rule.ValidationParameters["dependentproperty"] = (context as ViewContext).ViewData.TemplateInfo.GetFullHtmlFieldId(PropertyName);

            yield return rule;
        }
    }
}