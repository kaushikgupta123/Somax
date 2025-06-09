using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Text.RegularExpressions;


namespace Client.CustomValidation
{
    public class ValidatePinForUSA : ValidationAttribute, IClientValidatable
    {
        private String PropertyName { get; set; }
        private readonly RequiredAttribute _innerAttribute;
        public ValidatePinForUSA(String propertyName)
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
            if (dValue != string.Empty && dValue.ToUpper().Equals("USA"))
            {
                string pinCode = Convert.ToString(value);
                 Regex regex = new Regex(@"^\d{5}$|^\d{5}-\d{4}$");               
                    Match match = regex.Match(pinCode);
                if(!match.Success)
                {
                    ErrorMessage = "Please enter proper ZIP code";
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
                ValidationType = "validatepinforusa",
            };
            rule.ValidationParameters["dependentproperty"] = (context as ViewContext).ViewData.TemplateInfo.GetFullHtmlFieldId(PropertyName);

            yield return rule;
        }
    }
}