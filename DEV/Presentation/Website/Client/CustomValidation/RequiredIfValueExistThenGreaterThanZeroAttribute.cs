using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.CustomValidation
{
    public class RequiredIfValueExistThenGreaterThanZeroAttribute : ValidationAttribute, IClientValidatable
    {
        private String PropertyName { get; set; }
        //  private readonly RequiredAttribute _innerAttribute;

        public RequiredIfValueExistThenGreaterThanZeroAttribute(String propertyName)
        {
            PropertyName = propertyName;
            // _innerAttribute = new RequiredAttribute();
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            string dValue = string.Empty;
            var dependentValue = context.ObjectInstance.GetType().GetProperty(PropertyName).GetValue(context.ObjectInstance, null);
            if (dependentValue != null)
                dValue = dependentValue.ToString();

            if (dValue != string.Empty)
            {
                decimal EnteredValue;

                bool parseResult = decimal.TryParse(Convert.ToString(value), out EnteredValue);
                if (parseResult && EnteredValue <1)
                {
                    ErrorMessage = "Please Fill Proper Schedule Duration";
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ErrorMessageString,
                ValidationType = "requiredifvalueexistthengreaterthanzeroattribute",
            };
            rule.ValidationParameters["dependentproperty"] = (context as ViewContext).ViewData.TemplateInfo.GetFullHtmlFieldId(PropertyName);

            yield return rule;
        }
    }
}