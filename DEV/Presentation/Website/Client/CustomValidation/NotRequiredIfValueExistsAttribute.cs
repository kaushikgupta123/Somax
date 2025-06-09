using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.CustomValidation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class NotRequiredIfValueExistsAttribute : ValidationAttribute, IClientValidatable
    {
        private string PropertyName { get; set; }
        private readonly RequiredAttribute _innerAttribute;

        public NotRequiredIfValueExistsAttribute(string propertyName)
        {
            PropertyName = propertyName;
            _innerAttribute = new RequiredAttribute();
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            decimal lValue= Convert.ToDecimal(value);
            decimal dValue = 0;
            var dependentValue = context.ObjectInstance.GetType().GetProperty(PropertyName).GetValue(context.ObjectInstance, null);
            if (dependentValue != null)
                dValue = Convert.ToDecimal(dependentValue);

            if (lValue != 0 && dValue != 0)
            {
                return new ValidationResult(FormatErrorMessage(context.DisplayName), new[] { context.MemberName });
            }
            return ValidationResult.Success;
        }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule mvr = new ModelClientValidationRule();
            mvr.ErrorMessage = ErrorMessageString;
            mvr.ValidationType = "notrequiredifvalueexists";
            mvr.ValidationParameters["otherfield"] = (context as ViewContext).ViewData.TemplateInfo.GetFullHtmlFieldId(PropertyName);
             return new[] { mvr };
        }
    }
}