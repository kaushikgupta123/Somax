using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.CustomValidation
{
    public class RequiredSecurityProfileForUser : ValidationAttribute, IClientValidatable
    {
        private String PropertyName { get; set; }
        private readonly RequiredAttribute _innerAttribute;
        public RequiredSecurityProfileForUser(String propertyName)
        {
            PropertyName = propertyName;//usa
            _innerAttribute = new RequiredAttribute();
        }
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            string dValue = string.Empty;
            var dependentValue = context.ObjectInstance.GetType().GetProperty(PropertyName).GetValue(context.ObjectInstance, null);
            if (dependentValue != null)
                dValue = dependentValue.ToString();
            if (dValue != string.Empty && dValue.ToUpper().Equals("FULL"))
            {
                string securityProfile = Convert.ToString(value);
                if(string.IsNullOrEmpty(securityProfile))
                {
                    ErrorMessage = "Please select security profile";
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
                ValidationType = "requiredsecurityprofileforuser",
            };
            rule.ValidationParameters["dependentproperty"] = (context as ViewContext).ViewData.TemplateInfo.GetFullHtmlFieldId(PropertyName);

            yield return rule;
        }
    }
}