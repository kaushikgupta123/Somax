using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Admin.CustomValidation
{
    public class RequiredIfValueExistAttribute : ValidationAttribute, IClientValidatable
    {
        private String PropertyName { get; set; }
        private readonly RequiredAttribute _innerAttribute;

        public RequiredIfValueExistAttribute(String propertyName)
        {
            PropertyName = propertyName;
            _innerAttribute = new RequiredAttribute();
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            string dValue = string.Empty;
            var dependentValue = context.ObjectInstance.GetType().GetProperty(PropertyName).GetValue(context.ObjectInstance, null);
            if (dependentValue != null)
                dValue = dependentValue.ToString();

            if (dValue != string.Empty)
            {
                if (!_innerAttribute.IsValid(value))
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
                ValidationType = "requiredifvalueexist",
            };
            rule.ValidationParameters["dependentproperty"] = (context as ViewContext).ViewData.TemplateInfo.GetFullHtmlFieldId(PropertyName);

            yield return rule;
        }
    }
}